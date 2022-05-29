using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Collections;


public partial class Transaction_BarangKeluar : System.Web.UI.Page
{
    StringBuilder sql = new StringBuilder();
    StringBuilder sql1 = new StringBuilder();
    SqlTransaction trans;
    SqlTransaction trans1;
    SqlConnection conn;
    SqlConnection conn1;
    SqlCommand cmd;
    SqlCommand cmd1;
    SqlDataReader dr;
    SqlDataReader dr1;
    DataTable dt;
    ClassAdih cAdih = new ClassAdih();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (Request.QueryString["mode"].ToString() == "add")
            {
                tbTransDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                tbRealDeliveryDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                lStatus.Text = "";
                statusChanged();
            }
            else
            {
                tbTransNo.Text = Request.QueryString["trans_no"].ToString();

                sql.Length = 0;
                sql.Append("SELECT a.trans_no, a.trans_date, a.reff, a.reff_order_no, ");
                sql.Append("(CASE");
                sql.Append("    WHEN a.reff = 'STOCK TRANSPORT ORDER' ");
                sql.Append("    THEN CONVERT(VARCHAR, sto.created_date, 106) ");
                //sql.Append("    WHEN a.reff = 'RENTAL RETURN ORDER' ");
                //sql.Append("    THEN CONVERT(VARCHAR, rro.posted_date, 106) ");
                sql.Append("    WHEN a.reff = 'RENTAL ORDER' ");
                sql.Append("    THEN CONVERT(VARCHAR, ro.posted_date, 106) ");
                //sql.Append("    WHEN a.reff = 'SALES ORDER' ");
                //sql.Append("    THEN CONVERT(VARCHAR, so.posted_date, 106) ");
                sql.Append("END) AS reff_order_date, ");
                sql.Append("a.from_warehouse,b.wh_description AS from_warehouse_name, ");
                sql.Append("a.ship_to, h.customer_name AS ship_to_name, ");
                sql.Append("a.real_delivery_date, a.movement_type, j.description AS movement_type_name, a.note, a.status ");
                sql.Append("FROM outbound_delivery a ");
                sql.Append("LEFT JOIN site_wh b WITH(READPAST) ON a.from_warehouse = b.wh_id ");
                sql.Append("LEFT JOIN customer h WITH(READPAST) ON a.ship_to = h.customer_no ");
                sql.Append("LEFT JOIN movement_type j ON a.movement_type = j.id ");
                sql.Append("LEFT JOIN stock_transport_order sto ON a.reff_order_no = sto.trans_no AND sto.status = '1' ");
                sql.Append("LEFT JOIN rental_order ro ON a.reff_order_no = ro.trans_no AND ro.status = '1' ");
                //sql.Append("LEFT JOIN rental_return_order rro ON a.reff_order_no = rro.trans_no AND rro.status = '1' ");
                sql.Append("WHERE 1 = 1 ");
                sql.Append("AND a.trans_no = '" + tbTransNo.Text + "' ");

                using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
                {
                    conn.Open();
                    using (cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        using (dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                tbTransDate.Text = DateTime.Parse(dr["trans_date"].ToString()).ToString("dd-MM-yyyy");
                                lbReff.Text = dr["reff"].ToString();
                                tbReffOrderNo.Text = dr["reff_order_no"].ToString();
                                lbReffOrderNo.Text = dr["reff_order_date"].ToString();
                                if (dr["reff_order_date"].ToString() != "01 Jan 00")
                                {
                                    tbReffOrderNo.ReadOnly = true;
                                    ibSearchReffOrder.Visible = false;
                                }
                                tbFromWH.Text = dr["from_warehouse"].ToString();
                                tbFromWH.ReadOnly = true;
                                lbFromWH.Text = dr["from_warehouse_name"].ToString();
                                ibSearchFromWH.Visible = false;
                                tbShipTo.Text = dr["ship_to"].ToString();
                                tbShipTo.ReadOnly = true;
                                lbShipTo.Text = dr["ship_to_name"].ToString();
                                tbRealDeliveryDate.Text = DateTime.Parse(dr["real_delivery_date"].ToString()).ToString("dd-MM-yyyy");
                                if (lbReff.Text == "STOCK TRANSPORT ORDER")
                                {
                                    tbRealDeliveryDate.ReadOnly = true;
                                    ceRealDeliveryDate.Enabled = false;
                                }
                                tbMovementType.Text = dr["movement_type"].ToString();
                                lbMovementType.Text = dr["movement_type_name"].ToString();
                                tbMovementType.ReadOnly = true;
                                ibSearchMovementType.Visible = false;
                                tbNote.Text = dr["note"].ToString();
                                if (dr["status"].ToString() == "False")
                                {
                                    lStatus.Text = "OPEN";
                                }
                                else if (dr["status"].ToString() == "True")
                                {
                                    lStatus.Text = "POSTED";
                                }

                                if (lStatus.Text == "POSTED")
                                {
                                    btnSave.Visible = false;
                                    btnPosting.Visible = false;
                                    tbNote.ReadOnly = true;
                                }
                            }
                        }
                    }
                }

