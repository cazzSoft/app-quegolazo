Public Enum Language
    Englis = 1
    Spanish = 2
End Enum

Public Class mailUsuario
    Private miusuario As usuario
    Private email As String
    Public idioma As Language = Language.Spanish
    Public smtp As String
    Public falla As String

    Public Shared ClaveMail As String = "%LoQueÉlMaIlséLlevo%"

    Public Sub New(ByVal usr As usuario)
        Me.miusuario = usr
        Me.email = usr.email
        smtp = funciones.getConfiguracion("SMTP") '"localhost"
        If smtp = "" Then smtp = "localhost"
    End Sub

    ''' <summary>
    ''' Envia un mail al registrado para confirmación 
    ''' </summary>
    ''' <param name="url"></param>
    ''' <param name="redir"></param>
    ''' <param name="NombreCarpeta"></param>
    ''' <remarks></remarks>
    Public Sub enviaMailUsuarioParaConfirmacion(ByVal url As String, Optional ByVal redir As String = "", Optional ByVal NombreCarpeta As String = "", Optional ByVal Bcc As String = Nothing)
        Dim cuerpo As New System.Text.StringBuilder
        Dim tema, de, stringUrl, separacion As String
        Dim smtp As System.Net.Mail.SmtpClient
        Dim mensaje As System.Net.Mail.MailMessage

        tema = "Confirme su registro"
        de = "web@" & url.Substring(11)
        separacion = "<br />"
        stringUrl = String.Format("{0}|{1}", Me.miusuario.id, redir)
        stringUrl = funciones.EncryptString(stringUrl, ClaveMail)
        stringUrl = String.Format("{0}/{1}validaNuevoUsuario.aspx?ss={2}", url, NombreCarpeta, stringUrl)

        If Not NombreCarpeta = "" Then NombreCarpeta &= "/"

        cuerpo.AppendFormat("Se ha creado su cuenta en {0} con los siguientes datos:{1}", url, separacion)
        cuerpo.AppendFormat("Usuario: {0}{1}", Me.miusuario.nombreUsuario, separacion)
        cuerpo.AppendFormat("Contraseña: {0}{1}", Me.miusuario.contraseña, separacion)
        cuerpo.AppendFormat("Para confirmar su cuenta, haga clic en el siguiente link: {0}", separacion)
        cuerpo.AppendFormat("<a href='{0}'>", stringUrl)
        cuerpo.Append(stringUrl)
        cuerpo.Append("</a>")

        smtp = New System.Net.Mail.SmtpClient
        mensaje = New System.Net.Mail.MailMessage
        Try
            mensaje.Body = cuerpo.ToString
            mensaje.IsBodyHtml = True
            mensaje.From = New System.Net.Mail.MailAddress(de)
            mensaje.Subject = tema
            mensaje.To.Add(Me.email)
            If Not Bcc Is Nothing Then mensaje.Bcc.Add(Bcc)
            'mensaje.Bcc.Add("iviteri@gmail.com")
            smtp.Host = Me.smtp
            smtp.Send(mensaje)

        Catch ex As Exception
            Throw ex
            falla = ex.Message
        Finally
            mensaje = Nothing
        End Try

    End Sub



    Public Function enviaClaveUsuario(ByVal dominio As String, ByVal de As String, Optional ByVal Bcc As String = Nothing) As Boolean
        Dim cuerpo As New System.Text.StringBuilder
        Dim tema, espacio As String
        Dim smtp As System.Net.Mail.SmtpClient
        Dim mensaje As System.Net.Mail.MailMessage

        espacio = vbCrLf
        Select Case Me.idioma
            Case Language.Spanish
                tema = "Su contraseña"
                cuerpo.AppendFormat("Su contraseña registrada en {0} es:", dominio, espacio)
            Case Language.Englis
                tema = "Your password"
                cuerpo.AppendFormat("Your password registered in {0} is:", dominio, espacio)
        End Select

        cuerpo.AppendFormat("{0}", Me.miusuario.contraseña)

        smtp = New System.Net.Mail.SmtpClient
        mensaje = New System.Net.Mail.MailMessage
        Try
            mensaje.Body = cuerpo.ToString
            mensaje.From = New System.Net.Mail.MailAddress(de)
            mensaje.Subject = tema
            mensaje.To.Add(Me.email)
            If Not Bcc Is Nothing Then mensaje.Bcc.Add(Bcc)
            smtp.Host = Me.smtp
            smtp.Send(mensaje)

        Catch ex As Exception
            Throw ex
            falla = ex.Message
        Finally
            mensaje = Nothing
        End Try

    End Function


End Class
