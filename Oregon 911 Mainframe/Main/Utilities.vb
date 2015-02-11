Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports Tamir.SharpSsh

Public Module Utilities

#Region "Oregon 911 Specific"
    Private UNIT_URL = "http://cad.oregon911.net/call?call="
    Public Function GetDepartment(ByVal agency)
        Select Case agency
            Case "CBOC"
                Return "Tualatin Valley Fire & Rescue"
            Case "TRS"
                Return "Tualatin Valley Fire & Rescue"
            Case "SHW"
                Return "Tualatin Valley Fire & Rescue"
            Case "TUA"
                Return "Tualatin Valley Fire & Rescue"
            Case "KCF"
                Return "Tualatin Valley Fire & Rescue"
            Case "WAL"
                Return "Tualatin Valley Fire & Rescue"
            Case "TIG"
                Return "Tualatin Valley Fire & Rescue"
            Case "WIL"
                Return "Tualatin Valley Fire & Rescue"
            Case "PRO"
                Return "Tualatin Valley Fire & Rescue"
            Case "CNL"
                Return "Tualatin Valley Fire & Rescue"
            Case "CHS"
                Return "Tualatin Valley Fire & Rescue"
            Case "ALO"
                Return "Tualatin Valley Fire & Rescue"
            Case "RCK"
                Return "Tualatin Valley Fire & Rescue"
            Case "WSL"
                Return "Tualatin Valley Fire & Rescue"
            Case "BRR"
                Return "Tualatin Valley Fire & Rescue"
            Case "BVM"
                Return "Tualatin Valley Fire & Rescue"
            Case "KAI"
                Return "Tualatin Valley Fire & Rescue"
            Case "SKY"
                Return "Tualatin Valley Fire & Rescue"
            Case "CMT"
                Return "Tualatin Valley Fire & Rescue"
            Case "ERD"
                Return "Tualatin Valley Fire & Rescue"
            Case "MTR"
                Return "Tualatin Valley Fire & Rescue"
            Case "BOL"
                Return "Tualatin Valley Fire & Rescue"
            Case "WFD"
                Return "Tualatin Valley Fire & Rescue"
            Case "BTH"
                Return "Tualatin Valley Fire & Rescue"
            Case "HBM"
                Return "Hillsboro Fire & Rescue"
            Case "HWH"
                Return "Hillsboro Fire & Rescue"
            Case "HRA"
                Return "Hillsboro Fire & Rescue"
            Case "HJF"
                Return "Hillsboro Fire & Rescue"
            Case "HCL"
                Return "Hillsboro Fire & Rescue"
            Case "FGF"
                Return "Forest Grove Fire & Rescue"
            Case "GCF"
                Return "Forest Grove Fire & Rescue"
            Case "NPF"
                Return "Washington County Fire District #2"
            Case "MWF"
                Return "Washington County Fire District #2"
            Case "COF"
                Return "Cornelius Fire Department"
            Case "BKF"
                Return "Banks Fire District #13"
            Case "BUX"
                Return "Banks Fire District #13"
            Case "TIM"
                Return "Banks Fire District #13"
            Case "GAF"
                Return "Gaston Rural Fire District #11"
            Case "CNB"
                Return "Canby Fire District #62"
            Case "LNE"
                Return "Canby Fire District #62"
            Case "TCR"
                Return "Clackamas Fire District #1"
            Case "MIL"
                Return "Clackamas Fire District #1"
            Case "OGR"
                Return "Clackamas Fire District #1"
            Case "LKR"
                Return "Clackamas Fire District #1"
            Case "CAU"
                Return "Clackamas Fire District #1"
            Case "HVA"
                Return "Clackamas Fire District #1"
            Case "PVA"
                Return "Clackamas Fire District #1"
            Case "CLA"
                Return "Clackamas Fire District #1"
            Case "HOL"
                Return "Clackamas Fire District #1"
            Case "BCK"
                Return "Clackamas Fire District #1"
            Case "RDL"
                Return "Clackamas Fire District #1"
            Case "LOG"
                Return "Clackamas Fire District #1"
            Case "CLK"
                Return "Clackamas Fire District #1"
            Case "HLD"
                Return "Clackamas Fire District #1"
            Case "JAS"
                Return "Clackamas Fire District #1"
            Case "HLT"
                Return "Clackamas Fire District #1"
            Case "SND"
                Return "Clackamas Fire District #1"
            Case "BOR"
                Return "Clackamas Fire District #1"
            Case "ECR"
                Return "Clackamas Fire District #1"
            Case "DAM"
                Return "Clackamas Fire District #1"
            Case "COL"
                Return "Colton Fire District #70"
            Case "EST"
                Return "Estacada Fire District #69"
            Case "GEO"
                Return "Estacada Fire District #69"
            Case "GLA"
                Return "Gladstone Fire Department"
            Case "WEL"
                Return "Hoodland Fire District #74"
            Case "BRI"
                Return "Hoodland Fire District #74"
            Case "GOV"
                Return "Hoodland Fire District #74"
            Case "MOL"
                Return "Molalla Fire District #73"
            Case "MUL"
                Return "Molalla Fire District #73"
            Case "SAN"
                Return "Sandy Fire District #72"
            Case "DVR"
                Return "Sandy Fire District #72"
            Case "RLK"
                Return "Sandy Fire District #72"
            Case "AUA"
                Return "Aurora Fire District #63"
            Case "WCCCA"
                Return "Oregon Department of Forestry"
            Case "ODF"
                Return "Oregon Department of Forestry"
            Case "P1"
                Return "Portland Fire Bureau"
            Case "P3"
                Return "Portland Fire Bureau"
            Case "P4"
                Return "Portland Fire Bureau"
            Case "P5"
                Return "Portland Fire Bureau"
            Case "P7"
                Return "Portland Fire Bureau"
            Case "P11"
                Return "Portland Fire Bureau"
            Case "P15"
                Return "Portland Fire Bureau"
            Case "P16"
                Return "Portland Fire Bureau"
            Case "P18"
                Return "Portland Fire Bureau"
            Case "P20"
                Return "Portland Fire Bureau"
            Case "P25"
                Return "Portland Fire Bureau"
            Case "P27"
                Return "Portland Fire Bureau"
            Case "P28"
                Return "Portland Fire Bureau"
            Case "P29"
                Return "Portland Fire Bureau"
            Case "YAM"
                Return "Yamhill Fire Protection District"
            Case "NBF"
                Return "Newberg Fire Department"
            Case "LCOM"
                Return "Lake Oswego Fire Department"
            Case "VER"
                Return "Vernonia Rural Fire Department"
            Case "USF"
                Return "United States Forest Service"
            Case "HUB"
                Return "Hubbard Fire District"
            Case Else
                Return "Unknown"
        End Select
    End Function
    Public Function processMarkers(ByVal html)
        Dim output As String = html

        ' Add GUID marker so its easier to sort
        output = output.Replace("CallNo"">", "CallNo"">GUID: ")

        ' Add Unit marker so its easier to sort
        output = output.Replace("lblUnits"">", "lblUnits""> Units: ")

        ' Strip all HTML Tags & White Space
        output = StripWhiteSpace(StripTags(output))

        Return output
    End Function
    Public Function CreateUnitURL(ByVal GUID As Integer, ByVal county As Char)
        Return UNIT_URL & GUID & "&county=" & county
    End Function
    Function getDistance(ByVal LATLON_FROM As List(Of Double), ByVal LATLON_TO As List(Of Double))
        Dim pi80 = Math.PI / 180

        Dim LATLON_FROM_B = LATLON_FROM.ToList
        Dim LATLON_TO_B = LATLON_TO.ToList

        LATLON_FROM(0) *= pi80
        LATLON_FROM(1) *= pi80
        LATLON_TO(0) *= pi80
        LATLON_TO(1) *= pi80

        Dim r = "6372.797" ' mean radius of Earth in km
        Dim dlat = LATLON_TO(0) - LATLON_FROM(0)
        Dim dlon = LATLON_TO(1) - LATLON_FROM(1)

        Dim a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) + Math.Cos(LATLON_FROM(0)) * Math.Cos(LATLON_TO(0)) * Math.Sin(dlon / 2) * Math.Sin(dlon / 2)
        Dim c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a))
        Dim m = ((r * c) / 0.001)
        Return m
    End Function
