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
        HueNumericUpDown = New NumericUpDown()
        HexTextBox = New TextBox()
        HueTrackBar = New TrackBar()
        CType(BrightnessTrackBar, ComponentModel.ISupportInitialize).BeginInit()
        CType(SaturationTrackBar, ComponentModel.ISupportInitialize).BeginInit()
        CType(BrightnessNumericUpDown, ComponentModel.ISupportInitialize).BeginInit()
        CType(SaturationNumericUpDown, ComponentModel.ISupportInitialize).BeginInit()
        CType(HueNumericUpDown, ComponentModel.ISupportInitialize).BeginInit()
        CType(HueTrackBar, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' BrightnessTrackBar
        ' 
        BrightnessTrackBar.LargeChange = 1000
        BrightnessTrackBar.Location = New Point(323, 382)
        BrightnessTrackBar.Maximum = 10000
        BrightnessTrackBar.Name = "BrightnessTrackBar"
        BrightnessTrackBar.Size = New Size(312, 45)
        BrightnessTrackBar.SmallChange = 100
        BrightnessTrackBar.TabIndex = 5
        BrightnessTrackBar.TickFrequency = 9
        ' 
        ' SaturationTrackBar
        ' 
        SaturationTrackBar.Location = New Point(323, 265)
        SaturationTrackBar.Maximum = 100
        SaturationTrackBar.Name = "SaturationTrackBar"
        SaturationTrackBar.Size = New Size(312, 45)
        SaturationTrackBar.TabIndex = 3
        ' 
        ' BrightnessNumericUpDown
        ' 
        BrightnessNumericUpDown.DecimalPlaces = 2
        BrightnessNumericUpDown.Location = New Point(532, 341)
        BrightnessNumericUpDown.Name = "BrightnessNumericUpDown"
        BrightnessNumericUpDown.Size = New Size(101, 23)
        BrightnessNumericUpDown.TabIndex = 4
        ' 
        ' SaturationNumericUpDown
        ' 
        SaturationNumericUpDown.DecimalPlaces = 2
        SaturationNumericUpDown.Location = New Point(532, 225)
        SaturationNumericUpDown.Name = "SaturationNumericUpDown"
        SaturationNumericUpDown.Size = New Size(101, 23)
        SaturationNumericUpDown.TabIndex = 2
        ' 
        ' HueNumericUpDown
        ' 
        HueNumericUpDown.DecimalPlaces = 3
        HueNumericUpDown.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        HueNumericUpDown.Location = New Point(532, 111)
        HueNumericUpDown.Maximum = New Decimal(New Integer() {360, 0, 0, 0})
        HueNumericUpDown.Name = "HueNumericUpDown"
        HueNumericUpDown.Size = New Size(101, 23)
        HueNumericUpDown.TabIndex = 0
        ' 
        ' HexTextBox
        ' 
        HexTextBox.Location = New Point(532, 58)
        HexTextBox.Name = "HexTextBox"
        HexTextBox.Size = New Size(101, 23)
        HexTextBox.TabIndex = 6
        ' 
        ' HueTrackBar
        ' 
        HueTrackBar.LargeChange = 3000
        HueTrackBar.Location = New Point(323, 151)
        HueTrackBar.Maximum = 36000
        HueTrackBar.Name = "HueTrackBar"
        HueTrackBar.Size = New Size(312, 45)
        HueTrackBar.SmallChange = 1000
        HueTrackBar.TabIndex = 7
        HueTrackBar.TickFrequency = 13
        ' 
        ' Form1
        ' 
        AutoScaleMode = AutoScaleMode.None
        ClientSize = New Size(649, 477)
        Controls.Add(HueTrackBar)
        Controls.Add(HexTextBox)
        Controls.Add(HueNumericUpDown)
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
        CType(HueNumericUpDown, ComponentModel.ISupportInitialize).EndInit()
        CType(HueTrackBar, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents BrightnessTrackBar As TrackBar
    Friend WithEvents SaturationTrackBar As TrackBar
    Friend WithEvents BrightnessNumericUpDown As NumericUpDown
    Friend WithEvents SaturationNumericUpDown As NumericUpDown
    Friend WithEvents HueNumericUpDown As NumericUpDown
    Friend WithEvents HexTextBox As TextBox
    Friend WithEvents HueTrackBar As TrackBar

End Class
