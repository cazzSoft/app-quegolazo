Public Class emailDesdePagina

    Public html As String 'contiene el html de la pagina web
    Public url As String 'contiene el url de la pagina web
    Public ok As Boolean 'para ver si fue satisfactoria la consulta

    Public Sub New(ByVal web As String)
        Me.url = web
        Me.ok = obtenerHTML()
    End Sub

    Private Function obtenerHTML() As Boolean
        Dim Ok As Boolean = True
        Dim Wresquest As System.Net.HttpWebRequest
        Dim Wresponse As System.Net.HttpWebResponse
        Dim Strm As System.IO.Stream
        Dim texto As String = Nothing

        Try
            Wresquest = System.Net.WebRequest.Create(Me.url)
            Wresponse = Wresquest.GetResponse
            Strm = Wresponse.GetResponseStream()

            Dim readStream As New System.IO.StreamReader(Strm)
            texto = readStream.ReadToEnd

            Wresponse.Close()
            Strm.Close()
        Catch ex As Exception
            Ok = False
        Finally
            Strm.Dispose()
            Me.html = texto
        End Try

        Return Ok

    End Function

    Public Sub enviar(ByVal de As String, ByVal para As String, ByVal asunto As String)

        Dim mCliente As New System.Net.Mail.MailMessage

        mCliente.IsBodyHtml = True
        mCliente.Body = Me.html
        mCliente.From = New System.Net.Mail.MailAddress(de)
        mCliente.Subject = asunto
        mCliente.To.Add(para)

        Dim smtp As New System.Net.Mail.SmtpClient
        smtp.Host = System.Configuration.ConfigurationSettings.AppSettings("SMTP")
        smtp.Send(mCliente)
    End Sub

End Class
