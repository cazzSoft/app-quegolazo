Imports System.Reflection
Imports System.Windows.Forms

'Depende de espacio solo por fechas


Public Class LM


#Region "Objetos"

    Public Shared Sub BaseAObjeto(ByVal fila As System.Data.IDataReader, ByVal obj As Object)
        Dim i As Short
        Dim tipo As Type = obj.GetType
        Dim campo As FieldInfo
        Dim prop As PropertyInfo
        Dim member As MemberInfo
        Dim campos() As MemberInfo

        Dim valor As Object
        Dim guardo As Boolean

        campos = tipo.GetMembers(BindingFlags.Instance Or BindingFlags.Public)
        For i = 0 To campos.GetUpperBound(0)
            member = campos(i)

            guardo = DeboLeer(member)

            'si se guerda, 
            If guardo = True Then

                Try
                    valor = fila(member.Name)
                Catch ex As Exception
                    valor = fila(String.Format("{0}.{1}", obj.mitabla, member.Name))
                End Try

                Select Case member.MemberType
                    Case MemberTypes.Field
                        campo = member
                        If Not IsDBNull(valor) Then campo.SetValue(obj, Convert.ChangeType(valor, campo.FieldType))
                        If Not IsDBNull(valor) Then campo.SetValue(obj, Convert.ChangeType(valor, campo.FieldType))
                    Case MemberTypes.Property
                        prop = member
                        If Not IsDBNull(valor) AndAlso Not prop.CanWrite = False Then prop.SetValue(obj, Convert.ChangeType(valor, prop.PropertyType), Nothing)
                End Select

            End If

        Next

        Exit Sub


    End Sub

    Public Shared Sub BaseAObjeto(ByVal fila As System.Data.DataRow, ByVal obj As Object)
        Dim i As Short
        Dim tipo As Type = obj.GetType
        Dim campo As FieldInfo
        Dim prop As PropertyInfo
        Dim member As MemberInfo
        Dim campos() As MemberInfo

        Dim valor As Object
        Dim guardo As Boolean

        campos = tipo.GetMembers(BindingFlags.Instance Or BindingFlags.Public)
        For i = 0 To campos.GetUpperBound(0)
            member = campos(i)

            guardo = DeboLeer(member)

            'si se guerda, 
            If guardo = True Then

                Try
                    valor = fila.Item(member.Name)
                Catch ex As Exception
                    'Debug.Write(ex.Message)
                    valor = fila(String.Format("{0}.{1}", obj.mitabla, member.Name))
                End Try

                Select Case member.MemberType
                    Case MemberTypes.Field
                        campo = member
                        If Not IsDBNull(valor) Then campo.SetValue(obj, Convert.ChangeType(valor, campo.FieldType))

                    Case MemberTypes.Property
                        prop = member
                        If Not IsDBNull(valor) AndAlso Not prop.CanWrite = False Then prop.SetValue(obj, Convert.ChangeType(valor, prop.PropertyType), Nothing)
                End Select

            End If

        Next

        Exit Sub


    End Sub

    Public Shared Sub GuardaObjeto(ByVal obj As Object)
        Dim sql As String
        Dim mytabla As String

        mytabla = obj.miTabla

        If Not obj.id = 0 Then
            sql = String.Format("UPDATE {0} SET {1} WHERE id={2}", mytabla, sqlupdate(obj), obj.id)
        Else
            obj.id = espacio.ManejadorBD.secuencial(mytabla)
            If obj.id = 0 Then Throw New Exception
            sql = String.Format("INSERT INTO {0} {1} ", mytabla, sqlInsertCompleto(obj))
        End If

        espacio.ManejadorBD.ejecuta(sql)


    End Sub

    Public Shared Sub guardaobjeto(ByVal obj As Object, ByVal ForzarNuevo As Boolean)
        Dim mytabla As String
        Dim nsql As New System.Text.StringBuilder

        mytabla = obj.miTabla

        If Not ForzarNuevo = True Then
            nsql.AppendFormat("update {0} set {1} where id={2}", mytabla, sqlupdate(obj), CStr(obj.id))
        Else
            nsql.AppendFormat("INSERT INTO {0} {1} ", mytabla, sqlInsertCompleto(obj))            
        End If

        espacio.ManejadorBD.ejecuta(nsql.ToString)

    End Sub

    'Guarda un objeto sin importarle si el id =0
    'siempre inserta, a menos que pruebaedita=true Y ya haya otro objeto con ese id

    Public Shared Sub GuardaNuevoObjetoSinID(ByVal obj As Object, Optional ByVal pruebaSiEdita As Boolean = False)
        Dim sql As String
        Dim mytabla As String
        Dim guardar As Boolean


        If pruebaSiEdita = True Then
            If EditaObjeto(obj) = True Then
                guardar = False
            Else
                guardar = True
            End If
        Else
            guardar = True
        End If

        If guardar = True Then
            mytabla = obj.miTabla

            sql = String.Format("INSERT INTO {0} {1} ", mytabla, sqlInsertCompleto(obj))

            espacio.ManejadorBD.ejecuta(sql)
        End If

    End Sub

    Public Shared Function EditaObjeto(ByVal obj As Object) As Boolean
        Dim sql As String
        Dim mytabla As String
        Dim ra As Long

        mytabla = obj.miTabla
        sql = String.Format("update {0} set {1} where id={2}", mytabla, sqlupdate(obj), CStr(obj.id))

        ra = espacio.ManejadorBD.ejecuta(sql)

        If ra > 0 Then Return True Else Return False

    End Function

    Public Shared Function cargaObjeto(ByVal sql As String, ByRef obj As Object) As Boolean
        Dim dr As IDataReader

        dr = espacio.ManejadorBD.traeReader(sql)

        If dr.Read Then
            BaseAObjeto(dr, obj)
            dr.Close()
            Return True
        Else
            dr.Close()
            Return False
        End If

    End Function

    Public Shared Function cargaObjeto(ByVal dr As DataRow, ByRef obj As Object) As Boolean

        BaseAObjeto(dr, obj)

    End Function


    ''' <summary>
    ''' Esta funcion devuelve un datatable con TODOS los campos del objeto
    ''' </summary>
    ''' <param name="listaObjetos">un arraylist de objetos a convertir</param>
    ''' <returns></returns>
    ''' <remarks>Podriamos implementar versolo los guardables...como en otras cosas</remarks>
    Public Shared Function ObjetoADatatable(ByVal listaObjetos As ArrayList) As DataTable

        Dim txtCampos As New ArrayList
        Dim txtCampo As String
        Dim obj As Object = listaObjetos(0)
        Dim tipo As Type = obj.GetType

        Dim member As MemberInfo
        Dim campos() As FieldInfo
        Dim campos1() As PropertyInfo

        Dim i As Long

        'Veo todos los campos del objeto
        campos = tipo.GetFields(BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.GetField Or BindingFlags.GetProperty)
        For i = 0 To campos.GetUpperBound(0)
            member = campos(i)
            txtCampo = member.Name

            txtCampos.Add(txtCampo)
        Next

        'campos1 = tipo.GetProperties(BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.GetField Or BindingFlags.GetProperty)
        'For i = 0 To campos.GetUpperBound(0)
        '    member = campos(i)
        '    txtCampo = member.Name

        '    txtCampos.Add(txtCampo)
        'Next



        'ahora llamo a la funcion
        Return ObjetoADatatable(listaObjetos, txtcampos)

    End Function

    ''' <summary>
    ''' Esta funcion devuelve un datatable con los campos del objeto que se envien com parametro
    ''' </summary>
    ''' <param name="listaObjetos">un arraylist de objetos a convertir</param>
    ''' <param name="campos">Que campos quiero que vengan en el DT, deben ser textos simples</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ObjetoADatatable(ByVal listaObjetos As ArrayList, ByVal campos As ArrayList) As DataTable

        Dim campo As String
        Dim ob As Object 'GENERIC???????
        Dim dr As DataRow
        Dim dt As New DataTable
        Dim col As DataColumn

        'primero creo la estructura de la tabla
        For Each campo In campos
            col = New DataColumn(campo)
            dt.Columns.Add(col)
        Next

        'luego lleno la tabla
        For Each ob In listaObjetos
            dr = dt.NewRow

            For Each campo In campos
                dr(campo) = CallByName(ob, campo, CallType.Get)
            Next

            dt.Rows.Add(dr)

        Next

        Return dt

    End Function


#End Region

#Region "sqls"
    Public Shared Function traesql1(ByVal obj As Object) As String
        Dim sql As New System.Text.StringBuilder
        Dim i As Short
        Dim tipo As Type = obj.GetType
        Dim campo As FieldInfo
        Dim cmp As String
        Dim guardo As Boolean
        Dim campos() As MemberInfo
        Dim prop As PropertyInfo
        Dim member As MemberInfo

        campos = tipo.GetMembers(BindingFlags.Instance Or BindingFlags.Public)

        For i = 0 To campos.GetUpperBound(0)
            '.GetFields.GetUpperBound(0)
            member = campos(i)
            Select Case member.MemberType
                Case MemberTypes.Field
                    guardo = DeboGuardar(member)
                    If guardo = True Then
                        campo = member
                        cmp = campo.Name
                    End If
                Case MemberTypes.Property
                    guardo = DeboGuardar(member)
                    If guardo = True Then
                        prop = member
                        cmp = prop.Name
                    End If
                Case Else
                    guardo = False
            End Select

            If guardo = True Then sql.Append(cmp & ",")

        Next

        'quito la ultima coma
        Return sql.ToString.TrimEnd(",")


    End Function

    Public Shared Function sqlupdate(ByVal obj As Object) As String
        Dim sql As New System.Text.StringBuilder
        Dim i As Short
        Dim tipo As Type = obj.GetType
        Dim campo As FieldInfo
        Dim valor As Object
        Dim txt As String
        Dim guardo As Boolean
        Dim campos() As MemberInfo
        Dim prop As PropertyInfo
        Dim member As MemberInfo
        Dim tipodato As String

        campos = tipo.GetMembers(BindingFlags.Instance Or BindingFlags.Public)

        For i = 0 To campos.GetUpperBound(0)
            '.GetFields.GetUpperBound(0)
            member = campos(i)

            If Not member.Name.ToLower = "id" Then 'No hago update del ID, algunas bdd molestan
                Select Case member.MemberType
                    Case MemberTypes.Field
                        guardo = DeboGuardar(member)
                        If guardo = True Then
                            campo = member
                            valor = campo.GetValue(obj)
                            tipodato = campo.FieldType.ToString
                        End If
                    Case MemberTypes.Property
                        guardo = DeboGuardar(member)
                        If guardo = True Then
                            prop = member
                            valor = prop.GetValue(obj, Nothing)
                            tipodato = prop.PropertyType.ToString
                        End If
                    Case Else
                        guardo = False
                End Select

                'si se guerda, aumento el sql dependiendo del tipo
                If guardo = True Then
                    Select Case tipodato
                        Case "System.String"
                            valor = valor & ""
                            txt = "'" & valor.Replace("'", "''") & "'"
                        Case "System.DateTime"

                            Dim ats2 As Object = member.GetCustomAttributes(GetType(ATfechaconHora), False)
                            Dim conhora As Boolean

                            If ats2.Length > 0 Then
                                Dim at2 As ATfechaconHora = ats2(0)
                                If at2.tipo = True Then conhora = True
                            Else
                                conhora = False
                            End If
                            txt = espacio.ManejadorBD.formatofecha(valor, conhora)

                        Case "System.Boolean"
                            If valor = True Then txt = "-1" Else txt = "0"
                        Case Else
                            txt = funciones.FormatoNUMbase(valor)
                    End Select

                    sql.AppendFormat(" {0}={1},", member.Name, txt)

                End If 'guardo

            End If 'campo = id

        Next 'member

        Return sql.ToString.TrimEnd(",")

    End Function

    Public Shared Function sqlInsertCompleto(ByVal obj As Object) As String
        Dim sql As New System.Text.StringBuilder
        Dim i As Short
        Dim tipo As Type = obj.GetType
        Dim campo As FieldInfo
        Dim valor As Object
        Dim txt As String
        Dim guardo As Boolean
        Dim campos() As MemberInfo
        Dim prop As PropertyInfo
        Dim member As MemberInfo
        Dim tipodato As String
        Dim conhora As Boolean
        Dim cmp As String

        Dim sql2 As New System.Text.StringBuilder

        campos = tipo.GetMembers(BindingFlags.Instance Or BindingFlags.Public)

        For i = 0 To campos.GetUpperBound(0)
            '.GetFields.GetUpperBound(0)
            member = campos(i)
            Select Case member.MemberType
                Case MemberTypes.Field
                    guardo = DeboGuardar(member)
                    If guardo = True Then
                        campo = member
                        valor = campo.GetValue(obj)
                        tipodato = campo.FieldType.ToString
                        cmp = campo.Name
                    End If
                Case MemberTypes.Property
                    guardo = DeboGuardar(member)
                    If guardo = True Then
                        prop = member
                        valor = prop.GetValue(obj, Nothing)
                        tipodato = prop.PropertyType.ToString
                        cmp = prop.Name
                    End If
                Case Else
                    guardo = False
            End Select

            'si se guerda, aumento el sql dependiendo del tipo
            If guardo = True Then
                Select Case tipodato
                    Case "System.String"
                        If valor Is Nothing Then valor = ""
                        txt = "'" & valor.Replace("'", "''") & "'"
                    Case "System.DateTime"
                        Dim ats As Object = member.GetCustomAttributes(GetType(ATfechaconHora), False)
                        If ats.Length > 0 Then
                            Dim at As ATfechaconHora = ats(0)
                            If at.tipo = True Then conhora = True
                        Else
                            conhora = False
                        End If
                        txt = espacio.ManejadorBD.formatofecha(valor, conhora)
                    Case "System.Boolean"
                        If valor = True Then txt = "-1" Else txt = "0"
                    Case Else
                        txt = funciones.FormatoNUMbase(valor)
                End Select

                sql.AppendFormat(" {0},", txt)
                sql2.AppendFormat(" {0},", cmp)

            End If
        Next


        Return String.Format("({0}) VALUES ({1}) ", sql2.ToString.TrimEnd(","), sql.ToString.TrimEnd(","))


    End Function
#End Region

    Public Shared Function CargaCol(ByVal sql As String, ByVal tipo As Object) As Collection
        Dim tabla As System.Data.IDataReader
        Dim temp As Object
        Dim col As New Collection

        tabla = espacio.ManejadorBD.traeReader(sql)

        While tabla.Read
            temp = System.Activator.CreateInstance(tipo.GetType)
            BaseAObjeto(tabla, temp)
            'pilas con herencias

            col.Add(temp, CStr(temp.id))
        End While

        tabla.Close()
        tabla = Nothing

        Return col

    End Function

    Public Shared Function CargaAL(ByVal tabla As DataTable, ByVal tipo As Object) As ArrayList
        Dim temp As Object
        Dim col As New ArrayList
        Dim dr As DataRow

        For Each dr In tabla.Rows
            temp = System.Activator.CreateInstance(tipo.GetType)
            BaseAObjeto(dr, temp)

            col.Add(temp)
        Next

        Return col

    End Function


    Public Shared Function CargaAL(ByVal sql As String, ByVal tipo As Object) As ArrayList
        Dim tabla As System.Data.IDataReader
        Dim temp As Object
        Dim col As New ArrayList

        tabla = espacio.ManejadorBD.traeReader(sql)

        While tabla.Read
            temp = System.Activator.CreateInstance(tipo.GetType)
            'otroyo puede ser mas rapido
            BaseAObjeto(tabla, temp)
            'pilas con herencias

            col.Add(temp)
        End While

        tabla.Close()
        tabla = Nothing

        Return col

    End Function

    Private Shared Function DeboLeer(ByVal member As MemberInfo) As Boolean
        Dim at As AtGuardable
        Dim ats As Attribute()

        If member.MemberType = MemberTypes.Field Or member.MemberType = MemberTypes.Property Then

            ats = member.GetCustomAttributes(GetType(AtGuardable), False)
            If ats.Length > 0 Then
                at = CType(ats(0), AtGuardable)
                If at.tipo = lmTipoGuardo.lmNno Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return True
            End If
        Else
            Return False
        End If

    End Function

    Private Shared Function DeboGuardar(ByVal member As MemberInfo) As Boolean
        Dim at As AtGuardable
        Dim ats As Attribute()

        If member.MemberType = MemberTypes.Field Or member.MemberType = MemberTypes.Property Then

            ats = member.GetCustomAttributes(GetType(AtGuardable), False)
            If ats.Length > 0 Then
                at = CType(ats(0), AtGuardable)
                If at.tipo = lmTipoGuardo.lmNno Or at.tipo = lmTipoGuardo.lmsoloLeo Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return True
            End If
        Else
            Return False
        End If

    End Function

#Region "WINforms"

    Public Shared Sub FormaAobjeto(ByVal cons As Control.ControlCollection, ByVal obj As Object)
        Dim tempcon As Control
        Dim i As String
        Dim prop As PropertyInfo
        Dim member As MemberInfo
        Dim campo As FieldInfo
        Dim tipo As Type = obj.GetType
        Dim valor As Object

        For Each tempcon In cons
            i = tempcon.Tag
            If Not i = "" Then
                'para buscar formatos y no hacerles caso
                If i.IndexOf("@") > -1 Then i = Split(i, "@")(0)

                If TypeOf tempcon Is TextBox Then
                    valor = tempcon.Text
                End If
                If TypeOf tempcon Is ComboBox Then
                    Dim temp As ComboBox = tempcon
                    If temp.DropDownStyle = ComboBoxStyle.DropDownList Then
                        If Not temp.SelectedItem Is Nothing Then
                            valor = temp.SelectedItem.id
                        Else
                            valor = 0
                        End If
                    Else
                        valor = tempcon.Text
                    End If
                End If

                If TypeOf tempcon Is DateTimePicker Then
                    Dim temp As DateTimePicker = tempcon
                    If temp.ShowCheckBox = True Then
                        If temp.Checked = True Then
                            valor = temp.Value
                        Else
                            valor = CDate("12:00 am")
                        End If
                    Else
                        valor = temp.Value
                    End If
                End If

                If TypeOf tempcon Is CheckBox Then
                    Dim temp As CheckBox = tempcon
                    If temp.Checked = True Then
                        valor = -1
                    Else
                        valor = 0
                    End If
                End If

                Try
                    member = tipo.GetMember(i)(0)
                    Select Case member.MemberType
                        Case MemberTypes.Field
                            campo = member
                            campo.SetValue(obj, Convert.ChangeType(valor, campo.FieldType))
                        Case MemberTypes.Property
                            prop = member
                            prop.SetValue(obj, Convert.ChangeType(valor, prop.PropertyType), Nothing)
                    End Select
                Catch ex As Exception

                End Try

            End If
        Next

    End Sub

    Public Shared Sub ObjetoAForma(ByVal cons As Control.ControlCollection, ByVal obj As Object)
        Dim tempcon As Control
        Dim i, i2 As String
        Dim prop As PropertyInfo
        Dim member As MemberInfo
        Dim campo As FieldInfo
        Dim tipo As Type = obj.GetType
        Dim valor As Object
        Dim formato As String

        For Each tempcon In cons
            i2 = tempcon.Tag

            If Not i2 = "" Then
                'para buscar formatos
                If i2.IndexOf("@") > -1 Then
                    i = Split(i2, "@")(0)
                    formato = Split(i2, "@")(1)
                Else
                    i = i2
                    formato = ""
                End If


                Try
                    member = tipo.GetMember(i)(0)
                    Select Case member.MemberType
                        Case MemberTypes.Field
                            campo = member
                            valor = campo.GetValue(obj)
                        Case MemberTypes.Property
                            prop = member
                            valor = prop.GetValue(obj, Nothing)
                    End Select
                Catch ex As Exception

                End Try


                If TypeOf tempcon Is TextBox Then
                    If IsNumeric(valor) And formato <> "" Then
                        tempcon.Text = CType(valor, Double).ToString(formato)
                        'REVISAR EFECTOS DE ESE DOBLE, pero ojo que solo molesta si necesito formato
                    Else
                        tempcon.Text = valor
                    End If
                End If
                If TypeOf tempcon Is ComboBox Then
                    'parche para los codigos letras = if 

                    Dim temp As ComboBox = tempcon
                    If temp.DropDownStyle = ComboBoxStyle.DropDownList Then

                        If IsNumeric(valor) Then
                            win.generales.seleccionacombo(temp, valor)
                        Else
                            win.generales.seleccionacomboCod(temp, valor)
                        End If


                    Else
                        tempcon.Text = valor
                    End If
                End If

                If TypeOf tempcon Is DateTimePicker Then
                    Dim temp As DateTimePicker = tempcon
                    If Not valor = CDate("12:00 AM") Then
                        If temp.ShowCheckBox = True Then temp.Checked = True
                        temp.Value = valor
                    Else
                        If temp.ShowCheckBox = True Then temp.Checked = False
                    End If
                End If

                If TypeOf tempcon Is CheckBox Then
                    Dim temp As CheckBox = tempcon
                    If valor = True Then
                        temp.Checked = True
                    Else
                        temp.Checked = False
                    End If
                End If
            End If 'i2=""
        Next

    End Sub

    Public Shared Sub ObjetoAFormaConFunciones(ByVal cons As Control.ControlCollection, ByVal obj As Object)
        Dim tempcon As Control
        Dim i, i2 As String
        Dim prop As PropertyInfo
        Dim member As MemberInfo
        Dim campo As FieldInfo
        Dim met As MethodInfo
        Dim tipo As Type = obj.GetType
        Dim valor As Object
        Dim formato As String

        For Each tempcon In cons
            i2 = tempcon.Tag

            If Not i2 = "" Then
                'para buscar formatos
                If i2.IndexOf("@") > -1 Then
                    i = Split(i2, "@")(0)
                    formato = Split(i2, "@")(1)
                Else
                    i = i2
                    formato = ""
                End If


                Try
                    member = tipo.GetMember(i)(0)
                    Select Case member.MemberType
                        Case MemberTypes.Field
                            campo = member
                            valor = campo.GetValue(obj)
                        Case MemberTypes.Property
                            prop = member
                            valor = prop.GetValue(obj, Nothing)
                        Case MemberTypes.Method
                            met = member
                            valor = met.Invoke(obj, Nothing)
                    End Select
                Catch ex As Exception

                End Try


                If TypeOf tempcon Is TextBox Then
                    If IsNumeric(valor) And formato <> "" Then
                        tempcon.Text = CType(valor, Double).ToString(formato)
                        'REVISAR EFECTOS DE ESE DOBLE, pero ojo que solo molesta si necesito formato
                    Else
                        tempcon.Text = valor
                    End If
                End If
                If TypeOf tempcon Is ComboBox Then
                    'parche para los codigos letras = if 

                    Dim temp As ComboBox = tempcon
                    If temp.DropDownStyle = ComboBoxStyle.DropDownList Then

                        If IsNumeric(valor) Then
                            win.generales.seleccionacombo(temp, valor)
                        Else
                            win.generales.seleccionacomboCod(temp, valor)
                        End If


                    Else
                        tempcon.Text = valor
                    End If
                End If

                If TypeOf tempcon Is DateTimePicker Then
                    Dim temp As DateTimePicker = tempcon
                    If Not valor = CDate("12:00 AM") Then
                        If temp.ShowCheckBox = True Then temp.Checked = True
                        temp.Value = valor
                    Else
                        If temp.ShowCheckBox = True Then temp.Checked = False
                    End If
                End If

                If TypeOf tempcon Is CheckBox Then
                    Dim temp As CheckBox = tempcon
                    If valor = True Then
                        temp.Checked = True
                    Else
                        temp.Checked = False
                    End If
                End If
            End If 'i2=""
        Next

    End Sub

#End Region

End Class
