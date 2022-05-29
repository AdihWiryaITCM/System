using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class wuc_wucSearch : System.Web.UI.UserControl
{
    public event EventHandler Hide;

    private StringBuilder sql = new StringBuilder();
    private ClassAdih cAdih = new ClassAdih();

    private string strResult = "";
    private string strResult1 = "";

    protected void imgSearch_Click(Object sender, EventArgs e)
    {
        realLoadGrid();
    }

    protected void realLoadGrid()
    {
        try
        {
            grid.Columns[0].Visible = false;
            grid.Columns[1].Visible = false;
            grid.Columns[2].Visible = false;
            grid.Columns[3].Visible = false;

            sql.Length = 0;
            if (Session["search"].ToString() == "SearchPR")
            {
                sql.Append("SELECT trans_no as col0 ,format(trans_date,'dd MMM yyyy') as col1,pr.note as col2,sw.wh_description as col3 ");
                sql.Append("FROM purchase_requisition pr ");
                sql.Append("INNER JOIN site_wh sw on sw.wh_id = pr.ship_to ");
                sql.Append("WHERE status = 'POSTED' ");
                sql.Append("AND ( trans_no like '%" + txtSearch.Text + "%' ");
                sql.Append("OR sw.wh_description like '%" + txtSearch.Text + "%'  ");
                sql.Append("OR pr.note like '%" + txtSearch.Text + "%' ) ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[2].Visible = true;
                grid.Columns[3].Visible = true;
                grid.Columns[0].HeaderText = "Trans No";
                grid.Columns[1].HeaderText = "Trans Date";
                grid.Columns[2].HeaderText = "Note";
                grid.Columns[3].HeaderText = "Ship To";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            }
            else if (Session["search"].ToString() == "article_type")
            {
                sql.Append("SELECT TOP 100 id AS col0, description AS col1, '' AS col2, '' AS col3 ");
                sql.Append("FROM article_type ");
                sql.Append("WHERE 1 = 1 ");
                sql.Append("AND (id LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR description LIKE '%" + txtSearch.Text + "%') ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[0].HeaderText = "ID";
                grid.Columns[1].HeaderText = "Description";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            }
            else if (Session["search"].ToString() == "uom")
            {
                sql.Append("SELECT TOP 100 id AS col0, description AS col1, '' AS col2, '' AS col3 ");
                sql.Append("FROM uom ");
                sql.Append("WHERE 1 = 1 ");
                sql.Append("AND (id LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR description LIKE '%" + txtSearch.Text + "%') ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[0].HeaderText = "ID";
                grid.Columns[1].HeaderText = "Description";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            }
            else if (Session["search"].ToString() == "article_serial_no")
            {
                sql.Append("SELECT TOP 100 a.serial_no AS col0, '' AS col1, '' AS col2, '' AS col3 ");
                sql.Append("FROM article_serial_no a ");
                sql.Append("WHERE 1 = 1 ");
                sql.Append("AND a.active = '1' ");
                sql.Append("AND article_no = '" + Session["searchParamArticleNo"].ToString() + "' ");
                sql.Append("AND a.serial_no not in (select serial_no from outbound_delivery_detail odd with(readpast) ");
                sql.Append("                        inner join outbound_delivery od with(readpast) on odd.trans_no = od.trans_no ");
                sql.Append("                        where odd.article_no = '" + Session["searchParamArticleNo"].ToString() + "' ");
                sql.Append("                        and od.status = 0 ) ");
                if (txtSearch.Text.Replace("'", "`").Trim() != "")
                {
                    sql.Append("AND a.serial_no LIKE '%" + txtSearch.Text + "%' ");
                }
                sql.Append("ORDER BY a.serial_no ASC ");

                grid.Columns[0].Visible = true;
                grid.Columns[0].HeaderText = "Serial No";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            }
            else if (Session["search"].ToString() == "vendor")
            {
                sql.Append("SELECT vendor_no as col0 ,vendor_name as col1,'' as col2,'' as col3 ");
                sql.Append("FROM vendor ");
                sql.Append("WHERE vendor_no like '%" + txtSearch.Text + "%' ");
                sql.Append("OR vendor_name like '%" + txtSearch.Text + "%' ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[2].Visible = false;
                grid.Columns[3].Visible = false;
                grid.Columns[0].HeaderText = "Vendor No";
                grid.Columns[1].HeaderText = "Vendor Name";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            }
            else if (Session["search"].ToString() == "id_reff")
            {
                sql.Append("EXEC sp_search_id_reff '" + txtSearch.Text + "' ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[2].Visible = true;
                grid.Columns[3].Visible = true;
                grid.Columns[0].HeaderText = "Trans No";
                grid.Columns[1].HeaderText = "Vendor Name";
                grid.Columns[1].HeaderText = "Note";
                grid.Columns[1].HeaderText = "Reff";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            }
            else if (Session["search"].ToString() == "od_movement_type")
            {
                sql.Append("SELECT TOP 100 a.id AS col0, a.description AS col1, '' AS col2, '' AS col3 ");
                sql.Append("FROM movement_type a ");
                sql.Append("WHERE 1 = 1 ");
                if (Session["searchParamReffType"].ToString() == "RENTAL RETURN ORDER")
                {
                    sql.Append("AND a.id IN ('Z03') ");
                }
                else if (Session["searchParamReffType"].ToString() == "RENTAL ORDER")
                {
                    sql.Append("AND a.id IN ('Z01') ");
                }
                else if (Session["searchParamReffType"].ToString() == "STOCK TRANSPORT ORDER")
                {
                    sql.Append("AND a.id IN ('301', '303') ");
                }
                else if (Session["searchParamReffType"].ToString() == "SALES ORDER")
                {
                    sql.Append("AND a.id IN ('601') ");
                }
                if (txtSearch.Text.Replace("'", "`").Trim() != "")
                {
                    sql.Append("AND (a.id LIKE '%" + txtSearch.Text + "%' OR a.description LIKE '%" + txtSearch.Text + "%') ");
                }
                sql.Append("ORDER BY a.id ASC ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[0].HeaderText = "ID";
                grid.Columns[1].HeaderText = "Description";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            }
            else if (Session["search"].ToString() == "od_reff")
            {
                sql.Append("EXEC sp_search_od_reff '" + txtSearch.Text + "' ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[2].Visible = true;
                grid.Columns[3].Visible = true;
                grid.Columns[0].HeaderText = "Trans No";
                grid.Columns[1].HeaderText = "Vendor Name";
                grid.Columns[1].HeaderText = "Note";
                grid.Columns[1].HeaderText = "Reff";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            }
            else if (Session["search"].ToString() == "id_movement_type")
            {
                sql.Append("SELECT TOP 100 a.id AS col0, a.description AS col1, '' AS col2, '' AS col3 ");
                sql.Append("FROM movement_type a ");
                sql.Append("WHERE 1 = 1 ");
                if (Session["searchParamReffType"].ToString() == "PURCHASE ORDER")
                {
                    sql.Append("AND a.id IN ('101') ");
                }
                else if (Session["searchParamReffType"].ToString() == "OUTBOUND DELIVERY")
                {
                    sql.Append("AND a.id IN ('305') ");
                }
                else if (Session["searchParamReffType"].ToString() == "RENTAL RETURN ORDER")
                {
                    sql.Append("AND a.id IN ('Z02') ");
                }
                if (txtSearch.Text.Replace("'", "`").Trim() != "")
                {
                    sql.Append("AND (a.id LIKE '%" + txtSearch.Text + "%' OR a.description LIKE '%" + txtSearch.Text + "%') ");
                }
                sql.Append("ORDER BY a.id ASC ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[0].HeaderText = "ID";
                grid.Columns[1].HeaderText = "Description";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            }
            else if (Session["search"].ToString() == "customer")
            {
                sql.Append("SELECT TOP 100 a.customer_no AS col0, a.customer_name AS col1, a.vat_reg_no AS col2, a.street_address AS col3 ");
                sql.Append("FROM (	select customer_no, customer_name, vat_reg_no, street_address ");
                sql.Append("		from customer ");
                sql.Append("	 )a ");
                sql.Append("WHERE (a.customer_no LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR a.customer_name LIKE '%" + txtSearch.Text + "%') ");
                sql.Append("ORDER BY col1 ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[2].Visible = true;
                grid.Columns[3].Visible = true;
                grid.Columns[0].HeaderText = "Customer No";
                grid.Columns[1].HeaderText = "Customer Name";
                grid.Columns[2].HeaderText = "NPWP No";
                grid.Columns[3].HeaderText = "Address";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            }
            else if (Session["search"].ToString() == "customer_bill_to")
            {
                sql.Append("SELECT TOP 100 a.id AS col0, a.street_address AS col1, ");
                sql.Append("(CASE WHEN a.alias_name = '' THEN a.alias_name_full ELSE a.alias_name_full + ' ' + a.alias_name END) AS col2, '' AS col3    ");
                sql.Append("FROM customer_address a WITH(READPAST) ");
                sql.Append("WHERE 1 = 1 ");
                sql.Append("AND a.address_type = '01' ");
                sql.Append("AND a.customer_no = '" + Session["searchParamCustomerNo"].ToString() + "' ");
                sql.Append("AND (a.id LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR a.street_address LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR a.alias_name LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR a.alias_name_full LIKE '%" + txtSearch.Text + "%') ");
                sql.Append("ORDER BY col0 ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[2].Visible = true;
                grid.Columns[0].HeaderText = "Address ID";
                grid.Columns[1].HeaderText = "Street Address";
                grid.Columns[2].HeaderText = "Alias Name";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            }
            else if (Session["search"].ToString() == "customer_ship_to")
            {
                sql.Append("SELECT TOP 100 a.id AS col0, a.street_address AS col1, ");
                sql.Append("(CASE WHEN a.alias_name = '' THEN a.alias_name_full ELSE a.alias_name_full + ' ' + a.alias_name END) AS col2, '' AS col3    ");
                sql.Append("FROM customer_address a WITH(READPAST) ");
                sql.Append("WHERE 1 = 1 ");
                sql.Append("AND a.address_type = '02' ");
                sql.Append("AND a.customer_no = '" + Session["searchParamCustomerNo"].ToString() + "' ");
                sql.Append("AND (a.id LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR a.street_address LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR a.alias_name LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR a.alias_name_full LIKE '%" + txtSearch.Text + "%') ");
                sql.Append("ORDER BY col0 ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[2].Visible = true;
                grid.Columns[0].HeaderText = "Address ID";
                grid.Columns[1].HeaderText = "Street Address";
                grid.Columns[2].HeaderText = "Alias Name";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            }
            else if (Session["search"].ToString() == "payment_term")
            {
                sql.Append("SELECT TOP 100 a.id AS col0, a.description AS col1, '' AS col2, '' AS col3 ");
                sql.Append("FROM payment_term a ");
                sql.Append("WHERE 1 = 1 ");
                if (txtSearch.Text.Replace("'", "`").Trim() != "")
                {
                    sql.Append("AND (a.id LIKE '%" + txtSearch.Text + "%' ");
                    sql.Append("OR a.description LIKE '%" + txtSearch.Text + "%') ");
                }
                sql.Append("ORDER BY a.days ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[0].HeaderText = "ID";
                grid.Columns[1].HeaderText = "Description";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            }
            else if (Session["search"].ToString() == "rental_order_article")
            {
                sql.Append("SELECT TOP 100 article_no AS col0, article_description AS col1, article_type AS col2, base_uom AS col3 ");
                sql.Append("FROM article WITH(READPAST) ");
                sql.Append("WHERE 1 = 1 ");
                sql.Append("AND article_type IN ('ZMNR', 'ZREN') ");
                if (txtSearch.Text.Replace("'", "`").Trim() != "")
                {
                    sql.Append("AND (article_number LIKE '%" + txtSearch.Text + "%' ");
                    sql.Append("OR article_description LIKE '%" + txtSearch.Text + "%' ");
                    sql.Append("OR article_type LIKE '%" + txtSearch.Text + "%' ");
                    sql.Append("OR base_uom LIKE '%" + txtSearch.Text + "%') ");
                }

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[2].Visible = true;
                grid.Columns[3].Visible = true;
                grid.Columns[0].HeaderText = "ID";
                grid.Columns[1].HeaderText = "Description";
                grid.Columns[2].HeaderText = "Type";
                grid.Columns[3].HeaderText = "Base UOM";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            }
            else if (Session["search"].ToString() == "article")
            {
                sql.Append("SELECT TOP 100 article_no AS col0, article_description AS col1, article_type AS col2, base_uom AS col3 ");
                sql.Append("FROM article WITH(READPAST) ");
                sql.Append("WHERE 1 = 1 ");
                if (txtSearch.Text.Replace("'", "`").Trim() != "")
                {
                    sql.Append("AND (article_number LIKE '%" + txtSearch.Text + "%' ");
                    sql.Append("OR article_description LIKE '%" + txtSearch.Text + "%' ");
                    sql.Append("OR article_type LIKE '%" + txtSearch.Text + "%' ");
                    sql.Append("OR base_uom LIKE '%" + txtSearch.Text + "%') ");
                }

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[2].Visible = true;
                grid.Columns[3].Visible = true;
                grid.Columns[0].HeaderText = "ID";
                grid.Columns[1].HeaderText = "Description";
                grid.Columns[2].HeaderText = "Type";
                grid.Columns[3].HeaderText = "Base UOM";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            }
            else if (Session["search"].ToString() == "article_time_unit")
            {
                sql.Append("SELECT TOP 100 time_unit AS col0, REPLACE(CONVERT(VARCHAR,CONVERT(MONEY, sales_price), 1), '.00','') AS col1, '' AS col2, '' AS col3 ");
                sql.Append("FROM article_rental_price arp ");
                sql.Append("inner join  customer_address ca on (arp.zone_id = ca.price_zone) ");
                sql.Append("WHERE 1 = 1 ");
                sql.Append("AND article_no = '" + Session["searchParamArticleNo"].ToString() + "' ");
                sql.Append("AND currency = '" + Session["searchParamCurrency"].ToString() + "' ");
                sql.Append("and	ca.id = '" + Session["searchStoreID"].ToString() + "' ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[0].HeaderText = "Time Unit";
                grid.Columns[1].HeaderText = "Description";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            }
            else if (Session["search"].ToString() == "quot_rent")
            {
                sql.Append("SELECT DISTINCT TOP 100 qr.trans_no AS col0, CONVERT(VARCHAR, qr.posted_date, 106) AS col1, c.customer_name AS col2,  ");
                sql.Append("(CASE WHEN cas.alias_name = '' THEN cas.alias_name_full ELSE cas.alias_name_full + ' ' + cas.alias_name END) + '<br />' + cas.street_address + ' ' + cas.city  AS col3, qr.posted_date ");
                sql.Append("FROM quotation_rental qr WITH(READPAST) ");
                sql.Append("INNER JOIN quotation_rental_detail qrd WITH(READPAST) ON qr.trans_no = qrd.trans_no ");
                sql.Append("INNER JOIN customer c WITH(READPAST) ON qr.sold_to = c.customer_no ");
                sql.Append("INNER JOIN customer_address cab WITH(READPAST) ON qr.sold_to = cab.customer_no AND cab.address_type = '01' AND qr.bill_to = cab.id ");
                sql.Append("INNER JOIN customer_address cas WITH(READPAST) ON qr.sold_to = cas.customer_no AND cas.address_type = '02' AND qr.ship_to = cas.id ");
                sql.Append("LEFT JOIN( ");
                sql.Append("    SELECT ro.quot_trans_no, rod.reff_line_no, rod.article_no, SUM(rod.qty) AS qty ");
                sql.Append("    FROM rental_order ro WITH(READPAST) ");
                sql.Append("    INNER JOIN rental_order_detail rod WITH(READPAST) ON ro.trans_no = rod.trans_no ");
                sql.Append("    GROUP BY ro.quot_trans_no, rod.reff_line_no, rod.article_no ");
                sql.Append(") AS ro ON qrd.trans_no = ro.quot_trans_no AND qrd.line_no = ro.reff_line_no ");
                sql.Append("WHERE 1 = 1 ");
                sql.Append("AND qr.status = '1' ");
                sql.Append("AND qrd.qty - ISNULL(ro.qty, 0) > 0 ");
                if (txtSearch.Text.Trim().Length > 0)
                {
                    sql.Append("AND (c.customer_name  LIKE '%" + txtSearch.Text + "%' OR cas.alias_name_full LIKE '%" + txtSearch.Text + "%' OR qr.trans_no LIKE '%" + txtSearch.Text + "%') ");
                }
                sql.Append("ORDER BY qr.posted_date DESC ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[2].Visible = true;
                grid.Columns[3].Visible = true;

                grid.Columns[0].HeaderText = "Trans No";
                grid.Columns[1].HeaderText = "Posted Date";
                grid.Columns[2].HeaderText = "Customer";
                grid.Columns[3].HeaderText = "Ship To";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            }
            else if (Session["search"].ToString() == "source_ro")
            {
                sql.Append("SELECT TOP 100 id AS col0, description AS col1, '' AS col2, '' AS col3 ");
                sql.Append("FROM source WITH(READPAST) ");
                sql.Append("WHERE 1 = 1 ");
                sql.Append("AND source_type = '" + Session["searchParamSourceType"].ToString() + "' ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[0].HeaderText = "ID";
                grid.Columns[1].HeaderText = "Description";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            }
            else if (Session["search"].ToString() == "all_warehouse")
            {
                sql.Append("EXEC sp_search_all_warehouse '" + txtSearch.Text.Trim().Replace("'", "`") + "' ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[2].Visible = true;
                grid.Columns[0].HeaderText = "Site ID";
                grid.Columns[1].HeaderText = "Site Name";
                grid.Columns[2].HeaderText = "Type";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            }
            else if (Session["search"].ToString() == "pi_purchase_order_no")
            {
                sql.Append("SELECT DISTINCT TOP 100 id.reff_order_no AS col0, CONVERT(VARCHAR, po.trans_date, 106) AS col1, v.vendor_name + '<br />' + isnull(s.site_name, '') AS col2, po.note AS col3, po.trans_date ");
                sql.Append("FROM inbound_delivery id WITH(READPAST) ");
                sql.Append("INNER JOIN purchase_order po WITH(READPAST) ON id.reff_order_no = po.trans_no ");
                sql.Append("INNER JOIN vendor v WITH(READPAST) ON po.vendor_id = v.vendor_no ");
                sql.Append("left join( ");
                sql.Append("    select  wh_id as site_id, wh_description as site_name, '' 'ship_to_id' ");
                sql.Append("    from site_wh WITH(READPAST) ");
                sql.Append("        union ");
                sql.Append("    select vendor_no, vendor_name, '' ");
                sql.Append("    from vendor WITH(READPAST) ");
                sql.Append(") as s ON(po.ship_to = s.ship_to_id) ");
                sql.Append("WHERE 1 = 1 ");
                sql.Append("AND id.status = '1' AND id.reff = 'PURCHASE ORDER' ");
                sql.Append("AND id.trans_no NOT IN(SELECT reff_order_no FROM invoice_confirmation_receipt WITH(READPAST) WHERE status = 1) ");
                if (txtSearch.Text.Replace("'", "`").Trim() != "")
                {
                    sql.Append("AND (id.reff_order_no LIKE '%" + txtSearch.Text + "%' ");
                    sql.Append("OR v.vendor_name LIKE '%" + txtSearch.Text + "%' OR po.note LIKE '%" + txtSearch.Text + "%') ");
                }
                sql.Append("ORDER BY po.trans_date DESC ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[2].Visible = true;
                grid.Columns[3].Visible = true;
                grid.Columns[0].HeaderText = "PO Trans No";
                grid.Columns[1].HeaderText = "PO Trans Date";
                grid.Columns[2].HeaderText = "Vendor / Ship To";
                grid.Columns[3].HeaderText = "Note";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            }
            else if (Session["search"].ToString() == "pi_inbound_delivery_no")
            {
                sql.Append("SELECT id.trans_no AS col0, CONVERT(VARCHAR, id.posted_date, 106) AS col1, id.note AS col2, '' AS col3, id.posted_date ");
                sql.Append("FROM inbound_delivery id WITH(READPAST) ");
                sql.Append("WHERE id.reff_order_no = '" + Session["searchParamPOTransNo"].ToString() + "' ");
                sql.Append("AND id.status = '1' ");
                sql.Append("AND id.trans_no NOT IN (");
                sql.Append("    SELECT iccd.inbound_delivery_no FROM invoice_confirmation_receipt icc WITH(READPAST) ");
                sql.Append("    INNER JOIN invoice_confirmation_receipt_detail iccd ON icc.trans_no = iccd.trans_no ");
                sql.Append("    WHERE icc.status = 1) ");
                if (txtSearch.Text.Replace("'", "`").Trim() != "")
                {
                    sql.Append("AND (id.trans_no LIKE '%" + txtSearch.Text + "%' OR id.note LIKE '%" + txtSearch.Text + "%') ");
                }
                sql.Append("ORDER BY id.posted_date ASC ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[2].Visible = true;
                grid.Columns[3].Visible = false;
                grid.Columns[0].HeaderText = "ID Trans No";
                grid.Columns[1].HeaderText = "ID Posted Date";
                grid.Columns[2].HeaderText = "Note";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            }
            else if (Session["search"].ToString() == "si_sales_no")
            {
                sql.Append("EXEC [dbo].[sp_sales_invoice_for_lookup_po_no]  '" + txtSearch.Text + "' ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[2].Visible = true;
                grid.Columns[3].Visible = true;
                grid.Columns[0].HeaderText = "Trans No";
                grid.Columns[1].HeaderText = "Posted Date";
                grid.Columns[2].HeaderText = "PO NO";
                grid.Columns[3].HeaderText = "Reff No";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            }
            else if (Session["search"].ToString() == "si_lookup_reff_no")
            {
                sql.Append("EXEC [dbo].[sp_sales_invoice_for_lookup_reff_no]  '" + txtSearch.Text + "','" + Session["reff_no"].ToString() + "','" + Session["reff_type"].ToString() + "' ");

                grid.Columns[0].Visible = true;
                grid.Columns[1].Visible = true;
                grid.Columns[2].Visible = true;
                grid.Columns[3].Visible = true;
                grid.Columns[0].HeaderText = "Trans No";
                grid.Columns[1].HeaderText = "Reff No";
                grid.Columns[2].HeaderText = "Posted Date";
                grid.Columns[3].HeaderText = "Type";
                grid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            }

            cAdih.loadGridView(sql.ToString(), cAdih.getConnStr("Connection"), grid);

            upSearchLink.Update();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mpeSearch", "$('#mpeSearch').modal();", true);
        }
        catch
        {
        }
    }

    public void loadGrid()
    {
        txtSearch.Text = "";
        realLoadGrid();
    }

    protected void link_Click(Object sender, EventArgs e)
    {
        GridViewRow gridRow = (sender as LinkButton).Parent.Parent as GridViewRow;
        LinkButton link0 = (LinkButton)gridRow.FindControl("link0");
        result = link0.Text;
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "mpeSearch", "$('#mpeSearch').modal('hide');", true);
        if (Hide != null)
        {
            Hide(this, e);
        }
    }

    public string result
    {
        get
        {
            return strResult;
        }
        set
        {
            strResult = value;
        }
    }

    public string result1
    {
        get
        {
            return strResult1;
        }
        set
        {
            strResult1 = value;
        }
    }

    protected void OnHide(EventArgs e)
    {
        if (Hide != null)
        {
            Hide(this, e);
        }
    }
}