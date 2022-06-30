Imports System.Net.Sockets
Imports System.Text
Imports System.IO
Imports System.Threading
Imports System.Net
Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmMain

    'Reads the set IP address and port number from the settings.txt file
    Dim objfileReader As System.IO.StreamReader = My.Computer.FileSystem.OpenTextFileReader(Directory.GetCurrentDirectory() & "\settings.txt")
    Dim strPortNum As String = objfileReader.ReadLine()
    Dim objLocalAddress As IPAddress = IPAddress.Parse(objfileReader.ReadLine())
    Dim objClient As TcpClient
    Dim objStream As NetworkStream
    Dim strData As String
    Dim intRead As Integer
    Dim strPath As String = Directory.GetCurrentDirectory() & "\RawData\" & CDate(Date.Now).ToString("yyyyMMdd") & ".txt"
    Dim objTimerLock As New Object
    Dim intTesterTimerCounter As Integer

    'Checks if the leak tester is currently connected to the application
    'Public Function isConnected() As String
    '    Dim str As String = ""
    '    Try
    '        If objClient.Connected = True Then
    '            str = "Connected"
    '        Else
    '            str = "Not Connected"
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show(ex.ToString)
    '    End Try
    '    Return str
    'End Function

    Public Sub Connect()
        Const MAX_RETRIES As Integer = 1
        Dim intRetries As Integer
        While intRetries < MAX_RETRIES
            Try
                objClient = New TcpClient(objLocalAddress.ToString, strPortNum)
                Exit While
            Catch ex As Exception
                Thread.Sleep(1000)
                intRetries = intRetries + 1
            End Try
        End While
        If intRetries = MAX_RETRIES Then
            frmError.ShowDialog()
            Environment.Exit(True)
        End If
    End Sub

    Public Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Temporarily displays program name along with version number on a seperate form
        Dim frmStartPage = New frmStartPage
        frmStartPage.ShowDialog()

        'If Not isConnected() Then
        '    Const MAX_RETRIES As Integer = 1
        '    Dim intRetries As Integer
        '    While intRetries < MAX_RETRIES
        '        Try
        '            objClient.Client.Disconnect(False)
        '            objClient = New TcpClient()
        '            objClient.Connect(objLocalAddress, strPortNum)
        '            Exit While
        '        Catch ex As Exception
        '            Thread.Sleep(1000)
        '            intRetries = intRetries + 1
        '        End Try
        '    End While
        '    If intRetries = MAX_RETRIES Then
        '        frmError.ShowDialog()
        '        Application.Exit()
        '    End If
        'End If

        Connect()

        'Checks for Data folder, creates one if there is not one in the directory
        If Not Directory.Exists(Directory.GetCurrentDirectory() & "\Data") Then
            Directory.CreateDirectory(Directory.GetCurrentDirectory() & "\Data")
        End If

        'Checks for RawData folder, creates one if there is not one in the directory
        If Not Directory.Exists(Directory.GetCurrentDirectory() & "\RawData") Then
            Directory.CreateDirectory(Directory.GetCurrentDirectory() & "\RawData")
        End If

        'Date and Time
        lblDate.Text = DateAndTime.Today
        lblTime.Text = DateAndTime.Now.ToLongTimeString

        'Displays connection status
        'lblConnection.Text = "Status: " & isConnected()

        'Starts the timer that updates the date, time, and connection status
        tmrTimeDate.Start()

        'Starts the background workers/timer of the application
        bgwMain.RunWorkerAsync()
        bgwTesterTimer.RunWorkerAsync()
        tmrSqlProcessor.Start()
    End Sub

    'Helper method used to avoid cross-thread exceptions
    Private Delegate Sub AppendTextBoxDelegate(ByVal txt As RichTextBox, ByVal str As String)
    Private Sub AppendTextBoxes(ByVal txt As RichTextBox, ByVal str As String)
        If txt.InvokeRequired Then
            txt.Invoke(New AppendTextBoxDelegate(AddressOf AppendTextBoxes), New Object() {txt, str})
        Else
            txt.AppendText(str + Environment.NewLine)
        End If
    End Sub

    'Helper method used to avoid cross-thread exceptions
    Private Delegate Sub AddChartDataDelegate(ByVal chart As Windows.Forms.DataVisualization.Charting.Chart, ByVal str As String, ByVal str2 As String)
    Private Sub AddChartData(ByVal chart As Windows.Forms.DataVisualization.Charting.Chart, ByVal str As String, ByVal str2 As String)
        If chart.InvokeRequired Then
            chart.Invoke(New AddChartDataDelegate(AddressOf AddChartData), New Object() {chart, str, str2})
        Else
            If str = "****" Then
                chart.Series.Item(str2).Points.Add(0)
            Else
                chart.Series.Item(str2).Points.Add(str)
            End If
        End If
    End Sub

    'Helper method used to avoid cross-thread exceptions
    Private Delegate Sub ClearChartDataDelegate(ByVal chart As Windows.Forms.DataVisualization.Charting.Chart, ByVal str As String)
    Private Sub ClearChartData(ByVal chart As Windows.Forms.DataVisualization.Charting.Chart, ByVal str As String)
        If chart.InvokeRequired Then
            chart.Invoke(New ClearChartDataDelegate(AddressOf ClearChartData), New Object() {chart, str})
        Else
            chart.Series.Item(str).Points.Clear()
        End If
    End Sub

    'Gets the stream from the TCP client, checks if it is possible to read from the stream, and if it is,
    'it reads the input in a loop until the client is disconnected. Simultaneously displays the input
    'in the DataTxtBox while also writing to a new .sql file that is created everytime the application runs
    Private Sub bgwMain_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bgwMain.DoWork
        Dim arrColumns As String()
        Dim objGmtTime As Date = Date.UtcNow
        Dim objLocalTime As Date = Date.Now
        Dim intSeq As Integer = 0
        Dim intChartCount As Integer = 0
        Try
            objStream = objClient.GetStream()
            If objStream.CanRead Then
                Do Until Not objClient.Connected
                    Dim arrBytes(1024) As Byte
                    intRead = objStream.Read(arrBytes, 0, arrBytes.Length)
                    If intChartCount = 5 Then
                        ClearChartData(chartOutput, "Read 1")
                        ClearChartData(chartOutput, "Read 2")
                        ClearChartData(chartOutput, "Read 3")
                        ClearChartData(chartOutput, "Read 4")
                        intChartCount = 0
                    End If
                    intTesterTimerCounter = -1
                    strData = Encoding.ASCII.GetString(arrBytes)
                    arrColumns = Split(strData, Environment.NewLine)
                    AppendTextBoxes(txtData, strData)
                    For i As Integer = 0 To arrColumns.Length - 1
                        Dim arrLine As String() = Split(arrColumns(i), "	")
                        If arrColumns(i).StartsWith("C") Or arrColumns(i).StartsWith("F") Or arrColumns(i).StartsWith("V") Or arrColumns(i).StartsWith("S") Or arrColumns(i).StartsWith("T") Then
                            My.Computer.FileSystem.WriteAllText(strPath, arrColumns(i).Substring(0, 34) + Environment.NewLine, True, Encoding.ASCII) '<--- Raw Data
                            If arrLine.Length = 7 Then
                                AddChartData(chartOutput, arrLine(3), "Read 1")
                                AddChartData(chartOutput, arrLine(4), "Read 2")
                                AddChartData(chartOutput, arrLine(5), "Read 3")
                                AddChartData(chartOutput, arrLine(6), "Read 4")
                                Create_SQL_File("exec up_LeakTesterFullData_AddRec @MachineID='1',@GMTTime='" + objGmtTime.ToString + "',@LocalTime='" + objLocalTime.ToString + "',@Seq=" + intSeq.ToString + ",@Step='" + arrLine(0).TrimEnd + "',@Seconds='" + arrLine(1) + arrLine(2) + "',@Data1='" + arrLine(3) + "',@Data2='" + arrLine(4) + "',@Data3='" + arrLine(5) + "',@Data4='" + arrLine(6).Substring(0, 6) + "'", "_" & CDate(Date.Now).ToString("ff"))
                                intSeq += 1
                            ElseIf arrLine.Length = 6 Then
                                AddChartData(chartOutput, arrLine(3), "Read 1")
                                AddChartData(chartOutput, arrLine(4), "Read 2")
                                AddChartData(chartOutput, arrLine(5), "Read 3")
                                Create_SQL_File("exec up_LeakTesterFullData_AddRec @MachineID='1',@GMTTime='" + objGmtTime.ToString + "',@LocalTime='" + objLocalTime.ToString + "',@Seq=" + intSeq.ToString + ",@Step='" + arrLine(0).TrimEnd + "',@Seconds='" + arrLine(1) + arrLine(2) + "',@Data1='" + arrLine(3) + "',@Data2='" + arrLine(4) + "',@Data3='" + arrLine(5).Substring(0, 6) + "',@Data4='****'", "_" & CDate(Date.Now).ToString("ff"))
                                intSeq += 1
                            ElseIf arrLine.Length = 5 Then
                                AddChartData(chartOutput, arrLine(3), "Read 1")
                                AddChartData(chartOutput, arrLine(4), "Read 2")
                                Create_SQL_File("exec up_LeakTesterFullData_AddRec @MachineID='1',@GMTTime='" + objGmtTime.ToString + "',@LocalTime='" + objLocalTime.ToString + "',@Seq=" + intSeq.ToString + ",@Step='" + arrLine(0).TrimEnd + "',@Seconds='" + arrLine(1) + arrLine(2) + "',@Data1='" + arrLine(3) + "',@Data2='" + arrLine(4).Substring(0, 6) + "',@Data3='****',@Data4='****'", "_" & CDate(Date.Now).ToString("ff"))
                                intSeq += 1
                            ElseIf arrLine.Length = 4 Then
                                AddChartData(chartOutput, arrLine(3), "Read 1")
                                Create_SQL_File("exec up_LeakTesterFullData_AddRec @MachineID='1',@GMTTime='" + objGmtTime.ToString + "',@LocalTime='" + objLocalTime.ToString + "',@Seq=" + intSeq.ToString + ",@Step='" + arrLine(0).TrimEnd + "',@Seconds='" + arrLine(1) + arrLine(2) + "',@Data1='" + arrLine(3).Substring(0, 6) + "',@Data2='****',@Data3='****',@Data4='****'", "_" & CDate(Date.Now).ToString("ff"))
                                intSeq += 1
                            End If
                        ElseIf arrColumns(i).StartsWith("0") Or arrColumns(i).StartsWith("1") Then
                            If arrLine.Length = 13 Then

                                Create_SQL_File("exec up_LeakTester_AddRec @MachineID='1',@GMTTime='" + objGmtTime.ToString + "',@LocalTime='" + objLocalTime.ToString + "',@ProgramNumber=" + arrLine(2) + ",@ProgramName='" + arrLine(3).TrimEnd + "',@TestType='" + arrLine(4).TrimEnd + "',@Result1='" + arrLine(5).TrimEnd + "',@Value1='" + arrLine(6).TrimEnd + "',@Result2='" + arrLine(7).TrimEnd + "',@Value2='" + arrLine(8).TrimEnd + "',@Result3='" + arrLine(9).TrimEnd + "',@Value3='" + arrLine(10).TrimEnd + "',@Result4='" + arrLine(11).TrimEnd + "',@Value4='" + arrLine(12).TrimEnd + "'", "")
                                My.Computer.FileSystem.WriteAllText(strPath, objLocalTime.ToString + "" + ControlChars.Tab + arrLine(2) + "  " + arrLine(3).TrimEnd() + "  " + arrLine(4) + ControlChars.Tab + arrLine(5).TrimEnd() + " " + arrLine(6) + " " + arrLine(7) + " " + arrLine(8) + " " + arrLine(9) + " " + arrLine(10) + " " + arrLine(11) + " " + arrLine(12) + Environment.NewLine, True, Encoding.ASCII)
                                intSeq += 1

                            ElseIf arrLine.Length = 11 Then

                                Create_SQL_File("exec up_LeakTester_AddRec @MachineID='1',@GMTTime='" + objGmtTime.ToString + "',@LocalTime='" + objLocalTime.ToString + "',@ProgramNumber=" + arrLine(2) + ",@ProgramName='" + arrLine(3).TrimEnd + "',@TestType='" + arrLine(4).TrimEnd + "',@Result1='" + arrLine(5).TrimEnd + "',@Value1='" + arrLine(6).TrimEnd + "',@Result2='" + arrLine(7).TrimEnd + "',@Value2='" + arrLine(8).TrimEnd + "',@Result3='" + arrLine(9).TrimEnd + "',@Value3='" + arrLine(10).TrimEnd + "',@Result4='****',@Value4='****'", "")
                                My.Computer.FileSystem.WriteAllText(strPath, objLocalTime.ToString + "" + ControlChars.Tab + arrLine(2) + "  " + arrLine(3).TrimEnd() + "  " + arrLine(4) + ControlChars.Tab + arrLine(5).TrimEnd() + " " + arrLine(6) + " " + arrLine(7) + " " + arrLine(8) + " " + arrLine(9) + " " + arrLine(10) + Environment.NewLine, True, Encoding.ASCII)
                                intSeq += 1

                            ElseIf arrLine.Length = 9 Then

                                Create_SQL_File("exec up_LeakTester_AddRec @MachineID='1',@GMTTime='" + objGmtTime.ToString + "',@LocalTime='" + objLocalTime.ToString + "',@ProgramNumber=" + arrLine(2) + ",@ProgramName='" + arrLine(3).TrimEnd + "',@TestType='" + arrLine(4).TrimEnd + "',@Result1='" + arrLine(5).TrimEnd + "',@Value1='" + arrLine(6).TrimEnd + "',@Result2='" + arrLine(7).TrimEnd + "',@Value2='" + arrLine(8).TrimEnd + "',@Result3='****',@Value3='****',@Result4='****',@Value4='****'", "")
                                My.Computer.FileSystem.WriteAllText(strPath, objLocalTime.ToString + "" + ControlChars.Tab + arrLine(2) + "  " + arrLine(3).TrimEnd() + "  " + arrLine(4) + ControlChars.Tab + arrLine(5).TrimEnd() + " " + arrLine(6) + " " + arrLine(7) + " " + arrLine(8) + Environment.NewLine, True, Encoding.ASCII)
                                intSeq += 1

                            ElseIf arrLine.Length = 7 Then

                                Create_SQL_File("exec up_LeakTester_AddRec @MachineID='1',@GMTTime='" + objGmtTime.ToString + "',@LocalTime='" + objLocalTime.ToString + "',@ProgramNumber=" + arrLine(2) + ",@ProgramName='" + arrLine(3).TrimEnd + "',@TestType='" + arrLine(4).TrimEnd + "',@Result1='" + arrLine(5).TrimEnd + "',@Value1='" + arrLine(6).TrimEnd + "'@Result2='****',@Value2='****',@Result3='****',@Value3='****',@Result4='****',@Value4='****'", "")
                                My.Computer.FileSystem.WriteAllText(strPath, objLocalTime.ToString + "" + ControlChars.Tab + arrLine(2) + "  " + arrLine(3).TrimEnd() + "  " + arrLine(4) + ControlChars.Tab + arrLine(5).TrimEnd() + " " + arrLine(6) + Environment.NewLine, True, Encoding.ASCII)
                                intSeq += 1

                            End If
                            intSeq = 0
                            objGmtTime = Date.UtcNow
                            objLocalTime = Date.Now
                            intChartCount += 1
                        End If

                    Next
                Loop

            Else
                objClient.Close()
                objStream.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    'Reused from Voltaire code, tries to create file with current date/time, if file already exists, sleeps for half a second and tries again. Max tries is six
    Public Sub Create_SQL_File(strPassedData As String, strPassedEnd As String)
        Const MAX_RETRIES As Integer = 6

        Dim strFileName As String
        Dim intRetries As Integer

        intRetries = 0
        While intRetries < MAX_RETRIES
            strFileName = Directory.GetCurrentDirectory() & "\Data\" & CDate(Date.Now).ToString("yyyyMMddHHmmss") & strPassedEnd & ".sql"
            Try
                My.Computer.FileSystem.WriteAllText(strFileName, strPassedData, True, Encoding.ASCII)
                Exit While
            Catch ex As Exception
                Thread.Sleep(500)
                intRetries = intRetries + 1
            End Try
        End While
        If intRetries = MAX_RETRIES Then
            MessageBox.Show("ZurexLeakTester.Create_SQL_File - Exceeded max retries.")
        End If
    End Sub

    'Checks every sixty seconds to see if SQL Processor is running, starts it if it is not
    Private Sub tmrSqlProcessor_Tick(sender As Object, e As EventArgs) Handles tmrSqlProcessor.Tick
        Dim objP() As Process
        objP = Process.GetProcessesByName("SQL Processor")
        If objP.Count = 0 Then
            Try
                Dim objTempProcess = New Process()
                objTempProcess.StartInfo.FileName = "C:\CDI\SQL Processor\SQL Processor.exe"
                objTempProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized
                objTempProcess.Start()
            Catch ex As Exception
            End Try
        End If
    End Sub

    'Helper methods used to avoid cross-thread exceptions
    Public Sub changeText(obj, str)
        obj.text = str
    End Sub
    Public Sub invokeChangeText(obj, str)
        obj.invoke(Sub() changeText(obj, str))
    End Sub

    'Increments the testerTimerCounter variable by one every second while updating the corresponding label
    'If the counter gets to 60, bgwTesterConnect_DoWork is called, and the counter is reset
    Private Sub bgwTesterTimer_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwTesterTimer.DoWork
        While True
            SyncLock (objTimerLock)
                'sleep for 1 second
                Thread.Sleep(1000)
                SyncLock (objTimerLock)
                    intTesterTimerCounter += 1
                End SyncLock
                invokeChangeText(lblConnectionTimer, intTesterTimerCounter)
                'Count to 60
                If intTesterTimerCounter >= 60 Then
                    bgwTesterConnect.RunWorkerAsync()
                    SyncLock (objTimerLock)
                        intTesterTimerCounter = -1
                    End SyncLock
                End If
            End SyncLock
        End While
    End Sub

    'Pause the background worker, disconnect the client, resume the background worker, reconnect the client
    Private Sub bgwTesterConnect_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwTesterConnect.DoWork
        Try
            bgwMain.WorkerSupportsCancellation = True
            bgwMain.CancelAsync()
            objClient.Client.Disconnect(False)

            Application.DoEvents()
            System.Threading.Thread.Sleep(10)
            Application.DoEvents()

            objClient = New TcpClient()
            objClient.Connect(objLocalAddress, strPortNum)
            bgwMain.RunWorkerAsync()
            intTesterTimerCounter = -1
        Catch ex As Exception

        End Try
    End Sub

    ''Subroutine that dynamically updates the current date, time, and connection status
    Private Sub tmrTimeDate_Tick(sender As Object, e As EventArgs) Handles tmrTimeDate.Tick
        lblTime.Text = DateTime.Now.ToLongTimeString
        'lblConnection.Text = "Status: " & isConnected()
        lblConnection.Text = "Connected: " & objClient.Connected
    End Sub

End Class

