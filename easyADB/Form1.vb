Imports System.IO
Public Class Form1

    Private Sub GETSYSTEMDATAToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles GETSYSTEMDATAToolStripMenuItem.Click
        TreeView1.Nodes.Add("DATA")
        TreeView1.Nodes.Add("SDCARD")
        TreeView1.Nodes.Add("SYSTEM")
        adb_shell_script("script\Getdataapps.eadbss", 1, "")
    End Sub
    Private Sub InstallApplicationToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles InstallApplicationToolStripMenuItem.Click
        'APKinstaller.ShowDialog()
    End Sub

    Private Sub APKhandeler_FileOk(sender As System.Object, e As System.ComponentModel.CancelEventArgs)
        'adb_command("install """ & APKinstaller.FileName & """")
    End Sub

    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If TreeView1.SelectedNode.Level = 1 Then
            If APKpuller.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Dim sort As Integer = TreeView1.SelectedNode.Parent.Index
                Select Case sort
                    Case 0
                        adb_command("pull /data/app/" & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text & ".apk """ & APKpuller.SelectedPath & """")
                    Case 1
                        adb_command("pull /mnt/asec/" & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text & "/pkg.apk """ & APKpuller.SelectedPath & """")
                        My.Computer.FileSystem.RenameFile("pkg.apk", TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text & ".apk")
                    Case 2
                        adb_command("pull /system/app/" & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text & ".apk """ & APKpuller.SelectedPath & """")
                End Select
            End If
        End If

    End Sub
    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        If TreeView1.SelectedNode.Level = 1 Then
            Dim sort As Integer = TreeView1.SelectedNode.Parent.Index
            Select Case sort
                Case 0
                    adb_command("uninstall " & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text.Replace("-1", "").Replace("-2", ""))
                Case 1
                    adb_command("uninstall " & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text.Replace("-1", "").Replace("-2", ""))
                Case 2
                    MsgBox("Deleting system apps are tempory not avaible")
            End Select
        End If
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
                    ListBox1.Items.Add(output)
                End If
            Loop
            Process1.WaitForExit()
        End If
        MsgBox(Process1.ExitCode)
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
            Dim sort As Boolean
            Do Until Process1.StandardOutput.EndOfStream
                Dim output As String = Process1.StandardOutput.ReadLine
                If Not output = Nothing Then
                    Select Case show
                        Case 0
                            TreeView1.Nodes.Add(output)
                        Case 1
                            If Not output.Contains(" ") Then
                                If output.Contains(".apk") Then
                                    If sort = False Then
                                        TreeView1.Nodes(0).Nodes.Add(output.Replace(".apk", ""))
                                    Else
                                        TreeView1.Nodes(2).Nodes.Add(output.Replace(".apk", ""))
                                    End If
                                Else
                                    If Not output.Contains(".odex") Then
                                        TreeView1.Nodes(1).Nodes.Add(output)
                                    End If
                                    sort = True

                                    End If

                            End If
                    End Select
                    
                End If
            Loop
            Process1.WaitForExit()
        End If
    End Sub
    Sub start_server()
        Process1.StartInfo.Arguments = "start-server"
        Process1.Start()
        Process1.WaitForExit()
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click

    End Sub
End Class
