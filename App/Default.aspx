<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>


<%@ Register src="~/Wuc/wucConfirmBox.ascx" tagname="wucConfirmBox" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="Server">
    <div class="page-title">
        <div class="title_left">
            <h3>Change Password</h3>
        </div>
    </div>
    <div class="x_panel">
        <div class="form-group row">
            <div class="col-sm-12">
                <div class="col-sm-2">
                    <asp:Label runat="server">Name</asp:Label>
                </div>
                <div class="col-sm-10">
                    <div class="form-inline">
                        <asp:Label ID="lbluserName" runat="server" Text="" />
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-12">
                <div class="col-sm-2">
                    <asp:Label runat="server">Current Password</asp:Label>
                </div>
                <div class="col-sm-10">
                    <div class="form-inline">
                        <asp:TextBox ID="txtCurrPassword" runat="server" TextMode="Password" CssClass="form-control"/>  
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-12">
                <div class="col-sm-2">
                    <asp:Label runat="server">New Password</asp:Label>
                </div>
                <div class="col-sm-10">
                    <div class="form-inline">
                        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="form-control"/>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-12">
                <div class="col-sm-2">
                    <asp:Label runat="server">Confirmation New Password</asp:Label>
                </div>
                <div class="col-sm-10">
                    <div class="form-inline">
                        <asp:TextBox ID="txtNewPassword2" runat="server" TextMode="Password" CssClass="form-control"/>    
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-12">
                <div class="col-sm-2">
                </div>
                <div class="col-sm-10">
                    <div class="form-inline">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnSave" runat="server" Text="Simpan" OnClick="btnSave_Click" CssClass="btn btn-primary btn-block btn-sm"/>
                            </ContentTemplate>
                        </asp:UpdatePanel> 
                    </div>
                </div>
            </div>
        </div>
    </div>
    <uc2:wucConfirmBox ID="wucConfirmBox1" runat="server" OnHide="wucConfirmBox_Hide" />
</asp:Content>