#End Region

#Region "Text"
    Public Function dtrim(ByVal text, ByVal var, ByVal direction)
        Try
            If direction = 1 Then
                Dim strTemp As String, strNew As String
                strTemp = text
                strNew = Mid$(strTemp, 1, InStr(1, strTemp, var) + Len(var) - 1)
                Return (strNew)
            Else
                Return text.Remove(0, text.IndexOf(var) - 1)
            End If
        Catch ex As Exception
            Console.WriteLine(DateAndTime.Now & ": Error processing PITS Data, PITS has been updated or you're recieved a bad file!")
            Return 0
        End Try
    End Function

    Public Sub Log(ByVal folder As String, ByVal message As String)
        Dim dir = "Logs\" & folder
        If Not My.Computer.FileSystem.DirectoryExists(dir) Then
            My.Computer.FileSystem.CreateDirectory(dir)
        End If
        My.Computer.FileSystem.WriteAllText("Logs\" & folder & "\LOG " & DateAndTime.Now.ToString("yyyy-MM-dd") & ".txt", message & vbNewLine, True)
    End Sub


    Function StripNumber(stdText As String)
        Dim str As String = Nothing, i As Integer
        'strips the number from a longer text string
        stdText = Trim(stdText)

        For i = 1 To Len(stdText)
            If Not IsNumeric(Mid(stdText, i, 1)) Then
                str = str & Mid(stdText, i, 1)
            End If
        Next i

        StripNumber = str ' * 1

    End Function
    Public Function StripTags(ByVal html As String) As String
        ' Remove HTML tags.
        Return Regex.Replace(Regex.Replace(html, "<[^>]*>", ""), "<.*?>", "")
    End Function
    Public Function StripWhiteSpace(ByVal text As String) As String
        ' Remove HTML tags.
        Return Regex.Replace(text, "\s{2,}", " ").Replace(vbLf, "").Replace(vbNewLine, "")
    End Function
    Public Function DeleteItem(Arr As Object, index As Long)
        Dim i As Long
        For i = index To UBound(Arr) - 1
            Arr(i) = Arr(i + 1)
        Next
        Arr(UBound(Arr)) = ""
        Return Arr
    End Function
    Public Function txtDelete(ByVal txt As String, pattern As String)
        Return txt.Replace(pattern, "")
    End Function
#End Region
#Region "Disk"
    Public Function IsFileLocked(filename As String) As Boolean
        Dim Locked As Boolean = False
        Try
            Dim fs As FileStream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None)
            fs.Close()
        Catch ex As IOException
            Locked = True
        End Try
        Return Locked
    End Function
