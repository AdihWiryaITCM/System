<%@ Page Title="CMSystem || Article Serial No" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ArticleSerialNo.aspx.cs" Inherits="Report_ArticleSerialNo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Wuc/wucSearch.ascx" tagname="wucSearch" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
            $(document).ready(function () {
                $('[data-toggle="tooltip"]').tooltip();
            });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Article Serial No</h3>
        </div>
    </div>
    <div class="x_panel">
        <asp:UpdatePanel ID="upHeader" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:Label runat="server">Site</asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:TextBox ID="tbSiteID" runat="server" Width="100px" CssClass="toUpper form-control" ReadOnly="true"/>
                                <div class="form-group">
                                <asp:ImageButton ID="ibSearchSiteID" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchSiteID_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:Label runat="server">Article</asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:TextBox ID="tbArticle" runat="server" Width="100px" CssClass="toUpper form-control" ReadOnly="true"/>
                                <div class="form-group">
                                    <asp:ImageButton ID="ibSearchArticle" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchArticle_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:Label runat="server">Serial No</asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:TextBox ID="tbSerialNo" runat="server" Width="200px" CssClass="toUpper form-control"/>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                        </div>
                        <div class="col-sm-10">
                            <div class="fom-inline">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="bShow" runat="server" ForeColor="White" OnClick="bShow_Click" CssClass="btn btn-primary btn-sm"><i class="fa fa-chevron-down"></i>Show</asp:LinkButton>
                                        <asp:LinkButton ID="bExcel" runat="server" ForeColor="White" OnClick="bExcel_Click" CssClass="btn btn-success btn-sm"><i class="fa fa-file-excel-o"></i>Excel</asp:LinkButton>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="float:left; width:100%; height:20px;">&nbsp;</div>
        <div style="float:left; width:100%;">
            <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                <ContentTemplate>      
                    <div class="table-responsive"> 
                    <asp:GridView ID="grid" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%" 
                    class="table table-striped table-bordered table-hover" 
                    EmptyDataText="NO DATA" BackColor="White">
                    <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" />
                    <EmptyDataRowStyle HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#2A3F54" />
                    <AlternatingRowStyle BackColor="White" />
                    <PagerStyle BorderWidth="1px" BackColor="White" ForeColor="#2A3F54" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" HorizontalAlign="Right" />
                        <Columns>
                            <asp:TemplateField HeaderText="Site" HeaderStyle-CssClass="text-center">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lSite" runat="server" Text='<%# Bind("site_name") %>'/>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Article No" HeaderStyle-CssClass="text-center">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lArticleNo" runat="server" Text='<%# Bind("article_no") %>'/>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="text-center">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lArticleDesc" runat="server" Text='<%# Bind("article_description") %>'/>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Serial No" HeaderStyle-CssClass="text-center">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lSerialNo" runat="server" Text='<%# Bind("serial_no") %>'/>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="250px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <uc1:wucSearch ID="wucSearch1" runat="server" OnHide="wucSearch_Hide" />
    </div>
</asp:Content>

