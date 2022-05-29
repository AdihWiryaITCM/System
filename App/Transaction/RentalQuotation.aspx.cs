using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;
using System.Data;
public partial class Transaction_RentalQuotation : System.Web.UI.Page
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
        //Session["RentalOrderMode"] = "add";
        //Session["ServiceCallBeforePage"] = "~/Transaction/ServiceCall.aspx";
        Response.Redirect("~/Transaction/RentalQuotationProcess.aspx?mode=add");
    }

    protected void loadGrid()
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT a.trans_no, a.trans_date, a.sold_to,a.bill_to,a.ship_to, c.customer_name AS sold_to_name,");
            sql.Append("a.status, a.created_date, a.total_amount, a.note, ");
            sql.Append("a.bill_to, (CASE WHEN d.alias_name = '' THEN d.alias_name_full ELSE d.alias_name_full + ' ' + d.alias_name END) AS bill_to_name, ");
            sql.Append("a.ship_to, (CASE WHEN e.alias_name = '' THEN e.alias_name_full ELSE e.alias_name_full + ' ' + e.alias_name END) AS ship_to_name ");
            sql.Append("FROM quotation_rental a WITH(READPAST) INNER JOIN customer c WITH(READPAST) ON( a.sold_to = c.customer_no)");
            sql.Append("INNER JOIN customer_address d WITH(READPAST) ON a.bill_to = d.id AND d.customer_no = a.sold_to AND d.address_type = '01' ");
            sql.Append("INNER JOIN customer_address e WITH(READPAST) ON a.ship_to = e.id AND e.customer_no = a.sold_to AND e.address_type = '02' ");
	  
            sql.Append("WHERE 1 = 1 ");
            if (ddlFilter.SelectedValue == "HOLD")
            {
                sql.Append("AND a.status = '0' ");
            }
            else if (ddlFilter.SelectedValue == "POSTED")
            {
                sql.Append("AND a.status = '1' ");
            }
            if (txtSearch.Text.Trim().Replace("'", "`") != "")
            {
                sql.Append("AND (a.trans_no LIKE '%" + txtSearch.Text + "%' OR a.sold_to LIKE '%" + txtSearch.Text + "%' OR c.customer_name LIKE '%" + txtSearch.Text + "%' OR rot.description LIKE '%" + txtSearch.Text + "%' ");
                sql.Append(" OR a.note LIKE '%" + txtSearch.Text + "%' or e.alias_name_full like '%" + txtSearch.Text + "%' or e.alias_name like '%" + txtSearch.Text + "%' or d.alias_name_full like '%" + txtSearch.Text + "%' or d.alias_name like '%" + txtSearch.Text + "%' ) ");
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
                LinkButton imgEdit = (LinkButton)Row.FindControl("lbEdit");
                LinkButton imgDelete = (LinkButton)Row.FindControl("lbDelete");
                Label lblDelete = (Label)Row.FindControl("lblDelete");

                if (lblStatus.Text == "0")
                {
                    lblStatus.Text = "HOLD";
                    imgDelete.Visible = true;
                    lblDelete.Visible = false;
                }
                else if (lblStatus.Text == "1")
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

    protected void imgEdit_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Label lblTransNo = (Label)gridRow.FindControl("lblTransNo");

        Response.Redirect("~/Transaction/RentalQuotationProcess.aspx?mode=edit&trans_no=" + lblTransNo.Text + "");
    }

    protected void imgDelete_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Label lblTransNo = (Label)gridRow.FindControl("lblTransNo");

        Session["RentalOrderMode"] = "delete";
        Session["RentalOrderTransNo"] = lblTransNo.Text;
       

        showConfirmBox("Delete " + lblTransNo.Text + "?");
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (wucConfirmBox1.StrMode == "Yes")
            {
                if (Session["RentalOrderMode"].ToString() == "delete")
                {
                    sql.Length = 0;
                    sql.Append("DELETE FROM quotation_rental WHERE trans_no = '" + Session["RentalOrderTransNo"].ToString() + "' ");
                    sql.Append("DELETE FROM quotation_rental_detail WHERE trans_no = '" + Session["RentalOrderTransNo"].ToString() + "' ");

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