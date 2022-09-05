using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

public partial class Report_StockCard : System.Web.UI.Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                tbStartDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                tbEndDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void ibSearchSiteID_Click(object sender, EventArgs e)
    {
        Session["search"] = "all_warehouse";
        wucSearch1.loadGrid();
    }

    protected void bShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (cbDetail.Checked)
            {
                queryLoadGridDetail();
                cAdih.loadGridView(sql.ToString(), cAdih.getConnStr("Connection"), gridDetail);
                grid.Visible = false;
                gridDetail.Visible = true;
                upGrid.Update();
            }
            else
            {
                queryLoadGrid();
                cAdih.loadGridView(sql.ToString(), cAdih.getConnStr("Connection"), grid);
                grid.Visible = true;
                gridDetail.Visible = false;
                upGrid.Update();
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    private void queryLoadGridDetail()
    {
        sql.Length = 0;
        sql.Append("SELECT tbl.site_id,isnull(ss.wh_description, vv.vendor_name) as site_name, tbl.posted_date, tbl.document_no, ");
        sql.Append("ISNULL(s.wh_description, ISNULL(v.vendor_name, ISNULL(c.customer_name, tbl.sourcedest))) AS sourcedest, ");
        sql.Append("tbl.article_no, a.article_description AS article_desc, u.description AS uom, tbl.qty ");
        sql.Append("FROM( ");
        sql.Append("    SELECT astc.id, astc.site_id, astc.document_no, astc.posted_date, ");
        sql.Append("    ISNULL(od.ship_to, '') AS sourcedest, astc.article_no, astc.base_uom_id, sum(astc.qty) 'qty' ");
        sql.Append("    FROM article_stock_card astc WITH(READPAST) ");
        sql.Append("    LEFT JOIN outbound_delivery od WITH(READPAST) ON astc.document_no = od.trans_no ");
        sql.Append("    WHERE qty < 0 ");
        sql.Append("    group by astc.id, astc.site_id, astc.document_no, astc.posted_date, ");
        sql.Append("    od.ship_to, astc.article_no, astc.base_uom_id ");
        sql.Append("    UNION ");
        sql.Append("    SELECT astc.id, astc.site_id, astc.document_no, astc.posted_date, ");
        sql.Append("    ISNULL(id.from_site, '') AS sourcedest, astc.article_no, astc.base_uom_id, sum(astc.qty) 'qty' ");
        sql.Append("    FROM article_stock_card astc WITH(READPAST) ");
        sql.Append("    LEFT JOIN( ");
        sql.Append("        SELECT trans_no, from_site FROM inbound_delivery WITH(READPAST) ");
        sql.Append("    ) AS id ON astc.document_no = id.trans_no ");
        sql.Append("    WHERE qty >= 0 ");
        sql.Append("    group by astc.id, astc.site_id, astc.document_no, astc.posted_date, ");
        sql.Append("    id.from_site, astc.article_no, astc.base_uom_id ");
        sql.Append(") AS tbl ");
        sql.Append("INNER JOIN article a WITH(READPAST) ON tbl.article_no = a.article_no AND a.article_type <> 'ZSER' ");
        sql.Append("INNER JOIN uom u WITH(READPAST) ON tbl.base_uom_id = u.id ");
        sql.Append("LEFT JOIN site_wh s WITH(READPAST) ON tbl.sourcedest = s.wh_id ");
        sql.Append("LEFT JOIN site_wh ss WITH(READPAST) ON tbl.site_id = ss.wh_id ");
        sql.Append("LEFT JOIN vendor v WITH(READPAST) ON tbl.sourcedest = v.vendor_no ");
        sql.Append("LEFT JOIN vendor vv WITH(READPAST) ON tbl.site_id = vv.vendor_no ");
        sql.Append("LEFT JOIN customer c WITH(READPAST) ON tbl.sourcedest = c.customer_no ");
        sql.Append("WHERE 1 = 1 ");
        if (tbSiteID.Text != "")
        {
            sql.Append("    AND tbl.site_id = '" + tbSiteID.Text + "' ");
        }
        if (tbArticleGroup.Text != "")
        {
            sql.Append("    AND tbl.article_no = '" + tbArticleGroup.Text + "' ");
        }
    }

    protected void wucSearch_Hide(object sender, EventArgs e)
    {
        if (Session["search"].ToString() == "all_warehouse")
        {
            tbSiteID.Text = wucSearch1.result;
            upHeader.Update();
        }
        else if (Session["search"].ToString() == "article")
        {
            tbArticleGroup.Text = wucSearch1.result;
            upHeader.Update();
        }
        Session["search"] = null;
    }

    protected void queryLoadGrid()
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT tbl.article_no, tbl.article_description, tbl.unitID, tbl.unitDesc, ");
            sql.Append("tbl.beg_balance, tbl.stock_in, tbl.stock_out, tbl.end_balance ");
            sql.Append("FROM (");
            sql.Append("SELECT a.article_no, a.article_description AS article_description, a.base_uom AS unitID, b.description AS unitDesc, ");
            sql.Append("ISNULL(c.sum_qty, 0) AS beg_balance, ABS(ISNULL(d.sum_qty, 0)) AS stock_in, ABS(ISNULL(e.sum_qty, 0)) AS stock_out, ");
            sql.Append("(ISNULL(c.sum_qty, 0) + ABS(ISNULL(d.sum_qty, 0)) - ABS(ISNULL(e.sum_qty, 0))) AS end_balance ");
            sql.Append(" ");
            sql.Append("FROM article a WITH(READPAST) ");
            sql.Append("INNER JOIN uom b WITH(READPAST) ON a.base_uom = b.id ");
            sql.Append("LEFT JOIN( ");
            sql.Append("	SELECT article_no, base_uom_id, SUM(qty) AS sum_qty ");
            sql.Append("	FROM article_stock_card WITH(READPAST) ");
            sql.Append("    WHERE 1 = 1 ");
            if (tbSiteID.Text != "")
            {
                sql.Append("    AND site_id = '" + tbSiteID.Text + "' ");
            }
            sql.Append("    AND CONVERT(VARCHAR(10), posted_date, 120) < '" + cAdih.engFormatDate(tbStartDate.Text) + "' ");
            sql.Append("	GROUP BY article_no, base_uom_id ");
            sql.Append(") AS c ON a.article_no = c.article_no AND a.base_uom = c.base_uom_id ");
            sql.Append("LEFT JOIN( ");
            sql.Append("	SELECT article_no, base_uom_id, SUM(qty) AS sum_qty ");
            sql.Append("	FROM article_stock_card WITH(READPAST) ");
            sql.Append("    WHERE 1 = 1 AND qty > 0 ");
            if (tbSiteID.Text != "")
            {
                sql.Append("    AND site_id = '" + tbSiteID.Text + "' ");
            }
            sql.Append("    AND CONVERT(VARCHAR(10), posted_date, 120) BETWEEN '" + cAdih.engFormatDate(tbStartDate.Text) + "' AND '" + cAdih.engFormatDate(tbEndDate.Text) + "' ");
            sql.Append("	GROUP BY article_no, base_uom_id ");
            sql.Append(") AS d ON a.article_no = d.article_no AND a.base_uom = d.base_uom_id ");
            sql.Append("LEFT JOIN( ");
            sql.Append("	SELECT article_no, base_uom_id, SUM(qty) AS sum_qty ");
            sql.Append("	FROM article_stock_card WITH(READPAST) ");
            sql.Append("    WHERE 1 = 1 AND qty < 0 ");
            if (tbSiteID.Text != "")
            {
                sql.Append("    AND site_id = '" + tbSiteID.Text + "' ");
            }
            sql.Append("    AND CONVERT(VARCHAR(10), posted_date, 120) BETWEEN '" + cAdih.engFormatDate(tbStartDate.Text) + "' AND '" + cAdih.engFormatDate(tbEndDate.Text) + "' ");
            sql.Append("	GROUP BY article_no, base_uom_id ");
            sql.Append(") AS e ON a.article_no = e.article_no AND a.base_uom = e.base_uom_id ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append("AND a.article_type NOT IN ('ZSER') "); 
            if (tbArticleGroup.Text != "")
            {
                sql.Append("    AND a.article_no = '" + tbArticleGroup.Text + "' ");
            }
            sql.Append(") AS tbl ");
            sql.Append("WHERE (tbl.beg_balance <> '0' OR tbl.stock_in <> '0' OR tbl.stock_out <> '0' AND tbl.end_balance <> '0')  ");
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
            MasterPage master = (MasterPage)this.Master;

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
            Session["excel_name"] = "StockCard";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( '../excel.aspx', null, 'fullscreen=yes,status=yes,toolbar=no,menubar=no,location=no' );", true);
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void ibSearchArticleGroup_Click(object sender, ImageClickEventArgs e)
    {
        Session["search"] = "article";
        wucSearch1.loadGrid();
    }

    protected string query_filter()
    {
        try
        {
            string filter = "";

            return filter;
        }
        catch
        {
            return "";
        }
    }
}