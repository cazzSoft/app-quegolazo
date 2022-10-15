Public Enum tiposdatos
    entero
    texto
    fecha
End Enum

Public MustInherit Class conectador
    Private cons As Collection
    Private _Con1 As IDbConnection

    Private _conStr As String 'variable para la propiedad

    Public tipo As tiposConexion
    Public trans As IDbTransaction

    Public MustOverride Function nuevaCon(ByVal constr As String) As IDbConnection
    Public MustOverride Function nuevocmd(ByVal sql As String) As IDbCommand
    Public MustOverride Function formatofecha(ByVal fecha As Date, Optional ByVal conHora As Boolean = False) As String


    Public Property constr() As String
        Get
            Dim logre As Boolean

            If _conStr = "" Then
                Try
                    _conStr = System.Web.Configuration.WebConfigurationManager.AppSettings("CON")
                    logre = True
                Catch ex As Exception
                    logre = False
                End Try
            Else
                logre = True
            End If

            If logre = False Then _conStr = "estandar apppath,"

            Return _conStr
        End Get
        Set(ByVal Value As String)
            _conStr = Value
        End Set
    End Property

    Sub New()
        Me.tipo = tiposConexion.Libre
    End Sub

    Sub New(ByVal constr As String)
        Me.constr = constr
        Me.tipo = tiposConexion.Libre
    End Sub

    Sub New(ByVal conStr As String, ByVal tipo As tiposConexion)
        Me.constr = conStr
        Me.tipo = tipo

        Select Case tipo
            Case tiposConexion.Windows
                _Con1 = nuevaCon(conStr)
                _Con1.Open()

            Case tiposConexion.Web
                If cons Is Nothing Then cons = New Collection

        End Select
    End Sub

    Sub cerrar()
        If Not _Con1 Is Nothing Then
            _Con1.Close()
            _Con1.Dispose()
        End If

        If Not Me.cons Is Nothing Then
            Try
                Dim con As IDbConnection = cons(System.Web.HttpContext.Current.Session.SessionID)
                con.Close()
                con.Dispose()

                cons.Remove(System.Web.HttpContext.Current.Session.SessionID)
            Catch ex As Exception
            End Try
        End If

    End Sub

    Public ReadOnly Property con() As IDbConnection
        Get
            Dim con1 As IDbConnection

            'If _Con1 Is Nothing OrElse _Con1.State = ConnectionState.Closed Then
            Select Case tipo
                Case tiposConexion.Libre
                    '_Con1 = nuevaCon(constr)
                    '_Con1.Open()

                    'con1 = _Con1 ' la mantengo para luego cerrarla

                    'devuelvo una nueva cada vez
                    con1 = nuevaCon(constr)
                    con1.Open()

                Case tiposConexion.Web

                    Try
                        con1 = CType(cons(System.Web.HttpContext.Current.Session.SessionID), IDbConnection)
                    Catch ex As System.ArgumentException
                        con1 = nuevaCon(constr)
                        con1.Open()
                        Try
                            cons.Add(con1, System.Web.HttpContext.Current.Session.SessionID)
                        Catch ex1 As Exception

                        End Try

                    Catch ex As Exception
                        Throw ex
                    End Try

                    If con1.State = ConnectionState.Closed Then
                        con1.ConnectionString = constr
                        con1.Open()
                    End If


                Case tiposConexion.Windows
                    con1 = _Con1
                Case Else
                    con1 = _Con1

            End Select


            'Else
            'con1 = _Con1
            'End If


            Return con1

        End Get
    End Property

    Friend Sub cierraLibre1()
        If Me.tipo = tiposConexion.Libre And Me.trans Is Nothing Then
            _Con1.Close()
            _Con1.Dispose()
        End If
    End Sub

