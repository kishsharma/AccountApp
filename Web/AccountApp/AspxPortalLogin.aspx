<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AspxPortalLogin.aspx.vb"
    Inherits="AspxPortalLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Accounting Solutions</title>
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
    </style>
    <link href="Resource/CSS/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function preventBack() {
            window.history.forward();
        }
        window.onunload = function () {
            null;
        };
        setTimeout("preventBack()", 0);
    </script>
    <script type="text/javascript">
//        document.addEventListener("contextmenu", function (e) {
//            e.preventDefault();
//            //e.stopPropagation();
//            //alert('Right Click Not Allowed');
//        }, false);
    </script>
    <link rel="icon" href="Resource/images/appico/favicon-16x16.png" type="image/gif" sizes="16x16">
</head>
<body>
    <!--Check Box CSS Starts Here-->
    <style type="text/css">
        input[type="checkbox"]
        {
            display: none;
            border: none !important;
            box-shadow: none !important;
        }
        
        input[type="checkbox"] + label span
        {
            display: inline-block;
            vertical-align: middle;
            width: 25px;
            height: 25px;
            background: url(Resource/Images/uncheck.png);
        }
        
        input[type="checkbox"]:checked + label span
        {
            background: url(Resource/Images/check_2.png);
            content: '\f14a';
            color: #fff;
            vertical-align: middle;
            width: 25px;
            height: 25px;
        }
    </style>
    <!--Check Box CSS Ends Here-->
    <form id="login" class="well" runat="server">
    <div class="wrapper">
        <div class="login_box">
            <div class="login_logo">
                <img src="Resource/images/login_logo.jpg" /></div>
            <div class="login_white_box">
                <div id="forgotpass2" class="forgotpass_open">
                    <a href="javascript:void(0)" onclick="document.getElementById('forgotpass2').className ='forgotpass2';document.getElementById('').style.display='none'"
                        class="close_btn">X</a>
                    <div class="forgot_pass_heading">
                        Enter your Login-Id</div>
                    <asp:TextBox ID="txtFgpdEmail" runat="server" onkeypress="return IsAlphaNumericForEmail(event);"
                        placeholder="type here..." ondrop="return false;" onpaste="return true;" />
                    <asp:Button ID="BtnSendPassword" runat="server" OnClientClick="return validateForgotPwd();"
                        TabIndex="5" Text="Send" />
                </div>
                <div class="login_inner">
                    <div class="login_img">
                        <img src="Resource/images/login_img.jpg"/></div>
                    <div class="login_label">
                        Username</div> <!--onkeypress="return IsAlphaNumericForEmail(event);"-->
                    <input type="text" name="textfield" id="txtUserName" value="" placeholder="User Name"
                        runat="server" tabindex="1" clientidmode="Static" 
                        ondrop="return false;" onpaste="return true;" />
                    <div class="login_label">
                        Password</div>
                    <input type="password" id="txtPassword" value="" placeholder="******" runat="server"
                        name="password" tabindex="2" />
                    <input type="checkbox" id="chkLogn" runat="server" />
                    <label class="panel_list_text" for="chkLogn" style="display:none;" TabIndex="5">
                        <span></span>&nbsp;&nbsp;I am HUMAN.
                    </label>
                    <br />
                    <div class="captcha_box" style="display:none;">
                        <asp:TextBox ID="captcha" runat="server" Width="90px" Height="25px" Style="background-image: url('Resource/Images/imgCaptch.jpg');"
                            ForeColor="#FF7F00" Font-Bold="true"></asp:TextBox>
                        &nbsp; &nbsp; &nbsp; &nbsp; <a href="#" id="imgGenerateSrv" runat="server" onclick="return resetCaptcha();">
                            <img id="imgGenerate" runat="server" src="Resource/Images/Refresh.png" alt="" /></a>
                        <asp:TextBox ID="txtCaptcha" runat="server" Width="90px" Height="25px" Font-Bold="true" TabIndex="4" ondrop="return false;" onpaste="return false;"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="login_btn">
                <asp:Button name="button" ID="btnLogin" OnClientClick="return validate();" TabIndex="3"
                    Text="Login" runat="server" />
                <asp:Button name="button" ID="BtnClr" TabIndex="4" OnClientClick="return resetFields()" Text="Clear" Style="background-color: #515151; color: #fff;" runat="server" />
                <a class="forgot_pass_link" href="javascript:void(0)" onclick="document.getElementById('forgotpass2').className ='forgotpass';document.getElementById('').style.display='block'">
                    Forgot Password?</a>
            </div>
        </div>
    </div>
    </form>
</body>

