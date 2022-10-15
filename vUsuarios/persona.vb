Friend Class persona

    Public Shared miTabla As String = "MIS_personas"
    Public id As Integer
    Public nombre1 As String
    Public apellido1 As String
    Public email As String
    <htcLib.AtGuardable(htcLib.lmTipoGuardo.lmNno)> Public miUsuario As usuario
    Sub New()
    End Sub

    Sub New(ByVal id As Integer)
        Dim sql As String = String.Format("select * from {0} where id= {1}", miTabla, id)
        htcLib.LM.cargaObjeto(sql, Me)
        'cargamos de una vez al usuario que le pertenece a esta persona
        Me.CargaMiUsuario()
    End Sub

    Public Sub guardar()

        htcLib.LM.GuardaObjeto(Me)

        Me.miUsuario.idPersona = Me.id
    End Sub

    Public Sub eliminar()
        Dim sql As String = String.Format("delete from {0} where id = {1}", miTabla, Me.id)
        htcLib.espacio.ManejadorBD.ejecuta(sql)
    End Sub

    Private Sub CargaMiUsuario()
        Dim sql As String

        Me.miUsuario = New usuario
        Sql = String.Format("USR_buscaUsuarioIdPersona {0}", Me.id)
        htcLib.LM.cargaObjeto(Sql, Me.miUsuario)
    End Sub

End Class
