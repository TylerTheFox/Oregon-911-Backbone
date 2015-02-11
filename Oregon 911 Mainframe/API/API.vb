Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module API
    ' Private API_KEY = "API_KEY"
    Private API_KEY = My.Computer.FileSystem.ReadAllText("key.txt")
#Region "Basic Functions"
    Public Function Status() As Boolean
        Dim result = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=status" & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        If Not result.ToUpper.Contains("ERROR") Then
            Dim responce As JObject = JObject.Parse(Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=status" & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", ""))
            If responce.SelectToken("status").SelectToken("value") = "OK" Then
                Return True
            End If
        End If
        Return False
    End Function
    Public Function ListCalls() As JArray
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=listcalls" & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            JSON = JArray.Parse(result)
        Else
            Return Nothing
        End If
        If JSON.Count = 0 Then
            JSON.Add("NONE")
        End If
        Return JSON
    End Function
    Public Function ListUnits() As JArray
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=listunits" & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            JSON = JArray.Parse(result)
        Else
            Return Nothing
        End If
        If JSON.Count = 0 Then
            JSON.Add("NONE")
        End If
        Return JSON
    End Function
    Public Function ListHospitals() As JArray
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=listhospitals" & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            JSON = JArray.Parse(result)
        Else
            Return Nothing
        End If
        If JSON.Count = 0 Then
            Return Nothing
        End If
        Return JSON
    End Function
    Public Function ListLifeFlight() As JArray
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=listlifeflight" & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            JSON = JArray.Parse(result)
        Else
            Return Nothing
        End If
        If JSON.Count = 0 Then
            Return Nothing
        End If
        Return JSON
    End Function
    Public Function ListSchools() As JArray
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=listschools" & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            JSON = JArray.Parse(result)
        Else
            Return Nothing
        End If
        If JSON.Count = 0 Then
            Return Nothing
        End If
        Return JSON
    End Function
    Public Function ListStations() As JArray
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=liststations" & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            JSON = JArray.Parse(result)
        Else
            Return Nothing
        End If
        If JSON.Count = 0 Then
            Return Nothing
        End If
        Return JSON
    End Function
    Public Function ListUnitInfo() As JArray
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=listunitinfo" & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            JSON = JArray.Parse(result)
        Else
            Return Nothing
        End If
        If JSON.Count = 0 Then
            Return Nothing
        End If
        Return JSON
    End Function
    Public Function ListUnitTable() As JArray
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=listunittable" & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            JSON = JArray.Parse(result)
        Else
            Return Nothing
        End If
        If JSON.Count = 0 Then
            Return Nothing
        End If
        Return JSON
    End Function
    Public Function GetUnitsStation(ByVal UNIT, County) As JArray
        Dim Pass As Boolean = False
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=getunitstation&unit=" & UNIT & "&county=" & County & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            JSON = JArray.Parse(result)
            Pass = True
        Else
            Return Nothing
        End If
        If JSON.Count = 0 Then
            Return Nothing
        End If
        If Pass Then
            Utilities.Log("API\GetUnitsStation", DateAndTime.Now & ": " & Pass & " |   " & UNIT & " " & County)
        Else
            Utilities.Log("API\GetUnitsStation", DateAndTime.Now & ": " & Pass & "EXCEPTION: " & result & " |   " & UNIT & " " & County)
        End If
        Return JSON
    End Function
    Public Function GetUnitAgency(ByVal UNIT, County) As JArray
        Dim Pass As Boolean = False
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=getunitagency&unit=" & UNIT & "&county=" & County & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            JSON = JArray.Parse(result)
            Pass = True
        End If
        If JSON.Count = 0 Then
            Return Nothing
        End If
        If Pass Then
            Utilities.Log("API\GetUnitAgency", DateAndTime.Now & ": " & Pass & " |   " & UNIT & " " & County)
        Else
            Utilities.Log("API\GetUnitAgency", DateAndTime.Now & ": " & Pass & "EXCEPTION: " & result & " |   " & UNIT & " " & County)
        End If
        Return JSON
    End Function
    Public Function GetStation(ByVal station, County) As JArray
        Dim Pass As Boolean = False
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=getstation&station=" & station & "&county=" & County & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            JSON = JArray.Parse(result)
            Pass = True
        Else
            Return Nothing
        End If
        If Pass Then
            Utilities.Log("API\GetStation", DateAndTime.Now & ": " & Pass & " |   " & station & " " & County)
        Else
            Utilities.Log("API\GetStation", DateAndTime.Now & ": " & Pass & "EXCEPTION: " & result & " |   " & station & " " & County)
        End If
        Return JSON
    End Function
    Public Function GetCallIcon(ByVal incident As calls) As JArray
        Dim Pass As Boolean = False
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=getcallicon&callSum=" & incident.getCallType & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            JSON = JArray.Parse(result)
            Pass = True
        Else
            Return Nothing
        End If
        If JSON.Count = 0 Then
            Return Nothing
        End If
        If Pass Then
            Utilities.Log("API\GetCallIcon", DateAndTime.Now & ": " & Pass & " |   " & incident.getCallType)
        Else
            Utilities.Log("API\GetCallIcon", DateAndTime.Now & ": " & Pass & "EXCEPTION: " & result & " |   " & incident.getCallType)
        End If
        Return JSON
    End Function
    Public Function GetFlags(ByVal GUID As String, county As String) As JArray
        Dim Pass As Boolean = False
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=getflags&GUID=" & GUID & "&county=" & county & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            JSON = JArray.Parse(result)
            Pass = True
        Else
            Return Nothing
        End If
        If JSON.Count = 0 Then
            Return Nothing
        End If
        If Pass Then
            Utilities.Log("API\GetFlags", DateAndTime.Now & ": " & Pass & " |   " & GUID & " " & county)
        Else
            Utilities.Log("API\GetFlags", DateAndTime.Now & ": " & Pass & "EXCEPTION: " & result & " |   " & GUID & " " & county)
        End If
        Return JSON
    End Function
    Public Function GetCallUnitAverage(ByVal callSum As String, county As String) As JArray
        Dim Pass As Boolean = False
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=getcallunitaverage&callSum=" & callSum & "&county=" & county & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            JSON = JArray.Parse(result)
            Pass = True
        Else
            Return Nothing
        End If
        If JSON.Count = 0 Then
            Return Nothing
        End If
        If Pass Then
            Utilities.Log("API\GetCallUnitAverage", DateAndTime.Now & ": " & Pass & " |   " & callSum & " " & county)
        Else
            Utilities.Log("API\GetCallUnitAverage", DateAndTime.Now & ": " & Pass & "EXCEPTION: " & result & " |   " & callSum & " " & county)
        End If
        Return JSON
    End Function
    Public Function GetCADFlags(ByVal callSum As String) As JArray
        Dim Pass As Boolean = False
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=getcadevents&callSum=" & callSum & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            JSON = JArray.Parse(result)
            Pass = True
        Else
            Return Nothing
        End If
        If JSON.Count = 0 Then
            Return Nothing
        End If
        If Pass Then
            Utilities.Log("API\GetCADFlags", DateAndTime.Now & ": " & Pass & " |   " & callSum)
        Else
            Utilities.Log("API\GetCADFlags", DateAndTime.Now & ": " & Pass & "EXCEPTION: " & result & " |   " & callSum)
        End If
        Return JSON
    End Function
