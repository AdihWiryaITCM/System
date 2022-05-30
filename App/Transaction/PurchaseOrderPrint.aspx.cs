using System;
using System.Data.SqlClient;
using System.Text;

public partial class Transaction_PurchaseOrderPrint : System.Web.UI.Page
{
    SqlConnection conn;
    SqlCommand cmd;
    SqlDataReader dr;
    StringBuilder sql = new StringBuilder();
    ClassAdih cAdih = new ClassAdih();

    protected void Page_Load(object sender, EventArgs e)
    {
        string FormTitle = "PURCHASE ORDER";
        int RowMin = 10;
        string BUName = "";
        string BUAddress = "";
        string vatRegNo = "";
        string TransNo = "";
        string TransDate = "";
        string vendor = "";
        string approval_status = "";
        string ship_to = "";
        string req_delv_date = "";
        string payment_term = "";
        string currency = "";
        string Note = "";
        double total_amount = 0;
        double total_proxy = 0;
        string CreatedBy = "";
        string createdDate = "";
        string PostedBy = "";
        int i = 1;
        int j = 0;
        double sum_amount = 0;
        double sum_tax = 0;
        double sum_total_amount = 0;

        sql.Length = 0;
        sql.Append("SELECT COUNT(*) FROM purchase_order_detail WITH(READPAST) WHERE trans_no = '" + Request.QueryString["transNo"].ToString() + "' ");

        conn = new SqlConnection(cAdih.getConnStr("Connection"));
        conn.Open();

        cmd = new SqlCommand(sql.ToString(), conn);
        int CountRow = int.Parse(cmd.ExecuteScalar().ToString());
        double PageTotal = Math.Ceiling(Convert.ToDouble(CountRow));

        string[] PRNo = new string[CountRow];
        string[] article = new string[CountRow];
        string[] unit = new string[CountRow];
        double[] qty = new double[CountRow];
        double[] unit_price = new double[CountRow];
        double[] amount = new double[CountRow];
        double[] tax = new double[CountRow];
        double[] total = new double[CountRow];
        string[] note = new string[CountRow];
        string[] content = new string[CountRow];

        sql.Length = 0;
        sql.Append("SELECT a.purchase_requisition_no,  ");
        sql.Append("a.trans_no, a.trans_date, ");
        sql.Append("l.wh_description AS ship_to_name, ");
        sql.Append("e.vendor_name, a.req_delv_date, h.description AS payment_term, a.currency_id AS currency, a.note, ");
        sql.Append("a.status, ISNULL(j.username, a.created_by) AS created_by, a.created_date, ISNULL(k.username, '') AS posted_by, a.posted_date, a.total_amount, ISNULL(ta.sum_approval_proxy, 0) AS total_proxy, a.approval_status ");
        sql.Append("FROM purchase_order a WITH(READPAST) ");
        sql.Append("INNER JOIN vendor e WITH(READPAST) ON a.vendor_id = e.vendor_no ");
        sql.Append("INNER JOIN payment_term h WITH(READPAST) ON a.payment_term = h.id ");
        sql.Append("LEFT JOIN[user] j WITH(READPAST) ON a.created_by = j.user_id ");
        sql.Append("LEFT JOIN[user] k WITH(READPAST) ON a.posted_by = k.user_id ");
        sql.Append("LEFT JOIN site_wh l WITH(READPAST) ON a.ship_to = l.wh_id ");
        sql.Append("LEFT JOIN( ");
        sql.Append("    SELECT trans_no, SUM(approval_proxy) AS sum_approval_proxy, MAX(approval_date) AS max_approval_date ");
        sql.Append("    FROM trans_approval WITH(READPAST) ");
        sql.Append("    GROUP BY trans_no ");
        sql.Append(") AS ta ON a.trans_no = ta.trans_no ");
        sql.Append("WHERE a.trans_no = '" + Request.QueryString["transNo"].ToString() + "' ");

        cmd = new SqlCommand(sql.ToString(), conn);
        using (dr = cmd.ExecuteReader())
        {
            if (dr.HasRows)
            {
                dr.Read();
                BUName = "Cahaya Manunggal PT";
                BUAddress = "KOMPLEK TANGCITY BLOK F NO. 37 CIKOKOL, TANGERANG, BANTEN";
                vatRegNo = "02.909.009.9-415.000";
                TransNo = dr["trans_no"].ToString();
                TransDate = DateTime.Parse(dr["trans_date"].ToString()).ToString("dd MMM yyyy");
                vendor = dr["vendor_name"].ToString();
                ship_to = dr["ship_to_name"].ToString();
                req_delv_date = DateTime.Parse(dr["req_delv_date"].ToString()).ToString("dd MMM yyyy");
                approval_status = dr["approval_status"].ToString();
                payment_term = dr["payment_term"].ToString();
                currency = dr["currency"].ToString();
                Note = dr["note"].ToString();
                CreatedBy = dr["created_by"].ToString();
                createdDate = DateTime.Parse(dr["created_date"].ToString()).ToString("dd MMM yyyy");
                PostedBy = dr["posted_by"].ToString();
                total_amount = double.Parse(dr["total_amount"].ToString());
                total_proxy = double.Parse(dr["total_proxy"].ToString());
            }
        }

        sql.Length = 0;
        sql.Append("SELECT a.approval_by, b.username as user_name, a.approval_date ");
        sql.Append("FROM trans_approval a ");
        sql.Append("INNER JOIN [user] b ON a.approval_by = b.user_id ");
        sql.Append("WHERE a.trans_no = '" + Request.QueryString["transNo"].ToString() + "' ");
        sql.Append("ORDER BY a.approval_date ASC, a.approval_proxy ASC ");

        cmd = new SqlCommand(sql.ToString(), conn);
        using (dr = cmd.ExecuteReader())
        {
            if (dr.HasRows)
            {
                PostedBy = "";
                while (dr.Read())
                {
                    PostedBy += dr["user_name"].ToString() + ", " + DateTime.Parse(dr["approval_date"].ToString()).ToString("dd MMM yyy") + "<br />";
                }
            }
        }

        sql.Length = 0;
        sql.Append("SELECT ROW_NUMBER() OVER (ORDER BY a.line_no ASC, a.line_no ASC) AS row_no, a.purchase_requisition_no, ");
        sql.Append("b.article_description, u.description AS unit_name, a.qty, a.unit_price, a.amount, a.amount*a.tax/100 as tax, a.total_amount, a.note, '1' as content, u.description as uom ");
        sql.Append("FROM purchase_order_detail a WITH(READPAST) ");
        sql.Append("INNER JOIN article b WITH(READPAST) ON a.article_no = b.article_no ");
        sql.Append("INNER JOIN uom u WITH(READPAST) ON b.base_uom = u.id ");
        sql.Append("WHERE a.trans_no = '" + Request.QueryString["transNo"].ToString() + "' ");

        cmd = new SqlCommand(sql.ToString(), conn);
        using (dr = cmd.ExecuteReader())
        {
            if (dr.HasRows)
            {
                i = 0;
                while (dr.Read())
                {
                    PRNo[i] = dr["purchase_requisition_no"].ToString();
                    article[i] = dr["article_description"].ToString();
                    unit[i] = dr["unit_name"].ToString();
                    qty[i] = double.Parse(dr["qty"].ToString());
                    unit_price[i] = double.Parse(dr["unit_price"].ToString());
                    amount[i] = double.Parse(dr["amount"].ToString());
                    tax[i] = double.Parse(dr["tax"].ToString());
                    total[i] = double.Parse(dr["total_amount"].ToString());
                    note[i] = dr["note"].ToString();
                    if (dr["unit_name"].ToString() != dr["uom"].ToString())
                    {
                        content[i] = "/<br />" + dr["conten"].ToString() + " " + dr["uom"].ToString();
                    }
                    else
                    {
                        content[i] = "";
                    }
                    sum_amount += double.Parse(dr["amount"].ToString());
                    sum_tax += double.Parse(dr["tax"].ToString());
                    sum_total_amount += double.Parse(dr["total_amount"].ToString());
                    i++;
                }
            }
        }


        conn.Close();

        int PageNo = 1;
            if (total_amount > total_proxy)
            {
                Response.Write("<table border='0' width='730px' cellpadding='2' cellspacing='0' bordercolor='black' background='../Images/not_valid.jpg'>");
            }
            else {
                Response.Write("<table border='0' width='730px' cellpadding='2' cellspacing='0' bordercolor='black'>");
            }
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
            Response.Write("<tr>");
            Response.Write("<td colspan='2'>" + vatRegNo + "</td>");
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
            Response.Write("<td width='100'></td><td width='10'></td><td width=''></td>");
            Response.Write("</tr>");
            //Response.Write("<tr>");
            //Response.Write("<td valign='top'>PR No </td><td>:</td><td>" + PRNo + "</td>");
            //Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Vendor</td><td>:</td><td>" + vendor + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Ship To</td><td valign='top'>:</td><td>" + ship_to + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Req Delv Date</td><td>:</td><td>" + req_delv_date.ToUpper() + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Curr / Term</td><td>:</td><td>" + currency + " / " + payment_term + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Status</td><td>:</td><td>" + approval_status + "</td>");
            Response.Write("</tr>");
            //Response.Write("<tr>");
            //Response.Write("<td valign='top'>&nbsp;</td><td>&nbsp;</td><td>" + (item_wo_order == "True" ? "☑" : "☐") + " allow receive article without order</td>");
            //Response.Write("</tr>");
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
            Response.Write("<th width='70' align='center'>Unit</th>");
            Response.Write("<th width='50' align='center'>Qty</th>");
            Response.Write("<th width='80' align='center'>Unit Price</th>");
            Response.Write("<th width='80' align='center'>Amount</th>");
            Response.Write("<th width='80' align='center'>Tax</th>");
            Response.Write("<th width='80' align='center'>Total Amount</th>");
            Response.Write("<th width='100' align='center'>Note</th>");
            Response.Write("</tr>");

            if (CountRow > RowMin)
            {
                for (i = 0; i <= CountRow - 1; i++)
                {
                    Response.Write("<tr valign='top'>");
                    if (j <= CountRow - 1)
                    {
                        Response.Write("<td><div align='right'>" + (j + 1).ToString() + "</div></td>");
                        Response.Write("<td><div align='left'>" + article[j] + "</div></td>");
                        Response.Write("<td><div align='center'>" + unit[j] + content[j] + "</div></td>");
                        Response.Write("<td><div align='right'>" + qty[j].ToString("#,##0.00") + "</div></td>");
                        Response.Write("<td><div align='right'>" + unit_price[j].ToString("#,##0.00") + "</div></td>");
                        Response.Write("<td><div align='right'>" + amount[j].ToString("#,##0.00") + "</div></td>");
                        Response.Write("<td><div align='right'>" + tax[j].ToString("#,##0.00") + "</div></td>");
                        Response.Write("<td><div align='right'>" + total[j].ToString("#,##0.00") + "</div></td>");
                        Response.Write("<td><div align='left'>" + note[j] + "</div></td>");
                    }
                    else
                    {
                        Response.Write("" + cAdih.strRepeat("<td>&nbsp;</td>", 9) + "");
                    }
                    Response.Write("</tr>");
                    j += 1;
                }
            }
            else
            {
                for (i = 0; i <= RowMin - 1; i++)
                {
                    Response.Write("<tr valign='top'>");
                    if (j <= CountRow - 1)
                    {
                        Response.Write("<td><div align='right'>" + (j + 1).ToString() + "</div></td>");
                        Response.Write("<td><div align='left'>" + article[j] + "</div></td>");
                        Response.Write("<td><div align='center'>" + unit[j] + content[j] + "</div></td>");
                        Response.Write("<td><div align='right'>" + qty[j].ToString("#,##0.00") + "</div></td>");
                        Response.Write("<td><div align='right'>" + unit_price[j].ToString("#,##0.00") + "</div></td>");
                        Response.Write("<td><div align='right'>" + amount[j].ToString("#,##0.00") + "</div></td>");
                        Response.Write("<td><div align='right'>" + tax[j].ToString("#,##0.00") + "</div></td>");
                        Response.Write("<td><div align='right'>" + total[j].ToString("#,##0.00") + "</div></td>");
                        Response.Write("<td><div align='left'>" + note[j] + "</div></td>");
                    }
                    else
                    {
                        Response.Write("" + cAdih.strRepeat("<td>&nbsp;</td>", 9) + "");
                    }
                    Response.Write("</tr>");
                    j += 1;
                }
            }
            if (CountRow == PageTotal)
            {
                Response.Write("<tr valign='top'>");
                Response.Write("<td colspan='5' class='T1-B0-L0-R0'><div align='right'>Total&nbsp;<div></td>");
                //Response.Write("<td class='T1-B0-L0-R0'><div align='right'>" + sum_qty.ToString("#,##0.00") + "</div></td>");
                //Response.Write("<td class='T1-B0-L0-R0'><div align='right'>" + sum_price.ToString("#,##0.00") + "</div></td>");
                Response.Write("<td class='T1-B0-L0-R0'><div align='right'>" + sum_amount.ToString("#,##0.00") + "</div></td>");
                Response.Write("<td class='T1-B0-L0-R0'><div align='right'>" + sum_tax.ToString("#,##0.00") + "</div></td>");
                Response.Write("<td class='T1-B0-L0-R0'><div align='right'>" + sum_total_amount.ToString("#,##0.00") + "</div></td>");
                Response.Write("<td class='T1-B0-L0-R0'><div align='right'></div></td>");

            }
            Response.Write("</tr>");
            Response.Write("</table>");
            Response.Write("</td>");
            Response.Write("</tr>");

            Response.Write("<tr height='5'><td></td></tr>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("<table border='1' width='730' cellpadding='2' cellspacing='0'>");
            Response.Write("<tr>");
            Response.Write("<td style='border-bottom:0px; width:50%'><div align='center' >Special Instruction</div></td>");
            Response.Write("<td style='width:25%'><div align='center' >Buyer</div></td>");
            Response.Write("<td style='width:25%'><div align='center' >Purchase Authorization</div></td>");
            Response.Write("</tr>");
            Response.Write("<tr height='" + (PageNo == PageTotal ? "60" : "70") + "' valign='bottom'>");
            Response.Write("<td valign='top'><div align='left'>" + Note + "</div></td>");
            Response.Write("<td><div align='center' vertical-align='text-bottom'>" + CreatedBy + ", " + createdDate + "</div></td>");
            Response.Write("<td><div align='center' vertical-align='text-bottom'>" + PostedBy + "</div></td>");
            Response.Write("</tr>");
            Response.Write("</table>");
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("<table align='center' border='0' width='100%' cellpadding='2' cellspacing='0' cols='3' bordercolor='black'>");
            Response.Write("<tr>");
            Response.Write("<td width='15'></td><td width=''></td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td colspan='2'>Catatan untuk Vendor:</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>1.</td><td>Vendor harus membawa Delivery Order pada saat pengiriman barang / pemenuhan jasa ke pihak " + BUName + ".</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>2.</td><td>Pihak " + BUName + " berhak menolak setiap pengiriman barang / pemenuhan jasa yang jika persyaratan 1 di atas tidak dipenuhi.</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>3.</td><td>Pada saat penagihan, Vendor harus membawa dokumen, dan diurutkan dimulai dari Tanda Terima Invoice, Invoice bermaterai disertai nomor + nama rek pembayaran, Purchase Order asli " + BUName + ", Faktur Pajak asli, Surat Jalan asli. Pihak " + BUName + " berhak menolak setiap tagihan jika persyaratan tersebut tidak dipenuhi.</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>4.</td><td>Kami menghimbau untuk tidak memberikan imbalan dalam bentuk apapun kepada karyawan " + BUName + ". Pelanggaran terhadap ketentuan tersebut berakibat pada pemutusan hubungan kerjasama dengan perusahaan anda.</td>");
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
        }
    
}   