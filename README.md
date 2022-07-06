# Zurex Leak Tester
The Zurex Leak Tester program is a windows forms application written in Visual Basic .NET. The purpose of writing this program was to be able to more easily capture and store data coming from an Isaac Leak Tester.

# Startup
When running, the leak tester is connected to a desktop via ethernet cable. Upon startup, the program attempts to connect to the leak tester by instantiating a new TcpClient object. The parameters for this object are read from a settings file which is stored in the project directory. If the program is unable to connect to the leak tester, a form is diplayed on the screen, and the program shuts itself down. If the connection is sucessful, a form which has the name of the program and the version number is displayed on the screen for a few seconds. Behind the scenes, the program checks to see if folders titled Data and Raw Data exist, and if they do not, they are created. After this, the main form is displayed. 

# Main function
bgwMain.RunWorkerAsync is called in the form's load method, and this is where the code that drives the application is executed. In the bgwMain_DoWork method, the code looks to see if data can be read from the objStream NetworkStream object. If it can, a Do Until loop executed, and this runs until the the TcpClient object is disconnected. Nothing happens with the program until a button is pressed on the leak tester. When the button is pressed, the data is read onto the screen. This data is displayed in two different places: in the form's textbox, and in the form's chart. The data that is pushed out is a number of lines with different times and numbers, and at the end of the data is a summary line. An example of the data is displayed below. So, after an iteration is finished, the program then sits and waits for more data to come in. 

# Disconnect / Reconnect
There is a timer on the form that starts at zero and counts up to sixty. With each iteration, this timer is reset to zero. If no iterations are run within sixty seconds, the program then the bgwTesterConnect.RunWorkerAsync() is executed. Upon this happening, the bgwMain background worker is cancelled, the TcpClient is manually disconnected, and the thread is put to sleep for a tenth of a millisecond. Then, a new TcpClient is instantiated and connected with the same settings as before, and bgwMain.RunWorkerAsync() is called again. The Do Until loop is not broken because the bgwMain background worker is cancelled before the Tcpclient is disconnected and then started again only after the TcpClient is reconnected.

# Recording the data - raw data
All of the data that comes from the leak tester is stored in multiple ways. The first way is simply recording the raw data, just the way it looks in the example below. One file is created per day to record this raw data, and each iteration is appended to this file. The title of this file is in the format yyyymmdd.sql. 

# Recording the data - _ff.sql files
The second way that the data is recorded is line by line. First, the data is split into each individual line and stored into an array. Next, a for-loop is started that goes for the length of the array, one iteration for each line. In the for-loop, each line is split again, this time into each individual piece of data. Each line starts with a word except the summary line, which starts with the date (see below). In order to see if it is a summary line or not, there is an if statement that checks if the current line starts with a letter. If it does, a new file is created and only that single line is written to it. The title of the file is yyyyMMddHHmmss_ff.sql. Because there can be a high volume of files being created not very far apart from each other, the _ff extension (current milliseconds) is added. The files are written this way because they are intended to be picked up by a companion program called Sql Processor, which executes the files and stores them into a database. That being said, what actually gets written to these files is a sql query. The lines are broken up into each individual piece of data so that other text can be added in between.  The code for what that looks like, along with the actual contents of one of these files is listed below. One of these files is created for every regular line of data.

# Recording the data - .sql files
If the input line does not begin with a letter, then an elseif statement checks to see if the line starts with a 1 or a 0 (since the first two characters in a summary line represent the month it was recorded). If it does, then a new file is created; the name of these summary line files slightly differ from the other lines. They are yyyyMMddHHmmss.sql, so the same except no milliseconds on the end. This is to differentiate them for users. The code for what that looks like, along with the actual contents of one of these files is listed below.

# Summary
All of these lines are written for each iteration, or each time the button on the leak tester is pressed. The program ends when the user closes out of it.

