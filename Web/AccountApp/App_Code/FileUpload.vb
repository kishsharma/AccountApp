Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.IO
Imports System.Data
Imports System.ServiceModel.Web


' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
'<WebService(Namespace:="http://tempuri.org/")> _
'<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
'<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
<ServiceContract()> _
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)> _
<ServiceBehavior(InstanceContextMode:=InstanceContextMode.PerCall)>
Public Class FileUpload
    Inherits System.Web.Services.WebService

    '<WebMethod()> _
    'Public Function HelloWorld() As String
    '    Return "Hello World"
    'End Function

    '<WebMethod()> _
    'Public Function Upload(contents As Byte(), CompanyId As String, IdUser As String, SeletedTransactionTypeId As String, TransactionID As String, TransactionImageName As String, TransactionImageType As String, TransactionRemark As String, TransactionTags As String) As String
    '    Dim StrReturn As String = String.Empty
    '    Dim StrLogMsg As String = String.Empty
    '    Dim StrLogAPIParameter As String = CompanyId & " | " & IdUser & " | " & SeletedTransactionTypeId & " | " & TransactionID & " | " & TransactionImageName & " | " & TransactionRemark & " | " & TransactionTags
    '    Dim StrLogAPIAccessDateTime As String = Now.ToString
    '    Dim fileName As String = String.Empty
    '    Dim LngReturn As Long = 0
    '    TransactionImageName = TransactionImageName & "." & TransactionImageType
    '    Dim StrExp As String = String.Empty
    '    Try
    '        fileName = TransactionImageName
    '        Dim appData = Server.MapPath("~/Upload/Transaction")
    '        Dim file__1 = Path.Combine(appData, Path.GetFileName(fileName))
    '        File.WriteAllBytes(file__1, contents)
    '        TransactionImageName = "Upload/Transaction/" & TransactionImageName
    '        Dim StrQry As New StringBuilder
    '        StrQry.Append(" Insert into tbl_UploadTransaction(IDCompany, idTransactionType, TransactionId, UploadTransactionFileName, UploadDate, IdUser, UploadFrom, TransactionRemark, TransactionCurrentStatus, TransactionTags) ")
    '        StrQry.Append(" Values( " & CompanyId & " , " & SeletedTransactionTypeId & ", '" & TransactionID & "', '" & TransactionImageName & "', getdate(), " & IdUser & ", 2, '" & TransactionRemark & "', 0, '" & TransactionTags & "')")
    '        Dim clsObj As New clsData
    '        LngReturn = clsObj.fnExecute(StrQry.ToString, StrExp, 1)
    '        clsObj = Nothing
    '        If LngReturn > 0 Then
    '            'Dim DtsReturn As New DataSet
    '            'DtsReturn.Tables.Add("DTRetrun")
    '            'DtsReturn.Tables(0).Columns.Add("Remark")
    '            'DtsReturn.Tables(0).Columns.Add("ResponceStatus")
    '            'DtsReturn.Tables(0).Rows.Add()
    '            'DtsReturn.Tables(0).Rows(0)("Remark") = "Transaction File Uploaded Successfully, Named: " & fileName
    '            'DtsReturn.Tables(0).Rows(0)("ResponceStatus") = "1"
    '            'StrReturn = fnGetJson(DtsReturn.Tables(0))
    '            StrReturn = "Transaction File Uploaded Successfully, Named: " & fileName
    '            StrLogMsg = "Transaction File Uploaded Successfully, Named: " & fileName

    '            Dim clsAPILog As New clsData
    '            Dim LngAPILogID As Long = clsAPILog.fnInsertAPILog(StrLogAPIParameter, StrLogAPIAccessDateTime, StrLogMsg, Now.ToString)
    '            clsAPILog = Nothing
    '        End If
    '    Catch ex As Exception
    '        StrReturn = "Transaction File Upload was Unsuccess With Exception : " & ex.Message 'fnReturnFalseJSON(ex.Message)
    '        StrLogMsg = "Transaction File Upload was Unsuccess With Exception : " & ex.Message

    '        Dim clsObj As New clsData
    '        Dim LngAPILogID As Long = clsObj.fnInsertAPILog(StrLogAPIParameter, StrLogAPIAccessDateTime, StrLogMsg, Now.ToString)
    '        clsObj = Nothing

    '        StrLogMsg = ex.Message & " for API Id : " & LngAPILogID
    '        clsObj = New clsData
    '        clsObj.fninsertLog("Transaction Upload", "Transaction Upload", StrLogMsg, IdUser, "", 1)
    '        clsObj = Nothing
    '    End Try
    '    Return StrReturn
    'End Function

    <WebMethod()> _
    Public Function UploadTransaction(contents As Byte(), CompanyId As String, IdUser As String, SeletedTransactionTypeId As String, TransactionID As String, TransactionImageName As String, TransactionImageType As String, TransactionRemark As String, TransactionTags As String, TransactionDate As String) As String
        Dim StrReturn As String = String.Empty
        Dim StrLogMsg As String = String.Empty
        Dim StrLogAPIParameter As String = CompanyId & " | " & IdUser & " | " & SeletedTransactionTypeId & " | " & TransactionID & " | " & TransactionImageName & " | " & TransactionRemark & " | " & TransactionTags & " | " & TransactionDate
        TransactionImageName = Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(TransactionImageName, "/", "_"), "\", "_"), ":", "_"), "*", "_"), "?", "_"), """", "_"), "<", "_"), ">", "_"), "|", "_")
        Dim StrLogAPIAccessDateTime As String = Now.ToString
        Dim fileName As String = String.Empty
        Dim LngReturn As Long = 0
        TransactionImageName = TransactionImageName & "." & TransactionImageType
        Dim StrExp As String = String.Empty
        Try
            fileName = TransactionImageName
            Dim appData = Server.MapPath("~/Upload/Transaction")
            Dim file__1 = Path.Combine(appData, Path.GetFileName(fileName))
            File.WriteAllBytes(file__1, contents)
            TransactionImageName = "Upload/Transaction/" & TransactionImageName
            Dim StrQry As New StringBuilder
            StrQry.Append(" Insert into tbl_UploadTransaction(IDCompany, idTransactionType, TransactionId, UploadTransactionFileName, UploadDate, IdUser, UploadFrom, TransactionRemark, TransactionCurrentStatus, TransactionTags, TransactionDate) ")
            StrQry.Append(" Values( " & CompanyId & " , " & SeletedTransactionTypeId & ", '" & TransactionID & "', '" & TransactionImageName & "', getdate(), " & IdUser & ", 2, '" & TransactionRemark & "', 0, '" & TransactionTags & "', '" & TransactionDate & "')")
            Dim clsObj As New clsData
            LngReturn = clsObj.fnExecute(StrQry.ToString, StrExp, 1)
            clsObj = Nothing
            If LngReturn > 0 Then
                'Dim DtsReturn As New DataSet
                'DtsReturn.Tables.Add("DTRetrun")
                'DtsReturn.Tables(0).Columns.Add("Remark")
                'DtsReturn.Tables(0).Columns.Add("ResponceStatus")
                'DtsReturn.Tables(0).Rows.Add()
                'DtsReturn.Tables(0).Rows(0)("Remark") = "Transaction File Uploaded Successfully, Named: " & fileName
                'DtsReturn.Tables(0).Rows(0)("ResponceStatus") = "1"
                'StrReturn = fnGetJson(DtsReturn.Tables(0))
                StrReturn = "Transaction File Uploaded Successfully, Named: " & fileName
                StrLogMsg = "Transaction File Uploaded Successfully, Named: " & fileName

                Dim clsAPILog As New clsData
                Dim LngAPILogID As Long = clsAPILog.fnInsertAPILog(StrLogAPIParameter, StrLogAPIAccessDateTime, StrLogMsg, Now.ToString)
                clsAPILog = Nothing
            Else
                'Dim DtsReturn As New DataSet
                'DtsReturn.Tables.Add("DTRetrun")
                'DtsReturn.Tables(0).Columns.Add("Remark")
                'DtsReturn.Tables(0).Columns.Add("ResponceStatus")
                'DtsReturn.Tables(0).Rows.Add()
                'DtsReturn.Tables(0).Rows(0)("Remark") = "Transaction File Upload Failed, Please Contact System Admin"
                'DtsReturn.Tables(0).Rows(0)("ResponceStatus") = "2"
                'StrReturn = fnGetJson(DtsReturn.Tables(0))
                StrReturn = "Transaction File Upload Failed, Please Contact System Admin"
                StrLogMsg = "Transaction File Upload Failed, Please Contact System Admin"

                Dim clsAPILog As New clsData
                Dim LngAPILogID As Long = clsAPILog.fnInsertAPILog(StrLogAPIParameter, StrLogAPIAccessDateTime, StrLogMsg, Now.ToString)
                clsAPILog = Nothing
            End If
        Catch ex As Exception
            'Dim DtsReturn As New DataSet
            'DtsReturn.Tables.Add("DTRetrun")
            'DtsReturn.Tables(0).Columns.Add("Remark")
            'DtsReturn.Tables(0).Columns.Add("ResponceStatus")
            'DtsReturn.Tables(0).Rows.Add()
            'DtsReturn.Tables(0).Rows(0)("Remark") = "Transaction File Upload was Unsuccess With Exception : " & ex.Message 'fnReturnFalseJSON(ex.Message)
            'DtsReturn.Tables(0).Rows(0)("ResponceStatus") = "2"
            'StrReturn = fnGetJson(DtsReturn.Tables(0))
            StrReturn = "Transaction File Upload was Unsuccess With Exception : " & ex.Message 'fnReturnFalseJSON(ex.Message)
            StrLogMsg = "Transaction File Upload was Unsuccess With Exception : " & ex.Message

            Dim clsObj As New clsData
            Dim LngAPILogID As Long = clsObj.fnInsertAPILog(StrLogAPIParameter, StrLogAPIAccessDateTime, StrLogMsg, Now.ToString)
            clsObj = Nothing

            StrLogMsg = ex.Message & " for API Id : " & LngAPILogID
            clsObj = New clsData
            clsObj.fninsertLog("Transaction Upload", "Transaction Upload", StrLogMsg, IdUser, "", 1)
            clsObj = Nothing
        End Try
        Return StrReturn
    End Function
    ''' <summary>
    ''' JSON Value for Un-Successfull Result or Exception Message on API Consumption
    ''' </summary>
    ''' <param name="StrRemark"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function fnReturnFalseJSON(StrRemark) As Stream
        Dim DtsReturn As New DataSet
        DtsReturn.Tables.Add("DTRetrun")
        DtsReturn.Tables(0).Columns.Add("ResponceStatus")
        DtsReturn.Tables(0).Columns.Add("ResponceStatusDetails")
        DtsReturn.Tables(0).Rows.Add()
        DtsReturn.Tables(0).Rows(0)("ResponceStatus") = "0"
        DtsReturn.Tables(0).Rows(0)("ResponceStatusDetails") = StrRemark
        Dim StrReturn As Stream = fnGetJson(DtsReturn.Tables(0))
        Return StrReturn
    End Function

    ''' <summary>
    ''' Create JSON String from Datatable Passed
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function fnGetJson(ByVal dt As DataTable) As Stream
        Dim Jserializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim rowsList As New List(Of Dictionary(Of String, Object))()
        Dim row As Dictionary(Of String, Object)
        For Each dr As DataRow In dt.Rows
            row = New Dictionary(Of String, Object)()
            For Each col As DataColumn In dt.Columns
                row.Add(col.ColumnName, dr(col))
            Next
            rowsList.Add(row)
        Next
        Dim StrJSON As String = Jserializer.Serialize(rowsList)
        Return ConvertToPureJSON(StrJSON)
    End Function

    ''' <summary>
    ''' Convert string to json Stream
    ''' </summary>
    ''' <param name="ret"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvertToPureJSON(ByVal ret As String) As Stream
        Dim Result As Stream = Nothing
        WebOperationContext.Current.OutgoingResponse.ContentType = "application/json;charset=utf-8"
        Result = New MemoryStream(Encoding.UTF8.GetBytes(ret))
        Return Result
    End Function
End Class