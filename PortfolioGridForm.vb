Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class PortfolioGridForm

    Private Sub ApplyStyling()
        Me.BackColor = Color.FromArgb(245, 247, 250)
        Me.Text = "Customer Risk Portfolio"
        Me.MinimumSize = New Size(900, 550)

        btnLoad.BackColor = Color.FromArgb(33, 97, 140)
        btnLoad.ForeColor = Color.White
        btnLoad.FlatStyle = FlatStyle.Flat
        btnLoad.FlatAppearance.BorderSize = 0
        btnLoad.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnLoad.Cursor = Cursors.Hand
        btnLoad.Size = New Size(150, 35)
        btnLoad.Location = New Point(12, 12)
        btnLoad.Text = "Load Portfolio"
        btnLoad.Anchor = AnchorStyles.Top Or AnchorStyles.Left

        dataGridView1.Location = New Point(12, 55)
        dataGridView1.Size = New Size(Me.ClientSize.Width - 24, Me.ClientSize.Height - 67)
        dataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or
                               AnchorStyles.Left Or AnchorStyles.Right
        dataGridView1.BackgroundColor = Color.White
        dataGridView1.BorderStyle = BorderStyle.None
        dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 97, 140)
        dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dataGridView1.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dataGridView1.DefaultCellStyle.Font = New Font("Segoe UI", 9)
        dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(174, 214, 241)
        dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black
        dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 245, 251)
        dataGridView1.RowHeadersVisible = False
        dataGridView1.GridColor = Color.FromArgb(200, 220, 235)
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dataGridView1.ScrollBars = ScrollBars.Both
        dataGridView1.AllowUserToAddRows = False
    End Sub

    Private Sub frmPortfolioGrid_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplyStyling()
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Try
            Dim connStr As String = ConfigurationManager.ConnectionStrings("CreditRiskDB").ConnectionString
            Using conn As New SqlConnection(connStr)
                Dim query As String = "SELECT * FROM vCustomerRiskSummary ORDER BY NumericScore DESC"
                Dim da As New SqlDataAdapter(query, conn)
                Dim dt As New DataTable()
                da.Fill(dt)
                dataGridView1.DataSource = dt
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Connection Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class