Public Class corp

    Public Enum tiposTarjetas
        visa = 1
        master = 2
        discovery = 3
        amex = 4
    End Enum

    Public Shared Function VerificaCedula(ByVal Cedula As String) As Boolean
        Dim PrimerosDigitos As String
        Dim DigitVerificador As String
        Dim NumDigitos As Byte

        If Len(Cedula) = 11 Or Len(Cedula) = 10 Then
            PrimerosDigitos = Mid(Cedula, 1, 9)
            DigitVerificador = Mid(Cedula, 11, 1)
            NumDigitos = True
            If Len(Cedula) = 10 Then _
                DigitVerificador = Mid(Cedula, 10, 1)
        Else
            Return False
        End If

        If DigitVerificador = GetDigitoVerificador(PrimerosDigitos) Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Shared Function GetDigitoVerificador(ByVal Digitos As String) As Int16
        Dim i As Byte
        Dim ValImpares As String
        Dim ValPares As String
        Dim Sum1 As Byte
        Dim Sum2 As Byte
        Dim TotalSum As Byte

        For i = 1 To 9 Step 2
            ValImpares = Mid(Digitos, i, 1) * 2
            If ValImpares >= 10 Then
                ValImpares = ValImpares - 9
            End If
            Sum1 = Sum1 + ValImpares
            ValPares = Mid(Digitos, (i + 1), 1)
            If i < 9 Then Sum2 = Sum2 + ValPares

        Next
        TotalSum = Sum1 + Sum2
        If TotalSum Mod 10 = 0 Then
            Return 0
        Else
            Sum1 = (Mid(TotalSum, 1, 1) + 1) * 10
            TotalSum = Sum1 - TotalSum
            Return TotalSum

        End If

    End Function


    Public Shared Function ValidaTarjeta(ByVal tipo As tiposTarjetas, ByVal num As String) As Boolean

        'PRIMERO REVISO LA CANTIDAD DE DIGITOS

        num = Trim(num)

        Select Case tipo
            Case tiposTarjetas.visa
                If (num.Length = 13 Or num.Length = 16) AndAlso (num.Substring(0, 1) = "4") AndAlso (luhn(num) = True) Then Return True
            Case tiposTarjetas.master
                If (num.Length = 16) AndAlso (num.Substring(0, 1) = "5") AndAlso (luhn(num) = True) Then Return True

            Case Else
                Return True
        End Select

        Return False

    End Function

    Public Shared Function luhn(ByVal numcc As String) As Boolean
        Dim i As Integer
        Dim total As Integer
        Dim even As String
        Dim len As Integer = numcc.Length
        Dim digits(len - 1) As Integer

        For i = 0 To len - 1
            digits(i) = CInt(numcc.Substring(i, 1))
        Next

        For i = len - 1 To 1 Step -2
            total += digits(i)
            even = (digits(i - 1) * 2).ToString("00")
            total += Convert.ToInt32(even.Substring(0, 1))
            total += Convert.ToInt32(even.Substring(1, 1))
        Next

        If len Mod 2 <> 0 Then
            total += digits(0)
        End If

        If total Mod 10 = 0 Then
            Return True
        End If

        Return False

    End Function

    Public Shared Function ValidaEmail(ByVal mail As String) As Boolean

        Dim temp As Boolean = True  'asumo que esta bien
        Dim Mleft, Mright As String

        If InStr(1, mail, "@") > 0 Then
            Mleft = Split(mail, "@")(0)
            Mright = Split(mail, "@")(1)

            If Mleft = "" Then temp = False
            If Mright = "" Then temp = False

            If temp = True Then
                If InStr(Mright, ".") = 0 Then temp = False
            End If

        Else
            temp = False
        End If

        Return temp

    End Function

End Class
