<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UI
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.C_GUID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.C_County = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.C_CallType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.C_Address = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.C_CallEntry = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.C_CallDispatched = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.C_Enroute = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.C_OnScene = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.C_Agency = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.C_AgencyName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.C_Priority = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.C_UnitCount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.C_Station = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.C_Type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.C_Units = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.C_LAT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.C_LON = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.U_GUID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.U_County = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.U_UNIT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.U_Agency = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.U_Station = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.U_Dispatched = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.U_Enroute = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.U_OnScene = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.U_Clear = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Update = New System.Windows.Forms.Timer(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.UploadStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 24)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.DataGridView1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.DataGridView2)
        Me.SplitContainer1.Size = New System.Drawing.Size(1505, 591)
        Me.SplitContainer1.SplitterDistance = 334
        Me.SplitContainer1.TabIndex = 1
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.C_GUID, Me.C_County, Me.C_CallType, Me.C_Address, Me.C_CallEntry, Me.C_CallDispatched, Me.C_Enroute, Me.C_OnScene, Me.C_Agency, Me.C_AgencyName, Me.C_Priority, Me.C_UnitCount, Me.C_Station, Me.C_Type, Me.C_Units, Me.C_LAT, Me.C_LON})
        Me.DataGridView1.Cursor = System.Windows.Forms.Cursors.Default
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.EnableHeadersVisualStyles = False
        Me.DataGridView1.Location = New System.Drawing.Point(0, 0)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(1505, 334)
        Me.DataGridView1.TabIndex = 0
        '
        'C_GUID
        '
        Me.C_GUID.HeaderText = "GUID"
        Me.C_GUID.Name = "C_GUID"
        Me.C_GUID.ReadOnly = True
        '
        'C_County
        '
        Me.C_County.HeaderText = "County"
        Me.C_County.Name = "C_County"
        Me.C_County.ReadOnly = True
        '
        'C_CallType
        '
        Me.C_CallType.HeaderText = "Call Type"
        Me.C_CallType.Name = "C_CallType"
        Me.C_CallType.ReadOnly = True
        '
        'C_Address
        '
        Me.C_Address.HeaderText = "Address"
        Me.C_Address.Name = "C_Address"
        Me.C_Address.ReadOnly = True
        '
        'C_CallEntry
        '
        Me.C_CallEntry.HeaderText = "Call Entry"
        Me.C_CallEntry.Name = "C_CallEntry"
        Me.C_CallEntry.ReadOnly = True
        '
        'C_CallDispatched
        '
        Me.C_CallDispatched.HeaderText = "Call Dispatched"
        Me.C_CallDispatched.Name = "C_CallDispatched"
        Me.C_CallDispatched.ReadOnly = True
        '
        'C_Enroute
        '
        Me.C_Enroute.HeaderText = "En Route"
        Me.C_Enroute.Name = "C_Enroute"
        Me.C_Enroute.ReadOnly = True
        '
        'C_OnScene
        '
        Me.C_OnScene.HeaderText = "On Scene"
        Me.C_OnScene.Name = "C_OnScene"
        Me.C_OnScene.ReadOnly = True
        '
        'C_Agency
        '
        Me.C_Agency.HeaderText = "Agency"
        Me.C_Agency.Name = "C_Agency"
        Me.C_Agency.ReadOnly = True
        '
        'C_AgencyName
        '
        Me.C_AgencyName.HeaderText = "Agency Name"
        Me.C_AgencyName.Name = "C_AgencyName"
        Me.C_AgencyName.ReadOnly = True
        '
        'C_Priority
        '
        Me.C_Priority.HeaderText = "Priority"
        Me.C_Priority.Name = "C_Priority"
        Me.C_Priority.ReadOnly = True
        '
        'C_UnitCount
        '
        Me.C_UnitCount.HeaderText = "Unit Count"
        Me.C_UnitCount.Name = "C_UnitCount"
        Me.C_UnitCount.ReadOnly = True
        '
        'C_Station
        '
        Me.C_Station.HeaderText = "Station"
        Me.C_Station.Name = "C_Station"
        Me.C_Station.ReadOnly = True
        '
        'C_Type
        '
        Me.C_Type.HeaderText = "Type"
        Me.C_Type.Name = "C_Type"
        Me.C_Type.ReadOnly = True
        '
        'C_Units
        '
        Me.C_Units.HeaderText = "Units"
        Me.C_Units.Name = "C_Units"
        Me.C_Units.ReadOnly = True
        '
        'C_LAT
        '
        Me.C_LAT.HeaderText = "LAT"
        Me.C_LAT.Name = "C_LAT"
        Me.C_LAT.ReadOnly = True
        '
        'C_LON
        '
        Me.C_LON.HeaderText = "LON"
        Me.C_LON.Name = "C_LON"
        Me.C_LON.ReadOnly = True
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.U_GUID, Me.U_County, Me.U_UNIT, Me.U_Agency, Me.U_Station, Me.U_Dispatched, Me.U_Enroute, Me.U_OnScene, Me.U_Clear})
        Me.DataGridView2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView2.EnableHeadersVisualStyles = False
        Me.DataGridView2.Location = New System.Drawing.Point(0, 0)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.ReadOnly = True
        Me.DataGridView2.RowHeadersVisible = False
        Me.DataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView2.Size = New System.Drawing.Size(1505, 253)
        Me.DataGridView2.TabIndex = 1
        '
        'U_GUID
        '
        Me.U_GUID.HeaderText = "GUID"
        Me.U_GUID.Name = "U_GUID"
        Me.U_GUID.ReadOnly = True
        '
        'U_County
        '
        Me.U_County.HeaderText = "County"
        Me.U_County.Name = "U_County"
        Me.U_County.ReadOnly = True
        '
        'U_UNIT
        '
        Me.U_UNIT.HeaderText = "Unit"
        Me.U_UNIT.Name = "U_UNIT"
        Me.U_UNIT.ReadOnly = True
        '
        'U_Agency
        '
        Me.U_Agency.HeaderText = "Agency"
        Me.U_Agency.Name = "U_Agency"
        Me.U_Agency.ReadOnly = True
        '
        'U_Station
        '
        Me.U_Station.HeaderText = "Station"
        Me.U_Station.Name = "U_Station"
        Me.U_Station.ReadOnly = True
        '
        'U_Dispatched
        '
        Me.U_Dispatched.HeaderText = "Dispatched"
        Me.U_Dispatched.Name = "U_Dispatched"
        Me.U_Dispatched.ReadOnly = True
        '
        'U_Enroute
        '
        Me.U_Enroute.HeaderText = "En Route"
        Me.U_Enroute.Name = "U_Enroute"
        Me.U_Enroute.ReadOnly = True
        '
        'U_OnScene
        '
        Me.U_OnScene.HeaderText = "On Scene"
        Me.U_OnScene.Name = "U_OnScene"
        Me.U_OnScene.ReadOnly = True
        '
        'U_Clear
        '
        Me.U_Clear.HeaderText = "Clear"
        Me.U_Clear.Name = "U_Clear"
        Me.U_Clear.ReadOnly = True
        '
        'Update
        '
        Me.Update.Enabled = True
        Me.Update.Interval = 2000
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.UploadStatus})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 615)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1505, 22)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(107, 17)
        Me.ToolStripStatusLabel1.Text = "Status: Not Loaded"
        '
        'UploadStatus
        '
        Me.UploadStatus.Name = "UploadStatus"
        Me.UploadStatus.Size = New System.Drawing.Size(0, 17)
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(92, 22)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OptionsToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(48, 20)
        Me.ToolsToolStripMenuItem.Text = "&Tools"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.OptionsToolStripMenuItem.Text = "&Options"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.AboutToolStripMenuItem.Text = "&About..."
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1505, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'UI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1505, 637)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "UI"
        Me.Text = "Oregon 911 Mainframe"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents C_GUID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents C_County As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents C_CallType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents C_Address As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents C_CallEntry As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents C_CallDispatched As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents C_Enroute As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents C_OnScene As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents C_Agency As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents C_AgencyName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents C_Priority As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents C_UnitCount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents C_Station As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents C_Type As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents C_Units As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents C_LAT As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents C_LON As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents Update As System.Windows.Forms.Timer
    Friend WithEvents U_GUID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents U_County As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents U_UNIT As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents U_Agency As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents U_Station As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents U_Dispatched As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents U_Enroute As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents U_OnScene As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents U_Clear As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents UploadStatus As System.Windows.Forms.ToolStripStatusLabel
End Class
