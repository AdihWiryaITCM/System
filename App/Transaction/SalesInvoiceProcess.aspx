<%@ Page Title="CMSystem || Sales Invoice" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SalesInvoiceProcess.aspx.cs" Inherits="Transaction_SalesInvoiceDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Wuc/wucSearch.ascx" tagname="wucSearch" tagprefix="uc1" %>
<%@ Register src="~/Wuc/wucConfirmBox.ascx" tagname="wucConfirmBox" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Sales Invoice</h3>
        </div>
    </div>
    <div class="x_panel">
        <asp:UpdatePanel ID="upHeader" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlHeader" runat="server">
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Invoice No / Date</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="txtTransNo" runat="server" Width="220px" CssClass="form-control" ReadOnly="true" />
                                    <asp:TextBox ID="txtTransDate" runat="server" Width="110px" ReadOnly="true" CssClass="form-control" />
                                    <asp:Label ID="lblStatus" runat="server" Width="70px" Visible="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Sales No.</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="txtPONo" runat="server" Width="170px" CssClass="form-control text-uppercase"/>
                                    <asp:LinkButton ID="libPONo" runat="server" Text="" OnClick="libPONo_Click" CssClass="alert-link" CausesValidation="false"/>
                                    <asp:ImageButton ID="imbPONo" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="imbPONo_Click" CausesValidation="false"/>
                                    <asp:RequiredFieldValidator ID="rfvPONo" runat="server" ErrorMessage="Required Field!" ControlToValidate="txtPONo" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                  </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Reff Order No.</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbReffOrderNo" runat="server" Width="180px" CssClass="form-control text-uppercase"/>
                                    <asp:LinkButton ID="lbReffOrderNo" runat="server" Text="" OnClick="lbReffOrderNo_Click" CssClass="alert-link" CausesValidation="false"/>
                                    <asp:ImageButton ID="ibSearchReffOrder" runat="server" 
                                        ToolTip="click to search" ImageUrl="~/images/search.png" 
                                        OnClick="ibSearchReffOrder_Click" CausesValidation="false" Width="16px"/>
                                    <asp:RequiredFieldValidator ID="rfvReffOrderNo" runat="server" ErrorMessage="Required Field!" ControlToValidate="tbReffOrderNo" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:Label ID="lSalesType" runat="server" Visible="false" />
                                    <asp:Label ID="lSalesOrderNo" runat="server" Visible="false" />
                                    <asp:Label ID="lCustPONo" runat="server" Visible="false" />
                                    <asp:Label ID="lDeliveryNo" runat="server" Visible="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Customer</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="txtCustomerNo" runat="server" Width="120px" CssClass="form-control text-uppercase" ReadOnly="true" />
                                    <asp:LinkButton ID="libCustomerName" runat="server" CssClass="alert-link" CausesValidation="false"/>
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
                                <asp:label runat="server"></asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" 
                                        Width="250px" MaxLength="500" ReadOnly = "True" CssClass="form-control"/>
                                    <asp:LinkButton ID="lbVatReg" runat="server" CssClass="alert-link" CausesValidation="false"/>
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
                                    <asp:TextBox ID="txtCustomerBillTo" runat="server" Width="60px" CssClass="form-control text-uppercase" ReadOnly="true" />
                                    <asp:LinkButton ID="libCustomerBillToName" runat="server" CssClass="alert-link" CausesValidation="false"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label ID="Label1" runat="server">Ship To</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="txtCustomerShipTo" runat="server" Width="60px" CssClass="form-control text-uppercase" ReadOnly="true" />
                                    <asp:LinkButton ID="libCustomerShipToName" runat="server" CssClass="alert-link" CausesValidation="false"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Amount</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="txtTotalInvoice" runat="server" Width="110px" Text="0" CssClass="form-control numbox" ReadOnly="true"/>
                                    <asp:RequiredFieldValidator ID="rfvTotalInvoice" runat="server" ErrorMessage="Required Field!" ControlToValidate="txtTotalInvoice" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">VAT</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="txtPPN" runat="server" Width="110px" Text="0" CssClass="form-control numbox" ReadOnly="true"/>
                                    <asp:RequiredFieldValidator ID="rfvPPN" runat="server" ErrorMessage="Required Field!" ControlToValidate="txtPPN" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div> 
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Total Amount</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="txtCurrencyID" runat="server" Width="80px" CssClass="form-control"/>
                                    <asp:TextBox ID="txtTotalBill" runat="server" Width="110px" Text="0" CssClass="form-control" ReadOnly="true"/>
                                    <asp:RequiredFieldValidator ID="rfvTotalBill" runat="server" ErrorMessage="Required Field!" ControlToValidate="txtTotalBill" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:Panel ID="pnlPaymentTerm" runat="server">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="col-sm-2">
                                    <asp:label runat="server">Payment Term</asp:label>
                                </div>
                                <div class="col-sm-10">
                                    <div class="form-inline">
                                        <asp:TextBox ID="txtPaymentTerm" runat="server" Width="80px" CssClass="form-control" ReadOnly="true"/>
                                        <asp:LinkButton ID="libPaymentTerm" runat="server" CssClass="alert-link" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>        
                <asp:Panel ID="pnlDueDate" runat="server">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="col-sm-2">
                                    <asp:label runat="server">Due Date</asp:label>
                                </div>
                                <div class="col-sm-10">
                                    <div class="form-inline">
                                        <asp:TextBox ID="txtDueDate" runat="server" Width="110px" CssClass="form-control" ReadOnly="true"/>
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
                                <asp:label runat="server">Note</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" Width="250px" CssClass="form-control text-uppercase" MaxLength="1000"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Tax Facture No.</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="txtTaxFactureNo" runat="server" Width="180px" CssClass="form-control text-uppercase" MaxLength="19"/>
                                    <asp:Label runat="server" Font-Bold="true"> (exp : 010.001-18.12345678)</asp:Label>
                                    <asp:RequiredFieldValidator ID="rfvTaxFactureNo" runat="server" ErrorMessage="Required Field!" ControlToValidate="txtTaxFactureNo" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
        <asp:Panel ID="pnlDetail" runat="server">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" Visible="false">
            <ContentTemplate>
                <div class="row">
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="imgSearch">
                        <div style="float: right;">
                            <div class="input-group">
                                <asp:TextBox ID="tbSearch" runat="server" placeholder="Search" 
                                AutoPostBack="true" CssClass="form-control" value="" Width="200px" OnTextChanged="imgSearch_Click"/>
                                <div class="input-group-addon input-group-addon1">
                                    <asp:LinkButton ID="imgSearch" runat="server" OnClick="imgSearch_Click"><i class="fa fa-search"></i></asp:LinkButton><br />
                                </div>
                            </div>
                        </div>
                    </asp:Panel> 
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="row">
            <div class="col-sm-12">
                <div>
                    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="grid" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="table table-striped table-bordered table-hover" 
                                EmptyDataText="NO DATA" DataKeyNames="article_No" BackColor="White" OnRowDataBound="grid_OnRowDataBound"
                                AllowPaging="false" PageSize="10" OnPageIndexChanging="grid_PageIndexChanging" PagerSettings-Visible="false">
                                <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White"  />
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#2A3F54" />
                                <AlternatingRowStyle BackColor="White" />
                                <PagerStyle BorderWidth="1px" BackColor="White" ForeColor="#2A3F54" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="No." HeaderStyle-CssClass="text-center">
                                        <ItemStyle HorizontalAlign="right" Width="2px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <%# Convert.ToInt32(DataBinder.Eval(Container, "DataItemIndex")) + 1 %>.
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="POD No." HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lReffNo" runat="server" Text='<%# Bind("REFF_NO") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Source" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lSource" runat="server" Text='<%# Bind("SOURCE") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Article No" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lArticleNo" runat="server" Text='<%# Bind("article_No") %>'/><br />
                                            <asp:Label ID="lArticleDesc" runat="server" Text='<%# Bind("article_description") %>'/>
                                            <asp:Label ID="lArticleType" runat="server" Text='<%# Bind("article_Type") %>' Visible="false"/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="220px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty Order" Visible="false" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lQtyOrder" runat="server" Text='<%# Bind("qty_order", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="0px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit Price" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lUnitPrice" runat="server" Text='<%# Bind("unit_price", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="VAT" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnitTax" runat="server" Text='<%# Bind("unit_tax", "{0:##,#0.#0}") %>'  />
                                            <asp:Label ID="lblTaxAmount" runat="server" Text='<%# Bind("tax_amount", "{0:##,#0.#0}")  %>' Visible="false" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty Delivered" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />    
                                        <ItemTemplate>
                                            <asp:Label ID="lQtyDelivered" runat="server" Text='<%# Bind("qty_delivered", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Period" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lPeriod" runat="server" Text='<%# Bind("period") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" Visible="false" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="txtOutstanding" runat="server" Text='<%# Bind("outstanding_duration", "") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="0px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Time Duration" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBillDuration" ReadOnly="true" runat="server" Width="35px" style="text-align:center" MaxLength="2" Text='<%# Bind("bill_duration", "") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Price" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="tbPrice" runat="server" Text='<%# Bind("price", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discount" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="tbDiscount" runat="server" Text='<%# Bind("discount", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="tbNetAmount" runat="server" Text='<%# Bind("net_amount", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gross Amount" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="tbTotalAmount" runat="server" Text='<%# Bind("total_amount", "{0:##,#0.#0}") %>'/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Note" HeaderStyle-CssClass="text-center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbNoteDetail" runat="server" Text='<%# Bind("note") %>' CssClass="toUpper"/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="20px" />
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
    </asp:Panel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <div class="row text-center">
                    <div class="col-sm-12 buttonSet">
                        <asp:LinkButton ID="btnSave" runat="server" ForeColor="White" CssClass="btn btn-primary btn-sm" OnClick="btnSave_Click" ><i class="fa fa-save"></i>&nbsp;Save</asp:LinkButton>
                        <asp:LinkButton ID="btnPost" runat="server" ForeColor="White" CssClass="btn btn-success btn-sm" OnClick="btnPost_Click" Visible="false"><i class="fa fa-arrow-circle-o-up"></i>&nbsp;Post</asp:LinkButton>
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

