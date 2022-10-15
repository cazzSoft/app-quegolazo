Public Interface Idatos

    Property constr() As String

    Sub inicializa(ByVal constr As String)

    Function secuencial(ByVal nomtabla As String) As Long

    Function formatofecha(ByVal fecha As DateTime, Optional ByVal conHora As Boolean = False, Optional ByVal signo As String = "#", Optional ByVal sep As String = "/") As String

    Function ejecuta(ByVal sql As String, ByVal ParamArray paras() As Object) As Long

    Function traeReader(ByVal sql As String) As IDataReader

    Function formatofechabase(ByVal fecha As DateTime, Optional ByVal conHora As Boolean = False) As String

    ReadOnly Property con() As IDbConnection

End Interface
