
''' <summary>
''' Clase que guarda el historial de los logins.
''' Esto sirve para que el usuario no pueda repetir contraseñas.
''' </summary>
''' <remarks></remarks>

Friend Class historialLogin
    Public Shared mitabla As String = "USR_historialLogins"

    Public id As Long
    Public idUsuario As Long
    Public clave As String
    Public fechaRegistro As Date


    Public Sub New(ByVal idUSR As Long)
        Me.idUsuario = idUSR
        Me.fechaRegistro = Today
    End Sub

    ''' <summary>
    ''' Crea el objeto registro con el usuario y password enviado y la fecha actual
    ''' </summary>
    ''' <param name="pwd"></param>
    ''' <param name="idUsr"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal idUsr As Long, ByVal pwd As String)
        Me.idUsuario = idUsr
        Me.clave = pwd
        Me.fechaRegistro = Today
    End Sub

    ''' <summary>
    ''' Guarda el registro
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub guardar()
        Dim sql As String
        Dim maximo As Long
        'no se actualiza debido a que es un historial
        sql = String.Format("SELECT max(id) from {0}", mitabla)
        maximo = htcLib.espacio.ManejadorBD.ejecutaEscalar(sql, htcLib.tiposdatos.entero)
        Me.id = maximo + 1
        sql = String.Format("INSERT into {0} values({1},{2},'{3}',{4})", mitabla, Me.id, Me.idUsuario, Me.clave, htcLib.espacio.ManejadorBD.formatofecha(Me.fechaRegistro))
        htcLib.espacio.ManejadorBD.ejecuta(sql)
    End Sub

    ''' <summary>
    ''' Busca en la tabla de historial de logins si existen claves identicas anteriores desde la fecha dada, el password debe venir encriptado
    ''' </summary>
    ''' <param name="clave">password encriptado</param>
    ''' <param name="desde">desde que fecha buscar</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function existeEnHistorial(ByVal clave As String, ByVal desde As Date) As Boolean
        Dim sql As String
        Dim informacion As IDataReader
        Dim retorno As Boolean

        sql = String.Format("SELECT * FROM {0} where idUsuario={1} AND clave='{2}' AND fechaRegistro>={3}", mitabla, Me.idUsuario, clave, htcLib.espacio.ManejadorBD.formatofecha(desde))

        'podria cargar en el objeto?
        informacion = htcLib.espacio.ManejadorBD.traeReader(sql)

        If informacion.Read Then
            retorno = True
        Else
            retorno = False
        End If

        informacion.Close()

        Return retorno

    End Function

End Class
