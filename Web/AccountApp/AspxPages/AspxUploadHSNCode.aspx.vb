Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Data.SqlClient
Imports System.Configuration

Partial Class AspxPages_AspxUploadHSNCode
    Inherits System.Web.UI.Page

    Private Sub AspxPages_AspxUploadHSNCode_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim StrAbsoluteURL As String = HttpContext.Current.Request.Url.AbsoluteUri
        Dim StrPageUrl As String = StrReverse(Mid(StrReverse(StrAbsoluteURL), 1, InStr(StrReverse(StrAbsoluteURL), "/") - 1))
        Dim StrRequiredURL As String = StrAbsoluteURL
        Session("BackPage") = Replace(StrAbsoluteURL, StrPageUrl, "AspxUploadHSNCode.aspx?idMenu=32")
    End Sub

    Private Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        If FileUpload1.HasFile Then
            ViewState("FileName") = Path.GetFileName(FileUpload1.PostedFile.FileName)
            Dim StrFileName As String = Path.GetFileName(FileUpload1.PostedFile.FileName)
            Dim StrOnlyFileName As String = Replace(Replace(Replace(Mid(StrFileName, 1, InStr(StrFileName, ".") - 1) & "_" & Date.Now(), "-", ""), " ", ""), ":", "")
            Dim StrNewFileName As String = Mid(StrFileName, 1, InStr(StrFileName, ".") - 1)
            Dim StrExtension As String = Path.GetExtension(FileUpload1.PostedFile.FileName)
            Dim StrNewFileNamewithExt As String = StrOnlyFileName & StrExtension
            Dim StrChkSQLResult As String = String.Empty
            Dim StrChkSQL As String = "SELECT idGST FROM tbl_GSTMaster where UploadFileName = '" & StrNewFileName & "'"
            Dim clsobj As New clsData
            Dim DtsChckFile As New DataSet
            Dim StrExcep As String = String.Empty
            DtsChckFile = clsobj.fnGetDataSet(StrChkSQL, StrExcep)
            If Not IsNothing(DtsChckFile) Then
                If DtsChckFile.Tables.Count > 0 Then
                    If DtsChckFile.Tables(0).Rows.Count > 0 Then
                        ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('GST - HSN Code File : '" & ViewState("FileName").ToString & " Already Exists!!!');</script>")
                        Exit Sub
                    Else
                        Dim StrFolderPath As String = ConfigurationManager.AppSettings("HSNFolderPath")
                        Dim StrFilePath As String = Replace(Server.MapPath(StrFolderPath + StrNewFileNamewithExt), "AspxPages\", "")
                        'determine if file exist
                        Try
                            FileUpload1.SaveAs(StrFilePath)
                        Catch ex As Exception

                        End Try
                        'Import_To_Grid(StrFilePath, Mid(StrFileName, 1, InStr(StrFileName, ".") - 1), StrExtension, rbHDR.SelectedItem.Text)
                        Import_To_Grid(StrFilePath, StrExtension, rbHDR.SelectedItem.Text)
                        BtnUploadHSN.Visible = True
                    End If
                End If
            End If
        Else
            Dim DtHSNCode As New DataTable
            DtHSNCode.Columns.Add("Data Found")
            DtHSNCode.Rows.Add("No Records in Source File")
        End If
    End Sub

    'Private Sub Import_To_Grid(ByVal StrFilePath As String, ByVal StrFileName As String, ByVal StrExtension As String, ByVal StrisHDR As String)
    '    Dim conStr As String = ""
    '    Select Case StrExtension
    '        Case ".xls"
    '            'Excel 97-03
    '            conStr = ConfigurationManager.ConnectionStrings("Excel03ConString").ConnectionString
    '            Exit Select
    '        Case ".xlsx"
    '            'Excel 07
    '            conStr = ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
    '            Exit Select
    '    End Select
    '    conStr = String.Format(conStr, StrFilePath, StrisHDR)
    '    Dim connExcel As New OleDbConnection(conStr)
    '    Dim cmdExcel As New OleDbCommand()
    '    Dim oda As New OleDbDataAdapter()
    '    Dim DtHSNCode As New DataTable()
    '    cmdExcel.Connection = connExcel
    '    'Get the name of First Sheet
    '    connExcel.Open()
    '    Dim dtExcelSchema As DataTable
    '    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
    '    Dim SheetName As String = dtExcelSchema.Rows(0)("TABLE_NAME").ToString()
    '    connExcel.Close()
    '    'Read Data from First Sheet
    '    connExcel.Open()
    '    cmdExcel.CommandText = "SELECT GSTCode, GSTCodeDescription, CGSTRate, SGSTRate, IGSTRate, CessCondition, GSTRateWEF, BulkUpload, UploadFileName From [" & SheetName & "]"
    '    oda.SelectCommand = cmdExcel
    '    oda.Fill(DtHSNCode)
    '    If Not IsNothing(DtHSNCode) Then
    '        If DtHSNCode.Rows.Count > 0 Then
    '            ViewState("DtHSNCode") = DtHSNCode
    '        Else
    '            DtHSNCode.Rows.Add("No Records in Source File")
    '        End If
    '    End If
    '    Dim ObjClsCmn As New ClsCommonFunction
    '    LblSearchData.Text = ObjClsCmn.DataTableToHTMLTable(DtHSNCode, "DataList", "table table-striped table-bordered", "0", "100%", True)
    '    ObjClsCmn = Nothing
    'End Sub



    Private Sub Import_To_Grid(ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String)
        Dim conStr As String = ""
        Select Case Extension
            Case ".xls"
                'Excel 97-03
                conStr = ConfigurationManager.ConnectionStrings("Excel03ConString").ConnectionString
                Exit Select
            Case ".xlsx"
                'Excel 07
                conStr = ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
                Exit Select
        End Select
        conStr = String.Format(conStr, FilePath, isHDR)
        Dim connExcel As New OleDbConnection(conStr)
        Dim cmdExcel As New OleDbCommand()
        Dim oda As New OleDbDataAdapter()
        Dim dt As New DataTable()
        cmdExcel.Connection = connExcel
        'Get the name of First Sheet
        connExcel.Open()
        Dim dtExcelSchema As DataTable
        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
        Dim SheetName As String = dtExcelSchema.Rows(0)("TABLE_NAME").ToString()
        connExcel.Close()
        'Read Data from First Sheet
        connExcel.Open()
        cmdExcel.CommandText = "SELECT * From [" & SheetName & "]"
        oda.SelectCommand = cmdExcel
        oda.Fill(dt)
        connExcel.Close()
        ViewState("DtHSNCode") = dt
        ''Bind Data to GridView
        'GridView1.Caption = Path.GetFileName(FilePath)
        'GridView1.DataSource = dt
        'GridView1.DataBind()
        Dim ObjClsCmn As New ClsCommonFunction
        LblSearchData.Text = ObjClsCmn.DataTableToHTMLTable(dt, "DataList", "table table-striped table-bordered", "0", "100%", True)
        ObjClsCmn = Nothing

    End Sub


    Private Sub BtnUploadHSN_Click(sender As Object, e As EventArgs) Handles BtnUploadHSN.Click
        Dim DtHSNCode As New DataTable
        DtHSNCode = ViewState("DtHSNCode")
        If Not IsNothing(DtHSNCode) Then
            If DtHSNCode.Rows.Count > 0 Then
                Dim StrFieldName() As String
                StrFieldName = Replace("GSTCode, GSTCodeDescription, CGSTRate, SGSTRate, IGSTRate, CessCondition, GSTRateWEF, BulkUpload, UploadFileName", " ", "").Split(",")
                Dim StrResult As String = String.Empty
                Dim ClsObj As New clsData
                StrResult = ClsObj.FnBulkInsertToDataBase(DtHSNCode, "tbl_GSTMaster", StrFieldName)
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('GST - HSN Code " & StrResult & " using File: '" & ViewState("FileName").ToString & "');</script>")
                If StrResult = "1" Then
                    fnFillList()
                End If
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('GST - HSN Code " & StrResult & " using File: '" & ViewState("FileName").ToString & "');</script>")
                DtHSNCode = Nothing
                ViewState("DtHSNCode") = Nothing
                Exit Sub
            Else
                DtHSNCode = New DataTable
                DtHSNCode.Columns.Add("Data Found")
                DtHSNCode.Rows.Add("No Records in Source File")
            End If
        Else
            DtHSNCode = New DataTable
            DtHSNCode.Columns.Add("Data Found")
            DtHSNCode.Rows.Add("No Records in Source File")
        End If
        Dim ObjClsCmn As New ClsCommonFunction
        LblSearchData.Text = ObjClsCmn.DataTableToHTMLTable(DtHSNCode, "DataList", "table table-striped table-bordered", "0", "100%", True)
        ObjClsCmn = Nothing
    End Sub

    'Private Sub FnBulkInsertToDataBase(DtBlkInsTable As DataTable, StrSQLTableName As String, StrFieldName() As String)
    '    Dim DtBulkInsertTable As New DataTable
    '    DtBulkInsertTable = DtBlkInsTable
    '    Dim retScon As New SqlConnection
    '    Dim retScmd As New SqlCommand
    '    retScon.ConnectionString = ConfigurationManager.ConnectionStrings("AdminConnection").ToString
    '    retScmd.Connection = retScon
    '    'creating object of SqlBulkCopy  
    '    Dim objbulk As SqlBulkCopy = New SqlBulkCopy(retScon)
    '    'assigning Destination table name  
    '    objbulk.DestinationTableName = StrSQLTableName
    '    'Mapping Table column  
    '    Dim intFieldCount As Integer = 0
    '    For intFieldCount = 0 To StrFieldName.Length - 1
    '        objbulk.ColumnMappings.Add(StrFieldName(intFieldCount).Trim, StrFieldName(intFieldCount).Trim)
    '    Next
    '    retScon.Open()
    '    Try
    '        'inserting bulk Records into DataBase   
    '        objbulk.WriteToServer(DtBulkInsertTable)
    '        ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('GST - HSN Code Upload Successfully using File:'" & StrOthersPrm & "'); location.href='" & Session("BackPage").ToString() & "';</script>")
    '    Catch ex As Exception

    '    End Try

    '    retScon.Close()
    'End Sub

    Private Sub BtnAddHREF_ServerClick(sender As Object, e As EventArgs) Handles BtnAddHREF.ServerClick
        Response.Redirect("AspxAddGSTHSNData.aspx", False)
    End Sub

    Private Sub BtnSearch_ServerClick(sender As Object, e As EventArgs) Handles BtnSearch.ServerClick
        fnFillList()
    End Sub

    Private Sub fnFillList()
        Dim objCls As New clsData
        Dim dtsGrd As New DataSet
        Dim StrSQL As String = "Select GSTCode, GSTCodeDescription, CGSTRate, SGSTRate, IGSTRate, CessCondition, GSTRateWEF from tbl_GSTMaster "
        dtsGrd = objCls.fnDataSet(StrSQL, 1, 1)
        If Not IsNothing(dtsGrd) Then
            If dtsGrd.Tables.Count > 0 Then
                If dtsGrd.Tables(0).Rows.Count > 0 Then
                    Dim ObjClsCmn As New ClsCommonFunction
                    LblSearchData.Text = ObjClsCmn.DataTableToHTMLTable(dtsGrd.Tables(0), "DataList", "table table-striped table-bordered", "0", "100%", True)
                    ObjClsCmn = Nothing
                    'BtnAddHREF.Visible = False
                Else
                    LblSearchData.Text = "<table><tr><td>No Record(s) Found</td></tr></table>"
                End If
            Else
                LblSearchData.Text = "<table><tr><td>No Record(s) Found</td></tr></table>"
            End If
        Else
            LblSearchData.Text = "<table><tr><td>No Record(s) Found</td></tr></table>"
        End If
    End Sub
End Class
