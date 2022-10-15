Public Class numAtexto

    Public Function numatxt(ByVal num As Decimal) As String
        Dim dig As Byte
        Dim numtxt As String
        Dim numint As Long
        Dim dec As Single
        Dim txtdec As String

        numint = Int(num)

        dec = Math.Round(num - numint, 2) * 100
        txtdec = Format(dec, "00")

        numtxt = CStr(numint)
        dig = Len(numtxt)

        numatxt = CallByName(Me, "dig" & dig, vbMethod, numint)

        numatxt = numatxt & " CON " & txtdec & "/100"

    End Function

    Function dig1(ByVal num As Long) As String

        Dim dig As String

        Select Case num
            Case 1 : dig = "UNO"
            Case 2 : dig = "DOS"
            Case 3 : dig = "TRES"
            Case 4 : dig = "CUATRO"
            Case 5 : dig = "CINCO"
            Case 6 : dig = "SEIS"
            Case 7 : dig = "SIETE"
            Case 8 : dig = "OCHO"
            Case 9 : dig = "NUEVE"
        End Select

        Return dig

    End Function

    Function dig2(ByVal num As Long) As String
        Dim dig As String
        Dim seg As String

        Select Case num
            Case 90 To 99 : dig = "NOVENTA"
            Case 80 To 89 : dig = "OCHENTA"
            Case 70 To 79 : dig = "SETENTA"
            Case 60 To 69 : dig = "SESENTA"
            Case 50 To 59 : dig = "CINCUENTA"
            Case 40 To 49 : dig = "CUARENTA"
            Case 30 To 39 : dig = "TREINTA"
            Case 20 To 29 : dig = "VEINTE"
            Case 10, 16 To 19
                dig = "DIEZ"
            Case 11 : dig = "ONCE"
            Case 12 : dig = "DOCE"
            Case 13 : dig = "TRECE"
            Case 14 : dig = "CATORCE"
            Case 15 : dig = "QUINCE"

            Case Is < 10 : dig = ""

        End Select

        seg = Right(CStr(num), 1)

        If Not seg = "0" And Not (num > 10 And num < 16) Then
            If dig = "" Then
                dig = dig & " " & dig1(CByte(seg))
            Else
                dig = dig & " Y " & dig1(CByte(seg))
            End If
        End If

        Return dig

    End Function

    Function dig3(ByVal num As Long) As String
        Dim n1 As String
        Dim n2 As String
        Dim dig As String

        n1 = Left(num, 1)
        n2 = Right(num, 2)

        Select Case num
            Case Is < 100
                dig = ""
            Case 100 : dig = "CIEN"
            Case 101 To 199 : dig = "CIENTO"
            Case 500 To 599 : dig = "QUINIENTOS"
            Case 700 To 799 : dig = "SETECIENTOS"
            Case 900 To 999 : dig = "NOVECIENTOS"

            Case Else
                dig = dig1((n1)) & "CIENTOS"
        End Select

        If Not n2 = 0 Then
            dig = dig & " " & dig2(n2)
        End If

        Return dig

    End Function

    Function dig4(ByVal num As Long) As String
        Dim n3 As String
        Dim n1 As String
        Dim dig As String

        n1 = Left(num, 1)
        n3 = Mid(num, 2, 3)

        Select Case num
            Case 1000 To 1999 : dig = "MIL "

            Case Else
                dig = dig1(n1) & " " & " MIL"
        End Select

        dig = dig & " " & dig3((n3))

        Return dig

    End Function

    Function dig5(ByVal num As Long) As String

        Dim n1y2 As String
        Dim resto As String
        Dim diezmil As String

        Dim dig As String

        n1y2 = Left(num, 2)
        resto = Mid(num, 3)

        diezmil = dig2(n1y2)

        'parceh para el UN
        If Right(n1y2, 1) = 1 And Not Left(n1y2, 1) = 1 Then
            diezmil = Left(diezmil, Len(diezmil) - 1)
        End If

        diezmil = diezmil & " MIL "

        dig = diezmil & dig3((resto))

        Return dig

    End Function
    Function dig6(ByVal num As Long) As String

        Dim n1 As String, n2y3 As String
        Dim resto As String
        Dim txtresto As String
        Dim dig As String


        n1 = Left(num, 1)
        n2y3 = Mid(num, 2, 2)

        resto = Mid(num, 2)
        txtresto = dig5(resto)

        'parche maldito
        If n1 = "1" And Not n2y3 = "00" Then
            dig = "CIENTO " & txtresto
        Else
            dig = dig3(CLng(n1 & "00")) & " " & txtresto
        End If

        Return dig

    End Function

End Class
