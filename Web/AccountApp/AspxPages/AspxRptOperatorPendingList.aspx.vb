Imports System.Data

Partial Class AspxPages_AspxRptOperatorPendingList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            If IsNothing(Session("IDUser")) Then
                Response.Redirect("~/Default.aspx", False)
                Exit Sub
            End If
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
            fnFillList()
        End If
    End Sub
    Private Sub fnFillList()
        Try
            Dim StrQry As New StringBuilder
            StrQry.Append(" select tbl_user.Username, tbl_user.UserFullName, dbo.fnCountAssingTask(tbl_taskAllotment.iduser) as 'Assign Task', dbo.fnCountCompletedTask(tbl_taskAllotment.iduser) as 'Completed Task', dbo.fnCountPendingTask(tbl_taskAllotment.iduser)  as 'Pending Task' ")
            ''StrQry.Append(" dbo.fnCountRejectedTask(tbl_taskAllotment.iduser) as 'Task Rejected' ")
            StrQry.Append(" from tbl_taskAllotment  ")
            StrQry.Append(" Left outer join tbl_user on tbl_user.IdUser = tbl_taskAllotment.iduser and idRole = 4 ")
            StrQry.Append(" group by tbl_taskAllotment.iduser, tbl_user.Username, tbl_user.UserFullName ")
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
End Class
