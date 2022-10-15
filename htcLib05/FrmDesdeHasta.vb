Public Class FrmDesdeHasta
    Inherits System.Windows.Forms.Form

#Region " Código generado por el Diseñador de Windows Forms "

    Public Sub New()
        MyBase.New()

        'El Diseñador de Windows Forms requiere esta llamada.
        InitializeComponent()

        'Agregar cualquier inicialización después de la llamada a InitializeComponent()

    End Sub

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
    'Puede modificarse utilizando el Diseñador de Windows Forms. 
    'No lo modifique con el editor de código.
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboMes As System.Windows.Forms.ComboBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents RButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents RBMes As System.Windows.Forms.RadioButton
    Friend WithEvents RBDesde As System.Windows.Forms.RadioButton
    Friend WithEvents dtHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblDelAno As System.Windows.Forms.Label
    Friend WithEvents Lbl_Texto As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Label2 = New System.Windows.Forms.Label
        Me.dtHasta = New System.Windows.Forms.DateTimePicker
        Me.dtDesde = New System.Windows.Forms.DateTimePicker
        Me.ComboMes = New System.Windows.Forms.ComboBox
        Me.RButton3 = New System.Windows.Forms.RadioButton
        Me.RBMes = New System.Windows.Forms.RadioButton
        Me.RBDesde = New System.Windows.Forms.RadioButton
        Me.Button2 = New System.Windows.Forms.Button
        Me.lblDelAno = New System.Windows.Forms.Label
        Me.Lbl_Texto = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.Enabled = False
        Me.Label2.Location = New System.Drawing.Point(208, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 24)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Hasta :"
        '
        'dtHasta
        '
        Me.dtHasta.CustomFormat = "dd-MMM-yy"
        Me.dtHasta.Enabled = False
        Me.dtHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtHasta.Location = New System.Drawing.Point(264, 56)
        Me.dtHasta.Name = "dtHasta"
        Me.dtHasta.Size = New System.Drawing.Size(128, 21)
        Me.dtHasta.TabIndex = 24
        '
        'dtDesde
        '
        Me.dtDesde.CustomFormat = "dd-MMM-yy"
        Me.dtDesde.Enabled = False
        Me.dtDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtDesde.Location = New System.Drawing.Point(80, 56)
        Me.dtDesde.Name = "dtDesde"
        Me.dtDesde.Size = New System.Drawing.Size(128, 21)
        Me.dtDesde.TabIndex = 25
        '
        'ComboMes
        '
        Me.ComboMes.Enabled = False
        Me.ComboMes.Location = New System.Drawing.Point(80, 104)
        Me.ComboMes.Name = "ComboMes"
        Me.ComboMes.Size = New System.Drawing.Size(128, 21)
        Me.ComboMes.TabIndex = 29
        Me.ComboMes.Text = "- Seleccione -"
        '
        'RButton3
        '
        Me.RButton3.Location = New System.Drawing.Point(16, 152)
        Me.RButton3.Name = "RButton3"
        Me.RButton3.Size = New System.Drawing.Size(136, 24)
        Me.RButton3.TabIndex = 3
        Me.RButton3.Text = "Lo que va del año:"
        '
        'RBMes
        '
        Me.RBMes.Location = New System.Drawing.Point(16, 104)
        Me.RBMes.Name = "RBMes"
        Me.RBMes.Size = New System.Drawing.Size(56, 24)
        Me.RBMes.TabIndex = 2
        Me.RBMes.Text = "Mes :"
        '
        'RBDesde
        '
        Me.RBDesde.Location = New System.Drawing.Point(16, 56)
        Me.RBDesde.Name = "RBDesde"
        Me.RBDesde.Size = New System.Drawing.Size(72, 24)
        Me.RBDesde.TabIndex = 1
        Me.RBDesde.Text = "Desde :"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(304, 184)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(88, 23)
        Me.Button2.TabIndex = 35
        Me.Button2.Text = "Aceptar"
        '
        'lblDelAno
        '
        Me.lblDelAno.Location = New System.Drawing.Point(144, 156)
        Me.lblDelAno.Name = "lblDelAno"
        Me.lblDelAno.Size = New System.Drawing.Size(248, 23)
        Me.lblDelAno.TabIndex = 36
        Me.lblDelAno.Text = "Label1"
        '
        'Lbl_Texto
        '
        Me.Lbl_Texto.Location = New System.Drawing.Point(16, 16)
        Me.Lbl_Texto.Name = "Lbl_Texto"
        Me.Lbl_Texto.Size = New System.Drawing.Size(368, 23)
        Me.Lbl_Texto.TabIndex = 37
        Me.Lbl_Texto.Text = "Seleccione el periodo de tiempo"
        '
        'FrmDesdeHasta
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.ClientSize = New System.Drawing.Size(400, 213)
        Me.Controls.Add(Me.Lbl_Texto)
        Me.Controls.Add(Me.lblDelAno)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.dtDesde)
        Me.Controls.Add(Me.RBDesde)
        Me.Controls.Add(Me.RBMes)
        Me.Controls.Add(Me.RButton3)
        Me.Controls.Add(Me.ComboMes)
        Me.Controls.Add(Me.dtHasta)
        Me.Controls.Add(Me.Label2)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FrmDesdeHasta"
        Me.Text = "Periodo"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Dim RBVal As Byte = 1
    Public min, max As Date
    Public desde, hasta As Date

    Private Sub FrmDesdeHasta_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim i As Byte
        Me.desde = DateSerial(Today.Year, 1, 1)
        Me.hasta = Now

        For i = 1 To 12
            Me.ComboMes.Items.Add(DateSerial(desde.Year, i, 1).ToString("MMMM"))
        Next
        Me.lblDelAno.Text = (Me.desde.ToString("yyyy/MM/dd") & " - " & Me.hasta.ToString("yyyy/MM/dd"))

    End Sub

    Private Sub RBDesde_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RBDesde.CheckedChanged
        opcionesRB(True)
    End Sub
    Sub opcionesRB(ByVal ok As Boolean)

        Me.dtDesde.Enabled = ok
        Me.dtHasta.Enabled = ok
        Me.ComboMes.Enabled = Not ok

    End Sub

    Private Sub RBMes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RBMes.CheckedChanged
        opcionesRB(False)
    End Sub

    Private Sub RButton3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RButton3.CheckedChanged
        opcionesRB(False)
        Me.ComboMes.Enabled = False
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim mes As Byte

        mes = Me.ComboMes.SelectedIndex + 1

        If Me.RBDesde.Checked Then
            desde = Me.dtDesde.Text
            hasta = Me.dtHasta.Text
        End If
        If Me.RBMes.Checked Then
            desde = DateSerial(Today.Year, mes, 1)
            hasta = DateSerial(Today.Year, mes, numdias(mes, Today.Year))
        End If
        If Me.RButton3.Checked Then
            desde = DateSerial(Today.Year, 1, 1)
            hasta = Today
        End If

        Me.Close()
    End Sub
    Public Function numdias(ByVal mes As Byte, ByVal ano As Integer) As Integer
        Dim dias As Byte

        Select Case mes
            Case 1, 3, 5, 7, 8, 10, 12
                dias = 31
            Case 4, 6, 9, 11
                dias = 30
            Case 2
                If ano Mod 4 = 0 Then dias = 29 Else dias = 28
        End Select

        numdias = dias

        Return numdias

    End Function

End Class