#End Region

#Region "WWW"
    Public Function HTTP(ByVal url As String, ByVal method As String, ByVal data As String, Optional retry As Integer = 0)
        Try
            If retry < 3 Then
                Dim request As System.Net.WebRequest = System.Net.WebRequest.Create(url)
                request.Method = method
                Dim postData = data
                Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
                request.ContentType = "application/x-www-form-urlencoded"
                request.ContentLength = byteArray.Length
                Dim dataStream As Stream = request.GetRequestStream()
                dataStream.Write(byteArray, 0, byteArray.Length)
                dataStream.Close()
                request.Timeout = 120000
                Dim response As WebResponse = request.GetResponse()
                dataStream = response.GetResponseStream()
                Dim reader As New StreamReader(dataStream)
                Dim responseFromServer As String = reader.ReadToEnd()
                reader.Close()
                dataStream.Close()
                response.Close()
                Return (responseFromServer)
            Else
                Return "ERROR"
            End If
        Catch ex As Exception
            Utilities.Log("error", "Utilities- HTTP FAILURE ( " & retry & " ) URL: " & url & " method: " & method & " data: " & data & " ERROR: " & ex.Message)
            Threading.Thread.Sleep(500)
            retry += 1
            Return HTTP(url, method, data, retry)
        End Try
    End Function
#End Region

End Module