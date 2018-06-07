Imports System.Data

Partial Class AspxPages_AspxUnlockUser
    Inherits System.Web.UI.Page
    Dim StrKeyValue As String() = Nothing
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsNothing(Request.QueryString("SearchKey")) Then
            Dim objCls As New clsData
            Dim StrSQL As String = "Select pagesetting from tbl_menudetail where idmenudtl = 16"
            StrKeyValue = objCls.fnStrRetrive(StrSQL, 1).Split("|")
            objCls = Nothing
            StrKeyValue(0) = Replace(StrKeyValue(0), "[login Time]", Session("SQLDTFormat"))
            If StrKeyValue(0) <> String.Empty Then ViewState("strsql") = StrKeyValue(0) Else Exit Sub
            fnFillList()
        Else
            Dim objCls As New clsData
            Session("SearchKey") = Request.QueryString("SearchKey")
            Dim StrSQL As String = "update tbl_user set loginstatus=1,loginstatusdatetime=null where iduser= " & Session("SearchKey")
            If objCls.fnIUD(StrSQL.ToString, 1, "NonQuery") = True Then
                objCls = Nothing
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('User UnLocked Updated');</script>")
                fnFillList()
            End If
        End If
       
    End Sub

    Private Sub fnFillList()
        If Not IsNothing(ViewState("strsql")) Then
            Dim objCls As New clsData
            Dim dtsGrd As New DataSet
            dtsGrd = objCls.fnDataSet(ViewState("strsql"), 1, 1)
            If Not IsNothing(dtsGrd) Then
                If dtsGrd.Tables.Count > 0 Then
                    If dtsGrd.Tables(0).Rows.Count > 0 Then
                        Dim ObjClsCmn As New ClsCommonFunction
                        LblSearchData.Text = ObjClsCmn.DataTableToHTMLTable(dtsGrd.Tables(0), "DataList", "table table-striped table-bordered", "0", "100%", False)
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
