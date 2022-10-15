Public Class frmfecha
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents calendario As System.Windows.Forms.MonthCalendar
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdcancel As System.Windows.Forms.Button
    Friend WithEvents lbltexto As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.calendario = New System.Windows.Forms.MonthCalendar
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdcancel = New System.Windows.Forms.Button
        Me.lbltexto = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'calendario
        '
        Me.calendario.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.calendario.Location = New System.Drawing.Point(8, 64)
        Me.calendario.Name = "calendario"
        Me.calendario.TabIndex = 0
        '
        'cmdOK
        '
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Location = New System.Drawing.Point(232, 68)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "Aceptar"
        '
        'cmdcancel
        '
        Me.cmdcancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdcancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdcancel.Location = New System.Drawing.Point(232, 100)
        Me.cmdcancel.Name = "cmdcancel"
        Me.cmdcancel.TabIndex = 2
        Me.cmdcancel.Text = "Cancelar"
        '
        'lbltexto
        '
        Me.lbltexto.Location = New System.Drawing.Point(12, 8)
        Me.lbltexto.Name = "lbltexto"
        Me.lbltexto.Size = New System.Drawing.Size(292, 44)
        Me.lbltexto.TabIndex = 3
        Me.lbltexto.Text = "Label1"
        '
        'frmfecha
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.CancelButton = Me.cmdcancel
        Me.ClientSize = New System.Drawing.Size(314, 236)
        Me.Controls.Add(Me.lbltexto)
        Me.Controls.Add(Me.cmdcancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.calendario)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmfecha"
        Me.Text = "Fecha"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public cancelo As Boolean
    Public fecha As Date

    Private Sub frmfecha_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub cmdcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdcancel.Click
        cancelo = True
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Me.Hide()
    End Sub


    Private Sub calendario_DateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles calendario.DateChanged
        Me.fecha = e.Start
    End Sub
End Class
