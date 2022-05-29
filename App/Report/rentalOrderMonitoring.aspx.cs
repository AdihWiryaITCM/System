using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Report_rentalOrderMonitoring : Page
{
    private StringBuilder sql = new StringBuilder();
    private StringBuilder sql1 = new StringBuilder();
    private ClassAdih cAdih = new ClassAdih();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void ibSearchSoldTo_Click(object sender, EventArgs e)
    {
        Session["search"] = "customer";
        wucSearch1.loadGrid();
    }

    protected void ibSearchBillTo_Click(object sender, EventArgs e)
    {
        if (tbSoldTo.Text.Trim() == "")
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox("Input rent to!");
            return;
        }
        else
        {
            Session["searchParamCustomerNo"] = tbSoldTo.Text.Trim();
            Session["search"] = "customer_bill_to";
            wucSearch1.loadGrid();
        }
    }

    protected void ibSearchShipTo_Click(object sender, EventArgs e)
    {
        if (tbSoldTo.Text.Trim() == "")
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox("Input rent to!");
            return;
        }
        else
        {
            Session["searchParamCustomerNo"] = tbSoldTo.Text.Trim();
            Session["search"] = "customer_ship_to";
            wucSearch1.loadGrid();
        }
    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridViewRow Row = e.Row;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lStatus = (Label)Row.FindControl("lStatus");
                Label lPostedDate = (Label)Row.FindControl("lPostedDate");
                Label lODRealDelvDate = (Label)Row.FindControl("lPODRealDelvDate");
                Label lODQty = (Label)Row.FindControl("lODQty");

                if (lStatus.Text == "True")
                {
                    lPostedDate.Visible = true;
                }
                else
                {
                    lPostedDate.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void bShow_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (cbDetail.Checked)
            {
                queryLoadGridDetail();
                grid.Columns[10].Visible = true;
                grid.Columns[11].Visible = true;
                grid.Columns[12].Visible = true;
                grid.Columns[13].Visible = true;
                grid.Columns[14].Visible = true;
                grid.Columns[15].Visible = true;
                grid.Columns[16].Visible = true;
                grid.Columns[17].Visible = true;
                grid.Columns[18].Visible = true;
                grid.Columns[19].Visible = true;
            }                
            else
            {
                queryLoadGrid();
                grid.Columns[10].Visible = false;
                grid.Columns[11].Visible = false;
                grid.Columns[12].Visible = false;
                grid.Columns[13].Visible = false;
                grid.Columns[14].Visible = false;
                grid.Columns[15].Visible = false;
                grid.Columns[16].Visible = false;
                grid.Columns[17].Visible = false;
                grid.Columns[18].Visible = false;
                grid.Columns[19].Visible = false;
            }
            cAdih.loadGridView(sql.ToString(), cAdih.getConnStr("Connection"), grid);
            upGrid.Update();
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void wucSearch_Hide(object sender, EventArgs e)
    {
        if (Session["search"].ToString() == "customer")
        {
            tbSoldTo.Text = wucSearch1.result;
            upHeader.Update();
        }
        else if (Session["search"].ToString() == "customer_bill_to")
        {
            tbBillTo.Text = wucSearch1.result;
            upHeader.Update();
            Session["searchParamCustomerNo"] = null;
        }
        else if (Session["search"].ToString() == "customer_ship_to")
        {
            tbShipTo.Text = wucSearch1.result;
            upHeader.Update();
            Session["searchParamCustomerNo"] = null;
        }
        Session["search"] = null;
    }

    protected string query_filter()
    {
        try
        {
            string filter = "";

            if (tbSoldTo.Text.Trim().Replace("'", "`") != "")
            {
                sql.Append("AND ro.sold_to = '" + tbSoldTo.Text.Trim().Replace("'", "`") + "' ");
            }

            if (tbBillTo.Text.Trim().Replace("'", "`") != "")
            {
                sql.Append("AND ro.bill_to = '" + tbBillTo.Text.Trim().Replace("'", "`") + "' ");
            }

            if (tbShipTo.Text.Trim().Replace("'", "`") != "")
            {
                sql.Append("AND ro.ship_to = '" + tbShipTo.Text.Trim().Replace("'", "`") + "' ");
            }

            if (tbStartTransDate.Text.Trim() != "" && tbEndTransDate.Text.Trim() == "")
            {
                filter += "AND ro.trans_date = '" + cAdih.engFormatDate(tbStartTransDate.Text) + "' ";
            }
            else if (tbStartTransDate.Text.Trim() == "" && tbEndTransDate.Text.Trim() != "")
            {
                filter += "AND ro.trans_date = '" + cAdih.engFormatDate(tbEndTransDate.Text) + "' ";
            }
            else if (tbStartTransDate.Text.Trim() != "" && tbEndTransDate.Text.Trim() != "")
            {
                filter += "AND ro.trans_date BETWEEN '" + cAdih.engFormatDate(tbStartTransDate.Text) + "' AND '" + cAdih.engFormatDate(tbEndTransDate.Text) + "' ";
            }

            if (tbStartPostedDate.Text.Trim() != "" && tbEndPostedDate.Text.Trim() == "")
            {
                filter += "AND CONVERT(VARCHAR(10), ro.posted_date, 120) = '" + cAdih.engFormatDate(tbStartPostedDate.Text) + "' ";
            }
            else if (tbStartPostedDate.Text.Trim() == "" && tbEndPostedDate.Text.Trim() != "")
            {
                filter += "AND CONVERT(VARCHAR(10), ro.posted_date, 120) = '" + cAdih.engFormatDate(tbEndPostedDate.Text) + "' ";
            }
            else if (tbStartPostedDate.Text.Trim() != "" && tbEndPostedDate.Text.Trim() != "")
            {
                filter += "AND CONVERT(VARCHAR(10), ro.posted_date, 120) BETWEEN '" + cAdih.engFormatDate(tbStartPostedDate.Text) + "' AND '" + cAdih.engFormatDate(tbEndPostedDate.Text) + "' ";
            }


            if (ddlStatus.SelectedValue == "OPEN")
            {
                filter += "AND ro.status = '0' ";
            }
            else if (ddlStatus.SelectedValue == "POSTED")
            {
                filter += "AND ro.status = '1' AND cc.reason IS NULL and cl.closed_reason IS NULL ";
            }
            if (ddlDelvStatus.SelectedValue == "OUTSTANDING")
            {
                filter += "AND rod.qty > ISNULL(od.qty_POD,0) ";
            }
            else if (ddlDelvStatus.SelectedValue == "COMPLETE")
            {
                filter += "AND rod.qty = ISNULL(od.qty_POD,0) ";
            }

            return filter;
        }
        catch
        {
            return "";
        }
    }

    protected void queryLoadGrid()
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT DISTINCT ro.trans_no, ro.trans_date, ro.status, ro.posted_date, c.customer_name,  ");
            sql.Append("(CASE WHEN cab.alias_name = '' THEN cab.alias_name_full ELSE cab.alias_name_full + ' ' + cab.alias_name END) AS bill_to,  ");
            sql.Append("(CASE WHEN cas.alias_name = '' THEN cas.alias_name_full ELSE cas.alias_name_full + ' ' + cas.alias_name END) AS ship_to,  ");
            sql.Append("cas.street_address + ' ' + cas.city AS street_address,  ");
            sql.Append("ro.cust_po_no, ro.cust_po_date, ro.installation_date, s.description AS installation_by, ISNULL(si.wh_description, '') AS installation_by_site_name,  ");
            sql.Append("'' AS article_no, '' AS article_description, '' AS unit, rod.qty, 0 AS time_duration, '' AS time_unit, 0 AS rate,  ");
            sql.Append("'' AS source, '' AS warehouse, '' AS serial_no, ro.currency_id, ro.total_amount, ro.note,   ");
            sql.Append("ISNULL(od.trans_no, '') AS od_trans_no, CAST(CONVERT(VARCHAR(10), ISNULL(od.posted_date, 0), 120) AS DATETIME) AS od_posted_date, ISNULL(od.real_delivery_date, 0) AS od_real_delivery_date, od.qty_POD AS od_qty,0 as Total,0 as discount  ");
            sql.Append("FROM rental_order ro WITH(READPAST)  ");
            sql.Append("INNER JOIN customer c WITH(READPAST) ON ro.sold_to = c.customer_no  "); 
            sql.Append("INNER JOIN customer_address cab WITH(READPAST) ON ro.sold_to = cab.customer_no AND cab.address_type = '01' AND ro.bill_to = cab.id  ");
            sql.Append("INNER JOIN customer_address cas WITH(READPAST) ON ro.sold_to = cas.customer_no AND cas.address_type = '02' AND ro.ship_to = cas.id  ");
            sql.Append("INNER JOIN source s WITH(READPAST) ON ro.installation_by = s.id  ");
            sql.Append("LEFT JOIN site_wh si WITH(READPAST) ON ro.installation_by_site = si.wh_id  ");
            sql.Append("INNER JOIN rental_order_detail rod WITH(READPAST) ON ro.trans_no = rod.trans_no  ");
            sql.Append("LEFT JOIN(  ");
            sql.Append("    SELECT od.trans_no, od.posted_date, od.reff_order_no, od.real_delivery_date, odd.article_no, SUM(odd.qty_ordered) AS qty_delivery, sum(coalesce(odd.qty_delivered,0)) as qty_POD, odd.reff_line_no  ");
            sql.Append("     FROM outbound_delivery od WITH(READPAST)  ");
            sql.Append("    INNER JOIN outbound_delivery_detail odd WITH(READPAST) ON od.trans_no = odd.trans_no  ");
            sql.Append("    GROUP BY od.trans_no, od.posted_date, od.real_delivery_date, od.reff_order_no, odd.article_no, odd.reff_line_no ");
            sql.Append(") AS od ON ro.trans_no = od.reff_order_no AND rod.line_no = od.reff_line_no ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append(query_filter());
            sql.Append("ORDER BY ro.trans_date ");
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void queryLoadGridDetail()
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT DISTINCT ro.trans_no, ro.trans_date, ro.status, ro.posted_date, c.customer_name,  ");
            sql.Append("(CASE WHEN cab.alias_name = '' THEN cab.alias_name_full ELSE cab.alias_name_full + ' ' + cab.alias_name END) AS bill_to, ");
            sql.Append("(CASE WHEN cas.alias_name = '' THEN cas.alias_name_full ELSE cas.alias_name_full + ' ' + cas.alias_name END) AS ship_to, ");
            sql.Append("cas.street_address, ro.cust_po_no, ro.cust_po_date, ro.installation_date, s.description AS installation_by, ISNULL(si.wh_description, '') AS installation_by_site_name, ");
            sql.Append("rod.article_no, a.article_description, rod.qty, rod.time_duration, 'MON' AS time_unit, rod.rate, rod.total_amount as [Total], rod.disc as [discount],  ");
            sql.Append("sd.description AS source, ISNULL(sw.wh_description, ISNULL(replace(v.vendor_name, ',', ''), '')) AS warehouse, ro.currency_id, ro.total_amount, ro.note,  ");
            sql.Append("ISNULL(od.trans_no, '') AS od_trans_no, CAST(CONVERT(VARCHAR(10), ISNULL(od.posted_date, 0), 120) AS DATETIME) AS od_posted_date, ISNULL(od.real_delivery_date, 0) AS od_real_delivery_date, ISNULL(od.qty_POD, 0) AS od_qty ");
            sql.Append("FROM rental_order ro WITH(READPAST) ");
            sql.Append("INNER JOIN customer c WITH(READPAST) ON ro.sold_to = c.customer_no ");
            sql.Append("INNER JOIN customer_address cab WITH(READPAST) ON ro.sold_to = cab.customer_no AND cab.address_type = '01' AND ro.bill_to = cab.id ");
            sql.Append("INNER JOIN customer_address cas WITH(READPAST) ON ro.sold_to = cas.customer_no AND cas.address_type = '02' AND ro.ship_to = cas.id ");
            sql.Append("INNER JOIN source s WITH(READPAST) ON ro.installation_by = s.id ");
            sql.Append("LEFT JOIN site_wh si WITH(READPAST) ON ro.installation_by_site = si.wh_id ");
            sql.Append("INNER JOIN rental_order_detail rod WITH(READPAST) ON ro.trans_no = rod.trans_no ");
            sql.Append("INNER JOIN article a WITH(READPAST) ON rod.article_no = a.article_no ");
            sql.Append("INNER JOIN source sd WITH(READPAST) ON rod.source_id = sd.id ");
            sql.Append("LEFT JOIN site_wh sw WITH(READPAST) ON rod.wh_id = sw.wh_id ");
            sql.Append("LEFT JOIN vendor v WITH(READPAST) ON rod.wh_id = v.vendor_no ");
            sql.Append("LEFT JOIN( ");
            sql.Append("    SELECT od.trans_no, od.posted_date, od.reff_order_no, od.real_delivery_date, odd.article_no, SUM(odd.qty_ordered) AS qty_delivery, sum(coalesce(odd.qty_delivered,0)) as qty_POD, odd.reff_line_no ");
            sql.Append("     FROM outbound_delivery od WITH(READPAST) ");
            sql.Append("    INNER JOIN outbound_delivery_detail odd WITH(READPAST) ON od.trans_no = odd.trans_no ");
            sql.Append("    GROUP BY od.trans_no, od.posted_date, od.real_delivery_date, od.reff_order_no, odd.article_no, odd.reff_line_no ");
            sql.Append(") AS od ON ro.trans_no = od.reff_order_no AND rod.line_no = od.reff_line_no ");
            sql.Append("WHERE 1 = 1 "); 
            sql.Append(query_filter());
            sql.Append("ORDER BY ro.trans_date ");
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void bExcel_Click(object sender, EventArgs e)
    {
        //try
        //{
        if (cbDetail.Checked)
        {
            queryLoadGridDetail();
        }
        else
        {
            queryLoadGrid();
        }

        Session["export_excel"] = null;
        Session["export_name"] = null;
        Session["export_excel"] = cAdih.getResultDataSet(sql.ToString(), cAdih.getConnStr("Connection"));
        Session["excel_name"] = "RentalOrderMonitoring";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( '../excel.aspx', null, 'fullscreen=yes,status=yes,toolbar=no,menubar=no,location=no' );", true);
        //}
        //catch (Exception ex)
        //{
        //}
    }
}