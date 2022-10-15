Namespace win

    Public Class pantallas


        Public Shared Sub AdministraAl(ByVal lista As ArrayList, ByVal tabla As String)
            Dim f As New frmIDnombre
            f.al = lista
            f.tabla = tabla
            f.ShowDialog()
        End Sub

        Public Shared Function buscaItemDeCol(ByVal col As ArrayList, Optional ByVal titulo As String = "Seleccion", Optional ByVal texto As String = "") As Object
            Dim f As New frmBuscaItemdeCol
            Dim o As Object

            f.col = col
            f.Text = titulo
            f.lbltexto.Text = texto
            f.ShowDialog()

            o = f.itemsel
            f.Close()

            Return o

        End Function


        Public Shared Function Buscafecha(Optional ByVal titulo As String = "", Optional ByVal texto As String = "", Optional ByVal def As Date = #1/1/1900#, Optional ByVal min As Date = #1/1/1900#, Optional ByVal max As Date = #1/1/2100#) As Date

            If def = #1/1/1900# Then def = DateTime.Now


            Dim f As New frmfecha

            If Not titulo = "" Then f.Text = titulo
            f.lbltexto.Text = texto

            f.fecha = def
            With f.calendario
                .MinDate = min
                .MaxDate = max
                .SetDate(def)
            End With

            f.ShowDialog()

            If f.cancelo = True Then
                Return #1/1/1900#
            Else
                Return f.fecha
            End If

            f.Close()


        End Function

    End Class

End Namespace
