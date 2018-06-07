Imports System.Data

Partial Class AspxPages_AspxTransactionImageReviewer
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            HdnIdentity.Value = Request.QueryString("SearchKey")
            fnFillScreen()
        End If
    End Sub

    Private Sub fnFillScreen()
        Dim DtsSearchData As New DataSet
        Dim StrQrySearch As New StringBuilder
        StrQrySearch.Append(" Select TranImage, IdTransactionType,TransactionType, idUPloadTransaction, Remark ")
        StrQrySearch.Append(" from VwFillImageScreen ")
        StrQrySearch.Append(" where idUPloadTransaction = " & HdnIdentity.Value & "")
        Dim clsobj As New clsData
        DtsSearchData = clsobj.fnDataSet(StrQrySearch.ToString, CommandType.Text)
        clsobj = Nothing
        If Not IsNothing(DtsSearchData) Then
            If DtsSearchData.Tables.Count > 0 Then
                If DtsSearchData.Tables(0).Rows.Count > 0 Then
                    imgprvw.Src = DtsSearchData.Tables(0).Rows(0)("TranImage")
                    imgprvw.Attributes.Add("title", Replace(DtsSearchData.Tables(0).Rows(0)("TranImage"), "../Upload/Transaction/", ""))
                    imgprvw.Alt = Replace(DtsSearchData.Tables(0).Rows(0)("TranImage"), "../Upload/Transaction/", "")
                    Dim ImgPart() As String = DtsSearchData.Tables(0).Rows(0)("TranImage").Split("/")
                    HdnIDImage.Value = ImgPart(3).Substring(0, ImgPart(3).Length - 4)
                    lblPrevRemark.InnerText = DtsSearchData.Tables(0).Rows(0)("Remark")
                End If
            End If
        End If
    End Sub

    Protected Sub btnReview_Click(sender As Object, e As System.EventArgs) Handles btnReview.Click
        Dim IntCntr As Integer = 0
        Dim StrAllRemark As String = String.Empty
        If ddlStatus.SelectedValue = "2" Then
            Dim StrRejectCntrQry As String = "Select isnull(RejectCount,0) RejectCount from tbl_uploadTransaction  where idUPloadTransaction = " & HdnIdentity.Value & ""
            Dim dtsRejectCntr As New DataSet
            Dim clsObjRjCntr As New clsData
            dtsRejectCntr = clsObjRjCntr.fnDataSet(StrRejectCntrQry, 0)
            If Not IsNothing(dtsRejectCntr) Then
                If dtsRejectCntr.Tables.Count > 0 Then
                    If dtsRejectCntr.Tables(0).Rows.Count > 0 Then
                        IntCntr = dtsRejectCntr.Tables(0).Rows(0)("RejectCount")
                    End If
                End If
            End If
            IntCntr = IntCntr + 1
            StrAllRemark = lblPrevRemark.InnerText & vbCr & txtRemark.Value
        End If

        Dim SqlQry As String = "Update tbl_uploadTransaction set ReviewStatus=" & ddlStatus.SelectedValue & ", RejectCount= " & IntCntr & ", Remark='" & StrAllRemark & "', TaskReviewBy = " & Session("IDUser").ToString & " where idUPloadTransaction = " & HdnIdentity.Value & ""
        Dim StrExp As String = String.Empty
        Dim clsObj As New clsData
        clsObj.fnExecute(SqlQry, StrExp, 0)
        clsObj = Nothing
        Response.Redirect("AspxSearchList.aspx?idMenu=" & Session("IdMenu"), False)
    End Sub
End Class
