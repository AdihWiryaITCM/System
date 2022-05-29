<%@ Control Language="C#" AutoEventWireup="true" CodeFile="wucSearch.ascx.cs" Inherits="wuc_wucSearch" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:UpdatePanel ID="upMdlSearch" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="modal fade" id="mpeSearch" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog" style="width: 1200px">
                <div class="modal-content">
                    <div class="modal-body">
                        <asp:UpdatePanel ID="upSearchLink" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="search" runat="server" DefaultButton="imgSearch">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" />
                                                <div class="input-group-addon">
                                                    <asp:LinkButton ID="imgSearch" runat="server" OnClick="imgSearch_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div style="height: 300px; overflow: auto">
                                            <asp:GridView ID="grid" runat="server"
                                                class="table-striped table-bordered table-hover" Width="100%" AutoGenerateColumns="False"
                                                EmptyDataText="NO DATA" DataKeyNames="col0" BackColor="White"
                                                EmptyDataRowStyle-ForeColor="White" EmptyDataRowStyle-Font-Bold="true"
                                                EmptyDataRowStyle-BackColor="#2A3F54">
                                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                                <RowStyle BackColor="#E6E6FA" ForeColor="Black" />
                                                <PagerStyle BorderWidth="1px" BackColor="#1E90FF" ForeColor="#284775" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#2A3F54" Font-Bold="True" ForeColor="White" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Trans No" HeaderStyle-CssClass="text-center">
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="link0" runat="server" Text='<%# Bind("col0") %>' OnClick="link_Click" CausesValidation="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Trans Date" HeaderStyle-CssClass="text-center">
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="link1" runat="server" Text='<%# Bind("col1") %>' OnClick="link_Click" CausesValidation="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delv Date" HeaderStyle-CssClass="text-center">
                                                        <ItemStyle HorizontalAlign="Left" Width="30%" VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="link2" runat="server" Text='<%# Bind("col2") %>' OnClick="link_Click" CausesValidation="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Note" HeaderStyle-CssClass="text-center">
                                                        <ItemStyle HorizontalAlign="Left" Width="30%" VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="link3" runat="server" Text='<%# Bind("col3") %>' OnClick="link_Click" CausesValidation="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <div class="row text-right">
                                    <div class="col-sm-12" style="margin-top: 5px">
                                        <asp:Button ID="btnClose" runat="server" CssClass="btn btn-danger btn-sm" Text="Close" data-dismiss="modal" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>