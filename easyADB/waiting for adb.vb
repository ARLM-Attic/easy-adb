﻿Imports System.Windows.Forms

Public Class waiting_for_adb

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub waiting_for_adb_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
