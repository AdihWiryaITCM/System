<%@ Page Title="CMSystem || Rental Quotation" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RentalQuotationProcess.aspx.cs" Inherits="Transaction_RentalQuotationProcess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Wuc/wucSearch.ascx" TagName="wucSearch" TagPrefix="uc1" %>
<%@ Register Src="~/Wuc/wucConfirmBox.ascx" TagName="wucConfirmBox" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Rental Quotation</h3>
        </div>
    </div>
    <div class="x_panel">
        <asp:UpdatePanel ID="upHeader" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Trans No / Date</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbTransNo" runat="server" CssClass="form-control" Width="160px" ReadOnly="true" />
                                    <asp:TextBox ID="tbTransDate" runat="server" CssClass="form-control" Width="110px" ReadOnly="true" />
                                    <asp:Label ID="lStatus" runat="server" Width="70px" Visible="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Rent To</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:Panel ID="pSoldTo" runat="server" DefaultButton="bSearchSoldTo">
                                        <asp:TextBox ID="tbSoldTo" runat="server" Width="120px" CssClass="form-control text-uppercase" />
                                        <asp:LinkButton ID="lbSoldTo" runat="server" Text="" OnClick="lbSoldTo_Click" CssClass="alert-link" />
                                        <asp:ImageButton ID="ibSearchSoldTo" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchSoldTo_Click" />
                                        <asp:Button ID="bSearchSoldTo" runat="server" Style="display: none;" OnClick="bSearchSoldTo_Click" />
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Bill To</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:Panel ID="pBillTo" runat="server" DefaultButton="bSearchBillTo">
                                        <asp:TextBox ID="tbBillTo" runat="server" Width="60px" CssClass="form-control text-uppercase" />
                                        <asp:LinkButton ID="lbBillTo" runat="server" Text="" OnClick="lbBillTo_Click" CssClass="alert-link" />
                                        <asp:ImageButton ID="ibSearchBillTo" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchBillTo_Click" />
                                        <asp:Button ID="bSearchBillTo" runat="server" Style="display: none;" OnClick="bSearchBillTo_Click" />
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Rent To Site</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:Panel ID="pShipTo" runat="server" DefaultButton="bSearchShipTo">
                                        <asp:TextBox ID="tbShipTo" runat="server" Width="80px" CssClass="form-control text-uppercase" />
                                        <asp:LinkButton ID="lbShipTo" runat="server" Text="" OnClick="lbShipTo_Click" CssClass="alert-link" />
                                        <asp:ImageButton ID="ibSearchShipTo" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchShipTo_Click" />
                                        <asp:Button ID="bSearchShipTo" runat="server" Style="display: none;" OnClick="bSearchShipTo_Click" />
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Req Delv Date</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbInstallationDate" runat="server" Width="110px" CssClass="form-control" />
                                    <cc1:CalendarExtender ID="ceInstallationDate" runat="server" TargetControlID="tbInstallationDate" Format="dd-MM-yyyy" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Term of Payment</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:Panel ID="pPaymentTerm" runat="server" DefaultButton="bSearchPaymentTerm">
                                        <asp:TextBox ID="tbPaymentTerm" runat="server" Width="80px" CssClass="form-control text-uppercase" />
                                        <asp:LinkButton ID="lbPaymentTerm" runat="server" Text="" OnClick="lbPaymentTerm_OnClick" CssClass="alert-link" />
                                        <asp:ImageButton ID="ibSearchPaymentTerm" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchPaymentTerm_OnClick" />
                                        <asp:Button ID="bSearchPaymentTerm" runat="server" Style="display: none;" OnClick="bSearchPaymentTerm_OnClick" />
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Currency</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbCurrency" runat="server" Width="60px" CssClass="form-control" Text="IDR" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Installation Charge</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbInstallationCharge" CssClass="form-control" runat="server" Width="120px" Text="0" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Amount</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbAmt" runat="server" Width="120px" CssClass="form-control" ReadOnly="true" Text="0" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">VAT</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbTaxAmt" runat="server" Width="120px" CssClass="form-control" ReadOnly="true" Text="0" />
                                    <asp:Label ID="lblTRate" runat="server" Width="70px" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Total Amount</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbNetValue" runat="server" Width="120px" CssClass="form-control" ReadOnly="true" Text="0" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label ID="Label1" runat="server">Note</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbNote" runat="server" TextMode="MultiLine" Width="250px" CssClass="form-control text-uppercase" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row">
                    <div class="col-sm-12">
                        <div>
                            <div>
                                <asp:GridView ID="grid" runat="server" AllowPaging="false"
                                    AutoGenerateColumns="False" Width="100%"
                                    CssClass="table table-striped table-bordered table-hover"
                                    EmptyDataText="NO DATA" BackColor="White"
                                    OnRowDataBound="grid_RowDataBound">
                                    <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" />
                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <PagerStyle BorderWidth="1px" BackColor="White" ForeColor="#2A3F54" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" HorizontalAlign="Right" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Article" HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lLineNo" runat="server" Text='<%# Bind("lineNo") %>' Visible="false" />
                                                <asp:Label ID="lArticleNo" runat="server" Text='<%# Bind("articleNo") %>' /><br />
                                                <asp:Label ID="lArticleDesc" runat="server" Text='<%# Bind("articleDesc") %>' />
                                                <asp:Label ID="lArticleType" runat="server" Text='<%# Bind("articleType") %>' Visible="false" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty" HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lQty" runat="server" Text='<%# Bind("qty", "{0:##,#0}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rent Duration (MON)" HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lTimeDuration" runat="server" Text='<%# Bind("timeDuration", "{0:##}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top"  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rent Rate" HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbRate" runat="server" Text='<%# Bind("rate", "{0:##,#0.#0}") %>' Width="100%" CssClass="form-control numbox" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VAT" HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lTax" runat="server" Text='<%# Bind("tax", "{0:##,#0.#0}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Disc Rate" HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbDisc" runat="server" CssClass="form-control numbox" Text='<%# Bind("disc") %>'/>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lAmount" runat="server" Text='<%# Bind("totalAmount", "{0:##,#0.#0}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="imgMinus" runat="server" OnClick="imgDelete_Click"><i class="fa fa-trash" style="font-size:17px"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="15px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="upDetail" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label runat="server">Article Number</asp:Label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-inline">
                                            <asp:Panel ID="pSearchArticle" runat="server" DefaultButton="bSearchArticle">
                                                <asp:TextBox ID="tbArticleNo" runat="server" Width="110px" CssClass="form-control text-uppercase" />
                                                <asp:TextBox ID="tbArticleType" runat="server" Visible="false" />
                                                <asp:LinkButton ID="lbArticleDesc" runat="server" Text="" CssClass="alert-link" OnClick="lbClearArticle_Click" />
                                                <asp:ImageButton ID="ibSearchArticle" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchArticle_Click" />
                                                <asp:Button ID="bSearchArticle" runat="server" Style="display: none;" OnClick="bSearchArticle_Click" />
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label runat="server">Qty</asp:Label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-inline">
                                            <asp:TextBox ID="tbQty" CssClass="form-control" runat="server" Width="70px" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label runat="server">Time Duration</asp:Label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-inline">
                                            <asp:TextBox ID="tbTimeDuration" CssClass="form-control" runat="server" Width="70px" /> &nbsp; MON
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label runat="server">Rent Rate</asp:Label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-inline">
                                            <asp:TextBox ID="tbRate" CssClass="form-control" runat="server" Width="120px" /> 
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:Label runat="server">Disc Rate</asp:Label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-inline">
                                            <asp:TextBox ID="tbDisc" runat="server" Width="120px" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-inline">
                                            <asp:LinkButton ID="bAdd" ForeColor="White" runat="server" CssClass="btn btn-primary btn-sm" OnClick="bAdd_Click"><i class="fa fa-plus"></i>&nbsp;Add</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="row text-center buttonSet">
                    <div class="col-sm-12">
                        <asp:LinkButton ID="btnSave" runat="server" ForeColor="White" CssClass="btn btn-primary btn-sm" OnClick="btnSave_Click"><i class="fa fa-save"></i>&nbsp;Save</asp:LinkButton>
                        <asp:LinkButton ID="btnPosting" runat="server" ForeColor="White" CssClass="btn btn-success btn-sm" OnClick="btnPosting_Click"><i class="fa fa-arrow-circle-o-up"></i>&nbsp;Post</asp:LinkButton>
                        <asp:LinkButton ID="btnCancel" runat="server" ForeColor="White" CssClass="btn btn-danger btn-sm" OnClick="btnCancel_Click"><i class="fa fa-undo"></i>&nbsp;Back</asp:LinkButton>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <uc1:wucSearch ID="wucSearch1" runat="server" OnHide="wucSearch_Hide" />
        <uc2:wucConfirmBox ID="wucConfirmBox1" runat="server" OnHide="wucConfirmBox_Hide" />
    </div>
</asp:Content>