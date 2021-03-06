﻿Imports System.Data
Imports System.Globalization
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.html
Imports iTextSharp.text.html.simpleparser
Imports System.Web.Services

Partial Class AspxPages_AspxInvoiceForm
    Inherits System.Web.UI.Page
    Dim temp As New DataTable()
    Dim increment As Integer
    Dim tempdataset As New DataSet()
    Public Amnt As Decimal = 0D, TotQty As Decimal = 0D, TotRate As Decimal = 0D, TotDiscAmt As Decimal = 0D, TotTaxValue As Decimal = 0D, TotIGSTRate As Decimal = 0D, TotIGSTAmnt As Decimal = 0D, TotCGSTRate As Decimal = 0D, TotCGSTAmnt As Decimal = 0D, TotSGSTRate As Decimal = 0D, TotSGSTAmnt As Decimal = 0D, TotAmnt As Decimal = 0D

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim StrSQL As String = "Select TxtCompanyName as 'TxtCompanyName', ClientLogo as 'ClientLogo',TxtMailingAddress as 'TxtMailingAddress',TxtTelephoneNo as 'TxtTelephoneNo',TxtGSTNO,TxtBankName,TxtBankBranchName	, TxtBankAccountNumber ,	TxtBankIFSCCode ,txtState,GSTStateCode  from tbl_client where idClient=" & Session("IDClient")
        ViewState("strsql") = StrSQL
        'Dim stringReader As String
        'stringReader = fileReader.ReadLine()
        'Dim sr As StreamReader = New StreamReader("D:/KishoreDev/AccountApp/PrasadJoshi/AccountApp/AccountApp/Resource/invoice.html")
        generateInvoice()
        If Not IsPostBack Then
            Dim clsodbcObj As New clsData
            fnFillDDL()
            addClear()
            ClearAll()
            fnFillList()
            Dim StrInvoiceReverseCharge As String = System.Configuration.ConfigurationManager.AppSettings("InvoiceReverseCharge").ToString()
            DDLReverseCharge.SelectedValue = StrInvoiceReverseCharge
        End If

    End Sub

    Private Sub fnFillDDL()
        ''Dim StrDDLListQry As String = " select '0' id,'Select' data1 Union All select SII.IdItems, CONCAT(SII.ItemName, ' - ' , SII.HSNCODE , ' - ' , REPLACE(CONVERT(varchar(11), SII.wefdate,106),' ','-')) ItemName, SII.WEFDate from tbl_SupplierItems where idClient = " & Session("IDClient")
        Dim StrDDLListQry As String = " select '0' id,'Select' data1 Union All select UMCode, UMCode + '-' + UMName from Vw_UnitsOfMeasurement  with (Nolock) "
        Dim clsodbcObj As New clsData
        clsodbcObj.fnDropDownFill(StrDDLListQry, txtUOM, "data1", "id")
        clsodbcObj = Nothing

        Dim strBuyerConsigneestate As String = " select '0' StateCode,'Select' StateName Union All select StateCode, StateName+ '-' + stateCode from Tbl_GSTStateCode  with (Nolock) "
        Dim clsodbcObjstate As New clsData
        clsodbcObjstate.fnDropDownFill(strBuyerConsigneestate, ddlBuyerState, "StateName", "StateCode")
        'clsodbcObj = Nothing
        clsodbcObjstate.fnDropDownFill(StrDDLListQry, ddlConsigneeState, "StateName", "StateCode")
        clsodbcObjstate = Nothing

    End Sub

    <WebMethod()> _
    Public Shared Function GetNo(no As String) As String
        Dim DtsSearchData As New DataSet
        Dim StrQrySearch As String
        StrQrySearch = ("select count(IDInvoiceMaster) as COUNT from dbo.tbl_InvoiceMaster where txtInvoiceNo='" & no & "'")
        Dim clsobj As New clsData
        DtsSearchData = clsobj.fnDataSet(StrQrySearch.ToString, CommandType.Text)
        clsobj = Nothing
        Dim valid As String = "True"
        If DtsSearchData.Tables(0).Rows(0)("COUNT").ToString() = "1" Then
            valid = "True"
        Else
            valid = "False"
        End If
        Return valid
    End Function
    Private Sub fnFillList()
        If Not IsNothing(ViewState("strsql")) Then
            Dim objCls As New clsData
            Dim dtsGrd As New DataSet
            dtsGrd = objCls.fnDataSet(ViewState("strsql"), 1, 1)
            If Not IsNothing(dtsGrd) Then
                If dtsGrd.Tables.Count > 0 Then
                    If dtsGrd.Tables(0).Rows.Count > 0 Then
                        Dim ObjClsCmn As New ClsCommonFunction
                        lblCompanyAddress.InnerHtml = dtsGrd.Tables(0).Rows(0)("TxtMailingAddress").ToString()
                        imgcompanyId.HRef = "../" + dtsGrd.Tables(0).Rows(0)("ClientLogo").ToString()
                        lblCompanyName.InnerHtml = dtsGrd.Tables(0).Rows(0)("TxtCompanyName").ToString()
                        lblCompayNo.InnerHtml = dtsGrd.Tables(0).Rows(0)("TxtTelephoneNo").ToString()
                        lblGstin.InnerText = dtsGrd.Tables(0).Rows(0)("TxtGSTNO").ToString()
                        txtBankAc.Value = dtsGrd.Tables(0).Rows(0)("TxtBankAccountNumber").ToString()
                        txtifsc.Value = dtsGrd.Tables(0).Rows(0)("TxtBankIFSCCode").ToString()
                        txtbankName.Value = dtsGrd.Tables(0).Rows(0)("TxtBankName").ToString()
                        txtBankBranch.Value = dtsGrd.Tables(0).Rows(0)("TxtBankBranchName").ToString()
                        lblCompanystate.InnerText = dtsGrd.Tables(0).Rows(0)("TxtState").ToString()
                        lblCompanyGstStateCode.InnerText = dtsGrd.Tables(0).Rows(0)("GSTStateCode").ToString()

                    End If

                End If

            End If
            objCls = Nothing
        End If
    End Sub
    Private Sub createDynamicGridInvoice()
        If temp.Columns.Count = 0 Then
            'temp.Columns.Add("SlNo")
            'temp.Columns("SlNo").AutoIncrement = True
            'temp.Columns("SlNo").AutoIncrementSeed = 1
            'temp.Columns("SlNo").AutoIncrementStep = 1
            temp.Columns.Add("ItemName")
            temp.Columns.Add("HSNCode")
            temp.Columns.Add("UOM")
            temp.Columns.Add("Nos")
            temp.Columns.Add("Rate")
            temp.Columns.Add("Amount")
            temp.Columns.Add("Discount")
            temp.Columns.Add("TaxValue")
            temp.Columns.Add("IGSTRate")
            temp.Columns.Add("IGSTAmount")
            temp.Columns.Add("CGSTRate")
            temp.Columns.Add("CGSTAmount")
            temp.Columns.Add("SGSTRate")
            temp.Columns.Add("SGSTAmount")
            temp.Columns.Add("TotalAmount")
            temp.Columns.Add("Itemdescription")
        End If
    End Sub

    Protected Sub btnAddItem_Click(sender As Object, e As System.EventArgs) Handles btnAddItem.Click
        Dim dtU1 As New DataTable()
        temp = dtU1
        If Session("InvGvd") Is Nothing Then
            createDynamicGridInvoice()
        Else
            temp = DirectCast(Session("InvGvd"), DataTable)
        End If
        'temp.Rows.Add(txtItemName.SelectedItem.Text, txtHSNCode.Value, txtUOM.Value, txtNos.Value, txtRate.Value, txtAmount.Value, txtDiscAmt.Value, txtTaxValue.Value, txtIGSTRate.Value, txtIGSTAmount.Value, txtCGSTRate.Value, txtCGSTAmount.Value, txtSGSTRate.Value, txtSGSTAmount.Value, txtTotalValue.Value)
        temp.Rows.Add(txtItemName.Value, txtHSNCode.Value, txtUOM.SelectedItem.Value, txtNos.Value, txtRate.Value, txtAmount.Value, txtDiscAmt.Value, txtTaxValue.Value, txtIGSTRate.Value, txtIGSTAmount.Value, txtCGSTRate.Value, txtCGSTAmount.Value, txtSGSTRate.Value, txtSGSTAmount.Value, txtTotalValue.Value, txtItemDescription.Value)
        Session("InvGvd") = temp
        GridView1.DataSource = temp
        GridView1.DataBind()
        ScriptManager.RegisterStartupScript(updtPnlPopUp1, updtPnlPopUp1.GetType(), "calculateword", "calculateword();", True)
        updtPnlPopUp1.Update()
        addClear()
    End Sub
   
    Public Function loadReportdata() As StringBuilder
        Dim tempFooter As New DataTable()
        tempFooter = DirectCast(Session("GvdFooter"), DataTable)
        Dim sb As New StringBuilder()

        If temp.Rows.Count > 0 Then
            sb.Append(" <table id='example' class='display' cellspacing='0' width='100%'>")
            sb.Append(" <thead>")
            sb.Append("<tr><th rowspan='2'>Sl No</th><th rowspan='2'> Product</th><th rowspan='2'> HSN(Code)</th><th rowspan='2'> UOM</th><th rowspan='2'>Qty</th><th rowspan='2'> Rate</th><th rowspan='2'> Amount</th><th rowspan='2'>Discount</th><th rowspan='2'>Taxable Value</th><th colspan='2'>CGST</th><th colspan='2'> SGST</th><th rowspan='2'>Total</th> </tr><tr><th> Rate</th> <th> Amount</th><th>Rate</th> <th>Amount</th></tr>")
            sb.Append("</thead>")
            sb.Append(" <tbody>")
            For Each row As DataRow In temp.Rows
                sb.Append("<tr><td> <a href='#' onclick='deleterow(" & row("SlNo").ToString() & ")' >Delete</a>" & row("SlNo").ToString() & "</td> <td style='text-align: right;'> " & row("ItemName").ToString() & "<span>" & row("Itemdescription").ToString() & "</span></td><td>" & row("HSNCode").ToString() & "</td> <td>" & row("UOM").ToString() & " </td><td>" & row("Nos").ToString() & "</td> <td>" & row("Rate").ToString() & "</td> <td>" & row("Amount").ToString() & "</td><td> " & row("Discount").ToString() & " </td><td>" & row("TaxValue").ToString() & "</td><td>" & row("CGSTRate").ToString() & "</td><td> " & row("CGSTAmount").ToString() & "</td><td> " & row("SGSTRate").ToString() & "</td> <td>" & row("SGSTAmount").ToString() & "</td><td>" & row("TotalAmount").ToString() & "</td> </tr>")
            Next
            sb.Append(" </tbody>")
            If tempFooter.Rows.Count > 0 Then
                sb.Append(" <tfoot>")
                sb.Append("<tr><th colspan='4'> Total</th> <th> " & tempFooter.Rows(0)("Nos") & " </th> <th></th><th> " & tempFooter.Rows(0)("Amount") & "</th><th>" & tempFooter.Rows(0)("Discount") & "</th> <th> " & tempFooter.Rows(0)("TaxValue") & " </th> <th> </th><th>" & tempFooter.Rows(0)("CGSTAmount") & "</th> <th></th> <th>" & tempFooter.Rows(0)("SGSTAmount") & "</th><th>" & tempFooter.Rows(0)("TotalAmount") & "</th></tr>")
                sb.Append("<tr><th colspan='4' rowspan='5'> Total amount in words</th> <th colspan='6' id='thword' rowspan='5'> <span id='spWord'> welcome</span> </th> <th colspan='3'>Total Amount before Discount:</th> <th>" & tempFooter.Rows(0)("TaxValue") & "</th></tr>")
                sb.Append("<tr> </th> <th colspan='3'>Add: CGST:</th> <th>" & tempFooter.Rows(0)("CGSTAmount") & "</th></tr>")
                sb.Append("<tr> </th> <th colspan='3'>Add: SGST:</th> <th>" & tempFooter.Rows(0)("SGSTAmount") & "</th></tr>")
                sb.Append("<tr> </th> <th colspan='3'>Total Tax Amount:</th> <th>" & Convert.ToDecimal(tempFooter.Rows(0)("CGSTAmount")) + Convert.ToDecimal(tempFooter.Rows(0)("SGSTAmount")) & "</th></tr>")
                sb.Append("<tr> </th> <th colspan='3'>Total Amount after Tax:</th> <th>" & tempFooter.Rows(0)("TotalAmount") + Convert.ToDecimal(tempFooter.Rows(0)("CGSTAmount")) + Convert.ToDecimal(tempFooter.Rows(0)("SGSTAmount")) & "</th></tr>")
                sb.Append("</tfoot>")

            End If
            sb.Append(" </table>")
        End If
        Return sb
    End Function
    Private Sub addClear()
        'txtItemName.SelectedValue = "0"
        txtItemName.Value = String.Empty
        'txtUOM.Value = String.Empty
        txtUOM.SelectedValue = "0"
        txtHSNCode.Value = String.Empty
        txtNos.Value = String.Empty
        txtRate.Value = String.Empty
        txtAmount.Value = String.Empty
        txtDiscAmt.Value = String.Empty
        txtTaxValue.Value = String.Empty
        txtIGSTRate.Value = String.Empty
        txtIGSTAmount.Value = String.Empty
        txtCGSTRate.Value = String.Empty
        txtCGSTAmount.Value = String.Empty
        txtSGSTRate.Value = String.Empty
        txtSGSTAmount.Value = String.Empty
        txtTotalValue.Value = String.Empty
        txtHsnDescription.Value = String.Empty
        txtItemDescription.Value = String.Empty
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim invoiceDetails As New DataTable()
        Dim SupplyDate As DateTime
        Dim InvoiceDate As DateTime
        Dim clsObj As New clsData
        Dim usDtfi As DateTimeFormatInfo = New CultureInfo("en-US", False).DateTimeFormat
        SupplyDate = Convert.ToDateTime(TxtSupplyDate.Value, usDtfi)
        InvoiceDate = Convert.ToDateTime(txtInvoiceDate.Value.ToString(), usDtfi)
        Dim uploadfilename = Session("IDClient") & "_" & txtInvoiceNo.Value.Trim() & "_" & txtInvoiceDate.Value.Trim().Replace("-", "_").Replace("-", "_") & ".pdf"
        Dim dt As DateTime
        dt = Convert.ToDateTime(System.DateTime.Now(), usDtfi)
        Dim rdochecked As Integer = 0
        If DDLReverseCharge.SelectedValue = "Yes" Then
            rdochecked = 1
        End If
        'Dim Names As String() = {"IdClient", "txtTransportMode", "txtVehicleNo", "TxtSupplyDate", "txtSupplyPlace", "txtInvoiceNo", "txtInvoiceDate", "txtCharge", "txtInvoiceState", "txtInvoiceCode", "txtShipName", "txtShipAddress", "txtShipGSTIN", "txtShipState", "txtShipCode", "txtBillName", "txtBillAddress", "txtBillGSTIN", "txtBillState", "txtBillCode", "txtBankAc", "txtifsc", "txtTotalValuebeforeTax", "txtTotalCGSTTax", "txtTotalSGSTTax", "txtTotalGSTTax", "txtTotalAmount", "txtGSTCharge", "AllotmentDate", "CreatedBy", "CreatedDate", "ApprovedStatus", "Approvedby", "Approved", "uploadpdfpath"}
        'Dim Values As String() = {Session("IDClient"), txtTransportMode.Value, txtVehicleNo.Value, SupplyDate, txtSupplyPlace.Value, txtInvoiceNo.Value, InvoiceDate, rdochecked, txtInvoiceState.Value, txtInvoiceCode.Value, txtShipName.Value, txtShipAddress.Value, txtShipGSTIN.Value, txtShipState.Value, txtShipCode.Value, txtBillName.Value, txtBillAddress.Value, txtBillGSTIN.Value, txtBillState.Value, txtBillCode.Value, txtBankAc.Value, txtifsc.Value, txtTotalValuebeforeTax.Value, txtTotalCGSTTax.Value, txtTotalSGSTTax.Value, txtTotalGSTTax.Value, txtTotalAmount.Value, txtGSTCharge.Value, dt.ToString, Session("IDUser"), dt.ToString, 1, Session("IDUser"), dt.ToString, "Upload/Reports/" & uploadfilename}
        'hdnIDInvoiceMaster.Value = DirectCast(clsObj.fnInsertUpdate("[P_InsertInvoiceMaster24Jan2018]", Names, Values), String)
        Dim StrSQLInvoiceMaster As String = String.Empty
        StrSQLInvoiceMaster = " insert into tbl_InvoiceMaster (                     IdClient,                txtTransportMode,                txtVehicleNo,       TxtSupplyDate,                txtSupplyPlace,                txtInvoiceNo,       txtInvoiceDate,           txtCharge,                txtInvoiceState,                txtInvoiceCode,                txtShipName,                txtShipAddress,                txtShipGSTIN,                txtShipState,                txtShipCode,                txtBillName,                txtBillAddress,                txtBillGSTIN,                txtBillState,                txtBillCode,                txtBankAc,                txtifsc,                txtTotalValuebeforeTax,                txtTotalCGSTTax,                txtTotalSGSTTax,                txtTotalGSTTax,                txtTotalAmount,                txtGSTCharge, AllotmentDate,                  CreatedBy,CreatedDate,ApprovedStatus,                 Approvedby,  Approved,uploadpdfpath) "
        StrSQLInvoiceMaster = StrSQLInvoiceMaster & (" Values('" & Session("IDClient") & "','" & txtTransportMode.Value & "','" & txtVehicleNo.Value & "','" & SupplyDate & "','" & txtSupplyPlace.Value & "','" & txtInvoiceNo.Value & "','" & InvoiceDate & "','" & rdochecked & "','" & txtInvoiceState.Value & "','" & txtInvoiceCode.Value & "','" & txtShipName.Value & "','" & txtShipAddress.Value & "','" & txtShipGSTIN.Value & "','" & ddlConsigneeState.SelectedItem.Text() & "','" & txtShipCode.Value & "','" & txtBillName.Value & "','" & txtBillAddress.Value & "','" & txtBillGSTIN.Value & "','" & ddlBuyerState.SelectedItem.Text() & "','" & txtBillCode.Value & "','" & txtBankAc.Value & "','" & txtifsc.Value & "','" & txtTotalValuebeforeTax.Value & "','" & txtTotalCGSTTax.Value & "','" & txtTotalSGSTTax.Value & "','" & txtTotalGSTTax.Value & "','" & txtTotalAmount.Value & "','" & txtGSTCharge.Value & "',     getdate(),'" & Session("IDUser") & "',  getdate(),           '1','" & Session("IDUser") & "', getdate(),'" & "Upload/Reports/" & uploadfilename & "')")
        Dim ClsIns As New clsData
        hdnIDInvoiceMaster.Value = ClsIns.fnInsertScopeIdentity(StrSQLInvoiceMaster, "")

        If IsNumeric(hdnIDInvoiceMaster.Value) Then
            invoiceDetails = DirectCast(Session("InvGvd"), DataTable)
            Dim Query As String() = New String(-1) {}
            Dim index As Integer = 0, countInv As Integer = invoiceDetails.Rows.Count
            Query = New String(1 + (countInv - 1)) {}
            Query(0) = "Delete from tbl_InvoiceDetails where IDInvoiceMaster='" & hdnIDInvoiceMaster.Value & "'"
            index = 1
            For Each row As DataRow In invoiceDetails.Rows
                Query(index) = "insert into tbl_InvoiceDetails values(" & hdnIDInvoiceMaster.Value & ",'" & row("ItemName").ToString() & "','" & row("HSNCode").ToString() & "','" & row("UOM").ToString() & "','" & row("Nos").ToString() & "','" & row("Rate").ToString() & "','" & row("Amount").ToString() & "','" & row("Discount").ToString() & "','" & row("TaxValue").ToString() & "','" & row("IGSTRate").ToString() & "','" & row("IGSTAmount").ToString() & "','" & row("CGSTRate").ToString() & "','" & row("CGSTAmount").ToString() & "','" & row("SGSTRate").ToString() & "','" & row("SGSTAmount").ToString() & "','" & row("TotalAmount").ToString() & "','" & row("itemDescription").ToString() & "'  )"
                index += 1
            Next

            clsObj.funTransaction(Query)
            clsObj = Nothing
            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Invoice Added Successfully'); location.href='" & Session("BackPage").ToString() & "';</script>")
            generateInvoice()
        Else
            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('" & hdnIDInvoiceMaster.Value & "');</script>")
        End If
        ''ClearAll()
    End Sub

    Private Sub ClearAll()

        Session("InvGvd") = Nothing
        txtTransportMode.Value = String.Empty
        txtVehicleNo.Value = String.Empty
        TxtSupplyDate.Value = String.Empty
        txtSupplyPlace.Value = String.Empty
        txtInvoiceNo.Value = String.Empty
        txtInvoiceDate.Value = String.Empty
        txtInvoiceState.Value = String.Empty
        txtInvoiceCode.Value = String.Empty
        txtShipName.Value = String.Empty
        txtShipAddress.Value = String.Empty
        txtShipGSTIN.Value = String.Empty
        'txtShipState.Value = String.Empty
        ddlConsigneeState.SelectedValue = "0"
        txtShipCode.Value = String.Empty

        txtBillName.Value = String.Empty
        txtBillAddress.Value = String.Empty
        txtBillGSTIN.Value = String.Empty
        'txtBillState.Value = String.Empty
        ddlBuyerState.SelectedValue = "0"
        txtBillCode.Value = String.Empty
        txtBankAc.Value = String.Empty
        txtifsc.Value = String.Empty

        txtTotalValuebeforeTax.Value = String.Empty
        txtTotalCGSTTax.Value = String.Empty
        txtTotalSGSTTax.Value = String.Empty
        txtTotalGSTTax.Value = String.Empty
        txtTotalAmount.Value = String.Empty
        txtGSTCharge.Value = String.Empty
    End Sub

    'Protected Sub txtItemName_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles txtItemName.SelectedIndexChanged
    '    Dim clsodbcObj As New clsData
    '    Dim StrSupplierDetails As String = " Select ItemOum, HSNCode, IGSTRate, CGSTRate, SGSTRate from tbl_SupplierItems where IdItems = " & txtItemName.SelectedValue
    '    Dim DtsSupplierDtl As New DataSet
    '    Dim strErr As String = String.Empty
    '    DtsSupplierDtl = clsodbcObj.fnGetDataSet(StrSupplierDetails, strErr)
    '    clsodbcObj = Nothing
    '    If Not IsNothing(DtsSupplierDtl) Then
    '        If DtsSupplierDtl.Tables.Count > 0 Then
    '            If DtsSupplierDtl.Tables(0).Rows.Count > 0 Then
    '                txtUOM.Value = DtsSupplierDtl.Tables(0).Rows(0)("ItemOum")
    '                txtHSNCode.Value = DtsSupplierDtl.Tables(0).Rows(0)("HSNCode")
    '                txtIGSTRate.Value = DtsSupplierDtl.Tables(0).Rows(0)("IGSTRate")
    '                txtCGSTRate.Value = DtsSupplierDtl.Tables(0).Rows(0)("CGSTRate")
    '                txtSGSTRate.Value = DtsSupplierDtl.Tables(0).Rows(0)("SGSTRate")
    '            End If
    '        End If
    '    End If
    '    temp = DirectCast(Session("InvGvd"), DataTable)
    '    GridView1.DataSource = temp
    '    GridView1.DataBind()
    'End Sub

    Private Function generateInvoice()
        Dim PdfStatus As Boolean = False
        Try
            'Dim attachment As String = "attachment; filename=" + "abc" + ".pdf"
            'Response.ClearContent()
            'Response.AddHeader("content-disposition", attachment)
            'Response.ContentType = "application/pdf"
            'Dim s_tw As New StringWriter()
            'Dim h_textw As New HtmlTextWriter(s_tw)
            'h_textw.AddStyleAttribute("font-size", "7pt")
            'h_textw.AddStyleAttribute("color", "Black")
            'pnlPerson.RenderControl(h_textw)
            ''Name of the Panel
            'Dim doc As New Document()
            'doc = New Document(PageSize.A4, 5, 5, 15, 5)
            'PdfWriter.GetInstance(doc, Response.OutputStream)
            'doc.Open()
            'Dim s_tr As New StringReader(s_tw.ToString())
            'Dim html_worker As New HTMLWorker(doc)
            'html_worker.Parse(s_tr)
            'doc.Close()
            'Response.Write(doc)

            Dim appPath As String = String.Empty
            Dim PDFPath As String = String.Empty
            Dim path As String = String.Empty
            Dim Str As New StringBuilder
            Dim sw As New StringWriter()
            appPath = HttpContext.Current.Request.ApplicationPath
            PDFPath = ConfigurationManager.AppSettings("LoadPdf").ToString()
            'path = appPath & PDFPath & "1.pdf"
            'path = appPath & PDFPath & Session("IDClient") & "_" & txtInvoiceNo.Value.Trim() & "_" & txtInvoiceDate.Value.Trim().Replace("-", "_").Replace("-", "_") & ".pdf"
            path = appPath & PDFPath & "invocetemplate111.pdf"
            path = Server.MapPath(path)
            Dim Filename As New FileInfo(path)
            If Filename.Exists Then
                File.Delete(path)
            End If
            Dim doc As New Document()

            Str.Append("<html><head><style> .scolor{color:red}</style></head><body><table  style='font-family: Arial Unicode MS, FreeSans; font-size:10px; font-weight: normal;width:100%;'>")
            Str.Append("<tr ><th colspan='2'><h3>Invoice</h3></th><th colspan='4'>" & lblCompanyName.InnerText & "," & lblCompanyAddress.InnerText & "," & lblCompayNo.InnerText & "</th><th colspan='2'>" & lblGstin.InnerText & "</th></tr>")
            Str.Append("</table>")

            Str.Append("<table  style='width:100%;'>")
            Str.Append("<tr border='1'><td bgcolor='#00bcd5' colspan='8' style='text-align: center;font-family: Arial Unicode MS, FreeSans; font-size:10px; font-weight: normal;'>Tax Invoice</td></tr>")
            Str.Append("<tr style='font-family: Arial Unicode MS, FreeSans; font-size:8px; font-weight: normal;'><td style='text-align: right;'>Invoice No:</td><td style='text-align: left;'>" & txtInvoiceNo.Value & "</td><td style='text-align: right;'>Invoice Date:</td><td style='text-align: left;'>" & txtInvoiceDate.Value & "</td><td style='text-align: right;'>Reverse Charge:</td><td style='text-align: left;'>" & DDLReverseCharge.SelectedItem.Text & "</td><td style='text-align: right;'>State/Code:</td><td style='text-align: left;'>" & txtInvoiceState.Value & "/" & txtInvoiceCode.Value & "</td></tr>")
            Str.Append("<tr style='font-family: Arial Unicode MS, FreeSans; font-size:8px; font-weight: normal;'><td style='text-align: right;'>Transport Mode:</td><td style='text-align: left;'>" & txtTransportMode.Value & "</td><td style='text-align: right;'>Vehicle number:</td><td style='text-align: left;'>" & txtVehicleNo.Value & "</td><td style='text-align: right;'>Date of Supply:</td><td style='text-align: left;'>" & TxtSupplyDate.Value & "</td><td style='text-align: right;'>Place of Supply:</td><td  style='text-align: left;'>" & txtSupplyPlace.Value & "</td></tr>")
            Str.Append("</table>")

            Str.Append("<table   style='width:100%;'>")
            Str.Append("<tr border='1' bgcolor='#00bcd5'><td colspan='4' style='text-align: center;font-family: Arial Unicode MS, FreeSans; font-size:10px; font-weight: normal;'>BILL TO PARTY</td></tr>")
            Str.Append("<tr style='font-family: Arial Unicode MS, FreeSans; font-size:8px; font-weight: normal;'><td style='text-align: right;'>Name:</td><td style='text-align: left;'>" & txtBillName.Value & "</td><td style='text-align: right;'>Address:</td><td style='text-align: left;'>" & txtBillAddress.Value & "</td></tr>")
            Str.Append("<tr style='font-family: Arial Unicode MS, FreeSans; font-size:8px; font-weight: normal;'><td style='text-align: right;'>GSTIN:<td style='text-align: left;'>" & txtBillGSTIN.Value & "</td><td style='text-align: right;'>State/Code:</td><td style='text-align: left;'>" & ddlBuyerState.SelectedItem.Text & "/" & txtBillCode.Value & "</td></tr>")
            Str.Append("</table>")

            Str.Append("<table   style='width:100%;border-collapse:collapse;'>")
            Str.Append("<tr border='1' bgcolor='#00bcd5'><td colspan='4' style='font-family: Arial Unicode MS, FreeSans; font-size:10px; font-weight: normal;text-align: center;'>SHIP TO PARTY</td></tr>")
            Str.Append("<tr style='font-family: Arial Unicode MS, FreeSans; font-size:8px; font-weight: normal;'><td style='text-align: right;'>Name:</td><td style='text-align: left;'>" & txtShipName.Value & "</td><td style='text-align: right;'>Address:</td><td style='text-align: left;'>" & txtShipAddress.Value & "</td></tr>")
            Str.Append("<tr style='font-family: Arial Unicode MS, FreeSans; font-size:8px; font-weight: normal;'><td style='text-align: right;'>GSTIN:<td style='text-align: left;'>" & txtShipGSTIN.Value & "</td><td style='text-align: right;'>State/Code:</td><td style='text-align: left;'>" & ddlConsigneeState.SelectedItem.Text() & "/" & txtShipCode.Value & "</td></tr>")
            Str.Append("</table>")

            Str.Append("<table border='1'  style='width:100%;border-collapse:collapse;'>")
            Str.Append("<tr bgcolor='#00bcd5'><td colspan='16' style='font-family: Arial Unicode MS, FreeSans; font-size:10px; font-weight: normal;text-align: center;'>Item Details</td></tr>")
            Str.Append("<tr bgcolor='#A8A8A8' style='font-family: Arial Unicode MS, FreeSans; font-size:6px; font-weight: normal;'><td colspan='9'></td><td colspan='2' style='text-align: center;'>IGST Tax</td><td colspan='2' style='text-align: center;'>CGST Tax</td><td style='text-align: center;' colspan='2'>SGST Tax</td><td></td></tr>")
            Str.Append("<tr bgcolor='#A8A8A8' style='font-family: Arial Unicode MS, FreeSans; font-size:6px; font-weight: normal;'><td>Sl No</td><td>ItemName</td><td>HSNCode</td><td>UOM</td><td>Nos</td><td>Rate</td><td>Amount</td><td>Discount</td><td>TaxValue</td><td>IGSTRate</td><td>IGSTAmt</td><td>CGSTRate</td><td>CGSTAmt</td><td>SGSTRate</td><td>SGSTAmt</td><td>TotalAmt</td></tr>")

            'Dim path As String = path.Combine(Directory.GetCurrentDirectory(), "../Resource/invoice.html")
            'String text = File.ReadAllText("Some\\Path.txt");
            'Dim text = File.ReadAllText("../Resource/invoice.html")
            'Dim sr As StreamReader = New StreamReader("../Resource/invoice.html")
            Str.Append("")
            '" & GridView1.Rows(i).Cells(2).Text.ToString() & "

            For i As Integer = 0 To GridView1.Rows.Count - 1
                Str.Append("<tr style='font-family: Arial Unicode MS, FreeSans; font-size:6px; font-weight: normal;'><td>" & i + 1 & "</td><td>" & GridView1.Rows(i).Cells(2).Text.ToString() & "</td><td>" & GridView1.Rows(i).Cells(3).Text.ToString() & "</td><td>" & GridView1.Rows(i).Cells(4).Text.ToString() & "</td><td style='text-align: right;'>" & GridView1.Rows(i).Cells(5).Text.ToString() & "</td><td style='text-align: right;'>" & GridView1.Rows(i).Cells(6).Text.ToString() & "</td><td style='text-align: right;'>" & GridView1.Rows(i).Cells(7).Text.ToString() & "</td><td style='text-align: right;'>" & GridView1.Rows(i).Cells(8).Text.ToString() & "</td><td style='text-align: right;'>" & GridView1.Rows(i).Cells(9).Text.ToString() & "</td><td style='text-align: right;'>" & GridView1.Rows(i).Cells(10).Text.ToString() & "</td><td style='text-align: right;'>" & GridView1.Rows(i).Cells(11).Text.ToString() & "</td><td style='text-align: right;'>" & GridView1.Rows(i).Cells(12).Text.ToString() & "</td><td style='text-align: right;'>" & GridView1.Rows(i).Cells(13).Text.ToString() & "</td><td style='text-align: right;'>" & GridView1.Rows(i).Cells(14).Text.ToString() & "</td><td style='text-align: right;'>" & GridView1.Rows(i).Cells(15).Text.ToString() & "</td><td style='text-align: right;'>" & GridView1.Rows(i).Cells(16).Text.ToString() & "</td></tr>")
            Next
            Str.Append("<tr bgcolor='#A8A8A8' style='font-family: Arial Unicode MS, FreeSans; font-size:6px; font-weight: normal;'><td colspan='4' style='text-align: right;'>TOTAL</td><td style='text-align: right;'>" & ViewState("TotQty") & "</td><td style='text-align: right;'>" & ViewState("TotRate") & "</td><td style='text-align: right;'>" & ViewState("Amnt") & "</td><td style='text-align: right;'>" & ViewState("TotDiscAmt") & "</td><td style='text-align: right;'>" & ViewState("TotTaxValueD") & "</td><td style='text-align: right;'>" & ViewState("TotIGSTRate") & "</td><td style='text-align: right;'>" & ViewState("TotIGSTAmnt") & "</td><td style='text-align: right;'>" & ViewState("TotCGSTRate") & "</td><td style='text-align: right;'>" & ViewState("TotCGSTAmnt") & "</td><td style='text-align: right;'>" & ViewState("TotSGSTRate") & "</td><td style='text-align: right;'>" & ViewState("TotSGSTAmnt") & "</td><td style='text-align: right;'>" & ViewState("TotAmnt") & "</td></tr>")
            Str.Append("<tr style='font-family: Arial Unicode MS, FreeSans; font-size:6px; font-weight: normal;'><td colspan='4' style='text-align: right;'>TOTAL AMOUNT IN WORDS</td><td colspan='5' style='text-align: left;'>" & TxtAmtinWord.Value & "</td><td colspan='6' style='text-align: right;'>Total Amount before Discount:</td><td style='text-align: right;'>" & txtTotalValuebeforeTax.Value & "</td></tr>")
            Str.Append("<tr border='0' style='font-family: Arial Unicode MS, FreeSans; font-size:6px; font-weight: normal;'><td colspan='15' style='text-align: right;'>Add: IGST:</td><td style='text-align: right;'>" & txtTotalIGSTTax.Value & "</td></tr>")
            Str.Append("<tr border='0' style='font-family: Arial Unicode MS, FreeSans; font-size:6px; font-weight: normal;'><td colspan='15' style='text-align: right;'>Add: CGST:</td><td style='text-align: right;'>" & txtTotalCGSTTax.Value & "</td></tr>")
            Str.Append("<tr border='0' style='font-family: Arial Unicode MS, FreeSans; font-size:6px; font-weight: normal;'><td colspan='15' style='text-align: right;'>Add: SGST:</td><td style='text-align: right;'>" & txtTotalSGSTTax.Value & "</td></tr>")
            Str.Append("<tr border='0' style='font-family: Arial Unicode MS, FreeSans; font-size:6px; font-weight: normal;'><td colspan='15' style='text-align: right;'>Total Tax Amount:</td><td style='text-align: right;'>" & txtTotalGSTTax.Value & "</td></tr>")
            Str.Append("<tr border='0' style='font-family: Arial Unicode MS, FreeSans; font-size:6px; font-weight: normal;'><td colspan='15' style='text-align: right;'>Total Amount after Tax:</td><td style='text-align: right;'>" & txtTotalCGSTTax.Value + txtTotalSGSTTax.Value + txtTotalIGSTTax.Value + txtTotalAmount.Value & "</td></tr>")
            Str.Append("</table>")

            Str.Append("<table border='1'  style='width:100%;'>")
            Str.Append("<tr bgcolor='#00bcd5'><td colspan='4' style='font-family: Arial Unicode MS, FreeSans; font-size:10px; font-weight: normal;text-align: center;'>Bank Details</td></tr>")
            Str.Append("<tr style='font-family: Arial Unicode MS, FreeSans; font-size:8px; font-weight: normal;'><td style='text-align: right;'>Bank A/C:</td><td style='text-align: left;'>" & txtBankAc.Value & "</td><td style='text-align: right;'>Bank IFSC:</td><td style='text-align: left;'>" & txtifsc.Value & "</td></tr>")
            Str.Append("</table><p class='scolor'>welcoem to pdf</p></body></html>")

            doc = New Document(PageSize.A4, 10.0F, 10.0F, 20.0F, 0.0F)
            Dim writer As PdfWriter = PdfWriter.GetInstance(doc, New FileStream(path, FileMode.Create))
            doc.Open()
            Dim hw As New HTMLWorker(doc)
            Dim fileReader As System.IO.StreamReader
            fileReader = My.Computer.FileSystem.OpenTextFileReader("D:/KishoreDev/AccountApp/PrasadJoshi/AccountApp/AccountApp/Resource/invoicetablenewBorderwitColor.html")
            Dim stringReader As String
            stringReader = fileReader.ReadToEnd()

            hw.Parse(New StringReader(stringReader.ToString()))
            doc.Close()
            PdfStatus = True

        Catch ex As Exception

        End Try
        Return PdfStatus
    End Function
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
    End Sub
   
    Protected Sub GridView1_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' GridView1.Columns(0).Visible = False
            e.Row.Cells(0).Visible = False
            TotQty += Convert.ToDecimal(e.Row.Cells(5).Text)
            TotRate += Convert.ToDecimal(e.Row.Cells(6).Text)
            Amnt += Convert.ToDecimal(e.Row.Cells(7).Text)
            TotDiscAmt += Convert.ToDecimal(e.Row.Cells(8).Text)
            TotTaxValue += Convert.ToDecimal(e.Row.Cells(9).Text)

            TotIGSTRate += Convert.ToDecimal(e.Row.Cells(10).Text)
            TotIGSTAmnt += Convert.ToDecimal(e.Row.Cells(11).Text)

            TotCGSTRate += Convert.ToDecimal(e.Row.Cells(12).Text)
            TotCGSTAmnt += Convert.ToDecimal(e.Row.Cells(13).Text)

            TotSGSTRate += Convert.ToDecimal(e.Row.Cells(14).Text)
            TotSGSTAmnt += Convert.ToDecimal(e.Row.Cells(15).Text)

            TotAmnt += Convert.ToDecimal(e.Row.Cells(16).Text)

            increment += 1
        End If
        If e.Row.RowType = DataControlRowType.Footer Then
            ViewState("TotQty") = TotQty
            ViewState("TotRate") = TotRate
            ViewState("Amnt") = Amnt
            ViewState("TotDiscAmt") = TotDiscAmt
            ViewState("TotTaxValue") = TotTaxValue

            ViewState("TotIGSTRate") = TotIGSTRate
            ViewState("TotIGSTAmnt") = TotIGSTAmnt

            ViewState("TotCGSTRate") = TotCGSTRate
            ViewState("TotCGSTAmnt") = TotCGSTAmnt

            ViewState("TotSGSTRate") = TotSGSTRate
            ViewState("TotSGSTAmnt") = TotSGSTAmnt

            ViewState("TotAmnt") = TotAmnt

            txtTotalValuebeforeTax.Value = Amnt.ToString()
            txtTotalIGSTTax.Value = TotIGSTAmnt.ToString()
            txtTotalCGSTTax.Value = TotCGSTAmnt.ToString()
            txtTotalSGSTTax.Value = TotSGSTAmnt.ToString()
            txtTotalGSTTax.Value = (TotIGSTAmnt + TotCGSTAmnt + TotSGSTAmnt).ToString()
            txtTotalAmount.Value = TotAmnt.ToString()

            Dim drowFooter As New GridViewRow(0, -1, DataControlRowType.Footer, DataControlRowState.Normal)
            Dim Total As TableCell = New TableHeaderCell()
            Total.Text = "Total"
            Total.CssClass = "rowDiff"
            Total.Style.Add("text-align", "right")
            Total.ColumnSpan = 4
            drowFooter.Cells.Add(Total)

            Dim Nos As TableCell = New TableHeaderCell()
            Nos.Text = ViewState("TotQty")
            Nos.CssClass = "rowDiff"
            drowFooter.Cells.Add(Nos)

            Dim Rate As TableCell = New TableHeaderCell()
            Rate.Text = ViewState("TotRate")
            Rate.CssClass = "rowDiff"
            drowFooter.Cells.Add(Rate)

            Dim Amount As TableCell = New TableHeaderCell()
            Amount.Text = ViewState("Amnt")
            Amount.CssClass = "rowDiff"
            drowFooter.Cells.Add(Amount)

            Dim Discount As TableCell = New TableHeaderCell()
            Discount.Text = ViewState("TotDiscAmt")
            Discount.CssClass = "rowDiff"
            drowFooter.Cells.Add(Discount)

            Dim TotTaxValueD As TableCell = New TableHeaderCell()
            TotTaxValueD.Text = ViewState("TotTaxValue")
            TotTaxValueD.CssClass = "rowDiff"
            drowFooter.Cells.Add(TotTaxValueD)

            Dim IGSTRate As TableCell = New TableHeaderCell()
            IGSTRate.Text = ViewState("TotIGSTRate")
            IGSTRate.CssClass = "rowDiff"
            drowFooter.Cells.Add(IGSTRate)

            Dim IGSTAmount As TableCell = New TableHeaderCell()
            IGSTAmount.Text = ViewState("TotIGSTAmnt")
            IGSTAmount.CssClass = "rowDiff"
            drowFooter.Cells.Add(IGSTAmount)

            Dim CGSTRate As TableCell = New TableHeaderCell()
            CGSTRate.Text = ViewState("TotCGSTRate")
            CGSTRate.CssClass = "rowDiff"
            drowFooter.Cells.Add(CGSTRate)

            Dim CGSTAmount As TableCell = New TableHeaderCell()
            CGSTAmount.Text = ViewState("TotCGSTAmnt")
            CGSTAmount.CssClass = "rowDiff"
            drowFooter.Cells.Add(CGSTAmount)

            Dim SGSTRate As TableCell = New TableHeaderCell()
            SGSTRate.Text = ViewState("TotSGSTRate")
            SGSTRate.CssClass = "rowDiff"
            drowFooter.Cells.Add(SGSTRate)

            Dim SGSTAmount As TableCell = New TableHeaderCell()
            SGSTAmount.Text = ViewState("TotSGSTAmnt")
            SGSTAmount.CssClass = "rowDiff"
            drowFooter.Cells.Add(SGSTAmount)

            Dim TotalAmount As TableCell = New TableHeaderCell()
            TotalAmount.Text = ViewState("TotAmnt")
            TotalAmount.CssClass = "rowDiff"
            drowFooter.Cells.Add(TotalAmount)

            DirectCast(GridView1.Controls(0), Table).Rows.AddAt(increment + 1, drowFooter)

            Dim drowFooter1 As New GridViewRow(0, -1, DataControlRowType.Footer, DataControlRowState.Normal)
            Dim TotalAmountWords As TableCell = New TableCell()
            TotalAmountWords.Text = "TOTAL AMOUNT IN WORDS"
            TotalAmountWords.Style.Add("text-align", "right")
            TotalAmountWords.ColumnSpan = 6
            TotalAmountWords.RowSpan = 6
            drowFooter1.Cells.Add(TotalAmountWords)
            Dim Words As TableCell = New TableCell()
            Words.ID = "spWord"
            Words.ColumnSpan = 5
            Words.CssClass = "csstdword"
            Words.RowSpan = 6
            drowFooter1.Cells.Add(Words)

            Dim TotbTax As TableCell = New TableCell()
            TotbTax.Text = "TOTAL AMOUNT BEFORE Discount:"
            TotbTax.Style.Add("text-align", "right")
            TotbTax.ColumnSpan = 4
            drowFooter1.Cells.Add(TotbTax)
            Dim TotbTaxamt As TableCell = New TableCell()
            TotbTaxamt.Text = Amnt
            drowFooter1.Cells.Add(TotbTaxamt)
            DirectCast(GridView1.Controls(0), Table).Rows.AddAt(increment + 2, drowFooter1)

            Dim drowFooter2 As New GridViewRow(0, -1, DataControlRowType.Footer, DataControlRowState.Normal)
            Dim igst As TableCell = New TableCell()
            igst.Text = "ADD: IGST:"
            igst.Style.Add("text-align", "right")
            igst.ColumnSpan = 4
            drowFooter2.Cells.Add(igst)
            Dim igstamt As TableCell = New TableCell()
            igstamt.Text = TotIGSTAmnt
            drowFooter2.Cells.Add(igstamt)
            DirectCast(GridView1.Controls(0), Table).Rows.AddAt(increment + 3, drowFooter2)

            Dim drowFooter3 As New GridViewRow(0, -1, DataControlRowType.Footer, DataControlRowState.Normal)
            Dim cgst As TableCell = New TableCell()
            cgst.Text = "ADD: CGST:"
            cgst.Style.Add("text-align", "right")
            cgst.ColumnSpan = 4
            drowFooter3.Cells.Add(cgst)
            Dim cgstamt As TableCell = New TableCell()
            cgstamt.Text = TotCGSTAmnt
            drowFooter3.Cells.Add(cgstamt)
            DirectCast(GridView1.Controls(0), Table).Rows.AddAt(increment + 4, drowFooter3)

            Dim drowFooter4 As New GridViewRow(0, -1, DataControlRowType.Footer, DataControlRowState.Normal)
            Dim sgst As TableCell = New TableCell()
            sgst.Text = "ADD: SGST:"
            sgst.Style.Add("text-align", "right")
            sgst.ColumnSpan = 4
            drowFooter4.Cells.Add(sgst)
            Dim sgstamt As TableCell = New TableCell()
            sgstamt.Text = TotSGSTAmnt
            drowFooter4.Cells.Add(sgstamt)
            DirectCast(GridView1.Controls(0), Table).Rows.AddAt(increment + 5, drowFooter4)

            Dim drowFooter5 As New GridViewRow(0, -1, DataControlRowType.Footer, DataControlRowState.Normal)
            Dim ttm As TableCell = New TableCell()
            ttm.Text = "TOTAL TAX AMOUNT:"
            ttm.ColumnSpan = 4
            ttm.Style.Add("text-align", "right")
            drowFooter5.Cells.Add(ttm)
            Dim ttmamt As TableCell = New TableCell()
            ttmamt.Text = TotIGSTAmnt + TotCGSTAmnt + TotSGSTAmnt
            drowFooter5.Cells.Add(ttmamt)
            DirectCast(GridView1.Controls(0), Table).Rows.AddAt(increment + 6, drowFooter5)

            Dim drowFooter6 As New GridViewRow(0, -1, DataControlRowType.Footer, DataControlRowState.Normal)
            Dim tmat As TableCell = New TableCell()
            tmat.Text = "TOTAL AMOUNT AFTER TAX:"
            tmat.Style.Add("text-align", "right")
            tmat.ColumnSpan = 4
            drowFooter6.Cells.Add(tmat)
            Dim tmatamt As TableCell = New TableCell()
            tmatamt.Text = TotAmnt
            drowFooter6.Cells.Add(tmatamt)
            DirectCast(GridView1.Controls(0), Table).Rows.AddAt(increment + 7, drowFooter6)
        End If
    End Sub



    Protected Sub GridView1_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridView1.RowDeleting

        Dim index As Integer = Convert.ToInt32(e.RowIndex)
        temp = TryCast(Session("InvGvd"), DataTable)
        temp.Rows(index).Delete()
        temp.AcceptChanges()
        Session("InvGvd") = temp
        GridView1.DataSource = temp
        GridView1.DataBind()
        updtPnlPopUp1.Update()
    End Sub
End Class
