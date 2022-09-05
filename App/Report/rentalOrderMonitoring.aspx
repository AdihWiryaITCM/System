<%@ Page Title="CMSystem || Rental Order Monitoring" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="rentalOrderMonitoring.aspx.cs" Inherits="Report_rentalOrderMonitoring" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Wuc/wucSearch.ascx" tagname="wucSearch" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Rental Order Monitoring</h3>
        </div>
    </div>
    <div class="x_panel">
        <asp:UpdatePanel ID="upHeader" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:label runat="server">Trans Date</asp:label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:TextBox ID="tbStartTransDate" runat="server" CssClass="form-control" Width="110px"/>
                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="tbStartTransDate" Format="dd-MM-yyyy"></cc1:CalendarExtender> -
                                <asp:TextBox ID="tbEndTransDate" runat="server" CssClass="form-control" Width="110px"/>
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
                            <asp:label runat="server">Posted Date</asp:label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:TextBox ID="tbStartPostedDate" runat="server" Width="110px" CssClass="form-control"/>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="tbStartPostedDate" Format="dd-MM-yyyy"></cc1:CalendarExtender> -
                                <asp:TextBox ID="tbEndPostedDate" runat="server" Width="110px" CssClass="form-control"/>
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
                            <asp:label runat="server">Rent To</asp:label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:TextBox ID="tbSoldTo" runat="server" Width="120px" CssClass="form-control text-uppercase"/>
                                <asp:ImageButton ID="ibSearchSoldTo" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchSoldTo_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:label runat="server">Bill To</asp:label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:TextBox ID="tbBillTo" runat="server" Width="80px" CssClass="form-control text-uppercase"/>
                                <asp:ImageButton ID="ibSearchBillTo" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchBillTo_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="col-sm-2">
                            <asp:label runat="server">Ship To</asp:label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:TextBox ID="tbShipTo" runat="server" Width="80px" CssClass="form-control text-uppercase"/>
                                <asp:ImageButton ID="ibSearchShipTo" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchShipTo_Click" />
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
                                    <asp:ListItem Value="%" Text="ALL" />
                                    <asp:ListItem Value="OPEN" Text="OPEN" />
                                    <asp:ListItem Value="POSTED" Text="POSTED" />
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
                            <asp:label runat="server">Delv. Status</asp:label>
                        </div>
                        <div class="col-sm-10">
                            <div class="form-inline">
                                <asp:DropDownList ID="ddlDelvStatus" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="%" Text="ALL" />
                                    <asp:ListItem Value="OUTSTANDING" Text="OUTSTANDING" />
                                    <asp:ListItem Value="COMPLETE" Text="COMPLETE" />
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
                                <asp:CheckBox ID="cbDetail" runat="server"/>&nbsp;Detail
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
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <AlternatingRowStyle BackColor="White" />
                        <PagerStyle BorderWidth="1px" BackColor="White" ForeColor="#2A3F54" HorizontalAlign="Center" />
                        <FooterStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" HorizontalAlign="Right" />
                            <Columns>
                                <asp:TemplateField HeaderText="Trans No / Date" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lTransNo" runat="server" Text='<%# Bind("trans_no") %>'/><br />
                                        <asp:Label ID="lTransDate" runat="server" Text='<%# Bind("trans_date", "{0:dd MMM yyyy}") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="135px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Posted Date" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lPostedDate" runat="server" Text='<%# Bind("posted_date", "{0:dd MMM yyyy}") %>'/>
                                        <asp:Label ID="lStatus" runat="server" Text='<%# Bind("status") %>' Visible="false"/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lSoldTo" runat="server" Text='<%# Bind("customer_name") %>'/><br />
                                        <asp:Label ID="lBillTo" runat="server" Text='<%# Bind("bill_to") %>'/><br />
                                        <asp:Label ID="lShipTo" runat="server" Text='<%# Bind("ship_to") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="200px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cust. Address" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lStreetAddress" runat="server" Text='<%# Bind("street_address") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="250px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cust PO No. / Date" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lCustPONo" runat="server" Text='<%# Bind("cust_po_no") %>'/><br />
                                        <asp:Label ID="lCustPODate" runat="server" Text='<%# Bind("cust_po_date", "{0:dd MMM yyyy}") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="150" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Req Delv. Date" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lInstallationDate" runat="server" Text='<%# Bind("installation_date", "{0:dd MMM yyyy}") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Installation By" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lInstallationBy" runat="server" Text='<%# Bind("installation_by") %>'/><br />
                                        <asp:Label ID="lInstallationSite" runat="server" Text='<%# Bind("installation_by_site_name") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Curr" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lCurrID" runat="server" Text='<%# Bind("currency_id") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="30px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="120px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lAmount" runat="server" Text='<%# Bind("total_amount", "{0:##,#0.#0}") %>'/>
                                    </ItemTemplate>                        
                                    <FooterTemplate>
                                        <asp:Label ID="lTotalAmount" runat="server" Text="0"/>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Note" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lNote" runat="server" Text='<%# Bind("note") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Article" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lArticleNo" runat="server" Text='<%# Bind("article_no") %>'/><br />
                                        <asp:Label ID="lArticleDesc" runat="server" Text='<%# Bind("article_description") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="300px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="80px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lQty" runat="server" Text='<%# Bind("qty", "{0:##,#0.#0}") %>'/>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lTotalQty" runat="server" Text="0"/>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Duration" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lTimeDuration" runat="server" Text='<%# Bind("time_duration", "{0:##,#0}") %>'/>
                                        <asp:Label ID="lTimeUnit" runat="server" Text='<%# Bind("time_unit") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lRate" runat="server" Text='<%# Bind("rate", "{0:##,#0.#0}") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lTotal" runat="server" Text='<%# Bind("total", "{0:##,#0.#0}") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lDiscount" runat="server" Text='<%# Bind("discount", "{0:##,#0.#0}") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Source" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lSource" runat="server" Text='<%# Bind("source") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Warehouse" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lWarehouse" runat="server" Text='<%# Bind("warehouse") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="OD Real Delv Date" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lPODRealDelvDate" runat="server" Text='<%# Bind("od_real_delivery_date", "{0:dd MMM yyyy}") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="OD Qty" HeaderStyle-CssClass="text-center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lODQty" runat="server" Text='<%# Bind("od_qty", "{0:##,#0.#0}") %>'/>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lTotalODQty" runat="server" Text="0"/>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
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

