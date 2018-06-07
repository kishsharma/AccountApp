Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class AutocompleteHSN
    Inherits System.Web.Services.WebService
    Dim strName As String = "Welcome"
    Dim strAge As String = "Hearly"
    <WebMethod()>
    Public Function HSNcodeAutocomplete(HSNCODe As String, limit As String) As String()
        Dim lstHSNCODe As List(Of String) = New List(Of String)

        Dim dtHSNCODE As DataTable = New DataTable()

        Try


            Dim DtsSearchData As New DataSet
            Dim StrQrySearch As String
            StrQrySearch = ("select top " & limit & " GSTCOde,replace(GSTCodeDescription,'-','') as GSTCodeDescription,CGSTRate,SGSTRate,IGSTRate,convert(varchar,GSTRateWEF,103) as GSTRateWEF from tbl_GSTMaster where GSTCode like '%" & HSNCODe & "%'")
            Dim clsobj As New clsData
            DtsSearchData = clsobj.fnDataSet(StrQrySearch.ToString, CommandType.Text)
            clsobj = Nothing
            dtHSNCODE = DtsSearchData.Tables(0)
            For Each DtRow As DataRow In dtHSNCODE.Rows
                Dim name = Convert.ToString(DtRow("GSTCOde")) + "-" + Convert.ToString(DtRow("GSTRateWEF")) + "-" + Convert.ToString(DtRow("CGSTRate")) + "-" + Convert.ToString(DtRow("SGSTRate")) + "-" + Convert.ToString(DtRow("IGSTRate")) + "-" + Convert.ToString(DtRow("GSTCodeDescription"))
                lstHSNCODe.Add(name)
            Next
            Return lstHSNCODe.ToArray()
        Catch ex As Exception

        End Try

    End Function

    <WebMethod()>
    Public Function ItemListInvoice(ItemName As String, limit As String) As String()
        Dim lstItemName As List(Of String) = New List(Of String)

        Dim dtItemlist As DataTable = New DataTable()

        Try

            Dim DtsSearchData As New DataSet
            Dim StrQrySearch As String
            StrQrySearch = ("select top " & limit & " txtitemname,txtUOM from tbl_InvoiceDetails  where txtitemname like '%" & ItemName & "%'")
            Dim clsobj As New clsData
            DtsSearchData = clsobj.fnDataSet(StrQrySearch.ToString, CommandType.Text)
            clsobj = Nothing
            dtItemlist = DtsSearchData.Tables(0)
            For Each DtRow As DataRow In dtItemlist.Rows
                Dim name = Convert.ToString(DtRow("txtitemname")) & "::" & Convert.ToString(DtRow("txtUOM"))
                lstItemName.Add(name)
            Next
            Return lstItemName.ToArray()
        Catch ex As Exception

        End Try

    End Function

End Class