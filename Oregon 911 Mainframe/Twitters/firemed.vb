Imports Newtonsoft.Json.Linq

Module firemed
    Dim database As incidents

    Public Sub Algorithm(ByRef db As incidents)
        Try

            database = db
            If Not database.Updating Then
                Dim PITS As JArray = API.ListCalls
                If Not IsNothing(PITS) Then
                    If Not PITS.Item(0).ToString = "NONE" Then
                        For i = 0 To PITS.Count - 1
                            Dim GUID As String = PITS.Item(i).Item("GUID")
                            Dim County As String = PITS.Item(i).Item("county")
                            Dim PostedRAW As Integer = PITS.Item(i).Item("posted")
                            Dim posted As Boolean = PostedRAW
                            For Each incident In database.database
                                If incident.getGUID = GUID And incident.getCounty = County And Not posted And Not incident.getCounty = "M" Then
                                    If Not incident.getCallType = "PRE NOTIFICATION" Then ' Ignore that bs!
                                        Dim result As JArray
                                        If incident.getCounty = "C" Then
                                            Dim countyIcon = "CCOM"
                                            Dim iconRAW As JArray = API.GetCallIcon(incident)
                                            Dim icon As String
                                            If Not IsNothing(iconRAW) Then
                                                If Not iconRAW.Item(0).Item("icon").ToString = Nothing Then
                                                    incident.setType(iconRAW.Item(0).Item("type"))
                                                    icon = iconRAW.Item(0).Item("icon")
                                                Else
                                                    incident.setType("F")
                                                    icon = "general.png"
                                                End If
                                            Else
                                                icon = "general.png"
                                            End If
                                            result = API.Query("UPDATE oregon911_cad.pdx911_calls SET posted=1, type='" & incident.getType & "', icon='/images/" & countyIcon & "/" & icon & "' WHERE GUID = '" & GUID & "' AND county ='" & County & "'")
                                            ' PDXAccidents
                                            Dim TrfAccid() = {"TRF ACC, UNK INJ", "BLOCKING", "NOT BLOCKING", "TRF ACC, INJURY", "MVA-INJURY ACCID", "TRF ACC, NON-INJ", "TAI-TRAPPED VICT", "TAI-HIGH MECHANI", "TAI-PT NOT ALERT", "MVA-UNK INJURY"}

                                            For Each accident In TrfAccid
                                                If incident.getCallType = accident Then
                                                    API.PDXAccidents(incident.getCounty & ". " & incident.getCallType & " At " & incident.getAddress & " #pdxtraffic", incident.getGeo(0), incident.getGeo(1), Utilities.CreateUnitURL(incident.getGUID, incident.getCounty))
                                                    Exit For
                                                End If
                                            Next
                                        Else
                                            result = API.Query("UPDATE oregon911_cad.pdx911_calls SET posted=1 WHERE GUID = '" & GUID & "' AND county ='" & County & "'")
                                        End If
                                        If Not IsNothing(result) Then
                                            If incident.getCounty = "C" Then
                                                API.clackco_firemed(incident.getCallType & " | " & incident.getAddress & " | " & incident.getTime(0), incident.getGeo(0), incident.getGeo(1), Utilities.CreateUnitURL(incident.getGUID, incident.getCounty))
                                            ElseIf incident.getCounty = "W" Then
                                                If incident.getCallType = "HAZARD" Or incident.getCallType = "TRF ACC, NON-INJ" Or incident.getCallType.Contains("FIREWORK") Then
                                                    API.washco_police(incident.getCallType & " | " & incident.getAddress & " | " & incident.getTime(0), incident.getGeo(0), incident.getGeo(1), Utilities.CreateUnitURL(incident.getGUID, incident.getCounty) & "&type=P")
                                                Else
                                                    API.washco_firemed(incident.getCallType & " | " & incident.getAddress & " | " & incident.getTime(0), incident.getGeo(0), incident.getGeo(1), Utilities.CreateUnitURL(incident.getGUID, incident.getCounty))
                                                End If
                                            End If
                                            ' PDXAccidents
                                            Dim TrfAccid() = {"TRF ACC, UNK INJ", "BLOCKING", "NOT BLOCKING", "TRF ACC, INJURY", "MVA-INJURY ACCID", "TRF ACC, NON-INJ", "TAI-TRAPPED VICT", "TAI-HIGH MECHANI", "TAI-PT NOT ALERT", "MVA-UNK INJURY"}

                                            For Each accident In TrfAccid
                                                If incident.getCallType = accident Then
                                                    API.PDXAccidents(incident.getCounty & ". " & incident.getCallType & " At " & incident.getAddress & " #pdxtraffic", incident.getGeo(0), incident.getGeo(1), Utilities.CreateUnitURL(incident.getGUID, incident.getCounty))
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                    End If
                                End If
                            Next
                        Next
                    End If
                End If
            End If
        Catch ex As Exception
            ' Get stack trace for the exception with source file information
            Dim st = New StackTrace(ex, True)
            ' Get the top stack frame
            Dim frame = st.GetFrame(0)
            ' Get the line number from the stack frame
            Dim line = frame.ToString
            Utilities.Log("error", "firemed.vb: " & ex.Message & " | " & frame.ToString)
        End Try
    End Sub

End Module
