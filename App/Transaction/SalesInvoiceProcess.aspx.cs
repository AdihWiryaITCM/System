using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transaction_SalesInvoiceDetail : Page
{
    private static string TABLE_NAME = "sales_invoice";
    private static string TABLE_NAME_DETAIL = "sales_invoice_detail";
    private static string TABLE_NAME_UPLOAD = "sales_invoice_upload";
    private static string TABLE_NAME_CHECK = "sales_invoice_check";

    private StringBuilder sql = new StringBuilder();
    private SqlTransaction trans;
    private SqlConnection conn;
    private SqlCommand cmd;
    private SqlDataReader dr;
    private DataTable dt;
    private ClassAdih cAdih = new ClassAdih();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            grid.Columns[7].Visible = false;
            grid.Columns[15].Visible = false;
            txtTransDate.Enabled = false;

            if (Request.Params["action"].Equals("edit"))
            {
                Session["SaveMode"] = "edit";

                btnCancel.Text = "Back";
                LoadData();
                BindData();

                if (lblStatus.Text == "POSTED")
                {
                    btnSave.Visible = false;
                    btnPost.Visible = false;

                    pnlHeader.Enabled = false;
                    pnlDetail.Enabled = false;
                }
                else
                {
                    btnPost.Visible = true;
                }
            }
            else
            {
                Session["SaveMode"] = "add";

                txtTransDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }
        }
    }

    private void LoadData()
    {
        try
        {
            conn = new SqlConnection(cAdih.getConnStr("Connection"));
            conn.Open();
            sql.Length = 0;
            sql.Append("exec [dbo].[sp_sales_invoice_getrow] '"+ Request.Params["transno"].ToString() +"' ");

            using (cmd = new SqlCommand(sql.ToString(), conn))
            {
                using (dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        txtTransNo.Text = dr["TRANS_NO"].ToString();
                        txtTransDate.Text = Shared.DBtoUIDate(dr["TRANS_DATE"].ToString());
                        tbReffOrderNo.Text = dr["REFF_ORDER_NO"].ToString();
                        lbReffOrderNo.Text = dr["SALES_ORDER_NO"].ToString();
                        txtPONo.Text = dr["SALES_ORDER_NO"].ToString();
                        lSalesType.Text = dr["OD_REFF"].ToString();
                        libPONo.Text = "Cust PO No: " + dr["CUST_PO_NO"].ToString();
                        imbPONo.Visible = false;
                        tbReffOrderNo.ReadOnly = true;
                        ibSearchReffOrder.Visible = false;
                        txtCustomerNo.Text = dr["CUSTOMER"].ToString();
                        libCustomerName.Text = dr["CUSTOMER_NAME"].ToString();
                        txtCustomerBillTo.Text = dr["BILL_TO"].ToString();
                        libCustomerBillToName.Text = dr["ALIAS_NAME_FULL"].ToString();
                        txtCustomerShipTo.Text = dr["ship_too"].ToString();
                        libCustomerShipToName.Text = dr["SHIP_TO_NAME"].ToString();
                        txtCurrencyID.Text = dr["CURRENCY_ID"].ToString();
                        txtTotalInvoice.Text = double.Parse(dr["TOTAL_INVOICE"].ToString()).ToString("##,#0.#0");
                        txtPPN.Text = double.Parse(dr["PPN"].ToString()).ToString("##,#0.#0");
                        txtTotalBill.Text = double.Parse(dr["TOTAL_AMOUNT"].ToString()).ToString("##,#0.#0");
                        txtPaymentTerm.Text = dr["TERM_OF_PAYMENT"].ToString();
                        txtDueDate.Text = Shared.DBtoUIDate(dr["DUE_DATE"].ToString());
                        libPaymentTerm.Text = dr["TOP_DESC"].ToString();
                        txtTaxFactureNo.Text = dr["TAX_FACTURE_NO"].ToString();
                        txtDescription.Text = dr["street_address"].ToString();
                        lbVatReg.Text = dr["vat_reg_no"].ToString();

                        if (txtNote.ReadOnly == false)
                        {
                            txtNote.Text = dr["note"].ToString();
                        }
                        if (dr["status"].ToString() == "False")
                        {
                            lblStatus.Text = "OPEN";
                        }
                        else if (dr["status"].ToString() == "True")
                        {
                            lblStatus.Text = "POSTED";
                        }
                        else if (dr["od_reff"].ToString() == "RENTAL ORDER" || dr["od_reff"].ToString() == "SALES ORDER" || dr["od_reff"].ToString() == "RENTAL EXTENSION" || dr["od_reff"].ToString() == "RENTAL RETURN ORDER")
                        {
                            pnlPaymentTerm.Visible = true;
                            txtPaymentTerm.Text = dr["term_of_payment"].ToString();
                            libPaymentTerm.Text = dr["top_desc"].ToString(); ;
                            txtDueDate.Text = DateTime.Parse(dr["due_date"].ToString()).ToString("dd-MM-yyyy");

                            grid.Columns[10].Visible = false;
                            grid.Columns[12].Visible = false;
                        }
                    }
                }
            }
            conn.Close();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message + "<br />" + ex.InnerException);
        }
    }

    private void saveData()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            //CREATE NEW
            if (txtTransNo.Text == "")
            {
                conn = new SqlConnection(cAdih.getConnStr("Connection"));
                conn.Open();
                trans = conn.BeginTransaction();

                sql.Length = 0;
                sql.Append("SELECT RIGHT('000000' + CAST(ISNULL(MAX(CAST(RIGHT(trans_no, 6) AS INT)), 0) + 1 AS VARCHAR), 6) AS running_no, ");
                sql.Append("YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) AS tahun, RIGHT('00' + CAST(MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) AS VARCHAR), 2) AS bulan ");
                sql.Append("FROM sales_invoice ");
                sql.Append("WHERE YEAR(trans_date) = YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) ");
                sql.Append("AND MONTH(trans_date) = MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    using (dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            txtTransNo.Text = "SI" + dr["tahun"].ToString() + dr["bulan"].ToString() + dr["running_no"].ToString();
                            upHeader.Update();
                        }
                    }
                }

                sql.Length = 0;
                sql.Append("INSERT INTO sales_invoice(trans_no, trans_date, reff_order_no, sales_order_no, customer, bill_to, term_of_payment, ");
                sql.Append("total_amount, note, status, created_by, created_date,total_invoice, ppn,tax_facture_no,duedate ) VALUES( ");
                sql.Append("'" + txtTransNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("CONVERT(VARCHAR(10), GETDATE(), 120), ");
                sql.Append("'" + tbReffOrderNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + lbReffOrderNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + txtCustomerNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + txtCustomerBillTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + txtPaymentTerm.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + double.Parse(txtTotalInvoice.Text) + "', ");
                sql.Append("'" + txtNote.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'0', ");
                sql.Append("'" + Session["username"].ToString() + "', ");
                sql.Append("GETDATE(), ");
                sql.Append("'" + double.Parse(txtTotalBill.Text) + "', ");
                sql.Append("'" + double.Parse(txtPPN.Text) + "', ");
                sql.Append("'" + txtTaxFactureNo.Text + "', ");
                sql.Append("'" + cAdih.engFormatDate(txtDueDate.Text) + "') ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                int line_no = 10;
                foreach (GridViewRow gridRow in grid.Rows)
                {
                    sql.Length = 0;
                    sql.Append("EXEC [dbo].[sp_sales_invoice_detail_insert] ");
                    sql.Append(" '" + txtTransNo.Text + "' ");
                    sql.Append(", '" + line_no + "' ");
                    sql.Append(", '" + ((Label)gridRow.FindControl("lArticleNo")).Text + "' ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("lQtyOrder")).Text) + "'  ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("lQtyDelivered")).Text) + "'  ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("lUnitPrice")).Text) + "'  ");
                    sql.Append(", '" + ((Label)gridRow.FindControl("lPeriod")).Text + "'  ");
                    sql.Append(", '" + ((Label)gridRow.FindControl("txtOutstanding")).Text + "'  ");
                    sql.Append(", '" + ((TextBox)gridRow.FindControl("txtBillDuration")).Text + "'  ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("tbPrice")).Text) + "'  ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("lblUnitTax")).Text) + "'  ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("lblTaxAmount")).Text) + "'  ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("tbDiscount")).Text) + "'  ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("tbNetAmount")).Text) + "'  ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("tbTotalAmount")).Text) + "'  ");
                    sql.Append(", '" + ((TextBox)gridRow.FindControl("tbNoteDetail")).Text + "'  ");

                    using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                    {
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }

                    line_no += 10;
                }
                line_no = 10;

                btnPost.Visible = true;
                trans.Commit();
                conn.Close();
                master.messageBox("Data has been saved. Trans No: " + txtTransNo.Text + "");
                UpdatePanel1.Update();
            }
            //EDIT
            else if (txtTransNo.Text != "")
            {
                conn = new SqlConnection(cAdih.getConnStr("Connection"));
                conn.Open();
                trans = conn.BeginTransaction();

                sql.Length = 0;
                sql.Append("UPDATE sales_invoice SET reff_order_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("sales_order_no = '" + lbReffOrderNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("customer = '" + txtCustomerNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("bill_to = '" + txtCustomerBillTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("term_of_payment = '" + txtPaymentTerm.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("total_amount = '" + double.Parse(txtTotalInvoice.Text) + "', ");
                sql.Append("note = '" + txtNote.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("last_modified_by = '" + Session["username"].ToString() + "', ");
                sql.Append("total_invoice = '" + double.Parse(txtTotalBill.Text) + "', ");
                sql.Append("ppn = '" + double.Parse(txtPPN.Text) + "', ");
                sql.Append("tax_facture_no = '" + txtTaxFactureNo.Text + "', ");
                sql.Append("duedate = '" + cAdih.engFormatDate(txtDueDate.Text) + "', ");
                sql.Append("last_modified_date = GETDATE() ");
                sql.Append("WHERE trans_no = '" + txtTransNo.Text + "' ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                sql.Length = 0;
                sql.Append("DELETE sales_invoice_detail WHERE trans_no = '" + txtTransNo.Text + "' ");
                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                int line_no = 10;
                
                foreach (GridViewRow gridRow in grid.Rows)
                {
                    sql.Length = 0;
                    sql.Append("EXEC [dbo].[sp_sales_invoice_detail_insert] ");
                    sql.Append(" '" + txtTransNo.Text + "' ");
                    sql.Append(", '" + line_no + "' ");
                    sql.Append(", '" + ((Label)gridRow.FindControl("lArticleNo")).Text + "' ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("lQtyOrder")).Text) + "'  ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("lQtyDelivered")).Text) + "'  ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("lUnitPrice")).Text) + "'  ");
                    sql.Append(", '" + ((Label)gridRow.FindControl("lPeriod")).Text + "'  ");
                    sql.Append(", '" + ((Label)gridRow.FindControl("txtOutstanding")).Text + "'  ");
                    sql.Append(", '" + ((TextBox)gridRow.FindControl("txtBillDuration")).Text + "'  ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("tbPrice")).Text) + "'  ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("lblUnitTax")).Text) + "'  ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("lblTaxAmount")).Text) + "'  ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("tbDiscount")).Text) + "'  ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("tbNetAmount")).Text) + "'  ");
                    sql.Append(", '" + double.Parse(((Label)gridRow.FindControl("tbTotalAmount")).Text) + "'  ");
                    sql.Append(", '" + ((TextBox)gridRow.FindControl("tbNoteDetail")).Text + "'  ");

                    using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                    {
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }

                    line_no += 10;
                }

                line_no = 10;

                trans.Commit();
                conn.Close();
                master.messageBox("Data has been updated.");
            }
            //lStatus.Text = "OPEN";
            //statusChanged();
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
            trans.Rollback();
            conn.Close();
        }
    }

    private void PostData()
    {
        MasterPage master = (MasterPage)this.Master;
        conn = new SqlConnection(cAdih.getConnStr("Connection"));
        conn.Open();
        trans = conn.BeginTransaction();

        try
        {
            conn = new SqlConnection(cAdih.getConnStr("Connection"));
            conn.Open();
            trans = conn.BeginTransaction();

            sql.Length = 0;
            sql.Append("EXEC sp_sales_invoice_post '" + txtTransNo.Text + "','" + Session["username"].ToString() + "','" + DateTime.Now + "','" + cAdih.getIPAddress() + "' ");

            using (cmd = new SqlCommand(sql.ToString(), conn, trans))
            {
                cmd.ExecuteNonQuery();
                cmd = null;
            }

            trans.Commit();
            btnSave.Visible = false;
            btnPost.Visible = false;

            pnlHeader.Enabled = false;
            pnlDetail.Enabled = false;
            upHeader.Update();
            master.messageBox("Data has been posted.");
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message + " <br> " + ex.InnerException);
            trans.Rollback();
            conn.Close();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Session["ConfirmMode"] = "Save";
        showConfirmBox("Save This Data?");
    }

    protected void btnPost_Click(object sender, EventArgs e)
    {
        Session["ConfirmMode"] = "Post";
        showConfirmBox("Are you sure post this data?");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("SalesInvoiceList.aspx");
    }

    protected void showConfirmBox(string message)
    {
        wucConfirmBox1.SubShowConfirmBox(message);
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        try
        {
            if (wucConfirmBox1.StrMode == "Yes")
            {
                if (Session["ConfirmMode"].ToString() == "Post")
                {
                    PostData();
                }
                else if (Session["ConfirmMode"].ToString() == "Save")
                {
                    saveData();
                }
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void imbPONo_Click(object sender, EventArgs e)
    {
        Session["search"] = "si_sales_no";
        wucSearch.loadGrid();
    }

    private void bSearchPOno_Click(object sender, EventArgs e)
    {
        sql.Length = 0;
        sql.Append("select	trans_no as col0 ");
        sql.Append(", ro.trans_no + ' || ' + CONVERT(VARCHAR(11), ro.posted_date, 106) as col1 ");
        sql.Append(", ro.cust_po_no + ' || ' + CONVERT(VARCHAR(11), ro.cust_po_date, 106) + ' || ' + cu.customer_name + ' || ' + ca.alias_name_full + ' || ' + ca2.alias_name_full + ' (' + ca2.alias_name + ')' as col2 ");
        sql.Append(", 'Rental Order' as col3 ");
        sql.Append(", ro.posted_date ");
        sql.Append(", ro.cust_po_no ");
        sql.Append(", 'url' ");
        sql.Append("from rental_order ro ");
        sql.Append("inner join customer cu with(readpast) on ro.sold_to = cu.customer_no ");
        sql.Append("inner join customer_address ca with(readpast) on ro.sold_to = ca.customer_no and ca.address_type = '01' and ro.bill_to = ca.id ");
        sql.Append("inner join customer_address ca2 with(readpast) on ro.sold_to = ca2.customer_no and ca2.address_type = '02' and ro.ship_to = ca2.id ");
        sql.Append("where   ro.status = '1' ");

        using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
        {
            conn.Open();
            using (cmd = new SqlCommand(sql.ToString(), conn))
            {
                using (dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        txtPONo.Text = dr["col0"].ToString();
                        lSalesType.Text = dr["col3"].ToString();
                        libPONo.Text = "Cust PO No: " + dr["cust_po_no"].ToString();
                        imbPONo.Visible = false;
                        Session["dt"] = "NULL";
                    }
                    upHeader.Update();
                }
            }
        }
    }

    protected void libPONo_Click(object sender, EventArgs e)
    {
        txtPONo.Text = "";
        libPONo.Text = "";
        imbPONo.Visible = true;
        tbReffOrderNo.Text = "";
        lbReffOrderNo.Text = "";
        libPONo.Text = "";
        tbReffOrderNo.ReadOnly = false;
        ibSearchReffOrder.Visible = true;
        lSalesType.Text = "";
        lSalesOrderNo.Text = "";
        lDeliveryNo.Text = "";
        txtCustomerNo.Text = "";
        libCustomerName.Text = "";
        txtCustomerBillTo.Text = "";
        libCustomerBillToName.Text = "";
        txtDescription.Text = "";
        lbVatReg.Text = "";
        txtCurrencyID.Text = "";
        //pPaymentTerm.Visible = true;
        txtPaymentTerm.Text = "";
        libPaymentTerm.Text = "";
        txtTotalInvoice.Text = "0";
        txtPPN.Text = "0";
        txtTotalBill.Text = "0";
        txtDueDate.Text = "";
        txtNote.Text = "";
        upHeader.Update();
        Session["dt"] = null;

        grid.DataSource = null;
        grid.DataBind();
        upGrid.Update();
    }

    protected void ibSearchReffOrder_Click(object sender, EventArgs e)
    {
        Session["reff_no"] = txtPONo.Text;
        Session["reff_type"] = lSalesType.Text;
        Session["search"] = "si_lookup_reff_no";
        wucSearch.loadGrid();
    }

    protected void lbReffOrderNo_Click(object sender, EventArgs e)
    {
        tbReffOrderNo.Text = "";
        lbReffOrderNo.Text = "";
        tbReffOrderNo.ReadOnly = false;
        ibSearchReffOrder.Visible = true;
        upHeader.Update();
    }

    protected void bSearchReffOrder_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT DISTINCT 'Rental Order' AS sales_type, c.trans_no AS delivery_trans_no, CONVERT(VARCHAR(11), c.posted_date, 106) AS posted_date, c.real_delivery_date, c.note, d.trans_no AS sales_trans_no, d.cust_po_no AS cust_po_no,  ");
            sql.Append("d.bill_to, d.sold_to, e.customer_name, d.bill_to, f.alias_name_full AS bill_to_name, e.vat_reg_no, e.street_address, d.ship_to, g.alias_name_full + ' (' + g.alias_name + ')' AS ship_to_name, ");
            sql.Append("d.currency_id, d.payment_term, pt.description AS payment_term_desc, 0 AS days ");
            sql.Append("FROM outbound_delivery c WITH(READPAST) ");
            sql.Append("INNER JOIN rental_order d WITH(READPAST) ON c.reff_order_no = d.trans_no ");
            sql.Append("INNER JOIN payment_term pt WITH(READPAST) ON d.payment_term = pt.id ");
            sql.Append("INNER JOIN customer e WITH(READPAST) ON d.sold_to = e.customer_no ");
            sql.Append("INNER JOIN customer_address f WITH(READPAST) ON d.sold_to = f.customer_no AND f.address_type = '01' AND d.bill_to = f.id ");
            sql.Append("INNER JOIN customer_address g WITH(READPAST) ON d.sold_to = g.customer_no AND g.address_type = '02' AND d.ship_to = g.id ");
            sql.Append("LEFT JOIN sales_invoice h WITH(READPAST) ON c.trans_no = h.reff_order_no ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append("AND c.reff = 'RENTAL ORDER' ");
            sql.Append("AND c.status = '1' ");
            sql.Append("AND c.trans_no = '" + tbReffOrderNo.Text + "' ");
            ////sql.Append("AND h.trans_no IS NULL ");
            //sql.Append("UNION ");
            //sql.Append("SELECT DISTINCT 'Sales Order' AS sales_type, a.trans_no AS delivery_trans_no, CONVERT(VARCHAR(11), a.posted_date, 106) AS posted_date, a.real_delivery_date, a.note, d.trans_no AS sales_trans_no, d.cust_po_no AS cust_po_no, ");
            //sql.Append("d.bill_to, d.sold_to, e.customer_name, d.bill_to, f.alias_name_full AS bill_to_name, e.vat_reg_no, e.street_address, d.ship_to, g.alias_name_full + ' (' + g.alias_name + ')'  AS ship_to_name, ");
            //sql.Append("d.currency_id, d.payment_term, h.description AS payment_term_desc, h.days ");
            //sql.Append("FROM proof_of_delivery a WITH(READPAST) ");
            //sql.Append("left JOIN proof_of_delivery_detail b WITH(READPAST) ON a.trans_no = b.trans_no ");
            //sql.Append("INNER JOIN outbound_delivery c WITH(READPAST) ON a.reff_order_no = c.trans_no ");
            //sql.Append("INNER JOIN sales_order d WITH(READPAST) ON c.reff_order_no = d.trans_no ");
            //sql.Append("INNER JOIN customer e WITH(READPAST) ON d.sold_to = e.customer_no ");
            //sql.Append("INNER JOIN customer_address f WITH(READPAST) ON d.sold_to = f.customer_no AND f.address_type = '01' AND d.bill_to = f.id ");
            //sql.Append("INNER JOIN customer_address g WITH(READPAST) ON d.sold_to = g.customer_no AND g.address_type = '02' AND d.ship_to = g.id ");
            //sql.Append("INNER JOIN payment_term h WITH(READPAST) ON d.payment_term = h.id  ");
            //sql.Append("LEFT JOIN sales_invoice i WITH(READPAST) ON a.trans_no = i.reff_order_no ");
            //sql.Append("WHERE 1 = 1 ");
            //sql.Append("AND a.reff = 'SALES ORDER' ");
            //sql.Append("AND a.company_id = '" + txtCompanyID.Text + "' ");
            //sql.Append("AND a.status = '1' ");
            //sql.Append("AND a.trans_no = '" + tbReffOrderNo.Text + "' ");
            ////sql.Append("AND i.trans_no IS NULL ");
            //sql.Append("UNION ");
            //sql.Append("SELECT DISTINCT 'Rental Extension' AS sales_type, re.trans_no AS delivery_trans_no, CONVERT(VARCHAR(11), re.posted_date, 106) AS posted_date, re.posted_date, re.note, re.trans_no AS sales_trans_no, re.cust_po_no AS cust_po_no, ");
            //sql.Append("re.bill_to, re.customer, e.customer_name, re.bill_to, f.alias_name_full AS bill_to_name, e.vat_reg_no, e.street_address, re.customer_site as ship_to, g.alias_name_full + ' (' + g.alias_name + ')'  AS ship_to_name, ");
            //sql.Append("re.currency_id, re.term_of_payment AS payment_term, h.description AS payment_term_desc, h.days ");
            //sql.Append("FROM rental_extension re WITH(READPAST) ");
            //sql.Append("INNER JOIN customer e WITH(READPAST) ON re.customer = e.customer_no ");
            //sql.Append("INNER JOIN customer_address f WITH(READPAST) ON re.customer = f.customer_no AND f.address_type = '01' AND re.bill_to = f.id ");
            //sql.Append("INNER JOIN customer_address g WITH(READPAST) ON re.customer = g.customer_no AND g.address_type = '02' AND re.customer_site = g.id ");
            //sql.Append("INNER JOIN payment_term h WITH(READPAST) ON re.term_of_payment = h.id  ");
            //sql.Append("LEFT JOIN sales_invoice i WITH(READPAST) ON re.trans_no = i.reff_order_no ");
            //sql.Append("WHERE 1 = 1 ");
            //sql.Append("AND re.sales_org = '" + txtCompanyID.Text + "' ");
            //sql.Append("AND re.status = '1' ");
            //sql.Append("AND re.trans_no = '" + tbReffOrderNo.Text + "' ");
            ////sql.Append("AND i.trans_no IS NULL ");
            //sql.Append("UNION ");
            //sql.Append("SELECT DISTINCT 'Rental Return Order' AS sales_type, a.trans_no AS delivery_trans_no, CONVERT(VARCHAR(11), a.posted_date, 106) AS posted_date, a.real_delivery_date, a.note, d.trans_no AS sales_trans_no, d.cust_po_no AS cust_po_no, ");
            //sql.Append("d.bill_to, d.customer, e.customer_name, d.bill_to, f.alias_name_full AS bill_to_name, e.vat_reg_no, e.street_address, d.customer_site, g.alias_name_full + ' (' + g.alias_name + ')'  AS ship_to_name, ");
            //sql.Append("d.currency_id, d.term_of_payment AS payment_term, h.description AS payment_term_desc, h.days ");
            //sql.Append("FROM proof_of_delivery a WITH(READPAST) ");
            //sql.Append("LEFT JOIN proof_of_delivery_detail b WITH(READPAST) ON a.trans_no = b.trans_no ");
            //sql.Append("INNER JOIN outbound_delivery c WITH(READPAST) ON a.reff_order_no = c.trans_no ");
            //sql.Append("INNER JOIN rental_return_order d WITH(READPAST) ON c.reff_order_no = d.trans_no ");
            //sql.Append("INNER JOIN customer e WITH(READPAST) ON d.customer = e.customer_no ");
            //sql.Append("INNER JOIN customer_address f WITH(READPAST) ON d.customer = f.customer_no AND f.address_type = '01' AND d.bill_to = f.id ");
            //sql.Append("INNER JOIN customer_address g WITH(READPAST) ON d.customer = g.customer_no AND g.address_type = '02' AND d.customer_site = g.id ");
            //sql.Append("INNER JOIN payment_term h WITH(READPAST) ON d.term_of_payment = h.id ");
            //sql.Append("LEFT JOIN sales_invoice i WITH(READPAST) ON a.trans_no = i.reff_order_no ");
            //sql.Append("WHERE 1 = 1 ");
            //sql.Append("AND a.reff = 'RENTAL RETURN ORDER' ");
            //sql.Append("AND a.company_id = '" + txtCompanyID.Text + "' ");
            //sql.Append("AND a.status = '1' ");
            //sql.Append("AND a.trans_no = '" + tbReffOrderNo.Text + "' ");

            ////sql.Append("SELECT DISTINCT 'Rental Return Order' AS sales_type, re.trans_no AS delivery_trans_no, CONVERT(VARCHAR(11), re.posted_date, 106) AS posted_date, re.posted_date, re.note, re.trans_no AS sales_trans_no, re.cust_po_no AS cust_po_no, ");
            ////sql.Append("re.bill_to, re.customer, e.customer_name, re.bill_to, f.alias_name_full AS bill_to_name, e.vat_reg_no, e.street_address, re.customer_site, g.alias_name_full + ' (' + g.alias_name + ')' AS ship_to_name, ");
            ////sql.Append("re.currency_id, re.term_of_payment AS payment_term, h.description AS payment_term_desc, h.days ");
            ////sql.Append("FROM rental_return_order re WITH(READPAST) ");
            ////sql.Append("INNER JOIN customer e WITH(READPAST) ON re.customer = e.customer_no ");
            ////sql.Append("INNER JOIN customer_address f WITH(READPAST) ON re.customer = f.customer_no AND f.address_type = '01' AND re.bill_to = f.id ");
            ////sql.Append("INNER JOIN customer_address g WITH(READPAST) ON re.customer = g.customer_no AND g.address_type = '02' AND re.customer_site = g.id ");
            ////sql.Append("INNER JOIN payment_term h WITH(READPAST) ON re.term_of_payment = h.id  ");
            ////sql.Append("LEFT JOIN sales_invoice i WITH(READPAST) ON re.trans_no = i.reff_order_no ");
            ////sql.Append("WHERE 1 = 1 ");
            ////sql.Append("AND re.sales_org = '" + txtCompanyID.Text + "' ");
            ////sql.Append("AND re.status = '1' ");
            ////sql.Append("AND re.trans_no = '" + tbReffOrderNo.Text + "' ");
            //////sql.Append("AND i.trans_no IS NULL ");

            using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
            {
                conn.Open();
                using (cmd = new SqlCommand(sql.ToString(), conn))
                {
                    using (dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            tbReffOrderNo.ReadOnly = true;
                            ibSearchReffOrder.Visible = false;
                            lSalesType.Text = dr["sales_type"].ToString();
                            //lDeliveryNo.Text = dr["delivery_trans_no"].ToString();
                            lbReffOrderNo.Text = dr["sales_trans_no"].ToString();
                            lSalesOrderNo.Text = dr["sales_trans_no"].ToString();
                            lCustPONo.Text = dr["cust_po_no"].ToString();
                            txtCustomerNo.Text = dr["sold_to"].ToString();
                            libCustomerName.Text = dr["customer_name"].ToString();
                            txtCustomerBillTo.Text = dr["bill_to"].ToString();
                            libCustomerBillToName.Text = dr["bill_to_name"].ToString();
                            txtCustomerShipTo.Text = dr["ship_to"].ToString();
                            libCustomerShipToName.Text = dr["ship_to_name"].ToString();
                            txtCurrencyID.Text = dr["currency_id"].ToString();
                            txtDescription.Text = dr["street_address"].ToString();
                            lbVatReg.Text = dr["vat_reg_no"].ToString();
                            txtNote.Text = "Cust PO No: " + dr["cust_po_no"].ToString() + Environment.NewLine +
                                "Real Delivery Date: " + DateTime.Parse(dr["real_delivery_date"].ToString()).ToString("dd MMM yyyy") + Environment.NewLine +
                                "Cust Site: " + dr["ship_to_name"].ToString() + Environment.NewLine +
                                "POD Note: " + dr["note"].ToString();
                            txtTotalInvoice.Text = "0";
                            //if (lSalesType.Text == "Rental Order")
                            //{
                            //    pnlPaymentTerm.Visible = true;
                            //    txtPaymentTerm.Text = dr["payment_term1"].ToString();
                            //    libPaymentTerm.Text = dr["payment_term_desc1"].ToString();
                            //    pnlDueDate.Visible = false;
                            //}
                            //else //if (lSalesType.Text == "Sales Order" || lSalesType.Text == "Rental Extension")
                            //{
                            pnlPaymentTerm.Visible = true;
                            txtPaymentTerm.Text = dr["payment_term"].ToString();
                            libPaymentTerm.Text = dr["payment_term_desc"].ToString(); ;
                            txtDueDate.Text = DateTime.Now.AddDays(int.Parse(dr["days"].ToString())).ToString("dd-MM-yyyy");
                            //}
                        }
                        else
                        {
                            lbReffOrderNo.Text = "";
                            tbReffOrderNo.ReadOnly = false;
                            lSalesType.Text = "";
                            lDeliveryNo.Text = "";
                            ibSearchReffOrder.Visible = true;
                            txtCustomerNo.Text = "";
                            libCustomerName.Text = "";
                            txtCustomerBillTo.Text = "";
                            libCustomerBillToName.Text = "";
                            txtCurrencyID.Text = "";
                            pnlPaymentTerm.Visible = true;
                            txtPaymentTerm.Text = "";
                            libPaymentTerm.Text = "";
                            txtDueDate.Text = "";
                            txtNote.Text = "";
                            master.messageBox("Invalid reff delivery no!");
                            return;
                        }
                        upHeader.Update();
                    }
                }
            }

            sql.Length = 0;
            if (lSalesType.Text == "Rental Order")
            {
                sql.Length = 0;
                sql.Append("SELECT e.trans_no, c.article_type, f.article_no, c.article_description,  (select description from source s where s.id = h.source_id) 'source',  ");
                sql.Append("SUM(ISNULL(h.qty, 0)) as qty, (isnull(dbo.fn_get_rental_order_billing_plan_rate_unit(g.trans_no, f.article_no, dbo.fn_get_rental_order_billing_plan_last_period(g.trans_no, f.article_no)), 0)) AS unit_price, ISNULL(dbo.fn_get_rental_order_billing_plan_qty(g.trans_no, f.article_no), 0) AS qty_od, isnull(h.tax, 0) as unit_tax, '' as note, ");
                sql.Append("(isnull(dbo.fn_get_rental_order_billing_plan_last_period(g.trans_no, f.article_no), 0)) AS period, ");
                sql.Append("(isnull(dbo.fn_get_rental_order_billing_plan_outstanding_duration(g.trans_no, f.article_no, dbo.fn_get_rental_order_billing_plan_last_period(g.trans_no, f.article_no)), 0)) AS outstanding_duration, ");
                sql.Append("(isnull(dbo.fn_get_rental_order_billing_plan_discount(g.trans_no, f.article_no, dbo.fn_get_rental_order_billing_plan_last_period(g.trans_no, f.article_no)), 0)) AS disc ");
                sql.Append("FROM outbound_delivery e WITH(READPAST) ");
                sql.Append("INNER JOIN outbound_delivery_detail f WITH(READPAST) ON e.trans_no = f.trans_no ");
                sql.Append("INNER JOIN article c WITH(READPAST) ON f.article_no = c.article_no ");
                sql.Append("LEFT JOIN rental_order g WITH(READPAST) ON e.reff_order_no = g.trans_no ");
                sql.Append("LEFT JOIN rental_order_detail h WITH(READPAST) ON g.trans_no = h.trans_no AND f.reff_line_no = h.line_no ");
                sql.Append("INNER JOIN rental_order_billing_plan robp on robp.trans_no = g.trans_no AND robp.period = (dbo.fn_get_rental_order_billing_plan_last_period(g.trans_no, f.article_no)) ");
                sql.Append("WHERE 1 = 1 ");
                sql.Append("AND e.trans_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`").ToUpper() + "' ");
                sql.Append("AND(dbo.fn_get_rental_order_billing_plan_last_period(g.trans_no, f.article_no)) is not null ");
                sql.Append("AND year(robp.billing_date) <= year(getdate()) ");
                sql.Append("GROUP BY e.trans_no, c.article_type, f.article_no, c.article_description, h.source_id, ");
                sql.Append("h.qty, h.rate, g.trans_no, h.tax ");
            }
            //else if (lSalesType.Text == "Rental Return Order")
            //{
            //    sql.Append("SELECT a.trans_no, c.article_type, cp.value 'article_no', c.article_description, c.base_uom 'unit_id', d.description AS unit_name, '' as source , ");
            //    sql.Append("SUM(ISNULL(h.qty, 0)) as qty, CAST((g.cut_off_charge / (select sum(qty) from rental_return_order_detail rod where rod.trans_no = g.trans_no)) as decimal(18, 2)) AS unit_price, SUM(b.qty_delivered) AS qty_od, CAST((g.cut_off_charge / (select sum(qty) from rental_return_order_detail rod where rod.trans_no = g.trans_no) * (SELECT [dbo].[fn_sales_invoice_tax_rate] ('" + txtPONo.Text.Trim() + "'))/100) as DECIMAL(18, 2)) as unit_tax, 0 as disc, '' as note, ");
            //    sql.Append("'1' AS period, isnull(NULL, 0) AS outstanding_duration ");
            //    sql.Append("FROM proof_of_delivery a WITH(READPAST) ");
            //    sql.Append("LEFT JOIN proof_of_delivery_detail b WITH(READPAST) ON a.trans_no = b.trans_no ");
            //    sql.Append("left join company_param cp WITH(READPAST) on cp.code = 'RRO' ");
            //    sql.Append("INNER JOIN article c WITH(READPAST) ON cp.value = c.article_number ");
            //    sql.Append("INNER JOIN uom d WITH(READPAST) ON c.base_uom = d.id ");
            //    sql.Append("LEFT JOIN outbound_delivery e WITH(READPAST) ON a.reff_order_no = e.trans_no ");
            //    sql.Append("LEFT JOIN outbound_delivery_detail f WITH(READPAST) ON e.trans_no = f.trans_no AND b.reff_line_no = f.line_no ");
            //    sql.Append("LEFT JOIN rental_return_order g WITH(READPAST) ON e.reff_order_no = g.trans_no ");
            //    sql.Append("LEFT JOIN rental_return_order_detail h WITH(READPAST) ON g.trans_no = h.trans_no AND f.reff_line_no = h.line_no ");
            //    sql.Append("WHERE 1 = 1 ");
            //    sql.Append("AND a.trans_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`").ToUpper() + "' ");
            //    sql.Append("AND a.status = '1' ");
            //    sql.Append("GROUP BY a.trans_no, c.article_type, cp.value, c.article_description, c.base_uom, d.description, g.cut_off_charge, g.trans_no ");

            //}

            using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
            {
                conn.Open();
                using (cmd = new SqlCommand(sql.ToString(), conn))
                {
                    using (dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            double dDuration = 0;
                            double dPrice = 0;
                            double dTaxAmount = 0;
                            double dDiscAmount = 0;

                            if (Session["dt"].ToString() == "NULL")
                            {
                                DataTable dt = new DataTable();
                                dt = createDataTable();
                                Session["dt"] = dt;
                            }

                            while (dr.Read())
                            {
                                dDiscAmount = double.Parse(dr["disc"].ToString());
                                if (lSalesType.Text == "Rental Order")
                                {
                                    double vat = 11;
                                    if (dr["outstanding_duration"].ToString() == "0")
                                        dDuration = double.Parse(dr["outstanding_duration"].ToString().Replace("0", "1"));
                                    else
                                        dDuration = double.Parse(dr["outstanding_duration"].ToString());

                                    dPrice = double.Parse(dr["qty_od"].ToString()) * double.Parse(dr["unit_price"].ToString()) * dDuration;
                                    if (dPrice != 0)
                                        dTaxAmount = (dPrice - dDiscAmount) * vat / 100.00; //double.Parse(dr["unit_tax"].ToString()) * double.Parse(dr["qty_od"].ToString()) * dDuration;
                                    else
                                    {
                                        dTaxAmount = 0;
                                        dDiscAmount = 0;
                                        dDuration = 0;
                                    }
                                }
                                else
                                {
                                    dDuration = 1;
                                    dPrice = double.Parse(dr["qty_od"].ToString()) * double.Parse(dr["unit_price"].ToString()) * dDuration;
                                    dTaxAmount = double.Parse(dr["unit_tax"].ToString()) * double.Parse(dr["qty_od"].ToString()) * dDuration;
                                }

                                //if (dPrice != 0)
                                addDataToTable(tbReffOrderNo.Text.Trim(), dr["source"].ToString(), dr["article_no"].ToString(), dr["article_type"].ToString(), dr["article_description"].ToString(),
                                double.Parse(dr["qty"].ToString()), double.Parse(dr["qty_od"].ToString()),
                                double.Parse(dr["unit_price"].ToString()), double.Parse(dr["period"].ToString()), double.Parse(dr["outstanding_duration"].ToString()),
                                double.Parse(dr["outstanding_duration"].ToString()), dPrice,
                                double.Parse(dr["unit_tax"].ToString()), dTaxAmount, dDiscAmount, dPrice - dDiscAmount, dr["note"].ToString(), DateTime.Now, (DataTable)Session["dt"]);

                                dTaxAmount = 0;
                                dDuration = 1;
                            }
                            loadGrid();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected DataTable createDataTable()
    {
        try
        {
            dt = new DataTable();

            DataColumn dtColumn;

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "reff_no";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "source";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "article_No";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "article_Type";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "article_Description";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "unit_ID";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "unit_Desc";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "qty_order";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "qty_delivered";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "unit_price";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "period";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "outstanding_duration";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "bill_duration";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "price";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "unit_tax";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "tax_amount";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "discount";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "net_amount";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "total_amount";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "note";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.DateTime");
            dtColumn.ColumnName = "addedDate";
            dt.Columns.Add(dtColumn);

            return dt;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private void addDataToTable(string reff_no, string source, string articleNo, string articleType, string articleDesc,
        double qty, double qty_received, double unit_price, double period, double outstanding_duration, double bill_duration, double price,
        double unit_tax, double tax_amount, double discount, double net_amount, String note, DateTime addedDate, DataTable Table)
    {
        try
        {
            DataRow dtRow;

            dtRow = Table.NewRow();

            dtRow["reff_no"] = reff_no;
            dtRow["source"] = source;
            dtRow["article_No"] = articleNo;
            dtRow["article_Type"] = articleType;
            dtRow["article_description"] = articleDesc;
            dtRow["qty_order"] = qty;
            dtRow["qty_delivered"] = qty_received;
            dtRow["unit_price"] = unit_price;
            dtRow["period"] = period;
            dtRow["outstanding_duration"] = outstanding_duration;
            dtRow["bill_duration"] = bill_duration;
            dtRow["price"] = price;
            dtRow["unit_tax"] = unit_tax;
            dtRow["tax_amount"] = tax_amount;
            dtRow["discount"] = discount;
            dtRow["net_amount"] = net_amount;
            dtRow["total_amount"] = price + tax_amount - discount;
            dtRow["note"] = note;
            dtRow["addedDate"] = addedDate;

            Table.Rows.Add(dtRow);
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void loadGrid()
    {
        try
        {
            grid.DataSource = ((DataTable)Session["dt"]).DefaultView;
            grid.DataBind();

            if (grid.Rows.Count >= 1)
            {
                double vat = 11;
                txtTotalInvoice.Text = Double.Parse(((DataTable)Session["dt"]).Compute("SUM(net_amount)", "").ToString()).ToString("##,#0.#0");
                double TotalPPn = double.Parse(((DataTable)Session["dt"]).Compute("SUM(net_amount)", "").ToString());
                txtPPN.Text = (TotalPPn * vat / 100).ToString("##,#0.#0");
            }
            else
            {
                txtTotalInvoice.Text = "0";
                txtPPN.Text = "0";
            }

            if (lSalesType.Text == "Sales Order" || lSalesType.Text == "Rental Return Order")
            {
                grid.Columns[9].Visible = false;
                grid.Columns[11].Visible = false;
                grid.Columns[9].HeaderStyle.Width = new Unit("0px");
                grid.Columns[9].ItemStyle.Width = new Unit("0px");
                grid.Columns[11].HeaderStyle.Width = new Unit("0px");
                grid.Columns[11].ItemStyle.Width = new Unit("0px");

                grid.Width = new Unit("100%");
            }
            else if (lSalesType.Text == "Rental Order" || lSalesType.Text == "Rental Extension")
            {
                grid.Columns[2].Visible = false;
                grid.Columns[2].HeaderStyle.Width = new Unit("0px");
                grid.Columns[2].ItemStyle.Width = new Unit("0px");
            }

            txtTotalBill.Text = (decimal.Parse(txtTotalInvoice.Text) + decimal.Parse(txtPPN.Text)).ToString("##,#0.#0");

            upHeader.Update();
            upGrid.Update();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void wucLookup_Hide(object sender, EventArgs e)
    {
        if (Session["search"].ToString() == "si_sales_no")
        {
            txtPONo.Text = wucSearch.result;

            bSearchPOno_Click(sender, e);
            //bSearchReffOrder_Click(sender, e);
            upHeader.Update();
        }
        else if (Session["search"].ToString() == "si_lookup_reff_no")
        {
            tbReffOrderNo.Text = wucSearch.result;
            bSearchReffOrder_Click(sender, e);
            upHeader.Update();
        }
        //ExecSP _dal = null;
        //Hashtable _ht = null;

        //try
        //{
        //    _dal = new ExecSP();
        //    _ht = new Hashtable();

        //    if (Session["lookupcode"].ToString() == "LU00011")
        //    {
        //        txtPONo.Text = wuclookup1.result[0].ToString();
        //        lSalesType.Text = wuclookup1.result[1].ToString();
        //        libPONo.Text = "Cust PO No: " + wuclookup1.result[2].ToString();
        //        libViewDocument.Attributes["onclick"] = "javascript:window.open('" + wuclookup1.result[3].ToString() + "', 'viewer', 'fullscreen=0, status=0, menubar=0, scrollbars=0, resizeable=1, toolbar=0, width=600, height=400');";
        //        imbPONo.Visible = false;
        //        libViewDocument.Visible = true;
        //        Session["dt"] = "NULL";

        //    }
        //    else if (Session["lookupcode"].ToString() == "LU00005")
        //    {
        //        tbReffOrderNo.Text = wuclookup1.result[0].ToString();
        //        bSearchReffOrder_Click(sender, e);
        //    }
        //    upHeader.Update();
        //}

        //catch (Exception ex)
        //{
        //    MasterPage master = (MasterPage)this.Master;
        //    master.messageBox("<br />" + ex.InnerException);
        //}
    }

    #region Detail

    private void BindData()
    {
        sql.Length = 0;
        sql.Append("exec sp_sales_invoice_detail_getrows '" + txtTransNo.Text + "' ");

        cAdih.loadGridView(sql.ToString(), cAdih.getConnStr("Connection"), grid);
        upGrid.Update();
    }

    protected void imgSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grid.PageIndex = e.NewPageIndex;
    }

    protected void grid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label period = (Label)e.Row.FindControl("lPeriod");

            if (period.Text == "0")
                ((TextBox)e.Row.FindControl("txtBillDuration")).Enabled = false;
        }
    }

    #endregion Detail
}