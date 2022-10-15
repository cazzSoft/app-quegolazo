''' <summary>
''' Clase para crear cadenas que sirven para dar accceso al sistema a un usuario
''' Esta cadena se envia al usuario (cookie o un email) y se valida en la base de datos
''' </summary>
''' <remarks></remarks>
Friend Class token
    Public Shared mitabla As String = "USR_tokens"

    Public token As String
    Public idUsuario As Long
    Public fechaExpiracion As Date
    Public sirveUnaVez As Boolean
    ''' <summary>
    ''' Determina si el token es válido o no. Si es valido, pero solo sirve una vez, lo elimino.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <htcLib.AtGuardable(htcLib.lmTipoGuardo.lmNno)> Public ReadOnly Property Valido() As Boolean
        Get
            If Me.idUsuario > 0 And token <> "" Then
                If Me.sirveUnaVez = True Then Me.eliminar()

                Return True

            Else
                Return False
            End If
        End Get
    End Property

    ''' <summary>
    ''' Busca el token en la base de datos, siempre que esté valido.
    ''' </summary>
    ''' <param name="token"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal token As String)

        Dim sql As String
        sql = String.Format("SELECT * FROM {0} WHERE token='{1}'", mitabla, token)
        htcLib.LM.cargaObjeto(sql, Me)

        If (Not Me.fechaExpiracion = Nothing) And (Not Me.fechaExpiracion >= Today) Then
            Me.idUsuario = 0

            ' si no vale, de una vez lo elimino
            Me.eliminar()
        End If

    End Sub




    ''' <summary>
    ''' Crea un token de acceso para el idUsuario enviado, y lo guarda en la base.
    ''' </summary>
    ''' <param name="idUsr"></param>
    ''' <param name="hasta">hasta cuando vale este token</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal idUsr As Long, ByVal hasta As Date, ByVal unSoloUso As Boolean)
        Dim num As Long

        'creo un numero temporal y con este una cadena
        VBMath.Randomize()
        num = Rnd() * 100000

        Me.token = num.GetHashCode
        Me.idUsuario = idUsr
        Me.fechaExpiracion = hasta
        Me.sirveUnaVez = unSoloUso

        Me.guardar()

    End Sub

    ''' <summary>
    ''' Guarda el token en la tabla de tokens
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub guardar()
        Dim sql As String
        Dim verdad As Long
        Select Case Me.sirveUnaVez
            Case True
                verdad = 1
            Case False
                verdad = 0
        End Select
        sql = String.Format("INSERT INTO {0} VALUES({1},{2},{3},{4})", mitabla, Me.token, Me.idUsuario, htcLib.espacio.ManejadorBD.formatofecha(Me.fechaExpiracion), verdad)
        htcLib.espacio.ManejadorBD.ejecuta(sql)
    End Sub

    ''' <summary>
    ''' Elimina el token
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub eliminar()
        Dim sql As String
        sql = String.Format("DELETE FROM {0} WHERE token='{1}' and idUsuario={2}", mitabla, Me.token, Me.idUsuario)
        htcLib.espacio.ManejadorBD.ejecuta(sql)
    End Sub

End Class
