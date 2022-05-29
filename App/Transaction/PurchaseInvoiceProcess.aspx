<%@ Page Title="CMSystem || Purchase Invoice" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PurchaseInvoiceProcess.aspx.cs" Inherits="Transaction_PurchaseInvoiceProcessProcess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Wuc/wucSearch.ascx" tagname="wucSearch" tagprefix="uc1" %>
<%@ Register src="~/Wuc/wucConfirmBox.ascx" tagname="wucConfirmBox" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .change:hover {
            color: darkorange;
            text-decoration: underline;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Purchase Invoice</h3>
        </div>
    </div>
    <div class="x_panel">
        <asp:UpdatePanel ID="upHeader" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Trans No / Date</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbTransNo" CssClass="form-control" runat="server" Width="180px" ReadOnly="true" />
                                    <asp:TextBox ID="tbTransDate" CssClass="form-control" runat="server" Width="110px" ReadOnly="true" />
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
                                <asp:label runat="server">Purchase Order No.</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:Panel ID="pSearchReffOrder" runat="server" DefaultButton="bSearchReffOrder">
                                        <asp:TextBox ID="tbReffOrderNo" runat="server" Width="170px" CssClass="form-control text-uppercase"/>
                                        <asp:LinkButton ID="lbReffOrderNo" runat="server" Text="" OnClick="lbReffOrderNo_Click" CssClass="alert-link"/>
                                        <asp:ImageButton ID="ibSearchReffOrder" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchReffOrder_Click" />
                                        <asp:Button ID="bSearchReffOrder" runat="server" style="display:none;" OnClick="bSearchReffOrder_Click" />
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
                                <asp:label runat="server">Vendor</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbVendorNo" runat="server" Width="110px" CssClass="form-control" ReadOnly="true" />
                                    <asp:LinkButton ID="lbVendorName" runat="server" CssClass="alert-link" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Vendor Invoice No.</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbVendorInvoiceNo" runat="server" Width="230px" CssClass="form-control text-uppercase" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Tax Facture No.</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbTaxFactureNo" runat="server" Width="200px" CssClass="form-control text-uppercase" MaxLength="19"/>
                                    <asp:TextBox ID="tbTglFacture" CssClass="form-control" runat="server" Width="110px"/>
                                    <cc1:CalendarExtender ID="ceInstallationDate" runat="server" TargetControlID="tbTglFacture" Format="dd-MM-yyyy" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server"></asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:Label ID="Label2" runat="server" Font-Bold="true"> (exp : 010.001-18.12345678)</asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Amount</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbACurrencyID" runat="server" CssClass="form-control" Width="70px" ReadOnly="true"/>
                                    <asp:TextBox ID="tbTAmount" runat="server" CssClass="form-control" Width="110px" ReadOnly="true"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">VAT</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbVCurrencyID" runat="server" CssClass="form-control" Width="70px" ReadOnly="true"/>
                                    <asp:TextBox ID="tbTVAT" runat="server" CssClass="form-control" Width="110px" ReadOnly="true"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Total Amount</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbCurrencyID" runat="server" CssClass="form-control" Width="70px" ReadOnly="true"/>
                                    <asp:TextBox ID="tbTotalInvoice" runat="server" CssClass="form-control" Width="110px" ReadOnly="true"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Payment Term</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbPaymentTerm" runat="server" CssClass="form-control" ReadOnly="true" Width="70px"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Due Date</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbDueDate" runat="server" CssClass="form-control" Width="110px"/>
                                    <cc1:CalendarExtender ID="ceDueDate" runat="server" TargetControlID="tbDueDate" Format="dd-MM-yyyy" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Note</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbNote" runat="server" TextMode="MultiLine" Width="250px" CssClass="form-control text-uppercase"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="row">
            <div class="col-sm-12">
                <div>
                    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>       
                            <asp:GridView ID="grid" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%" 
                            DataKeyNames="articleNo" EmptyDataText="NO DATA" BackColor="White" 
                            CssClass="table table-striped table-bordered table-hover">
                            <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" />
                            <EmptyDataRowStyle HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#2A3F54" />
                            <AlternatingRowStyle BackColor="White" />
                            <PagerStyle BorderWidth="1px" BackColor="White" ForeColor="#2A3F54" HorizontalAlign="Center" />
                            <FooterStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" HorizontalAlign="Right" />
                                <Columns>
                                    <asp:TemplateField HeaderText="ID Trans No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lIDTransNo" runat="server" Text='<%# Bind("idTransNo") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="150px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Article No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lArticleNo" runat="server" Text='<%# Bind("articleNo") %>'/><br />
                                            <asp:Label ID="lArticleDesc" runat="server" Text='<%# Bind("articleDesc") %>'/>
                                            <asp:Label ID="lArticleType" runat="server" Text='<%# Bind("articleType") %>' Visible="false"/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty Order" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lQtyOrder" runat="server" Text='<%# Bind("qty_order", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty Received" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lQtyReceived" runat="server" Text='<%# Bind("qty_received", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit Price" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lUnitPrice" runat="server" Text='<%# Bind("unit_price", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VAT" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lVAT" runat="server" Text='<%# Bind("tax", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="tbTotalAmount" runat="server" Text='<%# Bind("total_amount", "{0:##,#0.#0}") %>' Visible="false"/>
                                              <asp:Label ID="Label1" runat="server" Text='<%# Bind("Total_unit_price", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Note" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbNoteDetail" runat="server" Width="100%" Text='<%# Bind("note") %>' CssClass="form-control text-uppercase"/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="upDetail" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Inbound Delivery No</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:Panel ID="pSearchIDTransNo" runat="server" DefaultButton="bSearchIDTransNo">
                                        <asp:TextBox ID="tbIDTransNo" runat="server" Width="170px" CssClass="form-control text-uppercase"/>
                                        <asp:LinkButton ID="lbIDTransNo" runat="server" Text="" CssClass="alert-link" OnClick="lbIDTransNo_Click"/>
                                        <asp:ImageButton ID="ibSearchIDTransNo" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchIDTransNo_Click" />
                                        <asp:Button ID="bSearchIDTransNo" runat="server" style="display:none;" OnClick="bSearchIDTransNo_Click" />
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
                                <asp:Label runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <asp:LinkButton ID="bAdd" ForeColor="White" runat="server" CssClass="btn btn-primary btn-sm" onclick="bAdd_Click" ><i class="fa fa-plus"></i>&nbsp;Add</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <div class="row text-center">
                        <div class="col-sm-12 buttonSet">
                            <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="btn btn-primary btn-sm" ForeColor="White" ><i class="fa fa-save"></i>&nbsp;Save</asp:LinkButton>
                            <asp:LinkButton ID="btnPosting" runat="server" OnClick="btnPosting_Click" CssClass="btn btn-success btn-sm" ForeColor="White" ><i class="fa fa-arrow-circle-o-up"></i>&nbsp;Post</asp:LinkButton>
                            <asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="btn btn-danger btn-sm" ForeColor="White" CausesValidation="false"><i class="fa fa-undo"></i>&nbsp;Back</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <uc1:wucSearch ID="wucSearch1" runat="server" OnHide="wucSearch_Hide" />
        <uc2:wucConfirmBox ID="wucConfirmBox1" runat="server" OnHide="wucConfirmBox_Hide" />
    </div>
</asp:Content>

