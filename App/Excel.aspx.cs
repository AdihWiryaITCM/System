using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Data.SqlClient;
using System.Web;
using System.IO;

public partial class excel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
            expExcell((DataSet)Session["export_excel"]);
    }

    protected void expExcell(DataSet dtSet)
    {
        DataTable dtaExp=dtSet.Tables[0];
        Response.ClearContent();

        Response.AddHeader("content-disposition", "attachment;filename=" + Session["excel_name"].ToString() + "_" + Session["username"].ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        Response.ContentType = "application/ms-excel";///vnd.ms-excel";
        string tab = string.Empty;
        //string style = "<html><style>.textmode { mso-number-format:\\@;font-size:9.5pt; font-family:Calibri; }</style><table>";
        string style = "<html><style>.textmode { mso-number-format: \\general;font-size:9.5pt; font-family:Calibri; }, .textmode2 { mso-number-format:\\@;font-size:9.5pt; font-family:Calibri; }, .date {mso-number-format: dd\\ MMM\\ yyyy;font-size:9.5pt; font-family:Calibri; }, .int {mso-number-format:0;font-size:9.5pt; font-family:Calibri; }, .float {mso-number-format:0\\.00;font-size:9.5pt; font-family:Calibri; }</style><table>";

        Response.Write(style);
       
        foreach (DataColumn datacol in dtaExp.Columns)
            {
                Response.Write("<th>");
                Response.Write(tab + datacol.ColumnName);
                Response.Write("</th>");
                tab = "\t";
            }
            Response.Write("\n");
            foreach (DataRow dr in dtaExp.Rows)
            {
                tab = "";
                Response.Write("<tr>");
                for (int j = 0; j < dtaExp.Columns.Count; j++)
                {
                    //Response.Write("<td class = textmode>");
                    //Response.Write(tab + Convert.ToString(dr[j]));
                    //Response.Write("</td>");
                    if (dr[j] is DateTime)
                    {
                        Response.Write("<td class = date>");
                        Response.Write(tab + (dr[j]));
                        Response.Write("</td>");
                    }
                    else if (dr[j] is String)
                    {
                        if (dr[j].ToString().StartsWith("0") && dr[j].ToString().Length > 1)
                        {
                            Response.Write("<td class = textmode2>");
                            Response.Write(tab + Convert.ToString(dr[j]));
                            Response.Write("</td>");
                        }
                        else
                        {
                            Response.Write("<td class = textmode>");
                            Response.Write(tab + Convert.ToString(dr[j]));
                            Response.Write("</td>");
                        }
                    }
                    else if (dr[j] is float)
                    {
                        Response.Write("<td class = float>");
                        Response.Write(tab + Convert.ToString(dr[j]));
                        Response.Write("</td>");
                    }
                    else
                    {
                        Response.Write("<td class = int>");
                        Response.Write(tab + Convert.ToString(dr[j]));
                        Response.Write("</td>");
                    }
                }
                Response.Write("</tr>");
                Response.Write("\n");

            }
            Response.Write("</table>");
            Response.Write("</html>");
            Response.Flush();
            Response.End();
            dtaExp.Dispose();
    }

}