using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Report_PurchaseOrderMonitoring : Page
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

    protected void ibSearchVendor_Click(object sender, EventArgs e)
    {
        Session["search"] = "vendor";
        wucSearch1.loadGrid();
    }

    protected void ibSearchArticleNo_Click(object sender, EventArgs e)
    {
        Session["search"] = "article_no";
        wucSearch1.loadGrid();
    }

    protected void bShow_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            queryLoadGrid();

            cAdih.loadGridView(sql.ToString(), cAdih.getConnStr("Connection"), grid);
            upGrid.Update();
        }
        catch (Exception ex)
        {
        }
    }

    protected void wucSearch_Hide(object sender, EventArgs e)
    {
        if (Session["search"].ToString() == "vendor")
        {
            tbVendor.Text = wucSearch1.result;
            upHeader.Update();
        }
        else if (Session["search"].ToString() == "article_no")
        {
            tbArticleNo.Text = wucSearch1.result;
            upHeader.Update();
        }
        Session["search"] = null;
    }

    protected string query_filter()
    {
        try
        {
            string filter = "";

            if (tbVendor.Text.Trim().Replace("'", "`") != "")
            {
                sql.Append("AND po.vendor_id = '" + tbVendor.Text.Trim().Replace("'", "`") + "' ");
            }

            if (tbStartTransDate.Text.Trim() != "" && tbEndTransDate.Text.Trim() == "")
            {
                filter += "AND po.trans_date = '" + cAdih.engFormatDate(tbStartTransDate.Text) + "' ";
            }
            else if (tbStartTransDate.Text.Trim() == "" && tbEndTransDate.Text.Trim() != "")
            {
                filter += "AND po.trans_date = '" + cAdih.engFormatDate(tbEndTransDate.Text) + "' ";
            }
            else if (tbStartTransDate.Text.Trim() != "" && tbEndTransDate.Text.Trim() != "")
            {
                filter += "AND po.trans_date BETWEEN '" + cAdih.engFormatDate(tbStartTransDate.Text) + "' AND '" + cAdih.engFormatDate(tbEndTransDate.Text) + "' ";
            }

            if (tbStartPostedDate.Text.Trim() != "" && tbEndPostedDate.Text.Trim() == "")
            {
                filter += "AND CONVERT(VARCHAR(10), ISNULL(ta.approval_date, 0), 120) = '" + cAdih.engFormatDate(tbStartPostedDate.Text) + "' ";
            }
            else if (tbStartPostedDate.Text.Trim() == "" && tbEndPostedDate.Text.Trim() != "")
            {
                filter += "AND CONVERT(VARCHAR(10), ISNULL(ta.approval_date, 0), 120) = '" + cAdih.engFormatDate(tbEndPostedDate.Text) + "' ";
            }
            else if (tbStartPostedDate.Text.Trim() != "" && tbEndPostedDate.Text.Trim() != "")
            {
                filter += "AND CONVERT(VARCHAR(10), ISNULL(ta.approval_date, 0), 120) BETWEEN '" + cAdih.engFormatDate(tbStartPostedDate.Text) + "' AND '" + cAdih.engFormatDate(tbEndPostedDate.Text) + "' ";
            }

            if (tbStartRequiredDate.Text.Trim() != "" && tbEndRequiredDate.Text.Trim() == "")
            {
                filter += "AND po.req_delv_date = '" + cAdih.engFormatDate(tbStartRequiredDate.Text) + "' ";
            }
            else if (tbStartRequiredDate.Text.Trim() == "" && tbEndRequiredDate.Text.Trim() != "")
            {
                filter += "AND po.req_delv_date = '" + cAdih.engFormatDate(tbEndRequiredDate.Text) + "' ";
            }
            else if (tbStartRequiredDate.Text.Trim() != "" && tbEndRequiredDate.Text.Trim() != "")
            {
                filter += "AND po.req_delv_date BETWEEN '" + cAdih.engFormatDate(tbStartRequiredDate.Text) + "' AND '" + cAdih.engFormatDate(tbEndRequiredDate.Text) + "' ";
            }

            if (ddlReceivedStatus.SelectedValue == "OUTSTANDING")
            {
                filter += "AND pod.qty - ISNULL(id.qty_received, 0) > 0 ";
            }
            else if (ddlReceivedStatus.SelectedValue == "FULLY RECEIVED")
            {
                filter += "AND pod.qty - ISNULL(id.qty_received, 0) = 0 ";
            }

            if (ddlStatus.SelectedValue == "OPEN")
            {
                filter += "AND po.total_amount > ISNULL(ta.approval_proxy, 0) ";
            }
            else if (ddlStatus.SelectedValue == "APPROVED")
            {
                filter += "AND po.total_amount <= ISNULL(ta.approval_proxy, 0) ";
            }

            return filter;
        }
        catch (Exception ex)
        {
            return "";
        }
    }

    protected void queryLoadGrid()
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT DISTINCT po.trans_no, po.trans_date, ISNULL(ta.approval_date, 0) AS posted_date, po.purchase_requisition_no, v.vendor_name, po.req_delv_date,  ");
            sql.Append("s.wh_description AS ship_to, po.payment_term, po.currency_id, po.note, ");
            sql.Append("po.created_date, ");
            sql.Append("isnull(po.amount, 0) as amount, isnull(po.tax, 0) as tax, isnull(po.amount, 0)+isnull(po.tax, 0) as total_amount, ");
            sql.Append("ISNULL(id.trans_no, '') AS id_trans_no, ISNULL(id.posted_date, 0) AS id_posted_date, '0' AS id_qty, (select username from[user] u1 where u1.user_id = po.created_by) as created_by,  ");
            sql.Append("po.created_date, isnull((select username from[user] u2 where u2.user_id = po.posted_by), '') as posted_by, po.posted_date, '0' as amount_detail, '0' as tax_detail, '0' as total_amount_detail ");
            sql.Append("FROM purchase_order po WITH(READPAST) ");
            sql.Append("LEFT JOIN site_wh s WITH(READPAST) ON po.ship_to = s.wh_id ");
            sql.Append("INNER JOIN vendor v WITH(READPAST) ON po.vendor_id = v.vendor_no ");
            sql.Append("INNER JOIN purchase_order_detail pod WITH(READPAST) ON po.trans_no = pod.trans_no ");
            sql.Append("INNER JOIN article a WITH(READPAST) ON pod.article_no = a.article_no ");
            sql.Append("LEFT JOIN( ");
            sql.Append("    SELECT ta.trans_no, SUM(ta.approval_proxy) AS approval_proxy, MAX(ta.approval_date) AS approval_date ");
            sql.Append("    FROM trans_approval ta WITH(READPAST) ");
            sql.Append("    GROUP BY ta.trans_no ");
            sql.Append(") AS ta ON po.trans_no = ta.trans_no ");
            sql.Append("LEFT JOIN( ");
            sql.Append("    SELECT id.trans_no, id.posted_date, id.reff_order_no, idd.reff_line_no, SUM(idd.qty_received) AS qty_received ");
            sql.Append("    FROM inbound_delivery id WITH(READPAST) ");
            sql.Append("    INNER JOIN inbound_delivery_detail idd WITH(READPAST) ON id.trans_no = idd.trans_no ");
            sql.Append(" ");
            sql.Append("    GROUP BY id.trans_no, id.reff_order_no, id.posted_date, idd.reff_line_no ");
            sql.Append(") AS id ON po.trans_no = id.reff_order_no AND pod.line_no = id.reff_line_no ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append(query_filter());
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void bExcel_Click(object sender, EventArgs e)
    {
        try
        {
            queryLoadGrid();
            Session["export_excel"] = cAdih.getResultDataSet(sql.ToString(), cAdih.getConnStr("Connection"));
            Session["excel_name"] = "PurchaseRequisitionMonitoring";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( '../excel.aspx', null, 'fullscreen=yes,status=yes,toolbar=no,menubar=no,location=no' );", true);
        }
        catch (Exception ex)
        {

        }
    }
}