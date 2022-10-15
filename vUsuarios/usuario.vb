Imports htcLib

Public Enum estadosClave
    Caducado = 0
    Caducandose = 1 'cerca a caducarse. 4 días.
    Normal = 2
    DeboCambiar = 3 'se le exige cambiar la contraseña para continuar
End Enum

''' <summary>
''' Información y métodos de un usuario
''' </summary>
Public Class usuario
    Public Shared mitabla As String = "USR_usuarios"
    Public id As Long
    Public idPersona As Long
    Public nombreUsuario As String
    Public clave As String 'dato encriptado
    Public debeCambiarClave As Boolean

    Public nivel As Long
    Public fechaCreacion As Date
    Public fechaModificacionPWD As Date
    Public fechaValidezPWD As Date
    Public activo As Boolean


    'Para que estos datos se llenen, el qry que lee de la base debe hacer join con la tabla MIS_personas
    <htcLib.AtGuardable(htcLib.lmTipoGuardo.lmsoloLeo)> Public nombreCompleto As String
    <htcLib.AtGuardable(htcLib.lmTipoGuardo.lmsoloLeo)> Public email As String

    'dato temporal
    <htcLib.AtGuardable(htcLib.lmTipoGuardo.lmNno)> Public Razon As String ' por que no lo dejo entrar

    'clave de cifrado de datos
    <htcLib.AtGuardable(htcLib.lmTipoGuardo.lmNno)> Private llaveEncriptacion As String = "VerticeEncriptaciónUsuario"

    <htcLib.AtGuardable(htcLib.lmTipoGuardo.lmNno)> Public Property contraseña() As String 'desencriptado
        Get
            Return funciones.DecryptString(Me.clave, Me.llaveEncriptacion)
        End Get
        Set(ByVal value As String)
            Me.clave = funciones.EncryptString(value, Me.llaveEncriptacion)
        End Set
    End Property

    'dias hasta cambio de clave desde .config o por defecto 90 dias
    <htcLib.AtGuardable(htcLib.lmTipoGuardo.lmNno)> Public ReadOnly Property diasValidezClave() As Long
        Get
            Dim dias As Long
            dias = funciones.getConfiguracion("numDiasCaducidadPWD")
            Return dias
        End Get
    End Property

    ''' <summary>
    ''' meses de validez para consulta de passwords antiguos, .config o por defecto 6 meses
    ''' </summary>
    <htcLib.AtGuardable(htcLib.lmTipoGuardo.lmNno)> Public ReadOnly Property mesesNoPuedoRepetirClave() As Long
        Get
            Dim meses As Long
            meses = funciones.getConfiguracion("mesesConsultoPWDAntiguo")
            If meses = 0 Then meses = 6
            Return meses
        End Get
    End Property

    <htcLib.AtGuardable(htcLib.lmTipoGuardo.lmNno)> Public ReadOnly Property mails() As mailUsuario
        Get
            Dim correos As mailUsuario
            correos = New mailUsuario(Me)
            Return correos
        End Get
    End Property

    ''' <summary>
    ''' Proporciona el estado de la clave
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <htcLib.AtGuardable(htcLib.lmTipoGuardo.lmNno)> Public ReadOnly Property estadoPWD() As estadosClave
        Get
            Dim dias, diascaducidad As Long
            Dim estado As estadosClave

            dias = DateDiff(DateInterval.Day, Today, Me.fechaValidezPWD)
            diascaducidad = getConfiguracion("numDiasPorCaducirPWD")
            Select Case True
                Case Me.debeCambiarClave = True
                    estado = estadosClave.DeboCambiar
                Case Today.Date > Me.fechaValidezPWD
                    estado = estadosClave.Caducado
                Case dias < diascaducidad
                    estado = estadosClave.Caducandose
                Case Else
                    estado = estadosClave.Normal
            End Select

            Return estado
        End Get

    End Property

    ''' <summary>
    ''' determina si el usuario puede acceder al sistema.    
    ''' </summary>    
    ''' <returns></returns>
    ''' <remarks>Cuando debe cambiar la clave, no puede acceder al sistema, hasta que la cambie.</remarks>
    <htcLib.AtGuardable(htcLib.lmTipoGuardo.lmNno)> Public ReadOnly Property permitirIngreso() As Boolean
        Get
            Dim valido As Boolean

            'If Me.activo = False Then
            '    valido = False
            '    razon = "Usuario no activo"
            'Else
            Select Case Me.estadoPWD
                Case estadosClave.Caducado ', estadosClave.DeboCambiar 'no cuando debeb cambiar porque debeb ingresar pero al cambio
                    valido = False
                    Razon = "contraseña caducada"
                Case Else
                    valido = True
            End Select
            'End If

            Return valido
        End Get

    End Property

    '/***************************************************************************************************/

    Public Sub New()
    End Sub

    Public Sub New(ByVal correo As String)
        Dim sql As String

        sql = String.Format("USR_buscaUsuarioMail '{0}'", correo)
        LM.cargaObjeto(sql, Me)
    End Sub
    ''' <summary>
    ''' Crea el objeto usuario en caso de existir un usuario en la base con ese username y password
    ''' </summary>
    ''' <param name="nombreUsr">nombre de usuario</param>
    ''' <param name="pwd">password sin encriptar</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal nombreUsr As String, ByVal pwd As String)
        Dim pwdEncriptado As String
        Dim sql As String

        pwdEncriptado = funciones.EncryptString(pwd, Me.llaveEncriptacion)
        sql = String.Format("USR_buscaUsuario '{0}','{1}'", nombreUsr, pwdEncriptado)

        htcLib.LM.cargaObjeto(sql, Me)

    End Sub

    ''' <summary>
    ''' Crea el objeto usuario por el id del usuario 
    ''' </summary>
    ''' <param name="id">id de Usuario</param>
    ''' <remarks>se usa cuando tengo el id, ejemplo desde un token, o desde la administración</remarks>
    Public Sub New(ByVal id As Long)
        Me.id = id
        Me.cargacompleto()
    End Sub

    ''' <summary>
    ''' Carga toda la información del usuario a partir del ID. 
    ''' Esto es util porque a veces el usuario esta cargado desde un cookie y no tiene tod la info.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub cargacompleto()
        Dim sql As String

        sql = String.Format("USR_BuscaUsuarioId {0}", Me.id)
        htcLib.LM.cargaObjeto(sql, Me)

    End Sub

    ''' <summary>
    ''' Elimina al usuario de la base
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub eliminar()
        Dim sql As String = String.Format("DELETE from {0} where id = {1}", mitabla, Me.id)
        htcLib.espacio.ManejadorBD.ejecuta(sql)
    End Sub

    ''' <summary>
    ''' Carga información del usuario en el objeto, a parftir del string proveniente en el token
    ''' No se carga usuario y password
    ''' </summary>
    ''' <param name="tokenTemporal">cadena almacenada como cookie temporal del browser</param>
    ''' <remarks>la cadena esta encriptada y contiene información del usuario</remarks>
    Public Overridable Function cargaUsuarioDesdeTokenTemporal(ByVal tokenTemporal As String) As Boolean
        Dim stringToken() As String
        Dim fechas() As String
        Dim retorno As Boolean

        If Not tokenTemporal Is Nothing AndAlso Not tokenTemporal.CompareTo("") = 0 Then
            tokenTemporal = funciones.DecryptString(tokenTemporal, Me.llaveEncriptacion)
            stringToken = tokenTemporal.Split("%")
            fechas = stringToken(3).Split("|")
            With Me
                .nivel = CLng(stringToken(2)) / 7
                .id = (CLng(stringToken(1)) - Me.nivel) / 5
                .nombreCompleto = stringToken(0)
                .fechaValidezPWD = New Date(fechas(0), fechas(1), fechas(2))
                .debeCambiarClave = CBool(stringToken(4))
                .idPersona = stringToken(5)
            End With
            retorno = True
        Else
            retorno = False
        End If

        Return retorno
    End Function

    ''' <summary>
    ''' Crea el string token para ser enviado como cookie hacia el browser
    ''' </summary>
    ''' <returns>el token encriptado</returns>
    ''' <remarks></remarks>
    Public Overridable Function getTokenTemporal() As String
        Dim cadena As String
        Dim token As String
        Dim id As Long, nivel As Long
        Dim nombreUsuario, debeCambiar As String

        'El token lleva la información del usuario que luego verifico para darle acceso
        nivel = Me.nivel * 7
        id = (Me.id * 5) + Me.nivel
        nombreUsuario = Me.nombreCompleto
        debeCambiar = CLng(Me.debeCambiarClave)
        cadena = String.Format("{0}%{1}%{2}%{3}|{4}|{5}%{6}%{7}", Me.nombreUsuario, id, nivel, Me.fechaValidezPWD.Year, Me.fechaValidezPWD.Month, Me.fechaValidezPWD.Day, debeCambiar, Me.idPersona)

        token = funciones.EncryptString(cadena, Me.llaveEncriptacion)

        Return token

    End Function


    ''' <summary>
    ''' Guarda el objeto usuario 
    ''' </summary>
    Public Sub guardar()
        Dim sql As String
        Dim maximo As Long

        If Me.id = 0 Then
            sql = String.Format("SELECT max(id) from {0}", mitabla)
            maximo = htcLib.espacio.ManejadorBD.ejecutaEscalar(sql, htcLib.tiposdatos.entero)
            Me.id = maximo + 1
            sql = String.Format("INSERT into {0}(id,idPersona,nombreUsuario,clave,nivel,fechaCreacion,fechaModificacionPWD,fechaValidezPWD,activo,debeCambiarClave) values({1},{2},'{3}','{4}',{5},{6},{7},{8},{9},{10})", mitabla, Me.id, Me.idPersona, Me.nombreUsuario, Me.clave, Me.nivel, htcLib.espacio.ManejadorBD.formatofecha(Me.fechaCreacion), htcLib.espacio.ManejadorBD.formatofecha(Me.fechaModificacionPWD), htcLib.espacio.ManejadorBD.formatofecha(Me.fechaValidezPWD), funciones.CBoolData(Me.activo), funciones.CBoolData(Me.debeCambiarClave))
            htcLib.espacio.ManejadorBD.ejecuta(sql)
        Else
            sql = String.Format("UPDATE {0} set idPersona={1}, nombreUsuario='{2}', clave='{3}', nivel={4}, fechaCreacion={5}, fechaModificacionPWD={6}, fechaValidezPWD={7}, activo={8}, debeCambiarClave={9} WHERE id={10}", mitabla, Me.idPersona, Me.nombreUsuario, Me.clave, Me.nivel, htcLib.espacio.ManejadorBD.formatofecha(Me.fechaCreacion), htcLib.espacio.ManejadorBD.formatofecha(Me.fechaModificacionPWD), htcLib.espacio.ManejadorBD.formatofecha(Me.fechaValidezPWD), funciones.CBoolData(Me.activo), funciones.CBoolData(Me.debeCambiarClave), Me.id)
            htcLib.espacio.ManejadorBD.ejecuta(sql)
        End If

    End Sub

    ''' <summary>
    ''' Determina si el usuario tiene permiso al nivel enviado
    ''' </summary>
    ''' <param name="nivelIn"></param>
    ''' <returns>devuelve si en su nivel, esta incluido el nivel enviado.</returns>
    ''' <remarks>trabaja con comparaciones bit a bit</remarks>
    Public Function tengoPermiso(ByVal nivelIn As Long) As Boolean
        Dim temp As Long
        Dim retorno As Boolean

        If Me.permitirIngreso Then

            temp = Me.nivel And nivelIn
            If temp = nivelIn Then
                retorno = True
            Else
                retorno = False
            End If

        Else
            retorno = False
        End If

        Return retorno

    End Function

    ''' <summary>
    ''' Extiende la validez de la clave (porque esta expirada), para dejar entrar al usuario, pero le exige cambiarla la próxima vez.
    ''' Se extiende un plazo largo, pero no puede ingresar al sistema hasta que cambie.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ExtiendeValidezClave()
        Me.fechaValidezPWD = Today.AddDays(Me.diasValidezClave)
        Me.debeCambiarClave = True

        Me.guardar()
    End Sub


    ''' <summary>
    ''' Cambia de nivel de acceso, aumentando el que se envía. Ej. si el usuario tiene 2 y le mando 4, dse hace 6. 
    ''' Si el usuario es 2 y le mando 2, se queda en 2.
    ''' </summary>
    ''' <param name="nuevoNivel">Permiso adicional que se asigna al usuario</param>
    Public Sub aumentaPermiso(ByVal nuevoNivel As Long)
        If Not (Me.nivel And nuevoNivel) = nuevoNivel Then
            Me.nivel += nuevoNivel
            Me.guardar()
        End If
    End Sub


    ''' <summary>
    ''' Guarda un registro de los cambios de las claves, con fecha
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub registraHistorialClave()
        If funciones.getConfiguracion("verificoClavesViejas") Then
            Dim r As New historialLogin(Me.id, Me.clave)
            r.guardar()
        End If
    End Sub

    ''' <summary>
    ''' Retorna true si la clave ya ha existido anteriormente en el plazo establecido
    ''' </summary>}
    Private Function verificaClaveRepetida(ByVal claveNueva As String) As Boolean
        Dim claveCodificada As String
        Dim retorno As Boolean
        Dim historial As historialLogin

        claveCodificada = funciones.EncryptString(claveNueva, Me.llaveEncriptacion)

        historial = New historialLogin(Me.id)
        retorno = historial.existeEnHistorial(claveCodificada, Today.AddMonths(-Me.mesesNoPuedoRepetirClave))

        Return retorno

    End Function

    ''' <summary>
    ''' Retorna true cuando ya existe ese usuario
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function VerificaNombreUsuarioRepetido(ByVal nombreUSR As String) As Boolean
        Dim sql As String
        Dim id As IDataReader
        Dim retorno As Boolean

        sql = String.Format("SELECT * FROM {0} WHERE nombreUsuario='{1}' ", mitabla, nombreUSR)
        id = htcLib.espacio.ManejadorBD.traeReader(sql)
        If id.Read Then retorno = True
        id.Close()

        Return retorno
    End Function

    ''' <summary>
    ''' Toda la logica para cambiar la contraseña
    ''' </summary>
    ''' <param name="nuevoPWD"></param>
    ''' <param name="nuevoPWDReingreso"></param>
    ''' <param name="pwdAnterior"></param>
    ''' <returns>verdadero si lo logro ,falso si no hizo el cambio</returns>
    ''' <remarks>Controla validez, datos correctos y registra el cambio</remarks>
    Public Function cambiaClave(ByVal nuevoPWD As String, ByVal nuevoPWDReingreso As String, ByVal pwdAnterior As String) As Boolean
        Dim retorno As Boolean
        Dim claveAnterior As String
        Dim deboVerificarClavesviejas As Boolean

        claveAnterior = funciones.EncryptString(pwdAnterior, Me.llaveEncriptacion)
        deboVerificarClavesviejas = funciones.getConfiguracion("verificoClavesViejas")
        Me.cargacompleto()

        'verifico que la info ingresada es correcta
        If nuevoPWD = nuevoPWDReingreso And claveAnterior = Me.clave And nuevoPWD.Length > 5 Then
            'ahora verifico que pueda usar esa clave, porque no la he usado o porquue este sistema no valida eso
            If deboVerificarClavesviejas = False Or Me.verificaClaveRepetida(nuevoPWD) = False Then
                Me.contraseña = nuevoPWD
                If diasValidezClave > 0 Then
                    Me.fechaValidezPWD = Today.AddDays(diasValidezClave)
                End If
                Me.debeCambiarClave = False
                Me.fechaModificacionPWD = Today
                Me.guardar()
                Me.registraHistorialClave()
                web.EnviaCookieConTokenTemporal(Me)
                retorno = True
            Else
                retorno = False
            End If
        Else
            retorno = False
        End If

        Return retorno

    End Function

    ''' <summary>
    ''' Cambia la pwd del usuario a una establecida, y lo obliga a cambiar en seguida.
    ''' </summary>
    ''' <param name="nuevoPwd"></param>
    ''' <remarks></remarks>
    Private Sub AsignaClaveTemporal(ByVal nuevoPwd As String)
        If Me.id <> 0 Then
            Me.contraseña = nuevoPwd
            Me.fechaModificacionPWD = Today
            If Not (Me.diasValidezClave = 0) Then
                Me.fechaValidezPWD = Today.AddDays(Me.diasValidezClave)
            End If
            Me.debeCambiarClave = True
            Me.guardar()
        End If

    End Sub