<script type="text/javascript" language="javascript">
    function validateForgotPwd() {
        if (document.getElementById("<%=txtFgpdEmail.ClientID %>").value == 'super' || document.getElementById("<%=txtFgpdEmail.ClientID %>").value == 'admin') {
            // return true
        } else {
            var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
            var inputText;
            inputText = document.getElementById("<%=txtFgpdEmail.ClientID %>");
            if (inputText.value.match(mailformat)) {
                //alert('valid email');
            }
            else {
                var ElementCssClass = document.getElementById("forgotpass2").className;
                //alert(ElementCssClass);
                if (ElementCssClass == 'forgotpass' && document.getElementById("<%=txtFgpdEmail.ClientID %>").value != '') {
                    alert("You have entered an invalid email address! 123");
                    document.getElementById("<%=txtFgpdEmail.ClientID %>").value = "";
                    document.getElementById("<%=txtFgpdEmail.ClientID %>").focus();
                    return false;
                }
            }
        }

        return true;
    }
</script>
<script type="text/javascript" language="javascript">
    function resetFields() {
        document.getElementById("<%=txtUserName.ClientID%>").value = "";
        document.getElementById("<%=txtPassword.ClientID%>").value = "";
        document.getElementById("<%=txtCaptcha.ClientID%>").value = "";
        document.getElementById("<%=chkLogn.ClientID%>").checked = false;

        return true;
    }
    </script>
<script type="text/javascript" language="javascript">
    function validate() {
        //alert(document.getElementById("<%=txtUserName.ClientID %>").value);
        if (document.getElementById("<%=txtUserName.ClientID %>").value.trim() == "") {
            alert("Please enter username");
            document.getElementById("<%=txtUserName.ClientID %>").focus();
            return false;
        }

        if (document.getElementById("<%=txtUserName.ClientID %>").value == 'super' || document.getElementById("<%=txtUserName.ClientID %>").value == 'admin') {
            // return true
        } else {
            var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
            var inputText;
            inputText = document.getElementById("<%=txtUserName.ClientID %>");
            if (inputText.value.match(mailformat)) {
                //alert('valid email');
            }
            else {
                if (document.getElementById("<%=txtUserName.ClientID %>").value != 'super' || document.getElementById("<%=txtUserName.ClientID %>").value != 'admin') {
                    //alert("You have entered an invalid email address!");
                    //document.getElementById("<%=txtUserName.ClientID %>").value = "";
                    //document.form1.text1.focus();
                    //return false;
                    return true;
                }
            }
        }

        if (document.getElementById("<%=txtPassword.ClientID %>").value.trim() == "") {
            alert("Please enter password");
            document.getElementById("<%=txtPassword.ClientID %>").focus();
            return false;
        }
        if (document.getElementById("<%=txtCaptcha.ClientID %>").value.trim() != "" && document.getElementById("<%=chkLogn.ClientID %>").checked == true) {
            //alert('A');
            alert("Please check either I am a human or enter the captcha text.");
            document.getElementById("<%=txtCaptcha.ClientID %>").focus();
            return false;
        }
        if (document.getElementById("<%=chkLogn.ClientID %>").checked == false && document.getElementById("<%=txtCaptcha.ClientID %>").value.trim() == "") {
            //alert('D');
            alert("Please check I am a human or enter the captcha text.");
            document.getElementById("<%=txtCaptcha.ClientID %>").focus();
            return false;
        }
//        if (document.getElementById("<%=chkLogn.ClientID %>").checked == true && document.getElementById("<%=txtCaptcha.ClientID %>").value.trim() == "") {
//            //alert('B');
//            return true;
//        }
//        if (document.getElementById("<%=chkLogn.ClientID %>").checked == false && document.getElementById("<%=txtCaptcha.ClientID %>").value.trim() != "") {
//            //alert('C');
//            return true;
//        }
//        if (document.getElementById("<%=txtCaptcha.ClientID %>").value.trim() == "") {
//            alert("Please enter the captcha text.");
//            document.getElementById("<%=txtCaptcha.ClientID %>").focus();
//            return false;
//        }
        if (document.getElementById("<%=captcha.ClientID %>").value.trim() != "" && document.getElementById("<%=txtCaptcha.ClientID %>").value.trim() != "") {
            if (document.getElementById("<%=captcha.ClientID %>").value.trim() != document.getElementById("<%=txtCaptcha.ClientID %>").value.trim()) {
                alert("captcha text does not match.");
                document.getElementById("<%=txtCaptcha.ClientID %>").focus();
                return false;
            }
        }
        return true;
    }
</script>
</html>
