using System;
using System.Data.SqlClient;
using System.Text;

public partial class Transaction_PurchaseRequisitionPrint : System.Web.UI.Page
{
    SqlConnection conn;
    SqlCommand cmd;
    SqlDataReader dr;
    StringBuilder sql = new StringBuilder();
    classKres cKres = new classKres();

    protected void Page_Load(object sender, EventArgs e)
    {
        string FormTitle = "PURCHASE REQUISITION";
        int RowMax = 10;
        string BUName = "";
        string BUAddress = "";
        string TransNo = "";
        string TransDate = "";
        string ShipTo = "";
        string ShipToSite = "";
        string ShipToAddress = "";
        string automatic_order = "";
        string Note = "";
        string CreatedBy = "";
        string createdDate = "";
        string PostedBy = "";
        string postedDate = "";
        string Closedreason = "";
        string Cancelreason = "";
        string closedBy = "";
        string closedDate = "";
        int i = 1;
        int j = 0;
        double sum_qty = 0;
        double sum_amount = 0;

        sql.Length = 0;
        sql.Append("SELECT COUNT(*) FROM purchase_requisition_detail WITH(READPAST) WHERE trans_no = '" + Request.QueryString["transNo"].ToString() + "' ");

        conn = new SqlConnection(cKres.getConnStr("eRental"));
        conn.Open();

        cmd = new SqlCommand(sql.ToString(), conn);
        int CountRow = int.Parse(cmd.ExecuteScalar().ToString());

        double PageTotal = Math.Ceiling(Convert.ToDouble(CountRow) / Convert.ToDouble(RowMax));

        string[] article = new string[CountRow];
        string[] unit = new string[CountRow];
        double[] qty = new double[CountRow];
        double[] unit_price = new double[CountRow];
        string[] required_date = new string[CountRow];
        double[] total = new double[CountRow];
        string[] note = new string[CountRow];

        sql.Length = 0;
        sql.Append("SELECT b.company_name, a.site_id + ' - ' + c.site_name AS site, a.trans_no, a.trans_date, a.note, ");
        sql.Append("coalesce(d.site_name, e.customer_name, v.vendor_name) AS ship_to_name, ISNULL(f.alias_name_full + '(' + f.alias_name + ')', '') AS ship_to_site_name, ISNULL(f.street_address, '') AS street_address,  ");
        sql.Append("a.automatic_order, a.status, ISNULL(j.user_name, '') AS created_by, a.created_date, ISNULL(k.user_name, '') AS posted_by, a.posted_date, ");
        sql.Append("isnull(prc1.cancelled_reason,'') as cancel_reason, isnull(prc1.cancelled_date,'') as cancel_date, isnull(l.user_name,'') as cancel_by, ");
        sql.Append("isnull(prc2.closed_reason,'') as closed_reason, isnull(prc2.closed_date,'') as closed_date, isnull(m.user_name,'') as closed_by ");
        sql.Append("FROM purchase_requisition a WITH(READPAST) ");
        sql.Append("INNER JOIN company b WITH(READPAST) ON a.company_id = b.company_id ");
        sql.Append("INNER JOIN site c WITH(READPAST) ON a.site_id = c.site_id ");
        sql.Append("LEFT JOIN site d WITH(READPAST) ON a.ship_to = d.site_id ");
        sql.Append("LEFT JOIN customer e WITH(READPAST) ON a.ship_to = e.customer_no ");
        sql.Append("LEFT JOIN customer_address f WITH(READPAST) ON a.ship_to = f.customer_no AND f.address_type = '02' AND a.ship_to_site = f.id ");
        sql.Append("LEFT JOIN [user] j WITH(READPAST) ON a.created_by = j.user_id ");
        sql.Append("LEFT JOIN [user] k WITH(READPAST) ON a.posted_by = k.user_id ");
        sql.Append("LEFT JOIN vendor v WITH(READPAST) ON a.ship_to = v.vendor_no ");
        sql.Append("LEFT JOIN purchase_requisition_cancelled prc1 on prc1.trans_no = a.trans_no  ");
        sql.Append("LEFT JOIN purchase_requisition_closed prc2 on prc2.trans_no = a.trans_no  ");
        sql.Append("LEFT JOIN [user] l WITH(READPAST) ON prc1.cancelled_by = l.user_id ");
        sql.Append("LEFT JOIN [user] m WITH(READPAST) ON prc2.closed_by = m.user_id ");
        sql.Append("WHERE a.trans_no = '" + Request.QueryString["transNo"].ToString() + "' ");

        cmd = new SqlCommand(sql.ToString(), conn);
        using (dr = cmd.ExecuteReader())
        {
            if (dr.HasRows)
            {
                dr.Read();
                BUName = dr["company_name"].ToString();
                TransNo = dr["trans_no"].ToString();
                TransDate = DateTime.Parse(dr["trans_date"].ToString()).ToString("dd MMM yyyy");
                ShipTo = dr["ship_to_name"].ToString();
                ShipToSite = dr["ship_to_site_name"].ToString();
                ShipToAddress = dr["street_address"].ToString();
                automatic_order = dr["automatic_order"].ToString();
                Note = dr["note"].ToString();
                CreatedBy = dr["created_by"].ToString();
                createdDate = DateTime.Parse(dr["created_date"].ToString()).ToString("dd MMM yyyy");
                PostedBy = dr["posted_by"].ToString();
                postedDate = DateTime.Parse(dr["posted_date"].ToString()).ToString("dd MMM yyyy");
                if(dr["closed_reason"].ToString() != "")
                {
                    Closedreason = dr["closed_reason"].ToString();
                    closedBy = dr["closed_by"].ToString();
                    closedDate = DateTime.Parse(dr["closed_date"].ToString()).ToString("dd MMM yyyy");
                }
                else if(dr["cancel_reason"].ToString() != "")
                {
                    Cancelreason = dr["cancel_reason"].ToString();
                    closedBy = dr["cancel_by"].ToString();
                    closedDate = DateTime.Parse(dr["cancel_date"].ToString()).ToString("dd MMM yyyy");
                }

            }
        }

        sql.Length = 0;
        sql.Append("SELECT ROW_NUMBER() OVER (ORDER BY a.line_no ASC, a.line_no ASC) AS row_no, ");
        sql.Append("b.article_description, c.id AS unit_name, a.qty, a.info_price, a.required_date,cast(conten as varchar)+' '+u.description as [content], (a.qty * a.info_price) AS total, a.note ");
        sql.Append("FROM purchase_requisition_detail a WITH(READPAST) ");
        sql.Append("INNER JOIN article b WITH(READPAST) ON a.article_no = b.article_number ");
        sql.Append("INNER JOIN inner_pack c WITH(READPAST) ON a.unit_id = c.id ");
        sql.Append("INNER JOIN uom u WITH(READPAST) ON c.uom_id = u.id ");
        sql.Append("WHERE a.trans_no = '" + Request.QueryString["transNo"].ToString() + "' ");

        cmd = new SqlCommand(sql.ToString(), conn);
        using (dr = cmd.ExecuteReader())
        {
            if (dr.HasRows)
            {
                i = 0;
                while (dr.Read())
                {
                    article[i] = dr["article_description"].ToString();
                    unit[i] = dr["unit_name"].ToString();
                    qty[i] = double.Parse(dr["qty"].ToString());
                    unit_price[i] = double.Parse(dr["info_price"].ToString());
                    required_date[i] = DateTime.Parse(dr["required_date"].ToString()).ToString("dd MMM yyyy");
                    total[i] = double.Parse(dr["total"].ToString());
                    note[i] = dr["note"].ToString();
                    sum_qty += double.Parse(dr["qty"].ToString());
                    sum_amount += double.Parse(dr["total"].ToString());
                    i++;
                }
            }
        }

        conn.Close();

        for (int PageNo = 1; PageNo <= Convert.ToInt16(PageTotal); PageNo++)
        {
            Response.Write("<table border='0' width='730px' cellpadding='2' cellspacing='0' bordercolor='black'>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("<table border='0' width='100%' cellpadding='0' cellspacing='0' bordercolor='black'>");
            Response.Write("<tr>");
            Response.Write("<td width='520'></td><td width='70'></td><td width='10'></td><td width='140'></td>");
            Response.Write("</tr>");
            Response.Write("<tr><td align='left' style='font-size:8px;'>Page " + PageNo.ToString() + "/" + PageTotal.ToString() + "</td></tr>");
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
            Response.Write("<td width='100'></td><td width='10'></td><td width=''></td>");
            Response.Write("</tr>");
            if (ShipToSite == "")
            {
                Response.Write("<tr>");
                Response.Write("<td>Ship To</td><td>:</td><td>" + ShipTo + "</td>");
                Response.Write("</tr>");
            }
            else {
                Response.Write("<tr>");
                Response.Write("<td>Ship To</td><td>:</td><td>" + ShipTo + " / " + ShipToSite + "</td>");
                Response.Write("</tr>");
                Response.Write("<tr>");
                Response.Write("<td>&nbsp;</td><td>&nbsp;</td><td>" + ShipToAddress + "</td>");
                Response.Write("</tr>");
            }
            Response.Write("<tr>");
            Response.Write("<td>Note</td><td>:</td><td>" + Note + "</td>");
            Response.Write("</tr>");
            if (Closedreason != "")
            {
                Response.Write("<tr>");
                Response.Write("<td>Closed Reason</td><td>:</td><td>" + Closedreason + "</td>");
                Response.Write("</tr>");
            }
            if (Cancelreason != "")
            {
                Response.Write("<tr>");
                Response.Write("<td>Cancel Reason</td><td>:</td><td>" + Cancelreason + "</td>");
                Response.Write("</tr>");
            }
            Response.Write("</table>");
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("<tr height='2'><td></td></tr>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("<table border='1' width='100%' cellpadding='2' cellspacing='0' bordercolor='black' rules='cols'>");
            Response.Write("<tr>");
            Response.Write("<th width='20' align='center'>No.</th>");
            Response.Write("<th width='150' align='center'>Article</th>");
            Response.Write("<th width='30' align='center'>Unit</th>");
            Response.Write("<th width='30' align='center'>Qty</th>");
            Response.Write("<th width= 60' align='center'>Info Price</th>");
            Response.Write("<th width='70' align='center'>Req Date</th>");
            Response.Write("<th width='60' align='center'>Total Amount</th>");
            Response.Write("<th width='140' align='center'>Note</th>");
            Response.Write("</tr>");
            for (i = 0; i <= RowMax - 1; i++)
            {
                Response.Write("<tr valign='top'>");
                if (j <= CountRow - 1)
                {
                    Response.Write("<td><div align='right'>" + (j + 1).ToString() + "</div></td>");
                    Response.Write("<td><div align='left'>" + article[j] + "</div></td>");
                    Response.Write("<td><div align='center'>" + unit[j] + "</div></td>");
                    Response.Write("<td><div align='right'>" + qty[j].ToString("#,#0.#0") + "</div></td>");
                    Response.Write("<td><div align='right'>" + unit_price[j].ToString("#,#0.#0") + "</div></td>");
                    Response.Write("<td><div align='center'>" + required_date[j] + "</div></td>");
                    Response.Write("<td><div align='right'>" + total[j].ToString("#,#0.#0") + "</div></td>");
                    Response.Write("<td><div align='left'>" + note[j] + "</div></td>");
                }
                else
                {
                    Response.Write("" + cKres.strRepeat("<td>&nbsp;</td>", 8) + "");
                }
                Response.Write("</tr>");
                j += 1;
            }
            if (PageNo == PageTotal)
            {
                Response.Write("<tr valign='top'>");
                Response.Write("<td colspan='3' class='T1-B0-L0-R0'><div align='right'>Total&nbsp;<div></td>");
                Response.Write("<td class='T1-B0-L0-R0'><div align='right'>" + sum_qty.ToString("#,#0.#0") + "</div></td>");
                Response.Write("<td class='T1-B0-L0-R0'><div align='right'>&nbsp;</div></td>");
                Response.Write("<td class='T1-B0-L0-R0'><div align='right'>&nbsp;</div></td>");

                Response.Write("<td class='T1-B0-L0-R0'><div align='right'>" + sum_amount.ToString("#,#0.#0") + "</div></td>");
                Response.Write("<td class='T1-B0-L0-R0'><div align='right'>&nbsp;</div></td>");
            }
            Response.Write("</tr>");
            Response.Write("</table>");
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("<table border='0' width='730' cellpadding='2' cellspacing='0' cols='3'>");
            if (Closedreason != "" || Cancelreason != "")
            {
                Response.Write("<tr><td width='33%'></td><td width='33%'></td><td width='33%'></td></tr>");
            }
            else
            {
                Response.Write("<tr><td width='50%'></td><td width='50%'></td>/tr>");
            }
            Response.Write("<tr>");
            Response.Write("<td><div align='center'>Created by,</div></td>");
            Response.Write("<td><div align='center'>Posted by,</div></td>");
            if (Closedreason != "")
            {
                Response.Write("<td><div align='center'>Closed by,</div></td>");
            }
            if (Cancelreason != "")
            {
                Response.Write("<td><div align='center'>Cancelled by,</div></td>");
            }
            Response.Write("</tr>");
            if (Closedreason != "" || Cancelreason != "")
            {
                Response.Write("<tr><td width='33%'></td><td width='33%'></td><td width='33%'></td></tr>");
            }
            else
            {
                Response.Write("<tr><td width='50%'></td><td width='50%'></td>/tr>");
            }
            Response.Write("<tr>");
            Response.Write("<td><div align='center'>" + createdDate + "</div></td>");
            Response.Write("<td><div align='center'>" + (postedDate == "01-01-1900" ? "&nbsp;" : postedDate) + "</div></td>");
            if (Closedreason != "" || Cancelreason != "")
            {
                Response.Write("<td><div align='center'>" + (closedDate == "01-01-1900" ? "&nbsp;" : closedDate) + "</div></td>");
            }
            Response.Write("</tr>");
            Response.Write("<tr height='" + (PageNo == PageTotal ? "40" : "50") + "' valign='bottom'>");
            Response.Write("<td><div align='center'>(" + CreatedBy + ")</div></td>");
            Response.Write("<td><div align='center'>(" + PostedBy + ")</div></td>");
            if (Closedreason != "" || Cancelreason != "")
            {
                Response.Write("<td><div align='center'>(" + closedBy + ")</div></td>");
            }
            Response.Write("</tr>");
            Response.Write("</table>");
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("</table>");
            if (PageNo != PageTotal)
            {
                Response.Write("<br /><br />");
            }
        }
    }
}