using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Report_SalesInvoiceMonitoring : Page
{
    private StringBuilder sql = new StringBuilder();
    private SqlTransaction trans;
    private SqlConnection conn;
    private SqlCommand cmd;
    private SqlDataReader dr;
    private DataTable dt;
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

    protected void bShow_Click(object sender, EventArgs e)
    {
        try
        {
            grid.Visible = true;
            queryLoadGrid();
            grid.Visible = true;
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
            queryLoadGrid();
            Session["export_excel"] = cAdih.getResultDataSet(sql.ToString(), cAdih.getConnStr("Connection"));
            Session["excel_name"] = Session["company_id"].ToString() + "SalesInvoiceMonitoring";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( '../excel.aspx', null, 'fullscreen=yes,status=yes,toolbar=no,menubar=no,location=no' );", true);
        }
        catch (Exception ex)
        {
        }
    }

    protected void ibSearchCustomer_Click(object sender, EventArgs e)
    {
        Session["search"] = "customer";
        wucSearch1.loadGrid();
    }

    protected void ibSearchCustBillToSite_Click(object sender, EventArgs e)
    {
        if (tbCustomer.Text.Trim() == "")
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox("Input customer!");
            return;
        }
        else
        {
            Session["searchParamCustomerNo"] = tbCustomer.Text.Trim();
            Session["search"] = "customer_bill_to";
            wucSearch1.loadGrid();
        }
    }

    protected void wucSearch_Hide(object sender, EventArgs e)
    {
        if (Session["search"].ToString() == "customer")
        {
            tbCustomer.Text = wucSearch1.result;
        }
        else if (Session["search"].ToString() == "customer_bill_to")
        {
            tbCustBillToSite.Text = wucSearch1.result;
            Session["searchParamCustomerNo"] = null;
        }
        upHeader.Update();
        Session["search"] = null;
    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            
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

            if (tbCustomer.Text.Trim().Replace("'", "`") != "")
            {
                filter += "AND si.customer = '" + tbCustomer.Text.Trim().Replace("'", "`").ToUpper() + "' ";
            }

            if (tbCustBillToSite.Text.Trim().Replace("'", "`") != "")
            {
                filter += "AND si.bill_to = '" + tbCustBillToSite.Text.Trim().Replace("'", "`").ToUpper() + "' ";
            }

            if (tbStartDueDate.Text.Trim() != "" && tbEndDueDate.Text.Trim() == "")
            {
                filter += "AND rh.receipt_date = '" + cAdih.engFormatDate(tbStartDueDate.Text) + "' ";
            }
            else if (tbStartDueDate.Text.Trim() == "" && tbEndDueDate.Text.Trim() != "")
            {
                filter += "AND rh.receipt_date = '" + cAdih.engFormatDate(tbEndDueDate.Text) + "' ";
            }
            else if (tbStartDueDate.Text.Trim() != "" && tbEndDueDate.Text.Trim() != "")
            {
                filter += "AND rh.receipt_date BETWEEN '" + cAdih.engFormatDate(tbStartDueDate.Text) + "' AND '" + cAdih.engFormatDate(tbEndDueDate.Text) + "' ";
            }


            if (tbKeyWord.Text.Trim().Replace("'", "`") != "")
            {
                filter += "AND (si.note LIKE '%" + tbKeyWord.Text.Trim().Replace("'", "`").ToUpper() + "%' OR si.sales_order_no LIKE '%" + tbKeyWord.Text.Trim().Replace("'", "`").ToUpper() + "%' ";
                filter += "OR ro.cust_po_no LIKE '%" + tbKeyWord.Text.Trim().Replace("'", "`").ToUpper() + "%') ";
            }

            if (ddlPaymentStatus.SelectedValue == "OUTSTANDING")
            {
                filter += "AND si.total_amount < rd.base_amount ";
            }
            else if (ddlPaymentStatus.SelectedValue == "COMPLETE")
            {
                filter += "AND si.total_amount >= rd.base_amount  ";
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
            sql.Append("SELECT si.trans_no, si.trans_date, CONVERT(VARCHAR(10), si.posted_date, 120) AS posted_date,  ");
            sql.Append("c.customer_name, ca.alias_name_full AS cust_name_bill_to_name, ");
            sql.Append("si.sales_order_no, ro.cust_po_no AS cust_po_no, si.note, si.total_amount, rd.rv_no as bm_no, rh.receipt_date as bm_date, rd.amount_paid as bm_amount ");
            sql.Append("FROM sales_invoice si WITH(READPAST) ");
            sql.Append("INNER JOIN customer c WITH(READPAST) ON si.customer = c.customer_no ");
            sql.Append("INNER JOIN customer_address ca WITH(READPAST) ON si.customer = ca.customer_no AND si.bill_to = ca.id ");
            sql.Append("LEFT JOIN rental_order ro WITH(READPAST) ON si.sales_order_no = ro.trans_no ");
            sql.Append("LEFT JOIN rv_detail rd WITH(READPAST) on rd.reff_no = si.trans_no ");
            sql.Append("LEFT JOIN rv_header rh WITH(READPAST) on rh.rv_no = rd.rv_no ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append(query_filter());
            sql.Append("ORDER BY si.trans_date ");
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }
}