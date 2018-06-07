<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AspxActivation.aspx.vb"
    Inherits="AspxPages_AspxActivation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background-color: #00bcd4;
            background-repeat: repeat-x;
            margin-left: 0px;
            margin-top: 0px;
            margin-right: 0px;
            margin-bottom: 0px;
        }
        h1{text-align: center;}
        h2{text-align: center;}
    </style>
    <link href="Resource/CSS/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrapper">
        <div class="login_box">
            <div class="login_white_box">
                <div id="Activation" runat="server">
                    <h1>
                        Thank you for the Activation</h1>
                </div>
                <div id="Deactivation" runat="server">
                    <h2>
                        You Account is Already Activated</h2>
                         <center><a href="http://103.233.76.155/accountsapp/">Click Here to Start the Portal</a></center>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
