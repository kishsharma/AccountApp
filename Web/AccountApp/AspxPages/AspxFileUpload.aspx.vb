Imports System.IO
Imports System.Data

Partial Class AspxPages_AspxFileUpload
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            fnFillDDL()
            'fnFillList()
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
        StrDDLListQry = " select '0' id,'Select' data1 Union All select idDefination, DefinationDesc from tbl_Defination Where idDefType = 'Roles'"
        clsodbcObj.fnDropDownFill(StrDDLListQry, DDLIDRole, "data1", "id")
        clsodbcObj = Nothing
    End Sub

    'fnUpload is use to upload the file and also to View the image 
    Protected Sub fnUpload(ByVal sender As Object, ByVal e As EventArgs)
        If fnValidate() = False Then
            Exit Sub
        End If
        
        If flupload.HasFile Then
            Dim strpath As String = Path.GetExtension(flupload.FileName)
            Dim fileName As String = Replace(flupload.FileName, " ", "") ''Replace(Replace(Replace(Replace(Replace(DDLCompany.SelectedItem.Text, " ", "", ), ",", ""), "/", ""), "\", "") & "." & Path.GetExtension(flupload.PostedFile.FileName), "..", ".") ' Path.GetFileName(flupload.PostedFile.FileName)
            'If strpath.ToLower() = ".pdf" AndAlso strpath.ToLower() = ".xls" AndAlso strpath.ToLower() = ".xlsx" Then
            flupload.PostedFile.SaveAs(Server.MapPath("~/Upload/Files/Reports/") + fileName)
            ViewState("fileName") = "Upload/Files/Reports/" + fileName
            Dim objCls As New clsData
            Session("SearchKey") = DDLCompany.SelectedValue
            ''Dim StrSQL As String = "update tbl_client set document='" & ViewState("fileName").ToString() & "' where idClient= " & Session("SearchKey")
            Dim StrSQL As String = " Insert into Tbl_UploadReports(UploadReportName,                   UploadForClient,               UploadForClientRole, UploadedDateTime,                          UploadFileName, UploadedBy, UploadRemark) "
            StrSQL = StrSQL & "           Values('" & TxtReportName.Value.Trim & "','" & DDLCompany.SelectedValue & "', '" & DDLIDRole.SelectedValue & "',        getdate(),'" & ViewState("fileName").ToString & "', '" & Session("IDUser").ToString & "','" & txtRemark.Value.Trim & "'    )"
            Dim StrLngId As String = "0"
            StrLngId = objCls.fnInsertScopeIdentity(StrSQL.ToString, "NonQuery")
            If StrLngId <> "0" Then
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('File Uploaded succesfully');</script>")
                fnFillList()
            Else
                Dim path As String = Server.MapPath("~/Upload/Files/Reports/") + fileName
                Dim file As New FileInfo(path)
                If file.Exists Then
                    file.Delete()
                End If
            End If
            objCls = Nothing
            'Else
            '    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Please Upload pdf/excel files');</script>")
            'End If
        End If
    End Sub
    'fnClear is use to clear the image
    Protected Sub fnClear(ByVal sender As Object, ByVal e As EventArgs)
        ViewState("fileName") = "Resource/images/User.png"
    End Sub
    
#End Region

    Protected Sub DDLIDRole_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLIDRole.SelectedIndexChanged
        Dim strComp As String = "2,3"
        Companylist.Attributes.Add("style", "display:block")
        If Not strComp.Contains(DDLIDRole.SelectedValue) Then
            Companylist.Attributes.Add("style", "display:none")
        End If

    End Sub
    Private Sub fnFillList()
        Dim StrSQL As String = "select idUploadReports, UploadReportName, txtCompanyName UploadForClient, UploadedDateTime, "
        StrSQL = StrSQL & " '<a class=""btn btn-primary"" target=""_blank"" href=""../'+cast(UploadFileName as varchar(max))+'"">'+ replace(UploadFileName,'Upload/Files/Reports/','') + '</a>' as 'Download', "
        StrSQL = StrSQL & " tbl_user.UserFullName as 'UploadedBy', UploadRemark "
        StrSQL = StrSQL & " from Tbl_UploadReports "
        StrSQL = StrSQL & " Left outer join tbl_User on UploadedBy = tbl_user.idUser "
        StrSQL = StrSQL & " Left outer join tbl_Client on UploadForClient = tbl_Client.IDClient "
        StrSQL = StrSQL & " Where UploadForClient in (" & DDLCompany.SelectedValue & ",0)"
        ViewState("strsql") = StrSQL
        If Not IsNothing(ViewState("strsql")) Then
            Dim objCls As New clsData
            Dim dtsGrd As New DataSet
            dtsGrd = objCls.fnDataSet(ViewState("strsql"), 1, 1)
            If Not IsNothing(dtsGrd) Then
                If dtsGrd.Tables.Count > 0 Then
                    If dtsGrd.Tables(0).Rows.Count > 0 Then
                        Dim ObjClsCmn As New ClsCommonFunction
                        LblSearchData.Text = ObjClsCmn.DataTableToHTMLTable(dtsGrd.Tables(0), "DataList", "table table-striped table-bordered", "0", "100%", True)
                        ObjClsCmn = Nothing
                        'BtnAddHREF.Visible = False
                    Else
                        LblSearchData.Text = "<table><tr><td>No Record(s) Found</td></tr></table>"
                    End If
                Else
                    LblSearchData.Text = "<table><tr><td>No Record(s) Found</td></tr></table>"
                End If
            Else
                LblSearchData.Text = "<table><tr><td>No Record(s) Found</td></tr></table>"
            End If
            objCls = Nothing
        End If
    End Sub

    Protected Sub DDLCompany_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLCompany.SelectedIndexChanged
        fnFillList()
    End Sub
    Private Function fnValidate() As Boolean
        If TxtReportName.Value.ToString.Trim = String.Empty Then
            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Please Provide Report Name');</script>")
            Return False
        End If
        'If DDLCompany.SelectedValue = 0 Then
        '    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Please Select Company');</script>")
        '    Return False
        'End If
        Dim fileName As String = Replace(flupload.FileName, " ", "")
        fileName = "Upload/Files/Reports/" + fileName
        Dim strEmailIdCheck As String = "select UploadFileName from Tbl_UploadReports where UploadFileName = '" & fileName & "'"
        Dim clsObj As New clsData
        If clsObj.fnStrRetrive(strEmailIdCheck, 1).Length > 0 Then
            ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script type='text/javascript' language='javascript'>alert('File Already Available...');</script>")
            Return False
        End If
        Return True
    End Function
End Class
