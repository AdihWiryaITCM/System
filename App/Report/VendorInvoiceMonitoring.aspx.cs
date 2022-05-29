using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Report_VendorInvoiceMonitoring : Page
{
    private StringBuilder sql = new StringBuilder();
    private SqlTransaction trans;
    private SqlConnection conn;
    private SqlCommand cmd;
    private SqlDataReader dr;
    private DataTable dt;
    private ClassAdih cAdih = new ClassAdih();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void bShow_Click(object sender, EventArgs e)
    {
        try
        {
            grid.Visible = false;
            gridDetail.Visible = false;
            if (cbDetail.Checked)
            {
                queryLoadGridDetail();
                gridDetail.Visible = true;
                cAdih.loadGridView(sql.ToString(), cAdih.getConnStr("Connection"), gridDetail);
            }
            else
            {
                queryLoadGrid();
                grid.Visible = true;
                cAdih.loadGridView(sql.ToString(), cAdih.getConnStr("Connection"), grid);
            }
            upGrid.Update();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void bExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (cbDetail.Checked)
            {
                queryLoadGridDetail();
            }
            else
            {
                queryLoadGrid();
            }
            Session["export_excel"] = cAdih.getResultDataSet(sql.ToString(), cAdih.getConnStr("Connection"));
            Session["excel_name"] = Session["company_id"].ToString() + "VendorInvoiceMonitoring";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( '../excel.aspx', null, 'fullscreen=yes,status=yes,toolbar=no,menubar=no,location=no' );", true);
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void ibSearchVendor_Click(object sender, EventArgs e)
    {
        Session["search"] = "vendor";
        wucSearch1.loadGrid();
    }

    protected void wucSearch_Hide(object sender, EventArgs e)
    {
        if (Session["search"].ToString() == "vendor")
        {
            tbVendor.Text = wucSearch1.result;
            upHeader.Update();
        }
        Session["search"] = null;
    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridViewRow Row = e.Row;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lBKNo = (Label)Row.FindControl("lBKNo");
                Label lBKDate = (Label)Row.FindControl("lBKDate");
                Label lBKAmount = (Label)Row.FindControl("lBKAmount");
                Label lBKUpdateBy = (Label)Row.FindControl("lBKUpdateBy");
                Label lBKUpdateDate = (Label)Row.FindControl("lBKUpdateDate");

                if (lBKNo.Text == "")
                {
                    lBKDate.Visible = false;
                    lBKAmount.Visible = false;
                    lBKUpdateBy.Visible = false;
                    lBKUpdateDate.Visible = false;
                }
                else
                {
                    lBKDate.Visible = true;
                    lBKAmount.Visible = true;
                    lBKUpdateBy.Visible = true;
                    lBKUpdateDate.Visible = true;
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

    protected string query_filter()
    {
        try
        {
            string filter = "";

            if (tbVendor.Text.Trim().Replace("'", "`") != "")
            {
                filter += "AND icc.vendor_no = '" + tbVendor.Text.Trim().Replace("'", "`").ToUpper() + "' ";
            }

            if (tbPurchaseOrderNo.Text.Trim().Replace("'", "`") != "")
            {
                filter += "AND icc.reff_order_no LIKE '%" + tbPurchaseOrderNo.Text.Trim().Replace("'", "`").ToUpper() + "%' ";
            }

            if (tbInboundDeliveryNo.Text.Trim().Replace("'", "`") != "")
            {
                filter += "AND iccd.inbound_delivery_no LIKE '%" + tbInboundDeliveryNo.Text.Trim().Replace("'", "`").ToUpper() + "%' ";
            }

            if (tbStartTransDate.Text.Trim() != "" && tbEndTransDate.Text.Trim() == "")
            {
                filter += "AND CONVERT(VARCHAR(10), icc.trans_date, 120) = '" + cAdih.engFormatDate(tbStartTransDate.Text) + "' ";
            }
            else if (tbStartTransDate.Text.Trim() == "" && tbEndTransDate.Text.Trim() != "")
            {
                filter += "AND CONVERT(VARCHAR(10), icc.trans_date, 120) = '" + cAdih.engFormatDate(tbEndTransDate.Text) + "' ";
            }
            else if (tbStartTransDate.Text.Trim() != "" && tbEndTransDate.Text.Trim() != "")
            {
                filter += "AND CONVERT(VARCHAR(10), icc.trans_date, 120) BETWEEN '" + cAdih.engFormatDate(tbStartTransDate.Text) + "' AND '" + cAdih.engFormatDate(tbEndTransDate.Text) + "' ";
            }

            if (tbStartInvoiceDate.Text.Trim() != "" && tbEndInvoiceDate.Text.Trim() == "")
            {
                filter += "AND CONVERT(VARCHAR(10), icc.posted_date, 120) = '" + cAdih.engFormatDate(tbStartInvoiceDate.Text) + "' ";
            }
            else if (tbStartInvoiceDate.Text.Trim() == "" && tbEndInvoiceDate.Text.Trim() != "")
            {
                filter += "AND CONVERT(VARCHAR(10), icc.posted_date, 120) = '" + cAdih.engFormatDate(tbEndInvoiceDate.Text) + "' ";
            }
            else if (tbStartInvoiceDate.Text.Trim() != "" && tbEndInvoiceDate.Text.Trim() != "")
            {
                filter += "AND CONVERT(VARCHAR(10), icc.posted_date, 120) BETWEEN '" + cAdih.engFormatDate(tbStartInvoiceDate.Text) + "' AND '" + cAdih.engFormatDate(tbEndInvoiceDate.Text) + "' ";
            }

            if (tbStartDueDate.Text.Trim() != "" && tbEndDueDate.Text.Trim() == "")
            {
                filter += "AND icc.due_date = '" + cAdih.engFormatDate(tbStartDueDate.Text) + "' ";
            }
            else if (tbStartDueDate.Text.Trim() == "" && tbEndDueDate.Text.Trim() != "")
            {
                filter += "AND icc.due_date = '" + cAdih.engFormatDate(tbEndDueDate.Text) + "' ";
            }
            else if (tbStartDueDate.Text.Trim() != "" && tbEndDueDate.Text.Trim() != "")
            {
                filter += "AND icc.due_date BETWEEN '" + cAdih.engFormatDate(tbStartDueDate.Text) + "' AND '" + cAdih.engFormatDate(tbEndDueDate.Text) + "' ";
            }

            if (tbStartBKDate.Text.Trim() != "" && tbEndBKDate.Text.Trim() == "")
            {
                filter += "AND ipt.payment_date = '" + cAdih.engFormatDate(tbStartBKDate.Text) + "' ";
            }
            else if (tbStartBKDate.Text.Trim() == "" && tbEndBKDate.Text.Trim() != "")
            {
                filter += "AND ipt.payment_date = '" + cAdih.engFormatDate(tbEndBKDate.Text) + "' ";
            }
            else if (tbStartBKDate.Text.Trim() != "" && tbEndBKDate.Text.Trim() != "")
            {
                filter += "AND ipt.payment_date BETWEEN '" + cAdih.engFormatDate(tbStartBKDate.Text) + "' AND '" + cAdih.engFormatDate(tbEndBKDate.Text) + "' ";
            }

            if (ddlStatus.SelectedValue != "%")
            {
                filter += "AND icc.status = '" + ddlStatus.SelectedValue + "'";
            }

            if (ddlInvStatus.SelectedValue == "1")
            {
                filter += "AND icc.total_amount - (isnull((select sum (pd.amount_paid) from purchase_payment_detail pd where pd.reff_no = icc.trans_no ),0 ) + isnull((select sum (pvd.disc_amount) from purchase_payment_detail pvd where pvd.reff_no = icc.trans_no),0)) != 0 ";
            }
            else if (ddlInvStatus.SelectedValue == "0")
            {
                filter += "AND icc.total_amount - (isnull((select sum (pd.amount_paid) from purchase_payment_detail pd where pd.reff_no = icc.trans_no ),0 ) + isnull((select sum (pvd.disc_amount) from purchase_payment_detail pvd where pvd.reff_no = icc.trans_no),0)) = 0 ";
            }

            return filter;
        }
        catch
        {
            return "";
        }
    }

    protected void queryLoadGrid()
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT DISTINCT icc.trans_no, icc.trans_date, id.posted_date, icc.reff_order_no, iccd.inbound_delivery_no, ");
            sql.Append("icc.vendor_no, v.vendor_name, CAST(CONVERT(VARCHAR(10), icc.posted_date, 120) AS DATETIME) AS invoice_date, icc.tax_facture_no, icc.vendor_invoice_no, ");
            sql.Append("ISNULL(s.wh_description, s.wh_description) AS ship_to, ISNULL(ca.alias_name_full + ' (' + ca.alias_name + ')', '') AS ship_to_site, ");
            sql.Append("'IDR' as currency_id, sum(iccd.unit_price * iccd.qty_received) 'DPP_Amount', sum(iccd.unit_tax * iccd.qty_received) 'TAX', icc.total_amount, icc.due_date, icc.note, ISNULL(up.username, icc.posted_by) AS received_by, icc.posted_date, ");
            sql.Append("ISNULL(ipt.voucher_no, '') AS bk_no, ISNULL(ipt.payment_date, 0) AS bk_date, ISNULL(ipt.payment_amount, 0) AS bk_amount, ");
            sql.Append("ISNULL(ub.username, '') AS bk_update_by, ISNULL(ipt.create_date, 0) AS bk_update_date, id.posted_date, ISNULL(uc.username, '') created_by, ");
            sql.Append("icc.total_amount - (isnull((select sum(pd.amount_paid) from purchase_payment_detail pd where pd.reff_no = icc.trans_no ), 0) + isnull((select sum(pvd.disc_amount) from purchase_payment_detail pvd where pvd.reff_no = icc.trans_no), 0)) as outstanding ");
            sql.Append("FROM invoice_confirmation_receipt icc WITH(READPAST) ");
            sql.Append("INNER JOIN invoice_confirmation_receipt_detail iccd WITH(READPAST) ON icc.trans_no = iccd.trans_no ");
            sql.Append("INNER JOIN vendor v WITH(READPAST) ON icc.vendor_no = v.vendor_no ");
            sql.Append("INNER JOIN purchase_order po WITH(READPAST) ON icc.reff_order_no = po.trans_no ");
            sql.Append("LEFT JOIN site_wh s WITH(READPAST) ON po.ship_to = s.wh_id ");
            sql.Append("LEFT JOIN customer_address ca WITH(READPAST) ON po.ship_to = CAST(ca.id AS VARCHAR) ");
            sql.Append("left join( ");
            sql.Append("                select icr.vendor_invoice_no, icrpp.voucher_no, icrpp.payment_date, icrpp.payment_amount, icrpp.create_date, icrpp.create_by ");
            sql.Append("                from invoice_confirmation_receipt_partial_payment icrpp ");
            sql.Append("                inner join invoice_confirmation_receipt icr on (icr.trans_no = icrpp.trans_no) ");
            sql.Append("			) ipt on(icc.vendor_invoice_no = ipt.vendor_invoice_no) ");
            sql.Append("LEFT JOIN[user] up WITH(READPAST) ON icc.posted_by = up.user_id ");
            sql.Append("LEFT JOIN[user] uc WITH(READPAST) ON icc.created_by = uc.user_id ");
            sql.Append("LEFT JOIN[user] ub WITH(READPAST) ON ipt.create_by = ub.user_id ");
            sql.Append("LEFT JOIN inbound_delivery id WITH(READPAST) ON iccd.inbound_delivery_no = id.trans_no ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append(query_filter());
            sql.Append("group by icc.trans_no, icc.trans_date, id.posted_date, icc.reff_order_no, iccd.inbound_delivery_no, s.wh_description, ");
            sql.Append("ca.alias_name_full, ca.alias_name, icc.vendor_no, v.vendor_name, icc.vendor_invoice_no, icc.posted_date, icc.tax_facture_no, ");
            sql.Append("icc.total_amount, icc.due_date, icc.note, up.username, icc.posted_by, icc.posted_date, ");
            sql.Append("ipt.voucher_no, ipt.payment_date, ipt.payment_amount, ub.username, ipt.create_date, uc.username  ");


        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void queryLoadGridDetail()
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT icc.trans_no, icc.trans_date, id.posted_date, icc.reff_order_no, ");
            sql.Append("iccd.inbound_delivery_no, icc.vendor_no, v.vendor_name, CAST(CONVERT(VARCHAR(10), icc.posted_date, 120) AS DATETIME) AS invoice_date, ");
            sql.Append("icc.tax_facture_no, icc.vendor_invoice_no, ISNULL(s.wh_description, s.wh_description) AS ship_to, ISNULL(ca.alias_name_full + ' (' + ca.alias_name + ')', '') AS ship_to_site, ");
            sql.Append("'IDR' currency_id, sum(iccd.unit_price * iccd.qty_received) 'dpp_Amount', sum(iccd.unit_tax * iccd.qty_received)'TAX', (sum(iccd.unit_price * iccd.qty_received)) + (sum(iccd.unit_tax * iccd.qty_received)) 'total_amount', ");
            sql.Append("icc.due_date, icc.note, ISNULL(uc.username, '') created_by, ISNULL(up.username, icc.posted_by) AS posted_by, icc.posted_date, ");
            sql.Append("ISNULL(ipt.voucher_no, '') AS bk_no, ISNULL(ipt.payment_date, 0) AS bk_date, ISNULL(ipt.payment_amount, 0) AS bk_amount, ");
            sql.Append("ISNULL(ub.username, '') AS bk_update_by, ISNULL(ipt.create_date, 0) AS bk_update_date, ");
            sql.Append("iccd.article_no, a.article_description AS article_description, a.base_uom as unit_id, u.description AS unit_name, ");
            sql.Append("iccd.qty_received, iccd.unit_price, iccd.qty_received * iccd.unit_price AS amount, iccd.note AS note_detail, id.posted_date as id_posted_date, ");
            sql.Append("icc.total_amount - (isnull((select sum(pd.amount_paid) from purchase_payment_detail pd where pd.reff_no = icc.trans_no ), 0) + isnull((select sum(pvd.disc_amount) from purchase_payment_detail pvd where pvd.reff_no = icc.trans_no), 0)) as outstanding ");
            sql.Append("FROM invoice_confirmation_receipt icc WITH(READPAST) ");
            sql.Append("INNER JOIN vendor v WITH(READPAST) ON icc.vendor_no = v.vendor_no ");
            sql.Append("INNER JOIN purchase_order po WITH(READPAST) ON icc.reff_order_no = po.trans_no ");
            sql.Append("INNER JOIN invoice_confirmation_receipt_detail iccd WITH(READPAST) ON icc.trans_no = iccd.trans_no ");
            sql.Append("INNER JOIN article a WITH(READPAST) ON iccd.article_no = a.article_no ");
            sql.Append("INNER JOIN uom u WITH(READPAST) ON a.base_uom = u.id ");
            sql.Append("LEFT JOIN site_wh s WITH(READPAST) ON po.ship_to = s.wh_id ");
            sql.Append("LEFT JOIN customer_address ca WITH(READPAST) ON po.ship_to = CAST(ca.id AS VARCHAR) ");
            sql.Append("left join( ");
            sql.Append("                select icr.vendor_invoice_no, icrpp.voucher_no, icrpp.payment_date, icrpp.payment_amount, icrpp.create_date, icrpp.create_by ");
            sql.Append("                from invoice_confirmation_receipt_partial_payment icrpp ");
            sql.Append("                inner join invoice_confirmation_receipt icr on (icr.trans_no = icrpp.trans_no)  ");
            sql.Append("			) ipt on(icc.vendor_invoice_no = ipt.vendor_invoice_no) ");
            sql.Append("LEFT JOIN[user] up WITH(READPAST) ON icc.posted_by = up.user_id ");
            sql.Append("LEFT JOIN[user] uc WITH(READPAST) ON icc.created_by = uc.user_id ");
            sql.Append("LEFT JOIN[user] ub WITH(READPAST) ON ipt.create_by = ub.user_id ");
            sql.Append("LEFT JOIN inbound_delivery id WITH(READPAST) ON iccd.inbound_delivery_no = id.trans_no ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append(query_filter());
            sql.Append("group by icc.trans_no, icc.trans_date, id.posted_date, icc.reff_order_no, s.wh_description, ca.alias_name_full, ca.alias_name, icc.vendor_no, ");
            sql.Append("v.vendor_name, icc.vendor_invoice_no, icc.posted_date, icc.tax_facture_no, icc.total_amount, icc.due_date, icc.note, ");
            sql.Append("up.username, icc.posted_by, icc.posted_date, ipt.voucher_no, ipt.payment_date, ipt.payment_amount, ub.username, ipt.create_date, iccd.inbound_delivery_no, ");
            sql.Append("iccd.article_no, a.article_description, a.base_uom, u.description, iccd.qty_received, iccd.unit_price, iccd.note, id.posted_date, uc.username ");
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }
}