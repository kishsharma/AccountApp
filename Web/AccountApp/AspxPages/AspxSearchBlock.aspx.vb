Imports System.Data

Partial Class AspxPages_AspxSearchBlock
    Inherits System.Web.UI.Page
    Dim lngid As Long
    Dim blnupdflg As Boolean = True
    Dim StrKeyValue As String() = Nothing


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Session("IDClient")) = False Then
            Response.Redirect("AspxLostSessions.aspx", False)
            Exit Sub
        End If
        If IsNothing(Request.QueryString("idMenu")) Then
            Exit Sub
        End If
        If Not IsPostBack Then
            Dim objCls As New clsData
            Session("IdMenu") = Request.QueryString("idMenu")
            Dim StrSQL As String = "Select pagesetting from tbl_menudetail where idmenudtl = " & Request.QueryString("idMenu")
            StrKeyValue = objCls.fnStrRetrive(StrSQL, 1).Split("|")
            objCls = Nothing
            objCls = New clsData
            StrSQL = "Select case when [Description] ='Company' then '" & Session("ProjectType") & "' else [Description] end [Description] from tbl_menudetail where idmenudtl = " & Request.QueryString("idMenu")
            LblScreenName.Value = objCls.fnStrRetrive(StrSQL, 1)
            Session("ScreenName") = LblScreenName.Value
            DirectCast(Page.Master.FindControl("lblSiteMap"), Label).Text = "Screen: Search - " & LblScreenName.Value
            objCls = Nothing
            StrKeyValue(0) = Replace(StrKeyValue(0), "[dateformat]", Session("SQLDTFormat"))
            StrKeyValue(0) = Replace(StrKeyValue(0), "[IDUser]", Session("IDUser"))
            If StrKeyValue(0) <> String.Empty Then ViewState("strsql") = StrKeyValue(0) Else Exit Sub
            If StrKeyValue(1) <> String.Empty Then ViewState("StrPageName") = StrKeyValue(1) Else Exit Sub
            If StrKeyValue(2) <> String.Empty Then ViewState("TableName") = StrKeyValue(2) Else ViewState("TableName") = "" : BtnInactiveRecord.Enabled = False
            If StrKeyValue(3) <> String.Empty Then ViewState("IdRecordColumn") = StrKeyValue(3) Else ViewState("TableName") = "" : BtnDeleteRecord.Enabled = False
            Dim StrAbsoluteURL As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim StrRelativeURL As String = Replace(HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath, "~/", "")
            Dim StrPageUrl As String = StrReverse(Mid(StrReverse(StrAbsoluteURL), 1, InStr(StrReverse(StrAbsoluteURL), "/") - 1))
            Session("BackPage") = StrPageUrl
            If InStr(ViewState("strsql").ToString, ";") > 0 Then
                ViewState("strsql") = Replace(ViewState("strsql").ToString, ";", "")
            End If
            If Session("IDClient").ToString > "1" Then
                If Session("IdMenu") = 7 Then
                    ViewState("strsql") = ViewState("strsql").ToString & " and tbl_company.idcompany = " & Session("IDClient")
                Else
                    ViewState("strsql") = ViewState("strsql").ToString & " and tbl_user.idcompany = " & Session("IDClient")
                End If
            End If
            ViewState("strsql") = ViewState("strsql") & ";"
            fnFillBlock()
        End If
    End Sub


    Private Sub fnFillBlock()
        Dim StrQryBlockList As New StringBuilder
        'StrQryBlockList.Append("Select idMainMenu IdBlock, [Description] as BlockDetail, ImagePath as BlockImage from tbl_MainMenu")
        StrQryBlockList.Append(ViewState("strsql"))
        Dim clsObj As New clsData
        Dim dtstSearchDtls As New DataSet
        dtstSearchDtls = clsObj.fnDataSet(StrQryBlockList.ToString, 1, 1)
        If Not IsNothing(dtstSearchDtls) Then
            If dtstSearchDtls.Tables.Count > 0 Then
                If dtstSearchDtls.Tables(0).Rows.Count > 0 Then
                    RptrBlock.DataSource = dtstSearchDtls.Tables(0)
                    RptrBlock.DataBind()
                    clsObj = Nothing
                Else

                End If
            Else

            End If
        Else
        End If
        clsObj = Nothing

    End Sub
    Protected Sub BtnEditRecord_Click(sender As Object, e As System.EventArgs) Handles BtnEditRecord.Click
        Dim StrURL As String = ViewState("StrPageName") & "?idUpdate=" & HdnIdBlock.Value
        Response.Redirect(StrURL)
    End Sub

    Protected Sub BtnAddHREF_ServerClick(sender As Object, e As System.EventArgs) Handles BtnAddHREF.ServerClick
        If Not IsNothing(ViewState("StrPageName")) Then
            Response.Redirect(ViewState("StrPageName"))
        End If
    End Sub

    Protected Sub BtnInactiveRecord_Click(sender As Object, e As System.EventArgs) Handles BtnInactiveRecord.Click
        Dim strTableName As String = ViewState("TableName")
        Dim StrIdRecordColumn As String = ViewState("IdRecordColumn")
        Dim StrQry As New StringBuilder
        If HdnIdBlockActiveType.Value = "Make In-Active" Then
            StrQry.Append(" Update " & strTableName & " set RecordStatus = 1 where " & StrIdRecordColumn & " = " & HdnIdBlock.Value)
        Else
            StrQry.Append(" Update " & strTableName & " set RecordStatus = 0 where " & StrIdRecordColumn & " = " & HdnIdBlock.Value)
        End If
        Dim clsobj As New clsData
        clsobj.fnIUD(StrQry.ToString, 1, "NonQuery")
        clsobj = Nothing
        fnFillBlock()
    End Sub

    Protected Sub BtnDeleteRecord_Click(sender As Object, e As System.EventArgs) Handles BtnDeleteRecord.Click
        Dim strTableName As String = ViewState("TableName")
        Dim StrIdRecordColumn As String = ViewState("IdRecordColumn")
        Dim StrQry As New StringBuilder
        StrQry.Append(" Update " & strTableName & " set RecordStatus = 2 where " & StrIdRecordColumn & " = " & HdnIdBlock.Value)
        Dim clsobj As New clsData
        clsobj.fnIUD(StrQry.ToString, 1, "NonQuery")
        clsobj = Nothing
        fnFillBlock()
        ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Deleted Successfully'); location.href='" & Session("BackPage").ToString() & "';</script>")
    End Sub
End Class
