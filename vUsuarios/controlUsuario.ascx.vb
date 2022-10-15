Partial Public Class controlUsuario
    Inherits System.Web.UI.UserControl

    Public Event UsuarioValidado()
    Public Event masTarde()
    Public Event UsuarioNoActivo()

    Public miusuario As usuario
    Public soloCambiaPassword As Boolean
    Private utilizarRecordado As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Cargar()
        If Not IsPostBack Then
            Me.lblErrorValidar.Visible = False
            Me.lblErrorCambio.Visible = False
            Me.lblLogueado.Visible = False
            Me.TBLmostrarCambioPWD.Visible = False
            If Me.soloCambiaPassword Then Me.mostrarCambioPWD(True)
            If Me.utilizarRecordado Then Me.CargarRecordado()
        End If

    End Sub

    Private Sub Cargar()
        If Not CBool(funciones.getConfiguracion("mostrarRecordar")) Then
            Me.chkRecordar.Visible = False
            Me.chkRecordar.Checked = False
            Me.utilizarRecordado = False
        Else
            Me.utilizarRecordado = True
        End If
    End Sub

    Private Sub CargarRecordado()
        Dim u As usuario

        u = vUsuarios.web.TraeUsuarioRegistrado()
        If u.id <> 0 Then
            Me.txtNombreUsuario.Text = u.nombreUsuario
            Me.txtPassword.Text = u.contraseña
            Me.txtPassword.Attributes.Add("value", u.contraseña)
            Me.chkRecordar.Checked = True
        End If
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Dim intentos As Long

        intentos = funciones.getConfiguracion("numeroIntentos")
        Select Case intentos
            Case Is <= 0
                Me.validar()
            Case Is > 0
                If Not Me.validar() Then
                    If CLng(Me.Session(vUsuarios.web.CONSesionIntentos)) >= intentos Then 'por el 0 del sesion
                        Me.btnAceptar.Enabled = False
                    End If
                End If
        End Select

    End Sub

    Protected Sub btnCambiar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCambiar.Click
        Me.cambiarPWD()
    End Sub

    Private Function validar() As Boolean
        Dim nombreUsuario As String
        Dim password As String
        Dim usr As usuario
        Dim recuerda As Boolean
        Dim echo, retorno As Boolean

        nombreUsuario = Me.txtNombreUsuario.Text
        password = Me.txtPassword.Text
        usr = New usuario
        recuerda = Me.chkRecordar.Checked
        web.validaYRegistraenBrowser(usr, nombreUsuario, password, recuerda)

        Me.lblErrorValidar.Visible = False
        Me.lblUsuarioDesactivado.Visible = False
        Me.lblPasswordExpiro.Visible = False

        If usr.activo = False Or usr.estadoPWD = estadosClave.Caducado Then
            If usr.id = 0 Then
                Me.lblErrorValidar.Visible = True
                echo = True
            End If
            If (Not echo) And (usr.id > 0) And (Not usr.activo) Then
                Me.lblUsuarioDesactivado.Visible = True
                echo = True
                RaiseEvent UsuarioNoActivo()
            End If
            If (Not echo) And usr.estadoPWD = estadosClave.Caducado Then Me.lblPasswordExpiro.Visible = True
        Else
            Me.miusuario = usr
            Select Case usr.estadoPWD
                Case estadosClave.Caducandose

                    Me.mostrarCambioPWD(True)
                Case estadosClave.DeboCambiar

                    Me.mostrarCambioPWD(Not usr.debeCambiarClave)
                Case estadosClave.Normal

                    Me.lblLogueado.Text = String.Format("{0} {1}", Me.miusuario.nombreCompleto, Me.lblLogueado.Text)
                    Me.lblLogueado.Visible = True
                    RaiseEvent UsuarioValidado()
            End Select
            retorno = True
        End If

        Return retorno
    End Function

    Private Sub cambiarPWD()
        Dim usr As usuario
        Dim pwdActual, pwdNuevo, pwdReingreso As String

        pwdActual = Me.txtPasswordActual.Text
        pwdNuevo = Me.txtPasswordNuevo.Text
        pwdReingreso = Me.txtPasswordNuevoReingreso.Text
        usr = web.TraeUsuarioRegistrado
        If usr.cambiaClave(pwdNuevo, pwdReingreso, pwdActual) Then
            Me.miusuario = usr
            RaiseEvent UsuarioValidado()
        Else
            Select Case usr.estadoPWD
                Case estadosClave.Caducado, estadosClave.DeboCambiar
                    mostrarCambioPWD(False)
                Case Else
                    mostrarCambioPWD(True)
            End Select
            Me.lblErrorCambio.Visible = True
        End If

    End Sub

    Private Sub mostrarLogin()
        Me.TBLmostrarCambioPWD.Visible = False
        Me.TBLmostrarLogin.Visible = True
    End Sub

    Private Sub mostrarCambioPWD(ByVal dejoPasar As Boolean)
        Dim usr As usuario
        Dim fecha As Date
        usr = web.TraeUsuarioRegistrado
        'Single ENTRE POR ACA, RAISEVENT CAMBIO PWD
        fecha = usr.fechaValidezPWD
        Me.TBLmostrarLogin.Visible = False
        Me.TBLmostrarCambioPWD.Visible = True
        Me.txtPasswordActual.Text = ""
        Me.txtPasswordNuevo.Text = ""
        Me.txtPasswordNuevoReingreso.Text = ""
        Me.lblExpiraFecha.Text = String.Format("{0:dd/MMM/yyyy}", usr.fechaValidezPWD)
        If Not dejoPasar Then
            Me.lbtnCambiarTarde.Visible = False
        End If

    End Sub


    Protected Sub lbtnCambiarTarde_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnCambiarTarde.Click
        Me.miusuario = web.TraeUsuarioRegistrado
        RaiseEvent masTarde()
    End Sub
End Class