Public Class web


    ''' <summary>
    ''' carga el webcontrol con el sql enviado, puede conservar valore
    ''' </summary>
    ''' <param name="WebControl"></param>
    ''' <param name="sql"></param>
    ''' <param name="conservar"></param>
    ''' <remarks></remarks>
    Public Shared Sub CargarWebControlIdNombre(ByRef WebControl As Object, ByVal sql As String, Optional ByVal conservar As Boolean = False, Optional ByVal mensajeInicial As String = Nothing)
        Dim arreglo As ArrayList
        Dim total As ArrayList

        total = New ArrayList
        arreglo = New ArrayList()

        If conservar Then total.AddRange(GetItemsWebControl(WebControl))

        arreglo = htcLib.LM.CargaAL(sql, New IdNombre())

        If mensajeInicial <> Nothing Then total.Add(New IdNombre(0, mensajeInicial))

        total.AddRange(arreglo)

        WebControl.DataSource = total
        WebControl.DataValueField = "idProp"
        WebControl.DataTextField = "nomProp"
        WebControl.DataBind()
    End Sub

    ''' <summary>
    ''' Retorna un arreglo de los items (todos o seleccionados) de un WebControl, de tipo id nombre
    ''' </summary>
    ''' <param name="objeto"></param>
    ''' <param name="soloSeleccionados"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetItemsWebControl(ByVal objeto As Object, Optional ByVal soloSeleccionados As Boolean = False) As ArrayList
        Dim arreglo As New ArrayList
        Dim contador As Long
        Dim item As Object

        contador = 0
        For Each item In objeto.Items
            Select Case soloSeleccionados
                Case True
                    If item.selected Then arreglo.Add(item)
                Case False
                    arreglo.Add(item)
            End Select
            contador += 1
        Next
        Return arreglo
    End Function


    Public Shared Sub formaAmail(ByVal letras As String, ByVal forma As System.Collections.Specialized.NameValueCollection, ByVal para As String, ByVal De As String, ByVal subject As String, Optional ByVal smtp As String = "", Optional ByVal diratt As String = "", Optional ByVal Bcc As String = "", Optional ByVal responderA As String = "", Optional ByVal extras As String = "", Optional ByVal nombreDE As String = "", Optional ByVal usuario As String = "", Optional ByVal pwd As String = "", Optional ByVal HabilitarSSL As Boolean = False)

        Dim smtp1 As New System.Net.Mail.SmtpClient(smtp)

        Dim OBatt As System.Net.Mail.Attachment
        Dim item As String


        Dim valores As New System.Text.StringBuilder

        For Each item In forma
            If item.StartsWith(letras) Then
                valores.Append(item.Substring(letras.Length) & ": ")
                valores.Append(forma(item) & vbCrLf)
            End If
        Next

        If Not extras = "" Then
            'extras debe tener la forma campo@valor,campo2@valor2,Tarjeta@1234567894512,cvv@231
            Dim nuevos() As String = extras.Split(",")
            Dim i As Long

            For i = 0 To nuevos.GetUpperBound(0)
                valores.Append(nuevos(i).Split("@")(0) & ": ")
                valores.Append(nuevos(i).Split("@")(1) & vbCrLf)
            Next
        End If


        Dim m As New System.Net.Mail.MailMessage

        Try
            m.Body = valores.ToString
            m.From = New System.Net.Mail.MailAddress(De, nombreDE)

            m.Subject = subject
            m.To.Add(para)
            If Not Bcc = "" Then m.Bcc.Add(Bcc)
            If Not responderA = "" Then m.ReplyTo = New System.Net.Mail.MailAddress(responderA)

            If Not diratt = "" Then
                OBatt = New System.Net.Mail.Attachment(diratt)
                m.Attachments.Add(OBatt)
            End If




            If usuario <> "" And pwd <> "" Then
                Dim basicAuthentication As System.Net.NetworkCredential = New System.Net.NetworkCredential(usuario, pwd)
                smtp1.UseDefaultCredentials = False
                smtp1.Credentials = basicAuthentication
                smtp1.EnableSsl = HabilitarSSL
                smtp1.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network
            End If

            smtp1.Send(m)

        Catch ex As Exception
            Throw ex ' para ver el error por ahora
        Finally
            OBatt = Nothing
            m = Nothing
        End Try

    End Sub

    Public Shared Sub formaAmailHtml(ByVal letras As String, ByVal forma As System.Collections.Specialized.NameValueCollection, ByVal para As String, ByVal De As String, ByVal subject As String, Optional ByVal smtp As String = "", Optional ByVal diratt As String = "", Optional ByVal Bcc As String = "")
        'sin usar nombrecampos

        Dim smtp1 As New System.Net.Mail.SmtpClient(smtp)

        Dim OBatt As System.Net.Mail.Attachment
        Dim item As String

        Dim itemC As String
        Dim valores As New System.Text.StringBuilder

        For Each item In forma
            If item.StartsWith(letras) Then
                itemC = item.Substring(letras.Length) & ": "

                valores.AppendFormat("<b>{0}</b>", itemC)
                valores.Append(forma(item) & "<br>")
            End If
        Next

        Dim m As New System.Net.Mail.MailMessage

        Try
            m.IsBodyHtml = True
            m.Body = valores.ToString
            m.From = New System.Net.Mail.MailAddress(De)
            m.Subject = subject
            m.To.Add(para)
            m.Bcc.Add(Bcc)

            If Not diratt = "" Then
                OBatt = New System.Net.Mail.Attachment(diratt)
                m.Attachments.Add(OBatt)
            End If

            smtp1.Send(m)

        Catch ex As Exception
            Throw ex ' para ver el error por ahora
        Finally
            OBatt = Nothing
            m = Nothing
        End Try

    End Sub

    Public Shared Function SQLFormaATabla(ByVal letras As String, ByVal forma As System.Collections.Specialized.NameValueCollection, ByVal tabla As String, ByVal id As Long, ByVal ParamArray otros() As String) As String
        Dim item, letr, form As String
        Dim sql As New Text.StringBuilder
        Dim tempLet As New System.Text.StringBuilder
        Dim tempFor As New System.Text.StringBuilder
        Dim todo, campo, valor As String


        Dim valores As New System.Text.StringBuilder


        If id = 0 Then 'sql para nuevo

            id = espacio.ManejadorBD.secuencial(tabla)
            tempLet.Append("(id,")
            tempFor.AppendFormat("({0},", id)

            For Each item In forma

                If item.StartsWith(letras) Then

                    tempLet.AppendFormat("{0},", item.Substring(letras.Length))

                    If forma(item).Length = 0 Then
                        tempFor.AppendFormat("'{0}',", "0")
                    Else
                        tempFor.AppendFormat("'{0}',", forma(item))
                    End If

                End If
            Next

            'Ahora pongo los campos adicionales
            For Each todo In otros
                campo = todo.Split("@")(0)
                valor = todo.Split("@")(1)

                tempLet.AppendFormat("{0},", campo)
                tempFor.AppendFormat("'{0}',", valor)
            Next


            letr = String.Format("{0})", tempLet.ToString.TrimEnd(","))
            form = String.Format("{0})", tempFor.ToString.TrimEnd(","))

            sql.AppendFormat("insert into {0} {1} values {2}", tabla, letr, form)

        Else 'sql para update
            For Each item In forma
                If item.StartsWith(letras) Then
                    valores.AppendFormat("{0}='{1}',", item.Substring(letras.Length), forma(item))
                End If
            Next

            'Ahora pongo los campos adicionales
            For Each todo In otros
                campo = todo.Split("@")(0)
                valor = todo.Split("@")(1)

                valores.AppendFormat("{0}='{1}',", campo, valor)
            Next

            sql.AppendFormat("Update {0} set {1} Where id={2}", tabla, valores.ToString.TrimEnd(","), id)
        End If

        Return sql.ToString

    End Function


    Public Shared Sub seleccionaCombo(ByVal combo As System.Web.UI.WebControls.DropDownList, ByVal id As Long)

        '        Dim it As System.Web.UI.WebControls.ListItem
        Dim i As Long

        For i = 0 To combo.Items.Count - 1
            If combo.Items(i).Value = id Then
                combo.SelectedIndex = i
            End If
        Next

    End Sub

    'esta podria ser general y no solo web
    Public Shared Function dataTableaStringXL(ByVal dt As DataTable) As String

        Dim text As New System.Text.StringBuilder
        Dim r As DataRow
        Dim c As DataColumn

        For Each c In dt.Columns
            text.AppendFormat("{0}{1}", c.Caption, vbTab) 'para los titulos
        Next

        text.Append(vbCrLf)

        For Each r In dt.Rows
            For Each c In dt.Columns
                text.AppendFormat("{0}{1}", r(c), vbTab) 'para las celdas
            Next
            text.Append(vbCrLf)
        Next

        Return text.ToString

    End Function

    Public Shared Function sqlAStringXL(ByVal sql As String) As String

        Dim dt As DataTable

        dt = espacio.ManejadorBD.traetabla(sql)

        Return dataTableaStringXL(dt)

    End Function

    Public Shared Sub descargarSqlComoExcel(ByVal sql As String)

        Dim r As System.Web.HttpResponse
        Dim txt As String = sqlAStringXL(sql)

        r = System.Web.HttpContext.Current.Response

        r.Clear()
        r.ClearHeaders()
        r.ContentType = "Application/x-msexcel"
        'Response.AddHeader("Content-Disposition", nombre)
        r.Write(txt)
        r.End()

    End Sub

    Public Shared Sub descargarDataTableComoExcel(ByVal tabla As DataTable)

        Dim r As System.Web.HttpResponse
        Dim txt As String = dataTableaStringXL(tabla)

        r = System.Web.HttpContext.Current.Response

        r.Clear()
        r.ClearHeaders()
        r.ContentType = "Application/x-msexcel"
        'Response.AddHeader("Content-Disposition", nombre)
        r.Write(txt)
        r.End()

    End Sub

    Public Shared Function getSmtpPruebasGmail() As System.Net.Mail.SmtpClient
        Dim smtp1 As System.Net.Mail.SmtpClient

        Dim basicAuthentication As System.Net.NetworkCredential = New System.Net.NetworkCredential("htcprueba@gmail.com", "htcprueba1")
        smtp1 = New System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
        smtp1.UseDefaultCredentials = False
        smtp1.Credentials = basicAuthentication
        smtp1.EnableSsl = True
        smtp1.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network

        Return smtp1

    End Function

End Class
