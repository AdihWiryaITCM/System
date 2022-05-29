using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transaction_PurchaseOrder : System.Web.UI.Page
{
    private SqlConnection conn;
    private SqlConnection conn2;
    private SqlCommand cmd;
    private SqlCommand cmd2;
    private SqlDataReader dr;
    private SqlDataReader dr2;
    private SqlDataAdapter da;
    private DataTable dt;
    private StringBuilder sql = new StringBuilder();
    private StringBuilder sql2 = new StringBuilder();
    private ClassAdih cAdih = new ClassAdih();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            loadGrid();
            loadDdlPageOf();
            loadRecords();
        }
    }

    protected void showConfirmBox(string message)
    {
        wucConfirmBox1.SubShowConfirmBox(message);
    }

    protected void imgSearch_Click(object sender, EventArgs e)
    {
        loadGrid();
        loadDdlPageOf();
        loadRecords();
    }

    protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadGrid();
        loadDdlPageOf();
        loadRecords();
    }

    protected void ddlLastStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadGrid();
        loadDdlPageOf();
        loadRecords();
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transaction/PurchaseOrderProcess.aspx?mode=add");
    }

    protected void loadGrid()
    {
        try
        {
            sql.Length = 0;
            sql.Append("EXEC [sp_load_trans_purchase_order] '" + ddlFilter.SelectedValue + "', '" + tbSearch.Text.Trim().Replace("'", "`") + "' ");

            cAdih.loadGridView(sql.ToString(), cAdih.getConnStr("Connection"), grid);

            upGrid.Update();
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
        loadGrid();
        loadRecords();
    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridViewRow Row = e.Row;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblTransNo = (Label)Row.FindControl("lblTransNo");
                Label lblStatus = (Label)Row.FindControl("lblStatus");
                Label lblClosed = (Label)Row.FindControl("lblClosed");

                LinkButton lbEdit = (LinkButton)Row.FindControl("lbEdit");
                LinkButton lbDelete = (LinkButton)Row.FindControl("lbDelete");
                Label lblDelete = (Label)Row.FindControl("lblDelete");
                Label lblTotalAmount = (Label)Row.FindControl("lblTotalAmount");
                Label lblApproval = (Label)Row.FindControl("lblApprovalStatus");
                ImageButton imgMail = (ImageButton)Row.FindControl("imgMail");
                ImageButton imgDeliveryOrder = (ImageButton)Row.FindControl("imgDeliveryOrder");

                if(lblStatus.Text == "True")
                {
                    lbDelete.Visible = false;
                    lblDelete.Visible = true;
                }
                lblTotalAmount.Text = "Total PO: " + lblTotalAmount.Text + "<br />";
                //if (double.Parse(lblTotalApproval.Text) == 0)
                //{
                //    lblTotalApproval.Text = "Total Proxy: <br />" + lblTotalApproval.Text;
                //}
                //else
                //{
                //    lblTotalApproval.Text = "Total Proxy: " + lblTotalApproval.Text;
                //}
            }
            upGrid.Update();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
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
                System.Web.UI.WebControls.ListItem ls = new System.Web.UI.WebControls.ListItem();
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

    protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grid.PageIndex = e.NewPageIndex;
        //Session["PageNoLastEdited"] = e.NewPageIndex;
        loadGrid();
    }

    protected void linkFirst_Click(Object sender, EventArgs e)
    {
        grid.PageIndex = 0;
        //Session["PageNoLastEdited"] = "0";
        loadGrid();
        ddlPageOf.SelectedValue = "1";
        loadRecords();
    }

    protected void linkPrev_Click(Object sender, EventArgs e)
    {
        if (grid.PageIndex > 0)
        {
            grid.PageIndex = grid.PageIndex - 1;
            //Session["PageNoLastEdited"] = grid.PageIndex;
            loadGrid();
            ddlPageOf.SelectedValue = (grid.PageIndex + 1).ToString(); ;
            loadRecords();
        }
    }

    protected void linkNext_Click(Object sender, EventArgs e)
    {
        grid.PageIndex = grid.PageIndex + 1;
        //Session["PageNoLastEdited"] = grid.PageIndex;
        loadGrid();
        ddlPageOf.SelectedValue = (grid.PageIndex + 1).ToString(); ;
        loadRecords();
    }

    protected void linkLast_Click(Object sender, EventArgs e)
    {
        grid.PageIndex = grid.PageCount - 1;
        //Session["PageNoLastEdited"] = grid.PageIndex;
        loadGrid();
        ddlPageOf.SelectedValue = grid.PageCount.ToString();
        loadRecords();
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
        catch
        {
        }
    }

    protected void lbEdit_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Label lblTransNo = (Label)gridRow.FindControl("lblTransNo");
        Response.Redirect("~/Transaction/PurchaseOrderProcess.aspx?mode=edit&trans_no=" + lblTransNo.Text + "");
    }

    protected void lbDelete_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Label lblTransNo = (Label)gridRow.FindControl("lblTransNo");

        Session["PurchaseOrderMode"] = "delete";
        Session["PurchaseOrderTransNo"] = lblTransNo.Text;
        showConfirmBox("Delete " + lblTransNo.Text + "?");
    }

    protected void imgPrint_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Label lblTransNo = (Label)gridRow.FindControl("lblTransNo");

        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( 'PurchaseOrderPrint.aspx?transNo=" + lblTransNo.Text + "', null, 'width=750px,fullscreen=no,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes' );", true);
    }

    protected void imgEmail_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            GridViewRow gridRow = ((sender as ImageButton).Parent.Parent as GridViewRow);
            Label lblTransNo = (Label)gridRow.FindControl("lblTransNo");
            Session["PurchaseOrderMode"] = "sending";
            showConfirmBox("Send Purchase Order " + lblTransNo.Text.Trim() + "?");
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected string getURL()
    {
        string pageUrl = "";
        char[] delimiterChars = { '/' };
        pageUrl = System.Web.HttpContext.Current.Request.Url.ToString();
        string[] arrUrl = pageUrl.Split(delimiterChars);
        int length = arrUrl.Length;
        string strOut = "";
        for (int i = 0; i < length - 1; i++)
        {
            if (i == 0)
            {
                strOut += arrUrl[i] + "//";
            }
            else
            {
                if (i != 1)
                {
                    strOut += arrUrl[i] + "/";
                }
            }
        }
        return strOut;
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (wucConfirmBox1.StrMode == "Yes")
            {
                if (Session["PurchaseOrderMode"].ToString() == "delete")
                {
                    sql.Length = 0;
                    sql.Append("DELETE FROM purchase_order WHERE trans_no = '" + Session["PurchaseOrderTransNo"].ToString() + "' ");
                    sql.Append("DELETE FROM purchase_order_detail WHERE trans_no = '" + Session["PurchaseOrderTransNo"].ToString() + "' ");
                    sql.Append("DELETE FROM trans_approval WHERE trans_no = '" + Session["PurchaseOrderTransNo"].ToString() + "' ");

                    cAdih.executeNonQuery(sql.ToString(), cAdih.getConnStr("Connection").ToString());
                    master.messageBox("Data has been deleted");
                }
                loadGrid();
            }
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }
}