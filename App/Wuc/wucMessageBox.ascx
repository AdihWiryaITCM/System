<%@ Control Language="C#" AutoEventWireup="true" CodeFile="wucMessageBox.ascx.cs" Inherits="wuc_wucMessageBox" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:UpdatePanel ID="upModal" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="myModalMessage" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-body text-center text-uppercase" style="background-color:Black">
                        <asp:Label ID="lModalBody" runat="server" ForeColor="White" Font-Bold="true"/>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

