Imports htcLib

Friend Class historialIngreso
    Public Shared mitabla As String = "USR_HistorialIngreso"
    Public idUsuario As Long
    Public ipMaquina As String
    Public fecha As Date

    Public Sub New(ByVal u As usuario)
        Me.idUsuario = u.id
        Me.ipMaquina = GetIP()
        Me.fecha = Now
    End Sub

    Private Function GetIP() As String
        Dim ip As String
        Dim context As System.Web.HttpContext
        context = System.Web.HttpContext.Current
        ip = context.Request.ServerVariables("REMOTE_ADDR")
        Return ip
    End Function

    Public Sub guardar()
        Dim sql As String

        sql = String.Format("INSERT INTO {0}(idUsuario,ipMaquina,fecha) values({1},'{2}',{3})", mitabla, Me.idUsuario, Me.ipMaquina, espacio.ManejadorBD.formatofecha(Me.fecha, True))
        espacio.ManejadorBD.ejecuta(sql)

    End Sub

End Class
