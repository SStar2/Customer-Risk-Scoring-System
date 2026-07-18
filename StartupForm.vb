Public Class StartupForm

    Private Sub ApplyStyling()
        Me.BackColor = Color.FromArgb(245, 247, 250)
        Me.Text = "Credit Risk System"
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False

        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is Button Then
                Dim btn As Button = CType(ctrl, Button)
                btn.BackColor = Color.FromArgb(33, 97, 140)
                btn.ForeColor = Color.White
                btn.FlatStyle = FlatStyle.Flat
                btn.FlatAppearance.BorderSize = 0
                btn.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                btn.Cursor = Cursors.Hand
            End If
            If TypeOf ctrl Is Label Then
                ctrl.ForeColor = Color.FromArgb(33, 97, 140)
            End If
        Next

        lblTitle.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        lblSubtitle.Font = New Font("Segoe UI", 10)
        lblSubtitle.ForeColor = Color.Gray
    End Sub

    Private Sub StartupForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplyStyling()
    End Sub

    Private Sub btnPortfolio_Click(sender As Object, e As EventArgs) Handles btnPortfolio.Click
        Dim frm As New PortfolioGridForm()
        frm.Show()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim frm As New SearchByTierForm()
        frm.Show()
    End Sub

    Private Sub btnDetail_Click(sender As Object, e As EventArgs) Handles btnDetail.Click
        Dim frm As New CustomerDetailForm()
        frm.Show()
    End Sub

    Private Sub btnTabs_Click(sender As Object, e As EventArgs) Handles btnTabs.Click
        Dim frm As New CustomerProfileTabs()
        frm.Show()
    End Sub

End Class