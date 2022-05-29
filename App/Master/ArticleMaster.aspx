<%@ Page Title="CMSystem || Item Master" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ArticleMaster.aspx.cs" Inherits="Master_ArticleMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Wuc/wucConfirmBox.ascx" TagName="wucConfirmBox" TagPrefix="uc2" %>
<%@ Register Src="~/Wuc/wucSearch.ascx" TagName="wucSearch" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Article Master</h3>
        </div>
    </div>
    <div class="x_panel">
        <asp:UpdatePanel ID="updHeader" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="form-inline" style="display: inline">
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="lbSearch">
                                <div style="float: left;">
                                    <asp:LinkButton ID="lbAddNew" runat="server" CssClass="btn btn-primary btn-sm" ForeColor="White" OnClick="lbAddNew_Click"><i class="fa fa-arform-group row-up"></i>&nbsp;Add New</asp:LinkButton>
                                </div>
                                <div style="float: right;">
                                    <div class="input-group">
                                        <asp:TextBox ID="tbSearch" runat="server" placeholder="Search.."
                                            CssClass="form-control" value="" Width="200px" OnTextChanged="lbSearch_Click" />
                                        <div class="input-group-addon input-group-addon1">
                                            <asp:LinkButton ID="lbSearch" runat="server" OnClick="lbSearch_Click"><i class="fa fa-search"></i></asp:LinkButton><br />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updForm" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlForm" runat="server" Visible="false">
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <div class="form-inline">
                                    <div class="col-sm-2">
                                        <asp:Label runat="server">Article Number </asp:Label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-group">
                                            <asp:TextBox ID="tbArticleNo" runat="server" Width="100px" CssClass="form-control toUpper" ReadOnly="true" />
                                            <div class="form-group">
                                                <i>(leave blank if add new)</i>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <div class="form-inline">
                                    <div class="col-sm-2">
                                        <asp:Label runat="server">Article Description </asp:Label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-group">
                                            <asp:TextBox ID="tbArticleDesc" runat="server" Width="500px" CssClass="form-control toUpper" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <div class="form-inline">
                                    <div class="col-sm-2">
                                        <asp:Label runat="server">Article Type </asp:Label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-group">
                                            <asp:Panel ID="pSearchArticleType" runat="server" DefaultButton="bSearchArticleType">
                                                <asp:TextBox ID="tbArticleTypeID" runat="server" Width="85px" CssClass="form-control toUpper" />
                                                <asp:LinkButton ID="lbArticleTypeDesc" runat="server" CssClass="formControl" OnClick="lbArticleTypeDesc_Click" />
                                                <asp:ImageButton ID="ibSearchArticleType" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchArticleType_Click" />
                                                <asp:Button ID="bSearchArticleType" runat="server" Style="display: none;" OnClick="bSearchArticleType_Click" />
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <div class="form-inline">
                                    <div class="col-sm-2">
                                        <asp:Label runat="server">Base UOM </asp:Label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-group">
                                            <asp:Panel ID="pSearchBaseUOM" runat="server" DefaultButton="bSearchBaseUOM">
                                                <asp:TextBox ID="tbBaseUOMID" runat="server" Width="85px" ReadOnly="true" CssClass="form-control toUpper" />
                                                <asp:LinkButton ID="lbBaseUOMDesc" runat="server" CssClass="formControl" OnClick="lbBaseUOMDesc_Click" />
                                                <asp:ImageButton ID="ibSearchBaseUOM" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchBaseUOM_Click" />
                                                <asp:Button ID="bSearchBaseUOM" runat="server" Style="display: none;" OnClick="bSearchBaseUOM_Click" />
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row text-center">
                                    <div>
                                        <div class="col-sm-12">
                                            <div class="col-sm-12 buttonSet">
                                                <div>
                                                    <asp:LinkButton ID="lbSave" runat="server" OnClick="lbSave_Click" Width="73px" CssClass="btn btn-primary btn-sm" ForeColor="White"><i class="fa fa-save"></i>&nbsp;Save</asp:LinkButton>
                                                    <asp:LinkButton ID="lbCancel" runat="server" OnClick="lbCancel_Click" CssClass="btn btn-danger btn-sm" ForeColor="White"><i class="fa fa-undo"></i>&nbsp;Cancel</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="form-group row">
            <div class="col-sm-12">
                <div>
                    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="pnlData" runat="server">
                                <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" Width="100%" class="table table-striped table-bordered table-hover"
                                    EmptyDataText="NO DATA" DataKeyNames="article_no" BackColor="White"
                                    AllowPaging="True" PageSize="10" OnPageIndexChanging="grid_PageIndexChanging" PagerSettings-Visible="false">
                                    <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" />
                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <PagerStyle BorderWidth="1px" BackColor="White" ForeColor="#2A3F54" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Code" HeaderStyle-CssClass="text-center">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="70px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Text='<%# Bind("article_no") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="text-center">
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="450px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("article_description") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type" HeaderStyle-CssClass="text-center">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="120px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblTypeID" runat="server" Text='<%# Bind("article_type") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="UOM" HeaderStyle-CssClass="text-center">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="100px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblUOMID" runat="server" Text='<%# Bind("base_uom") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Center" Width="30px" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbEdit" runat="server" OnClick="imgEdit_Click"><i class="fa fa-pencil" style="font-size:17px"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <asp:UpdatePanel ID="upFooter" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div style="float: left; width: 100%; text-align: center; margin: 12px 0px 0px 0px;">
                                                <u>
                                                    <asp:LinkButton ID="linkFirst" runat="server" CssClass="alert-link" ToolTip="click to move to first page" OnClick="linkFirst_Click">First</asp:LinkButton></u>
                                                <u>
                                                    <asp:LinkButton ID="linkPrev" runat="server" CssClass="alert-link" ToolTip="click to move to previous page" OnClick="linkPrev_Click">Prev</asp:LinkButton></u>
                                                <asp:Label ID="lblRecords" runat="server" Text="Records: 1-10" />
                                                <u>
                                                    <asp:LinkButton ID="linkNext" runat="server" CssClass="alert-link" ToolTip="click to move to next page" OnClick="linkNext_Click">Next</asp:LinkButton></u>
                                                <u>
                                                    <asp:LinkButton ID="linkLast" runat="server" CssClass="alert-link" ToolTip="click to move to last page" OnClick="linkLast_Click">Last</asp:LinkButton></u>
                                                <br />
                                                Total Records:
                                                <asp:Label ID="lblTotalRecords" runat="server" Text="" />
                                            </div>
                                            <div style="float: left; width: 100%; margin: 0px 0px 0px 8px;">
                                                Page
                                            <asp:DropDownList ID="ddlPageOf" runat="server" CssClass="combobox" AutoPostBack="true" OnSelectedIndexChanged="ddlPageOf_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:Label ID="lblPageOf" runat="server" Text="" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lbSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <uc2:wucConfirmBox ID="wucConfirmBox1" runat="server" OnHide="wucConfirmBox_Hide" />
    <uc3:wucSearch ID="wucSearch1" runat="server" OnHide="wucSearch_Hide" />
</asp:Content>