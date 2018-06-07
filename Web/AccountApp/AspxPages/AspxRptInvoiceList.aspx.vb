Imports System.Data

Partial Class AspxPages_AspxRptInvoiceList
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
            StrQry.Append(" select [Date], [INVOICE NO], [GST NO], [PARTY NAME], [NET AMOUNT], [IGST], [CGST], [SGST], [TOTAL] ")
            StrQry.Append(" from Vw_InvoiceList  ")
            StrQry.Append(" Where 1 = 1 ")
            If Session("IDClient").ToString > "1" Then
                StrQry.Append(" and idclient = " & Session("IDClient").ToString)
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
End Class
