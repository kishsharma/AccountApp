Imports System.Data

Partial Class AspxPages_AspxTransactionCompanyUserList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            ViewState("Companyid") = Request.QueryString("Companyid")
            ViewState("UserID") = Request.QueryString("UserID")
            ViewState("DataForDate") = Request.QueryString("DataForDate")
            ViewState("Type") = Request.QueryString("Type")
            fnFillList()
        End If
    End Sub
    Private Sub fnFillList()
        Dim StrSQL As New StringBuilder
        If CInt(Session("IDRole").ToString) <= 2 Then
            StrSQL.Clear()
            StrSQL.Append(" Select case  when isnull(PM.ReviewStatus,0)=2 then 'Red' ELSE 'White' end as 'StatusColor', ")
            StrSQL.Append(" concat('<a href=""AspxTransactionPurchase.aspx?SearchKey=',CAST(tbl_UPloadTransaction.idUPloadTransaction as char(10)),'"">', TT.DefinationCode + ' - ' + TT.DefinationDesc +  ' - ' + TransactionId ,'</a>') As 'Click', ")
            StrSQL.Append(" TxtCompanyName ClientName,  UserName + '  -  ' + UserFullName 'Alloted To User',tbl_TaskAllotment.AllotmentDate as 'Allotment date'  ")
            StrSQL.Append(" from tbl_TaskAllotment ")
            StrSQL.Append(" Left outer join tbl_UPloadTransaction on tbl_UPloadTransaction.idUPloadTransaction = tbl_TaskAllotment.IdTransactionimageId ")
            StrSQL.Append(" Left outer join tbl_defination TT on TT.idDefination = tbl_UPloadTransaction.idTransactionType ")
            StrSQL.Append(" Left outer join Vw_ClientMaster on ClientId = tbl_UPloadTransaction.idCompany ")
            StrSQL.Append(" Left Outer Join tbl_user on tbl_user.idUser = tbl_TaskAllotment.idUser ")
            StrSQL.Append(" Left Outer Join tbl_PurchaseMaster PM on PM.idUPloadTransaction = tbl_UPloadTransaction.idUPloadTransaction ")
            StrSQL.Append(" where 1=1  and PM.idUPloadTransaction not in ( select idUPloadTransaction from tbl_PurchaseMaster where PM.ReviewStatus in (0,1,3) )  ")
            StrSQL.Append(" and idTransactionType =  6  and Transactionstatus = 0 and isnull(tbl_TaskAllotment.idRejectedAllotment,0) =0 ")
            'StrSQL.Append(" and tbl_UPloadTransaction.idCompany = '" & ViewState("Companyid").ToString & "'")
            Select Case ViewState("Type").ToString
                Case "C"
                    StrSQL.Append(" and tbl_UPloadTransaction.idCompany = '" & ViewState("Companyid").ToString & "'")
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
            StrSQL.Append(" Select case  when isnull(PM.ReviewStatus,0)=2 then 'Red' ELSE 'White' end as 'StatusColor', ")
            StrSQL.Append(" concat('<a href=""AspxTransactionPurchase.aspx?SearchKey=',CAST(tbl_UPloadTransaction.idUPloadTransaction as char(10)),'"">', TT.DefinationCode + ' - ' + TT.DefinationDesc +  ' - ' + TransactionId ,'</a>') As 'Click', ")
            StrSQL.Append(" TxtCompanyName ClientName,  UserName + '  -  ' + UserFullName 'Alloted To User',tbl_TaskAllotment.AllotmentDate as 'Allotment date'  ")
            StrSQL.Append(" from tbl_TaskAllotment ")
            StrSQL.Append(" Left outer join tbl_UPloadTransaction on tbl_UPloadTransaction.idUPloadTransaction = tbl_TaskAllotment.IdTransactionimageId ")
            StrSQL.Append(" Left outer join tbl_defination TT on TT.idDefination = tbl_UPloadTransaction.idTransactionType ")
            StrSQL.Append(" Left outer join Vw_ClientMaster on ClientId = tbl_UPloadTransaction.idCompany ")
            StrSQL.Append(" Left Outer Join tbl_user on tbl_user.idUser = tbl_TaskAllotment.idUser ")
            StrSQL.Append(" Left Outer Join tbl_PurchaseMaster PM on PM.idUPloadTransaction = tbl_UPloadTransaction.idUPloadTransaction ")
            StrSQL.Append(" where 1=1  and PM.idUPloadTransaction not in ( select idUPloadTransaction from tbl_PurchaseMaster where PM.ReviewStatus in (0,1,3) )  ")
            StrSQL.Append(" and idTransactionType =  6  and Transactionstatus = 0 and isnull(tbl_TaskAllotment.idRejectedAllotment,0) =0 ")
            StrSQL.Append(" and tbl_UPloadTransaction.idCompany = '" & ViewState("Companyid").ToString & "'")
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
End Class
