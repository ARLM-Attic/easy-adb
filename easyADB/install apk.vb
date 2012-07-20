Imports System.Windows.Forms

Public Class Dialog1

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        'Me.Close()
        Dim SDcard As String
        Dim arguments As String
        If CheckBox2.Checked = True Then
            SDcard = "-s"
        Else
            SDcard = ""
        End If
        
        If CheckBox1.Checked = True Then
            arguments = "-r"
        Else
            arguments = ""
        End If
        installcomand = "install " & arguments & " " & SDcard & " """ & TextBox1.Text & """"
        'MsgBox("install " & arguments & " " & SDcard & " """ & TextBox1.Text & """")
        Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        APKhandler.ShowDialog()
    End Sub

    Private Sub APKhandler_FileOk(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles APKhandler.FileOk
        TextBox1.Text = APKhandler.FileName
    End Sub

    Private Sub Dialog1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        TextBox1.ReadOnly = True
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked = True Then
            If MsgBox("Warning, make sure that this is a system/app", MsgBoxStyle.YesNo Or MsgBoxStyle.Critical) = MsgBoxResult.Yes Then

            End If
        End If
    End Sub
End Class
