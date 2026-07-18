Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class SearchByTierForm

    Private Sub ApplyStyling()
        Me.BackColor = Color.FromArgb(245, 247, 250)
        Me.Text = "Search by Risk Tier"
        Me.MinimumSize = New Size(900, 550)

        cboTier.Location = New Point(12, 15)
        cboTier.Size = New Size(150, 30)
        cboTier.Font = New Font("Segoe UI", 10)
        cboTier.FlatStyle = FlatStyle.Flat
        cboTier.Anchor = AnchorStyles.Top Or AnchorStyles.Left

        btnSearch.Location = New Point(170, 13)
        btnSearch.Size = New Size(100, 35)
        btnSearch.BackColor = Color.FromArgb(33, 97, 140)
        btnSearch.ForeColor = Color.White
        btnSearch.FlatStyle = FlatStyle.Flat
        btnSearch.FlatAppearance.BorderSize = 0
        btnSearch.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnSearch.Cursor = Cursors.Hand
        btnSearch.Text = "Search"
        btnSearch.Anchor = AnchorStyles.Top Or AnchorStyles.Left

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
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dataGridView1.ScrollBars = ScrollBars.Both
        dataGridView1.AllowUserToAddRows = False
    End Sub

    Private Sub frmSearchByTier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplyStyling()
        cboTier.Items.AddRange(New String() {"EXCL", "VGOOD", "GOOD", "FAIR", "POOR"})
        cboTier.SelectedIndex = 0
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try
            If cboTier.SelectedItem Is Nothing Then
                MessageBox.Show("Please select a tier.", "Input Required",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim connStr As String = ConfigurationManager.ConnectionStrings("CreditRiskDB").ConnectionString
            Using conn As New SqlConnection(connStr)
                Dim query As String = "SELECT * FROM vCustomerRiskSummary WHERE RiskTierCode = @Tier ORDER BY NumericScore DESC"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@Tier", cboTier.SelectedItem.ToString())
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable()
                da.Fill(dt)
                dataGridView1.DataSource = dt
                If dt.Rows.Count = 0 Then
                    MessageBox.Show("No customers found for this tier.", "No Results",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class