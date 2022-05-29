using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;
using System.Data;

public partial class Transaction_PurchaseInvoice : System.Web.UI.Page
{
    SqlConnection conn;
    SqlCommand cmd;
    SqlDataReader dr;
    DataTable dt;
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
        Response.Redirect("~/Transaction/PurchaseInvoiceProcess.aspx?mode=add");
    }

    protected void loadGrid()
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT a.trans_no, a.trans_date, a.vendor_no, c.vendor_name, a.reff_order_no AS po_trans_no, ");
            sql.Append("a.vendor_invoice_no, a.tax_facture_no, a.note, a.status ");
            sql.Append("FROM invoice_confirmation_receipt a WITH(READPAST) ");
            sql.Append("INNER JOIN purchase_order b WITH(READPAST) ON a.reff_order_no = b.trans_no ");
            sql.Append("INNER JOIN vendor c WITH(READPAST) ON a.vendor_no = c.vendor_no ");
            sql.Append("WHERE 1 = 1 ");
            if (ddlFilter.SelectedValue != "") 
            {
                sql.Append("AND a.status = '" + ddlFilter.SelectedValue + "' ");
            }
            if (txtSearch.Text.Trim().Replace("'", "`") != "")
            {
                sql.Append("AND (a.trans_no LIKE '%" + txtSearch.Text + "%' OR a.vendor_no LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR c.vendor_name LIKE '%" + txtSearch.Text + "%' OR a.reff_order_no LIKE '%" + txtSearch.Text + "%' ");
                sql.Append("OR a.vendor_invoice_no LIKE '%" + txtSearch.Text + "%' OR a.tax_facture_no LIKE '%" + txtSearch.Text + "%' OR a.note LIKE '%" + txtSearch.Text + "%') ");
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
                CheckBox ChkCheck = (CheckBox)Row.FindControl("chkPosting");
                Image imgPosted = (Image)Row.FindControl("imgPosted");
                LinkButton imgEdit = (LinkButton)Row.FindControl("imgEdit");
                LinkButton imgDelete = (LinkButton)Row.FindControl("lbDelete");
                Label lbDelete = (Label)Row.FindControl("lblDelete");

                if (lblStatus.Text == "True")
                {
                    imgDelete.Visible = false;
                    lbDelete.Visible = true;
                    lblStatus.Text = "POSTED";
                }
                else
                {
                    imgDelete.Visible = true;
                    lbDelete.Visible = false;
                    lblStatus.Text = "HOLD";
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

        Response.Redirect("~/Transaction/PurchaseInvoiceProcess.aspx?mode=edit&trans_no=" + lblTransNo.Text + "");
    }
    protected void lbDelete_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Label lblTransNo = (Label)gridRow.FindControl("lblTransNo");

        Session["InvoiceConfirmationReceiptMode"] = "delete";
        Session["InvoiceConfirmationTransNo"] = lblTransNo.Text;
        showConfirmBox("Delete " + lblTransNo.Text + "?");
    }

    protected void lbPrint_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Label lblTransNo = (Label)gridRow.FindControl("lblTransNo");

        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open('InvoiceConfirmationReceiptPrint.aspx?transNo=" + lblTransNo.Text + "', null, 'width=750px,fullscreen=no,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes' );", true);
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (wucConfirmBox1.StrMode == "Yes")
            {
                if (Session["InvoiceConfirmationReceiptMode"].ToString() == "delete")
                {
                    sql.Length = 0;
                    sql.Append("DELETE FROM invoice_confirmation_receipt WHERE trans_no = '" + Session["InvoiceConfirmationTransNo"].ToString() + "' ");
                    sql.Append("DELETE FROM invoice_confirmation_receipt_detail WHERE trans_no = '" + Session["InvoiceConfirmationTransNo"].ToString() + "' ");

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