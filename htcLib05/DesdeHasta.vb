Public Class DesdeHasta
    Public Desde As Date
    Public Hasta As Date

    Public Function getPeriodo(Optional ByVal titulo As String = "", Optional ByVal texto As String = "", Optional ByVal min As Date = #1/1/1900#, Optional ByVal max As Date = #1/1/2100#) As Date
        Dim f As New FrmDesdeHasta

        If titulo <> "" Then f.Text = titulo
        If texto <> "" Then f.Lbl_Texto.Text = texto
        If min <> #1/1/1900# Then f.dtDesde.MinDate = min
        If max <> #1/1/1900# Then f.dtHasta.MaxDate = max

        f.ShowDialog()
        Me.Desde = f.desde
        Me.Hasta = f.hasta

    End Function

End Class

