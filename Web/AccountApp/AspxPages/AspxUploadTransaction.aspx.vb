''Todo
'' Not able to Clear and bring back the default image on clear this was not happing earlier also need to fix
'' Need to Save to table


Imports System.Globalization
Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Net
Imports System.Runtime.Serialization.Json

Partial Class AspxPages_AspxUploadTransaction
    Inherits System.Web.UI.Page
    Dim clsObj As New clsData

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If IsNothing(Session("IDRole")) = True Or IsNothing(Session("IDRole")) = True Then
                Response.Redirect("~/Default.aspx", False)
                Exit Sub
            End If
            If IsNothing(Request.QueryString("status")) Then

            Else
                Dim strStatu As String = Request.QueryString("status")
                If strStatu = "true" Then
                    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Transaction Added Successfully');</script>")
                End If
            End If
            fnFillDDL()
        End If
    End Sub

    Private Sub fnFillDDL()

        Dim clsodbcObj As New clsData
        Dim StrDDLListQry As String = " select '0' id,'Select' data1 Union All select ClientId as id, ClientShortCode + ' - ' + TxtCompanyName as Data1 from Vw_ClientMaster  where RecordStatus = 0  "
        If Session("IDRole").ToString = 2 Or Session("IDRole").ToString = 3 Then
            StrDDLListQry = StrDDLListQry & " and ClientId = " & Session("IDClient")
        End If
        clsodbcObj.fnDropDownFill(StrDDLListQry, DDLCompany, "data1", "id")

        If Session("IDRole").ToString = 2 Or Session("IDRole").ToString = 3 Then
            If DDLCompany.Items.Count > 0 Then
                DDLCompany.SelectedValue = Session("IDClient")
                fnFillDDLchange()
            End If
        End If


        clsodbcObj = Nothing
        clsodbcObj = New clsData
        StrDDLListQry = " select '0' id,'Select' data1 Union All Select idDefination id, definationcode +' - ' + DefinationDesc Data1 from tbl_Defination where idDefType = 'TT' "
        clsodbcObj.fnDropDownFill(StrDDLListQry, DDLTransaction, "data1", "id")
        clsodbcObj = Nothing
    End Sub

    'fnUpload is use to upload the file and also to View the image 
    Protected Sub fnUpload(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim strFileNameCommasep As String
            If DDLCompany.SelectedValue = 0 Then
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Please Select Company');</script>")
                Exit Sub
            End If

            'If DDLTransaction.SelectedValue = 0 Then
            '    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Please Select Transaction Type');</script>")
            '    Exit Sub
            'End If

            If TxtTransactionId.Text.Trim = "" Then
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Please Provide Purchase ID / Sales ID / Receipt ID / Payment ID/ Journal ID');</script>")
                Exit Sub
            End If
            Dim StrCompCode As String = LTrim(RTrim(Mid(DDLCompany.SelectedItem.Text, 1, InStr(DDLCompany.SelectedItem.Text, "-") - 1)))
            'Dim StrTTCode As String = Mid(DDLTransaction.SelectedItem.Text, 1, InStr(DDLTransaction.SelectedItem.Text, "-") - 1)
            Dim StrTTCode As String = String.Empty
            If RdPUR.Checked = True Then StrTTCode = "PUR"
            If RdSL.Checked = True Then StrTTCode = "SL"
            If RdRCPT.Checked = True Then StrTTCode = "RCPT"
            If RdPAY.Checked = True Then StrTTCode = "PAY"
            If RdJE.Checked = True Then StrTTCode = "JE"
            Dim StrTrnID As String = TxtTransactionId.Text.Trim
            Dim StrTrnDate As String = Replace(Replace(Replace(Replace(TxtTransactionDt.Text.Trim, "-", "_"), " ", "_"), ":", "_"), "/", "_")


            If flupload.HasFile Then
                Dim IntFlCount As Integer = 1

                For Each uploadedFile As HttpPostedFile In flupload.PostedFiles
                    If (uploadedFile.ContentLength > 0) Then
                        Dim strpath As String = Path.GetExtension(uploadedFile.FileName)
                        Dim StrFileExt As String = Path.GetExtension(uploadedFile.FileName)
                        Dim StrFileName As String = LTrim(RTrim(StrCompCode)) & "_" & StrTTCode & "_" & StrTrnID & "_" & StrTrnDate & "_" & IntFlCount & "." & Replace(StrFileExt, "..", ".") ' Path.GetFileName(flupload.PostedFile.FileName)
                        StrFileName = Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(StrFileName, "/", "_"), "\", "_"), ":", "_"), "*", "_"), "?", "_"), """", "_"), "<", "_"), ">", "_"), "|", "_"), "..", ".") ''Replace(StrFileName, "..", ".")
                        If strpath <> ".pdf" AndAlso strpath <> ".xls" AndAlso strpath <> ".xlsx" Then
                            'Dim strm As Stream = flupload.PostedFile.InputStream
                            uploadedFile.SaveAs(Server.MapPath("~/Upload/Transaction/") + StrFileName)
                            'Dim targetPath As String = Server.MapPath("../Upload/Transaction/Thumb/" & StrFileName)
                            ''Dim targetFile = targetPath
                            'GenerateThumbnails(0.4, strm, targetPath)
                            'flupload.PostedFile.SaveAs(Server.MapPath("~/Upload/Transaction/") + StrFileName)
                            imgprvw.Src = "../Upload/Transaction/" + StrFileName
                            imgprvw.Alt = StrFileName
                            ViewState("StrfileNameUP") = "Upload/Transaction/" + StrFileName
                        Else
                            flupload.PostedFile.SaveAs(Server.MapPath("~/Upload/Transaction/") + StrFileName)
                            ViewState("StrfileNameUP") = "Upload/Transaction/" + StrFileName
                            imgprvw.Attributes.Add("style", "display:none")
                            Dim path As String = Server.MapPath("../" & ViewState("StrfileNameUP"))
                            Dim embed As String = "<object data=""{0}"" type=""application/pdf"" width=""500px"" height=""500px"">"
                            embed += "If you are unable to view file, you can download from <a href = ""{0}"">here</a>"
                            embed += " or download <a target = ""_blank"" href = ""http://get.adobe.com/reader/"">Adobe PDF Reader</a> to view the file."
                            embed += "</object>"
                            ltEmbed.Text = String.Format(embed, ResolveUrl("~/" & ViewState("StrfileNameUP")))
                        End If
                        Dim StrURL As String = Request.Url.GetLeftPart(UriPartial.Authority) & "/Upload/Transaction/" + StrFileName
                        listofuploadedfiles.Text = (listofuploadedfiles.Text + String.Format("{0}<br />", "<a href = """ & StrURL & """ target=""_blank"">" & StrFileName & "</a>"))
                        strFileNameCommasep = strFileNameCommasep + "," + ViewState("StrfileNameUP").ToString()
                        'listofuploadedfiles.Text = (listofuploadedfiles.Text + String.Format("{0}<br />", StrFileName))
                        IntFlCount = IntFlCount + 1
                    End If

                Next

                If Mid(strFileNameCommasep, 1, 1) = "," Then
                    strFileNameCommasep = Mid(strFileNameCommasep, 2)
                End If

                ViewState("FileNames") = strFileNameCommasep
                ViewState("StrfileNameUP") = ""
                'For Each file In flupload.PostedFile
                '    'upload logic
                '    Response.Write((file.FileName + (" - " + (file.ContentLength + " Bytes. <br />"))))
                'Next

                'Dim IntTotalFiles As Integer = flupload.PostedFile.ContentType.Length '0 'flupload.PostedFiles
                'For IntFlCount = 0 To IntTotalFiles - 1
                '    Dim strpath As String = "abc" 'Path.GetExtension(flupload.PostedFiles(IntFlCount).filename)
                '    Dim StrCompCode As String = Mid(DDLCompany.SelectedItem.Text, 1, InStr(DDLCompany.SelectedItem.Text, "-") - 1)
                '    Dim StrTTCode As String = Mid(DDLTransaction.SelectedItem.Text, 1, InStr(DDLTransaction.SelectedItem.Text, "-") - 1)
                '    Dim StrTrnID As String = TxtTransactionId.Text.Trim
                '    Dim StrTrnDate As String = Replace(Replace(Replace(Replace(TxtTransactionDt.Text.Trim, "-", "_"), " ", "_"), ":", "_"), "/", "_")
                '    Dim StrFileExt As String = "jpg" 'Path.GetExtension(flupload.PostedFile)
                '    Dim StrFileName As String = StrCompCode & "_" & StrTTCode & "_" & StrTrnID & "_" & StrTrnDate & "_" & IntFlCount + 1 & "." & Replace(StrFileExt, "..", ".") ' Path.GetFileName(flupload.PostedFile.FileName)
                '    StrFileName = Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(StrFileName, "/", "_"), "\", "_"), ":", "_"), "*", "_"), "?", "_"), """", "_"), "<", "_"), ">", "_"), "|", "_"), "..", ".") ''Replace(StrFileName, "..", ".")
                '    If strpath <> ".pdf" AndAlso strpath <> ".xls" AndAlso strpath <> ".xlsx" Then
                '        Dim strm As Stream = flupload.PostedFile.InputStream
                '        Dim targetPath As String = Server.MapPath("../Upload/Transaction/Thumb/" & StrFileName)
                '        'Dim targetFile = targetPath
                '        GenerateThumbnails(0.4, strm, targetPath)
                '        flupload.PostedFile.SaveAs(Server.MapPath("~/Upload/Transaction/") + StrFileName)
                '        imgprvw.Src = "../Upload/Transaction/" + StrFileName
                '        imgprvw.Alt = StrFileName
                '        ViewState("StrfileNameUP") = "Upload/Transaction/" + StrFileName
                '    Else
                '        flupload.PostedFile.SaveAs(Server.MapPath("~/Upload/Transaction/") + StrFileName)
                '        ViewState("StrfileNameUP") = "Upload/Transaction/" + StrFileName
                '        imgprvw.Attributes.Add("style", "display:none")
                '        Dim path As String = Server.MapPath("../" & ViewState("StrfileNameUP"))
                '        Dim embed As String = "<object data=""{0}"" type=""application/pdf"" width=""500px"" height=""500px"">"
                '        embed += "If you are unable to view file, you can download from <a href = ""{0}"">here</a>"
                '        embed += " or download <a target = ""_blank"" href = ""http://get.adobe.com/reader/"">Adobe PDF Reader</a> to view the file."
                '        embed += "</object>"
                '        ltEmbed.Text = String.Format(embed, ResolveUrl("~/" & ViewState("StrfileNameUP")))
                '        'Dim client As New WebClient()
                '        'Dim buffer As [Byte]() = client.DownloadData(path)
                '        'If buffer IsNot Nothing Then
                '        '    Response.ContentType = "application/pdf"
                '        '    Response.AddHeader("content-length", buffer.Length.ToString())
                '        '    Response.BinaryWrite(buffer)
                '        'End If
                '    End If
                'Next
            End If




            'If flupload.HasFile Then
            'Dim strpath As String = Path.GetExtension(flupload.FileName)
            'Dim StrCompCode As String = Mid(DDLCompany.SelectedItem.Text, 1, InStr(DDLCompany.SelectedItem.Text, "-") - 1)
            'Dim StrTTCode As String = Mid(DDLTransaction.SelectedItem.Text, 1, InStr(DDLTransaction.SelectedItem.Text, "-") - 1)
            'Dim StrTrnID As String = TxtTransactionId.Text.Trim

            'Dim StrFileName As String = StrCompCode & "_" & StrTTCode & "_" & StrTrnID & "_" & Replace(Replace(Replace(Replace(DateTime.Parse(Now().ToString), "-", "_"), " ", "_"), ":", "_"), "/", "_") & "." & Replace(Path.GetExtension(flupload.PostedFile.FileName), "..", ".") ' Path.GetFileName(flupload.PostedFile.FileName)
            'StrFileName = Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(StrFileName, "/", "_"), "\", "_"), ":", "_"), "*", "_"), "?", "_"), """", "_"), "<", "_"), ">", "_"), "|", "_"), "..", ".") ''Replace(StrFileName, "..", ".")
            'If strpath <> ".pdf" AndAlso strpath <> ".xls" AndAlso strpath <> ".xlsx" Then
            '    Dim strm As Stream = flupload.PostedFile.InputStream
            '    Dim targetPath As String = Server.MapPath("../Upload/Transaction/Thumb/" & StrFileName)
            '    'Dim targetFile = targetPath
            '    GenerateThumbnails(0.4, strm, targetPath)
            '    flupload.PostedFile.SaveAs(Server.MapPath("~/Upload/Transaction/") + StrFileName)
            '    imgprvw.Src = "../Upload/Transaction/" + StrFileName
            '    imgprvw.Alt = StrFileName
            '    ViewState("StrfileNameUP") = "Upload/Transaction/" + StrFileName
            'Else
            '    flupload.PostedFile.SaveAs(Server.MapPath("~/Upload/Transaction/") + StrFileName)
            '    ViewState("StrfileNameUP") = "Upload/Transaction/" + StrFileName
            '    imgprvw.Attributes.Add("style", "display:none")
            '    Dim path As String = Server.MapPath("../" & ViewState("StrfileNameUP"))
            '    Dim embed As String = "<object data=""{0}"" type=""application/pdf"" width=""500px"" height=""500px"">"
            '    embed += "If you are unable to view file, you can download from <a href = ""{0}"">here</a>"
            '    embed += " or download <a target = ""_blank"" href = ""http://get.adobe.com/reader/"">Adobe PDF Reader</a> to view the file."
            '    embed += "</object>"
            '    ltEmbed.Text = String.Format(embed, ResolveUrl("~/" & ViewState("StrfileNameUP")))
            '    'Dim client As New WebClient()
            '    'Dim buffer As [Byte]() = client.DownloadData(path)
            '    'If buffer IsNot Nothing Then
            '    '    Response.ContentType = "application/pdf"
            '    '    Response.AddHeader("content-length", buffer.Length.ToString())
            '    '    Response.BinaryWrite(buffer)
            '    'End If
            'End If

            'Else
            'ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Please browse image first');</script>")
            'End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub GenerateThumbnails(scaleFactor As Double, sourcePath As Stream, targetPath As String)
        Try
            Using image__1 = Image.FromStream(sourcePath)
                Dim newWidth = CInt(image__1.Width * scaleFactor)
                Dim newHeight = CInt(image__1.Height * scaleFactor)
                Dim thumbnailImg = New Bitmap(newWidth, newHeight)
                Dim thumbGraph = Graphics.FromImage(thumbnailImg)
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic
                Dim imageRectangle = New Rectangle(0, 0, newWidth, newHeight)
                thumbGraph.DrawImage(image__1, imageRectangle)
                thumbnailImg.Save(targetPath, image__1.RawFormat)
            End Using
        Catch ex As Exception

        End Try
    End Sub
    'fnClear is use to clear the image
    Protected Sub fnClear(ByVal sender As Object, ByVal e As EventArgs)
        ltEmbed.Text = ""
        listofuploadedfiles.Text = String.Empty
        imgprvw.Attributes.Add("display", "inline")
        imgprvw.Src = "../Upload/Transaction/UploadTransaction.png"
        imgprvw.Alt = "Upload Transaction"
        ViewState("StrfileNameUP") = True
    End Sub

    Protected Sub btnreset_Click(sender As Object, e As System.EventArgs) Handles btnreset.Click
        DDLCompany.SelectedValue = "0"
        DDLTransaction.SelectedValue = "0"
        ltEmbed.Text = ""
        listofuploadedfiles.Text = String.Empty
        imgprvw.Attributes.Add("display", "inline")
        imgprvw.Src = "../Upload/Transaction/UploadTransaction.png"
        imgprvw.Alt = "Upload Transaction"
        ViewState("StrfileNameUP") = True

    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If ViewState("TxtFinancialYearFrom") = Nothing Then
            Dim StrMsg As String = "Please Set Financial Year"
            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('" & StrMsg & "');</script>")
            Exit Sub
        End If
        Dim StrInsQry As New StringBuilder
        If ViewState("FileNames") = "" Then
            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Please upload image first');</script>")
            ViewState("FileNames") = "Upload/Transaction/Thumb/UploadTransaction.png"
            Exit Sub
        End If
        Dim TxtRpt As DateTime
        Dim usDtfi As DateTimeFormatInfo = New CultureInfo("en-US", False).DateTimeFormat
        TxtRpt = Convert.ToDateTime(TxtTransactionDt.Text.ToString(), usDtfi)
        Dim finstartdate As DateTime
        Dim usDtfin As DateTimeFormatInfo = New CultureInfo("en-US", False).DateTimeFormat
        finstartdate = Convert.ToDateTime(ViewState("TxtFinancialYearFrom").ToString(), usDtfin)
        If TxtRpt.Date < finstartdate.Date Then
            Dim StrMsg As String = "Upload is supported for transaction equal to or greater than : " & finstartdate
            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('" & StrMsg & "');</script>")
            Exit Sub
        End If

        Dim StrTTValue As String = String.Empty
        If RdPUR.Checked = True Then StrTTValue = RdPUR.Value
        If RdSL.Checked = True Then StrTTValue = RdSL.Value
        If RdRCPT.Checked = True Then StrTTValue = RdRCPT.Value
        If RdPAY.Checked = True Then StrTTValue = RdPAY.Value
        If RdJE.Checked = True Then StrTTValue = RdJE.Value

        Dim StrLngId As String = String.Empty
        Dim Names As String() = {"IDCompany", "idTransactionType", "UploadTransactionFileName", "UploadTransactionID", "IdUser", "TransactionRemark", "TransactionTags", "TransactionDate"}
        Dim Values As String() = {DDLCompany.SelectedValue, StrTTValue, ViewState("StrfileNameUP"), TxtTransactionId.Text.Trim, Session("IDUser"), TxtTransactionRemark.Value.Trim, TxtTransactionTags.Value.Trim, TxtRpt}
        StrLngId = DirectCast(clsObj.fnInsertUpdate("P_UploadTransaction", Names, Values), String)

        'Below code added by kishore for multiple file upload on 7th march18
        Names = {"IdUploadTransaction", "UploadTransactionFileName"}
        Values = {StrLngId, ViewState("FileNames").ToString()}
        StrLngId = DirectCast(clsObj.fnInsertUpdate("P_UploadTransactionDetails", Names, Values), String)
        If StrLngId.Length > 0 Then
            HdnIdentity.Value = String.Empty.ToString()
            If IsNumeric(StrLngId) = True Then
                If CInt(StrLngId) > 0 Then
                    Response.Redirect("AspxUploadTransaction.aspx?status=true")
                Else
                    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Upload Transaction Created'); location.href='AspxUploadTransaction.aspx?status=true';</script>")
                End If
            Else
                ' StrLngId = "alert('" & StrLngId & "')"
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "k1", StrLngId, True)
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Upload Transaction Updated'); location.href='AspxUploadTransaction.aspx?status=true';</script>")
            End If
        Else
            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Upload Transaction Created'); location.href='AspxUploadTransaction.aspx?status=true';</script>")
        End If
    End Sub

    Protected Sub DDLCompany_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLCompany.SelectedIndexChanged
        fnFillDDLchange()
    End Sub

    Private Sub fnFillDDLchange()
        If DDLCompany.SelectedValue > 0 Then
            Dim StrCompDetails As String = String.Empty
            StrCompDetails = " Select TxtFinancialYearFrom from VwCompDetails where idClient = " & DDLCompany.SelectedValue
            Dim clsobj As New clsData
            Dim DtsCompDetails As New DataSet
            DtsCompDetails = clsobj.fnDataSet(StrCompDetails, 0, "CompDetails")
            If Not IsNothing(DtsCompDetails) Then
                If DtsCompDetails.Tables.Count > 0 Then
                    If DtsCompDetails.Tables(0).Rows.Count > 0 Then
                        ViewState("TxtFinancialYearFrom") = DtsCompDetails.Tables(0).Rows(0)("TxtFinancialYearFrom").ToString
                    End If
                End If
            End If
        End If
    End Sub

    'Private Sub RdPUR_CheckedChanged(sender As Object, e As EventArgs) Handles RdPUR.CheckedChanged
    '    If RdPUR.Checked = True Then ViewState("TT") = "PUR"
    'End Sub

    'Private Sub RdSL_CheckedChanged(sender As Object, e As EventArgs) Handles RdSL.CheckedChanged
    '    If RdSL.Checked = True Then ViewState("TT") = "SL"
    'End Sub

    'Private Sub RdRCPT_CheckedChanged(sender As Object, e As EventArgs) Handles RdRCPT.CheckedChanged
    '    If RdRCPT.Checked = True Then ViewState("TT") = "RCPT"
    'End Sub

    'Private Sub RdPAY_CheckedChanged(sender As Object, e As EventArgs) Handles RdPAY.CheckedChanged
    '    If RdPAY.Checked = True Then ViewState("TT") = "PAY"
    'End Sub

    'Private Sub RdJE_CheckedChanged(sender As Object, e As EventArgs) Handles RdJE.CheckedChanged
    '    If RdJE.Checked = True Then ViewState("TT") = "JE"
    'End Sub



    'Protected Sub BtnFTP_Click(sender As Object, e As System.EventArgs) Handles BtnFTP.Click
    '    'FTP Server URL.
    '    Dim ftp As String = "ftp://103.233.76.155/accountsapp"

    '    'FTP Folder name. Leave blank if you want to upload to root folder.
    '    Dim ftpFolder As String = "Upload/"

    '    Dim fileBytes As Byte() = Nothing

    '    ''file.txt
    '    ''Create Request To Upload File'
    '    'Dim wrUpload As FtpWebRequest = DirectCast(WebRequest.Create("ftp://103.233.76.155/accountsapp/upload/"), FtpWebRequest)
    '    'Read the FileName and convert it to Byte array.
    '    Dim fileName As String = Path.GetFileName(flupload.FileName)
    '    Using fileStream As New StreamReader(flupload.PostedFile.InputStream)
    '        fileBytes = Encoding.UTF8.GetBytes(fileStream.ReadToEnd())
    '        fileStream.Close()
    '    End Using

    '    Try
    '        'Create FTP Request.
    '        Dim request As FtpWebRequest = DirectCast(WebRequest.Create(ftp & ftpFolder & fileName), FtpWebRequest)
    '        request.Method = WebRequestMethods.Ftp.UploadFile

    '        'Enter FTP Server credentials.
    '        request.Credentials = New NetworkCredential("sibzaccountftp", "sibZ@1234")
    '        request.ContentLength = fileBytes.Length
    '        request.UsePassive = True
    '        request.UseBinary = True
    '        request.ServicePoint.ConnectionLimit = fileBytes.Length
    '        request.EnableSsl = False

    '        Using requestStream As Stream = request.GetRequestStream()
    '            requestStream.Write(fileBytes, 0, fileBytes.Length)
    '            requestStream.Close()
    '        End Using

    '        Dim response As FtpWebResponse = DirectCast(request.GetResponse(), FtpWebResponse)

    '        'lblMessage.Text &= fileName & " uploaded.<br />"
    '        response.Close()
    '    Catch ex As WebException
    '        Throw New Exception(TryCast(ex.Response, FtpWebResponse).StatusDescription)
    '    End Try
    'End Sub
End Class
