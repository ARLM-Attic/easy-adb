Imports System.IO
Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Collections

Public Class Form1
    Public commandoutput As String
    Public commandoutputscript As String
    Public treeview1isclicked As Integer
    Public currentpath As String = ""
    Public historyback(0 To 5) As String
    Public historyforward(0 To 5) As String

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles Me.Load
        getdataapps()
        explorerexplore("/")
        ToolStripTextBox1.Text = "Home"
        TreeView1.ExpandAll()
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
    Private Sub ExitToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub
    Private Sub Form1_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        TreeView1.Size = New Size(Width - 130, Height - 129)
        Button1.Location = New Point(Width - 120, 6)
        Button2.Location = New Point(Width - 120, 35)
        Button3.Location = New Point(Width - 120, 64)
        ListView1.Size = New Size(Width - 33, Height - 140)
    End Sub
    Private Sub AboutEasyADBToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AboutEasyADBToolStripMenuItem.Click
        AboutBox1.ShowDialog()
    End Sub
    Private Sub ListView1_DoubleClick(sender As Object, e As System.EventArgs) Handles ListView1.DoubleClick
        Select Case ListView1.SelectedItems(0).SubItems(1).Text
            Case "Link"
                currentpath = ListView1.SelectedItems(0).SubItems(2).Text
                ToolStripTextBox1.Text = currentpath
                explorerexplore(currentpath)
            Case "Folder"
                currentpath = currentpath & "/" & ListView1.SelectedItems.Item(0).Text
                explorerexplore(currentpath)
                ToolStripTextBox1.Text = currentpath
        End Select


    End Sub
    'moet aangepast worden 
    Private Sub ToolStripButton1_Click(sender As Object, e As System.EventArgs) Handles ToolStripButton1.Click
        If Not currentpath = Nothing Then
            Dim count As Integer = currentpath.Split("/").Length - 1
            currentpath = currentpath.Remove(currentpath.Length - currentpath.Split("/")(count).Length - 1)
            explorerexplore(currentpath)
            ToolStripTextBox1.Text = currentpath
        End If
    End Sub
    Private Sub ToolStripButton2_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripButton2.Click
        currentpath = ToolStripTextBox1.Text
        explorerexplore(currentpath)
    End Sub
    Private Sub ToolStripTextBox1_TextChanged1(sender As Object, e As System.EventArgs) Handles ToolStripTextBox1.TextChanged
        If currentpath = Nothing Or currentpath = "/" Then
            ToolStripTextBox1.Text = "Home"
        Else
            currentpath = ToolStripTextBox1.Text
        End If
    End Sub

    'opdrachten
    Sub explorerexplore(path As String)
        Process1.StartInfo.Arguments = "shell"
        Dim stopper As Integer = 0
        Process1.Start()
        Process1.StandardInput.WriteLine("cd """ & path & """ ")
        Process1.StandardInput.WriteLine("ls -l | grep ^d ")
        Process1.StandardInput.WriteLine("ls -l | grep ^l ")
        Process1.StandardInput.WriteLine("ls -l | grep ^- ")
        Process1.StandardInput.WriteLine("exit ")
        Dim first As String = Process1.StandardOutput.ReadLine
        Dim count As Integer = 0
        ListView1.Items.Clear()
        Do Until Process1.StandardOutput.EndOfStream
            Dim output As String = Process1.StandardOutput.ReadLine
            If output.Contains(" ") And output.Contains("can't cd to") Then
                MsgBox("Error Path doesn't exits")
                currentpath = "/"
                ToolStripTextBox1.Text = currentpath
                explorerexplore(currentpath)
                GoTo errorline
            End If
            If Not output = Nothing And Not output.EndsWith(" ") Then
                Dim objectname As String = output.Split(" ")(output.Split(" ").Length - 1)
                Select Case output.Substring(0, 1)
                    Case "d"
                        ListView1.Items.Add(objectname, 0).SubItems.Add("Folder")
                    Case "l"
                        ' ListView1.Items.Add(output.Split(" ")(output.Split(" ").Length - 3), 1)
                        ListView1.Items.Add(New ListViewItem(New String() {output.Split(" ")(output.Split(" ").Length - 3), "Link", objectname}, 1))

                    Case ("-")
                        ListView1.Items.Add(objectname, 2).SubItems.Add("File")
                End Select
            Else

            End If

        Loop
errorline:
        Process1.WaitForExit()
    End Sub
    Sub uninstallapp()
        ToolStripStatusLabel1.Text = "unistalling application, please wait..."
        If treeview1isclicked = True Then
            If TreeView1.SelectedNode.Level = 1 Then
                Dim sort As Integer = TreeView1.SelectedNode.Parent.Index
                Select Case sort
                    Case 0
                        If MsgBox("Are You sure that you wanna uninstall """ & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text & """", MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation) = MsgBoxResult.Yes Then
                            adb_command("uninstall " & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text.Replace("-1", "").Replace("-2", ""))
                            MsgBox(commandoutput)
                            getdataapps()
                        End If
                    Case 1
                        If MsgBox("Are You sure that you wanna uninstall """ & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text & """", MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation) = MsgBoxResult.Yes Then
                            adb_command("uninstall " & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text.Replace("-1", "").Replace("-2", ""))
                            MsgBox(commandoutput)
                            getdataapps()
                        End If
                    Case 2
                        If MsgBox("Warning this is a system application, uninstalling this may cause malfunctioning of your device!!  are you sure you want to uninstall " & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text, MsgBoxStyle.YesNo Or MsgBoxStyle.Critical) = MsgBoxResult.Yes Then
                            adb_command("pull /system/app/" & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text & ".apk backup\" & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text & ".apk")
                            adb_command("remount")
                            adb_command("shell rm /system/app/" & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text & ".apk")
                            MsgBox("Done")
                        End If
                End Select
            End If
        End If
        getdataapps()
        ToolStripStatusLabel1.Text = "Done!"
    End Sub
    Sub getdataapps()
        ToolStripStatusLabel1.Text = "Loading application data..."
        TreeView1.Nodes.Clear()
        TreeView1.Nodes.Add("DATA")
        adb_shell_script("script\GetappDATA.eadbss", 1, 0, "")
        TreeView1.Nodes.Add("SDCARD")
        adb_shell_script("script\GetappSDCARD.eadbss", 1, 1, "")
        TreeView1.Nodes.Add("SYSTEM")
        adb_shell_script("script\GetappSYSTEM.eadbss", 1, 2, "")
        ToolStripStatusLabel1.Text = "Done!"
    End Sub
    Sub pullapp()
        If treeview1isclicked = True Then
            If TreeView1.SelectedNode.Level = 1 Then
                ToolStripStatusLabel1.Text = "pulling application from device, please wait..."
                If APKpuller.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    Dim sort As Integer = TreeView1.SelectedNode.Parent.Index
                    Select Case sort
                        Case 0
                            adb_command("pull /data/app/" & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text & ".apk """ & APKpuller.SelectedPath & """")
                        Case 1
                            adb_command("pull /mnt/asec/" & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text & "/pkg.apk """ & APKpuller.SelectedPath & """")
                            My.Computer.FileSystem.RenameFile(APKpuller.SelectedPath & "\pkg.apk", TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text & ".apk")
                        Case 2
                            adb_command("pull /system/app/" & TreeView1.Nodes(sort).Nodes(TreeView1.SelectedNode.Index).Text & ".apk """ & APKpuller.SelectedPath & """")
                    End Select
                End If
                ToolStripStatusLabel1.Text = "Done!"
            End If
        End If
    End Sub
    Sub installapk()
        'install application
        If Dialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            ToolStripStatusLabel1.Text = "installing application, please wait..."
            If installcomand.Contains("#INSTALLSYSTEM#") Then
                Dim filename As String = My.Computer.FileSystem.GetFileInfo(installcomand.Replace("#INSTALLSYSTEM#", "")).Name
                adb_command("remount")
                adb_command("shell [ -f /system/app/" & filename & " ] && echo ""File exists"" || echo ""File does not exists""")
                If commandoutput = "File exists" Then
                    If MsgBox("Warning, The application already exist! Do you want to overwrite", MsgBoxStyle.YesNo Or MsgBoxStyle.Critical) = MsgBoxResult.Yes Then
                        GoTo verder
                    End If
                Else
