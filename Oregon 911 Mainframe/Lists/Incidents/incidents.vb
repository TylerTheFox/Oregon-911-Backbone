Imports Oregon_911_Mainframe.calls
Imports HtmlAgilityPack
Imports Newtonsoft.Json.Linq


Public Class incidents
    Public database As New List(Of calls)
    Public Updating As Boolean = False

    Private GUID As New List(Of String) ' These should've been a struct but w/e.
    Private County As New List(Of String)

    Structure Unit_Tmp
        Dim UNIT As String
        Dim GUID As String
        Dim County
    End Structure

    Structure New_Unit_Tmp
        Dim UnitString As String
        Dim UnitList As List(Of units)
    End Structure

    Dim Temp_units As New List(Of Unit_Tmp)
    Public Property useAPI As Boolean = False

#Region "Public Functions"
    Public Function updateIncdients() As Boolean
        Dim result = False
        Try
            If GUID.Count > 0 Then
                Temp_units.Clear()
                GUID.Clear()
                County.Clear()
            End If

            Updating = True
            If useAPI Then
                API_ProcessCalls()
                API_PraseUnits()
            Else
                result = NonAPI_ProcessCalls()
                'NonAPI_PraseUnits()
            End If

            If result Then
                CleanCalls()
            End If

            Updating = False
        Catch ex As Exception
            ' Get stack trace for the exception with source file information
            Dim st = New StackTrace(ex, True)
            ' Get the top stack frame
            Dim frame = st.GetFrame(0)
            ' Get the line number from the stack frame
            Dim line = frame.ToString
            Utilities.Log("error", "UpdateIncidents: " & ex.Message & " | " & frame.ToString)
            Return False
        End Try
        Return result
    End Function
#End Region

#Region "API"
    Private Sub API_ProcessCalls()
        Try
            Dim PITS As JArray = API.ListCalls
            If Not IsNothing(PITS) Then
                For i = 0 To PITS.Count - 1
                    Dim [call] As New calls
                    [call].setGUID(PITS.Item(i).Item("GUID"))
                    [call].setCounty(PITS.Item(i).Item("county"))
                    [call].setCallType(PITS.Item(i).Item("callSum"))
                    [call].setAddress(PITS.Item(i).Item("address"))
                    [call].setAgencyName(PITS.Item(i).Item("agency"))
                    [call].setStation(PITS.Item(i).Item("station"))
                    [call].setAgency("NA", useAPI)
                    [call].setType(PITS.Item(i).Item("type"))
                    [call].setUnits(PITS.Item(i).Item("units"))
                    [call].setPriority(PITS.Item(i).Item("priority"))
                    Dim CallStart = PITS.Item(i).Item("timestamp").ToString.Split(" ")
                    Dim time As New List(Of String)
                    time.Add(CallStart(1))
                    time.Add("NA")
                    time.Add("NA")
                    time.Add("NA")
                    [call].setTime(time)
                    Dim Geo As New List(Of Double)
                    Geo.Add(PITS.Item(i).Item("lat"))
                    Geo.Add(PITS.Item(i).Item("lon"))
                    [call].setGeo(Geo)
                    AddOrUpdateCall([call])
                Next
            End If
        Catch ex As Exception
            ' Get stack trace for the exception with source file information
            Dim st = New StackTrace(ex, True)
            ' Get the top stack frame
            Dim frame = st.GetFrame(0)
            ' Get the line number from the stack frame
            Dim line = frame.ToString
            Utilities.Log("error", "API_ProcessCalls: " & ex.Message & " | " & frame.ToString)
        End Try
    End Sub
    Private Sub API_PraseUnits()
        Try
            Dim PITS As JArray = API.ListUnits
            If Not IsNothing(PITS) Then
                For Each incident In database
                    For i = 0 To PITS.Count - 1
                        Dim unit As New units
                        unit.setGUID(PITS.Item(i).Item("GUID"))
                        unit.setCounty(PITS.Item(i).Item("county"))
                        If unit.getGUID = incident.getGUID And unit.getCounty = incident.getCounty Then
                            unit.setUnit(PITS.Item(i).Item("unit"))
                            unit.setAgency(PITS.Item(i).Item("agency"))
                            unit.setStation(PITS.Item(i).Item("agency"))
                            unit.setStation(PITS.Item(i).Item("station"))
                            Dim time As New List(Of String)
                            time.Add(PITS.Item(i).Item("dispatched"))
                            time.Add(PITS.Item(i).Item("enroute"))
                            time.Add(PITS.Item(i).Item("onscene"))
                            time.Add(PITS.Item(i).Item("clear"))
                            unit.setTime(time)
                            AddOrUpdateUnit(incident, unit)
                        End If
                    Next
                Next
            End If
        Catch ex As Exception
            ' Get stack trace for the exception with source file information
            Dim st = New StackTrace(ex, True)
            ' Get the top stack frame
            Dim frame = st.GetFrame(0)
            ' Get the line number from the stack frame
            Dim line = frame.ToString
            Utilities.Log("error", "API_PraseUnits: " & ex.Message & " | " & frame.ToString)
        End Try
    End Sub
