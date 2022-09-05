<%@ Page Title="CMSystem || Stock Card" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StockCard.aspx.cs" Inherits="Report_StockCard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Wuc/wucSearch.ascx" tagname="wucSearch" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Stock Card</h3>
        </div>
    </div>
    <div class="x_panel">
        <asp:UpdatePanel ID="upHeader" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Site</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbSiteID" runat="server" Width="110px" CssClass="form-control text-uppercase"/>
                                    <asp:ImageButton ID="ibSearchSiteID" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchSiteID_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Period</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbStartDate" runat="server" Width="110px" CssClass="form-control"/>
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="tbStartDate" Format="dd-MM-yyyy"></cc1:CalendarExtender> -
                                    <asp:TextBox ID="tbEndDate" runat="server" Width="110px" CssClass="form-control"/>
                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="tbEndDate" Format="dd-MM-yyyy"></cc1:CalendarExtender>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Article</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbArticleGroup" runat="server" Width="110px" CssClass="form-control text-uppercase"/>
                                    <asp:ImageButton ID="ibSearchArticleGroup" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchArticleGroup_Click" />
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
                            <asp:GridView ID="grid" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%"
                            CssClass="table table-striped table-bordered table-hover"  
                            DataKeyNames="article_no" EmptyDataText="NO DATA" BackColor="White">
                            <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" />
                            <EmptyDataRowStyle HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <AlternatingRowStyle BackColor="White" />
                            <PagerStyle BorderWidth="1px" BackColor="White" ForeColor="#2A3F54" HorizontalAlign="Center" />
                            <FooterStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" HorizontalAlign="Right" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Article No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lArticleNo" runat="server" Text='<%# Bind("article_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
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
                                            <asp:Label ID="lUnitID" runat="server" Text='<%# Bind("unitID") %>' Visible="false"/>
                                            <asp:Label ID="lUnitDesc" runat="server" Text='<%# Bind("unitDesc") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Beg" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lBegBalance" runat="server" Text='<%# Bind("beg_balance", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="In" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lStockIn" runat="server" Text='<%# Bind("stock_in", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Out" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lStockOut" runat="server" Text='<%# Bind("stock_out", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End Balance" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lEndBalanceIncBooked" runat="server" Text='<%# Bind("end_balance", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                            <asp:GridView ID="gridDetail" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%"
                            CssClass="table table-striped table-bordered table-hover"  
                            EmptyDataText="NO DATA" BackColor="White">
                            <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" />
                            <EmptyDataRowStyle HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <AlternatingRowStyle BackColor="White" />
                            <PagerStyle BorderWidth="1px" BackColor="White" ForeColor="#2A3F54" HorizontalAlign="Center" />
                            <FooterStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" HorizontalAlign="Right" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Site ID" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lSiteID" runat="server" Text='<%# Bind("site_id") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site Name" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lSiteName" runat="server" Text='<%# Bind("site_name") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Posted Date" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lPosted_date" runat="server" Text='<%# Bind("posted_date", "{0:dd MMM yyyy}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Document No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lDocumentNo" runat="server" Text='<%# Bind("document_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Source" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lSource" runat="server" Text='<%# Bind("sourcedest") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Article No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lArticleNo" runat="server" Text='<%# Bind("article_no") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="UOM" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lUOM" runat="server" Text='<%# Bind("uom") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lQty" runat="server" Text='<%# Bind("qty", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
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

