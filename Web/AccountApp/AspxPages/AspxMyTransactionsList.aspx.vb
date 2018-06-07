Imports System.Net
Imports System.IO
Imports System.Data

Partial Class AspxPages_AspxMyTransactionsList
    Inherits System.Web.UI.Page
    'Dim StrKeyValue As String() = Nothing

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            If IsNothing(Session("IDUser")) Then
                Response.Redirect("~/Default.aspx", False)
                Exit Sub
            End If
            fnFillDDL()
            Dim objCls As New clsData
            Session("IdMenu") = Request.QueryString("idMenu")
            Dim StrSQL As String = String.Empty
            objCls = New clsData
            StrSQL = "Select case when [Description] ='Company' then '" & Session("ProjectType") & "' else [Description] end [Description] from tbl_menudetail where idmenudtl = " & Request.QueryString("idMenu")
            LblScreenName.Value = objCls.fnStrRetrive(StrSQL, 1)
            Session("ScreenName") = LblScreenName.Value
            DirectCast(Page.Master.FindControl("lblSiteMap"), Label).Text = "Screen: Search - " & LblScreenName.Value
            objCls = Nothing
            'StrKeyValue(0) = Replace(StrKeyValue(0), "[dateformat]", Session("SQLDTFormat"))
            'StrKeyValue(0) = Replace(StrKeyValue(0), "[IDUser]", Session("IDUser"))
            Dim StrAbsoluteURL As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim StrRelativeURL As String = Replace(HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath, "~/", "")
            Dim StrPageUrl As String = StrReverse(Mid(StrReverse(StrAbsoluteURL), 1, InStr(StrReverse(StrAbsoluteURL), "/") - 1))
            Session("BackPage") = StrPageUrl

        End If
    End Sub

    Private Sub fnFillList()
        Try
            Dim StrQry As New StringBuilder
            Dim StrTTValue As String = String.Empty
            If RdPUR.Checked = True Then StrTTValue = RdPUR.Value
            If RdSL.Checked = True Then StrTTValue = RdSL.Value
            If RdRCPT.Checked = True Then StrTTValue = RdRCPT.Value
            If RdPAY.Checked = True Then StrTTValue = RdPAY.Value
            If RdJE.Checked = True Then StrTTValue = RdJE.Value
            StrQry.Append(" select idTransaction, concat(TransactionId,'-', TransactionType) 'Tran Type and No ', TransactionApprovalStatus, replace(replace(ImageName,'Upload/',''),'Transaction/','') as 'ImageName', ")
            StrQry.Append(" UploadTransactionDateTime 'Transaction Date Time', ")
            StrQry.Append(" replace(Convert(nvarchar(20), UploadDate, 106),' ','-') 'Upload Date' ")
            If CInt(Session("IDRole").ToString) <= 2 Then
                StrQry.Append(" , UploadedBy ")
            End If
            StrQry.Append(" from Vw_TransactionList ")
            StrQry.Append(" where 1=1 ")
            If CInt(Session("IDRole").ToString) > 2 Then
                StrQry.Append(" and idCompany = " & Session("IDClient").ToString & "")
                StrQry.Append(" and IdUser  =  " & Session("IDUser").ToString)
            Else
                If DDLCompany.SelectedValue > 0 Then
                    StrQry.Append(" and idCompany = " & DDLCompany.SelectedValue & "")
                End If
                If TxtTransactionDtFrm.Text.Trim.Length > 0 Then
                    Dim strFrmDt As String = TxtTransactionDtFrm.Text.Trim & " 00:00:00"
                    Dim strToDt As String = TxtTransactionDtTo.Text.Trim & " 23:59:59"
                    StrQry.Append(" and CAST(UploadTransactionDateTime AS DATEtime) between CAST('" & strFrmDt & "' AS DATEtime) and CAST('" & strToDt & "' AS DATEtime) or '" & txtBillNo.Text.Trim() & "' or idTransactiontype='" & StrTTValue & "';")
                ElseIf txtBillNo.Text.Trim() <> String.Empty Then

                    StrQry.Append(" and ( transactionId='" & txtBillNo.Text.Trim() & "');")
                ElseIf StrTTValue <> String.Empty Then
                    StrQry.Append(" and ( idTransactiontype='" & StrTTValue & "');")
                End If
            End If
            Dim clsObj As New clsData
            Dim dtstTransactionType As New DataSet
            dtstTransactionType = clsObj.fnDataSet(StrQry.ToString, 1, 1)
            If Not IsNothing(dtstTransactionType) Then
                If dtstTransactionType.Tables.Count > 0 Then
                    If dtstTransactionType.Tables(0).Rows.Count > 0 Then
                        Dim ObjClsCmn As New ClsCommonFunction
                        LblSearchData.Text = ObjClsCmn.DataTableToHTMLTable(dtstTransactionType.Tables(0), "DataList", "table table-striped table-bordered", "0", "100%", True)
                        ObjClsCmn = Nothing
                    Else
                        ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('No Data found');</script>")
                    End If
                Else
                    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('No Data found');</script>")
                End If
            End If
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Please check for internet connection / The Site is Under Maintenance');</script>")
        End Try
    End Sub

#Region "User Defined Functions"
    'Fill Drop down List
    Private Sub fnFillDDL()
        Dim clsodbcObj As New clsData
        Dim StrDDLListQry As String = " select '0' id,'Select' data1 Union All select ClientId as id, ClientShortCode + ' - ' + TxtCompanyName as Data1 from Vw_ClientMaster  where RecordStatus = 0  "
        If Session("IDRole").ToString = 2 Or Session("IDRole").ToString = 3 Then
            StrDDLListQry = StrDDLListQry & " and ClientId = " & Session("IDClient")
        End If
        clsodbcObj.fnDropDownFill(StrDDLListQry, DDLCompany, "data1", "id")
        clsodbcObj = Nothing
        If Session("IDRole").ToString = 2 Or Session("IDRole").ToString = 3 Then
            If DDLCompany.Items.Count > 0 Then
                DDLCompany.SelectedValue = Session("IDClient")
            End If
        End If
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        fnFillList()
    End Sub
#End Region
End Class
