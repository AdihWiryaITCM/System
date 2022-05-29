using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for Shared
/// </summary>
public class Shared
{

    SqlConnection conn;
    SqlCommand cmd;
    SqlDataReader dr;
    SqlDataAdapter da;
    DataTable dt;
    public string strConnStr;

    public Shared()
    {
        //
        // TODO: Add constructor logic here
        //
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

    public static string UItoDBchb(CheckBox chb)
    {
        if (chb.Checked == true)
            return "1";
        else
            return "0";
    }

    public static string UItoDBDate(string date)
    {
        try
        {
            String[] LblStartDate = date.Split('-');
            String StartDate = LblStartDate[2] + "-" + LblStartDate[1] + "-" + LblStartDate[0];
            return StartDate;
        }
        catch
        {
            return "1990-01-01";
        }
    }

    public static string DBtoUIDate(string date)
    {
        try
        {
            if (DateTime.Parse(date).ToString("dd-MM-yyyy") == "01-01-1990")
                return "";
            else
                return DateTime.Parse(date).ToString("dd-MM-yyyy");
        }
        catch
        {
            return "";
        }
    }
}