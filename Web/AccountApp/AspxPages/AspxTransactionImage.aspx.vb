Imports System.Data
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Partial Class AspxPages_AspxTransactionImage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            HdnIdentity.Value = Request.QueryString("SearchKey")
            fnFillScreen()
        End If
    End Sub

    Private Sub fnFillScreen()
        Dim DtsSearchData As New DataSet
        Dim strGetfileextension As String
        Dim embed As String
        Dim strImagetag As String = "<img class='mySlides' src='[Path]' style='width:100%;'  />"
        Dim strImageTagInnerHtml As String = String.Empty
        Dim strPdfInnerHtml As String = String.Empty
        Dim StrQrySearch As New StringBuilder
        StrQrySearch.Append(" Select TranImage, IdTransactionType,TransactionType, idUPloadTransaction, Remark,IdUpTransDetail ")
        StrQrySearch.Append(" from VwFillImageScreen ")
        StrQrySearch.Append(" where idUPloadTransaction = " & HdnIdentity.Value & "")
        Dim clsobj As New clsData
        DtsSearchData = clsobj.fnDataSet(StrQrySearch.ToString, CommandType.Text)
        clsobj = Nothing
        If Not IsNothing(DtsSearchData) Then
            If DtsSearchData.Tables.Count > 0 Then
                If DtsSearchData.Tables(0).Rows.Count > 0 Then
                    strGetfileextension = DtsSearchData.Tables(0).Rows(0)("TranImage")
                    If strGetfileextension.ToString().Contains(".pdf") Or strGetfileextension.ToString().Contains(".xls") Or strGetfileextension.ToString().Contains(".xlsx") Then
                        imgprvw.Attributes.Add("style", "display:none")
                        embed = "<object data=""{0}"" class='mySlides' type=""application/pdf"" width=""90%"" height=""500px"">"
                        embed += "If you are unable to view file, you can download from <a href = ""{0}"">here</a>"
                        embed += " or download <a target = ""_blank"" href = ""http://get.adobe.com/reader/"">Adobe PDF Reader</a> to view the file."
                        embed += "</object>"
                        ltEmbed.Text = String.Format(embed, ResolveUrl(strGetfileextension))
                    Else
                        imgprvw.Visible = False
                        If DtsSearchData.Tables(0).Rows.Count = 1 Then
                            imgprvw.Visible = True
                            imgprvw.Src = DtsSearchData.Tables(0).Rows(0)("TranImage")
                            imgprvw.Attributes.Add("title", Replace(DtsSearchData.Tables(0).Rows(0)("TranImage"), "../Upload/Transaction/", ""))
                            imgprvw.Alt = Replace(DtsSearchData.Tables(0).Rows(0)("TranImage"), "../Upload/Transaction/", "")
                        Else
                            Dim ImgPart() As String = DtsSearchData.Tables(0).Rows(0)("TranImage").Split("/")
                            HdnIDImage.Value = ImgPart(3).Substring(0, ImgPart(3).Length - 4)
                            lblReViewRemark.Text = DtsSearchData.Tables(0).Rows(0)("Remark")
                            'LblBuyerName.InnerText = DtsSearchData.Tables(0).Rows(0)("ClientName")
                            'LblBuyerAddress.InnerText = DtsSearchData.Tables(0).Rows(0)("ClientAddress")
                            'LblECCNo.InnerText = "ECC No.:" & DtsSearchData.Tables(0).Rows(0)("TxtECCNO")
                            'LblCSTTINNO.InnerText = "CST TIN No.:" & DtsSearchData.Tables(0).Rows(0)("TxtCSTTINNO")
                            'LblVATTINNO.InnerText = "VAT TIN No.:" & DtsSearchData.Tables(0).Rows(0)("TxtVATTINNO")
                            For Each raw As DataRow In DtsSearchData.Tables(0).Rows
                                If raw("TranImage").ToString().Contains(".pdf") Then
                                    embed = embed.Replace("{0}", raw("TranImage").ToString())
                                    strPdfInnerHtml = strPdfInnerHtml + embed
                                Else
                                    strImageTagInnerHtml = strImageTagInnerHtml + "<img class='mySlides' id='[id]' src='[Path]' data-rotate='0' style='width:80%;' />".Replace("[Path]", raw("TranImage")).Replace("[id]", Request.QueryString("SearchKey") + "_" + raw("IdUpTransDetail").ToString())
                                End If

                            Next
                            divImageSlider.InnerHtml = divImageSlider.InnerHtml + strImageTagInnerHtml + strPdfInnerHtml

                        End If
                    End If

                End If
            End If
        End If

    End Sub

    Protected Sub BtnRequestNewImage_Click(sender As Object, e As System.EventArgs) Handles BtnRequestNewImage.Click
        Dim Names As String() = {"idUPloadTransaction", "RejectedByUserId", "RejectReason"}
        Dim Values As String() = {HdnIdentity.Value, Session("IDUser"), TxtRejectRemark.Text.Trim}
        Dim StrLngId As String = String.Empty
        Dim clsObj As New clsData
        StrLngId = DirectCast(clsObj.fnInsertUpdate("P_RejectAllotment", Names, Values), String)
        clsObj = Nothing
        ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Image Rejection is Informed to Admin'); location.href='AspxTransactionImageList.aspx?idMenu=20';</script>")
        'Response.Redirect("AspxTransactionList.aspx?SearchKey=" & HdnIdentity.Value, False)
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim StrUpdate As String = " Update tbl_uploadtransaction Set transactionCompleted = 1, TransactionCurrentStatus = 1, ReviewStatus = null, TaskCompletedBy = " & Session("IDUser").ToString & " where idUPloadTransaction =" & HdnIdentity.Value
        Dim clsObj As New clsData
        If clsObj.fnIUD(StrUpdate, 1, "NonQuery") = True Then
            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Purchase Added'); location.href='" & Session("BackPage").ToString() & "';</script>")
            Response.Redirect("AspxTransactionImageList.aspx?idMenu=" & Session("IdMenu"), False)
        Else

        End If
    End Sub


End Class
