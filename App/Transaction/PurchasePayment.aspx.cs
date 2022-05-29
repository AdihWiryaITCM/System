using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transaction_PurchasePayment : Page
{
    private StringBuilder sql = new StringBuilder();
    private StringBuilder sql1 = new StringBuilder();
    private SqlTransaction trans;
    private SqlTransaction trans1;
    private SqlConnection conn;
    private SqlConnection conn1;
    private SqlCommand cmd;
    private SqlCommand cmd1;
    private SqlDataReader _dr;
    private SqlDataReader dr;
    private DataTable dt;
    private DataTable dt1;
    private ClassAdih cAdih = new ClassAdih();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.Params["action"].Equals("edit"))
            {
                Session["SaveMode"] = "edit";
                txtTransNo.Text = Request.Params["pvno"].ToString();
                LoadData();
                BindData();

                if (lblStatus.Text == "POSTED")
                {
                    grid.Columns[1].Visible = false;

                    btnSave.Visible = false;
                    btnPost.Visible = false;

                    pnlHeader.Enabled = false;
                }
            }
            else
            {
                Session["SaveMode"] = "add";

                txtPVDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtPaymentDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }
        }
    }

    private void LoadData()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {

            sql.Length = 0;
            sql.Append("select	trans_no as pv_no, ");
            sql.Append("trans_date as pv_date, ");
            sql.Append("ph.status, ");
            sql.Append("payment_date, ");
            sql.Append("ph.vendor_no, ");
            sql.Append("ph.admin_fee, ");
            sql.Append("note, ");
            sql.Append("isnull(ORIG_AMOUNT,0) as orig_amount, ");
            sql.Append("isnull(base_amount,0) as amount, ");
            sql.Append("ph.to_bank_account_name, ");
            sql.Append("ph.to_bank_account_no, ");
            sql.Append("ph.created_date, ");
            sql.Append("ph.posted_date, ");
            sql.Append("ISNULL(ve.vendor_name, '') 'vendor_name', ");
            sql.Append("ISNULL(ve.street_address, '') 'street_address' ");
            sql.Append("from dbo.purchase_payment ph ");
            sql.Append("LEFT JOIN dbo.vendor ve on(ve.vendor_no = ph.vendor_no) ");
            sql.Append("WHERE trans_no = '" + txtTransNo.Text +"' ");

            using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
            {
                conn.Open();
                using (cmd = new SqlCommand(sql.ToString(), conn))
                {
                    using (_dr = cmd.ExecuteReader())
                    {
                        if (_dr.HasRows)
                        {
                            _dr.Read();
                            txtPVDate.Text = Shared.DBtoUIDate(_dr["PV_DATE"].ToString());
                            txtPaymentDate.Text = Shared.DBtoUIDate(_dr["PAYMENT_DATE"].ToString());
                            txtVendorNo.Text = _dr["VENDOR_NO"].ToString();
                            libVendorNo.Text = _dr["VENDOR_NAME"].ToString();
                            imbVendorNo.Visible = false;
                            txtToBank.Text = _dr["to_bank_account_no"].ToString();
                            libToBankAccountNo.Text = _dr["TO_BANK_ACCOUNT_NAME"].ToString();
                            ddlCurrency.SelectedValue = "IDR";
                            txtTotal.Text = decimal.Parse(_dr["ORIG_AMOUNT"].ToString()).ToString("##,#0.#0");
                            txtNote.Text = _dr["NOTE"].ToString();
                            txtAmount.Text = decimal.Parse(_dr["AMOUNT"].ToString()).ToString("##,#0.#0");
                            txtAdminFee.Text = decimal.Parse(_dr["ADMIN_FEE"].ToString()).ToString("##,#0.#0");
                            lblStatus.Text = _dr["STATUS"].ToString();
                        }
                        upHeader.Update();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message + "<br />" + ex.InnerException);
        }
    }

    private void saveData()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (txtTransNo.Text == "")
            {
                conn = new SqlConnection(cAdih.getConnStr("Connection"));
                conn.Open();
                trans = conn.BeginTransaction();

             
                sql.Length = 0;
                sql.Append("SELECT RIGHT('000000' + CAST(ISNULL(MAX(CAST(RIGHT(trans_no, 6) AS INT)), 0) + 1 AS VARCHAR), 6) AS running_no,  ");
                sql.Append("YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) AS tahun, RIGHT('00' + CAST(MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) AS VARCHAR), 2) AS bulan ");
                sql.Append("FROM purchase_payment ");
                sql.Append("WHERE YEAR(trans_date) = YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) ");
                sql.Append("AND MONTH(trans_date) = MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    using (dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            txtTransNo.Text = "PP" + dr["tahun"].ToString() + dr["bulan"].ToString() + dr["running_no"].ToString();
                            upHeader.Update();
                        }
                    }
                }

                sql.Length = 0;
                sql.Append("INSERT INTO [dbo].[purchase_payment] ");
                sql.Append("([trans_no] "); 
                sql.Append(",[trans_date] ");
                sql.Append(",[status] ");
                sql.Append(",[payment_date] ");
                sql.Append(",[vendor_no] ");
                sql.Append(",[base_amount] ");
                sql.Append(",[orig_amount] ");
                sql.Append(",[note] ");
                sql.Append(",[posted_by] ");
                sql.Append(",[posted_date] ");
                sql.Append(",[created_by] ");
                sql.Append(",[created_date] ");
                sql.Append(",[created_ip_address] ");
                sql.Append(",[mod_by] ");
                sql.Append(",[mod_date] ");
                sql.Append(",[mod_ip_address] ");
                sql.Append(",[admin_fee] ");
                sql.Append(",[to_bank_account_no] ");
                sql.Append(",[to_bank_account_name]) ");
                sql.Append("VALUES (");
                sql.Append("'"+ txtTransNo.Text +"'");
                sql.Append(",'"+ cAdih.engFormatDate(txtPVDate.Text) + "'");
                sql.Append(",'"+ "HOLD" +"'");
                sql.Append(",'"+ cAdih.engFormatDate(txtPaymentDate.Text) + "'");
                sql.Append(",'"+ txtVendorNo.Text +"'");
                sql.Append(",'"+ double.Parse(txtAmount.Text) + "'");
                sql.Append(",'" + double.Parse(txtTotal.Text) + "'");
                sql.Append(",'" + txtNote.Text + "'");
                sql.Append(",''");
                sql.Append(",''");
                sql.Append(",'"+ Session["username"].ToString() +"'");
                sql.Append(",GETDATE() ");
                sql.Append(",'"+ cAdih.getIPAddress() +"'");
                sql.Append(",'" + Session["username"].ToString() + "'");
                sql.Append(",GETDATE() ");
                sql.Append(",'" + cAdih.getIPAddress() + "'");
                sql.Append(",'"+ double.Parse(txtAdminFee.Text) + "'");
                sql.Append(",'"+ txtToBank.Text +"'");
                sql.Append(",'"+ libToBankAccountNo.Text +"')");

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
                        sql.Append("INSERT INTO[dbo].[purchase_payment_detail] ");
                        sql.Append("([trans_no] ");
                        sql.Append(",[reff_no] ");
                        sql.Append(",[base_amount] ");
                        sql.Append(",[created_by] ");
                        sql.Append(",[created_date] ");
                        sql.Append(",[created_ip_address] ");
                        sql.Append(",[mod_by] ");
                        sql.Append(",[mod_date] ");
                        sql.Append(",[mod_ip_address] ");
                        sql.Append(",[amount_paid] ");
                        sql.Append(",[disc_amount]) VALUES ( ");
                        sql.Append("'" + txtTransNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + ((LinkButton)gridRow.FindControl("libReffNo")).Text + "', ");
                        sql.Append("'" + double.Parse(((LinkButton)gridRow.FindControl("libBaseAmount")).Text) + "', ");
                        sql.Append("'" + Session["username"].ToString() + "', ");
                        sql.Append("GETDATE(), ");
                        sql.Append("'" + cAdih.getIPAddress() + "', ");
                        sql.Append("'" + Session["username"].ToString() + "', ");
                        sql.Append("GETDATE(), ");
                        sql.Append("'" + cAdih.getIPAddress() + "', ");
                        sql.Append("'" + double.Parse(((TextBox)gridRow.FindControl("txtPaymentAmount")).Text) + "', ");
                        sql.Append("'" + double.Parse(((TextBox)gridRow.FindControl("libTotalDiscount")).Text) + "') ");

                        using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                        {
                            cmd.ExecuteNonQuery();
                            cmd = null;
                        }
                    }
                }

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
                sql.Append("UPDATE [dbo].[purchase_payment] ");
                sql.Append("SET [payment_date] = '" + cAdih.engFormatDate(txtPaymentDate.Text) + "' ");
                sql.Append(",[vendor_no]            = '" + txtVendorNo.Text +"'");
                sql.Append(",[base_amount]          = '" + double.Parse(txtAmount.Text) + "'");
                sql.Append(",[orig_amount]          = '" + double.Parse(txtTotal.Text) + "'");
                sql.Append(",[note]                 = '" + txtNote.Text +"'");
                sql.Append(",[mod_by]               = '" + Session["username"].ToString()  + "'");
                sql.Append(",[mod_date]             = GETDATE() ");
                sql.Append(",[mod_ip_address]       = '" + cAdih.getIPAddress() +"'");
                sql.Append(",[admin_fee]            = '" + double.Parse(txtAdminFee.Text) + "'");
                sql.Append(",[to_bank_account_no]   = '" + txtToBank.Text  + "'");
                sql.Append(",[to_bank_account_name] = '" + libToBankAccountNo.Text  + "'");
                sql.Append("WHERE trans_no = '" + txtTransNo.Text +"' ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }


                sql.Append("DELETE purchase_payment_detail WHERE trans_no = '" + txtTransNo.Text + "' ");

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
                        sql.Append("INSERT INTO[dbo].[purchase_payment_detail] ");
                        sql.Append("([trans_no] ");
                        sql.Append(",[reff_no] ");
                        sql.Append(",[base_amount] ");
                        sql.Append(",[created_by] ");
                        sql.Append(",[created_date] ");
                        sql.Append(",[created_ip_address] ");
                        sql.Append(",[mod_by] ");
                        sql.Append(",[mod_date] ");
                        sql.Append(",[mod_ip_address] ");
                        sql.Append(",[amount_paid] ");
                        sql.Append(",[disc_amount]) VALUES ( ");
                        sql.Append("'" + txtTransNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + ((LinkButton)gridRow.FindControl("libReffNo")).Text + "', ");
                        sql.Append("'" + double.Parse(((LinkButton)gridRow.FindControl("libBaseAmount")).Text) + "', ");
                        sql.Append("'" + Session["username"].ToString() + "', ");
                        sql.Append("GETDATE(), ");
                        sql.Append("'" + cAdih.getIPAddress() + "', ");
                        sql.Append("'" + Session["username"].ToString() + "', ");
                        sql.Append("GETDATE(), ");
                        sql.Append("'" + cAdih.getIPAddress() + "', ");
                        sql.Append("'" + double.Parse(((TextBox)gridRow.FindControl("txtPaymentAmount")).Text) + "', ");
                        sql.Append("'" + double.Parse(((TextBox)gridRow.FindControl("libTotalDiscount")).Text) + "') ");

                        using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                        {
                            cmd.ExecuteNonQuery();
                            cmd = null;
                        }
                    }
                }

                trans.Commit();
                conn.Close();
                master.messageBox("Data has been saved. Trans No: " + txtTransNo.Text + "");
            }
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message + "<br />" + ex.InnerException);
            trans.Rollback();
            conn.Close();
        }
    }

    private void PostData()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            conn = new SqlConnection(cAdih.getConnStr("Connection"));
            conn.Open();
            trans = conn.BeginTransaction();
            sql.Append("EXEC [dbo].[sp_pv_header_post] '"+ txtTransNo.Text +"','"+ cAdih.engFormatDate(txtPaymentDate.Text) + "','"+ Session["username"].ToString() +"','"+ DateTime.Now +"','"+ cAdih.getIPAddress() +"' ");

            using (cmd = new SqlCommand(sql.ToString(), conn, trans))
            {
                cmd.ExecuteNonQuery();
                cmd = null;
            }
            trans.Commit();
            conn.Close();


            master.messageBox("Data has been posted.");

            Response.Redirect("PurchasePayment.aspx?action=edit&pvno=" + txtTransNo.Text + "");
        }
        catch (Exception ex)
        {
            trans.Rollback();
            conn.Close();
            master.messageBox("Fail To Execute" + "<br />" + ex.InnerException);
        }
    }

    private void setTotalReceipt()
    {
        decimal dTotalPayment = 0;

        foreach (GridViewRow gridRow in grid.Rows)
        {
            if (((CheckBox)gridRow.FindControl("chbPaid")).Checked == true)
            {
                dTotalPayment += decimal.Parse(((TextBox)gridRow.FindControl("txtPaymentAmount")).Text);
            }
        }
        txtAmount.Text = dTotalPayment.ToString("##,#0.#0");
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
                TextBox txtPaymentAmount = (TextBox)gridRow.FindControl("txtPaymentAmount");

                if (decimal.Parse(lbBaseAmount.Text) < (decimal.Parse(txtPaymentAmount.Text)))
                {
                    master.messageBox("payment amount + total discount TTF no. " + lbReffNo.Text + " can't be greater than outstanding amount! ");
                    return;
                }

                dTotalReceipt += decimal.Parse(txtPaymentAmount.Text);
            }
        }

        if (dTotalReceipt != decimal.Parse(txtAmount.Text))
        {
            master.messageBox("please calculate first! ");
            return;
        }

        if ((decimal.Parse(txtAmount.Text) + decimal.Parse(txtAdminFee.Text)) != decimal.Parse(txtTotal.Text))
        {
            master.messageBox("pi amount + admin fee does'nt match with mutation amount! ");
            return;
        }

        Session["ConfirmMode"] = "Save";
        showConfirmBox("Save This Data?");
    }

    protected void btnPost_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;

        if (lblStatus.Text == "")
        {
            master.messageBox("Save Data First!");
        }
        else
        {
            Session["ConfirmMode"] = "Post";
            showConfirmBox("Are you sure post this data?");
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("PurchasePaymentList.aspx");
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
                else
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

    protected void imbVendorNo_OnClick(object sender, EventArgs e)
    {
        Session["search"] = "vendor";
        wucSearch1.loadGrid();
    }

    protected void wucSearch_Hide(object sender, EventArgs e)
    {
        if (Session["search"].ToString() == "vendor")
        {
            txtVendorNo.Text = wucSearch1.result;
            bSearchVendor_Click(sender, e);
            upHeader.Update();
        }
        Session["search"] = null;
    }

    protected void bSearchVendor_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT a.vendor_name, 'IDR' as currency_id,bank_account_no,bank_name ");
            sql.Append("FROM vendor a WITH(READPAST) ");
            sql.Append("WHERE a.status = '1' ");

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
                            libVendorNo.Text = dr["vendor_name"].ToString();
                            txtToBank.Text = dr["bank_account_no"].ToString();
                            libToBankAccountNo.Text = dr["bank_name"].ToString();
                            txtVendorNo.ReadOnly = true;
                            imbVendorNo.Visible = false;
                            getDetail(sender, e);
                        }
                        else
                        {
                            txtVendorNo.Text = "";
                            txtVendorNo.ReadOnly = false;
                            imbVendorNo.Visible = true;
                            master.messageBox("Invalid vendor!");
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

    protected void libVendorNo_OnClick(object sender, EventArgs e)
    {
        txtVendorNo.Text = "";
        libVendorNo.Text = "";
        imbVendorNo.Visible = true;
        txtToBank.Text = "";
        libToBankAccountNo.Text = "";
        upHeader.Update();

        grid.Columns[1].Visible = true;
        grid.Columns[7].Visible = true;

        grid.DataSource = null;
        grid.DataBind();
        upGrid.Update();
    }

    protected void getDetail(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("select trans_no, vendor_invoice_no, 'IDR' currency_id, due_date, total_amount,  ");
            sql.Append("(total_amount - isnull((select sum(payment_amount) from invoice_confirmation_receipt_partial_payment pp where pp.trans_no = cr.trans_no), 0)) as outstanding ");
            sql.Append("from dbo.invoice_confirmation_receipt cr ");
            sql.Append("where vendor_no = '" + txtVendorNo.Text + "' ");
            sql.Append("and	status = 1  ");
            sql.Append("group BY trans_no, cr.total_amount, cr.vendor_invoice_no, cr.due_date ");
            sql.Append("HAVING total_amount <> isnull((select sum(payment_amount) from invoice_confirmation_receipt_partial_payment pp where pp.trans_no = cr.trans_no), 0.01) ");
            sql.Append("ORDER BY cr.due_date, cr.trans_no ");

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
                                addDataToTable(line_id, dr["trans_no"].ToString(), dr["vendor_invoice_no"].ToString(), double.Parse(dr["total_amount"].ToString()), DateTime.Parse(dr["due_date"].ToString()),
                                    dr["currency_id"].ToString(), double.Parse(dr["outstanding"].ToString()), (DataTable)Session["dt"]);

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
            dtColumn.ColumnName = "reff_no";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "vendor_invoice_no";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "pi_amount";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.DateTime");
            dtColumn.ColumnName = "reff_date";
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

            return dt;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private void addDataToTable(int id, string reff_no, string vendor_invoice_no, double pi_amount, DateTime reff_date, string base_currency, double base_amount, DataTable Table)
    {
        try
        {
            DataRow dtRow;

            dtRow = Table.NewRow();

            dtRow["id"] = id;
            dtRow["is_paid"] = "0";
            dtRow["reff_no"] = reff_no;
            dtRow["reff_date"] = reff_date;
            dtRow["vendor_invoice_no"] = vendor_invoice_no;
            dtRow["pi_amount"] = pi_amount;
            dtRow["base_currency"] = base_currency;
            dtRow["base_amount"] = base_amount;
            dtRow["amount_paid"] = "0.00";
            dtRow["total_discount"] = "0.00";

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

    #region Detail

    private void BindData()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = createDataTable();
            Session["dt"] = dt;

            sql.Length = 0;
            sql.Append("exec [dbo].[sp_pv_detail_getrows] '" + txtTransNo.Text + "','" + txtVendorNo.Text + "' ");

            cAdih.loadGridView(sql.ToString(), cAdih.getConnStr("Connection"), grid);

            upHeader.Update();
            upGrid.Update();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message + "<br />" + ex.InnerException);
        }
    }

    protected void lbDelete_OnClick(object sender, EventArgs e)
    {
        //ExecSP _dal = null;
        //Hashtable _ht = null;

        //try
        //{
        //    _dal = new ExecSP();
        //    _ht = new Hashtable();

        //    GridView grid = ((sender as LinkButton).Parent.Parent.Parent.Parent as GridView);
        //    GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);

        //    _ht["p_id"] = grid.DataKeys[gridRow.RowIndex][0].ToString();

        //    _dal.Delete(TABLE_NAME_DETAIL, _ht);

        //    BindData();
        //}
        //catch (Exception ex)
        //{
        //    MasterPage master = (MasterPage)this.Master;
        //    master.messageBox(ex.Message + "<br />" + ex.InnerException);
        //}
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

            ((TextBox)gridRow.FindControl("txtPaymentAmount")).Enabled = false;
            ((TextBox)gridRow.FindControl("txtPaymentAmount")).Text = "0.00";
            ((TextBox)gridRow.FindControl("libTotalDiscount")).Enabled = false;
            ((TextBox)gridRow.FindControl("libTotalDiscount")).Text = "0.00";
        }
        else if (((CheckBox)gridRow.FindControl("chbPaid")).Checked == true)
        {
            ((TextBox)gridRow.FindControl("txtPaymentAmount")).Enabled = true;
            ((TextBox)gridRow.FindControl("txtPaymentAmount")).Text = ((LinkButton)gridRow.FindControl("libBaseAmount")).Text;
            ((TextBox)gridRow.FindControl("libTotalDiscount")).Enabled = true;
            ((TextBox)gridRow.FindControl("libTotalDiscount")).Text = "0.00";
        }
        //calculate_paidAmount();
    }

    protected void grid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (grid.DataKeys[e.Row.RowIndex][1].ToString() == "1")
            {
                ((CheckBox)e.Row.FindControl("chbPaid")).Checked = true;

                ((TextBox)e.Row.FindControl("txtPaymentAmount")).Enabled = true;
                ((TextBox)e.Row.FindControl("libTotalDiscount")).Enabled = true;
            }
            else
            {
                ((TextBox)e.Row.FindControl("txtPaymentAmount")).Enabled = false;
                ((TextBox)e.Row.FindControl("libTotalDiscount")).Enabled = false;
            }

            if (lblStatus.Text == "POSTED")
            {
                ((TextBox)e.Row.FindControl("txtPaymentAmount")).Enabled = false;
                ((TextBox)e.Row.FindControl("libTotalDiscount")).Enabled = false;
            }
        }
    }

    #endregion Detail
}