#End Region

#Region "Root Permissions"
    Public Function Query(ByVal q As String) As JArray
        Dim Pass As Boolean = False
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=query&query=" & System.Uri.EscapeDataString(q) & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            JSON = JArray.Parse(result)
            Pass = True
        Else
            Return Nothing
        End If
        If JSON.Count = 0 Then
            JSON.Add("OK")
            Pass = True
        End If
        If Pass Then
            Utilities.Log("API\SQL", DateAndTime.Now & ": " & Pass & " |   " & q)
        Else
            Utilities.Log("API\SQL", DateAndTime.Now & ": " & Pass & "EXCEPTION: " & result & " |   " & q)
        End If
        Return JSON
    End Function
    Public Function GetPlacesInfomation(ByVal incident As calls) As JArray
        Dim Pass As Boolean = False
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=getplacesinfomation&GUID=" & incident.getGUID & "&county=" & incident.getCounty & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            JSON = JArray.Parse(result)
            Pass = True
        Else
            Return Nothing
        End If
        If JSON.Count = 0 Then
            Return Nothing
        End If
        If Pass Then
            Utilities.Log("API\GetPlacesInfomation", DateAndTime.Now & ": " & Pass & " |   " & incident.getGUID & " " & incident.getCounty)
        Else
            Utilities.Log("API\GetPlacesInfomation", DateAndTime.Now & ": " & Pass & "EXCEPTION: " & result & " |   " & incident.getGUID & " " & incident.getCounty)
        End If
        Return JSON
    End Function
    Public Function SendPlacesAlert(ByVal incident As calls, user_id As Integer, name As String, phonenum As String, carrierIndex As Integer, email As String, mail As Integer, txt As Integer) As Boolean ' 
        Dim Pass As Boolean = False
        Dim result As String = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=sendplacesalert&GUID=" & incident.getGUID & "&county=" & incident.getCounty & "&user_id=" & user_id & "&name=" & name & "&phone=" & phonenum & "&carrier=" & carrierIndex & "&email=" & email & "&mail=" & mail & "&txt=" & txt & "&callSum=" & incident.getCallType & "&address=" & incident.getAddress & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        Dim JSON As New JArray
        If Not result.ToUpper.Contains("ERROR") Then
            Pass = True
        Else
            Pass = False
        End If
        If Not Pass And JSON.Count = 0 Then
            Pass = True
        End If
        If Pass Then
            Utilities.Log("API\SendPlacesAlert", DateAndTime.Now & ": " & Pass & " |   '" & incident.getGUID & "' '" & incident.getCounty & "' '" & user_id & "' '" & name & "' '" & phonenum & "' '" & carrierIndex & "' '" & email & "' '" & mail & "' '" & txt)
        Else
            Utilities.Log("API\SendPlacesAlert", DateAndTime.Now & ": " & Pass & "EXCEPTION: " & result & " |   " & incident.getGUID & " " & incident.getCounty)
        End If
        Return Pass ' I guess?
    End Function
    Public Sub PDXLifeflight(ByVal message As String, ByVal lat As Double, ByVal lon As Double, ByVal cadurl As String)
        Dim pass As Boolean = False
        Dim status = message

        If status.Length > 110 Then
            status = status.Substring(0, 110)
        End If

        status = status & " " & cadurl

        status = System.Uri.EscapeDataString(status)

        Dim responce = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=pdxlifeflighttwitter&message=" & status & "&lat=" & lat & "&lon=" & lon & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        If Not responce.ToLower.Contains("error") Then
            pass = True
        End If
        If pass Then
            Utilities.Log("API\PDXLifeflight", DateAndTime.Now & ": Twitter (PDXLIFEFLIGHT): " & message)
        Else
            Utilities.Log("API\PDXLifeflight", DateAndTime.Now & ": Twitter (PDXLIFEFLIGHT): " & message & "ERROR: " & responce)
        End If
    End Sub
    Public Sub CADAlerts(ByVal message As String, ByVal lat As Double, ByVal lon As Double, ByVal cadurl As String)
        Dim pass As Boolean = False
        Dim status = message

        If status.Length > 110 Then
            status = status.Substring(0, 110)
        End If

        status = status & " " & cadurl

        status = System.Uri.EscapeDataString(status)

        Dim responce = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=cadalertstwitter&message=" & status & "&lat=" & lat & "&lon=" & lon & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")

        If Not responce.ToLower.Contains("error") Then
            pass = True
        End If
        If pass Then
            Utilities.Log("API\CADAlerts", DateAndTime.Now & ": Twitter (CADALERTS): " & message)
        Else
            Utilities.Log("API\CADAlerts", DateAndTime.Now & ": Twitter (CADALERTS): " & message & "ERROR: " & responce)
        End If
    End Sub
    Public Sub PDXAccidents(ByVal message As String, ByVal lat As Double, ByVal lon As Double, ByVal cadurl As String)
        Dim pass As Boolean = False
        Dim status = message

        If status.Length > 110 Then
            status = status.Substring(0, 110)
        End If

        status = status & " " & cadurl

        status = System.Uri.EscapeDataString(status)

        Dim responce = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=pdxaccidentstwitter&message=" & status & "&lat=" & lat & "&lon=" & lon & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        If Not responce.ToLower.Contains("error") Then
            pass = True
        End If
        If pass Then
            Utilities.Log("API\PDXAccidents", DateAndTime.Now & ": Twitter (PDXACCIDENTS): " & message)
        Else
            Utilities.Log("API\PDXAccidents", DateAndTime.Now & ": Twitter (PDXACCIDENTS): " & message & "ERROR: " & responce)
        End If
    End Sub
    Public Sub clackco_firemed(ByVal message As String, ByVal lat As Double, ByVal lon As Double, ByVal cadurl As String)
        Dim pass As Boolean = False
        Dim status = message

        If status.Length > 110 Then
            status = status.Substring(0, 110)
        End If

        status = status & " " & cadurl

        status = System.Uri.EscapeDataString(status)

        Dim responce = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=clackcofiremedtwitter&message=" & status & "&lat=" & lat & "&lon=" & lon & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        If Not responce.ToLower.Contains("error") Then
            pass = True
        End If
        If pass Then
            Utilities.Log("API\clackco_firemed", DateAndTime.Now & ": Twitter (CLACKCOFIREMED): " & message)
        Else
            Utilities.Log("API\clackco_firemed", DateAndTime.Now & ": Twitter (CLACKCOFIREMED): " & message & "ERROR: " & responce)
        End If
    End Sub
    Public Sub washco_firemed(ByVal message As String, ByVal lat As Double, ByVal lon As Double, ByVal cadurl As String)
        Dim pass As Boolean = False
        Dim status = message

        If status.Length > 110 Then
            status = status.Substring(0, 110)
        End If

        status = status & " " & cadurl

        status = System.Uri.EscapeDataString(status)

        Dim responce = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=washcofiremedtwitter&message=" & status & "&lat=" & lat & "&lon=" & lon & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        If Not responce.ToLower.Contains("error") Then
            pass = True
        End If
        If pass Then
            Utilities.Log("API\washco_firemed", DateAndTime.Now & ": Twitter (WASHCOFIREMED): " & message)
        Else
            Utilities.Log("API\washco_firemed", DateAndTime.Now & ": Twitter (WASHCOFIREMED): " & message & "ERROR: " & responce)
        End If
    End Sub

    Public Sub washco_police(ByVal message As String, ByVal lat As Double, ByVal lon As Double, ByVal cadurl As String)
        Dim pass As Boolean = False
        Dim status = message

        If status.Length > 110 Then
            status = status.Substring(0, 110)
        End If

        status = status & " " & cadurl

        status = System.Uri.EscapeDataString(status)

        Dim responce = Utilities.HTTP("http://www.api.oregon911.net/api/1.0/?method=washcopolicetwitter&message=" & status & "&lat=" & lat & "&lon=" & lon & "&type=JSON&jsoncallback=DEMO&key=" & API_KEY, "POST", "")
        If Not responce.ToLower.Contains("error") Then
            pass = True
        End If
        If pass Then
            Utilities.Log("API\washco_police", DateAndTime.Now & ": Twitter (WASHCOPOLICE): " & message)
        Else
            Utilities.Log("API\washco_police", DateAndTime.Now & ": Twitter (WASHCOPOLICE): " & message & "ERROR: " & responce)
        End If
    End Sub

#End Region
End Module