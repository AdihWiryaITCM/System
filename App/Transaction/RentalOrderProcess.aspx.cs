using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transaction_RentalOrderProcess : Page
{
    private StringBuilder sql = new StringBuilder();
    private StringBuilder sql1 = new StringBuilder();
    private StringBuilder sql2 = new StringBuilder();
    private StringBuilder sql3 = new StringBuilder();
    private SqlTransaction trans;
    private SqlTransaction trans1;
    private SqlTransaction trans2;
    private SqlTransaction trans3;
    private SqlConnection conn;
    private SqlConnection conn1;
    private SqlConnection conn2;
    private SqlConnection conn3;
    private SqlCommand cmd;
    private SqlCommand cmd1;
    private SqlCommand cmd2;
    private SqlCommand cmd3;
    private SqlDataReader dr;
    private SqlDataReader dr1;
    private SqlDataReader dr2;
    private SqlDataReader dr3;
    private DataTable dt;
    private ClassAdih cAdih = new ClassAdih();
    private string fileFolder = "";
    private static string strFileName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            grid.Columns[6].Visible = false;
            if (!this.IsPostBack)
            {
                loadDdlInstallationBy();
                ddlInstallationBy_SelectedIndexChanged(sender, e);
                loadDdlInstallationBySite();

                DataTable dt = new DataTable();
                dt = createDataTable();
                Session["dt"] = dt;

                if (Request.QueryString["mode"].ToString() == "add")
                {
                    tbPaymentTerm.ReadOnly = true;
                    tbTransDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    tbCurrency.Text = "IDR";
                    lStatus.Text = "";
                    statusChanged(lStatus.Text);
                }
                else if (Request.QueryString["mode"].ToString() == "edit")
                {
                    tbTransNo.Text = Request.QueryString["trans_no"].ToString();

                    sql.Length = 0;
                    sql.Append("SELECT a.trans_no, a.trans_date, a.status,   ");
                    sql.Append("a.sold_to, c.customer_name AS sold_to_name, a.bill_to, (CASE WHEN d.alias_name = '' THEN d.alias_name_full ELSE d.alias_name_full + ' ' + d.alias_name END) AS bill_to_name,  ");
                    sql.Append("a.ship_to, (CASE WHEN e.alias_name = '' THEN e.alias_name_full ELSE e.alias_name_full + ' ' + e.alias_name END) AS ship_to_name,  ");
                    sql.Append("a.cust_po_no, a.cust_po_date, a.installation_date, a.currency_id, a.payment_term, i.description AS payment_term_desc, a.installation_charge, a.installation_by, a.installation_by_site, h.description AS installation_desc, a.note, a.status,  ");
                    sql.Append("a.total_amount, a.disc_amount, a.tax_amount, a.amount, a.quot_trans_no, qr.trans_date AS quot_date  ");
                    sql.Append("FROM rental_order a WITH(READPAST)  ");
                    sql.Append("INNER JOIN customer c WITH(READPAST) ON a.sold_to = c.customer_no  ");
                    sql.Append("INNER JOIN customer_address d WITH(READPAST) ON a.bill_to = d.id AND d.customer_no = a.sold_to AND d.address_type = '01'  ");
                    sql.Append("INNER JOIN customer_address e WITH(READPAST) ON a.ship_to = e.id AND e.customer_no = a.sold_to AND e.address_type = '02'  ");
                    sql.Append("INNER JOIN source h WITH(READPAST) ON a.installation_by = h.id AND h.source_type = 'Service'  ");
                    sql.Append("INNER JOIN payment_term i WITH(READPAST) ON a.payment_term = i.id  ");
                    sql.Append("LEFT JOIN quotation_rental qr WITH(READPAST) ON a.quot_trans_no = qr.trans_no  ");
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
                                    tbSearchQuot.Text = dr["quot_trans_no"].ToString();
                                    lbSearchQuot.Text = DateTime.Parse(dr["quot_date"].ToString()).ToString("dd-MM-yyyy");
                                    if (dr["status"].ToString() == "False")
                                    {
                                        lStatus.Text = "OPEN";
                                    }
                                    else if (dr["status"].ToString() == "True")
                                    {
                                        lStatus.Text = "POSTED";
                                    }
                                    statusChanged(lStatus.Text);
                                    tbSoldTo.Text = dr["sold_to"].ToString();
                                    tbSoldTo.ReadOnly = true;
                                    lbSoldTo.Text = dr["sold_to_name"].ToString();
                                    tbBillTo.Text = dr["bill_to"].ToString();
                                    tbBillTo.ReadOnly = true;
                                    lbBillTo.Text = dr["bill_to_name"].ToString();
                                    tbShipTo.Text = dr["ship_to"].ToString();
                                    tbShipTo.ReadOnly = true;
                                    lbShipTo.Text = dr["ship_to_name"].ToString();
                                    ibSearchQuot.Visible = false;
                                    tbPaymentTerm.Text = dr["payment_term"].ToString();
                                    tbPaymentTerm.ReadOnly = true;
                                    lbPaymentTerm.Text = dr["payment_term_desc"].ToString();
                                    ddlInstallationBy.SelectedValue = dr["installation_by"].ToString();
                                    if (ddlInstallationBy.SelectedValue == "S01")
                                    {
                                        ddlInstallationBySite.Visible = true;
                                        ddlInstallationBySite.SelectedValue = dr["installation_by_site"].ToString();
                                    }
                                    else
                                    {
                                        ddlInstallationBySite.Visible = false;
                                    }
                                    tbInstallationCharge.Text = double.Parse(dr["installation_charge"].ToString()).ToString("##,#0");
                                    tbAmt.Text = double.Parse(dr["amount"].ToString()).ToString("##,#0.#0");
                                    tbDisc.Text = double.Parse(dr["disc_amount"].ToString()).ToString("##,#0.#0");
                                    tbTax.Text = double.Parse(dr["tax_amount"].ToString()).ToString("##,#0.#0");
                                    tbNetValue.Text = double.Parse(dr["total_amount"].ToString()).ToString("##,#0.#0");
                                    tbCustPONo.Text = dr["cust_po_no"].ToString();
                                    tbCustPODate.Text = DateTime.Parse(dr["cust_po_date"].ToString()).ToString("dd-MM-yyyy");
                                    tbInstallationDate.Text = DateTime.Parse(dr["installation_date"].ToString()).ToString("dd-MM-yyyy");
                                    tbNote.Text = dr["note"].ToString();
                                }
                            }
                        }
                    }

                    sql.Length = 0;
                    sql.Append("SELECT a.line_no, b.article_type, a.article_no, b.article_description,  ");
                    sql.Append("a.qty, 'MON' time_unit_id, 'MONTHLY' AS time_unit_desc, ");
                    sql.Append("a.rate, a.time_duration, a.source_id, ISNULL(e.description, '') AS source_desc, a.wh_id, ISNULL(sw.wh_description, v.vendor_name) AS wh_name, ");
                    sql.Append("a.rate, a.disc, a.tax, reff_line_no ");
                    sql.Append("FROM rental_order_detail a WITH(READPAST) ");
                    sql.Append("INNER JOIN article b WITH(READPAST) ON a.article_no = b.article_no ");
                    sql.Append("LEFT JOIN source e WITH(READPAST) ON a.source_id = e.id ");
                    sql.Append("LEFT JOIN site_wh sw WITH(READPAST) on sw.wh_id = a.wh_id ");
                    sql.Append("LEFT JOIN vendor v WITH(READPAST) on v.vendor_no = a.wh_id ");
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
                                        int.Parse(dr["qty"].ToString()), dr["time_unit_id"].ToString(), dr["time_unit_desc"].ToString(),
                                        double.Parse(dr["rate"].ToString()), double.Parse(dr["disc"].ToString()), double.Parse(dr["tax"].ToString()), int.Parse(dr["time_duration"].ToString()),
                                        dr["source_id"].ToString(), dr["source_desc"].ToString(), dr["wh_id"].ToString(), dr["wh_name"].ToString(),
                                        int.Parse(dr["qty"].ToString()) * (double.Parse(dr["rate"].ToString()) /*- double.Parse(dr["disc"].ToString())*/) * int.Parse(dr["time_duration"].ToString()),
                                        int.Parse(dr["reff_line_no"].ToString()), DateTime.Now, (DataTable)Session["dt"]);
                                    }
                                    loadGrid();
                                    upGrid.Update();
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
            }
        }
    }

    protected void showConfirmBox(string message)
    {
        wucConfirmBox1.SubShowConfirmBox(message);
    }

    protected void ibSearchQuot_Click(object sender, EventArgs e)
    {
        Session["search"] = "quot_rent";
        wucSearch1.loadGrid();
    }

    protected void bSearchQuot_Click(object sender, EventArgs e)
    {
        // MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT DISTINCT qr.trans_date, qr.sold_to, c.customer_name, qr.bill_to, (CASE WHEN cab.alias_name = '' THEN cab.alias_name_full ELSE cab.alias_name_full + ' ' + cab.alias_name END) AS bill_to_name, ");
            sql.Append("qr.ship_to, (CASE WHEN cas.alias_name = '' THEN cas.alias_name_full ELSE cas.alias_name_full + ' ' + cab.alias_name END) AS ship_to_name, ");
            sql.Append("qr.currency_id, qr.payment_term, pt.description AS payment_term_desc, qr.delivery_date, qr.installation_charge, qr.amount, qr.tax, qr.total_amount, qr.note ");
            sql.Append("FROM quotation_rental qr WITH(READPAST) ");
            sql.Append("INNER JOIN quotation_rental_detail qrd WITH(READPAST) ON qr.trans_no = qrd.trans_no ");
            sql.Append("INNER JOIN payment_term pt WITH(READPAST) ON(qr.payment_term = pt.id) ");
            sql.Append("INNER JOIN customer c WITH(READPAST) ON qr.sold_to = c.customer_no ");
            sql.Append("INNER JOIN customer_address cab WITH(READPAST) ON qr.sold_to = cab.customer_no AND cab.address_type = '01' AND qr.bill_to = cab.id ");
            sql.Append("INNER JOIN customer_address cas WITH(READPAST) ON qr.sold_to = cas.customer_no AND cas.address_type = '02' AND qr.ship_to = cas.id ");
            sql.Append("LEFT JOIN( ");
            sql.Append("    SELECT ro.quot_trans_no, rod.reff_line_no, rod.article_no, SUM(rod.qty) AS qty ");
            sql.Append("    FROM rental_order ro WITH(READPAST) ");
            sql.Append("    INNER JOIN rental_order_detail rod WITH(READPAST) ON ro.trans_no = rod.trans_no ");
            sql.Append("    WHERE ro.quot_trans_no = '" + tbSearchQuot.Text.Trim().Replace("'", "`").ToUpper() + "' ");
            sql.Append("    GROUP BY ro.quot_trans_no, rod.reff_line_no, rod.article_no ");
            sql.Append(") AS ro ON qrd.trans_no = ro.quot_trans_no AND qrd.line_no = ro.reff_line_no ");
            sql.Append("WHERE 1 = 1 ");
            sql.Append("AND qr.status = '1' ");
            sql.Append("AND qr.trans_no = '" + tbSearchQuot.Text.Trim().Replace("'", "`").ToUpper() + "' ");
            sql.Append("AND qrd.qty - ISNULL(ro.qty, 0) > 0 ");

            using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
            {
                conn.Open();
                using (cmd = new SqlCommand(sql.ToString(), conn))
                {
                    using (dr = cmd.ExecuteReader())
                    {
                        DataTable dtaDetail = (DataTable)Session["dt"];
                        dtaDetail.Rows.Clear();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            lbSearchQuot.Text = DateTime.Parse(dr["trans_date"].ToString()).ToString("dd-MM-yyyy");
                            tbSoldTo.Text = dr["sold_to"].ToString();
                            lbSoldTo.Text = dr["customer_name"].ToString();
                            tbBillTo.Text = dr["bill_to"].ToString();
                            lbBillTo.Text = dr["bill_to_name"].ToString();
                            tbShipTo.Text = dr["ship_to"].ToString();
                            lbShipTo.Text = dr["ship_to_name"].ToString();
                            tbAmt.Text = double.Parse(dr["amount"].ToString()).ToString("##,#0.#0");
                            tbTax.Text = double.Parse(dr["tax"].ToString()).ToString("##,#0.#0");
                            tbCurrency.Text = dr["currency_id"].ToString();
                            tbPaymentTerm.Text = dr["payment_term"].ToString();
                            lbPaymentTerm.Text = dr["payment_term_desc"].ToString();
                            tbNetValue.Text = double.Parse(dr["total_amount"].ToString()).ToString("##,#0.#0");
                            tbInstallationDate.Text = DateTime.Parse(dr["delivery_date"].ToString()).ToString("dd-MM-yyyy");
                            tbNote.Text = dr["note"].ToString();

                            ibSearchQuot.Visible = false;
                            lbSearchQuot.Visible = true;
                            upHeader.Update();
                        }
                        else
                        {
                            MasterPage master = (MasterPage)this.Master;
                            master.messageBox("Invalid rental quotation number <br /> Or Quotation already closed");
                            return;
                        }
                    }
                }
            }

            sql.Length = 0;
            sql.Append("SELECT qrd.line_no, qrd.article_no, a.article_type, a.article_description,  ");
            sql.Append("qrd.qty AS qty, (qrd.qty - ISNULL(ro.qty, 0)) AS qty_outstanding, 'MON' AS unit_id, 'MONTHLY' AS unit_desc,  ");
            sql.Append("qrd.rent_duration as time_duration, qrd.rate, qrd.disc, qrd.tax,  ");
            sql.Append("'', '', ''  ");
            sql.Append("FROM quotation_rental_detail qrd WITH(READPAST)  ");
            sql.Append("INNER JOIN article a WITH(READPAST) ON qrd.article_no = a.article_no  ");
            sql.Append("LEFT JOIN(  ");
            sql.Append("    SELECT ro.quot_trans_no, rod.reff_line_no, rod.article_no, SUM(rod.qty) AS qty  ");
            sql.Append("    FROM rental_order ro WITH(READPAST)  ");
            sql.Append("    INNER JOIN rental_order_detail rod WITH(READPAST) ON ro.trans_no = rod.trans_no  ");
            sql.Append("    WHERE ro.trans_no = '" + tbSearchQuot.Text.Trim().ToUpper() + "' ");
            sql.Append("    GROUP BY ro.quot_trans_no, rod.reff_line_no, rod.article_no  ");
            sql.Append(") AS ro ON qrd.trans_no = ro.quot_trans_no AND qrd.line_no = ro.reff_line_no  ");
            sql.Append("WHERE 1 = 1  ");
            sql.Append("AND qrd.trans_no = '" + tbSearchQuot.Text.Trim().ToUpper() + "' ");

            using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
            {
                conn.Open();
                using (cmd = new SqlCommand(sql.ToString(), conn))
                {
                    using (dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            string sSourceID = "";
                            string sSourceDesc = "";
                            string slocation = "";
                            string slocationName = "";
                            string sSerialNo = "";

                            while (dr.Read())
                            {
                                sSourceID = "";
                                sSourceDesc = "";
                                slocation = "";
                                slocationName = "";
                                sSerialNo = "";
                                addDataToTable(dr["article_no"].ToString(), dr["article_type"].ToString(),
                                    dr["article_description"].ToString(), double.Parse(dr["qty"].ToString()), dr["unit_id"].ToString(), dr["unit_desc"].ToString(),
                                    double.Parse(dr["rate"].ToString()), double.Parse(dr["disc"].ToString()), double.Parse(dr["tax"].ToString()),
                                    double.Parse(dr["time_duration"].ToString()), sSourceID, sSourceDesc, slocation, slocationName.ToString(),
                                    double.Parse(dr["qty"].ToString()) * (double.Parse(dr["rate"].ToString())) * double.Parse(dr["time_duration"].ToString()),
                                    int.Parse(dr["line_no"].ToString()), DateTime.Now, (DataTable)(Session["dt"]));
                            }
                            loadGrid();
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

    protected void lbSearchQuot_Click(object sender, EventArgs e)
    {
        lbSearchQuot.Text = "";
        tbSearchQuot.Text = "";
        lbSearchQuot.Visible = false;
        tbSearchQuot.ReadOnly = false;
        ibSearchQuot.Visible = true;
        tbSoldTo.Text = "";
        tbInstallationDate.Text = "";
        tbInstallationDate.Enabled = true;
        tbPaymentTerm.Text = "";
        tbPaymentTerm.ReadOnly = true;
        tbBillTo.Text = "";
        tbShipTo.Text = "";
        tbCustPONo.Text = "";
        tbCustPODate.Text = "";
        lbSearchQuot.Text = "";
        lbSoldTo.Text = "";
        lbBillTo.Text = "";
        lbShipTo.Text = "";
        lbPaymentTerm.Visible = false;
        tbAmt.Text = "0.00";
        tbTax.Text = "0.00";
        tbNetValue.Text = "0.00";
        tbNote.Text = "";
        upHeader.Update();

        grid.DataSource = null;
        grid.DataBind();
        upGrid.Update();
    }

    protected void loadDdlInstallationBy()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT id AS value, UPPER(description) AS text ");
            sql.Append("FROM source WITH(READPAST) ");
            sql.Append("WHERE source_type = 'Service' ");
            sql.Append("ORDER BY text ");

            cAdih.loadDdl(sql.ToString(), cAdih.getConnStr("Connection"), ddlInstallationBy);
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void ddlInstallationBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlInstallationBy.SelectedValue == "S01")
        {
            ddlInstallationBySite.Visible = true;
        }
        else
        {
            ddlInstallationBySite.Visible = false;
        }
        upHeader.Update();
    }

    protected void loadDdlInstallationBySite()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT wh_id AS value, UPPER(wh_description) AS text ");
            sql.Append("FROM site_wh WITH(READPAST) ");
            sql.Append("ORDER BY text ");

            cAdih.loadDdl(sql.ToString(), cAdih.getConnStr("Connection"), ddlInstallationBySite);
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void setWHVisibility(TextBox paramTbSource, Panel paramPSearchWH, ImageButton paramIbSearchWH, TextBox paramTbWHID, LinkButton paramLbWHName)
    {
        paramTbWHID.Text = "";
        paramLbWHName.Text = "";
        if (paramTbSource.Text.ToUpper().Trim().Replace("'", "`") == "I01")
        {
            paramPSearchWH.Visible = true;
            paramIbSearchWH.Visible = true;
            paramTbWHID.Visible = true;
            paramLbWHName.Visible = true;
        }
        else
        {
            paramPSearchWH.Visible = false;
            paramIbSearchWH.Visible = false;
            paramTbWHID.Visible = false;
            paramLbWHName.Visible = false;
        }
    }

    protected void ibSearchSourceInstall_Click(object sender, EventArgs e)
    {
        Session["search"] = "source_ro";
        Session["searchParamSourceType"] = cAdih.getSourceType("ZSER");
        wucSearch1.loadGrid();
    }

    protected void ibSearchSource_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            GridViewRow gridRow = ((sender as ImageButton).Parent.Parent.Parent as GridViewRow);
            Label lArticleType = (Label)gridRow.FindControl("lArticleType");
            Session["search"] = "source_ro";
            Session["searchParamSourceType"] = cAdih.getSourceType(lArticleType.Text);
            Session["searchGridRowIndex"] = gridRow.RowIndex.ToString();
            wucSearch1.loadGrid();
        }
        catch (Exception ex)
        {
        }
    }

    protected void searchSourceResult(ImageButton paramIbSearchSource, TextBox paramTbSource, LinkButton paramLbSourceDesc, Panel paramPSearchWH)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT description FROM source WITH(READPAST) ");
            sql.Append("WHERE id = '" + paramTbSource.Text.ToUpper().Trim().Replace("'", "`") + "' ");

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
                            paramTbSource.ReadOnly = true;
                            paramTbSource.Visible = false;
                            paramLbSourceDesc.Text = dr["description"].ToString();
                            paramIbSearchSource.Visible = false;
                            paramPSearchWH.Visible = true;
                        }
                        else
                        {
                            paramTbSource.ReadOnly = false;
                            paramTbSource.Visible = true;
                            paramLbSourceDesc.Text = "";
                            paramIbSearchSource.Visible = true;
                            paramPSearchWH.Visible = false;
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

    protected void lbSourceDesc_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent.Parent as GridViewRow);
        Label lLineNo = (Label)gridRow.FindControl("lLineNo");
        ImageButton ibSearchSource = (ImageButton)gridRow.FindControl("ibSearchSource");
        ImageButton ibAssetList = (ImageButton)gridRow.FindControl("ibAssetList");
        TextBox tbSourceID = (TextBox)gridRow.FindControl("tbSourceID");
        LinkButton lbSourceDesc = (LinkButton)gridRow.FindControl("lbSourceDesc");
        Panel pSearchWH = (Panel)gridRow.FindControl("pSearchWH");
        TextBox tbWHID = (TextBox)gridRow.FindControl("tbWHID");
        LinkButton lbWHDesc = (LinkButton)gridRow.FindControl("lbWHDesc");

        tbSourceID.Text = "";
        tbSourceID.Visible = true;
        tbSourceID.ReadOnly = false;
        lbSourceDesc.Text = "";
        ibSearchSource.Visible = true;
        ibAssetList.Visible = false;

        pSearchWH.Visible = false;
        lbWHDesc_Click(sender, e);

        upGrid.Update();
    }

    protected void ibSearchWH_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            GridViewRow gridRow = ((sender as ImageButton).Parent.Parent.Parent as GridViewRow);
            Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
            TextBox tbSourceID = (TextBox)gridRow.FindControl("tbSourceID");
            LinkButton lbSourceDesc = (LinkButton)gridRow.FindControl("lbSourceDesc");

            if (lbSourceDesc.Text == "")
            {
                master.messageBox("Input source!");
                return;
            }
            else
            {
                Session["search"] = "all_warehouse";
                Session["searchParamArticleNo"] = lArticleNo.Text;
                Session["searchGridRowIndex"] = gridRow.RowIndex.ToString();
                wucSearch1.loadGrid();
            }
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void searchWHResult(TextBox paramTbSourceID, ImageButton paramIbSearchWH, TextBox paramTbWHID, LinkButton paramLbWHDesc)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT site_name FROM( ");
            sql.Append("    SELECT wh_id as site_id, wh_description as site_name FROM site_wh WITH(READPAST) ");
            sql.Append(") AS tbl ");
            sql.Append("WHERE tbl.site_id = '" + paramTbWHID.Text.ToUpper().Trim().Replace("'", "`") + "' ");

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
                            paramLbWHDesc.Text = dr["site_name"].ToString();
                            paramTbWHID.Visible = false;
                            paramTbWHID.ReadOnly = true;
                            paramIbSearchWH.Visible = false;
                        }
                        else
                        {
                            paramLbWHDesc.Text = "";
                            paramTbWHID.Visible = true;
                            paramTbWHID.ReadOnly = false;
                            paramIbSearchWH.Visible = true;
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

    protected void lbWHDesc_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gridRow = ((sender as LinkButton).Parent.Parent.Parent as GridViewRow);
            Label lLineNo = (Label)gridRow.FindControl("lLineNo");
            ImageButton ibSearchWH = (ImageButton)gridRow.FindControl("ibSearchWH");
            TextBox tbWHID = (TextBox)gridRow.FindControl("tbWHID");
            LinkButton lbWHDesc = (LinkButton)gridRow.FindControl("lbWHDesc");

            ibSearchWH.Visible = true;
            tbWHID.Text = "";
            tbWHID.Visible = true;
            tbWHID.ReadOnly = false;
            lbWHDesc.Text = "";
            upGrid.Update();
        }
        catch (Exception ex)
        {
        }
    }

    protected void lbGridWHName_Click(object sender, EventArgs e)
    {
        GridViewRow gridRow = ((sender as LinkButton).Parent.Parent.Parent as GridViewRow);
        ImageButton ibGridSearchWH = (ImageButton)gridRow.FindControl("ibWH");
        TextBox tbGridWHID = (TextBox)gridRow.FindControl("tbWHID");
        LinkButton lbGridWHName = (LinkButton)gridRow.FindControl("lbWH");

        clearWH(ibGridSearchWH, tbGridWHID, lbGridWHName);
    }

    protected void clearWH(ImageButton paramIbSearchWH, TextBox paramTbWHID, LinkButton paramLbWHName)
    {
        paramIbSearchWH.Visible = true;
        paramTbWHID.Text = "";
        paramTbWHID.Visible = true;
        paramTbWHID.ReadOnly = false;
        paramLbWHName.Text = "";
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
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "timeUnitID";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "timeUnitDesc";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "rate";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "disc";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "tax";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "timeDuration";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "sourceID";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "sourceDesc";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "WHID";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.String");
            dtColumn.ColumnName = "WHName";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Double");
            dtColumn.ColumnName = "totalAmount";
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = Type.GetType("System.Int64");
            dtColumn.ColumnName = "reffLineNo";
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

    private void addDataToTable(string articleNo, string articleType, string articleDesc,
        double qty, string timeUnitID, string timeUnitDesc, double rate, double disc, double tax, double timeDuration,
        string sourceID, string sourceDesc, string whID, string whName, Double totalAmount, int reffLineNo,
        DateTime addedDate, DataTable Table)
    {
        try
        {
            DataRow dtRow;

            dtRow = Table.NewRow();

            string maxLineNo = ((DataTable)Session["dt"]).Compute("MAX(lineNo)", "").ToString();

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
            dtRow["timeUnitID"] = timeUnitID;
            dtRow["timeUnitDesc"] = timeUnitDesc;
            dtRow["rate"] = rate;
            dtRow["disc"] = disc;
            dtRow["tax"] = tax;
            dtRow["timeDuration"] = timeDuration;
            dtRow["sourceID"] = sourceID;
            dtRow["sourceDesc"] = sourceDesc;
            dtRow["whID"] = whID;
            dtRow["whName"] = whName;
            dtRow["totalAmount"] = totalAmount;
            dtRow["qty"] = qty;
            dtRow["reffLineNo"] = reffLineNo;
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

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lArticleType = (Label)Row.FindControl("lArticleType");
                TextBox tbSourceID = (TextBox)Row.FindControl("tbSourceID");
                LinkButton lbSourceDesc = (LinkButton)Row.FindControl("lbSourceDesc");
                TextBox tbWHID = (TextBox)Row.FindControl("tbWHID");
                ImageButton ibSearchSource = (ImageButton)Row.FindControl("ibSearchSource");
                LinkButton lbWHDesc = (LinkButton)Row.FindControl("lbWHDesc");
                ImageButton ibSearchWH = (ImageButton)Row.FindControl("ibSearchWH");
                Panel pSearchWH = (Panel)Row.FindControl("pSearchWH");

                if (tbSourceID.Text.Trim().Length > 0)
                {
                    tbSourceID.ReadOnly = true;
                    tbSourceID.Visible = false;
                    ibSearchSource.Visible = false;
                }

                if (tbWHID.Text.Trim().Length > 0)
                {
                    tbWHID.ReadOnly = true;
                    tbWHID.Visible = false;
                    ibSearchWH.Visible = false;
                }

                pSearchWH.Visible = true;
            }
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
            grid.DataSource = ((DataTable)Session["dt"]).DefaultView;
            grid.DataBind();

            if (grid.Rows.Count >= 1)
            {
                calculateNetValue();
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

    protected void calculateNetValue()
    {
        try
        {
            double netRate = 0;
            double netDisc = 0;
            double netTax = 0;
            double netValue = 0;
            foreach (GridViewRow gridRow in grid.Rows)
            {
                Label lQty = (Label)gridRow.FindControl("lQty");
                Label lRate = (Label)gridRow.FindControl("lRate");
                Label lDisc = (Label)gridRow.FindControl("lDisc");
                Label lTax = (Label)gridRow.FindControl("lTax");
                Label lTimeDuration = (Label)gridRow.FindControl("lTimeDuration");
                Label lbAmount = (Label)gridRow.FindControl("lbAmount");
                Label lArticleType = (Label)gridRow.FindControl("lArticleType");
                Label lSourceID = (Label)gridRow.FindControl("lSourceID");
                TextBox tbWHID = (TextBox)gridRow.FindControl("tbWHID");
                ImageButton ibWH = (ImageButton)gridRow.FindControl("ibWH");
                LinkButton lbWH = (LinkButton)gridRow.FindControl("lbWH");
                netRate += (double.Parse(lQty.Text) * double.Parse(lRate.Text) * double.Parse(lTimeDuration.Text));
                netDisc += double.Parse(lDisc.Text); //(double.Parse(lQty.Text) * double.Parse(lDisc.Text) * double.Parse(lTimeDuration.Text));
                netTax += (double.Parse(lQty.Text) * double.Parse(lTax.Text) * double.Parse(lTimeDuration.Text));
            }
            tbAmt.Text = netRate.ToString("##,#0.#0");
            tbDisc.Text = netDisc.ToString("##,#0.#0");
            tbTax.Text = netTax.ToString("##,#0.#0");
            netValue = (netRate /*- netDisc*/ + netTax) + double.Parse(tbInstallationCharge.Text);
            tbNetValue.Text = netValue.ToString("##,#0.#0");
            upHeader.Update();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void imgDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string strItemID = "";
            string strLine = "";
            GridViewRow gridRow = ((sender as ImageButton).Parent.Parent as GridViewRow);
            Label lLineNo = (Label)gridRow.FindControl("lLineNo");
            Label lQID = (Label)gridRow.FindControl("lQID");
            Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
            strItemID = lQID.Text;
            strLine = lLineNo.Text;
            //refresTable();
            deleteDetail(strItemID, strLine);
            //sql.Length = 0;
            //sql.Append("UPDATE rental_order_detail SET inlist=0 WHERE ref_quotation='" + lQID.Text + "' ;");

            //cAdih.executeNonQuery(sql.ToString(),cAdih.getConnStr("Connection"));
            loadGrid();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void deleteDetail(string itemID, string lineNo)
    {
        try
        {
            if (itemID.Trim() != "")
            {
                DataRow[] dtRow = ((DataTable)Session["dt"]).Select("structureArticleNo='" + itemID + "'");

                if (dtRow.Length >= 1)
                {
                    for (int i = 0; i < dtRow.Length; i++)
                    {
                        ((DataTable)Session["dt"]).Rows.Remove(dtRow[i]);
                    }
                }
            }
            else
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
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void ibSearchRentalUOM_Click(object sender, EventArgs e)
    {
        //if (txtProductName.Text == "")
        //{
        //    MasterPage master = (MasterPage)this.Master;
        //    master.messageBox("Input Stock Code First!");
        //    tbArticleNo.Focus();
        //    return;
        //}
        //else
        //{
        //    Session["search"] = "uomByStockCode";
        //    Session["searchMainStockCode"] = tbArticleNo.Text;
        //    wucSearch1.loadGrid();
        //}
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;

        if (tbSearchQuot.Text.Trim() == "")
        {
            master.messageBox("Input quotation reference!");
            return;
        }
        else if (tbCustPONo.Text.Trim().Replace("'", "`") == "")
        {
            master.messageBox("Input Cust PO No!");
            return;
        }
        else if (tbCustPODate.Text.Trim().Replace("'", "`") == "")
        {
            master.messageBox("Input Cust PO Date!");
            return;
        }
        else if (tbInstallationDate.Text.Trim().Replace("'", "`") == "")
        {
            master.messageBox("Input Installation Date!");
            return;
        }
        else if (tbNote.Text.Trim().Replace("'", "`") == "")
        {
            master.messageBox("Input Note!");
            return;
        }
        else if (double.Parse((tbNetValue.Text == "" ? "0" : tbNetValue.Text)) <= 0)
        {
            master.messageBox("Input Detail Order!");
            return;
        }

        foreach (GridViewRow gridRow in grid.Rows)
        {
            Label lLineNo = (Label)gridRow.FindControl("lLineNo");
            Label lQty = (Label)gridRow.FindControl("lQty");
            Label lArticleDesc = (Label)gridRow.FindControl("lArticleDesc");
            LinkButton lbSourceDesc = (LinkButton)gridRow.FindControl("lbSourceDesc");
            LinkButton lbWHDesc = (LinkButton)gridRow.FindControl("lbWHDesc");
            TextBox tbSerialNo = (TextBox)gridRow.FindControl("tbSerialNo");

            if (lbSourceDesc.Text.Trim() == "")
            {
                master.messageBox("Input source on article " + lArticleDesc.Text + "");
                return;
            }

            if (lbSourceDesc.Text.Trim() == "Stock" || lbSourceDesc.Text.Trim() == "Asset")
            {
                if (lbWHDesc.Text.Trim() == "")
                {
                    master.messageBox("Input warehouse on article " + lArticleDesc.Text + "");
                    return;
                }
            }

            if (lbSourceDesc.Text.Trim() == "Vendor to Site")
            {
                if (lbWHDesc.Text.Trim() == "")
                {
                    master.messageBox("Input warehouse on article " + lArticleDesc.Text + "");
                    return;
                }
            }

            if (DateTime.Parse(cAdih.engFormatDate(tbCustPODate.Text)).AddDays(45) < DateTime.Parse(cAdih.engFormatDate(tbInstallationDate.Text)))
            {
                master.messageBox("Maximum Req Delv Date 45 days is greater than cust PO date. ");
                return;
            }
        }

        if (tbTransNo.Text == "")
        {
            sql.Length = 0;
            sql.Append("SELECT trans_no AS result FROM rental_order WITH(READPAST) ");
            sql.Append("WHERE sold_to = '" + tbSoldTo.Text.Trim().Replace("'", "`").ToUpper() + "' ");
            sql.Append("AND cust_po_no = '" + tbCustPONo.Text.Trim().Replace("'", "`").ToUpper() + "' ");

            string exists = cAdih.getResultString(sql.ToString(), cAdih.getConnStr("Connection"));

            if (exists != "")
            {
                master.messageBox("PO Cust No. " + tbCustPONo.Text.Trim().Replace("'", "`").ToUpper() + " already transacted with Rental Order No. " + exists + "!");
                return;
            }
        }

        Session["RentalOrderMode"] = "Save";
        showConfirmBox("Save Data?");
    }

    protected void saveData()
    {
        MasterPage master = (MasterPage)this.Master;
        string sFileName = "";

        try
        {
            Session["ConfirmMode"] = "OK";

            if (Session["ConfirmMode"].Equals("OK"))
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
                    sql.Append("FROM rental_order WITH(READPAST) ");
                    sql.Append("WHERE  YEAR(trans_date) = YEAR(CONVERT(VARCHAR(10), GETDATE(), 120)) ");
                    sql.Append("AND MONTH(trans_date) = MONTH(CONVERT(VARCHAR(10), GETDATE(), 120)) ");

                    using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                    {
                        using (dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                tbTransNo.Text = "RO" + dr["tahun"].ToString() + dr["bulan"].ToString() + dr["running_no"].ToString();
                                upHeader.Update();
                            }
                        }
                    }

                    sql.Length = 0;
                    sql.Append("INSERT INTO [dbo].[rental_order] ");
                    sql.Append("([trans_no] ");
                    sql.Append(",[trans_date] ");
                    sql.Append(",[sold_to] ");
                    sql.Append(",[bill_to] ");
                    sql.Append(",[ship_to] ");
                    sql.Append(",[cust_po_no] ");
                    sql.Append(",[cust_po_date] ");
                    sql.Append(",[quot_trans_no] ");
                    sql.Append(",[installation_date] ");
                    sql.Append(",[installation_by] ");
                    sql.Append(",[installation_by_site] ");
                    sql.Append(",[currency_id] ");
                    sql.Append(",[payment_term] ");
                    sql.Append(",[installation_charge] ");
                    sql.Append(",[total_amount] ");
                    sql.Append(",[tax_amount] ");
                    sql.Append(",[disc_amount] ");
                    sql.Append(",[amount] ");
                    sql.Append(",[note] ");
                    sql.Append(",[status] ");
                    sql.Append(",[created_by] ");
                    sql.Append(",[created_date] ");
                    sql.Append(",[last_modified_by] ");
                    sql.Append(",[last_modified_date] ");
                    sql.Append(",[posted_by] ");
                    sql.Append(",[posted_date]) ");
                    sql.Append("VALUES ");
                    sql.Append("( ");
                    sql.Append("'" + tbTransNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("CONVERT(VARCHAR(10), GETDATE(), 120), ");
                    sql.Append("'" + tbSoldTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + tbBillTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + tbShipTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + tbCustPONo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + cAdih.engFormatDate(tbCustPODate.Text) + "', ");
                    sql.Append("'" + tbSearchQuot.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + cAdih.engFormatDate(tbInstallationDate.Text) + "', ");
                    sql.Append("'" + ddlInstallationBy.SelectedValue + "', ");
                    if (ddlInstallationBy.SelectedValue == "S01")
                    {
                        sql.Append("'" + ddlInstallationBySite.SelectedValue + "', ");
                    }
                    else
                    {
                        sql.Append("'', ");
                    }
                    sql.Append("'" + tbCurrency.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + tbPaymentTerm.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + double.Parse(tbInstallationCharge.Text) + "', ");
                    sql.Append("'" + double.Parse(tbNetValue.Text) + "', ");
                    sql.Append("'" + double.Parse(tbTax.Text) + "', ");
                    sql.Append("'" + double.Parse(tbDisc.Text) + "', ");
                    sql.Append("'" + double.Parse(tbAmt.Text) + "', ");
                    sql.Append("'" + tbNote.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'0', ");
                    sql.Append("'" + Session["username"].ToString() + "', ");
                    sql.Append("GETDATE(),'','','','') ");

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
                        Label lQty = (Label)gridRow.FindControl("lQty");
                        Label lRate = (Label)gridRow.FindControl("lRate");
                        Label lDisc = (Label)gridRow.FindControl("lDisc");
                        Label lTax = (Label)gridRow.FindControl("lTax");
                        Label lAmount = (Label)gridRow.FindControl("lAmount");
                        Label lTimeDuration = (Label)gridRow.FindControl("lTimeDuration");
                        TextBox tbSourceID = (TextBox)gridRow.FindControl("tbSourceID");
                        TextBox tbWHID = (TextBox)gridRow.FindControl("tbWHID");
                        Label lReffLineNo = (Label)gridRow.FindControl("lReffLineNo");

                        line_no += 10;

                        sql.Length = 0;
                        sql.Append("INSERT INTO [dbo].[rental_order_detail] ");
                        sql.Append("([trans_no] ");
                        sql.Append(",[line_no] ");
                        sql.Append(",[article_no] ");
                        sql.Append(",[qty] ");
                        sql.Append(",[rate] ");
                        sql.Append(",[tax] ");
                        sql.Append(",[disc] ");
                        sql.Append(",[time_duration] ");
                        sql.Append(",[total_amount] ");
                        sql.Append(",[source_id] ");
                        sql.Append(",[wh_id] ");
                        sql.Append(",[reff_line_no]) ");
                        sql.Append("VALUES ");
                        sql.Append("( ");
                        sql.Append("'" + tbTransNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + line_no + "', ");
                        sql.Append("'" + lArticleNo.Text + "', ");
                        sql.Append("'" + double.Parse(lQty.Text) + "', ");
                        sql.Append("'" + double.Parse(lRate.Text) + "', ");
                        sql.Append("'" + double.Parse(lTax.Text) + "', ");
                        sql.Append("'" + double.Parse(lDisc.Text) + "', ");
                        sql.Append("'" + int.Parse(lTimeDuration.Text) + "', ");
                        sql.Append("'" + double.Parse(lAmount.Text) + "', ");
                        sql.Append("'" + tbSourceID.Text.Trim() + "', ");
                        sql.Append("'" + tbWHID.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + lReffLineNo.Text + "') ");

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
                    sql.Append("UPDATE rental_order SET sold_to = '" + tbSoldTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("bill_to = '" + tbBillTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("ship_to = '" + tbShipTo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("cust_po_no = '" + tbCustPONo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("cust_po_date = '" + cAdih.engFormatDate(tbCustPODate.Text) + "', ");
                    sql.Append("installation_date = '" + cAdih.engFormatDate(tbInstallationDate.Text) + "', ");
                    sql.Append("currency_id = '" + tbCurrency.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    //sql.Append("billing_methode = '" + ddlBillingMethod.SelectedValue.Trim().Replace("'", "`") + "', ");
                    sql.Append("payment_term = '" + tbPaymentTerm.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("installation_charge = '" + double.Parse(tbInstallationCharge.Text) + "', ");
                    sql.Append("amount = '" + double.Parse(tbAmt.Text) + "', ");
                    sql.Append("disc_amount = '" + double.Parse(tbDisc.Text) + "', ");
                    sql.Append("tax_amount = '" + double.Parse(tbTax.Text) + "', ");
                    sql.Append("total_amount = '" + double.Parse(tbNetValue.Text) + "', ");
                    sql.Append("note = '" + tbNote.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("last_modified_by = '" + Session["username"].ToString() + "', ");
                    sql.Append("last_modified_date = GETDATE(),quot_trans_no='" + tbSearchQuot.Text.Trim() + "', ");
                    sql.Append("installation_by = '" + ddlInstallationBy.SelectedValue + "', ");
                    if(ddlInstallationBy.SelectedValue == "S01")
                    {
                        sql.Append("installation_by_site = '" + ddlInstallationBySite.SelectedValue + "' ");
                    }
                    else{
                        sql.Append("installation_by_site = '' ");
                    }
                    sql.Append("WHERE trans_no = '" + tbTransNo.Text + "' ");

                    using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                    {
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }

                    sql.Length = 0;
                    sql.Append("DELETE rental_order_detail WHERE trans_no = '" + tbTransNo.Text + "' ");
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
                        Label lQty = (Label)gridRow.FindControl("lQty");
                        Label lRate = (Label)gridRow.FindControl("lRate");
                        Label lDisc = (Label)gridRow.FindControl("lDisc");
                        Label lTax = (Label)gridRow.FindControl("lTax");
                        Label lAmount = (Label)gridRow.FindControl("lAmount");
                        Label lTimeDuration = (Label)gridRow.FindControl("lTimeDuration");
                        TextBox tbSourceID = (TextBox)gridRow.FindControl("tbSourceID");
                        TextBox tbWHID = (TextBox)gridRow.FindControl("tbWHID");
                        Label lReffLineNo = (Label)gridRow.FindControl("lReffLineNo");

                        line_no += 10;

                        sql.Length = 0;
                        sql.Append("INSERT INTO [dbo].[rental_order_detail] ");
                        sql.Append("([trans_no] ");
                        sql.Append(",[line_no] ");
                        sql.Append(",[article_no] ");
                        sql.Append(",[qty] ");
                        sql.Append(",[rate] ");
                        sql.Append(",[tax] ");
                        sql.Append(",[disc] ");
                        sql.Append(",[time_duration] ");
                        sql.Append(",[total_amount] ");
                        sql.Append(",[source_id] ");
                        sql.Append(",[wh_id] ");
                        sql.Append(",[reff_line_no]) ");
                        sql.Append("VALUES ");
                        sql.Append("( ");
                        sql.Append("'" + tbTransNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + line_no + "', ");
                        sql.Append("'" + lArticleNo.Text + "', ");
                        sql.Append("'" + double.Parse(lQty.Text) + "', ");
                        sql.Append("'" + double.Parse(lRate.Text) + "', ");
                        sql.Append("'" + double.Parse(lTax.Text) + "', ");
                        sql.Append("'" + double.Parse(lDisc.Text) + "', ");
                        sql.Append("'" + int.Parse(lTimeDuration.Text) + "', ");
                        sql.Append("'" + double.Parse(lAmount.Text) + "', ");
                        sql.Append("'" + tbSourceID.Text.Trim() + "', ");
                        sql.Append("'" + tbWHID.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                        sql.Append("'" + lReffLineNo.Text + "') ");

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
        Session["dt"] = null;
        Response.Redirect("~/Transaction/RentalOrder.aspx");
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        try
        {
            if (wucConfirmBox1.StrMode == "Yes")
            {
                if (Session["RentalOrderMode"].ToString() == "Save")
                {
                    saveData();
                }
                else if (Session["RentalOrderMode"].ToString() == "Posting")
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
        if (Session["search"].ToString() == "quot_rent")
        {
            tbSearchQuot.Text = wucSearch1.result;
            bSearchQuot_Click(sender, e);
        }
        else if (Session["search"].ToString() == "source_ro")
        {
            if (Session["searchParamSourceType"].ToString() == "Item")
            {
                TextBox tbSourceID = (TextBox)grid.Rows[int.Parse(Session["searchGridRowIndex"].ToString())].FindControl("tbSourceID");
                LinkButton lbSourceDesc = (LinkButton)grid.Rows[int.Parse(Session["searchGridRowIndex"].ToString())].FindControl("lbSourceDesc");
                ImageButton ibSearchSource = (ImageButton)grid.Rows[int.Parse(Session["searchGridRowIndex"].ToString())].FindControl("ibSearchSource");
                Panel pSearchWH = (Panel)grid.Rows[int.Parse(Session["searchGridRowIndex"].ToString())].FindControl("pSearchWH");
                tbSourceID.Text = wucSearch1.result;
                searchSourceResult(ibSearchSource, tbSourceID, lbSourceDesc, pSearchWH);
                upGrid.Update();
                Session["searchParamSourceType"] = null;
                Session["searchGridRowIndex"] = null;
                upGrid.Update();
            }
        }
        else if (Session["search"].ToString() == "all_warehouse")
        {
            TextBox tbSourceID = (TextBox)grid.Rows[int.Parse(Session["searchGridRowIndex"].ToString())].FindControl("tbSourceID");
            TextBox tbWHID = (TextBox)grid.Rows[int.Parse(Session["searchGridRowIndex"].ToString())].FindControl("tbWHID");
            ImageButton ibSearchWH = (ImageButton)grid.Rows[int.Parse(Session["searchGridRowIndex"].ToString())].FindControl("ibSearchWH");
            LinkButton lbWHDesc = (LinkButton)grid.Rows[int.Parse(Session["searchGridRowIndex"].ToString())].FindControl("lbWHDesc");
            tbWHID.Text = wucSearch1.result;
            searchWHResult(tbSourceID, ibSearchWH, tbWHID, lbWHDesc);
            upGrid.Update();
            Session["searchGridRowIndex"] = null;
            Session["searchParamArticleNo"] = null;
            Session["searchParamCompanyID"] = null;
        }
        Session["search"] = null;
    }

    protected void btnPosting_Click(object sender, EventArgs e)
    {
        Session["RentalOrderMode"] = "Posting";
        showConfirmBox("Posting Data?");
    }

    protected void ibWH_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gridRow = ((sender as ImageButton).Parent.Parent.Parent as GridViewRow);
            Label lArticleNo = (Label)gridRow.FindControl("lArticleNo");
            //Label lWHID = (Label)gridRow.FindControl("lWHID");
            //TextBox
            TextBox tbWHID = (TextBox)gridRow.FindControl("tbWHID");
            Session["search"] = "fixed_assets_serial_no_available";
            Session["searchParamTransNo"] = lArticleNo.Text;
            Session["searchParamSiteID"] = tbWHID.Text;
            Session["searchParamArticleNo"] = lArticleNo.Text;
            Session["searchGridRowIndex"] = gridRow.RowIndex.ToString();
            wucSearch1.loadGrid();
        }
        catch (Exception ex)
        {
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
            sql1.Append("EXEC sp_rental_order_create_pr_to_vendor '" + tbTransNo.Text + "','" + Session["username"].ToString() + "' ");
            sql1.Append(" ");
            sql1.Append("UPDATE rental_order SET status = '1', ");
            sql1.Append("posted_by = '" + Session["username"].ToString() + "', ");
            sql1.Append("posted_date = GETDATE() ");
            sql1.Append("WHERE trans_no = '" + tbTransNo.Text + "' ");

            using (cmd1 = new SqlCommand(sql1.ToString(), conn1, trans1))
            {
                cmd1.ExecuteNonQuery();
                cmd1 = null;
            }
            //updateImage();
            trans1.Commit();
            conn1.Close();
            master.messageBox("Data has been posted.");
            lStatus.Text = "POSTED";
            statusChanged(lStatus.Text);
        }
        catch (Exception ex)
        {
            master.messageBox("Fail To Execute" + "<br />" + ex.InnerException);
            trans1.Rollback();
            conn1.Close();
        }
    }
}