#Region "ejecutas"
    'devuelve el numero de registros afectados
    Public Overridable Function ejecuta(ByVal sql As String, ByVal ParamArray paras() As Object) As Long
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

        If Not sql.ToLower.StartsWith("select") And Not sql.ToLower.StartsWith("update") And Not sql.ToLower.StartsWith("insert") And Not sql.ToLower.StartsWith("delete") Then
            cmd.CommandType = CommandType.StoredProcedure
        End If

        'reg = cmd.ExecuteNonQuery()
        reg = cmd.ExecuteNonQuery()
        ' si la llamada fue libre, cierro la conexion
        If Me.tipo = tiposConexion.Libre Then  'And Me.trans Is Nothing 
            cmd.Connection.Close()
            cmd.Connection.Dispose()
        End If

        cmd.Dispose()


        Return reg

    End Function

    Public Overridable Function ejecutaEscalar(ByVal sql As String, ByVal tipoDato As tiposdatos, ByVal ParamArray paras() As Object) As Object
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

        If Not sql.ToLower.StartsWith("select") Then
            cmd.CommandType = CommandType.StoredProcedure
        End If

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

    Public Overridable Function traetabla(ByVal sql As String, ByVal ParamArray paras() As Object) As Data.DataTable
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
        If sql.ToLower.StartsWith("select") = False Then cmd.CommandType = CommandType.StoredProcedure

        cmd.Transaction = Me.trans

        Dim ad As New Data.OleDb.OleDbDataAdapter
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

    ''' <summary>
    ''' Retorla un iDataReader, se debe cerrar la conexion despues de usar el dataReader
    ''' </summary>
    ''' <param name="sql"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function traeReader(ByVal sql As String) As IDataReader
        Dim dr As IDataReader
        Dim cmd As IDbCommand

        cmd = nuevocmd(sql)

        cmd.Transaction = Me.trans

        If sql.ToLower.StartsWith("select") = False Then cmd.CommandType = CommandType.StoredProcedure

        ' si la llamada fue libre, cierro la conexion
        If Me.tipo = tiposConexion.Libre Then  'And Me.trans Is Nothing 
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            'se cierrra la conexion cuando se cierra el reader
        Else
            dr = cmd.ExecuteReader()
        End If

        Return dr

    End Function


    Public Overridable Function InsertarTabla(ByVal tabla As DataTable) As Boolean
        Select Case htcLib.espacio.tipoDeBase
            Case tiposBase.Access
                InsertaTableAccess(tabla)
            Case tiposBase.Sql
                InsertaTableSQLServer(tabla)
        End Select
    End Function

    ''' <summary>
    ''' Inserta los datos de un datatable en la base de datos.
    ''' La tabla ya debe existir y debe ser similar a la que se va ingresar en nombre y en tipos de datos
    ''' </summary>
    ''' <param name="tabla">Tabla a insertar</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function InsertaTableAccess(ByVal tabla As DataTable) As Boolean
        Dim ad As OleDb.OleDbDataAdapter
        Dim build As OleDb.OleDbCommandBuilder
        Dim nombreTabla As String

        nombreTabla = tabla.TableName
        ad = New OleDb.OleDbDataAdapter(String.Format("SELECT * FROM {0}", nombreTabla), htcLib.espacio.ManejadorBD.constr)
        build = New OleDb.OleDbCommandBuilder(ad)
        ad.InsertCommand = build.GetInsertCommand
        ad.Update(tabla)
    End Function

    Private Shared Function InsertaTableSQLServer(ByVal tabla As DataTable) As Boolean
        Dim ad As SqlClient.SqlDataAdapter
        Dim build As SqlClient.SqlCommandBuilder
        Dim nombreTabla As String

        nombreTabla = tabla.TableName
        ad = New SqlClient.SqlDataAdapter(String.Format("SELECT * FROM {0}", nombreTabla), htcLib.espacio.ManejadorBD.constr)
        build = New SqlClient.SqlCommandBuilder(ad)
        ad.InsertCommand = build.GetInsertCommand
        ad.Update(tabla)
    End Function
#End Region

    Public Sub abreTransaccion()
        Me.trans = Me.con.BeginTransaction
    End Sub

    Public Sub cierraTransaccion(ByVal comit As Boolean)

        If comit Then
            Me.trans.Commit()
        Else
            Me.trans.Rollback()
        End If

        Me.trans = Nothing

    End Sub

    Public Overridable Function secuencial(ByVal nomtabla As String) As Long
        Return funciones.secuencial(nomtabla)
    End Function

End Class

