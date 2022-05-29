﻿<%@ Page Title="CMSystem || Inbound Delivery" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="InboundDeliveryProcess.aspx.cs" Inherits="Transaction_InboundDeliveryProcess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Wuc/wucSearch.ascx" tagname="wucSearch" tagprefix="uc1" %>
<%@ Register src="~/Wuc/wucConfirmBox.ascx" tagname="wucConfirmBox" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
                function CheckNumeric() {
                    return event.keyCode >= 48 && event.keyCode <= 57;
                }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <script type="text/javascript" src="../Scripts/js_function.js"></script>
    <div class="page-title">
        <div class="title_left">
            <h3>Inbound Delivery</h3>
        </div>
    </div>
    <div class="x_panel">
        <asp:UpdatePanel ID="upHeader" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Trans No / Date</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbTransNo" CssClass="form-control" runat="server" Width="160px" ReadOnly="true" />
                                    <asp:TextBox ID="tbTransDate" runat="server" CssClass="form-control" Width="110px" ReadOnly="true" />
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
                                <asp:label runat="server">Reff Order No.</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:Panel ID="pSearchReffOrder" runat="server" DefaultButton="bSearchReffOrder">
                                        <asp:TextBox ID="tbReffOrderNo" runat="server" Width="160px" CssClass="form-control text-uppercase"/>
                                        <asp:LinkButton ID="lbReffOrderNo" runat="server" Text="" OnClick="lbReffOrderNo_Click" CssClass="alert-link"/>
                                        <asp:LinkButton ID="lbReff" runat="server" OnClick="lbReffOrderNo_Click" CssClass="alert-link" Visible="false"/>
                                        <asp:ImageButton ID="ibSearchReffOrder" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchReffOrder_Click" />
                                        <asp:Button ID="bSearchReffOrder" runat="server" style="display:none;" OnClick="bSearchReffOrder_Click" />
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
                                <asp:label runat="server">Source</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="bSearchReffOrder">
                                        <asp:TextBox ID="tbFrom" runat="server" Width="90px" CssClass="form-control text-uppercase" ReadOnly="true"/>
                                        <asp:LinkButton ID="lbFrom" runat="server" Text="" CssClass="alert-link"/>
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
                                <asp:label runat="server">Destination</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="tbShipTo" runat="server" Width="105px" CssClass="form-control text-uppercase" ReadOnly="true"/>
                                    <asp:LinkButton ID="lbShipTo" runat="server" Text="" CssClass="alert-link"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Movement Type</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:Panel ID="pSearchMovementType" runat="server" DefaultButton="bSearchMovementType">
                                        <asp:TextBox ID="tbMovementType" runat="server" Width="70px" CssClass="form-control text-uppercase"/>
                                        <asp:LinkButton ID="lbMovementType" runat="server" Text="" OnClick="lbMovementType_Click" CssClass="alert-link"/>
                                        <asp:ImageButton ID="ibSearchMovementType" runat="server" ToolTip="click to search" ImageUrl="~/images/search.png" OnClick="ibSearchMovementType_Click" />
                                        <asp:Button ID="bSearchMovementType" runat="server" style="display:none;" OnClick="bSearchMovementType_Click" />
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
                                <asp:label runat="server">Real Receive Date</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="txtRealReceiveDate" runat="server" Width="110px" CssClass="form-control"/>
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtRealReceiveDate" Format="dd-MM-yyyy" ></cc1:CalendarExtender>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="col-sm-2">
                                <asp:label runat="server">Document Receive Date</asp:label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-inline">
                                    <asp:TextBox ID="txtDocReceivedate" runat="server" Width="110px" CssClass="form-control"/>
                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDocReceivedate" Format="dd-MM-yyyy" ></cc1:CalendarExtender>
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
                                    <asp:TextBox ID="tbNote" runat="server" TextMode="MultiLine" Width="250px" CssClass="form-control text-uppercase"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>          
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="row">
            <div class="col-sm-12">
                <asp:UpdatePanel ID="upDetail" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-sm-12">
                                <div>
                                    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>      
                                            <asp:GridView ID="grid" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%" 
                                            DataKeyNames="articleNo" EmptyDataText="NO DATA" BackColor="White" OnRowDataBound="grid_RowDataBound"
                                            CssClass="table table-striped table-bordered table-hover">
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
                                                            <asp:Label ID="lReffLineNo" runat="server" Text='<%# Bind("reffLineNo") %>' Visible="false" />
                                                            <asp:Label ID="lArticleNo" runat="server" Text='<%# Bind("articleNo") %>'/>
                                                            <asp:Label ID="lArticleType" runat="server" Text='<%# Bind("articleType") %>' Visible="false"/>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Article Desc" HeaderStyle-CssClass="text-center">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lArticleDesc" runat="server" Text='<%# Bind("articleDesc") %>'/>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width=""/>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unit" HeaderStyle-CssClass="text-center">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lUnitDesc" runat="server" Text='<%# Bind("unitID") %>'/>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="120px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qty" HeaderStyle-CssClass="text-center">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lQty" runat="server" Text='<%# Bind("qty", "{0:##,#0.#0}") %>'/>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qty Received" HeaderStyle-CssClass="text-center">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="tbQtyReceived" runat="server" Text='<%# Bind("qty_received", "{0:##,#0.#0}") %>' CssClass="form-control numbox" Width="91%"/>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="60px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Serial No" HeaderStyle-CssClass="text-center">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lManagedItemBy" runat="server" Text='<%# Bind("managed_item_by") %>' Visible="false"/>
                                                            <asp:Textbox ID="tbSerialNo" runat="server" Text='<%# Bind("serial_no") %>' CssClass="form-control" Width="97%"/>
                                                            <asp:ImageButton ID="ibSearchSerialNo" runat="server" ImageUrl="~/Images/search.png" OnClick="ibSearchSerialNo_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="170px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Note" HeaderStyle-CssClass="text-center">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="tbNoteDetail" CssClass="form-control" runat="server" Text='<%# Bind("note") %>' Width="93%"/>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="100px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>   
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                     </ContentTemplate>
                 </asp:UpdatePanel>
             </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                    <div class="row text-center buttonSet">
                        <div class="col-sm-12">
                            <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="btn btn-primary btn-sm" ForeColor="White" ><i class="fa fa-save"></i>&nbsp;Save</asp:LinkButton>
                            <asp:LinkButton ID="btnPosting" runat="server" OnClick="btnPosting_Click" CssClass="btn btn-success btn-sm" ForeColor="White" ><i class="fa fa-arrow-circle-o-up"></i>&nbsp;Post</asp:LinkButton>
                            <asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="btn btn-danger btn-sm" ForeColor="White" CausesValidation="false"><i class="fa fa-undo"></i>&nbsp;Back</asp:LinkButton>
                        </div>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <uc1:wucSearch ID="wucSearch1" runat="server" OnHide="wucSearch_Hide" />
        <uc2:wucConfirmBox ID="wucConfirmBox1" runat="server" OnHide="wucConfirmBox_Hide" />
    </div>
</asp:Content>
