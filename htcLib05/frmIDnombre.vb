Public Class frmIDnombre
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
    Friend WithEvents Lista As System.Windows.Forms.ListBox
    Friend WithEvents cmdNuevo As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Lista = New System.Windows.Forms.ListBox
        Me.cmdNuevo = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Lista
        '
        Me.Lista.Location = New System.Drawing.Point(12, 12)
        Me.Lista.Name = "Lista"
        Me.Lista.Size = New System.Drawing.Size(240, 355)
        Me.Lista.TabIndex = 0
        '
        'cmdNuevo
        '
        Me.cmdNuevo.Location = New System.Drawing.Point(160, 380)
        Me.cmdNuevo.Name = "cmdNuevo"
        Me.cmdNuevo.Size = New System.Drawing.Size(92, 24)
        Me.cmdNuevo.TabIndex = 1
        Me.cmdNuevo.Text = "Nuevo"
        '
        'frmIDnombre
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(260, 418)
        Me.Controls.Add(Me.cmdNuevo)
        Me.Controls.Add(Me.Lista)
        Me.MaximizeBox = False
        Me.Name = "frmIDnombre"
        Me.Text = "Datos"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public al As ArrayList
    Public tabla As String
    Public nuevo As [Delegate]
    Public edita As [Delegate]

    Private Sub frmIDnombre_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Lista.DataSource = Me.al
    End Sub

    Sub nuevoStandar()
        Dim nom As String = InputBox("Ingrese el nombre del nuevo elemento")

        If Not nom.Trim = "" Then
            Dim idn As New IdNombre(0, nom)
            idn.guardar(Me.tabla)
            al.Add(idn)
        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNuevo.Click

        If Me.Nuevo Is Nothing Then
            Me.nuevoStandar()
        Else
            nuevo.DynamicInvoke(Nothing)
        End If

        Me.Lista.DataSource = Nothing
        Me.Lista.DataSource = Me.al

    End Sub

    Sub editaEstandar()
        Dim obj As IdNombre = Me.al(Lista.SelectedIndices(0))

        Dim nom As String = InputBox("Ingrese el nuevo nombre del nuevo elemento", "DATOS", obj.nombre)

        If Not nom.Trim = "" Then
            obj.nombre = nom
            obj.guardar(Me.tabla)
        End If
    End Sub

    Private Sub Lista_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lista.DoubleClick

        If Not Me.Lista.SelectedItem Is Nothing Then
            If Me.edita Is Nothing Then
                editaEstandar()
            Else
                Dim ob As Object = Me.al(Lista.SelectedIndices(0))
                Dim args(0) As Object
                args(0) = ob
                Me.edita.DynamicInvoke(args)
            End If

        End If
        Me.Lista.DataSource = Nothing
        Me.Lista.DataSource = Me.al

    End Sub

    Private Sub Lista_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Lista.KeyPress
        If e.KeyChar = ControlChars.Back Then
            'Windows.Forms.MessageBox.Show("No se puede eliminar")
        End If
    End Sub
End Class
