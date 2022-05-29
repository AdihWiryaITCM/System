using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;
using System.Data;


public partial class Transaction_PermintaanPembelianBarangList : System.Web.UI.Page
{
    SqlConnection conn;
    SqlCommand cmd;
    SqlDataReader dr;
    StringBuilder sql = new StringBuilder();
    ClassAdih cAdih = new ClassAdih();
    DataTable dt;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            loadGrid();
            loadDdlPageOf();
            loadRecords();
        }
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
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transaction/PurchaseRequisition.aspx?action=add");
    }

    protected void loadGrid()
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT a.trans_no, a.trans_date,b.wh_description, a.total_amount,a.note,a.status ");
            sql.Append("FROM purchase_requisition a WITH(READPAST) ");
            sql.Append("LEFT JOIN site_wh b WITH(READPAST) ON a.ship_to = b.wh_id ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append("AND a.status = case when '" + ddlFilter.SelectedValue + "' = '%' THEN a.status ELSE '" + ddlFilter.SelectedValue + "' END ");
            if (txtSearch.Text.Trim().Replace("'", "`") != "")
            {
                sql.Append("AND (a.trans_no LIKE '%" + txtSearch.Text + "%' OR a.note LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR b.wh_description LIKE '%" + txtSearch.Text + "%'");
           }
            sql.Append("ORDER BY a.create_date DESC");

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
                Label lblStatus = (Label)Row.FindControl("lblStatus");
                LinkButton lbEdit = (LinkButton)Row.FindControl("lbEdit");
                Label lblEdit = (Label)Row.FindControl("lblEdit");
                LinkButton lbDelete = (LinkButton)Row.FindControl("lbDelete");
                Label lblDelete = (Label)Row.FindControl("lblDelete");

                if (lblStatus.Text == "HOLD")
                {
                    lbDelete.Visible = true;
                    lblDelete.Visible = false;
                }
                else
                {
                    lbDelete.Visible = false;
                    lblDelete.Visible = true;
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
        String FirstRowNo = (((grid.PageIndex) * grid.PageSize) + 1).ToString();
        if (grid.Rows.Count == 0)
        {
            FirstRowNo = "0";
        }
        String LastRowNo = ((grid.PageSize * grid.PageIndex) + (grid.Rows.Count)).ToString();
        lblRecords.Text = "Records: " + FirstRowNo + "-" + LastRowNo + "";
        upFooter.Update();
    }

    protected void lbEdit_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Label lblTransNo = (Label)gridRow.FindControl("lblTransNo");

        Response.Redirect("~/Transaction/PurchaseRequisition.aspx?action=edit&trans_no=" + lblTransNo.Text + "");
    }

    protected void lbDelete_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Label lblTransNo = (Label)gridRow.FindControl("lblTransNo");

        Session["ConfirmMode"] = "delete";
        Session["TransNo"] = lblTransNo.Text;
        showConfirmBox("Delete " + lblTransNo.Text + "?");
    }

    protected void showConfirmBox(string message)
    {
        wucConfirmBox1.SubShowConfirmBox(message);
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (wucConfirmBox1.StrMode == "Yes")
            {
                if (Session["ConfirmMode"].ToString() == "delete")
                {
                    sql.Length = 0;
                    sql.Append("DELETE FROM purchase_requisition WHERE trans_no = '" + Session["TransNo"].ToString() + "' ");
                    sql.Append("DELETE FROM purchase_requisition_detail WHERE trans_no = '" + Session["TransNo"].ToString() + "' ");

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

    protected void grid_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}