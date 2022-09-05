<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BarangKeluarList.aspx.cs" Inherits="Transaction_BarangKeluarList" %>


<%@ Register src="~/Wuc/wucConfirmBox.ascx" tagname="wucConfirmBox" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Outbound Deivery</h3>
        </div>
    </div>
    <div class="x_panel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-inline">
                    <div class="row">
                        <div class="col-sm-12">
                            <asp:LinkButton ID="btnAddNew2" runat="server" CssClass="btn btn-primary btn-sm" ForeColor="White" OnClick="btnAddNew_Click"><i class="fa fa-arrow-up"></i>&nbsp;Add New</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-inline" style="display:inline">
                    <div class="row">
                        <div class="col-sm-12">
                            <div style="float: left">
                                Status
                                <asp:DropDownList ID="ddlFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged">
                                    <asp:ListItem Value="%" Text="ALL"></asp:ListItem>
                                    <asp:ListItem Value="OPEN" Text="OPEN"></asp:ListItem>
                                    <asp:ListItem Value="POSTED" Text="POSTED"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="imgSearch">
                                <div style="float: right">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtSearch" runat="server" placeholder="Search" 
                                        CssClass="form-control" value="" Width="200px" OnTextChanged="imgSearch_Click"/>
                                        <div class="input-group-addon input-group-addon1">
                                            <asp:LinkButton ID="imgSearch" runat="server" OnClick="imgSearch_Click"><i class="fa fa-search"></i></asp:LinkButton><br />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="row">
            <div class="col-sm-12">
                <div>
                    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="grid" runat="server" Width="100%" AutoGenerateColumns="False" 
                                EmptyDataText="NO DATA" DataKeyNames="trans_no" BackColor="White"
                                CssClass="table table-striped table-bordered table-hover"
                                AllowPaging="True" PageSize="10" OnPageIndexChanging="grid_PageIndexChanging" PagerSettings-Visible="false" OnRowDataBound="grid_RowDataBound">
                                <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" />
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <AlternatingRowStyle BackColor="White" />
                                <PagerStyle BorderWidth="1px" BackColor="White" ForeColor="#2A3F54" HorizontalAlign="Center" />
                                <Columns>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="checkall" runat="server" onClick="checkAll(this)" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="checkbox1" runat="server"  onclick = "Check_Click(this)" />
                                    </ItemTemplate>                    
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="1%" />
                                </asp:TemplateField>
                                    <asp:TemplateField HeaderText="No." HeaderStyle-CssClass="text-center">
                                        <ItemStyle HorizontalAlign="right" Width="30px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <%# Convert.ToInt32(DataBinder.Eval(Container, "DataItemIndex")) + 1 %>.
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Trans No" HeaderStyle-CssClass="text-center">
                                        <ItemStyle HorizontalAlign="Left" Width="140px" VerticalAlign="Top"/>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransNo" runat="server" Text='<%# Bind("trans_no") %>' /><br />
                                            <asp:Label ID="lblTransDate" runat="server" Text='<%# Bind("trans_date","{0:dd MMM yyyy}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reff Order No" HeaderStyle-CssClass="text-center">
                                        <ItemStyle HorizontalAlign="Left" Width="" VerticalAlign="Top"/>
                                        <ItemTemplate>
                                            <asp:Label ID="lblReffOrderNo" runat="server" Text='<%# Bind("reff_order_no") %>' /><br />
                                            <asp:Label ID="lblReff" runat="server" Text='<%# Bind("reff") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Source" HeaderStyle-CssClass="text-center">
                                        <ItemStyle HorizontalAlign="Left" Width="" VerticalAlign="Top"/>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFromWarehouse" runat="server" Text='<%# Bind("from_warehouse") %>' Visible="false" />
                                            <asp:Label ID="lblFromWarehouseName" runat="server" Text='<%# Bind("wh_description") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Destination" HeaderStyle-CssClass="text-center">
                                        <ItemStyle HorizontalAlign="Left" Width="" VerticalAlign="Top"/>
                                        <ItemTemplate>
                                            <asp:Label ID="lblShipTo" runat="server" Text='<%# Bind("ship_to") %>' Visible="false" />
                                            <asp:Label ID="lblShipToName" runat="server" Text='<%# Bind("customer_name") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Note" HeaderStyle-CssClass="text-center">
                                        <ItemStyle HorizontalAlign="Left" Width="" VerticalAlign="Top"/>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNote" runat="server" Text='<%# Bind("note") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="30px" VerticalAlign="Top"/>
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("status") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="30px" VerticalAlign="Top"/>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbEdit" runat="server" OnClick="imgEdit_Click"><i class="fa fa-pencil" style="font-size:17px"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="30px" VerticalAlign="Top"/>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbDelete" runat="server" OnClick="imgDelete_Click"><i class="fa fa-trash" style="font-size:17px"></i></asp:LinkButton>
                                            <asp:Label ID="lblDelete" runat="server" Text="--" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <asp:UpdatePanel ID="upFooter" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div style="float: left; width: 100%; text-align: center; margin: 12px 0px 0px 0px;">
                            <u><asp:LinkButton ID="linkFirst" runat="server" CssClass="linktext" ToolTip="click to move to first page" OnClick="linkFirst_Click">First</asp:LinkButton></u>
                            <u><asp:LinkButton ID="linkPrev" runat="server" CssClass="linktext" ToolTip="click to move to previous page" OnClick="linkPrev_Click">Prev</asp:LinkButton></u>
                            <asp:Label ID="lblRecords" runat="server" Text="Records: 1-10" />
                            <u><asp:LinkButton ID="linkNext" runat="server" CssClass="linktext" ToolTip="click to move to next page" OnClick="linkNext_Click">Next</asp:LinkButton></u>
                            <u><asp:LinkButton ID="linkLast" runat="server" CssClass="linktext" ToolTip="click to move to last page" OnClick="linkLast_Click">Last</asp:LinkButton></u>
                            <br />
                            Total Records: <asp:Label ID="lblTotalRecords" runat="server" Text="" />
                        </div>
                        <div style="float: left; width: 100%; margin: 0px 0px 0px 8px;">
                            Page
                            <asp:DropDownList ID="ddlPageOf" runat="server" CssClass="combobox" AutoPostBack="true" OnSelectedIndexChanged="ddlPageOf_SelectedIndexChanged"></asp:DropDownList>
                            <asp:Label ID="lblPageOf" runat="server" Text="" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <uc1:wucConfirmBox ID="wucConfirmBox1" runat="server" OnHide="wucConfirmBox_Hide" />
</asp:Content>

