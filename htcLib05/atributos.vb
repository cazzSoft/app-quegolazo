
Public Enum lmTipoGuardo
    lmNno = 0
    lmsoloLeo = 1
    lmLeoyGuardo = 2
End Enum

<AttributeUsage(AttributeTargets.All)> _
Public Class AtGuardable
    Inherits Attribute

    ' The constructor is called when the attribute is set.
    Public Sub New(ByVal tipo As lmTipoGuardo)
        Me.tipo = tipo
    End Sub

    ' Keep a variable internally ...
    Public tipo As lmTipoGuardo

End Class

<AttributeUsage(AttributeTargets.All)> _
Public Class ATfechaconHora
    Inherits Attribute

    ' The constructor is called when the attribute is set.
    Public Sub New(ByVal tipo As Boolean)
        Me.tipo = tipo
    End Sub

    ' Keep a variable internally ...
    Public tipo As Boolean

End Class


