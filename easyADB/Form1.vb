Imports System.IO
Public Class Form1

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Process1.StartInfo.FileName = "adb\adb.exe"
        Process1.StartInfo.Arguments = "shell"
        Process1.StartInfo.UseShellExecute = False
        Process1.StartInfo.RedirectStandardOutput = True
        Process1.StartInfo.RedirectStandardInput = True
        Process1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        Process1.Start()
        'Process1.StandardInput.WriteLine(" ""/system/build.prop""" & " """ & Application.StartupPath & """")
        'Process1.StandardInput.WriteLine("shell")
        Process1.StandardInput.WriteLine("cd ""/data/app""")
        Process1.StandardInput.WriteLine("find")
        Process1.StandardInput.WriteLine("exit")
        Process1.StandardInput.WriteLine("exit")
        Do Until Process1.StandardOutput.EndOfStream
            Dim output As String = Process1.StandardOutput.ReadLine
            Dim filter As Integer
            If Not output = Nothing And output.Contains("./") Then
                ListBox1.Items.Add(output.Replace("./", ""))
            End If
            filter = 1
        Loop
        Process1.WaitForExit()



    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        MsgBox(Application.StartupPath)
    End Sub

    Private Sub Form1_SizeChanged(sender As Object, e As System.EventArgs) Handles Me.SizeChanged
        ListBox1.Size = New Size(800, Height - 62)
        Button1.Location = New Point(830, 50)
    End Sub
End Class
