using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Text;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections;
using System.Configuration;


/// <summary>
/// Summary description for ClassAdih
/// </summary>
public class ClassAdih
{
    SqlConnection conn;
    SqlCommand cmd;
    SqlDataReader dr;
    SqlDataAdapter da;
    DataTable dt;

    public string strConnStr;

    public ClassAdih()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string getConnStr(string connStr)
    {
        strConnStr = System.Configuration.ConfigurationManager.ConnectionStrings[connStr].ConnectionString;
        return strConnStr;
    }

    public string encrypt(string inp)
    {
        MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();
        byte[] tBytes = Encoding.ASCII.GetBytes(inp);
        byte[] hBytes = hasher.ComputeHash(tBytes);
        StringBuilder sb = new StringBuilder();
        for (int c = 0; c < hBytes.Length; c++)
            sb.AppendFormat("{0:x2}", hBytes[c]);
        return (sb.ToString());
    }

    public string left(string param, int length)
    {
        string result = param.Substring(0, length);
        return result;
    }

    public static string modText(string strText)
    {
        return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(strText.ToLower());
    }

    public string right(string param, int length)
    {
        string result = param.Substring(param.Length - length, length);
        return result;
    }

    public string mid(string param, int startIndex, int length)
    {
        string result = param.Substring(startIndex, length);
        return result;
    }

    public string getResultString(string sql, string connStr)
    {
        string result = "";

        using (conn = new SqlConnection(connStr))
        {
            conn.Open();
            using (cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                using (dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        result = dr["result"].ToString();
                    }
                    else
                    {
                        result = "";
                    }
                }
            }

        }

