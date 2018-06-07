Imports System.Data

Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not IsNothing(Session("IDUser")) Then
            If Session("IDUser").ToString.Length > 0 Then
                Dim StrUpdateLoginStatus As String = "Update tbl_user set LoginStatus = 1, LoginStatusDateTime = NULL where idUser = " & Session("IDUser").ToString
                Dim StrExp As String = String.Empty
                Dim clsObj As New clsData
                clsObj.fnExecute(StrUpdateLoginStatus, StrExp, 0)
                clsObj = Nothing
                If StrExp.Length > 0 Then
                    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Please check for internet connection / The Site is Under Maintenance');</script>")
                    Exit Sub
                End If
                Session.Clear()
                Session.RemoveAll()
                Session.Abandon()
            End If
        End If
        Response.Redirect("AspxPortalLogin.aspx", False)
    End Sub
End Class