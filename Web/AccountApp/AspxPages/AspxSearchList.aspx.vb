Imports System.Data

Partial Class AspxPages_AspxSearchList
    Inherits System.Web.UI.Page
    Dim lngid As Long
    Dim blnupdflg As Boolean = True
    Dim StrKeyValue As String() = Nothing

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Session("IDCOMPANY")) = False Then
            Response.Redirect("AspxLostSessions.aspx", False)
            Exit Sub
        End If
        If IsNothing(Request.QueryString("idMenu")) Then
            Exit Sub
        End If
        If Not IsPostBack Then
            Session("IdMenu") = Request.QueryString("idMenu")
            fnFillDDL()
            If Session("IDRole").ToString = 2 Or Session("IDRole").ToString = 3 Then
                Companylist.Visible = False
            End If


            Dim StrHideAddNewForMenuId As String = System.Configuration.ConfigurationManager.AppSettings("HideAddNewForMenuId").ToString()
            BtnAddHREF.Attributes.Add("style", "display:block")
            If StrHideAddNewForMenuId.Contains(Session("IdMenu").ToString) Then
                BtnAddHREF.Attributes.Add("style", "display:none")
            End If
            'idMenu=11

            Dim objCls As New clsData

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
            If CInt(Session("IDUser").ToString) > 2 Then
                StrKeyValue(0) = Replace(StrKeyValue(0), "[IDUser]", Session("IDUser"))
            Else
                StrKeyValue(0) = Replace(StrKeyValue(0), "and tbl_TaskAllotment.idUser = [IDUser]", "")
            End If
            If StrKeyValue(0) <> String.Empty Then ViewState("strsql") = StrKeyValue(0) Else Exit Sub
            If StrKeyValue.Length = 2 Then
                If StrKeyValue(1) <> String.Empty Then ViewState("StrPageName") = StrKeyValue(1) Else Exit Sub
            Else
                ViewState("StrPageName") = "#"
            End If

            Dim StrAbsoluteURL As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim StrRelativeURL As String = Replace(HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath, "~/", "")
            Dim StrPageUrl As String = StrReverse(Mid(StrReverse(StrAbsoluteURL), 1, InStr(StrReverse(StrAbsoluteURL), "/") - 1))
            Session("BackPage") = StrPageUrl


            ViewState("strsql") = ViewState("strsql") & ";"

        End If
    End Sub
    Private Sub fnFillList()


        'If DDLCompany.SelectedValue > 0 Then
        'ViewState("strsql") = ViewState("strsql").ToString & " and tbl_UPloadTransaction.idcompany = " & DDLCompany.SelectedValue
        'If InStr(ViewState("strsql").ToString, ";") > 0 Then
        '    ViewState("strsql") = Replace(ViewState("strsql").ToString, ";", "")
        'End If
        Dim StrSQL As String = String.Empty
        StrSQL = ViewState("strsql").ToString()

        If StrSQL.Contains("[IDCompany]") = True Then
            If Session("IDCOMPANY").ToString > "1" Then
                If DDLCompany.SelectedValue > 0 Then
                    StrSQL = Replace(StrSQL, "[IDCompany]", DDLCompany.SelectedValue)
                Else
                    StrSQL = Replace(StrSQL, "[IDCompany]", Session("IDCOMPANY"))
                End If
            Else
                If DDLCompany.SelectedValue > 0 Then
                    StrSQL = Replace(StrSQL, "[IDCompany]", DDLCompany.SelectedValue)
                Else
                    StrSQL = Replace(UCase(StrSQL), UCase("and idclient = [IDCompany]"), "")
                    StrSQL = Replace(UCase(StrSQL), UCase("And VwFillImageScreen.idcompany = [IDCompany]"), "")
                End If
            End If
            ViewState("strsql") = StrSQL
        Else

        End If
        If Not IsNothing(ViewState("strsql")) Then
            Dim objCls As New clsData
            Dim dtsGrd As New DataSet
            dtsGrd = objCls.fnDataSet(ViewState("strsql"), 1, 1)
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
            objCls = Nothing
        End If
        'End If
    End Sub
    'Protected Sub BtnEditRecord_Click(sender As Object, e As System.EventArgs) Handles BtnEditRecord.Click
    '    Dim StrURL As String = ViewState("StrPageName") & "?idUpdate=" & HdnIdBlock.Value
    '    Response.Redirect(StrURL)
    'End Sub

    Protected Sub BtnSearch_ServerClick(sender As Object, e As System.EventArgs) Handles BtnSearch.ServerClick
        Dim objCls As New clsData

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
        If CInt(Session("IDUser").ToString) > 2 Then
            StrKeyValue(0) = Replace(StrKeyValue(0), "[IDUser]", Session("IDUser"))
        Else
            StrKeyValue(0) = Replace(StrKeyValue(0), "and tbl_TaskAllotment.idUser = [IDUser]", "")
        End If
        If StrKeyValue(0) <> String.Empty Then ViewState("strsql") = StrKeyValue(0) Else Exit Sub
        If StrKeyValue.Length = 2 Then
            If StrKeyValue(1) <> String.Empty Then ViewState("StrPageName") = StrKeyValue(1) Else Exit Sub
        Else
            ViewState("StrPageName") = "#"
        End If

        Dim StrAbsoluteURL As String = HttpContext.Current.Request.Url.AbsoluteUri
        Dim StrRelativeURL As String = Replace(HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath, "~/", "")
        Dim StrPageUrl As String = StrReverse(Mid(StrReverse(StrAbsoluteURL), 1, InStr(StrReverse(StrAbsoluteURL), "/") - 1))
        Session("BackPage") = StrPageUrl


        ViewState("strsql") = ViewState("strsql") & ";"
        fnFillList()
    End Sub

    Protected Sub BtnAddHREF_ServerClick(sender As Object, e As System.EventArgs) Handles BtnAddHREF.ServerClick
        If DDLCompany.SelectedValue <> "0" Then
            Session("IDClient") = DDLCompany.SelectedValue
        End If
        If Not IsNothing(ViewState("StrPageName")) Then
            Response.Redirect(ViewState("StrPageName"))
        End If


    End Sub

#Region "User Defined Functions"
    'Fill Drop down List
    Private Sub fnFillDDL()
        Dim clsodbcObj As New clsData
        Dim StrDDLListQry As String = " select '0' id,'Select' data1 Union All select ClientId as id, ClientShortCode + ' - ' + TxtCompanyName as Data1 from Vw_ClientMaster  where RecordStatus = 0  "
        If Session("IDRole").ToString = 2 Or Session("IDRole").ToString = 3 Then
            StrDDLListQry = StrDDLListQry & " and ClientId = " & Session("IDCompany")
        End If
        clsodbcObj.fnDropDownFill(StrDDLListQry, DDLCompany, "data1", "id")
    End Sub
#End Region
End Class
