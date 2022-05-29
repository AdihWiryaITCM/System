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

public partial class Report_PurchaseRequisitionMonitoring : Page
{
    StringBuilder sql = new StringBuilder();
    SqlTransaction trans;
    SqlConnection conn;
    SqlCommand cmd;
    SqlDataReader dr;
    DataTable dt;
    ClassAdih cAdih = new ClassAdih();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                upHeader.Update();
            }

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
                Label lPostedDate = (Label)Row.FindControl("lPostedDate");

                if (lPostedDate.Text == "01 Jan 1900")
                {
                    lPostedDate.Visible = false;
                }
                else
                {
                    lPostedDate.Visible = true;
                }
            }
            upGrid.Update();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void bShow_Click(object sender, EventArgs e)
    {
        try
        {
            queryLoadGrid();
            cAdih.loadGridView(sql.ToString(), cAdih.getConnStr("Connection"), grid);
            upGrid.Update();
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
            queryLoadExcel();
            Session["export_excel"] = cAdih.getResultDataSet(sql.ToString(), cAdih.getConnStr("Connection"));
            Session["excel_name"] = "PurchaseRequisitionMonitoring";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( '../excel.aspx', null, 'fullscreen=yes,status=yes,toolbar=no,menubar=no,location=no' );", true);
        }
        catch (Exception ex)
        {

        }
    }

    protected void wucSearch_Hide(object sender, EventArgs e)
    {
        if (Session["search"].ToString() == "article")
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
            if (tbStartTransDate.Text.Trim() != "" && tbEndTransDate.Text.Trim() == "")
            {
                filter += "AND a.trans_date = '" + cAdih.engFormatDate(tbStartTransDate.Text) + "' ";
            }
            else if (tbStartTransDate.Text.Trim() == "" && tbEndTransDate.Text.Trim() != "")
            {
                filter += "AND a.trans_date = '" + cAdih.engFormatDate(tbEndTransDate.Text) + "' ";
            }
            else if (tbStartTransDate.Text.Trim() != "" && tbEndTransDate.Text.Trim() != "")
            {
                filter += "AND a.trans_date BETWEEN '" + cAdih.engFormatDate(tbStartTransDate.Text) + "' AND '" + cAdih.engFormatDate(tbEndTransDate.Text) + "' ";
            }

            if (tbStartPostedDate.Text.Trim() != "" && tbEndPostedDate.Text.Trim() == "")
            {
                filter += "AND CONVERT(VARCHAR(10), a.posting_date, 120) = '" + cAdih.engFormatDate(tbStartPostedDate.Text) + "' ";
            }
            else if (tbStartPostedDate.Text.Trim() == "" && tbEndPostedDate.Text.Trim() != "")
            {
                filter += "AND CONVERT(VARCHAR(10), a.posting_date, 120) = '" + cAdih.engFormatDate(tbEndPostedDate.Text) + "' ";
            }
            else if (tbStartPostedDate.Text.Trim() != "" && tbStartPostedDate.Text.Trim() != "")
            {
                filter += "AND CONVERT(VARCHAR(10), a.posting_date, 120) BETWEEN '" + cAdih.engFormatDate(tbStartPostedDate.Text) + "' AND '" + cAdih.engFormatDate(tbEndPostedDate.Text) + "' ";
            }

            if (tbStartRequiredDate.Text.Trim() != "" && tbEndRequiredDate.Text.Trim() == "")
            {
                filter += "AND b.required_date = '" + cAdih.engFormatDate(tbStartRequiredDate.Text) + "' ";
            }
            else if (tbStartRequiredDate.Text.Trim() == "" && tbEndRequiredDate.Text.Trim() != "")
            {
                filter += "AND b.required_date = '" + cAdih.engFormatDate(tbEndRequiredDate.Text) + "' ";
            }
            else if (tbStartRequiredDate.Text.Trim() != "" && tbEndRequiredDate.Text.Trim() != "")
            {
                filter += "AND b.required_date BETWEEN '" + cAdih.engFormatDate(tbStartRequiredDate.Text) + "' AND '" + cAdih.engFormatDate(tbEndRequiredDate.Text) + "' ";
            }

            if (ddlStatus.SelectedValue == "OPEN")
            {
                filter += "AND a.status = 'HOLD' ";
            }
            else if (ddlStatus.SelectedValue == "POSTED")
            {
                filter += "AND a.status = 'POSTED' ";
            }

            if (tbNote.Text.Trim() != "")
            {
                filter += "AND a.note LIKE '%" + tbNote.Text.Trim().Replace("'", "`") + "%' ";
            }

            if (ddlOrderedStatus.SelectedValue == "OUTSTANDING")
            {
                filter += "AND ISNULL(b.qty, 0) - ISNULL(tbl.total_qty,0) > 0 ";
                //TBL.total_qty untuk perbandingan
            }
            else if (ddlOrderedStatus.SelectedValue == "COMPLETE")
            {
                filter += "AND ISNULL(b.qty, 0) = ISNULL(tbl.total_qty, 0) ";

                //TBL.total_qty untuk perbandingan
            }


            if (tbArticleNo.Text.Trim().Length > 0)
            {
                string articleNo = "";
                String[] lblArticleNo = tbArticleNo.Text.Split(',');
                for (int i = 0; i < lblArticleNo.Length; i++)
                {
                    if (articleNo == "")
                    {
                        articleNo = "'" + lblArticleNo[i].Trim() + "', ";
                    }
                    else
                    {
                        articleNo += "'" + lblArticleNo[i].Trim() + "', ";
                    }
                }
                filter += "AND b.article_no IN (" + cAdih.left(articleNo, articleNo.Length - 2) + ") ";

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
            sql.Append("SELECT distinct  ");
            sql.Append("a.trans_no, ");
            sql.Append("a.trans_date, ");
            sql.Append("a.posting_date as POSTED_DATE, ");
            sql.Append("a.ship_to, ");
            sql.Append("ISNULL(e.wh_description, ISNULL(f.vendor_name, h.customer_name)) AS ship_to_name, ");
            sql.Append("a.note, ");
            sql.Append("b.line_no, ");
            sql.Append("b.article_no, ");
            sql.Append("c.article_description, ");
            sql.Append("c.base_uom as unit_id, ");
            sql.Append("d.description AS unit_name, ");
            sql.Append("b.qty, ");
            sql.Append("b.required_date, ");
            sql.Append("b.info_price, ");
            sql.Append("b.note AS note_detail, ");
            sql.Append("pod.trans_no AS nomor_po,  ");
            sql.Append("(select trans_date from purchase_order where trans_no = pod.trans_no) AS tgl_approval_po, ");
            sql.Append("ISNULL(TBL.total_qty, 0) AS TotalQty, ");
            sql.Append(" ISNULL(tbl.total_qty, 0) as qtyPO,  ");
            sql.Append("ISNULL(f.vendor_name, '') AS po_vendor_name, ");
            sql.Append(" ISNULL((select req_delv_date from purchase_order where trans_no = pod.trans_no),'') AS po_req_delv_date ");
            sql.Append("FROM purchase_requisition a WITH(READPAST) ");
            sql.Append("INNER JOIN purchase_requisition_detail b WITH(READPAST) ON(a.trans_no = b.trans_no) ");
            sql.Append("LEFT JOIN( ");
            sql.Append("          SELECT ");
            sql.Append("          pod.purchase_requisition_no, ");
            sql.Append("          pod.pr_line_no, ");
            sql.Append("          SUM(pod.qty) AS total_qty, ");
            sql.Append("          pod.article_no ");
            sql.Append("          FROM purchase_order po WITH(READPAST) ");
            sql.Append("          INNER JOIN purchase_order_detail pod WITH(READPAST) on (pod.trans_no = po.trans_no) ");
            sql.Append("          WHERE 1 = 1 ");
            sql.Append("          AND po.approval_status = 'APPROVED' ");
            sql.Append("          GROUP BY pod.purchase_requisition_no, pod.pr_line_no, pod.article_no ");
            sql.Append("          ) AS TBL on(TBL.purchase_requisition_no = a.trans_no AND TBL.pr_line_no = b.line_no AND TBL.article_no = b.article_no) ");
            sql.Append("left join purchase_order_detail pod on(pod.purchase_requisition_no = tbl.purchase_requisition_no and b.line_no = pod.pr_line_no and b.article_no = pod.article_no) ");
            sql.Append("INNER JOIN article c WITH(READPAST) ON(b.article_no = c.article_no) ");
            sql.Append("INNER JOIN uom d WITH(READPAST) ON(c.base_uom = d.id) ");
            sql.Append("LEFT JOIN site_wh e WITH(READPAST) ON(a.ship_to = e.wh_id) ");
            sql.Append("LEFT JOIN vendor f WITH(READPAST) ON(a.ship_to = f.vendor_no) ");
            sql.Append("LEFT JOIN customer h WITH(READPAST) ON(a.ship_to = h.customer_no) ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append(query_filter());
            sql.Append("ORDER BY a.trans_date, a.posting_date ");
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void queryLoadExcel()
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT distinct  ");
            sql.Append("a.trans_no AS[Trans No], ");
            sql.Append("a.trans_date AS[Trans Date], ");
            sql.Append("a.posting_date AS[Posted Date], ");
            sql.Append("a.ship_to AS[Ship To], ");
            sql.Append("ISNULL(e.wh_description, ISNULL(f.vendor_name, h.customer_name)) AS[Ship To Name], ");
            sql.Append("a.note AS[Note], ");
            sql.Append("b.line_no AS[Line No], ");
            sql.Append("b.article_no AS[Article No], ");
            sql.Append("c.article_description AS[Article Description], ");
            sql.Append("c.base_uom AS[Unit ID], ");
            sql.Append("d.description AS[Unit Desc], ");
            sql.Append("b.qty AS[Qty], ");
            sql.Append("b.required_date AS[Required Date], ");
            sql.Append("b.info_price AS[Info Price], ");
            sql.Append("b.note AS[Note Detail], ");
            sql.Append("pod.trans_no AS[Purchase Order No], ");
            sql.Append("(select trans_date from purchase_order where trans_no = pod.trans_no) AS[PO Approval Date],  ");
            sql.Append("ISNULL(tbl.total_qty, 0) as [PO QTY],  ");
            sql.Append("ISNULL(f.vendor_name, '') AS[Vendor Name],  ");
            sql.Append("ISNULL((select req_delv_date from purchase_order where trans_no = pod.trans_no),'') AS[Req Delv Date] ");
            sql.Append("FROM purchase_requisition a WITH(READPAST) ");
            sql.Append("INNER JOIN purchase_requisition_detail b WITH(READPAST) ON(a.trans_no = b.trans_no) ");
            sql.Append("LEFT JOIN( ");
            sql.Append("          SELECT pod.purchase_requisition_no, ");
            sql.Append("          pod.pr_line_no, ");
            sql.Append("          SUM(pod.qty) AS total_qty, ");
            sql.Append("          pod.article_no ");
            sql.Append("          FROM purchase_order po WITH(READPAST) ");
            sql.Append("          INNER JOIN purchase_order_detail pod WITH(READPAST) on (pod.trans_no = po.trans_no) ");
            sql.Append("          WHERE 1 = 1 ");
            sql.Append("          AND po.approval_status = 'APPROVED' ");
            sql.Append("          GROUP BY pod.purchase_requisition_no, pod.pr_line_no, pod.article_no ");
            sql.Append("          ) AS TBL on(TBL.purchase_requisition_no = a.trans_no AND TBL.pr_line_no = b.line_no AND TBL.article_no = b.article_no) ");
            sql.Append("left join purchase_order_detail pod on(pod.purchase_requisition_no = tbl.purchase_requisition_no and b.line_no = pod.pr_line_no and b.article_no = pod.article_no) ");
            sql.Append("INNER JOIN article c WITH(READPAST) ON(b.article_no = c.article_no) ");
            sql.Append("INNER JOIN uom d WITH(READPAST) ON(c.base_uom = d.id) ");
            sql.Append("LEFT JOIN site_wh e WITH(READPAST) ON(a.ship_to = e.wh_id) ");
            sql.Append("LEFT JOIN vendor f WITH(READPAST) ON(a.ship_to = f.vendor_no) ");
            sql.Append("LEFT JOIN customer h WITH(READPAST) ON(a.ship_to = h.customer_no) ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append(query_filter());
            sql.Append("ORDER BY a.trans_date, a.posting_date ");
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void ibSearchArticleNo_Click(object sender, EventArgs e)
    {
        Session["search"] = "article";
        wucSearch1.loadGrid();
    }
}