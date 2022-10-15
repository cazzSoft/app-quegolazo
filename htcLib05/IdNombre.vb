Public Class IdNombre
    Public id As Long
    Public nombre As String
    <AtGuardable(lmTipoGuardo.lmNno)> Public mitabla As String

    <AtGuardable(lmTipoGuardo.lmNno)> Public ReadOnly Property idProp() As Long
        Get
            Return Me.id
        End Get
    End Property

    <AtGuardable(lmTipoGuardo.lmNno)> Public ReadOnly Property nomProp() As String
        Get
            Return Me.nombre
        End Get
    End Property

    Public Overrides Function tostring() As String
        Return Me.nombre
    End Function

    Public Sub New(ByVal id1 As Long, ByVal nom As String)
        Me.id = id1
        Me.nombre = nom
    End Sub

    Public Sub New()
    End Sub


    Public Sub guardar(ByVal tabla As String)
        Me.mitabla = tabla
        LM.guardaobjeto(Me)
    End Sub

End Class

