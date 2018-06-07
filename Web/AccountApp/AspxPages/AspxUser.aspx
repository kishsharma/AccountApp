<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false"
    CodeFile="AspxUser.aspx.vb" Inherits="AspxPages_AspxUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<style type="text/css">
        .company_list input[type="checkbox"]
        {
            display: none;
            border: none !important;
            box-shadow: none !important;
        }
        .company_list input[type="checkbox"]
        {
            display: inline-block;
            vertical-align: middle;
            width: 25px;
            height: 25px;
            background: url(../Resource/Images/uncheck.png);
        }
        
        .company_list input[type="checkbox"]:checked 
        {
            background: url(../Resource/Images/check_2.png);
            content: '\f14a';
            color: #fff;
            vertical-align: middle;
            width: 25px;
            height: 25px;
        }
    </style>
    
    <script type="text/javascript">
        function Showalert() {
           
        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Resource/js/Chosen/chosen.css" rel="stylesheet" type="text/css" />
    <form id="frmUsers" runat="server"  clientidmode="Static" role="form">
    <asp:HiddenField ID="HdnIdentity" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HdnIdSelected" runat="server" />
    <asp:HiddenField ID="HdnSelected" runat="server" />
    <div class="filter_list_box" id="divMain" runat="server" style="margin: 40px 0 0 0">
        <div class="company_list">
            <label id="Label10" runat="server">
                Role *</label><br />
            <asp:DropDownList ID="DDLIDRole" class="chosen-select" runat="server" AutoPostBack="true"
                TabIndex="7" />
        </div>
        <div class="company_list" id="Companylist" runat="server">
            <label>
                Company</label>
            <asp:DropDownList ID="DDLCompany" class="chosen-select" runat="server" TabIndex="2" />
        </div>
        <div class="company_list">
            <label>
                User Name</label>
            <input type="text" id="TxtUserName" runat="server" name="textfield" placeholder="User Name"
                clientidmode="Static" tabindex="3" maxlength="100" onkeypress="return IsOnlyAlphabets(event);" />
        </div>
        <div class="company_list">
            <label>
                User Full-Name</label>
            <input type="text" id="TxtUserFullName" runat="server" name="textfield" placeholder="User Full-Name"
                clientidmode="Static" tabindex="3" maxlength="100" onkeypress="return IsNameWithApstrophe(event);" />
        </div>
        <div class="company_list">
            <label>
                User Address</label>
            <input type="text" id="TxtUserAddress" runat="server" name="textfield" placeholder="User Address"
                clientidmode="Static" tabindex="4" maxlength="255" onkeypress="return IsAddress(event);" />
        </div>
        <div class="company_list">
            <label>
                EmailId</label>
            <input type="text" id="TxtEmailId" runat="server" name="textfield" placeholder="EmailId"
                clientidmode="Static" tabindex="5" maxlength="100" onkeypress="return IsAlphaNumericForEmail(event);" />
        </div>
        <div class="company_list">
            <label>
                Mobile No</label>
            <input type="text" id="TxtPhoneNo" runat="server" name="textfield" placeholder="Mobile No"
                clientidmode="Static" tabindex="6" maxlength="255" onkeypress="return IsNumeric(event);" />
        </div>
    </div>
    <div class="filter_list_box" id="divchangepassword" runat="server" visible="false">
        <div class="company_list" style="width: 20%;">
            <label>
                Old Password</label>
            <input type="password" id="txtOldPassword" runat="server" name="textfield" placeholder="Old Password"
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
        <div class="company_list" style="width: 10%;">
         <label style="margin: 13px 0 0 0">Send Notification to User</label>
         <input type="checkbox" id="chkPassword" runat="server" />
        </div>
        <div class="company_list" style="width: 20%;">
        <label style="margin: 13px 0 0 0"></label>
         
         <asp:Button ID="btnChangePassword" runat="server" Text="Submit" CssClass="submit_btn"
                TabIndex="19" OnClientClick="return ValidatePassword()" />
        </div>
    </div>
    <div id="divUserPay" runat="server" visible="false">
        <div class="filter_list_box" style="margin: 40px 0">
            <div class="company_list">
                <label>
                    Payment in Rs.</label>
                <input type="text" id="TxtPament" runat="server" name="textfield" placeholder="Payment in Rs."
                    clientidmode="Static" tabindex="10" maxlength="5" onkeypress="return IsNumeric(event);" />
            </div>
            <div class="company_list">
                <label>
                    Incentive in %</label>
                <input type="text" id="TxtIncetivePercent" runat="server" name="textfield" placeholder="Incentive in %"
                    clientidmode="Static" tabindex="11" maxlength="2" onkeypress="return IsNumeric(event);" />
            </div>
            <div class="company_list">
                <label>
                    Penalty in %</label>
                <input type="text" id="TxtPenaltyPercent" runat="server" name="textfield" placeholder="Penalty in %"
                    clientidmode="Static" tabindex="12" maxlength="2" onkeypress="return IsNumeric(event);" />
            </div>
            <div class="company_list">
                <label>Enter OTP No</label>
            <input type="text" id="txtOTPNO" runat="server" name="textfield" placeholder="OTP No"
                clientidmode="Static" tabindex="1" maxlength="100" onkeypress="return IsNumeric(event);" />
                
            </div>
            <div class="company_list">
                <asp:Button ID="btnOTPValidation" runat="server" Text="Verify OTP" CssClass="submit_btn" TabIndex="17" OnClientClick="return validateOTP()" />
            </div>
            <div class="company_list" style="visibility:hidden;">
                <label>
                    Client</label>
                <asp:DropDownList ID="DDLSelected" runat="server" class="chosen-select" TabIndex="13"
                    AutoPostBack="true" />
            </div>
        </div>
        <div class="client_box">
            <ul id="multiselected">
                <asp:Literal ID="LtrlSelectedDDl" runat="server"></asp:Literal>
            </ul>
        </div>
    </div>
    <div id="divImage" runat="server" align="center">
        <div class="company_list" style="width: 35%;">
            <h2>
                Logo Image View before upload</h2>
            <img id="imgprvw" alt="Main Menu" runat="server" clientidmode="Static" src="../Resource/Images/Super.png"
                style="height: auto; border: 1px solid black; float: left; max-width: 100%; margin: 0px 10px 0px 0px;" />
            <br />
            <%--<div class="company_list">--%>
            <asp:FileUpload ID="flupload" runat="server" ClientIDMode="Static" CssClass="btnupload"
                TabIndex="14" Style="float: left; background-color: #fff; border: 0px none; padding: 5px;
                width: auto; margin: 0px 0px 10px;" />
            <%--</div>--%>
            <br />
            <asp:Button ID="btnUpload" runat="server" ClientIDMode="Static" CssClass="submit_btn"
                OnClick="fnUpload" Text="View" Style="float: left; padding: 5px 10px; font-size: 12px;"
                OnClientClick="return validateImageUpload()" TabIndex="15" />
            <%--OnClick="fnUpload" --%>
            <asp:Button ID="BtnClearImage" runat="server" ClientIDMode="Static" CssClass="submit_btn"
                Text="Clear" Style="float: left; padding: 5px 10px; font-size: 12px;" TabIndex="16" />
            <%--OnClick="fnClear"--%>
        </div>
        <div class="company_list" style="width: 60%;">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="submit_btn" TabIndex="17"
                OnClientClick="return validate()" />
            <asp:Button ID="btnreset" runat="server" Text="Reset" CssClass="submit_btn" TabIndex="18"
                OnClientClick="return resetFields()" />
           <a href="AspxUnlockUser.aspx" class="submit_btn">Unlock</a>
           
            <%--<a id="qwe" href="?SearchKe" class="submit_btn"></a>--%>
        </div>
    </div>
    <asp:Button ID="BtnRemoveSelected" runat="server" Style="display: none" />
    <div id="divOTP" runat="server">
    <div class="company_list" style="width: 20%;">
    </div>
        <div class="company_list">
        </div>
        <div class="company_list">
        <label style="margin: 13px 0 0 0"></label>
            
        </div>
    </div>
    </form>
    <script type="text/javascript" language="javascript">
        /*Alpha Numeric License,LIC,Mediclaim No Validation*/
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        specialKeys.push(32); //SpaceBar
        specialKeys.push(9); //Tab
        specialKeys.push(46); //Delete
        specialKeys.push(36); //Home
        specialKeys.push(35); //End
        specialKeys.push(37); //Left
        specialKeys.push(39); //Right
        //specialKeys.push(64); //@ symbol
        function IsOnlyAlphabets(e) {
            var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
            var ret = ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }
    </script>
    <script type="text/javascript">
        /*Validation*/
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        specialKeys.push(39); //Apostrophe
        specialKeys.push(32); //SpaceBar
        function IsNameWithApstrophe(e) {
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || specialKeys.indexOf(keyCode) != -1);
            return ret;
        }
    </script>
    <script type="text/javascript">
        /*Address1 And Address2 Validation*/
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        specialKeys.push(39); //Apostrophe
        specialKeys.push(32); //SpaceBar
        //specialKeys.push(35); // # symbol
        function IsAddress(e) {
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 35 && keyCode <= 37) || (keyCode >= 44 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || specialKeys.indexOf(keyCode) != -1);
            return ret;
        }
    </script>
    <script type="text/javascript" language="javascript">
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        specialKeys.push(32); //SpaceBar
        specialKeys.push(9); //Tab
        specialKeys.push(46); //Delete
        specialKeys.push(36); //Home
        specialKeys.push(35); //End
        specialKeys.push(37); //Left
        specialKeys.push(39); //Right
        function IsNumeric(e) {
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
            return ret;
        }
    </script>
    <script type="text/javascript" language="javascript">
        /*Email Validation*/
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        specialKeys.push(32); //SpaceBar
        specialKeys.push(9); //Tab
        //specialKeys.push(46); //Delete
        //specialKeys.push(36); //Home
        //specialKeys.push(35); //End
        //specialKeys.push(37); //Left
        //specialKeys.push(39); //Right
        specialKeys.push(64); //@ symbol
        specialKeys.push(190); //. symbol
        function IsAlphaNumericForEmail(e) {
            var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
            var ret = ((keyCode >= 35 && keyCode <= 39) || (keyCode >= 8 && keyCode <= 9) || (keyCode >= 45 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 95 && keyCode <= 122) || (keyCode >= 64 && keyCode <= 90) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }
    </script>
    <script type="text/javascript" language="javascript">
        /*Alpha Numeric License,LIC,Mediclaim No Validation*/
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        specialKeys.push(32); //SpaceBar
        specialKeys.push(9); //Tab
        specialKeys.push(46); //Delete
        specialKeys.push(36); //Home
        specialKeys.push(35); //End
        specialKeys.push(37); //Left
        specialKeys.push(39); //Right
        specialKeys.push(64); //@ symbol
        function IsAlphaNumericFormat(e) {
            var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
            var ret = ((keyCode >= 45 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }
    </script>
    <script type="text/javascript" language="javascript">
        /*Alpha Numeric Validation*/
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        specialKeys.push(32); //SpaceBar
        specialKeys.push(9); //Tab
        specialKeys.push(46); //Delete
        specialKeys.push(36); //Home
        specialKeys.push(35); //End
        specialKeys.push(37); //Left
        specialKeys.push(39); //Right
        function IsAlphaNumeric(e) {
            var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
            // var ret = ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
            var ret = ((keyCode >= 35 && keyCode <= 39) || (keyCode >= 8 && keyCode <= 9) || (keyCode >= 48 && keyCode <= 57) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }
    </script>
    <script type="text/javascript">
        function ValidatePassword() {
            if (document.getElementById("<%=txtPassword.ClientID %>").value.trim() == "") {
                alert("Please Provide New Password");
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
    <script type="text/javascript">
        /*Zipcode validation*/
        function CheckZipcode(e, field) {
            var val = field.value.trim();
            var re = /^([0-9]+[\.]?[0-9]?[0-9]?|[0-9]+)$/g;
            var re1 = /^([0-9]+[\.]?[0-9]?[0-9]?|[0-9]+)/g;
            if (re.test(val)) {
                //do something here
                var n = val.length;
                if (n < 6) {
                    var aler = "".concat("current length is ", n, " and should be equal to 6 digit");
                    alert(aler);
                    field.focus();
                    return false;
                }
                else {
                    return true;
                }
            } else {
                val = re1.exec(val);
                if (val) {
                    field.value = val[0];
                } else {
                    alert('Characters not allowed for Zip Code.');
                    field.value = "";
                    field.focus();
                    return false;
                }
            }
        }
    </script>
    <script type="text/javascript" language="javascript">
        function resetFields() {
            document.getElementById("<%=DDLCompany.ClientID%>").value = "0";
            document.getElementById("<%=TxtUserName.ClientID%>").value = "";
            document.getElementById("<%=TxtUserFullName.ClientID%>").value = "";
            document.getElementById("<%=TxtUserAddress.ClientID%>").value = "";
            document.getElementById("<%=TxtEmailId.ClientID%>").value = "";
            document.getElementById("<%=TxtPhoneNo.ClientID%>").value = "";
            document.getElementById("<%=DDLIDRole.ClientID%>").value = "0";
            document.getElementById("<%=TxtPament.ClientID%>").value = "";
            document.getElementById("<%=TxtIncetivePercent.ClientID%>").value = "";
            document.getElementById("<%=TxtPenaltyPercent.ClientID%>").value = "";

            return true;
        }
    </script>

    <script type="text/javascript" language="javascript">
        function validateImageUpload() {
            var avatar = document.getElementById("<%=flupload.ClientID%>");
            if (avatar.value == "") {
                alert("Please browse the image first!");
                return false
            }
            return true
        }
        function validateOTP() {
            if (document.getElementById("<%=txtOTPNO.ClientID %>").value.trim() == "") {
                alert("Please give OTP No");
                document.getElementById("<%=txtOTPNO.ClientID %>").focus();
                return false;
            }
        }
        function validate() {
            if (document.getElementById("<%=DDLIDRole.ClientID %>").value.trim() == "0") {
                alert("Please select the Role");
                document.getElementById("<%=DDLIDRole.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDLIDRole.ClientID %>").value.trim() == "2" || document.getElementById("<%=DDLIDRole.ClientID %>").value.trim() == "3") {
                if (document.getElementById("<%=DDLCompany.ClientID %>").value.trim() == "0") {
                    alert("Please Select Company Name");
                    document.getElementById("<%=DDLCompany.ClientID %>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TxtUserName.ClientID %>").value.trim() == "") {
                alert("Please Provide User Name for Application Access");
                document.getElementById("<%=TxtUserFullName.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=TxtUserFullName.ClientID %>").value.trim() == "") {
                alert("Please give User's Full Name");
                document.getElementById("<%=TxtUserFullName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TxtUserAddress.ClientID %>").value.trim() == "") {
                alert("Please give User's Address");
                document.getElementById("<%=TxtUserAddress.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TxtEmailId.ClientID %>").value.trim() == "") {
                alert("Please give User's Email Address");
                document.getElementById("<%=TxtEmailId.ClientID %>").focus();
                return false;
            }
            var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
            var inputText;
            inputText = document.getElementById("<%=TxtEmailId.ClientID %>");
            if (inputText.value.match(mailformat)) {
                //alert('valid email');
            }
            else {
                alert("You have entered an invalid EmailId!");
                document.getElementById("<%=TxtEmailId.ClientID %>").value = "";
                //document.form1.text1.focus();
                document.getElementById("<%=TxtEmailId.ClientID %>").focus();
                return false;

            }

            if (document.getElementById("<%=DDLIDRole.ClientID %>").value.trim() == "4") {
                if (document.getElementById("<%=TxtPament.ClientID %>").value.trim() == "") {
                    alert("Please Provide User Payment for Per Entry");
                    document.getElementById("<%=TxtPament.ClientID %>").focus();
                    return false;
                }
                if (document.getElementById("<%=TxtIncetivePercent.ClientID %>").value.trim() == "") {
                    alert("Please Provide User Incentive Percent Only Numbers");
                    document.getElementById("<%=TxtIncetivePercent.ClientID %>").focus();
                    return false;
                }
                if (document.getElementById("<%=TxtPenaltyPercent.ClientID %>").value.trim() == "") {
                    alert("Please Provide User Penalty Percent Only Numbers");
                    document.getElementById("<%=TxtPenaltyPercent.ClientID %>").focus();
                    return false;
                }
            }

            if (document.getElementById("<%=TxtPhoneNo.ClientID %>").value.trim() == "") {
                alert("Please give User's Contact No.");
                document.getElementById("<%=TxtPhoneNo.ClientID %>").focus();
                return false;
            }
            return true;

        }
    </script>
    <script type="text/javascript">
        function RemoveSelectedFromList(srcId, srcName) {
            //                        alert('a');
            //                        alert(srcId);
            document.getElementById('<%=HdnIdSelected.ClientId%>').value = srcId;
            document.getElementById('<%=HdnSelected.ClientId%>').value = srcName;
            callDummyBtnRemoveSelected();
        }
        function callDummyBtnRemoveSelected() {
            $('input[id$=BtnRemoveSelected]').click();
        }
    </script>
    <script src="../Resource/js/Chosen/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        var config = {
            '.chosen-select': {},
            '.chosen-select-deselect': { allow_single_deselect: true },
            '.chosen-select-no-single': { disable_search_threshold: 10 },
            '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' },
            '.chosen-select-width': { width: "95%" }
        }
        for (var selector in config) {
            $(selector).chosen(config[selector]);
        }
    </script>
</asp:Content>
