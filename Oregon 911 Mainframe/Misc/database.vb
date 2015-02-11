Imports Newtonsoft.Json.Linq

Module db
    Dim database As incidents

    Public Sub UpdateDatabase(ByRef db As incidents)
        Try
            database = db
            Dim t = DateTime.Now.ToString("HH:mm:ss")
            If Not database.Updating Then
                Dim PITS As JArray = API.ListCalls
                Dim UnitsRaw As JArray = API.ListUnits

                ' Delete / Archive Old Data 

                If Not IsNothing(PITS) Then
                    If Not PITS.Item(0).ToString = "NONE" Then
                        For i = 0 To PITS.Count - 1
                            Dim found As Boolean = False

                            For Each incident In database.database
                                If PITS.Item(i).Item("GUID") = incident.getGUID And PITS.Item(i).Item("county") = incident.getCounty Then
                                    found = True
                                    Exit For
                                End If
                            Next
                            If Not found Then
                                Dim x As JArray = API.Query("INSERT INTO oregon911_cad.pdx911_archive( GUID, county, callSum, priority, station, units, unitcont, address, agency, TYPE , lat, lon, icon, notes, flags, TIMESTAMP ) " & _
                                            "SELECT GUID, county, callSum, priority, station, units, unitcont, address, agency, TYPE , lat, lon, icon, notes, flags, TIMESTAMP FROM oregon911_cad.pdx911_calls" & _
                                            " WHERE GUID ='" & PITS.Item(i).Item("GUID").ToString & "' and county = '" & PITS.Item(i).Item("county").ToString & "'")
                                If Not IsNothing(x) Then
                                    API.Query("DELETE FROM oregon911_cad.pdx911_calls WHERE GUID ='" & PITS.Item(i).Item("GUID").ToString & "' and county = '" & PITS.Item(i).Item("county").ToString & "'")
                                    API.Query("UPDATE oregon911_cad.pdx911_units SET clear='" & t & "' WHERE GUID ='" & PITS.Item(i).Item("GUID").ToString & "' and county = '" & PITS.Item(i).Item("county").ToString & "' AND clear='00:00:00'")
                                Else
                                    ' If call exists in the archive, remove it!
                                    API.Query("DELETE FROM oregon911_cad.pdx911_archive WHERE GUID ='" & PITS.Item(i).Item("GUID").ToString & "' and county = '" & PITS.Item(i).Item("county").ToString & "'")
                                End If
                            End If
                        Next
                    End If

                    ' Update All existing infomation  
                    For Each incident In database.database
                        Dim countyIcon
                        If incident.getCounty = "W" Then
                            countyIcon = "WCCCA"
                        ElseIf incident.getCounty = "C" Then
                            countyIcon = "CCOM"
                        ElseIf incident.getCounty = "M" Then
                            countyIcon = "MULTCO"
                        End If

                        Dim iconRAW As JArray
                        Dim icon As String
                        If incident.getCounty = "W" And incident.getCallType = "TRF ACC, NON-INJ" Then
                            incident.setType("P")
                            icon = "police_caraccident.png"
                        ElseIf incident.getCounty = "M" And incident.getType = "P" Then
                            icon = "police.png"

                        ElseIf incident.getCounty = "M" And incident.getType = "F" Then
                            icon = "fire.png"
                        Else

                            iconRAW = API.GetCallIcon(incident)

                            If Not IsNothing(iconRAW) Then
                                If Not iconRAW.Item(0).Item("icon").ToString = Nothing Then
                                    incident.setType(iconRAW.Item(0).Item("type"))
                                    icon = iconRAW.Item(0).Item("icon")
                                Else
                                    incident.setType("F")
                                    icon = "general.png"
                                End If
                            Else
                                incident.setType("F")
                                icon = "general.png"
                            End If

                        End If

                        Dim found = False
                        For i = 0 To PITS.Count - 1
                            If Not PITS.Item(0).ToString = "NONE" Then
                                If PITS.Item(i).Item("GUID") = incident.getGUID And PITS.Item(i).Item("county") = incident.getCounty Then
                                    found = True
                                    API.Query("UPDATE oregon911_cad.pdx911_calls SET callSum='" & incident.getCallType & "', unitcont='" & incident.GetUnitCount & "', address='" & incident.getAddress & "', units='" & incident.getUnits & "', lat='" & incident.getGeo(0) & "', lon='" & incident.getGeo(1) & "' WHERE GUID='" & incident.getGUID & "' and county='" & incident.getCounty & "'")

                                    Dim UnitStr = ""
                                    For Each unit In incident.IncidentsUnits
                                        UnitStr += "'" & unit.getUnit & "',"
                                        Dim unit_found As Boolean = False
                                        If Not IsNothing(UnitsRaw) Then
                                            If Not UnitsRaw.Item(0).ToString = "NONE" Then
                                                For index = 0 To UnitsRaw.Count - 1
                                                    If UnitsRaw.Item(index).Item("GUID") = unit.getGUID And UnitsRaw.Item(index).Item("county") = unit.getCounty And UnitsRaw.Item(index).Item("unit") = unit.getUnit Then
                                                        unit_found = True
                                                        '    If unit.getCounty = "M" Then
                                                        If Not unit.getTime(3) = "00:00:00" Then
                                                            API.Query("UPDATE oregon911_cad.pdx911_units SET type = '" & incident.getType() & "', agency='" & unit.getAgency & "', county='" & unit.getCounty & "', station='" & unit.getStation & "', clear='" & unit.getTime(3) & "' WHERE GUID='" & unit.getGUID & "' AND unit='" & unit.getUnit & "' AND county='" & unit.getCounty & "'")
                                                        ElseIf Not unit.getTime(2) = "00:00:00" Then
                                                            API.Query("UPDATE oregon911_cad.pdx911_units SET type = '" & incident.getType() & "', agency='" & unit.getAgency & "', county='" & unit.getCounty & "', station='" & unit.getStation & "', onscene='" & unit.getTime(2) & "' WHERE GUID='" & unit.getGUID & "' AND unit='" & unit.getUnit & "' AND county='" & unit.getCounty & "'")
                                                        ElseIf Not unit.getTime(1) = "00:00:00" Then
                                                            API.Query("UPDATE oregon911_cad.pdx911_units SET type = '" & incident.getType() & "', agency='" & unit.getAgency & "', county='" & unit.getCounty & "', station='" & unit.getStation & "', enroute='" & unit.getTime(1) & "' WHERE GUID='" & unit.getGUID & "' AND unit='" & unit.getUnit & "' AND county='" & unit.getCounty & "'")
                                                        ElseIf Not unit.getTime(0) = "00:00:00" Then
                                                            API.Query("UPDATE oregon911_cad.pdx911_units SET type = '" & incident.getType() & "', agency='" & unit.getAgency & "', county='" & unit.getCounty & "', station='" & unit.getStation & "', dispatched='" & unit.getTime(0) & "', enroute='" & unit.getTime(1) & "', onscene='" & unit.getTime(2) & "', clear='" & unit.getTime(3) & "' WHERE GUID='" & unit.getGUID & "' AND unit='" & unit.getUnit & "' AND county='" & unit.getCounty & "'")
                                                        End If
                                                        'Else
                                                        '     API.Query("UPDATE oregon911_cad.pdx911_units SET type = '" & incident.getType() & "', agency='" & unit.getAgency & "', county='" & unit.getCounty & "', station='" & unit.getStation & "', dispatched='" & unit.getTime(0) & "', enroute='" & unit.getTime(1) & "', onscene='" & unit.getTime(2) & "', clear='" & unit.getTime(3) & "' WHERE GUID='" & unit.getGUID & "' AND unit='" & unit.getUnit & "' AND county='" & unit.getCounty & "'")
                                                        ' End If
                                                    End If
                                                Next
                                            End If
                                        End If
                                        If Not unit_found Then
                                            API.Query("INSERT INTO oregon911_cad.pdx911_units (GUID, county, unit, type, agency, station, dispatched, enroute, onscene, clear) VALUES " & _
                                                                 " ('" & unit.getGUID & "', '" & unit.getCounty & "', '" & unit.getUnit & "', '" & incident.getType & "', '" & unit.getAgency & "', '" & unit.getStation & "', '" & unit.getTime(0) & "', '" & unit.getTime(1) & "', '" & unit.getTime(2) & "', '" & unit.getTime(3) & "')")
                                        End If
                                    Next
                                    If incident.IncidentsUnits.Count > 0 Then
                                        UnitStr = UnitStr.Trim().Substring(0, UnitStr.Length - 1)
                                        API.Query("UPDATE oregon911_cad.pdx911_units SET clear='" & t & "' WHERE GUID ='" & PITS.Item(i).Item("GUID").ToString & "' and county = '" & PITS.Item(i).Item("county").ToString & "' AND unit NOT IN (" & UnitStr & ") AND clear='00:00:00'")
                                    End If
                                End If
                            End If
                        Next

                        If Not found Then
                            Dim d As String = Date.Now.Year & "-" & Date.Now.Month & "-" & Date.Now.Day
                            API.Query("INSERT INTO oregon911_cad.pdx911_calls (GUID, county, callSum, type, priority, unitcont, station, units, address, agency, lat, lon, icon, timestamp) VALUES " & _
                                                                          "('" & incident.getGUID & "','" & incident.getCounty & "','" & incident.getCallType & "','" & incident.getType & "','" & incident.GetPriority & "','" & incident.GetUnitCount & "','" & incident.getStation & "','" & incident.getUnits & "','" & incident.getAddress & "','" & incident.getAgencyName & "','" & incident.getGeo(0) & "','" & incident.getGeo(1) & "'," & "'/images/" & countyIcon & "/" & icon & "','" & d & " " & incident.getTime()(0) & "')")
                            For Each unit In incident.IncidentsUnits

                                API.Query("INSERT INTO oregon911_cad.pdx911_units (GUID, county, unit, agency, station, dispatched, enroute, onscene, clear) VALUES " & _
                                                                      " ('" & unit.getGUID & "', '" & unit.getCounty & "', '" & unit.getUnit & "', '" & unit.getAgency & "', '" & unit.getStation & "', '" & unit.getTime(0) & "', '" & unit.getTime(1) & "', '" & unit.getTime(2) & "', '" & unit.getTime(3) & "')")
                            Next
                        End If

                        Dim CallTypeHistory As List(Of String) = incident.getCallTypeHistory.ToList
                        For Each calltype_history In CallTypeHistory
                            If Not IsNothing(API.Query("INSERT INTO `oregon911_cad`.`pdx911_records` (`GUID`, `county`, `callSum`, `address`, `lat`, `lon`, `update`) VALUES ('" & incident.getGUID & "', '" & incident.getCounty & "', '" & calltype_history & "', '" & incident.getAddress & "', '" & incident.getGeo()(0) & "', '" & incident.getGeo()(1) & "', '1')")) Then
                                incident.RemoveCallTypeHistory(calltype_history)
                            End If
                        Next
                        Dim GPSHistory = incident.getGeoHistory.ToList
                        For Each geo_history In GPSHistory
                            If Not IsNothing(API.Query("INSERT INTO `oregon911_cad`.`pdx911_records` (`GUID`, `county`, `callSum`, `address`, `lat`, `lon`, `update`) VALUES ('" & incident.getGUID & "', '" & incident.getCounty & "', '" & incident.getCallType & "', '" & incident.getAddress & "', '" & geo_history(0) & "', '" & geo_history(1) & "', '3')")) Then
                                incident.removeGeoHistory(geo_history)
                            End If
                        Next

                        Dim AddrHistory As List(Of String) = incident.getAddressHistory.ToList
                        For Each address_history In AddrHistory
                            If Not IsNothing(API.Query("INSERT INTO `oregon911_cad`.`pdx911_records` (`GUID`, `county`, `callSum`, `address`, `lat`, `lon`, `update`) VALUES ('" & incident.getGUID & "', '" & incident.getCounty & "', '" & incident.getCallType & "', '" & address_history & "', '" & incident.getGeo()(0) & "', '" & incident.getGeo()(1) & "', '2')")) Then
                                incident.RemoveAddressHistory(address_history)
                            End If
                        Next

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
            Utilities.Log("error", "database.vb: " & ex.Message & " | " & frame.ToString)
        End Try
    End Sub

End Module