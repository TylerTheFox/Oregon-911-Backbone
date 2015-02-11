Imports Newtonsoft.Json.Linq

Module cadalerts
    Dim database As incidents

    Public Sub Algorithm(ByRef db As incidents)
        Try
            database = db
            If Not database.Updating Then
                For Each incident In database.database
                    Dim AlertType As JArray = API.GetCADFlags(incident.getCallType)
                    Dim FlagsRaw As JArray = API.GetFlags(incident.getGUID, incident.getCounty)
                    Dim flags
                    If Not IsNothing(FlagsRaw) Then
                        flags = FlagsRaw.Item(0).Item("flags").ToString.Split(";")
                    Else
                        flags = ""
                    End If

                    Dim abort = False
                    If Not IsNothing(AlertType) Then
                        Dim responce As String = AlertType.Item(0).Item("Responce")
                        Dim EventID As String = AlertType.Item(0).Item("EventID")
                        Dim Icon As String = AlertType.Item(0).Item("icon")
                        If Not IsNothing(FlagsRaw) Then
                            For Each flag In flags
                                If flag = EventID Then
                                    abort = True
                                End If
                            Next
                            If Not abort Then
                                SendAlert(responce, EventID, Icon, incident)
                            End If
                        End If
                    End If

                    For Each unit In incident.IncidentsUnits
                        If unit.getUnit = "SAG" Then
                            ' Second Alarm Clackamas
                            If CheckCall(FlagsRaw, flags, "SAG") Then
                                SendAlert("Second Alarm at ", "SAG", "fire.png", incident)
                            End If
                        ElseIf unit.getUnit = "2ALMM" Then
                            ' MCI
                            If CheckCall(FlagsRaw, flags, "2ALMM") Then
                                SendAlert("Mass-casualty incident at ", "2ALMM", "EMS.png", incident)
                            End If
                        ElseIf unit.getUnit.Contains("1ALM") Then
                            ' First alarm
                            If CheckCall(FlagsRaw, flags, "1ALM") Then
                                SendAlert("First Alarm at ", "1ALM", "fire.png", incident)
                            End If
                        ElseIf unit.getUnit.Contains("2ALM") Then
                            ' Second Alarm
                            If CheckCall(FlagsRaw, flags, "2ALM") Then
                                SendAlert("Second Alarm at ", "2ALM", "fire.png", incident)
                            End If
                        ElseIf unit.getUnit.Contains("3ALM") Then
                            ' Third Alarm
                            If CheckCall(FlagsRaw, flags, "3ALM") Then
                                SendAlert("Third Alarm+ at ", "3ALM", "fire.png", incident)
                            End If
                        ElseIf unit.getUnit.Contains("HM") Then
                            ' Hazmat
                            If CheckCall(FlagsRaw, flags, "HAZMAT0") Then
                                SendAlert("Hazmat at ", "HAZMAT0", "HAZMAT.png", incident)
                            End If
                        End If
                    Next

                    Dim UnitAvgRaw As JArray = API.GetCallUnitAverage(incident.getCallType, incident.getCounty)
                    If Not IsNothing(UnitAvgRaw) Then
                        Dim UnitAvg As Integer = UnitAvgRaw.Item(0).Item("avg")

                        Dim Percent20Increase = Math.Round((UnitAvg * 20) / 100)

                        Dim UNIT_COUNT = 0
                        For Each unit In incident.IncidentsUnits
                            If unit.getTime(3) = "00:00:00" Then
                                If Not unit.getTime(1) = "00:00:00" Or Not unit.getTime(2) = "00:00:00" Then
                                    UNIT_COUNT += 1
                                End If
                            End If
                        Next

                        If ((UNIT_COUNT > (UnitAvg + Percent20Increase)) Or (UNIT_COUNT = 0 And UNIT_COUNT > 10)) And (UNIT_COUNT > 5) Then
                            If CheckCall(FlagsRaw, flags, "OVRAVG") Then
                                SendAlert("Large Incident At ", "OVRAVG", Nothing, incident)
                            End If
                        End If
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
            Utilities.Log("error", "cadalerts.vb: " & ex.Message & " | " & frame.ToString)
        End Try
    End Sub
    Private Function SendAlert(ByVal Responce As String, EventID As String, icon As String, incident As calls) As Boolean
        If Not incident.getCounty = "M" Then
            Dim countyIcon
            If incident.getCounty = "WCCCA" Then
                countyIcon = "W"
            ElseIf incident.getCounty = "CCOM" Then
                countyIcon = "C"
            End If
            Dim result = API.Query("UPDATE oregon911_cad.pdx911_calls SET flags=concat(ifnull(flags,''), '" & EventID & ";') WHERE GUID = '" & incident.getGUID & "' AND county ='" & incident.getCounty & "'")
            If Not IsNothing(result) Then
                API.CADAlerts(Responce & " " & incident.getAddress & " - " & incident.getCallType & " - " & incident.getUnits, incident.getGeo(0), incident.getGeo(1), Utilities.CreateUnitURL(incident.getGUID, incident.getCounty))
                Return True
            End If
        Else

        End If
        Return False
    End Function
    Private Function CheckCall(ByVal FlagsRaw As JArray, flags As Array, flag As String) As Boolean
        Dim abort = False
        If Not IsNothing(FlagsRaw) Then
            For Each f In flags
                If f = flag Then
                    Return False
                End If
            Next
            If Not abort Then
                Return True
            End If
        End If
        Return False
    End Function

End Module