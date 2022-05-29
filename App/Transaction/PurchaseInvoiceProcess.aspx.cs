using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transaction_PurchaseInvoiceProcessProcess : System.Web.UI.Page
{
    private StringBuilder sql = new StringBuilder();
    private StringBuilder sql1 = new StringBuilder();
    private SqlTransaction trans;
    private SqlTransaction trans1;
    private SqlConnection conn;
    private SqlConnection conn1;
    private SqlCommand cmd;
    private SqlCommand cmd1;
    private SqlDataReader dr;
    private SqlDataReader dr1;
    private DataTable dt;
    private ClassAdih cAdih = new ClassAdih();
    private string sNote = "";

    private static string TABLE_NAME = "invoice_confirmation_receipt";
    private static string TABLE_NAME_DETAIL = "invoice_confirmation_receipt_detail";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                if (Request.QueryString["mode"].ToString() == "add")
                {
                    tbTransDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    lStatus.Text = "";
                    statusChanged();
                }
                else if (Request.QueryString["mode"].ToString() == "edit")
                {
                    tbTransNo.Text = Request.QueryString["trans_no"].ToString();

                    sql.Length = 0;
                    sql.Append("SELECT icc.trans_no, icc.trans_date, icc.reff_order_no, ");
                    sql.Append("icc.reff_order_no AS po_trans_no, CONVERT(VARCHAR, po.trans_date, 106) AS po_trans_date, icc.vendor_no, v.vendor_name, icc.vendor_invoice_no, icc.tax_facture_no, ");
                    sql.Append("'IDR' as currency_id, icc.payment_term, icc.due_date, icc.total_amount, icc.tax_facture_date, icc.note, icc.status, isnull(cust.cust_no, '') 'cust_no', isnull(cust.cust_name, '') 'cust_name', isnull(cust.ship_to_id, '') 'ship_to_id', isnull(cust.ship_to_name, '') 'ship_to_name' ");
                    sql.Append("FROM invoice_confirmation_receipt icc WITH(READPAST) ");
                    sql.Append("INNER JOIN purchase_order po WITH(READPAST) ON icc.reff_order_no = po.trans_no ");
                    sql.Append("left join( ");
                    sql.Append("    select  wh_id 'cust_no', wh_description 'cust_name', '' 'ship_to_id', '' 'ship_to_name' ");
                    sql.Append("    from site_wh WITH(READPAST) ");
                    sql.Append("        union ");
                    sql.Append("    select vendor_no, vendor_name, '', '' ");
                    sql.Append("    from vendor WITH(READPAST) "); 
                    sql.Append(") as cust ON(po.ship_to = cust.ship_to_id) ");
                    sql.Append("INNER JOIN vendor v WITH(READPAST) ON icc.vendor_no = v.vendor_no ");
                    sql.Append("WHERE 1 = 1 ");
                    sql.Append("AND icc.trans_no = '" + tbTransNo.Text + "' ");

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
                                    tbReffOrderNo.Text = dr["reff_order_no"].ToString();
                                    tbReffOrderNo.ReadOnly = true;
                                    ibSearchReffOrder.Visible = false;
                                    lbReffOrderNo.Text = dr["po_trans_date"].ToString();
                                    tbVendorNo.Text = dr["vendor_no"].ToString();
                                    lbVendorName.Text = dr["vendor_name"].ToString();
                                    tbVendorInvoiceNo.Text = dr["vendor_invoice_no"].ToString();
                                    tbTaxFactureNo.Text = dr["tax_facture_no"].ToString();
                                    tbTglFacture.Text = DateTime.Parse(dr["tax_facture_date"].ToString()).ToString("dd-MM-yyyy");
                                    tbCurrencyID.Text = dr["currency_id"].ToString();
                                    tbACurrencyID.Text = dr["currency_id"].ToString();
                                    tbVCurrencyID.Text = dr["currency_id"].ToString();
                                    tbTotalInvoice.Text = double.Parse(dr["total_amount"].ToString()).ToString("##,#0");
                                    tbPaymentTerm.Text = dr["payment_term"].ToString();
                                    tbDueDate.Text = DateTime.Parse(dr["due_date"].ToString()).ToString("dd-MM-yyyy");
                                    tbNote.Text = dr["note"].ToString();
                                    if (dr["status"].ToString() == "False")
                                    {
                                        lStatus.Text = "OPEN";
                                    }
                                    else if (dr["status"].ToString() == "True")
                                    {
                                        lStatus.Text = "POSTED";
                                        btnSave.Visible = false;
                                        btnPosting.Visible = false;
                                    }
                                }
                            }
                        }
                    }

                    sql.Length = 0;
                    sql.Append("SELECT distinct a.line_no, a.inbound_delivery_no, b.article_type, a.article_no, b.article_description, ");
                    sql.Append("a.qty_order, a.qty_received, a.unit_price,a.note, unit_tax as tax ");
                    sql.Append("FROM invoice_confirmation_receipt_detail a WITH(READPAST) ");
                    sql.Append("INNER JOIN article b WITH(READPAST) ON a.article_no = b.article_no ");
                    sql.Append("INNER JOIN inbound_delivery id on a.inbound_delivery_no = id.trans_no ");
                    sql.Append("INNER JOIN purchase_order_detail pod on pod.trans_no = id.reff_order_no and pod.article_no = a.article_no ");
                    sql.Append("WHERE 1 = 1 ");
                    sql.Append("AND a.trans_no = '" + tbTransNo.Text.Trim().Replace("'", "`") + "' ");

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
                                    while (dr.Read())
                                    {
                                        addDataToTable(dr["inbound_delivery_no"].ToString(), dr["article_no"].ToString(), dr["article_type"].ToString(), dr["article_description"].ToString(),
                                            double.Parse(dr["qty_order"].ToString()), double.Parse(dr["qty_received"].ToString()),
                                            double.Parse(dr["unit_price"].ToString()), (double.Parse(dr["tax"].ToString()) * double.Parse(dr["qty_received"].ToString())), (double.Parse(dr["qty_received"].ToString()) * (double.Parse(dr["unit_price"].ToString()) + double.Parse(dr["tax"].ToString()))), dr["note"].ToString(),
                                            DateTime.Now, (DataTable)Session["dt"]);
                                    }
                                    loadGrid();
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void statusChanged()
    {
        if (tbTransNo.Text == "")
        {
            btnPosting.Visible = false;
        }
        else
        {
            if (lStatus.Text == "")
            {
                btnPosting.Visible = false;
            }
            else if (lStatus.Text == "OPEN")
            {
                btnPosting.Visible = true;
            }
            else if (lStatus.Text == "POSTED")
            {
                btnSave.Visible = false;
                btnPosting.Visible = false;
            }
        }
    }

    protected void showConfirmBox(string message)
    {
        wucConfirmBox1.SubShowConfirmBox(message);
    }

    protected void ibSearchReffOrder_Click(object sender, EventArgs e)
    {
        Session["search"] = "pi_purchase_order_no";
        wucSearch1.loadGrid();
    }

    protected void bSearchReffOrder_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT DISTINCT CONVERT(VARCHAR, po.trans_date, 106) AS po_trans_date, po.vendor_id, v.vendor_name, po.payment_term, po.currency_id, CONVERT(VARCHAR(10), DATEADD(DAY, pt.days, GETDATE()), 120) AS due_date, isnull(cust.cust_no, '') 'cust_no', isnull(cust.cust_name, '') 'cust_name', isnull(cust.ship_to_id, '') 'ship_to_id', isnull(cust.ship_to_name, '') 'ship_to_name' ");
            sql.Append("FROM inbound_delivery id WITH(READPAST) ");
            sql.Append("INNER JOIN purchase_order po WITH(READPAST) ON id.reff_order_no = po.trans_no ");
            sql.Append("INNER JOIN vendor v WITH(READPAST) ON po.vendor_id = v.vendor_no ");
            sql.Append("INNER JOIN payment_term pt WITH(READPAST) ON po.payment_term = pt.id ");
            sql.Append("left join ( ");
            sql.Append("	select	wh_id 'cust_no', wh_description 'cust_name', '' 'ship_to_id', '' 'ship_to_name' ");
            sql.Append("	from	site_wh WITH(READPAST) ");
            sql.Append("		union ");
            sql.Append("	select	vendor_no, vendor_name, '', '' ");
            sql.Append("	from	vendor WITH(READPAST) ");
            sql.Append(")as cust ON (po.ship_to = cust.ship_to_id) ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append("AND id.status = '1' AND id.reff = 'PURCHASE ORDER' ");
            sql.Append("AND id.trans_no NOT IN (");
            sql.Append("    SELECT iccd.inbound_delivery_no FROM invoice_confirmation_receipt icc WITH(READPAST) ");
            sql.Append("    INNER JOIN invoice_confirmation_receipt_detail iccd ON icc.trans_no = iccd.trans_no ");
            sql.Append("    WHERE icc.status = 1) ");
            sql.Append("AND po.trans_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`") + "' ");

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
                            lbReffOrderNo.Text = dr["po_trans_date"].ToString();
                            tbVendorNo.Text = dr["vendor_id"].ToString();
                            lbVendorName.Text = dr["vendor_name"].ToString();
                            tbCurrencyID.Text = dr["currency_id"].ToString();
                            tbACurrencyID.Text = dr["currency_id"].ToString();
                            tbVCurrencyID.Text = dr["currency_id"].ToString();
                            tbPaymentTerm.Text = dr["payment_term"].ToString();
                            DataTable dt = new DataTable();
                            dt = createDataTable();
                            Session["dt"] = dt;
                        }
                        else
                        {
                            lbReffOrderNo.Text = "";
                            tbReffOrderNo.ReadOnly = false;
                            ibSearchReffOrder.Visible = true;
                            master.messageBox("Invalid Purchase Order No!");
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

    protected void lbReffOrderNo_Click(object sender, EventArgs e)
    {
        tbReffOrderNo.Text = "";
        lbReffOrderNo.Text = "";
        tbReffOrderNo.ReadOnly = false;
        ibSearchReffOrder.Visible = true;
        tbVendorNo.Text = "";
        lbVendorName.Text = "";
        tbVendorInvoiceNo.Text = "";
        tbTaxFactureNo.Text = "";
        tbTotalInvoice.Text = "";
        tbNote.Text = "";
        tbNote.ReadOnly = false;

        upHeader.Update();
        grid.DataSource = null;
        grid.DataBind();
        upGrid.Update();
    }

    protected DataTable createDataTable()
    {
        try
        {
            dt = new DataTable();

            DataColumn dtColumn;

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "idTransNo";
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
            dtColumn.ColumnName = "qty_order";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "qty_received";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "unit_price";
            dt.Columns.Add(dtColumn);


            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "Total_unit_price";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "tax";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "Total_tax";
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

    private void addDataToTable(string idTransNo, string articleNo, string articleType, string articleDesc,
        double qty, double qty_received, double unit_price, double tax, double total_amount,
        String note, DateTime addedDate, DataTable Table)
    {
        try
        {
            DataRow dtRow;

            dtRow = Table.NewRow();

            dtRow["idTransNo"] = idTransNo;
            dtRow["articleNo"] = articleNo;
            dtRow["articleType"] = articleType;
            dtRow["articleDesc"] = articleDesc;
            dtRow["qty_order"] = qty;
            dtRow["qty_received"] = qty_received;
            dtRow["unit_price"] = unit_price;
            dtRow["tax"] = tax / qty_received;
            dtRow["Total_unit_price"] = (qty_received * unit_price);
            dtRow["Total_tax"] = tax;
            dtRow["total_amount"] = total_amount;
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

    protected void ibSearchIDTransNo_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        if (lbReffOrderNo.Text.Trim().Length == 0)
        {
            master.messageBox("Input reff order no!");
            return;
        }

        Session["search"] = "pi_inbound_delivery_no";
        Session["searchParamPOTransNo"] = tbReffOrderNo.Text.Replace("'", "`").Trim();
        wucSearch1.loadGrid();
    }

    protected void bSearchIDTransNo_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT CONVERT(VARCHAR, id.posted_date, 106) AS id_posted_date ");
            sql.Append("FROM inbound_delivery id WITH(READPAST) ");
            sql.Append("INNER JOIN inbound_delivery_detail idd WITH(READPAST) ON id.trans_no = idd.trans_no ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append("AND id.status = '1' ");
            sql.Append("AND id.reff_order_no = '" + tbReffOrderNo.Text + "' ");
            sql.Append("AND id.trans_no = '" + tbIDTransNo.Text.Trim().Replace("'", "`") + "' ");

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
                            lbIDTransNo.Text = dr["id_posted_date"].ToString();
                            tbIDTransNo.ReadOnly = true;
                            ibSearchIDTransNo.Visible = false;
                        }
                        else
                        {
                            tbIDTransNo.Text = "";
                            tbIDTransNo.ReadOnly = false;
                            ibSearchIDTransNo.Visible = true;
                            master.messageBox("Invalid ID Trans No!");
                        }
                        upDetail.Update();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void lbIDTransNo_Click(object sender, EventArgs e)
    {
        tbIDTransNo.Text = "";
        lbIDTransNo.Text = "";
        tbIDTransNo.ReadOnly = false;
        ibSearchIDTransNo.Visible = true;
        upDetail.Update();
    }

    protected void bAdd_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            string pr_dept = "";
            deleteDetail(tbIDTransNo.Text.Trim().Replace("'", "`"));

            if (lbIDTransNo.Text == "")
            {
                master.messageBox("Input inbound delivery no!");
                return;
            }

            sql.Length = 0;
            sql.Append("SELECT id.trans_no, idd.article_no, idd.article_type, a.article_description, ");
            sql.Append("po.qty_po, po.unit_price, po.tax, SUM(idd.qty_received) AS qty_id, po.note_po AS note, idd.reff_line_no, po.tax_rate ");
            sql.Append("FROM inbound_delivery id WITH(READPAST) ");
            sql.Append("INNER JOIN inbound_delivery_detail idd WITH(READPAST) ON id.trans_no = idd.trans_no ");
            sql.Append("INNER JOIN article a WITH(READPAST) ON idd.article_no = a.article_no ");
            sql.Append("INNER JOIN( ");
            sql.Append("    SELECT po.trans_no, pod.line_no, pod.unit_price, pod.tax as tax, pod.qty AS qty_po, pod.note AS note_po, po.tax_rate ");
            sql.Append("    FROM purchase_order po WITH(READPAST) ");
            sql.Append("    INNER JOIN vendor v WITH(READPAST) on(po.vendor_id = v.vendor_no) ");
            sql.Append("    INNER JOIN purchase_order_detail pod WITH(READPAST) ON po.trans_no = pod.trans_no ");
            sql.Append(") AS po ON id.reff_order_no = po.trans_no AND idd.reff_line_no = po.line_no ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append("AND id.trans_no = '" + tbIDTransNo.Text.Trim().Replace("'", "`") + "' ");
            sql.Append("AND id.trans_no NOT IN( ");
            sql.Append("    SELECT iccd.inbound_delivery_no FROM invoice_confirmation_receipt icc WITH(READPAST) ");
            sql.Append("    INNER JOIN invoice_confirmation_receipt_detail iccd ON icc.trans_no = iccd.trans_no ");
            sql.Append("    WHERE icc.status = 1 ");
            sql.Append(") GROUP BY id.trans_no, idd.article_no, idd.article_type, a.article_description, po.qty_po, po.tax_rate ,po.unit_price, po.tax, note_po, idd.reff_line_no  ");

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
                                double dTaxRate = 0;
                                double dTaxAmount = 0;

                                if (double.Parse(dr["tax"].ToString()) > 0)
                                    dTaxRate = double.Parse(dr["tax_rate"].ToString()) / 100.00;

                                dTaxAmount = ((double.Parse(dr["qty_id"].ToString()) * double.Parse(dr["unit_price"].ToString()))) * dTaxRate;

                                addDataToTable(dr["trans_no"].ToString(), dr["article_no"].ToString(), dr["article_type"].ToString(), dr["article_description"].ToString(),
                                    double.Parse(dr["qty_po"].ToString()), double.Parse(dr["qty_id"].ToString()),
                                    double.Parse(dr["unit_price"].ToString()), dTaxAmount, (double.Parse(dr["qty_id"].ToString()) * double.Parse(dr["unit_price"].ToString())) + dTaxAmount, dr["note"].ToString(),
                                    DateTime.Now, (DataTable)Session["dt"]);
                            }
                            loadGrid();
                        }
                    }
                }
            }

            loadGrid();
            clearDetail();
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void clearDetail()
    {
        tbIDTransNo.Text = "";
        lbIDTransNo.Text = "";
        tbIDTransNo.ReadOnly = false;
        ibSearchIDTransNo.Visible = true;
    }

    protected void loadGrid()
    {
        try
        {
            grid.DataSource = ((DataTable)Session["dt"]).DefaultView;
            grid.DataBind();

            if (grid.Rows.Count >= 1)
            {
                tbTAmount.Text = Double.Parse(((DataTable)Session["dt"]).Compute("SUM(Total_unit_price)", "").ToString()).ToString("#,#0.#0;(#,#0.#0)");
                tbTVAT.Text = Double.Parse(((DataTable)Session["dt"]).Compute("SUM(Total_tax)", "").ToString()).ToString("#,#0.#0;(#,#0.#0)");

                tbTotalInvoice.Text = Double.Parse(((DataTable)Session["dt"]).Compute("SUM(total_amount)", "").ToString()).ToString("#,#0.#0;(#,#0.#0)");
            }

            upHeader.Update();
            upGrid.Update();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void deleteDetail(string idTransNo)
    {
        try
        {
            DataRow[] dtRow = ((DataTable)Session["dt"]).Select("idTransNo='" + idTransNo + "'");

            if (dtRow.Length >= 1)
            {
                for (int i = 0; i < dtRow.Length; i++)
                {
                    ((DataTable)Session["dt"]).Rows.Remove(dtRow[i]);
                }
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void clearGrid()
    {
        grid.DataSource = null;
        grid.DataBind();
        upGrid.Update();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;

        if (lbReffOrderNo.Text == "")
        {
            master.messageBox("Input inbound delivery no!");
            return;
        }
        else if (tbVendorInvoiceNo.Text.Trim().Replace("'", "`") == "")
        {
            master.messageBox("Input vendor invoice no!");
            return;
        }
        else if (tbDueDate.Text == "")
        {
            master.messageBox("Input Due Date!");
            return;
        }
        else if (tbNote.Text == "")
        {
            master.messageBox("Input note!");
            return;
        }
        else if (grid.Rows.Count <= 0)
        {
            master.messageBox("Input detail!");
            return;
        }

        sql.Length = 0;
        sql.Append("SELECT trans_no AS result FROM invoice_confirmation_receipt WITH(READPAST) ");
        sql.Append("WHERE vendor_invoice_no = '" + tbVendorInvoiceNo.Text.Trim().Replace("'", "`").ToUpper() + "' ");
        sql.Append("AND vendor_no = '" + tbVendorNo.Text.Trim().Replace("'", "`").ToUpper() + "' ");

        string doubleInvoiceNo = cAdih.getResultString(sql.ToString(), cAdih.getConnStr("Connection"));

        if (doubleInvoiceNo != "")
        {
            if (doubleInvoiceNo != tbTransNo.Text)
            {
                master.messageBox("Invoice No " + tbVendorInvoiceNo.Text.Trim().Replace("'", "`").ToUpper() + " already received in Trans No. " + doubleInvoiceNo + ".");
                return;
            }
        }
        Session["InvoiceConfirmationReceiptMode"] = "Save";
        showConfirmBox("Save Data?");
    }

    protected void saveData()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            string strTglFaktur = "";
            if (tbTglFacture.Text.Trim() != "")
            {
                strTglFaktur = cAdih.engFormatDate(tbTglFacture.Text.Trim());
            }

            if (tbTransNo.Text == "")
            {
                conn = new SqlConnection(cAdih.getConnStr("Connection"));
                conn.Open();
                trans = conn.BeginTransaction();

                if (tbTaxFactureNo.Text.Length == 19 || tbTaxFactureNo.Text == "-" || tbTaxFactureNo.Text == "")
                {
                    sql.Length = 0;
                    sql.Append("SELECT RIGHT('000000' + CAST(ISNULL(MAX(CAST(RIGHT(trans_no, 6) AS INT)), 0) + 1 AS VARCHAR), 6) AS running_no, ");
                    sql.Append("YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) AS tahun, RIGHT('00' + CAST(MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) AS VARCHAR), 2) AS bulan ");
                    sql.Append("FROM invoice_confirmation_receipt ");
                    sql.Append("WHERE YEAR(trans_date) = YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) ");
                    sql.Append("AND MONTH(trans_date) = MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) ");

                    using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                    {
                        using (dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                tbTransNo.Text = "PI" + dr["tahun"].ToString() + dr["bulan"].ToString() + dr["running_no"].ToString();
                                upHeader.Update();
                            }
                        }
                    }

                    sql.Length = 0;
                    sql.Append("INSERT INTO invoice_confirmation_receipt(trans_no, trans_date, reff_order_no, vendor_no, vendor_invoice_no, tax_facture_no,tax_facture_date, ");
                    sql.Append("total_amount, payment_term, due_date, note, status, created_by, created_date) VALUES( ");
                    sql.Append("'" + tbTransNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + cAdih.engFormatDate(tbTransDate.Text) + "', ");
                    sql.Append("'" + tbReffOrderNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + tbVendorNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + tbVendorInvoiceNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + tbTaxFactureNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + strTglFaktur + "', ");
                    sql.Append("'" + double.Parse(tbTotalInvoice.Text) + "', ");
                    sql.Append("'" + tbPaymentTerm.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + cAdih.engFormatDate(tbDueDate.Text) + "', ");
                    sql.Append("'" + tbNote.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'0', ");
                    sql.Append("'" + Session["username"].ToString() + "', ");
                    sql.Append("GETDATE()) ");

                    using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                    {
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }

                    int line_no = 10;

                    foreach (GridViewRow gridRow in grid.Rows)
                    {
                        Label lIDTransNo = (Label)gridRow.FindControl("lIDTransNo");
                        Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
                        Label lArticleType = (Label)gridRow.FindControl("lArticleType");
                        Label lQtyOrder = (Label)gridRow.FindControl("lQtyOrder");
                        Label lQtyReceived = (Label)gridRow.FindControl("lQtyReceived");
                        Label lUnitPrice = (Label)gridRow.FindControl("lUnitPrice");
                        Label lVAT = (Label)gridRow.FindControl("lVAT");
                        TextBox tbNoteDetail = (TextBox)gridRow.FindControl("tbNoteDetail");

                        if (double.Parse(lQtyReceived.Text) != 0)
                        {
                            sql.Length = 0;
                            sql.Append("INSERT INTO invoice_confirmation_receipt_detail(trans_no, line_no, inbound_delivery_no, article_no, qty_order, qty_received, unit_price,unit_tax, note) VALUES(");
                            sql.Append("'" + tbTransNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                            sql.Append("'" + line_no + "', ");
                            sql.Append("'" + lIDTransNo.Text + "', ");
                            sql.Append("'" + lArticleNo.Text + "', ");
                            sql.Append("'" + double.Parse(lQtyOrder.Text) + "', ");
                            sql.Append("'" + double.Parse(lQtyReceived.Text) + "', ");
                            sql.Append("'" + double.Parse(lUnitPrice.Text) + "', ");
                            sql.Append("'" + double.Parse(lVAT.Text) + "', ");
                            sql.Append("'" + tbNoteDetail.Text.Trim().Replace("'", "`") + "') ");

                            using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                            {
                                cmd.ExecuteNonQuery();
                                cmd = null;
                            }
                        }

                        line_no += 10;
                    }

                    trans.Commit();
                    conn.Close();
                    master.messageBox("Data has been saved. Trans No: " + tbTransNo.Text + "");
                }
                else
                {
                    master.messageBox("Tax facture number entered is not enough!");
                    return;
                }
            }
            //EDIT
            else if (tbTransNo.Text != "")
            {

                conn = new SqlConnection(cAdih.getConnStr("Connection"));
                conn.Open();
                trans = conn.BeginTransaction();

                if (tbTaxFactureNo.Text.Length == 19 || tbTaxFactureNo.Text == "-" || tbTaxFactureNo.Text == "")
                {
                    sql.Length = 0;
                    sql.Append("UPDATE invoice_confirmation_receipt SET reff_order_no = '" + tbReffOrderNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("vendor_no = '" + tbVendorNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("vendor_invoice_no = '" + tbVendorInvoiceNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("tax_facture_no = '" + tbTaxFactureNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("tax_facture_date = '" + strTglFaktur + "', ");
                    sql.Append("total_amount = '" + double.Parse(tbTotalInvoice.Text) + "', ");
                    sql.Append("payment_term = '" + tbPaymentTerm.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("due_date = '" + cAdih.engFormatDate(tbDueDate.Text) + "', ");
                    sql.Append("note = '" + tbNote.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("last_modified_by = '" + Session["username"].ToString() + "', ");
                    sql.Append("last_modified_date = GETDATE() ");
                    sql.Append("WHERE trans_no = '" + tbTransNo.Text + "' ");

                    using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                    {
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }

                    sql.Length = 0;
                    sql.Append("DELETE invoice_confirmation_receipt_detail WHERE trans_no = '" + tbTransNo.Text + "' ");
                    using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                    {
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }

                    int line_no = 10;

                    foreach (GridViewRow gridRow in grid.Rows)
                    {
                        Label lIDTransNo = (Label)gridRow.FindControl("lIDTransNo");
                        Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
                        Label lArticleType = (Label)gridRow.FindControl("lArticleType");
                        Label lQtyOrder = (Label)gridRow.FindControl("lQtyOrder");
                        Label lQtyReceived = (Label)gridRow.FindControl("lQtyReceived");
                        Label lUnitPrice = (Label)gridRow.FindControl("lUnitPrice");
                        Label lVAT = (Label)gridRow.FindControl("lVAT");
                        TextBox tbNoteDetail = (TextBox)gridRow.FindControl("tbNoteDetail");

                        if (double.Parse(lQtyReceived.Text) != 0)
                        {
                            sql.Length = 0;
                            sql.Append("INSERT INTO invoice_confirmation_receipt_detail(trans_no, line_no, inbound_delivery_no, article_no, qty_order, qty_received, unit_price,unit_tax, note) VALUES(");
                            sql.Append("'" + tbTransNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                            sql.Append("'" + line_no + "', ");
                            sql.Append("'" + lIDTransNo.Text + "', ");
                            sql.Append("'" + lArticleNo.Text + "', ");
                            sql.Append("'" + double.Parse(lQtyOrder.Text) + "', ");
                            sql.Append("'" + double.Parse(lQtyReceived.Text) + "', ");
                            sql.Append("'" + double.Parse(lUnitPrice.Text) + "', ");
                            sql.Append("'" + double.Parse(lVAT.Text) + "', ");
                            sql.Append("'" + tbNoteDetail.Text.Trim().Replace("'", "`") + "') ");

                            using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                            {
                                cmd.ExecuteNonQuery();
                                cmd = null;
                            }
                        }

                        line_no += 10;
                    }

                    trans.Commit();
                    conn.Close();
                    master.messageBox("Data has been updated.");
                }
                else
                {
                    master.messageBox("Tax facture number entered is not enough!");
                    return;
                }
            }
            lStatus.Text = "OPEN";
            statusChanged();
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message + "<br />" + ex.InnerException);
            trans.Rollback();
            conn.Close();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transaction/PurchaseInvoice.aspx");
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        try
        {
            if (wucConfirmBox1.StrMode == "Yes")
            {
                if (Session["InvoiceConfirmationReceiptMode"].ToString() == "Save")
                {
                    saveData();
                }
                else if (Session["InvoiceConfirmationReceiptMode"].ToString() == "Posting")
                {
                    postingData();
                }
            }
        }
        catch (Exception ex)
        {
            //MasterPage master = (MasterPage)this.Master;
            //master.messageBox(ex.Message);
        }
    }

    protected void wucSearch_Hide(object sender, EventArgs e)
    {
        if (Session["search"].ToString() == "pi_purchase_order_no")
        {
            tbReffOrderNo.Text = wucSearch1.result;
            bSearchReffOrder_Click(sender, e);
            upHeader.Update();
            Session["searchParamCompanyID"] = null;
        }
        else if (Session["search"].ToString() == "pi_inbound_delivery_no")
        {
            tbIDTransNo.Text = wucSearch1.result;
            bSearchIDTransNo_Click(sender, e);
            upHeader.Update();
            Session["searchParamPOTransNo"] = null;
        }

        Session["search"] = null;
    }

    protected void btnPosting_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;

        if (lbReffOrderNo.Text == "")
        {
            master.messageBox("Input reff order!");
            return;
        }
        else if (tbVendorInvoiceNo.Text.Trim().Replace("'", "`") == "")
        {
            master.messageBox("Input vendor invoice no!");
            return;
        }
        else if (tbNote.Text == "")
        {
            master.messageBox("Input note!");
            return;
        }
        else if (grid.Rows.Count <= 0)
        {
            master.messageBox("Input detail!");
            return;
        }
        else if (tbNote.Text == "")
        {
            master.messageBox("Input Note!");
            return;
        }
        //else if (Shared.ValidateParam("API", Session["username"].ToString()) == "0")
        //{
        //    master.messageBox("Please contact your head to approve this transaction! ");
        //    return;
        //}
        else
        {
            //belum ada validasi untuk serial no
            Session["InvoiceConfirmationReceiptMode"] = "Posting";
            showConfirmBox("Posting Data?");
        }
    }

    protected void postingData()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            conn1 = new SqlConnection(cAdih.getConnStr("Connection"));
            conn1.Open();
            trans1 = conn1.BeginTransaction();

            sql1.Length = 0;
            sql1.Append("UPDATE invoice_confirmation_receipt SET invoice_confirmation_receipt.status = '1', ");
            sql1.Append("invoice_confirmation_receipt.posted_by = '" + Session["username"].ToString() + "', ");
            sql1.Append("invoice_confirmation_receipt.posted_date = GETDATE() ");
            sql1.Append("FROM invoice_confirmation_receipt, payment_term ");
            sql1.Append("WHERE invoice_confirmation_receipt.trans_no = '" + tbTransNo.Text + "' ");
            sql1.Append("AND invoice_confirmation_receipt.payment_term = payment_term.id ");

            using (cmd1 = new SqlCommand(sql1.ToString(), conn1, trans1))
            {
                cmd1.ExecuteNonQuery();
                cmd1 = null;
            }

            trans1.Commit();
            conn1.Close();

            master.messageBox("Data has been posted.");
            lStatus.Text = "POSTED";
            statusChanged();
            Response.Redirect("~/Transaction/PurchaseInvoice.aspx");
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message + "<br />" + ex.InnerException);
            trans1.Rollback();
            conn1.Close();
        }
    }

}