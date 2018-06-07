<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false" CodeFile="AspxAddGSTHSNData.aspx.vb" Inherits="AspxPages_AspxAddGSTHSNData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script>
        $(function () {
            $("#TxtGSTRateWEF").datepicker({
                changeMonth: true,
                changeYear:true,
                dateFormat: 'dd-M-yy',
                onClose: function () {
                    $(this).trigger('blur');
                }
            }).val();
        });
    </script>
    <form id="frmApplicationsetting" runat="server" clientidmode="Static" role="form">
        <div id="resultsel1" style="display: block" class="company_details">
             <div class="company_list">
                <label>
                    GSTCode(HSN Code) *
                </label>
                <input type="text" class="form-control" runat="server" id="TxtGSTCode" placeholder="GSTCode (HSN Code)"
                    clientidmode="Static" tabindex="1" maxlength="25" />
            </div>
            <div class="company_list">
                <label>
                    GST Code Description *
                </label>
                <input type="text" class="form-control" runat="server" id="TxtGSTCodeDescription" placeholder="GST Code Description"
                    clientidmode="Static" tabindex="2" maxlength="200" />
            </div>
            <div class="company_list">
                <label>
                    C-GST Rate *
                </label>
                <input type="text" class="form-control" runat="server" id="TxtCGSTRate" placeholder="C-GST Rate"
                    clientidmode="Static" tabindex="3" maxlength="5" />
            </div>
            <div class="company_list">
                <label>
                    S GST Rate *
                </label>
                <input type="text" class="form-control" runat="server" id="TxtSGSTRate" placeholder="S-GST Rate"
                    clientidmode="Static" tabindex="4" maxlength="5" />
            </div>
            <div class="company_list">
                <label>
                    I-GST Rate *
                </label>
                <input type="text" class="form-control" runat="server" id="TxtIGSTRate" placeholder="I-GST Rate"
                    clientidmode="Static" tabindex="5" maxlength="5" />
            </div>
            <div class="company_list">
                <label>
                    Cess Condition *
                </label>
                <input type="text" class="form-control" runat="server" id="TxtCessCondition" placeholder="Cess Condition"
                    clientidmode="Static" tabindex="6" maxlength="255" onkeypress="return IsNumericforName(event);" />
            </div>
                            <div class="company_list date_box">
                    <label>
                       Date With Effect From *
                    </label>
                    <asp:TextBox ID="TxtGSTRateWEF" runat="server" ClientIDMode="Static" class="form-control"
                        onblur="return LoadValuetoSerchTag()" placeholder="Date GST W.E.F. (DD-MMM-YYYY)"
                        TabIndex="7" MaxLength="11" />
                </div>
            <div class="company_list">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="submit_btn" OnClientClick="return validate()"
                    TabIndex="13" />
               
            </div>
        </div>
        <script type="text/javascript">
            function validate() {
                if (document.getElementById("<%=TxtGSTCode.ClientID %>").value.trim() == "") {
                    alert("Please Provide GSTCode (HSN Code) as per Applicable");
                    document.getElementById("<%=TxtGSTCode.ClientID %>").focus();
                    return false;
                }
                if (document.getElementById("<%=TxtGSTCodeDescription.ClientID %>").value.trim() == "") {
                    alert("Please Provide GST Code Description as per Applicable");
                    document.getElementById("<%=TxtGSTCodeDescription.ClientID %>").focus();
                    return false;
                }
                if (document.getElementById("<%=TxtCGSTRate.ClientID %>").value.trim() == "") {
                    alert("Please Provide C-GST Rate as per Applicable");
                    document.getElementById("<%=TxtCGSTRate.ClientID %>").focus();
                    return false;
                }
                if (document.getElementById("<%=TxtSGSTRate.ClientID %>").value.trim() == "") {
                    alert("Please Provide S-GST Rate as per Applicable");
                    document.getElementById("<%=TxtSGSTRate.ClientID %>").focus();
                    return false;
                }
                if (document.getElementById("<%=TxtIGSTRate.ClientID %>").value.trim() == "") {
                    alert("Please Provide I-GST Rate as per Applicable");
                    document.getElementById("<%=TxtIGSTRate.ClientID %>").focus();
                    return false;
                }
                
                var CGST = document.getElementById("<%=TxtCGSTRate.ClientID %>").value.trim();
                var SGST = document.getElementById("<%=TxtSGSTRate.ClientID%>").value.trim();
                var IGST = +CGST + +SGST;
                var IGSTP = document.getElementById("<%=TxtIGSTRate.ClientID%>").value.trim();
                if (IGSTP != IGST){
                    alert("Provided I-GST Rate is not matching as per Applicable");
                    document.getElementById("<%=TxtIGSTRate.ClientID %>").focus();
                    return false;
                }
                 if (document.getElementById("<%=TxtCessCondition.ClientID %>").value.trim() == "") {
                    alert("Please Provide Cess Condition as per Applicable");
                    document.getElementById("<%=TxtCessCondition.ClientID %>").focus();
                    return false;
                }
                 if (document.getElementById("<%=TxtGSTRateWEF.ClientID %>").value.trim() == "") {
                    alert("Please Provide GST Rate With Effect From Date as per Applicable");
                    document.getElementById("<%=TxtGSTRateWEF.ClientID %>").focus();
                    return false;
                }
                    
                
            }

        </script>
    </form>
</asp:Content>

