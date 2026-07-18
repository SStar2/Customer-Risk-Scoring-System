Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class CustomerDetailForm

    Private Sub ApplyStyling()
        Me.BackColor = Color.FromArgb(245, 247, 250)
        Me.Text = "Customer Risk Detail"

        btnLoad.BackColor = Color.FromArgb(33, 97, 140)
        btnLoad.ForeColor = Color.White
        btnLoad.FlatStyle = FlatStyle.Flat
        btnLoad.FlatAppearance.BorderSize = 0
        btnLoad.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnLoad.Cursor = Cursors.Hand
        btnLoad.Text = "Load Customer"

        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is TextBox Then
                Dim tb As TextBox = CType(ctrl, TextBox)
                tb.Font = New Font("Segoe UI", 10)
                tb.BorderStyle = BorderStyle.FixedSingle
                tb.BackColor = Color.White
            End If
            If TypeOf ctrl Is Label Then
                ctrl.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                ctrl.ForeColor = Color.FromArgb(33, 97, 140)
            End If
        Next
    End Sub

    Private Sub CustomerDetailForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplyStyling()
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Try
            If String.IsNullOrEmpty(txtCustomerID.Text) Then
                MessageBox.Show("Please enter a Customer ID.", "Input Required",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim connStr As String = ConfigurationManager.ConnectionStrings("CreditRiskDB").ConnectionString
            Using conn As New SqlConnection(connStr)
                Dim cmd As New SqlCommand("dbo.usp_GetCustomerRiskReport", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@CustomerID", Integer.Parse(txtCustomerID.Text))
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    txtFullName.Text = reader("FullName").ToString()
                    txtEmployment.Text = reader("EmploymentStatus").ToString()
                    txtScore.Text = reader("LatestScore").ToString()
                    txtTier.Text = reader("RiskTier").ToString()
                    txtTotalAccounts.Text = reader("AccountID").ToString()
                    txtTotalBalance.Text = reader("Balance").ToString()
                Else
                    MessageBox.Show("No customer found with that ID.", "Not Found",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class