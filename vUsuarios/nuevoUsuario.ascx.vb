Public Partial Class nuevoUsuario
    Inherits System.Web.UI.UserControl
    Public midominio As String

    Public Event usuarioModificado()
    Public Event cancelado()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.lblError.Visible = False
        If Not IsPostBack Then
            Me.refrescar()
            Me.TBPersona.Visible = False
            Me.TBClave.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Carga solo el datagrid view de personas
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub refrescar()
        Me.gdvPersonas.DataSource = funciones.getPersonasUsuarios
        Me.gdvPersonas.DataBind()
    End Sub


    ''' <summary>
    ''' Realiza la paginacion en el datagrid view
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gdvPersonas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvPersonas.PageIndexChanging
        Me.gdvPersonas.PageIndex = 0
        Me.gdvPersonas.PageIndex = e.NewPageIndex
        Me.gdvPersonas.DataBind()
        Me.refrescar() 'llama a cargar el datagrid nuevamente
    End Sub

    ''' <summary>
    ''' Crea una nueva persona en la tabla Mis personas y por lo tanto creara un nuevo usuario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbtnNuevoUsuario_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnNuevoUsuario.Click
        Me.muestraPersonaUsuario(0) 'nuevo usuario
    End Sub

    ''' <summary>
    ''' Llama a mostrar a la persona seleccionada dependiendo de el link seleccionado en el datagrid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lnkPersona_Click1(ByVal sender As Object, ByVal e As EventArgs)
        Me.muestraPersonaUsuario(CLng(sender.commandargument))
    End Sub

    ''' <summary>
    ''' Muestra a la persona obtenida de la base o nueva
    ''' </summary>
    ''' <param name="idPersona"></param>
    ''' <remarks></remarks>
    Private Sub muestraPersonaUsuario(ByVal idPersona As Long)
        Dim p As persona

        p = New persona(idPersona)

        Me.TBPersonas.Visible = False
        Me.TBPersona.Visible = True

        With p
            Me.txtNombre1.Text = .nombre1
            Me.txtApellido1.Text = .apellido1
            Me.txtCorreo.Text = .email
        End With

        With p.miUsuario
            funciones.llenaTree(Me.trvPermisos, funciones.getPermisos, p.miUsuario) 'cargamos los permisos con los niveles del usuario
            Me.txtNombreUsuario.Text = .nombreUsuario
            Select Case .activo
                Case True
                    Me.rbtlEstado.Items(0).Selected = True
                Case False
                    Me.rbtlEstado.Items(1).Selected = True
            End Select

        End With

        If p.id = 0 Then
            Me.btnEliminar.Visible = False ' no mostramos eliminar ya que no existe
            Me.chkAsignarClave.Checked = True
            Me.chkAsignarClave.Enabled = False
        End If

        Me.btnGuardarUsuario.CommandArgument = p.id

    End Sub



    ''' <summary>
    ''' Guarda el usuario y le asigna una nueva clave que debe ser cambiada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardarUsuario_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardarUsuario.Click
        Dim persona As persona
        Dim idPersona As Long
        Dim codigo As String
        Dim permitidoGuardar As Boolean

        If Me.txtNombre1.Text = "" Or Me.txtApellido1.Text = "" Or Me.txtCorreo.Text = "" Or Me.txtNombreUsuario.Text = "" Then GoTo noPasa

        idPersona = sender.commandargument
        persona = New persona(idPersona)

        With persona
            .nombre1 = Me.txtNombre1.Text
            .apellido1 = Me.txtApellido1.Text
            .email = Me.txtCorreo.Text
        End With

        With persona.miUsuario
            .nombreUsuario = Me.txtNombreUsuario.Text
            .nivel = 0
            For Each nivel As htcLib.idNombre In funciones.getTree(Me.trvPermisos)
                .aumentaPermiso(nivel.id)
            Next
            If .fechaCreacion = Nothing Then .fechaCreacion = Today
            .activo = Me.rbtlEstado.Items(0).Selected

            If persona.miUsuario.id = 0 Then 'es nuevo
                permitidoGuardar = Not usuario.VerificaNombreUsuarioRepetido(.nombreUsuario)
            Else
                permitidoGuardar = True
            End If


        End With

        If permitidoGuardar Then
            persona.guardar()
            persona.miUsuario.guardar()

            If Me.chkAsignarClave.Checked Then 'asignar clave temporal
                codigo = usuario.AsignaClaveTemporal(persona.miUsuario.id)
                Me.lblClaveAsignada.Text = codigo

                Me.TBPersona.Visible = False
                Me.TBClave.Visible = True
            Else
                RaiseEvent usuarioModificado()
            End If
        Else

noPasa:     Me.lblError.Visible = True
        End If


    End Sub


    ''' <summary>
    ''' Llama al evento moidificado una vez que se ha cambiado a el usuario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        RaiseEvent usuarioModificado()
    End Sub

    ''' <summary>
    ''' Ejecuta el evento cancelado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        RaiseEvent cancelado()
    End Sub

    ''' <summary>
    ''' Elimina a el usuario y a la persona que se muestra
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnEliminar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEliminar.Click
        Dim persona As persona
        Dim idPersona As Long

        idPersona = Me.btnGuardarUsuario.CommandArgument
        If idPersona <> 0 Then

            persona = New persona(idPersona)
            persona.miUsuario.eliminar()
            persona.eliminar()

            Me.TBPersona.Visible = False ' por si acaso ocultamos a todos
            Me.TBClave.Visible = False

            RaiseEvent usuarioModificado()
        End If
    End Sub


    Protected Sub gdvPersonas_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gdvPersonas.SelectedIndexChanged

    End Sub
End Class