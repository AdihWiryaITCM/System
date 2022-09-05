using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
public partial class _Default : System.Web.UI.Page
{
    SqlConnection conn;
    SqlTransaction trans;
    SqlCommand cmd;
    SqlDataReader dr;
    StringBuilder sql = new StringBuilder();
    ClassAdih cAdih = new ClassAdih();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            lbluserName.Text = Session["name"].ToString();
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;

        sql.Length = 0;
        sql.Append("SELECT password AS result FROM [user] WHERE user_id = '" + Session["username"].ToString() + "' ");

        string currPassword = cAdih.getResultString(sql.ToString(), cAdih.getConnStr("Connection"));

        if (txtCurrPassword.Text == "")
        {
            master.messageBox("Input current password!");
            txtCurrPassword.Focus();
            return;
        }
        else if (txtCurrPassword.Text != currPassword)
        {
            master.messageBox("Wrong current password!");
            txtCurrPassword.Focus();
            return;
        }
        else if (txtNewPassword.Text == "")
        {
            master.messageBox("Input new password!");
            txtNewPassword.Focus();
            return;
        }
        else if (txtNewPassword.Text.Length < 6)
        {
            master.messageBox("Password minimal 6 character");
            txtNewPassword.Focus();
            return;
        }
        else if (txtNewPassword.Text.Length > 12)
        {
            master.messageBox("Password maximal 12 character");
            txtNewPassword.Focus();
            return;
        }
        else if (txtNewPassword.Text != txtNewPassword2.Text)
        {
            master.messageBox("Password doesn't match!");
            txtNewPassword2.Focus();
            return;
        }
        else if (txtCurrPassword.Text == txtNewPassword.Text)
        {
            master.messageBox("Password cannot be the same as the previous password!");
            txtNewPassword2.Focus();
            return;
        }

        showConfirmBox("Are you sure you want to change the password? ");
    }

    protected void showConfirmBox(string message)
    {
        wucConfirmBox1.SubShowConfirmBox(message);
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (wucConfirmBox1.StrMode == "Yes")
            {
                if (saveData())
                {
                    master.messageBox("Password changed successfully");
                }
            }
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected bool saveData()
    {
        try
        {
            sql.Length = 0;
            sql.Append("UPDATE [user] SET password = '" + txtNewPassword.Text + "' ");
            sql.Append("WHERE [user_id] = '" + Session["username"].ToString() + "' ");

            cAdih.executeNonQuery(sql.ToString(), cAdih.getConnStr("Connection"));

            return true;
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
            return false;
        }
    }
}