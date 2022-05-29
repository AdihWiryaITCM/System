using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transaction_PurchasePaymentList : Page
{
    private ClassAdih cAdih = new ClassAdih();
    private StringBuilder sql = new StringBuilder();

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
            sql.Append("select	trans_no as pv_no, ");
            sql.Append("trans_date as pv_date, ");
            sql.Append("ph.status, ");
            sql.Append("payment_date, ");
            sql.Append("ph.vendor_no, ");
            sql.Append(" '' as bank_code, ");
            sql.Append("note, ");
            sql.Append("isnull(admin_fee, 0) 'admin_fee', ");
            sql.Append("isnull(base_amount, 0) 'amount', ");
            sql.Append("isnull(orig_amount, 0) 'orig_amount', ");
            sql.Append("ve.vendor_name 'vendor_name' ");
            sql.Append("from dbo.purchase_payment ph ");
            sql.Append("INNER JOIN dbo.vendor ve on(ve.vendor_no = ph.vendor_no) ");
            sql.Append("WHERE 1 = 1 ");
            if (ddlStatus.SelectedValue != "ALL")
            {
                sql.Append("AND ph.status = '" + ddlStatus.SelectedValue + "' ");
            }
            if (tbSearch.Text.Trim().Replace("'", "`") != "")
            {
                sql.Append("AND (pv_no										like '%" + tbSearch.Text.Trim() + "%' ");
                sql.Append("or  convert(nvarchar(10), pv_date, 105)         like '%" + tbSearch.Text.Trim() + "%' ");
                sql.Append("or  convert(nvarchar(10), payment_date, 105)    like '%" + tbSearch.Text.Trim() + "%' ");
                sql.Append("or  ph.vendor_no                                like '%" + tbSearch.Text.Trim() + "%' ");
                sql.Append("or  note                                        like '%" + tbSearch.Text.Trim() + "%' ");
                sql.Append("or  ve.vendor_name                              like '%" + tbSearch.Text.Trim() + "%' ) ");
            }
            sql.Append("ORDER BY ph.created_date desc");

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
        MasterPage master = (MasterPage)this.Master;
        sql.Length = 0;
        sql.Append("DELETE FROM [purchase_payment] WHERE trans_no = '" + Session["InvoiceConfirmationTransNo"].ToString() + "' ");
        sql.Append("DELETE FROM [purchase_payment_detail] WHERE trans_no = '" + Session["InvoiceConfirmationTransNo"].ToString() + "' ");

        cAdih.executeNonQuery(sql.ToString(), cAdih.getConnStr("Connection").ToString());
        master.messageBox("Data has been deleted");
    }

    protected void showConfirmBox(string message)
    {
        wucConfirmBox1.SubShowConfirmBox(message);
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("PurchasePayment.aspx?action=add");
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

        string sPVNo = grid.DataKeys[gridRow.RowIndex][0].ToString();

        Session["PVHeaderMode"] = "delete";
        Session["PVHeaderPVNo"] = sPVNo;

        showConfirmBox("Delete " + sPVNo + "?");
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (wucConfirmBox1.StrMode == "Yes")
            {
                if (Session["PVHeaderMode"].ToString() == "delete")
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

        Response.Redirect(string.Format("PurchasePayment.aspx?action=edit&pvno={0}", grid.DataKeys[gridRow.RowIndex][0].ToString()));
    }

    protected void imgPrint_Click(object sender, EventArgs e)
    {
        GridView grid = ((sender as ImageButton).Parent.Parent.Parent.Parent as GridView);
        GridViewRow gridRow = ((sender as ImageButton).Parent.Parent as GridViewRow);

        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( 'pvprint.aspx?pvno=" + grid.DataKeys[gridRow.RowIndex][0].ToString() + "&type=PV" + "', null, 'width=750px,fullscreen=no,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes' );", true);
    }

    protected void imgPrintBuktiTransfer_Click(object sender, EventArgs e)
    {
        GridView grid = ((sender as ImageButton).Parent.Parent.Parent.Parent as GridView);
        GridViewRow gridRow = ((sender as ImageButton).Parent.Parent as GridViewRow);

        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( 'pvprintbuktitransfer.aspx?pvno=" + grid.DataKeys[gridRow.RowIndex][0].ToString() + "', null, 'width=750px,fullscreen=no,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes' );", true);
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
}