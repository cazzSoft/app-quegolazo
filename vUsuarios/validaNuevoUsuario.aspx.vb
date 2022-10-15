Imports htcLib
Partial Public Class validaNuevoUsuario
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            espacio.inicializa(ConfigurationManager.AppSettings("CON"), tiposConexion.Web)
            Me.ValidarNuevoUsuario()
        Catch ex As Exception
            Me.Response.Write(ex.ToString)
        Finally
            espacio.cerrar()
        End Try
    End Sub

    Private Sub validaNuevoCliente_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        espacio.cerrar()
    End Sub

    Private Sub ValidarNuevoUsuario()
        Dim cadenaRecibida, cadenas() As String
        Dim usr As usuario

        cadenaRecibida = Me.Request("ss")
        cadenaRecibida = funciones.DecryptString(cadenaRecibida, mailUsuario.ClaveMail)
        cadenas = cadenaRecibida.Split("|")
        usr = New usuario(CLng(cadenas(0)))
        usr.activo = True
        usr.guardar()

        If usr.permitirIngreso Then web.EnviaCookieConTokenTemporal(usr)


        espacio.cerrar()
        Me.Response.Redirect(cadenas(1))
    End Sub


End Class