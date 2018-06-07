Imports System.Data
Imports System.Globalization

Partial Class AspxPages_AspxApplicationSetting
    Inherits System.Web.UI.Page
    Dim clsObj As New clsData

    Public StrRowCreatedDt As String = String.Empty
    Public StrlastUpdtDt As String = String.Empty
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            fnFillScreen()
        End If
    End Sub
    Private Sub fnFillScreen()

        Dim dtstSearchDtls As New DataSet
        Dim clsfn As New ClsCommonFunction
        dtstSearchDtls = clsObj.fnDataSet("	select top 1 idcompanyrule,SMTP,Port,SMTPUsername,SMTPPassword,SMSUrl,accuracylevel from tbl_companyrule ", 1, 1)
        If Not IsNothing(dtstSearchDtls) Then
            If dtstSearchDtls.Tables.Count > 0 Then
                If dtstSearchDtls.Tables(0).Rows.Count > 0 Then
                    HdnIdentity.Value = dtstSearchDtls.Tables(0).Rows(0)("idcompanyrule").ToString()
                    TxtSMTP.Value = dtstSearchDtls.Tables(0).Rows(0)("SMTP").ToString()
                    TxtPort.Value = dtstSearchDtls.Tables(0).Rows(0)("Port").ToString()
                    TxtSMTPUsername.Value = dtstSearchDtls.Tables(0).Rows(0)("SMTPUsername").ToString()
                    TxtSMTPPassword.Value = dtstSearchDtls.Tables(0).Rows(0)("SMTPPassword").ToString()
                    TxtSMSUrl.Value = dtstSearchDtls.Tables(0).Rows(0)("SMSUrl").ToString()
                    TxtAccuracylevel.Value = dtstSearchDtls.Tables(0).Rows(0)("accuracylevel").ToString()

                End If
            End If
        End If
        clsfn = Nothing
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim Names As String() = {"idcompanyrule", "SMTP", "Port", "SMTPUsername", "SMTPPassword", "SMSUrl", "accuracylevel"}
        Dim Values As String() = {HdnIdentity.Value, TxtSMTP.Value, TxtPort.Value.Trim, TxtSMTPPassword.Value, TxtSMTPPassword.Value, TxtSMSUrl.Value.Trim, TxtAccuracylevel.Value.Trim}
        clsObj.fnInsertUpdateNoRtn("P_ApplicationSetting", Names, Values)
        ClientScript.RegisterStartupScript(Page.GetType(), "AlertMsg", "<script type='text/javascript' language='javascript'>alert('Application Setting Updated');</script>")
        fnFillScreen()
    End Sub
End Class
