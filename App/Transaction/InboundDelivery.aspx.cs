using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;
using System.Data;

public partial class Transaction_InboundDelivery : System.Web.UI.Page
{
    SqlConnection conn;
    SqlCommand cmd;
    SqlDataReader dr;
    StringBuilder sql = new StringBuilder();
    ClassAdih cAdih = new ClassAdih();

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
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transaction/InboundDeliveryProcess.aspx?mode=add");
    }

    protected void loadGrid()
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT TOP 100 a.trans_no, a.trans_date, a.reff_order_no, a.reff, ");
            sql.Append("a.from_site, c.vendor_name, a.ship_to, f.wh_description, j.description AS movement_type_name, a.note, a.status ");
            sql.Append("FROM inbound_delivery a WITH(READPAST) ");
            sql.Append("INNER JOIN vendor c WITH(READPAST) ON a.from_site = c.vendor_no ");
            sql.Append("INNER JOIN site_wh f WITH(READPAST) ON a.ship_to = f.wh_id ");
            sql.Append("INNER JOIN movement_type j ON a.movement_type = j.id ");
            sql.Append("WHERE 1 = 1 ");
            if (ddlFilter.SelectedValue != "%") {
                if (ddlFilter.SelectedValue == "3")
                {
                    sql.Append("AND a.is_reject = '1' ");
                }
                else
                {
                    sql.Append("AND a.status = '" + ddlFilter.SelectedValue + "' ");
                }
            } 
            if (txtSearch.Text.Trim().Replace("'", "`") != "")
            {
                sql.Append("(AND a.trans_no LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR c.vendor_name LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR a.note LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR a.reff_order_no LIKE '%" + txtSearch.Text + "%') ");
            }
            sql.Append("ORDER BY a.created_date DESC ");

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
                Label lblDelete = (Label)Row.FindControl("lblDelete");
                LinkButton lbDelete = (LinkButton)Row.FindControl("lbDelete"); 
                if (lblStatus.Text == "False")
                {
                    lbDelete.Visible = true;
                    lblDelete.Visible = false;
                    lblStatus.Text = "HOLD";
                }
                else if (lblStatus.Text == "True")
                {
                    lbDelete.Visible = false;
                    lblDelete.Visible = true;
                    lblStatus.Text = "POSTED";
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

        Response.Redirect("~/Transaction/InboundDeliveryProcess.aspx?mode=edit&trans_no=" + lblTransNo.Text + "");
    }

    protected void lbDelete_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Label lblTransNo = (Label)gridRow.FindControl("lblTransNo");

        Session["InboundDeliveryMode"] = "delete";
        Session["InboundDeliveryTransNo"] = lblTransNo.Text;
        showConfirmBox("Delete " + lblTransNo.Text + "?");
    }

    protected void imgPrint_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Label lblTransNo = (Label)gridRow.FindControl("lblTransNo");

        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( 'InboundDeliveryPrint.aspx?transNo=" + lblTransNo.Text + "', null, 'width=750px,fullscreen=no,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes' );", true);
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (wucConfirmBox1.StrMode == "Yes")
            {
                if (Session["InboundDeliveryMode"].ToString() == "delete")
                {
                    sql.Length = 0;
                    sql.Append("DELETE FROM inbound_delivery_detail WHERE trans_no = '" + Session["InboundDeliveryTransNo"].ToString() + "' ");
                    sql.Append("DELETE FROM inbound_delivery WHERE trans_no = '" + Session["InboundDeliveryTransNo"].ToString() + "' ");

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