<%@ Page Title="CMSystem || Vendor Invoice Monitoring" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VendorInvoiceMonitoring.aspx.cs" Inherits="Report_VendorInvoiceMonitoring" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Wuc/wucSearch.ascx" tagname="wucSearch" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Purchase invoice Monitoring</h3>
        </div>
    </div>
    <div class="x_panel">
        <asp:UpdatePanel ID="upHeader" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Vendor</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbVendor" runat="server" Width="100px" CssClass="form-control text-uppercase"/>
                                    <asp:ImageButton ID="ibSearchVendor" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchVendor_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Trans Date</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbStartTransDate" runat="server" CssClass="form-control" Width="110px"/>
                                    <cc1:CalendarExtender ID="CalendarExtender9" runat="server" TargetControlID="tbStartTransDate" Format="dd-MM-yyyy"></cc1:CalendarExtender> -
                                    <asp:TextBox ID="tbEndTransDate" runat="server" CssClass="form-control" Width="110px"/>
                                    <cc1:CalendarExtender ID="CalendarExtender10" runat="server" TargetControlID="tbEndTransDate" Format="dd-MM-yyyy"></cc1:CalendarExtender>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Posted Date</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbStartInvoiceDate" runat="server" CssClass="form-control" Width="110px"/>
                                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="tbStartInvoiceDate" Format="dd-MM-yyyy"></cc1:CalendarExtender> -
                                    <asp:TextBox ID="tbEndInvoiceDate" runat="server" CssClass="form-control" Width="110px"/>
                                    <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="tbEndInvoiceDate" Format="dd-MM-yyyy"></cc1:CalendarExtender>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Purchase Order No</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbPurchaseOrderNo" runat="server" Width="180px" CssClass="form-control text-uppercase"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Inbound Delivery No</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbInboundDeliveryNo" runat="server" Width="180px" CssClass="form-control"/>
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
                                    <asp:TextBox ID="tbStartDueDate" runat="server" CssClass="form-control" Width="110px"/>
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="tbStartDueDate" Format="dd-MM-yyyy"></cc1:CalendarExtender> -
                                    <asp:TextBox ID="tbEndDueDate" runat="server" CssClass="form-control" Width="110px"/>
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
                                <asp:label runat="server">BK Date</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbStartBKDate" runat="server" CssClass="form-control" Width="110px"/>
                                    <cc1:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="tbStartBKDate" Format="dd-MM-yyyy"></cc1:CalendarExtender> -
                                    <asp:TextBox ID="tbEndBKDate" runat="server" CssClass="form-control" Width="110px"/>
                                    <cc1:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="tbEndBKDate" Format="dd-MM-yyyy"></cc1:CalendarExtender>
                                </div>
                            </div>
                        </div>
                    </div>
                </div> 
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Status</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:DropDownList ID="ddlStatus" CssClass="form-control" runat="server">
                                        <asp:ListItem Value="%" Text="ALL"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="POSTED"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="OPEN"></asp:ListItem>
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
                                <asp:label runat="server">Payment Status</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:DropDownList ID="ddlInvStatus" CssClass="form-control" runat="server">
                                        <asp:ListItem Value="%" Text="ALL"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="OUTSTANDING"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="COMPLETE"></asp:ListItem>
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
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:CheckBox ID="cbDetail" runat="server" /> &nbsp;Detail
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
                            <asp:GridView ID="grid" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="3000px"
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
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Posted Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lPostedDate" runat="server" Text='<%# Bind("posted_date", "{0:dd MMM yyyy HH:mm}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="120px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Purchase Order No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lPurchaseOrderNo" runat="server" Text='<%# Bind("reff_order_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Inbound Delivery No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lInboundDeliveryNo" runat="server" Text='<%# Bind("inbound_delivery_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vendor No." HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lVendorNo" runat="server" Text='<%# Bind("vendor_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vendor" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lVendor" runat="server" Text='<%# Bind("vendor_name") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lInvoiceDate" runat="server" Text='<%# Bind("invoice_date", "{0:dd MMM yyyy}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax Facture No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lTaxFactureNo" runat="server" Text='<%# Bind("tax_facture_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lVendorInvoiceNo" runat="server" Text='<%# Bind("vendor_invoice_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ship To" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lShipTo" runat="server" Text='<%# Bind("ship_to") %>'/><br />
                                            <asp:Label ID="lShipToSite" runat="server" Text='<%# Bind("ship_to_site") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Curr" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lCurrencyID" runat="server" Text='<%# Bind("currency_id") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="30px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lDPP_Amount" runat="server" Text='<%# Bind("DPP_AMOUNT", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lDppAmount" runat="server" CssClass="rows" Text="0" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PPn" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lTax" runat="server" Text='<%# Bind("TAX", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lTax" runat="server" CssClass="rows" Text="0" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Amount" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lAmount" runat="server" Text='<%# Bind("total_amount", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lGrandTotalInvoice" runat="server" CssClass="rows" Text="0" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Outstanding" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalOutstanding" runat="server" Text='<%# Bind("outstanding", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lGrandTotalOutstanding" runat="server" CssClass="rows" Text="0" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Due Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lDueDate" runat="server" Text='<%# Bind("due_date", "{0:dd MMM yyyy}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Note" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lNote" runat="server" Text='<%# Bind("note") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Created By" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lCreatedBy" runat="server" Text='<%# Bind("created_by") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Posted By" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lReceivedBy" runat="server" Text='<%# Bind("received_by") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BK No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lBKNo" runat="server" Text='<%# Bind("bk_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BK Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lBKDate" runat="server" Text='<%# Bind("bk_date", "{0:dd MMM yyyy}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BK Amount" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lBKAmount" runat="server" Text='<%# Bind("bk_amount", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lGrandTotalBK" runat="server" CssClass="rows" Text="0" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BK Update By" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lBKUpdateBy" runat="server" Text='<%# Bind("bk_update_by") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BK Update Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lBKUpdateDate" runat="server" Text='<%# Bind("bk_update_date", "{0:dd MMM yyyy HH:mm}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="120px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID Posted Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lIDPostedDate" runat="server" Text='<%# Bind("posted_date", "{0:dd MMM yyyy HH:mm}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="120px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:GridView ID="gridDetail" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="4000px"
                            CssClass="table table-striped table-bordered table-hover "   
                            EmptyDataText="NO DATA" BackColor="White" ShowFooter="false">
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
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Posted Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lPostedDate" runat="server" Text='<%# Bind("posted_date", "{0:dd MMM yyyy HH:mm}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="120px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Purchase Order No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lPurchaseOrderNo" runat="server" Text='<%# Bind("reff_order_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Inbound Delivery No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lInboundDeliveryNo" runat="server" Text='<%# Bind("inbound_delivery_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vendor No." HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lVendorNo" runat="server" Text='<%# Bind("vendor_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vendor" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lVendor" runat="server" Text='<%# Bind("vendor_name") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lInvoiceDate" runat="server" Text='<%# Bind("invoice_date", "{0:dd MMM yyyy}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax Facture No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lTaxFactureNo" runat="server" Text='<%# Bind("tax_facture_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lVendorInvoiceNo" runat="server" Text='<%# Bind("vendor_invoice_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ship To" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lShipTo" runat="server" Text='<%# Bind("ship_to") %>'/><br />
                                            <asp:Label ID="lShipToSite" runat="server" Text='<%# Bind("ship_to_site") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Curr" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lCurrencyID" runat="server" Text='<%# Bind("currency_id") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="30px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lDpp_Amount_Detail" runat="server" Text='<%# Bind("DPP_AMOUNT", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lDppAmountDetail" runat="server" CssClass="rows" Text="0" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PPn" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lTax_Detail" runat="server" Text='<%# Bind("TAX", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lTaxDetail" runat="server" CssClass="rows" Text="0" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Amount" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lAmount_Detail" runat="server" Text='<%# Bind("total_amount", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                            <asp:Label ID="lAmountDetail" runat="server" CssClass="rows" Text="0" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Outstanding" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalOutstanding" runat="server" Text='<%# Bind("outstanding", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lGrandTotalOutstanding" runat="server" CssClass="rows" Text="0" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Due Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lDueDate" runat="server" Text='<%# Bind("due_date", "{0:dd MMM yyyy}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Note" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lNote" runat="server" Text='<%# Bind("note") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Created By" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lCreateBy" runat="server" Text='<%# Bind("created_by") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Posted By" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lPostedBy" runat="server" Text='<%# Bind("posted_by") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BK No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lBKNo" runat="server" Text='<%# Bind("bk_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BK Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lBKDate" runat="server" Text='<%# Bind("bk_date", "{0:dd MMM yyyy}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BK Amount" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lBKAmount" runat="server" Text='<%# Bind("bk_amount", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lGrandTotalBK" runat="server" CssClass="rows" Text="0" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BK Update By" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lBKUpdateBy" runat="server" Text='<%# Bind("bk_update_by") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BK Update Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lBKUpdateDate" runat="server" Text='<%# Bind("bk_update_date", "{0:dd MMM yyyy HH:mm}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="120px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Article" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lArticleNo" runat="server" Text='<%# Bind("article_no") %>'/><br />
                                            <asp:Label ID="lArticleDesc" runat="server" Text='<%# Bind("article_description") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="300px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lUnitID" runat="server" Text='<%# Bind("unit_id") %>' Visible="false"/>
                                            <asp:Label ID="lUnitDesc" runat="server" Text='<%# Bind("unit_name") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lQty_Received" runat="server" Text='<%# Bind("qty_received", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lQty" runat="server" CssClass="rows" Text="0" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit Price" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lUnit_Price" runat="server" Text='<%# Bind("unit_price", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lUnitPrice" runat="server" CssClass="rows" Text="0" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Detail Amount" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lTotal" runat="server" Text='<%# Bind("amount", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lGrandTotal" runat="server" CssClass="rows" Text="0" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Note" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lNoteDetail" runat="server" Text='<%# Bind("note_detail", "{0:##,#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="120px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID Posted Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lIDPostedDate" runat="server" Text='<%# Bind("posted_date", "{0:dd MMM yyyy HH:mm}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="120px" />
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

