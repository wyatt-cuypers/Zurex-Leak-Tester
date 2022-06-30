Public Class frmStartPage
    Private WithEvents tmrStartupPage As New Timer
    Private Sub frmStartPage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Sets the startUpPage timer's interval to two and a half seconds
        lblTitle.Text = lblTitle.Text + Application.ProductVersion
        tmrStartupPage.Interval = 2500
        tmrStartupPage.Start()
    End Sub
    Private Sub tmrStartupPage_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrStartupPage.Tick
        'Closes the form after two and a half seconds
        Close()
    End Sub
End Class