using System;
using System.Data;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text;

public partial class Transaction_RvHeaderList : Page
{
    private static string TABLE_NAME = "rv_header";
    ClassAdih cAdih = new ClassAdih();
    StringBuilder sql = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindData();
            loadDdlPageOf();
            loadRecords();
        }
    }

    private void BindData()
    {
        sql.Length = 0;
        sql.Append("exec sp_rv_header_getrows '" + tbSearch.Text + "','" + ddlStatus.SelectedValue + "' ");

        cAdih.loadGridView(sql.ToString(), cAdih.getConnStr("Connection"), grid);
        upGrid.Update();

    }

    private void DeleteData()
    {
        try
        {
            sql.Length = 0;
            sql.Append("DELETE FROM rv_header WHERE rv_no = '" + Session["RVHeaderRVNo"].ToString() + "' ");
            sql.Append("DELETE FROM rv_detail WHERE rv_no = '" + Session["RVHeaderRVNo"].ToString() + "' ");

            cAdih.executeNonQuery(sql.ToString(), cAdih.getConnStr("Connection").ToString());

            BindData();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message + "<br />" + ex.InnerException);
        }
    }

    protected void showConfirmBox(string message)
    {
        wucConfirmBox1.SubShowConfirmBox(message);
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("CustomerReceipt.aspx?action=add");
    }

    protected void imgSearch_Click(object sender, EventArgs e)
    {
        BindData();
        loadDdlPageOf();
        loadRecords();
    }

    protected void lbDelete_Click(object sender, EventArgs e)
    {
        GridView grid = ((sender as LinkButton).Parent.Parent.Parent.Parent as GridView);
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);

        string sRVNo = grid.DataKeys[gridRow.RowIndex][0].ToString();

        Session["RVHeaderMode"] = "delete";
        Session["RVHeaderRVNo"] = sRVNo;

        showConfirmBox("Delete " + sRVNo + "?");

    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (wucConfirmBox1.StrMode == "Yes")
            {
                if (Session["RVHeaderMode"].ToString() == "delete")
                {
                    DeleteData();
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void lbEdit_Click(object sender, EventArgs e)
    {
        GridView grid = ((sender as LinkButton).Parent.Parent.Parent.Parent as GridView);
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);

        Response.Redirect(string.Format("CustomerReceipt.aspx?action=edit&rvno={0}", grid.DataKeys[gridRow.RowIndex][0].ToString()));
    }

    protected void imgPrint_Click(object sender, EventArgs e)
    {
        GridView grid = ((sender as LinkButton).Parent.Parent.Parent.Parent as GridView);
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);

        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( 'CustomerReceipt.aspx?rvno=" + grid.DataKeys[gridRow.RowIndex][0].ToString() + "&type=RV" + "', null, 'width=750px,fullscreen=no,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes' );", true);
    }

    protected void ddlStatus_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
        loadDdlPageOf();
        loadRecords();
    }

    protected void linkFirst_Click(Object sender, EventArgs e)
    {
        grid.PageIndex = 0;
        //Session["PageNoLastEdited"] = "0";
        BindData();
        ddlPageOf.SelectedValue = "1";
        loadRecords();

    }

    protected void linkPrev_Click(Object sender, EventArgs e)
    {
        if (grid.PageIndex > 0)
        {
            grid.PageIndex = grid.PageIndex - 1;
            //Session["PageNoLastEdited"] = grid.PageIndex;
            BindData();
            ddlPageOf.SelectedValue = (grid.PageIndex + 1).ToString(); ;
            loadRecords();
        }

    }

    protected void linkNext_Click(Object sender, EventArgs e)
    {
        grid.PageIndex = grid.PageIndex + 1;
        //Session["PageNoLastEdited"] = grid.PageIndex;
        BindData();
        ddlPageOf.SelectedValue = (grid.PageIndex + 1).ToString(); ;
        loadRecords();

    }

    protected void linkLast_Click(Object sender, EventArgs e)
    {
        grid.PageIndex = grid.PageCount - 1;
        //Session["PageNoLastEdited"] = grid.PageIndex;
        BindData();
        ddlPageOf.SelectedValue = grid.PageCount.ToString();
        loadRecords();
    }

    protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grid.PageIndex = e.NewPageIndex;
        //Session["PageNoLastEdited"] = e.NewPageIndex;
        //loadGrid();
    }

    protected void loadRecords()
    {
        try
        {
            String FirstRowNo = (((grid.PageIndex) * grid.PageSize) + 1).ToString();
            if (grid.Rows.Count == 0)
            {
                FirstRowNo = "0";
            }
            String LastRowNo = ((grid.PageSize * grid.PageIndex) + (grid.Rows.Count)).ToString();
            lblRecords.Text = "Records: " + FirstRowNo + "-" + LastRowNo + "";
            upFooter.Update();
        }
        catch (Exception ex)
        {

        }
    }

    protected void loadDdlPageOf()
    {
        try
        {
            //LOAD TRANS
            int i = 1;
            int pageOf = 0;
            int sumTrans = (grid.DataSource as DataTable).Rows.Count;
            if (sumTrans % 10 > 0)
            {
                pageOf = (sumTrans / 10) + 1;
            }
            else if (sumTrans % 10 == 0)
            {
                pageOf = sumTrans / 10;
            }

            ddlPageOf.Items.Clear();
            for (i = 1; i <= pageOf; i++)
            {
                ListItem ls = new ListItem();
                ls.Value = i.ToString();
                ls.Text = i.ToString();
                ddlPageOf.Items.Add(ls);
            }

            lblPageOf.Text = "of " + pageOf.ToString() + "";
            lblTotalRecords.Text = "Total Records: " + sumTrans.ToString() + "";
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void ddlPageOf_SelectedIndexChanged(object sender, EventArgs e)
    {
        grid.PageIndex = int.Parse(ddlPageOf.SelectedValue) - 1;
        BindData();
        loadRecords();
    }

    protected void grid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[6].Text == "HOLD")
            {
                ((ImageButton)e.Row.FindControl("imgPrint")).Visible = false;
                ((Label)e.Row.FindControl("lblPrint")).Visible = true;
            }
        }
    }
}