Public Class units : Implements IDisposable

#Region "PRIVATE"
    Private GUID As String
    Private County As String
    Private Unit As String
    Private Agency As String
    Private Station As String
    Private Time As New List(Of String)
#End Region

#Region "Initializers & Tools"
    Sub New(ByVal _GUID As String, _County As String, _Unit As String, _Agency As String, _Station As String, _Time As List(Of String))
        Me.GUID = _GUID
        Me.County = _County
        Me.Unit = _Unit
        Me.Agency = _Agency
        Me.Station = _Station
        Me.Time = _Time
    End Sub
    Sub New()
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

    Public Sub setUnit(ByVal _Unit As String)
        Me.Unit = _Unit
    End Sub

    Public Sub setAgency(ByVal _Agency As String)
        Me.Agency = _Agency
    End Sub

    Public Sub setStation(ByVal _Station As String)
        Me.Station = _Station
    End Sub

    Public Sub setTime(ByVal _Time As List(Of String))
        Time.Clear()
        Me.Time = _Time
    End Sub

#End Region

#Region "GET"
    Public Function getGUID() As String
        Return Me.GUID
    End Function
    Public Function getCounty() As String
        Return Me.County
    End Function
    Public Function getUnit() As String
        Return Me.Unit
    End Function
    Public Function getAgency() As String
        Return Me.Agency
    End Function
    Public Function getStation() As String
        Return Me.Station
    End Function
    Public Function getTime() As List(Of String)
        Return Me.Time
    End Function
#End Region

#Region "Operators"
    Public Shared Operator =(obj1 As units, obj2 As units) As Boolean
        If (obj1.GUID = obj2.GUID) And (obj1.County = obj2.County And obj1.Unit = obj2.Unit) Then
            Return True
        End If
        Return False
    End Operator
    Public Shared Operator <>(obj1 As units, obj2 As units) As Boolean
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
            Unit = Nothing
            Agency = Nothing
            Station = Nothing
        End If
        Time = Nothing
        disposed = True
    End Sub
#End Region

End Class