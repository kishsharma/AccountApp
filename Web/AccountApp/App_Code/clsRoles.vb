Imports Microsoft.VisualBasic
Imports System.Data

Public Class clsRoles
    Shared intActivate As Integer
    'Function to find whether specific role has got access to a specific page or not 
    Public Function fnPageAccess(ByVal strFunction As String) As Boolean
        Dim dtRole As DataTable = CType(HttpContext.Current.Session("Roles"), DataTable)
        If dtRole.Select("functioncode=" & Chr(39) & strFunction & Chr(39)).Length > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    'Function to find whether specific role has got the access or not
    Public Function fnFindRole(ByVal strFunction As String, ByVal ctrlOperation As String) As Boolean
        Dim objdata As New clsData
        Try
            Dim intAdminID As Integer = objdata.fnIntRetrive("select iddefinition from tbl_DefinitionMS where idDefType='roles' and DefinitionCode='ad'", CommandType.Text)
            objdata.prpODataset.Clear()
            objdata.prpODataset.Reset()
            Dim intSuperID As Integer = objdata.fnIntRetrive("select iddefinition from tbl_DefinitionMS where idDefType='roles' and DefinitionCode='su'", CommandType.Text)
            objdata.prpODataset.Clear()
            objdata.prpODataset.Reset()
            If HttpContext.Current.Session("idrole") = intAdminID Or HttpContext.Current.Session("idrole") = intSuperID Then Return True
            If DirectCast(HttpContext.Current.Session("Roles"), DataTable).Select("functioncode='" & strFunction & "' and operationcode='" & ctrlOperation & "'").Length > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        Finally
            objdata = Nothing
        End Try

    End Function
End Class