#End Region

#Region "Non-API"
    Private Function NonAPI_ProcessCalls()
        Try
            Dim PITS = Utilities.HTTP("http://www.wccca.com/PITSv2/Default.aspx", "POST", "__viewstate=")
            If PITS.ToString.Contains("ERROR") = False And Not PITS = Nothing Then
                Dim htmlDoc As New HtmlDocument
                htmlDoc.LoadHtml(PITS)
                Dim root = htmlDoc.DocumentNode
                Dim a_nodes = root.Descendants("span").ToList()

                Dim Calls As Integer = 0
                Dim callNo As New List(Of Integer)
                Dim County As New List(Of Char)
                Dim CallType As New List(Of String)
                Dim Address As New List(Of String)
                Dim CallRecievedTime As New List(Of String)
                Dim CallDispatchedTime As New List(Of String)
                Dim CallEnRouteTime As New List(Of String)
                Dim CallOnsceneTime As New List(Of String)
                Dim Agency As New List(Of String)
                Dim AgencyName As New List(Of String)
                Dim Station As New List(Of String)
                Dim UnitList As New List(Of New_Unit_Tmp)

                For i As Integer = 0 To a_nodes.Count - 1
                    If a_nodes(i).GetAttributeValue("id", "").Contains("CallNo") Then
                        Calls += 1
                        callNo.Add(a_nodes(i).InnerText.Trim())
                        If a_nodes(i).GetAttributeValue("id", "").Contains("CCOM") Then
                            County.Add("C")
                        Else
                            County.Add("W")
                        End If
                    End If

                    If a_nodes(i).GetAttributeValue("id", "").Contains("CallType") Then
                        CallType.Add(a_nodes(i).InnerText.Trim().ToUpper)
                    End If

                    If a_nodes(i).GetAttributeValue("id", "").Contains("Units") Then
                        Dim unit_tmp As units
                        Dim time_tmp As List(Of String)
                        Dim UnitDoc As New HtmlDocument
                        Dim NewUnits As New New_Unit_Tmp
                        NewUnits.UnitString = (a_nodes(i).InnerText.Trim())
                        NewUnits.UnitList = New List(Of units)
                        UnitDoc.LoadHtml(a_nodes(i).OuterHtml.Trim())
                        Dim u_nodes = UnitDoc.DocumentNode.Descendants("span").ToList()
                        For Each u_node In u_nodes
                            If u_node.GetAttributeValue("class", "") = "dispatched" Then
                                unit_tmp = New units
                                time_tmp = New List(Of String)
                                unit_tmp.setUnit(u_node.InnerText.Trim().ToUpper())
                                time_tmp.Add(u_node.GetAttributeValue("title", "").Replace("Dispatched @ ", Nothing))
                                time_tmp.Add("NA")
                                time_tmp.Add("NA")
                                time_tmp.Add("NA")
                                unit_tmp.setTime(time_tmp)
                                NewUnits.UnitList.Add(unit_tmp)
                            ElseIf u_node.GetAttributeValue("class", "") = "enroute" Then
                                unit_tmp = New units
                                time_tmp = New List(Of String)
                                unit_tmp.setUnit(u_node.InnerText.Trim().ToUpper())
                                time_tmp.Add("NA")
                                time_tmp.Add(u_node.GetAttributeValue("title", "").Replace("En Route @ ", Nothing))
                                time_tmp.Add("NA")
                                time_tmp.Add("NA")
                                unit_tmp.setTime(time_tmp)
                                NewUnits.UnitList.Add(unit_tmp)
                            ElseIf u_node.GetAttributeValue("class", "") = "onscene" Then
                                unit_tmp = New units
                                time_tmp = New List(Of String)
                                unit_tmp.setUnit(u_node.InnerText.Trim().ToUpper())
                                time_tmp.Add("NA")
                                time_tmp.Add("NA")
                                time_tmp.Add(u_node.GetAttributeValue("title", "").Replace("On Scene @ ", Nothing))
                                time_tmp.Add("NA")
                                unit_tmp.setTime(time_tmp)
                                NewUnits.UnitList.Add(unit_tmp)
                            ElseIf u_node.GetAttributeValue("class", "") = "clear" Then
                                unit_tmp = New units
                                time_tmp = New List(Of String)
                                unit_tmp.setUnit(u_node.InnerText.Trim().ToUpper())
                                time_tmp.Add("NA")
                                time_tmp.Add("NA")
                                time_tmp.Add("NA")
                                time_tmp.Add(u_node.GetAttributeValue("title", "").Replace("Cleared @ ", Nothing))
                                unit_tmp.setTime(time_tmp)
                                NewUnits.UnitList.Add(unit_tmp)
                            End If
                        Next
                        UnitList.Add(NewUnits)
                    End If

                    If a_nodes(i).GetAttributeValue("class", "") = "address" Then
                        Address.Add(a_nodes(i).InnerText.Trim().ToUpper)
                    End If

                    If a_nodes(i).GetAttributeValue("title", "") = "Call Entry Time" And Not a_nodes(i).GetAttributeValue("class", "") = "time" Then
                        CallRecievedTime.Add(a_nodes(i).InnerText.Trim())
                    End If

                    If a_nodes(i).GetAttributeValue("title", "") = "Dispatch Time" Then
                        CallDispatchedTime.Add(a_nodes(i).InnerText.Trim())
                    End If

                    If a_nodes(i).GetAttributeValue("title", "") = "En Route Time" Then
                        CallEnRouteTime.Add(a_nodes(i).InnerText.Trim())
                    End If

                    If a_nodes(i).GetAttributeValue("title", "") = "On Scene Time" Then
                        CallOnsceneTime.Add(a_nodes(i).InnerText.Trim())
                    End If

                    If a_nodes(i).GetAttributeValue("class", "") = "units" Then
                        AgencyName.Add(HtmlEntity.DeEntitize(a_nodes(i + 1).GetAttributeValue("title", "")))
                        Station.Add(a_nodes(i + 2).InnerText.Trim())
                        Agency.Add(a_nodes(i + 1).InnerText.Trim())
                    End If
                Next

                For i As Integer = 0 To Calls - 1
                    Dim calltmp As New calls
                    If Not i > callNo.Count - 1 Then
                        calltmp.setGUID(callNo(i))
                    Else
                        Throw New Exception("Error callNo Out of bounds!")
                    End If
                    If Not i > County.Count - 1 Then
                        calltmp.setCounty(County(i))
                    Else
                        Throw New Exception("Error County Out of bounds!")
                    End If
                    If Not i > CallType.Count - 1 Then
                        calltmp.setCallType(CallType(i))
                    Else
                        Throw New Exception("Error setCallType Out of bounds!")
                    End If

                    If Not i > Address.Count - 1 Then
                        calltmp.setAddress(Address(i))
                    Else
                        Throw New Exception("Error Address Out of bounds!")
                    End If

                    Dim TimeTmp As New List(Of String)

                    If Not i > CallRecievedTime.Count - 1 Then
                        TimeTmp.Add(CallRecievedTime(i))
                    Else
                        Throw New Exception("Error CallRecievedTime Out of bounds!")
                    End If
                    If Not i > CallDispatchedTime.Count - 1 Then
                        TimeTmp.Add(CallDispatchedTime(i))
                    Else
                        Throw New Exception("Error CallDispatchedTime Out of bounds!")
                    End If
                    If Not i > CallEnRouteTime.Count - 1 Then
                        TimeTmp.Add(CallEnRouteTime(i))
                    Else
                        Throw New Exception("Error CallEnRouteTime Out of bounds!")
                    End If
                    If Not i > CallOnsceneTime.Count - 1 Then
                        TimeTmp.Add(CallOnsceneTime(i))
                    Else
                        Throw New Exception("Error CallOnsceneTime Out of bounds!")
                    End If

                    calltmp.setTime(TimeTmp)

                    If Not i > Agency.Count - 1 Then
                        calltmp.setAgency(Agency(i), False)
                    Else
                        Throw New Exception("Error Agency Out of bounds!")
                    End If

                    If Not i > AgencyName.Count - 1 Then
                        calltmp.setAgencyName(AgencyName(i))
                    Else
                        Throw New Exception("Error AgencyName Out of bounds!")
                    End If

                    If Not i > Station.Count - 1 Then
                        calltmp.setStation(Station(i))
                    Else
                        Throw New Exception("Error Station Out of bounds!")
                    End If

                    calltmp.setUnits(UnitList(i).UnitString)

                    Dim GEO_STRING = Utilities.dtrim(Utilities.dtrim(PITS, "LoadMarker(parseFloat(", 0), "updateMarkers();", 1).ToString.Split("LoadMarker(")

                    For Each item In GEO_STRING
                        If Not String.IsNullOrWhiteSpace(item) Then
                            Dim GEO_INFO = Utilities.dtrim(item, ";", 1).ToString.Split("', '")
                            If Trim(Utilities.txtDelete(GEO_INFO(3), "'")) = calltmp.getGUID Then
                                Dim Geo As New List(Of Double)
                                If calltmp.getCounty = "W" Then
                                    If GEO_INFO(5).Contains("wccca") Then
                                        GEO_INFO = GEO_INFO(0).ToString.Split(",")
                                        Geo.Add(Trim(Utilities.txtDelete(Utilities.txtDelete(GEO_INFO(0), "oadMarker(parseFloat("), ")")))
                                        Geo.Add(Trim(Utilities.txtDelete(Utilities.txtDelete(GEO_INFO(1), "parseFloat("), ")")))
                                    End If
                                Else
                                    If GEO_INFO(5).Contains("ccom") Then
                                        GEO_INFO = GEO_INFO(0).ToString.Split(",")
                                        Geo.Add(Trim(Utilities.txtDelete(Utilities.txtDelete(GEO_INFO(0), "oadMarker(parseFloat("), ")")))
                                        Geo.Add(Trim(Utilities.txtDelete(Utilities.txtDelete(GEO_INFO(1), "parseFloat("), ")")))
                                    End If
                                End If
                                calltmp.setGeo(Geo)
                            End If
                        End If
                    Next
                    AddOrUpdateCall(calltmp)

                    For UI As Integer = 0 To UnitList(i).UnitList.Count - 1
                        Dim CACHED_TIME As New List(Of String)
                        Dim NEW_TIME As New List(Of String)
                        For Each incident In database
                            For Each U In incident.IncidentsUnits
                                If U.getUnit = UnitList(i).UnitList(UI).getGUID Then
                                    CACHED_TIME = U.getTime
                                End If
                            Next
                        Next

                        If CACHED_TIME.Count = 0 Then
                            CACHED_TIME.Add("00:00:00")
                            CACHED_TIME.Add("00:00:00")
                            CACHED_TIME.Add("00:00:00")
                            CACHED_TIME.Add("00:00:00")
                        End If

                        If Not UnitList(i).UnitList(UI).getTime(0) = "NA" Then
                            NEW_TIME.Add(UnitList(i).UnitList(UI).getTime(0))
                            NEW_TIME.Add(CACHED_TIME(1))
                            NEW_TIME.Add(CACHED_TIME(2))
                            NEW_TIME.Add(CACHED_TIME(3))
                        ElseIf Not UnitList(i).UnitList(UI).getTime(1) = "NA" Then
                            NEW_TIME.Add(CACHED_TIME(0))
                            NEW_TIME.Add(UnitList(i).UnitList(UI).getTime(1))
                            NEW_TIME.Add(CACHED_TIME(2))
                            NEW_TIME.Add(CACHED_TIME(3))
                        ElseIf Not UnitList(i).UnitList(UI).getTime(2) = "NA" Then
                            NEW_TIME.Add(CACHED_TIME(0))
                            NEW_TIME.Add(CACHED_TIME(1))
                            NEW_TIME.Add(UnitList(i).UnitList(UI).getTime(2))
                            NEW_TIME.Add(CACHED_TIME(3))
                        ElseIf Not UnitList(i).UnitList(UI).getTime(3) = "NA" Then
                            NEW_TIME.Add(CACHED_TIME(0))
                            NEW_TIME.Add(CACHED_TIME(1))
                            NEW_TIME.Add(CACHED_TIME(2))
                            NEW_TIME.Add(UnitList(i).UnitList(UI).getTime(3))
                        Else
                            NEW_TIME.Add("00:00:00")
                            NEW_TIME.Add("00:00:00")
                            NEW_TIME.Add("00:00:00")
                            NEW_TIME.Add("00:00:00")
                        End If

                        UnitList(i).UnitList(UI).setTime(NEW_TIME)
                        UnitList(i).UnitList(UI).setGUID(calltmp.getGUID)
                        UnitList(i).UnitList(UI).setCounty(calltmp.getCounty)

                        ' OK because this was setup originally to handle units on a different page
                        ' We need to search for the call in the database.

                        For Each incident In database
                            If incident = calltmp Then
                                AddOrUpdateUnit(incident, UnitList(i).UnitList(UI))
                                Exit For
                            End If
                        Next
                    Next
                Next
                ' PraseCalls_OLD(WCCCA, "W", PITS)
                '  PraseCalls_OLD(CCOM, "C", PITS)
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            ' Get stack trace for the exception with source file information
            Dim st = New StackTrace(ex, True)
            ' Get the top stack frame
            Dim frame = st.GetFrame(0)
            ' Get the line number from the stack frame
            Dim line = frame.ToString
            Utilities.Log("error", "NonAPI_ProcessCalls: " & ex.Message & " | " & frame.ToString)
            Return False
        End Try
        Return False
    End Function

    ' UNUSED: For future reference only.
    Private Sub PraseCalls_OLD(ByVal ARR As String(), county As String, PITS As String)
        Try
            If ARR.Length > 0 Then
                For i As Integer = 0 To ARR.Count - 1
                    If String.IsNullOrWhiteSpace(ARR(i)) = False Then
                        Dim [call] As New Oregon_911_Mainframe.calls
                        [call].setCounty(county)
                        [call].setGUID(Trim(Utilities.txtDelete(Utilities.txtDelete(Utilities.dtrim(Utilities.dtrim(ARR(i), "Time:", 1), "GUID: ", 0), "Time:"), "GUID: ").ToString.ToUpper))
                        Dim calltype = Utilities.txtDelete(Utilities.dtrim(ARR(i), " Address:", 1), " Address:").ToString.ToUpper
                        If calltype.ToUpper = "*BLOCKING" Then
                            calltype = calltype.Replace("*", Nothing)
                        End If
                        [call].setCallType(calltype)
                        [call].setAddress(Trim(Utilities.txtDelete(Utilities.txtDelete(Utilities.dtrim(Utilities.dtrim(ARR(i), "Address:", 0), " GUID:", 1), "Address: "), " GUID:").ToString.ToUpper))
                        Dim TMP = Utilities.txtDelete(Utilities.dtrim(ARR(i), "Time: ", 0), "Time: ").ToString.Split(" ")
                        Dim TIME_TMP As New List(Of String)
                        For index As Integer = 0 To 4
                            If Not String.IsNullOrWhiteSpace(TMP(index)) Then
                                If TMP(index) = "--:--:--" Then
                                    TIME_TMP.Add("00:00:00")
                                Else
                                    TIME_TMP.Add(TMP(index))
                                End If
                            End If
                        Next
                        [call].setTime(TIME_TMP)
                        [call].setAgency(Utilities.txtDelete(TMP(6), "/"), useAPI)
                        [call].setStation(Utilities.txtDelete(TMP(6), " "))
                        [call].setUnits(Trim(Utilities.txtDelete(dtrim(ARR(i), "Units: ", 0), "Units: ")))
                        [call].setPriority("LOW")

                        Dim GEO_STRING = Utilities.dtrim(Utilities.dtrim(PITS, "LoadMarker(parseFloat(", 0), "updateMarkers();", 1).ToString.Split("LoadMarker(")

                        For Each item In GEO_STRING
                            If Not String.IsNullOrWhiteSpace(item) Then
                                Dim GEO_INFO = Utilities.dtrim(item, ";", 1).ToString.Split("', '")
                                If Trim(Utilities.txtDelete(GEO_INFO(3), "'")) = [call].getGUID Then
                                    Dim Geo As New List(Of Double)
                                    If county = "W" Then
                                        If GEO_INFO(5).Contains("wccca") Then
                                            GEO_INFO = GEO_INFO(0).ToString.Split(",")
                                            Geo.Add(Trim(Utilities.txtDelete(Utilities.txtDelete(GEO_INFO(0), "oadMarker(parseFloat("), ")")))
                                            Geo.Add(Trim(Utilities.txtDelete(Utilities.txtDelete(GEO_INFO(1), "parseFloat("), ")")))
                                        End If
                                    Else
                                        If GEO_INFO(5).Contains("ccom") Then
                                            GEO_INFO = GEO_INFO(0).ToString.Split(",")
                                            Geo.Add(Trim(Utilities.txtDelete(Utilities.txtDelete(GEO_INFO(0), "oadMarker(parseFloat("), ")")))
                                            Geo.Add(Trim(Utilities.txtDelete(Utilities.txtDelete(GEO_INFO(1), "parseFloat("), ")")))
                                        End If
                                    End If
                                    [call].setGeo(Geo)
                                End If
                            End If
                        Next
                        AddOrUpdateCall([call])
                    End If
                Next
            End If
        Catch ex As Exception
            ' Get stack trace for the exception with source file information
            Dim st = New StackTrace(ex, True)
            ' Get the top stack frame
            Dim frame = st.GetFrame(0)
            ' Get the line number from the stack frame
            Dim line = frame.ToString
            Utilities.Log("error", "PraseCalls: " & ex.Message & " | " & frame.ToString)
        End Try
    End Sub
    Private Sub NonAPI_PraseUnits_OLD()
        Try
            For Each incident In database
                Dim temp = ""
                Dim Unit_HTML = Utilities.HTTP("http://www.wccca.com/PITSv2/units.aspx?cn=" & incident.getGUID & "&a=" & incident.getCounty.ToString.ToLower & "&ct=OR911_IMPORT", "POST", "")
                If Unit_HTML = "ERROR" = False And Not Unit_HTML = Nothing Then
                    If Unit_HTML Is Nothing = False Then
                        Dim doc As New HtmlDocument()
                        doc.LoadHtml(Unit_HTML)
                        For Each table As HtmlNode In doc.DocumentNode.SelectNodes("//table")
                            For Each row As HtmlNode In table.SelectNodes("tr")

                                temp += incident.getGUID & " " & incident.getCounty & " "
                                For Each cell As HtmlNode In row.SelectNodes("th|td")
                                    If cell.InnerText = "Unit" = False And cell.InnerText = "Dispatch" = False And cell.InnerText = "En Route" = False And cell.InnerText = "On Scene" = False And cell.InnerText = "Clear" = False Then
                                        If String.IsNullOrWhiteSpace(cell.InnerText) = False Then
                                            temp += cell.InnerText.Replace("&nbsp;", "00:00:00") & " "
                                        End If
                                    End If
                                Next
                                temp += vbNewLine
                            Next
                        Next
                    End If
                End If
                Dim temp_split = temp.Split(vbCrLf)
                For i As Integer = 1 To temp_split.Length - 2
                    Dim UNIT_INFO = Utilities.txtDelete(temp_split(i), vbLf).ToString.Split(" ")
                    If UNIT_INFO(0) = incident.getGUID And UNIT_INFO(1) = incident.getCounty Then
                        Dim UNIT As New Oregon_911_Mainframe.units
                        UNIT.setGUID(UNIT_INFO(0))
                        UNIT.setCounty(UNIT_INFO(1))
                        Dim Time As New List(Of String)
                        For Index As Integer = 3 To 6
                            Time.Add(UNIT_INFO(Index))
                        Next
                        UNIT.setTime(Time)
                        UNIT.setUnit(UNIT_INFO(2))
                        AddOrUpdateUnit(incident, UNIT)
                    End If
                Next
            Next
        Catch ex As Exception
            ' Get stack trace for the exception with source file information
            Dim st = New StackTrace(ex, True)
            ' Get the top stack frame
            Dim frame = st.GetFrame(0)
            ' Get the line number from the stack frame
            Dim line = frame.ToString
            Utilities.Log("error", "NonAPI_PraseUnits: " & ex.Message & " | " & frame.ToString)
        End Try
    End Sub

