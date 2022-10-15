Imports System.Drawing
Imports System.Drawing.Graphics
Imports System.IO

Public Class imagenes

    Public Shared Function del() As Boolean
        Return False
    End Function
    Public Shared Sub GuardaNuevoTamaño(ByVal RutaOriginal As String, ByVal rutaDestino As String, ByVal borrarOriginal As Boolean, Optional ByVal alto As Decimal = -1, Optional ByVal ancho As Decimal = -1)

        GuardaNuevoTamaño(Bitmap.FromFile(RutaOriginal), rutaDestino, alto, ancho)

        If borrarOriginal = True Then
            If IO.File.Exists(RutaOriginal) Then IO.File.Delete(RutaOriginal)
        End If

    End Sub

    Public Shared Sub GuardaNuevoTamaño(ByVal Original As Stream, ByVal rutaDestino As String, Optional ByVal alto As Decimal = -1, Optional ByVal ancho As Decimal = -1)

        GuardaNuevoTamaño(Bitmap.FromStream(Original), rutaDestino, alto, ancho)

    End Sub

    Public Shared Sub GuardaNuevoTamaño(ByVal imagen As Bitmap, ByVal rutaDestino As String, Optional ByVal altoMaximo As Decimal = -1, Optional ByVal anchoMaximo As Decimal = -1)

        Dim sz As System.Drawing.Size
        Dim rel As Decimal

        rel = imagen.Size.Width / imagen.Size.Height

        'por si acaso no hay nada en config
        If altoMaximo = 0 Then altoMaximo = -1
        If anchoMaximo = 0 Then anchoMaximo = -1

        'borro si existia algo ahi
        If IO.File.Exists(rutaDestino) Then IO.File.Delete(rutaDestino)

        If anchoMaximo > imagen.Width Then anchoMaximo = imagen.Width
        If altoMaximo > imagen.Height Then altoMaximo = imagen.Height

        Select Case True
            Case altoMaximo = -1 And anchoMaximo > 0
                sz = New System.Drawing.Size(anchoMaximo, anchoMaximo / rel)
            Case anchoMaximo = -1 And altoMaximo > 0
                sz = New System.Drawing.Size(altoMaximo * rel, altoMaximo)
            Case altoMaximo > 0 And anchoMaximo > 0
                'le cambio el tamaño a la mas larga, para conservar el ratio.
                If imagen.Width > imagen.Height Then
                    sz = New System.Drawing.Size(anchoMaximo, anchoMaximo / rel)
                Else
                    sz = New System.Drawing.Size(altoMaximo * rel, altoMaximo)
                End If

            Case Else 'la dejo como esta
                sz = New System.Drawing.Size(imagen.Size.Width, imagen.Size.Height)
        End Select
        'End If

        Dim myBit As Bitmap = New Bitmap(imagen) ', sz)

        'para probar alternativas
        myBit.GetThumbnailImage(sz.Width, sz.Height, New System.Drawing.Image.GetThumbnailImageAbort(AddressOf del), System.IntPtr.Zero).Save(rutaDestino, System.Drawing.Imaging.ImageFormat.Jpeg)

        'myBit.SetResolution(72, 72)

        'myBit.Save(rutaDestino, System.Drawing.Imaging.ImageFormat.Jpeg)

        'myBit.Save(rutaDestino)

        myBit.Dispose()
        imagen.Dispose()

    End Sub
End Class
