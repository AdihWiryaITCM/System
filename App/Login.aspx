<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>
<html lang="en">

<head>

    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">

    <title>CMSystem</title>

    <link href="AssetLogin/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <link href="AssetLogin/vendor/metisMenu/metisMenu.min.css" rel="stylesheet">
    <link href="AssetLogin/dist/css/sb-admin-2.css" rel="stylesheet">
    <link href="AssetLogin/vendor/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css">

</head>
    <body style="background-image:url(Images/banner.jpg);background:fixed">
        <form runat="server">
        <asp:ScriptManager ID="scriptManager" runat="server" />
            <div class="container">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="login-panel panel panel-default">
                            <div class="panel-heading text-center">
                                <h3 class="panel-title">Silahkan Login</h3>
                            </div>
                            <div class="panel-body">
                                <div class="col-sm-12">
                                    <div class="text-center">
                                        <img src="../AssetLogin/Images/login.jpg" style="width:100%;max-width:600px"/>
                                    </div>
                                </div>
                                <div class="col-sm-12">
                                    <asp:UpdatePanel runat="server" ID="pLogin">
                                        <ContentTemplate>
                                            <asp:Panel ID="pError" runat="server" Visible="false">
                                                <div class="alert alert-danger">
                                                    <asp:Label ID="lError" runat="server" Text="" />
                                                </div>    
                                            </asp:Panel>
                                            <p style="font-weight:bold;font-size:20px">CMSystem</p>
                                            <div class="form-group">
                                                <asp:TextBox runat="server" ID="tbUser" CssClass="form-control" PlaceHolder="Masukan Username"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <asp:TextBox runat="server" ID="tbPassword" CssClass="form-control" TextMode="Password" PlaceHolder="Masukan Password"></asp:TextBox>
                                            </div>
                                            <asp:Button ID="bLogin" runat="server" CssClass="btn btn-success btn-block" Text="Login" OnClick="bLogin_click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <script src="vendor/jquery/jquery.min.js"></script>
            <script src="vendor/bootstrap/js/bootstrap.min.js"></script>
            <script src="vendor/metisMenu/metisMenu.min.js"></script>
            <script src="dist/js/sb-admin-2.js"></script>
        </form>
    </body>

</html>
