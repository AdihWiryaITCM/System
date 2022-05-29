using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.WebControls;

public partial class Transaction_InboundDeliveryProcess : System.Web.UI.Page
{
    private StringBuilder sql = new StringBuilder();
    private StringBuilder sql1 = new StringBuilder();
    private SqlTransaction trans;
    private SqlTransaction trans1;
    private SqlConnection conn;
    private SqlConnection conn1;
    private SqlCommand cmd;
    private SqlCommand cmd1;
    private SqlDataReader dr;
    private SqlDataReader dr1;
    private DataTable dt;
    private ClassAdih cAdih = new ClassAdih();
    private double dtotalDetail = 0;
    private string pricezone;
    private string roType;
    private static string TABLE_NAME = "inbound_delivery_header";
    private static string TABLE_NAME_DETAIL = "inbound_delivery_detail";
    private static string TABLE_NAME_WO_ORDER_DETAIL = "inbound_delivery_item_wo_order_detail";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                Session["article"] = "";
                if (Request.QueryString["mode"].ToString() == "add")
                {
                    tbTransDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    ibSearchMovementType.Visible = false;

                    lStatus.Text = "";
                    statusChanged();
                }
                else if (Request.QueryString["mode"].ToString() == "edit")
                {
                    tbTransNo.Text = Request.QueryString["trans_no"].ToString();

                    sql.Length = 0;
                    sql.Append("SELECT a.trans_no, a.trans_date, a.reff, a.reff_order_no, ");
                    sql.Append("(CASE ");
                    sql.Append("    WHEN a.reff = 'PURCHASE ORDER' THEN po.posted_date ");
                    //sql.Append("    --WHEN a.reff = 'OUTBOUND DELIVERY' THEN od.posted_date ");
                    //sql.Append("    --WHEN a.reff = 'RENTAL RETURN ORDER' THEN rro.posted_date ");
                    sql.Append("END) AS reff_order_desc, ");
                    sql.Append("a.from_site, e.vendor_name as from_site_name, ");
                    sql.Append("a.ship_to, h.wh_description AS ship_to_name, ");
                    sql.Append("a.movement_type, l.description AS movement_type_name, ");
                    sql.Append("CONVERT(VARCHAR(11), a.real_receive_date, 105) AS real_receive_date, a.note, a.status, CONVERT(VARCHAR(11), a.document_receive_date, 105) AS document_receive_date ");
                    sql.Append("FROM inbound_delivery a WITH(READPAST) ");
                    sql.Append("LEFT JOIN vendor e WITH(READPAST) ON a.from_site = e.vendor_no ");
                    sql.Append("LEFT JOIN site_wh h WITH(READPAST) ON a.ship_to = h.wh_id ");
                    sql.Append("LEFT JOIN vendor i WITH(READPAST) ON a.ship_to = i.vendor_no ");
                    sql.Append("LEFT JOIN movement_type l ON a.movement_type = l.id ");
                    sql.Append("LEFT JOIN( ");
                    sql.Append("    SELECT po.trans_no, CONVERT(VARCHAR(11), ta.max_approval_date, 106) AS posted_date ");
                    sql.Append("    FROM purchase_order po WITH(READPAST) ");
                    sql.Append("    LEFT JOIN( ");
                    sql.Append("        SELECT trans_no, SUM(approval_proxy) AS sum_approval_proxy, MAX(approval_date) AS max_approval_date ");
                    sql.Append("        FROM trans_approval WITH(READPAST) ");
                    sql.Append("        GROUP BY trans_no ");
                    sql.Append("    ) AS ta ON po.trans_no = ta.trans_no ");
                    sql.Append(") AS po ON a.reff_order_no = po.trans_no ");
                    sql.Append("--LEFT JOIN( ");
                    sql.Append("--SELECT od.trans_no, CONVERT(VARCHAR(11), od.posted_date, 106) AS posted_date ");
                    sql.Append("--  FROM outbound_delivery od WITH(READPAST) ");
                    sql.Append("--) AS od ON a.reff_order_no = od.trans_no ");
                    sql.Append("--LEFT JOIN( ");
                    sql.Append("--SELECT rro.trans_no, CONVERT(VARCHAR(11), rro.posted_date, 106) AS posted_date ");
                    sql.Append("--  FROM rental_return_order rro WITH(READPAST) ");
                    sql.Append("--) AS rro ON a.reff_order_no = rro.trans_no ");
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
                                    lbReffOrderNo.Text = dr["reff_order_desc"].ToString();
                                    tbReffOrderNo.ReadOnly = true;
                                    ibSearchReffOrder.Visible = false;
                                    tbFrom.Text = dr["from_site"].ToString();
                                    lbFrom.Text = dr["from_site_name"].ToString();
                                    tbFrom.ReadOnly = true;
                                    tbShipTo.Text = dr["ship_to"].ToString();
                                    lbShipTo.Text = dr["ship_to_name"].ToString();
                                    tbShipTo.ReadOnly = true;
                                    tbMovementType.Text = dr["movement_type"].ToString();
                                    lbMovementType.Text = dr["movement_type_name"].ToString();
                                    tbMovementType.ReadOnly = true;
                                    txtRealReceiveDate.Text = dr["real_receive_date"].ToString();
                                    txtDocReceivedate.Text = dr["document_receive_date"].ToString();
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
                                    }
                                    upDetail.Update();
                                }
                            }
                        }
                    }

                    sql.Length = 0;
                    sql.Append("SELECT a.line_no, b.article_type, a.article_no, b.article_description,b.base_uom as unit_id, ");
                    sql.Append("a.qty_ordered, qty_received, a.managed_item_by, a.serial_no, a.reff_line_no, a.note ");
                    sql.Append("FROM inbound_delivery_detail a WITH(READPAST) ");
                    sql.Append("INNER JOIN article b WITH(READPAST) ON a.article_no = b.article_no ");
                    sql.Append("WHERE a.trans_no = '" + tbTransNo.Text + "' ");
                    sql.Append("ORDER BY a.line_no ");

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
                                        addDataToTable(dr["article_no"].ToString(), dr["article_type"].ToString(), dr["article_description"].ToString(),
                                            dr["unit_id"].ToString(), double.Parse(dr["qty_ordered"].ToString()), double.Parse(dr["qty_received"].ToString()),
                                            dr["managed_item_by"].ToString(), dr["serial_no"].ToString(), dr["note"].ToString(), dr["reff_line_no"].ToString(), DateTime.Now, (DataTable)Session["dt"]);
                                    }
                                    loadGrid();
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
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

    protected void showConfirmBox(string message)
    {
        wucConfirmBox1.SubShowConfirmBox(message);
    }

    protected void ibSearchReffOrder_Click(object sender, EventArgs e)
    {
        Session["search"] = "id_reff";

        wucSearch1.loadGrid();
    }

    protected void bSearchReffOrder_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (tbReffOrderNo.Text.Contains("PO") == true)
            {
                sql.Length = 0;
                sql.Append("SELECT  a.vendor_id, ");
                sql.Append("        b.vendor_name, ");
                sql.Append("        a.ship_to, ");
                sql.Append("        d.wh_description, ");
                sql.Append("        CONVERT(VARCHAR, a.trans_date, 106) AS po_desc,");
                sql.Append("        a.note ");
                sql.Append("FROM purchase_order a WITH(READPAST) ");
                sql.Append("INNER JOIN vendor b WITH(READPAST) ON a.vendor_id = b.vendor_no ");
                sql.Append("INNER JOIN site_wh d WITH(READPAST) ON a.ship_to = d.wh_id ");
                sql.Append("INNER JOIN purchase_order_detail g WITH(READPAST) ON a.trans_no = g.trans_no ");
                sql.Append("LEFT JOIN( ");
                sql.Append("    SELECT trans_no, SUM(approval_proxy) AS sum_approval_proxy, MAX(approval_date) AS max_approval_date ");
                sql.Append("    FROM trans_approval WITH(READPAST) ");
                sql.Append("    GROUP BY trans_no ");
                sql.Append(") AS ta ON a.trans_no = ta.trans_no ");
                sql.Append("LEFT JOIN( ");
                sql.Append("    SELECT id.reff_order_no, idd.reff_line_no, SUM(idd.qty_received) AS qty_received ");
                sql.Append("    FROM inbound_delivery id WITH(READPAST) ");
                sql.Append("    INNER JOIN inbound_delivery_detail idd WITH(READPAST) ON id.trans_no = idd.trans_no ");
                sql.Append("    GROUP BY id.reff_order_no, idd.reff_line_no ");
                sql.Append(") id ON a.trans_no = id.reff_order_no AND g.line_no = id.reff_line_no ");
                sql.Append("WHERE 1 = 1 ");
                sql.Append("AND a.total_amount <= ISNULL(ta.sum_approval_proxy, 0) ");
                sql.Append("AND g.qty - ISNULL(id.qty_received, 0) > 0 ");
                sql.Append("AND a.trans_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`") + "' ");

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
                                tbReffOrderNo.ReadOnly = true;
                                ibSearchReffOrder.Visible = false;
                                lbReffOrderNo.Text = dr["po_desc"].ToString();
                                lbReff.Text = "PURCHASE ORDER";
                                tbFrom.Text = dr["vendor_id"].ToString();
                                lbFrom.Text = dr["vendor_name"].ToString();
                                tbShipTo.Text = dr["ship_to"].ToString();
                                lbShipTo.Text = dr["wh_description"].ToString();
                                ibSearchMovementType.Visible = true;

                                tbNote.Text = dr["note"].ToString();
                                upDetail.Update();
                            }
                            else
                            {
                                lbReffOrderNo.Text = "";
                                tbReffOrderNo.ReadOnly = false;
                                ibSearchReffOrder.Visible = true;
                                lbReff.Text = "";
                                tbFrom.Text = "";
                                lbFrom.Text = "";
                                tbShipTo.Text = "";
                                lbShipTo.Text = "";
                                upDetail.Update();
                                master.messageBox("Invalid reff order no!");
                                return;
                            }
                            upHeader.Update();
                        }
                    }
                }

                sql.Length = 0;
                sql.Append("SELECT b.line_no, c.article_type, b.article_no, c.article_description, ");
                sql.Append("b.qty - ISNULL(id.qty_received, 0) AS qty, c.managed_item_by, b.note AS note_detail, c.base_uom ");
                sql.Append("FROM purchase_order a WITH(READPAST) ");
                sql.Append("INNER JOIN purchase_order_detail b WITH(READPAST) ON a.trans_no = b.trans_no ");
                sql.Append("INNER JOIN article c WITH(READPAST) ON b.article_no = c.article_no ");
                sql.Append("LEFT JOIN( ");
                sql.Append("    SELECT trans_no, SUM(approval_proxy) AS sum_approval_proxy, MAX(approval_date) AS max_approval_date ");
                sql.Append("    FROM trans_approval WITH(READPAST) ");
                sql.Append(" ");
                sql.Append("    GROUP BY trans_no ");
                sql.Append(") AS ta ON a.trans_no = ta.trans_no ");
                sql.Append("LEFT JOIN( ");
                sql.Append("    SELECT id.reff_order_no, idd.reff_line_no, SUM(idd.qty_received) AS qty_received ");
                sql.Append("    FROM inbound_delivery id WITH(READPAST) ");
                sql.Append("    INNER JOIN inbound_delivery_detail idd WITH(READPAST) ON id.trans_no = idd.trans_no ");
                sql.Append("    GROUP BY id.reff_order_no, idd.reff_line_no ");
                sql.Append(") id ON a.trans_no = id.reff_order_no AND b.line_no = id.reff_line_no ");
                sql.Append("WHERE 1 = 1 ");
                sql.Append("AND a.total_amount <= ISNULL(ta.sum_approval_proxy, 0) ");
                sql.Append("AND a.trans_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`") + "' ");

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
                                    if (dr["managed_item_by"].ToString() == "SERIAL NO")
                                    {
                                        for (int i = 1; i <= double.Parse(dr["qty"].ToString()); i++)
                                        {
                                            addDataToTable(dr["article_no"].ToString(), dr["article_type"].ToString(), dr["article_description"].ToString(),
                                                dr["base_uom"].ToString(), 1, 0, dr["managed_item_by"].ToString(),
                                                "", dr["note_detail"].ToString(), dr["line_no"].ToString(), DateTime.Now, (DataTable)Session["dt"]);
                                        }
                                    }
                                    else
                                    {
                                        addDataToTable(dr["article_no"].ToString(), dr["article_type"].ToString(), dr["article_description"].ToString(),
                                            dr["base_uom"].ToString(), double.Parse(dr["qty"].ToString()), double.Parse(dr["qty"].ToString()), dr["managed_item_by"].ToString(),
                                            "", dr["note_detail"].ToString(), dr["line_no"].ToString(), DateTime.Now, (DataTable)Session["dt"]);
                                    }
                                }
                                loadGrid();
                            }
                        }
                    }
                }
            }
            //else if (tbReffOrderNo.Text.Contains("OD") == true)
            //{
            //    sql.Length = 0;
            //    sql.Append("SELECT DISTINCT a.from_warehouse AS source_id, ISNULL(b.site_name, c.vendor_name) AS source_name, a.ship_to AS dest_id, ISNULL(d.site_name, e.vendor_name) AS dest_name, ");
            //    sql.Append("CONVERT(VARCHAR(11), a.posted_date, 106) AS posted_date, a.note ");
            //    sql.Append("FROM outbound_delivery a WITH(READPAST) ");
            //    sql.Append("LEFT JOIN site b WITH(READPAST) ON a.from_warehouse = b.site_id ");
            //    sql.Append("LEFT JOIN vendor c WITH(READPAST) ON a.from_warehouse = c.vendor_no AND c.manage_warehouse = '1' ");
            //    sql.Append("LEFT JOIN site d WITH(READPAST) ON a.ship_to = d.site_id ");
            //    sql.Append("LEFT JOIN vendor e WITH(READPAST) ON a.ship_to = e.vendor_no AND e.manage_warehouse = '1' ");
            //    sql.Append("INNER JOIN outbound_delivery_detail g WITH(READPAST) ON a.trans_no = g.trans_no ");
            //    sql.Append("LEFT JOIN ( ");
            //    sql.Append("	SELECT id.reff_order_no, idd.reff_line_no, SUM(idd.qty_received) AS qty_received ");
            //    sql.Append("	FROM inbound_delivery id WITH(READPAST) ");
            //    sql.Append("	INNER JOIN inbound_delivery_detail idd WITH(READPAST) ON id.trans_no = idd.trans_no ");
            //    sql.Append("	GROUP BY id.reff_order_no, idd.reff_line_no ");
            //    sql.Append(") id ON a.trans_no = id.reff_order_no AND g.line_no = id.reff_line_no ");
            //    sql.Append("WHERE 1 = 1 ");
            //    sql.Append("AND g.qty_delivered - ISNULL(id.qty_received, 0) > 0 ");
            //    sql.Append("AND a.trans_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`") + "' ");
            //    sql.Append("AND a.status = '1' ");

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
            //                    tbReffOrderNo.ReadOnly = true;
            //                    ibSearchReffOrder.Visible = false;
            //                    lbReffOrderNo.Text = dr["posted_date"].ToString();
            //                    lbReff.Text = "OUTBOUND DELIVERY";
            //                    tbFrom.Text = dr["source_id"].ToString();
            //                    lbFrom.Text = dr["source_name"].ToString();
            //                    tbShipTo.Text = dr["dest_id"].ToString();
            //                    lbShipTo.Text = dr["dest_name"].ToString();
            //                    tbNote.Text = "";
            //                    tbMovementType.Text = "";
            //                    ibSearchMovementType.Visible = true;
            //                    upDetail.Update();
            //                }
            //                else
            //                {
            //                    lbReffOrderNo.Text = "";
            //                    tbReffOrderNo.ReadOnly = false;
            //                    ibSearchReffOrder.Visible = true;
            //                    lbReff.Text = "";
            //                    tbFrom.Text = "";
            //                    lbFrom.Text = "";
            //                    tbShipTo.Text = "";
            //                    lbShipTo.Text = "";
            //                    upDetail.Update();
            //                    master.messageBox("Invalid reff order no!");
            //                }
            //                upHeader.Update();
            //            }
            //        }
            //    }

            //    sql.Length = 0;
            //    sql.Append("SELECT b.line_no, c.article_type, b.item_type, b.article_no, c.article_description, b.unit_id, d.description AS unit_desc, ");
            //    sql.Append("b.qty_delivered - ISNULL(id.qty_received, 0) AS qty, c.managed_item_by, b.serial_no, b.note AS note_detail ");
            //    sql.Append("FROM outbound_delivery a WITH(READPAST) ");
            //    sql.Append("INNER JOIN outbound_delivery_detail b WITH(READPAST) ON a.trans_no = b.trans_no ");
            //    sql.Append("INNER JOIN article c WITH(READPAST) ON b.article_no = c.article_number ");
            //    sql.Append("INNER JOIN inner_pack d WITH(READPAST) ON b.unit_id = d.id ");
            //    sql.Append("INNER JOIN uom u WITH(READPAST) ON d.uom_id = u.id ");
            //    sql.Append("LEFT JOIN ( ");
            //    sql.Append("	SELECT id.reff_order_no, idd.reff_line_no, SUM(idd.qty_received) AS qty_received ");
            //    sql.Append("	FROM inbound_delivery id WITH(READPAST) ");
            //    sql.Append("	INNER JOIN inbound_delivery_detail idd WITH(READPAST) ON id.trans_no = idd.trans_no ");
            //    sql.Append("	GROUP BY id.reff_order_no, idd.reff_line_no ");
            //    sql.Append(") id ON a.trans_no = id.reff_order_no AND b.line_no = id.reff_line_no ");
            //    sql.Append("WHERE 1 = 1 ");
            //    sql.Append("AND a.trans_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`") + "' ");
            //    sql.Append("AND a.status = '1' ");

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
            //                        if (dr["managed_item_by"].ToString() == "SERIAL NO")
            //                        {
            //                            for (int i = 1; i <= double.Parse(dr["qty"].ToString()); i++)
            //                            {
            //                                addDataToTable(dr["article_no"].ToString(), dr["article_type"].ToString(), dr["item_type"].ToString(), dr["article_description"].ToString(),
            //                                    dr["unit_id"].ToString(), dr["unit_desc"].ToString(), 1, 1, dr["managed_item_by"].ToString(),
            //                                    dr["serial_no"].ToString(), dr["note_detail"].ToString(), dr["line_no"].ToString(), DateTime.Now, (DataTable)Session["dt"]);
            //                            }
            //                        }
            //                        else
            //                        {
            //                            addDataToTable(dr["article_no"].ToString(), dr["article_type"].ToString(), dr["item_type"].ToString(), dr["article_description"].ToString(),
            //                                dr["unit_id"].ToString(), dr["unit_desc"].ToString(), double.Parse(dr["qty"].ToString()), double.Parse(dr["qty"].ToString()), dr["managed_item_by"].ToString(),
            //                                "", dr["note_detail"].ToString(), dr["line_no"].ToString(), DateTime.Now, (DataTable)Session["dt"]);
            //                        }
            //                    }
            //                    loadGrid();
            //                }
            //            }
            //        }
            //    }
            //}
            //else if (tbReffOrderNo.Text.Contains("RRO") == true)
            //{
            //    sql.Length = 0;
            //    sql.Append("SELECT pr.trans_no, ISNULL(po.trans_no, '') AS po_trans_no, ISNULL(id.trans_no, '') AS id_trans_no ");
            //    sql.Append("FROM purchase_requisition pr WITH(READPAST) ");
            //    sql.Append("LEFT JOIN purchase_order po WITH(READPAST) ON pr.trans_no = po.purchase_requisition_no ");
            //    sql.Append("LEFT JOIN( ");
            //    sql.Append("	SELECT trans_no, SUM(approval_proxy) AS sum_approval_proxy, MAX(approval_date) AS max_approval_date ");
            //    sql.Append("	FROM trans_approval WITH(READPAST) ");
            //    sql.Append("	GROUP BY trans_no ");
            //    sql.Append(") AS ta ON po.trans_no = ta.trans_no AND po.total_amount <= ISNULL(ta.sum_approval_proxy, 0) ");
            //    sql.Append("LEFT JOIN inbound_delivery id WITH(READPAST) ON po.trans_no = id.reff_order_no AND id.status = '1' ");
            //    sql.Append("AND pr.sales_order_no  = '" + tbReffOrderNo.Text.Trim().Replace("'", "`") + "' ");
            //    sql.Append("AND po.trans_no not in (select trans_no from purchase_order_cancelled union select trans_no from purchase_order_closed) ");

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
            //    sql.Append("SELECT DISTINCT a.customer_site, CONVERT(VARCHAR, a.cut_off_date, 106) AS cut_off_date, ");
            //    sql.Append("a.customer, b.customer_name, a.deliver_to, c.site_name AS deliver_to_name ");
            //    sql.Append("FROM rental_return_order a WITH(READPAST) ");
            //    sql.Append("INNER JOIN customer b WITH(READPAST) ON a.customer = b.customer_no ");
            //    sql.Append("INNER JOIN vw_all_warehouse c WITH(READPAST) ON a.deliver_to = c.site_id ");
            //    sql.Append("INNER JOIN rental_return_order_detail d WITH(READPAST) ON a.trans_no = d.trans_no ");
            //    sql.Append("LEFT JOIN ( ");
            //    sql.Append("	SELECT id.reff_order_no, idd.reff_line_no, SUM(idd.qty_received) AS qty_received ");
            //    sql.Append("	FROM inbound_delivery id WITH(READPAST) ");
            //    sql.Append("	INNER JOIN inbound_delivery_detail idd WITH(READPAST) ON id.trans_no = idd.trans_no ");
            //    sql.Append("	GROUP BY id.reff_order_no, idd.reff_line_no ");
            //    sql.Append(") id ON a.trans_no = id.reff_order_no AND d.line_no = id.reff_line_no ");
            //    sql.Append("WHERE 1 = 1 ");
            //    sql.Append("AND a.status = '1' ");
            //    sql.Append("AND a.closed = '0' ");
            //    sql.Append("AND a.trans_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`") + "' ");
            //    sql.Append("and a.trans_no not in (SELECT trans_no FROM rental_return_order_cancelled WITH(READPAST) UNION SELECT trans_no FROM rental_return_order_closed WITH(READPAST)) ");
            //    sql.Append("AND d.qty - ISNULL(id.qty_received, 0) > 0  ");

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
            //                    lbReffOrderNo.Text = dr["cut_off_date"].ToString();
            //                    lbReff.Text = "RENTAL RETURN ORDER";
            //                    tbReffOrderNo.ReadOnly = true;
            //                    ibSearchReffOrder.Visible = false;
            //                    tbFrom.Text = dr["customer"].ToString();
            //                    lbFrom.Text = dr["customer_name"].ToString();
            //                    tbShipTo.Text = dr["deliver_to"].ToString();
            //                    lbShipTo.Text = dr["deliver_to_name"].ToString();
            //                    tbMovementType.Text = "";
            //                    ibSearchMovementType.Visible = true;
            //                    upDetail.Update();
            //                }
            //                else
            //                {
            //                    lbReffOrderNo.Text = "";
            //                    lbReff.Text = "";
            //                    tbReffOrderNo.ReadOnly = false;
            //                    ibSearchReffOrder.Visible = true;
            //                    tbFrom.Text = "";
            //                    lbFrom.Text = "";
            //                    tbShipTo.Text = "";
            //                    lbShipTo.Text = "";
            //                    master.messageBox("Invalid reff order no!");
            //                }
            //                upHeader.Update();
            //            }
            //        }
            //    }

            //    sql.Length = 0;
            //    sql.Append("SELECT DISTINCT a.article_no, a.article_type, a.item_type, b.article_description, ");
            //    sql.Append("a.unit_id, d.description AS unit_desc, a.qty, a.serial_no, a.line_no, b.managed_item_by ");
            //    sql.Append("FROM rental_return_order_detail a WITH(READPAST) ");
            //    sql.Append("INNER JOIN article b ON a.article_no = b.article_number ");
            //    sql.Append("INNER JOIN inner_pack d WITH(READPAST) ON a.unit_id = d.id ");
            //    sql.Append("INNER JOIN uom u WITH(READPAST) ON d.uom_id = u.id ");
            //    sql.Append("LEFT JOIN ( ");
            //    sql.Append("	SELECT id.reff_order_no, idd.reff_line_no, SUM(idd.qty_received) AS qty_received ");
            //    sql.Append("	FROM inbound_delivery id WITH(READPAST) ");
            //    sql.Append("	INNER JOIN inbound_delivery_detail idd WITH(READPAST) ON id.trans_no = idd.trans_no ");
            //    sql.Append("	GROUP BY id.reff_order_no, idd.reff_line_no ");
            //    sql.Append(") id ON a.trans_no = id.reff_order_no AND a.line_no = id.reff_line_no ");
            //    sql.Append("WHERE a.trans_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`") + "' ");
            //    sql.Append("AND a.qty - ISNULL(id.qty_received, 0) > 0  ");

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
            //                        addDataToTable(dr["article_no"].ToString(), dr["article_type"].ToString(), dr["item_type"].ToString(), dr["article_description"].ToString(),
            //                                dr["unit_id"].ToString(), dr["unit_desc"].ToString(), double.Parse(dr["qty"].ToString()), double.Parse(dr["qty"].ToString()),
            //                                    dr["managed_item_by"].ToString(), dr["serial_no"].ToString(), "", dr["line_no"].ToString(), DateTime.Now, (DataTable)Session["dt"]);
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
        lbReff.Text = "";
        lbReffOrderNo.Text = "";
        tbReffOrderNo.ReadOnly = false;
        ibSearchReffOrder.Visible = true;
        tbFrom.Text = "";
        lbFrom.Text = "";
        tbShipTo.Text = "";
        lbShipTo.Text = "";
        tbNote.Text = "";
        tbMovementType.Text = "";
        lbMovementType.Text = "";
        txtRealReceiveDate.Text = "";
        tbMovementType.ReadOnly = false;
        ibSearchMovementType.Visible = true;
        upHeader.Update();
        grid.DataSource = null;
        grid.DataBind();
        upGrid.Update();
    }

    protected void ibSearchMovementType_Click(object sender, EventArgs e)
    {
        Session["search"] = "id_movement_type";
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
            if (lbReff.Text == "PURCHASE ORDER")
            {
                sql.Append("AND a.id IN ('101') ");
            }
            else if (lbReff.Text == "OUTBOUND DELIVERY")
            {
                sql.Append("AND a.id IN ('305') ");
            }
            else if (lbReff.Text == "RENTAL RETURN ORDER")
            {
                sql.Append("AND a.id IN ('Z02') ");
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
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "unitID";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "qty";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "qty_received";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "managed_item_by";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "serial_no";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "note";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "reffLineNo";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.DateTime");
            dtColumn.ColumnName = "addedDate";
            dt.Columns.Add(dtColumn);

            return dt;
        }
        catch
        {
            return null;
        }
    }

    private void addDataToTable(string articleNo, string articleType, string articleDesc,
        string unitID, double qty, double qty_received, string managed_item_by, string serial_no, string note, string reffLineNo,
        DateTime addedDate, DataTable Table)
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
            dtRow["unitID"] = unitID;
            dtRow["qty"] = qty;
            dtRow["qty_received"] = qty_received;
            dtRow["managed_item_by"] = managed_item_by;
            dtRow["serial_no"] = serial_no;
            dtRow["note"] = note;
            dtRow["reffLineNo"] = reffLineNo;
            dtRow["addedDate"] = addedDate;

            Table.Rows.Add(dtRow);
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridViewRow Row = e.Row;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label tbManagedItemBy = (Label)Row.FindControl("lManagedItemBy");
                TextBox tbSerialNo = (TextBox)Row.FindControl("tbSerialNo");
                ImageButton ibSearchSerialNo = (ImageButton)Row.FindControl("ibSearchSerialNo");

                if (tbManagedItemBy.Text == "SERIAL NO")
                {
                    if (lbReff.Text == "PURCHASE ORDER")
                    {
                        tbSerialNo.Visible = true;
                        ibSearchSerialNo.Visible = false;
                    }
                    else if (lbReff.Text == "OUTBOUND DELIVERY")
                    {
                        tbSerialNo.ReadOnly = true;
                        ibSearchSerialNo.Visible = false;
                    }
                    else if (lbReff.Text == "RENTAL RETURN ORDER")
                    {
                        tbSerialNo.ReadOnly = true;
                        ibSearchSerialNo.Visible = false;
                    }
                    else
                    {
                        tbSerialNo.Visible = true;
                        ibSearchSerialNo.Visible = true;
                    }
                }
                else
                {
                    tbSerialNo.Visible = false;
                    ibSearchSerialNo.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
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

    protected void ibSearchSerialNo_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gridRow = ((sender as ImageButton).Parent.Parent as GridViewRow);
            Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");

            Session["search"] = "article_serial_no";
            Session["searchParamArticleNo"] = lArticleNo.Text;
            Session["searchGridRowIndex"] = gridRow.RowIndex.ToString();
            wucSearch1.loadGrid();
        }
        catch
        {
        }
    }

    protected void clearGrid()
    {
        grid.DataSource = null;
        grid.DataBind();
        upGrid.Update();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;

        string duplicateSerialNo = duplicateSerialNoExists();

        foreach (GridViewRow gridRow in grid.Rows)
        {
            Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
            Label lArticleDesc = (Label)gridRow.FindControl("lArticleDesc");
            Label lManagedItemBy = (Label)gridRow.FindControl("lManagedItemBy");
            TextBox lbSerialNo = (TextBox)gridRow.FindControl("tbSerialNo");
            TextBox tbQtyReceived = (TextBox)gridRow.FindControl("tbQtyReceived");

            if (double.Parse(tbQtyReceived.Text) > 0 && lbSerialNo.Text == "" & lManagedItemBy.Text == "SERIAL NO")
            {
                master.messageBox("Input serial no for article " + lArticleDesc.Text + " (" + lArticleNo.Text + ")!");
                return;
            }
        }
        if (lbReffOrderNo.Text == "")
        {
            master.messageBox("Input reff order!");
            return;
        }
        else if (lbFrom.Text == "")
        {
            master.messageBox("Input From!");
            return;
        }
        else if (lbShipTo.Text == "")
        {
            master.messageBox("Input Ship To!");
            return;
        }
        else if (lbMovementType.Text == "")
        {
            master.messageBox("Input movement type!");
            return;
        }
        else if (txtRealReceiveDate.Text == "")
        {
            master.messageBox("Input Real Receive Date!");
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
        else if (txtDocReceivedate.Text == "")
        {
            master.messageBox("Input Document Receive Date!");
            return;
        }

            Session["ProofOfDeliveryMode"] = "Save";
            showConfirmBox("Save Data?");
    }

    protected void saveData()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (tbTransNo.Text == "")
            {
                conn = new SqlConnection(cAdih.getConnStr("Connection"));
                conn.Open();
                trans = conn.BeginTransaction();

                sql.Length = 0;
                sql.Append("SELECT RIGHT('000000' + CAST(ISNULL(MAX(CAST(RIGHT(trans_no, 6) AS INT)), 0) + 1 AS VARCHAR), 6) AS running_no, ");
                sql.Append("YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) AS tahun, RIGHT('00' + CAST(MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) AS VARCHAR), 2) AS bulan ");
                sql.Append("FROM inbound_delivery ");
                sql.Append("WHERE YEAR(trans_date) = YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) ");
                sql.Append("AND MONTH(trans_date) = MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    using (dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            tbTransNo.Text = "ID" + dr["tahun"].ToString() + dr["bulan"].ToString() + dr["running_no"].ToString();
                            upHeader.Update();
                        }
                    }
                }

                sql.Length = 0;
                sql.Append("INSERT INTO [dbo].[inbound_delivery] ");
                sql.Append("([trans_no] ");
                sql.Append(",[trans_date] ");
                sql.Append(",[reff]                  ");
                sql.Append(",[reff_order_no]         ");
                sql.Append(",[from_site]             ");
                sql.Append(",[ship_to]               ");
                sql.Append(",[movement_type]         ");
                sql.Append(",[note]                  ");
                sql.Append(",[status]                ");
                sql.Append(",[real_receive_date]     ");
                sql.Append(",[created_by]            ");
                sql.Append(",[created_date]          ");
                sql.Append(",[document_receive_date])");
                sql.Append("VALUES ");
                sql.Append("('"+ tbTransNo.Text.Trim().Replace("'", "`").ToUpper() + "',");
                sql.Append("CONVERT(VARCHAR(10), GETDATE(), 120),");
                sql.Append("'" + lbReff.Text + "', ");
                sql.Append("'" + tbReffOrderNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + tbFrom.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + tbShipTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + tbMovementType.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + tbNote.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'0', ");
                sql.Append("CONVERT(DATETIME,'" + txtRealReceiveDate.Text.Trim().Replace("'", "`").ToUpper() + "',103), ");
                sql.Append("'" + Session["username"].ToString() + "', ");
                sql.Append("GETDATE(), ");
                sql.Append("CONVERT(DATETIME,'" + txtDocReceivedate.Text.Trim().Replace("'", "`").ToUpper() + "',103)) ");

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
                    Label lQty = (Label)gridRow.FindControl("lQty");
                    Label lManagedItemBy = (Label)gridRow.FindControl("lManagedItemBy");
                    TextBox tbQtyReceived = (TextBox)gridRow.FindControl("tbQtyReceived");
                    TextBox tbSerialNo = (TextBox)gridRow.FindControl("tbSerialNo");
                    TextBox tbNoteDetail = (TextBox)gridRow.FindControl("tbNoteDetail");
                    Label lReffLineNo = (Label)gridRow.FindControl("lReffLineNo");

                    if (double.Parse(tbQtyReceived.Text) > 0)
                    {
                        double test = double.Parse(lQty.Text);
                        double test1 = double.Parse(tbQtyReceived.Text);

                        sql.Length = 0;
                        sql.Append("INSERT INTO [dbo].[inbound_delivery_detail] ");
                        sql.Append("([trans_no] ");
                        sql.Append(",[line_no] ");
                        sql.Append(",[article_type] ");
                        sql.Append(",[article_no] ");
                        sql.Append(",[qty_ordered] ");
                        sql.Append(",[qty_received] ");
                        sql.Append(",[managed_item_by] ");
                        sql.Append(",[serial_no] ");
                        sql.Append(",[note] ");
                        sql.Append(",[reff_line_no]) ");
                        sql.Append("values ('" + tbTransNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + line_no + "', ");
                        sql.Append("'" + lArticleType.Text + "', ");
                        sql.Append("'" + lArticleNo.Text + "', ");
                        sql.Append("'" + double.Parse(lQty.Text) + "', ");
                        sql.Append("'" + double.Parse(tbQtyReceived.Text) + "', ");
                        sql.Append("'" + lManagedItemBy.Text + "', ");
                        sql.Append("'" + tbSerialNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + tbNoteDetail.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + lReffLineNo.Text + "') ");

                        using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                        {
                            cmd.ExecuteNonQuery();
                            cmd = null;
                        }
                    }

                    line_no += 10;
                }

                line_no = 10;

                trans.Commit();
                conn.Close();
                master.messageBox("Data has been saved. Trans No: " + tbTransNo.Text + "");
            }
            //EDIT
            else if (tbTransNo.Text != "")
            {
                conn = new SqlConnection(cAdih.getConnStr("Connection"));
                conn.Open();
                trans = conn.BeginTransaction();

                sql.Length = 0;
                sql.Append("UPDATE inbound_delivery SET reff = '" + lbReff.Text + "', ");
                sql.Append("reff_order_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("from_site = '" + tbFrom.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("ship_to = '" + tbShipTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("movement_type = '" + tbMovementType.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("note = '" + tbNote.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("real_receive_date = CONVERT(DATETIME,'" + txtRealReceiveDate.Text.Trim().Replace("'", "`").ToUpper() + "',103), ");
                sql.Append("document_receive_date =  CONVERT(DATETIME,'" + txtDocReceivedate.Text.Trim().Replace("'", "`").ToUpper() + "',103), ");
                sql.Append("last_modified_by = '" + Session["username"].ToString() + "', ");
                sql.Append("last_modified_date = GETDATE() ");
                sql.Append("WHERE trans_no = '" + tbTransNo.Text + "' ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                sql.Length = 0;
                sql.Append("DELETE inbound_delivery_detail WHERE trans_no = '" + tbTransNo.Text + "' ");
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
                    Label lQty = (Label)gridRow.FindControl("lQty");
                    Label lManagedItemBy = (Label)gridRow.FindControl("lManagedItemBy");
                    TextBox tbQtyReceived = (TextBox)gridRow.FindControl("tbQtyReceived");
                    TextBox tbSerialNo = (TextBox)gridRow.FindControl("tbSerialNo");
                    TextBox tbNoteDetail = (TextBox)gridRow.FindControl("tbNoteDetail");
                    Label lReffLineNo = (Label)gridRow.FindControl("lReffLineNo");

                    if (double.Parse(tbQtyReceived.Text) > 0)
                    {
                        double test = double.Parse(lQty.Text);
                        double test1 = double.Parse(tbQtyReceived.Text);

                        sql.Length = 0;
                        sql.Append("INSERT INTO [dbo].[inbound_delivery_detail] ");
                        sql.Append("([trans_no] ");
                        sql.Append(",[line_no] ");
                        sql.Append(",[article_type] ");
                        sql.Append(",[article_no] ");
                        sql.Append(",[qty_ordered] ");
                        sql.Append(",[qty_received] ");
                        sql.Append(",[managed_item_by] ");
                        sql.Append(",[serial_no] ");
                        sql.Append(",[note] ");
                        sql.Append(",[reff_line_no]) ");
                        sql.Append("values ('" + tbTransNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + line_no + "', ");
                        sql.Append("'" + lArticleType.Text + "', ");
                        sql.Append("'" + lArticleNo.Text + "', ");
                        sql.Append("'" + double.Parse(lQty.Text) + "', ");
                        sql.Append("'" + double.Parse(tbQtyReceived.Text) + "', ");
                        sql.Append("'" + lManagedItemBy.Text + "', ");
                        sql.Append("'" + tbSerialNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + tbNoteDetail.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + lReffLineNo.Text + "') ");

                        using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                        {
                            cmd.ExecuteNonQuery();
                            cmd = null;
                        }
                    }

                    line_no += 10;
                }

                trans.Commit();
                conn.Close();
                master.messageBox("Data has been updated.");
            }
            lStatus.Text = "OPEN";
            statusChanged();
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
            trans.Rollback();
            conn.Close();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transaction/InboundDelivery.aspx");
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        try
        {
            if (wucConfirmBox1.StrMode == "Yes")
            {
                if (Session["ProofOfDeliveryMode"].ToString() == "Save")
                {
                    saveData();
                }
                else if (Session["ProofOfDeliveryMode"].ToString() == "Posting")
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

    protected void wucSearch_Hide(object sender, EventArgs e)
    {
        if (Session["search"].ToString() == "id_reff")
        {
            tbReffOrderNo.Text = wucSearch1.result;
            bSearchReffOrder_Click(sender, e);
            upHeader.Update();
            Session["searchParamCompanyID"] = null;
            Session["searchParamSiteID"] = null;
        }
        else if (Session["search"].ToString() == "id_movement_type")
        {
            tbMovementType.Text = wucSearch1.result;
            bSearchMovementType_Click(sender, e);
            upHeader.Update();
            Session["searchParamReffType"] = null;
        }
        else if (Session["search"].ToString() == "article_serial_no")
        {
            TextBox tbSerialNo = (TextBox)grid.Rows[int.Parse(Session["searchGridRowIndex"].ToString())].FindControl("tbSerialNo");
            tbSerialNo.Text = wucSearch1.result;
            upGrid.Update();
            Session["searchParamCompanyID"] = null;
            Session["searchParamSiteID"] = null;
            Session["searchParamArticleNo"] = null;
        }
        Session["search"] = null;
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
        else if (lbFrom.Text == "")
        {
            master.messageBox("Input From Warehouse!");
            return;
        }
        else if (lbShipTo.Text == "")
        {
            master.messageBox("Input Ship To!");
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
            //belum ada validasi untuk serial no
            Session["ProofOfDeliveryMode"] = "Posting";
            showConfirmBox("Posting Data?");
        }
    }

    protected void postingData()
    {
        MasterPage master = (MasterPage)this.Master;

        try
        {
            conn = new SqlConnection(cAdih.getConnStr("Connection"));
            conn.Open();
            trans = conn.BeginTransaction();

            if (tbMovementType.Text == "101")
            {

                sql.Length = 0;
                sql.Append("EXEC sp_inbound_delivery_post '"+tbTransNo.Text+"','"+Session["username"].ToString()+"','"+ DateTime.Now +"','"+ cAdih.getIPAddress() + "' ");

                sql.Append(" ");
                sql.Append("UPDATE inbound_delivery SET status = '1', ");
                sql.Append("posted_by = '" + Session["username"].ToString() + "', ");
                sql.Append("posted_date = GETDATE() ");
                sql.Append("WHERE trans_no = '" + tbTransNo.Text + "' ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }
            }
            else if (tbMovementType.Text == "305")
            {
                //_dal.ExecRawSP("sp_inbound_delivery_post", _ht);

                sql.Length = 0;
                sql.Append("UPDATE inbound_delivery SET status = '1', ");
                sql.Append("posted_by = '" + Session["username"].ToString() + "', ");
                sql.Append("posted_date = GETDATE() ");
                sql.Append("WHERE trans_no = '" + tbTransNo.Text + "' ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }
            }
            else if (tbMovementType.Text == "Z02")
            {
                //_dal.ExecRawSP("sp_inbound_delivery_post_from_rro", _ht);

                sql.Length = 0;
                sql.Append("UPDATE inbound_delivery SET status = '1', ");
                sql.Append("posted_by = '" + Session["username"].ToString() + "', ");
                sql.Append("posted_date = GETDATE() ");
                sql.Append("WHERE trans_no = '" + tbTransNo.Text + "' ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }
            }

            trans.Commit();
            conn.Close();

            master.messageBox("Data has been posted.");
            lStatus.Text = "POSTED";
            statusChanged();
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message + "<br />" + ex.InnerException);
            trans1.Rollback();
            conn1.Close();
        }
    }

    protected string duplicateSerialNoExists()
    {
        MasterPage master = (MasterPage)this.Master;
        foreach (GridViewRow gridRow in grid.Rows)
        {
            Label lManagedItemBy = (Label)gridRow.FindControl("lManagedItemBy");
            Label lItemType = (Label)gridRow.FindControl("lItemType");
            TextBox tbQty = (TextBox)gridRow.FindControl("tbQty");
            TextBox tbSerialNo = (TextBox)gridRow.FindControl("tbSerialNo");

            if ((lManagedItemBy.Text == "SERIAL NO"))
            {
                int countSerialNo = 0;
                foreach (GridViewRow gridRowB in grid.Rows)
                {
                    TextBox tbSerialNoB = (TextBox)gridRowB.FindControl("tbSerialNo");
                    if (tbSerialNo.Text.Trim().Replace("'", "`") == tbSerialNoB.Text.Trim().Replace("'", "`"))
                    {
                        countSerialNo += 1;
                    }
                }
                if (countSerialNo > 1)
                {
                    return tbSerialNo.Text;
                }
            }
        }
        return "";
    }

    protected void lArticleDesc_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
        Label lArticleType = (Label)gridRow.FindControl("lArticleType");
        Label lUnitID = (Label)gridRow.FindControl("lUnitID");
        Label lUnitDesc = (Label)gridRow.FindControl("lUnitDesc");
        LinkButton lArticleDesc = (LinkButton)gridRow.FindControl("lArticleDesc");
        ImageButton ibSearchArticleGrid = (ImageButton)gridRow.FindControl("ibSearchArticleGrid");

        lArticleNo.Text = "";
        lArticleType.Text = "";
        lArticleDesc.Text = "";
        lUnitID.Text = "";
        lUnitDesc.Text = "";
        ibSearchArticleGrid.Visible = true;
    }
}