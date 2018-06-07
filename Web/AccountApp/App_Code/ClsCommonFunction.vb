Imports Microsoft.VisualBasic
Imports System.Text.RegularExpressions
Imports System.Net.Dns

Imports System.Data
Imports System.Data.Odbc

Public Class ClsCommonFunction
    'Single Quote Replacement
    Public Function fnReplaceSingleQuote(ByVal strSql As String) As String
        Dim strString As String
        strString = Replace(strSql, "'", "''")
        Return strString
    End Function

    'Replace Null Values
    Public Function fnReplaceNull(ByVal strValue As String) As String
        fnReplaceNull = IIf(IsDBNull(strValue) = True, "", strValue)
    End Function

    'Check ineteger value
    Public Function fnInteger(ByVal strInteger As String) As Boolean
        Dim rxInt As New Regex("^[0-9]+$")
        Return rxInt.IsMatch(Trim(strInteger))
    End Function

    'Float or Decimal
    Public Function fnFloatOrDecimal(ByVal strFD As String) As Boolean
        Dim rxFD As New Regex("^[0-9]+(\.[0-9]+)?$")
        Return rxFD.IsMatch(Trim(strFD))
    End Function

    'Character vefication
    Public Function fnAlpha(ByVal strString As String) As Boolean
        Dim rxStr As New Regex("^[a-zA-Z]+$")
        Return rxStr.IsMatch(Trim(strString))
    End Function

    'Numeric and caharacter verification
    Public Function fnAlphaNumeric(ByVal strString As String) As Boolean
        Dim rxStr As New Regex("^[a-zA-Z0-9]+$")
        Return rxStr.IsMatch(Trim(strString))
    End Function

    'Zipcode validation
    Public Function fnZipCodeValid(ByVal strZip As String) As Boolean
        Dim rxZip As New Regex("^\d{5}$")
        Return rxZip.IsMatch(Trim(strZip))
    End Function

    'Phone no validation
    Public Function fnPhoneValid(ByVal strPhone As String) As Boolean
        Dim rxPhone As New Regex("^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$")
        Return rxPhone.IsMatch(Trim(strPhone))
    End Function

    'Email Validation
    Public Function fnEmailValid(ByVal strEmail As String) As Boolean
        Dim rxEmail As New Regex("\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")
        If rxEmail.IsMatch(Trim(strEmail)) = False Then MessageBox.Show("Please enter valid email") : Return False
        Return rxEmail.IsMatch(Trim(strEmail))
    End Function

    'This will Give Server Date
    'Public Function fnGetDate() As String
    '    Dim ClsDBTrn As New clsODBCData
    '    Dim dtDate As String = ClsDBTrn.fnDataSet("select convert(varchar(10),getDate(),101) as Dates", CommandType.Text).Tables(0).Rows(0)(0).ToString
    '    Return (dtDate)
    'End Function

    ''This function will return date format (yyyy/mm/dd)
    'Public Function fnChangeDateFormat(ByVal strDate As String) As String
    '    Dim strArr As String() = Nothing
    '    Dim strFmDate As String = String.Empty
    '    strArr = Split(strDate, "/")
    '    Return Trim(strArr(2)) & "/" & Trim(strArr(0)) & "/" & Trim(strArr(1))
    'End Function

    'From date and to date validation
    Public Function fnDateFromTovalid(ByVal strFrom As String, ByVal strTo As String) As Integer
        Dim rxDate As New Regex("\d{1,2}\/\d{1,2}/\d{4}")
        If Not rxDate.IsMatch(Trim(strFrom)) Then
            Return False
        End If
        If Not rxDate.IsMatch(Trim(strTo)) Then
            Return False
        End If
        Dim intdiff As Integer
        intdiff = DateDiff(DateInterval.Hour, CDate(Trim(strFrom)), CDate(Trim(strTo)))
        If intdiff < 24 Then
            intdiff = 24
        End If
        Return intdiff
    End Function

    'Validate AM / PM
    Public Function fnAMPM(ByVal strTime As String) As Boolean
        Dim rxTime As New Regex("^ *(1[0-2]|[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$")
        Return rxTime.IsMatch(Trim(strTime))
    End Function

    ' Time format validation strTime = pass textbox value,  intTimeFormat : 0: 24 hour, 1: 12 hour
    Public Function fnIsValidTime(ByVal strTime As String, Optional ByVal intTimeFormat As Int16 = 0) As Boolean
        Try
            Dim rxTime As Regex = Nothing
            Select Case intTimeFormat
                Case 0
                    rxTime = New Regex("^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])(:([0-5]?[0-9]))?$")
                    If rxTime.IsMatch(strTime) = False Then MessageBox.Show("Invalid date format Ex : 00:00 to 23:59.") : Return False
                Case 1
                    rxTime = New Regex("^*(1[0-9]|[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$")
                    If rxTime.IsMatch(strTime) = False Then MessageBox.Show("Invalid date format Ex : 10:30 AM") : Return False
            End Select
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    '12 Time format validation : comparision between from and to date
    Public Function fnIsValid12HRTimeFromTo(ByVal strFromTime As String, ByVal strToTime As String) As Boolean
        Try
            Dim rxTime As New Regex("^*(1[0-9]|[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$")
            If rxTime.IsMatch(strFromTime) And rxTime.IsMatch(strToTime) = True Then
                Dim tmF As DateTime = CType(strFromTime, DateTime)
                Dim tmT As DateTime = CType(strToTime, DateTime)
                If tmF >= tmT Then MessageBox.Show("From time can't be Greater than or equal to time") : Return False
            Else
                MessageBox.Show("Invalid date format Ex : 10:30 AM") : Return False
            End If
            Return True
        Catch ex As Exception
            MessageBox.Show("Invalid date format Ex : 10:30 AM") : Return False
        End Try
    End Function

    '24 Time format validation : comparision between from and to date
    Public Function fnIsValid24HRTimeFromTo(ByVal strFromTime As String, ByVal strToTime As String) As Boolean
        Try
            Dim rxTime As New Regex("^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])(:([0-5]?[0-9]))?$")
            If rxTime.IsMatch(strFromTime) And rxTime.IsMatch(strToTime) = True Then
                Dim tmF As DateTime = CType(strFromTime, DateTime)
                Dim tmT As DateTime = CType(strToTime, DateTime)
                If tmF >= tmT Then MessageBox.Show("From time can't be Greater than or equal to time") : Return False
            Else
                MessageBox.Show("Invalid date format Ex : 00:00 to 23:59.") : Return False
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    'This will Validate Time Provided
    Public Function fnvaltime(ByVal strtm As String) As Boolean
        Dim rxTime As New Regex("^(([0-1][0-9])|([2][0-3])):([0-5][0-9]):([0-5][0-9])?$")
        Return rxTime.IsMatch(Trim(strtm))
    End Function

    'Mandatory Check  - Textbox
    Public Function fnCheckTextBox(ByVal txtName As TextBox) As Boolean
        If txtName.Text.Trim = String.Empty Then MessageBox.Show("Please enter " & txtName.Text) : txtName.Focus() : Return False
        Return True
    End Function

    'Mandatory Check  - Combobox
    'Public Function fnCheckCombo(ByVal cmb As ComboBox) As Boolean
    '    If cmb.SelectedIndex = 0 Then MessageBox.Show("Please select " & cmb.Tag) : cmb.Focus() : Return False
    '    Return True
    'End Function

    'Selection Check  - Combobox
    'Public Function fnCheckComboSelect(ByVal cmb As ComboBox) As Boolean
    '    If cmb.SelectedIndex = -1 Then MessageBox.Show("Please select " & cmb.Tag) : cmb.Focus() : Return False
    '    Return True
    'End Function

    'Validate Date as per server date
    Public Function fnValidateDate(ByVal dtDob As Date) As Integer
        Dim intdiff As Integer = 0
        Try
            Dim dtDate As Date
            dtDate = Me.fnGetServerDate()
            If IsDate(dtDate) = True Then
                intdiff = DateDiff("d", dtDate, dtDob)
            End If
            Return intdiff
        Catch ex As Exception
            Return intdiff
        End Try
    End Function

    'Return IP Address of client system
    Public Function GetIPAddress() As String
        'Dim objIpadd As System.Net.IPAddress
        Dim strIpAdd As String = String.Empty '"192.168.1.12"
        'With System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName())
        '    objIpadd = New System.Net.IPAddress(.AddressList(0).Address)
        '    strIpAdd = objIpadd.ToString
        'End With
        Return strIpAdd
    End Function

    'Get Sever date
    'Public Function fnGetServerDate() As Date
    '    Dim ClsDBTrn As New clsODBCData
    '    Dim dt As New Date
    '    dt = ClsDBTrn.fnGetStrFromDB("select curdate()", CommandType.Text)
    '    Return (dt)
    'End Function

    'Get Sever dateTime
    'Public Function fnGetServerDateTime() As Date
    '    Dim ClsDBTrn As New clsODBCData
    '    Dim dt As New Date
    '    dt = ClsDBTrn.fnGetStrFromDB("select Now()", CommandType.Text)
    '    Return (dt)
    'End Function

    'This will Insert Log in Log Table
    'Public Sub fninitlog(ByVal strFunction As String, ByVal strOperation As String, ByVal strLogDesc As String)
    '    Dim ClsDBTrn As New clsODBCData
    '    Dim strSql As String
    '    Dim MyName As String = Environment.MachineName.ToString
    '    Dim strNode As String = System.Windows.Forms.SystemInformation.ComputerName
    '    Try
    '        strSql = "insert into tbl_log (VCFunctionCode,VCOperationCode,VCLogDesc,VCUserNode,idBranch,idUser,DTMupdDate)"
    '        strSql = strSql & " Values('" & strFunction & "','" & strOperation & "','" & strLogDesc & "','" & GetIPAddress() & "'," & GlbLngIDIndustry() & "," & GlbLngIDUser() & ", Now())"
    '        ClsDBTrn.fninsertLog(strSql, CommandType.Text)
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Public Function fnChangeDateFormat(ByVal StrPassedDt As String, ByVal intDesFormat As Integer, ByVal StrDtSeparator As String, ByVal IntActDtFomat As Integer, ByVal IntDBReq As Integer) As String
        'If intDesFormat <> IntActDtFomat Then
        If StrPassedDt.ToString = String.Empty Then Return "1900-01-01"
        Dim StrNewDt As String = String.Empty
        Dim StrPssDt = Replace(StrPassedDt.ToString, "-", "/")
        Try
            Dim DtPart() As String = StrPssDt.Split("/")
            Dim StrDtPart As String = String.Empty
            Select Case IntActDtFomat
                Case 1 'dd/mm/yyyy

                    If Not IsNumeric(DtPart(1)) Then
                        StrDtPart = FnGetMonthNumber(DtPart(1).ToString)
                        StrPssDt = StrDtPart + "-" + DtPart(0) + "-" + DtPart(2) 'mm/dd/yyyy
                    Else
                        If DtPart(1) > 12 Then
                            StrPssDt = DtPart(0) + "-" + DtPart(1) + "-" + DtPart(2)
                        Else
                            If Not IsNumeric(DtPart(0)) Then
                                StrDtPart = FnGetMonthNumber(DtPart(0).ToString)
                                StrPssDt = StrDtPart + "-" + DtPart(0) + "-" + DtPart(2) 'mm/dd/yyyy
                            Else
                                StrPssDt = DtPart(0) + "-" + DtPart(1) + "-" + DtPart(2)
                            End If
                        End If
                    End If

                Case 2 'mm/dd/yyyy
                    If Not IsNumeric(DtPart(0)) Then
                        StrDtPart = FnGetMonthNumber(DtPart(0).ToString)
                        StrPssDt = StrDtPart + "-" + DtPart(1) + "-" + DtPart(2) 'mm/dd/yyyy
                    Else
                        StrPssDt = DtPart(0) + "-" + DtPart(1) + "-" + DtPart(2) 'mm/dd/yyyy
                    End If
                Case 3 'yyyy/mm/dd

                    'If Not IsNumeric(DtPart(1)) Then
                    'Select Case DtPart(1)

                    If Not IsNumeric(DtPart(1)) Then
                        StrDtPart = FnGetMonthNumber(DtPart(1).ToString)
                        If Not IsNumeric(DtPart(1)) Then
                            StrPssDt = DtPart(0) + "-" + StrDtPart + "-" + DtPart(2) 'yyyy/mm/dd
                        Else
                            StrPssDt = DtPart(0) + "-" + DtPart(1) + "-" + DtPart(2) 'yyyy/mm/dd
                        End If
                    Else
                        If intDesFormat = 3 Then
                            StrPssDt = DtPart(0) + "-" + DtPart(1) + "-" + DtPart(2) ''mm/dd/yyyy
                        Else
                            StrPssDt = DtPart(2) + "-" + DtPart(0) + "-" + DtPart(1) ''mm/dd/yyyy
                        End If
                        'StrPssDt = DtPart(1) + "-" + DtPart(2) + "-" + DtPart(0) ''mm/dd/yyyy
                    End If

            End Select
            Dim odate As Date = StrPssDt
            Dim StrTime As String = "00:00:00"
            If InStr(StrPassedDt, " ") > 0 Then StrTime = Mid(StrPassedDt, InStr(StrPassedDt, " ")).Trim
            If InStr(StrPassedDt, " ") > 0 Then StrPassedDt = Mid(StrPassedDt, 1, InStr(StrPassedDt, " ")).Trim
            Dim StrMonthNoWithZero As String = odate.Month.ToString
            If odate.Month < 9 Then
                StrMonthNoWithZero = "0" & odate.Month
            End If
            Select Case intDesFormat
                Case 1 'dd/mmm/yyyy
                    If IntDBReq = 0 Then
                        StrNewDt = odate.Day & StrDtSeparator & odate.ToString("MMM") & StrDtSeparator & odate.Year
                    Else
                        StrNewDt = odate.Day & StrDtSeparator & StrMonthNoWithZero & StrDtSeparator & odate.Year
                    End If
                Case 2 'mm/dd/yyyy
                    If IntDBReq = 0 Then
                        StrNewDt = odate.ToString("MMM") & StrDtSeparator & odate.Day & StrDtSeparator & odate.Year
                    Else
                        StrNewDt = StrMonthNoWithZero & StrDtSeparator & odate.Day & StrDtSeparator & odate.Year
                    End If

                Case 3 'yyyy/mmm/dd
                    If IntDBReq = 0 Then
                        StrNewDt = odate.Year & StrDtSeparator & odate.ToString("MMM") & StrDtSeparator & odate.Day
                    Else
                        StrNewDt = odate.Year & StrDtSeparator & StrMonthNoWithZero & StrDtSeparator & odate.Day
                    End If
                Case Else
                    StrNewDt = odate.Day & StrDtSeparator & odate.ToString("MMM") & StrDtSeparator & odate.Year
            End Select


        Catch ex As Exception

        End Try
        Return Replace(StrNewDt.ToString, "/", "-")
        'Else
        'Return StrPassedDt
        'End If
    End Function

    Private Function FnGetMonthNumber(ByVal strMonthName As String) As Integer
        Select Case UCase(strMonthName)
            Case "JAN", "JANAUARY"
                Return "1"
            Case "FEB", "FEBRUARY"
                Return "2"
            Case "MAR", "MARCH"
                Return "3"
            Case "APR", "APRIL"
                Return "4"
            Case "MAY"
                Return "5"
            Case "JUN", "JUNE"
                Return "6"
            Case "JUL", "JULY"
                Return "7"
            Case "AUG", "AUGUST"
                Return "8"
            Case "SEP", "SEPTEMBER"
                Return "9"
            Case "OCT", "OCTOBER"
                Return "10"
            Case "NOV", "NOVEMBER"
                Return "11"
            Case "DEC", "DECEMBER"
                Return "12"
            Case Else
                Return "0"
        End Select
    End Function

    Public Function fnCalculateDateDiff(ByVal p_DateFrom As Date, ByVal p_DateTo As Date) As Integer
        Dim m_DateFrom As Date
        Dim m_DateTo As Date
        Dim m_DateDiff As System.TimeSpan
        Dim m_Nights As Integer
        'RK> Get dates
        m_DateFrom = p_DateFrom
        m_DateTo = p_DateTo
        'RK> Calculate days
        m_DateDiff = m_DateTo.Subtract(m_DateFrom)
        If m_DateDiff.Days >= 0 Then
            If m_DateDiff.Hours > 0 Then
                m_Nights = m_DateDiff.Days + 1
            Else
                m_Nights = m_DateDiff.Days
            End If
        End If
        'RK> Return result
        Return m_Nights
    End Function

    Private Function MessageBox() As Object
        Throw New NotImplementedException
    End Function

    Private Function GlbLngIDIndustry() As String
        Throw New NotImplementedException
    End Function

    Private Function GlbLngIDUser() As String
        Throw New NotImplementedException
    End Function

    Private Function fnGetServerDate() As Date
        Throw New NotImplementedException
    End Function

    'Public Function DataTableToHTMLTable(ByVal inTable As DataTable, ByVal StrHTMLTableID As String, ByVal StrHTMLTableClass As String, ByVal StrHTMLCellSpacing As String, ByVal StrHTMLTablewidth As String) As String
    '    Dim dString As New StringBuilder
    '    dString.Append("<table id=" & StrHTMLTableID & " class= " & StrHTMLTableClass & " cellspacing=" & StrHTMLCellSpacing & " width=" & StrHTMLTablewidth & ">")
    '    dString.Append(fnGetHTMLHeader(inTable))
    '    dString.Append(fnGetHTMLBody(inTable))
    '    dString.Append("</table>")
    '    Return dString.ToString
    'End Function

    'Private Function fnGetHTMLHeader(ByVal dTable As DataTable) As String
    '    Dim dString As New StringBuilder
    '    dString.Append("<thead><tr>")
    '    For Each dColumn As DataColumn In dTable.Columns
    '        dString.AppendFormat("<th>{0}</th>", dColumn.ColumnName)
    '    Next
    '    dString.Append("</tr></thead>")

    '    Return dString.ToString
    'End Function

    'Private Function fnGetHTMLBody(ByVal dTable As DataTable) As String
    '    Dim dString As New StringBuilder
    '    dString.Append("<tbody>")
    '    For Each dRow As DataRow In dTable.Rows
    '        dString.Append("<tr>")
    '        For dCount As Integer = 0 To dTable.Columns.Count - 1
    '            dString.Append("<td> " & dRow.Item(dCount) & " </td>") ''dTable.Columns.Item(dCount).ToString
    '        Next
    '        dString.Append("</tr>")
    '    Next
    '    dString.Append("</tbody>")

    '    Return dString.ToString()
    'End Function

    'Private Function fnGetHTMLFooter(ByVal dTable As DataTable) As String
    '    Dim dString As New StringBuilder
    '    dString.Append("<tfoot><tr>")
    '    For Each dColumn As DataColumn In dTable.Columns
    '        dString.AppendFormat("<th>{0}</th>", dColumn.ColumnName)
    '    Next
    '    dString.Append("</tr></tfoot>")
    '    Return dString.ToString
    'End Function


    Public Function DataTableToHTMLTable(ByVal inTable As DataTable, ByVal StrHTMLTableID As String, ByVal StrHTMLTableClass As String, ByVal StrHTMLCellSpacing As String, ByVal StrHTMLTablewidth As String, ByVal BlnHTMLHideIDCol As Boolean) As String
        Dim dString As New StringBuilder
        dString.Append("<table id=""" & StrHTMLTableID & """ class= """ & StrHTMLTableClass & """ cellspacing=""" & StrHTMLCellSpacing & """ width=""" & StrHTMLTablewidth & """>")
        dString.Append(fnGetHTMLHeader(inTable, BlnHTMLHideIDCol))
        dString.Append(fnGetHTMLBody(inTable, BlnHTMLHideIDCol))
        dString.Append("</table>")
        Return dString.ToString
    End Function

    Private Function fnGetHTMLHeader(ByVal dTable As DataTable, ByVal BlnHTMLHideIDCol As Boolean) As String
        Dim dString As New StringBuilder
        dString.Append("<thead><tr>")
        For Each dColumn As DataColumn In dTable.Columns
            If BlnHTMLHideIDCol = True Then
                If UCase(Mid(dColumn.ColumnName, 1, 2)) = "ID" Then
                    dString.AppendFormat("<th style=""display:none;"">{0}</th>", dColumn.ColumnName)
                ElseIf UCase(dColumn.ColumnName) = "STATUSCOLOR" Then
                    dString.AppendFormat("<th style=""display:none;"">{0}</th>", dColumn.ColumnName)
                Else
                    dString.AppendFormat("<th>{0}</th>", dColumn.ColumnName)
                End If
            Else
                dString.AppendFormat("<th>{0}</th>", dColumn.ColumnName)
            End If
        Next
        dString.Append("</tr></thead>")

        Return dString.ToString
    End Function

    Private Function fnGetHTMLBody(ByVal dTable As DataTable, ByVal BlnHTMLHideIDCol As Boolean) As String
        Dim dString As New StringBuilder
        Dim strcolor As String = String.Empty
        dString.Append("<tbody>")
        For Each dRow As DataRow In dTable.Rows
            dString.Append("<tr>")
            For dCount As Integer = 0 To dTable.Columns.Count - 1
                If BlnHTMLHideIDCol = True Then
                    If UCase(Mid(dTable.Columns(dCount).ColumnName, 1, 2)) = "ID" Then
                        dString.Append("<td style=""display:none;""> " & dRow.Item(dCount) & " </td>") ''dTable.Columns.Item(dCount).ToString
                    ElseIf UCase(dTable.Columns(dCount).ColumnName) = "STATUSCOLOR" Then

                        dString.Append("<td style=""display:none;""> " & dRow.Item(dCount) & " </td>")
                        strcolor = dRow.Item(dCount)
                    ElseIf UCase(dTable.Columns(0).ColumnName) = "STATUSCOLOR" Then

                        dString.Append("<td style='color:black;background-color:" & strcolor & "'> " & dRow.Item(dCount) & " </td>") ''dTable.Columns.Item(dCount).ToString
                    Else
                        dString.Append("<td> " & dRow.Item(dCount) & " </td>")
                    End If
                Else
                    dString.Append("<td> " & dRow.Item(dCount) & " </td>") ''dTable.Columns.Item(dCount).ToString
                End If

            Next
            dString.Append("</tr>")
        Next
        dString.Append("</tbody>")

        Return dString.ToString()
    End Function

    Private Function fnGetHTMLFooter(ByVal dTable As DataTable) As String
        Dim dString As New StringBuilder
        dString.Append("<tfoot><tr>")
        For Each dColumn As DataColumn In dTable.Columns
            dString.AppendFormat("<th>{0}</th>", dColumn.ColumnName)
        Next
        dString.Append("</tr></tfoot>")
        Return dString.ToString
    End Function

    Public Function fnConvertJSONToDataTable(jsonString As String) As DataTable
        Dim dt As New DataTable
        'strip out bad characters
        Dim jsonParts As String() = jsonString.Replace("[", "").Replace("]", "").Split("},{")

        'hold column names
        Dim dtColumns As New List(Of String)

        'get columns
        For Each jp As String In jsonParts
            'only loop thru once to get column names
            Dim propData As String() = jp.Replace("{", "").Replace("}", "").Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
            For Each rowData As String In propData
                Try
                    Dim idx As Integer = rowData.IndexOf(":")
                    Dim n As String = rowData.Substring(0, idx - 1)
                    Dim v As String = rowData.Substring(idx + 1)
                    If Not dtColumns.Contains(n) Then
                        dtColumns.Add(n.Replace("""", ""))
                    End If
                Catch ex As Exception
                    'Throw New Exception(String.Format("Error Parsing Column Name : {0}", rowData))
                End Try

            Next
            Exit For
        Next

        'build dt
        For Each c As String In dtColumns
            dt.Columns.Add(c)
        Next
        'get table data
        For Each jp As String In jsonParts
            Dim propData As String() = jp.Replace("{", "").Replace("}", "").Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
            Dim nr As DataRow = dt.NewRow
            For Each rowData As String In propData
                Try
                    Dim idx As Integer = rowData.IndexOf(":")
                    Dim n As String = rowData.Substring(0, idx - 1).Replace("""", "")
                    Dim v As String = rowData.Substring(idx + 1).Replace("""", "")
                    nr(n) = v
                Catch ex As Exception
                    Continue For
                End Try

            Next
            dt.Rows.Add(nr)
        Next
        Return dt
    End Function

    'Check datafile is null
    Public Function IsNotDBNull(ByVal objToCheck As Object) As Boolean
        If (objToCheck Is Nothing Or objToCheck.GetType().Equals(System.Type.GetType("System.DBNull"))) Then
            Return False
        Else
            Return True
        End If
    End Function
End Class
