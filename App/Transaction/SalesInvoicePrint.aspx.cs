using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Transaction_SalesInvoicePrint : System.Web.UI.Page
{
    private SqlConnection conn;
    private SqlCommand cmd;
    private SqlDataReader dr;
    private StringBuilder sql = new StringBuilder();
    private ClassAdih cAdih = new ClassAdih();

    protected void Page_Load(object sender, EventArgs e)
    {
        string FormTitle = "SALES INVOICE";
        string mode = "";
        int RowMin = 15;
        string BUName = "";
        string BUAddress = "";
        string TransNo = "";
        string TransDate = "";
        string rentTo = "";
        string billTo = "";
        string shipTo = "";
        string PaymentTerm = "";
        string Duedate = "";
        string custPONo = "";
        string custPODate = "";
        string ppn = "";
        string totalBill = "";
        string terbilang = "";
        string namarek = "";
        string rekbca = "";
        string cabang = "";
        string vat = "";
        string tax_facture_no = "";
        string totalinvoice = "";

        //string currency = "";
        string note = "";
        string CreatedBy = "";
        string createdDate = "";
        string PostedBy = "";
        string postedDate = "";
        string PeriodHeader = "";
        int i = 1;
        int j = 0;
        double sum_price = 0;
        double sum_tax = 0;
        double sum_amount = 0;
        double sum_disc_amount = 0;

        sql.Length = 0;
        sql.Append("SELECT COUNT(1) FROM sales_invoice_detail WITH(READPAST) WHERE trans_no = '" + Request.QueryString["transNo"].ToString() + "' ");

        conn = new SqlConnection(cAdih.getConnStr("Connection"));
        conn.Open();

        cmd = new SqlCommand(sql.ToString(), conn);
        int CountRow = int.Parse(cmd.ExecuteScalar().ToString());

        mode = Request.QueryString["mode"].ToString();
        //mode = "RENTAL EXTENSION";

        sql.Length = 0;
        sql.Append("exec sp_sales_invoice_getrow '"+ Request.QueryString["transNo"].ToString() + "' ");

        cmd = new SqlCommand(sql.ToString(), conn);
        using (dr = cmd.ExecuteReader())
        {
            if (dr.HasRows)
            {
                dr.Read();
                BUName = "Cahaya Manunggal PT";
                TransNo = dr["trans_no"].ToString();
                TransDate = DateTime.Parse(dr["trans_date"].ToString()).ToString("dd MMM yyyy");
                rentTo = dr["customer_name"].ToString();
                billTo = dr["bill_to"].ToString() + " - " + dr["alias_name_full"].ToString();
                shipTo = dr["ship_to"].ToString();
                custPONo = dr["cust_po_no"].ToString();
                custPODate = DateTime.Parse(dr["cust_po_date"].ToString()).ToString("dd MMM yyyy");
                PaymentTerm = dr["top_desc"].ToString();
                Duedate = DateTime.Parse(dr["due_date"].ToString()).ToString("dd MMM yyyy");
                note = dr["note"].ToString();
                ppn = (decimal.Parse(dr["ppn"].ToString()).ToString("##,#0.#0"));
                totalBill = (decimal.Parse(dr["total_amount"].ToString()).ToString("##,#0.#0"));
                vat = (decimal.Parse(dr["ppn"].ToString()).ToString("##,#0.#0"));
                totalinvoice = (decimal.Parse(dr["total_invoice"].ToString()).ToString("##,#0.#0"));
                terbilang = dr["terbilang"].ToString();
                tax_facture_no = dr["tax_facture_no"].ToString();
                CreatedBy = dr["created_by"].ToString();
                createdDate = DateTime.Parse(dr["created_date"].ToString()).ToString("dd MMM yyyy");
                PostedBy = dr["posted_by"].ToString();
                postedDate = DateTime.Parse(dr["posted_date"].ToString()).ToString("dd MMM yyyy");
            }
        }



        //_ht.Clear();
        //_ht["p_search"] = "";
        //_ht["p_trans_no"] = Request.Params["transno"];
        DataTable _dt;

        double PageTotal = Math.Ceiling(Convert.ToDouble(CountRow));

        string[] article_no = new string[CountRow];
        string[] article_description = new string[CountRow];
        string[] rentalUnit = new string[CountRow];
        decimal[] unit_price = new decimal[CountRow];
        decimal[] qty = new decimal[CountRow];
        int[] period = new int[CountRow];
        int[] time_duration = new int[CountRow];
        decimal[] price = new decimal[CountRow];
        decimal[] tax = new decimal[CountRow];
        decimal[] discount = new decimal[CountRow];
        decimal[] total = new decimal[CountRow];
        string[] notedetail = new string[CountRow];
        decimal[] netamount = new decimal[CountRow];
        decimal[] price_after_disc = new decimal[CountRow];
        decimal[] total_price_after_disc = new decimal[CountRow];

        sql.Length = 0;
        sql.Append("exec sp_sales_invoice_detail_getrows '" + Request.QueryString["transNo"].ToString() + "' ");

        cmd = new SqlCommand(sql.ToString(), conn);
        using (dr = cmd.ExecuteReader())
        {
            if (dr.HasRows)
            {
                i = 0;
                while (dr.Read())
                {
                    article_description[i] = dr["ARTICLE_DESCRIPTION"].ToString();
                    unit_price[i] = Convert.ToDecimal(dr["UNIT_PRICE"].ToString());
                    qty[i] = Convert.ToDecimal(dr["QTY_DELIVERED"].ToString()) ;
                    period[i] = Convert.ToInt32(dr["PERIOD"].ToString()) ;
                    price_after_disc[i] = Convert.ToDecimal(dr["NET_AMOUNT"].ToString()) ;
                    notedetail[i] = dr["NOTE"].ToString();
                    time_duration[i] = Convert.ToInt32(dr["BILL_DURATION"].ToString()) ;
                    price[i] = Convert.ToDecimal(dr["PRICE"].ToString()) ;
                    tax[i] = Convert.ToDecimal(dr["UNIT_TAX"].ToString()) ;
                    discount[i] = Convert.ToDecimal(dr["DISCOUNT"].ToString()) ;

                    total[i] = Convert.ToDecimal(dr["TOTAL_AMOUNT"].ToString()) ;

                    if (period[i].ToString() != "0")
                        PeriodHeader = dr["PERIOD_DESC"].ToString();

                    sum_price += Convert.ToDouble(price[i]);
                    sum_tax += Convert.ToDouble(tax[i]);
                    sum_disc_amount += Convert.ToDouble(discount[i]);
                    sum_amount += Convert.ToDouble(total[i]);
                    i++;
                }
            }
        }

        int PageNo = 1;
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
        Response.Write("<td valign='right'>Invoice No.</td><td>:</td><td>" + TransNo + "</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td valign='top'>Invoice Date</td><td>:</td><td>" + TransDate + "</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td valign='top'>Tax Facture No</td><td>:</td><td>" + tax_facture_no + "</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td valign='top'>Rent To</td><td>:</td><td>" + rentTo + "</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td valign='top'>Bill To</td><td valign='top'>:</td><td>" + billTo + "</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td valign='top'>Ship To</td><td valign='top'>:</td><td>" + shipTo + "</td>");
        Response.Write("</tr>");
        //Response.Write("<tr>");
        //Response.Write("<td valign='top'>Rent To Site</td><td valign='top'>:</td><td>" + shipTo + "</td>");
        //Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td valign='top'>Cust. PO No. / Date</td><td>:</td><td>" + custPONo + " / " + custPODate.ToUpper() + "</td>");
        Response.Write("</tr>");
        //if (mode != "RENTAL ORDER")
        //{
        Response.Write("<tr>");
        Response.Write("<td valign='top'>Payment Term</td><td>:</td><td>" + PaymentTerm + "</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td valign='top'>Due Date</td><td>:</td><td>" + Duedate + "</td>");
        Response.Write("</tr>");
        //}
        //if (mode == "customer")
        //{
        //    Response.Write("<tr>");
        //    Response.Write("<td valign='top'>Total Amount</td><td>:</td><td>" + currency + " " + sum_amount.ToString("##,##.#0") + "</td>");
        //    Response.Write("</tr>");
        //}
        Response.Write("<tr>");
        Response.Write("<td valign='top'>Note</td><td>:</td><td>" + note + "</td>");
        Response.Write("</tr>");
        Response.Write("</table>");
        //Response.Write("<tr>");
        //Response.Write("<td valign='top'>" + PeriodHeader + "</td>");
        //Response.Write("</tr>");
        Response.Write("</td>");
        Response.Write("</tr>");
        Response.Write("<tr height='2'><td></td></tr>");
        Response.Write("<tr>");
        Response.Write("<td>");

        Response.Write("<table border='1' width='100%' cellpadding='2' cellspacing='0' bordercolor='black' rules='cols'>");
        Response.Write("<tr>");
        Response.Write("<th width='20' align='center'>No.</th>");
        Response.Write("<th width='80' align='center'>Article Description</th>");
        Response.Write("<th width='5' align='center'>Qty</th>");
        Response.Write("<th width='15' align='center'>Unit Price</th>");

        if (mode == "RENTAL ORDER" || mode == "rental extension")
        {
            Response.Write("<th width='15' align='center'>Time Duration / Month</th>");
        }

        Response.Write("<th width='10' align='center'>Price</th>");
        Response.Write("<th width='15' align='center'>Discount</th>");
        Response.Write("<th width='15' align='center'>Amount</th>");
        Response.Write("<th width='100' align='center'>Note</th>");

        Response.Write("</tr>");

        if (CountRow > RowMin)
        {
            for (i = 0; i <= CountRow - 1; i++)
            {
                Response.Write("<tr valign='top'>");
                if (j <= CountRow - 1)
                {
                    Response.Write("<td><div align='center'>" + (j + 1).ToString() + "</div></td>");
                    Response.Write("<td><div align='left'>" + article_description[j] + "</div></td>");
                    Response.Write("<td><div align='right'>" + qty[j] + "</div></td>");
                    Response.Write("<td><div align='right'>" + unit_price[j].ToString("##,#0.#0") + "</div></td>");

                    if (mode == "RENTAL ORDER" || mode == "rental extension")
                    {
                        Response.Write("<td><div align='center'>" + time_duration[j] + "</div></td>");
                    }

                    Response.Write("<td><div align='right'>" + price[j].ToString("##,#0.#0") + "</div></td>");
                    Response.Write("<td><div align='right'>" + discount[j].ToString("##,#0.#0") + "</div></td>");
                    Response.Write("<td><div align='right'>" + price_after_disc[j].ToString("##,#0.#0") + "</div></td>");

                    Response.Write("<td><div align='left'>" + notedetail[j] + "</div></td>");

                    //Response.Write("<td><div align='right'>" + tax[j].ToString("##,#0.#0") + "</div></td>");

                    //Response.Write("<td><div align='right'>" + total[j].ToString("##,#0.#0") + "</div></td>");
                }
                else
                {
                    if (mode == "RENTAL ORDER" || mode == "rental extension")
                    {
                        Response.Write("" + cAdih.strRepeat("<td>&nbsp;</td>", 9) + "");
                    }
                    else
                    {
                        Response.Write("" + cAdih.strRepeat("<td>&nbsp;</td>", 7) + "");
                    }
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
                    Response.Write("<td><div align='center'>" + (j + 1).ToString() + "</div></td>");
                    Response.Write("<td><div align='left'>" + article_description[j] + "</div></td>");
                    Response.Write("<td><div align='right'>" + qty[j] + "</div></td>");
                    Response.Write("<td><div align='right'>" + unit_price[j].ToString("##,#0.#0") + "</div></td>");

                    if (mode == "RENTAL ORDER" || mode == "rental extension")
                    {
                        Response.Write("<td><div align='center'>" + time_duration[j] + "</div></td>");
                    }

                    Response.Write("<td><div align='right'>" + price[j].ToString("##,#0.#0") + "</div></td>");
                    Response.Write("<td><div align='right'>" + discount[j].ToString("##,#0.#0") + "</div></td>");
                    Response.Write("<td><div align='right'>" + price_after_disc[j].ToString("##,#0.#0") + "</div></td>");

                    Response.Write("<td><div align='left'>" + notedetail[j] + "</div></td>");

                    //Response.Write("<td><div align='right'>" + tax[j].ToString("##,#0.#0") + "</div></td>");

                    //Response.Write("<td><div align='right'>" + total[j].ToString("##,#0.#0") + "</div></td>");
                }
                else
                {
                    if (mode == "RENTAL ORDER" || mode == "rental extension")
                    {
                        Response.Write("" + cAdih.strRepeat("<td>&nbsp;</td>", 9) + "");
                    }
                    else
                    {
                        Response.Write("" + cAdih.strRepeat("<td>&nbsp;</td>", 7) + "");
                    }
                }
                Response.Write("</tr>");
                j += 1;
            }
        }

        //membuat kolom total/vat/total invoice

        if (CountRow == PageTotal)
        {
            Response.Write("<tr valign='top'>");
            if (mode == "RENTAL ORDER" || mode == "rental extension")
            {
                Response.Write("<td colspan='7' class='T1-B0-L0-R0'><div align='right'>Total&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<div></td>");
            }
            else
            {
                Response.Write("<td colspan='6' class='T1-B0-L0-R0'><div align='right'>Total&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<div></td>");
            }

            Response.Write("<td class='T1-B0-L0-R0'><div align='right'>" + totalinvoice + "</div></td>");
        }
        Response.Write("</tr>");
        Response.Write("<tr>");

        if (mode == "RENTAL ORDER" || mode == "rental extension")
        {
            Response.Write("<td colspan='7' class='T1-B0-L0-R0'><div align='right'>VAT&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<div></td>");
        }
        else
        {
            Response.Write("<td colspan='6' class='T1-B0-L0-R0'><div align='right'>VAT&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<div></td>");
        }
        Response.Write("<td class='T1-B0-L0-R0'><div align='right'>" + ppn + "</div></td>");
        Response.Write("</tr>");
        Response.Write("<tr>");

        if (mode == "RENTAL ORDER" || mode == "rental extension")
        {
            Response.Write("<td colspan='7' class='T1-B0-L0-R0'><div align='right'>Total Invoice&nbsp;<div></td>");
        }
        else
        {
            Response.Write("<td colspan='6' class='T1-B0-L0-R0'><div align='right'>Total Invoice&nbsp;<div></td>");
        }
        Response.Write("<td class='T1-B0-L0-R0'><div align='right'>" + totalBill + "</div></td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("</tr>");
        Response.Write("</table>");
        Response.Write("</td>");
        Response.Write("</tr>");
        Response.Write("<tr height='5'><td></td></tr>");
        //

        //if (mode == "RENTAL ORDER")
        //{
        //    Response.Write("<td colspan='7' class='T1-B0-L0-R0'><div align='right'>Total &nbsp;<div></td>");
        //}
        //else
        //{
        //    Response.Write("<td colspan='6' class='T1-B0-L0-R0'><div align='right'>Total &nbsp;<div></td></td>");

        //}
        //Response.Write("<td class='T1-B0-L0-R0'><div align='right'>" + totalinvoice + "</div></td>");

        //Response.Write("<td class='T1-B0-L0-R0'><div align='right'>" + sum_tax.ToString("##,#0.#0") + "</div></td>");
        //Response.Write("<td class='T1-B0-L0-R0'><div align='right'>" + sum_disc_amount.ToString("##,#0.#0") + "</div></td>");
        //Response.Write("<td class='T1-B0-L0-R0'><div align='right'>" + totalBill + "</div></td>");

        //Response.Write("<tr height='5'><td>Billing Plan</td></tr>");
        //Response.Write("<tr>");
        //Response.Write("<td>");
        //Response.Write("<table border='1' width='100%' cellpadding='2' cellspacing='0' bordercolor='black' rules='cols'>");
        //Response.Write("<tr>");
        //Response.Write("<th width='100' align='center'>Date</th>");
        //Response.Write("<th width='' align='center'>Note</th>");
        //Response.Write("<th width='100' align='center'>Amount</th>");
        //Response.Write("</tr>");

        //sql.Length = 0;
        //sql.Append("SELECT a.billing_date, a.note, a.amount ");
        //sql.Append("FROM rental_order_billing a WITH(READPAST) ");
        //sql.Append("WHERE a.trans_no = '" + Request.QueryString["transNo"].ToString() + "' ");
        //sql.Append("ORDER BY a.billing_date ");

        //cmd = new SqlCommand(sql.ToString(), conn);
        //using (dr = cmd.ExecuteReader())
        //{
        //    if (dr.HasRows)
        //    {
        //        while (dr.Read())
        //        {
        //            Response.Write("<tr valign='top'>");
        //            Response.Write("<td><div align='center'>" + DateTime.Parse(dr["billing_date"].ToString()).ToString("dd MMM yyyy").ToUpper() + "</div></td>");
        //            Response.Write("<td><div align='left'>" + dr["note"].ToString().ToUpper() + "</div></td>");
        //            Response.Write("<td><div align='right'>" + double.Parse(dr["amount"].ToString()).ToString("##,#0.#0") + "</div></td>");
        //            Response.Write("</tr>");
        //        }
        //    }
        //}

        //Response.Write("</table>");
        //Response.Write("</td>");
        //Response.Write("</tr>");

        //Response.Write("<table border='0' width='598px' cellpadding='0' cellspacing='0' bordercolor='black'>");

        //Response.Write("<tr>");
        //Response.Write("<td width='70%'valign='left'></td> <td align='left'>VAT</td><td>:</td><td align='right'>" + ppn + "</td>  ");
        //Response.Write("</tr>");
        //Response.Write("<tr>");
        //Response.Write("<td valign='left'></td> <td>Total Invoice</td><td>:</td><td align='right' width='16%'>" +totalBill+"</td>");
        //Response.Write("</tr>");

        //Response.Write("</table>");

        //

        if (CountRow == PageTotal)
        {
            Response.Write("<table border='0' width='730' cellpadding='1' cellspacing='0' cols='3'>");

            Response.Write("<tr>");
            Response.Write("<td colspan='10'><h3> TERBILANG : " + terbilang + " </h3></td>");
            Response.Write("</tr>");

            Response.Write("<tr><td width='50%'></td><td width='50%'></td></tr>");
            Response.Write("<tr>");
            //Response.Write("<td><div align='LEFT'><b>Pembayaran Kepada:</b></div></td>");
            Response.Write("<td>&nbsp</td>");
            Response.Write("<td><div align='center'>Hormat Kami,</div></td>");
            Response.Write("</tr>");
            //Response.Write("<tr><td width='50%'></td><td width='50%'></td></tr>");
            //Response.Write("<tr height='" + (PageNo == PageTotal ? "40" : "50") + "' valign='bottom'>");

            //if (BUName == "Perkasa Internusa Mandiri PT")
            //{
            //    namarek = "PT. PERKASA INTERNUSA MANDIRI";
            //    rekbca = "689.012.2599";
            //    cabang = "MH. THAMRIN CIKOKOL";
            //}
            //    else if (BUName == "Cahaya Manunggal PT")
            //     {
            //         namarek = "PT. CAHAYA MANUNGGAL";
            //         rekbca = "6890.32.0609";
            //         cabang = "MH. THAMRIN CIKOKOL";
            //     }

            //    else if (BUName == "Delta Sukses Pratama PT")
            //    {
            //        namarek = "PT. DELTA SUKSES PRATAMA";
            //        rekbca = "689.043.0234";
            //        cabang = "MH. THAMRIN CIKOKOL";
            //    }

            //Response.Write("<td><div align='LEFT'><b>"+namarek+"</b> </div></td>");
            //Response.Write("<tr>&nbsp</tr>");
            //Response.Write("<td><div align='LEFT'><b>BCA&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : <b>" + rekbca + " </div></td>");
            //Response.Write("<tr>&nbsp</tr>");
            //Response.Write("<td><div align='LEFT'><b>CABANG : " + cabang + " </div></td>");
            //Response.Write("<td><div align='center'>(" + BUName + ")</div></td>");
            //Response.Write("</tr>");

            Response.Write("<tr>");
            Response.Write("<td>&nbsp</td>");
            Response.Write("<td>&nbsp</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td>&nbsp</td>");
            Response.Write("<td>&nbsp</td>");
            Response.Write("</tr>");
            Response.Write("<tr><td width='50%'></td><td width='50%'></td></tr>");
            Response.Write("<tr height='" + (PageNo == PageTotal ? "40" : "50") + "' valign='bottom'>");

            if (BUName == "Perkasa Internusa Mandiri PT")
            {
                namarek = "PT. PERKASA INTERNUSA MANDIRI";
                rekbca = "689.012.2599";
                cabang = "MH. THAMRIN CIKOKOL";
            }
            else if (BUName == "Cahaya Manunggal PT")
            {
                namarek = "PT. CAHAYA MANUNGGAL";
                rekbca = "6890.32.0609";
                cabang = "MH. THAMRIN CIKOKOL";
            }
            else if (BUName == "Delta Sukses Pratama PT")
            {
                namarek = "PT. DELTA SUKSES PRATAMA";
                rekbca = "689.043.0234";
                cabang = "MH. THAMRIN CIKOKOL";
            }

            Response.Write("<td><div align='LEFT'><b>Pembayaran Kepada:</b></div></td>");
            Response.Write("<tr>&nbsp</tr>");
            Response.Write("<td><div align='LEFT'><b>" + namarek + "</b> </div></td>");
            Response.Write("<tr>&nbsp</tr>");
            Response.Write("<td><div align='LEFT'><b>BCA&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : <b>" + rekbca + " </div></td>");
            Response.Write("<tr>&nbsp</tr>");
            Response.Write("<td><div align='LEFT'><b>CABANG : " + cabang + " </div></td>");
            Response.Write("<td><div align='center'><b>(" + BUName + ")</b></div></td>");
            Response.Write("</tr>");
            Response.Write("</table>");
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("<tr><td align='right' style='font-size:8px;'>&nbsp;</td></tr>");
            Response.Write("</table>");
        }

        if (PageNo != PageTotal)
        {
            Response.Write("<br /><br />");
        }
        conn.Close();
    }
}