Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic

Public Class clsDEValidations
    'this function should return "TRUE" or appropriate validation error msg as string
    Public Function fnCustomValidate(ByVal intCaseNum As Integer, ByVal strArrParamters As String()) As String
        'parameters to be received by this function can be obtained in the array strArrParamters
        'strArrParamters contains case number as a first element which should be ignored.
        Dim strReturn As String = String.Empty
        Select Case (intCaseNum)
            Case 1
                'Return "TRUE"
                'OR
                'Return "Appropriate validation msg"
                strReturn = String.Empty
            Case Else
                strReturn = "Invalid case number in configuration"
        End Select
        Return strReturn
    End Function
End Class
