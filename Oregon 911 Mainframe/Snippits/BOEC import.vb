Imports Oregon_911_Mainframe.calls
Imports HtmlAgilityPack
Imports Newtonsoft.Json.Linq


Class BOEC
    Dim database As List(Of calls)

    Public Sub ProcessBOEC()
        Dim BOEC As String = Utilities.HTTP("https://911-live.portlandoregon.gov/act_mediaMap.cfm", "POST", "")

        If ((BOEC = "ERROR" = False) Or (BOEC = Nothing = False)) Then
            ' Get LON
            Dim LON_CONDITON = "var jsXar =  new Array();"
            Dim LON = PhraseDataBOEC(LON_CONDITON, BOEC)

            ' GET LAT
            Dim LAT_CONDITON = "var jsYar =  new Array();"
            Dim LAT = PhraseDataBOEC(LAT_CONDITON, BOEC)

            ' GET GUID
            Dim GUID_CONDITON = "var jsIncs =  new Array();"
            Dim GUID = PhraseDataBOEC(GUID_CONDITON, BOEC)

            ' GET ICONS
            Dim ICON_CONDITON = "var jsSymbolURLs =  new Array();"
            Dim iCON = PhraseDataBOEC(ICON_CONDITON, BOEC)

            ' GET INFO
            Dim INFO_CONDITON = "var jsInfo =  new Array();"
            Dim INFO As Array = PhraseDataBOEC(INFO_CONDITON, BOEC)

            Dim Calltype(INFO.Length - 1)
            Dim timestamp(INFO.Length - 1)
            Dim Address(INFO.Length - 1)
            Dim Agency(INFO.Length - 1)

            ' Clean INFO Array and get items 
            For i As Integer = 0 To INFO.Length - 1
                Dim tmp1 = INFO(i).ToString
                Dim tmp2 = Split(tmp1, "<HR>")

                ' Call Type (final)
                Calltype(i) = tmp2(1).Replace("<br>", "")

                ' 
                Dim tmp3 = Split(tmp2(2), "<br>")

                ' Timestamp complete
                timestamp(i) = tmp3(0).Replace("Sent: ", "")
                Dim tmp4 = tmp3(1).Split("(")

                ' Address Complete
                Address(i) = tmp4(0)
                Agency(i) = tmp4(1).Replace(")", "")
            Next

            For i As Integer = 0 To INFO.Length - 1
                Dim INCIDENT As New Oregon_911_Mainframe.calls
                INCIDENT.setCounty("M")
                INCIDENT.setGUID(GUID(i).ToString.Replace(" ", ""))
                INCIDENT.setCallType(Calltype(i))

                Dim time As New List(Of String)
                time.Add(timestamp(i).ToString.Split(" ")(1) & ":00")
                time.Add(0)
                time.Add(0)
                time.Add(0)

                INCIDENT.setTime(time)

                INCIDENT.setAddress(Address(i))

                Dim geo As New List(Of Double)
                geo.Add(LAT(i))
                geo.Add(LON(i))
                INCIDENT.setGeo(geo)

                If iCON(i).ToString.ToLower.Contains("fire") Then
                    INCIDENT.setType("F")
                    INCIDENT.setAgencyName(Agency(i) & " Fire")
                Else
                    INCIDENT.setType("P")
                    INCIDENT.setAgencyName(Agency(i) & " Police")
                End If


                If iCON(i).ToString.ToLower.Contains("orange") Then
                    INCIDENT.setPriority("High")
                ElseIf iCON(i).ToString.ToLower.Contains("red") Then
                    INCIDENT.setPriority("High")
                Else
                    INCIDENT.setPriority("Low")
                End If
                AddOrUpdateCall(INCIDENT)
            Next
            GetUnitsBOEC()
        End If
    End Sub
    Private Sub GetUnitsBOEC()
        For Each incident In database
            If incident.getCounty = "M" Then
                Dim CALLNUM = incident.getGUID.Replace("-", New String("0", 14 - (incident.getGUID.Length)))
                Dim secret = ("https://911-live.portlandoregon.gov/qry_getIncAndUnitData.cfm?which_cad=" & incident.getType & "&complaint_rin=" & CALLNUM)

                Dim BOEC_UNIT As String = Utilities.HTTP("https://911-live.portlandoregon.gov/qry_getIncAndUnitData.cfm?which_cad=" & incident.getType & "&complaint_rin=" & CALLNUM, "POST", "")
                Dim unitlist(0)
                Dim totalutime(0)

                If (BOEC_UNIT.Contains("NO MAP AVAILABLE") Or BOEC_UNIT.Contains("ERROR")) = False Then

                    BOEC_UNIT = Utilities.dtrim(BOEC_UNIT, "<table border=""1"" cellspacing=""0"" cellpadding=""1"" width=""1040"">", 0)
                    BOEC_UNIT = Utilities.dtrim(BOEC_UNIT, "</table>", 1)

                    Dim doc As New HtmlDocument()
                    doc.LoadHtml(BOEC_UNIT)

                    Dim tables As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//table")
                    Dim rows As HtmlAgilityPack.HtmlNodeCollection = tables(0).SelectNodes("tr")

                    For i As Integer = 2 To rows.Count - 1
                        Dim INCIDENT_UNIT As New Oregon_911_Mainframe.units

                        Dim cols As HtmlAgilityPack.HtmlNodeCollection = rows(i).SelectNodes("td")

                        Dim UNIT As String = cols(0).InnerText
                        Dim STATUS As String = cols(1).InnerText
                        Dim time() As String = cols(2).InnerText.Replace("&nbsp;", Nothing).Split(" ")
                        Dim t = time(1)

                        Dim CACHED_TIME As New List(Of String)
                        Dim NEW_TIME As New List(Of String)
                        For Each U In incident.IncidentsUnits
                            If U.getUnit = UNIT Then
                                CACHED_TIME = U.getTime
                            End If
                        Next

                        If CACHED_TIME.Count = 0 Then
                            CACHED_TIME = New List(Of String)
                            CACHED_TIME.Add("00:00:00")
                            CACHED_TIME.Add("00:00:00")
                            CACHED_TIME.Add("00:00:00")
                            CACHED_TIME.Add("00:00:00")
                        End If

                        Select Case STATUS
                            Case "ENROUTE"
                                NEW_TIME.Add(CACHED_TIME(0))
                                NEW_TIME.Add(t & ":00")
                                NEW_TIME.Add(CACHED_TIME(2))
                                NEW_TIME.Add(CACHED_TIME(3))
                            Case "DISPATCHED"
                                NEW_TIME.Add(t & ":00")
                                NEW_TIME.Add(CACHED_TIME(1))
                                NEW_TIME.Add(CACHED_TIME(2))
                                NEW_TIME.Add(CACHED_TIME(3))
                            Case "ON SCENE"
                                NEW_TIME.Add(CACHED_TIME(0))
                                NEW_TIME.Add(CACHED_TIME(1))
                                NEW_TIME.Add(t & ":00")
                                NEW_TIME.Add(CACHED_TIME(3))
                            Case "CLEAR"
                                NEW_TIME.Add(CACHED_TIME(0))
                                NEW_TIME.Add(CACHED_TIME(1))
                                NEW_TIME.Add(CACHED_TIME(2))
                                NEW_TIME.Add(t & ":00")
                            Case Else
                                NEW_TIME.Add("00:00:00")
                                NEW_TIME.Add("00:00:00")
                                NEW_TIME.Add("00:00:00")
                                NEW_TIME.Add("00:00:00")
                        End Select
                        INCIDENT_UNIT.setTime(NEW_TIME)
                        INCIDENT_UNIT.setGUID(incident.getGUID)
                        INCIDENT_UNIT.setCounty(incident.getCounty)
                        INCIDENT_UNIT.setUnit(UNIT)

                        AddOrUpdateUnit(incident, INCIDENT_UNIT)
                    Next

                End If

            End If
        Next
    End Sub
    Private Function PhraseDataBOEC(ByVal CONDITON As String, ByVal html As String)
        If html = Nothing = False Then
            Dim Info = Utilities.dtrim(html, CONDITON, 0).ToString.Split(vbNewLine)

            Dim DATA As String = ""

            For Each DATASTRING In Info
                If DATASTRING.Replace(vbLf, "") = CONDITON = False Then
                    If DATASTRING.Replace(vbLf, "") = ";" = False Then
                        Dim LOC = DATASTRING.Replace(vbLf, "").Replace(";", "").Replace("""", "").Split("=")
                        If DATA.Length = 0 Then
                            DATA += LOC(1)
                        Else
                            DATA += "Õ" & LOC(1)
                        End If
                    Else
                        Exit For
                    End If
                End If
            Next

            If DATA.Length > 0 Then
                Dim final_LOC() = DATA.Split("Õ")
                Return final_LOC
            Else
                Return Nothing
            End If
        End If
        Return Nothing
    End Function

    Public Sub AddOrUpdateCall(ByVal something As calls)
        ' See incidents.vb
    End Sub

    Private Sub AddOrUpdateUnit(ByVal [call] As Oregon_911_Mainframe.calls, ByVal unit As Oregon_911_Mainframe.units)
        ' See incidents.vb
    End Sub

End Class