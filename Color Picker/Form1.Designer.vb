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
        SaturationTrackBar = New TrackBar()
        BrightnessNumericUpDown = New NumericUpDown()
        SaturationNumericUpDown = New NumericUpDown()
        HueTrackBar = New TrackBar()
        HueNumericUpDown = New NumericUpDown()
        HexTextBox = New TextBox()
        CType(BrightnessTrackBar, ComponentModel.ISupportInitialize).BeginInit()
        CType(SaturationTrackBar, ComponentModel.ISupportInitialize).BeginInit()
        CType(BrightnessNumericUpDown, ComponentModel.ISupportInitialize).BeginInit()
        CType(SaturationNumericUpDown, ComponentModel.ISupportInitialize).BeginInit()
        CType(HueTrackBar, ComponentModel.ISupportInitialize).BeginInit()
        CType(HueNumericUpDown, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' BrightnessTrackBar
        ' 
        BrightnessTrackBar.Location = New Point(15, 398)
        BrightnessTrackBar.Maximum = 100
        BrightnessTrackBar.Name = "BrightnessTrackBar"
        BrightnessTrackBar.Size = New Size(352, 69)
        BrightnessTrackBar.TabIndex = 5
        ' 
        ' SaturationTrackBar
        ' 
        SaturationTrackBar.Location = New Point(15, 286)
        SaturationTrackBar.Maximum = 100
        SaturationTrackBar.Name = "SaturationTrackBar"
        SaturationTrackBar.Size = New Size(352, 69)
        SaturationTrackBar.TabIndex = 3
        ' 
        ' BrightnessNumericUpDown
        ' 
        BrightnessNumericUpDown.DecimalPlaces = 2
        BrightnessNumericUpDown.Location = New Point(253, 361)
        BrightnessNumericUpDown.Name = "BrightnessNumericUpDown"
        BrightnessNumericUpDown.Size = New Size(101, 31)
        BrightnessNumericUpDown.TabIndex = 4
        ' 
        ' SaturationNumericUpDown
        ' 
        SaturationNumericUpDown.DecimalPlaces = 2
        SaturationNumericUpDown.Location = New Point(253, 249)
        SaturationNumericUpDown.Name = "SaturationNumericUpDown"
        SaturationNumericUpDown.Size = New Size(101, 31)
        SaturationNumericUpDown.TabIndex = 2
        ' 
        ' HueTrackBar
        ' 
        HueTrackBar.LargeChange = 500
        HueTrackBar.Location = New Point(15, 174)
        HueTrackBar.Maximum = 36000
        HueTrackBar.Name = "HueTrackBar"
        HueTrackBar.Size = New Size(352, 69)
        HueTrackBar.SmallChange = 100
        HueTrackBar.TabIndex = 1
        HueTrackBar.TickFrequency = 5
        ' 
        ' HueNumericUpDown
        ' 
        HueNumericUpDown.DecimalPlaces = 3
        HueNumericUpDown.Location = New Point(253, 137)
        HueNumericUpDown.Maximum = New Decimal(New Integer() {360, 0, 0, 0})
        HueNumericUpDown.Name = "HueNumericUpDown"
        HueNumericUpDown.Size = New Size(101, 31)
        HueNumericUpDown.TabIndex = 0
        ' 
        ' HexTextBox
        ' 
        HexTextBox.Location = New Point(253, 69)
        HexTextBox.Name = "HexTextBox"
        HexTextBox.Size = New Size(101, 31)
        HexTextBox.TabIndex = 6
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 560)
        Controls.Add(HexTextBox)
        Controls.Add(HueNumericUpDown)
        Controls.Add(HueTrackBar)
        Controls.Add(SaturationNumericUpDown)
        Controls.Add(BrightnessNumericUpDown)
        Controls.Add(SaturationTrackBar)
        Controls.Add(BrightnessTrackBar)
        Name = "Form1"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Form1"
        CType(BrightnessTrackBar, ComponentModel.ISupportInitialize).EndInit()
        CType(SaturationTrackBar, ComponentModel.ISupportInitialize).EndInit()
        CType(BrightnessNumericUpDown, ComponentModel.ISupportInitialize).EndInit()
        CType(SaturationNumericUpDown, ComponentModel.ISupportInitialize).EndInit()
        CType(HueTrackBar, ComponentModel.ISupportInitialize).EndInit()
        CType(HueNumericUpDown, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents BrightnessTrackBar As TrackBar
    Friend WithEvents SaturationTrackBar As TrackBar
    Friend WithEvents BrightnessNumericUpDown As NumericUpDown
    Friend WithEvents SaturationNumericUpDown As NumericUpDown
    Friend WithEvents HueTrackBar As TrackBar
    Friend WithEvents HueNumericUpDown As NumericUpDown
    Friend WithEvents HexTextBox As TextBox

End Class
