<%@ Control Language="C#" AutoEventWireup="true" CodeFile="wucConfirmBox.ascx.cs" Inherits="wuc_wucConfirmBox" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:UpdatePanel ID="upMpeConfirm" runat="server">
    <ContentTemplate>
        <div class="modal fade" id="mpeConfirm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-sm">
                <div class="modal-content"  style="background-color: #2A3F54;">
                    <div class="modal-header" style="justify-content:center">
                        <h4 class="modal-title" id="myModalLabel">
                            <asp:Label ID="lModalTitle" runat="server" CssClass="fontSmall" ForeColor="White" Font-Bold="true" />
                        </h4>
                    </div>
                    <div class="modal-footer" style="justify-content:center">
                        <asp:UpdatePanel ID="upButton" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="bYes" runat="server" OnClick="btnYes_Click" EnableViewState="false" CssClass="btn btn-success" Width="72px" Height="36px" ForeColor="White" Text="Yes"></asp:Button>
                                <asp:Button ID="bNo" runat="server" data-dismiss="modal" CssClass="btn btn-danger" Width="72px" Height="36px" ForeColor="White" Text="No"></asp:Button>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="bYes" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>