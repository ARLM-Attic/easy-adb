Imports easyADB.Form1
Imports System.IO
Module Module1
    Function Device_connected() As Boolean
        Form1.Process1.StartInfo.Arguments = "devices"
        Form1.Process1.Start()
        Dim functionoutput As String = "error"
        Do Until Form1.Process1.StandardOutput.EndOfStream
            Dim output As String = Form1.Process1.StandardOutput.ReadLine
            If Not output = Nothing Then
                functionoutput = output
            End If
        Loop
        If functionoutput.Contains("List") Then
            functionoutput = False
        Else
            functionoutput = True
        End If
        Form1.Process1.WaitForExit()
        Device_connected = functionoutput
    End Function
End Module
