using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class wuc_wucConfirmBox : System.Web.UI.UserControl
{
    public event EventHandler Hide;

    public void SubShowConfirmBox(string Message)
    {
        bYes.Enabled = true;
        bYes.Attributes.Add("onclick", " this.disabled = true; " + Page.ClientScript.GetPostBackEventReference(bYes, null) + ";");
        lModalTitle.Text = Message;
        //upModal.Update();
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#mpeConfirm').modal();", true);
    }

    protected void btnYes_Click(Object sender, EventArgs e)
    {
        bYes.Enabled = false;
        MStrMode = "Yes";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#mpeConfirm", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();", true); if (Hide != null)
        {
            Hide(this, e);
        }
    }

    private String MStrMode = "";

    public string StrMode
    {
        get
        {
            return MStrMode;
        }
        set
        {
            MStrMode = value;
        }
    }

    protected void OnHide(EventArgs e)
    {
        if (Hide != null)
        {
            Hide(this, e);
        }
    }   
}