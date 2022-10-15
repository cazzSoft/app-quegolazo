Imports System.Web.Mail

Public Class mailMasivo

    Public de As String
    Public nombreDe As String
    Public subject As String
    Public smtp As String

    Public plantilla As String
    Public tabla As DataTable
    Public contador As Long

    Public IsBodyHtml As Boolean = False
    Public LlaveEncriptacion As String  'establecer llave cuando se desea encriptacion

    Private Adjuntos As ArrayList

    Public Sub dispose()
        Me.tabla.Dispose()
        Me.tabla = Nothing
        Me.plantilla = Nothing
    End Sub

    Sub New()
        Me.Adjuntos = New ArrayList
    End Sub

    Sub New(ByVal pathPlantilla As String, ByVal de As String, ByVal subject As String, ByVal smtp As String)
        'lee el archivo y lo guarda como plantilla
        Me.Adjuntos = New ArrayList
    End Sub

    Sub New(ByVal solotexto As Boolean, ByVal texto As String, ByVal de As String, ByVal subject As String, ByVal smtp As String)
        Me.Adjuntos = New ArrayList
        Me.de = de
        Me.subject = subject
        Me.smtp = smtp

        Me.plantilla = texto

    End Sub


    Sub New(ByVal tabla As DataTable, ByVal texto As String, ByVal de As String, ByVal subject As String, ByVal smtp As String)
        Me.Adjuntos = New ArrayList
        Me.de = de
        Me.subject = subject
        Me.smtp = smtp
        Me.plantilla = texto
        Me.tabla = tabla

    End Sub


    Public Function textoFinalCampo(ByVal dr As DataRow) As String
        'aca reemplazo los datos. Asumo que vienen campos campo1, campo2, etc + 1 de diercciones
        Dim i As Long
        Dim final As String = plantilla
        Dim campo, campot As String

        For i = 1 To dr.Table.Columns.Count - 1
            campo = String.Format("%campo{0}%", i)
            campot = String.Format("campo{0}", i)
            final = final.Replace(campo, dr(campot))
        Next


        Return final
    End Function

    Public Function textoFinal(ByVal dr As DataRow) As String
        Dim final As String = plantilla
        Dim campo, nombreColumna As String
        Dim Columna As DataColumn

        For Each Columna In dr.Table.Columns
            nombreColumna = Columna.ColumnName
            If Not dr(nombreColumna) Is DBNull.Value Then
                campo = String.Format("%{0}%", nombreColumna)
                final = final.Replace(campo, dr(nombreColumna))
                campo = String.Format("${0}$", nombreColumna)
                If final.IndexOf(campo) > -1 Then
                    final = final.Replace(campo, Encriptacion.Encriptar(dr(nombreColumna), Me.LlaveEncriptacion)) 'si no la encuentra no la reemplaza
                End If

            End If

        Next

        Return final
    End Function
    'asumo una tabla direcciones en la 
    'Por hacer>> marcar los que ya hace
    Function enviaMails() As Long
        Dim contador As Long

        Dim dt As DataTable = htcLib.espacio.ManejadorBD.traetabla("select mail from direcciones")
        Dim dr As DataRow

        For Each dr In dt.Rows
            If enviaMail(textoFinal(dr), dr("mail")) = True Then contador += 1
        Next

        Return contador

    End Function

    Function enviaMails(ByVal paras As String) As Long
        Dim contador As Long
        Dim para() As String = paras.Split(",")
        Dim m As String

        For Each m In para
            If enviaMail(plantilla, m) = True Then contador += 1
        Next

        Return contador

    End Function


    Sub enviaMailsTemp()

    End Sub

    Public Sub Envia()
        Dim dr As DataRow

        For Each dr In Me.tabla.Rows
            If enviaMail(textoFinal(dr), dr("email")) = True Then contador += 1
        Next

        'enviaMail(textoFinal(dr), "iviteri@gmail.com")

        Me.dispose()

    End Sub

    Public Sub AddAdjunto(ByVal ParamArray dirDocumentos() As String)
        Dim documento As String
        Dim adjunto As System.Net.Mail.Attachment


        For Each documento In dirDocumentos
            If Not documento = "" Then
                Adjunto = New System.Net.Mail.Attachment(documento)
                Me.Adjuntos.Add(Adjunto)
            End If
        Next

    End Sub

    Function enviaMail(ByVal html As String, ByVal dir As String) As Boolean
        Dim m As New System.Net.Mail.MailMessage
        Dim pudo As Boolean
        Dim de1 As New System.Net.Mail.MailAddress(de, nombreDe)
        Dim smtp1 As New System.Net.Mail.SmtpClient(smtp)
        Dim att As System.Net.Mail.Attachment

        Try
            m.Body = html
            m.From = de1
            m.Subject = subject
            m.IsBodyHtml = IsBodyHtml

            If Me.Adjuntos.Count > 0 Then
                For Each att In Me.Adjuntos
                    m.Attachments.Add(att)
                Next
            End If

            m.To.Add(dir)

            smtp1.Send(m)

            pudo = True
        Catch ex As Exception
            pudo = False
        Finally
            m = Nothing
        End Try

        Return pudo

    End Function


End Class
