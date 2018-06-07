Imports System.Security.Cryptography
Imports System.IO

Partial Class AspxPages_AspxActivation
    Inherits System.Web.UI.Page
    Public Shared StrUserMail As String = String.Empty

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'AccountApp/AspxActivation.aspx?name=hbQIXSL4PwPq2zzn8cgoPQycLr%2brAIUYFrNLg8CMBfkeBtoW7%2bwd30ApTUmG9LoL
        Dim clsObj As New clsData
        StrUserMail = Decrypt(HttpUtility.UrlDecode(Request.QueryString("name")))
        Dim StrSQL As String = String.Empty
        Dim StrSQLUpdate As String = String.Empty
        StrSQL = "select isnull(Emailvalidation,0) from tbl_User where EmailId='" & StrUserMail & "'"
        StrSQLUpdate = clsObj.fnStrRetrive(StrSQL, 1)
        StrSQL = Nothing
        If StrSQLUpdate = 1 Then
            Activation.Attributes.Add("style", "display:none")
            Deactivation.Attributes.Add("style", "display:block")
        Else
            StrSQLUpdate = "update tbl_User set Emailvalidation=1 where EmailId='" & StrUserMail & "'"
            If clsObj.fnIUD(StrSQLUpdate.ToString, 1, "NonQuery") = True Then
                clsObj = Nothing
                Activation.Attributes.Add("style", "display:block")
                Deactivation.Attributes.Add("style", "display:none")
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", "Please Try again or Contact System / Web Administrator")
            End If
           
        End If
    End Sub
    Private Function Decrypt(cipherText As String) As String
        Dim EncryptionKey As String = "MAKV2SPBNI99212"
        cipherText = cipherText.Replace(" ", "+")
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
             &H65, &H64, &H76, &H65, &H64, &H65, _
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function
End Class
