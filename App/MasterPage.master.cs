using System;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["name"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        lblUserName.Text = "Selamat Datang <br>" + Session["name"].ToString();
        lblUser.Text = Session["name"].ToString();
        lblDept.Text = Session["dept"].ToString();
    }

    public void messageBox(string message)
    {
        wucMessageBox1.subShowMsgBox(message);
    }
}