# Example data (code) yyyyMMddHHmmss_ff.sql
`"exec up_LeakTesterFullData_AddRec @MachineID='1',@GMTTime='" + objGmtTime.ToString + "',@LocalTime='" + objLocalTime.ToString + "',@Seq=" + intSeq.ToString + ",@Step='" + arrLine(0).TrimEnd + "',@Seconds='" + arrLine(1) + arrLine(2) + "',@Data1='" + arrLine(3) + "',@Data2='" + arrLine(4) + "',@Data3='" + arrLine(5) + "',@Data4='" + arrLine(6).Substring(0, 6) + "'"`

# Example data 20220621143218_26.sql
exec up_LeakTesterFullData_AddRec @MachineID='1',@GMTTime='6/21/2022 7:32:14 PM',@LocalTime='6/21/2022 2:32:14 PM',@Seq=20,@Step='Vent',@Seconds='0.00s',@Data1='0.0000',@Data2='0.0000',@Data3='0.0000',@Data4='0.0000'

# Example data (code) yyyyMMddHHmmss.sql
`"exec up_LeakTester_AddRec @MachineID='1',@GMTTime='" + objGmtTime.ToString + "',@LocalTime='" + objLocalTime.ToString + "',@ProgramNumber=" + arrLine(2) + ",@ProgramName='" + arrLine(3).TrimEnd + "',@TestType='" + arrLine(4).TrimEnd + "',@Result1='" + arrLine(5).TrimEnd + "',@Value1='" + arrLine(6).TrimEnd + "',@Result2='" + arrLine(7).TrimEnd + "',@Value2='" + arrLine(8).TrimEnd + "',@Result3='" + arrLine(9).TrimEnd + "',@Value3='" + arrLine(10).TrimEnd + "',@Result4='" + arrLine(11).TrimEnd + "',@Value4='" + arrLine(12).TrimEnd + "'"`

# Example data 20220621143218.sql 
exec up_LeakTester_AddRec @MachineID='1',@GMTTime='6/21/2022 7:32:14 PM',@LocalTime='6/21/2022 2:32:14 PM',@ProgramNumber=01,@ProgramName='PROG 1',@TestType='PD',@Result1='Lo Pressure',@Value1='0.0120',@Result2='Lo Pressure',@Value2='0.0083',@Result3='Lo Pressure',@Value3='0.0011',@Result4='Lo Pressure',@Value4='0.0076'

# Example data
Clamp1	0.00	s	0.0000	0.0000	0.0000  
Fill  	1.40	s	0.0120	0.0083	0.0012  
Fill  	1.30	s	0.0120	0.0083	0.0012  
Fill  	1.20	s	0.0120	0.0083	0.0012  
Fill  	1.10	s	0.0120	0.0083	0.0012  
Fill  	1.00	s	0.0120	0.0083	0.0012  
Fill  	0.90	s	0.0120	0.0083	0.0012  
Fill  	0.80	s	0.0120	0.0083	0.0012  
Fill  	0.70	s	0.0120	0.0083	0.0012  
Fill  	0.60	s	0.0120	0.0083	0.0012  
Fill  	0.50	s	0.0120	0.0083	0.0012  
Fill  	0.40	s	0.0120	0.0083	0.0012  
Fill  	0.30	s	0.0120	0.0083	0.0012  
Fill  	0.20	s	0.0120	0.0083	0.0012  
Fill  	0.10	s	0.0120	0.0083	0.0012  
Fill  	0.00	s	0.0120	0.0083	0.0012  
Vent  	0.40	s	0.0000	0.0000	0.0000  
Vent  	0.30	s	0.0000	0.0000	0.0000  
Vent  	0.20	s	0.0000	0.0000	0.0000  
Vent  	0.10	s	0.0000	0.0000	0.0000  
Vent  	0.00	s	0.0000	0.0000	0.0000  
6/21/2022 2:34:22 PM	01  PROG 1  PD    	Lo Pressure 0.0120 Lo Pressure  0.0083 Lo Pressure  0.0012 Lo Pressure  0.0077
