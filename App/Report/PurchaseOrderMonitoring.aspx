<%@ Page Title="CMSystem || Purchase Order Monitoring" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PurchaseOrderMonitoring.aspx.cs" Inherits="Report_PurchaseOrderMonitoring" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Wuc/wucSearch.ascx" TagName="wucSearch" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Purchase Order Monitoring</h3>
        </div>
    </div>
    <div class="x_panel">
        <asp:UpdatePanel ID="upHeader" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Trans Date</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbStartTransDate" runat="server" Width="110px" CssClass="form-control" />
                                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="tbStartTransDate" Format="dd-MM-yyyy"></cc1:CalendarExtender>
                                    -
                                    <asp:TextBox ID="tbEndTransDate" runat="server" Width="110px" CssClass="form-control" />
                                    <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="tbEndTransDate" Format="dd-MM-yyyy"></cc1:CalendarExtender>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Approved Date</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbStartPostedDate" runat="server" Width="110px" CssClass="form-control" />
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="tbStartPostedDate" Format="dd-MM-yyyy"></cc1:CalendarExtender>
                                    -
                                    <asp:TextBox ID="tbEndPostedDate" runat="server" Width="110px" CssClass="form-control" />
                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="tbEndPostedDate" Format="dd-MM-yyyy"></cc1:CalendarExtender>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Req. Delv Date</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbStartRequiredDate" runat="server" Width="110px" CssClass="form-control" />
                                    <cc1:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="tbStartRequiredDate" Format="dd-MM-yyyy"></cc1:CalendarExtender>
                                    -
                                    <asp:TextBox ID="tbEndRequiredDate" runat="server" Width="110px" CssClass="form-control" />
                                    <cc1:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="tbEndRequiredDate" Format="dd-MM-yyyy"></cc1:CalendarExtender>
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
                                    <asp:TextBox ID="tbVendor" runat="server" Width="120px" CssClass="form-control text-uppercase" />
                                    <asp:ImageButton ID="ibSearchVendor" runat="server" ToolTip="click to search"
                                        ImageUrl="~/images/search.png" OnClick="ibSearchVendor_Click" Height="16px" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Status</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="%" Text="ALL"></asp:ListItem>
                                        <asp:ListItem Value="OPEN" Text="NEED APPROVED"></asp:ListItem>
                                        <asp:ListItem Value="APPROVED" Text="APPROVED"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Article No</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbArticleNo" runat="server" Width="120px" CssClass="form-control text-uppercase" />
                                    <asp:ImageButton ID="ibSearchArticleNo" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchArticleNo_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Received Status</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:DropDownList ID="ddlReceivedStatus" CssClass="form-control" runat="server">
                                        <asp:ListItem Value="ALL" Text="ALL"></asp:ListItem>
                                        <asp:ListItem Value="OUTSTANDING" Text="OUTSTANDING"></asp:ListItem>
                                        <asp:ListItem Value="FULLY RECEIVED" Text="FULLY RECEIVED"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:LinkButton ID="bShow" runat="server" ForeColor="White" OnClick="bShow_Click" CssClass="btn btn-primary btn-sm"><i class=" fa fa-chevron-down"></i>&nbsp;Show</asp:LinkButton>
                                        <asp:LinkButton ID="bExcel" runat="server" ForeColor="White" OnClick="bExcel_Click" CssClass="btn btn-success btn-sm"><i class=" fa fa-file-excel-o"></i>&nbsp;Excel</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="row">
            <div class="col-sm-12">
                <div class="table-responsive">
                    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="grid" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="2000px"
                                CssClass="table table-striped table-bordered table-hover"
                                EmptyDataText="NO DATA" BackColor="White" >
                                <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" />
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <AlternatingRowStyle BackColor="White" />
                                <PagerStyle BorderWidth="1px" BackColor="White" ForeColor="#2A3F54" HorizontalAlign="Center" />
                                <FooterStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" HorizontalAlign="Right" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Trans No / Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lTransNo" runat="server" Text='<%# Bind("trans_no") %>' /><br />
                                            <asp:Label ID="lTransDate" runat="server" Text='<%# Bind("trans_date", "{0:dd MMM yyyy}") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="150px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Approved Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lApprovedDate" runat="server" Text='<%# Bind("posted_date", "{0:dd MMM yyyy}") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Purchase Req. No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lPRTransNo" runat="server" Text='<%# Bind("purchase_requisition_no") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="150px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vendor" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lVendor" runat="server" Text='<%# Bind("vendor_name") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="250px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Req Delv. Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lReqDelvDate" runat="server" Text='<%# Bind("req_delv_date", "{0:dd MMM yyyy}") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ship To" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lShipTo" runat="server" Text='<%# Bind("ship_to") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="250px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payment Term" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lPaymentTerm" runat="server" Text='<%# Bind("payment_term") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lAmount" runat="server" Text='<%# Bind("amount", "{0:##,#0.#0}") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VAT" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lVat" runat="server" Text='<%# Bind("Tax", "{0:##,#0.#0}") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Amount" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lTotalAmount" runat="server" Text='<%# Bind("total_amount", "{0:##,#0.#0}") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Note" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lNote" runat="server" Text='<%# Bind("note") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <uc1:wucSearch ID="wucSearch1" runat="server" OnHide="wucSearch_Hide" />
    </div>
</asp:Content>

