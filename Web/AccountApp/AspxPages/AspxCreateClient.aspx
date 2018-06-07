<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false"
    CodeFile="AspxCreateClient.aspx.vb" Inherits="AspxPages_AspxCreateClient" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!--calendar-->
    <script type="text/javascript">
        $(function () {
            // $('input:text:first').focus();
            var $inp = $('.form-control');
            $inp.bind('keydown', function (e) {
                //var key = (e.keyCode ? e.keyCode : e.charCode);
                var key = e.which;
                if (key == 13) {
                    e.preventDefault();
                    var nxtIdx = $inp.index(this) + 1;
                    $(".form-control:eq(" + nxtIdx + ")").focus();
                }
            });
        });
    </script>
    <script>
        $(function () {
            $("#TxtFinancialYearFrom").datepicker({
                changeMonth: true,
                changeYear:true,
                dateFormat: 'dd-M-yy'
            }).val();
            $("#TxtBooksBeginFrom").datepicker({
                changeMonth: true,
                changeYear:true,
                dateFormat: 'dd-M-yy'
            }).val();
        });
    </script>
    <!--calendar-->
    <form id="frmClient" runat="server" clientidmode="Static" role="form">
    <style type="text/css">
        .heading15
        {
            float: left;
            width: 98%;
            height: auto;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 15px;
            color: #000000;
            text-transform: uppercase;
            padding: 0px 0 10px 0;
        }
    </style>
    <asp:HiddenField ID="HdnIdentity" runat="server" ClientIDMode="Static" Value="0" />
    <asp:HiddenField ID="HdnIdCompany" runat="server" ClientIDMode="Static" Value="0" />
    <asp:ScriptManager ID="SMLocationMaster" runat="server" EnablePageMethods="true"
        ScriptMode="Release">
    </asp:ScriptManager>
    <div class="heading24">
        <strong>Create company</strong></div>
    <div class="company_box">
        <div class="tabbing_menu">
            <span id='sel1' class="box_text" style="background-color: #00bcd5; color: #ffffff" onclick="show('sel1','resultsel1');">Create Company</span> 
            <span id='sel2' class="box_text" onclick="show('sel2','resultsel2');">Banking Details</span>
            <span id='sel3' class="box_text" onclick="show('sel3','resultsel3');">Suppliers Details</span>
            <%--            
            <span id='sel4' class="box_text" onclick="show('sel4','resultsel4');">Accounting Features</span> 
            <span id='sel5' class="box_text" onclick="show('sel5','resultsel5');">Statutory and Taxation</span> 
            <span id='sel6' class="box_text" onclick="show('sel6','resultsel6');">Inventory Feature</span>
            <span id='sel7' class="box_text" onclick="show('sel7','resultsel7');">Base Currency Information</span> 
            --%>
        </div>
        <div class="tabbing_box">
            <div id="resultsel1" style="display: block" class="company_details">
                <div class="company_list">
                    <label>
                        Company Name: *
                    </label>
                    <input type="text" class="form-control" runat="server" id="TxtCompanyName" placeholder="Company Name"
                        clientidmode="Static" tabindex="1" maxlength="100" onkeypress="return IsNumericforName(event);" />
                </div>
                <div class="company_list">
                    <label>
                        Mailing Address *
                    </label>
                    <input type="text" class="form-control" runat="server" id="TxtMailingAddress" placeholder="Mailing Address"
                        onkeypress="return ValidateAddress(event);" clientidmode="Static" tabindex="2"
                        maxlength="200" />
                </div>
                <div class="company_list">
                    <label>
                        Statutory Compliance For *
                    </label>
                    <input type="text" class="form-control" runat="server" id="TxtStatutoryComplianceFor"
                        placeholder="Statutory Compliance For" clientidmode="Static" tabindex="3" maxlength="50"
                        onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="company_list">
                    <label>
                        State *
                    </label>
                    <input type="text" class="form-control" runat="server" id="TxtState" placeholder="State"
                        clientidmode="Static" tabindex="4" maxlength="255" onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="company_list">
                    <label>
                        PIN *
                    </label>
                    <input type="text" class="form-control" runat="server" id="TxtPIN" placeholder="PIN"
                        clientidmode="Static" tabindex="5" maxlength="6" onblur="CheckZipcode(event , this);"
                        onkeypress="return IsNumericZipFaxNo(event);" ondrop="return false;" onpaste="return true;" />
                </div>
                <div class="company_list">
                    <label>
                        Telephone No *
                    </label>
                    <input type="text" class="form-control" runat="server" id="TxtTelephoneNo" placeholder="Telephone No"
                        clientidmode="Static" tabindex="6" maxlength="13" onkeypress="return IsNumeric(event);" />
                </div>
                <div class="company_list">
                    <label>
                        Mobile *
                    </label>
                    <input type="text" class="form-control" runat="server" id="TxtMobile" placeholder="Mobile"
                        clientidmode="Static" tabindex="7" maxlength="10" onkeypress="return IsNumeric(event);" />
                </div>
                <div class="company_list">
                    <label>
                        Email *
                    </label>
                    <input type="text" class="form-control" runat="server" id="TxtEmail" placeholder="Email"
                        clientidmode="Static" tabindex="8" maxlength="255" onblur="ValidateEmail(this);"
                        ondrop="return false;" onpaste="return true;" />
                </div>
                <div class="company_list">
                    <label>
                        Currency Symbol *
                    </label>
                    <input type="text" class="form-control" runat="server" id="TxtCurrencySymbol" placeholder="Currency Symbol"
                        clientidmode="Static" tabindex="8" maxlength="255" ondrop="return false;" onpaste="return true;" />
                </div>
                <div class="company_list">
                    <label>
                        Maintain *
                    </label>
                    <asp:DropDownList ID="ddlMaintain" runat="server" TabIndex="31">
                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                        <asp:ListItem Value="Accounts only">Accounts only</asp:ListItem>
                        <asp:ListItem Value="Account with Inventory">Account with Inventory</asp:ListItem>
                    </asp:DropDownList>
                    <%-- <input type="text" class="form-control" runat="server" id="TxtMaintain" placeholder="Maintain Accounts with Inventory"
                        clientidmode="Static" tabindex="10" maxlength="255" onkeypress="return IsAlphaNumeric(event);" />--%>
                </div>
                <div class="company_list date_box">
                    <label>
                        Accounts Maintained From *
                    </label>
                    <asp:TextBox ID="TxtFinancialYearFrom" runat="server" ClientIDMode="Static" class="form-control"
                        placeholder="Financial Year From (DD-MMM-YYYY)" TabIndex="11" MaxLength="11" />
                </div>
                <div class="company_list date_box">
                    <label>
                        Books Begin From *
                    </label>
                    <asp:TextBox ID="TxtBooksBeginFrom" runat="server" ClientIDMode="Static" class="form-control"
                        placeholder="Books Begin From (DD-MMM-YYYY)" TabIndex="12" MaxLength="11" />
                </div>
                <div class="company_list">
                    <label>
                        Client Short Code *
                    </label>
                    <input type="text" class="form-control" runat="server" id="TxtClientShortCode" placeholder="Client Short Code"
                        clientidmode="Static" tabindex="13" maxlength="4" onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkAutoBackup" class="form-control" runat="server" clientidmode="Static"
                        tabindex="13" />
                    <label class="panel_list_text" for="ChkAutoBackup">
                        <span></span>Enable Auto Backup
                    </label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkSecurityControl" class="form-control" runat="server"
                        clientidmode="Static" tabindex="14" />
                    <label class="panel_list_text" for="ChkSecurityControl">
                        <span></span>Use Security Control
                    </label>
                </div>
                <div class="heading15" style="font-weight: bold; margin: 20px 0px 0px; color: #fff;
                    padding: 9px 10px; background-color: #00bcd5">
                    <h2>
                        Taxation No.</h2>
                </div>
                <div class="company_list" style="visibility:hidden;">
                    <label>
                        ECC No.</label>
                    <input type="text" class="form-control" runat="server" id="TxtECCNO" placeholder="ECC No."
                        clientidmode="Static" tabindex="14" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="company_list" style="visibility:hidden;">
                    <label>
                        CST TIN No.</label>
                    <input type="text" class="form-control" runat="server" id="TxtCSTTINNO" placeholder="CST TIN No."
                        clientidmode="Static" tabindex="15" maxlength="11" onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="company_list" style="visibility:hidden;">
                    <label>
                        VAT TIN No.</label>
                    <input type="text" class="form-control" runat="server" id="TxtVATTINNO" placeholder="VAT TIN No."
                        clientidmode="Static" tabindex="16" maxlength="11" onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="company_list">
                    <label>
                        GST No.</label>
                    <input type="text" class="form-control" runat="server" id="TxtGSTNO" placeholder="GST No."
                        clientidmode="Static" tabindex="14" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="company_list">
                    <label>
                        PANo.</label>
                    <input type="text" class="form-control" runat="server" id="TxtPANo" placeholder="PANo"
                        clientidmode="Static" tabindex="15" maxlength="11" onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div align="center">
                    <div class="company_list" style="width: 35%;">
                        <h2>
                            Logo Image View before upload</h2>
                        <img id="imgprvw" alt="Main Menu" runat="server" clientidmode="Static" src="../Resource/Images/Super.png"
                            style="height: auto; border: 1px solid black; float: left; max-width: 100%; margin: 0px 10px 0px 0px;" />
                        <asp:FileUpload ID="flupload" runat="server" ClientIDMode="Static" CssClass="btnupload"
                            Style="float: left; background-color: #fff; border: 0px none; padding: 5px; width: auto;
                            margin: 0px 0px 10px;" TabIndex="15" />
                        <asp:Button ID="btnUpload" runat="server" ClientIDMode="Static" CssClass="submit_btn"
                            OnClick="fnUpload" Text="View" Style="float: left; padding: 5px 10px; font-size: 12px;"
                            OnClientClick="return validateImageUpload()" TabIndex="12" />
                        <asp:Button ID="BtnClearImage" runat="server" ClientIDMode="Static" CssClass="submit_btn"
                            Style="float: left; padding: 5px 10px; font-size: 12px;" Text="Clear" TabIndex="16" />
                    </div>
                    <div class="heading15">
                        <div align="center">
                        </div>
                    </div>
                </div>
            </div>
            <div id="resultsel2" class="box company_details">
                <div class="company_list">
                    <label>
                        Bank Name: *
                    </label>
                    <input type="text" class="form-control" runat="server" id="TxtBankName" placeholder="Bank Name"
                        clientidmode="Static" tabindex="1" maxlength="100" onkeypress="return IsNumericforName(event);" />
                </div>
                <div class="company_list">
                    <label>
                        Branch Name: *
                    </label>
                    <input type="text" class="form-control" runat="server" id="TxtBankBranchName" placeholder="Branch Name"
                        clientidmode="Static" tabindex="1" maxlength="100" onkeypress="return IsNumericforName(event);" />
                </div>
                <div class="company_list">
                    <label>
                        Account number: *
                    </label>
                    <input type="text" class="form-control" runat="server" id="TxtBankAccountNumber" placeholder="Account number"
                        clientidmode="Static" tabindex="1" maxlength="100"/>
                </div>
                <div class="company_list">
                    <label>
                        IFSC Code: *
                    </label>
                    <input type="text" class="form-control" runat="server" id="TxtBankIFSCCode" placeholder="IFSC Code"
                        clientidmode="Static" tabindex="1" maxlength="100"/>
                </div>
            </div>
            <div id="resultsel3" class="box company_details">
                <asp:UpdatePanel ID="updtPnlPopUp1" runat="server">
                    <ContentTemplate>
                <div class="company_list">
                    <label id="Label1" runat="server" clientidmode="Static">
                        Name Of Party</label>
                    <input type="text" class="form-control" runat="server" id="NameofParty" placeholder="Name Of Party"
                        clientidmode="Static" tabindex="17" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="company_list">
                    <label id="Label2" runat="server" clientidmode="Static">
                        Party Address</label>
                    <input type="text" class="form-control" runat="server" id="PartyAddress" placeholder="Party Address"
                        clientidmode="Static" tabindex="18" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="company_list">
                    <label id="Label3" runat="server" clientidmode="Static">
                        City</label>
                    <input type="text" class="form-control" runat="server" id="City" placeholder="City"
                        clientidmode="Static" tabindex="19" maxlength="25" onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="company_list">
                    <label id="Label4" runat="server" clientidmode="Static">
                        Excise/Service Tax No</label>
                    <input type="text" class="form-control" runat="server" id="ExciseServiceTaxNo" placeholder="Excise/Service Tax No"
                        clientidmode="Static" tabindex="20" maxlength="25" onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="company_list">
                    <label id="Label5" runat="server" clientidmode="Static">
                        Tin No</label>
                    <input type="text" class="form-control" runat="server" id="TinNo" placeholder="Tin No"
                        clientidmode="Static" tabindex="21" maxlength="25" onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="company_list">
                    <label style="margin: 13px 0 0 0">
                    </label>
                    <asp:Button ID="BtnAdd" runat="server" ClientIDMode="Static" Text="Add" CssClass="submit_btn"
                        OnClientClick="return validateSupplier()" />
                </div>
                <div class="company_list">
                    <asp:HiddenField id="HdnSupplierid" runat="server" />
                    <asp:GridView ID="Gvdetails" CssClass="total_list" HeaderStyle-ForeColor="Black"
                        ShowHeader="True" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="SlNo" ItemStyle-Width="37px">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="NameofParty" HeaderText="Party Name">
                                <ItemStyle VerticalAlign="Middle" HorizontalAlign="left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="City" HtmlEncode="false" HeaderText="City">
                                <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="180px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PartyAddress" HeaderText="Party Address">
                                <ItemStyle VerticalAlign="Middle" Width="100px" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ExciseServiceTaxNo" HeaderText="Excise/Service TaxNo">
                                <ItemStyle VerticalAlign="Middle" Width="60px" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TinNo" HeaderText="TinNo">
                                <ItemStyle VerticalAlign="Middle" Width="60px" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SupplierID" HeaderText="SupplierID">
                                <ItemStyle VerticalAlign="Middle" Width="60px" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Edit?">
                                <ItemTemplate>
                                    <span onclick="return confirm('Are you sure to Edit the record?')">
                                        <asp:LinkButton ID="lnkB" runat="Server" Text="Edit" CommandName="EditRow"></asp:LinkButton>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="resultsel4" class="box company_details">
                <div class="expand_collapse_box">
                    <span class="accordion">General</span>
                    <div class="panel">
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFIntegrateAccountsandInventory" runat="server" clientidmode="Static"
                                tabindex="14" />
                            <label class="panel_list_text" for="ChkAFIntegrateAccountsandInventory">
                                <span></span>Integrate Accounts and Inventory
                            </label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFIncomeandExpenceStatement" runat="server" clientidmode="Static"
                                tabindex="14" />
                            <label class="panel_list_text" for="ChkAFIncomeandExpenceStatement">
                                <span></span>Income and Expence Statement
                            </label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFAllowMultiCurrency" runat="server" clientidmode="Static"
                                tabindex="14" />
                            <label class="panel_list_text" for="ChkAFAllowMultiCurrency">
                                <span></span>Allow Multi Currency
                            </label>
                        </div>
                    </div>
                    <span class="accordion">Outstanding Management</span>
                    <div class="panel">
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFMaintainBillwiseDetails" runat="server" clientidmode="Static" />
                            <label for="ChkAFMaintainBillwiseDetails">
                                <span></span>Maintain Bill-wise Details(For Non-Trading A/cs Also)
                            </label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFActivateInterestCalculation" runat="server" clientidmode="Static" />
                            <label for="ChkAFActivateInterestCalculation">
                                <span></span>Activate Interest Calculation(Use Advance Parameters)</label>
                        </div>
                    </div>
                    <span class="accordion">Cost / Profit Centers Management</span>
                    <div class="panel">
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFMaintainPayroll" runat="server" clientidmode="Static" />
                            <label for="ChkAFMaintainPayroll">
                                <span></span>Maintain Payroll
                            </label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFMaintainCostCentres" runat="server" clientidmode="Static" />
                            <label for="ChkAFMaintainCostCentres">
                                <span></span>Maintain Cost Centres
                            </label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFUseCostCentreforJobCosting" runat="server" clientidmode="Static" />
                            <label for="ChkAFUseCostCentreforJobCosting">
                                <span></span>Use Cost Centre for Job Costing</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFMorethanONEPayrollCostCategory" runat="server" clientidmode="Static" />
                            <label for="ChkAFMorethanONEPayrollCostCategory">
                                <span></span>More than ONE Payroll / Cost Category</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFUsePreDefinedCostCenterAllocationDuringEntry" runat="server"
                                clientidmode="Static" />
                            <label for="ChkAFUsePreDefinedCostCenterAllocationDuringEntry">
                                <span></span>Use Pre-Defined Cost Center Allocation During Entry</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFShowOpeningBalanceforRevenueItemInReport" runat="server"
                                clientidmode="Static" />
                            <label for="ChkAFShowOpeningBalanceforRevenueItemInReport">
                                <span></span>Show Opening Balance for Revenue Items in Reports</label>
                        </div>
                    </div>
                    <span class="accordion">Invoicing</span>
                    <div class="panel">
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFAllowInvoicing" runat="server" clientidmode="Static" />
                            <label for="ChkAFAllowInvoicing">
                                <span></span>Allow Invoicing
                            </label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFEnterPurchasesinInvoiceFormat" runat="server" clientidmode="Static" />
                            <label for="ChkAFEnterPurchasesinInvoiceFormat">
                                <span></span>Enter Purchases in Invoice Format</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFUseDebitCreditNotes" runat="server" clientidmode="Static" />
                            <label for="ChkAFUseDebitCreditNotes">
                                <span></span>Use Debit / Credit Notes
                            </label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFUseInvoiceModeforCreditNotes" runat="server" clientidmode="Static" />
                            <label for="ChkAFUseInvoiceModeforCreditNotes">
                                <span></span>Use Invoice Mode for Credit Notes</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFUseInvoiceModeforDebitNotes" runat="server" clientidmode="Static" />
                            <label for="ChkAFUseInvoiceModeforDebitNotes">
                                <span></span>Use Invoice Mode for Debit Notes</label>
                        </div>
                    </div>
                    <span class="accordion">Budget and Scenario Management</span>
                    <div class="panel">
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFMaintainBudgetandControl" runat="server" clientidmode="Static" />
                            <label for="ChkAFMaintainBudgetandControl">
                                <span></span>Maintain Budget and Control</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFUseReversingJournalsandOptionalVouchers" runat="server"
                                clientidmode="Static" />
                            <label for="ChkAFUseReversingJournalsandOptionalVouchers">
                                <span></span>Use Reversing Journals and Optional Vouchers</label>
                        </div>
                    </div>
                    <span class="accordion">Banking Features</span>
                    <div class="panel">
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFEnableChequePrinting" runat="server" clientidmode="Static" />
                            <label for="ChkAFEnableChequePrinting">
                                <span></span>Enable Cheque Printing (Use Banking Configuration in Bank Ledger Master
                                for Cheque Printing Configuration)</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFAlterBankingFeatures" runat="server" clientidmode="Static" />
                            <label for="ChkAFAlterBankingFeatures">
                                <span></span>Alter Banking Features</label>
                        </div>
                    </div>
                    <span class="accordion">Other Features</span>
                    <div class="panel">
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFAllowZeroValuesEntries" runat="server" clientidmode="Static" />
                            <label for="ChkAFAllowZeroValuesEntries">
                                <span></span>Allow Zero Values Entries</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFMaintainMultipleMailingDetails" runat="server" clientidmode="Static" />
                            <label for="ChkAFMaintainMultipleMailingDetails">
                                <span></span>Maintain Multiple Mailing Details for Company and Ledgers</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFSetAlterCompanyMailingDetails" runat="server" clientidmode="Static" />
                            <label for="ChkAFSetAlterCompanyMailingDetails">
                                <span></span>Set / Alter Company Mailing Details</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkAFEnableCompanyLogo" runat="server" clientidmode="Static" />
                            <label for="ChkAFEnableCompanyLogo">
                                <span></span>Enable Company Logo</label>
                        </div>
                    </div>
                </div>
            </div>
            <div id="resultsel5" class="box company_details">
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkEnableExciseDetails" runat="server" clientidmode="Static" />
                    <label for="ChkEnableExciseDetails">
                        <span></span>Enable Excise Details
                    </label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkSetAlterExciseDetials" runat="server" clientidmode="Static" />
                    <label for="ChkSetAlterExciseDetials">
                        <span></span>Set/Alter Excise Detials(Note: Enable Maintain Multiple Godowns for
                        Multiple Excise Units)</label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkFollowExciseRules" runat="server" clientidmode="Static" />
                    <label for="ChkFollowExciseRules">
                        <span></span>Follow Excise Rules for Invoicing</label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkEnableValueAddTax" runat="server" clientidmode="Static" />
                    <label for="ChkEnableValueAddTax">
                        <span></span>Enable Value Added Tax (VAT)</label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkSetAlterVATDetails" runat="server" clientidmode="Static" />
                    <label for="ChkSetAlterVATDetails">
                        <span></span>Set / Alter VAT Details</label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkEnableServiceTax" runat="server" clientidmode="Static" />
                    <label for="ChkEnableServiceTax">
                        <span></span>Enable Service Tax</label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkSetAlterServiceTaxDetails" runat="server" clientidmode="Static" />
                    <label for="ChkSetAlterServiceTaxDetails">
                        <span></span>Set / Alter Service Tax Details</label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkEnableLocalBodyTax" runat="server" clientidmode="Static" />
                    <label for="ChkEnableLocalBodyTax">
                        <span></span>Enable Local Body Tax(LBT)</label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkSetAlterLBTDetails" runat="server" clientidmode="Static" />
                    <label for="ChkSetAlterLBTDetails">
                        <span></span>Set / Alter LBT Details</label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkEnableTaxDeductedAtSource" runat="server" clientidmode="Static" />
                    <label for="ChkEnableTaxDeductedAtSource">
                        <span></span>Enable Tax Deducted At Source(TDS)</label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkSetAlterTDSDetails" runat="server" clientidmode="Static" />
                    <label for="ChkSetAlterTDSDetails">
                        <span></span>Set / Alter TDS Details</label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkEnableTaxCollectedAtSource" runat="server" clientidmode="Static" />
                    <label for="ChkEnableTaxCollectedAtSource">
                        <span></span>Enable Tax Collected At Source(TCS)</label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkSetAlterTCSDetails" runat="server" clientidmode="Static" />
                    <label for="ChkSetAlterTCSDetails">
                        <span></span>Set / Alter TCS Details</label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkEnableFringeBenfitedTax" runat="server" clientidmode="Static" />
                    <label for="ChkEnableFringeBenfitedTax">
                        <span></span>Enable Fringe Benfited Tax(FBT)</label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkSetAlterFBTDetails" runat="server" clientidmode="Static" />
                    <label for="ChkSetAlterFBTDetails">
                        <span></span>Set / Alter FBT Details</label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkEnableMCAReports" runat="server" clientidmode="Static" />
                    <label for="ChkEnableMCAReports">
                        <span></span>Enable MCA Reports</label>
                </div>
            </div>
            <div id="resultsel6" class="box company_details">
                <div class="expand_collapse_box">
                    <span class="accordion">General</span>
                    <div class="panel">
                        <div class="company_list">
                            <input type="checkbox" id="ChkIFIntegrateAccountsandInventory" runat="server" clientidmode="Static" />
                            <label for="ChkIFIntegrateAccountsandInventory">
                                <span></span>Integrate Accounts and Inventory
                            </label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkIFAllowZeroValuesEntrie" runat="server" clientidmode="Static" />
                            <label for="ChkIFAllowZeroValuesEntrie">
                                <span></span>Allow Zero Values Entries
                            </label>
                        </div>
                    </div>
                    <span class="accordion">Storage and Classification</span>
                    <div class="panel">
                        <div class="company_list">
                            <input type="checkbox" id="ChkIFMaintainMultipleGodowns" runat="server" clientidmode="Static" />
                            <label for="ChkIFMaintainMultipleGodowns">
                                <span></span>Maintain Multiple Godowns / Excise Units
                            </label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkIFMaintainStockCategories" runat="server" clientidmode="Static" />
                            <label for="ChkIFMaintainStockCategories">
                                <span></span>Maintain Stock Categories</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkIFMaintainBatchwiseDetails" runat="server" clientidmode="Static" />
                            <label for="ChkIFMaintainBatchwiseDetails">
                                <span></span>Maintain Batch-wise Details</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkIFMaintainBatchwiseDetailsSetExpiryDate" runat="server"
                                clientidmode="Static" />
                            <label for="ChkIFMaintainBatchwiseDetailsSetExpiryDate">
                                <span></span>Maintain Batch-wise Details(Set Expiry Date for Batches)</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkIFUseDifferentActualBilledQty" runat="server" clientidmode="Static" />
                            <label for="ChkIFUseDifferentActualBilledQty">
                                <span></span>Use Different Actual & Billed Qty</label>
                        </div>
                    </div>
                    <span class="accordion">Order Processing</span>
                    <div class="panel">
                        <div class="company_list">
                            <input type="checkbox" id="ChkIFAllowPurchaseOrderProcessing" runat="server" clientidmode="Static" />
                            <label for="ChkIFAllowPurchaseOrderProcessing">
                                <span></span>Allow Purchase Order Processing</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkIFAllowSaleOrderProcessing" runat="server" clientidmode="Static" />
                            <label for="ChkIFAllowSaleOrderProcessing">
                                <span></span>Allow Sale Order Processing</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkIFAllowJobOrderProcessing" runat="server" clientidmode="Static" />
                            <label for="ChkIFAllowJobOrderProcessing">
                                <span></span>Allow Job Order Processing(Note: Enabling Maintain Multiple Godowns
                                and use of Material In/ Out)</label>
                        </div>
                    </div>
                    <span class="accordion">Invoicing</span>
                    <div class="panel">
                        <div class="company_list">
                            <input type="checkbox" id="ChkIFAllowInvoicing" runat="server" clientidmode="Static" />
                            <label for="ChkIFAllowInvoicing">
                                <span></span>Allow Invoicing
                            </label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkIFEnterPurchasesinInvoiceFormat" runat="server" clientidmode="Static" />
                            <label for="ChkIFEnterPurchasesinInvoiceFormat">
                                <span></span>Enter Purchases in Invoice Format</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkIFUseDebitCreditNotes" runat="server" clientidmode="Static" />
                            <label for="ChkIFUseDebitCreditNotes">
                                <span></span>Use Debit / Credit Notes
                            </label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkIFUseInvoiceModeforCreditNotes" runat="server" clientidmode="Static" />
                            <label for="ChkIFUseInvoiceModeforCreditNotes">
                                <span></span>Use Invoice Mode for Credit Notes</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkIFUseInvoiceModeforDebitNotes" runat="server" clientidmode="Static" />
                            <label for="ChkIFUseInvoiceModeforDebitNotes">
                                <span></span>Use Invoice Mode for Debit Notes</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkIFSeperateDiscountColumnonInvoices" runat="server"
                                clientidmode="Static" />
                            <label for="ChkIFSeperateDiscountColumnonInvoices">
                                <span></span>Seperate Discount Column on Invoices</label>
                        </div>
                    </div>
                    <span class="accordion">Purchase Management</span>
                    <div class="panel">
                        <div class="company_list">
                            <input type="checkbox" id="ChkTrackadditionalcostsofpurchase" runat="server" clientidmode="Static" />
                            <label for="ChkTrackadditionalcostsofpurchase">
                                <span></span>Track Additional Costs of Purchase</label>
                        </div>
                    </div>
                    <span class="accordion">Sales Management</span>
                    <div class="panel">
                        <div class="company_list">
                            <input type="checkbox" id="ChkUseMultiplePrizeLevels" runat="server" clientidmode="Static" />
                            <label for="ChkUseMultiplePrizeLevels">
                                <span></span>Use Multiple Prize Levels</label>
                        </div>
                    </div>
                    <span class="accordion">Other Features</span>
                    <div class="panel">
                        <div class="company_list">
                            <input type="checkbox" id="ChkUseTrackingNumbers" runat="server" clientidmode="Static" />
                            <label for="ChkUseTrackingNumbers">
                                <span></span>Use Tracking Numbers (Delivery / Receipts Notes)</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkUseRejectionInwardOutwardNotes" runat="server" clientidmode="Static" />
                            <label for="ChkUseRejectionInwardOutwardNotes">
                                <span></span>Use Rejection Inward / Outward Notes</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkUseMaterialInOut" runat="server" clientidmode="Static" />
                            <label for="ChkUseMaterialInOut">
                                <span></span>Use Material In/ Out</label>
                        </div>
                        <div class="company_list">
                            <input type="checkbox" id="ChkUseCostTrackingforStockItems" runat="server" clientidmode="Static" />
                            <label for="ChkUseCostTrackingforStockItems">
                                <span></span>Use Cost Tracking for Stock Items</label>
                        </div>
                    </div>
                </div>
            </div>
            <div id="resultsel7" class="box company_details">
                <div class="company_list">
                    <label id="Label9" runat="server" clientidmode="Static">
                        Base Currency Symbol</label>
                    <input type="text" class="form-control" runat="server" id="TxtBaseCurrencySymbol"
                        placeholder="Base Currency Symbol" clientidmode="Static" tabindex="17" maxlength="3"
                        onkeypress="return IsAlphaSymbol(event);" />
                </div>
                <div class="company_list">
                    <label id="Label10" runat="server" clientidmode="Static">
                        Formal Name</label>
                    <input type="text" class="form-control" runat="server" id="TxtFormalName" placeholder="Formal Name"
                        clientidmode="Static" tabindex="18" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="company_list">
                    <label id="Label15" runat="server" clientidmode="Static">
                        Number of Decimal Place</label>
                    <input type="text" class="form-control" runat="server" id="TxtNumberofDecimalPlace"
                        placeholder="Number of Decimal Place" clientidmode="Static" tabindex="19" maxlength="2"
                        onkeypress="return IsNumeric(event);" />
                </div>
                <div class="company_list">
                    <label id="Label17" runat="server" clientidmode="Static">
                        Symbol for Decimal Protion</label>
                    <input type="text" class="form-control" runat="server" id="TxtSymbolforDecimalProtion"
                        placeholder="Symbol for Decimal Protion" clientidmode="Static" tabindex="2" maxlength="1"
                        onkeypress="return IsSymbol(event);" />
                </div>
                <div class="company_list">
                    <label id="Label20" runat="server" clientidmode="Static">
                        Decimal Places for Printing Amount in Words</label>
                    <input type="text" class="form-control" runat="server" id="TxtDecimalPlacesforPrintingAmountinWords"
                        placeholder="Decimal Places for Printing Amount in Words" clientidmode="Static"
                        tabindex="20" maxlength="2" onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="company_list">
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkPutaSpacebetweenAmountandSymbol" runat="server" clientidmode="Static" />
                    <label for="ChkPutaSpacebetweenAmountandSymbol">
                        <span></span>Put a Space between Amount and Symbol</label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkIsSymbolSuffixedtoAmounts" runat="server" clientidmode="Static" />
                    <label for="ChkIsSymbolSuffixedtoAmounts">
                        <span></span>Is Symbol Suffixed to Amounts</label>
                </div>
                <div class="company_list">
                    <label>
                        &nbsp;
                    </label>
                    <input type="checkbox" id="ChkShowAmountinMillions" runat="server" clientidmode="Static" />
                    <label for="ChkShowAmountinMillions">
                        <span></span>Show Amount in Millions</label>
                </div>
            </div>
            
        </div>
        <div class="company_list">
            <asp:Button ID="btnSave" runat="server" ClientIDMode="Static" Text="Save" CssClass="submit_btn"
                OnClientClick="return validate()" />
            <asp:Button ID="BtnClear" runat="server" ClientIDMode="Static" Text="Clear" CssClass="submit_btn"
                OnClientClick="return resetScreen()" />
        </div>
    </div>
    <div style="float: left; width: 100%; min-height: 500px;">
        &nbsp;</div>
    </form>
    <script>
        var acc = document.getElementsByClassName("accordion");
        var i;

        for (i = 0; i < acc.length; i++) {
            acc[i].onclick = function () {
                this.classList.toggle("active");
                this.nextElementSibling.classList.toggle("show");
            }
        }
    </script>
    <!--TABBING-SCRIPTS-->
    <script type="text/javascript">
        var selected = "sel1";
        var disp = "resultsel1";
        function show(a, b) {
            document.getElementById(selected).style.backgroundColor = "#6a6a6a";
            document.getElementById(disp).style.display = "none";

            document.getElementById(a).style.backgroundColor = "#00bcd5";
            document.getElementById(a).style.color = "#ffffff";

            document.getElementById(b).style.display = "block";
            selected = a;
            disp = b;
        }
    </script>
    <!--TABBING-SCRIPTS-->
    <script type="text/javascript" language="javascript">
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        specialKeys.push(9); //Tab
        specialKeys.push(46); //Delete
        specialKeys.push(36); //Home
        specialKeys.push(35); //End
        specialKeys.push(37); //Left
        specialKeys.push(39); //Right
        function IsSymbol(e) {
            var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
            // alert(keyCode);
            var ret = ((keyCode >= 45 && keyCode <= 47) || (keyCode == 58) || (keyCode == 61) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }
    </script>
    <!--Check Name -->
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
            var ret = ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (keyCode == 39) || (keyCode == 32) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }
    </script>
    <script type="text/javascript" language="javascript">
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        specialKeys.push(9); //Tab
        specialKeys.push(46); //Delete
        specialKeys.push(36); //Home
        specialKeys.push(35); //End
        specialKeys.push(37); //Left
        specialKeys.push(39); //Right

        function ValidateAddress(e) {
            var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
            //alert(keyCode);
            var ret = ((keyCode == 32) || (keyCode == 35) || (keyCode == 39) || (keyCode >= 44 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }
    </script>
    <!-- Check the Keypressed is Numeric-->
    <script type="text/javascript" language="javascript">
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        specialKeys.push(9); //Tab
        specialKeys.push(46); //Delete
        specialKeys.push(36); //Home
        specialKeys.push(35); //End
        specialKeys.push(37); //Left
        specialKeys.push(39); //Right
        function IsNumeric(e) {
            var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
            var ret = ((keyCode >= 48 && keyCode <= 57) || (specialKeys.indexOf(keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }
    </script>
    <!-- Check the Keypressed is Alphabetic or Numeric or for Cursor Movement-->
    <script type="text/javascript">
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        specialKeys.push(9); //Tab
        specialKeys.push(46); //Delete
        specialKeys.push(36); //Home
        specialKeys.push(35); //End
        specialKeys.push(37); //Left
        specialKeys.push(39); //Right
        function IsAlphaNumeric(e) {
            var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
            var ret = ((keyCode == 32) || (keyCode == 39) || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }
    </script>
    <script type="text/javascript">
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        specialKeys.push(9); //Tab
        specialKeys.push(46); //Delete
        specialKeys.push(36); //Home
        specialKeys.push(35); //End
        specialKeys.push(37); //Left
        specialKeys.push(39); //Right
        function IsAlphaSymbol(e) {
            var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
            var ret = ((keyCode <= 47) || (keyCode >= 58) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }
    </script>
    <!--Zipcode validation-->
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
    <!--Email validation-->
    <script type="text/javascript">
        function ValidateEmail(inputText) {
            var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
            var inputText;
            inputText = document.getElementById("<%=TxtEmail.ClientID %>");
            if (inputText.value.match(mailformat)) {
                //alert('valid email');
            }
            else {
                alert("You have entered an invalid EmailId!");
                document.getElementById("<%=TxtEmail.ClientID %>").value = "";
                //                var urlString = 'url(../Resource/Images/General/invalid.png)';
                //                alert(urlString);
                document.getElementById("<%=TxtEmail.ClientID %>").style.background - image(urlString);
                document.getElementById("<%=TxtEmail.ClientID %>").value = "";
                document.getElementById("<%=TxtEmail.ClientID %>").value = "";
                document.getElementById("<%=TxtEmail.ClientID %>").focus();
                return false;

            }
        } 
    </script>
    <!-- Reset Screen-->
    <script type="text/javascript" language="javascript">
        function resetScreen() {
            document.getElementById("<%=TxtCompanyName.ClientID%>").value = "";
            document.getElementById("<%=TxtMailingAddress.ClientID%>").value = "";
            document.getElementById("<%=TxtStatutoryComplianceFor.ClientID%>").value = "";
            document.getElementById("<%=TxtState.ClientID%>").value = "";
            document.getElementById("<%=TxtPIN.ClientID%>").value = "";
            document.getElementById("<%=TxtTelephoneNo.ClientID%>").value = "";
            document.getElementById("<%=TxtMobile.ClientID%>").value = "";
            document.getElementById("<%=TxtEmail.ClientID%>").value = "";
            document.getElementById("<%=TxtClientShortCode.ClientID%>").value = "";
            document.getElementById("<%=TxtCurrencySymbol.ClientID%>").value = "";
            document.getElementById("<%=ddlMaintain.ClientID%>").value = "0";
            document.getElementById("<%=TxtFinancialYearFrom.ClientID%>").value = "";
            document.getElementById("<%=TxtBooksBeginFrom.ClientID%>").value = "";
            document.getElementById("<%=TxtECCNO.ClientID%>").value = "";
            document.getElementById("<%=TxtCSTTINNO.ClientID%>").value = "";
            document.getElementById("<%=TxtVATTINNO.ClientID%>").value = "";
            document.getElementById("<%=TxtVATTINNO.ClientID%>").value = "";


            document.getElementById("<%=TxtECCNO.ClientID%>").value = "";
            document.getElementById("<%=TxtCSTTINNO.ClientID%>").value = "";
        document.getElementById("<%=TxtVATTINNO.ClientID%>").value = "";
            document.getElementById("<%=TxtVATTINNO.ClientID%>").value = "";

            $('input[type=checkbox]').attr('checked', false);
            return true;
        }
    </script>
    <!-- Validate Screen before going to Server-->
    <script type="text/javascript" language="javascript">
        function validateImageUpload() {
            var avatar = document.getElementById("<%=flupload.ClientID%>");
            if (avatar.value == "") {
                alert("Please browse the image first!");
                return false
            }
            return true
        }
        function validateSupplier() {
            if (document.getElementById("<%=NameofParty.ClientID %>").value.trim() == "") {
                alert("Please Provide Name of Party");
                document.getElementById("<%=NameofParty.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=PartyAddress.ClientID %>").value.trim() == "") {
                alert("Please Provide Party Address");
                document.getElementById("<%=PartyAddress.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=City.ClientID %>").value.trim() == "") {
                alert("Please Provide City");
                document.getElementById("<%=City.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ExciseServiceTaxNo.ClientID %>").value.trim() == "") {
                alert("Please Provide Excise/Service Tax No");
                document.getElementById("<%=ExciseServiceTaxNo.ClientID %>").focus();
                return false;
            }
        }
        function validate() {
            if (document.getElementById("<%=TxtCompanyName.ClientID %>").value.trim() == "") {
                alert("Please Provide Company");
                document.getElementById("<%=TxtCompanyName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TxtMailingAddress.ClientID %>").value.trim() == "") {
                alert("Please give Company's Mailing Address");
                document.getElementById("<%=TxtMailingAddress.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TxtStatutoryComplianceFor.ClientID %>").value.trim() == "") {
                alert("Please give Compliance for");
                document.getElementById("<%=TxtStatutoryComplianceFor.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TxtState.ClientID %>").value.trim() == "") {
                alert("Please give State");
                document.getElementById("<%=TxtState.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=TxtPIN.ClientID %>").value.trim() == "") {
                alert("Please Provide PIN Code");
                document.getElementById("<%=TxtPIN.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TxtTelephoneNo.ClientID %>").value.trim() == "") {
                alert("Please Select TelephoneNo");
                document.getElementById("<%=TxtTelephoneNo.ClientID %>").focus();
                return false;
            }
            else {
                var teliphone = document.getElementById("<%=TxtTelephoneNo.ClientID %>").value;
                var patterntel = /^\d{10,13}$/;
                if (!patterntel.test(teliphone)) {
                    alert("It is not valid Telephone number.input 10-13 digits number!");
                    document.getElementById("<%=TxtTelephoneNo.ClientID %>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TxtMobile.ClientID %>").value.trim() == "") {
                alert("Please Select Mobile");
                document.getElementById("<%=TxtMobile.ClientID %>").focus();
                return false;
            }
            else {
                var mobile = document.getElementById("<%=TxtMobile.ClientID %>").value;
                var pattern = /^\d{10}$/;
                if (!pattern.test(mobile)) {
                    alert("It is not valid mobile number.input 10 digits number!");
                    document.getElementById("<%=TxtMobile.ClientID %>").focus();
                    return false;
                }
            }
            var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
            var inputText;
            inputText = document.getElementById("<%=TxtEmail.ClientID %>");
            if (inputText.value.match(mailformat)) {
                //alert('valid email');
            }
            else {
                alert("Please enter EmailId!");
                document.getElementById("<%=TxtEmail.ClientID %>").value = "";

                return false;
            }

            if (document.getElementById("<%=TxtCurrencySymbol.ClientID %>").value.trim() == "") {
                alert("Please Provide Currency Symbol");
                document.getElementById("<%=TxtCurrencySymbol.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TxtClientShortCode.ClientID %>").value.trim() == "") {
                alert("Please Provide Client Short Code");
                document.getElementById("<%=TxtClientShortCode.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlMaintain.ClientID %>").value.trim() == "") {
                alert("Please Provide Maintain Account for...");
                document.getElementById("<%=ddlMaintain.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TxtFinancialYearFrom.ClientID %>").value.trim() == "") {
                alert("Please Provide A/c Financial Year From Date");
                document.getElementById("<%=TxtFinancialYearFrom.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TxtBooksBeginFrom.ClientID %>").value.trim() == "") {
                alert("Please Provide A/c Books Begin From Date");
                document.getElementById("<%=TxtBooksBeginFrom.ClientID %>").focus();
                return false;
            }
            return true
        }
    </script>
</asp:Content>
