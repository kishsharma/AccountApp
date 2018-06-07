Imports System.Data
Imports System.Globalization
Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.html.simpleparser


Partial Class AspxPages_AspxTransactionPurchase
    Inherits System.Web.UI.Page
    Public Shared totalAcceblevalue As Decimal
    Dim eachrowttol As Decimal = 0, fullrowttol As Decimal = 0
    Dim temp As New DataTable()
    Public StrReceiptDate As String = String.Empty
    Public StrDateFormat As String

    Dim TotalBill As Decimal = "0"
    Public StrMSTaxStateNo As String = String.Empty
    Public strRoundoffValue As Decimal = 0
    Public StrDecimal As String
    Public Shared StrDecimalPlace As String
    Public Shared format As String
    Public Shared companyName As String

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'generatecounter()
        StrDateFormat = "d-M-Y"
        Dim StrDate As String = "01-Apr-" & Now.Year
        If Not IsPostBack Then
            Dim clsfn As New ClsCommonFunction
            StrReceiptDate = (clsfn.fnChangeDateFormat(Now.Date(), "1", "/", "2", 0))
            clsfn = Nothing
            TxtPurchaseDate.Text = StrReceiptDate
            fnFillDDL()
            HdnIdentity.Value = Request.QueryString("SearchKey")
            lblCompanyName.InnerText = Request.QueryString("Company")
            fnFillScreen()
            Dim clsodbcObj As New clsData
            Dim StrDDLListQry As String = " select '0' id,'Select' data1 Union All select SupplierID, NameofParty from tbl_SupplierDetails where idClient = " & hdnClientid.Value
            clsodbcObj.fnDropDownFill(StrDDLListQry, TxtNameofParty, "data1", "id")
            clsodbcObj = Nothing

            StrDecimalPlace = "0.0"
            format = "{0:N1}"
            If (StrDecimal = "2") Then
                StrDecimalPlace = "0.00"
                format = "{0:N2}"
            End If
            'IIf(StrDecimal = "2", StrDecimalPlace = "0.00", StrDecimalPlace = "0.0")
            loadvalue()
            'If Not IsNothing(Session("ScreenName")) Then

            'End If
            DirectCast(Page.Master.FindControl("lblSiteMap"), Label).Text = "Screen: " & Session("ScreenName").ToString
            Session.Remove("Gvd")
            HdnMSTaxStateNo.Value = System.Configuration.ConfigurationManager.AppSettings("MSTaxStateNo").ToString()
            fnFillStatusScreen()
        End If
    End Sub

    Private Sub fnFillDDL()
        Dim clsodbcObj As New clsData
        Dim StrDDLListQry As String = " select '0' id,'Select' data1 Union All select UMCode, UMCode + '-' + UMName from Vw_UnitsOfMeasurement "
        clsodbcObj.fnDropDownFill(StrDDLListQry, ddlMeasure, "data1", "id")
        clsodbcObj = Nothing
    End Sub

    Private Sub fnFillScreen()
        Dim DtsSearchData As New DataSet
        Dim StrQrySearch As New StringBuilder
        'StrQrySearch.Append(" select '../' + UploadTransactionFileName as 'TranImage', IdTransactionType, DefinationCode + ' - ' + DefinationDesc as 'TransactionType', idUPloadTransaction,")
        'StrQrySearch.Append(" IdClient, cm.TxtCompanyName as 'ClientName', cm.TxtMailingAddress + ', ' + cm.TxtState + ', ' + cm.TxtPIN as 'ClientAddress', cm.TxtECCNO, cm.TxtCSTTINNO,cm.TxtVATTINNO, TransactionID ,c.TxtDecimalPlacesforPrintingAmountinWords as 'DecimalPlace'")
        'StrQrySearch.Append(" from tbl_UPloadTransaction ")
        'StrQrySearch.Append(" Left outer join tbl_defination TT on TT.idDefination = idTransactionType ")
        'StrQrySearch.Append(" Left outer join Vw_ClientMaster cm on cm.ClientId = idCompany ")
        'StrQrySearch.Append(" Left outer join tbl_client c on c.idClient = idCompany  ")
        'StrQrySearch.Append(" where idUPloadTransaction = " & HdnIdentity.Value & "")

        StrQrySearch.Append(" Select TranImage, IdTransactionType,TransactionType, idUPloadTransaction, ")
        StrQrySearch.Append(" IdClient, ClientName, ClientAddress, TxtECCNO,  TxtCSTTINNO, TxtVATTINNO, TransactionID , DecimalPlace ")
        StrQrySearch.Append(" from VwFillScreenPurchase ")
        StrQrySearch.Append(" where idUPloadTransaction = " & HdnIdentity.Value & "")


        Dim clsobj As New clsData
        DtsSearchData = clsobj.fnDataSet(StrQrySearch.ToString, CommandType.Text)
        clsobj = Nothing
        If Not IsNothing(DtsSearchData) Then
            If DtsSearchData.Tables.Count > 0 Then
                If DtsSearchData.Tables(0).Rows.Count > 0 Then
                    hdnClientid.Value = DtsSearchData.Tables(0).Rows(0)("IdClient")
                    If DtsSearchData.Tables(0).Rows(0)("TranImage").ToString().Contains(".pdf") Or DtsSearchData.Tables(0).Rows(0)("TranImage").ToString().Contains(".xls") Or DtsSearchData.Tables(0).Rows(0)("TranImage").ToString().Contains(".xlsx") Then
                        imgprvw.Attributes.Add("style", "display:none")
                        Dim embed As String = "<object data=""{0}"" type=""application/pdf"" width=""500px"" height=""500px"">"
                        embed += "If you are unable to view file, you can download from <a href = ""{0}"">here</a>"
                        embed += " or download <a target = ""_blank"" href = ""http://get.adobe.com/reader/"">Adobe PDF Reader</a> to view the file."
                        embed += "</object>"
                        ltEmbed.Text = String.Format(embed, ResolveUrl(DtsSearchData.Tables(0).Rows(0)("TranImage")))
                    Else
                        imgprvw.Src = DtsSearchData.Tables(0).Rows(0)("TranImage")
                        imgprvw.Attributes.Add("title", Replace(DtsSearchData.Tables(0).Rows(0)("TranImage"), "../Upload/Transaction/", ""))
                        imgprvw.Alt = Replace(DtsSearchData.Tables(0).Rows(0)("TranImage"), "../Upload/Transaction/", "")
                    End If

                    StrDecimal = DtsSearchData.Tables(0).Rows(0)("DecimalPlace")
                    Dim ImgPart() As String = DtsSearchData.Tables(0).Rows(0)("TranImage").Split("/")
                    HdnIDImage.Value = ImgPart(3).Substring(0, ImgPart(3).Length - 4)
                    'LblBuyerName.InnerText = DtsSearchData.Tables(0).Rows(0)("ClientName")
                    'LblBuyerAddress.InnerText = DtsSearchData.Tables(0).Rows(0)("ClientAddress")
                    'LblECCNo.InnerText = "ECC No.:" & DtsSearchData.Tables(0).Rows(0)("TxtECCNO")
                    'LblCSTTINNO.InnerText = "CST TIN No.:" & DtsSearchData.Tables(0).Rows(0)("TxtCSTTINNO")
                    'LblVATTINNO.InnerText = "VAT TIN No.:" & DtsSearchData.Tables(0).Rows(0)("TxtVATTINNO")
                End If
            End If
        End If
    End Sub
    Private Sub fnFillStatusScreen()
        Dim DtsSearchData As New DataSet
        Dim dtsGrd As New DataSet
        Dim StrSQL As String = "select * from tbl_PurchaseMaster where idUPloadTransaction = " & HdnIdentity.Value & ""
        Dim clsobj As New clsData
        Dim StrReviewStatus As String
        Dim StrReviewText As String
        Dim clsfn As New ClsCommonFunction
        DtsSearchData = clsobj.fnDataSet(StrSQL.ToString, CommandType.Text)
        clsobj = Nothing
        If Not IsNothing(DtsSearchData) Then
            If DtsSearchData.Tables.Count > 0 Then
                If DtsSearchData.Tables(0).Rows.Count > 0 Then
                    hdnIDPurchaseMasteredit.Value = DtsSearchData.Tables(0).Rows(0)("IDPurchaseMaster")
                    Dim StrSQLItem As String = "select * from tbl_PurchaseDetails where IDPurchaseMaster = " & hdnIDPurchaseMasteredit.Value & ""
                    clsobj = New clsData
                    dtsGrd = clsobj.fnDataSet(StrSQLItem.ToString, CommandType.Text)
                    clsobj = Nothing
                    If Not IsNothing(dtsGrd) Then
                        If dtsGrd.Tables.Count > 0 Then
                            If dtsGrd.Tables(0).Rows.Count > 0 Then
                                createDynamicGridInvoice()
                                For Each row As DataRow In dtsGrd.Tables(0).Rows
                                    temp.Rows.Add("", row("txtItemName").ToString(), row("ddlMeasure").ToString(), row("txtNos").ToString(), row("txtRate").ToString(), row("txtDutyTax").ToString(), row("txtDutyTaxValue").ToString(), row("txtAssessableValue").ToString())
                                Next
                                Gvdetails.DataSource = temp
                                Gvdetails.DataBind()

                            End If
                        End If
                    End If
                    HdnIdentity.Value = DtsSearchData.Tables(0).Rows(0)("idUPloadTransaction")
                    hdnClientid.Value = DtsSearchData.Tables(0).Rows(0)("IdClient")
                    TxtPurchaseDate.Text = clsfn.fnChangeDateFormat(DtsSearchData.Tables(0).Rows(0)("TxtReceiptDate"), "1", "/", "2", 0)
                    TxtTransactionId.Value = DtsSearchData.Tables(0).Rows(0)("TxtTransactionId")
                    TxtNameofParty.Items.FindByText(DtsSearchData.Tables(0).Rows(0)("TxtNameofParty"))
                    TxtCity.Value = DtsSearchData.Tables(0).Rows(0)("TxtCity")
                    TxtExcNoSrvceTaxNo.Value = DtsSearchData.Tables(0).Rows(0)("TxtExcNoSrvceTaxNo")
                    TxtTinNo.Text = DtsSearchData.Tables(0).Rows(0)("TxtTinNo")
                    txtDiscount.Text = DtsSearchData.Tables(0).Rows(0)("txtDiscount")
                    txtDiscountValue.Value = DtsSearchData.Tables(0).Rows(0)("txtDiscountValue")
                    txtServicetax.Text = DtsSearchData.Tables(0).Rows(0)("txtServicetax")
                    txtServicetaxValue.Value = DtsSearchData.Tables(0).Rows(0)("txtServicetaxValue")
                    txtSbc.Text = DtsSearchData.Tables(0).Rows(0)("txtSbc")
                    txtSbcValue.Value = DtsSearchData.Tables(0).Rows(0)("txtSbcValue")
                    txtKkc.Text = DtsSearchData.Tables(0).Rows(0)("txtKkc")
                    txtKkcValue.Value = DtsSearchData.Tables(0).Rows(0)("txtKkcValue")
                    txtexciseduty.Text = DtsSearchData.Tables(0).Rows(0)("txtexciseduty")
                    txtexcisedutyValue.Value = DtsSearchData.Tables(0).Rows(0)("txtexcisedutyValue")
                    If TxtTinNo.Text <> "" Then
                        Dim str1 = TxtTinNo.Text.Trim().Substring(0, 2)
                        If HdnMSTaxStateNo.Value.Trim().Contains(str1) Then
                            txtOmspurchasecform.Text = DtsSearchData.Tables(0).Rows(0)("txtOmspurchasecform")
                            txtOmspurchasecformValue.Value = DtsSearchData.Tables(0).Rows(0)("txtOmspurchasecformValue")
                            txtOmspurchasefform.Text = DtsSearchData.Tables(0).Rows(0)("txtOmspurchasefform")
                            txtOmspurchasefformValue.Value = DtsSearchData.Tables(0).Rows(0)("txtOmspurchasefformValue")
                            txtOmspurchasewocform.Text = DtsSearchData.Tables(0).Rows(0)("txtOmspurchasewocform")
                            txtOmspurchasewocformValue.Value = DtsSearchData.Tables(0).Rows(0)("txtOmspurchasewocformValue")
                        Else
                            txtMspurchase.Text = DtsSearchData.Tables(0).Rows(0)("txtMspurchase")
                            txtMspurchaseValue.Value = DtsSearchData.Tables(0).Rows(0)("txtMspurchaseValue")
                        End If
                    End If
                    txtVatpurchase.Text = DtsSearchData.Tables(0).Rows(0)("txtVatpurchase")
                    txtVatpurchaseValue.Value = DtsSearchData.Tables(0).Rows(0)("txtVatpurchaseValue")
                    txtWorkscontracttax.Text = DtsSearchData.Tables(0).Rows(0)("txtWorkscontracttax")
                    txtWorkscontracttaxValue.Value = DtsSearchData.Tables(0).Rows(0)("txtWorkscontracttaxValue")
                    txtInputvatoncst.Text = DtsSearchData.Tables(0).Rows(0)("txtInputvatoncst")
                    txtInputvatoncstValue.Value = DtsSearchData.Tables(0).Rows(0)("txtInputvatoncstValue")
                    txtRoundoff.Text = DtsSearchData.Tables(0).Rows(0)("txtRoundoff")
                    txtDiscountbeforegross.Text = DtsSearchData.Tables(0).Rows(0)("txtDiscountbeforegross")
                    txtDiscountbeforegrossValue.Value = DtsSearchData.Tables(0).Rows(0)("txtDiscountbeforegrossValue")
                    txtGrossamountValue.Value = DtsSearchData.Tables(0).Rows(0)("txtGrossamountValue")
                    StrReviewStatus = DtsSearchData.Tables(0).Rows(0)("ReviewStatus")
                    If (clsfn.IsNotDBNull(DtsSearchData.Tables(0).Rows(0)("Remark")) = True) Then
                        StrReviewText = DtsSearchData.Tables(0).Rows(0)("Remark").ToString()
                    Else
                        StrReviewText = ""
                    End If
                    If StrReviewStatus = "2" Then
                        review.Visible = False
                        Review_List.Visible = True
                        btnreset.Visible = True
                        lblReview.InnerText = StrReviewText
                        btnSave.Text = "Save"
                        ScriptManager.RegisterStartupScript(updtPnlPopUp1, updtPnlPopUp1.GetType(), "script", " $(document).ready(function () { showtextbox();  CalculationSum(); $('input.form-control').attr('disabled', false);});", True)
                    Else
                        review.Visible = True
                        btnreset.Visible = False
                        btnSave.Text = "Apply Review"
                        ScriptManager.RegisterStartupScript(updtPnlPopUp1, updtPnlPopUp1.GetType(), "script", " $(document).ready(function () { showtextbox();  CalculationSum(); $('input.form-control').attr('disabled', true);});", True)
                    End If
                    

                    clsfn = Nothing
                End If
            End If
        End If
    End Sub
    Private Sub createDynamicGridInvoice()
        If temp.Columns.Count = 0 Then
            temp.Columns.Add("SlNo")
            temp.Columns.Add("ItemName")
            temp.Columns.Add("Measure")
            temp.Columns.Add("Nos")
            temp.Columns.Add("Rate")
            temp.Columns.Add("DutyTax")
            temp.Columns.Add("DutyTaxValue")
            temp.Columns.Add("AssessableValue")
        End If
    End Sub

    Protected Sub btnAddItem_Click(sender As Object, e As System.EventArgs) Handles btnAddItem.Click
            If txtDutyTax.Value = String.Empty Then
                txtAssessableValue.Value = Convert.ToDecimal(txtNos.Value) * Convert.ToDecimal(txtRate.Value)
            End If
            Dim dtU1 As New DataTable()
            If Gvdetails.HeaderRow IsNot Nothing Then
                For i As Integer = 0 To Gvdetails.HeaderRow.Cells.Count - 1
                    dtU1.Columns.Add(Gvdetails.HeaderRow.Cells(i).Text)
                Next
            End If
            For Each row As GridViewRow In Gvdetails.Rows
                Dim dr As DataRow = dtU1.NewRow()

                For i As Integer = 0 To row.Cells.Count - 1
                    dr(i) = row.Cells(i).Text
                Next
                dtU1.Rows.Add(dr)
            Next
            temp = dtU1
        If Session("TrnPurGvd") Is Nothing Then
            createDynamicGridInvoice()
        Else
            temp = DirectCast(Session("TrnPurGvd"), DataTable)
        End If
            temp.Rows.Add("", txtItemName.Value, ddlMeasure.SelectedValue, txtNos.Value, txtRate.Value, txtDutyTax.Value, txtDutyTaxValue.Value, txtAssessableValue.Value)
            Gvdetails.DataSource = temp
            Gvdetails.DataBind()
        Session("TrnPurGvd") = temp
        ScriptManager.RegisterStartupScript(updtPnlPopUp1, updtPnlPopUp1.GetType(), "showtextbox", "showtextbox(); CalculationSum();", True)
            addClear()
    End Sub
    Private Sub addClear()
        txtItemName.Value = String.Empty
        txtNos.Value = String.Empty
        txtRate.Value = String.Empty
        txtDutyTax.Value = String.Empty
        txtDutyTaxValue.Value = String.Empty
        txtAssessableValue.Value = String.Empty
        ddlMeasure.SelectedIndex = 0

    End Sub
    Protected Sub Gvdetails_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Gvdetails.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Then
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            eachrowttol = Convert.ToDecimal(e.Row.Cells(7).Text)
            fullrowttol = fullrowttol + eachrowttol
        End If
        If e.Row.RowType = DataControlRowType.Footer Then
            totalAcceblevalue = fullrowttol
            txtTotalValue.Text = totalAcceblevalue
            'LoadCalculation()
        End If

    End Sub
    Protected Sub Gvdetails_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles Gvdetails.RowDeleting
        temp = DirectCast(Session("TrnPurGvd"), DataTable)
        Dim index As Integer = Convert.ToInt32(e.RowIndex)
        temp = TryCast(Session("TrnPurGvd"), DataTable)
        temp.Rows(index).Delete()
        temp.AcceptChanges()
        Gvdetails.DataSource = temp
        Gvdetails.DataBind()
        Session("TrnPurGvd") = temp
        ScriptManager.RegisterStartupScript(updtPnlPopUp1, updtPnlPopUp1.GetType(), "showtextbox", "showtextbox(); CalculationSum();", True)
    End Sub

    'Private Sub LoadCalculation()
    '    If Not String.IsNullOrEmpty(totalAcceblevalue) Then
    '        txtTotDiscountValue.Value = String.Format(format, totalAcceblevalue)
    '        txtTotServicetaxValue.Value = String.Format(format, totalAcceblevalue)
    '        txtTotSbcValue.Value = String.Format(format, totalAcceblevalue)
    '        txtTotKkcValue.Value = String.Format(format, totalAcceblevalue)
    '        txtTotexcisedutyValue.Value = String.Format(format, totalAcceblevalue)
    '        txtTotMspurchaseValue.Value = String.Format(format, totalAcceblevalue)
    '        txtTotOmspurchasecformValue.Value = String.Format(format, totalAcceblevalue)
    '        txtTotOmspurchasefformValue.Value = String.Format(format, totalAcceblevalue)
    '        txtTotOmspurchasewocformValue.Value = String.Format(format, totalAcceblevalue)
    '        txtTotVatpurchaseValue.Value = String.Format(format, totalAcceblevalue)
    '        txtTotWorkscontracttaxValue.Value = String.Format(format, totalAcceblevalue)
    '        txtTotInputvatoncstValue.Value = String.Format(format, totalAcceblevalue)
    '        txtTotRoundoffValue.Value = String.Format(format, totalAcceblevalue)
    '        txtTotDiscountbeforegrossValue.Value = String.Format(format, totalAcceblevalue)
    '        txtGrossamountValue.Value = String.Format(format, totalAcceblevalue)
    '    End If
    'End Sub
    'Private Sub generatecounter()
    '    Dim clsObj As New clsData
    '    Dim cnt As String = "select isnull(max(IDPurchaseMaster+1),1) from tbl_PurchaseMaster"
    '    hdnIDPurchaseMaster.Value = clsObj.fnIntRetrive(cnt, 1)
    'End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim clsObj As New clsData
        If btnSave.Text = "Apply Review" Then
            Dim SqlQry As String = "Update tbl_PurchaseMaster set ReviewStatus=" & ddlStatus.SelectedValue & ",Remark='" & txtRemark.Value & "' where IDPurchaseMaster = " & hdnIDPurchaseMasteredit.Value & ""
            Dim StrExp As String = String.Empty
            clsObj = New clsData
            clsObj.fnExecute(SqlQry, StrExp, 0)
            clsObj = Nothing
            Response.Redirect("AspxSearchList.aspx?idMenu=" & Session("IdMenu"), False)
        Else
            If Gvdetails.Rows.Count = 0 Then
                btnSave.Enabled = False
            Else

                Dim StrLngId As String = String.Empty
                Dim dt As DateTime
                Dim TxtRpt As DateTime
                Dim usDtfi As DateTimeFormatInfo = New CultureInfo("en-US", False).DateTimeFormat
                dt = Convert.ToDateTime(System.DateTime.Now(), usDtfi)
                TxtRpt = Convert.ToDateTime(TxtPurchaseDate.Text.ToString(), usDtfi)
                Dim Names As String() = {"idUPloadTransaction", "IdClient", "TxtReceiptDate", "TxtTransactionId", "TxtNameofParty", "TxtCity", "TxtExcNoSrvceTaxNo", "TxtTinNo", "txtDiscount", "txtDiscountValue", "txtServicetax", "txtServicetaxValue", "txtSbc", "txtSbcValue", "txtKkc", "txtKkcValue", "txtexciseduty", "txtexcisedutyValue", "txtMspurchase", "txtMspurchaseValue", "txtOmspurchasecform", "txtOmspurchasecformValue", "txtOmspurchasefform", "txtOmspurchasefformValue", "txtOmspurchasewocform", "txtOmspurchasewocformValue", "txtVatpurchase", "txtVatpurchaseValue", "txtWorkscontracttax", "txtWorkscontracttaxValue", "txtInputvatoncst", "txtInputvatoncstValue", "txtRoundoff", "txtDiscountbeforegross", "txtDiscountbeforegrossValue", "txtGrossamountValue", "AllotmentDate", "CreatedBy", "CreatedDate", "ApprovedStatus", "Approvedby", "Approved", "uploadpdfpath"}
                Dim Values As String() = {HdnIdentity.Value, hdnClientid.Value, TxtRpt, TxtTransactionId.Value, TxtNameofParty.SelectedItem.Text, TxtCity.Value, TxtExcNoSrvceTaxNo.Value, TxtTinNo.Text, txtDiscount.Text, txtDiscountValue.Value, txtServicetax.Text, txtServicetaxValue.Value, txtSbc.Text, txtSbcValue.Value, txtKkc.Text, txtKkcValue.Value, txtexciseduty.Text, txtexcisedutyValue.Value, txtMspurchase.Text, txtMspurchaseValue.Value, txtOmspurchasecform.Text, txtOmspurchasecformValue.Value, txtOmspurchasefform.Text, txtOmspurchasefformValue.Value, txtOmspurchasewocform.Text, txtOmspurchasewocformValue.Value, txtVatpurchase.Text, txtVatpurchaseValue.Value, txtWorkscontracttax.Text, txtWorkscontracttaxValue.Value, txtInputvatoncst.Text, txtInputvatoncstValue.Value, txtRoundoff.Text, txtDiscountbeforegross.Text, txtDiscountbeforegrossValue.Value, txtGrossamountValue.Value, dt, Session("IDUser"), dt, 1, Session("IDUser"), dt, "../Upload/Reports/" & HdnIDImage.Value & ".pdf"}
                hdnIDPurchaseMaster.Value = DirectCast(clsObj.fnInsertUpdate("P_InsertPurchaseMaster", Names, Values), String)
                Dim Query As String() = New String(-1) {}
                Dim index As Integer = 0, countInv As Integer = Gvdetails.Rows.Count
                Query = New String(1 + (countInv - 1)) {}

                Query(0) = "Delete from tbl_PurchaseDetails where IDPurchaseMaster='" & hdnIDPurchaseMaster.Value & "'"
                'Query(1) = "Delete from tbl_PurchaseMaster where idUPloadTransaction='" & HdnIdentity.Value & "'"
                ' Query(2) = "insert into tbl_PurchaseMaster (idUPloadTransaction,IdClient,TxtReceiptDate,TxtTransactionId,TxtNameofParty,TxtCity,TxtExcNoSrvceTaxNo,TxtTinNo,txtDiscount,txtDiscountValue,txtServicetax,txtServicetaxValue,txtSbc,txtSbcValue,txtKkc,txtKkcValue,txtexciseduty,txtexcisedutyValue,txtMspurchase,txtMspurchaseValue,txtOmspurchasecform,txtOmspurchasecformValue,txtOmspurchasefform,txtOmspurchasefformValue,txtOmspurchasewocform,txtOmspurchasewocformValue,txtVatpurchase,txtVatpurchaseValue,txtWorkscontracttax,txtWorkscontracttaxValue,txtInputvatoncst,txtInputvatoncstValue,txtRoundoff,txtDiscountbeforegross,txtDiscountbeforegrossValue,txtGrossamountValue,AllotmentDate,CreatedBy,CreatedDate,ApprovedStatus,Approvedby,Approved,uploadpdfpath) values(" & HdnIdentity.Value & "," & hdnClientid.Value & ",'" & TxtRpt & "'," & TxtTransactionId.Value & ",'" & TxtNameofParty.Value & "','" & TxtCity.Value & "','" & TxtExcNoSrvceTaxNo.Value & "','" & TxtTinNo.Text & "','" & txtDiscount.Text & "','" & txtDiscountValue.Value & "','" & txtServicetax.Text & "','" & txtServicetaxValue.Value & "','" & txtSbc.Text & "','" & txtSbcValue.Value & "','" & txtKkc.Text & "','" & txtKkcValue.Value & "','" & txtexciseduty.Text & "','" & txtexcisedutyValue.Value & "','" & txtMspurchase.Text & "','" & txtMspurchaseValue.Value & "','" & txtOmspurchasecform.Text & "','" & txtOmspurchasecformValue.Value & "','" & txtOmspurchasefform.Text & "','" & txtOmspurchasefformValue.Value & "','" & txtOmspurchasewocform.Text & "','" & txtOmspurchasewocformValue.Value & "','" & txtVatpurchase.Text & "','" & txtVatpurchaseValue.Value & "','" & txtWorkscontracttax.Text & "','" & txtWorkscontracttaxValue.Value & "','" & txtInputvatoncst.Text & "','" & txtInputvatoncstValue.Value & "','" & txtRoundoff.Text & "','" & txtDiscountbeforegross.Text & "','" & txtDiscountbeforegrossValue.Value & "','" & txtGrossamountValue.Value & "','" & dt & "'," & Session("IDUser") & ",'" & dt & "',1," & Session("IDUser") & ",'" & dt & "','../Upload/Reports/" & HdnIDImage.Value & ".pdf')"
                index = 1
                For i As Integer = 0 To countInv - 1
                    Query(index) = "insert into tbl_PurchaseDetails values(" & hdnIDPurchaseMaster.Value & ",'" & Gvdetails.Rows(i).Cells(1).Text.ToString() & "','" & Gvdetails.Rows(i).Cells(2).Text.ToString() & "','" & Gvdetails.Rows(i).Cells(3).Text.Trim() & "','" & Gvdetails.Rows(i).Cells(4).Text.ToString().Trim() & "','" & Gvdetails.Rows(i).Cells(5).Text.ToString().Trim() & "','" & Gvdetails.Rows(i).Cells(6).Text.ToString().Trim() & "','" & Gvdetails.Rows(i).Cells(7).Text.ToString().Trim() & "')"
                    index += 1
                Next
                clsObj.funTransaction(Query)
                clsObj = Nothing
                generateInvoice()
                clearfields()
                loadvalue()
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Purchase Added'); location.href='" & Session("BackPage").ToString() & "';</script>")
                Response.Redirect("AspxSearchList.aspx?idMenu=" & Session("IdMenu"), False)
            End If
        End If
    End Sub
    Private Function generateInvoice()
        Dim PdfStatus As Boolean = False
        Try
            Dim appPath As String = String.Empty
            Dim PDFPath As String = String.Empty
            Dim path As String = String.Empty
            Dim Str As New StringBuilder
            Dim sw As New StringWriter()
            appPath = HttpContext.Current.Request.ApplicationPath
            PDFPath = ConfigurationManager.AppSettings("LoadPdf").ToString()
            path = appPath & PDFPath & HdnIDImage.Value & ".pdf"
            path = Server.MapPath(path)
            Dim Filename As New FileInfo(path)
            If Filename.Exists Then
                File.Delete(path)
            End If
            Dim doc As New Document()
            Dim htmlText As String = TxtCity.Value.Trim()
            Str.Append("<table style='width:100%'>")
            Str.Append("<tr><th colspan='4'><h3>Purchase</h3></th></tr>")
            Str.Append("<tr><td>Purchase Date</td><td>" & TxtPurchaseDate.Text & "</td><td>Bill No</td><td>" & TxtTransactionId.Value & "</td></tr>")
            Str.Append("<tr><td>Name of Party</td><td>" & TxtNameofParty.SelectedItem.Text & "</td><td>Party Address</td><td>" & TxtPartyAddress.Value & "</td></tr>")
            Str.Append("<tr><td>city</td><td>" & TxtCity.Value & "</td><td>Excise No/ServiceTax No</td><td>" & TxtExcNoSrvceTaxNo.Value & "</td></tr>")
            Str.Append("<tr><td>Tin No</td><td>" & TxtTinNo.Text & "</td><td></td><td></td></tr>")
            Str.Append("</table>")
            Str.Append("<table>")
            Str.Append("<tr><th>Slno</th><th>Item name</th><th>Measure</th><th>No</th><th>Rate</th><th>Tax</th><th>Tax value</th><th>Accessible value</th></tr>")
            For i As Integer = 0 To Gvdetails.Rows.Count - 1
                Str.Append("<tr><td>" & i + 1 & "</td><td>" & Gvdetails.Rows(i).Cells(1).Text.ToString() & "</td><td>" & Gvdetails.Rows(i).Cells(2).Text.ToString() & "</td><td>" & Gvdetails.Rows(i).Cells(3).Text.ToString() & "</td><td>" & Gvdetails.Rows(i).Cells(4).Text.ToString() & "</td><td>" & Gvdetails.Rows(i).Cells(5).Text.ToString() & "</td><td>" & Gvdetails.Rows(i).Cells(6).Text.ToString() & "</td><td>" & Gvdetails.Rows(i).Cells(7).Text.ToString() & "</td></tr>")
            Next
            Str.Append("</table>")
            Str.Append("<table style='width:100%'>")
            Str.Append("<tr><td>Total</td><td></td><td></td><td>" & totalAcceblevalue & "</td></tr>")
            Str.Append("<tr><td>Discount %</td><td>" & txtDiscount.Text & "</td><td>" & txtDiscountValue.Value & "</td><td>" & txtTotDiscountValue.Value & "</td></tr>")
            Str.Append("<tr><td>Service Tax %</td><td>" & txtServicetax.Text & "</td><td>" & txtServicetaxValue.Value & "</td><td>" & txtTotServicetaxValue.Value & "</td></tr>")
            Str.Append("<tr><td>SBC %</td><td>" & txtSbc.Text & "</td><td>" & txtSbcValue.Value & "</td><td>" & txtTotSbcValue.Value & "</td></tr>")
            Str.Append("<tr><td>KKc %</td><td>" & txtKkc.Text & "</td><td>" & txtKkcValue.Value & "</td><td>" & txtTotKkcValue.Value & "</td></tr>")
            Str.Append("<tr><td>Excise Duty %</td><td>" & txtexciseduty.Text & "</td><td>" & txtexcisedutyValue.Value & "</td><td>" & txtTotexcisedutyValue.Value & "</td></tr>")

            If TxtTinNo.Text <> "" Then
                Dim str1 = TxtTinNo.Text.Trim().Substring(0, 2)
                If HdnMSTaxStateNo.Value.Trim().Contains(str1) Then
                    Str.Append("<tr><td>OMS Purchase with 'C' form %</td><td>" & txtOmspurchasecform.Text & "</td><td>" & txtOmspurchasecformValue.Value & "</td><td>" & txtTotOmspurchasecformValue.Value & "</td></tr>")
                    Str.Append("<tr><td>OMS Purchase with 'F' form %</td><td>" & txtOmspurchasefform.Text & "</td><td>" & txtOmspurchasefformValue.Value & "</td><td>" & txtTotOmspurchasefformValue.Value & "</td></tr>")
                    Str.Append("<tr><td>OMS Purchase W/O 'C' form %</td><td>" & txtOmspurchasewocform.Text & "</td><td>" & txtOmspurchasewocformValue.Value & "</td><td>" & txtTotOmspurchasewocformValue.Value & "</td></tr>")
                Else
                    Str.Append("<tr><td> MS Purchase %</td><td>" & txtMspurchase.Text & "</td><td>" & txtMspurchaseValue.Value & "</td><td>" & txtTotMspurchaseValue.Value & "</td></tr>")
                End If
            End If
            Str.Append("<tr><td>VAT Purchase %</td><td>" & txtVatpurchase.Text & "</td><td>" & txtVatpurchaseValue.Value & "</td><td>" & txtTotVatpurchaseValue.Value & "</td></tr>")
            Str.Append("<tr><td>Works Contract Tax %</td><td>" & txtWorkscontracttax.Text & "</td><td>" & txtWorkscontracttaxValue.Value & "</td><td>" & txtTotWorkscontracttaxValue.Value & "</td></tr>")
            Str.Append("<tr><td>Input VAT On CST %</td><td>" & txtInputvatoncst.Text & "</td><td>" & txtInputvatoncstValue.Value & "</td><td>" & txtTotInputvatoncstValue.Value & "</td></tr>")
            Str.Append("<tr><td>Round Off ( + / - )</td><td></td><td>" & txtRoundoff.Text & "</td><td>" & txtTotRoundoffValue.Value & "</td></tr>")
            Str.Append("<tr><td>Discount before Gross %</td><td>" & txtDiscountbeforegross.Text & "</td><td>" & txtDiscountbeforegrossValue.Value & "</td><td>" & txtTotDiscountbeforegrossValue.Value & "</td></tr>")
            Str.Append("<tr><td>Gross Amount</td><td></td><td></td><td>" & txtGrossamountValue.Value & "</td></tr>")

            Str.Append("</table>")
            doc = New Document(PageSize.A4, 10.0F, 10.0F, 20.0F, 0.0F)
            Dim writer As PdfWriter = PdfWriter.GetInstance(doc, New FileStream(path, FileMode.Create))
            doc.Open()
            Dim hw As New HTMLWorker(doc)
            hw.Parse(New StringReader(Str.ToString()))
            doc.Close()
            PdfStatus = True
        Catch ex As Exception

        End Try
        Return PdfStatus
    End Function


    Private Shared Function customCertValidation(ByVal sender As Object, _
                                                ByVal cert As X509Certificate, _
                                                ByVal chain As X509Chain, _
                                                ByVal errors As SslPolicyErrors) As Boolean
        Return True
    End Function
    Public Shared Function ServerCertificateValidationCallback(ByVal sender As Object, _
                                                               ByVal cert As System.Security.Cryptography.X509Certificates.X509Certificate, _
                                                               ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, _
                                                               ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Return sslPolicyErrors = System.Net.Security.SslPolicyErrors.None
    End Function
    Private Sub loadvalue()
        txtDiscountValue.Value = StrDecimalPlace
        txtServicetaxValue.Value = StrDecimalPlace
        txtSbcValue.Value = StrDecimalPlace
        txtKkcValue.Value = StrDecimalPlace
        txtexcisedutyValue.Value = StrDecimalPlace
        txtMspurchaseValue.Value = StrDecimalPlace
        txtOmspurchasecformValue.Value = StrDecimalPlace
        txtOmspurchasefformValue.Value = StrDecimalPlace
        txtOmspurchasewocformValue.Value = StrDecimalPlace
        txtVatpurchaseValue.Value = StrDecimalPlace
        txtWorkscontracttaxValue.Value = StrDecimalPlace
        txtInputvatoncstValue.Value = StrDecimalPlace
        txtRoundoffValue.Value = StrDecimalPlace
        txtDiscountbeforegrossValue.Value = StrDecimalPlace
        txtGrossamountValue.Value = StrDecimalPlace

        txtTotDiscountValue.Value = StrDecimalPlace
        txtTotServicetaxValue.Value = StrDecimalPlace
        txtTotSbcValue.Value = StrDecimalPlace
        txtTotKkcValue.Value = StrDecimalPlace
        txtTotexcisedutyValue.Value = StrDecimalPlace
        txtTotMspurchaseValue.Value = StrDecimalPlace
        txtTotOmspurchasecformValue.Value = StrDecimalPlace
        txtTotOmspurchasefformValue.Value = StrDecimalPlace
        txtTotOmspurchasewocformValue.Value = StrDecimalPlace
        txtTotVatpurchaseValue.Value = StrDecimalPlace
        txtTotWorkscontracttaxValue.Value = StrDecimalPlace
        txtTotInputvatoncstValue.Value = StrDecimalPlace
        txtTotRoundoffValue.Value = StrDecimalPlace
        txtTotDiscountbeforegrossValue.Value = StrDecimalPlace
    End Sub

    Private Sub clearfields()
        Session.Remove("Gvd")
        HdnIdentity.Value = ""
        hdnIDPurchaseMaster.Value = ""
        hdnClientid.Value = ""
        TxtTinNo.Text = ""
        TxtTransactionId.Value = ""
        TxtNameofParty.SelectedValue = 0
        TxtPartyAddress.Value = ""
        TxtCity.Value = ""
        TxtExcNoSrvceTaxNo.Value = ""

        txtTotalValue.Text = ""
        txtDiscount.Text = ""
        txtServicetax.Text = ""
        txtSbc.Text = ""
        txtKkc.Text = ""
        txtexciseduty.Text = ""
        txtMspurchase.Text = ""
        txtOmspurchasecform.Text = ""
        txtOmspurchasefform.Text = ""
        txtOmspurchasewocform.Text = ""
        txtVatpurchase.Text = ""
        txtWorkscontracttax.Text = ""
        txtInputvatoncst.Text = ""
        txtRoundoff.Text = ""
        txtDiscountbeforegross.Text = ""

    End Sub

  
    Protected Sub btnreset_Click(sender As Object, e As System.EventArgs) Handles btnreset.Click
        Response.Redirect("AspxTransactionPurchase.aspx?SearchKey=" & HdnIdentity.Value, False)
    End Sub

    Protected Sub BtnRequestNewImage_Click(sender As Object, e As System.EventArgs) Handles BtnRequestNewImage.Click
        Dim Names As String() = {"idUPloadTransaction", "RejectedByUserId"}
        Dim Values As String() = {HdnIdentity.Value, Session("IDUser")}
        Dim StrLngId As String = String.Empty
        Dim clsObj As New clsData
        StrLngId = DirectCast(clsObj.fnInsertUpdate("P_RejectAllotment", Names, Values), String)
        clsObj = Nothing
        ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Image Rejection is Informed to Admin'); location.href='AspxTransactionList.aspx?SearchKey=" & HdnIdentity.Value & "';</script>")
        'Response.Redirect("AspxTransactionList.aspx?SearchKey=" & HdnIdentity.Value, False)
    End Sub

    Public Sub New()

    End Sub

    Protected Sub TxtNameofParty_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles TxtNameofParty.SelectedIndexChanged
        TxtTinNo.Text = String.Empty
        TxtPurchaseDate.Text = String.Empty
        TxtTransactionId.Value = String.Empty
        TxtPartyAddress.Value = String.Empty
        TxtCity.Value = String.Empty
        TxtExcNoSrvceTaxNo.Value = String.Empty
        Gvdetails.DataSource = Nothing
        Gvdetails.DataBind()
        If TxtNameofParty.SelectedValue > 0 Then
            Dim clsodbcObj As New clsData
            Dim strErr As String = String.Empty
            Dim StrSupplierDetails As String = "select PartyAddress, City, ExciseServiceTaxNo, TinNo from tbl_SupplierDetails where SupplierId = " & TxtNameofParty.SelectedValue
            Dim DtsSupplierDtl As New DataSet
            DtsSupplierDtl = clsodbcObj.fnGetDataSet(StrSupplierDetails, strErr)
            clsodbcObj = Nothing
            If Not IsNothing(DtsSupplierDtl) Then
                If DtsSupplierDtl.Tables.Count > 0 Then
                    If DtsSupplierDtl.Tables(0).Rows.Count > 0 Then
                        TxtPartyAddress.Value = DtsSupplierDtl.Tables(0).Rows(0)("PartyAddress")
                        TxtCity.Value = DtsSupplierDtl.Tables(0).Rows(0)("City")
                        TxtExcNoSrvceTaxNo.Value = DtsSupplierDtl.Tables(0).Rows(0)("ExciseServiceTaxNo")
                        TxtTinNo.Text = DtsSupplierDtl.Tables(0).Rows(0)("TinNo")
                    End If
                End If
            End If
        End If
    End Sub
End Class
