Public Class funciones

    ''' <summary>
    ''' Retorna el punto decimal utilizado por el servidor
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function puntoDecimal() As String
        Dim a As Decimal = 1.1
        Dim sep As String = a.ToString.Substring(1, 1)
        Return sep
    End Function

    
    ''' <summary>
    ''' cambia un texto ingresado, por el formato que la pc acepte.
    ''' ejemplo, alguien escribe 1,5 y el formato de pc es puntos, si no lo cambio se hace 15
    ''' ASUMO que no puso la coma como separador de miles...DEBO arreglar esto
    ''' </summary>
    ''' <param name="num"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CambiaNumSegunFormato(ByVal num As String) As Decimal
        Dim a As Char = puntoDecimal()
        Dim temp As String

        temp = num.Replace(",", a)
        temp = temp.Replace(".", a)

        Return Convert.ToDecimal(temp)

    End Function

    ''' <summary>
    ''' Cambia las comas por puntos
    ''' </summary>
    ''' <param name="num"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function FormatoNUMbase(ByVal num As Double) As String
        Return num.ToString.Replace(",", ".")
    End Function

    ''' <summary>
    ''' Solo se debe utilizar para presentaciones, no para consultas en bases de datos
    ''' </summary>
    ''' <param name="fecha"></param>
    ''' <param name="conHora"></param>
    ''' <param name="signo"></param>
    ''' <param name="sep"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function FormatoFecha(ByVal fecha As DateTime, Optional ByVal conHora As Boolean = False, Optional ByVal signo As String = "#", Optional ByVal sep As String = "/") As String
        Dim temp As String
        Dim formato As String

        If fecha = Date.MinValue Then Return 0

        If conHora = False Then
            formato = String.Format("yyyy{0}MM{0}dd", sep)
            temp = signo & Format(fecha, formato) & signo
        Else
            formato = String.Format("yyyy{0}MM{0}dd HH:mm", sep)
            temp = signo & Format(fecha, formato) & signo
        End If

        Return temp

    End Function

    ''' <summary>
    ''' Busca en un arraylist un objeto que tenga el mismo id que el enviado
    ''' </summary>
    ''' <param name="id"></param>
    ''' <param name="AL"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetItemDeAl(ByVal id As Long, ByVal AL As ArrayList) As Object
        Dim ob As Object

        For Each ob In AL
            If ob.id = id Then Return ob
        Next

        Return Nothing

    End Function

    'Busca el objeto con codigo espcifico en un arraylist
    Public Shared Function GetItemDeAl(ByVal codigo As String, ByVal AL As ArrayList) As Object
        Dim ob As Object

        For Each ob In AL
            If ob.codigo = codigo Then Return ob
        Next

        Return Nothing

    End Function


    Friend Shared Function Secuencial(ByVal nombreTabla As String) As Long
        Dim sqlUpdate, sql, tablaSistema, tablasecuencial As String
        Dim retorno, ejecucion As Long
        Dim dtr As IDataReader
        Dim sePuede As Boolean


        tablasecuencial = "HtcObjs"

        sqlUpdate = String.Format("UPDATE {0} set id=id+1 WHERE nombre='{1}'", tablasecuencial, nombreTabla)
        ejecucion = espacio.ManejadorBD.ejecuta(sqlUpdate)

        If ejecucion = 0 Then 'en el caso de no existir en la tabla de secuenciales
            Select Case espacio.tipoDeBase
                Case tiposBase.Access
                    tablaSistema = "MSysObjects"
                Case tiposBase.Sql
                    tablaSistema = "SysObjects"
            End Select

            sql = String.Format("SELECT NAME FROM {0} WHERE NAME='{1}'", tablaSistema, nombreTabla)
            dtr = espacio.ManejadorBD.traeReader(sql)
            'si existe la tabla en el sistema se puede crear en la tabla
            If dtr.Read Then sePuede = True
            dtr.Close()

            If sePuede Then
                sql = String.Format("INSERT INTO {0}(id,nombre) Values(1,'{1}')", tablasecuencial, nombreTabla)
                espacio.ManejadorBD.ejecuta(sql)
                retorno = 1
            End If

        Else
            sql = String.Format("SELECT id FROM {0} WHERE nombre='{1}';", tablasecuencial, nombreTabla)
            retorno = espacio.ManejadorBD.ejecutaEscalar(sql, tiposdatos.entero)
        End If

        Return retorno
    End Function


    ''' <summary>
    ''' Llama a un proceso en sql que genere el id del objeto. Es mas rápido
    ''' </summary>
    ''' <param name="nomtabla"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function SecuencialSP(ByVal nomtabla As String) As Long
        Dim sql As String
        Dim temp As Long

        'Try
        sql = String.Format("htc_Secuencia '{0}'", nomtabla)
        temp = espacio.ManejadorBD.ejecutaEscalar(sql, tiposdatos.entero)
        Return temp       


        'Catch ex As Exception 'creamos un error para generar el procedimiento, si no existe
        'Dim mensaje As String
        'mensaje = ex.Message
        'If mensaje.IndexOf("htc_Secuencia") <> -1 Then
        '    crearProcesoHTC()
        '    temp = secuencialSP(nomtabla)
        '    Return temp
        'End If

        'End Try

    End Function

    ''' <summary>
    ''' Crea el proceso y la tabla que requiere si estos fuesen necesarios
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub CrearProcesoHTC()
        Dim sql As System.Text.StringBuilder

        CrearTablaHTCObjs() 'primero creamos la tabla

        sql = New System.Text.StringBuilder

        With sql
            .AppendLine("create procedure htc_Secuencia(@Nombretabla varchar(100))")
            .AppendLine("as begin")
            .AppendLine("if exists( SELECT name FROM sysobjects WHERE name=@Nombretabla ) AND not exists(SELECT * from HtcObjs WHERE nombre=@NombreTabla)")
            .AppendLine("begin")
            .AppendLine("Insert into HtcObjs(id, nombre) values(0,@NombreTabla)")
            .AppendLine("end")
            .AppendLine("Begin tran")
            .AppendLine("UPDATE HtcObjs SET id=id+1 WHERE nombre=@Nombretabla")
            .AppendLine("select id from HtcObjs WHERE nombre=@Nombretabla")
            .AppendLine("Commit tran")
            .AppendLine("end")
        End With

        espacio.ManejadorBD.ejecuta(sql.ToString)
    End Sub

    Private Shared Sub CrearTablaHTCObjs()
        Dim sql As System.Text.StringBuilder

        If espacio.tipoDeBase = tiposBase.Sql Then
            sql = New System.Text.StringBuilder
            'para ver si existe la tabla
            sql.AppendLine("if not exists( SELECT name from sysobjects WHERE name='HTCObjs') ")
            sql.AppendLine("begin")
            sql.AppendLine("create table HTCObjs(id int,nombre varchar(50))")
            sql.AppendLine("end")
            espacio.ManejadorBD.ejecuta(sql.ToString)
        End If

    End Sub

    Public Shared Sub SincronizarSecuenciales()

        Select Case espacio.tipoDeBase
            Case tiposBase.Access
                SincronizarSecuencialesAccess()
            Case tiposBase.Sql
                SincronizarSecuencialesSQL()
        End Select
    End Sub

    Private Shared Sub SincronizarSecuencialesAccess()
        Dim sql, tablaSistema As String
        Dim dataR As IDataReader
        Dim sincronizoConTblSecs As Boolean
        Dim htcObjs, tblSecs As DataTable
        Dim fila, filanueva As DataRow
        Dim maximo As Long
        Dim columna As DataColumn

        'primero verifico si existe la tabla tblsecs para versiones antiguas y sincronizo
        tablaSistema = "MSysObjects"

        sql = String.Format("SELECT name FROM {0} WHERE name='{1}'", tablaSistema, "tblSecs")
        dataR = espacio.ManejadorBD.traeReader(sql)
        If dataR.Read Then sincronizoConTblSecs = True
        dataR.Close()
        sql = String.Format("SELECT * FROM HTCObjs")
        htcObjs = htcLib.espacio.ManejadorBD.traetabla(sql)

        If sincronizoConTblSecs Then
            sql = String.Format("SELECT * FROM tblSecs")
            tblSecs = espacio.ManejadorBD.traetabla(sql)

            For Each columna In tblSecs.Columns
                If htcObjs.Select(String.Format("Nombre='{0}'", columna.ColumnName)).Length = 0 Then 'si no existe en la tabla HTCOBJS
                    filanueva = htcObjs.NewRow
                    filanueva("id") = 0
                    filanueva("Nombre") = columna.ColumnName
                    htcObjs.Rows.Add(filanueva)
                End If
            Next
        End If

        For Each fila In htcObjs.Rows 'sincronizo con todas las tablas existentes
            sql = String.Format("SELECT max(id) from {0}", fila("Nombre"))
            maximo = espacio.ManejadorBD.ejecutaEscalar(sql, tiposdatos.entero)
            fila("id") = maximo
        Next

        sql = String.Format("DELETE FROM HTCObjs")
        espacio.ManejadorBD.ejecuta(sql)

        htcObjs.TableName = "HtcObjs"
        espacio.ManejadorBD.InsertarTabla(htcObjs)
    End Sub



    Private Shared Sub SincronizarSecuencialesSQL()
        Dim tablas, htcObjs As DataTable 'almacenamos aqui las tablas de la base del usuario
        Dim nombreHTC, sql, sqlTabla As String
        Dim fila, filaHTC As DataRow
        Dim existeId As Boolean
        Dim idr As IDataReader
        Dim maximo As Long

        If espacio.tipoDeBase = tiposBase.Sql Then
            nombreHTC = "HTCObjs"
            CrearTablaHTCObjs() 'evitamos si no existe, solo para sql
            sql = String.Format("DELETE FROM {0}", nombreHTC)
            espacio.ManejadorBD.ejecuta(sql)
            sql = String.Format("SELECT * FROM {0}", nombreHTC)
            htcObjs = htcLib.espacio.ManejadorBD.traetabla(sql)
            'cargamos a todas las tablas excepto htcObjs
            sql = String.Format("SELECT * FROM Information_Schema.Tables where Table_Type = 'BASE TABLE' AND TABLE_NAME<>'{0}'", nombreHTC)
            tablas = espacio.ManejadorBD.traetabla(sql)

            For Each fila In tablas.Rows ' vemos tabla por tabla
                existeId = False
                sqlTabla = String.Format("SELECT COLUMN_NAME, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH FROM Information_Schema.Columns WHERE TABLE_NAME = '{0}' and COLUMN_NAME ='id'", fila("TABLE_NAME"))
                idr = espacio.ManejadorBD.traeReader(sqlTabla)
                If idr.Read Then existeId = True
                idr.Close()
                If existeId Then 'si la tabla tiene id entonces sacamos el maximo e ingresamos a htcobjs
                    sql = String.Format("SELECT max(id) from {0}", fila("TABLE_NAME"))
                    maximo = espacio.ManejadorBD.ejecutaEscalar(sql, tiposdatos.entero)
                    filaHTC = htcObjs.NewRow
                    filaHTC("id") = maximo
                    filaHTC("nombre") = fila("TABLE_NAME")
                    htcObjs.Rows.Add(filaHTC)
                End If
            Next
            htcObjs.TableName = "HtcObjs"
            espacio.ManejadorBD.InsertarTabla(htcObjs)
        End If
    End Sub
End Class

