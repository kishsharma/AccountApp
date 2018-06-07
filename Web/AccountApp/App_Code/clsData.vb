'fnDataReader() returns a reader object after a call to the function call the prpCon.Close()  
'fnDataset() function returns the Dataset filled with data based on the query passed to it
'fnIUD() is used to Insert,Update,Delete the data from tables in database based on the query passed to it
'fnCommand() returns a Command Object which can be used in codebehind based on the requirement
'fnDropDownFill() fills the dropdownlist based on the query and control id of the dropdownlist 
'fnGridFill() fills the dropdownlist based on the query and control id of the GridView
'fnhistory() to store the updated or deleted records to history tables
Imports System.Data
'If we need to change to a different database, uncomment system.data.common...
'Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Configuration
Imports Microsoft.VisualBasic
Imports System.IO
'Imports System.Web.Mail
Imports System.Net.Mail
Imports System.Net

Public Class clsData
    Dim strNotify As String
    Dim blnFlag As Boolean = False
    Dim blnTmrStp As Boolean = False
    Dim settings As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("AdminConnection")
    Dim HistSettings As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("HistoryDatabase")
    Dim retScon As New SqlConnection
    Dim retScmd As New SqlCommand
    Dim sadaDapt As New SqlDataAdapter
    Dim datsData As New DataSet
    Dim sdrData As SqlDataReader
    Dim transaction As SqlTransaction
    Dim timer As New Timers.Timer
    Dim strTmSpanMlSMS As String = String.Empty
    'Dim alData, alBData As New ArrayList

    Public Property prpOReader() As SqlDataReader
        Get
            Return sdrData
        End Get
        Set(ByVal value As SqlDataReader)
            sdrData = value
        End Set
    End Property

    Public Property prpODataset() As DataSet
        Get
            Return datsData
        End Get
        Set(ByVal value As DataSet)
            datsData = value
        End Set
    End Property

    Public Property prpCon() As SqlConnection
        Get
            Return retScon
        End Get
        Set(ByVal value As SqlConnection)
            retScon = value
        End Set
    End Property

    Public Property prpCmd() As String
        Get
            Return strNotify
        End Get
        Set(ByVal value As String)
            strNotify = value
        End Set
    End Property

    Public Property prpReader() As String
        Get
            Return strNotify
        End Get
        Set(ByVal value As String)
            strNotify = value
        End Set
    End Property

    Public Property prpDataset() As String
        Get
            Return strNotify
        End Get
        Set(ByVal value As String)
            strNotify = value
        End Set
    End Property

    'fnDataReader() returns a reader object after a call to the function call the prpCon.Close()  
    Public Function fnDatareader(ByVal strQry As String, ByVal strCmdType As Integer) As SqlDataReader
        Try
            blnFlag = False
            retScon.ConnectionString = settings.ConnectionString
            retScmd.CommandText = strQry
            retScmd.CommandType = strCmdType
            retScmd.Connection = retScon
            retScon.Open()
            sdrData = retScmd.ExecuteReader
            Me.prpOReader = sdrData
            Me.prpCon = retScon
            Return sdrData
        Catch ex As Exception
            blnFlag = True
            Me.prpReader = ex.Message.ToString
        End Try
        If Not blnFlag.Equals(True) Then
            Return sdrData
        Else
            Return Nothing
        End If
        blnFlag = False
    End Function

    'fnDataset() function returns the Dataset filled with data based on the query passed to it
    Public Function fnDataSet(ByVal strQry As String, ByVal strCmdType As Integer, Optional ByVal strTabname As String = "") As DataSet
        Try
            blnFlag = False
            retScon.ConnectionString = settings.ConnectionString
            retScmd.CommandText = strQry
            retScmd.CommandType = strCmdType
            retScmd.Connection = retScon
            sadaDapt.SelectCommand = retScmd
            If strTabname.Trim <> "" Then
                sadaDapt.Fill(datsData, strTabname)
            Else
                sadaDapt.Fill(datsData)
            End If

            Me.prpODataset = datsData
            Return datsData
        Catch ex As Exception
            blnFlag = True
        End Try
        If blnFlag.Equals(True) Then
            Return Nothing
        Else
            Return datsData
        End If
        blnFlag = False
    End Function

    'fnIUD() is used to Insert,Update,Delete the data from tables in database based on the query passed to it
    Public Function fnIUD(ByVal strQry As String, ByVal strCmdType As Integer, ByVal strKnown As String) As Boolean
        Dim StrintID As String = String.Empty
        Try
            retScon.ConnectionString = settings.ConnectionString
            retScmd.Connection = retScon
            retScmd.CommandText = strQry
            retScmd.CommandType = strCmdType
            retScon.Open()
            transaction = retScon.BeginTransaction
            retScmd.Transaction = transaction
            If strKnown.Equals("NonQuery") Then
                retScmd.ExecuteNonQuery()
                transaction.Commit()
            Else
                retScmd.ExecuteScalar()
                transaction.Commit()
            End If

            retScon.Close()
        Catch sx As SqlException
            transaction.Rollback()
            retScon.Close()
            Return False
        Catch ex As Exception
            retScon.Close()
            Return False
        End Try
        If strKnown.Equals("NonQuery") Then
            Return True
        Else
            Return False
        End If
    End Function
    ' fnCommand() returns a Command Object which can be used in codebehind based on the requirement
    Public Function fnCommand(ByVal strQry As String, ByVal strCmdType As Integer, ByVal strknown As String) As SqlCommand
        Try
            If strknown.Equals("reader") Then
                retScon.ConnectionString = settings.ConnectionString
                retScmd.CommandText = strQry
                retScmd.CommandType = strCmdType
                retScmd.Connection = retScon
                Return retScmd
            End If
        Catch ex As Exception
            blnFlag = True
            Me.prpCmd = ex.Message.ToString
        End Try
        If blnFlag.Equals(True) Then
            Return Nothing
        Else
            Return retScmd
        End If
        blnFlag = False
    End Function

    'fnDropDwonFill() fills the dropdownlist based on the query and control id of the dropdownlist 
    Public Function fnDropDownFill(ByVal strQry As String, ByVal ddlId As DropDownList, ByVal column As String, ByVal bcolumn As String) As DropDownList
        ' Try
        'replace 'Select' with ' Select'
        strQry = strQry.Replace(Chr(39) & "Select" & Chr(39), Chr(39) & " Select" & Chr(39))
        Dim intInc As Integer
        Me.fnDataSet(strQry, CommandType.Text)
        If Not IsNothing(Me.prpODataset) Then
            If Me.prpODataset.Tables.Count > 0 Then
                If Me.prpODataset.Tables(0).Rows.Count <> 0 Then
                    ddlId.DataSource = Me.prpODataset
                    For intInc = 0 To Me.prpODataset.Tables(0).Rows.Count - 1
                        ddlId.DataTextField = Me.prpODataset.Tables(0).Columns(column).ColumnName.ToString
                        ddlId.DataValueField = Me.prpODataset.Tables(0).Columns(bcolumn).ColumnName.ToString
                    Next
                    ddlId.DataBind()
                End If
            End If
        End If

        'Try

        Return ddlId
        'Catch oE As System.Exception
        ' Handle exception here
        'End Try
        'Catch ex As Exception
        'blnFlag = True
        'End Try
        'If blnFlag.Equals(True) Then
        '    Return Nothing
        'Else
        '    Return ddlId
        'End If
        'blnFlag = False
    End Function
    'fnListBoxFill() fills the List filed based on the query and control id of the Lsitfield 
    Public Function fnListBoxFill(ByVal strQry As String, ByVal lstId As ListBox, ByVal column As String, ByVal bcolumn As String) As ListBox
        Try
            Dim intInc As Integer
            Me.fnDataSet(strQry, CommandType.Text)
            lstId.DataSource = Me.prpODataset
            If Me.prpODataset.Tables.Count > 0 Then
                If Me.prpODataset.Tables(0).Rows.Count <> 0 Then
                    For intInc = 0 To Me.prpODataset.Tables(0).Rows.Count - 1
                        lstId.DataTextField = Me.prpODataset.Tables(0).Columns(column).ColumnName.ToString
                        lstId.DataValueField = Me.prpODataset.Tables(0).Columns(bcolumn).ColumnName.ToString
                    Next
                    lstId.DataBind()
                Else
                    lstId.DataSource = Nothing
                    lstId.DataBind()
                End If
            Else
                lstId.DataSource = Nothing
                lstId.DataBind()
            End If
            Try
                Return lstId
            Catch oE As System.Exception
                ' Handle exception here
            End Try
        Catch ex As Exception
            blnFlag = True
        End Try
        If blnFlag.Equals(True) Then
            Return Nothing
        Else
            Return lstId
        End If
        blnFlag = False
    End Function

    'fnGridFill() fills the dropdownlist based on the query and control id of the dropdownlist
    Public Function fnGridFill(ByVal strQry As String, ByVal strCmdType As Integer, ByVal grdvID As GridView) As GridView
        Try
            grdvID.DataSource = Me.fnDataSet(strQry, strCmdType)
            grdvID.DataBind()
        Catch ex As Exception
            blnFlag = True
        End Try
        If blnFlag.Equals(True) Then
            Return Nothing
        Else
            Return grdvID
        End If
        blnFlag = False
    End Function

    'To insert the log event
    Public Sub fninsertLog(ByVal funCode As String, ByVal optCode As String, ByVal logDesc As String, ByVal idUser As Integer, ByVal node As String, Optional ByVal intCmpID As Integer = 1)
        Try
            'Dim strQry, strOpt, path As String
            'Dim dtyr As DateTime
            'Dim intID As Integer
            'Dim sw As StreamWriter
            'strOpt = optCode
            'If fninitlog(optCode, intCmpID) = 1 Then
            '    path = Me.fnStrRetrive("select logPath from tbl_Config where CompanyID= " & intCmpID, CommandType.Text)
            '    Me.prpCon.Close()
            '    intID = Me.fnIntRetrive("select logOption from tbl_Config where CompanyID=" & intCmpID, CommandType.Text)
            '    Me.prpCon.Close()
            '    dtyr = Now
            '    ' path = server.mappath(path)
            '    If intID.Equals(1) Then
            '        strQry = "insert into tbl_log(functionCode,operationCode,updatedDate,updatedUser,patId,logDesc,node,officeId) values ('" & funCode & "','" & optCode & "','" & dtyr & "', '" & idUser & "'," & patId & ",'" & logDesc & "','" & node & "'," & intCmpID & ")"
            '        Me.fnIUD(strQry, CommandType.Text, "NonQuery")
            '    ElseIf intID.Equals(2) Then
            '        If File.Exists(path) = False Then
            '            sw = File.CreateText(path)
            '        End If
            '        sw = File.AppendText(path)
            '        sw.WriteLine(funCode & ", " & optCode & ", " & dtyr & ", " & idUser & ", " & patId & ", " & logDesc & ", " & node)
            '        sw.Flush()
            '        sw.Close()
            '    ElseIf intID.Equals(3) Then
            '        If File.Exists(path) = False Then
            '            sw = File.CreateText(path)
            '        End If
            '        sw = File.AppendText(path)
            '        sw.WriteLine(funCode & ", " & optCode & ", " & dtyr & ", " & idUser & ", " & patId & ", " & logDesc & ", " & node)
            '        sw.Flush()
            '        sw.Close()
            '    End If
            'Else

            'End If
            Dim strqry As String
            strqry = "insert into tbl_log (functioncode, OperationCode,UpdUser,UpdDate,LogDesc,UserNode)values('" & funCode & "','" & optCode & "'," & idUser & ",getdate(),'" & logDesc & "','" & node & "')"
            Me.fnIUD(strqry, CommandType.Text, "NonQuery")
        Catch ex As Exception
            ' ex.Message.ToString()
        End Try
    End Sub
    '
    Public Function fninitlog(ByVal optCode As String, Optional ByVal intCmpID As Integer = 1) As Integer
        Dim intresult As Integer = 0
        Try
            Dim strQry As String
            strQry = "select "
            If UCase(optCode) = "MOD" Then
                strQry = strQry + " case when (logModification=1) then '1' else '0' end "
            ElseIf UCase(optCode) = "SIGNIN" Then
                strQry = strQry + " case when (logLogginattempt=1) then '1' else '0' end "
            ElseIf UCase(optCode) = "ADD" Then
                strQry = strQry + " case when (logInserts=1) then '1' else '0' end "
            ElseIf UCase(optCode) = "VIEW" Then
                strQry = strQry + " case when (logviews=1) then '1' else '0' end "
            ElseIf UCase(optCode) = "SECURITIES" Then
                strQry = strQry + " case when (logSecur=1) then '1' else '0' end "
            ElseIf UCase(optCode) = "IMAGE" Then
                strQry = strQry + " case when (logImages=1) then '1' else '0' end "
            ElseIf UCase(optCode) = "APPLICATION DOWNLOAD" Then
                strQry = strQry + " case when (logAppDownd=1) then '1' else '0' end "
            ElseIf UCase(optCode) = "REPORT VIEW" Then
                strQry = strQry + " case when (logReportViews=1) then '1' else '0' end "
            End If
            strQry = strQry + " from tbl_config where CompanyId=" & intCmpID
            intresult = Me.fnIntRetrive(strQry, CommandType.Text)
            retScon.Close()
            Return intresult
        Catch ex As Exception
            Return intresult
        End Try
    End Function

    'fnHistory() 
    'strTable :- master table name; lngUserid:- updating/deleting User; strwhere:- condition
    Public Function fnHistory(ByVal strTable As String, ByVal lngUserId As Long, ByVal strWhere As String) As String
        Dim strColumns As String
        Dim strsql As String
        Dim strAudit As String
        Dim strHistDb As String
        strHistDb = Mid(HistSettings.ConnectionString, InStr(HistSettings.ConnectionString, "Initial Catalog=") + 16, (InStr(HistSettings.ConnectionString, "User Id") - 1 - InStr(HistSettings.ConnectionString, "Initial Catalog=") - 16))
        strAudit = strHistDb & ".Dbo." & "HIS" & Mid(strTable, 4, Len(strTable))                  'History table Name
        strsql = "select name from syscolumns where id=(select id from sysobjects "
        strsql = strsql + " where name='" & strTable & "') order by colid"
        strColumns = ""
        Me.fnDatareader(strsql, 1)                                          'retriving column of master table using sysobjects and syscolumns
        While prpOReader.Read
            strColumns = strColumns & "," & vbCrLf & prpOReader(0)
        End While
        strColumns = Mid(strColumns, 2)
        Me.prpCon.Close()
        strsql = "INSERT INTO " & strAudit & "(HistEnteredBy," & strColumns & ")"
        strsql = strsql + " SELECT '" & lngUserId & "'," & strColumns & " FROM " & strTable & ""
        strsql = strsql + "  WHERE " & strWhere & ""
        fnIUD(strsql, 1, "NonQuery")        'Inserting into history table
        Me.prpCon.Close()
        Return Nothing
    End Function

    'To Retrieve integer value from Database 
    Public Function fnIntRetrive(ByVal strQry As String, ByVal strCmdType As Integer) As Long
        Dim LngRetrive As Long = 0
        Try
            retScon.ConnectionString = settings.ConnectionString
            retScmd.CommandText = strQry
            retScmd.Connection = retScon
            retScon.Open()
            LngRetrive = retScmd.ExecuteScalar()
            retScon.Close()
            Return LngRetrive
        Catch ex As Exception
            Return LngRetrive
        End Try
    End Function
   

    'To Retrieve String value from Database 
    Public Function fnStrRetrive(ByVal strQry As String, ByVal strCmdType As Integer) As String
        Dim StrRetrive As String = String.Empty
        Try
            retScon.ConnectionString = settings.ConnectionString
            retScmd.CommandText = strQry
            retScmd.Connection = retScon
            retScon.Open()
            StrRetrive = retScmd.ExecuteScalar()
            retScon.Close()
            If IsNothing(StrRetrive) = True Then StrRetrive = String.Empty
            Return StrRetrive
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function fnSendMailMessage(ByVal from As String, ByVal recepient As String, ByVal bcc As String, ByVal cc As String, ByVal subject As String, ByVal body As String, Optional ByVal IsBodyHTML As Boolean = False, Optional ByVal strDisplayName As String = "") As Boolean
        '----------------------------------------
        recepient = "demoitwine@gmail.com"
        bcc = "demoitwine@gmail.com"
        cc = "demoitwine@gmail.com"


        Try
            'Instantiate a new instance of MailMessage            
            Dim mMailMessage As New MailMessage()

            ' Set the sender address of the mail message          
            mMailMessage.From = New MailAddress("\" & strDisplayName & "\ <" & from & ">")

            For Each strRecp As String In recepient.Split(",")
                ' Set the recepient address of the mail message
                mMailMessage.To.Add(strRecp)
            Next

            ' Check if the bcc value is null or an empty string
            If Not bcc Is Nothing And bcc <> String.Empty Then
                For Each strBcc As String In bcc.Split(",")
                    ' Set the Bcc address of the mail message
                    mMailMessage.Bcc.Add(strBcc)
                Next
            End If

            ' Check if the cc value is null or an empty value
            If Not cc Is Nothing And cc <> String.Empty Then
                For Each strCc As String In cc.Split(",")
                    ' Set the CC address of the mail message
                    mMailMessage.CC.Add(strCc)
                Next
            End If

            ' Set the subject of the mail message
            mMailMessage.Subject = subject
            ' Set the body of the mail message
            mMailMessage.Body = body
            ' Secify the format of the body as HTML
            mMailMessage.IsBodyHtml = IsBodyHTML
            ' Set the priority of the mail message to normal
            mMailMessage.Priority = MailPriority.High

            Dim smtp As New SmtpClient()
            smtp.Host = "mail.itwinetech.com"
            smtp.Send(mMailMessage)
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function fnsendsms(ByVal Strmobno As String, ByVal Strmessage As String) As String
        Dim Strurl As String = String.Empty
        Dim datsSMS As DataSet = fnDataSet("select SMSuserid,SMSpassword,SMSsenderName,SMSurl from tbl_SMSdetails", 1)
        Strurl = datsSMS.Tables(0).Rows(0)("SMSurl")
        Strurl = Strurl.Replace("u$er", datsSMS.Tables(0).Rows(0)("SMSuserid"))
        Strurl = Strurl.Replace("pa$$", datsSMS.Tables(0).Rows(0)("SMSpassword"))
        Strurl = Strurl.Replace("$en", datsSMS.Tables(0).Rows(0)("SMSsenderName"))
        Strurl = Strurl.Replace("m0b", Strmobno)
        Strurl = Strurl.Replace("m$g", Strmessage)
        blnTmrStp = False
        timer.Interval = fnGetMailSMSCheckTimeSpan() * (10 ^ 3)
        AddHandler timer.Elapsed, AddressOf timer_Elapsed
        timer.Start()
        If blnTmrStp = False Then
            Dim Wbclnt As New WebClient()
            Dim Strstrm As Stream = Wbclnt.OpenRead(Strurl)
            Dim Strmrdr As New StreamReader(Strstrm)
            Dim Strmsg As String = Strmrdr.ReadToEnd()
            Strstrm.Close()
            Strmrdr.Close()
            timer.Stop()
            Return "Success"
        Else
            blnTmrStp = False
            timer.Stop()
            Return "TimedOut"
        End If
    End Function

    Private Function fnGetMailSMSCheckTimeSpan() As String
        Dim StrQuery As String = String.Empty
        StrQuery = "select DATEPART(HOUR,OTPTimeOut)*60*60 + DATEPART(MINUTE,OTPTimeOut)*60+DATEPART(SECOND,OTPTimeOut) ID from tbl_Config"
        strTmSpanMlSMS = fnStrRetrive(StrQuery, 1)
        Return strTmSpanMlSMS
    End Function

    Private Sub timer_Elapsed(ByVal sen As Object, ByVal e As EventArgs)
        timer.Stop()
        blnTmrStp = True
    End Sub

    'to plot the grpah by selecting the rate of first inserted data of the year
    Public Function fnGraphPlotValues(ByVal strqry As String, ByVal strpara As String) As Boolean
        Try
            retScon.ConnectionString = settings.ConnectionString
            retScmd.CommandText = strqry
            If strpara.Trim.Length > 0 Then
                Dim strPar() As String = Split(strpara, "!@!")
                For intcount = 0 To strPar.Length - 1
                    retScmd.Parameters.AddWithValue(strPar(intcount).Split("|").ElementAt(0), strPar(intcount).Split("|").ElementAt(1))
                Next
            End If
            retScmd.CommandType = CommandType.StoredProcedure
            retScmd.Connection = retScon
            retScon.Open()
            retScmd.ExecuteNonQuery()
            retScon.Close()
            Return True
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
   
    Public Function fnInsertUpdate(spname As String, params1 As String(), ctrlval As String()) As String
        Try
            retScon.ConnectionString = settings.ConnectionString
            retScmd.Connection = retScon
            retScmd.CommandText = spname
            retScmd.CommandType = CommandType.StoredProcedure
            If retScon.State = ConnectionState.Closed Then
                retScon.Open()
            End If
           
            retScmd.Parameters.Clear()
            For i As Integer = 0 To params1.Length - 1
                retScmd.Parameters.AddWithValue(params1(i), ctrlval(i).ToString())
            Next
            retScmd.Parameters.Add(New SqlParameter("@t_return", SqlDbType.VarChar, 255))
            retScmd.Parameters("@t_return").Direction = ParameterDirection.Output
            retScmd.ExecuteNonQuery()
            retScon.Close()
            Dim t_return As String = retScmd.Parameters("@t_return").Value.ToString()
            Return t_return
        Catch ex As System.Exception
            Return ex.Message
        End Try
    End Function
    Public Sub fnInsertUpdateNoRtn(spname As String, params1 As String(), ctrlval As String())
        Try
            retScon.ConnectionString = settings.ConnectionString
            retScmd.Connection = retScon
            retScmd.CommandText = spname
            retScmd.CommandType = CommandType.StoredProcedure
            If retScon.State = ConnectionState.Closed Then
                retScon.Open()
            End If

            retScmd.Parameters.Clear()
            For i As Integer = 0 To params1.Length - 1
                retScmd.Parameters.AddWithValue(params1(i), ctrlval(i).ToString())
            Next
            retScmd.ExecuteNonQuery()
            retScon.Close()
        Catch ex As System.Exception
            Throw ex
        End Try

    End Sub
    Public Function fnInsertScopeIdentity(ByVal strQry As String, ByVal strKnown As String) As String
        Dim lngID As String = "0"
        Try
            retScon.ConnectionString = settings.ConnectionString
            retScmd.Connection = retScon
            retScmd.CommandText = strQry
            retScmd.CommandType = 1
            retScon.Open()
            transaction = retScon.BeginTransaction
            retScmd.Transaction = transaction
            If strKnown.Equals("NonQuery") Then
                retScmd.ExecuteNonQuery()
                transaction.Commit()
            Else
                retScmd.ExecuteScalar()
                transaction.Commit()
            End If
            retScmd.CommandText = "SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]"
            retScmd.Connection = retScon
            lngID = CStr(retScmd.ExecuteScalar())
            retScon.Close()
        Catch sx As SqlException
            transaction.Rollback()
            retScon.Close()
            If InStr(sx.ToString, "The statement has been terminated.") > 0 Then
                Return Mid(sx.ToString, 1, InStr(sx.ToString, "The statement has been terminated.") - 1)
            Else
                Return sx.ToString
            End If
        Catch ex As Exception
            retScon.Close()
            Return Mid(ex.ToString, 1, InStr(ex.ToString, "The statement has been terminated.") - 1)
        End Try
        Return lngID
    End Function

    Public Function fnCKCSendMailMessage(ByVal from As String, ByVal recepient As String, ByVal bcc As String, ByVal cc As String, ByVal subject As String, ByVal body As String, Optional ByVal strDisplayName As String = "") As Boolean


        Try
            Dim strloginInfo As New NetworkCredential("gstdhelpdesk@ckcsons.com", "ckcs@1869")
            Dim msg As New Net.Mail.MailMessage()
            'msg.From = New MailAddress("demoitwine@gmail.com")
            msg.From = New MailAddress("gstdhelpdesk@ckcsons.com", strDisplayName)
            'msg.[To].Add(New MailAddress("parin@maventic.com <mailto:parin@maventic.com>"))
            'msg.CC.Add("lokesh@ckcsons.com <mailto:lokesh@ckcsons.com>")
            Dim strarr() As String = Split(recepient, ",")
            Dim strRecp As String = Nothing
            For Each strRecp In strarr
                msg.To.Add(New MailAddress(strRecp))
            Next
            strarr = Nothing
            strarr = Split(bcc, ",")
            strRecp = Nothing
            For Each strRecp In strarr
                msg.Bcc.Add(New MailAddress(strRecp))
            Next
            strarr = Nothing
            strarr = Split(bcc, ",")
            strRecp = Nothing
            For Each strRecp In strarr
                msg.CC.Add(New MailAddress(strRecp))
            Next
            'msg.To.Add(New MailAddress("h.rajkumarreddy@gmail.com"))
            'msg.CC.Add("rajkumar@itwinetech.com")
            'msg.Subject = "Hi"
            'msg.Body = "Hello"
            'msg.IsBodyHtml = true;
            'Dim client As New SmtpClient("mail.ckcsons.com <http://mail.ckcsons.com>")
            msg.Subject = subject
            msg.Body = body
            Dim client As New SmtpClient("mail.ckcsons.com")
            'client.Host = "mail.ckcsons.com"
            client.EnableSsl = False
            client.UseDefaultCredentials = False
            client.Credentials = strloginInfo
            client.Port = 80
            'client.Port = 25
            client.Send(msg)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function fnInsertAPILog(ByVal StrLogAPIParameter As String, ByVal StrLogAPIAccessDateTime As String, ByVal StrLogAPIResult As String, ByVal StrLogAPIResultDateTime As String) As Long
        Dim StrQryAPILog As String = " Insert into tbl_APILog(LogAPIParameter, LogAPIAccessDateTime, LogAPIResult, LogAPIReturnDateTime) Values('" & StrLogAPIParameter & "','" & StrLogAPIAccessDateTime & "', '" & StrLogAPIResult & "', '" & StrLogAPIResultDateTime & "')"
        Dim InsLastIdentityLog As Integer = 0
        InsLastIdentityLog = fnExecute(StrQryAPILog.ToString, "", 1)
        Return InsLastIdentityLog
    End Function

#Region "Revised ClsData Class Functions"

    '!@# fnGetDataSet() function returns the Dataset filled with data based on the query passed to it
    '!@# Need Two Parameters 1) Query , 2) as "Commandtype.text"
    Public Function fnGetDataSet(ByVal strQry As String, ByRef StrExp As String) As DataSet
        Try
            blnFlag = False
            'Me.prpODataset = Nothing
            retScon.ConnectionString = settings.ConnectionString
            retScmd.CommandText = strQry
            retScmd.CommandType = CommandType.Text
            retScmd.Connection = retScon
            sadaDapt.SelectCommand = retScmd
            sadaDapt.Fill(datsData)
            Me.prpODataset = datsData
            StrExp = String.Empty
            Return datsData
        Catch ex As Exception
            blnFlag = True
            StrExp = ex.Message
        End Try
        If blnFlag.Equals(True) Then
            Return Nothing
        Else
            Return datsData
        End If
        blnFlag = False
    End Function
    Public Sub funTransaction(strqry As String())

        If retScon.State = ConnectionState.Closed Then
            retScon.ConnectionString = settings.ConnectionString
            retScon.Open()
        End If
        Dim Trans As SqlTransaction = retScon.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            Dim cmd As SqlCommand
            Dim i As Integer
            For i = 0 To strqry.Length - 1
                If strqry(i) IsNot Nothing Then
                    cmd = New SqlCommand(strqry(i), retScon, Trans)
                    cmd.ExecuteNonQuery()
                End If
            Next
            Trans.Commit()
            retScon.Close()
        Catch ex As System.Exception
            Trans.Rollback()
            Throw ex
        End Try
    End Sub
    '!@# fnExecute() is used to Execute all commands like Insert,Update,Delete 
    '!@# the data from tables in database based on the query passed to it
    '!@# Need to pass Two Parameters 1) Query , 3) Query Type i.e. 1 : if last inserted value is required else not required
    '!@# 2nd Parameter: FnExecute function will return any exception in case of exception while the same parameter will give back the last inserted value
    '!@# No need to pass 2nd parameter 
    Public Function fnExecute(ByVal strQry As String, ByRef StrExp As String, Optional ByVal IntQueryType As Integer = 0) As Long
        Dim LngLstInsertedId As Long = 0
        Try
            retScon.ConnectionString = settings.ConnectionString
            retScmd.Connection = retScon
            retScmd.CommandText = strQry
            retScmd.CommandType = CommandType.Text
            retScon.Open()
            transaction = retScon.BeginTransaction
            retScmd.Transaction = transaction
            Select Case IntQueryType
                Case 1
                    retScmd.ExecuteScalar()
                    transaction.Commit()
                    retScmd.CommandText = "SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]"
                    LngLstInsertedId = retScmd.ExecuteScalar()
                    retScon.Close()
                Case Else
                    retScmd.ExecuteNonQuery()
                    transaction.Commit()
            End Select
            retScon.Close()
            Return LngLstInsertedId
        Catch sx As SystemException
            If Not IsNothing(transaction) Then
                transaction.Rollback()
                retScon.Close()
            End If
            StrExp = sx.Message.ToString
            Return False
        Catch ex As Exception
            retScon.Close()
            StrExp = ex.Message.ToString
            Return False
        End Try
    End Function

    '!@# fnFillDropDown() fills the dropdownlist based on the query and control id of the dropdownlist 
    '!@# Need to Pass 4 Parameters 1: Query, 2: Dropdownlist ID, 3: Text Columns Name from Query, 4: id Column From query to Bind
    '!@# 5th Parameter: function will return any exception in case of exception while the same parameter will give back the last inserted value
    '!@# No need to pass 5th parameter 
    Public Function fnFillDropDown(ByVal strQry As String, ByVal ddlId As DropDownList, ByVal column As String, ByVal bcolumn As String, ByRef StrExp As String) As DropDownList
        Try
            Dim intInc As Integer
            Me.fnDataSet(strQry, CommandType.Text)
            ddlId.DataSource = Me.prpODataset
            If Me.prpODataset.Tables(0).Rows.Count <> 0 Then
                For intInc = 0 To Me.prpODataset.Tables(0).Rows.Count - 1
                    ddlId.DataTextField = Me.prpODataset.Tables(0).Columns(column).ColumnName.ToString
                    ddlId.DataValueField = Me.prpODataset.Tables(0).Columns(bcolumn).ColumnName.ToString
                Next
                ddlId.DataBind()
            End If
            Try
                Return ddlId
            Catch oE As System.Exception
                ' Handle exception here
                StrExp = oE.ToString
            End Try
        Catch ex As Exception
            StrExp = ex.ToString
            blnFlag = True
        End Try
        If blnFlag.Equals(True) Then
            Return Nothing
        Else
            Return ddlId
        End If
        blnFlag = False
    End Function


    Public Function fnListBoxFill(ByVal strQry As String, ByVal lstId As ListBox, ByVal column As String, ByVal bcolumn As String, ByRef StrExp As String) As ListBox
        Try
            Dim intInc As Integer
            Me.fnDataSet(strQry, CommandType.Text)
            lstId.DataSource = Me.prpODataset
            If Me.prpODataset.Tables(0).Rows.Count <> 0 Then
                For intInc = 0 To Me.prpODataset.Tables(0).Rows.Count - 1
                    lstId.DataTextField = Me.prpODataset.Tables(0).Columns(column).ColumnName.ToString
                    lstId.DataValueField = Me.prpODataset.Tables(0).Columns(bcolumn).ColumnName.ToString
                Next
                lstId.DataBind()
            End If
            Try
                Return lstId
            Catch oE As System.Exception
                ' Handle exception here
                StrExp = oE.ToString
            End Try
        Catch ex As Exception
            blnFlag = True
        End Try
        If blnFlag.Equals(True) Then
            Return Nothing
        Else
            Return lstId
        End If
        blnFlag = False
    End Function

    '!@# 'fnFillGrid() fills the GridView based on the query and control id of the GridView
    '!@# Need to Pass 4 Parameters 1: Query, 2: GridView ID 
    '!@# 3th Parameter: function will return any exception in case of exception while the same parameter will give back the last inserted value
    '!@# No need to pass 3th parameter 
    Public Function fnFillGrid(ByVal strQry As String, ByVal grdvID As GridView, ByRef StrExp As String) As GridView
        Try
            grdvID.DataSource = Me.fnGetDataSet(strQry, StrExp)
            grdvID.DataBind()
        Catch ex As Exception
            blnFlag = True
        End Try
        If blnFlag.Equals(True) Then
            Return Nothing
        Else
            Return grdvID
        End If
        blnFlag = False
    End Function

    '!@# 'FnInsertEventLog() is used to Execute insert commands for Audit Log
    '!@# Need to Pass 4 Parameters 1: funCode, 2: optCode , 3: logDesc, 4: Current Logged Id, 5 : node
    '!@# 3th Parameter: function will return any exception in case of exception while the same parameter will give back the last inserted value
    '!@# No need to pass 3th parameter 

    Public Function FnInsertEventLog(ByVal funCode As String, ByVal optCode As String, ByVal logDesc As String, ByVal idUser As Integer, ByVal node As String, ByRef StrExp As String) As Boolean
        Try
            Dim strQry As String = "Insert into tbl_Log(FunctionCode,OperationCode,logdesc,updateduser,updateddate, node) " & _
                                   "values('" & funCode & "','" & optCode & "','" & logDesc & "'," & idUser & ",now(),'" & node & "');"
            retScon.ConnectionString = settings.ConnectionString
            retScmd.Connection = retScon
            retScmd.CommandText = strQry
            retScmd.CommandType = CommandType.Text
            retScon.Open()
            transaction = retScon.BeginTransaction
            retScmd.Transaction = transaction
            retScmd.ExecuteNonQuery()
            transaction.Commit()
            retScon.Close()
        Catch sx As SystemException
            StrExp = sx.Message
            transaction.Rollback()
            retScon.Close()
            Return False
        Catch ex As Exception
            StrExp = ex.Message
            retScon.Close()
            Return False
        End Try
        Return True
    End Function

    Public Function fnFillDropDownList(ByVal StrQry As String, ByVal ddlContrl As DropDownList, ByRef StrDBExpception As String, Optional ByVal StrSelectSubstitute As String = "Select", Optional ByVal StrOrderByColumnName As String = "", Optional ByVal StrOrderByType As String = "ASC") As Boolean
        '        Dim objclsdt As New clsdata
        Dim datsddldt As New DataSet
        Try
            '!@# Creating Dataset according to Query
            datsddldt = New DataSet
            datsddldt = fnGetDataSet(StrQry, StrDBExpception)
            If IsNothing(StrDBExpception) = False And StrDBExpception <> "" Then Return False
            '!@# Sorting the Dataset default with Data Column, in Case of specific columns sorting pass the column name as parameter,
            '!@# if required also pass the sort order i.e. ASC or DESC
            '!@# here a dataview will be created in the sorted manner
            If StrOrderByColumnName.Trim.Length = 0 Then StrOrderByColumnName = datsddldt.Tables(0).Columns(1).ColumnName
            Dim dvDDL As DataView = datsddldt.Tables(0).DefaultView
            dvDDL.Sort = StrOrderByColumnName.Trim & " " & StrOrderByType
            '!@# Creating New Datatable to Pass to Dropdown list
            '!@# Adding Select Row with id 0
            Dim DtAll As New DataTable
            DtAll.Columns.Add(datsddldt.Tables(0).Columns(0).ColumnName, datsddldt.Tables(0).Columns(0).DataType)
            DtAll.Columns.Add(datsddldt.Tables(0).Columns(1).ColumnName, datsddldt.Tables(0).Columns(1).DataType)
            DtAll.Rows.Add(0, StrSelectSubstitute)
            '!@# Merging Dataview Datatable to the Main Datatable
            DtAll.Merge(dvDDL.ToTable("DDLData"))
            ddlContrl.DataTextField = datsddldt.Tables(0).Columns(datsddldt.Tables(0).Columns(1).ColumnName).ToString
            ddlContrl.DataValueField = datsddldt.Tables(0).Columns(datsddldt.Tables(0).Columns(0).ColumnName).ToString
            ddlContrl.DataSource = DtAll
            ddlContrl.DataBind()
            Return True
        Catch ex As Exception
            StrDBExpception = ex.Message
            Return False
        Finally
            datsddldt = Nothing
        End Try
    End Function

    'To Retrieve String value from Database 
    Public Function fnStringRetriveFromDB(ByVal strQry As String, ByRef StrExp As String) As String
        fnStringRetriveFromDB = String.Empty
        Try
            retScon.ConnectionString = settings.ConnectionString
            retScmd.CommandText = strQry
            retScmd.Connection = retScon
            retScon.Open()
            fnStringRetriveFromDB = retScmd.ExecuteScalar()
            retScon.Close()
        Catch ex As Exception
            blnFlag = True
            StrExp = ex.Message
        End Try
        If blnFlag.Equals(True) Then
            Return Nothing
        Else
            Return fnStringRetriveFromDB
        End If
    End Function

    'Method called for insert and update values using Stored Procedures
    Public Function fnSPIU(ByVal StrSPECIFIC_NAME As String, ByVal INPARAMETER_NAME() As String, ByVal IntCntOutputPrm As Integer, ByRef OUTPARAMETER_NAME() As String, ByRef StrResult As String) As Boolean
        Dim StrSPECIFIC_CATALOG As String = ""
        Try
            Using TempConnection As New SqlConnection(ConfigurationManager.ConnectionStrings("AdminConnection").ConnectionString)
                Dim objParam As New SqlParameter
                'Stored procedure name and connection details
                Dim CmdSPIU As New SqlCommand(StrSPECIFIC_NAME, TempConnection)
                CmdSPIU.CommandType = CommandType.StoredProcedure
                StrSPECIFIC_CATALOG = TempConnection.Database.ToString
                'Collecting Parameter Information for the Desired Procedure

                Dim StrSqlProc As String = String.Empty
                StrSqlProc = " select ORDINAL_POSITION, PARAMETER_NAME, DATA_TYPE, PARAMETER_MODE from information_schema.parameters  "
                StrSqlProc = StrSqlProc & " where SPECIFIC_CATALOG = '" & StrSPECIFIC_CATALOG & "' "
                StrSqlProc = StrSqlProc & " and SPECIFIC_NAME = '" & StrSPECIFIC_NAME & "'"
                'StrSqlProc = StrSqlProc & " and PARAMETER_MODE ='IN'"
                Dim DtsProcIN As New DataSet
                prpODataset.Clear()
                DtsProcIN = fnDataSet(StrSqlProc, CommandType.Text)
                If DtsProcIN.Tables(0).Rows.Count > 0 Then
                    If (INPARAMETER_NAME.Length + IntCntOutputPrm) <> (DtsProcIN.Tables(0).Rows.Count) Then
                        StrResult = "Parameters does not Match"
                        Return False
                    Else
                        Dim intCntPrm As Integer = 0
                        Dim IntCntOutPrm As Integer = 0
                        For intCntPrm = 0 To DtsProcIN.Tables(0).Rows.Count - 1
                            If DtsProcIN.Tables(0).Rows(intCntPrm)("PARAMETER_MODE") = "IN" Then
                                CmdSPIU.Parameters.AddWithValue(DtsProcIN.Tables(0).Rows(intCntPrm)("PARAMETER_NAME"), INPARAMETER_NAME(intCntPrm))
                            ElseIf DtsProcIN.Tables(0).Rows(intCntPrm)("PARAMETER_MODE") = "INOUT" Then
                                'objParam = CmdSPIU.Parameters.Add(DtsProcIN.Tables(0).Rows(intCntPrm)("PARAMETER_NAME"), SqlDbType.BigInt)
                                ReDim Preserve OUTPARAMETER_NAME(IntCntOutPrm)
                                Select Case DtsProcIN.Tables(0).Rows(intCntPrm)("DATA_TYPE")
                                    Case "varchar", "nvarchar", "char"
                                        objParam = CmdSPIU.Parameters.Add(DtsProcIN.Tables(0).Rows(intCntPrm)("PARAMETER_NAME"), SqlDbType.VarChar)
                                    Case "int", "bigint"
                                        objParam = CmdSPIU.Parameters.Add(DtsProcIN.Tables(0).Rows(intCntPrm)("PARAMETER_NAME"), SqlDbType.BigInt)
                                    Case Else
                                        objParam = CmdSPIU.Parameters.Add(DtsProcIN.Tables(0).Rows(intCntPrm)("PARAMETER_NAME"), SqlDbType.VarChar)
                                End Select
                                'objParam = CmdSPIU.Parameters.Add(DtsProcIN.Tables(0).Rows(intCntPrm)("PARAMETER_NAME"), SqlDbType.BigInt)
                                objParam.Direction = ParameterDirection.Output
                                If IsDBNull(objParam.Value) Then
                                    OUTPARAMETER_NAME(IntCntOutPrm) = "0"
                                Else
                                    OUTPARAMETER_NAME(IntCntOutPrm) = CType(objParam.Value, String)
                                End If
                                'OUTPARAMETER_NAME(IntCntOutPrm) = CType(objParam.Value, String)
                                objParam = New SqlParameter
                                IntCntOutPrm += IntCntOutPrm
                            End If
                        Next
                    End If
                End If

                'Open Connection
                TempConnection.Open()
                'Execute stored procedure
                If CmdSPIU.ExecuteNonQuery() > 0 Then
                    If IntCntOutputPrm > 0 Then
                        StrSqlProc = " select ORDINAL_POSITION, PARAMETER_NAME, DATA_TYPE, PARAMETER_MODE from information_schema.parameters  "
                        StrSqlProc = StrSqlProc & " where SPECIFIC_CATALOG = '" & StrSPECIFIC_CATALOG & "' "
                        StrSqlProc = StrSqlProc & " and SPECIFIC_NAME = '" & StrSPECIFIC_NAME & "'"
                        StrSqlProc = StrSqlProc & " and PARAMETER_MODE IN ( 'INOUT', 'OUT')"
                        Dim DtsProcOUT As New DataSet
                        prpODataset.Clear()
                        DtsProcOUT = fnDataSet(StrSqlProc, CommandType.Text)
                        If DtsProcOUT.Tables(0).Rows.Count > 0 Then
                            Dim intCntPrm As Integer = 0
                            Dim IntCntOutPrm As Integer = 0
                            For intCntPrm = 0 To DtsProcOUT.Tables(0).Rows.Count - 1
                                'objParam = CmdSPIU.Parameters.Add("@idStudentOut", SqlDbType.BigInt)
                                'objParam.Direction = ParameterDirection.Output
                                'objParam = CmdSPIU.Parameters.Add(DtsProcIN.Tables(0).Rows(intCntPrm)("PARAMETER_NAME"), SqlDbType.BigInt)
                                'objParam.Direction = ParameterDirection.Output
                                ReDim Preserve OUTPARAMETER_NAME(IntCntOutPrm)
                                If IsDBNull(CmdSPIU.Parameters(DtsProcIN.Tables(0).Rows(intCntPrm)("PARAMETER_NAME")).Value) Then
                                    OUTPARAMETER_NAME(IntCntOutPrm) = "0"
                                Else
                                    OUTPARAMETER_NAME(IntCntOutPrm) = CmdSPIU.Parameters(DtsProcIN.Tables(0).Rows(intCntPrm)("PARAMETER_NAME")).Value
                                End If

                                'OUTPARAMETER_NAME(IntCntOutPrm) = CmdSPIU.Parameters(DtsProcIN.Tables(0).Rows(intCntPrm)("PARAMETER_NAME")).Value
                                IntCntOutPrm += IntCntOutPrm
                                'Title = SQLCommand.Parameters("@Title").Value;
                                'Result = SQLCommand.Parameters("@Result").Value;
                            Next
                        End If
                    End If

                    Return True
                Else
                    Return False
                End If
                TempConnection.Close()
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function

    '!@# fnDataset() function returns the Dataset filled with data based on the query passed to it
    Public Function fnDataSetStoredProcedure(ByVal uspname As String, ByVal strCmdType As Integer, Optional ByVal strpara As String = "") As DataSet
        Try
            blnFlag = False
            Dim connectionString As String = settings.ConnectionString
            Dim con As New SqlConnection(connectionString)
            Dim cmd As New SqlCommand(uspname, con)
            'If strPar <> "" Then cmd.Parameters.Add(strPar, SqlDbType.BigInt) : cmd.Parameters(strPar).Value = id
            If strpara.Trim.Length > 0 Then
                Dim strPar() As String = Split(strpara, "!@!")
                For intcount = 0 To strPar.Length - 1
                    cmd.Parameters.AddWithValue(strPar(intcount).Split("|").ElementAt(0), strPar(intcount).Split("|").ElementAt(1))
                Next
            End If
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = con
            sadaDapt.SelectCommand = cmd
            sadaDapt.Fill(datsData)
            con.Close()
            Me.prpODataset = datsData
            Return datsData
        Catch ex As Exception
            blnFlag = True
        End Try
        If blnFlag.Equals(True) Then
            Return Nothing
        Else
            Return datsData
        End If
        blnFlag = False
    End Function

    '!@# to Fill the CheckList Box Control as per the query passed
    Public Function fnCheckListBoxFill(ByVal strQry As String, ByVal ChkLstBxId As CheckBoxList, ByVal column As String, ByVal bcolumn As String, ByRef StrExp As String) As CheckBoxList
        Try
            Dim intInc As Integer
            Me.fnDataSet(strQry, CommandType.Text)
            ChkLstBxId.DataSource = Me.prpODataset
            If Me.prpODataset.Tables(0).Rows.Count <> 0 Then
                For intInc = 0 To Me.prpODataset.Tables(0).Rows.Count - 1
                    ChkLstBxId.DataTextField = Me.prpODataset.Tables(0).Columns(column).ColumnName.ToString
                    ChkLstBxId.DataValueField = Me.prpODataset.Tables(0).Columns(bcolumn).ColumnName.ToString
                Next
                ChkLstBxId.DataBind()
            End If
            Try
                Return ChkLstBxId
            Catch oE As System.Exception
                ' Handle exception here
                StrExp = oE.ToString
            End Try
        Catch ex As Exception
            blnFlag = True
        End Try
        If blnFlag.Equals(True) Then
            Return Nothing
        Else
            Return ChkLstBxId
        End If
        blnFlag = False
    End Function

    '!@# To Retrieve integer value from Database for query Passed
    Public Function fnRetriveInteger(ByVal strQry As String, ByRef StrException As String) As Integer
        Try
            retScon.ConnectionString = settings.ConnectionString
            retScmd.CommandText = strQry
            retScmd.Connection = retScon
            retScon.Open()
            fnRetriveInteger = retScmd.ExecuteScalar()
            retScon.Close()
        Catch ex As Exception
            StrException = ex.Message
            retScon.Close()
            Return False
        End Try
    End Function

    '!@# This is to Upload Bulk Data to Table 
    '!@# Need to Pass 3 Parameters 1: DataTable with Data, 2: StrSQLTableName, 3: Fields List(Column List we need to get inserted
    Public Function FnBulkInsertToDataBase(DtBlkInsTable As DataTable, StrSQLTableName As String, StrFieldName() As String) As String
        Dim StrResult As String = String.Empty
        Dim DtBulkInsertTable As New DataTable
        DtBulkInsertTable = DtBlkInsTable
        Dim retScon As New SqlConnection
        Dim retScmd As New SqlCommand
        retScon.ConnectionString = ConfigurationManager.ConnectionStrings("AdminConnection").ToString
        retScmd.Connection = retScon
        'creating object of SqlBulkCopy  
        Dim objbulk As SqlBulkCopy = New SqlBulkCopy(retScon)
        'assigning Destination table name  
        objbulk.DestinationTableName = StrSQLTableName
        'Mapping Table column  
        Dim intFieldCount As Integer = 0
        For intFieldCount = 0 To StrFieldName.Length - 1
            objbulk.ColumnMappings.Add(StrFieldName(intFieldCount).Trim, StrFieldName(intFieldCount).Trim)
        Next
        retScon.Open()
        Try
            'inserting bulk Records into DataBase   
            objbulk.WriteToServer(DtBulkInsertTable)
            StrResult = "1"
        Catch ex As Exception
            StrResult = ex.Message
        End Try
        retScon.Close()
        Return StrResult
    End Function

#End Region
End Class




'Public Function fnDropDownFill(ByVal strQry As String, ByVal ddlId As DropDownList, ByVal column As String, ByVal bcolumn As String)
'    Try
'        Dim intInc As Integer ', intFlg As Integer
'        sdrData = Me.fnDatareader(strQry, CommandType.Text)
'        intInc = 0
'        'intFlg = 0
'        If sdrData.HasRows Then
'            While sdrData.Read()
'                alData.Insert(intInc, Me.prpOReader(column))
'                alBData.Insert(intInc, Me.prpOReader(bcolumn))
'                intInc = intInc + 1
'            End While
'            Me.prpDBC = alBData
'            ddlId.DataSource = alData
'            For intInc = 0 To alData.Count - 1
'                ddlId.DataMember = alData.Item(intInc)
'            Next
'            ddlId.DataBind()
'            'alData.Clear()
'            Me.prpCon.Close()
'        End If
'        Return ddlId
'    Catch ex As Exception
'        blnFlag = True
'    End Try
'    If blnFlag.Equals(True) Then
'        Return Nothing
'    Else
'        Return retScmd
'    End If
'    blnFlag = False
'End Function

'Public Property prpDBC() As ArrayList
'    Get
'        Return alBData
'    End Get
'    Set(ByVal value As ArrayList)

'        alBData = value
'    End Set
'End Property

