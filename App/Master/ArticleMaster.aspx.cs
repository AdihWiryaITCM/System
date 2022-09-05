using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Data;

using System.Collections;


public partial class Master_ArticleMaster : Page
{
    StringBuilder sql = new StringBuilder();
    SqlTransaction trans;
    SqlConnection conn;
    SqlCommand cmd;
    SqlDataReader dr;
    DataTable dt;
  
    ClassAdih cAdih = new ClassAdih();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                loadData();
                loadDdlPageOf();
                loadRecords();
            }
        }
        catch (Exception ex)
        {
            //MasterPage master = (MasterPage)this.Master;
            //master.messageBox(ex.Message);
        }
    }

    protected void imgEdit_Click(object sender, EventArgs e)
    {
        MasterPage mstr = (MasterPage)this.Master;
        try
        {
            GridViewRow gridRow = ((sender as LinkButton).Parent.Parent as GridViewRow);
            Label lblID = (Label)gridRow.FindControl("lblID");
            tbArticleNo.Text = lblID.Text;
            //Response.Redirect("ArticleMasterDetail.aspx?action=edit&article_no=" + lblID.Text);
            loadArticle();
            pnlForm.Visible = true;
            Panel1.Visible = false;
            pnlData.Visible = false;
            upFooter.Visible = false;
            upFooter.Update();
            updHeader.Update();
            updForm.Update();

        }
        catch (Exception ex)
        {
            mstr.messageBox(ex.Message);
        }
    }

    protected void loadData()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT *  FROM article ");
            if (tbSearch.Text.Trim() != "")
            {
                sql.Append(" AND ( article_number LIKE '" + tbSearch.Text.Trim() + "%' OR " +
                           "       article_description LIKE '" + tbSearch.Text.Trim() + "%') ");
            }
            cAdih.loadGridView(sql.ToString(), cAdih.getConnStr("Connection"), grdData);
            upGrid.Update();
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

    protected void ibSearchArticleType_Click(object sender, EventArgs e)
    {
        Session["search"] = "article_type";
        wucSearch1.loadGrid();
    }

    protected void bSearchArticleType_Click(object sender, EventArgs e)
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT description ");
            sql.Append("FROM article_type ");
            sql.Append("WHERE id = '" + tbArticleTypeID.Text.Trim().Replace("'", "`") + "' ");

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
                            lbArticleTypeDesc.Text = dr["description"].ToString();
                            tbArticleTypeID.ReadOnly = true;
                            ibSearchArticleType.Visible = false;
                        }
                        else
                        {
                            lbArticleTypeDesc.Text = "";
                            tbArticleTypeID.ReadOnly = false;
                            ibSearchArticleType.Visible = true;
                        }
                        updForm.Update();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void lbArticleTypeDesc_Click(object sender, EventArgs e)
    {
        try
        {
            lbArticleTypeDesc.Text = "";
            tbArticleTypeID.Text = "";
            tbArticleTypeID.ReadOnly = false;
            ibSearchArticleType.Visible = true;
            updForm.Update();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void ibSearchBaseUOM_Click(object sender, EventArgs e)
    {
        Session["search"] = "uom";
        wucSearch1.loadGrid();
    }

    protected void bSearchBaseUOM_Click(object sender, EventArgs e)
    {
        try
        {
            sql.Length = 0;
            sql.Append("SELECT description ");
            sql.Append("FROM uom ");
            sql.Append("WHERE id = '" + tbBaseUOMID.Text.Trim().Replace("'", "`") + "' ");

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
                            lbBaseUOMDesc.Text = dr["description"].ToString();
                            tbBaseUOMID.ReadOnly = true;
                            ibSearchBaseUOM.Visible = false;
                        }
                        else
                        {
                            lbBaseUOMDesc.Text = "";
                            tbBaseUOMID.ReadOnly = false;
                            ibSearchBaseUOM.Visible = true;
                        }
                        updForm.Update();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void lbBaseUOMDesc_Click(object sender, EventArgs e)
    {
        try
        {
            lbBaseUOMDesc.Text = "";
            tbBaseUOMID.Text = "";
            tbBaseUOMID.ReadOnly = false;
            ibSearchBaseUOM.Visible = true;
            updForm.Update();
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void lbSave_Click(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        if (tbArticleDesc.Text.Trim() == "")
        {
            master.messageBox("Input article description!");
            return;
        }
        else if (tbArticleTypeID.Text.Trim() == "")
        {
            master.messageBox("Input article type!");
            return;
        }
        else
        {
            showConfirmBox("Save Data?");
        }
    }

    protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdData.PageIndex = e.NewPageIndex;
        //Session["PageNoLastEdited"] = e.NewPageIndex;
        loadData();
    }

    protected void linkFirst_Click(Object sender, EventArgs e)
    {
        grdData.PageIndex = 0;
        //Session["PageNoLastEdited"] = "0";
        loadData();
        ddlPageOf.SelectedValue = "1";
        loadRecords();

    }

    protected void linkPrev_Click(Object sender, EventArgs e)
    {
        if (grdData.PageIndex > 0)
        {
            grdData.PageIndex = grdData.PageIndex - 1;
            //Session["PageNoLastEdited"] = grid.PageIndex;
            loadData();
            ddlPageOf.SelectedValue = (grdData.PageIndex + 1).ToString(); ;
            loadRecords();
        }

    }

    protected void linkNext_Click(Object sender, EventArgs e)
    {
        grdData.PageIndex = grdData.PageIndex + 1;
        //Session["PageNoLastEdited"] = grid.PageIndex;
        loadData();
        ddlPageOf.SelectedValue = (grdData.PageIndex + 1).ToString(); ;
        loadRecords();

    }

    protected void linkLast_Click(Object sender, EventArgs e)
    {
        grdData.PageIndex = grdData.PageCount - 1;
        //Session["PageNoLastEdited"] = grid.PageIndex;
        loadData();
        ddlPageOf.SelectedValue = grdData.PageCount.ToString();
        loadRecords();
    }

    protected void loadRecords()
    {
        try
        {
            String FirstRowNo = (((grdData.PageIndex) * grdData.PageSize) + 1).ToString();
            if (grdData.Rows.Count == 0)
            {
                FirstRowNo = "0";
            }
            String LastRowNo = ((grdData.PageSize * grdData.PageIndex) + (grdData.Rows.Count)).ToString();
            lblRecords.Text = "Records: " + FirstRowNo + "-" + LastRowNo + "";
            upFooter.Update();
        }
        catch (Exception ex)
        {

        }
    }

    protected void loadDdlPageOf()
    {
        try
        {
            //LOAD TRANS
            int i = 1;
            int pageOf = 0;
            int sumTrans = (grdData.DataSource as DataTable).Rows.Count;
            if (sumTrans % 10 > 0)
            {
                pageOf = (sumTrans / 10) + 1;
            }
            else if (sumTrans % 10 == 0)
            {
                pageOf = sumTrans / 10;
            }

            ddlPageOf.Items.Clear();
            for (i = 1; i <= pageOf; i++)
            {
                ListItem ls = new ListItem();
                ls.Value = i.ToString();
                ls.Text = i.ToString();
                ddlPageOf.Items.Add(ls);
            }

            lblPageOf.Text = "of " + pageOf.ToString() + "";
            lblTotalRecords.Text = "Total Records: " + sumTrans.ToString() + "";
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void ddlPageOf_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdData.PageIndex = int.Parse(ddlPageOf.SelectedValue) - 1;
        loadData();
        loadRecords();
    }

    protected void saveData()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            if (tbArticleNo.Text.Trim() == "")
            {

                string new_article_no = "";
                conn = new SqlConnection(cAdih.getConnStr("Connection"));
                conn.Open();
                trans = conn.BeginTransaction();
              
                sql.Length = 0;
                sql.Append("SELECT RIGHT('00000000' + CAST(ISNULL(MAX(CAST(article_no AS INT)), 0) + 1 AS VARCHAR), 8) AS new_article_no ");
                sql.Append("FROM article ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    using (dr = cmd.ExecuteReader()) {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            new_article_no = dr["new_article_no"].ToString();

                        }
                        else {
                            return;
                        }
                    }
                }

                sql.Length = 0;
                sql.Append("INSERT INTO article(article_no, article_description, article_type,base_uom," +
                           "created_by, created_date) VALUES( ");
                sql.Append("'" + new_article_no + "', ");
                sql.Append("'" + tbArticleDesc.Text.ToUpper().Replace("'","`").Trim() + "', ");
                sql.Append("'" + tbArticleTypeID.Text.ToUpper().Replace("'", "`").Trim() + "', ");
                sql.Append("'" + tbBaseUOMID.Text.ToUpper().Replace("'", "`").Trim() + "', ");
                sql.Append("'" + Session["username"].ToString() + "', ");
                sql.Append("GETDATE()) ");

                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                trans.Commit();
                conn.Close();

                tbArticleNo.Text = new_article_no;
                master.messageBox("Article No: " + new_article_no + " has been created.");
            }
            else
            {

                conn = new SqlConnection(cAdih.getConnStr("Connection"));
                conn.Open();
                trans = conn.BeginTransaction();
                sql.Length = 0;

                sql.Append("UPDATE  article SET article_description='" + tbArticleDesc.Text.ToUpper().Replace("'", "`").Trim() + "'," +
                                   "article_type='" + tbArticleTypeID.Text.ToUpper().Replace("'", "`").Trim() + "', " +
                                   "base_uom='" + tbBaseUOMID.Text.ToUpper().Replace("'", "`").Trim() + "' " +
                                   "WHERE article_no='" + tbArticleNo.Text.Trim() + "';");


                using (cmd = new SqlCommand(sql.ToString(), conn, trans))
                {
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }

                trans.Commit();
                conn.Close();

             //   tbArticleNo.Text = new_article_no;
                
                master.messageBox("Article No: " + tbArticleNo.Text + " has been modify.");
            }
        }
        catch (Exception ex)
        {
            master.messageBox(ex.Message);
            trans.Rollback();
            conn.Close();
            return;
        }
    }

    protected void clearForm()
    {
        tbArticleNo.Text = "";
        tbArticleDesc.Text = "";
        tbArticleTypeID.Text = "";
        tbBaseUOMID.Text = "";
        bSearchArticleType_Click(ibSearchArticleType, new EventArgs());
        pnlData.Visible = false;
        upGrid.Update();
        updForm.Update();
     
        updHeader.Update();
    }

    protected void lbCancel_Click(object sender, EventArgs e)
    {
        clearForm();
        loadData();
        pnlForm.Visible = false;
        pnlData.Visible = true;
        Panel1.Visible = true;
        upGrid.Update();
        updForm.Update();
    }

    protected void wucConfirmBox_Hide(object sender, EventArgs e)
    {
        try
        {
            if (wucConfirmBox1.StrMode == "Yes")
            {
                saveData();
                lbCancel_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
            MasterPage master = (MasterPage)this.Master;
            master.messageBox(ex.Message);
        }
    }

    protected void wucSearch_Hide(object sender, EventArgs e)
    {
        if (Session["search"].ToString() == "article_type")
        {
            tbArticleTypeID.Text = wucSearch1.result;
            bSearchArticleType_Click(sender, e);
            updForm.Update();
        }
        else if (Session["search"].ToString() == "uom")
        {
            tbBaseUOMID.Text = wucSearch1.result;
            bSearchBaseUOM_Click(sender, e);
            updForm.Update();
        }
        else if (Session["search"].ToString() == "item_article")
        {
            tbArticleNo.Text = wucSearch1.result;
            loadArticle(); ;
            //  updForm.Update();
        }
        Session["search"] = null;
    }

    protected void wucLookup_Hide(object sender, EventArgs e)
    {
        if (Session["lookupcode"].ToString() == "LU00052")
        {
            //tbStepCode.Text = wuclookup1.result[0].ToString();
            //bStepCode_OnClick(sender, e);
            //updForm.Update();

            return;
        }
    }

    protected void loadArticle()
    {
        MasterPage master = (MasterPage)this.Master;
        try
        {
            sql.Length = 0;
            sql.Append("SELECT * ");
            sql.Append("FROM article a WHERE article_no ='" + tbArticleNo.Text.Trim() + "';");
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
                            tbArticleDesc.Text = dr["article_description"].ToString();
                            
                            tbArticleTypeID.Text = dr["article_type"].ToString();
                            tbBaseUOMID.Text = dr["base_uom"].ToString();

                            bSearchArticleType_Click(ibSearchArticleType, new EventArgs());
                            bSearchBaseUOM_Click(ibSearchArticleType, new EventArgs());
                            updForm.Update();
                          

                        }
                        else
                        {
                            tbArticleNo.Text = "";
                            master.messageBox("Invalid Stock Code!");
                            updForm.Update();
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

    protected void imgFind_Click(object sender, ImageClickEventArgs e)
    {
        Session["search"] = "item_article";
        wucSearch1.loadGrid();
     //   item_article
    }

    protected void lbAddNew_Click(object sender, EventArgs e)
    {
        clearForm();
        Panel1.Visible = false;
        updHeader.Update();
        pnlForm.Visible = true;
        pnlData.Visible = false;
        updForm.Update();
        upGrid.Update();
    }

    protected void lbSearch_Click(object sender, EventArgs e)
    {
        loadData();
    }
    
}