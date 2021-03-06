﻿Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Data
Imports System.Security.Cryptography

Partial Class AspxPages_AspxUser
    Inherits System.Web.UI.Page
    Public StrSelectedList As String = String.Empty
    Public Shared StrUserId As String = String.Empty
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'Dim StrActivationLink As String = String.Empty
        'Dim name As String = HttpUtility.UrlEncode(Encrypt("chetan@gmail.com"))
        'Dim myuri As New Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri)
        'Dim pathQuery As String = myuri.PathAndQuery
        'StrActivationLink = String.Format("/accountsapp/AspxPages/AspxActivation.aspx?name={0}", name)
        'Dim hostName As String = myuri.ToString().Replace(pathQuery, StrActivationLink)

        If Not IsPostBack Then
            fnFillDDL()
            If IsNothing(Request.QueryString("idUpdate")) Then
                DirectCast(Page.Master.FindControl("lblSiteMap"), Label).Text = "Screen: Add New - " & Session("ScreenName").ToString
            Else
                DirectCast(Page.Master.FindControl("lblSiteMap"), Label).Text = "Screen: Edit - " & Session("ScreenName").ToString
                HdnIdentity.Value = Request.QueryString("idUpdate")
                StrUserId = HdnIdentity.Value
                'Dim clsObj As New clsData
                'Dim StrOTP As String = String.Empty
                'Dim StrSQL As String = String.Empty
                'Dim StrSQLUpdate As String = String.Empty
                'StrSQL = "select isnull(OTPVerified,0) from tbl_User where idUser=" & StrUserId
                'StrSQLUpdate = clsObj.fnStrRetrive(StrSQL, 1)
                'StrSQL = Nothing
                'If StrSQLUpdate = 1 Then
                fnFillScreen()
                divchangepassword.Visible = True
                'Else
                '    divMain.Visible = False
                '    divchangepassword.Visible = False
                '    divImage.Visible = False
                '    divUserPay.Visible = False
                '    divOTP.Visible = True
                'End If
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If fnValidate() = False Then
            Exit Sub
        End If
        Dim StrInsQry As New StringBuilder
        Dim clsObj As New clsData
        Dim StrLngId As String = String.Empty
        Dim StrOTP As String = String.Empty
        If HdnIdentity.Value.Trim = "" Then
            Dim StrPassword As String = fnGenerateNewPassword()
            Dim Enc As New RijndaelSimple
            Dim EncPassword As String = Enc.Encrypt(StrPassword)
            Enc = Nothing
            Dim StrSQL As String = "SELECT ISNULL(OTPNo,0) from GenerateNumber"
            StrOTP = clsObj.fnStrRetrive(StrSQL, 1)

            StrInsQry.Append(" Insert into tbl_User(                       UserName,          UserPassword,                         UserFullName,                       ContactNo,                         EmailId,                         UserAddress,                             IDRole,                       ImageName,                          IDCompany,                    PaymentinRs,                       IncentiveinPrcnt,                        PenaltyinPrcnt,                 DeActivate, ClientId,OTP)   ")
            StrInsQry.Append("               Values('" & TxtUserName.Value.Trim & "', '" & EncPassword & "', '" & TxtUserFullName.Value.Trim & "', '" & TxtPhoneNo.Value.Trim & "', '" & TxtEmailId.Value.Trim & "', '" & Replace(TxtUserAddress.Value.Trim, ",", "") & "', '" & DDLIDRole.SelectedValue & "' , '" & ViewState("fileName") & "', '" & DDLCompany.SelectedValue & "', '" & TxtPament.Value.Trim & "','" & TxtIncetivePercent.Value.Trim & "','" & TxtPenaltyPercent.Value.Trim & "','0','" & ViewState("SelectedDDLID") & "','" & StrOTP & "')   ")
            StrUserId = clsObj.fnInsertScopeIdentity(StrInsQry.ToString, "")
            clsObj = Nothing
            If StrUserId.Length > 0 Then
                If IsNumeric(StrUserId) = True Then
                    If CInt(StrUserId) > 0 Then
                        If fnSendEmail(StrPassword, TxtUserName.Value.Trim) = True Then
                            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('User Created'); location.href='" & Session("BackPage").ToString() & "';</script>")
                        Else
                            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('User Created but Mail Not Send, Please Check Receivers Email Id / SMTP Credentials'); location.href='" & Session("BackPage").ToString() & "';</script>")
                        End If
                        'ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('User Created');</script>")
                        ' Response.Redirect("AspxSearchBlock.aspx?idMenu=" & Session("IdMenu"), False)
                        'divMain.Visible = False
                        'divchangepassword.Visible = False
                        'divImage.Visible = False
                        'divUserPay.Visible = False
                        'divOTP.Visible = True
                    Else
                        'ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('" & StrLngId & "'); location.href='" & Session("BackPage").ToString() & "';</script>")
                        'Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "RegisterClientScriptBlock", "document.write ('" & StrUserId & "');", True)
                        ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('" & StrUserId & "');</script>")
                    End If
                Else
                    'Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "RegisterClientScriptBlock", "document.write ('" & StrLngId & "');", True)
                    StrLngId = "'alert('" & StrUserId & "')'"
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "k1", StrLngId, True)
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "RegisterClientScriptBlock", "document.write ('" & StrUserId & "');", True)
                    'ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('" & StrUserId & "');</script>")
                End If
            Else
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('" & StrLngId & "'); location.href='" & Session("BackPage").ToString() & "';</script>")
            End If
        Else


            StrInsQry.Append(" UPdate tbl_User set UserFullName = '" & TxtUserFullName.Value & "', ContactNo = '" & TxtPhoneNo.Value.Trim & "', EmailId = '" & TxtEmailId.Value.Trim & "', ClientId = '" & ViewState("SelectedDDLID") & "', ")
            StrInsQry.Append(" UserAddress ='" & Replace(TxtUserAddress.Value.Trim, ",", "") & "', IDRole = '" & DDLIDRole.SelectedValue & "', ImageName =  '" & Replace(ViewState("fileName").ToString, "../", "") & "' , IDCompany = '" & DDLCompany.SelectedValue & "', PaymentinRs = '" & TxtPament.Value.Trim & "', IncentiveinPrcnt = '" & TxtIncetivePercent.Value.Trim & "', PenaltyinPrcnt ='" & TxtPenaltyPercent.Value.Trim & "' where ")
            StrInsQry.Append(" idUser = " & HdnIdentity.Value & "")
            If clsObj.fnIUD(StrInsQry.ToString, 1, "NonQuery") = True Then
                clsObj = Nothing
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('User Details Updated');</script>")
                Response.Redirect("AspxSearchBlock.aspx?idMenu=" & Session("IdMenu"), False)

            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", "Please Try again or Contact System / Web Administrator")
            End If
        End If
    End Sub

    Protected Sub DDLIDRole_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLIDRole.SelectedIndexChanged
        divUserPay.Visible = False
        Dim strComp As String = "2,3"
        Companylist.Attributes.Add("style", "display:block")
        If Not strComp.Contains(DDLIDRole.SelectedValue) Then
            Companylist.Attributes.Add("style", "display:none")
        End If
        If DDLIDRole.SelectedValue = "4" Then
            divUserPay.Visible = True
        End If
    End Sub

    Protected Sub DDLSelected_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLSelected.SelectedIndexChanged
        If DDLSelected.SelectedValue = "0" Then Exit Sub
        If IsNothing(ViewState("SelectedDDLText")) Then
            ViewState("SelectedDDLText") = DDLSelected.SelectedItem.Text
            ViewState("SelectedDDLID") = DDLSelected.SelectedValue

        Else
            If ViewState("SelectedDDLText").ToString.Contains(DDLSelected.SelectedItem.Text) = False Then
                ViewState("SelectedDDLText") = ViewState("SelectedDDLText") & "," & DDLSelected.SelectedItem.Text
                ViewState("SelectedDDLID") = ViewState("SelectedDDLID") & "," & DDLSelected.SelectedValue
            End If
        End If

        Dim StrSelectedDDLText As New StringBuilder
        Dim StrSelectedDDLID() As String = ViewState("SelectedDDLID").ToString.Split(",")
        Dim StrSelectedDDLTxt() As String = ViewState("SelectedDDLText").ToString.Split(",")
        For intcntr = 0 To StrSelectedDDLTxt.Length - 1
            If StrSelectedDDLTxt(intcntr).ToString.Length > 0 Then
                StrSelectedDDLText.Append("         <li class=""client_list"" tabindex=""0"">")
                StrSelectedDDLText.Append("             <a aria-haspopup=""true"" class=""client_heading1"" href=""#"" >")
                StrSelectedDDLText.Append("                 <p>" & StrSelectedDDLTxt(intcntr).ToString & "</p>")
                StrSelectedDDLText.Append("             </a>")
                'StrSelectedDDLText.Append("             <div class=""dropdown"">")
                'StrSelectedDDLText.Append("                 <div class=""dd-inner"">")
                'StrSelectedDDLText.Append("                     <div class=""column"" style=""background-color: red;"">")
                StrSelectedDDLText.Append("                         <a href=""#"" class=""client_remove"" onclick=" & Chr(34) & "javascript:RemoveSelectedFromList(" & StrSelectedDDLID(intcntr) & ",'" & StrSelectedDDLTxt(intcntr).ToString & "');" & Chr(34) & ">")
                StrSelectedDDLText.Append("                             <span class=""client_remove"">X</span>")
                StrSelectedDDLText.Append("                         </a>")
                'StrSelectedDDLText.Append("                     </div>")
                'StrSelectedDDLText.Append("                 </div>")
                'StrSelectedDDLText.Append("             </div>")
                StrSelectedDDLText.Append("         </li>")
            End If

            'StrSelectedDDLText.Append("<li>")
            'StrSelectedDDLText.Append("<a href=""#"">")
            'StrSelectedDDLText.Append("<p>" & StrSelectedDDLTxt(intcntr).ToString & "</p>")
            'StrSelectedDDLText.Append("</a>")
            'StrSelectedDDLText.Append("</li>")
        Next
        LtrlSelectedDDl.Text = StrSelectedDDLText.ToString
        StrSelectedList = ViewState("SelectedDDLID").ToString
        DDLSelected.SelectedValue = 0
    End Sub

    Protected Sub BtnRemoveSelected_Click(sender As Object, e As System.EventArgs) Handles BtnRemoveSelected.Click
        ViewState("SelectedDDLID") = ViewState("SelectedDDLID").ToString.Replace(HdnIdSelected.Value, "").Replace(",,", ",")
        ViewState("SelectedDDLText") = ViewState("SelectedDDLText").ToString.Replace(HdnSelected.Value, "").Replace(",,", ",")

        If ViewState("SelectedDDLID").ToString = "," Then ViewState("SelectedDDLID") = Nothing
        If ViewState("SelectedDDLText").ToString = "," Then ViewState("SelectedDDLText") = Nothing
        If Not IsNothing(ViewState("SelectedDDLID")) = True And Not IsNothing(ViewState("SelectedDDLText")) = True Then
            If Mid(ViewState("SelectedDDLID").ToString, 1, 1) = "," Then
                ViewState("SelectedDDLID") = Mid(ViewState("SelectedDDLID"), 2)
                If Mid(ViewState("SelectedDDLID").ToString, ViewState("SelectedDDLID").ToString.Length) = "," Then ViewState("SelectedDDLID") = Mid(ViewState("SelectedDDLID").ToString, 1, ViewState("SelectedDDLID").ToString.Length - 1)
            End If
            If Mid(ViewState("SelectedDDLText").ToString, 1, 1) = "," Then
                ViewState("SelectedDDLText") = Mid(ViewState("SelectedDDLText"), 2)
                If Mid(ViewState("SelectedDDLText").ToString, ViewState("SelectedDDLText").ToString.Length) = "," Then ViewState("SelectedDDLText") = Mid(ViewState("SelectedDDLText").ToString, 1, ViewState("SelectedDDLText").ToString.Length - 1)
            End If

        End If

        LtrlSelectedDDl.Text = String.Empty
        If Not IsNothing(ViewState("SelectedDDLID")) Then
            If ViewState("SelectedDDLID").ToString.Length > 0 Then
                Dim StrSelectedDDLID() As String = ViewState("SelectedDDLID").ToString.Split(",")
                Dim StrSelectedDDLTxt() As String = ViewState("SelectedDDLText").ToString.Split(",")
                Dim StrSelectedDDLText As New StringBuilder
                For intcntr = 0 To StrSelectedDDLTxt.Length - 1

                    If StrSelectedDDLTxt(intcntr).ToString.Length > 0 Then
                        StrSelectedDDLText.Append("         <li class=""client_list"" tabindex=""0"">")
                        StrSelectedDDLText.Append("             <a aria-haspopup=""true"" class=""client_heading1"" href=""#"" >")
                        StrSelectedDDLText.Append("                 <p>" & StrSelectedDDLTxt(intcntr).ToString & "</p>")
                        StrSelectedDDLText.Append("             </a>")
                        'StrSelectedDDLText.Append("             <div class=""dropdown"">")
                        'StrSelectedDDLText.Append("                 <div class=""dd-inner"">")
                        ' StrSelectedDDLText.Append("                     <div class=""column"" style=""background-color: red;"">")
                        StrSelectedDDLText.Append("                         <a href=""#"" class=""client_remove"" onclick=" & Chr(34) & "javascript:RemoveSelectedFromList(" & StrSelectedDDLID(intcntr) & ",'" & StrSelectedDDLTxt(intcntr).ToString & "');" & Chr(34) & ">")
                        StrSelectedDDLText.Append("                             <span class=""client_remove"">X</span>")
                        StrSelectedDDLText.Append("                         </a>")
                        ' StrSelectedDDLText.Append("                     </div>")
                        'StrSelectedDDLText.Append("                 </div>")
                        'StrSelectedDDLText.Append("             </div>")
                        StrSelectedDDLText.Append("         </li>")
                    End If
                Next
                If StrSelectedDDLText.ToString.Length > 0 Then
                    LtrlSelectedDDl.Text = StrSelectedDDLText.ToString
                    StrSelectedList = ViewState("SelectedDDLID").ToString
                End If
            Else
                ViewState("SelectedDDLID") = Nothing
                ViewState("SelectedDDLText") = Nothing
            End If
        Else
            ViewState("SelectedDDLID") = Nothing
            ViewState("SelectedDDLText") = Nothing
        End If
        DDLSelected.SelectedValue = 0
    End Sub
    Private Function Encrypt(StrMailTo As String) As String
        Dim EncryptionKey As String = "MAKV2SPBNI99212"
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(StrMailTo)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
             &H65, &H64, &H76, &H65, &H64, &H65, _
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                StrMailTo = Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
        Return StrMailTo
    End Function
    Private Function fnSendEmail(ByVal StrDecPassword As String, ByVal StrMailTo As String) As Boolean
        Dim BlnReturn As Boolean = False
        Dim StrLogMsg As String = String.Empty
        Dim StrActivationLink As String = String.Empty
        Dim name As String = HttpUtility.UrlEncode(Encrypt(StrMailTo))
        Dim myuri As New Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri)
        Dim pathQuery As String = myuri.PathAndQuery
        '103.233.76.155/accountsapp/AspxPages/AspxActivation.aspx?name=XXXX
        StrActivationLink = String.Format("/accountsapp/AspxActivation.aspx?name={0}", name)
        Dim hostName As String = myuri.ToString().Replace(pathQuery, StrActivationLink)
        Dim StrQry As New StringBuilder
        StrQry.Append(" Select IDUser, UserFullName, UserPassword, EmailId, UserName,OTP ")
        StrQry.Append(" From Tbl_User ")
        StrQry.Append(" where UserName = '" & StrMailTo & "'")
        Dim clsObj As New clsData
        Dim dtstSearchDtls As New DataSet
        dtstSearchDtls = clsObj.fnDataSet(StrQry.ToString, 1, 1)
        If Not IsNothing(dtstSearchDtls) Then
            If dtstSearchDtls.Tables.Count > 0 Then
                If dtstSearchDtls.Tables(0).Rows.Count > 0 Then
                    StrQry = New StringBuilder

                    StrQry.Append("select SMTP, Port, SMTPUsername, SMTPPassword, isGodaddysmtp from tbl_companyrule Where idcompanyrule = " & Session("EMailSendId"))
                    Dim dtstMailDtls As New DataSet
                    clsObj = New clsData
                    dtstMailDtls = clsObj.fnDataSet(StrQry.ToString, 1, 1)
                    clsObj = Nothing
                    If Not IsNothing(dtstMailDtls) Then
                        If dtstMailDtls.Tables.Count > 0 Then
                            If dtstMailDtls.Tables(0).Rows.Count > 0 Then
                                'Dim StrUserFullName As String = dtstSearchDtls.Tables(0).Rows(0)("UserFullName")
                                Dim Msg As New MailMessage()
                                Dim StrSMTPUsername As String = dtstMailDtls.Tables(0).Rows(0)("SMTPUsername")
                                Dim StrSMTPPassword As String = dtstMailDtls.Tables(0).Rows(0)("SMTPPassword")
                                ' Sender e-mail address.
                                Msg.From = New MailAddress(StrSMTPUsername)
                                ' Recipient e-mail address.
                                Msg.To.Add(dtstSearchDtls.Tables(0).Rows(0)("EmailId"))
                                Msg.CC.Add(StrSMTPUsername)
                                Msg.Subject = "Sibz Solution: Your Registration with " & Session("ProjectName").ToString & ""
                                Dim StrMessageBody As String = "Dear " & dtstSearchDtls.Tables(0).Rows(0)("UserFullName") & ", Welcome to " & Session("ProjectName").ToString & ". Your Login Id is: " & dtstSearchDtls.Tables(0).Rows(0)("UserName") & ", Your Password is : " & StrDecPassword & ". Your OTP  is: " & dtstSearchDtls.Tables(0).Rows(0)("OTP") & "<br />.<a id='qwe' href=" & hostName & " >Click here to Activate your account</a>"
                                Msg.Body = StrMessageBody
                                Msg.IsBodyHtml = True
                                ' your remote SMTP server IP.
                                Dim BlnIsGodaddy As Boolean = dtstMailDtls.Tables(0).Rows(0)("isGodaddysmtp")
                                Dim smtp As New SmtpClient()
                                smtp.Host = dtstMailDtls.Tables(0).Rows(0)("SMTP")
                                smtp.Port = dtstMailDtls.Tables(0).Rows(0)("Port")
                                If BlnIsGodaddy = False Then smtp.EnableSsl = True
                                smtp.Credentials = New System.Net.NetworkCredential(StrSMTPUsername, StrSMTPPassword)
                                Try
                                    smtp.Send(Msg)
                                    If Not IsNothing(Msg) Then
                                        Dim DtsReturn As New DataSet
                                        DtsReturn.Tables.Add("DTRetrun")
                                        DtsReturn.Tables(0).Columns.Add("Remark")
                                        DtsReturn.Tables(0).Columns.Add("ResponceStatus")
                                        DtsReturn.Tables(0).Rows.Add()
                                        DtsReturn.Tables(0).Rows(0)("Remark") = "Mail Send to Registered Email-Id"
                                        DtsReturn.Tables(0).Rows(0)("ResponceStatus") = "1"
                                        StrLogMsg = "Password Reminder: Mail Send to Registered Email-Id for Login Id: " & dtstSearchDtls.Tables(0).Rows(0)("UserName")
                                        BlnReturn = True
                                    Else
                                        StrLogMsg = "Password Reminder: Request Details Not Found for Login Id: " & dtstSearchDtls.Tables(0).Rows(0)("UserName")
                                        BlnReturn = False
                                    End If
                                Catch ex As Exception
                                    StrLogMsg = ex.Message
                                    BlnReturn = False
                                End Try
                            End If
                        End If
                    End If
                    clsObj = New clsData
                    clsObj.fninsertLog("User Registration Mail", "User Registration Mail", StrLogMsg, dtstSearchDtls.Tables(0).Rows(0)("IDUser"), "", 1)
                    clsObj = Nothing
                End If
            End If
        End If
        Return BlnReturn
    End Function

    Protected Sub btnreset_Click(sender As Object, e As System.EventArgs) Handles btnreset.Click
        If HdnIdentity.Value.Trim <> "" Then
            fnFillScreen()
        End If
    End Sub

