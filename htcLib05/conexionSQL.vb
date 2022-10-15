Imports System.Data.SqlClient

Public Class conexionSQL
    Inherits conectador

    Private cons As Collection
    Private _Con1 As SqlConnection

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
        Return New SqlConnection(constr)
    End Function

    Public Overrides Function nuevocmd(ByVal sql As String) As IDbCommand
        Return New SqlCommand(sql, Me.con)
    End Function

    Public Overrides Function formatofecha(ByVal fecha As Date, Optional ByVal conHora As Boolean = False) As String
        Return funciones.FormatoFecha(fecha, conHora, "'", "")
        'ej : '20060601'
    End Function

    Public Overrides Function secuencial(ByVal nomtabla As String) As Long
        Return funciones.secuencialSP(nomtabla)
    End Function


#Region "ejecutas"
    'devuelve el numero de registros afectados
    Public Overrides Function ejecuta(ByVal sql As String, ByVal ParamArray paras() As Object) As Long
        Dim sql1 As New Text.StringBuilder(sql)
        Dim i As Short
        Dim cmd As IDbCommand
        Dim reg As Long

        For i = 0 To paras.GetUpperBound(0)
            If Not paras(i).GetType Is GetType(System.DateTime) Then
                sql1.Append(" {0} ", paras(i).ToString)
            Else
                sql1.Append(" {0} ", formatofecha(paras(i).ToString))
            End If
        Next

        cmd = nuevocmd(sql1.ToString.TrimEnd(","))
        cmd.Transaction = Me.trans  'si existe

        reg = cmd.ExecuteNonQuery()

        ' si la llamada fue libre, cierro la conexion
        If Me.tipo = tiposConexion.Libre Then  'And Me.trans Is Nothing 
            cmd.Connection.Close()
            cmd.Connection.Dispose()
        End If


        cmd.Dispose()

        Return reg

    End Function


    Public Overrides Function ejecutaEscalar(ByVal sql As String, ByVal tipoDato As tiposdatos, ByVal ParamArray paras() As Object) As Object
        Dim sql1 As New Text.StringBuilder(sql)
        Dim i As Short
        Dim cmd As IDbCommand
        Dim temp As Object

        For i = 0 To paras.GetUpperBound(0)
            If Not paras(i).GetType Is GetType(System.DateTime) Then
                sql1.AppendFormat(" {0},", paras(i).ToString)
            Else
                sql1.AppendFormat(" {0},", formatofecha(paras(i).ToString))
            End If
        Next

        cmd = nuevocmd(sql1.ToString.TrimEnd(","))
        cmd.Transaction = Me.trans  'si existe

        temp = cmd.ExecuteScalar

        ' si la llamada fue libre, cierro la conexion
        If Me.tipo = tiposConexion.Libre Then  'And Me.trans Is Nothing 
            cmd.Connection.Close()
            cmd.Connection.Dispose()
        End If

        cmd.Dispose()

        Select Case tipoDato
            Case tiposdatos.entero
                If Not IsDBNull(temp) Then Return Convert.ToDecimal(temp) Else Return 0

            Case tiposdatos.fecha
                If Not IsDBNull(temp) Then Return Convert.ToDateTime(temp) Else Return DateTime.MinValue

            Case tiposdatos.texto
                If Not IsDBNull(temp) Then Return Convert.ToString(temp) Else Return ""
            Case Else
                Return 0
        End Select

    End Function

    Public Overrides Function traetabla(ByVal sql As String, ByVal ParamArray paras() As Object) As Data.DataTable
        Dim dt As New Data.DataTable
        Dim i As Long
        Dim cmd As IDbCommand

        For i = 0 To paras.GetUpperBound(0)
            If Not paras(i).GetType Is GetType(System.DateTime) Then
                sql = sql & " " & paras(i).ToString & ","
            Else
                sql = sql & " " & Me.formatofecha(paras(i)) & ","
            End If
        Next

        sql = sql.TrimEnd(",")

        cmd = nuevocmd(sql)

        cmd.Transaction = Me.trans

        Dim ad As New SqlClient.SqlDataAdapter
        ad.SelectCommand = cmd

        ad.Fill(dt)

        ' si la llamada fue libre, cierro la conexion
        If Me.tipo = tiposConexion.Libre Then  'And Me.trans Is Nothing 
            cmd.Connection.Close()
            cmd.Connection.Dispose()
        End If


        cmd.Dispose()
        ad.Dispose()

        Return dt

    End Function

    Public Overrides Function traeReader(ByVal sql As String) As IDataReader
        Dim dr As IDataReader
        Dim cmd As IDbCommand

        cmd = nuevocmd(sql)

        cmd.Transaction = Me.trans

        ' si la llamada fue libre, cierro la conexion
        If Me.tipo = tiposConexion.Libre Then  'And Me.trans Is Nothing 
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            'se cierrra la conexion cuando se cierra el reader
        Else
            dr = cmd.ExecuteReader()
        End If

        Return dr

    End Function


    

#End Region

End Class

