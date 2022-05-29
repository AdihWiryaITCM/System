using System;
using System.Data;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Drawing;
using System.Net.Mail;
using System.Data.SqlClient;


public partial class Transaction_RvHeader : Page
{
    private static string TABLE_NAME = "rv_header";
    private static string TABLE_NAME_DETAIL = "rv_detail";
    private static string TABLE_NAME_DISCOUNT = "rv_discount";

    StringBuilder sql = new StringBuilder();
    SqlTransaction trans;
    SqlConnection conn;
    SqlCommand cmd;
    SqlDataReader dr;
    DataTable dt;
    ClassAdih cAdih = new ClassAdih();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtRVDate.Enabled = false;

            if (Request.Params["action"].Equals("edit"))
            {
                Session["SaveMode"] = "edit";
                
                LoadData();
                BindData();
                
                if (lblStatus.Text == "POSTED")
                {
                    grid.Columns[1].Visible = false;
                    grid.Columns[9].Visible = false;

                    btnSave.Visible = false;
                    btnPost.Visible = false;

                    pnlHeader.Enabled = false;

                }

            }
            else
            {
                Session["SaveMode"] = "add";

                txtRVDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtReceiptDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
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
            sql.Append("exec [dbo].[sp_rv_header_getrow] '" + Request.Params["rvno"].ToString() + "' ");

            using (cmd = new SqlCommand(sql.ToString(), conn))
            {
                using (dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        txtTransNo.Text = dr["RV_NO"].ToString();
                        txtRVDate.Text = Shared.DBtoUIDate(dr["RV_DATE"].ToString());
                        txtReceiptDate.Text = Shared.DBtoUIDate(dr["RECEIPT_DATE"].ToString());
                        txtCustomerNo.Text = dr["CUSTOMER_NO"].ToString();
                        libCustomerNo.Text = dr["CUSTOMER_NAME"].ToString();
                        txtBillTo.Text = dr["BILL_TO"].ToString();
                        txtBillToDesc.Text = dr["STREET_ADDRESS"].ToString();
                        imbCustomerNo.Visible = false;
                        imbBillTo.Visible = false;
                        txtTotal.Text = double.Parse(dr["ORIG_AMOUNT"].ToString()).ToString("##,#0.#0");
                        txtAmount.Text = double.Parse(dr["AMOUNT"].ToString()).ToString("##,#0.#0");
                        txtAdminFee.Text = double.Parse(dr["ADMIN_FEE"].ToString()).ToString("##,#0.#0");
                        txtOverpayment.Text = double.Parse(dr["OVERPAYMENT"].ToString()).ToString("##,#0.#0");
                        txtNote.Text = dr["NOTE"].ToString();
                        lblStatus.Text = dr["STATUS"].ToString();
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

        //ExecSP _dal = null;
        //Hashtable _ht = null;

        //try
        //{
        //    _dal = new ExecSP();
        //    _ht = new Hashtable();

        //    _ht["p_rv_no"] = Request.Params["rvno"];
        //    DataRow _dr = _dal.GetRow(TABLE_NAME, _ht);

        //    txtTransNo.Text = _dr["RV_NO"].ToString();
        //    txtDocNo.Text = _dr["DOC_NO"].ToString();
        //    if (_dr["STATUS"].ToString() == "HOLD")
        //    {
        //        txtDocNo.Visible = false;
        //    }
        //    txtCompanyID.Text = _dr["COMPANY_ID"].ToString();
        //    libCompanyDesc.Text = _dr["COMPANY_NAME"].ToString();
        //    txtSiteID.Text = _dr["SITE_ID"].ToString();
        //    libSiteName.Text = _dr["SITE_NAME"].ToString();
        //    txtRVDate.Text = Shared.DBtoUIDate(_dr["RV_DATE"].ToString());
        //    txtReceiptDate.Text = Shared.DBtoUIDate(_dr["RECEIPT_DATE"].ToString());
        //    txtCustomerNo.Text = _dr["CUSTOMER_NO"].ToString();
        //    libCustomerNo.Text = _dr["CUSTOMER_NAME"].ToString();
        //    txtBillTo.Text = _dr["BILL_TO"].ToString();
        //    txtBillToDesc.Text = _dr["STREET_ADDRESS"].ToString();
        //    imbCustomerNo.Visible = false;
        //    imbBillTo.Visible = false;
        //    txtBank.Text = _dr["BANK_CODE"].ToString();
        //    libBankName.Text = _dr["BANK_NAME"].ToString();
        //    libBankAccountNo.Text = _dr["BANK_ACCOUNT_NO"].ToString();
        //    imbBank.Visible = false;
        //    ddlCurrency.SelectedValue = _dr["ORIG_CURRENCY"].ToString();
        //    txtTotal.Text = double.Parse(_dr["ORIG_AMOUNT"].ToString()).ToString("##,#0.#0");
        //    txtAmount.Text = double.Parse(_dr["AMOUNT"].ToString()).ToString("##,#0.#0");
        //    txtAdminFee.Text = double.Parse(_dr["ADMIN_FEE"].ToString()).ToString("##,#0.#0");
        //    txtOverpayment.Text = double.Parse(_dr["OVERPAYMENT"].ToString()).ToString("##,#0.#0");
        //    txtNote.Text = _dr["NOTE"].ToString();
        //    lblStatus.Text = _dr["STATUS"].ToString();

        //}
        //catch (Exception ex)
        //{
        //    MasterPage master = (MasterPage)this.Master;
        //    master.messageBox(ex.Message + "<br />" + ex.InnerException);
        //}
    }

    private void saveData()
    {
        MasterPage master = (MasterPage)this.Master;
        string sNextCode = "";

        try
        {
            if (txtTransNo.Text == "")
            {
                conn = new SqlConnection(cAdih.getConnStr("Connection"));
                conn.Open();
                trans = conn.BeginTransaction();

                sql.Length = 0;
                sql.Append("SELECT RIGHT('000000' + CAST(ISNULL(MAX(CAST(RIGHT(rv_no, 6) AS INT)), 0) + 1 AS VARCHAR), 6) AS running_no, ");
                sql.Append("YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) AS tahun, RIGHT('00' + CAST(MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) AS VARCHAR), 2) AS bulan ");
                sql.Append("FROM rv_header ");
                sql.Append("WHERE YEAR(rv_date) = YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) ");
                sql.Append("AND MONTH(rv_date) = MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    using (dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            txtTransNo.Text = "RV" + dr["tahun"].ToString() + dr["bulan"].ToString() + dr["running_no"].ToString();
                            upHeader.Update();
                        }
                    }
                }

                sql.Length = 0;
                sql.Append("EXEC [dbo].[sp_rv_header_insert] ");
                sql.Append(" '" + txtTransNo.Text + "' ");
                sql.Append(", '" + Shared.UItoDBDate(txtRVDate.Text) + "' ");
                sql.Append(", '" + Shared.UItoDBDate(txtReceiptDate.Text) + "' ");
                sql.Append(", '" + txtCustomerNo.Text + "'  ");
                sql.Append(", '" + txtBillTo.Text + "'  ");
                sql.Append(", '" + double.Parse(txtTotal.Text) + "'  ");
                sql.Append(", '" + double.Parse(txtAmount.Text) + "'  ");
                sql.Append(", '" + double.Parse(txtAdminFee.Text) + "'  ");
                sql.Append(", '" + double.Parse(txtOverpayment.Text) + "'  ");
                sql.Append(", '" + txtNote.Text + "'  ");
                sql.Append(",'" + Session["username"].ToString() + "' ");
                sql.Append(",'" + cAdih.engFormatDate(DateTime.Now.ToString("dd-MM-yyyy")) + "' ");
                sql.Append(", '" + cAdih.getIPAddress() + "'  ");
                sql.Append(",'" + Session["username"].ToString() + "' ");
                sql.Append(",'" + cAdih.engFormatDate(DateTime.Now.ToString("dd-MM-yyyy")) + "' ");
                sql.Append(", '" + cAdih.getIPAddress() + "'  ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                foreach (GridViewRow gridRow in grid.Rows)
                {
                    if (((CheckBox)gridRow.FindControl("chbPaid")).Checked == true)
                    {
                        sql.Length = 0;
                        sql.Append("EXEC [dbo].[sp_rv_detail_insert] ");
                        sql.Append(" '" + gridRow.RowIndex + "' ");
                        sql.Append(", '" + txtTransNo.Text + "' ");
                        sql.Append(", '" + ((LinkButton)gridRow.FindControl("libReffNo")).Text + "' ");
                        sql.Append(", '" + double.Parse(((LinkButton)gridRow.FindControl("libBaseAmount")).Text) + "'  ");
                        sql.Append(", '" + double.Parse(((TextBox)gridRow.FindControl("txtReceiptAmount")).Text) + "'  ");
                        sql.Append(", '" + double.Parse(((TextBox)gridRow.FindControl("libTotalDiscount")).Text) + "'  ");
                        sql.Append(",'" + Session["username"].ToString() + "' ");
                        sql.Append(",'" + cAdih.engFormatDate(DateTime.Now.ToString("dd-MM-yyyy")) + "' ");
                        sql.Append(", '" + cAdih.getIPAddress() + "'  ");
                        sql.Append(",'" + Session["username"].ToString() + "' ");
                        sql.Append(",'" + cAdih.engFormatDate(DateTime.Now.ToString("dd-MM-yyyy")) + "' ");
                        sql.Append(", '" + cAdih.getIPAddress() + "'  ");

                        using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                        {
                            cmd.ExecuteNonQuery();
                            cmd = null;
                        }
                    }
                }
                btnPost.Visible = true;
                trans.Commit();
                conn.Close();
                master.messageBox("Data has been saved. Trans No: " + txtTransNo.Text + "");

            }
            else
            {
                conn = new SqlConnection(cAdih.getConnStr("Connection"));
                conn.Open();
                trans = conn.BeginTransaction();

                sql.Length = 0;
                sql.Append("EXEC [dbo].[sp_rv_header_update] ");
                sql.Append(" '" + txtTransNo.Text + "' ");
                sql.Append(", '" + Shared.UItoDBDate(txtRVDate.Text) + "' ");
                sql.Append(", '" + txtCustomerNo.Text + "'  ");
                sql.Append(", '" + txtBillTo.Text + "'  ");
                sql.Append(", '" + double.Parse(txtTotal.Text) + "'  ");
                sql.Append(", '" + double.Parse(txtAmount.Text) + "'  ");
                sql.Append(", '" + double.Parse(txtAdminFee.Text) + "'  ");
                sql.Append(", '" + double.Parse(txtOverpayment.Text) + "'  ");
                sql.Append(", '" + txtNote.Text + "'  ");
                sql.Append(",'" + Session["username"].ToString() + "' ");
                sql.Append(",'" + cAdih.engFormatDate(DateTime.Now.ToString("dd-MM-yyyy")) + "' ");
                sql.Append(", '" + cAdih.getIPAddress() + "'  ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                sql.Length = 0;
                sql.Append("DELETE rv_detail WHERE rv_no = '" + txtTransNo.Text +"' ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                foreach (GridViewRow gridRow in grid.Rows)
                {
                    if (((CheckBox)gridRow.FindControl("chbPaid")).Checked == true)
                    {
                        sql.Length = 0;
                        sql.Append("EXEC [dbo].[sp_rv_detail_insert] ");
                        sql.Append(" '" + gridRow.RowIndex + "' ");
                        sql.Append(", '" + txtTransNo.Text + "' ");
                        sql.Append(", '" + ((LinkButton)gridRow.FindControl("libReffNo")).Text + "' ");
                        sql.Append(", '" + double.Parse(((LinkButton)gridRow.FindControl("libBaseAmount")).Text) + "'  ");
                        sql.Append(", '" + double.Parse(((TextBox)gridRow.FindControl("txtReceiptAmount")).Text) + "'  ");
                        sql.Append(", '" + double.Parse(((TextBox)gridRow.FindControl("libTotalDiscount")).Text) + "'  ");
                        sql.Append(",'" + Session["username"].ToString() + "' ");
                        sql.Append(",'" + cAdih.engFormatDate(DateTime.Now.ToString("dd-MM-yyyy")) + "' ");
                        sql.Append(", '" + cAdih.getIPAddress() + "'  ");
                        sql.Append(",'" + Session["username"].ToString() + "' ");
                        sql.Append(",'" + cAdih.engFormatDate(DateTime.Now.ToString("dd-MM-yyyy")) + "' ");
                        sql.Append(", '" + cAdih.getIPAddress() + "'  ");

                        using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                        {
                            cmd.ExecuteNonQuery();
                            cmd = null;
                        }
                    }
                }
                btnPost.Visible = true;
                trans.Commit();
                conn.Close();
                master.messageBox("Data has been update. Trans No: " + txtTransNo.Text + "");
            }

            //_ht.Clear();
            //_ht["p_rv_no"] = txtTransNo.Text;
            //Shared.DefaultHashtable(_ht);

            //_dal.Delete(TABLE_NAME_DETAIL, _ht);

            //foreach (GridViewRow gridRow in grid.Rows)
            //{
            //    if (((CheckBox)gridRow.FindControl("chbPaid")).Checked == true)
            //    {
            //        _ht["p_id"] = gridRow.RowIndex;
            //        _ht["p_reff_no"] = ((LinkButton)gridRow.FindControl("libReffNo")).Text;
            //        _ht["p_base_currency"] = "IDR";
            //        _ht["p_base_amount"] = ((LinkButton)gridRow.FindControl("libBaseAmount")).Text;
            //        _ht["p_amount_paid"] = ((TextBox)gridRow.FindControl("txtReceiptAmount")).Text;
            //        _ht["p_total_disc"] = ((LinkButton)gridRow.FindControl("libTotalDiscount")).Text;

            //        _dal.Insert(TABLE_NAME_DETAIL, _ht);
            //    }
            //}

            //_ht.Clear();
            //_ht["p_rv_no"] = txtTransNo.Text;
            //Shared.DefaultHashtable(_ht);

            //BindData();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            conn.Close();
            master.messageBox("Fail To Execute" + "<br />" + ex.InnerException);
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
            sql.Append("EXEC sp_rv_header_post '" + txtTransNo.Text + "','" + cAdih.engFormatDate(txtReceiptDate.Text) + "','" + Session["username"].ToString() + "','" + DateTime.Now + "','" + cAdih.getIPAddress() + "' ");

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

    private void setTotalReceipt()
    {
        decimal dTotalReceipt = 0;

        foreach (GridViewRow gridRow in grid.Rows)
        {
            if (((CheckBox)gridRow.FindControl("chbPaid")).Checked == true)
            {
                dTotalReceipt += decimal.Parse(((TextBox)gridRow.FindControl("txtReceiptAmount")).Text);
            }
        }
        txtAmount.Text = dTotalReceipt.ToString("##,#0.#0");
        upHeader.Update();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        decimal dTotalReceipt = 0;

        foreach (GridViewRow gridRow in grid.Rows)
        {
            if (((CheckBox)gridRow.FindControl("chbPaid")).Checked == true)
            {
                LinkButton lbReffNo = (LinkButton)gridRow.FindControl("libReffNo");
                LinkButton lbBaseAmount = (LinkButton)gridRow.FindControl("libBaseAmount");
                TextBox txtReceiptAmount = (TextBox)gridRow.FindControl("txtReceiptAmount");
                TextBox lbTotalDiscount = (TextBox)gridRow.FindControl("libTotalDiscount");

                if (decimal.Parse(lbBaseAmount.Text) < (decimal.Parse(txtReceiptAmount.Text) + decimal.Parse(lbTotalDiscount.Text)))
                {
                    master.messageBox("receipt amount + total discount can't be greater than outstanding amount! ");
                    return;
                }

                dTotalReceipt += decimal.Parse(txtReceiptAmount.Text);
            }
        }

        if (dTotalReceipt != decimal.Parse(txtAmount.Text))
        {
            master.messageBox("please calculate first! ");
            return;
        }

        if ((decimal.Parse(txtAmount.Text) - decimal.Parse(txtAdminFee.Text) + decimal.Parse(txtOverpayment.Text)) != decimal.Parse(txtTotal.Text))
        {
            master.messageBox("si amount - admin fee + overpayment does'nt match with mutation amount! ");
            return;
        }

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
        Response.Redirect("CustomerReceiptList.aspx");
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
                    PostData();
                else
                    saveData();
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void imbCustomerNo_OnClick(object sender, EventArgs e)
    {
        Session["search"] = "customer";
        wucSearch.loadGrid();
    }

    protected void libCustomerNo_OnClick(object sender, EventArgs e)
    {
        txtCustomerNo.Text = "";
        libCustomerNo.Text = "";
        txtBillTo.Text = "";
        txtBillToDesc.Text = "";
        imbCustomerNo.Visible = true;
        imbBillTo.Visible = true;
        upHeader.Update();

        grid.Columns[1].Visible = true;
        grid.Columns[7].Visible = true;

        grid.DataSource = null;
        grid.DataBind();
        upGrid.Update();
    }

    protected void imbBillTo_OnClick(object sender, EventArgs e)
    {
        if (txtCustomerNo.Text.Trim() == "")
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox("Input rent to!");
            return;
        }
        else
        {
            Session["searchParamCustomerNo"] = txtCustomerNo.Text.Trim();
            Session["search"] = "customer_bill_to";
            wucSearch.loadGrid();
        }
    }

    protected void getDetail(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("select	si.trans_no,  (si.total_amount - isnull((select sum(payment_amount) from sales_invoice_partial_payment where trans_no = si.trans_no), 0)) as outstanding,    ");
            sql.Append("si.trans_date, od.reff as od_reff,'OUTBOUND DELIVERY' as reff, si.total_amount, si.bill_to,'IDR' as CURRENCY_ID, si.note, isnull(si.tax_facture_no, '') as tax_facture_no  ");
            sql.Append("from dbo.sales_invoice si  ");
            sql.Append("left join outbound_delivery od with(readpast) on si.reff_order_no = od.trans_no  ");
            sql.Append("and	si.bill_to = '" + txtBillTo.Text + "' ");
            sql.Append("and si.status = 1 ");
            sql.Append("and	si.bill_to = '" + txtBillTo.Text + "' ");
            sql.Append("and si.total_amount <> 0  ");
            sql.Append("group by si.trans_no, si.trans_date, si.total_amount, od.reff, od.reff, si.bill_to, si.note, tax_facture_no  ");
            sql.Append("having(si.total_amount <> isnull((select sum(payment_amount) from sales_invoice_partial_payment where trans_no = si.trans_no), 0))  ");

            using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
            {
                conn.Open();
                using (cmd = new SqlCommand(sql.ToString(), conn))
                {
                    using (dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt = createDataTable();
                            Session["dt"] = dt;

                            int line_id = 1;

                            while (dr.Read())
                            {

                               addDataToTable(line_id, dr["trans_no"].ToString(), dr["reff"].ToString(), dr["od_reff"].ToString(), DateTime.Parse(dr["trans_date"].ToString()),
                               double.Parse(dr["total_amount"].ToString()), dr["currency_id"].ToString(), double.Parse(dr["outstanding"].ToString()), dr["tax_facture_no"].ToString(), (DataTable)Session["dt"]);

                                line_id++;
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

    protected void imbBank_OnClick(object sender, EventArgs e)
    {
        //MasterPage master = (MasterPage)this.Master;
        //ExecSP _dal = null;
        //Hashtable _ht = null;

        //try
        //{
        //    _dal = new ExecSP();
        //    _ht = new Hashtable();

        //    _ht["p_site_id"] = txtSiteID.Text;

        //    wuclookup1.loadGrid("LU00008", _ht);

        //}
        //catch (Exception ex)
        //{
        //    master.messageBox(ex.Message + "<br />" + ex.InnerException);
        //}
    }

    protected void imbCalculate_OnClick(object sender, EventArgs e)
    {
        setTotalReceipt();
    }

    protected DataTable createDataTable()
    {
        try
        {
            dt = new DataTable();

            DataColumn dtColumn;

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Int64");
            dtColumn.ColumnName = "id";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "is_paid";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "reff";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "reff_no";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "od_reff";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.DateTime");
            dtColumn.ColumnName = "reff_date";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "si_amount";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "base_currency";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "base_amount";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "amount_paid";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "total_discount";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "tax_facture_no";
            dt.Columns.Add(dtColumn);

            return dt;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private void addDataToTable(int id, string reff_no, string reff, string od_reff, DateTime reff_date, double si_amount, string base_currency, double base_amount,  string tax_facture_no, DataTable Table)
    {
        try
        {
            DataRow dtRow;

            dtRow = Table.NewRow();

            dtRow["id"] = id;
            dtRow["is_paid"] = "0";
            dtRow["reff"] = reff;
            dtRow["reff_no"] = reff_no;
            dtRow["od_reff"] = od_reff;
            dtRow["reff_date"] = reff_date;
            dtRow["si_amount"] = si_amount;
            dtRow["base_currency"] = base_currency;
            dtRow["base_amount"] = base_amount;
            dtRow["amount_paid"] = "0.00";
            dtRow["total_discount"] = "0.00";
            dtRow["tax_facture_no"] = tax_facture_no;

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

            //if (grid.Rows.Count >= 1)
            //{
            //    txtAmount.Text = Double.Parse(((DataTable)Session["dt"]).Compute("SUM(base_amount)", "").ToString()).ToString("#0.#0");
            //}

            upHeader.Update();
            upGrid.Update();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void bSearchSoldTo_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT a.customer_name, a.currency_id FROM customer a WITH(READPAST) ");
            sql.Append("WHERE a.customer_no = '" + txtCustomerNo.Text.Trim().Replace("'", "`") + "' ");
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
                            libCustomerNo.Text = dr["customer_name"].ToString();
                            imbCustomerNo.Visible = false;
                        }
                        upHeader.Update();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void bSearchBillTo_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT (CASE WHEN a.alias_name = '' THEN a.alias_name_full ELSE a.alias_name_full + ' ' + a.alias_name END) AS alias_name ");
            sql.Append("FROM customer_address a WITH(READPAST) ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append("AND a.customer_no = '" + txtCustomerNo.Text.Trim().Replace("'", "`") + "' ");
            sql.Append("AND a.id = '" + txtBillTo.Text.Trim().Replace("'", "`") + "' ");

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
                            txtBillToDesc.Text = dr["alias_name"].ToString();
                        }
                        upHeader.Update();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void wucLookup_Hide(object sender, EventArgs e)
    {
        if (Session["search"].ToString() == "customer")
        {
            txtCustomerNo.Text = wucSearch.result;
            bSearchSoldTo_Click(sender, e);
            upHeader.Update();
        }
        else if (Session["search"].ToString() == "customer_bill_to")
        {
            txtBillTo.Text = wucSearch.result;
            imbBillTo.Visible = false;
            bSearchBillTo_Click(sender, e);
            getDetail(sender, e);
            upHeader.Update();
            Session["searchParamCustomerNo"] = null;
        }
        //if (Session["lookupcode"].ToString() == "LU00009")
        //{
        //    txtCustomerNo.Text = wuclookup1.result[0].ToString();
        //    libCustomerNo.Text = wuclookup1.result[1].ToString();
        //    imbCustomerNo.Visible = false;

        //}
        //else if (Session["lookupcode"].ToString() == "LU00010")
        //{
        //    txtBillTo.Text = wuclookup1.result[0].ToString();
        //    txtBillToDesc.Text = wuclookup1.result[1].ToString();

        //    imbBillTo.Visible = false;
        //    getDetail(sender, e);
        //}
        //else if (Session["lookupcode"].ToString() == "LU00008")
        //{
        //    txtBank.Text = wuclookup1.result[0].ToString();
        //    libBankName.Text = wuclookup1.result[1].ToString();
        //    libBankAccountNo.Text = wuclookup1.result[2].ToString();
        //    imbBank.Visible = false;
        //}
        upHeader.Update();
    }

    #region Detail
    private void BindData()
    {
        sql.Length = 0;
        sql.Append("exec sp_rv_detail_getrows '" + txtTransNo.Text + "','" + txtCustomerNo.Text + "','" + txtBillTo.Text + "' ");

        cAdih.loadGridView(sql.ToString(), cAdih.getConnStr("Connection"), grid);
        upGrid.Update();
    }


    protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grid.PageIndex = e.NewPageIndex;
    }

    protected void chbPaid_OnCheckedChanged(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as CheckBox).Parent.Parent as GridViewRow);

        if (((CheckBox)gridRow.FindControl("chbPaid")).Checked == false)
        {
            Session["row_index"] = gridRow.RowIndex;
            Session["reff_si_no"] = ((LinkButton)gridRow.FindControl("libReffNo")).Text;

            ((TextBox)gridRow.FindControl("txtReceiptAmount")).Enabled = false;
            ((TextBox)gridRow.FindControl("txtReceiptAmount")).Text = "0.00";
            ((TextBox)gridRow.FindControl("libTotalDiscount")).Enabled = false;
            ((TextBox)gridRow.FindControl("libTotalDiscount")).Text = "0.00";
        }
        else if (((CheckBox)gridRow.FindControl("chbPaid")).Checked == true)
        {
            ((TextBox)gridRow.FindControl("txtReceiptAmount")).Enabled = true;
            ((TextBox)gridRow.FindControl("libTotalDiscount")).Enabled = true;
            ((TextBox)gridRow.FindControl("txtReceiptAmount")).Text = ((LinkButton)gridRow.FindControl("libBaseAmount")).Text;
            ((TextBox)gridRow.FindControl("libTotalDiscount")).Text = "0.00";
        }
    }

    protected void grid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (grid.DataKeys[e.Row.RowIndex][1].ToString() == "1")
            {
                ((CheckBox)e.Row.FindControl("chbPaid")).Checked = true;
                ((TextBox)e.Row.FindControl("txtReceiptAmount")).Enabled = true;
                ((TextBox)e.Row.FindControl("libTotalDiscount")).Enabled = true;
            }
            else
            {
                ((TextBox)e.Row.FindControl("txtReceiptAmount")).Enabled = false;
                ((TextBox)e.Row.FindControl("libTotalDiscount")).Enabled = false;
            }


            if (lblStatus.Text == "POSTED")
            {
                ((TextBox)e.Row.FindControl("txtReceiptAmount")).Enabled = false;
                ((TextBox)e.Row.FindControl("libTotalDiscount")).Enabled = false;
            }
        }
    }

    #endregion

}