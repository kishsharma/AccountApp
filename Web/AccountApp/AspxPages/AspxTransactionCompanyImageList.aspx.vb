Imports System.Data

Partial Class AspxPages_AspxTransactionCompanyImageList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            If IsNothing(Request.QueryString("Companyid")) Then
                Response.Redirect("~/Default.aspx", False)
                Exit Sub
            End If
            ViewState("Companyid") = Request.QueryString("Companyid")
            ViewState("UserID") = Request.QueryString("UserID")
            ViewState("DataForDate") = Request.QueryString("DataForDate")
            ViewState("Type") = Request.QueryString("Type")
            fnFillDDL()
            fnFillList()
        End If
    End Sub

    Private Sub fnFillDDL()
        Dim clsodbcObj As New clsData
        Dim StrDDLListQry As String = String.Empty
        clsodbcObj = New clsData
        StrDDLListQry = " select '0' id,'All' data1 Union All Select idDefination id, definationcode +' - ' + DefinationDesc Data1 from tbl_Defination where idDefType = 'TT' "
        clsodbcObj.fnDropDownFill(StrDDLListQry, DDLTransaction, "data1", "id")
        clsodbcObj = Nothing
    End Sub


    Private Sub fnFillList()
        Dim StrSQL As New StringBuilder
        If CInt(Session("IDRole").ToString) <= 2 Then
            StrSQL.Append("")
            StrSQL.Append(" Select case when isnull(VwFillImageScreen.ReviewStatus,0)=2 then 'Red' ELSE 'White' end as 'StatusColor', ")
            StrSQL.Append(" concat('<a href=""AspxTransactionImage.aspx?SearchKey=',CAST(VwFillImageScreen.idUPloadTransaction as char(10)),'"">', TT.DefinationCode + ' - ' + TT.DefinationDesc +  ' - ' + TransactionId ,'</a>') As 'Click', ")
            StrSQL.Append(" TxtCompanyName ClientName,  UserName + '  -  ' + UserFullName 'Alloted To User',tbl_TaskAllotment.AllotmentDate as 'Allotment date'  ")
            StrSQL.Append(" from tbl_TaskAllotment ")
            StrSQL.Append(" Left outer join VwFillImageScreen on VwFillImageScreen.IdUpTransDetail = tbl_TaskAllotment.IdTransactionimageId ")
            StrSQL.Append(" Left outer join tbl_defination TT on TT.idDefination = VwFillImageScreen.idTransactionType ")
            StrSQL.Append(" Left outer join Vw_ClientMaster on ClientId = VwFillImageScreen.idCompany ")
            StrSQL.Append(" Left Outer Join tbl_user on tbl_user.idUser = tbl_TaskAllotment.idUser ")
            StrSQL.Append(" where 1=1 ")
            ''StrSQL.Append("  and VwFillImageScreen.transactioncompleted is null ")
            StrSQL.Append(" and (VwFillImageScreen.transactioncompleted is null  or VwFillImageScreen.ReviewStatus = 2) ")
            If DDLTransaction.SelectedValue > 0 Then
                StrSQL.Append(" and idTransactionType = " & DDLTransaction.SelectedValue)
            End If
            StrSQL.Append("  and Transactionstatus = 0 and isnull(tbl_TaskAllotment.idRejectedAllotment,0) =0 ")
            'StrSQL.Append(" and VwFillImageScreen.idCompany = '" & ViewState("Companyid").ToString & "'")
            Select Case ViewState("Type").ToString
                Case "C"
                    StrSQL.Append(" and VwFillImageScreen.idCompany = '" & ViewState("Companyid").ToString & "'")
                Case "D"
                    StrSQL.Append(" and convert(varchar(20), tbl_TaskAllotment.AllotmentDate, 106) = '" & ViewState("DataForDate").ToString & "' ")
                Case "U"
                    'If Session("IDUser").ToString = ViewState("UserID").ToString Then
                    StrSQL.Append(" and tbl_TaskAllotment.idUser = '" & ViewState("UserID").ToString & "'")
                    'End If
                Case Else
            End Select
        Else
            StrSQL.Append("")
            StrSQL.Append(" Select case when isnull(VwFillImageScreen.ReviewStatus,0)=2 then 'Red' ELSE 'White' end as 'StatusColor', ")
            StrSQL.Append(" concat('<a href=""AspxTransactionImage.aspx?SearchKey=',CAST(VwFillImageScreen.IdUpTransDetail as char(10)),'"">', TT.DefinationCode + ' - ' + TT.DefinationDesc +  ' - ' + TransactionId ,'</a>') As 'Click', ")
            StrSQL.Append(" TxtCompanyName ClientName,  UserName + '  -  ' + UserFullName 'Alloted To User',tbl_TaskAllotment.AllotmentDate as 'Allotment date'  ")
            StrSQL.Append(" from tbl_TaskAllotment ")
            StrSQL.Append(" Left outer join VwFillImageScreen on VwFillImageScreen.IdUpTransDetail = tbl_TaskAllotment.IdTransactionimageId ")
            StrSQL.Append(" Left outer join tbl_defination TT on TT.idDefination = VwFillImageScreen.idTransactionType ")
            StrSQL.Append(" Left outer join Vw_ClientMaster on ClientId = VwFillImageScreen.idCompany ")
            StrSQL.Append(" Left Outer Join tbl_user on tbl_user.idUser = tbl_TaskAllotment.idUser ")
            StrSQL.Append(" where 1=1 ")
            ''StrSQL.Append("  and VwFillImageScreen.transactioncompleted is null ")
            StrSQL.Append(" and (VwFillImageScreen.transactioncompleted is null  or VwFillImageScreen.ReviewStatus = 2) ")
            If DDLTransaction.SelectedValue > 0 Then
                StrSQL.Append(" and idTransactionType = " & DDLTransaction.SelectedValue)
            End If
            StrSQL.Append(" and Transactionstatus = 0 and isnull(tbl_TaskAllotment.idRejectedAllotment,0) =0 ")
            StrSQL.Append(" and VwFillImageScreen.idCompany = '" & ViewState("Companyid").ToString & "'")
            StrSQL.Append(" and convert(varchar(20), tbl_TaskAllotment.AllotmentDate, 106) = '" & ViewState("DataForDate").ToString & "' ")
            StrSQL.Append(" and tbl_TaskAllotment.idUser = '" & ViewState("UserID").ToString & "'")
        End If
        If Not IsNothing(StrSQL.ToString) Then
            Dim objCls As New clsData
            Dim dtsGrd As New DataSet
            dtsGrd = objCls.fnDataSet(StrSQL.ToString, 1, 1)
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

    Protected Sub DDLTransaction_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLTransaction.SelectedIndexChanged
        fnFillList()
    End Sub
End Class
