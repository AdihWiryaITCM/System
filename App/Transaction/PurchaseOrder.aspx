<%@ Page Title="CMSystem || Purchase Order" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PurchaseOrder.aspx.cs" Inherits="Transaction_PurchaseOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Wuc/wucConfirmBox.ascx" tagname="wucConfirmBox" tagprefix="uc1" %>

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
            <h3>Purchase Order</h3>
        </div>
    </div>
    <div class="x_panel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row">
                    <div class="col-sm-12">
                    <asp:LinkButton ID="btnAddNew2" ForeColor="White" runat="server" Text="Add New" CssClass="btn btn-primary btn-sm" OnClick="btnAddNew_Click" ><i class="fa fa-arrow-up"></i>&nbsp;Add New</asp:LinkButton>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="upSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-inline" style="display: inline">
                    <div class="row">
                        <div class="col-sm-12">
                            <div style="float: left">
                                Status
                                <asp:DropDownList ID="ddlFilter" Width="110px" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged">
                                    <asp:ListItem Value="" Text="ALL"></asp:ListItem>
                                    <asp:ListItem Value="NEED APPROVE" Text="NEED APPROVE"></asp:ListItem>
                                    <asp:ListItem Value="APPROVED" Text="APPROVED"></asp:ListItem>
                                    <asp:ListItem Value="CANCELLED" Text="CANCELLED"></asp:ListItem>
                                    <asp:ListItem Value="CLOSED" Text="CLOSED"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="imgSearch">
                                <div style="float: right;display:inline" >
                                    <div class="input-group">
                                        <asp:TextBox ID="tbSearch" runat="server" placeholder="Search"
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
                                    <asp:TemplateField HeaderText="No." HeaderStyle-CssClass="text-center">
                                        <ItemStyle HorizontalAlign="right" Width="30px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <%# Convert.ToInt32(DataBinder.Eval(Container, "DataItemIndex")) + 1 %>.
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Trans No. / Date" HeaderStyle-CssClass="text-center">
                                        <ItemStyle HorizontalAlign="Left" Width="140px" VerticalAlign="Top"/>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransNo" runat="server" Text='<%# Bind("trans_no") %>' /><br />
                                            <asp:Label ID="lblTransDate" runat="server" Text='<%# Bind("trans_date","{0:dd MMM yyyy}") %>' />
                                            <asp:Label ID="lblPRNo" runat="server" Text='<%# Bind("purchase_requisition_no") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vendor" HeaderStyle-CssClass="text-center">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="150px"/>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVendorName" runat="server" Text='<%# Bind("vendor") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ship To" HeaderStyle-CssClass="text-center">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="150px" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblShipTo" runat="server" Text='<%# Bind("wh_description") %>' /><br />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Note" HeaderStyle-CssClass="text-center">
                                        <ItemStyle HorizontalAlign="Left" Width="" VerticalAlign="Top"/>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNote" runat="server" Text='<%# Bind("note") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="text-center">
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Bind("total_amount", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="text-center">
                                        <ItemStyle HorizontalAlign="Center" Width="" VerticalAlign="Top"/>
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("status") %>' Visible="false" />
                                            <asp:Label ID="lblApprovalStatus" runat="server" Text='<%# Bind("approval_status") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="30px" VerticalAlign="Top"/>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbEdit" runat="server" OnClick="lbEdit_Click"><i class="fa fa-pencil" style="font-size:17px"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="30px" VerticalAlign="Top"/>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbDelete" runat="server" OnClick="lbDelete_Click"><i class="fa fa-trash" style="font-size:17px"></i></asp:LinkButton>
                                            <asp:Label ID="lblDelete" runat="server" Text="--" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="30px" VerticalAlign="Top"/>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbPrint" runat="server" OnClick="imgPrint_Click"><i class="fa fa-print" style="font-size:17px"></i></asp:LinkButton>
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

