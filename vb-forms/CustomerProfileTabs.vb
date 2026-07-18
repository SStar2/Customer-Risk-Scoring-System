Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class CustomerProfileTabs

    Private Sub ApplyStyling()
        Me.BackColor = Color.FromArgb(245, 247, 250)
        Me.Text = "Customer Profile"
        Me.MinimumSize = New Size(900, 560)

        btnLoad.BackColor = Color.FromArgb(33, 97, 140)
        btnLoad.ForeColor = Color.White
        btnLoad.FlatStyle = FlatStyle.Flat
        btnLoad.FlatAppearance.BorderSize = 0
        btnLoad.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnLoad.Cursor = Cursors.Hand

        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is Label Then
                ctrl.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                ctrl.ForeColor = Color.FromArgb(33, 97, 140)
            End If
        Next

        txtCustomerID.Font = New Font("Segoe UI", 10)
        txtCustomerID.BorderStyle = BorderStyle.FixedSingle

        StyleGrid(dgvAccounts)
        StyleGrid(dgvPayments)
        StyleGrid(dgvTransactions)
    End Sub

    Private Sub StyleGrid(dgv As DataGridView)
        dgv.BackgroundColor = Color.White
        dgv.BorderStyle = BorderStyle.None
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 97, 140)
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgv.DefaultCellStyle.Font = New Font("Segoe UI", 9)
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(174, 214, 241)
        dgv.DefaultCellStyle.SelectionForeColor = Color.Black
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 245, 251)
        dgv.RowHeadersVisible = False
        dgv.GridColor = Color.FromArgb(200, 220, 235)
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgv.ScrollBars = ScrollBars.Both
        dgv.AllowUserToAddRows = False
        dgv.Dock = DockStyle.Fill
    End Sub

    Private Sub CustomerProfileTabs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplyStyling()
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Try
            If String.IsNullOrEmpty(txtCustomerID.Text) Then
                MessageBox.Show("Please enter a Customer ID.", "Input Required",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim customerID As Integer
            If Not Integer.TryParse(txtCustomerID.Text, customerID) Then
                MessageBox.Show("Please enter a valid numeric Customer ID.", "Invalid Input",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim connStr As String = ConfigurationManager.ConnectionStrings("CreditRiskDB").ConnectionString

            Using conn As New SqlConnection(connStr)
                conn.Open()

                ' Tab 1 - Account Summary
                Dim accountQuery As String =
                    "SELECT AccountID, AccountType, CreditLimit, Balance, OpenedDate, Status
                     FROM Account
                     WHERE CustomerID = @CustomerID
                     ORDER BY OpenedDate"
                Dim daAccounts As New SqlDataAdapter(accountQuery, conn)
                daAccounts.SelectCommand.Parameters.AddWithValue("@CustomerID", customerID)
                Dim dtAccounts As New DataTable()
                daAccounts.Fill(dtAccounts)
                dgvAccounts.DataSource = dtAccounts

                If dtAccounts.Rows.Count = 0 Then
                    MessageBox.Show("No accounts found for Customer ID " & customerID.ToString(),
                                    "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If

                ' Tab 2 - Payment History
                Dim paymentQuery As String =
                    "SELECT ph.PaymentID, ph.AccountID, ph.BillingCycleDate,
                            ph.AmountDue, ph.AmountPaid, ph.DaysLate
                     FROM PaymentHistory ph
                     JOIN Account a ON ph.AccountID = a.AccountID
                     WHERE a.CustomerID = @CustomerID
                     ORDER BY ph.BillingCycleDate DESC"
                Dim daPayments As New SqlDataAdapter(paymentQuery, conn)
                daPayments.SelectCommand.Parameters.AddWithValue("@CustomerID", customerID)
                Dim dtPayments As New DataTable()
                daPayments.Fill(dtPayments)
                dgvPayments.DataSource = dtPayments

                ' Tab 3 - Transaction History
                Dim transactionQuery As String =
                    "SELECT th.TransactionID, th.AccountID, th.TransactionDate,
                            th.Amount, th.TransactionType, th.Description
                     FROM TransactionHistory th
                     JOIN Account a ON th.AccountID = a.AccountID
                     WHERE a.CustomerID = @CustomerID
                     ORDER BY th.TransactionDate DESC"
                Dim daTransactions As New SqlDataAdapter(transactionQuery, conn)
                daTransactions.SelectCommand.Parameters.AddWithValue("@CustomerID", customerID)
                Dim dtTransactions As New DataTable()
                daTransactions.Fill(dtTransactions)
                dgvTransactions.DataSource = dtTransactions
            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class
