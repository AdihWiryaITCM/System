using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Collections;


public partial class Transaction_RentalQuotationProcess : System.Web.UI.Page
{
    StringBuilder sql = new StringBuilder();
    StringBuilder sql1 = new StringBuilder();
    StringBuilder sql2 = new StringBuilder();
    SqlTransaction trans;
    SqlTransaction trans1;
    SqlTransaction trans2;
    SqlConnection conn;
    SqlConnection conn1;
    SqlConnection conn2;
    SqlCommand cmd;
    SqlCommand cmd1;
    SqlCommand cmd2;
    SqlDataReader dr;
    SqlDataReader dr1;
    SqlDataReader dr2;
    DataTable dt;
    ClassAdih cAdih = new ClassAdih();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            MasterPage master = (MasterPage)this.Master;
                
            try
            {
                DataTable dt = new DataTable();
                dt = createDataTable();
                Session["dtQuotRental"] = dt;
               
                if (Request.QueryString["mode"].ToString() == "add")
                {
                    tbTransDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    tbCurrency.Text = "IDR";
                    lStatus.Text = "";
                    statusChanged(lStatus.Text);

                }
                else if (Request.QueryString["mode"].ToString() == "edit")
                {
                    tbTransNo.Text = Request.QueryString["trans_no"].ToString();

                    sql.Length = 0;
                    sql.Append("SELECT a.trans_no, a.trans_date, a.status, ");
                    sql.Append("a.sold_to, c.customer_name AS sold_to_name, a.bill_to, (CASE WHEN d.alias_name = '' THEN d.alias_name_full ELSE d.alias_name_full + ' ' + d.alias_name END) AS bill_to_name, ");
                    sql.Append("a.ship_to, (CASE WHEN e.alias_name = '' THEN e.alias_name_full ELSE e.alias_name_full + ' ' + e.alias_name END) AS ship_to_name, ");
                    sql.Append("a.currency_id, a.note, a.payment_term, h.description AS payment_term_desc, a.status, a.installation_charge, a.amount,a.tax, a.total_amount,a.delivery_date ");
                    sql.Append("FROM quotation_rental a WITH(READPAST) ");
                    sql.Append("INNER JOIN customer c WITH(READPAST) ON a.sold_to = c.customer_no ");
                    sql.Append("INNER JOIN customer_address d WITH(READPAST) ON a.bill_to = d.id AND d.customer_no = a.sold_to AND d.address_type = '01' ");
                    sql.Append("INNER JOIN customer_address e WITH(READPAST) ON a.ship_to = e.id AND e.customer_no = a.sold_to AND e.address_type = '02' ");
                    sql.Append("INNER JOIN payment_term h WITH(READPAST) ON a.payment_term = h.id ");
                    sql.Append("WHERE 1 = 1 ");
                    sql.Append("AND a.trans_no = '" + tbTransNo.Text + "' ");

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
                                    tbTransDate.Text = DateTime.Parse(dr["trans_date"].ToString()).ToString("dd-MM-yyyy");
                                    tbInstallationDate.Text = DateTime.Parse(dr["delivery_date"].ToString()).ToString("dd-MM-yyyy");
                                    if (dr["status"].ToString() == "0")
                                    {
                                        lStatus.Text = "OPEN";
                                    }
                                    else if (dr["status"].ToString() == "1")
                                    {
                                        lStatus.Text = "POSTED";
                                    }
                                    statusChanged(lStatus.Text);
                                    tbSoldTo.Text = dr["sold_to"].ToString();
                                    tbSoldTo.ReadOnly = true;
                                    lbSoldTo.Text = dr["sold_to_name"].ToString();
                                    ibSearchSoldTo.Visible = false;
                                    tbBillTo.Text = dr["bill_to"].ToString();
                                    tbBillTo.ReadOnly = true;
                                    lbBillTo.Text = dr["bill_to_name"].ToString();
                                    ibSearchBillTo.Visible = false;
                                    tbShipTo.Text = dr["ship_to"].ToString();
                                    tbShipTo.ReadOnly = true;
                                    lbShipTo.Text = dr["ship_to_name"].ToString();
                                    ibSearchShipTo.Visible = false;
                                    tbInstallationCharge.Text = double.Parse(dr["installation_charge"].ToString()).ToString("##,#0.#0");
                                    tbAmt.Text = double.Parse(dr["amount"].ToString()).ToString("##,#0.#0");
                                    tbTaxAmt.Text = double.Parse(dr["tax"].ToString()).ToString("##,#0.#0");
                                    tbCurrency.Text = dr["currency_id"].ToString();
                                    tbNetValue.Text = double.Parse( dr["total_amount"].ToString()).ToString("##,#0.#0");
                                    tbNote.Text = dr["note"].ToString();
                                    tbPaymentTerm.Text = dr["payment_term"].ToString();
                                    tbPaymentTerm.ReadOnly = true;
                                    lbPaymentTerm.Text = dr["payment_term_desc"].ToString();
                                    ibSearchPaymentTerm.Visible = false;
                                    bSearchSoldTo_Click(bSearchSoldTo, new EventArgs());
                                }
                            }
                        }
                    }

                    lblTRate.Text = "10";

                    sql.Length = 0;
                    sql.Append("SELECT a.line_no, b.article_type, a.article_no, b.article_description,");
                    sql.Append("a.qty, a.rate, a.disc, a.tax, a.rent_duration as time_duration,qty*(rate+tax)*rent_duration as total ");
                    sql.Append("FROM quotation_rental_detail a WITH(READPAST) ");
                    sql.Append("INNER JOIN article b WITH(READPAST) ON a.article_no = b.article_no ");
                    sql.Append("WHERE 1 = 1 ");
                    sql.Append("AND a.trans_no = '" + tbTransNo.Text + "' ");
                    sql.Append("ORDER BY a.line_no, a.article_no ASC ");

                    using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
                    {
                        conn.Open();
                        using (cmd = new SqlCommand(sql.ToString(), conn))
                        {
                            using (dr = cmd.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        addDataToTable(dr["article_no"].ToString(), dr["article_type"].ToString(), dr["article_description"].ToString(),
                                        double.Parse(dr["qty"].ToString()), double.Parse(dr["rate"].ToString()), double.Parse(dr["disc"].ToString()), double.Parse(dr["tax"].ToString()), double.Parse(dr["time_duration"].ToString()),
                                        double.Parse(dr["total"].ToString()), DateTime.Now, (DataTable)Session["dtQuotRental"]);
                                    }
                                    loadGrid();
                                  
                                    upGrid.Update();
                                }
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
        
    }

    protected void statusChanged(string status)
    {
        if (tbTransNo.Text == "")
        {
            btnPosting.Visible = false;
        }
        else
        {
            if (status == "")
            {
                btnPosting.Visible = false;
            }
            else if (status == "OPEN")
            {
                btnPosting.Visible = true;
            }
            else if (status == "POSTED")
            {
                btnSave.Visible = false;
                btnPosting.Visible = false;
                upDetail.Visible = false;
            }
        }
    }

    protected void showConfirmBox(string message)
    {
        wucConfirmBox1.SubShowConfirmBox(message);
    }

    protected void ibSearchSoldTo_Click(object sender, EventArgs e)
    {
        Session["search"] = "customer";
        wucSearch1.loadGrid();
    }

    protected void bSearchSoldTo_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT a.customer_name, a.currency_id FROM customer a WITH(READPAST) ");
            sql.Append("WHERE a.customer_no = '" + tbSoldTo.Text.Trim().Replace("'", "`") + "' ");
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
                            lbSoldTo.Text = dr["customer_name"].ToString();
                            tbCurrency.Text = dr["currency_id"].ToString();
                            lblTRate.Text = "10";
                            tbSoldTo.ReadOnly = true;
                            ibSearchSoldTo.Visible = false;
                        }
                        else
                        {
                            lblTRate.Text = "";
                            tbSoldTo.Text = "";
                            tbCurrency.Text = "";
                            tbSoldTo.ReadOnly = false;
                            ibSearchSoldTo.Visible = true;
                            master.messageBox("Invalid customer!");
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

    protected void lbSoldTo_Click(object sender, EventArgs e)
    {
        lbSoldTo.Text = "";
        tbSoldTo.Text = "";
        tbCurrency.Text = "";
        tbPaymentTerm.Text = "";
        tbPaymentTerm.ReadOnly = true;
        lbPaymentTerm.Text = "";
        ibSearchPaymentTerm.Visible = true;
      //  lblTax.Text = "";
        tbSoldTo.ReadOnly = false;
        ibSearchSoldTo.Visible = true;
        //lblTRate.Text = "";
        lbBillTo.Text = "";
        tbBillTo.Text = "";
        tbBillTo.ReadOnly = false;
        ibSearchBillTo.Visible = true;

        lbShipTo.Text = "";
        tbShipTo.Text = "";
        tbShipTo.ReadOnly = false;
        ibSearchShipTo.Visible = true;
        upHeader.Update();
    }

    protected void ibSearchBillTo_Click(object sender, EventArgs e)
    {
        if (tbSoldTo.Text.Trim() == "")
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox("Input rent to!");
            return;
        }
        else
        {
            Session["searchParamCustomerNo"] = tbSoldTo.Text.Trim();
            Session["search"] = "customer_bill_to";
            wucSearch1.loadGrid();
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
            sql.Append("AND a.customer_no = '" + tbSoldTo.Text.Trim().Replace("'", "`") + "' ");
            sql.Append("AND a.id = '" + tbBillTo.Text.Trim().Replace("'", "`") + "' ");

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
                            lbBillTo.Text = dr["alias_name"].ToString();
                            tbBillTo.ReadOnly = true;
                            ibSearchBillTo.Visible = false;
                        }
                        else
                        {
                            tbBillTo.Text = "";
                            tbBillTo.ReadOnly = false;
                            ibSearchBillTo.Visible = true;
                            master.messageBox("Invalid customer address!");
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

    protected void lbBillTo_Click(object sender, EventArgs e)
    {
        lbBillTo.Text = "";
        tbBillTo.Text = "";
        tbBillTo.ReadOnly = false;
        ibSearchBillTo.Visible = true;
    }

    protected void ibSearchShipTo_Click(object sender, EventArgs e)
    {
        if (tbSoldTo.Text.Trim() == "")
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox("Input rent to!");
            return;
        }
        else
        {
            Session["searchParamCustomerNo"] = tbSoldTo.Text.Trim();
            Session["search"] = "customer_ship_to";
            wucSearch1.loadGrid();
        }
    }

    protected void bSearchShipTo_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT (CASE WHEN a.alias_name = '' THEN a.alias_name_full ELSE a.alias_name_full + ' ' + a.alias_name END) AS alias_name ");
            sql.Append("FROM customer_address a WITH(READPAST) ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append("AND a.customer_no = '" + tbSoldTo.Text.Trim().Replace("'", "`") + "' ");
            sql.Append("AND a.id = '" + tbShipTo.Text.Trim().Replace("'", "`") + "' ");

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
                            lbShipTo.Text = dr["alias_name"].ToString();
                            tbShipTo.ReadOnly = true;
                            ibSearchShipTo.Visible = false;
                        }
                        else
                        {
                            tbShipTo.Text = "";
                            tbShipTo.ReadOnly = false;
                            ibSearchShipTo.Visible = true;
                            master.messageBox("Invalid customer address!");
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

    protected void lbShipTo_Click(object sender, EventArgs e)
    {
        lbShipTo.Text = "";
        tbShipTo.Text = "";
        tbShipTo.ReadOnly = false;
        ibSearchShipTo.Visible = true;
    }

    protected void lbPaymentTerm_OnClick(object sender, EventArgs e)
    {
        lbPaymentTerm.Text = "";
        tbPaymentTerm.Text = "";
        tbPaymentTerm.ReadOnly = false;
        ibSearchPaymentTerm.Visible = true;
    }

    protected void ibSearchPaymentTerm_OnClick(object sender, EventArgs e)
    {
        Session["search"] = "payment_term";
        wucSearch1.loadGrid();
    }

    protected void bSearchPaymentTerm_OnClick(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;

        try
        {
            sql.Length = 0;
            sql.Append("SELECT a.description FROM payment_term a ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append("AND a.id = '" + tbPaymentTerm.Text + "' ");

            using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
            {
                conn.Open();
                using (cmd = new SqlCommand(sql.ToString(), conn))
                {
                    using (dr = cmd.ExecuteReader())
                    {
                        if(dr.HasRows)
                        {
                            dr.Read();
                            lbPaymentTerm.Text = dr["description"].ToString();
                            tbPaymentTerm.ReadOnly = true;
                            ibSearchPaymentTerm.Visible = false;
                        }
                        else
                        {
                            tbPaymentTerm.Text = "";
                            tbPaymentTerm.ReadOnly = false;
                            ibSearchPaymentTerm.Visible = true;
                            master.messageBox("Invalid Payment Term!");  
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

    protected void ibSearchArticle_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        if (tbShipTo.Text == "")
        {
            master.messageBox("Input Rent To");
            return;
        }
        else {
            Session["search"] = "rental_order_article";
            wucSearch1.loadGrid();
        }
    }

    protected void bSearchArticle_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT a.article_type, a.article_description ");
            sql.Append("FROM article a WITH(READPAST) ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append("AND a.article_type IN ('ZMNR', 'ZREN') ");
            sql.Append("AND a.article_no = '" + tbArticleNo.Text.Trim().Replace("'", "`") + "' ");

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
                            tbArticleType.Text = dr["article_type"].ToString();
                            lbArticleDesc.Text = dr["article_description"].ToString();
                            tbArticleNo.ReadOnly = true;
                            ibSearchArticle.Visible = false;
                            tbTimeDuration.Text = "";
                            tbRate.Text = "0.00";
                            tbDisc.Text = "0.00";
                        }
                        else
                        {
                            tbArticleType.Text = "";
                            lbArticleDesc.Text = "";
                            tbArticleNo.ReadOnly = false;
                            ibSearchArticle.Visible = true;
                            tbQty.Text = "";
                            tbTimeDuration.Text = "";
                            tbRate.Text = "0.00";
                            tbDisc.Text = "0.00";
                            master.messageBox("Invalid article!");
                        }
                        
                      
                    }
                }
            }
            upDetail.Update();
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void lbClearArticle_Click(object sender, EventArgs e)
    {
        tbArticleNo.Text = "";
        tbArticleType.Text = "";
        lbArticleDesc.Text = "";
        tbArticleNo.ReadOnly = false;
        ibSearchArticle.Visible = true;

        tbQty.Text = "";
        tbTimeDuration.Text = "";

    }

   
    protected void clearDetail()
    {
        tbArticleNo.Text = "";
        tbArticleType.Text = "";
        lbArticleDesc.Text = "";
        tbArticleNo.ReadOnly = false;
        ibSearchArticle.Visible = true;
        tbQty.Text = "";
        tbTimeDuration.Text = "";
        tbRate.Text = "0.00";
        tbDisc.Text = "0.00";
        upDetail.Update();
    }

    protected DataTable createDataTable()
    {
        try
        {
            dt = new DataTable();

            DataColumn dtColumn;

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Int64");
            dtColumn.ColumnName = "lineNo";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "articleNo";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "articleType";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "articleDesc";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "qty";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "Rate";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "disc";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "Tax";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "timeDuration";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "totalAmount";
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
        catch
        {
            return null;
        }
    }

    private void addDataToTable(string articleNo, string articleType, string articleDesc, 
        double qty, double rate, double disc, double tax, double timeDuration,
        Double totalAmount, DateTime addedDate, DataTable Table)
    {
        try
        {
            DataRow dtRow;

            dtRow = Table.NewRow();

            string maxLineNo = ((DataTable)Session["dtQuotRental"]).Compute("MAX(lineNo)", "").ToString();

            if (maxLineNo == null || maxLineNo == "")
            {
                maxLineNo = "0";
            }

            maxLineNo = (int.Parse(maxLineNo) + 10).ToString();

            dtRow["lineNo"] = maxLineNo;
            dtRow["articleNo"] = articleNo;
            dtRow["articleType"] = articleType;
            dtRow["articleDesc"] = articleDesc;
            dtRow["qty"] = qty;
            dtRow["rate"] = rate;
            dtRow["disc"] = disc;
            dtRow["tax"] = tax;
            dtRow["timeDuration"] = timeDuration;
            dtRow["totalAmount"] = totalAmount;
            dtRow["qty"] = qty;
            dtRow["addedDate"] = addedDate;

            Table.Rows.Add(dtRow);
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridViewRow Row = e.Row;

           
            upGrid.Update();
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
            grid.DataSource = ((DataTable)Session["dtQuotRental"]).DefaultView;
            grid.DataBind();

         //   if (grid.Rows.Count >= 1)
          //  {
                calculateNetValue();
           // }
            upGrid.Update();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }
   
    protected void calculateNetValue()
    {
        try
        {
            MasterPage master = (MasterPage)this.Master;

            double netValue = 0;
            double amt = 0;
            double tax = 0;
            foreach (GridViewRow gridRow in grid.Rows)
            {
                Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
                Label lArticleDesc = (Label)gridRow.FindControl("lArticleDesc");
                Label lQty = (Label)gridRow.FindControl("lQty");
                TextBox tbRate = (TextBox)gridRow.FindControl("tbRate");
                TextBox tbDisc = (TextBox)gridRow.FindControl("tbDisc");
                Label lTimeDuration = (Label)gridRow.FindControl("lTimeDuration");
                Label lTax = (Label)gridRow.FindControl("lTax");
                Label lAmount = (Label)gridRow.FindControl("lAmount");

                tbRate.Text = double.Parse(tbRate.Text).ToString("##,#0.#0");
                tbDisc.Text = double.Parse(tbDisc.Text).ToString("##,#0.#0");
                lTax.Text = ((double.Parse(tbRate.Text) /*- double.Parse(tbDisc.Text)*/) * (double.Parse(lblTRate.Text.Replace("%", "")) / 100)).ToString("##,#0.#0");
                lAmount.Text = ((double.Parse(tbRate.Text) /*- double.Parse(tbDisc.Text)*/) * double.Parse(lQty.Text) * double.Parse(lTimeDuration.Text)).ToString("##,#0.#0");

                amt += double.Parse(lAmount.Text);
                tax += double.Parse(lTax.Text) * double.Parse(lQty.Text) * double.Parse(lTimeDuration.Text);
                
            }
            upGrid.Update();
            tax = tax + (double.Parse(tbInstallationCharge.Text) * (double.Parse(lblTRate.Text.Replace("%", "")) / 100));
            tbAmt.Text = amt.ToString("##,#0.#0");
            tbTaxAmt.Text = tax.ToString("##,#0.#0");

            netValue = (amt + tax) + double.Parse(tbInstallationCharge.Text);
            tbNetValue.Text = netValue.ToString("##,#0.#0");
            upHeader.Update();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void bAdd_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (tbShipTo.Text == "")
            {
                master.messageBox("Input Ship To location");
                return;
            }
            if (lbArticleDesc.Text == "")
            {
                master.messageBox("Input article!");
                return;
            }
            else if (tbQty.Text == "")
            {
                master.messageBox("Input qty!");
                return;
            }
            else if (Double.Parse(tbQty.Text) <= 0)
            {
                master.messageBox("Input qty!");
                return;
            }
            else if (tbTimeDuration.Text == "")
            {
                master.messageBox("Input time duration!");
                return;
            }
            else if (double.Parse(tbTimeDuration.Text) <= 0)
            {
                master.messageBox("Input time duration!");
                return;
            }
            else
            {
                double dblTax = (double.Parse(tbRate.Text) - double.Parse(tbDisc.Text)) * (double.Parse(lblTRate.Text.Replace("%", "")) / 100);
                addDataToTable(tbArticleNo.Text.ToUpper().Trim().Replace("'", "`"), tbArticleType.Text.ToUpper().Trim().Replace("'", "`"), lbArticleDesc.Text.ToUpper().Trim().Replace("'", "`"),
                double.Parse(tbQty.Text), double.Parse(tbRate.Text), double.Parse(tbDisc.Text), dblTax,double.Parse(tbTimeDuration.Text),
                double.Parse(tbQty.Text) * (double.Parse(tbRate.Text) - double.Parse(tbDisc.Text)) * double.Parse(tbTimeDuration.Text), DateTime.Now, (DataTable)(Session["dtQuotRental"]));
            }

            loadGrid();

            clearDetail();
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void imgDelete_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
            Label lLineNo = (Label)gridRow.FindControl("lLineNo");

            deleteDetail(lLineNo.Text);

            loadGrid();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void deleteDetail(string lineNo)
    {
        try
        {
            DataRow[] dtRow = ((DataTable)Session["dtQuotRental"]).Select("lineNo='" + lineNo + "'");

            if (dtRow.Length >= 1)
            {
                for (int i = 0; i < dtRow.Length; i++)
                {
                    ((DataTable)Session["dtQuotRental"]).Rows.Remove(dtRow[i]);
                }
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }

    }
    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;

        calculateNetValue();

        foreach (GridViewRow gridRow in grid.Rows)
        {
            Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
            Label lArticleDesc = (Label)gridRow.FindControl("lArticleDesc");
            TextBox tbRate = (TextBox)gridRow.FindControl("tbRate");

            if (tbRate.Text == "")
            {
                master.messageBox("Input rate for article " + lArticleNo.Text + " - " + lArticleDesc.Text + "!");
                return;
            }
            else if (cAdih.isNumber(tbRate.Text) == false)
            {
                master.messageBox("Invalid rate for article " + lArticleNo.Text + " - " + lArticleDesc.Text + "!");
                return;
            }
            else if (double.Parse(tbRate.Text) <= 0)
            {
                master.messageBox("Input rate for article " + lArticleNo.Text + " - " + lArticleDesc.Text + "!");
                return;
            }
            else if (tbPaymentTerm.Text == "")
            {
                master.messageBox("Input Payment Term!");
                return;
            }
        }
    
        if (lbSoldTo.Text == "")
        {
            master.messageBox("Input Rent To!");
            return;
        }
        else if (lbBillTo.Text == "")
        {
            master.messageBox("Input Bill To!");
            return;
        }
        else if (lbShipTo.Text == "")
        {
            master.messageBox("Input Rent To!");
            return;
        }
       
        else if (grid.Rows.Count <= 0)
        {
            master.messageBox("Input Detail Order!");
            return;
        }
        else if (tbInstallationDate.Text == "")
        {
            master.messageBox("Input Request Delivery Date!");
            return;
        }
        else if (tbNote.Text.Trim().Replace("'", "`") == "")
        {
            master.messageBox("Input Note!");
            return;
        }
      
        else
        {
            Session["RentalQuotationMode"] = "Save";
            showConfirmBox("Save Data?<br />Total Amount: " + tbCurrency.Text + " " + Double.Parse(tbNetValue.Text).ToString("#,##0.00") + "");
        }
    }

    protected void saveData()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            //CREATE NEW
            if (tbTransNo.Text == "")
            {
                conn = new SqlConnection(cAdih.getConnStr("Connection"));
                conn.Open();
                trans = conn.BeginTransaction();

                sql.Length = 0;
                sql.Append("SELECT RIGHT('000000' + CAST(ISNULL(MAX(CAST(RIGHT(trans_no, 6) AS INT)), 0) + 1 AS VARCHAR), 6) AS running_no, ");
                sql.Append("YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) AS tahun, RIGHT('00' + CAST(MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) AS VARCHAR), 2) AS bulan ");
                sql.Append("FROM quotation_rental WITH(READPAST) ");
                sql.Append("WHERE YEAR(trans_date) = YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) ");
                sql.Append("AND MONTH(trans_date) = MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    using (dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            tbTransNo.Text = "RQ" + dr["tahun"].ToString() + dr["bulan"].ToString() + dr["running_no"].ToString();
                            upHeader.Update();
                        }
                    }
                }

                sql.Length = 0;
                sql.Append("INSERT INTO[dbo].[quotation_rental] ");
                sql.Append("([trans_no] ");
                sql.Append(",[trans_date] ");
                sql.Append(",[delivery_date] ");
                sql.Append(",[sold_to]  ");
                sql.Append(",[bill_to] ");
                sql.Append(",[ship_to] ");
                sql.Append(",[currency_id] ");
                sql.Append(",[installation_charge] ");
                sql.Append(",[amount] ");
                sql.Append(",[tax] ");
                sql.Append(",[total_amount] ");
                sql.Append(",[note] ");
                sql.Append(",[payment_term] ");
                sql.Append(",[status] ");
                sql.Append(",[created_by] ");
                sql.Append(",[created_ip] ");
                sql.Append(",[created_date] )");
                sql.Append("VALUES ");
                sql.Append("('" + tbTransNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("CONVERT(VARCHAR(10), GETDATE(), 120), ");
                sql.Append("'" + cAdih.engFormatDate(tbInstallationDate.Text) + "', ");
                sql.Append("'" + tbSoldTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + tbBillTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + tbShipTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + tbCurrency.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + double.Parse(tbInstallationCharge.Text) + "', ");
                sql.Append("'" + double.Parse(tbAmt.Text) + "', ");
                sql.Append("'" + double.Parse(tbTaxAmt.Text) + "', ");
                sql.Append("'" + double.Parse(tbNetValue.Text) + "', ");
                sql.Append("'" + tbNote.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + tbPaymentTerm.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'0', ");
                sql.Append("'" + Session["username"].ToString() + "', ");
                sql.Append("'" + cAdih.getIPAddress() + "', ");
                sql.Append("GETDATE()) ");
                
                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                int line_no = 0;

                foreach (GridViewRow gridRow in grid.Rows)
                {
                    Label lLineNo = (Label)gridRow.FindControl("lLineNo");
                    Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
                    Label lArticleCategory = (Label)gridRow.FindControl("lArticleCategory");
                    Label lArticleType = (Label)gridRow.FindControl("lArticleType");
                    Label lRentalUnitID = (Label)gridRow.FindControl("lRentalUnitID");
                    Label lQty = (Label)gridRow.FindControl("lQty");
                    Label lTax = (Label)gridRow.FindControl("lTax");
                    Label lTimeUnitID = (Label)gridRow.FindControl("lTimeUnitID");
                    TextBox tbRate = (TextBox)gridRow.FindControl("tbRate");
                    TextBox tbDisc = (TextBox)gridRow.FindControl("tbDisc");
                    Label lTimeDuration = (Label)gridRow.FindControl("lTimeDuration");
                    Label lAmount = (Label)gridRow.FindControl("lAmount");
                    Label lSerialNo = (Label)gridRow.FindControl("lSerialNo");
                   
                    line_no += 10;

                    sql.Length = 0;
                    sql.Append("INSERT INTO [dbo].[quotation_rental_detail] ");
                    sql.Append("([trans_no] ");
                    sql.Append(",[line_no] ");
                    sql.Append(",[article_no] ");
                    sql.Append(",[qty] "); 
                    sql.Append(",[rate] ");
                    sql.Append(",[tax] ");
                    sql.Append(",[disc] ");
                    sql.Append(",[total_amount] ");
                    sql.Append(",[rent_duration])");
                    sql.Append("VALUES (");
                    sql.Append("'" + tbTransNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + line_no + "', ");
                    sql.Append("'" + lArticleNo.Text + "', ");
                    sql.Append("'" + double.Parse(lQty.Text) + "', ");
                    sql.Append("'" + double.Parse(tbRate.Text) + "', ");
                    sql.Append("'" + double.Parse(lTax.Text) + "', ");
                    sql.Append("'" + double.Parse(tbDisc.Text) + "', ");
                    sql.Append("'" + double.Parse(lAmount.Text) + "', ");
                    sql.Append("'" + int.Parse(lTimeDuration.Text) + "') ");
                  
                    using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                    {
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }
                }

                trans.Commit();
                conn.Close();
            }
            //EDIT
            else if (tbTransNo.Text != "")
            {
                conn = new SqlConnection(cAdih.getConnStr("Connection"));
                conn.Open();
                trans = conn.BeginTransaction();

                sql.Length = 0;
                sql.Append("UPDATE quotation_rental SET sold_to = '" + tbSoldTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("bill_to = '" + tbBillTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("ship_to = '" + tbShipTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("delivery_date = '" + cAdih.engFormatDate(tbInstallationDate.Text) + "', ");
                sql.Append("currency_id = '" + tbCurrency.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("installation_charge = '" + double.Parse(tbInstallationCharge.Text) + "', ");
                sql.Append("amount = '" + double.Parse(tbAmt.Text) + "', ");
                sql.Append("tax = '" + double.Parse(tbTaxAmt.Text) + "', ");
                sql.Append("total_amount = '" + double.Parse(tbNetValue.Text) + "', ");
                sql.Append("note = '" + tbNote.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("payment_term = '" + tbPaymentTerm.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("last_modified_by = '" + Session["username"].ToString() + "', ");
                sql.Append("last_modified_date = GETDATE() ");
                sql.Append("WHERE trans_no = '" + tbTransNo.Text + "' ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                sql.Length = 0;
                sql.Append("DELETE quotation_rental_detail WHERE trans_no = '" + tbTransNo.Text + "' ");
                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                int line_no = 0;

                foreach (GridViewRow gridRow in grid.Rows)
                {
                    Label lLineNo = (Label)gridRow.FindControl("lLineNo");
                    Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
                    Label lArticleCategory = (Label)gridRow.FindControl("lArticleCategory");
                    Label lArticleType = (Label)gridRow.FindControl("lArticleType");
                    Label lRentalUnitID = (Label)gridRow.FindControl("lRentalUnitID");
                    Label lQty = (Label)gridRow.FindControl("lQty");
                    Label lTax = (Label)gridRow.FindControl("lTax");
                    Label lTimeUnitID = (Label)gridRow.FindControl("lTimeUnitID");
                    TextBox tbRate = (TextBox)gridRow.FindControl("tbRate");
                    TextBox tbDisc = (TextBox)gridRow.FindControl("tbDisc");
                    Label lTimeDuration = (Label)gridRow.FindControl("lTimeDuration");
                    Label lAmount = (Label)gridRow.FindControl("lAmount");
                    Label lSerialNo = (Label)gridRow.FindControl("lSerialNo");

                    line_no += 10;

                    sql.Length = 0;
                    sql.Append("INSERT INTO [dbo].[quotation_rental_detail] ");
                    sql.Append("([trans_no] ");
                    sql.Append(",[line_no] ");
                    sql.Append(",[article_no] ");
                    sql.Append(",[qty] ");
                    sql.Append(",[rate] ");
                    sql.Append(",[tax] ");
                    sql.Append(",[disc] ");
                    sql.Append(",[total_amount] ");
                    sql.Append(",[rent_duration])");
                    sql.Append("VALUES (");
                    sql.Append("'" + tbTransNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + line_no + "', ");
                    sql.Append("'" + lArticleNo.Text + "', ");
                    sql.Append("'" + double.Parse(lQty.Text) + "', ");
                    sql.Append("'" + double.Parse(tbRate.Text) + "', ");
                    sql.Append("'" + double.Parse(lTax.Text) + "', ");
                    sql.Append("'" + double.Parse(tbDisc.Text) + "', ");
                    sql.Append("'" + double.Parse(lAmount.Text) + "', ");
                    sql.Append("'" + int.Parse(lTimeDuration.Text) + "') ");

                    using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                    {
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }
                }

                trans.Commit();
                conn.Close();
            }
            master.messageBox("Data has been saved. Trans No: " + tbTransNo.Text + "");
            lStatus.Text = "OPEN";
            statusChanged(lStatus.Text);
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
            tbTransNo.Text = "";
            trans.Rollback();
            conn.Close();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Session["dtQuotRental"] = null;
        Response.Redirect("~/Transaction/RentalQuotation.aspx");
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        try
        {
            if (wucConfirmBox1.StrMode == "Yes")
            {
                if (Session["RentalQuotationMode"].ToString() == "Save")
                {
                    saveData();
                }
                else if (Session["RentalQuotationMode"].ToString() == "Posting")
                {
                    postingData();
                }

            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void wucSearch_Hide(object sender, EventArgs e)
    {
        if (Session["search"].ToString() == "rental_order_article")
        {
            tbArticleNo.Text = wucSearch1.result;
            bSearchArticle_Click(sender, e);
            upDetail.Update();
        }

        // -- Adih Dipindahkan Ke WucLookup (2019-12-05)
        //else if (Session["search"].ToString() == "article_time_unit")
        //{
        //    tbTimeUnitID.Text = wucSearch1.result;
        //    bSearchTimeUnit_Click(sender, e);
        //    upDetail.Update();
        //    Session["searchParamArticle"] = null;
        //    Session["searchParamCurrency"] = null;
        //}
        
        else if (Session["search"].ToString() == "customer")
        {
            tbSoldTo.Text = wucSearch1.result;
            bSearchSoldTo_Click(sender, e);
            upHeader.Update();
        }
        else if (Session["search"].ToString() == "customer_bill_to")
        {
            tbBillTo.Text = wucSearch1.result;
            bSearchBillTo_Click(sender, e);
            upHeader.Update();
            Session["searchParamCustomerNo"] = null;
        }
        else if (Session["search"].ToString() == "customer_ship_to")
        {
            tbShipTo.Text = wucSearch1.result;
            bSearchShipTo_Click(sender, e);
            upHeader.Update();
            Session["searchParamCustomerNo"] = null;
        }
        else if (Session["search"].ToString() == "payment_term")
        {
            tbPaymentTerm.Text = wucSearch1.result;
            bSearchPaymentTerm_OnClick(sender, e);
            upHeader.Update();
        }
        Session["search"] = null;
    }

    protected void btnPosting_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;

        Session["RentalQuotationMode"] = "Posting";
        showConfirmBox("Posting Data?");
    }

    protected void postingData()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            conn = new SqlConnection(cAdih.getConnStr("Connection"));
            conn.Open();

            trans = conn.BeginTransaction();
            sql.Length = 0;
            sql.Append ( "UPDATE quotation_rental SET posted_by='" + Session["username"].ToString() + "',");
            sql.Append ( " posted_date=getdate(),posted_ip='" + cAdih.getIPAddress() + "',status=1 ");
            sql.Append ( " WHERE trans_no='" + tbTransNo.Text.Trim().Replace("'", "`") + "';");
            using (cmd = new SqlCommand(sql.ToString(), conn, trans))
            {
                cmd.ExecuteNonQuery();
                cmd = null;
            }
            trans.Commit();
            master.messageBox("Data has been posted. Trans No: " + tbTransNo.Text + "");
            lStatus.Text = "POSTED";
            statusChanged(lStatus.Text);
          
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

}