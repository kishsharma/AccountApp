Imports System.Data

Partial Class AspxPages_AspxSupplierItems
    Inherits System.Web.UI.Page
    Dim eachrowttol As Decimal = 0, fullrowttol As Decimal = 0
    Dim temp As New DataTable()

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            fnFillDDL()
        End If
    End Sub

    Private Sub fnFillDDL()
        Dim clsodbcObj As New clsData
        Dim StrDDLListQry As New StringBuilder
        StrDDLListQry.Append(" select '0' id,'Select' data1 Union All select idClient, TxtCompanyName from tbl_client with (Nolock) ")
        StrDDLListQry.Append(" select '0' id,'Select' data1 Union All select UMCode, UMCode + '-' + UMName from Vw_UnitsOfMeasurement  with (Nolock)")
        Dim DtsDDL As New DataSet
        DtsDDL = clsodbcObj.fnDataSet(StrDDLListQry.ToString, 1)
        clsodbcObj = Nothing
        If Not IsNothing(DtsDDL) Then
            If DtsDDL.Tables.Count > 0 Then
                If DtsDDL.Tables(0).Rows.Count > 0 Then
                    DDLClient.DataSource = DtsDDL.Tables(0)
                    For intInc = 0 To DtsDDL.Tables(0).Rows.Count - 1
                        DDLClient.DataTextField = DtsDDL.Tables(0).Columns("data1").ColumnName.ToString
                        DDLClient.DataValueField = DtsDDL.Tables(0).Columns("ID").ColumnName.ToString
                    Next
                    DDLClient.DataBind()
                End If
                If DtsDDL.Tables(1).Rows.Count > 0 Then
                    DDLMeasure.DataSource = DtsDDL.Tables(1)
                    For intInc = 0 To DtsDDL.Tables(1).Rows.Count - 1
                        DDLMeasure.DataTextField = DtsDDL.Tables(1).Columns("data1").ColumnName.ToString
                        DDLMeasure.DataValueField = DtsDDL.Tables(1).Columns("ID").ColumnName.ToString
                    Next
                    DDLMeasure.DataBind()
                End If
            End If
        End If
    End Sub

    Protected Sub DDLClient_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLClient.SelectedIndexChanged
        If DDLClient.SelectedValue > 0 Then
            Dim clsodbcObj As New clsData
            Dim StrDDLListQry As String = " select '0' id,'Select' data1 Union All select SupplierID, NameofParty from tbl_SupplierDetails where idClient = " & DDLClient.SelectedValue & ""
            clsodbcObj.fnDropDownFill(StrDDLListQry, DDLSupplier, "data1", "id")
            clsodbcObj = Nothing
            fnFillGrid()
        End If

    End Sub

    Protected Sub btnAddItem_Click(sender As Object, e As System.EventArgs) Handles btnAddItem.Click
        If fnValidate() = False Then
            Exit Sub
        End If
        Dim StrItemId As String = String.Empty
        Dim StrSQLAddSuppItem As New StringBuilder
        StrSQLAddSuppItem.Append(" Insert into tbl_SupplierItems(IdClient,IdSupplier,ItemName,ItemOUM,HSNCode, IGSTRATE, CGSTRATE, SGSTRATE, WEFDate) ")
        StrSQLAddSuppItem.Append(" Select " & DDLClient.SelectedValue & ", " & DDLSupplier.SelectedValue & ", '" & TxtItemName.Text.Trim & "', '" & DDLMeasure.SelectedValue & "', '" & TxtHSNCode.Text.Trim & "', '" & TxtIGSTRate.Text.Trim & "', '" & TxtCGSTRate.Text.Trim & "', '" & TxtSGSTRate.Text.Trim & "', '" & TxtWEFDate.Value.Trim & "' ")
        Dim clsObj As New clsData
        StrItemId = clsObj.fnInsertScopeIdentity(StrSQLAddSuppItem.ToString, "")
        clsObj = Nothing
        If StrItemId.Length > 0 Then
            If IsNumeric(StrItemId) = True Then
                If CInt(StrItemId) > 0 Then
                    fnFillGrid()
                    'Dim dtU1 As New DataTable()
                    'If Gvdetails.HeaderRow IsNot Nothing Then
                    '    For i As Integer = 0 To Gvdetails.HeaderRow.Cells.Count - 1
                    '        dtU1.Columns.Add(Gvdetails.HeaderRow.Cells(i).Text)
                    '    Next
                    'End If
                    'For Each row As GridViewRow In Gvdetails.Rows
                    '    Dim dr As DataRow = dtU1.NewRow()
                    '    For i As Integer = 0 To row.Cells.Count - 1
                    '        dr(i) = row.Cells(i).Text
                    '    Next
                    '    dtU1.Rows.Add(dr)
                    'Next
                    'temp = dtU1
                    'If Session("SupItemGvd") Is Nothing Then
                    '    createDynamicGridInvoice()
                    'Else
                    '    temp = DirectCast(Session("SupItemGvd"), DataTable)
                    'End If
                    'temp.Rows.Add("", TxtItemName.Text.Trim, DDLMeasure.SelectedValue)
                    'Gvdetails.DataSource = temp
                    'Gvdetails.DataBind()
                    'Session("SupItemGvd") = temp
                    ''ScriptManager.RegisterStartupScript(updtPnlPopUp1, updtPnlPopUp1.GetType(), "showtextbox", "showtextbox(); CalculationSum();", True)
                    'addClear()

                End If
            End If
        End If
    End Sub
    Private Sub createDynamicGridInvoice()
        If temp.Columns.Count = 0 Then
            temp.Columns.Add("SlNo")
            temp.Columns.Add("ItemName")
            temp.Columns.Add("Measure")
        End If
    End Sub
    Private Sub addClear()
        TxtItemName.Text = String.Empty
        DDLMeasure.SelectedIndex = 0
    End Sub
    Protected Sub Gvdetails_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Gvdetails.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Then
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            'eachrowttol = Convert.ToDecimal(e.Row.Cells(7).Text)
            'fullrowttol = fullrowttol + eachrowttol
        End If
        If e.Row.RowType = DataControlRowType.Footer Then
            'totalAcceblevalue = fullrowttol
            'txtTotalValue.Text = totalAcceblevalue
            'LoadCalculation()
        End If

    End Sub
    Protected Sub Gvdetails_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles Gvdetails.RowDeleting
        temp = DirectCast(Session("SupItemGvd"), DataTable)
        Dim index As Integer = Convert.ToInt32(e.RowIndex)
        temp = TryCast(Session("SupItemGvd"), DataTable)
        temp.Rows(index).Delete()
        temp.AcceptChanges()
        Gvdetails.DataSource = temp
        Gvdetails.DataBind()
        Session("SupItemGvd") = temp
        'ScriptManager.RegisterStartupScript(updtPnlPopUp1, updtPnlPopUp1.GetType(), "showtextbox", "showtextbox(); CalculationSum();", True)
    End Sub

    Private Sub fnFillGrid()
        Dim clsodbcObj As New clsData
        Dim StrDDLListQry As New StringBuilder
        StrDDLListQry.Append(" select ItemName, UMName as 'Measure',HSNCode, IGSTRATE, CGSTRATE, SGSTRATE, WEFDate ")
        StrDDLListQry.Append(" from tbl_SupplierItems with (Nolock) ")
        StrDDLListQry.Append(" Left Outer Join Vw_UnitsOfMeasurement with (Nolock) on UMCode = ItemOUM ")
        StrDDLListQry.Append(" Where 1=1 ")
        StrDDLListQry.Append(" and IdClient = '" & DDLClient.SelectedValue & "' ")
        If DDLSupplier.SelectedValue > 0 Then
            StrDDLListQry.Append(" and IdSupplier = '" & DDLSupplier.SelectedValue & "'")
        End If
        Dim DtsDDL As New DataSet
        DtsDDL = clsodbcObj.fnDataSet(StrDDLListQry.ToString, 1)
        clsodbcObj = Nothing
        If Not IsNothing(DtsDDL) Then
            If DtsDDL.Tables.Count > 0 Then
                If DtsDDL.Tables(0).Rows.Count > 0 Then
                    Gvdetails.DataSource = DtsDDL.Tables(0)
                    Gvdetails.DataBind()
                    temp = DtsDDL.Tables(0)
                    Session("SupItemGvd") = temp
                End If
            End If
        End If

    End Sub

    Protected Sub DDLSupplier_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLSupplier.SelectedIndexChanged
        fnFillGrid()
    End Sub

    Private Function fnValidate() As Boolean
        If TxtIGSTRate.Text.ToString.Trim <> String.Empty Then
            If TxtSGSTRate.Text.Trim.Length > 0 Then
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('any one of IGST or [CGST and SGST] rates will be applied. ');</script>")
                Return False
            End If
            If TxtCGSTRate.Text.Trim.Length > 0 Then
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('any one of IGST or [CGST and SGST] rates will be applied. ');</script>")
                Return False
            End If
        End If
        If TxtSGSTRate.Text.Trim.Length > 0 Then
            If TxtIGSTRate.Text.ToString.Trim <> String.Empty Then
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert(' IGST rates Cannot be applied SGST. ');</script>")
                Return False
            End If
            If TxtCGSTRate.Text.Trim.Length = 0 Then
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert(' CGST rates is required with SGST. ');</script>")
                Return False
            End If
        End If
        If TxtCGSTRate.Text.Trim.Length > 0 Then
            If TxtIGSTRate.Text.ToString.Trim <> String.Empty Then
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert(' IGST rates Cannot be applied CGST. ');</script>")
                Return False
            End If
            If TxtSGSTRate.Text.Trim.Length = 0 Then
                ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert(' SGST rates is required with CGST. ');</script>")
                Return False
            End If
        End If
        Return True
    End Function
End Class
