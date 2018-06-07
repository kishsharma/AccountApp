Imports System.Data
Imports System.IO
Imports System.Net
Imports System.Runtime.Serialization.Json

Partial Class AspxPortalLogin
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            'ProjectFor.HRef = System.Configuration.ConfigurationManager.AppSettings("SpnVendorName").ToString()
            'SpnVendorName.InnerHtml = System.Configuration.ConfigurationManager.AppSettings("VendorSite").ToString()
            'SpnNameFor.InnerHtml = System.Configuration.ConfigurationManager.AppSettings("SpnVendorName").ToString()
            ' Page.Title = System.Configuration.ConfigurationManager.AppSettings("PageTitle").ToString()
            txtUserName.Focus()
            captcha.Text = fnGenerateNewPassword()
            txtCaptcha.Text = captcha.Text
            captcha.Enabled = False
            Session("IDUser") = ""
        End If
    End Sub

    'fnGenerateNewPassword() returns a 6 digit string 
    Private Function fnGenerateNewPassword() As String
        ' Below code describes how to create random numbers.some of the digits and letters
        ' are ommited because they look same like "i","o","1","0","I","O".
        Dim NoofChar As String = "a,b,c,d,e,f,g,h,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z,"
        NoofChar += "A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z,"
        NoofChar += "2,3,4,5,6,7,8,9"
        Dim sep() As Char = {","c}
        Dim arr() As String = NoofChar.Split(sep)
        Dim passwordString As String = ""
        Dim temp As String
        Dim rand As Random = New Random()
        Dim i As Integer
        For i = 0 To 6 - 1 Step i + 1
            temp = arr(rand.Next(0, arr.Length))
            passwordString += temp
        Next
        Return passwordString
    End Function

    Private Sub imgGenerateSrv_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgGenerateSrv.ServerClick
        captcha.Text = fnGenerateNewPassword()
        txtCaptcha.Text = String.Empty
    End Sub

    'Protected Sub BtnLogin_ServerClick(sender As Object, e As System.EventArgs) Handles btnLogin.ServerClick
    Protected Sub BtnLogin_ServerClick(sender As Object, e As System.EventArgs) Handles btnLogin.Click
        If txtUserName.Value.Trim.ToString <> String.Empty And txtPassword.Value.Trim.ToString <> String.Empty Then
            Dim proxy As New WebClient
            Dim StrbaseUrl As String = System.Configuration.ConfigurationManager.AppSettings("APIURL").ToString()
            Dim StrURL As String = StrbaseUrl & "SibzAPILoginFromWeb/{0}/{1}"
            StrURL = String.Format(StrURL, txtUserName.Value.Trim, txtPassword.Value.Trim)
            'Try
            Dim data As Byte() = proxy.DownloadData(StrURL)
                Dim stream As IO.Stream = New MemoryStream(data)
                Dim StrJSONTxt As String = String.Empty
                'Dim obj As New DataContractJsonSerializer(GetType(String))
                'StrJSONTxt = TryCast(obj.ReadObject(Stream), String)
                Dim sr = New StreamReader(stream)
                Dim vr = sr.ReadToEnd()
                StrJSONTxt = vr

                If StrJSONTxt = String.Empty Then
                    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Please contact Web Administrator / Confirm your Crendentials');</script>")
                    'ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", "Please contact Web Administrator / Confirm your Crendentials")
                Else
                    Dim DtUser As New DataTable
                    Dim clscmn As New ClsCommonFunction
                    DtUser = clscmn.fnConvertJSONToDataTable(StrJSONTxt)
                    clscmn = Nothing
                    If Not IsNothing(DtUser) Then
                        If DtUser.Rows.Count > 0 Then
                            Select Case DtUser.Rows(0)("ResponceStatus")
                                Case 1
                                    Session("IDUser") = DtUser.Rows(0)("IDUser")
                                    Session("UserName") = DtUser.Rows(0)("UserName")
                                    Session("UserPassword") = DtUser.Rows(0)("UserPassword")
                                    Session("UserFullName") = DtUser.Rows(0)("UserFullName")
                                    Session("ContactNo") = DtUser.Rows(0)("ContactNo")
                                    Session("EmailId") = DtUser.Rows(0)("EmailId")
                                    Session("UserAddress") = DtUser.Rows(0)("UserAddress")
                                    Session("IDRole") = DtUser.Rows(0)("IDRole")
                                Session("ImageName") = DtUser.Rows(0)("ImageName")
                                Session("IDClient") = 0
                                If DtUser.Rows(0)("IDRole").ToString > 1 Then
                                    Session("IDClient") = DtUser.Rows(0)("IDClient")
                                End If
                                Session("IDCompany") = DtUser.Rows(0)("IDCompany")
                                Session("Company") = DtUser.Rows(0)("Company")
                                Session("CompShorCode") = DtUser.Rows(0)("CompShorCode")
                                Session("RoleCode") = DtUser.Rows(0)("RoleCode")
                                Session("RoleDesc") = DtUser.Rows(0)("RoleDesc")
                                    Session("SQLDTFormat") = System.Configuration.ConfigurationManager.AppSettings("DateFormat").ToString()
                                    If DtUser.Rows(0)("IDRole") > 1 Then
                                        'Assing IDMainMenu from Role Table
                                        Session("IDMainMenu") = String.Empty
                                    Else
                                        Session("IDMainMenu") = String.Empty
                                    End If
                                    Session("ProjectName") = System.Configuration.ConfigurationManager.AppSettings("ProjectName").ToString()
                                    Session("EMailSendId") = System.Configuration.ConfigurationManager.AppSettings("EMailSendId").ToString()
                                    Response.Redirect("AspxPages/AspxLandingPage.aspx", False)
                                Case 2
                                    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('" & "case : 2 - " & DtUser.Rows(0)("ResponceStatusDetails") & "');</script>")
                                Case Else
                                    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('case : 0 - Please contact Web Administrator / Confirm your Crendentials');</script>")
                            End Select
                        Else
                            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('No User Data Found : Please contact Web Administrator / Confirm your Crendentials');</script>")
                        End If
                    Else
                        ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Not able to Connect Users Object, Please contact Web Administrator / Confirm your Crendentials');</script>")
                    End If
                End If
            'Catch ex As Exception
            '    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('" & ex.Message & "' );</script>")
            'End Try

        End If
    End Sub


    Protected Sub BtnSendPassword_Click(sender As Object, e As System.EventArgs) Handles BtnSendPassword.Click
        If txtFgpdEmail.Text.Trim = String.Empty Then
            If txtUserName.Value.Trim <> String.Empty Then
                txtPassword.Focus()
            End If
            If txtPassword.Value.Trim <> String.Empty Then
                btnLogin.Focus()
                BtnLogin_ServerClick(sender, e)
                Exit Sub
            End If
            Exit Sub
        End If
        Dim proxy As New WebClient
        Dim StrbaseUrl As String = System.Configuration.ConfigurationManager.AppSettings("APIURL").ToString()
        Dim StrURL As String = StrbaseUrl & "SibzAPIPasswordReminder/{0}"
        StrURL = String.Format(StrURL, txtFgpdEmail.Text.Trim)
        Dim data As Byte() = proxy.DownloadData(StrURL)
        Dim stream As Stream = New MemoryStream(data)
        Dim strtxt As String = String.Empty
        Dim StrJSONTxt As String = String.Empty
        'Dim obj As New DataContractJsonSerializer(GetType(String))
        'StrJSONTxt = TryCast(obj.ReadObject(Stream), String)
        Dim sr = New StreamReader(stream)
        Dim vr = sr.ReadToEnd()
        StrJSONTxt = vr

        If StrJSONTxt = String.Empty Then
            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Please contact Web Administrator / Confirm your Crendentials');</script>")
        Else
            Dim DtUser As New DataTable
            Dim clscmn As New ClsCommonFunction
            DtUser = clscmn.fnConvertJSONToDataTable(StrJSONTxt)
            clscmn = Nothing
            If Not IsNothing(DtUser) Then
                If DtUser.Rows.Count > 0 Then
                    If DtUser.Rows(0)("ResponceStatus") = 1 Then
                        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", DtUser.Rows(0)("Remark"))
                    Else
                        ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Please contact Web Administrator / Confirm your Crendentials');</script>")
                    End If
                Else
                    'ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", "Please contact Web Administrator / Confirm your Crendentials")
                    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Please contact Web Administrator / Confirm your Crendentials');</script>")
                End If
            Else
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Please contact Web Administrator / Confirm your Crendentials');</script>")
            End If
        End If
    End Sub
End Class
