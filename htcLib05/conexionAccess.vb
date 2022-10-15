Imports System.Data.OleDb
Public Class conexionAccess

    Inherits conectador

    Private cons As Collection
    Private _Con1 As OleDbConnection

    Private _conStr As String 'variable para la propiedad

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal constr As String)
        MyBase.New(constr)
    End Sub

    Public Sub New(ByVal constr As String, ByVal tipo As tiposConexion)
        MyBase.New(constr, tipo)
    End Sub


    Public Overrides Function nuevaCon(ByVal constr As String) As IDbConnection
        Return New OleDbConnection(constr)
    End Function

    Public Overrides Function nuevocmd(ByVal sql As String) As IDbCommand
        Return New OleDbCommand(sql, Me.con)
    End Function

    Public Overrides Function formatofecha(ByVal fecha As Date, Optional ByVal conHora As Boolean = False) As String
        Return funciones.FormatoFecha(fecha, conHora) 'access = parametros default
    End Function

    Public Overrides Function secuencial(ByVal nomtabla As String) As Long
        Return funciones.secuencial(nomtabla)
    End Function

End Class
