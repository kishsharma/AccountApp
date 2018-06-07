<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false"
    CodeFile="AspxApplicationSetting.aspx.vb" Inherits="AspxPages_AspxApplicationSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        $(function () {
            $("#TxtRowCreatedDt").datepicker({
                changeMonth: true,
                changeYear:true,
                dateFormat: 'dd-M-yy'
            }).val();
            $("#TxtlastUpdtDt").datepicker({
                changeMonth: true,
                changeYear:true,
                dateFormat: 'dd-M-yy'
            }).val();
        });
    </script>
    <form id="frmApplicationsetting" runat="server" clientidmode="Static" role="form">
     <asp:HiddenField ID="HdnIdentity" runat="server" ClientIDMode="Static" />
    <div class="company_box">
        <div id="resultsel1" style="display: block" class="company_details">
             <div class="company_list">
                <label>
                    SMTP *
                </label>
                <input type="text" class="form-control" runat="server" id="TxtSMTP" placeholder="SMTP"
                    clientidmode="Static" tabindex="7" maxlength="200" />
            </div>
            <div class="company_list">
                <label>
                    Port *
                </label>
                <input type="text" class="form-control" runat="server" id="TxtPort" placeholder="Port"
                    clientidmode="Static" tabindex="8" maxlength="200" />
            </div>
            <div class="company_list">
                <label>
                    SMTP Username *
                </label>
                <input type="text" class="form-control" runat="server" id="TxtSMTPUsername" placeholder="SMTP Username"
                    clientidmode="Static" tabindex="9" maxlength="200" />
            </div>
            <div class="company_list">
                <label>
                    SMPT Password *
                </label>
                <input type="text" class="form-control" runat="server" id="TxtSMTPPassword" placeholder="SMPT Password"
                    clientidmode="Static" tabindex="10" maxlength="200" />
            </div>
            <div class="company_list">
                <label>
                    SMS Url *
                </label>
                <input type="text" class="form-control" runat="server" id="TxtSMSUrl" placeholder="SMS Url"
                    clientidmode="Static" tabindex="11" maxlength="200" />
            </div>
            <div class="company_list">
                <label>
                    Accuracy level for incentive/penalty in Percent(95% or 80%): *
                </label>
                <input type="text" class="form-control" runat="server" id="TxtAccuracylevel" placeholder="Accuracy level for incentive/penalty in Percent(%)"
                    clientidmode="Static" tabindex="12" maxlength="100" onkeypress="return IsNumericforName(event);" />
            </div>
            <div class="company_list">
                <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="submit_btn" OnClientClick="return validate()"
                    TabIndex="13" />
               
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" language="javascript">
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        specialKeys.push(9); //Tab
        specialKeys.push(46); //Delete
        specialKeys.push(36); //Home
        specialKeys.push(35); //End
        specialKeys.push(37); //Left
        specialKeys.push(39); //Right
        function IsNumericforName(e) {
            var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
            var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 46 || (keyCode == 37) || (specialKeys.indexOf(keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }
    </script>
     <script type="text/javascript" language="javascript">
         function validate() {
             if (document.getElementById("<%=TxtSMTP.ClientID %>").value.trim() == "") {
                 alert("Please Provide SMTP Details");
                 document.getElementById("<%=TxtSMTP.ClientID %>").focus();
                 return false;
             }
             if (document.getElementById("<%=TxtPort.ClientID %>").value.trim() == "") {
                 alert("Please Provide Port no");
                 document.getElementById("<%=TxtPort.ClientID %>").focus();
                 return false;
             }
             if (document.getElementById("<%=TxtSMTPUsername.ClientID %>").value.trim() == "") {
                 alert("Please Provide SMTP Username");
                 document.getElementById("<%=TxtSMTPUsername.ClientID %>").focus();
                 return false;
             }
             if (document.getElementById("<%=TxtSMTPPassword.ClientID %>").value.trim() == "") {
                 alert("Please Provide SMTP Password");
                 document.getElementById("<%=TxtSMTPPassword.ClientID %>").focus();
                 return false;
             }

             if (document.getElementById("<%=TxtSMSUrl.ClientID %>").value.trim() == "") {
                 alert("Please Provide SMS Url");
                 document.getElementById("<%=TxtSMSUrl.ClientID %>").focus();
                 return false;
             }
             if (document.getElementById("<%=TxtAccuracylevel.ClientID %>").value.trim() == "") {
                 alert("Please Provide Accuracylevel");
                 document.getElementById("<%=TxtAccuracylevel.ClientID %>").focus();
                 return false;
             }
             return true;
         }
     </script>
</asp:Content>
