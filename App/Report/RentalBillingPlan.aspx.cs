using System;
using System.Data;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class Report_RentalBillingPlan : Page
{

    StringBuilder sql = new StringBuilder();
    ClassAdih cAdih = new ClassAdih();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
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
        Session["search"] = null;
    }

    private void BindData()
    {
        try
        {
            sql.Append("EXEC [dbo].[sp_rpt_billing_plan_getrows] '"+ tbSearch.Text + "','" + Shared.UItoDBDate(txtStartDate.Text) + "','" + Shared.UItoDBDate(txtEndDate.Text) + "','" + ddlStatus.SelectedValue + "','" + Shared.UItoDBchb(cbDetail) + "','" + tbBillTo.Text + "' ");

            cAdih.loadGridView(sql.ToString(), cAdih.getConnStr("Connection"), grid);
            upGrid.Update();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message + "<br />" + ex.InnerException);
        }

    }

    private void ExecExcel()
    {
        try
        {
            MasterPage master = (MasterPage)this.Master;

            sql.Append("EXEC [dbo].[sp_rpt_billing_plan_getrows] '" + tbSearch.Text + "','" + Shared.UItoDBDate(txtStartDate.Text) + "','" + Shared.UItoDBDate(txtEndDate.Text) + "','" + ddlStatus.SelectedValue + "','" + Shared.UItoDBchb(cbDetail) + "','" + tbBillTo.Text + "' ");

            Session["export_excel"] = null;
            Session["export_name"] = null;
            Session["export_excel"] = cAdih.getResultDataSet(sql.ToString(), cAdih.getConnStr("Connection"));
            Session["excel_name"] = "RentalOrderBillingPlan";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( '../excel.aspx', null, 'fullscreen=yes,status=yes,toolbar=no,menubar=no,location=no' );", true);
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }

    }

    protected void imgSearch_Click(object sender, EventArgs e)
    {
        if (cbDetail.Checked == false)
        {
            grid.Columns[6].Visible = false;
            grid.Columns[9].Visible = false;
            grid.Columns[11].Visible = false;
        }
        else
        {
            grid.Columns[6].Visible = true;
            grid.Columns[9].Visible = true;
            grid.Columns[11].Visible = true;

        }
        BindData();
    }

    protected void bShow_Click(object sender, EventArgs e)
    {
        if (cbDetail.Checked == false)
        {
            grid.Columns[6].Visible = false;
            grid.Columns[9].Visible = false;
            grid.Columns[11].Visible = false;
        }
        else
        {
            grid.Columns[6].Visible = true;
            grid.Columns[9].Visible = true;
            grid.Columns[11].Visible = true;

        }
        BindData();

    }

    protected void bExcel_Click(object sender, EventArgs e)
    {
        ExecExcel();
    }

}