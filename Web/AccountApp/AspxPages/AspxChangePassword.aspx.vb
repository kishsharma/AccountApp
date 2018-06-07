
Partial Class AspxPages_AspxChangePassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            HdnIdentity.Value = Session("IDUser").ToString
            HdnCurrentPassword.Value = Session("UserPassword").ToString
        End If
    End Sub

    Protected Sub btnChangePassword_Click(sender As Object, e As System.EventArgs) Handles btnChangePassword.Click
        Dim StrLngId As String = String.Empty
        Dim clsobj As New clsData
        Dim Enc As New RijndaelSimple
        Dim EncOldPassword As String = "" 'Enc.Encrypt(txtOldPassword.Value)
        Dim EncNewPassword As String = Enc.Encrypt(txtPassword.Value)
        Dim EncConfiPassword As String = Enc.Encrypt(txtConfirmPassword.Value)
        Enc = Nothing
        Dim Names As String() = {"idUser", "UserPassword", "Newpassword", "ConfirmPassword"}
        Dim Values As String() = {HdnIdentity.Value, EncOldPassword, EncNewPassword, EncConfiPassword}
        StrLngId = DirectCast(clsobj.fnInsertUpdate("P_UpdatePassword", Names, Values), String)
        If (StrLngId.Contains("Successfully")) Then
            Session("UserPassword") = txtPassword.Value.Trim
            HdnIdentity.Value = String.Empty.ToString()
            If CInt(Session("IDRole").ToString) <= 2 Then
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Password Updatede Successfully'); location.href='AspxSearchBlock.aspx?idMenu=8';</script>")
            Else
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Password Updatede Successfully'); location.href='AspxLandingPage.aspx';</script>")
            End If
        Else
            ''ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Current Password Not Correct'); location.href='AspxSearchBlock.aspx?idMenu=8'</script>")
        End If
    End Sub
End Class