#End Region

#Region "Private Functions"
    Private Sub AddOrUpdateCall(ByVal [call] As Oregon_911_Mainframe.calls)
        Try
            Dim found As Boolean = False

            For Each incident In database
                If [call] = incident Then
                    found = True
                    If Not incident.getOverWrite Then
                        If Not (incident.getGeo(0) = [call].getGeo(0) And incident.getGeo(1) = [call].getGeo(1)) Then
                            incident.addGeoHistory([call].getGeo)
                        End If
                        incident.setGeo([call].getGeo)
                        If Not [call].getAddress = incident.getAddress Then
                            incident.AddAddressHistory(incident.getAddress)
                        End If
                        incident.setAddress([call].getAddress)
                    End If
                    incident.setAgency([call].getAgency, useAPI)
                    If Not [call].getCallType = incident.getCallType Then
                        incident.AddCallTypeHistory(incident.getCallType)
                    End If
                    incident.setCallType([call].getCallType)
                    incident.setTime([call].getTime)
                    incident.setUnits([call].getUnits)
                    GUID.Add([call].getGUID)
                    County.Add([call].getCounty)
                    Exit For
                End If
            Next

            If Not found Then
                GUID.Add([call].getGUID)
                County.Add([call].getCounty)
                database.Add([call])
            End If
        Catch ex As Exception
            ' Get stack trace for the exception with source file information
            Dim st = New StackTrace(ex, True)
            ' Get the top stack frame
            Dim frame = st.GetFrame(0)
            ' Get the line number from the stack frame
            Dim line = frame.ToString
            Utilities.Log("error", "AddOrUpdateCall: " & ex.Message & " | " & frame.ToString)
        End Try
    End Sub
    Private Sub AddOrUpdateUnit(ByVal [call] As Oregon_911_Mainframe.calls, ByVal unit As Oregon_911_Mainframe.units)
        Try
            For Each incident In database
                If [call] = incident Then
                    Dim found As Boolean = False

                    For Each incidentUnit In [call].IncidentsUnits
                        If incidentUnit.getUnit = unit.getUnit And incidentUnit.getCounty = unit.getCounty Then
                            incidentUnit.setTime(unit.getTime)
                            incident.setUnitCount(incident.IncidentsUnits.Count)
                            Dim unit_tmp As New Unit_Tmp
                            unit_tmp.UNIT = incidentUnit.getUnit
                            unit_tmp.GUID = incidentUnit.getGUID
                            unit_tmp.County = incidentUnit.getCounty
                            Temp_units.Add(unit_tmp)
                            found = True
                            Exit For
                        End If
                    Next

                    If found = True Then
                        Exit For
                    Else
                        ' Prevents high load time with requesting API over and over again per refresh
                        If unit.getAgency = Nothing Then
                            Dim Agency = API.GetUnitAgency(unit.getUnit, unit.getCounty)
                            If Not IsNothing(Agency) Then
                                unit.setAgency(Agency.Item(0).Item("AGENCY"))
                            Else
                                unit.setAgency("Unknown")
                            End If
                        End If

                        If unit.getStation = Nothing Then
                            Dim Station = API.GetUnitsStation(unit.getUnit, unit.getCounty)
                            If Not IsNothing(Station) Then
                                unit.setStation(Station.Item(0).Item("ABBV"))
                            Else
                                unit.setStation("Unknown")
                            End If
                        End If

                        incident.setUnitCount(incident.IncidentsUnits.Count)
                        Dim unit_tmp As New Unit_Tmp
                        unit_tmp.UNIT = unit.getUnit
                        unit_tmp.GUID = unit.getGUID
                        unit_tmp.County = unit.getCounty
                        Temp_units.Add(unit_tmp)
                        incident.IncidentsUnits.Add(unit)
                        found = True
                        Exit For
                    End If
                End If
            Next
        Catch ex As Exception
            ' Get stack trace for the exception with source file information
            Dim st = New StackTrace(ex, True)
            ' Get the top stack frame
            Dim frame = st.GetFrame(0)
            ' Get the line number from the stack frame
            Dim line = frame.ToString
            Utilities.Log("error", "AddOrUpdateUnit: " & ex.Message & " | " & frame.ToString)
        End Try
    End Sub
    Private Sub CleanCalls()
        Try
            Dim ToBeRemovedCalls As New List(Of Integer)
            For i As Integer = 0 To database.Count - 1
                ' Find old calls and prune them.
                Dim call_found = False
                For index As Integer = 0 To GUID.Count - 1
                    If DirectCast(database([i]), calls).getGUID() = GUID(index) And DirectCast(database([i]), calls).getCounty() = County(index) Then
                        call_found = True
                        Exit For
                    End If
                Next
                If Not call_found Then
                    ToBeRemovedCalls.Add(i)
                End If

                ' Find units that were deleted from the call and prune them.
                Dim ToBeRemovedUnits As New List(Of Integer)
                If database.Count > 0 Then
                    For index As Integer = 0 To DirectCast(database([i]), calls).IncidentsUnits.Count - 1
                        Dim unit_found As Boolean = False
                        For Each unit In Temp_units
                            If unit.UNIT = DirectCast(database([i]), calls).IncidentsUnits(index).getUnit Then
                                unit_found = True
                                Exit For
                            End If
                        Next
                        If Not unit_found Then
                            ToBeRemovedUnits.Add(index)
                        End If

                        ' Wait til the end to delete! Else we might go out of bounds or delete the wrong item.
                        If ToBeRemovedUnits.Count > 0 Then
                            If index = DirectCast(database([i]), calls).IncidentsUnits.Count - 1 Then
                                For idex = ToBeRemovedUnits.Count - 1 To 0 Step -1
                                    DirectCast(database([i]), calls).IncidentsUnits.RemoveAt(ToBeRemovedUnits(idex))
                                Next
                                ToBeRemovedUnits.Clear()
                            End If
                        End If
                    Next
                End If
            Next

            If GUID.Count = 0 Then
                database.Clear()
            Else
                If ToBeRemovedCalls.Count = database.Count Then
                    database.Clear()
                Else
                    For idex = ToBeRemovedCalls.Count - 1 To 0 Step -1
                        database.RemoveAt(ToBeRemovedCalls(idex))
                    Next
                End If
            End If
        Catch ex As Exception
            ' Get stack trace for the exception with source file information
            Dim st = New StackTrace(ex, True)
            ' Get the top stack frame
            Dim frame = st.GetFrame(0)
            ' Get the line number from the stack frame
            Dim line = frame.ToString
            Utilities.Log("error", "CleanCalls: " & ex.Message & " | " & frame.ToString)
        End Try
    End Sub

#End Region

End Class