                sql.Length = 0;
                sql.Append("SELECT a.line_no, b.article_type, a.article_no, b.article_description, d.source_id AS article_source, ");
                sql.Append("a.qty_ordered, qty_delivered, a.serial_no, b.managed_item_by, a.reff_line_no, a.note ");
                sql.Append("FROM outbound_delivery_detail a WITH(READPAST) ");
                sql.Append("INNER JOIN article b WITH(READPAST) ON a.article_no = b.article_no ");
                sql.Append("LEFT JOIN rental_order_detail d WITH(READPAST) ON d.trans_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`") + "' AND a.article_no = d.article_no ");
                sql.Append("WHERE a.trans_no = '" + tbTransNo.Text.Trim().Replace("'", "`").ToUpper() + "' ");

                using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
                {
                    conn.Open();
                    using (cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        using (dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                DataTable dt = new DataTable();
                                dt = createDataTable();
                                Session["dt"] = dt;
                                while (dr.Read())
                                {
                                    if (lbReff.Text == "STOCK TRANSPORT ORDER")
                                    {
                                        addDataToTable(dr["article_no"].ToString(), dr["article_type"].ToString(), dr["article_description"].ToString(),
                                        double.Parse(dr["qty_ordered"].ToString()), double.Parse(dr["qty_delivered"].ToString()),
                                        dr["managed_item_by"].ToString(), "", dr["reff_line_no"].ToString(), dr["note"].ToString(), dr["serial_no"].ToString(),
                                        DateTime.Now, (DataTable)Session["dt"]);
                                    }
                                    else
                                    {
                                        addDataToTable(dr["article_no"].ToString(), dr["article_type"].ToString(),dr["article_description"].ToString(),
                                        double.Parse(dr["qty_ordered"].ToString()), double.Parse(dr["qty_delivered"].ToString()),
                                        dr["managed_item_by"].ToString(), dr["article_source"].ToString(), dr["reff_line_no"].ToString(), dr["note"].ToString(), dr["serial_no"].ToString(),
                                        DateTime.Now, (DataTable)Session["dt"]);
                                    }
                                }
                                loadGrid();
                                upGrid.Update();
                            }
                        }
                    }
                }
            }
        }
    }


    protected void statusChanged()
    {
        if (tbTransNo.Text == "")
        {
            btnPosting.Visible = false;
        }
        else
        {
            if (lStatus.Text == "")
            {
                btnPosting.Visible = false;
            }
            else if (lStatus.Text == "OPEN")
            {
                btnPosting.Visible = true;
            }
            else if (lStatus.Text == "POSTED")
            {
                btnSave.Visible = false;
                btnPosting.Visible = false;
            }
        }
    }

    protected void ibSearchReffOrder_Click(object sender, EventArgs e)
    {
        Session["search"] = "od_reff";
        wucSearch1.loadGrid();
    }

    protected void bSearchReffOrder_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            //if (tbReffOrderNo.Text.Contains("RRO") == true)
            //{
            //    sql.Length = 0;
            //    sql.Append("SELECT pr.trans_no, ISNULL(po.trans_no, '') AS po_trans_no, ISNULL(id.trans_no, '') AS id_trans_no ");
            //    sql.Append("FROM purchase_requisition pr WITH(READPAST) ");
            //    sql.Append("LEFT JOIN dbo.purchase_order_detail pod WITH(READPAST) ON pr.trans_no = pod.purchase_requisition_no ");
            //    sql.Append("LEFT JOIN purchase_order po WITH(READPAST) ON pod.trans_no = po.trans_no ");
            //    sql.Append("LEFT JOIN( ");
            //    sql.Append("	SELECT trans_no, SUM(approval_proxy) AS sum_approval_proxy, MAX(approval_date) AS max_approval_date ");
            //    sql.Append("	FROM trans_approval WITH(READPAST) ");
            //    sql.Append("	GROUP BY trans_no ");
            //    sql.Append(") AS ta ON po.trans_no = ta.trans_no AND po.total_amount <= ISNULL(ta.sum_approval_proxy, 0) ");
            //    sql.Append("LEFT JOIN inbound_delivery id WITH(READPAST) ON po.trans_no = id.reff_order_no AND id.status = '1' ");
            //    sql.Append("WHERE po.trans_no not in (select trans_no from purchase_order_cancelled)");
            //    sql.Append("AND po.trans_no not in (select trans_no from purchase_order_closed)");
            //    sql.Append("AND pr.sales_order_no  = '" + tbReffOrderNo.Text.Trim().Replace("'", "`") + "' ");

            //    using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
            //    {
            //        conn.Open();
            //        using (cmd = new SqlCommand(sql.ToString(), conn))
            //        {
            //            using (dr = cmd.ExecuteReader())
            //            {
            //                if (dr.HasRows)
            //                {
            //                    dr.Read();

            //                    if (dr["po_trans_no"].ToString() == "" || dr["id_trans_no"].ToString() == "")
            //                    {
            //                        master.messageBox("Please fullfil PR No. " + dr["trans_no"].ToString() + " first!");
            //                        return;
            //                    }
            //                }
            //            }
            //        }
            //    }

            //    sql.Length = 0;
            //    sql.Append("SELECT id.trans_no FROM inbound_delivery id WITH(READPAST) ");
            //    sql.Append("WHERE id.status  = '1' ");
            //    sql.Append("AND id.reff_order_no  = '" + tbReffOrderNo.Text.Trim().Replace("'", "`") + "' ");

            //    using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
            //    {
            //        conn.Open();
            //        using (cmd = new SqlCommand(sql.ToString(), conn))
            //        {
            //            using (dr = cmd.ExecuteReader())
            //            {
            //                if (!dr.HasRows)
            //                {
            //                    master.messageBox("Please inbound RRO No. " + tbReffOrderNo.Text.Trim().Replace("'", "`") + " first!");
            //                    return;
            //                }
            //            }
            //        }
            //    }


            //    double installationCharge = 0;

            //    sql.Length = 0;
            //    sql.Append("SELECT CONVERT(VARCHAR, a.posted_date, 106) AS posted_date, a.customer, a.customer_site, ");
            //    sql.Append("b.customer_name + ' - ' + c.alias_name_full + ' (' + c.alias_name + ') ' AS customer_name, a.cut_off_by, a.currency_id, a.cut_off_charge ");
            //    sql.Append("FROM rental_return_order a WITH(READPAST) ");
            //    sql.Append("INNER JOIN customer b WITH(READPAST) ON a.customer = b.customer_no ");
            //    sql.Append("INNER JOIN customer_address c WITH(READPAST) ON a.customer = c.customer_no AND c.address_type = '02' AND a.customer_site = c.id ");
            //    sql.Append("LEFT JOIN outbound_delivery od WITH(READPAST) ON a.trans_no = od.reff_order_no ");
            //    sql.Append("WHERE 1 = 1 ");
            //    sql.Append("AND a.trans_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`") + "'");
            //    sql.Append("AND a.status = '1' ");
            //    sql.Append("AND od.trans_no IS NULL ");
            //    sql.Append("AND a.trans_no NOT IN (SELECT trans_no FROM rental_return_order_cancelled WITH(READPAST) UNION SELECT trans_no FROM rental_return_order_closed) ");

            //    using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
            //    {
            //        conn.Open();
            //        using (cmd = new SqlCommand(sql.ToString(), conn))
            //        {
            //            using (dr = cmd.ExecuteReader())
            //            {
            //                if (dr.HasRows)
            //                {
            //                    dr.Read();
            //                    lbReff.Text = "RENTAL RETURN ORDER";
            //                    lbReffOrderNo.Text = dr["posted_date"].ToString();
            //                    tbReffOrderNo.ReadOnly = true;
            //                    ibSearchReffOrder.Visible = false;
            //                    imbSource.Visible = true;
            //                    tbShipTo.Text = dr["customer"].ToString();
            //                    tbShipToSite.Text = dr["customer_site"].ToString();
            //                    lbShipTo.Text = dr["customer_name"].ToString();
            //                    installationCharge = double.Parse(dr["cut_off_charge"].ToString());
            //                    lbInstallationBy.Text = dr["cut_off_by"].ToString();
            //                    if (dr["cut_off_by"].ToString() == "S01")
            //                    {
            //                        //tpMaterial.Visible = true;
            //                        grid.Columns[7].Visible = false;
            //                    }
            //                    else
            //                    {
            //                        //tpMaterial.Visible = false;
            //                        grid.Columns[7].Visible = true;
            //                    }

            //                    sql.Length = 0;
            //                    sql.Append("SELECT rro.trans_no, a.article_number, a.article_description, a.article_type, a.base_uom, u.description AS unit_desc, SUM(rrod.qty) AS qty ");
            //                    sql.Append("FROM rental_return_order rro WITH(READPAST) ");
            //                    sql.Append("INNER JOIN rental_return_order_detail rrod WITH(READPAST) ON rro.trans_no = rrod.trans_no ");
            //                    sql.Append("CROSS JOIN article a WITH(READPAST) ");
            //                    sql.Append("INNER JOIN inner_pack u WITH(READPAST) ON a.base_uom = u.id ");
            //                    sql.Append("WHERE rro.trans_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`") + "' ");
            //                    sql.Append("AND a.article_number = '00000010' ");
            //                    sql.Append("GROUP BY rro.trans_no, a.article_number, a.article_description, a.article_type, a.base_uom, u.description ");

            //                    using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
            //                    {
            //                        conn.Open();
            //                        using (cmd = new SqlCommand(sql.ToString(), conn))
            //                        {
            //                            using (dr = cmd.ExecuteReader())
            //                            {
            //                                if (dr.HasRows)
            //                                {
            //                                    DataTable dt = new DataTable();
            //                                    dt = createDataTable();
            //                                    Session["dt"] = dt;
            //                                    dr.Read();
            //                                    addDataToTable(dr["article_number"].ToString(), dr["article_type"].ToString(), "", dr["article_description"].ToString(),
            //                                        dr["base_uom"].ToString(), dr["unit_desc"].ToString(), double.Parse(dr["qty"].ToString()), double.Parse(dr["qty"].ToString()),
            //                                        "", "", "", "0", "", DateTime.Now, (DataTable)Session["dt"]);
            //                                    loadGrid();
            //                                }
            //                            }
            //                        }
            //                    }
            //                    tpMaterial.Visible = false;
            //                    upDetail.Update();
            //                }
            //                else
            //                {
            //                    lbReff.Text = "";
            //                    lbReffOrderNo.Text = "";
            //                    tbReffOrderNo.ReadOnly = false;
            //                    ibSearchReffOrder.Visible = true;
            //                    tbShipTo.Text = "";
            //                    tbShipToSite.Text = "";
            //                    lbShipTo.Text = "";
            //                    lbInstallationBy.Text = "";
            //                    master.messageBox("Invalid reff order no!");
            //                    return;
            //                }
            //                upHeader.Update();
            //            }
            //        }
            //    }
            //}
            //else 
            if (tbReffOrderNo.Text.Contains("RO") == true)
            {

                sql.Length = 0;
                sql.Append("SELECT pr.trans_no, ISNULL(po.trans_no, '') AS po_trans_no, ISNULL(id.trans_no, '') AS id_trans_no ");
                sql.Append("FROM purchase_requisition pr WITH(READPAST) ");
                sql.Append("LEFT JOIN dbo.purchase_order_detail pod WITH(READPAST) ON pr.trans_no = pod.purchase_requisition_no ");
                sql.Append("LEFT JOIN purchase_order po WITH(READPAST) ON pod.trans_no = po.trans_no and po.closed = 0 ");
                sql.Append("LEFT JOIN( ");
                sql.Append("	SELECT trans_no, SUM(approval_proxy) AS sum_approval_proxy, MAX(approval_date) AS max_approval_date ");
                sql.Append("	FROM trans_approval WITH(READPAST) ");
                sql.Append("	GROUP BY trans_no ");
                sql.Append(") AS ta ON po.trans_no = ta.trans_no AND po.total_amount <= ISNULL(ta.sum_approval_proxy, 0) ");
                sql.Append("LEFT JOIN inbound_delivery id WITH(READPAST) ON po.trans_no = id.reff_order_no AND id.status = '1' ");
                sql.Append("WHERE pr.sales_order_no  = '" + tbReffOrderNo.Text.Trim().Replace("'", "`") + "' ");


                using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
                {
                    conn.Open();
                    using (cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        using (dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                if (dr["po_trans_no"].ToString() == "" || dr["id_trans_no"].ToString() == "")
                                {
                                    master.messageBox("Please fullfil PR No. " + dr["trans_no"].ToString() + " first!");
                                    return;
                                }
                            }
                        }
                    }
                }

                double installationCharge = 0;

                sql.Length = 0;
                sql.Append("SELECT CONVERT(VARCHAR, a.posted_date, 106) AS posted_date, a.sold_to, a.ship_to,  ");
                sql.Append("b.customer_name + ' - ' + c.alias_name_full + ' (' + c.alias_name + ') ' AS customer_name, a.installation_by, a.currency_id,a.installation_charge ");
                sql.Append("FROM rental_order a WITH(READPAST) ");
                sql.Append("INNER JOIN customer b WITH(READPAST) ON a.sold_to = b.customer_no ");
                sql.Append("INNER JOIN customer_address c WITH(READPAST) ON a.sold_to = c.customer_no AND c.address_type = '02' AND a.ship_to = c.id ");
                sql.Append("INNER JOIN rental_order_detail e WITH(READPAST) ON a.trans_no = e.trans_no ");
                sql.Append("WHERE 1 = 1 ");
                sql.Append("AND a.trans_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`") + "'");
                sql.Append("AND a.status = '1' ");

                using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
                {
                    conn.Open();
                    using (cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        using (dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                lbReff.Text = "RENTAL ORDER";
                                lbReffOrderNo.Text = dr["posted_date"].ToString();
                                tbReffOrderNo.ReadOnly = true;
                                ibSearchReffOrder.Visible = false;
                                tbShipTo.Text = dr["sold_to"].ToString();
                                tbShipToSite.Text = dr["ship_to"].ToString();
                                lbShipTo.Text = dr["customer_name"].ToString();
                                installationCharge = double.Parse(dr["installation_charge"].ToString());
                                lbInstallationBy.Text = dr["installation_by"].ToString();

                                //upDetail.Update();
                            }
                            else
                            {
                                master.messageBox("Invalid reff order no!");
                                lbReff.Text = "";
                                lbReffOrderNo.Text = "";
                                tbReffOrderNo.ReadOnly = false;
                                ibSearchReffOrder.Visible = true;
                                tbShipTo.Text = "";
                                tbShipToSite.Text = "";
                                lbShipTo.Text = "";
                                lbInstallationBy.Text = "";
                                return;
                            }
                            upHeader.Update();
                        }
                    }
                }
            }
            //else if (tbReffOrderNo.Text.Contains("STO") == true)
            //{
            //    sql.Length = 0;
            //    sql.Append("SELECT a.from_warehouse, ISNULL(b.site_name, c.vendor_name) AS from_warehouse_name, ");
            //    sql.Append("a.to_warehouse, ISNULL(d.site_name, e.vendor_name) AS to_warehouse_name, CONVERT(VARCHAR, a.delivery_date, 106) AS delivery_date, a.note ");
            //    sql.Append("FROM stock_transport_order a WITH(READPAST) ");
            //    sql.Append("LEFT JOIN site b WITH(READPAST) ON a.from_warehouse = b.site_id ");
            //    sql.Append("LEFT JOIN vendor c WITH(READPAST) ON a.from_warehouse = c.vendor_no AND c.manage_warehouse = '1' ");
            //    sql.Append("LEFT JOIN site d WITH(READPAST) ON a.to_warehouse = d.site_id ");
            //    sql.Append("LEFT JOIN vendor e WITH(READPAST) ON a.to_warehouse = e.vendor_no AND e.manage_warehouse = '1' ");
            //    sql.Append("WHERE 1 = 1 ");
            //    sql.Append("AND a.company_id = '" + tbCompanyID.Text + "' ");
            //    sql.Append("AND a.status = '1' ");
            //    sql.Append("AND a.trans_no not in (SELECT trans_no FROM stock_transport_order_cancelled UNION SELECT trans_no FROM stock_transport_order_closed)  ");
            //    if (Session["user_level"].ToString() != "2")//IF NOT PURCHASING
            //    {
            //        sql.Append("AND (a.from_warehouse = '" + tbSiteID.Text + "' OR a.site_id='" + tbSiteID.Text + "') ");
            //    }
            //    sql.Append("AND a.company_id = '" + tbCompanyID.Text + "' ");
            //    sql.Append("AND a.trans_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`").ToUpper() + "' ");

            //    using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
            //    {
            //        conn.Open();
            //        using (cmd = new SqlCommand(sql.ToString(), conn))
            //        {
            //            using (dr = cmd.ExecuteReader())
            //            {
            //                if (dr.HasRows)
            //                {
            //                    dr.Read();
            //                    lbReff.Text = "STOCK TRANSPORT ORDER";
            //                    grid.Columns[7].Visible = false;
            //                    tbRealDeliveryDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            //                    tbRealDeliveryDate.ReadOnly = true;
            //                    ceRealDeliveryDate.Enabled = false;
            //                    ibSearchFromWH.Visible = false;
            //                    imbSource.Visible = true;
            //                    lbReffOrderNo.Text = dr["delivery_date"].ToString();
            //                    tbReffOrderNo.ReadOnly = true;
            //                    ibSearchReffOrder.Visible = false;
            //                    tbFromWH.Text = dr["from_warehouse"].ToString();
            //                    lbFromWH.Text = dr["from_warehouse_name"].ToString();
            //                    tbShipTo.Text = dr["to_warehouse"].ToString();
            //                    lbShipTo.Text = dr["to_warehouse_name"].ToString();
            //                    tbNote.Text = dr["note"].ToString();
            //                    tpMaterial.Visible = false;
            //                    lbInstallationBy.Text = "";
            //                    upDetail.Update();
            //                }
            //                else
            //                {
            //                    lbReff.Text = "";
            //                    ibSearchFromWH.Visible = true;
            //                    imbSource.Visible = false;
            //                    lbReffOrderNo.Text = "";
            //                    tbReffOrderNo.ReadOnly = false;
            //                    ibSearchReffOrder.Visible = true;
            //                    tbFromWH.Text = "";
            //                    lbFromWH.Text = "";
            //                    tbShipTo.Text = "";
            //                    lbShipTo.Text = "";
            //                    tbNote.Text = "";
            //                    master.messageBox("Invalid reff order no!");
            //                    return;
            //                }
            //                upHeader.Update();
            //            }
            //        }
            //    }

            //    sql.Length = 0;
            //    sql.Append("SELECT a.line_no, a.article_type, a.item_type, a.article_no, b.article_description, a.unit_id, d.description AS unit_desc, a.qty, b.managed_item_by, a.serial_no ");
            //    sql.Append("FROM stock_transport_order_detail a WITH(READPAST) ");
            //    sql.Append("INNER JOIN article b WITH(READPAST) ON a.article_no = b.article_number ");
            //    sql.Append("INNER JOIN inner_pack d WITH(READPAST) ON a.unit_id = d.id ");
            //    sql.Append("INNER JOIN uom u WITH(READPAST) ON d.uom_id = u.id ");
            //    sql.Append("LEFT JOIN ( ");
            //    sql.Append("	SELECT od.reff_order_no, odd.reff_line_no, SUM(odd.qty_delivered) AS qty_delivered ");
            //    sql.Append("	FROM outbound_delivery od WITH(READPAST) ");
            //    sql.Append("	INNER JOIN outbound_delivery_detail odd WITH(READPAST) ON od.trans_no = odd.trans_no ");
            //    sql.Append("	GROUP BY od.reff_order_no, odd.reff_line_no ");
            //    sql.Append(") od ON a.trans_no = od.reff_order_no AND a.line_no = od.reff_line_no ");
            //    sql.Append("WHERE a.trans_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`").ToUpper() + "' ");
            //    sql.Append("AND a.qty - ISNULL(od.qty_delivered, 0) > 0 ");

            //    using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
            //    {
            //        conn.Open();
            //        using (cmd = new SqlCommand(sql.ToString(), conn))
            //        {
            //            using (dr = cmd.ExecuteReader())
            //            {
            //                if (dr.HasRows)
            //                {
            //                    DataTable dt = new DataTable();
            //                    dt = createDataTable();
            //                    Session["dt"] = dt;
            //                    while (dr.Read())
            //                    {
            //                        if (dr["item_type"].ToString() == "Stock" && dr["managed_item_by"].ToString() == "SERIAL NO")
            //                        {
            //                            for (int i = 1; i <= double.Parse(dr["qty"].ToString()); i++)
            //                            {
            //                                addDataToTable(dr["article_no"].ToString(), dr["article_type"].ToString(), dr["item_type"].ToString(), dr["article_description"].ToString(),
            //                                dr["unit_id"].ToString(), dr["unit_desc"].ToString(), 1, 1,
            //                                "", dr["managed_item_by"].ToString(), "", dr["line_no"].ToString(), "", DateTime.Now, (DataTable)Session["dt"]);
            //                            }
            //                        }
            //                        else if (dr["item_type"].ToString() == "Stock" && dr["managed_item_by"].ToString() != "SERIAL NO")
            //                        {
            //                            addDataToTable(dr["article_no"].ToString(), dr["article_type"].ToString(), dr["item_type"].ToString(), dr["article_description"].ToString(),
            //                                    dr["unit_id"].ToString(), dr["unit_desc"].ToString(), double.Parse(dr["qty"].ToString()), double.Parse(dr["qty"].ToString()),
            //                                    "", dr["managed_item_by"].ToString(), "", dr["line_no"].ToString(), "", DateTime.Now, (DataTable)Session["dt"]);
            //                        }
            //                        else if (dr["item_type"].ToString() == "Asset")
            //                        {
            //                            addDataToTable(dr["article_no"].ToString(), dr["article_type"].ToString(), dr["item_type"].ToString(), dr["article_description"].ToString(),
            //                                    dr["unit_id"].ToString(), dr["unit_desc"].ToString(), double.Parse(dr["qty"].ToString()), double.Parse(dr["qty"].ToString()),
            //                                    dr["serial_no"].ToString(), dr["managed_item_by"].ToString(), "", dr["line_no"].ToString(), "", DateTime.Now, (DataTable)Session["dt"]);
            //                        }
            //                    }
            //                    loadGrid();
            //                }
            //            }
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void lbReffOrderNo_Click(object sender, EventArgs e)
    {
        tbReffOrderNo.Text = "";
        lbReffOrderNo.Text = "";
        tbReffOrderNo.ReadOnly = false;
        ibSearchReffOrder.Visible = true;
        tbRealDeliveryDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        tbRealDeliveryDate.ReadOnly = false;
        ceRealDeliveryDate.Enabled = true;
        tbFromWH.Text = "";
        lbFromWH.Text = "";
        tbShipTo.Text = "";
        lbShipTo.Text = "";
        tbNote.Text = "";

        tbMovementType.Text = "";
        lbMovementType.Text = "";
        tbMovementType.ReadOnly = false;
        ibSearchMovementType.Visible = true;
        upHeader.Update();

        upHeader.Update();
        //grid.DataSource = null;
        //grid.DataBind();
        //upGrid.Update();
    }

    protected DataTable createDataTable()
    {
        try
        {
            dt = new DataTable();

            DataColumn dtColumn;

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Int64");
            dtColumn.ColumnName = "lineNo";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "articleNo";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "articleType";
            dt.Columns.Add(dtColumn);
            
            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "articleDesc";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "qty_ordered";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "qty_delivered";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "serial_no";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "managed_item_by";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "article_source";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "reffLineNo";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "noteDetail";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.DateTime");
            dtColumn.ColumnName = "addedDate";
            dt.Columns.Add(dtColumn);

            return dt;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private void addDataToTable(string articleNo, string articleType, string articleDesc,
    double qty_ordered, double qty_delivered,string managed_item_by, string article_source, string reffLineNo, string noteDetail, string serialNo, DateTime addedDate, DataTable Table)
    {
        try
        {
            DataRow dtRow;

            dtRow = Table.NewRow();

            string lineNo = ((DataTable)Session["dt"]).Compute("MAX(lineNo)", "").ToString();
            if (lineNo == "")
            {
                lineNo = "10";
            }
            else
            {
                lineNo = (int.Parse(lineNo) + 10).ToString();
            }

            dtRow["lineNo"] = lineNo;
            dtRow["articleNo"] = articleNo;
            dtRow["articleType"] = articleType;
            dtRow["articleDesc"] = articleDesc;
            dtRow["qty_ordered"] = qty_ordered;
            dtRow["qty_delivered"] = qty_delivered;
            dtRow["managed_item_by"] = managed_item_by;
            dtRow["article_source"] = article_source;
            dtRow["reffLineNo"] = reffLineNo;
            dtRow["noteDetail"] = noteDetail;
            dtRow["serial_no"] = serialNo;
            dtRow["addedDate"] = addedDate;

            Table.Rows.Add(dtRow);
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }


    protected void ibSearchMovementType_Click(object sender, EventArgs e)
    {
        if (lbReff.Text == "")
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox("Input reff order no!");
            return;
        }

        Session["search"] = "od_movement_type";
        Session["searchParamReffType"] = lbReff.Text;
        wucSearch1.loadGrid();
    }

    protected void bSearchMovementType_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT a.description FROM movement_type a WITH(READPAST) ");
            sql.Append("WHERE 1 = 1 ");
            if (lbReff.Text == "RENTAL RETURN ORDER")
            {
                sql.Append("AND a.id IN ('Z03') ");
            }
            else if (lbReff.Text == "RENTAL ORDER")
            {
                sql.Append("AND a.id IN ('Z01') ");
            }
            else if (lbReff.Text == "STOCK TRANSPORT ORDER")
            {
                sql.Append("AND a.id IN ('301', '303') ");
            }
            sql.Append("AND a.id = '" + tbMovementType.Text.Trim().Replace("'", "`") + "' ");

            using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
            {
                conn.Open();
                using (cmd = new SqlCommand(sql.ToString(), conn))
                {
                    using (dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            lbMovementType.Text = dr["description"].ToString();
                            tbMovementType.ReadOnly = true;
                            ibSearchMovementType.Visible = false;
                        }
                        else
                        {
                            lbMovementType.Text = "";
                            tbMovementType.ReadOnly = false;
                            ibSearchMovementType.Visible = true;
                            master.messageBox("Invalid movement type!");
                        }
                        upHeader.Update();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void lbMovementType_Click(object sender, EventArgs e)
    {
        tbMovementType.Text = "";
        lbMovementType.Text = "";
        tbMovementType.ReadOnly = false;
        ibSearchMovementType.Visible = true;
        upHeader.Update();
    }

    protected void lbSerialNo_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gridRow = ((sender as LinkButton).Parent.Parent.Parent as GridViewRow);
            TextBox tbSerialNo = (TextBox)gridRow.FindControl("tbSerialNo");
            LinkButton lbSerialNo = (LinkButton)gridRow.FindControl("lbSerialNo");
            ImageButton ibSearchSerialNo = (ImageButton)gridRow.FindControl("ibSearchSerialNo");

            if (lStatus.Text == "POSTED")
            {
                return;
            }
            else
            {
                tbSerialNo.Text = "";
                tbSerialNo.ReadOnly = false;
                tbSerialNo.Visible = true;
                lbSerialNo.Text = "";
                ibSearchSerialNo.Visible = true;
            }

            upGrid.Update();
        }
        catch (Exception ex)
        {

        }
    }

    protected void bSearchSerialNo_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gridRow = ((sender as Button).Parent.Parent.Parent as GridViewRow);
            Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
            Label lItemType = (Label)gridRow.FindControl("lItemType");
            TextBox tbSerialNo = (TextBox)gridRow.FindControl("tbSerialNo");
            LinkButton lbSerialNo = (LinkButton)gridRow.FindControl("lbSerialNo");
            ImageButton ibSearchSerialNo = (ImageButton)gridRow.FindControl("ibSearchSerialNo");

            if (lItemType.Text == "Stock" || lItemType.Text == "STOCK")
            {
                sql.Append("SELECT serial_no FROM article_serial_no WITH(READPAST) ");
                sql.Append("WHERE 1 = 1 ");
                sql.Append("AND active = '1' ");
                sql.Append("AND site_id = '" + tbFromWH.Text + "' ");
                sql.Append("AND article_no = '" + lArticleNo.Text + "' ");
                sql.Append("AND serial_no = '" + tbSerialNo.Text.Trim().Replace("'", "`").ToUpper() + "' ");

                if (cAdih.getResultHasRows(sql.ToString(), cAdih.getConnStr("Connection")))
                {
                    lbSerialNo.Text = tbSerialNo.Text;
                    lbSerialNo.Visible = true;
                    tbSerialNo.ReadOnly = true;
                    tbSerialNo.Visible = false;
                    ibSearchSerialNo.Visible = false;
                    upGrid.Update();
                }
                else
                {
                    MasterPage master = (MasterPage)this.Master;
                    master.messageBox("Invalid Serial No!");
                }
            }
            else if (lItemType.Text == "Asset")
            {

            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void ibSearchSerialNo_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gridRow = ((sender as ImageButton).Parent.Parent.Parent as GridViewRow);
            Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");

            Session["search"] = "article_serial_no";
            Session["searchParamSiteID"] = tbFromWH.Text;
            Session["searchParamArticleNo"] = lArticleNo.Text;
            Session["searchGridRowIndex"] = gridRow.RowIndex.ToString();
            wucSearch1.loadGrid();
        }
        catch (Exception ex)
        {

        }
    }


    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridViewRow Row = e.Row;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lManagedItemBy = (Label)Row.FindControl("lManagedItemBy");
                TextBox tbSerialNo = (TextBox)Row.FindControl("tbSerialNo");
                LinkButton lbSerialNo = (LinkButton)Row.FindControl("lbSerialNo");
                ImageButton ibSearchSerialNo = (ImageButton)Row.FindControl("ibSearchSerialNo");
                TextBox tbQtyDelivered = (TextBox)Row.FindControl("tbQtyDelivered");

                if (lManagedItemBy.Text == "SERIAL NO")
                {
                    tbSerialNo.Visible = true;
                    ibSearchSerialNo.Visible = true;
                }
                else
                {
                    tbSerialNo.Visible = false;
                    ibSearchSerialNo.Visible = false;
                }

                if(tbSerialNo.Text != "")
                {
                    tbSerialNo.Visible = false;
                    ibSearchSerialNo.Visible = false;
                    lbSerialNo.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        try
        {
            if (wucConfirmBox1.StrMode == "Yes")
            {
                if (Session["OutboundDeliveryMode"].ToString() == "Save")
                {
                    saveData();
                }
                else if (Session["OutboundDeliveryMode"].ToString() == "Posting")
                {
                    postingData();
                }

            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void postingData()
    {
        MasterPage master = (MasterPage)this.Master;
        conn = new SqlConnection(cAdih.getConnStr("Connection"));
        conn.Open();
        trans = conn.BeginTransaction();

        try
        {
            conn = new SqlConnection(cAdih.getConnStr("Connection"));
            conn.Open();
            trans = conn.BeginTransaction();

            sql.Length = 0;
            sql.Append("EXEC sp_outbound_delivery_post '" + tbTransNo.Text + "','" + Session["username"].ToString() + "','" + DateTime.Now + "','" + cAdih.getIPAddress() + "' ");

            using (cmd = new SqlCommand(sql.ToString(), conn, trans))
            {
                cmd.ExecuteNonQuery();
                cmd = null;
            }

            trans.Commit();
            master.messageBox("Data has been posted.");
            lStatus.Text = "POSTED";
            statusChanged();
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message + " <br> " + ex.InnerException);
            trans1.Rollback();
            conn1.Close();
        }
    }


    protected void saveData()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            conn = new SqlConnection(cAdih.getConnStr("Connection"));
            conn.Open();
            trans = conn.BeginTransaction();
            //CREATE NEW
            if (tbTransNo.Text == "")
            {
                sql.Length = 0;
                sql.Append("SELECT RIGHT('000000' + CAST(ISNULL(MAX(CAST(RIGHT(trans_no, 6) AS INT)), 0) + 1 AS VARCHAR), 6) AS running_no, ");
                sql.Append("YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) AS tahun, RIGHT('00' + CAST(MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) AS VARCHAR), 2) AS bulan ");
                sql.Append("FROM outbound_delivery ");
                sql.Append("WHERE YEAR(trans_date) = YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) ");
                sql.Append("AND MONTH(trans_date) = MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    using (dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            tbTransNo.Text = "OD" + dr["tahun"].ToString() + dr["bulan"].ToString() + dr["running_no"].ToString();
                            upHeader.Update();
                        }
                    }
                }

                sql.Length = 0;
                sql.Append("INSERT INTO outbound_delivery(trans_no, trans_date, reff, reff_order_no, from_warehouse, ship_to, ");
                sql.Append("real_delivery_date, movement_type, note, status, created_by, created_date) VALUES( ");
                sql.Append("'" + tbTransNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("CONVERT(VARCHAR(10), GETDATE(), 120), ");
                sql.Append("'" + lbReff.Text + "', ");
                sql.Append("'" + tbReffOrderNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + tbFromWH.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + tbShipTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + cAdih.engFormatDate(tbRealDeliveryDate.Text) + "', ");
                sql.Append("'" + tbMovementType.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + tbNote.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'0', ");
                sql.Append("'" + Session["username"].ToString() + "', ");
                sql.Append("GETDATE() ) ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                int line_no = 10;

                foreach (GridViewRow gridRow in grid.Rows)
                {
                    Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
                    Label lArticleType = (Label)gridRow.FindControl("lArticleType");
                    Label lItemType = (Label)gridRow.FindControl("lItemType");
                    Label lUnitID = (Label)gridRow.FindControl("lUnitID");
                    Label lQtyOrdered = (Label)gridRow.FindControl("lQtyOrdered");
                    TextBox tbQtyDelivered = (TextBox)gridRow.FindControl("tbQtyDelivered");
                    LinkButton lbSerialNo = (LinkButton)gridRow.FindControl("lbSerialNo");
                    Label lReffLineNo = (Label)gridRow.FindControl("lReffLineNo");
                    TextBox tbNoteDetail = (TextBox)gridRow.FindControl("tbNoteDetail");

                    if (double.Parse(tbQtyDelivered.Text) > 0)
                    {
                        sql.Length = 0;
                        sql.Append("INSERT INTO outbound_delivery_detail(trans_no, line_no,article_no, ");
                        sql.Append(" qty_ordered, qty_delivered, serial_no, reff_line_no, note) VALUES( ");
                        sql.Append("'" + tbTransNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + line_no + "', ");
                        sql.Append("'" + lArticleNo.Text + "', ");
                        sql.Append("'" + double.Parse(lQtyOrdered.Text) + "', ");
                        sql.Append("'" + double.Parse(tbQtyDelivered.Text) + "', ");
                        sql.Append("'" + lbSerialNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + lReffLineNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + tbNoteDetail.Text.Trim().Replace("'", "`").ToUpper() + "') ");

                        using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                        {
                            cmd.ExecuteNonQuery();
                            cmd = null;
                        }
                    }

                    line_no += 10;
                }

                line_no = 10;

                master.messageBox("Data has been saved. Trans No: " + tbTransNo.Text + "");
            }
            //EDIT
            else if (tbTransNo.Text != "")
            {

                sql.Length = 0;
                sql.Append("UPDATE outbound_delivery SET reff = '" + lbReff.Text + "', ");
                sql.Append("reff_order_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("from_warehouse = '" + tbFromWH.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("ship_to = '" + tbShipTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("real_delivery_date = '" + cAdih.engFormatDate(tbRealDeliveryDate.Text) + "', ");
                sql.Append("movement_type = '" + tbMovementType.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("note = '" + tbNote.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("last_modified_by = '" + Session["username"].ToString() + "', ");
                sql.Append("last_modified_date = GETDATE() ");
                sql.Append("WHERE trans_no = '" + tbTransNo.Text + "' ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                sql.Length = 0;
                sql.Append("DELETE outbound_delivery_detail WHERE trans_no = '" + tbTransNo.Text + "' ");
                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                int line_no = 10;

                foreach (GridViewRow gridRow in grid.Rows)
                {
                    Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
                    Label lArticleType = (Label)gridRow.FindControl("lArticleType");
                    Label lItemType = (Label)gridRow.FindControl("lItemType");
                    Label lUnitID = (Label)gridRow.FindControl("lUnitID");
                    Label lQtyOrdered = (Label)gridRow.FindControl("lQtyOrdered");
                    TextBox tbQtyDelivered = (TextBox)gridRow.FindControl("tbQtyDelivered");
                    LinkButton lbSerialNo = (LinkButton)gridRow.FindControl("lbSerialNo");
                    Label lReffLineNo = (Label)gridRow.FindControl("lReffLineNo");
                    TextBox tbNoteDetail = (TextBox)gridRow.FindControl("tbNoteDetail");

                    if (double.Parse(tbQtyDelivered.Text) > 0)
                    {
                        sql.Length = 0;
                        sql.Append("INSERT INTO outbound_delivery_detail(trans_no, line_no,article_no, ");
                        sql.Append(" qty_ordered, qty_delivered, serial_no, reff_line_no, note) VALUES( ");
                        sql.Append("'" + tbTransNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + line_no + "', ");
                        sql.Append("'" + lArticleNo.Text + "', ");
                        sql.Append("'" + double.Parse(lQtyOrdered.Text) + "', ");
                        sql.Append("'" + double.Parse(tbQtyDelivered.Text) + "', ");
                        sql.Append("'" + lbSerialNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + lReffLineNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + tbNoteDetail.Text.Trim().Replace("'", "`").ToUpper() + "') ");

                        using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                        {
                            cmd.ExecuteNonQuery();
                            cmd = null;
                        }
                    }

                    line_no += 10;
                }

                master.messageBox("Data has been updated.");
            }
            trans.Commit();
            conn.Close();
            lStatus.Text = "OPEN";
            statusChanged();

        }
        catch (Exception ex)
        {
            tbTransNo.Text = "";
            trans.Rollback();
            conn.Close();
            master.messageBox("Fail To Execute" + "<br />" + ex.InnerException);
        }
    }

    protected void wucSearch_Hide(object sender, EventArgs e)
    {
        if (Session["search"].ToString() == "od_movement_type")
        {
            tbMovementType.Text = wucSearch1.result;
            bSearchMovementType_Click(sender, e);
            upHeader.Update();
            Session["searchParamReffType"] = null;
        }
        else if (Session["search"].ToString() == "article_serial_no")
        {
            TextBox tbSerialNo = (TextBox)grid.Rows[int.Parse(Session["searchGridRowIndex"].ToString())].FindControl("tbSerialNo");
            LinkButton lbSerialNo = (LinkButton)grid.Rows[int.Parse(Session["searchGridRowIndex"].ToString())].FindControl("lbSerialNo");
            ImageButton ibSearchSerialNo = (ImageButton)grid.Rows[int.Parse(Session["searchGridRowIndex"].ToString())].FindControl("ibSearchSerialNo");
            tbSerialNo.Text = wucSearch1.result;
            tbSerialNo.ReadOnly = true;
            tbSerialNo.Visible = false;
            lbSerialNo.Text = wucSearch1.result;
            lbSerialNo.Visible = true;
            ibSearchSerialNo.Visible = false;
            upGrid.Update();
            Session["searchParamCompanyID"] = null;
            Session["searchParamSiteID"] = null;
            Session["searchParamArticleNo"] = null;
        }
        else if (Session["search"].ToString() == "single_article")
        { 
        //{
        //    tbArticleNo.Text = wucSearch1.result;
        //    bSearchArticle_Click(sender, e);
        //    upDetailMaterial.Update();
        }
        else if (Session["search"].ToString() == "od_source")
        {
            tbFromWH.Text = wucSearch1.result;
            //btnSource_Click(sender, e);
            upHeader.Update();
            upGrid.Update();
        }
        else if (Session["search"].ToString() == "od_reff")
        {
            tbReffOrderNo.Text = wucSearch1.result;
            bSearchReffOrder_Click(sender,e);
            upHeader.Update();
            upGrid.Update();
        }
        else if (Session["search"].ToString() == "all_warehouse")
        {
            tbFromWH.Text = wucSearch1.result;
            bSearchFromWH_Click(sender, e);
            upHeader.Update();
            upGrid.Update();
        }
        Session["search"] = null;
    }


    protected void ibSearchFromWH_Click(object sender, ImageClickEventArgs e)
    {
        Session["search"] = "all_warehouse";
        wucSearch1.loadGrid();
    }

    protected void bSearchFromWH_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (lbReff.Text == "RENTAL ORDER" || lbReff.Text == "RENTAL RETURN ORDER")
            {
                sql.Length = 0;
                sql.Append("SELECT wh_id, wh_description FROM (");
                sql.Append("    SELECT wh_id, wh_description FROM site_wh WITH(READPAST) ");
                sql.Append("    UNION ");
                sql.Append("    SELECT vendor_no, vendor_name FROM vendor WITH(READPAST) ");
                sql.Append(") AS tbl ");
                sql.Append("WHERE tbl.wh_id = '" + tbFromWH.Text.Trim().Replace("'", "`") + "' ");

                using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
                {
                    conn.Open();
                    using (cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        using (dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                lbFromWH.Text = dr["wh_description"].ToString();
                                tbFromWH.ReadOnly = true;
                                ibSearchFromWH.Visible = false;

                                if (tbReffOrderNo.Text.Contains("RO") == true)
                                {
                                    sql1.Length = 0;
                                    sql1.Append("SELECT DISTINCT a.line_no, b.article_type, d.description AS item_type, a.article_no, b.article_description, ");
                                    sql1.Append("a.qty, b.managed_item_by, a.source_id ");
                                    sql1.Append("FROM rental_order_detail a WITH(READPAST) ");
                                    sql1.Append("INNER JOIN article b WITH(READPAST) ON a.article_no = b.article_no ");
                                    sql1.Append("INNER JOIN source d WITH(READPAST) ON a.source_id = d.id ");
                                    sql1.Append("LEFT JOIN( ");
                                    sql1.Append("    SELECT od.reff_order_no, odd.reff_line_no, SUM(odd.qty_delivered) AS qty_delivered ");
                                    sql1.Append("    FROM outbound_delivery od WITH(READPAST) ");
                                    sql1.Append("    INNER JOIN outbound_delivery_detail odd WITH(READPAST) ON od.trans_no = odd.trans_no ");
                                    sql1.Append("    GROUP BY od.reff_order_no, odd.reff_line_no ");
                                    sql1.Append(") od ON a.trans_no = od.reff_order_no AND a.line_no = od.reff_line_no ");
                                    sql1.Append("WHERE 1 = 1 ");
                                    sql1.Append("and a.source_id <> 'I02' ");
                                    sql1.Append("AND a.qty - ISNULL(od.qty_delivered, 0) > 0 ");
                                    sql1.Append("AND a.wh_id = '" + tbFromWH.Text.Trim().Replace("'", "`") + "' ");
                                    sql1.Append("AND a.trans_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`") + "' ");

                                    using (conn1 = new SqlConnection(cAdih.getConnStr("Connection")))
                                    {
                                        conn1.Open();
                                        using (cmd1 = new SqlCommand(sql1.ToString(), conn1))
                                        {
                                            using (dr1 = cmd1.ExecuteReader())
                                            {
                                                if (dr1.HasRows)
                                                {
                                                    DataTable dt = new DataTable();
                                                    dt = createDataTable();
                                                    Session["dt"] = dt;
                                                    while (dr1.Read())
                                                    {
                                                        if (dr1["managed_item_by"].ToString() == "SERIAL NO")
                                                        {
                                                            for (int i = 1; i <= double.Parse(dr1["qty"].ToString()); i++)
                                                            {
                                                                addDataToTable(dr1["article_no"].ToString(), dr1["article_type"].ToString(),dr1["article_description"].ToString(),
                                                                1, 0, dr1["managed_item_by"].ToString(), dr1["source_id"].ToString(), dr1["line_no"].ToString(), "", "", DateTime.Now, (DataTable)Session["dt"]);
                                                            }
                                                        }
                                                        else if (dr1["managed_item_by"].ToString() != "SERIAL NO")
                                                        {
                                                            addDataToTable(dr1["article_no"].ToString(), dr1["article_type"].ToString(), dr1["article_description"].ToString(),
                                                            double.Parse(dr1["qty"].ToString()), double.Parse(dr1["qty"].ToString()),
                                                            dr1["managed_item_by"].ToString(), dr1["source_id"].ToString(), dr1["line_no"].ToString(), "","", DateTime.Now, (DataTable)Session["dt"]);
                                                        }
                                                    }
                                                    loadGrid();
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {
                                lbFromWH.Text = "";
                                tbFromWH.ReadOnly = false;
                                ibSearchFromWH.Visible = true;
                                master.messageBox("Invalid warehouse!");
                            }
                            upHeader.Update();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }
    protected void loadGrid()
    {
        try
        {
            grid.DataSource = ((DataTable)Session["dt"]).DefaultView;
            grid.DataBind();
            upHeader.Update();
            upGrid.Update();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;

        int countRowDetail = grid.Rows.Count;
        //if (countRowDetail + countRowMaterial <= 0)
        //{
        //    master.messageBox("Invalid Reff Order No " + tbReffOrderNo.Text + "");
        //    return;
        //}

        foreach (GridViewRow gridRow in grid.Rows)
        {
            Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
            Label lArticleDesc = (Label)gridRow.FindControl("lArticleDesc");
            Label lManagedItemBy = (Label)gridRow.FindControl("lManagedItemBy");
            Label lItemType = (Label)gridRow.FindControl("lItemType");
            TextBox tbQtyDelivery = (TextBox)gridRow.FindControl("tbQtyDelivered");
            LinkButton lbSerialNo = (LinkButton)gridRow.FindControl("lbSerialNo");
            Label lQtyOrdered = (Label)gridRow.FindControl("lQtyOrdered");
            Label lReffLineNo = (Label)gridRow.FindControl("lReffLineNo");

            if (lManagedItemBy.Text == "SERIAL NO" && lbSerialNo.Text == "" && double.Parse(tbQtyDelivery.Text) > 0 )
            {
                master.messageBox("Input serial no for article " + lArticleDesc.Text + " (" + lArticleNo.Text + "!");
                return;
            }

            if (double.Parse(lQtyOrdered.Text) < double.Parse(tbQtyDelivery.Text))
            {
                master.messageBox("Qty Delivery Exceeds Qty Ordered!");
                return;
            }
        }

        string duplicateSerialNo = duplicateSerialNoExists();

        if (lbReffOrderNo.Text == "")
        {
            master.messageBox("Input reff order!");
            return;
        }
        else if (lbFromWH.Text == "")
        {
            master.messageBox("Input source!");
            return;
        }
        else if (lbShipTo.Text == "")
        {
            master.messageBox("Input destination!");
            return;
        }
        else if (lbMovementType.Text == "")
        {
            master.messageBox("Input movement type!");
            return;
        }
        else if (tbNote.Text == "")
        {
            master.messageBox("Input Note!");
            return;
        }
        else if (duplicateSerialNo != "")
        {
            master.messageBox("Serial No: " + duplicateSerialNo + " duplicate!");
            return;
        }
        else
        {
            Session["OutboundDeliveryMode"] = "Save";
            showConfirmBox("Save Data?");
        }
    }


    protected void showConfirmBox(string message)
    {
        wucConfirmBox1.SubShowConfirmBox(message);
    }

    protected string duplicateSerialNoExists()
    {
        MasterPage master = (MasterPage)this.Master;
        foreach (GridViewRow gridRow in grid.Rows)
        {
            Label lManagedItemBy = (Label)gridRow.FindControl("lManagedItemBy");
            TextBox tbQtyDelivery = (TextBox)gridRow.FindControl("tbQtyDelivered");
            LinkButton lbSerialNo = (LinkButton)gridRow.FindControl("lbSerialNo");

            if (double.Parse(tbQtyDelivery.Text) > 0 && ((lManagedItemBy.Text == "SERIAL NO")))
            {
                int countSerialNo = 0;
                foreach (GridViewRow gridRowB in grid.Rows)
                {
                    LinkButton lbSerialNoB = (LinkButton)gridRowB.FindControl("lbSerialNo");
                    if (lbSerialNo.Text.Trim().Replace("'", "`") == lbSerialNoB.Text.Trim().Replace("'", "`"))
                    {
                        countSerialNo += 1;
                    }
                }
                if (countSerialNo > 1)
                {
                    return lbSerialNo.Text;
                }
            }
        }
        return "";
    }

    protected void btnPosting_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;

        string duplicateSerialNo = duplicateSerialNoExists();

        if (lbReffOrderNo.Text == "")
        {
            master.messageBox("Input reff order!");
            return;
        }
        else if (lbFromWH.Text == "")
        {
            master.messageBox("Input source!");
            return;
        }
        else if (lbShipTo.Text == "")
        {
            master.messageBox("Input destination!");
            return;
        }
        else if (lbMovementType.Text == "")
        {
            master.messageBox("Input movement type!");
            return;
        }
        else if (tbNote.Text == "")
        {
            master.messageBox("Input Note!");
            return;
        }
        else if (duplicateSerialNo != "")
        {
            master.messageBox("Serial No: " + duplicateSerialNo + " duplicate!");
            return;
        }
        else
        {
            Session["OutboundDeliveryMode"] = "Posting";
            showConfirmBox("Posting Data?");
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("BarangKeluarList.aspx");
    }
}