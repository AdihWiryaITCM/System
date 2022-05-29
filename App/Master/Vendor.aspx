<%@ Page Title="CMSystem || Vendor" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Vendor.aspx.cs" Inherits="Master_Vendor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Wuc/wucSearch.ascx" TagName="wucSearch" TagPrefix="uc1" %>
<%@ Register Src="~/wuc/wucConfirmBox.ascx" TagName="wucConfirmBox" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Vendor</h3>
        </div>
    </div>
    <div class="x_panel">
        <asp:UpdatePanel ID="upHeader" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pSearchVendorNo" runat="server" DefaultButton="bSearchVendorNo">
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Vendor No </asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbVendorID" runat="server" Width="100px" ReadOnly="true" CssClass="form-control" />
                                    <asp:Button ID="bSearchVendorNo" runat="server" Style="display: none;" OnClick="bSearchVendorNo_Click" />
                                    <asp:ImageButton ID="ibSearchVendorNo" runat="server" ImageUrl="~/Images/search.png" OnClick="ibSearchVendorNo_Click" />
                                    <div class="form-group">
                                        <i>(fill blank if add new)</i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:Label runat="server">Name </asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:TextBox ID="tbVendorName" runat="server" Width="250px" CssClass="form-control toUpper" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:Label runat="server">Additional Name </asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:TextBox ID="tbAddName" runat="server" Width="250px" CssClass="form-control toUpper" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:Label runat="server">Street Address </asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:TextBox ID="tbStreetAddress" runat="server" Width="245px" TextMode="MultiLine" CssClass="form-control toUpper" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:Label runat="server">Phone No </asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:TextBox ID="tbPhoneNo1" runat="server" Width="245px" CssClass="form-control toUpper" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:Label runat="server">Mobile Phone No </asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:TextBox ID="tbPhoneNo2" runat="server" Width="245px" CssClass="form-control toUpper" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:Label runat="server">Email </asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:TextBox ID="tbEmail1" runat="server" Width="245px" CssClass="form-control toUpper" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:Label runat="server">VAT Reg. No </asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:TextBox ID="tbVatRegNo" runat="server" Width="245px" CssClass="form-control toUpper" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:Label runat="server">Terms of Payment </asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:DropDownList ID="ddlPaymentTerm" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:Label runat="server">Bank Name</asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:TextBox ID="tbBankName" runat="server" Width="245px" CssClass="form-control toUpper" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:Label runat="server">Bank Account No.</asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:TextBox ID="tbBankAcc" runat="server" Width="245px" CssClass="form-control toUpper" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:Label runat="server">Tax Rate</asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                              <asp:TextBox ID="tbTaxRate" runat="server" Width="45px" CssClass="form-control toUpper" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="form-inline">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <label for="lTaxRate"> </label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-group">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="form-inline">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Status </asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <asp:DropDownList runat="server" ID="ddlStatus" CssClass="form-control">
                                    <asp:ListItem Value="1" Text="Active"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="Inactive"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="text-center buttonSet">
                            <div>
                                <asp:LinkButton ID="lbSave" runat="server" OnClick="lbSave_Click" CssClass="btn btn-primary btn-sm" ForeColor="White" ValidationGroup="save"><i class="fa fa-save"></i>&nbsp;Save</asp:LinkButton>
                                <asp:LinkButton ID="lbClear" runat="server" OnClick="lbClear_Click" CssClass="btn btn-danger btn-sm" ForeColor="White"><i class="fa fa-undo"></i>&nbsp;Clear</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="float: left; width: 100%; height: 20px">&nbsp;</div>
        <uc1:wucSearch ID="wucSearch1" runat="server" OnHide="wucSearch_Hide" />
        <uc2:wucConfirmBox ID="wucConfirmBox1" runat="server" OnHide="wucConfirmBox_Hide" />
    </div>
</asp:Content>