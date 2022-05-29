using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
public partial class Transaction_SalesInvoiceList : Page
{
    StringBuilder sql = new StringBuilder();
    ClassAdih cAdih = new ClassAdih();

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
        try
        {
            sql.Length = 0;
            sql.Append("select	si.trans_no "); 
            sql.Append(", si.trans_date ");
            sql.Append(", si.reff_order_no ");
            sql.Append(", si.tax_facture_no ");
            sql.Append(", c.customer_name ");
            sql.Append(", cab.alias_name_full as bill_to ");
            sql.Append(", si.note ");
            sql.Append(", si.total_amount ");
            sql.Append(", si.status ");
            sql.Append(", od.reff as od_reff ");
            sql.Append("from sales_invoice si with(readpast) ");
            sql.Append("inner join customer c with(readpast) on si.customer = c.customer_no ");
            sql.Append("inner join customer_address cab with(readpast) on si.customer = cab.customer_no and cab.address_type = '01' and cab.id = si.bill_to ");
            sql.Append("left join( ");
            sql.Append("                select od.trans_no, od.reff ");
            sql.Append("                from outbound_delivery od ");
            sql.Append("            ) od on(od.trans_no = si.reff_order_no) ");
            sql.Append("where   1 = 1 ");
            if (ddlStatus.SelectedValue != "")
            {
                sql.Append("AND si.status = '" + ddlStatus.SelectedValue + "' ");
            }
            if (tbSearch.Text.Trim().Replace("'", "`") != "")
            {
                sql.Append("and		(	");
                sql.Append("    si.trans_no                         like '%" + tbSearch.Text + "%' ");
                sql.Append("or  si.trans_date                       like '%" + tbSearch.Text + "%' ");
                sql.Append("or  si.reff_order_no                    like '%" + tbSearch.Text + "%' ");
                sql.Append("or  si.sales_order_no                   like '%" + tbSearch.Text + "%' ");
                sql.Append("or  c.customer_name                     like '%" + tbSearch.Text + "%' ");
                sql.Append("or  cab.alias_name_full                 like '%" + tbSearch.Text + "%' ");
                sql.Append("or  si.note                             like '%" + tbSearch.Text + "%' ");
                sql.Append("or  si.total_amount                     like '%" + tbSearch.Text + "%' ");
            }
            sql.Append("ORDER BY si.created_date desc");

            cAdih.loadGridView(sql.ToString(), cAdih.getConnStr("Connection"), grid);
            upGrid.Update();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message + "<br />" + ex.InnerException);
        }

    }

    private void DeleteData()
    {
        try
        {
            sql.Length = 0;
            sql.Append("DELETE FROM sales_invoice WHERE trans_no = '" + Session["SalesInvoiceTransNo"].ToString() + "' ");
            sql.Append("DELETE FROM sales_invoice_detail WHERE trans_no = '" + Session["SalesInvoiceTransNo"].ToString() + "' ");

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

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("SalesInvoiceProcess.aspx?action=add");
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

        string sTransNo = grid.DataKeys[gridRow.RowIndex][0].ToString();

        Session["SalesInvoiceMode"] = "delete";
        Session["SalesInvoiceTransNo"] = sTransNo;
        
        showConfirmBox("Delete " + sTransNo + "?");
    }

    protected void lbEdit_Click(object sender, EventArgs e)
    {
        GridView grid = ((sender as LinkButton).Parent.Parent.Parent.Parent as GridView);
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Response.Redirect(string.Format("SalesInvoiceProcess.aspx?action=edit&transno={0}", grid.DataKeys[gridRow.RowIndex][0].ToString()));
    }

    protected void imgPrint_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
        Label lblTransNo = (Label)gridRow.FindControl("lblTransNo");
        Label lblODReff = (Label)gridRow.FindControl("lblODReff");

        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( 'SalesInvoicePrint.aspx?transNo=" + lblTransNo.Text + "&type=SI" + "&mode=" + lblODReff.Text  +"', null, 'width=750px,fullscreen=no,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes' );", true);
    }

    protected void ddlStatus_OnSelectedIndexChanged(object sender, EventArgs e)
    { 
        BindData();
        loadDdlPageOf();
        loadRecords();
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (wucConfirmBox1.StrMode == "Yes")
            {
                if (Session["SalesInvoiceMode"].ToString() == "delete")
                {
                    DeleteData();
                    master.messageBox("Data has been deleted");
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
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
        BindData();
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
}