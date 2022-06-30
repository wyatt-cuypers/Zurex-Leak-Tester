Public Class frmError

    Private WithEvents tmrErrorPage As New Timer
    Private Sub frmError_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tmrErrorPage.Interval = 5000
        tmrErrorPage.Start()
    End Sub

    Private Sub tmrErrorPage_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrErrorPage.Tick
        'Closes the form after three seconds
        Application.Exit()
        Close()
    End Sub

End Class