#Region "User Defined Functions"
    'Fill Drop down List
    Private Sub fnFillDDL()
        Dim clsodbcObj As New clsData
        Dim StrDDLListQry As String = " select '0' id,'All Company' data1 Union All select ClientId, txtCompanyName ClientName from Vw_ClientMaster where RecordStatus = 0  "
        clsodbcObj.fnDropDownFill(StrDDLListQry, DDLCompany, "data1", "id")
        clsodbcObj = Nothing
        clsodbcObj = New clsData
        clsodbcObj.fnDropDownFill(StrDDLListQry, DDLSelected, "data1", "id")
        clsodbcObj = Nothing
        clsodbcObj = New clsData
        StrDDLListQry = " select '0' id,'Select' data1 Union All select idDefination, DefinationDesc from tbl_Defination Where idDefType = 'Roles'"
        clsodbcObj.fnDropDownFill(StrDDLListQry, DDLIDRole, "data1", "id")
        clsodbcObj = Nothing
    End Sub

    'fnUpload is use to upload the file and also to View the image 
    Protected Sub fnUpload(ByVal sender As Object, ByVal e As EventArgs)
        If flupload.HasFile Then
            'If TxtPhoneNo.Value.Trim = String.Empty Then Exit Sub
            Dim fileName As String = Replace(Replace(Replace(Replace(Replace(TxtPhoneNo.Value, " ", "", ), ",", ""), "/", ""), "\", "") & "." & Path.GetExtension(flupload.PostedFile.FileName), "..", ".") ' Path.GetFileName(flupload.PostedFile.FileName)
            flupload.PostedFile.SaveAs(Server.MapPath("~/Upload/UserPhoto/") + fileName)
            imgprvw.Src = "../Upload/UserPhoto/" + fileName
            imgprvw.Alt = fileName
            ViewState("fileName") = "Upload/UserPhoto/" + fileName
            ViewState("ClearLogoFile") = False
        End If
    End Sub
    'fnClear is use to clear the image
    Protected Sub fnClear(ByVal sender As Object, ByVal e As EventArgs)
        imgprvw.Src = "../Resource/images/User.png"
        imgprvw.Alt = "User "
        ViewState("ClearLogoFile") = True
        ViewState("fileName") = "Resource/images/User.png"
    End Sub
    Protected Sub BtnClearImage_Click(sender As Object, e As System.EventArgs) Handles BtnClearImage.Click
        imgprvw.Src = "../Resource/images/User.png"
        imgprvw.Alt = "User "
        ViewState("ClearLogoFile") = True
        ViewState("fileName") = "Resource/images/User.png"
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
        For i = 0 To 8 - 1 Step i + 1
            temp = arr(rand.Next(0, arr.Length))
            passwordString += temp
        Next
        Return passwordString
    End Function

    'Validate the Screen from Server and also check for exists of email id
    Private Function fnValidate() As Boolean
        Dim strEmailIdCheck As String = "Select EmailID from tbl_user where 1=1 and emailid = '" & TxtEmailId.Value.Trim & "'"
        If HdnIdentity.Value <> String.Empty Then
            strEmailIdCheck = strEmailIdCheck & " And idUser <> " & HdnIdentity.Value & ""
        End If
        Dim clsObj As New clsData
        If clsObj.fnStrRetrive(strEmailIdCheck, 1).Length > 0 Then
            ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script type='text/javascript' language='javascript'>alert('Email ID Already in Use with Different User');</script>")
            Return False
        End If
        If DDLIDRole.SelectedValue = 0 Then ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script type='text/javascript' language='javascript'>alert('Please select the Role');</script>") : Return False
        If DDLIDRole.SelectedValue = 2 And DDLIDRole.SelectedValue = 3 Then
            If DDLCompany.SelectedValue = 0 Then ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script type='text/javascript' language='javascript'>alert('Select Company');</script>") : Return False
        End If
        If TxtUserFullName.Value.Trim = String.Empty Then ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script type='text/javascript' language='javascript'>alert('Please Provide User's Full Name');</script>") : Return False
        If TxtUserAddress.Value.Trim = String.Empty Then ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script type='text/javascript' language='javascript'>alert('User Address ');</script>") : Return False
        If TxtEmailId.Value.Trim = String.Empty Then ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script type='text/javascript' language='javascript'>alert('User Email Address');</script>") : Return False
        If TxtPhoneNo.Value.Trim = String.Empty Then ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script type='text/javascript' language='javascript'>alert('User Contact No');</script>") : Return False
        If DDLIDRole.SelectedValue = 4 Then
            If TxtPament.Value.Trim = String.Empty Then ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script type='text/javascript' language='javascript'>alert('User Payment for Per Entry');</script>") : Return False
            If TxtIncetivePercent.Value.Trim = String.Empty Then ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script type='text/javascript' language='javascript'>alert('User Incentive Percent Only Numbers');</script>") : Return False
            If TxtPenaltyPercent.Value.Trim = String.Empty Then ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script type='text/javascript' language='javascript'>alert('User Penalty Percent Only Numbers');</script>") : Return False
        End If
        Return True
    End Function

    Private Sub fnFillScreen()
        divUserPay.Visible = False
        Dim DtsSearchData As New DataSet
        Dim StrQrySearch As New StringBuilder
        Dim Enc As New RijndaelSimple

        StrQrySearch.Append(" select idUser, UserName, UserFullName, ContactNo, EmailId, UserAddress, IDRole, case when ImageName ='' then '../Content/Images/Menu/Master/User.png' else '../' + ImageName end as 'ImageName', IDCompany, PaymentinRs, IncentiveinPrcnt, PenaltyinPrcnt, DeActivate, ClientId,UserPassword,isnull(OTPVerified,0) as 'OTPVerified',isnull(Emailvalidation,0) as 'Emailvalidation' from tbl_user ")
        StrQrySearch.Append(" where idUser = " & HdnIdentity.Value & "")
        Dim clsobj As New clsData
        DtsSearchData = clsobj.fnDataSet(StrQrySearch.ToString, CommandType.Text)
        clsobj = Nothing
        If Not IsNothing(DtsSearchData) Then
            If DtsSearchData.Tables.Count > 0 Then
                If DtsSearchData.Tables(0).Rows.Count > 0 Then
                    DDLIDRole.SelectedValue = DtsSearchData.Tables(0).Rows(0)("IDRole")
                    Dim strComp As String = "2,3"
                    Companylist.Attributes.Add("style", "display:block")
                    If Not strComp.Contains(DDLIDRole.SelectedValue) Then Companylist.Attributes.Add("style", "display:none")
                    DDLCompany.SelectedValue = DtsSearchData.Tables(0).Rows(0)("IDCompany")
                    TxtUserName.Value = DtsSearchData.Tables(0).Rows(0)("UserName")
                    TxtUserName.Disabled = True
                    TxtUserFullName.Value = DtsSearchData.Tables(0).Rows(0)("UserFullName")
                    TxtPhoneNo.Value = DtsSearchData.Tables(0).Rows(0)("ContactNo")
                    If DtsSearchData.Tables(0).Rows(0)("Emailvalidation") = 0 Then
                        TxtEmailId.Attributes.Add("style", "background:red")
                    Else
                        TxtEmailId.Attributes.Add("style", "background:green")
                    End If
                    If DtsSearchData.Tables(0).Rows(0)("OTPVerified") = 0 Then
                        TxtPhoneNo.Attributes.Add("style", "background:red")
                    Else
                        TxtPhoneNo.Attributes.Add("style", "background:green")
                    End If
                    TxtEmailId.Value = DtsSearchData.Tables(0).Rows(0)("EmailId")
                    TxtUserAddress.Value = DtsSearchData.Tables(0).Rows(0)("UserAddress")
                    imgprvw.Src = DtsSearchData.Tables(0).Rows(0)("ImageName")
                    ViewState("fileName") = DtsSearchData.Tables(0).Rows(0)("ImageName")
                    TxtPament.Value = DtsSearchData.Tables(0).Rows(0)("PaymentinRs")
                    TxtIncetivePercent.Value = DtsSearchData.Tables(0).Rows(0)("IncentiveinPrcnt")
                    TxtPenaltyPercent.Value = DtsSearchData.Tables(0).Rows(0)("PenaltyinPrcnt")


                    If DDLIDRole.SelectedValue = 4 Then
                        'TxtUserFullName.Value = DtsSearchData.Tables(0).Rows(0)("ClientId")
                        'To fill smsurl in dropdown list
                        ViewState("SelectedDDLID") = DtsSearchData.Tables(0).Rows(0)("ClientId")
                        If DtsSearchData.Tables(0).Rows(0)("ClientId").ToString <> String.Empty Then
                            Dim strSelectedVehicleID() As String = DtsSearchData.Tables(0).Rows(0)("ClientId").ToString.Split(",")
                            ViewState("SelectedDDLText") = String.Empty
                            For Intselcntr = 0 To strSelectedVehicleID.Length - 1
                                For intcntr = 0 To DDLSelected.Items.Count - 1
                                    If strSelectedVehicleID(Intselcntr).ToString.Length > 0 Then
                                        If DDLSelected.Items(intcntr).Value.Contains(strSelectedVehicleID(Intselcntr)) Then
                                            If ViewState("SelectedDDLText").ToString.Length = 0 Then
                                                ViewState("SelectedDDLText") = DDLSelected.Items(intcntr).Text
                                                Exit For
                                            Else
                                                ViewState("SelectedDDLText") = ViewState("SelectedDDLText") & "," & DDLSelected.Items(intcntr).Text
                                                Exit For
                                            End If
                                        End If
                                    End If

                                Next
                            Next
                            If Not IsNothing(ViewState("SelectedDDLID")) Then
                                Dim StrSelectedDDLText As New StringBuilder
                                Dim StrSelectedDDLID() As String = ViewState("SelectedDDLID").ToString.Split(",")
                                Dim StrSelectedDDLTxt() As String = ViewState("SelectedDDLText").ToString.Split(",")

                                For intcntr = 0 To StrSelectedDDLTxt.Length - 1
                                    If StrSelectedDDLTxt(intcntr).ToString.Length > 0 Then
                                        StrSelectedDDLText.Append("         <li class=""client_list"" tabindex=""0"">")
                                        StrSelectedDDLText.Append("             <a aria-haspopup=""true"" class=""client_heading1"" href=""#"" >")
                                        StrSelectedDDLText.Append("                 <p>" & StrSelectedDDLTxt(intcntr).ToString & "</p>")
                                        StrSelectedDDLText.Append("             </a>")
                                        'StrSelectedDDLText.Append("             <div class=""dropdown"">")
                                        'StrSelectedDDLText.Append("                 <div class=""dd-inner"">")
                                        'StrSelectedDDLText.Append("                     <div class=""column"" style=""background-color: red;"">")
                                        StrSelectedDDLText.Append("                         <a href=""#"" class=""client_remove"" onclick=" & Chr(34) & "javascript:RemoveSelectedFromList(" & StrSelectedDDLID(intcntr) & ",'" & StrSelectedDDLTxt(intcntr).ToString & "');" & Chr(34) & ">")
                                        StrSelectedDDLText.Append("                             <span class=""client_remove"">X</span>")
                                        StrSelectedDDLText.Append("                         </a>")
                                        'StrSelectedDDLText.Append("                     </div>")
                                        'StrSelectedDDLText.Append("                 </div>")
                                        'StrSelectedDDLText.Append("             </div>")
                                        StrSelectedDDLText.Append("         </li>")
                                    End If
                                    'StrSelectedDDLText.Append("<li>")
                                    'StrSelectedDDLText.Append("<a href=""#"">")
                                    'StrSelectedDDLText.Append("<p>" & StrSelectedDDLTxt(intcntr).ToString & "</p>")
                                    'StrSelectedDDLText.Append("</a>")
                                    'StrSelectedDDLText.Append("</li>")
                                Next
                                LtrlSelectedDDl.Text = StrSelectedDDLText.ToString
                                StrSelectedList = ViewState("SelectedDDLID").ToString
                            End If
                        End If

                        If DDLIDRole.SelectedValue = "4" Then
                            divUserPay.Visible = True
                        End If
                    End If

                End If
            End If
        End If
    End Sub
#End Region


    Protected Sub btnChangePassword_Click(sender As Object, e As System.EventArgs) Handles btnChangePassword.Click
        Dim StrLngId As String = String.Empty
        Dim clsobj As New clsData
        Dim Enc As New RijndaelSimple
        Dim EncOldPassword As String = Enc.Encrypt(txtOldPassword.Value)
        Dim EncNewPassword As String = Enc.Encrypt(txtPassword.Value)
        Dim EncConfiPassword As String = Enc.Encrypt(txtConfirmPassword.Value)
        Enc = Nothing
        Dim Names As String() = {"idUser", "UserPassword", "Newpassword", "ConfirmPassword"}
        Dim Values As String() = {HdnIdentity.Value, EncOldPassword, EncNewPassword, EncConfiPassword}
        StrLngId = DirectCast(clsobj.fnInsertUpdate("P_UpdatePassword", Names, Values), String)
        If (StrLngId.Contains("Successfully")) Then
            If chkPassword.Checked = True Then
                fnSendForgetEmail(txtPassword.Value, TxtEmailId.Value.Trim)
            End If
            HdnIdentity.Value = String.Empty.ToString()
            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Password Updatede Successfully'); location.href='AspxSearchBlock.aspx?idMenu=8';</script>")
        Else
            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Current Password Not Correct'); location.href='AspxSearchBlock.aspx?idMenu=8'</script>")
        End If
    End Sub
    Private Function fnSendForgetEmail(ByVal StrDecPassword As String, ByVal StrMailTo As String) As Boolean
        Dim BlnReturn As Boolean = False
        Dim StrLogMsg As String = String.Empty
        'Dim StrActivationLink As String = String.Empty
        Dim name As String = HttpUtility.UrlEncode(Encrypt(StrMailTo))
        Dim myuri As New Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri)
        Dim pathQuery As String = myuri.PathAndQuery
        '103.233.76.155/accountsapp/AspxPages/AspxActivation.aspx?name=XXXX
        'StrActivationLink = String.Format("/accountsapp/AspxActivation.aspx?name={0}", name)
        'Dim hostName As String = myuri.ToString().Replace(pathQuery, StrActivationLink)
        Dim StrQry As New StringBuilder
        StrQry.Append(" Select IDUser, UserFullName, UserPassword, EmailId, UserName ")
        StrQry.Append(" From Tbl_User ")
        StrQry.Append(" where EmailId = '" & StrMailTo & "'")
        Dim clsObj As New clsData
        Dim dtstSearchDtls As New DataSet
        dtstSearchDtls = clsObj.fnDataSet(StrQry.ToString, 1, 1)
        If Not IsNothing(dtstSearchDtls) Then
            If dtstSearchDtls.Tables.Count > 0 Then
                If dtstSearchDtls.Tables(0).Rows.Count > 0 Then
                    StrQry = New StringBuilder

                    StrQry.Append("select SMTP, Port, SMTPUsername, SMTPPassword, isGodaddysmtp from tbl_companyrule Where idcompanyrule = " & Session("EMailSendId"))
                    Dim dtstMailDtls As New DataSet
                    clsObj = New clsData
                    dtstMailDtls = clsObj.fnDataSet(StrQry.ToString, 1, 1)
                    clsObj = Nothing
                    If Not IsNothing(dtstMailDtls) Then
                        If dtstMailDtls.Tables.Count > 0 Then
                            If dtstMailDtls.Tables(0).Rows.Count > 0 Then
                                'Dim StrUserFullName As String = dtstSearchDtls.Tables(0).Rows(0)("UserFullName")
                                Dim Msg As New MailMessage()
                                Dim StrSMTPUsername As String = dtstMailDtls.Tables(0).Rows(0)("SMTPUsername")
                                Dim StrSMTPPassword As String = dtstMailDtls.Tables(0).Rows(0)("SMTPPassword")
                                ' Sender e-mail address.
                                Msg.From = New MailAddress(StrSMTPUsername)
                                ' Recipient e-mail address.
                                Msg.To.Add(StrMailTo)
                                Msg.Subject = "Sibz Solution: Your Password is Reset " & Session("ProjectName").ToString & ""
                                Dim StrMessageBody As String = "Dear " & dtstSearchDtls.Tables(0).Rows(0)("UserFullName") & ",<br /> Accounts Solutions Admin had changed your password .<br /> Your new Password is: " & StrDecPassword & "<br /> Thanks and Regards <br />Admin - Accounts Solutions"
                                Msg.Body = StrMessageBody
                                Msg.IsBodyHtml = True
                                ' your remote SMTP server IP.
                                Dim BlnIsGodaddy As Boolean = dtstMailDtls.Tables(0).Rows(0)("isGodaddysmtp")
                                Dim smtp As New SmtpClient()
                                smtp.Host = dtstMailDtls.Tables(0).Rows(0)("SMTP")
                                smtp.Port = dtstMailDtls.Tables(0).Rows(0)("Port")
                                If BlnIsGodaddy = False Then smtp.EnableSsl = True
                                smtp.Credentials = New System.Net.NetworkCredential(StrSMTPUsername, StrSMTPPassword)
                                Try
                                    smtp.Send(Msg)
                                    If Not IsNothing(Msg) Then
                                        Dim DtsReturn As New DataSet
                                        DtsReturn.Tables.Add("DTRetrun")
                                        DtsReturn.Tables(0).Columns.Add("Remark")
                                        DtsReturn.Tables(0).Columns.Add("ResponceStatus")
                                        DtsReturn.Tables(0).Rows.Add()
                                        DtsReturn.Tables(0).Rows(0)("Remark") = "Mail Send to Registered Email-Id"
                                        DtsReturn.Tables(0).Rows(0)("ResponceStatus") = "1"
                                        StrLogMsg = "Password Reminder: Mail Send to Registered Email-Id for Login Id: " & dtstSearchDtls.Tables(0).Rows(0)("UserName")
                                        BlnReturn = True
                                    Else
                                        StrLogMsg = "Password Reminder: Request Details Not Found for Login Id: " & dtstSearchDtls.Tables(0).Rows(0)("UserName")
                                        BlnReturn = False
                                    End If
                                Catch ex As Exception
                                    StrLogMsg = ex.Message
                                    BlnReturn = False
                                End Try
                            End If
                        End If
                    End If
                End If
            End If
        End If
        clsObj = New clsData
        clsObj.fninsertLog("User Password Recovary Mail", "User Password Recovary Mail", StrLogMsg, dtstSearchDtls.Tables(0).Rows(0)("IDUser"), "", 1)
        clsObj = Nothing
        Return BlnReturn
    End Function

    Protected Sub btnOTPValidation_Click(sender As Object, e As System.EventArgs) Handles btnOTPValidation.Click
        Dim clsObj As New clsData
        Dim StrOTP As String = String.Empty
        Dim StrSQL As String = String.Empty
        StrSQL = "SELECT OTP from tbl_User where idUser=" & StrUserId
        StrOTP = clsObj.fnStrRetrive(StrSQL, 1)
        StrSQL = Nothing
        Dim StrEntOTP As String = txtOTPNO.Value
        If StrOTP = StrEntOTP Then
            StrSQL = "update tbl_User set OTPVerified=1 where idUser=" & StrUserId
            If clsObj.fnIUD(StrSQL.ToString, 1, "NonQuery") = True Then
                clsObj = Nothing
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('OTP is Validated');</script>")
                Response.Redirect("AspxSearchBlock.aspx?idMenu=" & Session("IdMenu"), False)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", "Please Try again or Contact System / Web Administrator")
            End If
        Else
            txtOTPNO.Value = ""
            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('OTP is Invalid');</script>")
        End If
    End Sub

    'Protected Sub resendotp_ServerClick(sender As Object, e As System.EventArgs) Handles resendotp.ServerClick
    '    Dim clsObj As New clsData
    '    Dim StrOTP As String = String.Empty
    '    Dim StrSQL As String = String.Empty
    '    Dim StrSQLUpdate As String = String.Empty
    '    StrSQL = "SELECT ISNULL(OTPNo,0) from GenerateNumber"
    '    StrOTP = clsObj.fnStrRetrive(StrSQL, 1)
    '    StrSQLUpdate = "update tbl_User set OTP='" & StrOTP & "' where idUser=" & StrUserId
    '    If clsObj.fnIUD(StrSQLUpdate.ToString, 1, "NonQuery") = True Then
    '        clsObj = Nothing
    '        ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('OTP Send Successfully');</script>")
    '    Else
    '        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", "Please Try again or Contact System / Web Administrator")
    '    End If
    'End Sub



End Class
