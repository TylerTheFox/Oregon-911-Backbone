Imports Oregon_911_Mainframe.units
Public Class calls : Implements IDisposable

#Region "PRIVATE"
    Private GUID As String
    Private County As String
    Private CallType As String
    Private Address As String
    Private Time As New List(Of String)
    Private Agency As String
    Private AgencyName As String
    Private Priority As String
    Private UnitCount As Integer
    Private Station As String
    Private Type As String
    Private Units As String
    Private Geo As New List(Of Double)
    Private CallType_History As New List(Of String)
    Private Address_History As New List(Of String)
    Private Geo_History As New List(Of List(Of Double))
    Private overwrite As Boolean = False
#End Region

#Region "Initializers & Tools"
    Public IncidentsUnits As New List(Of Oregon_911_Mainframe.units)
    Sub New(ByVal _GUID As String, _County As String, _CallType As String, _Address As String, _Time As List(Of String), _Agency As String, _AgencyName As String, _Priority As String, _UnitCount As Integer, _Station As String, _Type As String, _Units As String, _Geo As List(Of Double))
        Me.GUID = _GUID
        Me.County = _County
        Me.CallType = _CallType
        Me.Address = _Address
        Me.Time = _Time
        Me.Agency = _Agency
        Me.AgencyName = _AgencyName
        Me.Priority = _Priority
        Me.UnitCount = _UnitCount
        Me.Station = _Station
        Me.Type = _Type
        Me.Units = _Units
        Me.Geo = _Geo
    End Sub
    Sub New()
        Geo.Add(0)
        Geo.Add(0)
        Time.Add("00:00:00")
        Time.Add("00:00:00")
        Time.Add("00:00:00")
        Time.Add("00:00:00")
    End Sub
#End Region

#Region "SET"
    Public Sub setGUID(ByVal _GUID As String)
        Me.GUID = _GUID
    End Sub
    Public Sub setCounty(ByVal _County As String)
        Me.County = _County
    End Sub
    Public Sub setCallType(ByVal _CallType As String)
        Me.CallType = _CallType
    End Sub
    Public Sub setAddress(ByVal _Address As String)
        Me.Address = _Address
    End Sub
    Public Sub setTime(ByVal _Time As List(Of String))
        Time.Clear()
        Me.Time = _Time
    End Sub
    Public Sub setAgency(ByVal _Agency As String, UseAPI As Boolean)
        Me.Agency = _Agency
        If overwrite = False And Not UseAPI = True And Not County = "M" And Not County = "W" Then
            Me.AgencyName = Utilities.GetDepartment(_Agency)
        End If
    End Sub
    Public Sub setAgencyName(ByVal _AgencyName As String)
        Me.AgencyName = _AgencyName
    End Sub
    Public Sub setPriority(ByVal _Priority As String)
        Me.Priority = _Priority
    End Sub
    Public Sub setUnitCount(ByVal _UnitCount As String)
        Me.UnitCount = _UnitCount
    End Sub
    Public Sub setStation(ByVal _Station As String)
        Me.Station = _Station
    End Sub
    Public Sub setType(ByVal _Type As String)
        Me.Type = _Type
    End Sub
    Public Sub setUnits(ByVal _Units As String)
        Me.Units = _Units
    End Sub
    Public Sub setGeo(ByVal _Geo As List(Of Double))
        Geo.Clear()
        Me.Geo = _Geo
    End Sub
    Public Sub setOverwrite(ByVal _OverWrite as boolean )
        Me.overwrite = _OverWrite
    End Sub
    Public Sub AddCallTypeHistory(ByVal _calltype As String)
        Me.CallType_History.Add(_calltype)
    End Sub
    Public Sub AddAddressHistory(ByVal _address As String)
        Me.Address_History.Add(_address)
    End Sub
    Public Sub addGeoHistory(ByVal _geo As List(Of Double))
        Me.Geo_History.Add(_geo)
    End Sub
#End Region

#Region "GET"
    Public Function getGUID() As String
        Return Me.GUID
    End Function
    Public Function getCounty() As String
        Return Me.County
    End Function
    Public Function getCallType() As String
        Return Me.CallType
    End Function
    Public Function getAddress() As String
        Return Me.Address
    End Function
    Public Function getTime() As List(Of String)
        Return Me.Time
    End Function
    Public Function getAgency() As String
        Return Me.Agency
    End Function
    Public Function getAgencyName() As String
        Return Me.AgencyName
    End Function
    Public Function GetPriority() As String
        Return Me.Priority
    End Function
    Public Function GetUnitCount() As Integer
        Return Me.UnitCount
    End Function
    Public Function getStation() As String
        Return Me.Station
    End Function
    Public Function [getType]() As String
        Return Me.Type
    End Function
    Public Function getUnits() As String
        Return Me.Units
    End Function
    Public Function getGeo() As List(Of Double)
        If Me.Geo.Count > 0 Then
            Return Me.Geo
        Else
            Me.Geo.Add(0)
            Me.Geo.Add(0)
            Return Me.Geo
        End If
    End Function
    Public Function getCallTypeHistory() As List(Of String)
        Return Me.CallType_History
    End Function
    Public Function getAddressHistory() As List(Of String)
        Return Me.Address_History
    End Function
    Public Function getGeoHistory() As List(Of List(Of Double))
        Return Me.Geo_History
    End Function
    Public Function getOverWrite() As Boolean
        Return Me.overwrite
    End Function
#End Region

#Region "Remove"
    Public Sub RemoveCallTypeHistory(ByVal _calltype)
        CallType_History.Remove(_calltype)
    End Sub
    Public Sub RemoveAddressHistory(ByVal _address)
        Address_History.Remove(_address)
    End Sub
    Public Sub removeGeoHistory(ByVal _geo As List(Of Double))
        Dim index As Integer = -1
        For i As Integer = 0 To Geo_History.Count - 1
            If Geo_History(i)(0) = _geo(0) And Geo_History(i)(1) = _geo(1) Then
                index = i
                Exit For
            End If
        Next
        If index > -1 Then
            getGeoHistory.RemoveAt(index)
        End If
    End Sub
#End Region

#Region "Operators"
    Public Shared Operator =(obj1 As calls, obj2 As calls) As Boolean
        If (obj1.GUID = obj2.GUID) And (obj1.County = obj2.County) Then
            Return True
        End If
        Return False
    End Operator
    Public Shared Operator <>(obj1 As calls, obj2 As calls) As Boolean
        Return Not (obj1 = obj2)
    End Operator
#End Region

#Region "Destructor(s) DO NOT TOUCH!!!!"
    Dim disposed As Boolean = False
    Public Sub Dispose() _
               Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Sub Dispose(disposing As Boolean)
        If disposed Then Return
        If disposing Then
            GUID = Nothing
            County = Nothing
            CallType = Nothing
            Address = Nothing
            Agency = Nothing
            AgencyName = Nothing
            Priority = Nothing
            UnitCount = Nothing
            Station = Nothing
            Type = Nothing
            Units = Nothing
        End If
        Time = Nothing
        Geo = Nothing
        Geo_History = Nothing
        CallType_History = Nothing
        IncidentsUnits = Nothing
        disposed = True
    End Sub
#End Region

End Class