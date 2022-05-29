using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Microsoft.VisualBasic;

public partial class Master_Vendor : Page
{
    SqlTransaction trans;
    StringBuilder sql = new StringBuilder();
    SqlConnection conn;
    SqlCommand cmd;
    SqlDataReader dr;
    DataTable dt;
    ClassAdih cAdih = new ClassAdih();

    private static string TABLE_NAME_DETAIL = "vendor_bank";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                loadDdlTOP();
                clearForm();
            }

        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected bool saveData()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            conn = new SqlConnection(cAdih.getConnStr("Connection"));
            conn.Open();
            trans = conn.BeginTransaction();
            //   tbVendorID
            if (tbVendorID.Text.Trim() == "")
            {
                sql.Length = 0;
                sql.Append("SELECT RIGHT('000000' + CAST(ISNULL(MAX(CAST(RIGHT(vendor_no, 6) AS INT)), 0) + 1 AS VARCHAR), 6) AS running_no ");
                sql.Append("FROM vendor WITH(READPAST) ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    using (dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            tbVendorID.Text = "V" + dr["running_no"].ToString();
                            upHeader.Update();
                        }
                    }
                }

                sql.Length = 0;

                sql.Append("INSERT INTO [dbo].[vendor] ");
                sql.Append("([vendor_no] ");
                sql.Append(",[vendor_name] ");
                sql.Append(",[additional_name] ");
                sql.Append(",[vat_reg_no] ");
                sql.Append(",[street_address] ");
                sql.Append(",[email] ");
                sql.Append(",[phone_no] ");
                sql.Append(",[mobile_phone_no] ");
                sql.Append(",[status] ");
                sql.Append(",[payment_terms_id] ");
                sql.Append(",[bank_name] ");
                sql.Append(",[bank_account_no] ");
                sql.Append(",[tax_id]) ");
                sql.Append("VALUES('" + tbVendorID.Text + "', ");
                sql.Append("'" + tbVendorName.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + tbAddName.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + tbVatRegNo.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + tbStreetAddress.Text.Trim().Replace("'", "`").ToLower() + "', ");
                sql.Append("'" + tbEmail1.Text.Trim().Replace("'", "`").ToLower() + "', ");
                sql.Append("'" + tbPhoneNo1.Text.Trim().Replace("'", "`").ToLower() + "', ");
                sql.Append("'" + tbPhoneNo2.Text.Trim().Replace("'", "`").ToLower() + "', ");
                sql.Append("'" + ddlStatus.SelectedValue + "','" + ddlPaymentTerm.SelectedValue + "', ");
                sql.Append("'" + tbBankName.Text.Trim().Replace("'", "`").ToUpper() + "','" + tbBankAcc.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("'" + tbTaxRate.Text + "') ");
                cAdih.executeNonQuery(sql.ToString(), cAdih.getConnStr("Connection"));

            }
            else if (tbVendorID.Text != "")
            {
                sql.Length = 0;
                sql.Append("UPDATE Vendor SET vendor_name='" + tbVendorName.Text.Trim().Replace("'", "`").ToUpper() + "', additional_name='" + tbAddName.Text.Trim().Replace("'", "`").ToUpper() + "', ");
                sql.Append("vat_reg_no='" + tbVatRegNo.Text.Trim().Replace("'", "`").ToUpper() + "',");
                sql.Append("street_address='" + tbStreetAddress.Text.Trim().Replace("'", "`").ToUpper() + "',");
                sql.Append("email='" + tbEmail1.Text.Trim().Replace("'", "`").ToLower() + "', ");
                sql.Append("phone_no='" + tbPhoneNo1.Text.Trim().Replace("'", "`").ToLower() + "', mobile_phone_no='" + tbPhoneNo2.Text.Trim().Replace("'", "`").ToLower() + "',");
                sql.Append("status='" + ddlStatus.SelectedValue + "',");
                sql.Append("payment_terms_id='" + ddlPaymentTerm.SelectedValue + "', ");
                sql.Append("bank_name='" + tbBankName.Text.Trim().Replace("'", "`").ToUpper() + "',bank_account_no='" + tbBankAcc.Text.Trim().Replace("'", "`").ToUpper() + "' ,tax_id='" + tbTaxRate.Text.Trim().Replace("'", "`").ToUpper() + "' ");
                sql.Append("WHERE vendor_no='" + tbVendorID.Text + "' ");

                
                cAdih.executeNonQuery(sql.ToString(), cAdih.getConnStr("Connection"));
            }
            trans.Commit();
            conn.Close();

            return true;
        }
        catch (Exception ex)
        {
            trans.Rollback();
            conn.Close();
            master.messageBox(ex.Message);
            return false;
        }
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
                    bSearchVendorNo_Click(sender, new EventArgs());
                }
            }
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void bSearchVendorNo_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT * FROM vendor WHERE vendor_no='" + tbVendorID.Text + "'");

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
                            tbVendorName.Text = dr["vendor_name"].ToString();
                            tbAddName.Text = dr["additional_name"].ToString();
                            tbPhoneNo1.Text = dr["phone_no"].ToString();
                            tbPhoneNo2.Text = dr["mobile_phone_no"].ToString();
                            tbEmail1.Text = dr["email"].ToString();
                            tbTaxRate.Text = dr["tax_id"].ToString();
                            tbStreetAddress.Text = dr["street_address"].ToString();
                            //  tbt
                            tbVatRegNo.Text = dr["vat_reg_no"].ToString();
                            // ddlAddrCountry.SelectedValue = dr["country_id"].ToString();
                            ddlPaymentTerm.SelectedValue = dr["payment_terms_id"].ToString();
                            tbBankName.Text = dr["bank_name"].ToString();
                            tbBankAcc.Text = dr["bank_account_no"].ToString();
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

    protected void loadDdlTOP()
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT id AS value, description AS text FROM payment_term WITH(READPAST) ORDER BY days ");

            cAdih.loadDdl(sql.ToString(), cAdih.getConnStr("Connection"), ddlPaymentTerm);
        }
        catch (Exception ex)
        {

        }
    }

    protected void clearForm()
    {
        tbVendorID.Text = "";
        //tcDetail.ActiveTabIndex =0;
        tbVendorName.Text = "";
        tbAddName.Text = "";
        tbStreetAddress.Text = "";
        tbTaxRate.Text = "";
        tbPhoneNo1.Text = "";
        tbPhoneNo2.Text = "";
        tbEmail1.Text = "";

        tbVatRegNo.Text = "";
        ddlPaymentTerm.SelectedIndex = 0;

        tbBankName.Text = "";
        tbBankAcc.Text = "";
        upHeader.Update();
    }

    protected void wucSearch_Hide(object sender, EventArgs e)
    {
        if (Session["search"].ToString() == "vendor")
        {
            clearForm();
            tbVendorID.Text = wucSearch1.result;
            bSearchVendorNo_Click(sender, e);
        }
        else if (Session["search"].ToString() == "intenal")
        {

            tbVendorID.Text = wucSearch1.result;
            //lblVendorName.Text = "";
            bSearchVendorNo_Click(sender, e);
            upHeader.Update();
            //  upDetail.Update();
        }
        Session["search"] = null;
    }

    protected void ibSearchVendorNo_Click(object sender, ImageClickEventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            Session["search"] = "vendor";
            wucSearch1.loadGrid();
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

    protected void lbSave_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (tbVendorName.Text.Trim() == "")
            {
                master.messageBox("Please fill vendor name");
                return;
            }
            else if (tbVendorName.Text.Trim() == "")
            {
                master.messageBox("Please fill vendor name");
                return;
            }
            else if (tbPhoneNo1.Text.Trim() == "")
            {
                master.messageBox("Please fill phone no");
                return;
            }
            else if (tbPhoneNo2.Text.Trim() == "")
            {
                master.messageBox("Please fill mobile phone no");
                return;
            }
            else if (tbVatRegNo.Text.Trim() == "")
            {
                master.messageBox("Please fill vat reg no");
                return;
            }
            else if (ddlPaymentTerm.SelectedValue == "")
            {
                master.messageBox("Please fill payment term");
                return;
            }
            else if (tbBankName.Text.Trim() == "")
            {
                master.messageBox("Please fill bank name");
                return;
            }
            else if (!Information.IsNumeric(tbTaxRate.Text))
            {
                master.messageBox("Please fill Tax Rate Correctly");
                return;
            }

            else if (tbEmail1.Text.Trim() != (""))
            {

                if (!tbEmail1.Text.Trim().Contains("@"))
                {
                    master.messageBox("Invalid Format Email 1 !!");
                    return;
                }
            }


            showConfirmBox("Save Data?");

        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
        }
    }

    protected void lbClear_Click(object sender, EventArgs e)
    {
        clearForm();
    }


}