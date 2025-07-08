<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        BrightnessTrackBar = New TrackBar()
        TrackBar2 = New TrackBar()
        BrightnessNumericUpDown = New NumericUpDown()
        CType(BrightnessTrackBar, ComponentModel.ISupportInitialize).BeginInit()
        CType(TrackBar2, ComponentModel.ISupportInitialize).BeginInit()
        CType(BrightnessNumericUpDown, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' BrightnessTrackBar
        ' 
        BrightnessTrackBar.Location = New Point(12, 172)
        BrightnessTrackBar.Maximum = 100
        BrightnessTrackBar.Name = "BrightnessTrackBar"
        BrightnessTrackBar.Size = New Size(335, 69)
        BrightnessTrackBar.TabIndex = 0
        ' 
        ' TrackBar2
        ' 
        TrackBar2.Location = New Point(12, 257)
        TrackBar2.Maximum = 100
        TrackBar2.Name = "TrackBar2"
        TrackBar2.Size = New Size(335, 69)
        TrackBar2.TabIndex = 1
        ' 
        ' BrightnessNumericUpDown
        ' 
        BrightnessNumericUpDown.Location = New Point(262, 135)
        BrightnessNumericUpDown.Name = "BrightnessNumericUpDown"
        BrightnessNumericUpDown.Size = New Size(75, 31)
        BrightnessNumericUpDown.TabIndex = 2
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(BrightnessNumericUpDown)
        Controls.Add(TrackBar2)
        Controls.Add(BrightnessTrackBar)
        Name = "Form1"
        Text = "Form1"
        CType(BrightnessTrackBar, ComponentModel.ISupportInitialize).EndInit()
        CType(TrackBar2, ComponentModel.ISupportInitialize).EndInit()
        CType(BrightnessNumericUpDown, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents BrightnessTrackBar As TrackBar
    Friend WithEvents TrackBar2 As TrackBar
    Friend WithEvents BrightnessNumericUpDown As NumericUpDown

End Class
