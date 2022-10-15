Public Class frmFormatos
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
    Friend WithEvents lista As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.lista = New System.Windows.Forms.ListBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lista
        '
        Me.lista.Location = New System.Drawing.Point(12, 88)
        Me.lista.Name = "lista"
        Me.lista.Size = New System.Drawing.Size(220, 303)
        Me.lista.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(228, 60)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Seleccione en que formato desea imprimir. Para no imprimir, cierre la ventana  o " & _
        "presione [ESC]"
        '
        'frmFormatos
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.ClientSize = New System.Drawing.Size(256, 413)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lista)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmFormatos"
        Me.Text = "Formatos"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public dirsel As String

    Private Sub frmFormatos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub


    Private Sub lista_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lista.DoubleClick
        selecciona()
    End Sub

    Sub selecciona()
        Dim dt As DataTable = Me.lista.DataSource
        Me.dirsel = dt.Rows(Me.lista.SelectedIndex).Item("archivo")
        Me.Hide()
    End Sub

    Private Sub lista_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lista.KeyPress
        If e.KeyChar = ControlChars.Cr Or e.KeyChar = ControlChars.CrLf Then
            selecciona()
        End If

    End Sub

    Private Sub lista_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lista.SelectedIndexChanged

    End Sub

    Private Sub lista_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lista.KeyDown
        If e.KeyCode = System.Windows.Forms.Keys.Escape Then
            Me.dirsel = ""
            Me.Hide()
        End If
    End Sub
End Class
