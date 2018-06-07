Imports System.Data
Partial Class AspxPages_AspxAssignImages
    Inherits System.Web.UI.Page

    Dim dtstSearchDtls As DataSet
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Hdnstatus.Value = Request.QueryString("SearchKey")
            HdnTrnType.Value = Request.QueryString("TType")
            fnFillDDL()
            DDLCompanyname.SelectedValue = Hdnstatus.Value
            DDLTransactiontype.SelectedValue = HdnTrnType.Value
            DDLTransactiontype_SelectedIndexChanged(sender, e)
            If IsNothing(Request.QueryString("status")) Then

            Else
                Dim strStatu As String = Request.QueryString("status")
                If strStatu = "true" Then
                    ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Task Added Successfully');</script>")
                End If
            End If


        End If

        btnSave.Attributes.Add("onclick", "return selectE2();")
    End Sub
    'Private Sub loadimage()
    '    Dim clsodbcObj As New clsData
    '    Dim StrDDLListQry As String = "select '../' + UploadTransactionFileName as 'UploadTransactionFileName' from tbl_TaskAllotment inner join tbl_UPloadTransaction on IdTransactionimageId=idUPloadTransaction where"
    '    If DDLCompanyname.SelectedValue > 0 Then
    '        StrDDLListQry = StrDDLListQry & " IDCompany=" & DDLCompanyname.SelectedValue
    '    End If
    '    If DDLTransactiontype.SelectedValue > 0 Then
    '        StrDDLListQry = StrDDLListQry & " and idTransactionType= " & DDLTransactiontype.SelectedValue
    '    End If
    '    If DDLUsername.SelectedValue > 0 Then
    '        StrDDLListQry = StrDDLListQry & " and tbl_TaskAllotment.Iduser=" & DDLUsername.SelectedValue & " "
    '    End If
    '    dtstSearchDtls = clsodbcObj.fnDataSet(StrDDLListQry, 1, 1)
    '    Dim pStr As String = "<nav class='slidernav'>" & vbLf
    '    pStr = pStr & "<div id='navbtns' class='clearfix'>" & vbLf
    '    pStr = pStr & "<a href='#' class='previous' style='float:left;'><img src='../Resource/images/blue_btn2.png'></a>" & vbLf
    '    pStr = pStr & "<a href='#' class='next' style='float:right'><img src='../Resource/images/blue_btn1.png'></a>" & vbLf
    '    pStr = pStr & "</div>" & vbLf
    '    pStr = pStr & "</nav>" & vbLf
    '    pStr = pStr & "<div class='crsl-items' data-navigation='navbtns' >" & vbLf
    '    pStr = pStr & "<div class='crsl-wrap'>" & vbLf
    '    If Not IsNothing(dtstSearchDtls) Then
    '        If dtstSearchDtls.Tables.Count > 0 Then
    '            If dtstSearchDtls.Tables(0).Rows.Count > 0 Then
    '                For Each row As DataRow In dtstSearchDtls.Tables(0).Rows
    '                    pStr = pStr & "<div class='crsl-item'>" & vbLf
    '                    pStr = pStr & "<div class='panel'>" & vbLf
    '                    pStr = pStr & "<img src='" & row("UploadTransactionFileName").ToString & "' />" & vbLf
    '                    pStr = pStr & "<div class='enlarge_box'>" & vbLf
    '                    pStr = pStr & "<img src='" & row("UploadTransactionFileName").ToString & "' />" & vbLf
    '                    pStr = pStr & "</div>"
    '                    pStr = pStr & "</div>"
    '                    pStr = pStr & "</div>"
    '                Next
    '            End If
    '        End If
    '    End If
    '    pStr = pStr & "</div>"
    '    pStr = pStr & "</div>"
    '    mygallery.InnerHtml = pStr
    'End Sub
    Private Sub fnFillDDL()
        Dim clsodbcObj As New clsData
        Dim StrDDLListQry As String = " select '0' id,'All Company' data1 Union All select ClientId, txtCompanyName ClientName from Vw_ClientMaster  where RecordStatus = 0  "
        clsodbcObj.fnDropDownFill(StrDDLListQry, DDLCompanyname, "data1", "id")
        clsodbcObj = Nothing
        clsodbcObj = New clsData
        StrDDLListQry = " select '0' id,'Select' data1 Union All Select idDefination idTrnType, DefinationDesc TransactionType from tbl_Defination where idDefType = 'TT' "
        clsodbcObj.fnDropDownFill(StrDDLListQry, DDLTransactiontype, "data1", "id")
        clsodbcObj = Nothing
        clsodbcObj = New clsData

        'Hardcoded with Role Id 4 because we need only those user who has Role as Computer Operator
        StrDDLListQry = " select '0' id,'Select' data1 Union All Select idUser as id, UserName + ' - ' + UserFullName as data1 from tbl_User where IdRole =4  and RecordStatus = 0  "
        clsodbcObj.fnDropDownFill(StrDDLListQry, DDLUsername, "data1", "id")
        clsodbcObj = Nothing

    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click

        Dim strAllocatedDrivers As String = Me.strDrivers.Value.ToString
        Dim clsodbcObj As New clsData
        Dim StrLngId As String = String.Empty
        Dim Names As String() = {"Iduser", "IdTransactionimageId"}
        Dim Values As String() = {DDLUsername.SelectedValue, Convert.ToString(Replace(strAllocatedDrivers, "'", ""))}
        StrLngId = DirectCast(clsodbcObj.fnInsertUpdate("P_TaskAllotment", Names, Values), String)
        If StrLngId.Length > 0 Then
            strDrivers.Value = String.Empty.ToString()
            If IsNumeric(StrLngId) = True Then
                If CInt(StrLngId) > 0 Then
                    'ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Task Added Successfully');</script>")
                    Response.Redirect("AspxAssignImages.aspx?status=true")
                Else
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "RegisterClientScriptBlock", "document.write ('" & StrLngId & "');", True)
                End If
            Else
                StrLngId = "alert('" & StrLngId & "')"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "k1", StrLngId, True)
            End If
        Else
            ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Task Added Successfully'); location.href='AspxAssignImages.aspx?status=true';</script>")
        End If
    End Sub

    Protected Sub DDLCompanyname_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLCompanyname.SelectedIndexChanged
        'If DDLCompanyname.SelectedValue > 0 Then
        lstUnassignedimage.Items.Clear()
        lstUnassignedimage.DataSource = Nothing
        lstUnassignedimage.DataBind()
        lstAssignedimage.Items.Clear()
        lstAssignedimage.DataSource = Nothing
        lstAssignedimage.DataBind()
        'If DDLCompanyname.SelectedValue > 0 Then
        '    fnFillTransactionImage()
        '    loadimage()
        'End If
    End Sub
    Protected Sub DDLTransactiontype_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLTransactiontype.SelectedIndexChanged
        'If DDLTransactiontype.SelectedValue > 0 Then
        lstUnassignedimage.Items.Clear()
        lstUnassignedimage.DataSource = Nothing
        lstUnassignedimage.DataBind()
        lstAssignedimage.Items.Clear()
        lstAssignedimage.DataSource = Nothing
        lstAssignedimage.DataBind()
        If DDLTransactiontype.SelectedValue > 0 Then
            fnFillTransactionImage()
            'loadimage()
        End If
    End Sub

    Protected Sub DDLUsername_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLUsername.SelectedIndexChanged
        lstUnassignedimage.Items.Clear()
        lstUnassignedimage.DataSource = Nothing
        lstUnassignedimage.DataBind()
        lstAssignedimage.Items.Clear()
        lstAssignedimage.DataSource = Nothing
        lstAssignedimage.DataBind()
        If DDLUsername.SelectedValue > 0 Then
            fnFillTransactionImage()
            'loadimage()
        End If
    End Sub

    Private Sub fnFillTransactionImage()
        Dim StrDDLListQry As String = "select id,  data1 "
        StrDDLListQry = StrDDLListQry & " from Vw_UnassignedImages "
        StrDDLListQry = StrDDLListQry & "   Where 1 = 1    "
        If DDLCompanyname.SelectedValue > 0 Then
            StrDDLListQry = StrDDLListQry & " And idCompany =  " & DDLCompanyname.SelectedValue
        End If
        If DDLTransactiontype.SelectedValue > 0 Then
            StrDDLListQry = StrDDLListQry & " And IDTransactionType =  " & DDLTransactiontype.SelectedValue
        End If
        If ChkOnlyPDF.Checked = True Then
            StrDDLListQry = StrDDLListQry & " And UploadTransactionFileName  Like '%.pdf%' "
        End If
        StrDDLListQry = StrDDLListQry & " order by id "
        Dim clsodbcObj As New clsData
        clsodbcObj.fnListBoxFill(StrDDLListQry, lstUnassignedimage, "data1", "id")
        clsodbcObj = Nothing
        StrDDLListQry = "  select id, data1, idcompany, IDTransactionType, Iduser from Vw_AssignedImages "
        StrDDLListQry = StrDDLListQry & " where 1 = 1 and transactionCompleted =0  "
        StrDDLListQry = StrDDLListQry & " from tbl_TaskAllotment  "
        StrDDLListQry = StrDDLListQry & " where 1 = 1 and transactionCompleted =0 "
        If DDLCompanyname.SelectedValue > 0 Then
            StrDDLListQry = StrDDLListQry & " And idCompany =  " & DDLCompanyname.SelectedValue
        End If
        If DDLTransactiontype.SelectedValue > 0 Then
            StrDDLListQry = StrDDLListQry & " And IDTransactionType =  " & DDLTransactiontype.SelectedValue
        End If
        If DDLUsername.SelectedValue > 0 Then
            StrDDLListQry = StrDDLListQry & " And Iduser = " & DDLUsername.SelectedValue & " "
        End If
        If ChkOnlyPDF.Checked = True Then
            StrDDLListQry = StrDDLListQry & " And VwFillImageScreen.UploadTransactionFileName Like '%.pdf%' "
        End If
        StrDDLListQry = StrDDLListQry & " order by id "
        clsodbcObj = New clsData
        clsodbcObj.fnListBoxFill(StrDDLListQry, lstAssignedimage, "data1", "id")
        clsodbcObj = Nothing

    End Sub


    Protected Sub ChkOnlyPDF_ServerChange(sender As Object, e As System.EventArgs) Handles ChkOnlyPDF.ServerChange
        fnFillTransactionImage()
    End Sub
End Class
