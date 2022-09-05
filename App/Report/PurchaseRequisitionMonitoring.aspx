<%@ Page Title="CMSystem || Purchase Requisition Monitoring" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PurchaseRequisitionMonitoring.aspx.cs" Inherits="Report_PurchaseRequisitionMonitoring" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Wuc/wucSearch.ascx" tagname="wucSearch" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Purchase Requisition Monitoring</h3>
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
                                    <asp:TextBox ID="tbStartTransDate" runat="server" Width="110px" CssClass="form-control"/>
                                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="tbStartTransDate" Format="dd-MM-yyyy"></cc1:CalendarExtender> -
                                    <asp:TextBox ID="tbEndTransDate" runat="server" Width="110px" CssClass="form-control"/>
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
                                <asp:label runat="server">Required Date</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbStartRequiredDate" runat="server" Width="110px" CssClass="form-control"/>
                                    <cc1:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="tbStartRequiredDate" Format="dd-MM-yyyy"></cc1:CalendarExtender> -
                                    <asp:TextBox ID="tbEndRequiredDate" runat="server" Width="110px" CssClass="form-control"/>
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
                                <asp:label runat="server">Status</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:DropDownList ID="ddlStatus" CssClass="form-control" runat="server">
                                        <asp:ListItem Value="ALL" Text="ALL"></asp:ListItem>
                                        <asp:ListItem Value="OPEN" Text="OPEN"></asp:ListItem>
                                        <asp:ListItem Value="POSTED" Text="POSTED"></asp:ListItem>
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
                                <asp:label runat="server">Article No</asp:label>
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
                                <asp:label runat="server">Ordered Status</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:DropDownList ID="ddlOrderedStatus" CssClass="form-control" runat="server">
                                        <asp:ListItem Value="ALL" Text="ALL"></asp:ListItem>
                                        <asp:ListItem Value="OUTSTANDING" Text="OUTSTANDING"></asp:ListItem>
                                        <asp:ListItem Value="COMPLETE" Text="COMPLETE"></asp:ListItem>
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
                                <asp:label runat="server">Note</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbNote" runat="server" Width="250px" CssClass="form-control text-uppercase" />
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
                            DataKeyNames="article_no" EmptyDataText="NO DATA" BackColor="White" OnRowDataBound="grid_RowDataBound">
                            <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" />
                            <EmptyDataRowStyle HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
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
                                    <asp:TemplateField HeaderText="Posted Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lPostedDate" runat="server" Text='<%# Bind("posted_date", "{0:dd MMM yyyy}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ship To" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lShipTo" runat="server" Text='<%# Bind("ship_to") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ship To Name" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lShipToName" runat="server" Text='<%# Bind("ship_to_name") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="250px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Note" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lNote" runat="server" Text='<%# Bind("note") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Line No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lLineNo" runat="server" Text='<%# Bind("line_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="30px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Article No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lArticleNo" runat="server" Text='<%# Bind("article_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Article Desc" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lArticleDesc" runat="server" Text='<%# Bind("article_description") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
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
                                            <asp:Label ID="lQty" runat="server" Text='<%# Bind("qty", "{0:##,#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Required Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lReqDate" runat="server" Text='<%# Bind("required_date", "{0:dd MMM yyyy}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Info Price" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lInfoPrice" runat="server" Text='<%# Bind("info_price", "{0:##,#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Note" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lNoteDetail" runat="server" Text='<%# Bind("note_detail", "{0:##,#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Purchase Order NO" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lNomorPO" runat="server" Text='<%# Bind("nomor_po") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Po Approval Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lPoApprovalDate" runat="server" Text='<%# Bind("tgl_approval_po", "{0:dd MMM yyyy}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="PO QTY" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lpoqty" runat="server" Text='<%# Bind("qtyPO") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Vendor Name" Visible="false" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lPoVendorName" runat="server" Text='<%# Bind("po_vendor_name") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="200px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Req Delv Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lPoReqDelvDate" runat="server" Text='<%# Bind("po_req_delv_date", "{0:dd MMM yyyy}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="100px" />
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

