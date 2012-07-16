Public Class Form1

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Process1.StartInfo.FileName = "adb"
        Process1.StartInfo.Arguments = "shell"
        Process1.StartInfo.UseShellExecute = False
        Process1.StartInfo.RedirectStandardOutput = True
        Process1.StartInfo.RedirectStandardInput = True
        Process1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        Process1.Start()
        Process1.StandardInput.WriteLine("cd /data/app")
        Process1.StandardInput.WriteLine("find")
        Process1.StandardInput.WriteLine("exit")
        Do Until Process1.StandardOutput.EndOfStream
            Dim output As String = Process1.StandardOutput.ReadLine
            Dim filter As Integer
            If Not output = Nothing And output.Contains(".") And filter > 0 Then
                ListBox1.Items.Add(output)
            End If
        Loop
        Process1.WaitForExit()
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Form1_SizeChanged(sender As Object, e As System.EventArgs) Handles Me.SizeChanged
        ListBox1.Size = New Size(189, Height - 62)
    End Sub
End Class
