Imports System.IO
Public Class Form1

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs)
        
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        
    End Sub

    Private Sub Form1_SizeChanged(sender As Object, e As System.EventArgs) Handles Me.SizeChanged

    End Sub

    Private Sub GETSYSTEMDATAToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles GETSYSTEMDATAToolStripMenuItem.Click
        TreeView1.Nodes.Add("DATA")
        TreeView1.Nodes.Add("SDCARD")
        adb_shell_script("script\Getdataapps.eadbss", 1, "")
    End Sub
    Private Sub InstallApplicationToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles InstallApplicationToolStripMenuItem.Click
        APKhandeler.ShowDialog()
    End Sub

    Private Sub APKhandeler_FileOk(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles APKhandeler.FileOk
        adb_command("install """ & APKhandeler.FileName & """")
    End Sub


    'functions
    Sub adb_command(command As String)
        start_server()
        If Device_connected() = True Then
            Process1.StartInfo.Arguments = command
            Process1.Start()
            Do Until Process1.StandardOutput.EndOfStream
                Dim output As String = Process1.StandardOutput.ReadLine
                If Not output = Nothing Then
                    'ListBox1.Items.Add(output.Replace("./", ""))
                End If
            Loop
            Process1.WaitForExit()
        End If
    End Sub

    Sub adb_shell_script(path As String, show As Integer, file As String)
        start_server()
        If Device_connected() = True Then
            Process1.StartInfo.Arguments = "shell"
            Dim script As FileStream = New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)
            Dim scriptreader As StreamReader = New StreamReader(script)
            Process1.Start()
            Do Until scriptreader.EndOfStream
                Process1.StandardInput.WriteLine(scriptreader.ReadLine().Replace("#file#", file) & " ")
            Loop
            scriptreader.Close()
            Do Until Process1.StandardOutput.EndOfStream
                Dim output As String = Process1.StandardOutput.ReadLine
                Dim limit As Boolean
                If Not output = Nothing Then
                    Select Case show
                        Case 0
                            'ListBox1.Items.Add(output)
                        Case 1
                            If Not output.Contains(" ") Then
                                If output.Contains(".apk") Then
                                    TreeView1.Nodes(0).Nodes.Add(output.Replace(".apk", ""))
                                Else
                                    TreeView1.Nodes(1).Nodes.Add(output.Replace(".apk", ""))
                                End If

                            End If
                    End Select
                    limit = False
                End If
            Loop
            Process1.WaitForExit()
        End If
    End Sub

    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If TreeView1.SelectedNode.Level = 1 Then
            Dim sort As Integer = TreeView1.SelectedNode.Parent.Index
            Select Case sort
                Case 0
                    MsgBox("pull /data/app/" & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text & ".apk " & Application.StartupPath)
                Case 1
                    MsgBox(TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text)
            End Select

        End If

    End Sub
    Sub start_server()
        Process1.StartInfo.Arguments = "start-server"
        Process1.Start()
        Process1.WaitForExit()
    End Sub
End Class
