using System;
using System.Data.SqlClient;
using System.Text;

public partial class Transaction_InvoiceConfirmationReceiptPrint : System.Web.UI.Page
{
    private SqlConnection conn;
    private SqlCommand cmd;
    private StringBuilder sql = new StringBuilder();
    private ClassAdih cAdih = new ClassAdih();

    private static string TABLE_NAME = "invoice_confirmation_receipt";
    private static string TABLE_NAME_DETAIL = "invoice_confirmation_receipt_detail";

    protected void Page_Load(object sender, EventArgs e)
    {
        string FormTitle = "PURCHASE INVOICE";
        string mode = "";
        int RowMax = 20;
        string BUName = "";
        string BUAddress = "";
        string TransNo = "";
        string TransDate = "";
        string vendorName = "";
        string VendorInvoiceNo = "";
        string TaxFactureNo = "";
        string TaxFactureDate = "";
        string TotalAmount = "";
        string PaymentTerm = "";
        string Duedate = "";
        string Ttfdate = "";
        string CurrencyID = "";
        string PONO = "";
        string note = "";
        string CreatedBy = "";
        string createdDate = "";
        string PostedBy = "";
        string postedDate = "";
        string type = "";
        int i = 1;
        int j = 0;
        double sum_amount = 0;
        double sum_amt = 0;
        SqlDataReader dr;
        double sum_tax = 0;

        sql.Length = 0;
        sql.Append("SELECT COUNT(1) FROM invoice_confirmation_receipt_detail WITH(READPAST) WHERE trans_no = '" + Request.QueryString["transNo"].ToString() + "' ");

        conn = new SqlConnection(cAdih.getConnStr("Connection"));
        conn.Open();

        cmd = new SqlCommand(sql.ToString(), conn);
        int CountRow = int.Parse(cmd.ExecuteScalar().ToString());

        sql.Length = 0;
        sql.Append("SELECT icc.trans_no, icc.trans_date, icc.reff_order_no, ");
        sql.Append("icc.reff_order_no AS po_trans_no, CONVERT(VARCHAR, po.trans_date, 106) AS po_trans_date, icc.vendor_no, v.vendor_name, icc.vendor_invoice_no, icc.tax_facture_no, ");
        sql.Append("'IDR' as currency_id, icc.payment_term, icc.due_date, icc.total_amount, icc.tax_facture_date, icc.note, icc.status, isnull(cust.cust_no, '') 'cust_no', isnull(cust.cust_name, '') 'cust_name', isnull(cust.ship_to_id, '') 'ship_to_id', isnull(cust.ship_to_name, '') 'ship_to_name' ");
        sql.Append("    ,(select username from dbo.[user] where user_id = icc.created_by) 'created_by', ");
        sql.Append("	icc.created_date, ");
        sql.Append("	(select username from dbo.[user] where user_id = icc.posted_by) 'posted_by', ");
        sql.Append("	icc.posted_date ");
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
        sql.Append("AND icc.trans_no = '" + Request.QueryString["transNo"].ToString() + "' ");

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

                        BUName = "Cahaya Manunggal PT";
                        TransNo = dr["trans_no"].ToString();
                        TransDate = DateTime.Parse(dr["trans_date"].ToString()).ToString("dd MMM yyyy");
                        PONO = dr["reff_order_no"].ToString();
                        vendorName = dr["vendor_name"].ToString();
                        VendorInvoiceNo = dr["vendor_invoice_no"].ToString();
                        TaxFactureNo = dr["tax_facture_no"].ToString();
                        CurrencyID = dr["currency_id"].ToString();
                        TotalAmount = double.Parse(dr["total_amount"].ToString()).ToString("##,#0.#0");
                        PaymentTerm = dr["payment_term"].ToString();
                        Duedate = DateTime.Parse(dr["due_date"].ToString()).ToString("dd MMM yyyy");
                        note = dr["note"].ToString();
                        CreatedBy = dr["created_by"].ToString();
                        createdDate = DateTime.Parse(dr["created_date"].ToString()).ToString("dd MMM yyyy");
                        PostedBy = dr["posted_by"].ToString();
                        postedDate = DateTime.Parse(dr["posted_date"].ToString()).ToString("dd MMM yyyy");
                        if (dr["tax_facture_date"].ToString() != "")
                        {
                            TaxFactureDate = DateTime.Parse(dr["tax_facture_date"].ToString()).ToString("dd MMM yyyy");
                        }
                    }
                }
            }
        }


        string[] ID_no = new string[CountRow];
        string[] article_no = new string[CountRow];
        string[] article_description = new string[CountRow];
        string[] rentalUnit = new string[CountRow];
        decimal[] unit_price = new decimal[CountRow];
        decimal[] tax = new decimal[CountRow];
        decimal[] qtyOrder = new decimal[CountRow];
        decimal[] qtyDeliv = new decimal[CountRow];
        decimal[] total = new decimal[CountRow];
        string[] noteDetail = new string[CountRow];
        decimal[] nettAmt = new decimal[CountRow];

        sql.Length = 0;
        sql.Append("select	a.line_no, b.article_type, a.article_no, b.article_description, b.base_uom as unit_id, c.description as unit_name,  ");
        sql.Append("a.qty_order, a.qty_received, a.unit_price, a.unit_tax, a.note, a.qty_received * (a.unit_price + a.unit_tax) AS total_amount ");
        sql.Append(", a.inbound_delivery_no ");
        sql.Append("from invoice_confirmation_receipt_detail a ");
        sql.Append("inner join article b on a.article_no = b.article_no ");
        sql.Append("left join uom c on b.base_uom = c.id ");
        sql.Append("AND a.trans_no = '" + Request.QueryString["transNo"].ToString() + "' ");

        double PageTotal = Math.Ceiling(Convert.ToDouble(CountRow) / Convert.ToDouble(RowMax));

        using (conn = new SqlConnection(cAdih.getConnStr("Connection")))
        {
            conn.Open();
            using (cmd = new SqlCommand(sql.ToString(), conn))
            {
                using (dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        i = 0;
                        while (dr.Read())
                        {
                            ID_no[i] = dr["INBOUND_DELIVERY_NO"].ToString();
                            article_no[i] = dr["ARTICLE_NO"].ToString(); 
                            article_description[i] = dr["ARTICLE_DESCRIPTION"].ToString(); 
                            rentalUnit[i] = dr["UNIT_NAME"].ToString(); 
                            qtyOrder[i] = Convert.ToDecimal(dr["QTY_ORDER"].ToString()); 
                            qtyDeliv[i] = Convert.ToDecimal(dr["QTY_RECEIVED"].ToString()); 
                            unit_price[i] = Convert.ToDecimal(dr["UNIT_PRICE"].ToString()); 
                            tax[i] = Convert.ToDecimal(dr["UNIT_TAX"].ToString()); 
                            total[i] = Convert.ToDecimal(dr["TOTAL_AMOUNT"].ToString()); 
                            nettAmt[i] = Convert.ToDecimal(dr["UNIT_PRICE"].ToString()) * Convert.ToDecimal(dr["QTY_RECEIVED"].ToString());
                            noteDetail[i] = dr["NOTE"].ToString();
                            sum_amt += Convert.ToDouble(qtyDeliv[i]) * Convert.ToDouble(unit_price[i]);
                            sum_tax += Convert.ToDouble(qtyDeliv[i]) * Convert.ToDouble(tax[i]);
                            sum_amount += Convert.ToDouble(total[i]);
                            i++;
                        }
                    }
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
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td>" + BUAddress + "</td>");
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
            Response.Write("<td valign='top'>Trans No. / Date</td><td>:</td><td>" + TransNo + " / " + TransDate + " </td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Purchase Order No.</td><td>:</td><td>" + PONO + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Vendor</td><td valign='top'>:</td><td>" + vendorName + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Vendor Invoice No.</td><td valign='top'>:</td><td>" + VendorInvoiceNo + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Tax Facture No.</td><td>:</td><td>" + TaxFactureNo + " | " + TaxFactureDate + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Nett Amount </td><td>:</td><td>" + CurrencyID + "  " + sum_amt.ToString("#,##0.#0") + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>VAT </td><td>:</td><td>" + CurrencyID + "  " + sum_tax.ToString("#,##0.#0") + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Gross Amount </td><td>:</td><td>" + CurrencyID + "  " + TotalAmount + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Payment Term</td><td>:</td><td>" + PaymentTerm + "</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>Due Date</td><td>:</td><td>" + Duedate + "</td>");
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
            Response.Write("<th width='2' align='center'>No.</th>");
            Response.Write("<th width='15' align='center'>Inbound Delivery No.</th>");
            Response.Write("<th width='25' align='center'>Article No.</th>");
            Response.Write("<th width='80' align='center'>Article Description</th>");
            Response.Write("<th width='10' align='center'>Unit</th>");
            Response.Write("<th width='5' align='center'>Qty Order</th>");
            Response.Write("<th width='3' align='center'>Qty Received</th>");
            Response.Write("<th width='15' align='center'>Unit Price</th>");
            Response.Write("<th width='15' align='center'>VAT</th>");
            Response.Write("<th width='15' align='center'>Nett Amount</th>");
            Response.Write("<th width='40' align='center'>Note</th>");
            Response.Write("</tr>");
            for (i = 0; i <= RowMax - 1; i++)
            {
                Response.Write("<tr valign='top'>");
                if (j <= CountRow - 1)
                {
                    Response.Write("<td><div align='center'>" + (j + 1).ToString() + "</div></td>");
                    Response.Write("<td><div align='center'>" + ID_no[j] + "</div></td>");
                    Response.Write("<td><div align='center'>" + article_no[j] + "</div></td>");
                    Response.Write("<td><div align='left'>" + article_description[j] + "</div></td>");
                    Response.Write("<td><div align='center'>" + rentalUnit[j] + "</div></td>");
                    Response.Write("<td><div align='right'>" + qtyOrder[j] + "</div></td>");
                    Response.Write("<td><div align='right'>" + qtyDeliv[j] + "</div></td>");
                    Response.Write("<td><div align='right'>" + unit_price[j].ToString("##,#0.#0") + "</div></td>");
                    Response.Write("<td><div align='right'>" + tax[j].ToString("##,#0.#0") + "</div></td>");
                    Response.Write("<td><div align='right'>" + nettAmt[j].ToString("##,#0.#0") + "</div></td>");
                    Response.Write("<td><div align='left'>" + noteDetail[j] + "</div></td>");
                }
                else
                {
                    Response.Write("" + cAdih.strRepeat("<td>&nbsp;</td>", 11) + "");
                }
                Response.Write("</tr>");
                j += 1;
            }
            if (PageNo == PageTotal)
            {
                Response.Write("<tr valign='top'>");
                Response.Write("<td colspan='9' class='T1-B0-L0-R0'><div align='right'>Nett Amount&nbsp;<div></td>");
                Response.Write("<td class='T1-B0-L0-R0'><div align='right'>" + sum_amt.ToString("##,#0.#0") + "</div></td>");
                Response.Write("<td class='T1-B0-L0-R0'><div align='right'>&nbsp;</div></td></tr>");
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