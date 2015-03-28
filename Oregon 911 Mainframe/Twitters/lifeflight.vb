Imports Newtonsoft.Json.Linq

Module lifeflight
    Dim database As incidents
    Private NULL = "00:00:00"

    ' This module uses almost the same methods as CADAlerts... maybe merge them for proformance? But that'll be messy and gross!
    Public Sub Algorithm(ByRef db As incidents)
        Try
            database = db
            If Not database.Updating Then
                For Each incident In database.database
                    If Not incident.getCounty = "M" Then
                        Dim Standby As Boolean = False
                        Dim Activated As Boolean = False
                        Dim StandDown As Boolean = False
                        Dim Helicopters As Integer = 0
                        Dim Reach As Boolean = False
                        Dim FlagsRaw As JArray = API.GetFlags(incident.getGUID, incident.getCounty)
                        Dim flags
                        If Not IsNothing(FlagsRaw) Then
                            flags = FlagsRaw.Item(0).Item("flags").ToString.Split(";")
                        Else
                            flags = ""
                        End If

                        For Each unit In incident.IncidentsUnits
                            If unit.getUnit.ToUpper = "LFSTBY" Then
                                If Not unit.getTime(3) = NULL Then
                                    StandDown = True
                                Else
                                    Standby = True
                                    StandDown = False
                                    Helicopters += 1
                                End If
                            ElseIf unit.getUnit.ToUpper.Contains("LIFE") Then
                                If Not unit.getTime(3) = NULL And unit.getTime(2) = NULL Then
                                    StandDown = True
                                Else
                                    If (unit.getTime(1) = NULL = False Or unit.getTime(2) = NULL = False) Then
                                        Activated = True
                                        StandDown = False
                                        Helicopters += 1
                                    Else
                                        Standby = True
                                        StandDown = False
                                        Helicopters += 1
                                    End If
                                End If
                            ElseIf unit.getUnit.ToUpper.Contains("LF") Then
                                If Not unit.getTime(3) = NULL And unit.getTime(2) = NULL Then
                                    StandDown = True
                                Else
                                    If (unit.getTime(1) = NULL = False Or unit.getTime(2) = NULL = False) Then
                                        Activated = True
                                        StandDown = False
                                        Helicopters += 1
                                    Else
                                        Standby = True
                                        StandDown = False
                                        Helicopters += 1
                                    End If
                                End If
                            ElseIf unit.getUnit.ToUpper.Contains("REACH") Then
                                If Not unit.getTime(3) = NULL And unit.getTime(2) = NULL Then
                                    StandDown = True
                                    Reach = True
                                Else
                                    If (unit.getTime(1) = NULL = False Or unit.getTime(2) = NULL = False) Then
                                        Activated = True
                                        Reach = True
                                        StandDown = False
                                        Helicopters += 1
                                    Else
                                        Standby = True
                                        Reach = True
                                        StandDown = False
                                        Helicopters += 1
                                    End If
                                End If
                            End If
                        Next

                        If Activated Then
                            If CheckCall(FlagsRaw, flags, "LFACT") Then
                                If Reach Then
                                    SendAlert("REACH ACTIVATED for ", "LFACT", "helicopter.png", incident)
                                Else
                                    SendAlert("ACTIVATED for ", "LFACT", "helicopter.png", incident)
                                End If
                            End If
                        ElseIf Standby Then
                            If CheckCall(FlagsRaw, flags, "LFSTBY") Then
                                If Reach Then
                                    SendAlert("REACH STAND BY for ", "LFSTBY", "helicopter.png", incident)
                                Else
                                    SendAlert("STAND BY for ", "LFSTBY", "helicopter.png", incident)
                                End If
                            End If
                        ElseIf StandDown Then
                            ' If Not CheckStandDown(incident.getGUID, incident.getCounty) Then
                            If CheckCall(FlagsRaw, flags, "LFSTANDWN") Then
                                If Reach Then
                                    SendAlert("REACH STAND DOWN for ", "LFSTANDWN", "helicopter.png", incident)
                                Else
                                    SendAlert("STAND DOWN for ", "LFSTANDWN", "helicopter.png", incident)
                                End If
                            End If
                            'End If
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
            Utilities.Log("error", "lifeflight.vb: " & ex.Message & " | " & frame.ToString)
        End Try
    End Sub
    Private Function CheckStandDown(ByVal GUID, county) As Boolean
        Dim result As JArray = API.Query("select onscene from oregon911_cad.pdx911_units WHERE GUID = '" & GUID & "' AND county ='" & county & "' and (unit LIKE 'LF%' OR unit like 'LIFE%' OR unit like 'REACH%' or unit LIKE 'LFSTBY' ")
        If Not IsNothing(result) Then
            If result.Item(0).Item("onscene") Then
                Return False
            End If
        End If
        Return True
    End Function

    Private Function SendAlert(ByVal Responce As String, EventID As String, icon As String, incident As calls) As Boolean
        Dim countyIcon
        If incident.getCounty = "WCCCA" Then
            countyIcon = "W"
        ElseIf incident.getCounty = "CCOM" Then
            countyIcon = "C"
        End If
        Dim result = API.Query("UPDATE oregon911_cad.pdx911_calls SET flags=concat(ifnull(flags,''), '" & EventID & ";') WHERE GUID = '" & incident.getGUID & "' AND county ='" & incident.getCounty & "'")
        If Not IsNothing(result) Then
            Dim ccounty = ""
            If incident.getCounty = "C" Then
                ccounty = "Clackamas County"
            ElseIf incident.getCounty = "W" Then
                ccounty = "Washington County"
            ElseIf incident.getCounty = "M" Then
                ccounty = "Multnomah County"
            End If
            API.PDXLifeflight(Responce & ccounty & " at " & incident.getAddress, incident.getGeo(0), incident.getGeo(1), Utilities.CreateUnitURL(incident.getGUID, incident.getCounty))
            Return True
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