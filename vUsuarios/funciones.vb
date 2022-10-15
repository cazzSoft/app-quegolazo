Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Reflection.Assembly

Public Module funciones

    ''' <summary>
    ''' Encripta una cadena ingresando una clave y el tipo de descripcion
    ''' </summary>
    ''' <param name="InputString"></param>
    ''' <param name="SecretKey"></param>
    ''' <param name="CyphMode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EncryptString(ByVal InputString As String, ByVal SecretKey As String, Optional ByVal CyphMode As CipherMode = CipherMode.ECB) As String
        Try
            Dim Des As New TripleDESCryptoServiceProvider
            'Put the string into a byte array
            Dim InputbyteArray() As Byte = System.Text.Encoding.UTF8.GetBytes(InputString)
            'Create the crypto objects, with the key, as passed in
            Dim hashMD5 As New MD5CryptoServiceProvider
            Des.Key = hashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(SecretKey))
            Des.Mode = CyphMode
            Dim ms As System.IO.MemoryStream = New MemoryStream
            Dim cs As CryptoStream = New CryptoStream(ms, Des.CreateEncryptor(), _
            CryptoStreamMode.Write)
            'Write the byte array into the crypto stream
            '(It will end up in the memory stream)
            cs.Write(InputbyteArray, 0, InputbyteArray.Length)
            cs.FlushFinalBlock()
            'Get the data back from the memory stream, and into a string
            Dim ret As StringBuilder = New StringBuilder
            Dim b() As Byte = ms.ToArray
            ms.Close()
            Dim I As Integer
            For I = 0 To UBound(b)
                'Format as hex
                ret.AppendFormat("{0:X2}", b(I))
            Next

            Return ret.ToString()
        Catch ex As System.Security.Cryptography.CryptographicException
            Return ""
        End Try

    End Function

    ''' <summary>
    ''' Desencripta la cadena ingresada deacuerdo a la clave y al tipo de cifrado
    ''' </summary>
    ''' <param name="InputString"></param>
    ''' <param name="SecretKey"></param>
    ''' <param name="CyphMode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DecryptString(ByVal InputString As String, ByVal SecretKey As String, Optional ByVal CyphMode As CipherMode = CipherMode.ECB) As String
        If InputString = String.Empty Then
            Return ""
        Else
            Dim Des As New TripleDESCryptoServiceProvider
            'Put the string into a byte array
            Dim InputbyteArray(CType(InputString.Length / 2 - 1, Integer)) As Byte '= Encoding.UTF8.GetBytes(InputString)
            'Create the crypto objects, with the key, as passed in
            Dim hashMD5 As New MD5CryptoServiceProvider

            Des.Key = hashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(SecretKey))
            Des.Mode = CyphMode
            'Put the input string into the byte array

            Dim X As Integer

            For X = 0 To InputbyteArray.Length - 1
                Dim IJ As Int32 = (Convert.ToInt32(InputString.Substring(X * 2, 2), 16))
                Dim BT As New System.ComponentModel.ByteConverter
                InputbyteArray(X) = New Byte
                InputbyteArray(X) = CType(BT.ConvertTo(IJ, GetType(Byte)), Byte)
            Next

            Dim ms As MemoryStream = New MemoryStream
            Dim cs As CryptoStream = New CryptoStream(ms, Des.CreateDecryptor(), _
            CryptoStreamMode.Write)

            'Flush the data through the crypto stream into the memory stream
            cs.Write(InputbyteArray, 0, InputbyteArray.Length)
            cs.FlushFinalBlock()

            '//Get the decrypted data back from the memory stream
            Dim ret As StringBuilder = New StringBuilder
            Dim B() As Byte = ms.ToArray

            ms.Close()

            Dim I As Integer

            For I = 0 To UBound(B)
                ret.Append(Chr(B(I)))
            Next

            Return ret.ToString()
        End If
    End Function

    ''' <summary>
    ''' Retorna la configuracion de usrConfig.xml para la configuracion de usuarios
    ''' </summary>
    ''' <param name="key"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function getConfiguracion(ByVal key As String) As Object
        Dim documento As XmlDocument
        Dim valor As Object
        Dim configuraciones As XmlNodeList
        Dim nodo As XmlNode
        Dim localizacion As String
        'localizacion = IO.Path.GetDirectoryName(My)

        localizacion = System.AppDomain.CurrentDomain.BaseDirectory
        localizacion = String.Format("{0}{1}\{2}", localizacion, "bin", "usrConfig.xml")
        documento = New XmlDocument
        documento.Load(localizacion)

        configuraciones = documento.GetElementsByTagName("parametro") 'obtenemos todos los worksheet
        valor = Nothing
        For Each nodo In configuraciones
            If nodo.Attributes("key").Value = key Then 'si existe con este atributo
                valor = nodo.Attributes("valor").Value
                Exit For
            End If
        Next

        Return valor
    End Function


    Friend Function getPersonasUsuarios() As DataTable
        Dim sql As String
        Dim informacion As DataTable
        sql = "SELECT MIS_personas.id, MIS_personas.nombre1+ ' ' + MIS_personas.apellido1 as persona, USR_usuarios.fechaCreacion, USR_usuarios.activo FROM MIS_personas INNER JOIN USR_usuarios ON MIS_personas.id = USR_usuarios.idPersona; "
        informacion = htcLib.espacio.ManejadorBD.traetabla(sql)
        Return informacion
    End Function


    Friend Function getPermisos() As DataTable
        Dim sql As String
        Dim informacion As DataTable
        sql = "SELECT * FROM USR_Permisos"
        informacion = htcLib.espacio.ManejadorBD.traetabla(sql)
        Return informacion
    End Function

    Friend Function CBoolData(ByVal dato As Boolean) As Integer
        Select Case dato
            Case True
                CBoolData = 1
            Case False
                CBoolData = 0
        End Select
    End Function

    Friend Sub llenaTree(ByRef arbol As TreeView, ByVal info As DataTable, ByVal usr As usuario)
        Dim miNodo As TreeNode

        For Each r As DataRow In info.Rows
            miNodo = New TreeNode(r("Nombre"), r("id"))
            miNodo.Expanded = False
            miNodo.Selected = False
            miNodo.Checked = usr.tengoPermiso(r("id"))
            arbol.Nodes.Add(miNodo)
        Next
    End Sub

    Friend Function getTree(ByRef arbol As TreeView) As ArrayList
        Dim miNodo As TreeNode
        Dim retorno As ArrayList

        retorno = New ArrayList
        For Each miNodo In arbol.Nodes
            If miNodo.Checked Then
                retorno.Add(New htcLib.idNombre(miNodo.Value, miNodo.Text))
            End If
        Next
        Return retorno
    End Function

End Module
