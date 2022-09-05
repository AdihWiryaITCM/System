using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

public partial class Transaction_PurchaseOrderProcess : System.Web.UI.Page
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
    private DataTable dt;
    private ClassAdih cAdih = new ClassAdih();

    private decimal damountDetail = 0;
    private decimal dtaxDetail = 0;
    private decimal dtotalDetail = 0;
    private decimal ddisc = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                //Shared.BindPOPaymentMethod(ddlPaymentMethod);
                if (Request.QueryString["mode"].ToString() == "add")
                {
                    tbTransDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    tbCurrency.Text = "IDR";
                    lStatus.Text = "";
                    statusChanged();
                }
                else if (Request.QueryString["mode"].ToString() == "edit")
                {
                    tbTransNo.Text = Request.QueryString["trans_no"].ToString();

                    sql.Length = 0;
                    sql.Append("SELECT a.trans_no, a.trans_date, ");
                    sql.Append("a.vendor_id, e.vendor_name, a.purchase_requisition_no, ISNULL(f.mod_date, GETDATE()) AS pr_date, a.tax_rate, ");
                    sql.Append("a.ship_to, j.wh_description AS ship_to_name, ");
                    sql.Append("a.req_delv_date, a.payment_term, a.payment_term AS payment_term_name, a.currency_id, a.note, a.status ");
                    sql.Append("FROM purchase_order a WITH(READPAST) ");
                    sql.Append("INNER JOIN vendor e WITH(READPAST) ON a.vendor_id = e.vendor_no ");
                    sql.Append("INNER JOIN purchase_requisition f WITH(READPAST) ON a.purchase_requisition_no = f.trans_no ");
                    sql.Append("INNER JOIN site_wh j WITH(READPAST) ON a.ship_to = j.wh_id ");
                    sql.Append("INNER JOIN vendor v WITH(READPAST) ON a.vendor_id = v.vendor_no ");
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

                                    tbVendor.Text = dr["vendor_id"].ToString();
                                    tbVendor.ReadOnly = true;
                                    lblTax.Text = dr["tax_rate"].ToString();
                                    lbVendor.Text = dr["vendor_name"].ToString();
                                    if (lbVendor.Text != "")
                                    {
                                        ibSearchVendor.Visible = false;
                                    }
                                    tbPRNo.Text = dr["purchase_requisition_no"].ToString();
                                    tbPRNo.ReadOnly = true;
                                    lbPRNo.Text = DateTime.Parse(dr["pr_date"].ToString()).ToString("dd MMM yyyy");
                                    if (lbPRNo.Text != "01 Jan 1990")
                                    {
                                        ibSearchPRNo.Visible = false;
                                    }
                                    tbShipTo.Text = dr["ship_to"].ToString();
                                    lbShipTo.Text = dr["ship_to_name"].ToString();
                                    tbShipTo.ReadOnly = true;
                                    tbReqDelvDate.Text = DateTime.Parse(dr["req_delv_date"].ToString()).ToString("dd-MM-yyyy");
                                    tbPaymentTerm.Text = dr["payment_term"].ToString();
                                    tbPaymentTerm.ReadOnly = true;
                                    tbCurrency.Text = dr["currency_id"].ToString();
                                    tbCurrency.ReadOnly = true;
                                    tbNote.Text = dr["note"].ToString();
                                    //if (dr["office_necessities"].ToString() == "1")
                                    //{
                                    //    cbOfficeNecessities.Checked = true;
                                    //}
                                    //else if (dr["office_necessities"].ToString() == "0")
                                    //{
                                    //    cbOfficeNecessities.Checked = false;
                                    //}
                                    if (dr["status"].ToString() == "False")
                                    {
                                        lStatus.Text = "OPEN";
                                    }
                                    else if (dr["status"].ToString() == "True")
                                    {
                                        lStatus.Text = "POSTED";
                                    }
                                    statusChanged();
                                }
                            }
                        }
                    }

                    sql.Length = 0;
                    sql.Append("SELECT a.line_no, a.trans_no, a.article_no, b.article_description,  ");
                    sql.Append("a.qty, ISNULL((select sum(qty) from purchase_order_detail z inner ");
                    sql.Append("                               join purchase_order po on (z.trans_no = po.trans_no) ");
                    sql.Append(" ");
                    sql.Append("where z.trans_no <> e.trans_no and z.purchase_requisition_no = a.trans_no and z.pr_line_no = a.line_no and po.closed = '0'), 0) AS qty_ordered, ISNULL(e.qty, 0) AS qty_po, ");
                    sql.Append("ISNULL(e.unit_price, 0) AS unit_price, ISNULL(e.tax, 0) AS tax, e.note, isnull(e.unit_price, 0) as base_amount ");
                    sql.Append("FROM purchase_requisition_detail a WITH(READPAST) ");
                    sql.Append("INNER JOIN article b WITH(READPAST) ON a.article_no = b.article_no ");
                    sql.Append("LEFT JOIN purchase_order_detail e WITH(READPAST) ON e.purchase_requisition_no = a.trans_no AND a.line_no = e.pr_line_no ");
                    sql.Append("WHERE e.trans_no = '" + tbTransNo.Text.Trim().Replace("'", "`").ToUpper() + "' ");
                    sql.Append("order by trans_no, line_no ");

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
                                    int line_no = 10;
                                    double dblTax = 0;
                                    while (dr.Read())
                                    {
                                        dblTax = ((double.Parse(dr["qty_po"].ToString()) * double.Parse(dr["unit_price"].ToString()))) * (double.Parse(lblTax.Text) / 100);
                                        addDataToTable(line_no, dr["trans_no"].ToString(), dr["article_no"].ToString(),
                                            dr["article_description"].ToString(),double.Parse(dr["qty"].ToString()), double.Parse(dr["qty_ordered"].ToString()), double.Parse(dr["qty_po"].ToString()),
                                            double.Parse(dr["unit_price"].ToString()), dblTax, double.Parse(dr["base_amount"].ToString()), dr["note"].ToString(), int.Parse(dr["line_no"].ToString()),
                                            ((double.Parse(dr["qty_po"].ToString()) * double.Parse(dr["unit_price"].ToString()))) + dblTax, DateTime.Now, (DataTable)Session["dt"]);
                                        line_no += 10;
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
            else
            {
                btnPosting.Visible = true;

                sql.Length = 0;
                sql.Append("SELECT a.approval_by, b.username, a.approval_date ");
                sql.Append("FROM trans_approval a WITH(READPAST) ");
                sql.Append("INNER JOIN [user] b WITH(READPAST) ON a.approval_by = b.user_id ");
                sql.Append("WHERE a.trans_no = '" + tbTransNo.Text + "' ");
                sql.Append("ORDER BY a.approval_date ");

                using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
                {
                    conn.Open();
                    using (cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        using (dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                string par_approval = "";
                                btnSave.Visible = false;
                                while (dr.Read())
                                {
                                    par_approval += dr["username"].ToString() + " at " + DateTime.Parse(dr["approval_date"].ToString()).ToString("dd MMM yyy HH:mm") + "<br />";
                                    if (dr["approval_by"].ToString() == Session["username"].ToString())
                                    {
                                        btnPosting.Visible = false;
                                    }
                                }
                                //lblPARApproval.Text = par_approval.Trim();
                                upHeader.Update();
                            }
                        }
                    }
                }
            }
        }
        upGrid.Update();
    }

    protected void showConfirmBox(string message)
    {
        wucConfirmBox1.SubShowConfirmBox(message);
    }

    protected void ibSearchVendor_Click(object sender, EventArgs e)
    {
        Session["search"] = "vendor";
        wucSearch1.loadGrid();
    }

    protected void bSearchVendor_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT a.vendor_name, 'IDR' as currency_id,payment_terms_id,tax_id ");
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
                            lbVendor.Text = dr["vendor_name"].ToString();
                            tbCurrency.Text = dr["currency_id"].ToString();
                            tbPaymentTerm.Text = dr["payment_terms_id"].ToString();
                            tbVendor.ReadOnly = true;
                            ibSearchVendor.Visible = false;
                            lblTax.Text = dr["tax_id"].ToString();
                        }
                        else
                        {
                            lblTax.Text = "0";
                            tbVendor.Text = "";
                            tbCurrency.Text = "";
                            tbVendor.ReadOnly = false;
                            ibSearchVendor.Visible = true;
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

    protected void lbVendor_Click(object sender, EventArgs e)
    {
        lbVendor.Text = "";
        tbVendor.Text = "";
        tbCurrency.Text = "";
        tbVendor.ReadOnly = false;
        ibSearchVendor.Visible = true;
        upHeader.Update();
    }

    protected void ibSearchPRNo_Click(object sender, EventArgs e)
    {
        Session["search"] = "SearchPR";
        wucSearch1.loadGrid();
    }

    protected void bSearchPRNo_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (grid.Rows.Count == 0)
            {
                Session["dt"] = "NULL";
            }

            sql.Length = 0;
            sql.Append("SELECT DISTINCT CONVERT(VARCHAR(10), pr.posting_date, 120) AS posted_date, ");
            sql.Append("pr.ship_to, s.wh_description AS ship_to_name, pr.note ");
            sql.Append("FROM purchase_requisition pr WITH(READPAST) ");
            sql.Append("INNER JOIN purchase_requisition_detail prd WITH(READPAST) ON pr.trans_no = prd.trans_no ");
            //sql.Append("--LEFT JOIN( ");
            //sql.Append("--SELECT pod.purchase_requisition_no, pod.pr_line_no, CASE WHEN podd.trans_no is null then SUM(pod.qty) else SUM(pod.qty) - sum(isnull(idd.qty_received, 0)) end AS sum_qty ");
            //sql.Append("--  FROM purchase_order po WITH(READPAST) ");
            //sql.Append("--INNER JOIN purchase_order_detail pod WITH(READPAST) ON po.trans_no = pod.trans_no ");
            //sql.Append("--  LEFT JOIN purchase_order_closed podd WITH(READPAST) ON po.trans_no = podd.trans_no ");
            //sql.Append("--  LEFT JOIN inbound_delivery id WITH(READPAST) on id.reff_order_no = po.trans_no ");
            //sql.Append("--  LEFT JOIN inbound_delivery_detail idd WITH(READPAST) ON idd.trans_no = id.trans_no and idd.reff_line_no = pod.line_no ");
            //sql.Append("--    WHERE 1 = 1 ");
            //sql.Append("--    AND po.trans_no NOT IN(SELECT trans_no FROM purchase_order_cancelled WITH(READPAST)) ");
            //sql.Append("--  GROUP BY pod.purchase_requisition_no, pod.pr_line_no,podd.trans_no ");
            //sql.Append("--) AS po ON pr.trans_no = po.purchase_requisition_no AND prd.line_no = po.pr_line_no ");
            sql.Append("INNER JOIN site_wh s WITH(READPAST) ON pr.ship_to = s.wh_id ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append("AND pr.trans_no = '" + tbPRNo.Text.Trim().Replace("'", "`") + "' ");

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
                            lbPRNo.Text = DateTime.Parse(dr["posted_date"].ToString()).ToString("dd MMM yyyy");
                            tbPRNo.ReadOnly = true;
                            ibSearchPRNo.Visible = false;
                            tbNote.Text = dr["note"].ToString();

                            tbShipTo.Text = dr["ship_to"].ToString();
                            tbShipTo.ReadOnly = true;
                            lbShipTo.Text = dr["ship_to_name"].ToString();
                        }
                        else
                        {
                            lbPRNo.Text = "";
                            tbShipTo.Text = "";
                            lbShipTo.Text = "";
                            tbPRNo.ReadOnly = false;
                            ibSearchPRNo.Visible = true;
                            tbNote.Text = "";
                            master.messageBox("Invalid PR No!");
                            return;
                        }
                        upHeader.Update();
                    }
                }
            }

            sql1.Length = 0;
            sql1.Append("SELECT a.trans_no, b.article_no,c.article_description,b.qty, ");
            sql1.Append("isnull(e.sum_qty,0) AS qty_po, b.note, b.line_no ");
            sql1.Append("FROM purchase_requisition a WITH(READPAST) ");
            sql1.Append("INNER JOIN purchase_requisition_detail b WITH(READPAST) ON a.trans_no = b.trans_no ");
            sql1.Append("INNER JOIN article c WITH(READPAST) ON b.article_no = c.article_no ");
            sql1.Append("LEFT JOIN( ");
            sql1.Append("SELECT pod.purchase_requisition_no, pod.pr_line_no, sum(pod.qty) AS sum_qty  ");
            sql1.Append("  FROM purchase_order po WITH(READPAST) ");
            sql1.Append("  INNER JOIN purchase_order_detail pod WITH(READPAST) ON po.trans_no = pod.trans_no ");
            sql1.Append("    WHERE 1 = 1 ");
            sql1.Append("    AND pod.purchase_requisition_no = '" + tbPRNo.Text.Trim().Replace("'", "`") + "'  ");
            sql1.Append("  GROUP BY pod.purchase_requisition_no, pod.pr_line_no ");
            sql1.Append(") AS e ON a.trans_no = e.purchase_requisition_no AND b.line_no = e.pr_line_no ");
            sql1.Append("WHERE 1 = 1 ");
            sql1.Append("AND a.trans_no = '" + tbPRNo.Text.Trim().Replace("'", "`") + "'  ");
            sql1.Append("order by a.trans_no, b.line_no ");

            using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
            {
                conn.Open();
                using (cmd = new SqlCommand(sql1.ToString(), conn))
                {
                    using (dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            if (Session["dt"].ToString() == "NULL")
                            {
                                DataTable dt = new DataTable();
                                dt = createDataTable();
                                Session["dt"] = dt;
                            }

                            int line_no = 10;
                            while (dr.Read())
                            {
                                double dblPrice = 0;
                                if (lblTax.Text == "")
                                {
                                    lblTax.Text = "0";
                                }

                                double dblTax = double.Parse(dblPrice.ToString()) * (double.Parse(lblTax.Text) / double.Parse("100.00"));
                                double qtyPO = double.Parse(dr["qty"].ToString()) - double.Parse(dr["qty_po"].ToString());

                                deleteDataToTable(dr["trans_no"].ToString(), dr["line_no"].ToString());
                                addDataToTable(line_no, dr["trans_no"].ToString(), dr["article_no"].ToString(), dr["article_description"].ToString()
                                    , double.Parse(dr["qty"].ToString()), double.Parse(dr["qty_po"].ToString()),
                                    qtyPO, dblPrice, dblTax, dblPrice, dr["note"].ToString(), int.Parse(dr["line_no"].ToString()), (dblPrice + dblTax) * (double.Parse(dr["qty"].ToString()) - double.Parse(dr["qty_po"].ToString())),
                                    DateTime.Now, (DataTable)Session["dt"]);

                                line_no++;
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

    protected void lbPRNo_Click(object sender, EventArgs e)
    {
        lbPRNo.Text = "";
        tbPRNo.Text = "";
        tbPRNo.ReadOnly = false;
        ibSearchPRNo.Visible = true;

        tbVendor.Text = "";
        lbVendor.Text = "";
        ibSearchVendor.Visible = true;

        tbShipTo.Text = "";
        lbShipTo.Text = "";
        tbNote.Text = "";
        tbShipTo.ReadOnly = false;
        Session["dt"] = null;

        //if (ddlPaymentMethod.SelectedValue == "P2")
        //{
        //    tbPARNo.Text = "";
        //    lbPARNo.Text = "";
        //    ibPARNo.Visible = true;
        //    ibSearchVendor.Visible = false;
        //}

        upHeader.Update();

        clearGrid();
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
            dtColumn.ColumnName = "PRNo";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "articleNo";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "articleDesc";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "qtyPR";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "qtyOrdered";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "qtyPO";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "unitPrice";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "Amount";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "tax";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "baseAmount";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "note";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Int64");
            dtColumn.ColumnName = "prLineNo";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "totalAmount";
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

    private void addDataToTable(int lineNo, string PRNo, string articleNo, string articleDesc,
        double qtyPR, double qtyOrdered, double qtyPO, double unitPrice, double tax, double baseAmount, string note, int pr_line_no,
        Double totalAmount, DateTime addedDate, DataTable Table)
    {
        try
        {
            DataRow dtRow;

            dtRow = Table.NewRow();

            dtRow["lineNo"] = lineNo;
            dtRow["PRNo"] = PRNo;
            dtRow["articleNo"] = articleNo;
            dtRow["articleDesc"] = articleDesc;
            dtRow["qtyPR"] = qtyPR;
            dtRow["qtyOrdered"] = qtyOrdered;
            dtRow["qtyPO"] = qtyPO;
            dtRow["unitPrice"] = unitPrice;
            dtRow["note"] = note;
            dtRow["amount"] = qtyPO * unitPrice;
            dtRow["tax"] = tax;
            dtRow["prLineNo"] = pr_line_no;
            dtRow["totalAmount"] = ((qtyPO * unitPrice)) + tax;
            dtRow["baseAmount"] = baseAmount;
            dtRow["addedDate"] = addedDate;

            Table.Rows.Add(dtRow);
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void deleteDataToTable(string PRNo, string prLineNo)
    {
        try
        {
            DataRow[] dtRow = ((DataTable)Session["dt"]).Select("PRNo='" + PRNo + "' and prLineNo='" + prLineNo + "' ");
            //dtRow = ((DataTable)Session["dt"]).Select("prLineNo='" + prLineNo + "' ");

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

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridViewRow Row = e.Row;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox tbQtyPO = (TextBox)Row.FindControl("tbQtyPO");
                TextBox tbUnitPrice = (TextBox)Row.FindControl("tbUnitPrice");
                Label lblDAmount = (Label)Row.FindControl("lblDamount");
                Label lblDTax = (Label)Row.FindControl("lblDTax");
                Label lTotalAmount = (Label)Row.FindControl("lTotalAmount");

                damountDetail = damountDetail + decimal.Parse(lblDAmount.Text);
                dtaxDetail = dtaxDetail + decimal.Parse(lblDTax.Text);
                dtotalDetail = dtotalDetail + decimal.Parse(lTotalAmount.Text);
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[8].Text = damountDetail.ToString("##,#0.#0");
                e.Row.Cells[9].Text = dtaxDetail.ToString("##,#0.#0");
                e.Row.Cells[10].Text = dtotalDetail.ToString("##,#0.#0");

                lTotal.Text = damountDetail.ToString();
            }
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
                //ddlStore.Enabled = false;
            }
            else
            {
                //ddlStore.Enabled = true;
            }
            upHeader.Update();
            upPopUp.Update();
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

    protected void imgDelete_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gridRow = ((sender as ImageButton).Parent.Parent as GridViewRow);
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
            DataRow[] dtRow = ((DataTable)Session["dt"]).Select("lineNo='" + lineNo + "'");

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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;

        double totalAmount = 0;
        string strDate = DateTime.Now.ToShortDateString();
        foreach (GridViewRow gridRow in grid.Rows)
        {
            Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
            Label lArticleDesc = (Label)gridRow.FindControl("lArticleDesc");
            Label lQtyPR = (Label)gridRow.FindControl("lQtyPR");
            Label lQtyOrdered = (Label)gridRow.FindControl("lQtyOrdered");
            Label lQtyPO = (Label)gridRow.FindControl("lQtyPO");
            Label lblDTax = (Label)gridRow.FindControl("lblDtax");
            TextBox tbQtyPO = (TextBox)gridRow.FindControl("tbQtyPO");
            TextBox tbUnitPrice = (TextBox)gridRow.FindControl("tbUnitPrice");

            if (double.Parse(lQtyPR.Text) < double.Parse(lQtyOrdered.Text) + double.Parse(tbQtyPO.Text.Trim().Replace("'", "`")))
            {
                master.messageBox("Max Qty PO is " + (double.Parse(lQtyPR.Text) - double.Parse(lQtyOrdered.Text)).ToString() + "!");
                return;
            }

            if (double.Parse(tbQtyPO.Text.Trim().Replace("'", "`")) > 0)
            {
                totalAmount += double.Parse(tbQtyPO.Text.Trim()) * double.Parse(tbUnitPrice.Text.Trim()) + double.Parse(lblDTax.Text.Trim());
            }
        }
        if (lbVendor.Text == "")
        {
            master.messageBox("Input Vendor!");
            return;
        }
        else if (tbVendor.Text == tbShipTo.Text)
        {
            master.messageBox("Invalid Vendor!");
            return;
        }
        else if (tbPaymentTerm.Text == "")
        {
            master.messageBox("Input Payment Term!");
            return;
        }
        else if (lbPRNo.Text == "")
        {
            master.messageBox("Input Purchase Requisition No!");
            return;
        }
        else if (lbShipTo.Text == "")
        {
            master.messageBox("Input Ship To!");
            return;
        }
        else if (tbReqDelvDate.Text.Trim().Replace("'", "`") == "")
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
            Session["PurchaseOrderMode"] = "Save";
            showConfirmBox("Save Data?<br />Total Amount: " + tbCurrency.Text + " " + totalAmount.ToString("##,#0") + "");
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
                sql.Append("FROM purchase_order WITH(READPAST) ");
                sql.Append("WHERE YEAR(trans_date) = YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) ");
                sql.Append("AND MONTH(trans_date) = MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    using (dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            tbTransNo.Text = "PO" + dr["tahun"].ToString() + dr["bulan"].ToString() + dr["running_no"].ToString();
                            upHeader.Update();
                        }
                        else
                        {
                            master.messageBox("Invalid user id!");
                            return;
                        }
                    }
                }

                int line_no = 10;

                double totalAmount = 0;
                double total_tax = 0;
                double amount = 0;

                foreach (GridViewRow gridRow in grid.Rows)
                {
                    Label lPRNo = (Label)gridRow.FindControl("lPRNo");
                    Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
                    Label lPRLineNo = (Label)gridRow.FindControl("lPRLineNo");
                    TextBox tbQtyPO = (TextBox)gridRow.FindControl("tbQtyPO");
                    TextBox tbUnitPrice = (TextBox)gridRow.FindControl("tbUnitPrice");
                    TextBox tbNoteDetail = (TextBox)gridRow.FindControl("tbNote");
                    Label lAmount = (Label)gridRow.FindControl("lblDamount");
                    Label lTotalAmount = (Label)gridRow.FindControl("lTotalAmount");
                    Label lblDTax = (Label)gridRow.FindControl("lblDTax");
                    Label lBaseAmount = (Label)gridRow.FindControl("lBaseAmount");

                    lAmount.Text = (double.Parse(tbQtyPO.Text) * double.Parse(tbUnitPrice.Text)).ToString();
                    lblDTax.Text = (((double.Parse(tbQtyPO.Text) * double.Parse(tbUnitPrice.Text))) * double.Parse(lblTax.Text) / 100.00).ToString();
                    lTotalAmount.Text = ((double.Parse(tbQtyPO.Text) * double.Parse(tbUnitPrice.Text)) + double.Parse(lblDTax.Text)).ToString();

                    if (double.Parse(tbQtyPO.Text) > 0)
                    {
                        sql.Length = 0;
                        sql.Append("INSERT INTO[dbo].[purchase_order_detail] ");
                        sql.Append("([trans_no] ");
                        sql.Append(",[line_no] ");
                        sql.Append(",[purchase_requisition_no] ");
                        sql.Append(",[article_no] "); 
                        sql.Append(",[qty] ");
                        sql.Append(",[unit_price] ");
                        sql.Append(",[amount] ");
                        sql.Append(",[tax] ");
                        sql.Append(",[total_amount] ");
                        sql.Append(",[note] ");
                        sql.Append(",[pr_line_no]) ");
                        sql.Append("VALUES ");
                        sql.Append("('" + tbTransNo.Text + "' ");
                        sql.Append(",'" + line_no.ToString()  + "' ");
                        sql.Append(",'" + tbPRNo.Text + "' ");
                        sql.Append(",'" + lArticleNo.Text + "' ");
                        sql.Append(",'" + tbQtyPO.Text + "' ");
                        sql.Append(",'" + tbUnitPrice.Text  + "' ");
                        sql.Append(",'" + lAmount.Text + "' ");
                        sql.Append(",'" + lblTax.Text + "' ");
                        sql.Append(",'" + lTotalAmount.Text + "' ");
                        sql.Append(",'" + tbNoteDetail.Text  + "' ");
                        sql.Append(",'" + lPRLineNo.Text + "') ");

                        using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                        {
                            cmd.ExecuteNonQuery();
                            cmd = null;
                        }

                        line_no += 10;

                        //lTotalAmount.Text = (double.Parse(tbQtyPO.Text) * (double.Parse(tbUnitPrice.Text) + double.Parse(lblDTax.Text))).ToString("##,#0");
                        total_tax += double.Parse(lblDTax.Text);
                        amount += double.Parse(lAmount.Text);
                        totalAmount += double.Parse(lTotalAmount.Text); //((double.Parse(tbQtyPO.Text) * (double.Parse(tbUnitPrice.Text))) + double.Parse(lblDTax.Text));
                    }

                    grid.FooterRow.Cells[8].Text = amount.ToString("##,#0.#0");
                    grid.FooterRow.Cells[9].Text = total_tax.ToString("##,#0.#0");
                    grid.FooterRow.Cells[10].Text = totalAmount.ToString("##,#0.#0");
                    upGrid.Update();
                }

                sql.Length = 0;
                sql.Append("INSERT INTO[dbo].[purchase_order] ");
                sql.Append("([trans_no] ");
                sql.Append("          ,[trans_date] ");
                sql.Append("          ,[vendor_id] ");
                sql.Append("          ,[purchase_requisition_no] ");
                sql.Append("          ,[ship_to] ");
                sql.Append("          ,[req_delv_date] ");
                sql.Append("          ,[payment_term] ");
                sql.Append("          ,[currency_id] ");
                sql.Append("          ,[note] ");
                sql.Append("          ,[status] ");
                sql.Append("          ,[tax_rate] ");
                sql.Append("          ,[amount] ");
                sql.Append("          ,[tax] ");
                sql.Append("          ,[total_amount] ");
                sql.Append("          ,[approval_proxy] ");
                sql.Append("          ,[approval_status] ");
                sql.Append("          ,[created_by] ");
                sql.Append("          ,[created_date]) ");
                sql.Append("VALUES ");
                sql.Append("      ('" + tbTransNo.Text + "'");
                sql.Append("      ,'" + Shared.UItoDBDate(tbTransDate.Text) + "'");
                sql.Append("      ,'" + tbVendor.Text.Trim() + "'");
                sql.Append("      ,'" + tbPRNo.Text.Trim() + "'");
                sql.Append("      ,'" + tbShipTo.Text + "'");
                sql.Append("      ,'" + Shared.UItoDBDate(tbReqDelvDate.Text) + "'");
                sql.Append("      ,'" + tbPaymentTerm.Text + "'");
                sql.Append("      ,'" + tbCurrency.Text + "'");
                sql.Append("      ,'" + tbNote.Text + "'");
                sql.Append("      ,'0'");
                sql.Append("      ,'" + lblTax.Text + "'");
                sql.Append("      ,'" + double.Parse(amount.ToString()) + "'");
                sql.Append("      ,'" + double.Parse(total_tax.ToString()) + "'");
                sql.Append("      ,'" + double.Parse(totalAmount.ToString()) + "'");
                sql.Append("      ,'0'");
                sql.Append("      ,'NEED APPROVE'");
                sql.Append("      ,'" + Session["username"].ToString() + "'");
                sql.Append("      ,GETDATE() )");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                trans.Commit();
                conn.Close();
                master.messageBox("Data has been saved. Trans No: " + tbTransNo.Text + "");
            }

            //EDIT
            else if (tbTransNo.Text != "")
            {
                conn = new SqlConnection(cAdih.getConnStr("Connection"));
                conn.Open();
                trans = conn.BeginTransaction();

                sql.Length = 0;
                sql.Append("DELETE purchase_order_detail WHERE trans_no = '" + tbTransNo.Text + "' ");
                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                int line_no = 10;

                double totalAmount = 0;
                double total_tax = 0;
                double amount = 0;

                foreach (GridViewRow gridRow in grid.Rows)
                {
                    Label lPRNo = (Label)gridRow.FindControl("lPRNo");
                    Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
                    Label lPRLineNo = (Label)gridRow.FindControl("lPRLineNo");
                    TextBox tbQtyPO = (TextBox)gridRow.FindControl("tbQtyPO");
                    TextBox tbUnitPrice = (TextBox)gridRow.FindControl("tbUnitPrice");
                    TextBox tbNoteDetail = (TextBox)gridRow.FindControl("tbNote");
                    Label lAmount = (Label)gridRow.FindControl("lblDamount");
                    Label lTotalAmount = (Label)gridRow.FindControl("lTotalAmount");
                    Label lblDTax = (Label)gridRow.FindControl("lblDTax");
                    Label lBaseAmount = (Label)gridRow.FindControl("lBaseAmount");

                    lAmount.Text = (double.Parse(tbQtyPO.Text) * double.Parse(tbUnitPrice.Text)).ToString();
                    lblDTax.Text = (((double.Parse(tbQtyPO.Text) * double.Parse(tbUnitPrice.Text))) * double.Parse(lblTax.Text) / 100.00).ToString();
                    lTotalAmount.Text = ((double.Parse(tbQtyPO.Text) * double.Parse(tbUnitPrice.Text)) + double.Parse(lblDTax.Text)).ToString();

                    if (double.Parse(tbQtyPO.Text) > 0)
                    {
                        sql.Length = 0;
                        sql.Append("INSERT INTO[dbo].[purchase_order_detail] ");
                        sql.Append("([trans_no] ");
                        sql.Append(",[line_no] ");
                        sql.Append(",[purchase_requisition_no] ");
                        sql.Append(",[article_no] ");
                        sql.Append(",[qty] ");
                        sql.Append(",[unit_price] ");
                        sql.Append(",[amount] ");
                        sql.Append(",[tax] ");
                        sql.Append(",[total_amount] ");
                        sql.Append(",[note] ");
                        sql.Append(",[pr_line_no]) ");
                        sql.Append("VALUES ");
                        sql.Append("('" + tbTransNo.Text + "' ");
                        sql.Append(",'" + line_no.ToString() + "' ");
                        sql.Append(",'" + tbPRNo.Text + "' ");
                        sql.Append(",'" + lArticleNo.Text + "' ");
                        sql.Append(",'" + tbQtyPO.Text + "' ");
                        sql.Append(",'" + double.Parse(tbUnitPrice.Text) + "' ");
                        sql.Append(",'" + double.Parse(lAmount.Text) + "' ");
                        sql.Append(",'" + double.Parse(lblTax.Text) + "' ");
                        sql.Append(",'" + double.Parse(lTotalAmount.Text) + "' ");
                        sql.Append(",'" + tbNoteDetail.Text + "' ");
                        sql.Append(",'" + lPRLineNo.Text + "') ");

                        using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                        {
                            cmd.ExecuteNonQuery();
                            cmd = null;
                        }

                        line_no += 10;

                        //lTotalAmount.Text = (double.Parse(tbQtyPO.Text) * (double.Parse(tbUnitPrice.Text) + double.Parse(lblDTax.Text))).ToString("##,#0");
                        total_tax += double.Parse(lblDTax.Text);
                        amount += double.Parse(lAmount.Text);
                        totalAmount += double.Parse(lTotalAmount.Text); //((double.Parse(tbQtyPO.Text) * (double.Parse(tbUnitPrice.Text))) + double.Parse(lblDTax.Text));

                    }


                    grid.FooterRow.Cells[8].Text = amount.ToString("##,#0.#0");
                    grid.FooterRow.Cells[9].Text = total_tax.ToString("##,#0.#0");
                    grid.FooterRow.Cells[10].Text = totalAmount.ToString("##,#0.#0");
                    upGrid.Update();
                }

                sql.Length = 0;
                sql.Append("UPDATE purchase_order SET ");
                sql.Append("vendor_id = '" + tbVendor.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("purchase_requisition_no = '" + tbPRNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("ship_to = '" + tbShipTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("req_delv_date = '" + cAdih.engFormatDate(tbReqDelvDate.Text) + "', ");
                sql.Append("payment_term = '" + tbPaymentTerm.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("currency_id = '" + tbCurrency.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("note = '" + tbNote.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("tax_rate = '" + lblTax.Text + "', ");
                sql.Append("total_amount = '" + totalAmount + "', ");
                sql.Append("last_modified_by = '" + Session["username"].ToString() + "', ");
                sql.Append("last_modified_date = GETDATE() ");
                sql.Append("WHERE trans_no = '" + tbTransNo.Text + "' ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                trans.Commit();
                conn.Close();
                master.messageBox("Data has been updated.");
            }
            lStatus.Text = "OPEN";
            statusChanged();
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
            trans.Rollback();
            conn.Close();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transaction/PurchaseOrder.aspx");
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        try
        {
            MasterPage master = (MasterPage)this.Master;
            if (wucConfirmBox1.StrMode == "Yes")
            {
                if (Session["PurchaseOrderMode"].ToString() == "Save")
                {
                    saveData();
                }
                else if (Session["PurchaseOrderMode"].ToString() == "Posting")
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
        if (Session["search"].ToString() == "vendor")
        {
            tbVendor.Text = wucSearch1.result;
            bSearchVendor_Click(sender, e);
            upHeader.Update();
        }
        else if (Session["search"].ToString() == "SearchPR")
        {
            tbPRNo.Text = wucSearch1.result;
            bSearchPRNo_Click(sender, e);
            upHeader.Update();
        }
        Session["search"] = null;
    }

    protected void btnPosting_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;

        double totalAmount = 0;
        double totalQty = 0;

        foreach (GridViewRow gridRow in grid.Rows)
        {
            Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
            Label lArticleDesc = (Label)gridRow.FindControl("lArticleDesc");
            Label lQtyPR = (Label)gridRow.FindControl("lQtyPR");
            Label lQtyOrdered = (Label)gridRow.FindControl("lQtyOrdered");
            Label lQtyPO = (Label)gridRow.FindControl("lQtyPO");
            Label lBaseAmount = (Label)gridRow.FindControl("lBaseAmount");
            TextBox tbQtyPO = (TextBox)gridRow.FindControl("tbQtyPO");
            TextBox tbUnitPrice = (TextBox)gridRow.FindControl("tbUnitPrice");

            if (double.Parse(lQtyPR.Text) < double.Parse(lQtyOrdered.Text) + double.Parse(tbQtyPO.Text.Trim().Replace("'", "`")))
            {
                master.messageBox("Max Qty PO is " + (double.Parse(lQtyPR.Text) - double.Parse(lQtyOrdered.Text)).ToString() + "!");
                return;
            }

            if (double.Parse(tbQtyPO.Text.Trim().Replace("'", "`")) > 0)
            {
                totalQty += double.Parse(tbQtyPO.Text.Trim().Replace("'", "`"));

                totalAmount = double.Parse(tbQtyPO.Text.Trim().Replace("'", "`")) * double.Parse(tbUnitPrice.Text.Trim().Replace("'", "`"));
            }
        }
        if (lbVendor.Text == "")
        {
            master.messageBox("Input Vendor!");
            return;
        }
        else if (lbPRNo.Text == "")
        {
            master.messageBox("Input Purchase Requisition No!");
            return;
        }
        else if (lbShipTo.Text == "")
        {
            master.messageBox("Input Ship To!");
            return;
        }
        else if (tbReqDelvDate.Text.Trim().Replace("'", "`") == "")
        {
            master.messageBox("Input Request Delivery Date!");
            return;
        }
        //else if (lbPaymentTerm.Text == "" && ddlPaymentMethod.SelectedValue != "P2")
        //{
        //    master.messageBox("Input Payment Term!");
        //    return;
        //}
        else if (tbNote.Text.Trim().Replace("'", "`") == "")
        {
            master.messageBox("Input Note!");
            return;
        }
        else if (totalQty == 0)
        {
            master.messageBox("Input qty!");
            return;
        }
        else
        {
            Session["PurchaseOrderMode"] = "Posting";
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
            sql1.Append("INSERT INTO trans_approval(trans_no, approval_by, approval_proxy, approval_date) ");
            sql1.Append("SELECT '" + tbTransNo.Text + "', user_approval, user_proxy, GETDATE() FROM form_approval WITH(READPAST) ");
            sql1.Append("WHERE user_approval = '" + Session["username"].ToString() + "' ");

            using (cmd1 = new SqlCommand(sql1.ToString(), conn1, trans1))
            {
                cmd1.ExecuteNonQuery();
                cmd1 = null;
            }

            sql1.Length = 0;
            sql1.Append("UPDATE purchase_order SET status = '1', ");
            sql1.Append("posted_by = '" + Session["username"].ToString() + "', ");
            sql1.Append("posted_date = GETDATE() ");
            sql1.Append("WHERE trans_no = '" + tbTransNo.Text + "' ");

            using (cmd1 = new SqlCommand(sql1.ToString(), conn1, trans1))
            {
                cmd1.ExecuteNonQuery();
                cmd1 = null;
            }

            trans1.Commit();
            conn1.Close();

            double po_total_amount = 0;
            double po_approval_proxy = 0;

            sql.Length = 0;
            sql.Append("SELECT po.trans_no, po.total_amount, ISNULL(ta.sum_approval_proxy, 0) AS approval_proxy ");
            sql.Append("FROM purchase_order po WITH(READPAST) ");
            sql.Append("LEFT JOIN( ");
            sql.Append("    SELECT trans_no, SUM(approval_proxy) AS sum_approval_proxy, MAX(approval_date) AS max_approval_date ");
            sql.Append("    FROM trans_approval WITH(READPAST) ");
            sql.Append("    GROUP BY trans_no ");
            sql.Append(") AS ta ON po.trans_no = ta.trans_no ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append("AND po.trans_no = '" + tbTransNo.Text + "' ");

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
                            po_total_amount = double.Parse(dr["total_amount"].ToString());
                            po_approval_proxy = double.Parse(dr["approval_proxy"].ToString());
                        }
                    }
                }
            }

            string message; //= "This transaction has been approved";

            if (po_total_amount > po_approval_proxy)
            {
                message = "This transaction has not been approved, proxy is still not enough.<br />An please contact to your Head.";
            }
            else
            {
                if (po_total_amount == 0 && po_approval_proxy == 0)
                {
                    message = "This transaction has not been approved, proxy is still not enough.<br />An please contact to your Head.";
                }
                else
                {
                    message = "This transaction has been approved";
                }
            }

            master.messageBox(message);
            lStatus.Text = "POSTED";
            statusChanged();
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
            trans1.Rollback();
            conn1.Close();
        }
    }

}