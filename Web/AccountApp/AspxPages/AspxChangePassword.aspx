<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false" CodeFile="AspxChangePassword.aspx.vb" Inherits="AspxPages_AspxChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form id="frmUsers" runat="server"  clientidmode="Static" role="form">
        <asp:HiddenField ID="HdnCurrentPassword" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="HdnIdentity" runat="server" ClientIDMode="Static" />
        <div class="filter_list_box" id="divchangepassword" runat="server">
            <div class="company_list" style="width: 20%;">
                <label>
                    Current Password</label>
                <input type="password" id="txtOldPassword" runat="server" name="textfield" placeholder="Current Password"
                    clientidmode="Static" tabindex="7" maxlength="255" onkeypress="return IsAddress(event);" />
            </div>
            <div class="company_list" style="width: 20%;">
                <label>
                    New Password</label>
                <input type="password" id="txtPassword" runat="server" name="textfield" placeholder="New Password"
                    clientidmode="Static" tabindex="8" maxlength="100" onkeypress="return IsAddress(event);" />
            </div>
            <div class="company_list" style="width: 20%;">
                <label>
                    Confirm Password</label>
                <input type="password" id="txtConfirmPassword" runat="server" name="textfield" placeholder="Confirm Password"
                    clientidmode="Static" tabindex="9" maxlength="255" onblur="return ValidatePassword()"
                    onkeypress="return IsAddress(event);" />
            </div>
            <div class="company_list" style="width: 20%;">
            <label style="margin: 13px 0 0 0"></label>
             <asp:Button ID="btnChangePassword" runat="server" Text="Submit" CssClass="submit_btn"
                    TabIndex="19" OnClientClick="return ValidatePassword()" />
            </div>
        </div>
    </form>
    <script type="text/javascript">
        function ValidatePassword() {
            var oldpassword = document.getElementById("txtOldPassword").value;
            if (document.getElementById("<%=txtOldPassword.ClientID %>").value.trim() == "") {
                alert("Please Provide Current Password");
                document.getElementById("<%=txtOldPassword.ClientID %>").focus();
                document.getElementById("<%=txtOldPassword.ClientID%>").value = "";
                return false;
            }
            var currentpassword = document.getElementById("HdnCurrentPassword").value;
            if (oldpassword != currentpassword) {
                alert("Provided Current Passwords does not match to database Password");
                document.getElementById("<%=txtOldPassword.ClientID %>").focus();
                document.getElementById("<%=txtOldPassword.ClientID%>").value = "";
                return false;
            }
            var password = document.getElementById("txtPassword").value;
            if (document.getElementById("<%=txtPassword.ClientID %>").value.trim() == "") {
                alert("Please Provide New Password");
                document.getElementById("<%=txtPassword.ClientID %>").focus();
                document.getElementById("<%=txtPassword.ClientID%>").value = "";
                return false;
            }
            if (oldpassword == password) {
                alert("Provided Current Passwords is same to Provided New Password thus no need to change");
                document.getElementById("<%=txtPassword.ClientID %>").focus();
                document.getElementById("<%=txtPassword.ClientID%>").value = "";
                return false;
            }
            if (document.getElementById("<%=txtConfirmPassword.ClientID %>").value.trim() == "") {
                alert("Please Provide Confirm Password");
                document.getElementById("<%=txtConfirmPassword.ClientID %>").focus();
                document.getElementById("<%=txtConfirmPassword.ClientID%>").value = "";
                return false;
            }
            var password = document.getElementById("txtPassword").value;
            var confirmPassword = document.getElementById("txtConfirmPassword").value;
            if (password != confirmPassword) {
                alert("Confirm Passwords do not match.");
                document.getElementById("<%=txtConfirmPassword.ClientID%>").value = "";
                return false;
            }
            return true;
        }
    </script>
</asp:Content>

