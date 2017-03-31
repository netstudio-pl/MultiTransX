<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class formTranslate
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(formTranslate))
        Me.txtTlumaczenie = New System.Windows.Forms.TextBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.pbGrafika = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblKIerunekTlumaczenia = New System.Windows.Forms.Label()
        CType(Me.pbGrafika, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtTlumaczenie
        '
        Me.txtTlumaczenie.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.txtTlumaczenie.Location = New System.Drawing.Point(12, 12)
        Me.txtTlumaczenie.Multiline = True
        Me.txtTlumaczenie.Name = "txtTlumaczenie"
        Me.txtTlumaczenie.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtTlumaczenie.Size = New System.Drawing.Size(360, 253)
        Me.txtTlumaczenie.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.btnClose.Location = New System.Drawing.Point(58, 478)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(100, 40)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Zamknij"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'pbGrafika
        '
        Me.pbGrafika.BackColor = System.Drawing.Color.White
        Me.pbGrafika.Location = New System.Drawing.Point(12, 271)
        Me.pbGrafika.Name = "pbGrafika"
        Me.pbGrafika.Size = New System.Drawing.Size(360, 173)
        Me.pbGrafika.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbGrafika.TabIndex = 2
        Me.pbGrafika.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Label1.Location = New System.Drawing.Point(225, 478)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(147, 19)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Kierunek tłumaczenia"
        '
        'lblKIerunekTlumaczenia
        '
        Me.lblKIerunekTlumaczenia.AutoSize = True
        Me.lblKIerunekTlumaczenia.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblKIerunekTlumaczenia.Location = New System.Drawing.Point(225, 499)
        Me.lblKIerunekTlumaczenia.Name = "lblKIerunekTlumaczenia"
        Me.lblKIerunekTlumaczenia.Size = New System.Drawing.Size(0, 19)
        Me.lblKIerunekTlumaczenia.TabIndex = 4
        '
        'formTranslate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 544)
        Me.Controls.Add(Me.lblKIerunekTlumaczenia)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pbGrafika)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.txtTlumaczenie)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "formTranslate"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MultiTransX - Wynik tłumaczenia"
        Me.TopMost = True
        CType(Me.pbGrafika, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtTlumaczenie As TextBox
    Friend WithEvents btnClose As Button
    Friend WithEvents pbGrafika As PictureBox
    Friend WithEvents Label1 As Label
    Friend WithEvents lblKIerunekTlumaczenia As Label
End Class
