Imports System.Data

Partial Class AspxPages_AspxTransactionImageList
    Inherits System.Web.UI.Page
    Dim StrKeyValue As String() = Nothing

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            If IsNothing(Session("IDUser")) Then
                Response.Redirect("~/Default.aspx", False)
                Exit Sub
            End If
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
            If CInt(Session("IDUser").ToString) > "2" Then
                StrKeyValue(0) = Replace(StrKeyValue(0), "[IDUser]", Session("IDUser"))
            Else
                StrKeyValue(0) = Replace(StrKeyValue(0), "and tbl_TaskAllotment.idUser = [IDUser]", "")
            End If
            If StrKeyValue(0) <> String.Empty Then ViewState("strsql") = StrKeyValue(0) Else Exit Sub
            If StrKeyValue(1) <> String.Empty Then ViewState("StrPageName") = StrKeyValue(1) Else Exit Sub
            Dim StrAbsoluteURL As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim StrRelativeURL As String = Replace(HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath, "~/", "")
            Dim StrPageUrl As String = StrReverse(Mid(StrReverse(StrAbsoluteURL), 1, InStr(StrReverse(StrAbsoluteURL), "/") - 1))

            Session("BackPage") = StrPageUrl
            fnFillList()
        End If
    End Sub

    Private Sub fnFillList()
        Dim StrSQL As New StringBuilder
        If CInt(Session("IDRole").ToString) <= 2 Then
            StrSQL.Append(" Select ")
            StrSQL.Append(" concat('<a href=""AspxTransactionCompanyImageList.aspx?Companyid=',VwFillImageScreen.idCompany,'&Type=D&UserID=',tbl_TaskAllotment.idUser,'&DataForDate=',convert(varchar(20), tbl_TaskAllotment.AllotmentDate, 106),'"">', convert(varchar(20), tbl_TaskAllotment.AllotmentDate, 106) ,'</a>')  as 'Allotment date', ")
            StrSQL.Append(" concat('<a href=""AspxTransactionCompanyImageList.aspx?Companyid=',VwFillImageScreen.idCompany,'&Type=C&UserID=',tbl_TaskAllotment.idUser,'&DataForDate=',convert(varchar(20), tbl_TaskAllotment.AllotmentDate, 106),'"">', TxtCompanyName ,'</a>') As 'Client Name',")
            StrSQL.Append(" concat('<a href=""AspxTransactionCompanyImageList.aspx?Companyid=',VwFillImageScreen.idCompany,'&Type=U&UserID=',tbl_TaskAllotment.idUser,'&DataForDate=',convert(varchar(20), tbl_TaskAllotment.AllotmentDate, 106),'"">', tbl_user.username ,'</a>') as 'Assigned to',")
            StrSQL.Append(" count(VwFillImageScreen.IdUpTransDetail) as 'Number of Bills' ")
            StrSQL.Append(" from tbl_TaskAllotment ")
            StrSQL.Append(" Left outer join VwFillImageScreen on VwFillImageScreen.IdUpTransDetail = tbl_TaskAllotment.IdTransactionimageId ")
            StrSQL.Append(" Left outer join tbl_defination TT on TT.idDefination = VwFillImageScreen.idTransactionType ")
            StrSQL.Append(" Left outer join Vw_ClientMaster on ClientId = VwFillImageScreen.idCompany ")
            StrSQL.Append(" Left Outer Join tbl_user on tbl_user.idUser = tbl_TaskAllotment.idUser ")
            StrSQL.Append(" where 1=1  ")
            ''StrSQL.Append(" and VwFillImageScreen.idCompany = 2 and VwFillImageScreen.transactioncompleted is null ")
            StrSQL.Append(" and (VwFillImageScreen.transactioncompleted is null  or VwFillImageScreen.ReviewStatus = 2) ")
            ''StrSQL.Append(" and idTransactionType =  6  ")
            StrSQL.Append(" and Transactionstatus = 0 and isnull(tbl_TaskAllotment.idRejectedAllotment,0) =0 ")
            If CInt(Session("IDUser").ToString) > 2 Then
                If CInt(Session("IDRole").ToString) > 2 Then
                    StrSQL.Append(" and tbl_TaskAllotment.idUser =  " & Session("IDUser").ToString)
                End If
            End If
            StrSQL.Append(" group by convert(varchar(20), tbl_TaskAllotment.AllotmentDate, 106), TxtCompanyName, VwFillImageScreen.idCompany, tbl_user.username,tbl_TaskAllotment.idUser ")
        Else
            StrSQL.Append(" Select convert(varchar(20), tbl_TaskAllotment.AllotmentDate, 106) as 'Allotment date', ")
            StrSQL.Append(" concat('<a href=""AspxTransactionCompanyImageList.aspx?Companyid=',VwFillImageScreen.idCompany,'&Type=C&UserID=',tbl_TaskAllotment.idUser,'&DataForDate=',convert(varchar(20), tbl_TaskAllotment.AllotmentDate, 106),'"">', TxtCompanyName ,'</a>') As 'Client Name',")
            StrSQL.Append(" count(VwFillImageScreen.IdUpTransDetail) as 'Number of Bills', tbl_user.username as 'Assigned to' ")
            StrSQL.Append(" from tbl_TaskAllotment ")
            StrSQL.Append(" Left outer join VwFillImageScreen on VwFillImageScreen.IdUpTransDetail = tbl_TaskAllotment.IdTransactionimageId ")
            StrSQL.Append(" Left outer join tbl_defination TT on TT.idDefination = VwFillImageScreen.idTransactionType ")
            StrSQL.Append(" Left outer join Vw_ClientMaster on ClientId = VwFillImageScreen.idCompany ")
            StrSQL.Append(" Left Outer Join tbl_user on tbl_user.idUser = tbl_TaskAllotment.idUser ")
            StrSQL.Append(" where 1=1 ")
            ''StrSQL.Append(" and VwFillImageScreen.idCompany = 2  and VwFillImageScreen.transactioncompleted is null ")
            StrSQL.Append(" and (VwFillImageScreen.transactioncompleted is null  or VwFillImageScreen.ReviewStatus = 2) ")
            ''StrSQL.Append(" and idTransactionType =  6 ")
            StrSQL.Append("  and Transactionstatus = 0 and isnull(tbl_TaskAllotment.idRejectedAllotment,0) =0 ")
            If CInt(Session("IDUser").ToString) > 2 Then
                StrSQL.Append(" and tbl_TaskAllotment.idUser =  " & Session("IDUser").ToString)
            End If
            StrSQL.Append(" group by convert(varchar(20), tbl_TaskAllotment.AllotmentDate, 106), TxtCompanyName, VwFillImageScreen.idCompany, tbl_user.username,tbl_TaskAllotment.idUser ")
        End If
        If Not IsNothing(StrSQL.ToString) Then
            Dim objCls As New clsData
            Dim dtsGrd As New DataSet
            dtsGrd = objCls.fnDataSet(StrSQL.ToString, 1, 1)
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
    End Sub
End Class
