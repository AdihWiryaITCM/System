<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BarangKeluar.aspx.cs" Inherits="Transaction_BarangKeluar" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Wuc/wucSearch.ascx" TagName="wucSearch" TagPrefix="uc1" %>
<%@ Register Src="~/Wuc/wucConfirmBox.ascx" TagName="wucConfirmBox" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Outbound Deivery</h3>
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
                                <asp:Label runat="server">Reff Order No.</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:Panel ID="pSearchReffOrder" runat="server" DefaultButton="bSearchReffOrder">
                                        <asp:TextBox ID="tbReffOrderNo" runat="server" Width="170px" CssClass="form-control text-uppercase" />
                                        <asp:LinkButton ID="lbReffOrderNo" runat="server" Text="" OnClick="lbReffOrderNo_Click" CssClass="alert-link" />
                                        <asp:LinkButton ID="lbReff" runat="server" Text="" CssClass="alert-link" Visible="false" />
                                        <asp:LinkButton ID="lbInstallationBy" runat="server" Text="" CssClass="alert-link" Visible="false" />
                                        <asp:ImageButton ID="ibSearchReffOrder" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchReffOrder_Click" />
                                        <asp:Button ID="bSearchReffOrder" runat="server" Style="display: none;" OnClick="bSearchReffOrder_Click" />
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
                                <asp:Label runat="server">Source</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbFromWH" runat="server" Width="100px" CssClass="form-control text-uppercase" ReadOnly="true" />
                                    <asp:LinkButton ID="lbFromWH" runat="server" Text="" CssClass="alert-link" />
                                    <asp:ImageButton ID="ibSearchFromWH" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchFromWH_Click" />
                                    <asp:Button ID="bSearchFromWH" runat="server" Style="display: none;" OnClick="bSearchFromWH_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:Label runat="server">Destination</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbShipTo" runat="server" Width="100px" CssClass="form-control text-uppercase" ReadOnly="true" />
                                    <asp:TextBox ID="tbShipToSite" runat="server" CssClass="form-control text-uppercase" ReadOnly="true" Visible="false" />
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
                                <asp:Label runat="server">Real Delivery Date</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbRealDeliveryDate" CssClass="form-control" runat="server" Width="110px" />
                                    <cc1:CalendarExtender ID="ceRealDeliveryDate" runat="server" TargetControlID="tbRealDeliveryDate" Format="dd-MM-yyyy" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <asp:Panel ID="pMovementType" runat="server" Visible="false" DefaultButton="bSearchMovementType">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="col-sm-2">
                                    <asp:Label runat="server">Movement Type</asp:Label>
                                </div>
                                <div class="col-sm-10">
                                    <div class="form-inline">
                                        <asp:Panel ID="pSearchMovementType" runat="server" DefaultButton="bSearchMovementType">
                                            <asp:TextBox ID="tbMovementType" runat="server" Text="Z01" Width="80px" CssClass="form-control text-uppercase" />
                                            <asp:LinkButton ID="lbMovementType" runat="server" Text="Goods issue for rental order" OnClick="lbMovementType_Click" CssClass="alert-link" />
                                            <asp:ImageButton ID="ibSearchMovementType" runat="server" Visible="false" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchMovementType_Click" />
                                            <asp:Button ID="bSearchMovementType" runat="server" Style="display: none;" OnClick="bSearchMovementType_Click" />
                                        </asp:Panel>
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
        <asp:UpdatePanel ID="upDetail" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-sm-12">
                                <div>
                                    <asp:GridView ID="grid" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%"
                                        CssClass="table table-striped table-bordered table-hover"
                                        DataKeyNames="articleNo" EmptyDataText="NO DATA" BackColor="White" OnRowDataBound="grid_RowDataBound">
                                        <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <PagerStyle BorderWidth="1px" BackColor="White" ForeColor="#2A3F54" HorizontalAlign="Center" />
                                        <FooterStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." HeaderStyle-CssClass="text-center">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <%# Convert.ToInt32(DataBinder.Eval(Container, "DataItemIndex")) + 1 %>.
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="20px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Article No" HeaderStyle-CssClass="text-center">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lLineNo" runat="server" Text='<%# Bind("lineNo") %>' Visible="false" />
                                                    <asp:Label ID="lArticleNo" runat="server" Text='<%# Bind("articleNo") %>' />
                                                    <asp:Label ID="lArticleType" runat="server" Text='<%# Bind("articleType") %>' Visible="false" />
                                                    <asp:Label ID="lArticleSource" runat="server" Text='<%# Bind("article_source") %>' Visible="false" />
                                                    <asp:Label ID="lManagedItemBy" runat="server" Text='<%# Bind("managed_item_by") %>' Visible="false" />
                                                    <asp:Label ID="lReffLineNo" runat="server" Text='<%# Bind("reffLineNo") %>' Visible="false" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Article Desc" HeaderStyle-CssClass="text-center">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lArticleDesc" runat="server" Text='<%# Bind("articleDesc") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty Ordered" HeaderStyle-CssClass="text-center">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lQtyOrdered" runat="server" Text='<%# Bind("qty_ordered", "{0:##,#0.#0}") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty Delivery" HeaderStyle-CssClass="text-center">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="tbQtyDelivered" runat="server" Text='<%# Bind("qty_delivered", "{0:##,#0.#0}") %>' CssClass="form-control numbox" Width="97%" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Serial No" HeaderStyle-CssClass="text-center">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Panel ID="pSearchSerialNo" runat="server" DefaultButton="bSearchSerialNo">
                                                        <asp:TextBox ID="tbSerialNo" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Bind("serial_no") %>' Width="120px" />
                                                        <asp:LinkButton ID="lbSerialNo" runat="server" Text='<%# Bind("serial_no") %>' CssClass="linkDescription" Visible="false" OnClick="lbSerialNo_Click" />
                                                        <asp:ImageButton ID="ibSearchSerialNo" runat="server" ImageUrl="~/Images/search.png" OnClick="ibSearchSerialNo_Click" />
                                                        <asp:Button ID="bSearchSerialNo" runat="server" Style="display: none;" OnClick="bSearchSerialNo_Click" />
                                                    </asp:Panel>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Note" HeaderStyle-CssClass="text-center">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="tbNoteDetail" runat="server" Text='<%# Bind("noteDetail") %>' TextMode="MultiLine" CssClass="form-control toUpper" Width="95%" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <div class="row text-center">
                        <div class="col-sm-12 buttonSet">
                            <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-primary btn-sm" ForeColor="White" OnClick="btnSave_Click"><i class="fa fa-save"></i>&nbsp;Save</asp:LinkButton>
                            <asp:LinkButton ID="btnPosting" runat="server" CssClass="btn btn-success btn-sm" ForeColor="White" OnClick="btnPosting_Click"><i class="fa fa-arrow-circle-o-up"></i>&nbsp;Post</asp:LinkButton>
                            <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-danger btn-sm" ForeColor="White" OnClick="btnCancel_Click" CausesValidation="false"><i class="fa fa-undo"></i>&nbsp;Back</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <uc1:wucSearch ID="wucSearch1" runat="server" OnHide="wucSearch_Hide" />
    <uc2:wucConfirmBox ID="wucConfirmBox1" runat="server" OnHide="wucConfirmBox_Hide" />
</asp:Content>