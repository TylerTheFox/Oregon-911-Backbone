Imports Newtonsoft.Json.Linq

Module places
    Dim database As incidents

    Public Sub Algorithm(ByRef db As incidents)
        Try
            database = db
            If Not database.Updating Then
                For Each incident In database.database
                    Dim AccountInfomationRAW As JArray = API.GetPlacesInfomation(incident)
                    If Not IsNothing(AccountInfomationRAW) And Not incident.getCounty = "M" Then
                        For i = 0 To AccountInfomationRAW.Count - 1
                            Dim USR_ID As Integer = AccountInfomationRAW.Item(i).Item("FK_user_id")
                            Dim phoneNum As String = AccountInfomationRAW.Item(i).Item("pf_phone")
                            Dim CarrierIndex As Integer = AccountInfomationRAW.Item(i).Item("pf_carrier")
                            Dim Email As String = AccountInfomationRAW.Item(i).Item("user_email")
                            Dim Name As String = AccountInfomationRAW.Item(i).Item("Name")
                            Dim distance As Integer = AccountInfomationRAW.Item(i).Item("distance")
                            Dim mail As String = AccountInfomationRAW.Item(i).Item("email")
                            Dim txt As String = AccountInfomationRAW.Item(i).Item("txt")
                            Dim LATLON As New List(Of Double)
                            LATLON.Add(AccountInfomationRAW.Item(i).Item("lat"))
                            LATLON.Add(AccountInfomationRAW.Item(i).Item("lon"))
                            If Utilities.getDistance(LATLON, incident.getGeo.ToList) <= distance Then
                                API.SendPlacesAlert(incident, USR_ID, Name, phoneNum, CarrierIndex, Email, mail, txt)
                            End If
                        Next
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
            Utilities.Log("error", "places.vb: " & ex.Message & " | " & frame.ToString)
        End Try
    End Sub

End Module