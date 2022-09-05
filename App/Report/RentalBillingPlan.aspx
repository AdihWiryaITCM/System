<%@ Page Title="CMSystem || Rental Billing Plan" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RentalBillingPlan.aspx.cs" Inherits="Report_RentalBillingPlan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Wuc/wucSearch.ascx" tagname="wucSearch" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Rental Order Billing Plan</h3>
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
                                    <asp:label runat="server">Rent To</asp:label>
                                </div>
                                <div class="col-sm-10">
                                    <div class="form-inline">
                                        <asp:TextBox ID="tbSoldTo" runat="server" Width="100px" CssClass="form-control text-uppercase"/>
                                        <asp:ImageButton ID="ibSearchSoldTo" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchSoldTo_Click" CausesValidation="false"/>
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
                                        <asp:ImageButton ID="ibSearchBillTo" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchBillTo_Click" CausesValidation="false"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="col-sm-2">
                                    <asp:label runat="server">Trans Date *</asp:label>
                                </div>
                                <div class="col-sm-10">
                                    <div class="form-inline">
                                        <asp:TextBox ID="txtStartDate" runat="server" Width="110px" CssClass="form-control"/>
                                        <%--<asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Required Field!" ControlToValiddate="txtStartDate" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>--%>                
                                        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtStartDate" Format="dd-MM-yyyy"></cc1:CalendarExtender> -
                                        <asp:TextBox ID="txtEndDate" runat="server" Width="110px" CssClass="form-control"/>
                                        <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtEndDate" Format="dd-MM-yyyy"></cc1:CalendarExtender>
                                        <%--<asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="Required Field!" ControlToValidate="txtEndDate" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator> --%>      
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
                                            <asp:ListItem Value="All" Text="All" Selected="True" ></asp:ListItem>
                                            <asp:ListItem Value="Completed" Text="Completed" ></asp:ListItem>
                                            <asp:ListItem Value="Not Complete" Text="Not Complete" ></asp:ListItem>
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
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row">
                    <div  class="col-sm-12">
                        <div class="form-inline" style="display:inline">
                            <asp:Panel ID="Panel1" runat="server" style="float:right" DefaultButton="imgSearch">
                                <div class="input-group">
                                    <asp:TextBox ID="tbSearch" runat="server" placeholder="Search" 
                                    AutoPostBack="true" CssClass="form-control" value="" Width="200px" OnTextChanged="imgSearch_Click"/>
                                    <div class="input-group-addon">
                                        <asp:LinkButton ID="imgSearch" runat="server" OnClick="imgSearch_Click"><i class="fa fa-search"></i></asp:LinkButton><br />
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
                <div class="table-responsive">
                    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="grid" runat="server" Width="100%" AutoGenerateColumns="False" 
                                CssClass="table table-striped table-bordered table-hover" 
                                EmptyDataText="NO DATA" DataKeyNames="TRANS_NO" BackColor="White"
                                AllowPaging="false" PageSize="10" PagerSettings-Visible="false" >
                                <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" />
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#2A3F54" />
                                <AlternatingRowStyle BackColor="White" />
                                <PagerStyle BorderWidth="1px" BackColor="White" ForeColor="#2A3F54" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="No." HeaderStyle-CssClass="text-center">
                                        <ItemStyle HorizontalAlign="right" Width="1px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <%# Convert.ToInt32(DataBinder.Eval(Container, "DataItemIndex")) + 1 %>.
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Code">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="30%" />
                                        <ItemTemplate>
                                            <asp:Label id="txtCoaNo" runat="server" Text='<%# Bind("COA_NO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:BoundField DataField="TRANS_NO" HeaderText="Trans No." HeaderStyle-CssClass="text-center">
                                        <ItemStyle Width="20px" HorizontalAlign="Center" VerticalAlign="Top" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SHIP_TO" HeaderText="Ship To"  HeaderStyle-CssClass="text-center">
                                        <ItemStyle Width="20px" HorizontalAlign="Left" VerticalAlign="Top" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUST_PO_NO" HeaderText="Cust PO No."  HeaderStyle-CssClass="text-center" >
                                        <ItemStyle Width="20px" HorizontalAlign="Left" VerticalAlign="Top" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BILLING_DATE" HeaderText="Billing Date" DataFormatString="{0:dd-MM-yyyy}"  HeaderStyle-CssClass="text-center">
                                        <ItemStyle Width="15px" HorizontalAlign="Center" VerticalAlign="Top"  />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ARTICLE" HeaderText="Article"  HeaderStyle-CssClass="text-center">
                                        <ItemStyle Width="15px" HorizontalAlign="Center" VerticalAlign="Top"  />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SERIAL_NO" HeaderText="Serial No."  HeaderStyle-CssClass="text-center">
                                        <ItemStyle Width="30px" HorizontalAlign="Left" VerticalAlign="Top"  />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QTY" HeaderText="Qty"  HeaderStyle-CssClass="text-center" >
                                        <ItemStyle Width="10px" HorizontalAlign="Center" VerticalAlign="Top"  />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PERIOD" HeaderText="Period"  HeaderStyle-CssClass="text-center" >
                                        <ItemStyle Width="10px" HorizontalAlign="Center" VerticalAlign="Top"  />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RENT_TIME_DURATION" HeaderText="Rent Time"  HeaderStyle-CssClass="text-center" >
                                        <ItemStyle Width="25px" HorizontalAlign="Center" VerticalAlign="Top"  />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RENT_RATE" HeaderText="Rate" DataFormatString="{0:##,#0.#0}"  HeaderStyle-CssClass="text-center">
                                        <ItemStyle Width="25px" HorizontalAlign="Right" VerticalAlign="Top"  />
                                    </asp:BoundField>         
                                    <asp:BoundField DataField="OUTSTANDING_DURATION" HeaderText="Outstanding Time"  HeaderStyle-CssClass="text-center">
                                        <ItemStyle Width="25px" HorizontalAlign="Center" VerticalAlign="Top"  />
                                    </asp:BoundField>    
                                    <asp:BoundField DataField="NOTE" HeaderText="Note"  HeaderStyle-CssClass="text-center" >
                                        <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Top"  />
                                    </asp:BoundField>       
                                    <asp:BoundField DataField="TOTAL" HeaderText="Total" DataFormatString="{0:N2}"  HeaderStyle-CssClass="text-center">
                                        <ItemStyle Width="80px" HorizontalAlign="Right" VerticalAlign="Top"  />
                                    </asp:BoundField>       
                                    <asp:BoundField DataField="VOUCHER_NO" HeaderText="SI No."  HeaderStyle-CssClass="text-center">
                                        <ItemStyle Width="25px" HorizontalAlign="Center" VerticalAlign="Top"  />
                                    </asp:BoundField>
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

