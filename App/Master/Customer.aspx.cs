using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;


public partial class Master_Customer : Page
{
    SqlTransaction trans;
    StringBuilder sql = new StringBuilder();
    StringBuilder sql1 = new StringBuilder();
    SqlConnection conn;
    SqlConnection conn1;
    SqlCommand cmd;
    SqlCommand cmd1;
    SqlTransaction trans1;
    SqlDataReader dr;
    ClassAdih cAdih = new ClassAdih();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                Session["AliasCode"] = "NotChange";
            }

        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void ibSearchCustomerNo_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            Session["search"] = "customer";
            wucSearch1.loadGrid();
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void bSearchCustomerNo_Click(object sender, EventArgs e) 
    {
        MasterPage master = (MasterPage)this.Master;
        try 
        {
            sql.Length = 0;
            sql.Append("SELECT * FROM customer c WITH(READPAST) ");
            sql.Append("WHERE c.customer_no = '" + tbCustomerID.Text + "' ");

            using (conn = new SqlConnection(cAdih.getConnStr("Connection"))) 
            {
                conn.Open();
                using (cmd = new SqlCommand(sql.ToString(), conn)) 
                {
                    using (dr = cmd.ExecuteReader()) 
                    {
                        if (dr.HasRows) 
                        {
                            dr.Read();
                            tbCustomerName.Text = dr["customer_name"].ToString();

                            tbPhoneNo1.Text = dr["phone_no"].ToString();
                            tbEmail.Text = dr["email"].ToString();
                            tbNamaNPWP.Text = dr["npwp_name"].ToString();
                            tbVatRegNo.Text = dr["vat_reg_no"].ToString();
                            tbTaxAddr.Text = dr["street_address"].ToString();
                            ddlStatus.SelectedValue = dr["status"].ToString();
                            upHeader.Update();
                        }
                    }
                }
            }
        }
        catch (Exception ex) 
        {    
            master.messageBox(ex.Message);
        }
    }

    protected void showConfirmBox(string message)
    {
        wucConfirmBox1.SubShowConfirmBox(message);
    }

    protected void imgDelete_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;

        GridViewRow gridRow = ((sender as ImageButton).Parent.Parent as GridViewRow);
        Label lblVendorID = (Label)gridRow.FindControl("lblVendorID");
        Label lblVendorName = (Label)gridRow.FindControl("lblVendorName");

        sql.Length = 0;
        sql.Append("SELECT TOP 1 vendor_id AS result ");
        sql.Append("FROM store_vendor ");
        sql.Append("WHERE vendor_id = '" + lblVendorID.Text + "' ");
        sql.Append("UNION ");
        sql.Append("SELECT TOP 1 vendor_id AS result ");
        sql.Append("FROM service_call ");
        sql.Append("WHERE vendor_id = '" + lblVendorID.Text + "' ");

        if (cAdih.getResultHasRows(sql.ToString(), cAdih.getConnStr("Connection")))
        {
            master.messageBox("Vendor " + lblVendorID.Text + " has been used. Cannot delete.");
            return;
        }

        Session["MasterVendorMode"] = "Delete";
        Session["MasterVendorID"] = lblVendorID.Text;

        showConfirmBox("Delete " + lblVendorID.Text + " - " + lblVendorName.Text + "?");
    }

    protected void lbSaveHeader_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (tbCustomerName.Text.Trim().Replace("'", "`") == "")
            {
                master.messageBox("Input Customer Name");
                return;
            }
            Session["MasterCustomerMode"] = "SaveHeader";

            showConfirmBox("Save Data?");
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void lbClearHeader_Click(object sender, EventArgs e)
    {
        tbCustomerID.Text = "";
        clearHeader();
    }
    protected bool saveData()
    {
        try
        {
            if (Session["MasterCustomerMode"].ToString() == "SaveHeader")
            {
                conn = new SqlConnection(cAdih.getConnStr("Connection"));
                conn.Open();
                trans = conn.BeginTransaction();

                if (tbCustomerID.Text == "")
                {
                    sql.Length = 0;
                    sql.Append("SELECT RIGHT('000000' + CAST(ISNULL(MAX(CAST(RIGHT(customer_no, 6) AS INT)), 0) + 1 AS VARCHAR), 6) AS running_no  ");
                    sql.Append("FROM customer WITH(READPAST) ");

                    using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                    {
                        using (dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                tbCustomerID.Text = "C" + dr["running_no"].ToString();
                                upHeader.Update();
                            }
                        }
                    }

                    sql.Length = 0;
                    sql.Append("INSERT INTO customer (customer_no,customer_name,currency_id, ");
                    sql.Append("phone_no,email, ");
                    sql.Append("npwp_name, vat_reg_no,street_address,status ) VALUES(");
                    sql.Append("'" + tbCustomerID.Text + "', ");
                    sql.Append("'" + tbCustomerName.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'IDR', ");
                    sql.Append("'" + tbPhoneNo1.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + tbEmail.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + tbNamaNPWP.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + tbVatRegNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + tbTaxAddr.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("'" + ddlStatus.SelectedValue + "') ");

                    cAdih.executeNonQuery(sql.ToString(), cAdih.getConnStr("Connection"));

                }
                else if (tbCustomerID.Text != "")
                {
                    sql.Length = 0;                   
                    sql.Append("UPDATE customer SET customer_name = '" + tbCustomerName.Text.Replace("'", "`").ToUpper() + "', ");
                    sql.Append("phone_no = '" + tbPhoneNo1.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("email = '" + tbEmail.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("npwp_name = '" + tbNamaNPWP.Text.Replace("'", "`").ToUpper() + "', ");
                    sql.Append("vat_reg_no = '" + tbVatRegNo.Text.Replace("'", "`").ToUpper() + "', ");
                    sql.Append("street_address='" + tbTaxAddr.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                    sql.Append("status = '" + ddlStatus.SelectedValue + "' ");
                    sql.Append("WHERE customer_no = '" + tbCustomerID.Text + "' ");                    

                    cAdih.executeNonQuery(sql.ToString(), cAdih.getConnStr("Connection"));
                }

                
            }
            return true;
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
            return false;
        }
    }
    protected void wucSearch_Hide(object sender, EventArgs e)
    {
        if (Session["search"].ToString() == "customer")
        {
            tbCustomerID.Text = wucSearch1.result;
            bSearchCustomerNo_Click(sender, e);
            upHeader.Update();
        }
        Session["search"] = null;
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
                    master.messageBox("Data has been saved!");
                }
            }
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void clearHeader()
    {
        tbCustomerName.Text = "";
        upHeader.Update();

        tbPhoneNo1.Text = "";
        tbEmail.Text = "";
        tbTaxAddr.Text = "";
        tbNamaNPWP.Text = "";
        tbVatRegNo.Text = "";

    }


}