Imports System.Windows.Forms

Namespace win

    Public Class generales

        Public Shared Function soloNumeros(ByRef e As System.Windows.Forms.KeyPressEventArgs, Optional ByVal aceptaNegativos As Boolean = True) As String
            'If num = "," Or num = "." Then Return puntoDecimal()
            If Not Char.IsControl(e.KeyChar) Then
                e.Handled = True
                Dim num As String = e.KeyChar
                Select Case True
                    Case Char.IsLetter(num) : Return ""
                    Case num = "." Or num = "," : Return funciones.puntoDecimal()
                    Case aceptaNegativos = False And num = "-" : Return ""
                    Case Else : Return num
                End Select
            Else
                Return ""
            End If

        End Function

        Public Shared Sub EnableGridInsert(ByVal grd As System.Windows.Forms.DataGrid, ByVal allowNew As Boolean)
            Dim bm As System.Windows.Forms.BindingManagerBase = grd.BindingContext(grd.DataSource, grd.DataMember)
            Dim cm As System.Windows.Forms.CurrencyManager = CType(bm, System.Windows.Forms.CurrencyManager)
            Dim drv As DataView = CType(cm.List, DataView)
            drv.AllowNew = allowNew
        End Sub

        Public Shared Sub seleccionacomboCod(ByVal temp As ComboBox, ByVal valor As String)
            Dim i As Object

            For Each i In temp.Items
                If i.id = valor Then
                    temp.SelectedItem = i
                    Exit For
                End If
            Next

        End Sub

        Public Shared Sub seleccionacombo(ByVal temp As ComboBox, ByVal valor As Long)
            Dim i As Object

            For Each i In temp.Items
                If i.id = valor Then
                    temp.SelectedItem = i
                    'temp.SelectedValue = i
                    Exit For
                End If
            Next

        End Sub

    End Class


End Namespace
