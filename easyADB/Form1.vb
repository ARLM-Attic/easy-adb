Imports System.IO
Public Class Form1
    Public commandoutput As String
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        getdataapps()
    End Sub
    Private Sub InstallApplicationToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles InstallApplicationToolStripMenuItem.Click
        installapk()
    End Sub
    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        pullapp()
    End Sub
    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        installapk()
    End Sub
    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        uninstallapp()
    End Sub
    Private Sub RefreshToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles RefreshToolStripMenuItem.Click
        getdataapps()
    End Sub
    Private Sub Form1_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        TreeView1.Size = New Size(Width - 130, Height - 129)
        Button1.Location = New Point(Width - 120, 6)
        Button2.Location = New Point(Width - 120, 35)
        Button3.Location = New Point(Width - 120, 64)
    End Sub

    'opdrachten
    Sub uninstallapp()
        If TreeView1.SelectedNode.Level = 1 Then
            Dim sort As Integer = TreeView1.SelectedNode.Parent.Index
            Select Case sort
                Case 0
                    adb_command("uninstall " & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text.Replace("-1", "").Replace("-2", ""))
                    MsgBox(commandoutput)
                    getdataapps()
                Case 1
                    adb_command("uninstall " & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text.Replace("-1", "").Replace("-2", ""))
                    MsgBox(commandoutput)
                    getdataapps()
                Case 2
                    MsgBox("Deleting system apps are tempory not avaible")
            End Select
        End If


    End Sub
    Sub getdataapps()
        ToolStripStatusLabel1.Text = "Loading application data..."
        TreeView1.Nodes.Clear()
        TreeView1.Nodes.Add("DATA")
        adb_shell_script("script\GetappDATA.eadbss", 1, 0)
        TreeView1.Nodes.Add("SDCARD")
        adb_shell_script("script\GetappSDCARD.eadbss", 1, 1)
        TreeView1.Nodes.Add("SYSTEM")
        adb_shell_script("script\GetappSYSTEM.eadbss", 1, 2)
        ToolStripStatusLabel1.Text = "Done!"
    End Sub
    Sub pullapp()
        ToolStripStatusLabel1.Text = "pulling app from device..."
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
        ToolStripStatusLabel1.Text = "Done!"
    End Sub
    Sub installapk()
        'install application
        If Dialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            ToolStripStatusLabel1.Text = "installing application, Pleas WAIT..."
            adb_command(installcomand)
            Select Case commandoutput
                Case "Failure [INSTALL_FAILED_ALREADY_EXISTS]"
                    ToolStripStatusLabel1.Text = "failed, application already exist"
                    MsgBox("failed, application already exist", MsgBoxStyle.Critical)
                Case Else
                    MsgBox(commandoutput)
                    ToolStripStatusLabel1.Text = "done"
                    Clipboard.SetText(commandoutput)
            End Select
            getdataapps()
        End If
    End Sub

    'functions
    Sub adb_command(command As String)
        start_server()
        If Device_connected() = True Then
            Process1.StartInfo.Arguments = command
            Process1.Start()
            Do Until Process1.StandardOutput.EndOfStream
                Application.DoEvents()
                Dim output As String = Process1.StandardOutput.ReadLine
                If Not output = Nothing Then
                    commandoutput = output
                End If
            Loop
            Process1.WaitForExit()
        End If
    End Sub

    Sub adb_shell_script(path As String, show As Integer, categorie As Integer)
        start_server()
        If Device_connected() = True Then
            Process1.StartInfo.Arguments = "shell"
            Dim script As FileStream = New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)
            Dim scriptreader As StreamReader = New StreamReader(script)
            Process1.Start()
            Do Until scriptreader.EndOfStream
                Application.DoEvents()
                Dim input As String = scriptreader.ReadLine()
                Process1.StandardInput.WriteLine(input & " ")

            Loop
            scriptreader.Close()
            Do Until Process1.StandardOutput.EndOfStream
                Application.DoEvents()
                Dim output As String = Process1.StandardOutput.ReadLine
                If Not output = Nothing Then
                    Select Case show
                        Case 0
                            MsgBox(output)
                        Case 1
                            If Not output.Contains(" ") And Not output.Contains(".tmp") And Not output.Contains("odex") Then
                                TreeView1.Nodes(categorie).Nodes.Add(output.Replace(".apk", ""))
                            End If
                    End Select

                End If
            Loop
            Select Case show
                Case 2
                    'If installerror.Contains( Then

                    'End If
            End Select

            Process1.WaitForExit()
        End If
    End Sub
    Sub start_server()
        Process1.StartInfo.Arguments = "start-server"
        Process1.Start()
        Process1.WaitForExit()
    End Sub
End Class
