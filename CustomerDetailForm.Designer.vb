<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CustomerDetailForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.txtCustomerID = New System.Windows.Forms.TextBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.txtFullName = New System.Windows.Forms.TextBox()
        Me.txtEmployment = New System.Windows.Forms.TextBox()
        Me.txtScore = New System.Windows.Forms.TextBox()
        Me.txtTier = New System.Windows.Forms.TextBox()
        Me.txtTotalAccounts = New System.Windows.Forms.TextBox()
        Me.txtTotalBalance = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtCustomerID
        '
        Me.txtCustomerID.Location = New System.Drawing.Point(175, 25)
        Me.txtCustomerID.Name = "txtCustomerID"
        Me.txtCustomerID.Size = New System.Drawing.Size(80, 26)
        Me.txtCustomerID.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'txtFullName
        '
        Me.txtFullName.Location = New System.Drawing.Point(182, 67)
        Me.txtFullName.Name = "txtFullName"
        Me.txtFullName.ReadOnly = True
        Me.txtFullName.Size = New System.Drawing.Size(142, 26)
        Me.txtFullName.TabIndex = 2
        '
        'txtEmployment
        '
        Me.txtEmployment.Location = New System.Drawing.Point(225, 107)
        Me.txtEmployment.Name = "txtEmployment"
        Me.txtEmployment.ReadOnly = True
        Me.txtEmployment.Size = New System.Drawing.Size(130, 26)
        Me.txtEmployment.TabIndex = 3
        '
        'txtScore
        '
        Me.txtScore.Location = New System.Drawing.Point(182, 147)
        Me.txtScore.Name = "txtScore"
        Me.txtScore.ReadOnly = True
        Me.txtScore.Size = New System.Drawing.Size(100, 26)
        Me.txtScore.TabIndex = 4
        '
        'txtTier
        '
        Me.txtTier.Location = New System.Drawing.Point(182, 187)
        Me.txtTier.Name = "txtTier"
        Me.txtTier.ReadOnly = True
        Me.txtTier.Size = New System.Drawing.Size(100, 26)
        Me.txtTier.TabIndex = 5
        '
        'txtTotalAccounts
        '
        Me.txtTotalAccounts.Location = New System.Drawing.Point(182, 227)
        Me.txtTotalAccounts.Name = "txtTotalAccounts"
        Me.txtTotalAccounts.ReadOnly = True
        Me.txtTotalAccounts.Size = New System.Drawing.Size(100, 26)
        Me.txtTotalAccounts.TabIndex = 6
        '
        'txtTotalBalance
        '
        Me.txtTotalBalance.Location = New System.Drawing.Point(182, 267)
        Me.txtTotalBalance.Name = "txtTotalBalance"
        Me.txtTotalBalance.ReadOnly = True
        Me.txtTotalBalance.Size = New System.Drawing.Size(100, 26)
        Me.txtTotalBalance.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(103, 20)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Customer ID:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 20)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Full Name"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(20, 110)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(148, 20)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Employment Status"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(20, 150)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(113, 20)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Numeric Score"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(20, 190)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(78, 20)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Tier Label"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(20, 230)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(115, 20)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "Total Accounts"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(20, 270)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(106, 20)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Total Balance"
        '
        'btnLoad
        '
        Me.btnLoad.Location = New System.Drawing.Point(256, 24)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(140, 39)
        Me.btnLoad.TabIndex = 16
        Me.btnLoad.Text = "Load Customer"
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'CustomerDetailForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(644, 410)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtTotalBalance)
        Me.Controls.Add(Me.txtTotalAccounts)
        Me.Controls.Add(Me.txtTier)
        Me.Controls.Add(Me.txtScore)
        Me.Controls.Add(Me.txtEmployment)
        Me.Controls.Add(Me.txtFullName)
        Me.Controls.Add(Me.txtCustomerID)
        Me.Name = "CustomerDetailForm"
        Me.Text = "CustomerDetailForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtCustomerID As TextBox
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents txtFullName As TextBox
    Friend WithEvents txtEmployment As TextBox
    Friend WithEvents txtScore As TextBox
    Friend WithEvents txtTier As TextBox
    Friend WithEvents txtTotalAccounts As TextBox
    Friend WithEvents txtTotalBalance As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents btnLoad As Button
End Class
