using System;
using System.Data.SqlClient;
using System.Text;

public partial class Transaction_RentalOrderPrint : System.Web.UI.Page
{
    private SqlConnection conn;
    private SqlCommand cmd;
    private SqlDataReader dr;
    private StringBuilder sql = new StringBuilder();
    private ClassAdih cAdih = new ClassAdih();

    protected void Page_Load(object sender, EventArgs e)
    {
        string FormTitle = "RENTAL ORDER";
        int RowMax = 20;
        string BUName = "";
        string BUAddress = "";
        string TransNo = "";
        string TransDate = "";
        string quotationNo = "";
        string rentTo = "";
        string billTo = "";
        string shipTo = "";
        string custPONo = "";
        string custPODate = "";
        string installationDate = "";
        string currency = "";
        string installationBy = "";
        string installationCharge = "";
        string note = "";
        string CreatedBy = "";
        string createdDate = "";
        string PostedBy = "";
        string postedDate = "";
        int i = 1;
        int j = 0;
        double sum_amount = 0;
        double sum_tax = 0;
        double sum_total_amount = 0;

        sql.Length = 0;
        sql.Append("SELECT COUNT(*) FROM rental_order_detail WITH(READPAST) WHERE trans_no = '" + Request.QueryString["transNo"].ToString() + "' ");

        conn = new SqlConnection(cAdih.getConnStr("Connection"));
        conn.Open();

        cmd = new SqlCommand(sql.ToString(), conn);
        int CountRow = int.Parse(cmd.ExecuteScalar().ToString());

        double PageTotal = Math.Ceiling(Convert.ToDouble(CountRow) / Convert.ToDouble(RowMax));

        string[] article = new string[CountRow];
        double[] qty = new double[CountRow];
        string[] timeUnit = new string[CountRow];
        double[] rate = new double[CountRow];
        double[] disc = new double[CountRow];
        double[] timeDuration = new double[CountRow];
        string[] source = new string[CountRow];
        double[] tax = new double[CountRow];
        double[] amount = new double[CountRow];
        string[] wh = new string[CountRow];

        sql.Length = 0;
        sql.Append("SELECT a.trans_no, a.trans_date, a.status,   ");
        sql.Append("a.sold_to, c.customer_name AS sold_to_name, a.bill_to, (CASE WHEN d.alias_name = '' THEN d.alias_name_full ELSE d.alias_name_full + ' ' + d.alias_name END) AS bill_to_name,  ");
        sql.Append("a.ship_to,e.street_address as ship_to_address , (CASE WHEN e.alias_name = '' THEN e.alias_name_full ELSE e.alias_name_full + ' ' + e.alias_name END) AS ship_to_name,  ");
        sql.Append("a.cust_po_no, a.cust_po_date, a.installation_date, a.currency_id, a.payment_term, i.description AS payment_term_desc, a.installation_charge, a.installation_by, a.installation_by_site, h.description AS installation_by_name,sw.wh_description, a.note, a.status,  ");
        sql.Append("a.total_amount, a.disc_amount, a.tax_amount, a.amount, a.quot_trans_no, qr.trans_date AS quot_date, ISNULL(u1.username, a.created_by) AS created_by, a.created_date, ISNULL(u2.username, '') AS posted_by, a.posted_date  ");
        sql.Append("FROM rental_order a WITH(READPAST)  ");
        sql.Append("INNER JOIN customer c WITH(READPAST) ON a.sold_to = c.customer_no  ");
        sql.Append("INNER JOIN customer_address d WITH(READPAST) ON a.bill_to = d.id AND d.customer_no = a.sold_to AND d.address_type = '01'  ");
        sql.Append("INNER JOIN customer_address e WITH(READPAST) ON a.ship_to = e.id AND e.customer_no = a.sold_to AND e.address_type = '02'  ");
        sql.Append("INNER JOIN source h WITH(READPAST) ON a.installation_by = h.id AND h.source_type = 'Service'  ");
        sql.Append("INNER JOIN payment_term i WITH(READPAST) ON a.payment_term = i.id  ");
        sql.Append("LEFT JOIN quotation_rental qr WITH(READPAST) ON a.quot_trans_no = qr.trans_no  ");
        sql.Append("LEFT JOIN site_wh sw WITH(READPAST) ON sw.wh_id = a.installation_by  ");
        sql.Append("LEFT JOIN [user] u1 WITH(READPAST) ON a.created_by = u1.user_id  ");
        sql.Append("LEFT JOIN [user] u2 WITH(READPAST) ON a.posted_by = u2.user_id   ");
        sql.Append("WHERE 1 = 1 ");
        sql.Append("AND a.trans_no = '" + Request.QueryString["transNo"].ToString() + "' ");

        cmd = new SqlCommand(sql.ToString(), conn);
        using (dr = cmd.ExecuteReader())
        {
            if (dr.HasRows)
            {
                dr.Read();
                BUName = "Cahaya Manunggal PT";
                TransNo = dr["trans_no"].ToString();
                TransDate = DateTime.Parse(dr["trans_date"].ToString()).ToString("dd MMM yyyy");
                quotationNo = dr["quot_trans_no"].ToString();
                rentTo = dr["sold_to_name"].ToString();
                billTo = dr["bill_to_name"].ToString();
                shipTo = dr["ship_to_name"].ToString() + "<br />" + dr["ship_to_address"].ToString();
                custPONo = dr["cust_po_no"].ToString();
                custPODate = DateTime.Parse(dr["cust_po_date"].ToString()).ToString("dd MMM yyyy");
                installationDate = DateTime.Parse(dr["installation_date"].ToString()).ToString("dd MMM yyyy");
                currency = "IDR";
                installationBy = (dr["installation_by"].ToString() == "S01" ? dr["wh_description"].ToString() : dr["installation_by_name"].ToString());
                installationCharge = double.Parse(dr["installation_charge"].ToString()).ToString("##,#0.#0");
                note = dr["note"].ToString();
                sum_amount = double.Parse(dr["amount"].ToString());
                sum_tax = double.Parse(dr["tax_amount"].ToString());
                sum_total_amount = double.Parse(dr["total_amount"].ToString());
                CreatedBy = dr["created_by"].ToString();
                createdDate = DateTime.Parse(dr["created_date"].ToString()).ToString("dd MMM yyyy");
                PostedBy = dr["posted_by"].ToString();
                postedDate = DateTime.Parse(dr["posted_date"].ToString()).ToString("dd MMM yyyy");
            }
        }

        sql.Length = 0;
        sql.Append("SELECT ROW_NUMBER() OVER (ORDER BY a.line_no ASC, a.line_no ASC) AS row_no,  ");
        sql.Append("a.line_no, b.article_type, a.article_no, b.article_description, ");
        sql.Append("a.qty, 'MON' time_unit_id, ");
        sql.Append("a.rate, a.disc, a.tax, a.time_duration, a.source_id, ISNULL(e.description, '') AS source_desc, a.wh_id, ISNULL(f.wh_description, '') AS wh_name ");
        sql.Append("FROM rental_order_detail a WITH(READPAST) ");
        sql.Append("INNER JOIN article b WITH(READPAST) ON a.article_no = b.article_no ");
        sql.Append("LEFT JOIN source e WITH(READPAST) ON a.source_id = e.id ");
        sql.Append("LEFT JOIN site_wh f WITH(READPAST) ON a.wh_id = f.wh_id ");
        sql.Append("WHERE 1 = 1 ");
        sql.Append("AND a.trans_no = '" + Request.QueryString["transNo"].ToString() + "' ");

        cmd = new SqlCommand(sql.ToString(), conn);
        using (dr = cmd.ExecuteReader())
        {
            if (dr.HasRows)
            {
                i = 0;
                while (dr.Read())
                {
                    article[i] = dr["article_description"].ToString();
                    qty[i] = double.Parse(dr["qty"].ToString());
                    timeUnit[i] = dr["time_unit_id"].ToString();
                    rate[i] = double.Parse(dr["rate"].ToString());
                    disc[i] = double.Parse(dr["disc"].ToString());
                    tax[i] = double.Parse(dr["tax"].ToString());
                    amount[i] = (double.Parse(dr["qty"].ToString()) * double.Parse(dr["time_duration"].ToString()) * (double.Parse(dr["rate"].ToString()) /*- double.Parse(dr["disc"].ToString())*/));
                    timeDuration[i] = double.Parse(dr["time_duration"].ToString());
                    source[i] = dr["source_desc"].ToString();
                    wh[i] = dr["wh_name"].ToString();
                    i++;
                }
            }
        }

        for (int PageNo = 1; PageNo <= Convert.ToInt16(PageTotal); PageNo++)
        {
            Response.Write("<table border='0' width='730px' cellpadding='2' cellspacing='0' bordercolor='black'>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("<table border='0' width='100%' cellpadding='0' cellspacing='0' bordercolor='black'>");
            Response.Write("<tr>");
            Response.Write("<td width='520'></td><td width='70'></td><td width='10'></td><td width='140'></td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td><b>" + BUName + "</b></td>");
            Response.Write("<td>Trans No</td><td>:</td><td>" + TransNo + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td>" + BUAddress + "</td>");
            Response.Write("<td>Trans Date</td><td>:</td><td>" + TransDate + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr height='2'><td colspan='4'>&nbsp;</td></tr>");
            Response.Write("<tr><td colspan='4'><div align='center' style='font-size:12px; font-weight:bold;'>" + FormTitle + "</div></td></tr>");
            Response.Write("</table>");
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("<table align='center' border='0' width='100%' cellpadding='2' cellspacing='0' cols='3' bordercolor='black'>");
            Response.Write("<tr>");
            Response.Write("<td width='120'></td><td width='10'></td><td width=''></td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Rental Quotation No</td><td>:</td><td>" + quotationNo + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Rent To</td><td>:</td><td>" + rentTo + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Bill To</td><td valign='top'>:</td><td>" + billTo + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Rent To Site</td><td valign='top'>:</td><td>" + shipTo + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Cust. PO No. / Date</td><td>:</td><td>" + custPONo + " / " + custPODate.ToUpper() + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Req. Delv. Date</td><td>:</td><td>" + installationDate.ToUpper() + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Installation Charge</td><td>:</td><td>" + currency + " " + double.Parse(installationCharge).ToString("##,#0.#0") + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Net Amount</td><td>:</td><td>" + currency + " " + sum_amount.ToString("##,#0.#0") + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>VAT</td><td>:</td><td>" + currency + " " + sum_tax.ToString("##,#0.#0") + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Gross Amount</td><td>:</td><td>" + currency + " " + sum_total_amount.ToString("##,#0.#0") + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Note</td><td>:</td><td>" + note + "</td>");
            Response.Write("</tr>");
            Response.Write("</table>");
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("<tr height='2'><td></td></tr>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("<table border='1' width='100%' cellpadding='2' cellspacing='0' bordercolor='black' rules='cols'>");
            Response.Write("<tr>");
            Response.Write("<th width='20' align='center'>No.</th>");
            Response.Write("<th width='' align='center'>Article</th>");
            Response.Write("<th width='40' align='center'>Qty</th>");
            Response.Write("<th width='50' align='center'>Rent Duration</th>");
            Response.Write("<th width='80' align='center'>Rent Rate</th>");
            Response.Write("<th width='60' align='center'>VAT</th>");
            Response.Write("<th width='80' align='center'>Amount</th>");
            Response.Write("<th width='60' align='center'>Discount</th>");
            Response.Write("</tr>");
            for (i = 0; i <= RowMax - 1; i++)
            {
                Response.Write("<tr valign='top'>");
                if (j <= CountRow - 1)
                {
                    Response.Write("<td><div align='right'>" + (j + 1).ToString() + "</div></td>");
                    Response.Write("<td><div align='left'>" + article[j] + "</div></td>");
                    Response.Write("<td><div align='right'>" + qty[j].ToString("##,#0.#0") + "</div></td>");
                    Response.Write("<td><div align='center'>" + timeDuration[j].ToString("##,#0") + " " + timeUnit[j] + "</div></td>");
                    Response.Write("<td><div align='right'>" + rate[j].ToString("##,#0.#0") + "</div></td>");
                    Response.Write("<td><div align='right'>" + tax[j].ToString("##,#0.#0") + "</div></td>");
                    Response.Write("<td><div align='right'>" + amount[j].ToString("##,#0.#0") + "</div></td>");
                    Response.Write("<td><div align='right'>" + disc[j].ToString("##,#0.#0") + "</div></td>");
                }
                else
                {
                    Response.Write("" + cAdih.strRepeat("<td>&nbsp;</td>", 8) + "");
                }
                Response.Write("</tr>");
                j += 1;
            }
            Response.Write("</tr>");
            Response.Write("</table>");
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("<tr height='5'><td></td></tr>");
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("<table border='0' width='730' cellpadding='2' cellspacing='0' cols='3'>");
            Response.Write("<tr><td width='50%'></td><td width='50%'></td></tr>");
            Response.Write("<tr>");
            Response.Write("<td><div align='center'>Created by,</div></td>");
            Response.Write("<td><div align='center'>Posted by,</div></td>");
            Response.Write("</tr>");
            Response.Write("<tr><td width='50%'></td><td width='50%'></td></tr>");
            Response.Write("<tr>");
            Response.Write("<td><div align='center'>" + createdDate + "</div></td>");
            Response.Write("<td><div align='center'>" + (postedDate == "01-01-1900" ? "&nbsp;" : postedDate) + "</div></td>");
            Response.Write("</tr>");
            Response.Write("<tr height='" + (PageNo == PageTotal ? "40" : "50") + "' valign='bottom'>");
            Response.Write("<td><div align='center'>(" + CreatedBy + ")</div></td>");
            Response.Write("<td><div align='center'>(" + PostedBy + ")</div></td>");
            Response.Write("</tr>");
            Response.Write("</table>");
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("<tr><td align='right' style='font-size:8px;'>&nbsp;</td></tr>");
            Response.Write("</table>");
            if (PageNo != PageTotal)
            {
                Response.Write("<br /><br />");
            }
            conn.Close();
        }
    }
}