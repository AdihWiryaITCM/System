using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

public partial class Login : System.Web.UI.Page
{
    SqlConnection conn;
    SqlCommand cmd;
    SqlDataReader dr;
    StringBuilder sql = new StringBuilder();
    StringBuilder sql1 = new StringBuilder();
    private string connName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        connName = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
    }

    protected void bLogin_click(object sender, EventArgs e)
    {
        try
        {
            if (tbUser.Text.Trim().Length == 0)
            {
                lError.Text = "Tolong Masukan Username";
                pError.Visible = true;
                pLogin.Update();
                return;
            }
            else if (tbPassword.Text.Trim().Length == 0)
            {
                lError.Text = "Tolong Masukan Password";
                pError.Visible = true;
                pLogin.Update();
            }
            else
            {
                sql.Append("Select user_id as username,username as name,email,no_telp,description,dept FROM [user]" +
                    "inner join access_department on dept = access_code   " +
                    "WHERE user_id = '" + tbUser.Text + "' " +
                    "and   password = '" + tbPassword.Text + "' ");
                using (conn = new SqlConnection(connName))
                {
                    conn.Open();
                    using (cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        using (dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                Session["username"] = dr["username"].ToString();
                                Session["name"] = dr["name"].ToString();
                                Session["email"] = dr["email"].ToString();
                                Session["no_telp"] = dr["no_telp"].ToString();
                                Session["dept_code"] = dr["dept"].ToString();
                                Session["dept"] = dr["description"].ToString();
                                Response.Redirect("Default.aspx");
                            }
                            else
                            {
                                lError.Text = "Username atau Password Salah";
                                pError.Visible = true;
                                pLogin.Update();
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
}