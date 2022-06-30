<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim CustomLabel1 As System.Windows.Forms.DataVisualization.Charting.CustomLabel = New System.Windows.Forms.DataVisualization.Charting.CustomLabel()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series3 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series4 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.lblConnection = New System.Windows.Forms.Label()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.tmrTimeDate = New System.Windows.Forms.Timer(Me.components)
        Me.lblTime = New System.Windows.Forms.Label()
        Me.txtData = New System.Windows.Forms.RichTextBox()
        Me.tmrReset = New System.Windows.Forms.Timer(Me.components)
        Me.bgwMain = New System.ComponentModel.BackgroundWorker()
        Me.bgwTesterTimer = New System.ComponentModel.BackgroundWorker()
        Me.lblConnectionTimer = New System.Windows.Forms.Label()
        Me.bgwTesterConnect = New System.ComponentModel.BackgroundWorker()
        Me.tmrSqlProcessor = New System.Windows.Forms.Timer(Me.components)
        Me.chartOutput = New System.Windows.Forms.DataVisualization.Charting.Chart()
        CType(Me.chartOutput, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblConnection
        '
        Me.lblConnection.AutoSize = True
        Me.lblConnection.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblConnection.Location = New System.Drawing.Point(10, 8)
        Me.lblConnection.Name = "lblConnection"
        Me.lblConnection.Size = New System.Drawing.Size(126, 21)
        Me.lblConnection.TabIndex = 1
        Me.lblConnection.Text = "                             "
        '
        'lblDate
        '
        Me.lblDate.AutoSize = True
        Me.lblDate.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblDate.Location = New System.Drawing.Point(1357, 8)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(82, 21)
        Me.lblDate.TabIndex = 2
        Me.lblDate.Text = "                  "
        '
        'tmrTimeDate
        '
        '
        'lblTime
        '
        Me.lblTime.AutoSize = True
        Me.lblTime.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblTime.Location = New System.Drawing.Point(1353, 32)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(86, 21)
        Me.lblTime.TabIndex = 3
        Me.lblTime.Text = "                   "
        '
        'txtData
        '
        Me.txtData.HideSelection = False
        Me.txtData.Location = New System.Drawing.Point(10, 76)
        Me.txtData.Name = "txtData"
        Me.txtData.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
        Me.txtData.Size = New System.Drawing.Size(1429, 300)
        Me.txtData.TabIndex = 5
        Me.txtData.Text = ""
        '
        'tmrReset
        '
        Me.tmrReset.Interval = 60000
        '
        'bgwMain
        '
        '
        'bgwTesterTimer
        '
        '
        'lblConnectionTimer
        '
        Me.lblConnectionTimer.AutoSize = True
        Me.lblConnectionTimer.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConnectionTimer.Location = New System.Drawing.Point(667, 20)
        Me.lblConnectionTimer.Name = "lblConnectionTimer"
        Me.lblConnectionTimer.Size = New System.Drawing.Size(36, 37)
        Me.lblConnectionTimer.TabIndex = 6
        Me.lblConnectionTimer.Text = "0"
        '
        'bgwTesterConnect
        '
        '
        'tmrSqlProcessor
        '
        Me.tmrSqlProcessor.Interval = 60000
        '
        'chartOutput
        '
        Me.chartOutput.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
        CustomLabel1.GridTicks = System.Windows.Forms.DataVisualization.Charting.GridTickTypes.TickMark
        CustomLabel1.LabelMark = System.Windows.Forms.DataVisualization.Charting.LabelMarkStyle.SideMark
        CustomLabel1.Text = "222222"
        ChartArea1.AxisX.CustomLabels.Add(CustomLabel1)
        ChartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount
        ChartArea1.AxisX.IsLabelAutoFit = False
        ChartArea1.AxisX.IsMarksNextToAxis = False
        ChartArea1.AxisX.LabelStyle.Interval = 0R
        ChartArea1.AxisX.LabelStyle.IntervalOffset = 0R
        ChartArea1.AxisX.LabelStyle.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.[Auto]
        ChartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.[Auto]
        ChartArea1.AxisX.MajorGrid.Enabled = False
        ChartArea1.AxisX.MajorTickMark.Enabled = False
        ChartArea1.AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.[Auto]
        ChartArea1.AxisX2.MajorGrid.Enabled = False
        ChartArea1.AxisX2.MajorTickMark.Enabled = False
        ChartArea1.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount
        ChartArea1.AxisY.LabelStyle.Interval = 0R
        ChartArea1.AxisY.LabelStyle.IntervalOffset = 0R
        ChartArea1.AxisY.LabelStyle.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea1.AxisY.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.[Auto]
        ChartArea1.AxisY.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.[Auto]
        ChartArea1.AxisY.Maximum = 10.0R
        ChartArea1.AxisY.Minimum = 8.0R
        ChartArea1.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Stacked
        ChartArea1.AxisY.Title = "Input Values"
        ChartArea1.Name = "ChartArea1"
        Me.chartOutput.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.chartOutput.Legends.Add(Legend1)
        Me.chartOutput.Location = New System.Drawing.Point(10, 382)
        Me.chartOutput.Name = "chartOutput"
        Me.chartOutput.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright
        Me.chartOutput.RightToLeft = System.Windows.Forms.RightToLeft.No
        Series1.BorderWidth = 5
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series1.Color = System.Drawing.Color.Red
        Series1.CustomProperties = "IsXAxisQuantitative=False"
        Series1.Legend = "Legend1"
        Series1.Name = "Read 1"
        Series2.BackImageTransparentColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Series2.BorderWidth = 5
        Series2.ChartArea = "ChartArea1"
        Series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series2.Color = System.Drawing.Color.Yellow
        Series2.Legend = "Legend1"
        Series2.Name = "Read 2"
        Series3.BackImageTransparentColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Series3.BorderWidth = 5
        Series3.ChartArea = "ChartArea1"
        Series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series3.Color = System.Drawing.Color.SkyBlue
        Series3.Legend = "Legend1"
        Series3.Name = "Read 3"
        Series4.BackImageTransparentColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Series4.BorderWidth = 5
        Series4.ChartArea = "ChartArea1"
        Series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series4.Color = System.Drawing.Color.MediumSpringGreen
        Series4.Legend = "Legend1"
        Series4.Name = "Read 4"
        Me.chartOutput.Series.Add(Series1)
        Me.chartOutput.Series.Add(Series2)
        Me.chartOutput.Series.Add(Series3)
        Me.chartOutput.Series.Add(Series4)
        Me.chartOutput.Size = New System.Drawing.Size(1429, 486)
        Me.chartOutput.TabIndex = 7
        Me.chartOutput.Text = "Chart1"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1451, 880)
        Me.Controls.Add(Me.chartOutput)
        Me.Controls.Add(Me.lblConnectionTimer)
        Me.Controls.Add(Me.txtData)
        Me.Controls.Add(Me.lblTime)
        Me.Controls.Add(Me.lblDate)
        Me.Controls.Add(Me.lblConnection)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        CType(Me.chartOutput, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblConnection As Label
    Friend WithEvents lblDate As Label
    Friend WithEvents tmrTimeDate As Timer
    Friend WithEvents lblTime As Label
    Friend WithEvents txtData As RichTextBox
    Friend WithEvents tmrReset As Timer
    Friend WithEvents bgwMain As System.ComponentModel.BackgroundWorker
    Friend WithEvents bgwTesterTimer As System.ComponentModel.BackgroundWorker
    Friend WithEvents lblConnectionTimer As Label
    Friend WithEvents bgwTesterConnect As System.ComponentModel.BackgroundWorker
    Friend WithEvents tmrSqlProcessor As Timer
    Friend WithEvents chartOutput As DataVisualization.Charting.Chart
End Class
