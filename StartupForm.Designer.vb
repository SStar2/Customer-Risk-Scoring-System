<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StartupForm
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
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblSubtitle = New System.Windows.Forms.Label()
        Me.btnPortfolio = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnDetail = New System.Windows.Forms.Button()
        Me.btnTabs = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(0, 20)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(228, 20)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Customer Risk Scoring System"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSubtitle
        '
        Me.lblSubtitle.AutoSize = True
        Me.lblSubtitle.Location = New System.Drawing.Point(0, 65)
        Me.lblSubtitle.Name = "lblSubtitle"
        Me.lblSubtitle.Size = New System.Drawing.Size(268, 20)
        Me.lblSubtitle.TabIndex = 1
        Me.lblSubtitle.Text = "Select an option below to get started"
        Me.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnPortfolio
        '
        Me.btnPortfolio.Location = New System.Drawing.Point(160, 120)
        Me.btnPortfolio.Name = "btnPortfolio"
        Me.btnPortfolio.Size = New System.Drawing.Size(220, 40)
        Me.btnPortfolio.TabIndex = 2
        Me.btnPortfolio.Text = "View Risk Portfolio"
        Me.btnPortfolio.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(160, 175)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(220, 40)
        Me.btnSearch.TabIndex = 3
        Me.btnSearch.Text = "Search by Risk Tier"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnDetail
        '
        Me.btnDetail.Location = New System.Drawing.Point(160, 230)
        Me.btnDetail.Name = "btnDetail"
        Me.btnDetail.Size = New System.Drawing.Size(220, 40)
        Me.btnDetail.TabIndex = 4
        Me.btnDetail.Text = "Customer Detail"
        Me.btnDetail.UseVisualStyleBackColor = True
        '
        'btnTabs
        '
        Me.btnTabs.Location = New System.Drawing.Point(160, 285)
        Me.btnTabs.Name = "btnTabs"
        Me.btnTabs.Size = New System.Drawing.Size(220, 40)
        Me.btnTabs.TabIndex = 5
        Me.btnTabs.Text = "Customer Profile"
        Me.btnTabs.UseVisualStyleBackColor = True
        '
        'StartupForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(518, 364)
        Me.Controls.Add(Me.btnTabs)
        Me.Controls.Add(Me.btnDetail)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.btnPortfolio)
        Me.Controls.Add(Me.lblSubtitle)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "StartupForm"
        Me.Text = "StartupForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents lblSubtitle As Label
    Friend WithEvents btnPortfolio As Button
    Friend WithEvents btnSearch As Button
    Friend WithEvents btnDetail As Button
    Friend WithEvents btnTabs As Button
End Class
