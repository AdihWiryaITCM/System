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

        if(Session["dept_code"].ToString() == "A01")
        {
            Master.Visible = true;
            Customer.Visible = true;
            CustomerAddress.Visible = true;
            ArticleMaster.Visible = true;
            Transaction.Visible = true;
            RentalQuotation.Visible = true;
            RentalOrder.Visible = true;
            PurchaseRequistiion.Visible = true;
            Report.Visible = true;
            RentalOrder.Visible = true;
            StockCard.Visible = true;
            ArticleSerialNo.Visible = true;
        }
        else if (Session["dept_code"].ToString() == "A02")
        {
            Master.Visible = true;
            Vendor.Visible = true;
            Transaction.Visible = true;
            PurchcaseOrder.Visible = true;
            Report.Visible = true;
            PurchaseRequisitionMonitoring.Visible = true;
            PurchaseOrderMonitoring.Visible = true;
            StockCard.Visible = true;
        }
        else if (Session["dept_code"].ToString() == "A03")
        {
            Transaction.Visible = true;
            InboundDelivery.Visible = true;
            Report.Visible = true;
            ArticleSerialNo.Visible = true;
            PurchaseOrderMonitoring.Visible = true;
            RentalOrderMonitoring.Visible = true;
            StockCard.Visible = true;
        }
        else if (Session["dept_code"].ToString() == "A04")
        {
            Transaction.Visible = true;
            OutboundDelivery.Visible = true;
            Report.Visible = true;
            ArticleSerialNo.Visible = true;
            PurchaseOrderMonitoring.Visible = true;
            RentalOrderMonitoring.Visible = true;
            StockCard.Visible = true;
        }
        else if (Session["dept_code"].ToString() == "A05")
        {
            Transaction.Visible = true;
            PurchaseInvoice.Visible = true;
            Report.Visible = true;
            PurchaseInvoiceMonitoring.Visible = true;
        }
        else if (Session["dept_code"].ToString() == "A06")
        {
            Transaction.Visible = true;
            PurchasePayment.Visible = true;
            Report.Visible = true;
            PurchaseInvoiceMonitoring.Visible = true;
        }
        else if (Session["dept_code"].ToString() == "A07")
        {
            Transaction.Visible = true;
            SalesInvoice.Visible = true;
            Report.Visible = true;
            SalesInvoiceMonitoring.Visible = true;
            RentalOrderBillingPlan.Visible = true;
        }
        else if (Session["dept_code"].ToString() == "A08")
        {
            Transaction.Visible = true;
            CustomerReceipt.Visible = true;
            Report.Visible = true;
            SalesInvoiceMonitoring.Visible = true;
        }
    }

    public void messageBox(string message)
    {
        wucMessageBox1.subShowMsgBox(message);
    }
}