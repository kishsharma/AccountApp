
Partial Class AspxPages_AspxRejectedImage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            HdnIdentity.Value = Request.QueryString("SearchKey")
            imgprvw.Src = "../Upload/Transaction/" & Replace(HdnIdentity.Value, "^", ".")
        End If
    End Sub

End Class