        return result;
    }

    public double getResultDouble(string sql, string connStr)
    {
        double result = 0;

        using (conn = new SqlConnection(connStr))
        {
            conn.Open();
            using (cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                using (dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        result = double.Parse(dr["result"].ToString());
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }

        }

        return result;
    }
    public string getTaxRate(string strTipe, string strID)
    {
        string result = "";

        if (strTipe == "VENDOR")
        {
            result = "SELECT coalesce(stax_rate,0) as result FROM vendor WITH(READPAST) LEFT JOIN stg_tax WITH(READPAST) ON(tax_id=stax_id) WHERE vendor_no='" + strID + "';";
        }
        else if (strTipe == "CUST")
        {
            result = "SELECT coalesce(stax_rate,0) as result FROM customer as c WITH(READPAST) LEFT JOIN stg_tax as t  WITH(READPAST) ON(c.stax_id=t.stax_id) WHERE customer_no='" + strID + "';";
        }
        else if (strTipe == "COMPANY")
        {
            result = "SELECT coalesce(vat,0) as result FROM company as c WITH(READPAST) WHERE company_id='" + strID + "';";
        }
        else if (strTipe == "SALES")
        {
            result = "SELECT [dbo].[fn_sales_invoice_tax_rate] ('" + strID + "') as result ";
        }
        return result;
    }
    public bool getResultHasRows(string sql, string connStr)
    {
        bool result = false;

        using (conn = new SqlConnection(connStr))
        {
            conn.Open();
            using (cmd = new SqlCommand(sql, conn))
            {
                using (dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }

        }

        return result;
    }

    public DateTime getResultDateTime(string sql, string connStr)
    {
        DateTime result = DateTime.Now;

        using (conn = new SqlConnection(connStr))
        {
            conn.Open();
            using (cmd = new SqlCommand(sql, conn))
            {
                using (dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        result = DateTime.Parse(dr["result"].ToString());
                    }
                    else
                    {
                        DateTime.Now.ToString();
                    }
                }
            }

        }

        return result;
    }

    public DataSet getResultDataSet(string sql, string connstr)
    {
        DataSet ds = null; ;

        using (conn = new SqlConnection(connstr))
        {
            conn.Open();
            using (cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                using (da = new SqlDataAdapter(cmd))
                {

                    ds = new DataSet();
                    da.Fill(ds);
                }
            }

        }
        return ds;
    }

    public void loadDdl(string sql, string connStr, DropDownList ddl)
    {
        ddl.Items.Clear();
        using (conn = new SqlConnection(connStr))
        {
            conn.Open();
            using (cmd = new SqlCommand(sql, conn))
            {
                using (dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ListItem ls = new ListItem();
                            ls.Value = dr["value"].ToString();
                            ls.Text = dr["text"].ToString();
                            ddl.Items.Add(ls);
                        }
                    }
                }

            }
        }
    }

    public void loadRdl(string sql, string connStr, RadioButtonList ddl)
    {
        ddl.Items.Clear();
        using (conn = new SqlConnection(connStr))
        {
            conn.Open();
            using (cmd = new SqlCommand(sql, conn))
            {
                using (dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ListItem ls = new ListItem();
                            ls.Value = dr["value"].ToString();
                            ls.Text = dr["text"].ToString();
                            ddl.Items.Add(ls);
                        }
                    }
                }

            }
        }
    }

    public void loadGridView(string sql, string connStr, GridView grid)
    {
        using (conn = new SqlConnection(connStr))
        {
            conn.Open();
            cmd = new SqlCommand(sql, conn);
            cmd.CommandTimeout = 0;
            using (da = new SqlDataAdapter(cmd))
            {
                dt = new DataTable();
                da.Fill(dt);
                grid.DataSource = dt;
                grid.DataBind();
            }
        }
    }

    public void executeNonQuery(string sql, string connStr)
    {
        using (conn = new SqlConnection(connStr))
        {
            conn.Open();
            using (cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
        }
    }

    public string engFormatDate(string transdate)
    {
        String[] LblStartDate = transdate.Split('-');
        String StartDate = LblStartDate[2] + "-" + LblStartDate[1] + "-" + LblStartDate[0];
        return StartDate;
    }

    public string getErrMessage(string errMessage)
    {
        string result = "";
        if (errMessage == "Timeout expired.  The timeout period elapsed prior to completion of the operation or the server is not responding.")
        {
            result = "Server sedang sibuk, coba lagi!";
        }
        else if (errMessage == "A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: Named Pipes Provider, error: 40 - Could not open a connection to SQL Server)")
        {
            result = "Koneksi putus, hubungi IT headoffice!";
        }
        else
        {
            result = "Error: " + errMessage;
        }
        return result;
    }

    public string strRepeat(string text, int times)
    {
        return new String(' ', times).Replace(" ", text);
    }

    public bool isNumber(string number)
    {
        try
        {
            double result = double.Parse(number);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public DataTable GetDataTable(string sql, string connStr)
    {
        using (conn = new SqlConnection(connStr))
        {
            conn.Open();
            cmd = new SqlCommand(sql, conn);
            cmd.CommandTimeout = 0;
            using (da = new SqlDataAdapter(cmd))
            {
                dt = new DataTable();
                da.Fill(dt);
            }
        }
        return dt;
    }

    private string terbilang(int angka)
    {
        string strterbilang = "";
        string[] a = { "", "satu", "dua", "tiga", "empat", "lima", "enam", "tujuh", "delapan", "sembilan", "sepuluh", "sebelas" };

        if (angka < 12)
        {
            strterbilang = " " + a[angka];
        }
        else if (angka < 20)
        {
            strterbilang = this.terbilang(angka - 10) + " belas";
        }
        else if (angka < 100)
        {
            strterbilang = this.terbilang(angka / 10) + " puluh" + this.terbilang(angka % 10);
        }
        else if (angka < 200)
        {
            strterbilang = " seratus" + this.terbilang(angka - 100);
        }
        else if (angka < 1000)
        {
            strterbilang = this.terbilang(angka / 100) + " ratus" + this.terbilang(angka % 10);
        }
        else if (angka < 2000)
        {
            strterbilang = " seribu" + this.terbilang(angka - 1000);
        }
        else if (angka < 1000000)
        {
            strterbilang = this.terbilang(angka / 1000) + " ribu" + this.terbilang(angka % 1000);
        }
        else if (angka < 1000000000)
        {
            strterbilang = this.terbilang(angka / 1000000) + " juta" + this.terbilang(angka % 1000000);
        }

        strterbilang = System.Text.RegularExpressions.Regex.Replace(strterbilang, @"^\s+|\s+$", " ");
        return strterbilang;
    }

    //////////////////////////////////////////////////////////////

    public string getSourceType(string articleType)
    {
        if (articleType == "ZSER")
        {
            return "Service";
        }
        else
        {
            return "Item";
        }
    }

    public string getIPAddress()
    {
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (!string.IsNullOrEmpty(ipAddress))
        {
            string[] addresses = ipAddress.Split(',');
            if (addresses.Length != 0)
            {
                return addresses[0];
            }
        }

        return context.Request.ServerVariables["REMOTE_ADDR"];
    }

    public string generateJSON(List<Dictionary<string, object>> list)
    {
        string result = "";

        JavaScriptSerializer j = new JavaScriptSerializer();

        result = j.Serialize(list.ToArray());
        result = "{\"result\":" + result + "}";

        return result;
    }

    public SqlCommand addParameter(SqlCommand cmd, string param, System.Web.HttpRequest Request, SqlDbType datatype)
    {
        string value;

        if (Request.QueryString[param] == null)
        {
            value = "";
        }
        else
        {
            value = Request.QueryString[param].Replace("*plus*", "+").Replace("*and*", "&");
        }

        cmd.Parameters.Add(param, datatype).Value = value;

        return cmd;
    }

    public List<Dictionary<string, object>> createList(SqlDataReader dr, SqlCommand cmd, bool withLoop = true)
    {
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        list = new List<Dictionary<string, object>>();

        if (withLoop)
        {
            while (dr.Read())
            {
                Dictionary<string, object> d = new Dictionary<string, object>(dr.FieldCount);
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    d[dr.GetName(i)] = dr.GetValue(i);
                }
                list.Add(d);
            }
        }
        else
        {
            Dictionary<string, object> d = new Dictionary<string, object>(dr.FieldCount);
            for (int i = 0; i < dr.FieldCount; i++)
            {
                d[dr.GetName(i)] = dr.GetValue(i);
            }
            list.Add(d);
        }
        return list;
    }
}