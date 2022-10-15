Public Class frmBuscaItemdeCol
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
    Friend WithEvents cmbCol As System.Windows.Forms.ComboBox
    Friend WithEvents cmdAcepta As System.Windows.Forms.Button
    Friend WithEvents lbltexto As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.cmbCol = New System.Windows.Forms.ComboBox
        Me.cmdAcepta = New System.Windows.Forms.Button
        Me.lbltexto = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmbCol
        '
        Me.cmbCol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCol.Location = New System.Drawing.Point(8, 56)
        Me.cmbCol.Name = "cmbCol"
        Me.cmbCol.Size = New System.Drawing.Size(324, 21)
        Me.cmbCol.TabIndex = 0
        '
        'cmdAcepta
        '
        Me.cmdAcepta.Location = New System.Drawing.Point(236, 84)
        Me.cmdAcepta.Name = "cmdAcepta"
        Me.cmdAcepta.Size = New System.Drawing.Size(92, 24)
        Me.cmdAcepta.TabIndex = 1
        Me.cmdAcepta.Text = "Aceptar"
        '
        'lbltexto
        '
        Me.lbltexto.Location = New System.Drawing.Point(12, 8)
        Me.lbltexto.Name = "lbltexto"
        Me.lbltexto.Size = New System.Drawing.Size(312, 40)
        Me.lbltexto.TabIndex = 2
        '
        'frmBuscaItemdeCol
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.ClientSize = New System.Drawing.Size(340, 127)
        Me.Controls.Add(Me.lbltexto)
        Me.Controls.Add(Me.cmdAcepta)
        Me.Controls.Add(Me.cmbCol)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmBuscaItemdeCol"
        Me.Text = "Seleccion"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public col As ArrayList
    Public itemsel As Object

    Private Sub frmBuscaItemdeCol_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.cmbCol.DataSource = col

    End Sub

    Private Sub cmdAcepta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAcepta.Click
        If Not Me.cmbCol.SelectedItem Is Nothing Then
            Me.itemsel = Me.cmbCol.SelectedItem
            Me.Hide()
        End If

    End Sub
End Class
