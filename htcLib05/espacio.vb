Public Class espacio

    'implementar patron singleton
    Private Shared _ManejadorBD As conectador
    Public Shared tipoDeBase As tiposBase
    Public Shared conStr As String

    Public Shared ReadOnly Property ManejadorBD() As conectador
        Get
            If _ManejadorBD Is Nothing Then
                abreManejador()
            End If

            Return _ManejadorBD

        End Get
    End Property

    Private Shared Sub abreManejador(Optional ByVal tipo As tiposConexion = tiposConexion.Libre)

   

        Select Case tipoDeBase
            Case tiposBase.Access
                If conStr = "" Then
                    _ManejadorBD = New conexionAccess
                Else
                    _ManejadorBD = New conexionAccess(conStr, tipo)
                End If

            Case tiposBase.Sql
                If conStr = "" Then
                    _ManejadorBD = New conexionSQL
                Else
                    _ManejadorBD = New conexionSQL(conStr, tipo)
                End If
        End Select
    End Sub

    Private Sub New()
        'debo setear tipobase antes de poder usar

    End Sub

    Public Sub New(ByVal base As tiposBase)
        tipoDeBase = base
    End Sub


    ''' <summary>
    ''' Inicializa de forma privada la conexion
    ''' </summary>
    ''' <param name="conStr"></param>
    ''' <param name="base"></param>
    ''' <param name="tipo"></param>
    ''' <remarks></remarks>
    Private Shared Sub inicializa(ByVal conStr As String, ByVal base As tiposBase, ByVal tipo As tiposConexion)

        espacio.tipoDeBase = base
        espacio.conStr = conStr

        Select Case tipo
            Case tiposConexion.Libre
                'no hago nada, no deberia pasar nunca
            Case tiposConexion.Windows
                abreManejador(tipo)
            Case tiposConexion.Web
                abreManejador(tipo)
        End Select

    End Sub

    ''' <summary>
    ''' Inicializa la conexion a la base de datos 
    ''' </summary>
    ''' <param name="conStr">String de conexion </param>
    ''' <param name="tipo">Tipo de conexion ACCESS o SQL</param>
    ''' <remarks></remarks>
    Public Shared Sub inicializa(ByVal conStr As String, ByVal tipo As tiposConexion)

        'depende del string de conexion para sacar el tipo
        Dim tipoBase As tiposBase

        Select Case True
            Case conStr.IndexOf(".mdb") > 0
                tipoBase = tiposBase.Access
            Case Else
                tipoBase = tiposBase.Sql
        End Select

        inicializa(conStr, tipoBase, tipo)

    End Sub


    Public Shared Sub cerrar()
        If Not _ManejadorBD Is Nothing Then _ManejadorBD.cerrar()
        _ManejadorBD = Nothing
        '.dispose
    End Sub


End Class
