using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;

public partial class Transaction_BarangKeluarList : System.Web.UI.Page
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

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transaction/BarangKeluar.aspx?mode=add");
    }

    protected void loadGrid()
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT a.trans_no, a.trans_date, a.reff, a.reff_order_no, ");
            sql.Append("a.from_warehouse, f.wh_description, ");
            sql.Append("a.ship_to, c.customer_name, ");
            sql.Append("j.description AS movement_type_name, a.note, a.status ");
            sql.Append("FROM outbound_delivery a WITH(READPAST) ");
            sql.Append("INNER JOIN site_wh f WITH(READPAST) ON a.from_warehouse = f.wh_id ");
            sql.Append("INNER JOIN customer c WITH(READPAST) on c.customer_no = a.ship_to ");
            sql.Append("INNER JOIN movement_type j ON a.movement_type = j.id ");
            sql.Append("WHERE 1 = 1 ");
            if (ddlFilter.SelectedValue == "OPEN")
            {
                sql.Append("AND a.status = '0' ");
            }
            else if (ddlFilter.SelectedValue == "POSTED")
            {
                sql.Append("AND a.status = '1' ");
            }
            if (txtSearch.Text.Trim().Replace("'", "`") != "")
            {
                sql.Append("AND (a.trans_no LIKE '%" + txtSearch.Text + "%' OR a.reff_order_no LIKE '%" + txtSearch.Text + "%' OR a.reff LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR a.from_warehouse LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR d.customer_name LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR a.ship_to LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR j.description LIKE '%" + txtSearch.Text + "%' OR a.note LIKE '%" + txtSearch.Text + "%') ");
            }
            sql.Append("ORDER BY a.created_date DESC");

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
                LinkButton imgDelete = (LinkButton)Row.FindControl("lbDelete");
                Label lblDelete = (Label)Row.FindControl("lblDelete");

                if (lblStatus.Text == "False")
                {
                    lblStatus.Text = "HOLD";
                    imgDelete.Visible = true;
                    lblDelete.Visible = false;
                }
                else if (lblStatus.Text == "True")
                {
                    lblStatus.Text = "POSTED";
                    imgDelete.Visible = false;
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

    protected void imgEdit_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Label lblTransNo = (Label)gridRow.FindControl("lblTransNo");

        Response.Redirect("~/Transaction/BarangKeluar.aspx?mode=edit&trans_no=" + lblTransNo.Text + "");
    }

    protected void imgDelete_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as ImageButton).Parent.Parent as GridViewRow);
        Label lblTransNo = (Label)gridRow.FindControl("lblTransNo");

        Session["OutboundDeliveryMode"] = "delete";
        Session["OutboundDeliveryTransNo"] = lblTransNo.Text;
        showConfirmBox("Delete " + lblTransNo.Text + "?");
    }

    protected void imgPrint_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Label lblTransNo = (Label)gridRow.FindControl("lblTransNo");

        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open('OutboundDeliveryPrint.aspx?transNo=" + lblTransNo.Text + "', null, 'width=750px,fullscreen=no,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes' );", true);
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (wucConfirmBox1.StrMode == "Yes")
            {
                if (Session["OutboundDeliveryMode"].ToString() == "delete")
                {
                    sql.Length = 0;
                    sql.Append("DELETE FROM outbound_delivery WHERE trans_no = '" + Session["OutboundDeliveryTransNo"].ToString() + "' ");
                    sql.Append("DELETE FROM outbound_delivery_detail WHERE trans_no = '" + Session["OutboundDeliveryTransNo"].ToString() + "' ");

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