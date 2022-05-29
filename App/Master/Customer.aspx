<%@ Page Title="CMSystem || Master Customer" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Customer.aspx.cs" Inherits="Master_Customer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Wuc/wucSearch.ascx" TagName="wucSearch" TagPrefix="uc1" %>
<%@ Register Src="~/wuc/wucConfirmBox.ascx" TagName="wucConfirmBox" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Customer</h3>
        </div>
    </div>
    <div class="x_panel">
        <asp:UpdatePanel ID="upHeader" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:Label runat="server">Customer No </asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:Panel ID="pSearchCustomerNo" runat="server" DefaultButton="bSearchCustomerNo">
                                    <asp:TextBox ID="tbCustomerID" runat="server" Width="250px" ReadOnly="true" CssClass="form-control" />
                                    <asp:ImageButton ID="ibSearchCustomerNo" runat="server" ImageUrl="~/Images/search.png" OnClick="ibSearchCustomerNo_Click" />
                                    <div class="form-inline">
                                        <i>(fill blank if add new)</i>
                                        <asp:Button ID="bSearchCustomerNo" runat="server" Style="display: none;" OnClick="bSearchCustomerNo_Click" />
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="form-inline">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Name </asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbCustomerName" runat="server" Width="250px" CssClass="form-control toUpper" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="form-inline">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Phone No  </asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <asp:TextBox ID="tbPhoneNo1" runat="server" Width="245px" CssClass="form-control toUpper" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="form-inline">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Email  </asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <asp:TextBox ID="tbEmail" runat="server" Width="245px" CssClass="form-control toUpper" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="form-inline">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Nama NPWP  </asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <asp:TextBox ID="tbNamaNPWP" runat="server" Width="245px" CssClass="form-control toUpper" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="form-inline">
                            <div class="col-sm-2">
                                <asp:Label runat="server">VAT Reg. No  </asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <asp:TextBox ID="tbVatRegNo" runat="server" Width="245px" CssClass="form-control toUpper" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group row">
                    <div class="col-sm-12">
                        <div class="form-inline">
                            <div class="col-sm-2">
                                <asp:Label runat="server">VAT Reg. Address  </asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <asp:TextBox ID="tbTaxAddr" runat="server" Width="245px" TextMode="MultiLine" CssClass="form-control toUpper" />
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
        <asp:UpdatePanel ID="UpButton" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row">
                    <div class="col-sm-12  text-center">
                        <div class="buttonSet">
                            <div>
                                <asp:LinkButton ID="lbSavea" runat="server" OnClick="lbSaveHeader_Click" CssClass="btn btn-primary btn-sm" ForeColor="White"><i class="fa fa-save"></i>&nbsp;Save</asp:LinkButton>
                                <asp:LinkButton ID="lbClear" runat="server" OnClick="lbClearHeader_Click" CssClass="btn btn-danger btn-sm" ForeColor="White"><i class="fa fa-close"></i>&nbsp;Clear</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
        <uc1:wucSearch ID="wucSearch1" runat="server" OnHide="wucSearch_Hide" />
        <uc2:wucConfirmBox ID="wucConfirmBox1" runat="server" OnHide="wucConfirmBox_Hide" />
</asp:Content>