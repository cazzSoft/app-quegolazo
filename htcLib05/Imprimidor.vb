Imports System.Drawing.Printing
Imports System.Drawing

Imports System.Reflection


Public Class Imprimidor

    Public Enum tiposItem
        buscaPropiedad = 1
        TextoFijo = 2
        BuscaEnItemDeCol = 3
        lineaCol = 4
        buscaEnExtras = 5

    End Enum

    Public Shared Sub imprime(ByVal pathDoc As String, ByVal obj As Object, Optional ByVal impresora As PrinterSettings = Nothing, Optional ByVal extras() As String = Nothing)

        Dim x As New documento

        x.cargar(pathDoc)
        If Not extras Is Nothing Then x.extras.AddRange(extras)
        x.llenar(obj)

        Dim pd As PrintDocument
        pd = x.GetPrintDoc
        If Not impresora Is Nothing Then pd.PrinterSettings = impresora
        pd.Print()

    End Sub

    Public Shared Function getPrintDoc(ByVal pathDoc As String, ByVal obj As Object, ByVal ParamArray extras() As String) As PrintDocument

        Dim x As New documento
        x.cargar(pathDoc)
        If Not extras Is Nothing Then x.extras.AddRange(extras)

        x.llenar(obj)
        Return x.GetPrintDoc

    End Function

    'windows
    Public Shared Function getFormato(ByVal tipo As Long, Optional ByVal pregunta As String = "") As String
        Dim sql As String = String.Format("SELECT * FROM tblPosiblesFormatos WHERE tipo = {0}", tipo)
        Dim temp As String

        If pregunta = "" Then pregunta = "Desea imprimir el documento"

        Dim dt As DataTable = espacio.ManejadorBD.traetabla(sql)

        Select Case dt.Rows.Count
            Case 0
                temp = ""
            Case 1
                If System.Windows.Forms.MessageBox.Show(pregunta, "Impresiones", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then
                    temp = dt.Rows(0).Item("archivo")
                Else
                    temp = ""
                End If

            Case Else
                Dim f As New frmFormatos
                f.lista.DataSource = dt
                f.lista.DisplayMember = "nombre"
                'f.lista.ValueMember = ""
                f.ShowDialog()
                temp = f.dirsel
                f.Close()
        End Select

        dt.Dispose()
        Return temp

    End Function

    Public Shared Function getExtraDeUser(ByVal nom As String) As String
        Return InputBox("Ingrese el valor para " & nom & ":", "Impresion", "")
    End Function



    '---

    Public Class documento

        Public tamanoFont As Long
        Public nombreFont As String

        Private printFont As Font

        Public factorx, factory As Single


        Public items As ArrayList
        Public lineas As ArrayList
        Public cols As ArrayList
        Public extras As ArrayList

        Public paginas As ArrayList
        Public iPagAct As Long

        Public ReadOnly Property pagActual() As pagina
            Get
                Return Me.paginas(iPagAct)
            End Get
        End Property


        Public Sub New()

            factorx = 5
            factory = 2

            printFont = New Font("Arial", 10)

            Me.items = New ArrayList
            Me.cols = New ArrayList
            Me.lineas = New ArrayList
            Me.extras = New ArrayList

        End Sub

        Function getExtra(ByVal nombre As String) As String
            Dim s As String
            Dim arr(1) As String

            For Each s In Me.extras
                arr = s.Split("@")
                If arr(0).ToUpper = nombre.ToUpper Then
                    Return arr(1)
                End If
            Next

            'si no encuentra
            Return Imprimidor.getExtraDeUser(nombre)

        End Function

        Public Sub cargar(ByVal dir1 As String)

            Dim doc As New Xml.XmlDocument
            Dim col As coleccion
            Dim linea As linea


            doc.Load(dir1)

            Dim El1, el2, el3 As Xml.XmlElement 'elementos temporales para navegar y crear

            Me.tamanoFont = doc.ChildNodes(0).Attributes("tamanofont").Value
            Me.nombreFont = doc.ChildNodes(0).Attributes("nombrefont").Value

            Me.factorx = doc.ChildNodes(0).Attributes("factorx").Value
            Me.factory = doc.ChildNodes(0).Attributes("factory").Value

            Try
                printFont = New Font(Me.nombreFont, Me.tamanoFont)
            Catch ex As Exception
            End Try


            For Each El1 In doc.ChildNodes(0).ChildNodes

                Select Case El1.Name
                    Case "item"
                        Me.items.Add(cargaItem(El1))

                    Case "linea"
                        linea = New linea
                        linea.x = El1.Attributes("x").Value
                        linea.x2 = El1.Attributes("x2").Value
                        linea.y = El1.Attributes("y").Value
                        linea.y2 = El1.Attributes("y2").Value

                        Me.lineas.Add(linea)

                    Case "coleccion"
                        col = New coleccion
                        col.nombreEnObjeto = El1.Attributes("nombreenobjeto").Value
                        col.yInicio = El1.Attributes("yinicio").Value
                        Try
                            col.maxY = El1.Attributes("maxY").Value
                        Catch ex As Exception
                            col.maxY = -1
                        End Try


                        Try
                            col.incrementoY = El1.Attributes("incrementoy").Value
                        Catch ex As Exception
                        End Try


                        col.items = New ArrayList
                        For Each el2 In El1.ChildNodes
                            Select Case el2.Name
                                Case "item"
                                    col.items.Add(cargaItem(el2))
                                Case "pie"
                                    col.pie = New ArrayList
                                    For Each el3 In el2.ChildNodes
                                        col.pie.Add(cargaItem(el3))
                                    Next
                            End Select

                        Next

                        Me.cols.Add(col)
                End Select

            Next



        End Sub

        Function cargaItem(ByVal El1 As Xml.XmlElement) As item
            Dim it As New item

            it.x = El1.Attributes("x").Value
            it.y = El1.Attributes("y").Value
            it.tipo = El1.Attributes("tipo").Value

            Try
                it.alineacion = El1.Attributes("align").Value
            Catch ex As Exception
            End Try

            Try
                it.formato = El1.Attributes("formato").Value
            Catch ex As Exception
            End Try


            Select Case it.tipo
                Case tiposItem.buscaPropiedad, tiposItem.BuscaEnItemDeCol, tiposItem.buscaEnExtras
                    it.nombreEnObjeto = El1.Attributes("nombreenobjeto").Value
                    it.titulo = El1.Attributes("valor").Value
                Case tiposItem.TextoFijo, tiposItem.lineaCol
                    it.valor = El1.Attributes("valor").Value
            End Select

            Return it

        End Function

        'sacar a LM y usra en funciones
        Function getvalor(ByVal ob As Object, ByVal nomProp As String) As Object
            Dim tipo As Type = ob.GetType

            Dim valor As Object

            Dim member As MemberInfo

            Dim campo As FieldInfo
            Dim met As MethodInfo
            Dim prop As PropertyInfo


            If nomProp.IndexOf(".") > 0 Then
                Dim arr() As String
                arr = nomProp.Split(".")
                'UN NIVEL POR AHORA, MEJORAR PARA N niveles: iterar, hacer objeto temp

                ob = getvalor(ob, arr(0))
                tipo = ob.GetType
                nomProp = arr(1)
            End If

            member = tipo.GetMember(nomProp)(0)

            Select Case member.MemberType
                Case MemberTypes.Field
                    campo = member
                    valor = campo.GetValue(ob)
                Case MemberTypes.Property
                    prop = member
                    valor = prop.GetValue(ob, Nothing)
                Case MemberTypes.Method
                    met = member
                    valor = met.Invoke(ob, Nothing)
                Case Else
                    prop = member
                    valor = prop.GetValue(ob, Nothing)
            End Select

            Return valor

        End Function


        Public Sub llenar(ByVal obj As Object)

            Dim valor As Object
            Dim item As item
            Dim i As String


            For Each item In Me.items

                Select Case item.tipo

                    Case tiposItem.TextoFijo, tiposItem.lineaCol

                    Case tiposItem.buscaEnExtras
                        item.valor = Me.getExtra(item.nombreEnObjeto)

                    Case Else

                        i = item.nombreEnObjeto
                        valor = getvalor(obj, i)

                        If Not item.formato = "" Then
                            valor = CType(valor, IFormattable).ToString(item.formato, Nothing)
                        End If

                        item.valor = valor

                End Select

            Next

            Me.paginas = New ArrayList
            Me.paginas.Add(New pagina)
            Me.iPagAct = 0
            Me.pagActual.items.AddRange(Me.items)

            llenaColecciones(obj)

        End Sub


        Sub llenaColecciones(ByVal obj As Object)

            Dim col As coleccion
            Dim obcol As ArrayList
            Dim ob As Object

            Dim item As item
            Dim y As Single
            Dim yAusar As Single


            'colecciones
            For Each col In Me.cols ' colecciones del doc

                obcol = getvalor(obj, col.nombreEnObjeto)

                y = col.yInicio

                For Each ob In obcol 'objetos de la coleccion

                    For Each item In col.items
                        creaItem(obj, ob, item, y + item.y)
                    Next 'items en obs

                    If col.incrementoY = 0 Then
                        y += printFont.GetHeight() * 0.254
                    Else
                        y += col.incrementoY
                    End If
                    If Not item.y = -1 Then y += item.y

                    If col.maxY > 0 AndAlso y >= col.maxY Then
                        y = 20
                        Me.paginas.Add(New pagina)
                        Me.iPagAct = Me.paginas.Count - 1
                    End If

                Next 'objetos

                If Not col.pie Is Nothing Then
                    For Each item In col.pie
                        If item.y = -1 Then yAusar = y Else yAusar = y + item.y
                        creaItem(obj, Nothing, item, yAusar)
                        If Not item.y = -1 Then
                            If col.incrementoY = 0 Then
                                y += printFont.GetHeight() * 0.254
                            Else
                                y += col.incrementoY
                            End If
                            y += item.y
                        End If
                    Next

                End If


            Next 'colecciones


        End Sub

        Sub creaItem(ByVal obj As Object, ByVal objCOL As Object, ByVal itemcol As item, ByVal y As Single)
            Dim i As String
            Dim valor As Object

            Dim nuevoItem As item


            i = itemcol.nombreEnObjeto

            Select Case itemcol.tipo
                Case tiposItem.buscaPropiedad
                    valor = getvalor(obj, i)

                Case tiposItem.TextoFijo
                    valor = itemcol.valor

                Case tiposItem.lineaCol

                Case tiposItem.BuscaEnItemDeCol
                    'Try
                    valor = getvalor(objCOL, i)

            End Select

            If Not itemcol.formato = "" Then
                valor = CType(valor, IFormattable).ToString(itemcol.formato, Nothing)
            End If


            If Not itemcol.tipo = tiposItem.lineaCol Then
                nuevoItem = New item
                nuevoItem.x = itemcol.x
                nuevoItem.y = y
                nuevoItem.valor = valor
                nuevoItem.titulo = itemcol.titulo
                nuevoItem.alineacion = itemcol.alineacion

                'Me.items.Add(nuevoItem)
                Me.pagActual.items.Add(nuevoItem)
            Else
                Dim l As New linea
                l.y = y
                l.y2 = y
                l.x = itemcol.x
                l.x2 = l.x + itemcol.valor

                Me.lineas.Add(l)
            End If
        End Sub

        Public Function GetPrintDoc() As PrintDocument

            'Try
            Me.iPagAct = 0
            Dim pd As New PrintDocument

            AddHandler pd.PrintPage, AddressOf Me.pd_PrintPage

            pd.OriginAtMargins = False

            Return pd

        End Function

        'este es el metodo donde se imprime de verdad
        Private Sub pd_PrintPage(ByVal sender As Object, ByVal ev As PrintPageEventArgs)
            Dim item As item
            Dim l As linea

            'linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics)

            For Each item In pagActual.items

                If item.alineacion = 1 Then
                    Dim formato As New Drawing.StringFormat
                    formato.Alignment = StringAlignment.Far

                    ev.Graphics.DrawString(item.imprimible, printFont, Brushes.Black, item.Xcm(factorx), item.Ycm(factory), formato)
                Else
                    ev.Graphics.DrawString(item.imprimible, printFont, Brushes.Black, item.Xcm(factorx), item.Ycm(factory))
                End If

            Next

            For Each l In Me.lineas
                ev.Graphics.DrawLine(Pens.Black, l.Xcm(factorx), l.Ycm(factory), l.X2cm(factorx), l.Y2cm(factory))
            Next

            If Me.iPagAct < Me.paginas.Count - 1 Then
                Me.iPagAct += 1
                ev.HasMorePages = True
            Else
                ev.HasMorePages = False
            End If

        End Sub


    End Class

    Public Class item

        Public tipo As tiposItem
        Public x, y As Single
        Public nombre As String
        Public titulo As String
        Public nombreEnObjeto As String
        Public alineacion As Int16 ' 0 =izq 1= der
        Public formato As String

        Public valor As String

        Public Function imprimible() As String
            Return Me.titulo & Me.valor
        End Function

        Public Function Xcm(ByVal factor As Single, Optional ByVal pf As Font = Nothing) As Single
            Return (Me.x - factor) / 0.254

        End Function

        Public Function Ycm(ByVal factor As Single) As Single
            Return (Me.y - factor) / 0.254
        End Function


    End Class

    Public Class coleccion
        Public yInicio As Single
        Public x As Single
        Public nombreEnObjeto As String
        Public incrementoY As Single

        Public maxY As Single


        Public items As ArrayList

        Public pie As ArrayList
    End Class

    Public Class linea
        Public x, x2, y, y2 As Single

        Public Function X2cm(ByVal factor As Single) As Single
            Return (Me.x2 - factor) / 0.254
        End Function

        Public Function Y2cm(ByVal factor As Single) As Single
            Return (Me.y2 - factor) / 0.254
        End Function

        Public Function Xcm(ByVal factor As Single) As Single
            Return (Me.x - factor) / 0.254
        End Function

        Public Function Ycm(ByVal factor As Single) As Single
            Return (Me.y - factor) / 0.254
        End Function
    End Class

    Public Class pagina
        Public items As ArrayList

        Sub New()
            Me.items = New ArrayList
        End Sub
    End Class


End Class
