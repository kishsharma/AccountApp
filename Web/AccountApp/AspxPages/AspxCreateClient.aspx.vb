Imports System.Globalization
Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D

Partial Class AspxPages_AspxCreateClient
    Inherits System.Web.UI.Page
    Public StrDateFormat As String
    Public StrFinancialYearFrom As String = String.Empty
    Public StrTxtBooksBeginFrom As String = String.Empty
    Dim clsObj As New clsData
    Dim temp As New DataTable()
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("ScreenName")) Then
            Response.Redirect("~/Default.aspx", False)
            Exit Sub
        End If
        TxtEmail.Attributes.Add("type", "email")
        StrDateFormat = "d-M-Y"
        Dim StrDate As String = "01-Apr-" & Now.Year
        If Not IsPostBack Then
            Dim clsfn As New ClsCommonFunction
            StrFinancialYearFrom = (clsfn.fnChangeDateFormat(StrDate, "1", "/", "2", 0))
            StrTxtBooksBeginFrom = (clsfn.fnChangeDateFormat(StrDate, "1", "/", "2", 0))
            clsfn = Nothing
            TxtFinancialYearFrom.Text = StrFinancialYearFrom
            TxtBooksBeginFrom.Text = StrTxtBooksBeginFrom
            If IsNothing(Request.QueryString("idUpdate")) Then
                HdnIdentity.Value = "0"
                DirectCast(Page.Master.FindControl("lblSiteMap"), Label).Text = "Screen: Add New - " & Session("ScreenName").ToString
            Else
                DirectCast(Page.Master.FindControl("lblSiteMap"), Label).Text = "Screen: Edit - " & Session("ScreenName").ToString
                HdnIdentity.Value = Request.QueryString("idUpdate")
                fnFillScreen()
            End If
            TxtCompanyName.Focus()
        End If


    End Sub
    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click

        'Dim strValidateQry As String = String.Empty
        'If HdnIdentity.Value = 0 Then
        '    strValidateQry = " select idClient from tbl_client where txteccno = '" & TxtECCNO.Value.Trim & "'"
        '    strValidateQry = strValidateQry & " select idClient from tbl_client where txtvattinno = '" & TxtVATTINNO.Value.Trim & "' "
        '    strValidateQry = strValidateQry & " select idClient from tbl_client where txtcsttinno = '" & TxtCSTTINNO.Value.Trim & "'"
        'Else
        '    strValidateQry = " select idClient from tbl_client where txteccno = '" & TxtECCNO.Value.Trim & "' and idClient <> " & HdnIdentity.Value
        '    strValidateQry = strValidateQry & " select idClient from tbl_client where txtvattinno = '" & TxtVATTINNO.Value.Trim & "'  and idClient <> " & HdnIdentity.Value
        '    strValidateQry = strValidateQry & " select idClient from tbl_client where txtcsttinno = '" & TxtCSTTINNO.Value.Trim & "' and idClient <> " & HdnIdentity.Value
        'End If
        'Dim StrExp As String = String.Empty
        'Dim clsObjValidate As New clsData
        'Dim DatsValidate As New DataSet
        'DatsValidate = clsObjValidate.fnGetDataSet(strValidateQry, StrExp)
        'If Not IsNothing(DatsValidate) Then
        '    If DatsValidate.Tables.Count > 0 Then
        '        If DatsValidate.Tables(0).Rows.Count > 0 Then
        '            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('ECC No. Exists');</script>")
        '            Exit Sub
        '        End If
        '        If DatsValidate.Tables(1).Rows.Count > 0 Then
        '            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('VAT/Tin No. Exists');</script>")
        '            Exit Sub
        '        End If
        '        If DatsValidate.Tables(2).Rows.Count > 0 Then
        '            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('CST No. Exists');</script>")
        '            Exit Sub
        '        End If
        '    End If
        'End If

        Dim strValidateQry As String = String.Empty
        If HdnIdentity.Value = 0 Then
            strValidateQry = " select idClient from tbl_client where TxtGSTNO ='" & TxtGSTNO.Value.Trim & "'"
            strValidateQry = strValidateQry & " select idClient from tbl_client where TXTPano = '" & TxtPANo.Value.Trim & "' "
        Else
            strValidateQry = " select idClient from tbl_client where TxtGSTNO = '" & TxtGSTNO.Value.Trim & "' and idClient <> " & HdnIdentity.Value & ""
            strValidateQry = strValidateQry & " select idClient from tbl_client where TXTPano = '" & TxtPANo.Value.Trim & "'  and idClient <> " & HdnIdentity.Value
        End If
        Dim StrExp As String = String.Empty
        Dim clsObjValidate As New clsData
        Dim DatsValidate As New DataSet
        DatsValidate = clsObjValidate.fnGetDataSet(strValidateQry, StrExp)
        If Not IsNothing(DatsValidate) Then
            If DatsValidate.Tables.Count > 0 Then
                If DatsValidate.Tables(0).Rows.Count > 0 Then
                    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('GST NO. Exists');</script>")
                    Exit Sub
                End If
           
                If DatsValidate.Tables(1).Rows.Count > 0 Then
                    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('PANo. Exists');</script>")
                    Exit Sub
                End If
            End If
        End If



        Dim usDtfi As DateTimeFormatInfo = New CultureInfo("en-US", False).DateTimeFormat
        Dim StrInsQry As New StringBuilder
        If ViewState("fileName") = "" Then
            ViewState("fileName") = "Upload/ClientLogo/Super.png"
        End If
        Dim StrLngId As String = String.Empty
        Dim FinancialYearFrom As DateTime = Convert.ToDateTime(TxtFinancialYearFrom.Text, usDtfi)
        Dim BooksBeginFrom As DateTime = Convert.ToDateTime(TxtBooksBeginFrom.Text, usDtfi)
        Dim Names As String() = {"idClient", "TxtCompanyName", "TxtMailingAddress", "TxtStatutoryComplianceFor", "TxtState",
                                 "TxtPIN", "TxtTelephoneNo", "TxtMobile", "TxtEmail", "TxtCurrencySymbol",
                                 "TxtMaintain", "TxtFinancialYearFrom", "TxtBooksBeginFrom",
                                 "ChkAutoBackup", "ChkSecurityControl", "ChkAFIntegrateAccountsandInventory", "ChkAFIncomeandExpenceStatement",
                                 "ChkAFAllowMultiCurrency", "ChkAFMaintainBillwiseDetails", "ChkAFActivateInterestCalculation", "ChkAFMaintainPayroll",
                                 "ChkAFMaintainCostCentres", "ChkAFUseCostCentreforJobCosting", "ChkAFMorethanONEPayrollCostCategory",
                                 "ChkAFUsePreDefinedCostCenterAllocationDuringEntry", "ChkAFShowOpeningBalanceforRevenueItemInReport", "ChkAFAllowInvoicing",
                                 "ChkAFEnterPurchasesinInvoiceFormat", "ChkAFUseDebitCreditNotes", "ChkAFUseInvoiceModeforCreditNotes",
                                 "ChkAFUseInvoiceModeforDebitNotes", "ChkAFMaintainBudgetandControl", "ChkAFUseReversingJournalsandOptionalVouchers",
                                 "ChkAFEnableChequePrinting", "ChkAFAlterBankingFeatures", "ChkAFAllowZeroValuesEntries",
                                 "ChkAFMaintainMultipleMailingDetails", "ChkAFSetAlterCompanyMailingDetails", "ChkAFEnableCompanyLogo",
                                 "ChkEnableExciseDetails", "ChkSetAlterExciseDetials", "ChkFollowExciseRules", "ChkEnableValueAddTax",
                                 "ChkSetAlterVATDetails", "ChkEnableServiceTax", "ChkSetAlterServiceTaxDetails", "ChkEnableLocalBodyTax",
                                 "ChkSetAlterLBTDetails", "ChkEnableTaxDeductedAtSource", "ChkSetAlterTDSDetails", "ChkEnableTaxCollectedAtSource",
                                 "ChkSetAlterTCSDetails", "ChkEnableFringeBenfitedTax", "ChkSetAlterFBTDetails", "ChkEnableMCAReports",
                                 "ChkIFIntegrateAccountsandInventory", "ChkIFAllowZeroValuesEntrie", "ChkIFMaintainMultipleGodowns",
                                 "ChkIFMaintainStockCategories", "ChkIFMaintainBatchwiseDetails", "ChkIFMaintainBatchwiseDetailsSetExpiryDate",
                                 "ChkIFUseDifferentActualBilledQty", "ChkIFAllowPurchaseOrderProcessing", "ChkIFAllowSaleOrderProcessing",
                                 "ChkIFAllowJobOrderProcessing", "ChkIFAllowInvoicing", "ChkIFEnterPurchasesinInvoiceFormat", "ChkIFUseDebitCreditNotes",
                                 "ChkIFUseInvoiceModeforCreditNotes", "ChkIFUseInvoiceModeforDebitNotes", "ChkIFSeperateDiscountColumnonInvoices",
                                 "ChkTrackadditionalcostsofpurchase", "ChkUseMultiplePrizeLevels", "ChkUseTrackingNumbers",
                                 "ChkUseRejectionInwardOutwardNotes", "ChkUseMaterialInOut", "ChkUseCostTrackingforStockItems",
                                 "TxtBaseCurrencySymbol", "TxtFormalName", "TxtNumberofDecimalPlace",
                                 "TxtSymbolforDecimalProtion", "ChkPutaSpacebetweenAmountandSymbol", "TxtDecimalPlacesforPrintingAmountinWords", "ChkIsSymbolSuffixedtoAmounts",
                                 "ChkShowAmountinMillions", "ClientLogo", "ClientShortCode",
                                 "TxtECCNO", "TxtCSTTINNO", "TxtVATTINNO", "TxtGSTNO", "TxtPANo", "idCompany", "TxtBankName", "TxtBankBranchName", "TxtBankAccountNumber", "TxtBankIFSCCode"}
        Dim Values As String() = {HdnIdentity.Value, TxtCompanyName.Value.Trim, TxtMailingAddress.Value.Trim, TxtStatutoryComplianceFor.Value.Trim, TxtState.Value.Trim,
                                  TxtPIN.Value.Trim, TxtTelephoneNo.Value.Trim, TxtMobile.Value.Trim, TxtEmail.Value.Trim, TxtCurrencySymbol.Value.Trim,
                                  ddlMaintain.SelectedValue, FinancialYearFrom, BooksBeginFrom,
                                  ChkAutoBackup.Checked, ChkSecurityControl.Checked, ChkAFIntegrateAccountsandInventory.Checked, ChkAFIncomeandExpenceStatement.Checked,
                                  ChkAFAllowMultiCurrency.Checked, ChkAFMaintainBillwiseDetails.Checked, ChkAFActivateInterestCalculation.Checked, ChkAFMaintainPayroll.Checked,
                                  ChkAFMaintainCostCentres.Checked, ChkAFUseCostCentreforJobCosting.Checked, ChkAFMorethanONEPayrollCostCategory.Checked,
                                  ChkAFUsePreDefinedCostCenterAllocationDuringEntry.Checked, ChkAFShowOpeningBalanceforRevenueItemInReport.Checked, ChkAFAllowInvoicing.Checked,
                                  ChkAFEnterPurchasesinInvoiceFormat.Checked, ChkAFUseDebitCreditNotes.Checked, ChkAFUseInvoiceModeforCreditNotes.Checked,
                                  ChkAFUseInvoiceModeforDebitNotes.Checked, ChkAFMaintainBudgetandControl.Checked, ChkAFUseReversingJournalsandOptionalVouchers.Checked,
                                  ChkAFEnableChequePrinting.Checked, ChkAFAlterBankingFeatures.Checked, ChkAFAllowZeroValuesEntries.Checked,
                                  ChkAFMaintainMultipleMailingDetails.Checked, ChkAFSetAlterCompanyMailingDetails.Checked, ChkAFEnableCompanyLogo.Checked,
                                  ChkEnableExciseDetails.Checked, ChkSetAlterExciseDetials.Checked, ChkFollowExciseRules.Checked, ChkEnableValueAddTax.Checked,
                                  ChkSetAlterVATDetails.Checked, ChkEnableServiceTax.Checked, ChkSetAlterServiceTaxDetails.Checked, ChkEnableLocalBodyTax.Checked,
                                  ChkSetAlterLBTDetails.Checked, ChkEnableTaxDeductedAtSource.Checked, ChkSetAlterTDSDetails.Checked, ChkEnableTaxCollectedAtSource.Checked,
                                  ChkSetAlterTCSDetails.Checked, ChkEnableFringeBenfitedTax.Checked, ChkSetAlterFBTDetails.Checked, ChkEnableMCAReports.Checked,
                                  ChkIFIntegrateAccountsandInventory.Checked, ChkIFAllowZeroValuesEntrie.Checked, ChkIFMaintainMultipleGodowns.Checked,
                                  ChkIFMaintainStockCategories.Checked, ChkIFMaintainBatchwiseDetails.Checked, ChkIFMaintainBatchwiseDetailsSetExpiryDate.Checked,
                                  ChkIFUseDifferentActualBilledQty.Checked, ChkIFAllowPurchaseOrderProcessing.Checked, ChkIFAllowSaleOrderProcessing.Checked,
                                  ChkIFAllowJobOrderProcessing.Checked, ChkIFAllowInvoicing.Checked, ChkIFEnterPurchasesinInvoiceFormat.Checked, ChkIFUseDebitCreditNotes.Checked,
                                  ChkIFUseInvoiceModeforCreditNotes.Checked, ChkIFUseInvoiceModeforDebitNotes.Checked, ChkIFSeperateDiscountColumnonInvoices.Checked,
                                  ChkTrackadditionalcostsofpurchase.Checked, ChkUseMultiplePrizeLevels.Checked, ChkUseTrackingNumbers.Checked,
                                  ChkUseRejectionInwardOutwardNotes.Checked, ChkUseMaterialInOut.Checked, ChkUseCostTrackingforStockItems.Checked,
                                  TxtBaseCurrencySymbol.Value.Trim, TxtFormalName.Value.Trim, TxtNumberofDecimalPlace.Value.Trim, TxtSymbolforDecimalProtion.Value.Trim,
                                  ChkPutaSpacebetweenAmountandSymbol.Checked, TxtDecimalPlacesforPrintingAmountinWords.Value.Trim, ChkIsSymbolSuffixedtoAmounts.Checked,
                                  ChkShowAmountinMillions.Checked, ViewState("fileName"), TxtClientShortCode.Value.Trim,
                                  TxtECCNO.Value.Trim, TxtCSTTINNO.Value.Trim, TxtVATTINNO.Value.Trim, TxtGSTNO.Value.Trim, TxtPANo.Value.Trim, HdnIdCompany.Value, TxtBankName.Value.Trim, TxtBankBranchName.Value.Trim, TxtBankAccountNumber.Value.Trim, TxtBankIFSCCode.Value.Trim}
        StrLngId = DirectCast(clsObj.fnInsertUpdate("P_ClientRegistrationInfo_24May2018", Names, Values), String)
        'If StrLngId.Length > 0 Then
        ''HdnIdentity.Value = String.Empty.ToString()
        If IsNumeric(StrLngId) = True Then
            If CInt(StrLngId) > 0 Then
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Client Created'); location.href='" & Session("BackPage").ToString() & "';</script>")
                'Response.Redirect("AspxSearchBlock.aspx?idMenu=" & Session("IdMenu"), False)
            Else
                'ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('" & StrLngId & "'); location.href='" & Session("BackPage").ToString() & "';</script>")
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Client Created'); location.href='" & Session("BackPage").ToString() & "';</script>")
            End If
        Else
            'Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "RegisterClientScriptBlock", "document.write ('" & StrLngId & "');", True)
            StrLngId = "alert('" & StrLngId & "')"
            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Client Updated'); location.href='" & Session("BackPage").ToString() & "';</script>")
            clearfields()
        End If
        'If Not HdnIdentity.Value = "" Then
        '    StrLngId = HdnIdentity.Value
        'End If
        'Dim Query As String() = New String(-1) {}
        'Dim index As Integer = 0, countInv As Integer = Gvdetails.Rows.Count
        'Query = New String(1 + (countInv - 1)) {}
        'Query(0) = "Delete from tbl_SupplierDetails where idClient='" & StrLngId & "'"
        'index = 1
        'For i As Integer = 0 To countInv - 1
        '    Query(index) = "insert into tbl_SupplierDetails values('" & Gvdetails.Rows(i).Cells(1).Text.ToString() & "'," & StrLngId & ",'" & Gvdetails.Rows(i).Cells(3).Text.ToString() & "','" & Gvdetails.Rows(i).Cells(2).Text.Trim() & "','" & Gvdetails.Rows(i).Cells(4).Text.ToString().Trim() & "','" & Gvdetails.Rows(i).Cells(5).Text.ToString().Trim() & "')"
        '    index += 1
        'Next
        'clsObj.funTransaction(Query)
        'clsObj = Nothing
        'Session.Remove("Gvd")
        'HdnIdentity.Value = String.Empty.ToString()
        'Else
        '    Session.Remove("Gvd")
        '    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Duplicate TIN no Exists'); location.href='" & Session("BackPage").ToString() & "';</script>")
        'End If
    End Sub
    Private Sub fnFillScreen()
        Dim dtstSearchDtls As New DataSet
        Dim dtsGrd As New DataSet
        Dim clsfn As New ClsCommonFunction
        dtstSearchDtls = clsObj.fnDataSet("Select idClient, TxtCompanyName, TxtMailingAddress, TxtStatutoryComplianceFor, TxtState, TxtPIN, TxtTelephoneNo, TxtMobile, TxtEmail, TxtCurrencySymbol, TxtMaintain, TxtFinancialYearFrom, TxtBooksBeginFrom, ChkAutoBackup, ChkSecurityControl, ChkAFIntegrateAccountsandInventory, ChkAFIncomeandExpenceStatement, ChkAFAllowMultiCurrency, ChkAFMaintainBillwiseDetails, ChkAFActivateInterestCalculation, ChkAFMaintainPayroll, ChkAFMaintainCostCentres, ChkAFUseCostCentreforJobCosting, ChkAFMorethanONEPayrollCostCategory, ChkAFUsePreDefinedCostCenterAllocationDuringEntry, ChkAFShowOpeningBalanceforRevenueItemInReport, ChkAFAllowInvoicing, ChkAFEnterPurchasesinInvoiceFormat, ChkAFUseDebitCreditNotes, ChkAFUseInvoiceModeforCreditNotes, ChkAFUseInvoiceModeforDebitNotes, ChkAFMaintainBudgetandControl, ChkAFUseReversingJournalsandOptionalVouchers, ChkAFEnableChequePrinting, ChkAFAlterBankingFeatures, ChkAFAllowZeroValuesEntries, ChkAFMaintainMultipleMailingDetails, ChkAFSetAlterCompanyMailingDetails, ChkAFEnableCompanyLogo, ChkEnableExciseDetails, ChkSetAlterExciseDetials, ChkFollowExciseRules, ChkEnableValueAddTax, ChkSetAlterVATDetails, ChkEnableServiceTax, ChkSetAlterServiceTaxDetails, ChkEnableLocalBodyTax, ChkSetAlterLBTDetails, ChkEnableTaxDeductedAtSource, ChkSetAlterTDSDetails, ChkEnableTaxCollectedAtSource, ChkSetAlterTCSDetails, ChkEnableFringeBenfitedTax, ChkSetAlterFBTDetails, ChkEnableMCAReports, ChkIFIntegrateAccountsandInventory, ChkIFAllowZeroValuesEntrie, ChkIFMaintainMultipleGodowns, ChkIFMaintainStockCategories, ChkIFMaintainBatchwiseDetails, ChkIFMaintainBatchwiseDetailsSetExpiryDate, ChkIFUseDifferentActualBilledQty, ChkIFAllowPurchaseOrderProcessing, ChkIFAllowSaleOrderProcessing, ChkIFAllowJobOrderProcessing, ChkIFAllowInvoicing, ChkIFEnterPurchasesinInvoiceFormat, ChkIFUseDebitCreditNotes, ChkIFUseInvoiceModeforCreditNotes, ChkIFUseInvoiceModeforDebitNotes, ChkIFSeperateDiscountColumnonInvoices, ChkTrackadditionalcostsofpurchase, ChkUseMultiplePrizeLevels, ChkUseTrackingNumbers, ChkUseRejectionInwardOutwardNotes, ChkUseMaterialInOut, ChkUseCostTrackingforStockItems, TxtBaseCurrencySymbol, TxtFormalName, TxtNumberofDecimalPlace, TxtSymbolforDecimalProtion, ChkPutaSpacebetweenAmountandSymbol, TxtDecimalPlacesforPrintingAmountinWords, ChkIsSymbolSuffixedtoAmounts, ChkShowAmountinMillions, ClientLogo, ClientShortCode, TxtECCNO, TxtCSTTINNO, TxtVATTINNO, RecordStatus, TxtGSTNO, TxtPANo, IdCompany, TxtBankName, TxtBankBranchName, TxtBankAccountNumber, TxtBankIFSCCode from tbl_client where idClient=" & HdnIdentity.Value, 1, 1)
        If Not IsNothing(dtstSearchDtls) Then
            If dtstSearchDtls.Tables.Count > 0 Then
                If dtstSearchDtls.Tables(0).Rows.Count > 0 Then
                    Dim StrSQLItem As String = "select NameOfParty, PartyAddress, City, ExciseServiceTaxNo, TinNo, SupplierId from tbl_SupplierDetails where idClient = " & HdnIdentity.Value & ""
                    clsObj = New clsData
                    dtsGrd = clsObj.fnDataSet(StrSQLItem.ToString, CommandType.Text)
                    clsObj = Nothing
                    If Not IsNothing(dtsGrd) Then
                        If dtsGrd.Tables.Count > 0 Then
                            If dtsGrd.Tables(0).Rows.Count > 0 Then
                                createDynamicGridInvoice()
                                For Each row As DataRow In dtsGrd.Tables(0).Rows
                                    temp.Rows.Add("", row("NameofParty").ToString(), row("PartyAddress").ToString(), row("City").ToString(), row("ExciseServiceTaxNo").ToString(), row("TinNo").ToString(), row("SupplierID").ToString())
                                Next
                                Gvdetails.DataSource = temp
                                Gvdetails.DataBind()
                                Session("ClientGvd") = temp
                            End If
                        End If
                    End If
                    TxtCompanyName.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtCompanyName").ToString()
                    TxtMailingAddress.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtMailingAddress").ToString()
                    TxtStatutoryComplianceFor.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtStatutoryComplianceFor").ToString()
                    TxtState.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtState").ToString()
                    TxtPIN.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtPIN").ToString()
                    TxtTelephoneNo.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtTelephoneNo").ToString()
                    TxtMobile.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtMobile").ToString()
                    TxtEmail.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtEmail").ToString()
                    TxtCurrencySymbol.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtCurrencySymbol").ToString()
                    ddlMaintain.SelectedValue = dtstSearchDtls.Tables(0).Rows(0)("TxtMaintain").ToString()
                    StrFinancialYearFrom = (clsfn.fnChangeDateFormat(dtstSearchDtls.Tables(0).Rows(0)("TxtFinancialYearFrom").ToString(), "1", "/", "2", 0))
                    StrTxtBooksBeginFrom = (clsfn.fnChangeDateFormat(dtstSearchDtls.Tables(0).Rows(0)("TxtBooksBeginFrom").ToString(), "1", "/", "2", 0))
                    TxtFinancialYearFrom.Text = StrFinancialYearFrom
                    TxtBooksBeginFrom.Text = StrTxtBooksBeginFrom
                    ChkAutoBackup.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAutoBackup").ToString()
                    ChkSecurityControl.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkSecurityControl").ToString()
                    ChkAFIntegrateAccountsandInventory.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFIntegrateAccountsandInventory").ToString()
                    ChkAFIncomeandExpenceStatement.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFIncomeandExpenceStatement").ToString()
                    ChkAFAllowMultiCurrency.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFAllowMultiCurrency").ToString()
                    ChkAFMaintainBillwiseDetails.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFMaintainBillwiseDetails").ToString()
                    ChkAFActivateInterestCalculation.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFActivateInterestCalculation").ToString()
                    ChkAFMaintainPayroll.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFMaintainPayroll").ToString()
                    ChkAFMaintainCostCentres.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFMaintainCostCentres").ToString()
                    ChkAFUseCostCentreforJobCosting.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFUseCostCentreforJobCosting").ToString()
                    ChkAFMorethanONEPayrollCostCategory.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFMorethanONEPayrollCostCategory").ToString()
                    ChkAFUsePreDefinedCostCenterAllocationDuringEntry.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFUsePreDefinedCostCenterAllocationDuringEntry").ToString()
                    ChkAFShowOpeningBalanceforRevenueItemInReport.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFShowOpeningBalanceforRevenueItemInReport").ToString()
                    ChkAFAllowInvoicing.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFAllowInvoicing").ToString()
                    ChkAFEnterPurchasesinInvoiceFormat.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFEnterPurchasesinInvoiceFormat").ToString()
                    ChkAFUseDebitCreditNotes.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFUseDebitCreditNotes").ToString()
                    ChkAFUseInvoiceModeforCreditNotes.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFUseInvoiceModeforCreditNotes").ToString()
                    ChkAFUseInvoiceModeforDebitNotes.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFUseInvoiceModeforDebitNotes").ToString()
                    ChkAFMaintainBudgetandControl.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFMaintainBudgetandControl").ToString()
                    ChkAFUseReversingJournalsandOptionalVouchers.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFUseReversingJournalsandOptionalVouchers").ToString()
                    ChkAFEnableChequePrinting.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFEnableChequePrinting").ToString()
                    ChkAFAlterBankingFeatures.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFAlterBankingFeatures").ToString()
                    ChkAFAllowZeroValuesEntries.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFAllowZeroValuesEntries").ToString()
                    ChkAFMaintainMultipleMailingDetails.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFMaintainMultipleMailingDetails").ToString()
                    ChkAFSetAlterCompanyMailingDetails.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFSetAlterCompanyMailingDetails").ToString()
                    ChkAFEnableCompanyLogo.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkAFEnableCompanyLogo").ToString()
                    ChkEnableExciseDetails.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkEnableExciseDetails").ToString()
                    ChkSetAlterExciseDetials.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkSetAlterExciseDetials").ToString()
                    ChkFollowExciseRules.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkFollowExciseRules").ToString()
                    ChkEnableValueAddTax.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkEnableValueAddTax").ToString()
                    ChkSetAlterVATDetails.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkSetAlterVATDetails").ToString()
                    ChkEnableServiceTax.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkEnableServiceTax").ToString()
                    ChkSetAlterServiceTaxDetails.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkSetAlterServiceTaxDetails").ToString()
                    ChkEnableLocalBodyTax.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkEnableLocalBodyTax").ToString()
                    ChkSetAlterLBTDetails.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkSetAlterLBTDetails").ToString()
                    ChkEnableTaxDeductedAtSource.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkEnableTaxDeductedAtSource").ToString()
                    ChkSetAlterTDSDetails.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkSetAlterTDSDetails").ToString()
                    ChkEnableTaxCollectedAtSource.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkEnableTaxCollectedAtSource").ToString()
                    ChkSetAlterTCSDetails.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkSetAlterTCSDetails").ToString()
                    ChkEnableFringeBenfitedTax.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkEnableFringeBenfitedTax").ToString()
                    ChkSetAlterFBTDetails.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkSetAlterFBTDetails").ToString()
                    ChkEnableMCAReports.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkEnableMCAReports").ToString()
                    ChkIFIntegrateAccountsandInventory.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkIFIntegrateAccountsandInventory").ToString()
                    ChkIFAllowZeroValuesEntrie.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkIFAllowZeroValuesEntrie").ToString()
                    ChkIFMaintainMultipleGodowns.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkIFMaintainMultipleGodowns").ToString()
                    ChkIFMaintainStockCategories.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkIFMaintainStockCategories").ToString()
                    ChkIFMaintainBatchwiseDetails.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkIFMaintainBatchwiseDetails").ToString()
                    ChkIFMaintainBatchwiseDetailsSetExpiryDate.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkIFMaintainBatchwiseDetailsSetExpiryDate").ToString()
                    ChkIFUseDifferentActualBilledQty.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkIFUseDifferentActualBilledQty").ToString()
                    ChkIFAllowPurchaseOrderProcessing.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkIFAllowPurchaseOrderProcessing").ToString()
                    ChkIFAllowSaleOrderProcessing.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkIFAllowSaleOrderProcessing").ToString()
                    ChkIFAllowJobOrderProcessing.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkIFAllowJobOrderProcessing").ToString()
                    ChkIFAllowInvoicing.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkIFAllowInvoicing").ToString()
                    ChkIFEnterPurchasesinInvoiceFormat.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkIFEnterPurchasesinInvoiceFormat").ToString()
                    ChkIFUseDebitCreditNotes.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkIFUseDebitCreditNotes").ToString()
                    ChkIFUseInvoiceModeforCreditNotes.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkIFUseInvoiceModeforCreditNotes").ToString()
                    ChkIFUseInvoiceModeforDebitNotes.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkIFUseInvoiceModeforDebitNotes").ToString()
                    ChkIFSeperateDiscountColumnonInvoices.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkIFSeperateDiscountColumnonInvoices").ToString()
                    ChkTrackadditionalcostsofpurchase.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkTrackadditionalcostsofpurchase").ToString()
                    ChkUseMultiplePrizeLevels.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkUseMultiplePrizeLevels").ToString()
                    ChkUseTrackingNumbers.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkUseTrackingNumbers").ToString()
                    ChkUseRejectionInwardOutwardNotes.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkUseRejectionInwardOutwardNotes").ToString()
                    ChkUseMaterialInOut.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkUseMaterialInOut").ToString()
                    ChkUseMaterialInOut.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkUseMaterialInOut").ToString()
                    ChkUseCostTrackingforStockItems.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkUseCostTrackingforStockItems").ToString()
                    ChkPutaSpacebetweenAmountandSymbol.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkPutaSpacebetweenAmountandSymbol").ToString()
                    ChkIsSymbolSuffixedtoAmounts.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkIsSymbolSuffixedtoAmounts").ToString()
                    ChkShowAmountinMillions.Checked = dtstSearchDtls.Tables(0).Rows(0)("ChkShowAmountinMillions").ToString()
                    TxtBaseCurrencySymbol.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtBaseCurrencySymbol").ToString()
                    TxtFormalName.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtFormalName").ToString()
                    TxtNumberofDecimalPlace.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtNumberofDecimalPlace").ToString()
                    TxtSymbolforDecimalProtion.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtSymbolforDecimalProtion").ToString()
                    TxtDecimalPlacesforPrintingAmountinWords.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtDecimalPlacesforPrintingAmountinWords").ToString()
                    imgprvw.Src = "../" & dtstSearchDtls.Tables(0).Rows(0)("ClientLogo").ToString()
                    ViewState("fileName") = dtstSearchDtls.Tables(0).Rows(0)("ClientLogo").ToString()
                    TxtClientShortCode.Value = dtstSearchDtls.Tables(0).Rows(0)("ClientShortCode").ToString()
                    TxtECCNO.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtECCNO").ToString()
                    TxtCSTTINNO.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtCSTTINNO").ToString()
                    TxtVATTINNO.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtVATTINNO").ToString()
                    TxtGSTNO.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtGSTNO").ToString()
                    TxtPANo.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtPANo").ToString()
                    TxtBankName.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtBankName").ToString()
                    TxtBankBranchName.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtBankBranchName").ToString()
                    TxtBankAccountNumber.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtBankAccountNumber").ToString()
                    TxtBankIFSCCode.Value = dtstSearchDtls.Tables(0).Rows(0)("TxtBankIFSCCode").ToString()
                    HdnIdCompany.Value = dtstSearchDtls.Tables(0).Rows(0)("IdCompany").ToString()
                End If
            End If
        End If
        clsfn = Nothing
    End Sub
    Protected Sub fnUpload(ByVal sender As Object, ByVal e As EventArgs)
        If flupload.HasFile Then
            If TxtCompanyName.Value.Trim = String.Empty Then Exit Sub
            Dim strpath As String = Path.GetExtension(flupload.FileName)
            If strpath <> ".jpg" AndAlso strpath <> ".jpeg" AndAlso strpath <> ".png" AndAlso strpath <> ".pdf" AndAlso strpath <> ".xls" AndAlso strpath <> ".xlsx" Then

                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('File Uploads should be in JPG/png/pdf/excel format');</script>")
            Else
                Dim fileSize As Integer = flupload.PostedFile.ContentLength
                If fileSize < 512000 Then
                    Dim fileName As String = Replace(Replace(Replace(Replace(Replace(Replace(TxtCompanyName.Value, "'", ""), " ", "", ), ",", ""), "/", ""), "\", "") & "." & Path.GetExtension(flupload.PostedFile.FileName), "..", ".") ' Path.GetFileName(flupload.PostedFile.FileName)
                    If strpath <> ".pdf" AndAlso strpath <> ".xls" AndAlso strpath <> ".xlsx" Then
                        Dim strm As Stream = flupload.PostedFile.InputStream
                        Dim targetPath As String = Server.MapPath("~/Upload/ClientLogo/Thumb/" & fileName)
                        Dim targetFile = targetPath
                        GenerateThumbnails(0.4, strm, targetFile)
                        flupload.PostedFile.SaveAs(Server.MapPath("~/Upload/ClientLogo/") + fileName)
                        imgprvw.Src = "../Upload/ClientLogo/Thumb/" + fileName
                        imgprvw.Alt = fileName
                        ViewState("fileName") = "Upload/ClientLogo/" + fileName
                        ViewState("ClearLogoFile") = False
                    Else
                        flupload.PostedFile.SaveAs(Server.MapPath("~/Upload/ClientDocument/") + fileName)
                        ViewState("fileName") = "Upload/ClientDocument/" + fileName
                    End If
                   
                Else
                    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('File Size Exceeded than 500 KB.');</script>")
                End If

            End If

        End If
    End Sub

    Private Sub GenerateThumbnails(scaleFactor As Double, sourcePath As Stream, targetPath As String)
        Using image__1 = Image.FromStream(sourcePath)
            Dim newWidth = CInt(image__1.Width * scaleFactor)
            Dim newHeight = CInt(image__1.Height * scaleFactor)
            Dim thumbnailImg = New Bitmap(newWidth, newHeight)
            Dim thumbGraph = Graphics.FromImage(thumbnailImg)
            thumbGraph.CompositingQuality = CompositingQuality.HighQuality
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic
            Dim imageRectangle = New Rectangle(0, 0, newWidth, newHeight)
            thumbGraph.DrawImage(image__1, imageRectangle)
            thumbnailImg.Save(targetPath, image__1.RawFormat)
        End Using
    End Sub

    'fnClear is use to clear the image
    Protected Sub fnClear(ByVal sender As Object, ByVal e As EventArgs)
        imgprvw.Src = "../Upload/ClientLogo/Super.png"
        imgprvw.Alt = "Company Logo"
        ViewState("ClearLogoFile") = True
    End Sub
    Protected Sub BtnClear_Click(sender As Object, e As System.EventArgs) Handles BtnClear.Click
        If HdnIdentity.Value.Trim <> "" Then
            fnFillScreen()
        End If
    End Sub

    Private Sub clearfields()
        TxtCompanyName.Value = ""
        TxtMailingAddress.Value = ""
        TxtStatutoryComplianceFor.Value = ""
        TxtState.Value = ""
        TxtPIN.Value = ""
        TxtTelephoneNo.Value = ""
        TxtMobile.Value = ""
        TxtEmail.Value = ""
        TxtCurrencySymbol.Value = ""
        ddlMaintain.SelectedValue = ""

        TxtFinancialYearFrom.Text = StrFinancialYearFrom
        TxtBooksBeginFrom.Text = StrTxtBooksBeginFrom
        ChkAutoBackup.Checked = False
        ChkSecurityControl.Checked = False
        ChkAFIntegrateAccountsandInventory.Checked = False
        ChkAFIncomeandExpenceStatement.Checked = False
        ChkAFAllowMultiCurrency.Checked = False
        ChkAFMaintainBillwiseDetails.Checked = False
        ChkAFActivateInterestCalculation.Checked = False
        ChkAFMaintainPayroll.Checked = False
        ChkAFMaintainCostCentres.Checked = False
        ChkAFUseCostCentreforJobCosting.Checked = False
        ChkAFMorethanONEPayrollCostCategory.Checked = False
        ChkAFUsePreDefinedCostCenterAllocationDuringEntry.Checked = False
        ChkAFShowOpeningBalanceforRevenueItemInReport.Checked = False
        ChkAFAllowInvoicing.Checked = False
        ChkAFEnterPurchasesinInvoiceFormat.Checked = False
        ChkAFUseDebitCreditNotes.Checked = False
        ChkAFUseInvoiceModeforCreditNotes.Checked = False
        ChkAFUseInvoiceModeforDebitNotes.Checked = False
        ChkAFMaintainBudgetandControl.Checked = False
        ChkAFUseReversingJournalsandOptionalVouchers.Checked = False
        ChkAFEnableChequePrinting.Checked = False
        ChkAFAlterBankingFeatures.Checked = False
        ChkAFAllowZeroValuesEntries.Checked = False
        ChkAFMaintainMultipleMailingDetails.Checked = False
        ChkAFSetAlterCompanyMailingDetails.Checked = False
        ChkAFEnableCompanyLogo.Checked = False
        ChkEnableExciseDetails.Checked = False
        ChkSetAlterExciseDetials.Checked = False
        ChkFollowExciseRules.Checked = False
        ChkEnableValueAddTax.Checked = False
        ChkSetAlterVATDetails.Checked = False
        ChkEnableServiceTax.Checked = False
        ChkSetAlterServiceTaxDetails.Checked = False
        ChkEnableLocalBodyTax.Checked = False
        ChkSetAlterLBTDetails.Checked = False
        ChkEnableTaxDeductedAtSource.Checked = False
        ChkSetAlterTDSDetails.Checked = False
        ChkEnableTaxCollectedAtSource.Checked = False
        ChkSetAlterTCSDetails.Checked = False
        ChkEnableFringeBenfitedTax.Checked = False
        ChkSetAlterFBTDetails.Checked = False
        ChkEnableMCAReports.Checked = False
        ChkIFIntegrateAccountsandInventory.Checked = False
        ChkIFAllowZeroValuesEntrie.Checked = False
        ChkIFMaintainMultipleGodowns.Checked = False
        ChkIFMaintainStockCategories.Checked = False
        ChkIFMaintainBatchwiseDetails.Checked = False
        ChkIFMaintainBatchwiseDetailsSetExpiryDate.Checked = False
        ChkIFUseDifferentActualBilledQty.Checked = False
        ChkIFAllowPurchaseOrderProcessing.Checked = False
        ChkIFAllowSaleOrderProcessing.Checked = False
        ChkIFAllowJobOrderProcessing.Checked = False
        ChkIFAllowInvoicing.Checked = False
        ChkIFEnterPurchasesinInvoiceFormat.Checked = False
        ChkIFUseDebitCreditNotes.Checked = False
        ChkIFUseInvoiceModeforCreditNotes.Checked = False
        ChkIFUseInvoiceModeforDebitNotes.Checked = False
        ChkIFSeperateDiscountColumnonInvoices.Checked = False
        ChkTrackadditionalcostsofpurchase.Checked = False
        ChkUseMultiplePrizeLevels.Checked = False
        ChkUseTrackingNumbers.Checked = False
        ChkUseRejectionInwardOutwardNotes.Checked = False
        ChkUseMaterialInOut.Checked = False
        ChkUseMaterialInOut.Checked = False
        ChkUseCostTrackingforStockItems.Checked = False
        ChkPutaSpacebetweenAmountandSymbol.Checked = False
        ChkIsSymbolSuffixedtoAmounts.Checked = False
        ChkShowAmountinMillions.Checked = False
        TxtBaseCurrencySymbol.Value = ""
        TxtFormalName.Value = ""
        TxtNumberofDecimalPlace.Value = ""
        TxtSymbolforDecimalProtion.Value = ""
        TxtDecimalPlacesforPrintingAmountinWords.Value = ""
        imgprvw.Src = "../Upload/ClientLogo/Super.png"
        TxtClientShortCode.Value = ""
        TxtECCNO.Value = ""
        TxtCSTTINNO.Value = ""
        TxtVATTINNO.Value = ""
    End Sub
    Private Sub createDynamicGridInvoice()
        If temp.Columns.Count = 0 Then
            temp.Columns.Add("SlNo")
            temp.Columns.Add("NameofParty")
            temp.Columns.Add("PartyAddress")
            temp.Columns.Add("City")
            temp.Columns.Add("ExciseServiceTaxNo")
            temp.Columns.Add("TinNo")
            temp.Columns.Add("SupplierID")
        End If
    End Sub
    Protected Sub BtnAdd_Click(sender As Object, e As System.EventArgs) Handles BtnAdd.Click
        'If NameofParty.Value = String.Empty Then

        'End If
        'Dim dtU1 As New DataTable()
        'If Gvdetails.HeaderRow IsNot Nothing Then
        '    For i As Integer = 0 To Gvdetails.HeaderRow.Cells.Count - 1
        '        dtU1.Columns.Add(Gvdetails.HeaderRow.Cells(i).Text)
        '    Next
        'End If
        'For Each row As GridViewRow In Gvdetails.Rows
        '    Dim dr As DataRow = dtU1.NewRow()

        '    For i As Integer = 0 To row.Cells.Count - 1
        '        dr(i) = row.Cells(i).Text
        '    Next
        '    dtU1.Rows.Add(dr)
        'Next
        'temp = dtU1
        'If Session("ClientGvd") Is Nothing Then
        '    createDynamicGridInvoice()
        'Else
        '    temp = DirectCast(Session("ClientGvd"), DataTable)
        'End If
        'temp.Rows.Add("", NameofParty.Value, PartyAddress.Value, City.Value, ExciseServiceTaxNo.Value, TinNo.Value)
        'Gvdetails.DataSource = temp
        'Gvdetails.DataBind()
        'Session("ClientGvd") = temp
        If HdnIdentity.Value.Length = 0 Then
            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Please Create Client and then Create the Supplier Details');</script>")
        End If

        If Gvdetails.Rows.Count > 0 Then
            For i As Integer = 0 To Gvdetails.Rows.Count - 1
                If ExciseServiceTaxNo.Value = Gvdetails.Rows(i).Cells(5).Text Then
                    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Excise/Service Tax No Exists');</script>")
                    Exit Sub
                End If
                If TinNo.Value = Gvdetails.Rows(i).Cells(5).Text Then
                    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Tin No Exists');</script>")
                    Exit Sub
                End If
            Next
        End If

        If HdnIdentity.Value.Length > 0 Then
            If HdnSupplierid.Value.Length = 0 Then
                Dim StrQuery As String = String.Empty
                clsObj = New clsData
                StrQuery = "insert into tbl_SupplierDetails values('" & NameofParty.Value & "'," & HdnIdentity.Value & ",'" & PartyAddress.Value & "','" & City.Value & "','" & ExciseServiceTaxNo.Value & "','" & TinNo.Value & "')"
                clsObj.fnIUD(StrQuery, 0, String.Empty)
                clsObj = Nothing
                If Session("ClientGvd") Is Nothing Then
                    Dim dtU1 As New DataTable()
                    If Gvdetails.HeaderRow IsNot Nothing Then
                        For i As Integer = 0 To Gvdetails.HeaderRow.Cells.Count - 1
                            dtU1.Columns.Add(Gvdetails.HeaderRow.Cells(i).Text)
                        Next
                    End If
                    temp = dtU1
                    createDynamicGridInvoice()
                Else
                    temp = DirectCast(Session("ClientGvd"), DataTable)
                End If
                temp.Rows.Add("", NameofParty.Value, PartyAddress.Value, City.Value, ExciseServiceTaxNo.Value, TinNo.Value)
            Else
                Dim StrQuery As String = String.Empty
                clsObj = New clsData
                StrQuery = "Update tbl_SupplierDetails Set NameOfParty ='" & NameofParty.Value & "',"
                StrQuery = StrQuery & "  PartyAddress ='" & PartyAddress.Value & "', "
                StrQuery = StrQuery & "  City = '" & City.Value & "', "
                StrQuery = StrQuery & "  ExciseServiceTaxNo = '" & ExciseServiceTaxNo.Value & "', "
                StrQuery = StrQuery & "  TinNo = '" & TinNo.Value & "' "
                StrQuery = StrQuery & " Where SupplierId =  " & HdnSupplierid.Value
                clsObj.fnIUD(StrQuery, 0, String.Empty)
                clsObj = Nothing
                Dim StrSQLItem As String = "select NameOfParty, PartyAddress, City, ExciseServiceTaxNo, TinNo, SupplierId from tbl_SupplierDetails where idClient = " & HdnIdentity.Value & ""
                clsObj = New clsData
                Dim dtsGrd As New DataSet
                dtsGrd = clsObj.fnDataSet(StrSQLItem.ToString, CommandType.Text)
                clsObj = Nothing
                HdnSupplierid.Value = String.Empty
                BtnAdd.Text = "Add"
                If Not IsNothing(dtsGrd) Then
                    If dtsGrd.Tables.Count > 0 Then
                        If dtsGrd.Tables(0).Rows.Count > 0 Then
                            createDynamicGridInvoice()
                            For Each row As DataRow In dtsGrd.Tables(0).Rows
                                temp.Rows.Add("", row("NameofParty").ToString(), row("PartyAddress").ToString(), row("City").ToString(), row("ExciseServiceTaxNo").ToString(), row("TinNo").ToString(), row("SupplierID").ToString())
                            Next
                            Gvdetails.DataSource = temp
                            Gvdetails.DataBind()
                            Session("ClientGvd") = temp
                        End If
                    End If
                End If
            End If
        End If
        Gvdetails.DataSource = temp
        Gvdetails.DataBind()
        Session("ClientGvd") = temp
        addClear()
    End Sub
    Private Sub addClear()
        NameofParty.Value = String.Empty
        PartyAddress.Value = String.Empty
        City.Value = String.Empty
        ExciseServiceTaxNo.Value = String.Empty
        TinNo.Value = String.Empty
    End Sub

    Protected Sub Gvdetails_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Gvdetails.RowCommand
        BtnAdd.Text = "Add"
        If e.CommandName = "EditRow" Then
            Dim gr As GridViewRow = CType(CType(e.CommandSource, LinkButton).NamingContainer, GridViewRow)
            HdnSupplierid.Value = gr.Cells(6).Text
            NameofParty.Value = gr.Cells(1).Text
            PartyAddress.Value = gr.Cells(2).Text
            City.Value = gr.Cells(3).Text
            ExciseServiceTaxNo.Value = gr.Cells(4).Text
            TinNo.Value = gr.Cells(5).Text
            BtnAdd.Text = "Update"
        End If
    End Sub

    'Protected Sub Gvdetails_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles Gvdetails.RowDeleting
    '    temp = DirectCast(Session("ClientGvd"), DataTable)
    '    Dim index As Integer = Convert.ToInt32(e.RowIndex) - 1
    '    temp = TryCast(Session("ClientGvd"), DataTable)
    '    temp.Rows(index).Delete()
    '    temp.AcceptChanges()
    '    Gvdetails.DataSource = temp
    '    Gvdetails.DataBind()
    '    Session("ClientGvd") = temp
    'End Sub

    'Protected Sub Gvdetails_RowEditing(sender As Object, e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles Gvdetails.RowEditing
    '    'Dim index As Integer = Convert.ToInt32(e.NewEditIndex)
    '    'NameofParty.Value = Gvdetails.Rows(index).Cells(1).Text
    '    'PartyAddress.Value = Gvdetails.Rows(index).Cells(2).Text
    '    'City.Value = Gvdetails.Rows(index).Cells(3).Text
    '    'ExciseServiceTaxNo.Value = Gvdetails.Rows(index).Cells(4).Text
    '    'TinNo.Value = Gvdetails.Rows(index).Cells(5).Text
    'End Sub
End Class
