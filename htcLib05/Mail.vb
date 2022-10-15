Public Class Mail
    Public De As String = System.Configuration.ConfigurationSettings.AppSettings("FROM")
    Public nombreDe As String
    Public Subject As String
    Public Smtp As String = System.Configuration.ConfigurationSettings.AppSettings("SMTP")
    Public ReplyTo As String
    Public Body As String
    Public Bcc As String
    Public destinatariosCorreo As String
    Public IsBodyHtml As Boolean = False

    Public Sub New()

    End Sub

    Public Sub New(ByVal CuerpoMail As String, ByVal TituloMail As String, Optional ByVal replyTo As String = Nothing, Optional ByVal CopiaA As String = Nothing, Optional ByVal nombreQuienEnvia As String = Nothing)
        Me.Subject = TituloMail
        Me.ReplyTo = replyTo
        Me.Body = CuerpoMail
        Me.Bcc = CopiaA
        Me.nombreDe = nombreQuienEnvia
    End Sub

  
    ''' <summary>
    ''' Envia el email 
    ''' </summary>
    ''' <param name="destinatario"></param>
    ''' <returns>verdadero si se envia el mail</returns>
    ''' <remarks></remarks>
    Public Function EnviarMail(ByVal destinatario As String) As Boolean
        Dim m As New System.Net.Mail.MailMessage
        Dim enviado As Boolean
        Dim smtp As New System.Net.Mail.SmtpClient
        Me.destinatariosCorreo = destinatario
        Try

            m.Body = Me.Body
            m.From = New System.Net.Mail.MailAddress(Me.De, Me.nombreDe)
            m.Subject = Me.Subject
            m.IsBodyHtml = Me.IsBodyHtml
            m.Priority = System.Net.Mail.MailPriority.High

            If Me.ReplyTo <> Nothing Then m.ReplyTo = New System.Net.Mail.MailAddress(Me.ReplyTo)
            m.To.Add(Me.destinatariosCorreo)

            If Not Me.Bcc Is Nothing Then m.Bcc.Add(Me.Bcc)

            smtp.Host = Me.Smtp
            'cometado por prueba
            smtp.Send(m)

            enviado = True
        Catch ex As Exception
            enviado = False
        Finally
            m = Nothing
        End Try

        Return enviado
    End Function

    ''' <summary>
    ''' Funcion que se puede utilizar en hilos para enviar mail
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EnviarMail() As Boolean
        Return Me.EnviarMail(Me.destinatariosCorreo)
    End Function

End Class