#Region "shared's"

    ''' <summary>
    ''' Coloca el nombre de usuario como la contraseña
    ''' </summary>
    ''' <param name="idUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function asignaClavePorDefecto(ByVal idUsuario As Long) As Boolean
        Dim usr As usuario

        usr = New usuario(idUsuario)
        usr.AsignaClaveTemporal(usr.nombreUsuario)

    End Function

    ''' <summary>
    ''' Crea una clave temporal y la asigna al usuario cuyo ID es enviado
    ''' </summary>
    ''' <param name="idUsuario">el id del usuario al que se cambiará la clave</param>
    ''' <returns>La nueva clave asignada</returns>
    ''' <remarks></remarks>
    Public Shared Function asignaClaveTemporal(ByVal idUsuario As Long) As String
        Dim temporal As Long
        Dim tmpPwd As String
        Dim usr As usuario

        VBMath.Randomize()
        temporal = Rnd() * 1000
        tmpPwd = temporal

        usr = New usuario(idUsuario)
        usr.AsignaClaveTemporal(tmpPwd)

        Return tmpPwd
    End Function


    ''' <summary>
    ''' Asigna una clave temporal la cual es solo enviada por mail
    ''' </summary>
    ''' <param name="idUsuario"></param>
    ''' <param name="dominio"></param>
    ''' <remarks></remarks>
    Public Shared Sub asignaClaveTemporalyMandaPorMail(ByVal idUsuario As Long, ByVal dominio As String, ByVal de As String)
        Dim nuevaClave As String
        Dim usr As usuario

        nuevaClave = AsignaClaveTemporal(idUsuario)
        usr = New usuario(idUsuario)
        usr.mails.enviaClaveUsuario(dominio, de)

    End Sub

#End Region
End Class