verder:
                    adb_command("push """ & installcomand.Replace("#INSTALLSYSTEM#", "") & """ /system/app")
                    adb_command("shell chmod 644 /system/app/" & filename)
                    If MsgBox("It is recommended that you reboot your device. do you want to reboot the device?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        adb_command("reboot")
                    End If
                End If
            Else
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
            End If
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

    Sub adb_shell_script(path As String, show As Integer, categorie As Integer, filename As String)
        start_server()
        If Device_connected() = True Then
            Process1.StartInfo.Arguments = "shell"
            Dim script As FileStream = New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)
            Dim scriptreader As StreamReader = New StreamReader(script)
            Process1.Start()
            Do Until scriptreader.EndOfStream
                Application.DoEvents()
                Dim input As String = scriptreader.ReadLine()
                Process1.StandardInput.WriteLine(input.Replace("#FILENAME#", filename) & " ")

            Loop
            scriptreader.Close()
            Do Until Process1.StandardOutput.EndOfStream
                Application.DoEvents()
                Dim output As String = Process1.StandardOutput.ReadLine
                If Not output = Nothing Then
                    Select Case show
                        Case 0
                            If Not output.Contains(" ") And Not output.Contains(".tmp") And Not output.Contains("odex") Then
                                commandoutput = output
                                MsgBox(output)
                            End If
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

    Private Sub TreeView1_Click(sender As Object, e As System.EventArgs) Handles TreeView1.Click
        treeview1isclicked = True
    End Sub

    Private Sub RestoreDeletedSystemAppsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles RestoreDeletedSystemAppsToolStripMenuItem.Click
        Restore_system_apps.ShowDialog()
    End Sub

    Private Sub InstallApplicationToolStripMenuItem_Click_1(sender As System.Object, e As System.EventArgs) Handles InstallApplicationToolStripMenuItem.Click

    End Sub
End Class
