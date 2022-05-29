<%@ Page Title="CMSystem || Purchase Order" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PurchaseOrderProcess.aspx.cs" Inherits="Transaction_PurchaseOrderProcess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Wuc/wucSearch.ascx" TagName="wucSearch" TagPrefix="uc1" %>
<%@ Register Src="~/Wuc/wucConfirmBox.ascx" TagName="wucConfirmBox" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="Server">
    <script type="text/javascript" src="../Scripts/js_function.js"></script>
    <div class="page-title">
        <div class="title_left">
            <h3>Purchase Order</h3>
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
                                    <asp:TextBox ID="tbTransNo" runat="server" Width="170px" CssClass="form-control" ReadOnly="true" />
                                    <asp:TextBox ID="tbTransDate" runat="server" Width="110px" CssClass="form-control" ReadOnly="true" />
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
                                <asp:Label runat="server">Purchase Requisition No</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbPRNo" runat="server" Width="160px" CssClass="form-control text-uppercase" />
                                    <asp:LinkButton ID="lbPRNo" runat="server" Text="" OnClick="lbPRNo_Click" CssClass="alert-link" />
                                    <asp:ImageButton ID="ibSearchPRNo" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchPRNo_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Ship To</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbShipTo" runat="server" Width="80px" CssClass="form-control text-uppercase" />
                                    <asp:LinkButton ID="lbShipTo" runat="server" Text="" CssClass="alert-link" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Vendor</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:Panel ID="pVendor" runat="server" DefaultButton="bSearchVendor">
                                        <asp:TextBox ID="tbVendor" runat="server" Width="100px" CssClass="form-control text-uppercase" />
                                        <asp:LinkButton ID="lbVendor" runat="server" Text="" OnClick="lbVendor_Click" CssClass="alert-link" />
                                        <asp:ImageButton ID="ibSearchVendor" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchVendor_Click" />
                                        <asp:Button ID="bSearchVendor" runat="server" Style="display: none;" OnClick="bSearchVendor_Click" />
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
                                <asp:Label runat="server">Req. Delv. Date</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbReqDelvDate" runat="server" Width="110px" CssClass="form-control" />
                                    <cc1:CalendarExtender ID="ceReqDelvDate" runat="server" TargetControlID="tbReqDelvDate" Format="dd-MM-yyyy" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:Panel ID="pTOP" runat="server">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="col-sm-2">
                                    <asp:Label ID="Label1" runat="server">Term of Payment</asp:Label>
                                </div>
                                <div class="col-sm-10">
                                    <div class="form-inline">
                                        <asp:TextBox ID="tbPaymentTerm" runat="server" Width="70px" CssClass="form-control text-uppercase" ReadOnly="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
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
                                <asp:Label runat="server">VAT Rate</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="lblTax" runat="server" Width="60px" Text="0" CssClass="form-control numbox" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Note</asp:Label>
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
        <asp:UpdatePanel ID="upPopUp" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lTotal" runat="server" Style="color: #fff;"></asp:Label>
                <asp:HiddenField ID="hfTotalPO" runat="server"></asp:HiddenField>
                <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-sm-12">
                                <asp:GridView ID="grid" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%"
                                    CssClass="table table-striped table-bordered table-hover" OnRowDataBound="grid_RowDataBound"
                                    DataKeyNames="articleNo" EmptyDataText="NO DATA" BackColor="White" ShowFooter="true">
                                    <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" />
                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <PagerStyle BorderWidth="1px" BackColor="White" ForeColor="#2A3F54" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#2A3F54" Font-Bold="true" ForeColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="PR No / Article No / Article Desc " HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lPRNo" runat="server" Text='<%# Bind("PRNo") %>' />
                                                <br />
                                                <asp:Label ID="lLineNo" runat="server" Text='<%# Bind("lineNo") %>' Visible="false" />
                                                <asp:Label ID="lArticleNo" runat="server" Text='<%# Bind("articleNo") %>' />
                                                <asp:Label ID="lPRLineNo" runat="server" Text='<%# Bind("prLineNo") %>' Visible="false" />
                                                <br />
                                                <asp:Label ID="lArticleDesc" runat="server" Text='<%# Bind("articleDesc") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Article No." Visible="false" HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lArticleNo1" runat="server" Text='<%# Bind("articleNo") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Article Desc" Visible="false" HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lArticleDesc1" runat="server" Text='<%# Bind("articleDesc") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty PR" HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lQtyPR" runat="server" Text='<%# Bind("qtyPR", "{0:##,#0.#0}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty Ordered" HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lQtyOrdered" runat="server" Text='<%# Bind("qtyOrdered", "{0:##,#0.#0}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty PO" HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbQtyPO" runat="server" Width="88%" Text='<%# Bind("qtyPO") %>' CssClass="form-control" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit Price" HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbUnitPrice" runat="server" Width="100px" Text='<%# Bind("unitPrice", "{0:##,#0.#0}") %>' CssClass="form-control" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblDamount" runat="server" Text='<%# Bind("Amount", "{0:##,#0.#0}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
                                            <FooterStyle Width="60px" HorizontalAlign="Right" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VAT" HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtax" runat="server" Text='<%# Bind("tax", "{0:##,#0.#0}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
                                            <FooterStyle Width="60px" HorizontalAlign="Right" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lTotalAmount" runat="server" Text='<%# Bind("totalAmount", "{0:##,#0.#0}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="80px" />
                                            <FooterStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Base Amount" HeaderStyle-CssClass="text-center" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lBaseAmount" runat="server" Text='<%# Bind("baseAmount", "{0:##,#0.#0}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="80px" />
                                            <FooterStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Note" HeaderStyle-CssClass="text-center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbNote" runat="server" Text='<%# Bind("note") %>' Width="92%" TextMode="MultiLine" CssClass="form-control toUpper" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="160px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-sm-12 text-center buttonSet">
                        <asp:LinkButton ID="btnSave" runat="server" ForeColor="White" CssClass="btn btn-primary btn-sm" OnClick="btnSave_Click"><i class="fa fa-save"></i>&nbsp;Save</asp:LinkButton>
                        <asp:LinkButton ID="btnPosting" runat="server" ForeColor="White" CssClass="btn btn-success btn-sm" OnClick="btnPosting_Click"><i class="fa fa-arrow-circle-o-up"></i>&nbsp;Post</asp:LinkButton>
                        <asp:LinkButton ID="btnCancel" runat="server" ForeColor="White" CssClass="btn btn-danger btn-sm" OnClick="btnCancel_Click"><i class="fa fa-undo"></i>&nbsp;Back</asp:LinkButton>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <uc1:wucSearch ID="wucSearch1" runat="server" OnHide="wucSearch_Hide" />
    <uc2:wucConfirmBox ID="wucConfirmBox1" runat="server" OnHide="wucConfirmBox_Hide" />
</asp:Content>