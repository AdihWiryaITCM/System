<%@ Page Title="eRental || Rental Order" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RentalOrderProcess.aspx.cs" Inherits="Transaction_RentalOrderProcess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Wuc/wucSearch.ascx" TagName="wucSearch" TagPrefix="uc1" %>
<%@ Register Src="~/Wuc/wucConfirmBox.ascx" TagName="wucConfirmBox" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Rental Order</h3>
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
                                    <asp:TextBox ID="tbTransNo" runat="server" Width="160px" CssClass="form-control" ReadOnly="true" />
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
                                <asp:Label runat="server">Quotation Number</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="bSearchQuot">
                                        <asp:TextBox ID="tbSearchQuot" runat="server" Width="150px" CssClass="form-control text-uppercase" />
                                        <asp:LinkButton ID="lbSearchQuot" runat="server" Text="" OnClick="lbSearchQuot_Click" CssClass="alert-link" />
                                        <asp:ImageButton ID="ibSearchQuot" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchQuot_Click" />
                                        <asp:Button ID="bSearchQuot" runat="server" Style="display: none;" OnClick="bSearchQuot_Click" />
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
                                <asp:Label runat="server">Rent To</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbSoldTo" runat="server" Width="110px" CssClass="form-control text-uppercase" />
                                    <asp:LinkButton ID="lbSoldTo" runat="server" Text="" CssClass="alert-link" />
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
                                    <asp:TextBox ID="tbBillTo" runat="server" Width="80px" CssClass="form-control text-uppercase" />
                                    <asp:LinkButton ID="lbBillTo" runat="server" Text="" CssClass="alert-link" />
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
                                    <asp:TextBox ID="tbShipTo" runat="server" Width="60px" CssClass="form-control text-uppercase" />
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
                                <asp:Label runat="server">Cust PO No / Date</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbCustPONo" runat="server" Width="200px" CssClass="form-control text-uppercase" />
                                    <asp:TextBox ID="tbCustPODate" runat="server" Width="110px" CssClass="form-control" />
                                    <cc1:CalendarExtender ID="ceCustPODate" runat="server" TargetControlID="tbCustPODate" Format="dd-MM-yyyy" />
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
                                    <asp:TextBox ID="tbPaymentTerm" runat="server" Width="80px" CssClass="form-control text-uppercase" />
                                    <asp:LinkButton ID="lbPaymentTerm" runat="server" Text="" CssClass="alert-link" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:Panel runat="server" Visible="false">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="col-sm-2">
                                    <asp:Label runat="server">Installation By</asp:Label>
                                </div>
                                <div class="col-sm-10">
                                    <div class="form-inline">
                                        <asp:DropDownList ID="ddlInstallationBy" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlInstallationBy_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:DropDownList ID="ddlInstallationBySite" CssClass="form-control" runat="server"></asp:DropDownList>
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
                                    <asp:TextBox ID="tbCurrency" runat="server" Width="70px" Text="IDR" CssClass="form-control" ReadOnly="true" />
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
                                    <asp:TextBox ID="tbInstallationCharge" runat="server" Width="110px" CssClass="form-control" Text="0" ReadOnly="true" />
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
                                    <asp:TextBox ID="tbAmt" runat="server" CssClass="form-control" Width="100px" ReadOnly="true" Text="0" />
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
                                    <asp:TextBox ID="tbTax" runat="server" Width="100px" CssClass="form-control" ReadOnly="true" Text="0" />
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
                                    <asp:TextBox ID="tbNetValue" runat="server" CssClass="form-control" Width="100px" ReadOnly="true" Text="0" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Discount</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbDisc" runat="server" Width="100px" CssClass="form-control" ReadOnly="true" Text="0" />
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
        <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="table-responsive">
                            <div>
                                <asp:GridView ID="grid" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%"
                                    CssClass="table table-striped table-bordered table-hover"
                                    DataKeyNames="articleNo" EmptyDataText="NO DATA" BackColor="White" OnRowDataBound="grid_RowDataBound">
                                    <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" />
                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#2A3F54" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <PagerStyle BorderWidth="1px" BackColor="White" ForeColor="#2A3F54" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" HorizontalAlign="Right" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="No.">
                                            <ItemStyle HorizontalAlign="right" Width="30px" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <%# Convert.ToInt32(DataBinder.Eval(Container, "DataItemIndex")) + 1 %>.
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Article">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lLineNo" runat="server" Text='<%# Bind("lineNo") %>' Visible="false" />
                                                <asp:Label ID="lArticleNo" runat="server" Text='<%# Bind("articleNo") %>' /><br />
                                                <asp:Label ID="lArticleDesc" runat="server" Text='<%# Bind("articleDesc") %>' />
                                                <asp:Label ID="lArticleType" runat="server" Text='<%# Bind("articleType") %>' Visible="false" />
                                                <asp:Label ID="lReffLineNo" runat="server" Text='<%# Bind("reffLineNo") %>' Visible="false" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lQty" runat="server" Text='<%# Bind("qty", "{0:##,#0}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="40px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rent Duration">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lTimeDuration" runat="server" Text='<%# Bind("timeDuration", "{0:##,#0}") %>' />
                                                <asp:Label ID="lTimeUnitID" runat="server" Text='<%# Bind("timeUnitID") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="60px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rent Rate">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lRate" runat="server" Text='<%# Bind("rate", "{0:##,#0.#0}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VAT">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lTax" runat="server" Text='<%# Bind("tax", "{0:##,#0.#0}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Disc">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lDisc" runat="server" Text='<%# Bind("disc", "{0:##,#0.#0}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lAmount" runat="server" Text='<%# Bind("totalAmount", "{0:##,#0.#0}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Source">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Panel ID="pSearchSource" runat="server" DefaultButton="bSearchSource">
                                                    <asp:TextBox ID="tbSourceID" runat="server" Width="70%" CssClass="form-control" Text='<%# Bind("sourceID") %>' />
                                                    <asp:LinkButton ID="lbSourceDesc" runat="server" Text='<%# Bind("sourceDesc") %>' CssClass="alert-link" OnClick="lbSourceDesc_Click" />
                                                    <asp:ImageButton ID="ibSearchSource" runat="server" ImageUrl="~/Images/search.png" OnClick="ibSearchSource_Click" />
                                                    <asp:Button ID="bSearchSource" runat="server" Style="display: none;" />
                                                </asp:Panel>
                                                <asp:Panel ID="pSearchWH" runat="server" DefaultButton="bSearchWH" Visible="false">
                                                    <asp:TextBox ID="tbWHID" runat="server" Width="70%" CssClass="form-control" Text='<%# Bind("WHID") %>' />
                                                    <asp:LinkButton ID="lbWHDesc" runat="server" Text='<%# Bind("WHName") %>' CssClass="alert-link" OnClick="lbWHDesc_Click" />
                                                    <asp:ImageButton ID="ibSearchWH" runat="server" ImageUrl="~/Images/search.png" OnClick="ibSearchWH_Click" />
                                                    <asp:Button ID="bSearchWH" runat="server" Style="display: none;" />
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="120px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="text-center buttonSet">
                            <asp:LinkButton ID="btnSave" runat="server" ForeColor="White" CssClass="btn btn-primary btn-sm" OnClick="btnSave_Click"><i class="fa fa-save"></i>&nbsp;Save</asp:LinkButton>
                            <asp:LinkButton ID="btnPosting" runat="server" ForeColor="White" CssClass="btn btn-success btn-sm" OnClick="btnPosting_Click"><i class="fa fa-arrow-circle-o-up"></i>&nbsp;Post</asp:LinkButton>
                            <asp:LinkButton ID="btnCancel" runat="server" ForeColor="White" CssClass="btn btn-danger btn-sm" OnClick="btnCancel_Click"><i class="fa fa-undo"></i>&nbsp;Back</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <uc1:wucSearch ID="wucSearch1" runat="server" OnHide="wucSearch_Hide" />
        <uc2:wucConfirmBox ID="wucConfirmBox1" runat="server" OnHide="wucConfirmBox_Hide" />
    </div>
</asp:Content>