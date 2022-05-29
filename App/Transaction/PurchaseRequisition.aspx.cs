using Microsoft.VisualBasic;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.WebControls;

public partial class Transaction_PermintaanPembelianBarang : System.Web.UI.Page
{
    private SqlConnection conn;
    private SqlCommand cmd;
    private SqlDataReader dr;
    private StringBuilder sql = new StringBuilder();
    private Shared all = new Shared();
    private DataTable dt;
    private ClassAdih cAdih = new ClassAdih();

    private string connName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            dt = createDataTable();
            Session["dt"] = dt;
            tbTanggalTransaksi.Text = DateTime.Now.ToString("dd-MM-yyyy");
            connName = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            loadArticle();
            loadWH();
            if (Request.Params["action"].ToString().Equals("edit"))
            {
                Session["SaveMode"] = "edit";
                tbPrNO.Text = Request.Params["trans_no"].ToString();
                LoadData();
                LoadStatus();
            }
            else
            {
                btnPosting.Visible = false;
                Session["SaveMode"] = "add";
            }
        }
    }

    private void LoadData()
    {
        sql.Length = 0;
        sql.Append("SELECT a.trans_no, a.trans_date,a.total_amount,a.note,a.status,a.ship_to ");
        sql.Append("FROM purchase_requisition a WITH(READPAST) ");
        sql.Append("WHERE 1 = 1 ");
        sql.Append("AND trans_no = '" + tbPrNO.Text + "'");

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
                        ddlWH.SelectedValue = dr["ship_to"].ToString();
                        lblStatus.Text = dr["status"].ToString();
                        tbTanggalTransaksi.Text = DateTime.Parse(dr["trans_date"].ToString()).ToString("dd-MM-yyyy");
                        tbNote.Text = dr["note"].ToString();
                        tbTotalAmount.Text = dr["total_amount"].ToString();
                    }
                }
            }
        }

        sql.Length = 0;
        sql.Append("SELECT a.article_no, b.article_description,a.qty,a.info_price,a.total_amount,a.required_date,a.note ");
        sql.Append("FROM purchase_requisition_detail a WITH(READPAST) ");
        sql.Append("INNER JOIN article b WITH(READPAST) on b.article_no = a.article_no ");
        sql.Append("WHERE 1 = 1 ");
        sql.Append("AND trans_no = '" + tbPrNO.Text + "'");

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
                            addDataToTable(dr["article_no"].ToString(), dr["article_description"].ToString(), double.Parse(dr["qty"].ToString()),
                                double.Parse(dr["info_price"].ToString()), double.Parse(dr["total_amount"].ToString()), DateTime.Parse(dr["required_date"].ToString()), dr["qty"].ToString(), dt);
                        }
                        loadGrid();
                    }
                }
            }
        }
    }

    private void loadWH()
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT '' AS value, '-' AS text ");
            sql.Append("UNION ");
            sql.Append("SELECT a.wh_id AS value, a.wh_description AS text FROM site_wh a WHERE 1 = 1  ");
            sql.Append("ORDER BY text ");

            all.loadDdl(sql.ToString(), connName, ddlWH);
            upHeader.Update();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    private void loadArticle()
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT '' AS value, '-' AS text ");
            sql.Append("UNION ");
            sql.Append("SELECT a.article_no AS value, a.article_description AS text FROM article a WHERE 1 = 1  ");
            sql.Append("ORDER BY text ");

            all.loadDdl(sql.ToString(), connName, ddlArticle);
            upHeader.Update();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
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
            dtColumn.DataType = Type.GetType("System.Int64");
            dtColumn.ColumnName = "lineNo";
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
            dtColumn.ColumnName = "qty";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "price";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "total_amount";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.DateTime");
            dtColumn.ColumnName = "required_date";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "note_detail";
            dt.Columns.Add(dtColumn);

            return dt;
        }
        catch
        {
            return null;
        }
    }

    private void addDataToTable(string articleNo, string articleDesc,
        double qty, double price, double total_amount, DateTime required_date, string note_detail, DataTable Table)
    {
        try
        {
            DataRow dtRow;

            dtRow = Table.NewRow();

            string lineNo = ((DataTable)Session["dt"]).Compute("MAX(lineNo)", "").ToString();
            if (lineNo == "")
            {
                lineNo = "10";
            }
            else
            {
                lineNo = (int.Parse(lineNo) + 10).ToString();
            }

            dtRow["lineNo"] = lineNo;
            dtRow["articleNo"] = articleNo;
            dtRow["articleDesc"] = articleDesc;
            dtRow["qty"] = qty;
            dtRow["price"] = price;
            dtRow["total_amount"] = total_amount;
            dtRow["required_date"] = required_date;
            dtRow["note_detail"] = note_detail;

            Table.Rows.Add(dtRow);
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
            if (ddlArticle.SelectedValue == "")
            {
                master.messageBox("Tolong masukan article!");
                return;
            }
            else if (tbQty.Text == "")
            {
                master.messageBox("Tolong masukan qty!");
                return;
            }
            else if (!Information.IsNumeric(tbQty.Text))
            {
                master.messageBox("Qty harus berupa angka!");
                return;
            }
            else if (decimal.Parse(tbQty.Text) <= 0)
            {
                master.messageBox("Qty harus lebih dari 0!");
                return;
            }
            else if (tbInfoPrice.Text == "")
            {
                master.messageBox("Tolong masukan info price!");
                return;
            }
            else if (!Information.IsNumeric(tbInfoPrice.Text))
            {
                master.messageBox("Info price harus berupa angka!");
                return;
            }
            else if (decimal.Parse(tbInfoPrice.Text) <= 0)
            {
                master.messageBox("Info price harus lebih dari 0!");
                return;
            }
            else if (tbReqDate.Text == "")
            {
                master.messageBox("Tolong masukan required date!");
                return;
            }
            else
            {
                addDataToTable(ddlArticle.SelectedValue, ddlArticle.SelectedItem.Text, double.Parse(tbQty.Text), double.Parse(tbInfoPrice.Text),
                    double.Parse(tbInfoPrice.Text) * double.Parse(tbQty.Text), DateTime.Parse(cAdih.engFormatDate(tbReqDate.Text)), tbNoteDetail.Text, (DataTable)(Session["dt"]));
            }

            loadGrid();
            clearDetail();
        }
        catch (Exception ex)
        {
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
                tbTotalAmount.Text = Double.Parse(((DataTable)Session["dt"]).Compute("SUM(total_amount)", "").ToString()).ToString("#,#0;(#,#0);0");
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

    protected void clearDetail()
    {
        ddlArticle.SelectedValue = "";
        tbQty.Text = "";
        tbNoteDetail.Text = "";
        tbInfoPrice.Text = "0";
        tbReqDate.Text = "";

        upDetail.Update();
    }

    protected void showConfirmBox(string message)
    {
        wucConfirmBox1.SubShowConfirmBox(message);
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        try
        {
            if (Session["ConfirmMode"].ToString() == "Save")
            {
                SaveData();
            }
            else
            {
                PostingData();
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    private void PostingData()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            conn = new SqlConnection(cAdih.getConnStr("Connection"));
            conn.Open();

            sql.Length = 0;
            sql.Append("UPDATE [dbo].[purchase_requisition] ");
            sql.Append("SET [status] = 'POSTED'");
            sql.Append("   ,[posting_by] = '" + Session["username"].ToString() + "'");
            sql.Append("   ,[posting_date] = GETDATE() ");
            sql.Append("WHERE trans_no = '" + tbPrNO.Text + "'");
            lblStatus.Text = "POSTED";

            using (cmd = new SqlCommand(sql.ToString(), conn))
            {
                cmd.ExecuteNonQuery();
                cmd = null;
            }

            conn.Close();
            master.messageBox("Data has been posted");
            LoadStatus();
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    private void LoadStatus()
    {
        if (lblStatus.Text == "1")
        {
            upDetail.Visible = false;
            btnPosting.Visible = false;
            btnSave.Visible = false;
            ddlWH.Enabled = false;
            tbNote.ReadOnly = true;
            grid.Columns[7].Visible = false;
        }
        else if(lblStatus.Text == "")
        {
            btnPosting.Visible = false;
        }
        upHeader.Update();
        upGrid.Update();
        upDetail.Update();
    }

    private void SaveData()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (Session["SaveMode"].ToString() == "add")
            {
                conn = new SqlConnection(cAdih.getConnStr("Connection"));
                conn.Open();
                sql.Length = 0;
                sql.Append("SELECT RIGHT('000000' + CAST(ISNULL(MAX(CAST(RIGHT(trans_no, 6) AS INT)), 0) + 1 AS VARCHAR), 6) AS running_no, ");
                sql.Append("YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) AS tahun, RIGHT('00' + CAST(MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) AS VARCHAR), 2) AS bulan ");
                sql.Append("FROM purchase_requisition ");
                sql.Append("WHERE YEAR(trans_date) = YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) ");
                sql.Append("AND MONTH(trans_date) = MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) ");

                using (cmd = new SqlCommand(sql.ToString(), conn))
                {
                    using (dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            tbPrNO.Text = "PR" + dr["tahun"].ToString() + dr["bulan"].ToString() + dr["running_no"].ToString();
                            upHeader.Update();
                        }
                    }
                }

                sql.Length = 0;
                sql.Append("INSERT INTO[dbo].[purchase_requisition] ");
                sql.Append("([trans_no] ");
                sql.Append("          ,[trans_date] ");
                sql.Append("          ,[ship_to] ");
                sql.Append("          ,[total_amount] ");
                sql.Append("          ,[note] ");
                sql.Append("          ,[STATUS] ");
                sql.Append("          ,[create_by] ");
                sql.Append("          ,[create_date] ");
                sql.Append("          ,[mod_by] ");
                sql.Append("          ,[mod_date]) ");
                sql.Append("VALUES( ");
                sql.Append("'" + tbPrNO.Text.Trim() + "'");
                sql.Append(",CONVERT(VARCHAR(10), GETDATE(),120)");
                sql.Append(",'" + ddlWH.SelectedValue.Trim() + "'");
                sql.Append(",'" + double.Parse(tbTotalAmount.Text) + "'");
                sql.Append(",'" + tbNote.Text.Trim() + "'");
                sql.Append(",'HOLD'");
                sql.Append(",'" + Session["username"].ToString() + "'");
                sql.Append(",GETDATE()");
                sql.Append(",'" + Session["username"].ToString() + "'");
                sql.Append(",GETDATE())");

                using (cmd = new SqlCommand(sql.ToString(), conn))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                int line_no = 10;

                foreach (GridViewRow gridRow in grid.Rows)
                {
                    Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
                    Label lQty = (Label)gridRow.FindControl("lQty");
                    Label lPrice = (Label)gridRow.FindControl("lPrice");
                    Label lTotalAmount = (Label)gridRow.FindControl("lTotalAmount");
                    Label lRequiredDate = (Label)gridRow.FindControl("lRequiredDate");
                    Label lNoteDetail = (Label)gridRow.FindControl("lNoteDetail");

                    sql.Length = 0;
                    sql.Append("INSERT INTO[dbo].[purchase_requisition_detail] ");
                    sql.Append("([trans_no]  ");
                    sql.Append("          ,[line_no] ");
                    sql.Append("          ,[article_no] ");
                    sql.Append("          ,[qty] ");
                    sql.Append("          ,[info_price] ");
                    sql.Append("          ,[total_amount] ");
                    sql.Append("          ,[required_date] ");
                    sql.Append("          ,[note]) ");
                    sql.Append("VALUES ");
                    sql.Append("      ('" + tbPrNO.Text.Trim() + "'");
                    sql.Append("      ,'" + line_no + "'");
                    sql.Append("      ,'" + lArticleNo.Text + "'");
                    sql.Append("      ,'" + lQty.Text + "'");
                    sql.Append("      ,'" + double.Parse(lPrice.Text) + "'");
                    sql.Append("      ,'" + double.Parse(lTotalAmount.Text) + "'");
                    sql.Append("      ,'" + cAdih.engFormatDate(lRequiredDate.Text) + "'");
                    sql.Append("      ,'" + lNoteDetail.Text + "')");

                    using (cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }
                    line_no += 10;
                }
                conn.Close();
                btnPosting.Visible = true;
                Session["SaveMode"] = "edit";
                master.messageBox("Data has been save");
            }
            else
            {
                conn = new SqlConnection(cAdih.getConnStr("Connection"));
                conn.Open();

                sql.Length = 0;
                sql.Append("UPDATE [dbo].[purchase_requisition] ");
                sql.Append("SET [ship_to] = '" + ddlWH.SelectedValue + "'");
                sql.Append("   ,[total_amount] = '" + double.Parse(tbTotalAmount.Text) + "'");
                sql.Append("   ,[status] = '" + lblStatus.Text + "'");
                sql.Append("   ,[note] = '" + tbNote.Text + "'");
                sql.Append("   ,[mod_by] = '" + Session["username"].ToString() + "'");
                sql.Append("   ,[mod_date] = GETDATE() ");
                sql.Append("WHERE trans_no = '" + tbPrNO.Text + "'");

                using (cmd = new SqlCommand(sql.ToString(), conn))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                sql.Length = 0;
                sql.Append("DELETE [purchase_requisition_detail] WHERE trans_no = '" + tbPrNO.Text + "' ");

                using (cmd = new SqlCommand(sql.ToString(), conn))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                int line_no = 10;

                foreach (GridViewRow gridRow in grid.Rows)
                {
                    Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
                    Label lQty = (Label)gridRow.FindControl("lQty");
                    Label lPrice = (Label)gridRow.FindControl("lPrice");
                    Label lTotalAmount = (Label)gridRow.FindControl("lTotalAmount");
                    Label lRequiredDate = (Label)gridRow.FindControl("lRequiredDate");
                    Label lNoteDetail = (Label)gridRow.FindControl("lNoteDetail");

                    sql.Length = 0;
                    sql.Append("INSERT INTO[dbo].[purchase_requisition_detail] ");
                    sql.Append("([trans_no]  ");
                    sql.Append("          ,[line_no] ");
                    sql.Append("          ,[article_no] ");
                    sql.Append("          ,[qty] ");
                    sql.Append("          ,[info_price] ");
                    sql.Append("          ,[total_amount] ");
                    sql.Append("          ,[required_date] ");
                    sql.Append("          ,[note]) ");
                    sql.Append("VALUES ");
                    sql.Append("      ('" + tbPrNO.Text.Trim() + "'");
                    sql.Append("      ,'" + line_no + "'");
                    sql.Append("      ,'" + lArticleNo.Text + "'");
                    sql.Append("      ,'" + lQty.Text + "'");
                    sql.Append("      ,'" + double.Parse(lPrice.Text) + "'");
                    sql.Append("      ,'" + double.Parse(lTotalAmount.Text) + "'");
                    sql.Append("      ,'" + cAdih.engFormatDate(lRequiredDate.Text) + "'");
                    sql.Append("      ,'" + lNoteDetail.Text + "')");

                    using (cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }
                    line_no += 10;
                }
                conn.Close();
                master.messageBox("Data has been update");
            }
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        if (ddlWH.SelectedValue == "")
        {
            master.messageBox("Tolong masukan WH Site!");
            return;
        }
        else if (tbNote.Text == "")
        {
            master.messageBox("Tolong masukan Note!");
            return;
        }
        else if (grid.Rows.Count <= 0)
        {
            master.messageBox("Tolong masukan detail!");
            return;
        }

        Session["ConfirmMode"] = "Save";
        showConfirmBox("Save Data?");
    }

    protected void grid_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            GridViewRow Row = e.Row;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lRequiredDate = (Label)Row.FindControl("lRequiredDate");
                lRequiredDate.Text = DateTime.Parse(lRequiredDate.Text).ToString("dd-MM-yyyy");
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void lbDelete_Click(object sender, EventArgs e)
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("PurchaseRequisitionList.aspx");
    }

    protected void btnPosting_Click(object sender, EventArgs e)
    {
        Session["ConfirmMode"] = "Posting";
        showConfirmBox("Posting Data?");
    }
}