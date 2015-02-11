Imports System.Threading
Imports Newtonsoft.Json.Linq
Imports System.IO
Imports Tamir.SharpSsh

Public Class UI
    Private Database As New Oregon_911_Mainframe.incidents

    Private Sub Update_Tick(sender As Object, e As EventArgs) Handles Update.Tick
        If Not Database.Updating Then
            DataGridView1.Rows.Clear()
            DataGridView2.Rows.Clear()
            Dim C As Integer = 0
            Dim W As Integer = 0
            Dim M As Integer = 0
            Dim U As Integer = 0
            For Each incident In Database.database
                If incident.getCounty = "C" Then
                    C += 1
                ElseIf incident.getCounty = "W" Then
                    W += 1
                ElseIf incident.getCounty = "M" Then
                    M += 1
                End If
                DataGridView1.Rows.Add(incident.getGUID, incident.getCounty, _
                incident.getCallType, incident.getAddress, _
                incident.getTime()(0), incident.getTime()(1), _
                incident.getTime()(2), incident.getTime()(3), _
                incident.getAgency(), incident.getAgencyName, _
                incident.GetPriority, incident.GetUnitCount, _
                incident.getStation, incident.getType, _
                incident.getUnits, incident.getGeo()(0), _
                incident.getGeo()(1))

                For Each unit In incident.IncidentsUnits
                    DataGridView2.Rows.Add(unit.getGUID, unit.getCounty, _
                                           unit.getUnit, unit.getAgency, _
                                           unit.getStation, unit.getTime()(0), _
                                           unit.getTime()(1), unit.getTime()(2), _
                                           unit.getTime()(3))
                    U += 1
                Next
            Next
            Dim listeners As Integer = 0
            Try
                listeners = Utilities.HTTP("http://127.0.0.1/listeners.php", "POST", "")
            Catch ex As Exception

            End Try
            ToolStripStatusLabel1.Text = "Multnomah County: " & M & " Washington County: " & W & " Clackamas County: " & C & " Total: " & C + W + M & " Total Units: " & U & " || Radio listeners " & listeners
        End If
    End Sub
    Dim hi = "h"

    Dim ProcessingCalls As Boolean = False
    Private Sub UpdateCalls()
        While True
            If Not ProcessingCalls Then
                ProcessingCalls = True
                Database.useAPI = False

                If Database.updateIncdients() Then
                    db.UpdateDatabase(Database)
                    ProcessingCalls = False
                Else
                    ProcessingCalls = False
                End If

                Dim CallProcessor = New Thread(AddressOf ProcessCalls)
                CallProcessor.IsBackground = True
                CallProcessor.Start()
            Else
                Dim DBtmp As New Oregon_911_Mainframe.incidents
                If DBtmp.updateIncdients() Then
                    db.UpdateDatabase(DBtmp)
                End If
            End If
            System.Threading.Thread.Sleep(5000)
        End While
    End Sub

    Private Sub ProcessCalls()
        If Not Database.Updating Then
            If Database.database.Count > 0 Then
                If Not ProcessingCalls Then
                    Try
                        ProcessingCalls = True

                        places.Algorithm(Database)
                        firemed.Algorithm(Database)
                        lifeflight.Algorithm(Database)
                        cadalerts.Algorithm(Database)

                        ProcessingCalls = False
                    Catch ex As Exception
                        ' Get stack trace for the exception with source file information
                        Dim st = New StackTrace(ex, True)
                        ' Get the top stack frame
                        Dim frame = st.GetFrame(0)
                        ' Get the line number from the stack frame
                        Dim line = frame.ToString
                        Utilities.Log("error", "ui.vb: " & ex.Message & " | " & frame.ToString)
                        ProcessingCalls = False
                    Finally
                        ProcessingCalls = False
                    End Try
                End If
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        MsgBox("This does nothing!")
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        MsgBox("Copyright (C) Brandan Tyler Lasley 2014", MsgBoxStyle.Information)
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OptionsToolStripMenuItem.Click
        MsgBox("Also doesn't do anything")
    End Sub

    Private Sub UI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim DatabaseUpdater = New Thread(AddressOf UpdateCalls)
        DatabaseUpdater.IsBackground = True
        DatabaseUpdater.Start()
    End Sub
End Class