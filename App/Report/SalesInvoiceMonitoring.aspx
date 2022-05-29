<%@ Page Title="CMSystem || Sales Invoice Monitoring" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SalesInvoiceMonitoring.aspx.cs" Inherits="Report_SalesInvoiceMonitoring" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Wuc/wucSearch.ascx" tagname="wucSearch" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Sales Invoice Monitoring</h3>
        </div>
    </div>
    <div class="x_panel">
        <asp:UpdatePanel ID="upHeader" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Customer</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbCustomer" runat="server" Width="100px" CssClass="form-control text-uppercase"/>
                                    <asp:ImageButton ID="ibSearchCustomer" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchCustomer_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Cust. Bill To SIte</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbCustBillToSite" runat="server" Width="100px" CssClass="form-control text-uppercase"/>
                                    <asp:ImageButton ID="ibSearchCustBillToSite" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchCustBillToSite_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Receipt Date</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbStartDueDate" runat="server" Width="110px" CssClass="form-control"/>
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="tbStartDueDate" Format="dd-MM-yyyy"></cc1:CalendarExtender> -
                                    <asp:TextBox ID="tbEndDueDate" runat="server" Width="110px" CssClass="form-control"/>
                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="tbEndDueDate" Format="dd-MM-yyyy"></cc1:CalendarExtender>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Key Word</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbKeyWord" runat="server" Width="250px" CssClass="form-control text-uppercase"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label ID="Label1" runat="server">Payment Status</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:DropDownList ID="ddlPaymentStatus" CssClass="form-control" runat="server">
                                        <asp:ListItem Value="%" Text="ALL" />
                                        <asp:ListItem Value="OUTSTANDING" Text="OUTSTANDING" />
                                        <asp:ListItem Value="COMPLETE" Text="COMPLETE" />
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
                                        <asp:LInkButton ID="bShow" runat="server" ForeColor = "White" OnClick="bShow_Click" CssClass="btn btn-primary btn-sm"><i class=" fa fa-chevron-down"></i>&nbsp;Show</asp:LInkButton>
                                        <asp:LinkButton ID="bExcel" runat="server" ForeColor = "White" OnClick="bExcel_Click" CssClass="btn btn-success btn-sm"><i class=" fa fa-file-excel-o"></i>&nbsp;Excel</asp:LinkButton>
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
                            <asp:GridView ID="grid" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                            CssClass="table table-striped table-bordered table-hover"  
                            EmptyDataText="NO DATA" BackColor="White" OnRowDataBound="grid_RowDataBound" ShowFooter="false">
                            <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" />
                            <EmptyDataRowStyle HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#2A3F54" />
                            <AlternatingRowStyle BackColor="White" />   
                            <PagerStyle BorderWidth="1px" BackColor="White" ForeColor="#2A3F54" HorizontalAlign="Center" />
                            <FooterStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" HorizontalAlign="Right" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Trans No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lTransNo" runat="server" Text='<%# Bind("trans_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="150px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Trans Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lTransDate" runat="server" Text='<%# Bind("trans_date", "{0:dd MMM yyyy}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Billing Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lPostedDate" runat="server" Text='<%# Bind("posted_date", "{0:dd MMM yyyy}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lCustomer" runat="server" Text='<%# Bind("customer_name") %>'/><br />
                                            <asp:Label ID="lCustomerBillTo" runat="server" Text='<%# Bind("cust_name_bill_to_name") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="200px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sales Order No<br />Cust PO No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lSalesOrderNo" runat="server" Text='<%# Bind("sales_order_no") %>'/>
                                            <asp:Label ID="lCustPONo" runat="server" Text='<%# Bind("cust_po_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="150px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Note" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lNote" runat="server" Text='<%# Bind("note") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lAmount" runat="server" Text='<%# Bind("total_amount", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lTotalAmount" runat="server" CssClass="rows" Text="0.00" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BM" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lBMNo" runat="server" Text='<%# Bind("bm_no") %>'/><br />
                                            <asp:Label ID="lBMDate" runat="server" Text='<%# Bind("bm_date", "{0:dd MMM yyyy}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="150px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BM Amount" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lBMAmount" runat="server" Text='<%# Bind("bm_amount", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lTotalBMAmount" runat="server" CssClass="rows" Text="0.00" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
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

