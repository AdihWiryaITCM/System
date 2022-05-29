using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class wuc_wucMessageBox : System.Web.UI.UserControl
{
    public void subShowMsgBox(string message)
    {
        lModalBody.Text = message;
        upModal.Update();
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModalMessage", "$('#myModalMessage').modal();", true);
    }
}