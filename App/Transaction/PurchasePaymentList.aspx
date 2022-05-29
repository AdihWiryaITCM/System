<%@ Page Title="CMSystem || Purchase Payment" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PurchasePaymentList.aspx.cs" Inherits="Transaction_PurchasePaymentList" %>

<%@ Register src="~/Wuc/wucConfirmBox.ascx" tagname="wucConfirmBox" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Purchase Payment</h3>
        </div>
    </div>
    <div class="x_panel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" DefaultButton="imgSearch">
                    <div class="form-inline" style="display:inline">
                        <div class="row">
                            <div class="col-sm-12">
                                <div style="float:left">
                                    <asp:LinkButton ID="btnAddNew" ForeColor="White" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnAddNew_Click" ><i class="fa fa-arrow-up"></i>&nbsp;Add New</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="Panel2" runat="server" DefaultButton="imgSearch">
                    <div class="form-inline" style="display:inline">
                        <div class="row">
                            <div class="col-sm-12">
                                <div style="float:left">
                                    Status
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_OnSelectedIndexChanged">
                                        <asp:ListItem Value="ALL" Text="ALL" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="POSTED" Text="POSTED" ></asp:ListItem>
                                        <asp:ListItem Value="HOLD" Text="HOLD"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div style="float: right;">
                                    <div class="input-group">
                                        <asp:TextBox ID="tbSearch" runat="server" placeholder="Search" 
                                        AutoPostBack="true" CssClass="form-control" value="" Width="200px" OnTextChanged="imgSearch_Click"/>
                                        <div class="input-group-addon input-group-addon1">
                                            <asp:LinkButton ID="imgSearch" runat="server" OnClick="imgSearch_Click"><i class="fa fa-search"></i></asp:LinkButton><br />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="row">
            <div class="col-sm-12">
                <div>
                    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="grid" runat="server" Width="100%" AutoGenerateColumns="False" 
                                EmptyDataText="NO DATA" DataKeyNames="PV_NO" BackColor="White"
                                CssClass="table table-striped table-bordered table-hover"
                                AllowPaging="true" PageSize="10" PagerSettings-Visible="false" >
                                <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" />
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#2A3F54" />
                                <AlternatingRowStyle BackColor="White" />
                                <PagerStyle BorderWidth="1px" BackColor="White" ForeColor="#2A3F54" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="No." HeaderStyle-CssClass="text-center">
                                        <ItemStyle HorizontalAlign="right" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <%# Convert.ToInt32(DataBinder.Eval(Container, "DataItemIndex")) + 1 %>.
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Trans No" HeaderStyle-CssClass="text-center">
                                        <ItemStyle HorizontalAlign="Left" Width="140px" VerticalAlign="Top"/>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransNo" runat="server" Text='<%# Bind("pv_no") %>' /><br />
                                            <asp:Label ID="lblTransDate" runat="server" Text='<%# Bind("pv_date","{0:dd MMM yyyy}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:BoundField DataField="PAYMENT_DATE" HeaderText="Payment Date" HeaderStyle-CssClass="text-center" DataFormatString="{0:dd-MM-yyyy}">
                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VENDOR_NAME" HeaderText="Vendor" HeaderStyle-CssClass="text-center">
                                        <ItemStyle Width="26%" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AMOUNT" HeaderText="PI Amount" DataFormatString="{0:##,#0.#0}" HeaderStyle-CssClass="text-center">
                                        <ItemStyle Width="10%" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="orig_amount" HeaderText="Orig Amount" DataFormatString="{0:##,#0.#0}" HeaderStyle-CssClass="text-center">
                                        <ItemStyle Width="10%" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ADMIN_FEE" HeaderText="Admin Fee" DataFormatString="{0:##,#0.#0}" HeaderStyle-CssClass="text-center">
                                        <ItemStyle Width="10%" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="STATUS" HeaderText="Status" HeaderStyle-CssClass="text-center">
                                        <ItemStyle Width="8%" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="30px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                             <asp:LinkButton ID="lbEdit" runat="server" OnClick="lbEdit_Click"><i class="fa fa-pencil" style="font-size:17px"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="30px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbDelete" runat="server" OnClick="lbDelete_Click"><i class="fa fa-trash" style="font-size:17px"></i></asp:LinkButton>
                                            <asp:Label ID="lblDelete" runat="server" Text="-" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="30px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                             <asp:LinkButton ID="lbPrint" runat="server"><i class="fa fa-print" style="font-size:17px"></i></asp:LinkButton>
                                       </ItemTemplate>
                                    </asp:TemplateField>
                                                                
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="imgSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div style="width: 100%; height: 80px;">
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
    <uc1:wucConfirmBox ID="wucConfirmBox1" runat="server" OnHide="wucConfirmBox_Hide" />
</asp:Content>

