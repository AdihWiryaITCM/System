<%@ Page Title="CMSystem || Customer Receipt" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CustomerReceipt.aspx.cs" Inherits="Transaction_RvHeader" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Wuc/wucSearch.ascx" tagname="wucSearch" tagprefix="uc1" %>
<%@ Register src="~/Wuc/wucConfirmBox.ascx" tagname="wucConfirmBox" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Customer Receipt</h3>
        </div>
    </div>
    <div class="x_panel">
        <asp:Panel ID="Panel2" runat="server" DefaultButton="btnSave">
            <asp:UpdatePanel ID="upHeader" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlHeader" runat="server">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:label runat="server">Trans No.</asp:label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-inline">
                                            <asp:TextBox ID="txtTransNo" runat="server" Width="170px" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            <asp:TextBox ID="txtRVDate" runat="server" CssClass="form-control" Width="110px"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:label runat="server">Receipt Date *</asp:label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-inline">
                                            <asp:TextBox ID="txtReceiptDate" runat="server" Width="110px" CssClass="form-control" ></asp:TextBox>
                                            <cc1:CalendarExtender ID="ceReqDelvDate" runat="server" TargetControlID="txtReceiptDate" Format="dd-MM-yyyy" />
                                            <asp:RequiredFieldValidator ID="rfvReceiptDate" runat="server" ErrorMessage="Required Field!" ControlToValidate="txtReceiptDate" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:label runat="server">Customer No. *</asp:label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-inline">
                                            <asp:TextBox ID="txtCustomerNo" runat="server" Width="120px" CssClass="form-control text-uppercase" Enabled ="false"/>
                                            <asp:LinkButton ID="libCustomerNo" runat="server" Text="" OnClick="libCustomerNo_OnClick" CssClass="alert-link" CausesValidation="false"></asp:LinkButton>
                                            <asp:ImageButton ID="imbCustomerNo" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" CausesValidation="false" OnClick="imbCustomerNo_OnClick"/>
                                            <asp:RequiredFieldValidator ID="rfvCustomerNo" runat="server" ErrorMessage="Required Field!" ControlToValidate="txtCustomerNo" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator> 
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:label runat="server">Bill To *</asp:label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-inline">
                                            <asp:TextBox ID="txtBillTo" runat="server" Width="100px" CssClass="form-control text-uppercase" Enabled="false"/>
                                            <asp:ImageButton ID="imbBillTo" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" CausesValidation="false" OnClick="imbBillTo_OnClick"/>
                                            <asp:RequiredFieldValidator ID="rfvBillTo" runat="server" ErrorMessage="Required Field!" ControlToValidate="txtBillTo" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>   
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
                                            <asp:TextBox ID="txtBillToDesc" runat="server" TextMode="MultiLine" Width="250px" CssClass="form-control text-uppercase" MaxLength="500" Enabled="false"/>   
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:label runat="server">Mutation Amount *</asp:label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-inline">
                                            <asp:TextBox ID="txtTotal" runat="server" Width="125px" CssClass="form-control numbox" Text="0.00" ></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvTotal" runat="server" ErrorMessage="Required Field!" ControlToValidate="txtTotal" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>                
                                            <asp:RegularExpressionValidator id="revTotal" runat="server" ControlToValidate="txtTotal" ValidationExpression="[0-9,]*\.?[0-9]*" Display="Dynamic" ErrorMessage="Only Numbers!" ForeColor="Red" ></asp:RegularExpressionValidator>   
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:label runat="server">SI Amount *</asp:label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-inline">
                                            <asp:TextBox ID="txtAmount" runat="server" Width="135px" CssClass="form-control numbox" Text="0.00" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ErrorMessage="Required Field!" ControlToValidate="txtAmount" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>                
                                            <%--<asp:RegularExpressionValidator id="revAmount" runat="server" ControlToValidate="txtAmount" ValidationExpression="[0-9,]*\.?[0-9]*" Display="Dynamic" ErrorMessage="Only Numbers!" ForeColor="Red" ></asp:RegularExpressionValidator>--%>
                                            <asp:LinkButton runat="server" ID="imbCalculte" OnClick="imbCalculate_OnClick" CausesValidation="false"><i class="fa fa-calculator"></i></asp:LinkButton>                                         </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:label  runat="server">Admin Fee *</asp:label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-inline">
                                            <asp:TextBox ID="txtAdminFee" runat="server" Width="120px" CssClass="form-control numbox" Text="0.00" ></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvAdminFee" runat="server" ErrorMessage="Required Field!" ControlToValidate="txtAdminFee" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>                
                                            <asp:RegularExpressionValidator id="revAdminFee" runat="server" ControlToValidate="txtAdminFee" ValidationExpression="[0-9,]*\.?[0-9]*" Display="Dynamic" ErrorMessage="Only Numbers!" ForeColor="Red" ></asp:RegularExpressionValidator> 
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:label runat="server">Overpayment *</asp:label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-inline">
                                            <asp:TextBox ID="txtOverpayment" runat="server" Width="100px" CssClass="form-control numbox" Text="0.00" ></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvOverpayment" runat="server" ErrorMessage="Required Field!" ControlToValidate="txtOverpayment" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>                
                                            <asp:RegularExpressionValidator id="revOverpayment" runat="server" ControlToValidate="txtOverpayment" ValidationExpression="[0-9,]*\.?[0-9]*" Display="Dynamic" ErrorMessage="Only Numbers!" ForeColor="Red" ></asp:RegularExpressionValidator> 
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:label runat="server">Note *</asp:label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-inline">
                                            <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" Width="250px" CssClass="form-control text-uppercase" MaxLength="500"/>
                                            <asp:RequiredFieldValidator ID="rfvNote" runat="server" ErrorMessage="Required Field!" ControlToValidate="txtNote" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>   
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-2">
                                        <asp:label ID="Label3" runat="server">Status</asp:label>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-inline">
                                            <asp:Label ID="lblStatus" runat="server" CssClass="labelForm"></asp:Label>   
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Panel ID="pnlDetail" runat="server">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="table-responsive">
                            <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="grid" runat="server" Width="100%" AutoGenerateColumns="False"
                                        CssClass="table table-striped table-bordered table-hover table-responsive" 
                                        EmptyDataText="NO DATA" DataKeyNames="ID, IS_PAID, REFF_NO, TAX_FACTURE_NO" BackColor="White" OnRowDataBound="grid_OnRowDataBound"
                                        AllowPaging="false" PageSize="10" OnPageIndexChanging="grid_PageIndexChanging" PagerSettings-Visible="false">
                                        <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#2A3F54" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <PagerStyle BorderWidth="1px" BackColor="White" ForeColor="#2A3F54" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." HeaderStyle-CssClass="text-center">
                                                <ItemStyle HorizontalAlign="right" Width="1%" VerticalAlign="Top" />
                                                <ItemTemplate>
                                                    <%# Convert.ToInt32(DataBinder.Eval(Container, "DataItemIndex")) + 1 %>.
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="text-center">
                                                <ItemStyle HorizontalAlign="Center" Width="3%" VerticalAlign="Top"/>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chbPaid" runat="server" AutoPostBack="true" OnCheckedChanged="chbPaid_OnCheckedChanged"></asp:CheckBox> 
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SI No." HeaderStyle-CssClass="text-center">
                                                <ItemStyle HorizontalAlign="Center" Width="14%" VerticalAlign="Top"/>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="libReffNo" runat="server" CssClass="linktext" Text='<%# Bind("REFF_NO") %>' CausesValidation="false"/>
                                                    <asp:LinkButton ID="libODReff" runat="server" CssClass="linktext" Text='<%# Bind("OD_REFF") %>' Visible="false"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax Facture" HeaderStyle-CssClass="text-center">
                                                <ItemStyle HorizontalAlign="Center" Width="12%" VerticalAlign="Top"/>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="libtax" runat="server" CssClass="linktext" Text='<%# Bind("TAX_FACTURE_NO") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SI Date" HeaderStyle-CssClass="text-center">
                                                <ItemStyle HorizontalAlign="Center" Width="10%" VerticalAlign="Top"/>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="libReffDate" runat="server" CssClass="linktext" Text='<%# Bind("REFF_DATE", "{0:dd MMM yyyy}") %>' CausesValidation="false"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type" HeaderStyle-CssClass="text-center">
                                                <ItemStyle HorizontalAlign="Center" Width="13%" VerticalAlign="Top"/>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="libType" runat="server" CssClass="linktext" Text='<%# Bind("OD_REFF") %>' CausesValidation="false"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="text-center">
                                                <ItemStyle HorizontalAlign="Right" Width="10%" VerticalAlign="Top"/>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="libSIAmount" runat="server" CssClass="linktext" Text='<%# Bind("SI_AMOUNT", "{0:##,#0.#0}") %>' CausesValidation="false"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Outstanding" HeaderStyle-CssClass="text-center">
                                                <ItemStyle HorizontalAlign="Right" Width="10%" VerticalAlign="Top"/>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="libBaseAmount" runat="server" CssClass="linktext" Text='<%# Bind("BASE_AMOUNT", "{0:##,#0.#0}") %>' CausesValidation="false"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Receipt Amount" HeaderStyle-CssClass="text-center">
                                                <ItemStyle HorizontalAlign="Right" Width="15%" VerticalAlign="Top"/>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtReceiptAmount" runat="server" CssClass="form-control numbox" Text='<%# Bind("AMOUNT_PAID", "{0:##,#0.#0}") %>'  />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Discount" HeaderStyle-CssClass="text-center">
                                                <ItemStyle HorizontalAlign="Right" Width="12%" VerticalAlign="Top"/>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="libTotalDiscount" runat="server" CssClass="form-control" Text='<%# Bind("TOTAL_DISCOUNT", "{0:##,#0.#0}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <div class="row">
                        <div class="col-sm-12 buttonSet text-center">
                            <asp:LinkButton ID="btnSave" runat="server" ForeColor="White" CssClass="btn btn-primary btn-sm" OnClick="btnSave_Click" ><i class="fa fa-save"></i>&nbsp;Save</asp:LinkButton>
                            <asp:LinkButton ID="btnPost" runat="server" ForeColor="White" CssClass="btn btn-success btn-sm" OnClick="btnPost_Click"><i class="fa fa-arrow-circle-o-up"></i>&nbsp;Post</asp:LinkButton>
                            <asp:LinkButton ID="btnCancel" runat="server" ForeColor="White" CssClass="btn btn-danger btn-sm" OnClick="btnCancel_Click" CausesValidation="false" ><i class="fa fa-undo"></i>&nbsp;Back</asp:LinkButton>
                        </div>
                    </div>
                </div>        
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <uc1:wucSearch ID="wucSearch" runat="server" OnHide="wucLookup_Hide"/>
    <uc2:wucConfirmBox ID="wucConfirmBox1" runat="server" OnHide="wucConfirmBox_Hide" />

</asp:Content>

