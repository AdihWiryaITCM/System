using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;


public partial class Report_ArticleSerialNo : Page
{
    StringBuilder sql = new StringBuilder();
    StringBuilder sql1 = new StringBuilder();
    ClassAdih cAdih = new ClassAdih();

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

    protected void ibSearchSiteID_Click(object sender, EventArgs e)
    {
        Session["search"] = "all_warehouse";
        wucSearch1.loadGrid();
    }

    protected void ibSearchArticle_Click(object sender, EventArgs e)
    {
        Session["search"] = "article";
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
            master.messageBox(ex.Message);
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
            tbArticle.Text = wucSearch1.result;
            upHeader.Update();
        }
        Session["search"] = null;
    }

    protected void queryLoadGrid()
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT ISNULL(s.wh_description, ISNULL(v.vendor_name, c.customer_name)) AS site_name, asn.article_no, a.article_description, asn.serial_no  ");
            sql.Append("FROM article_serial_no asn WITH(READPAST) ");
            sql.Append("LEFT JOIN site_wh s WITH(READPAST) ON asn.site_id = s.wh_id ");
            sql.Append("LEFT JOIN vendor v WITH(READPAST) ON asn.site_id = v.vendor_no ");
            sql.Append("LEFT JOIN customer c WITH(READPAST) on c.customer_no = asn.site_id ");
            sql.Append("INNER JOIN article a WITH(READPAST) ON asn.article_no = a.article_no ");
            sql.Append(query_filter());
            sql.Append("ORDER BY site_name, asn.article_no ");
        }
        catch (Exception ex)
        {

            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected string query_filter()
    {
        try
        {
            string filter = "";

            if (tbSiteID.Text.Trim().Replace("'", "`") != "")
            {
                filter += "AND asn.site_id = '" + tbSiteID.Text.Trim().Replace("'", "`") + "' ";
            }

            if (tbArticle.Text.Trim().Replace("'", "`") != "")
            {
                filter += "AND asn.article_no = '" + tbArticle.Text.Trim().Replace("'", "`") + "' ";
            }

            if (tbSerialNo.Text.Trim().Replace("'", "`") != "")
            {
                filter += "AND asn.serial_no LIKE '%" + tbSerialNo.Text.Trim().Replace("'", "`") + "%' ";
            } 

            return filter;
        }
        catch
        {
            return "";
        }
    }

    protected void bExcel_Click(object sender, EventArgs e)
    {
        try
        {
            queryLoadGrid();
            Session["export_excel"] = null;
            Session["export_name"] = null;
            Session["export_excel"] = cAdih.getResultDataSet(sql.ToString(), cAdih.getConnStr("Connection"));
            Session["excel_name"] = "ArticleSerialNo";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( '../excel.aspx', null, 'fullscreen=yes,status=yes,toolbar=no,menubar=no,location=no' );", true);
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }
}