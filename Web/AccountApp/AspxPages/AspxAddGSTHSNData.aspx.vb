
Partial Class AspxPages_AspxAddGSTHSNData
    Inherits System.Web.UI.Page

    Private Sub AspxPages_AspxAddGSTHSNData_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim StrAbsoluteURL As String = HttpContext.Current.Request.Url.AbsoluteUri
        Dim StrPageUrl As String = StrReverse(Mid(StrReverse(StrAbsoluteURL), 1, InStr(StrReverse(StrAbsoluteURL), "/") - 1))
        Dim StrRequiredURL As String = StrAbsoluteURL
        Session("BackPage") = Replace(StrAbsoluteURL, StrPageUrl, "AspxUploadHSNCode.aspx?idMenu=32")
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim StrInsQry As New StringBuilder
        StrInsQry.Append(" insert into tbl_GSTMaster(GSTCode, GSTCodeDescription, CGSTRate, SGSTRate, IGSTRate, CessCondition, GSTRateWEF, BulkUpload)  ")
        StrInsQry.Append("               Values('" & TxtGSTCode.Value.Trim & "', '" & TxtGSTCodeDescription.Value.Trim & "', '" & TxtCGSTRate.Value.Trim & "', '" & TxtSGSTRate.Value.Trim & "', '" & TxtIGSTRate.Value.Trim & "', '" & Replace(TxtCessCondition.Value.Trim, ",", "") & "', '" & TxtGSTRateWEF.Text.Trim & "','0') ")
        Dim StrGSTId As String = String.Empty
        Dim clsObj As New clsData
        StrGSTId = clsObj.fnInsertScopeIdentity(StrInsQry.ToString, "")
        clsObj = Nothing
        If StrGSTId.Length > 0 Then
            If IsNumeric(StrGSTId) = True Then
                If CInt(StrGSTId) > 0 Then
                    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('GST - HSN Code Created Successfully'); location.href='" & Session("BackPage").ToString() & "';</script>")
                End If
            End If
        End If
    End Sub

    'Private Sub BtnBack_Click(sender As Object, e As EventArgs) Handles BtnBack.Click
    '    Response.Redirect("AspxUploadHSNCode.aspx?idMenu=32", False)
    'End Sub
End Class
