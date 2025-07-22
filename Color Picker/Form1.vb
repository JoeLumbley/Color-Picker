' Color Picker

' MIT License
' Copyright(c) 2025 Joseph W. Lumbley

' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:

' The above copyright notice and this permission notice shall be included in all
' copies or substantial portions of the Software.

' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO AND THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE A NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
' SOFTWARE.

Public Class Form1

    Private Structure HueWheelStruct

        Public Location As Point
        Public Size As Size
        Public Radius As Integer
        Public Center As Point
        Public Bitmap As Bitmap
        Public Graphics As Graphics
        Public BackColor As Color
        Public Color As Color
        Public Padding As Integer
        Public SelectedHueAngle As Double

        Public Sub Draw(size As Integer, padding As Integer, backcolor As Color)

            If Bitmap Is Nothing OrElse Bitmap.Width <> size OrElse Bitmap.Height <> size Then
                Bitmap?.Dispose() ' Dispose of the old bitmap if it exists
                Bitmap = New Bitmap(size + padding * 2, size + padding * 2)
                Graphics = Graphics.FromImage(Bitmap)

            End If

            Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            Radius = size \ 2
            Me.Padding = padding
            Me.Size = New Size(size, size)
            Me.BackColor = backcolor

            Dim centerX As Integer = Radius + padding
            Dim centerY As Integer = Radius + padding

            For y As Integer = 0 To Bitmap.Height - 1
                For x As Integer = 0 To Bitmap.Width - 1

                    ' Calculate distance from center
                    Dim dx As Integer = x - centerX
                    Dim dy As Integer = y - centerY
                    Dim dist As Double = Math.Sqrt(dx * dx + dy * dy)

                    ' If the pixel is within the radius of the circle
                    If dist <= Radius Then

                        ' Calculate angle in degrees
                        Dim angle As Double = (Math.Atan2(dy, dx) * 180.0 / Math.PI + 360) Mod 360

                        ' Set pixel color based on angle
                        Bitmap.SetPixel(x, y, ColorFromHSV(angle, 1, 1))
                        ' Note: Saturation and Value are set to 1 for full color saturation and brightness

                    Else
                        ' Outside the circle, set pixel to transparent
                        Bitmap.SetPixel(x, y, Color.Transparent)

                    End If
                Next
            Next

            ' Draw border
            Dim borderRect As New Rectangle(padding, padding, size, size)

            Using pen As New Pen(Color.Black, 3)
                Graphics.DrawEllipse(pen, borderRect)
            End Using

        End Sub

        Public Sub GetColorAtPoint(point As Point)

            If Bitmap Is Nothing Then Return

            Dim centerX As Integer = Radius + Padding
            Dim centerY As Integer = Radius + Padding
            Dim dx As Integer = point.X - Location.X - centerX
            Dim dy As Integer = point.Y - Location.Y - centerY
            Dim dist As Double = Math.Sqrt(dx * dx + dy * dy)

            If dist <= Radius Then
                Dim angle As Double = (Math.Atan2(dy, dx) * 180.0 / Math.PI + 360) Mod 360

                Color = ColorFromHSV(CInt(angle), RGBtoHSV(Color).Saturation, RGBtoHSV(Color).Value)
                SelectedHueAngle = CInt(angle) ' Save angle for rendering pointer

            End If

        End Sub

        ' Function to convert HSV to RGB
        Public Function ColorFromHSV(hue As Double, saturation As Double, brightness As Double) As Color

            Dim r As Double = 0, g As Double = 0, b As Double = 0

            If saturation = 0 Then

                ' If saturation is 0, the color is a shade of gray
                r = brightness
                g = brightness
                b = brightness

            Else

                Dim sector As Integer = CInt(Math.Floor(hue / 60)) Mod 6
                Dim fractional As Double = (hue / 60) - Math.Floor(hue / 60)

                Dim p As Double = brightness * (1 - saturation)
                Dim q As Double = brightness * (1 - saturation * fractional)
                Dim t As Double = brightness * (1 - saturation * (1 - fractional))

                Select Case sector

                    Case 0
                        r = brightness
                        g = t
                        b = p
                    Case 1
                        r = q
                        g = brightness
                        b = p
                    Case 2
                        r = p
                        g = brightness
                        b = t
                    Case 3
                        r = p
                        g = q
                        b = brightness
                    Case 4
                        r = t
                        g = p
                        b = brightness
                    Case 5
                        r = brightness
                        g = p
                        b = q

                End Select

            End If

            Return Color.FromArgb(CInt(r * 255), CInt(g * 255), CInt(b * 255))

        End Function

        Public Function RGBtoHSV(color As Color) As (Hue As Double, Saturation As Double, Value As Double)

            Dim r As Double = color.R / 255.0
            Dim g As Double = color.G / 255.0
            Dim b As Double = color.B / 255.0

            Dim max As Double = Math.Max(r, Math.Max(g, b))
            Dim min As Double = Math.Min(r, Math.Min(g, b))
            Dim delta As Double = max - min

            Dim h As Double

            If delta = 0 Then
                h = 0
            ElseIf max = r Then
                h = 60 * (((g - b) / delta) Mod 6)
            ElseIf max = g Then
                h = 60 * (((b - r) / delta) + 2)
            Else
                h = 60 * (((r - g) / delta) + 4)
            End If

            If h < 0 Then h += 360

            Dim s As Double = If(max = 0, 0, delta / max)
            Dim v As Double = max

            Return (h, s, v)

        End Function

    End Structure

    Private Structure SaturationWheelStruct

        Public Location As Point
        Public Size As Size
        Public Radius As Integer
        Public Center As Point
        Public Bitmap As Bitmap
        Public Graphics As Graphics
        Public BackColor As Color
        Public Color As Color
        Public Padding As Integer
        Public SelectedHueAngle As Double
        Public Saturation As Double

        Public Sub Draw(size As Integer, padding As Integer, hueAngle As Double, backcolor As Color)

            ' Ensure the bitmap is created or resized if necessary
            If Bitmap Is Nothing OrElse Bitmap.Width <> size OrElse Bitmap.Height <> size Then
                Bitmap?.Dispose()
                Bitmap = New Bitmap(size + padding * 2, size + padding * 2)
                Graphics = Graphics.FromImage(Bitmap)
            End If

            Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            Radius = size \ 2
            Me.Size = New Size(size, size)
            Me.Padding = padding
            Me.BackColor = backcolor

            Dim centerX As Integer = Radius + padding
            Dim centerY As Integer = Radius + padding

            For y As Integer = 0 To Bitmap.Height - 1
                For x As Integer = 0 To Bitmap.Width - 1

                    ' Calculate distance from center
                    Dim dx As Integer = x - centerX
                    Dim dy As Integer = y - centerY
                    Dim dist As Double = Math.Sqrt(dx * dx + dy * dy)

                    If dist <= Radius Then

                        Dim angle As Double = (Math.Atan2(dy, dx) * 180.0 / Math.PI + 360) Mod 360
                        Dim saturation As Double = Math.Min(angle / 360.0, 1.0)
                        Dim color As Color = ColorFromHSV(hueAngle, saturation, 1.0)

                        ' Set pixel color based on angle and saturation
                        Bitmap.SetPixel(x, y, color)

                    Else

                        ' Outside the circle, set pixel to transparent
                        Bitmap.SetPixel(x, y, Color.Transparent)

                    End If
                Next
            Next

            Dim borderRect As New Rectangle(padding, padding, size, size)
            Using pen As New Pen(Color.Black, 3)
                Graphics.DrawEllipse(pen, borderRect)
            End Using

        End Sub

        Public Sub GetSaturationFromAnglePoint(point As Point)

            Dim centerX As Integer = Radius + Padding
            Dim centerY As Integer = Radius + Padding
            Dim dx As Integer = point.X - Location.X - centerX
            Dim dy As Integer = point.Y - Location.Y - centerY
            Dim dist As Double = Math.Sqrt(dx * dx + dy * dy)

            If dist <= Radius Then

                Dim angle As Double = (Math.Atan2(dy, dx) * 180.0 / Math.PI + 360) Mod 360

                Saturation = Math.Min(angle / 360.0, 1.0)

            End If

        End Sub

        Public Function ColorFromHSV(hue As Double, saturation As Double, brightness As Double) As Color

            Dim r As Double = 0, g As Double = 0, b As Double = 0

            If saturation = 0 Then
                r = brightness
                g = brightness
                b = brightness
            Else
                Dim sector As Integer = CInt(Math.Floor(hue / 60)) Mod 6
                Dim fractional As Double = (hue / 60) - Math.Floor(hue / 60)

                Dim p As Double = brightness * (1 - saturation)
                Dim q As Double = brightness * (1 - saturation * fractional)
                Dim t As Double = brightness * (1 - saturation * (1 - fractional))

                Select Case sector
                    Case 0
                        r = brightness
                        g = t
                        b = p
                    Case 1
                        r = q
                        g = brightness
                        b = p
                    Case 2
                        r = p
                        g = brightness
                        b = t
                    Case 3
                        r = p
                        g = q
                        b = brightness
                    Case 4
                        r = t
                        g = p
                        b = brightness
                    Case 5
                        r = brightness
                        g = p
                        b = q
                End Select
            End If

            Return Color.FromArgb(CInt(r * 255), CInt(g * 255), CInt(b * 255))

        End Function

    End Structure

    Private Structure WedgesWheelStruct

        Public Location As Point
        Public Size As Size
        Public Radius As Integer
        Public Center As Point
        Public Bitmap As Bitmap
        Public Graphics As Graphics
        Public BackColor As Color
        Public Color As Color
        Public Padding As Integer
        Public SelectedHueAngle As Double
        Public Saturation As Double

        Public Sub DrawWedges(size As Integer, padding As Integer, backcolor As Color)

            If Bitmap Is Nothing OrElse Bitmap.Width <> size OrElse Bitmap.Height <> size Then
                Bitmap?.Dispose()
                Bitmap = New Bitmap(size + padding * 2, size + padding * 2)
                Graphics = Graphics.FromImage(Bitmap)
            End If

            Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            Radius = size \ 2
            Me.Padding = padding
            Me.Size = New Size(size, size)
            Me.BackColor = backcolor

            Dim centerX As Integer = Radius + padding
            Dim centerY As Integer = Radius + padding
            Dim center As New Point(centerX, centerY)

            '12 Wedges at 30° each
            Dim wedgeColors As Color() = {
                ColorFromHSV(0, 1, 1),       ' Red
                ColorFromHSV(30, 1, 1),      ' Orange
                ColorFromHSV(60, 1, 1),      ' Yellow
                ColorFromHSV(90, 1, 1),      ' Chartreuse Green
                ColorFromHSV(120, 1, 1),     ' Green
                ColorFromHSV(150, 1, 1),     ' Spring Green
                ColorFromHSV(180, 1, 1),     ' Cyan
                ColorFromHSV(210, 1, 1),     ' Azure
                ColorFromHSV(240, 1, 1),     ' Blue
                ColorFromHSV(270, 1, 1),     ' Violet
                ColorFromHSV(300, 1, 1),     ' Magenta
                ColorFromHSV(330, 1, 1)      ' Rose
            }

            Dim rotationOffset As Single = -15 ' Counterclockwise rotation
            For i As Integer = 0 To 11
                Dim startAngle As Single = i * 30 + rotationOffset
                Dim path As New Drawing2D.GraphicsPath()
                path.AddPie(New Rectangle(padding, padding, size, size), startAngle, 30)

                Using brush As New SolidBrush(wedgeColors(i))
                    Graphics.FillPath(brush, path)
                End Using
            Next

            ' Border
            Dim borderRect As New Rectangle(padding, padding, size, size)
            Using pen As New Pen(Color.Black, 3)
                Graphics.DrawEllipse(pen, borderRect)
            End Using

        End Sub

        Public Sub GetColorAtPoint(point As Point)

            If Bitmap Is Nothing Then Return

            Dim centerX As Integer = Radius + Padding
            Dim centerY As Integer = Radius + Padding
            Dim dx As Integer = point.X - Location.X - centerX
            Dim dy As Integer = point.Y - Location.Y - centerY
            Dim dist As Double = Math.Sqrt(dx * dx + dy * dy)

            If dist <= Radius Then
                Dim rawAngle As Double = (Math.Atan2(dy, dx) * 180.0 / Math.PI + 360) Mod 360
                SelectedHueAngle = rawAngle

                ' Apply inverse rotation offset
                Dim adjustedAngle As Double = (rawAngle - (-15) + 360) Mod 360

                ' Determine which of the 12 wedges the angle belongs to
                Dim wedgeIndex As Integer = CInt(Math.Floor(adjustedAngle / 30)) Mod 12

                'Optional Store Color directly from wedge
                Dim wedgeColors As Color() = {
                    ColorFromHSV(0, 1, 1), ColorFromHSV(30, 1, 1), ColorFromHSV(60, 1, 1),
                    ColorFromHSV(90, 1, 1), ColorFromHSV(120, 1, 1), ColorFromHSV(150, 1, 1),
                    ColorFromHSV(180, 1, 1), ColorFromHSV(210, 1, 1), ColorFromHSV(240, 1, 1),
                    ColorFromHSV(270, 1, 1), ColorFromHSV(300, 1, 1), ColorFromHSV(330, 1, 1)
                }

                Color = wedgeColors(wedgeIndex)

            End If

        End Sub

        ' Function to convert HSV to RGB
        Public Function ColorFromHSV(hue As Double, saturation As Double, brightness As Double) As Color

            Dim r As Double = 0, g As Double = 0, b As Double = 0

            If saturation = 0 Then
                r = brightness
                g = brightness
                b = brightness
            Else
                Dim sector As Integer = CInt(Math.Floor(hue / 60)) Mod 6
                Dim fractional As Double = (hue / 60) - Math.Floor(hue / 60)

                Dim p As Double = brightness * (1 - saturation)
                Dim q As Double = brightness * (1 - saturation * fractional)
                Dim t As Double = brightness * (1 - saturation * (1 - fractional))

                Select Case sector
                    Case 0
                        r = brightness
                        g = t
                        b = p
                    Case 1
                        r = q
                        g = brightness
                        b = p
                    Case 2
                        r = p
                        g = brightness
                        b = t
                    Case 3
                        r = p
                        g = q
                        b = brightness
                    Case 4
                        r = t
                        g = p
                        b = brightness
                    Case 5
                        r = brightness
                        g = p
                        b = q
                End Select
            End If

            Return Color.FromArgb(CInt(r * 255), CInt(g * 255), CInt(b * 255))

        End Function

    End Structure

    Private Structure ValueWheelStruct

        Public Location As Point
        Public Size As Size
        Public Radius As Integer
        Public Center As Point
        Public Bitmap As Bitmap
        Public Graphics As Graphics
        Public BackColor As Color
        Public Color As Color
        Public Padding As Integer
        Public SelectedHueAngle As Double
        Public Value As Double

        Public Sub Draw(size As Integer, padding As Integer, hueAngle As Double, backcolor As Color)
            If Bitmap Is Nothing OrElse Bitmap.Width <> size OrElse Bitmap.Height <> size Then
                Bitmap?.Dispose()
                Bitmap = New Bitmap(size + padding * 2, size + padding * 2)
                Graphics = Graphics.FromImage(Bitmap)
            End If

            Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            Radius = size \ 2
            Me.Size = New Size(size, size)
            Me.Padding = padding
            Me.BackColor = backcolor

            Dim centerX As Integer = Radius + padding
            Dim centerY As Integer = Radius + padding

            For y As Integer = 0 To Bitmap.Height - 1
                For x As Integer = 0 To Bitmap.Width - 1
                    Dim dx As Integer = x - centerX
                    Dim dy As Integer = y - centerY
                    Dim dist As Double = Math.Sqrt(dx * dx + dy * dy)

                    If dist <= Radius Then
                        Dim angle As Double = (Math.Atan2(dy, dx) * 180.0 / Math.PI + 360) Mod 360
                        Dim value As Double = Math.Min(angle / 360.0, 1.0)
                        Dim color As Color = ColorFromHSV(hueAngle, 1.0, value)
                        Bitmap.SetPixel(x, y, color)
                    Else
                        Bitmap.SetPixel(x, y, Color.Transparent)
                    End If
                Next
            Next

            Dim borderRect As New Rectangle(padding, padding, size, size)
            Using pen As New Pen(Color.Black, 3)
                Graphics.DrawEllipse(pen, borderRect)
            End Using
        End Sub

        Public Sub GetValueFromAnglePoint(point As Point)
            Dim centerX As Integer = Radius + Padding
            Dim centerY As Integer = Radius + Padding
            Dim dx As Integer = point.X - Location.X - centerX
            Dim dy As Integer = point.Y - Location.Y - centerY
            Dim dist As Double = Math.Sqrt(dx * dx + dy * dy)

            If dist <= Radius Then
                Dim angle As Double = (Math.Atan2(dy, dx) * 180.0 / Math.PI + 360) Mod 360
                Value = Math.Min(angle / 360.0, 1.0)
            End If
        End Sub

        'Public Function ColorFromHSV(hue As Double, saturation As Double, brightness As Double) As Color
        '    ' Reuse your existing HSV logic here
        'End Function

        Public Function ColorFromHSV(hue As Double, saturation As Double, brightness As Double) As Color

            Dim r As Double = 0, g As Double = 0, b As Double = 0

            If saturation = 0 Then
                r = brightness
                g = brightness
                b = brightness
            Else
                Dim sector As Integer = CInt(Math.Floor(hue / 60)) Mod 6
                Dim fractional As Double = (hue / 60) - Math.Floor(hue / 60)

                Dim p As Double = brightness * (1 - saturation)
                Dim q As Double = brightness * (1 - saturation * fractional)
                Dim t As Double = brightness * (1 - saturation * (1 - fractional))

                Select Case sector
                    Case 0
                        r = brightness
                        g = t
                        b = p
                    Case 1
                        r = q
                        g = brightness
                        b = p
                    Case 2
                        r = p
                        g = brightness
                        b = t
                    Case 3
                        r = p
                        g = q
                        b = brightness
                    Case 4
                        r = t
                        g = p
                        b = brightness
                    Case 5
                        r = brightness
                        g = p
                        b = q
                End Select
            End If

            Return Color.FromArgb(CInt(r * 255), CInt(g * 255), CInt(b * 255))

        End Function

    End Structure

    Private HueWheel As HueWheelStruct

    Private SatWheel As SaturationWheelStruct

    Private ValWheel As ValueWheelStruct

    Private WedgesWheel As WedgesWheelStruct

    Private UpDatingColor As Boolean = False

    Private TheHue As Double = 280

    Private TheSat As Double = 1 ' 100%

    Private TheVal As Double = 1 ' 100%

    Private TheColor As Color = ColorFromHSV(TheHue, TheSat, TheVal)

    Private SelectedColorEllipseSize As Integer = 75

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        InitializeApplication()

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        DrawWedgesWheel(e)

        DrawHueWheel(e)

        DrawSatWheel(e)

        DrawValWheel(e)

        DrawHuePointer(e)

        DrawSaturationPointer(e)

        DrawValuePointer(e)

        DrawSelectedColor(e)

        DrawLables(e)

    End Sub

    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown

        ' Is the mouse over the selected color ellipse?
        If IsPointInsideCircle(
                e.X, e.Y, HueWheel.Location.X + HueWheel.Radius + HueWheel.Padding,
                HueWheel.Location.Y + HueWheel.Radius + HueWheel.Padding,
                SelectedColorEllipseSize \ 2) Then

            Return

        End If

        ' Is the mouse over the Value wheel?
        If IsPointInsideCircle(
        e.X, e.Y, ValWheel.Location.X + ValWheel.Radius + ValWheel.Padding,
        ValWheel.Location.Y + ValWheel.Radius + ValWheel.Padding, ValWheel.Radius) Then
            ' Yes, it's over the Value wheel.

            If e.Button = MouseButtons.Left Then

                ClearFocus()

                ValWheel.GetValueFromAnglePoint(New Point(e.X, e.Y))

                TheVal = Math.Round(ValWheel.Value, 2)


                UpdateUIValChange()

            End If

            Return

        End If

        ' Is the mouse over the Sat wheel?
        If IsPointInsideCircle(
                e.X, e.Y, SatWheel.Location.X + SatWheel.Radius + SatWheel.Padding,
                SatWheel.Location.Y + SatWheel.Radius + SatWheel.Padding, SatWheel.Radius) Then
            ' Yes, the mouse is over the Sat wheel.

            If e.Button = MouseButtons.Left Then

                ClearFocus()

                SatWheel.GetSaturationFromAnglePoint(New Point(e.X, e.Y))

                TheSat = Math.Round(SatWheel.Saturation, 2)

                UpdateUISatChange()

            End If

            Return

        End If

        ' Is the mouse over the Hue wheel?
        If IsPointInsideCircle(
                e.X, e.Y, HueWheel.Location.X + HueWheel.Radius + HueWheel.Padding,
                HueWheel.Location.Y + HueWheel.Radius + HueWheel.Padding, HueWheel.Radius) Then
            ' Yes, the mouse is over the Hue wheel.

            If e.Button = MouseButtons.Left Then

                ClearFocus()

                HueWheel.GetColorAtPoint(New Point(e.X, e.Y))

                TheHue = HueWheel.SelectedHueAngle

                UpdateUIHueChange()

            End If

            Return

        End If

        ' Is the mouse over the Wedges wheel?
        If IsPointInsideCircle(
                e.X, e.Y, WedgesWheel.Location.X + WedgesWheel.Radius + WedgesWheel.Padding,
                WedgesWheel.Location.Y + WedgesWheel.Radius + WedgesWheel.Padding, WedgesWheel.Radius) Then
            ' Yes, the mouse is over the Wedges wheel.

            If e.Button = MouseButtons.Left Then

                ClearFocus()

                WedgesWheel.GetColorAtPoint(New Point(e.X, e.Y))

                TheColor = WedgesWheel.Color

                ' Convert RGB to HSV
                Dim hsv = RGBtoHSV(TheColor)
                TheHue = CInt(hsv.Hue)
                TheSat = hsv.Saturation
                TheVal = hsv.Value

                UpDatingColor = True

                ' Update the HueWheel with the new color and hue
                HueWheel.Color = TheColor
                HueWheel.SelectedHueAngle = TheHue

                ' Update the trackbars and numeric up-downs
                HueTrackBar.Value = TheHue * 100
                HueNumericUpDown.Value = TheHue
                SaturationTrackBar.Value = TheSat * 10000
                SaturationNumericUpDown.Value = TheSat * 100
                SatWheel.Saturation = TheSat


                BrightnessTrackBar.Value = TheVal * 10000
                BrightnessNumericUpDown.Value = TheVal * 100
                ValWheel.Value = TheVal

                HexTextBox.Text = HsvToHex(TheHue, TheSat, TheVal)

                Invalidate()

                UpDatingColor = False

            End If

        End If

    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove

        ' Is the mouse over the selected color ellipse?
        If IsPointInsideCircle(
                e.X, e.Y, HueWheel.Location.X + HueWheel.Radius + HueWheel.Padding,
                HueWheel.Location.Y + HueWheel.Radius + HueWheel.Padding,
                SelectedColorEllipseSize \ 2) Then

            Return

        End If

        ' Is the mouse over the Value wheel?
        If IsPointInsideCircle(
        e.X, e.Y, ValWheel.Location.X + ValWheel.Radius + ValWheel.Padding,
        ValWheel.Location.Y + ValWheel.Radius + ValWheel.Padding, ValWheel.Radius) Then
            ' Yes, it's over the Value wheel.

            ' Check if the left mouse button is pressed
            If e.Button = MouseButtons.Left Then
                ' The mouse is over the Value wheel and the left button is pressed.

                ClearFocus()

                ValWheel.GetValueFromAnglePoint(New Point(e.X, e.Y))

                ' Round the value to 2 decimal places
                TheVal = Math.Round(ValWheel.Value, 2)

                UpdateUIValChange()

            End If

            Return

        End If

        ' Is the mouse over the Sat wheel?
        If IsPointInsideCircle(
                e.X, e.Y, SatWheel.Location.X + SatWheel.Radius + SatWheel.Padding,
                SatWheel.Location.Y + SatWheel.Radius + SatWheel.Padding, SatWheel.Radius) Then
            ' Yes, the mouse is over the Sat wheel.

            If e.Button = MouseButtons.Left Then

                ClearFocus()

                SatWheel.GetSaturationFromAnglePoint(New Point(e.X, e.Y))

                TheSat = SatWheel.Saturation

                UpdateUISatChange()

            End If

            Return

        End If

        ' Is the mouse over the Hue wheel?
        If IsPointInsideCircle(
                e.X, e.Y, HueWheel.Location.X + HueWheel.Radius + HueWheel.Padding,
                HueWheel.Location.Y + HueWheel.Radius + HueWheel.Padding, HueWheel.Radius) Then
            ' Yes, the mouse is over the Hue wheel.

            If e.Button = MouseButtons.Left Then

                ClearFocus()

                HueWheel.GetColorAtPoint(New Point(e.X, e.Y))

                TheHue = HueWheel.SelectedHueAngle

                UpdateUIHueChange()

            End If

            Return

        End If

        ' Is the mouse over the Wedges wheel?
        If IsPointInsideCircle(
                e.X, e.Y, WedgesWheel.Location.X + WedgesWheel.Radius + WedgesWheel.Padding,
                WedgesWheel.Location.Y + WedgesWheel.Radius + WedgesWheel.Padding, WedgesWheel.Radius) Then
            ' Yes, the mouse is over the Wedges wheel.

            If e.Button = MouseButtons.Left Then

                ClearFocus()

                WedgesWheel.GetColorAtPoint(New Point(e.X, e.Y))

                TheColor = WedgesWheel.Color

                ' Convert RGB to HSV
                Dim hsv = RGBtoHSV(TheColor)
                TheHue = CInt(hsv.Hue)
                TheSat = hsv.Saturation
                TheVal = hsv.Value

                UpDatingColor = True

                ' Update the HueWheel with the new color and hue
                HueWheel.Color = TheColor
                HueWheel.SelectedHueAngle = TheHue

                ' Update the trackbars and numeric up-downs
                HueTrackBar.Value = TheHue * 100
                HueNumericUpDown.Value = TheHue
                SaturationTrackBar.Value = TheSat * 10000
                SaturationNumericUpDown.Value = TheSat * 100
                SatWheel.Saturation = TheSat
                BrightnessTrackBar.Value = TheVal * 10000
                BrightnessNumericUpDown.Value = TheVal * 100
                ValWheel.Value = TheVal
                HexTextBox.Text = HsvToHex(TheHue, TheSat, TheVal)

                Invalidate()

                UpDatingColor = False

            End If

        End If

    End Sub

    Private Sub Form1_MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel

        ' Is the mouse over the selected color ellipse?
        If IsPointInsideCircle(
                e.X, e.Y, HueWheel.Location.X + HueWheel.Radius + HueWheel.Padding,
                HueWheel.Location.Y + HueWheel.Radius + HueWheel.Padding,
                SelectedColorEllipseSize \ 2) Then

            Return

        End If

        ' Is the mouse over the Value wheel?
        If IsPointInsideCircle(
                e.X, e.Y, ValWheel.Location.X + ValWheel.Radius + ValWheel.Padding,
                ValWheel.Location.Y + ValWheel.Radius + ValWheel.Padding, ValWheel.Radius) Then
            ' Yes, the mouse is over the Value wheel.

            ' Check if the mouse wheel is scrolled
            If e.Delta <> 0 Then
                ' mouse wheel scrolled, adjust the value

                ClearFocus()

                ' Adjust the value based on the mouse wheel scroll direction
                TheVal += If(e.Delta > 0, 0.01, -0.01) ' Increase or decrease value by 1 percent

                ' Ensure the value wraps around within 0-1 
                If TheVal < 0 Then TheVal = 1
                If TheVal > 1 Then TheVal = 0

                'ValWheel.Value = TheVal
                ValWheel.Value = Math.Round(TheVal, 2)
                UpdateUIValChange()

            End If

            ' Return to avoid further processing
            Return

        End If

        ' Is the mouse over the Sat wheel?
        If IsPointInsideCircle(
                e.X, e.Y, SatWheel.Location.X + SatWheel.Radius + SatWheel.Padding,
                SatWheel.Location.Y + SatWheel.Radius + SatWheel.Padding, SatWheel.Radius) Then
            ' Yes, the mouse is over the Sat wheel.

            If e.Delta <> 0 Then

                ClearFocus()

                ' Adjust the sat based on the mouse wheel scroll direction
                'TheSat += If(e.Delta > 0, 0.01, -0.01) ' Increase or decrease sat by 1 percent
                TheSat += If(e.Delta > 0, 0.1, -0.1) ' Increase or decrease sat by 10 percent

                ' Ensure the sat value wraps around within 0-1 
                If TheSat < 0 Then TheSat = 1
                If TheSat > 1 Then TheSat = 0

                SatWheel.Saturation = TheSat

                UpdateUISatChange()

            End If

            ' Return to avoid further processing
            Return

        End If

        ' Is the mouse over the Hue wheel?
        If IsPointInsideCircle(
                e.X, e.Y, HueWheel.Location.X + HueWheel.Radius + HueWheel.Padding,
                HueWheel.Location.Y + HueWheel.Radius + HueWheel.Padding, HueWheel.Radius) Then
            ' Yes, the mouse is over the Hue wheel.

            ' Check if the mouse wheel is scrolled
            If e.Delta <> 0 Then

                ClearFocus()

                ' Adjust the hue based on the mouse wheel scroll direction
                TheHue += If(e.Delta > 0, 10, -10) ' Increase or decrease hue by 10 degrees

                ' Ensure the hue value wraps around within 0-360 degrees
                If TheHue < 0 Then TheHue += 360
                If TheHue >= 360 Then TheHue -= 360

                ' Snap to nearest 10° increment
                Dim snappedHue As Double = Math.Round(TheHue / 10) * 10
                If snappedHue >= 360 Then snappedHue -= 360
                TheHue = snappedHue

                UpdateUIHueChange()

            End If

            Return

        End If

        ' Is the mouse over the Wedges wheel?
        If IsPointInsideCircle(
                e.X, e.Y, WedgesWheel.Location.X + WedgesWheel.Radius + WedgesWheel.Padding,
                WedgesWheel.Location.Y + WedgesWheel.Radius + WedgesWheel.Padding, WedgesWheel.Radius) Then
            ' Yes, the mouse is over the Wedges wheel.

            ' Check if the mouse wheel is scrolled
            If e.Delta <> 0 Then
                ' mouse wheel scrolled, adjust the hue angle

                ClearFocus()

                UpDatingColor = True

                ' Adjust the selected hue angle
                HueWheel.SelectedHueAngle += If(e.Delta > 0, 30, -30)

                ' Wrap around 0–360
                If HueWheel.SelectedHueAngle < 0 Then HueWheel.SelectedHueAngle += 360
                If HueWheel.SelectedHueAngle >= 360 Then HueWheel.SelectedHueAngle -= 360

                ' Snap to nearest wedge center (every 30°)
                Dim snappedHue As Double = Math.Round(HueWheel.SelectedHueAngle / 30) * 30
                If snappedHue >= 360 Then snappedHue -= 360
                'HueWheel.SelectedHueAngle = snappedHue

                TheHue = snappedHue
                TheSat = 1
                TheVal = 1

                TheColor = ColorFromHSV(TheHue, TheSat, TheVal)

                ' Update UI controls
                HueWheel.Color = TheColor

                HueWheel.SelectedHueAngle = TheHue
                HueTrackBar.Value = TheHue * 100

                HueNumericUpDown.Value = TheHue
                SaturationTrackBar.Value = TheSat * 10000
                SaturationNumericUpDown.Value = TheSat * 100

                BrightnessTrackBar.Value = TheVal * 10000
                BrightnessNumericUpDown.Value = TheVal * 100

                HexTextBox.Text = HsvToHex(TheHue, TheSat, TheVal)

                Invalidate()

                UpDatingColor = False

            End If

        End If

    End Sub

    Private Sub HueTrackBar_Scroll(sender As Object, e As EventArgs) Handles HueTrackBar.Scroll

        ' Check if we are currently updating the color to avoid recursive calls or cascading updates
        If UpDatingColor Then Return

        TheHue = HueTrackBar.Value / 100

        UpdateUIHueChange()

    End Sub

    Private Sub SaturationTrackBar_Scroll(sender As Object, e As EventArgs) Handles SaturationTrackBar.Scroll

        ' Check if we are currently updating the color to avoid recursive calls or cascading updates
        If UpDatingColor Then Return

        TheSat = SaturationTrackBar.Value / 10000

        SatWheel.Saturation = TheSat

        UpdateUISatChange()

    End Sub

    Private Sub BrightnessTrackBar_Scroll(sender As Object, e As EventArgs) Handles BrightnessTrackBar.Scroll

        ' Check if we are currently updating the color to avoid recursive calls or cascading updates
        If UpDatingColor Then Return

        UpDatingColor = True

        TheVal = BrightnessTrackBar.Value / 10000

        ValWheel.Value = TheVal


        HueWheel.Color = ColorFromHSV(TheHue, TheSat, TheVal)

        BrightnessNumericUpDown.Value = TheVal * 100

        HexTextBox.Text = HsvToHex(TheHue, TheSat, TheVal)

        Invalidate()

        UpDatingColor = False

    End Sub

    Private Sub HueNumericUpDown_ValueChanged(sender As Object, e As EventArgs) Handles HueNumericUpDown.ValueChanged

        ' Check if we are currently updating the color to avoid recursive calls or cascading updates
        If UpDatingColor Then Return

        TheHue = HueNumericUpDown.Value

        UpdateUIHueChange()

    End Sub

    Private Sub SaturationNumericUpDown_ValueChanged(sender As Object, e As EventArgs) Handles SaturationNumericUpDown.ValueChanged

        ' Check if we are currently updating the color to avoid recursive calls or cascading updates
        If UpDatingColor Then Return

        TheSat = SaturationNumericUpDown.Value / 100.0

        SatWheel.Saturation = TheSat

        UpdateUISatChange()

    End Sub

    Private Sub BrightnessNumericUpDown_ValueChanged(sender As Object, e As EventArgs) Handles BrightnessNumericUpDown.ValueChanged

        ' Check if we are currently updating the color to avoid recursive calls or cascading updates
        If UpDatingColor Then Return

        UpDatingColor = True

        TheVal = BrightnessNumericUpDown.Value / 100.0

        ValWheel.Value = TheVal


        BrightnessTrackBar.Value = TheVal * 10000

        HueWheel.Color = ColorFromHSV(TheHue, TheSat, TheVal)

        HexTextBox.Text = HsvToHex(TheHue, TheSat, TheVal)

        Invalidate()

        UpDatingColor = False

    End Sub

    Private Sub HexTextBox_TextChanged(sender As Object, e As EventArgs) Handles HexTextBox.TextChanged

        ' Check if we are currently updating the color to avoid recursive calls or cascading updates
        If UpDatingColor Then Return

        ' Check if the text is a valid hex color
        ' Ensure the text is in the correct format (6 hex digits, optionally prefixed with '#')
        If HexTextBox.Text.Length = 6 OrElse HexTextBox.Text.Length = 7 Then

            ' Remove the '#' character if present
            Dim hexColor As String = HexTextBox.Text.TrimStart("#"c)

            ' Validate the hex color format (6 hex digits)
            If System.Text.RegularExpressions.Regex.IsMatch(hexColor, "^[0-9A-Fa-f]{6}$") Then

                UpDatingColor = True

                ' Convert hex to RGB
                Dim r As Integer = Convert.ToInt32(hexColor.Substring(0, 2), 16)
                Dim g As Integer = Convert.ToInt32(hexColor.Substring(2, 2), 16)
                Dim b As Integer = Convert.ToInt32(hexColor.Substring(4, 2), 16)

                ' Ensure RGB values are within the valid range
                r = Math.Max(0, Math.Min(255, r))
                g = Math.Max(0, Math.Min(255, g))
                b = Math.Max(0, Math.Min(255, b))

                ' Create a new color from the RGB values
                TheColor = Color.FromArgb(r, g, b)

                ' Convert RGB to HSV
                Dim hsv = RGBtoHSV(TheColor)
                TheHue = hsv.Hue
                TheSat = hsv.Saturation
                TheVal = hsv.Value

                ' Update the HueWheel with the new color and hue
                HueWheel.Color = TheColor
                HueWheel.SelectedHueAngle = TheHue

                ' Update the trackbars and numeric up-downs
                HueTrackBar.Value = TheHue * 100
                HueNumericUpDown.Value = TheHue

                SaturationTrackBar.Value = TheSat * 10000
                SaturationNumericUpDown.Value = TheSat * 100

                SatWheel.Saturation = TheSat


                BrightnessTrackBar.Value = TheVal * 10000
                BrightnessNumericUpDown.Value = TheVal * 100
                ValWheel.Value = TheVal

                HexTextBox.Text = HsvToHex(TheHue, TheSat, TheVal)

                Invalidate() ' Redraw the form to reflect changes

                UpDatingColor = False

            End If

        End If

    End Sub

    Private Sub DrawLables(e As PaintEventArgs)

        e.Graphics.DrawString("Name: " & GetColorName(ColorFromHSV(TheHue, TheSat, TheVal)),
                             Me.Font, Brushes.Black, HueWheel.Location.X + HueWheel.Size.Width + HueWheel.Padding + 30, 20)

        e.Graphics.DrawString("Hex: " & HsvToHex(TheHue, TheSat, TheVal),
                             Me.Font, Brushes.Black, HueWheel.Location.X + HueWheel.Size.Width + HueWheel.Padding + 30, HexTextBox.Top + 2)

        e.Graphics.DrawString("Hue: " & RGBtoHSV(ColorFromHSV(TheHue, TheSat, TheVal)).Hue.ToString("0.###") & "°",
                             Me.Font, Brushes.Black, HueTrackBar.Left, HueTrackBar.Top - HueNumericUpDown.Height)


        e.Graphics.DrawString("Saturation: " & (RGBtoHSV(ColorFromHSV(TheHue, TheSat, TheVal)).Saturation * 100).ToString("0.##") & "%",
                             Me.Font, Brushes.Black, SaturationTrackBar.Left, SaturationTrackBar.Top - SaturationNumericUpDown.Height)


        e.Graphics.DrawString("Value: " & (RGBtoHSV(ColorFromHSV(TheHue, TheSat, TheVal)).Value * 100).ToString("0.##") & "%",
                      Me.Font, Brushes.Black, BrightnessTrackBar.Left, BrightnessTrackBar.Top - BrightnessNumericUpDown.Height)

    End Sub

    Private Sub DrawWedgesWheel(e As PaintEventArgs)

        e.Graphics.DrawImage(WedgesWheel.Bitmap,
                             WedgesWheel.Location.X,
                             WedgesWheel.Location.Y,
                             WedgesWheel.Bitmap.Width,
                             WedgesWheel.Bitmap.Height)

    End Sub

    Private Sub DrawHueWheel(e As PaintEventArgs)

        e.Graphics.DrawImage(HueWheel.Bitmap,
                             HueWheel.Location.X,
                             HueWheel.Location.Y,
                             HueWheel.Bitmap.Width,
                             HueWheel.Bitmap.Height)

    End Sub

    Private Sub DrawSatWheel(e As PaintEventArgs)

        SatWheel.Draw(SatWheel.Size.Width, 20, TheHue, Color.Transparent)

        e.Graphics.DrawImage(SatWheel.Bitmap,
                             SatWheel.Location.X,
                             SatWheel.Location.Y,
                             SatWheel.Bitmap.Width,
                             SatWheel.Bitmap.Height)

    End Sub

    Private Sub DrawValWheel(e As PaintEventArgs)

        ValWheel.Draw(ValWheel.Size.Width, 20, TheHue, Color.Transparent)

        e.Graphics.DrawImage(ValWheel.Bitmap,
                             ValWheel.Location.X,
                             ValWheel.Location.Y,
                             ValWheel.Bitmap.Width,
                             ValWheel.Bitmap.Height)

    End Sub

    Private Sub DrawSelectedColor(e As PaintEventArgs)

        ' Draw the selected color rectangle
        Dim selectedColorRect As New Rectangle(
            HueWheel.Location.X + 20 + (HueWheel.Size.Width - SelectedColorEllipseSize) \ 2,
            HueWheel.Location.Y + 20 + (HueWheel.Size.Width - SelectedColorEllipseSize) \ 2,
            SelectedColorEllipseSize,
            SelectedColorEllipseSize)
        'SelectedColorEllipseSize
        Using brush As New SolidBrush(ColorFromHSV(TheHue, TheSat, TheVal))
            e.Graphics.FillEllipse(brush, selectedColorRect)
        End Using

        Using pen As New Pen(Color.Black, 3)
            e.Graphics.DrawEllipse(pen, selectedColorRect)
        End Using

    End Sub

    Private Sub DrawHuePointer(e As PaintEventArgs)

        ' Draw the pointer triangle
        If HueWheel.Bitmap IsNot Nothing Then

            Dim centerX = HueWheel.Location.X + HueWheel.Radius + HueWheel.Padding
            Dim centerY = HueWheel.Location.Y + HueWheel.Radius + HueWheel.Padding
            Dim angleRad = HueWheel.SelectedHueAngle * Math.PI / 180

            ' Tip of the pointer
            Dim pointerLength = HueWheel.Radius + 6 'Pointer gap from the edge
            Dim tip = New Point(
                CInt(centerX + pointerLength * Math.Cos(angleRad)),
                CInt(centerY + pointerLength * Math.Sin(angleRad)))

            ' Base angles
            Dim baseOffset = 150 * Math.PI / 180
            Dim triangleSize = 15

            ' Base points before reflection
            Dim leftBase = New Point(
                CInt(tip.X + triangleSize * Math.Cos(angleRad + baseOffset)),
                CInt(tip.Y + triangleSize * Math.Sin(angleRad + baseOffset)))
            Dim rightBase = New Point(
                CInt(tip.X + triangleSize * Math.Cos(angleRad - baseOffset)),
                CInt(tip.Y + triangleSize * Math.Sin(angleRad - baseOffset)))

            ' Reflect base to position triangle tip-forward
            Dim points = {
                tip,
                New Point(2 * tip.X - rightBase.X, 2 * tip.Y - rightBase.Y),
                New Point(2 * tip.X - leftBase.X, 2 * tip.Y - leftBase.Y)}

            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            ' Draw triangle with theme-adaptive outline
            Dim fillColor = HueWheel.Color
            'Dim outlineColor = If(fillColor.GetBrightness() < 0.5, Color.White, Color.Black)

            Using brush As New SolidBrush(fillColor),
                  pen As New Pen(Color.Black, 3)
                e.Graphics.FillPolygon(brush, points)
                e.Graphics.DrawPolygon(pen, points)
            End Using

        End If

    End Sub

    Private Sub DrawSaturationPointer(e As PaintEventArgs)
        ' Draw the saturation pointer triangle
        If SatWheel.Bitmap IsNot Nothing Then
            Dim centerX = SatWheel.Location.X + SatWheel.Radius + SatWheel.Padding
            Dim centerY = SatWheel.Location.Y + SatWheel.Radius + SatWheel.Padding
            'Dim angleRad = SatWheel.SelectedHueAngle * Math.PI / 180
            ' Calculate the angle for the pointer based on the saturation
            Dim angleRad = (SatWheel.Saturation * 360) * Math.PI / 180 ' Convert saturation to angle


            ' Tip of the pointer
            Dim pointerLength = SatWheel.Radius + 6 'Pointer gap from the edge
            Dim tip = New Point(
                CInt(centerX + pointerLength * Math.Cos(angleRad)),
                CInt(centerY + pointerLength * Math.Sin(angleRad)))
            ' Base angles
            Dim baseOffset = 150 * Math.PI / 180
            Dim triangleSize = 15
            ' Base points before reflection
            Dim leftBase = New Point(
                CInt(tip.X + triangleSize * Math.Cos(angleRad + baseOffset)),
                CInt(tip.Y + triangleSize * Math.Sin(angleRad + baseOffset)))
            Dim rightBase = New Point(
                CInt(tip.X + triangleSize * Math.Cos(angleRad - baseOffset)),
                CInt(tip.Y + triangleSize * Math.Sin(angleRad - baseOffset)))
            ' Reflect base to position triangle tip-forward
            Dim points = {
                tip,
                New Point(2 * tip.X - rightBase.X, 2 * tip.Y - rightBase.Y),
                New Point(2 * tip.X - leftBase.X, 2 * tip.Y - leftBase.Y)}
            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            ' Draw triangle with theme-adaptive outline
            'Dim fillColor = SatWheel.Color

            Dim fillColor = Color.White

            'Dim outlineColor = If(fillColor.GetBrightness() < 0.5, Color.White, Color.Black)

            Using brush As New SolidBrush(fillColor),
                  pen As New Pen(Color.Black, 3)
                e.Graphics.FillPolygon(brush, points)
                e.Graphics.DrawPolygon(pen, points)
            End Using
        End If

    End Sub

    Private Sub DrawValuePointer(e As PaintEventArgs)
        ' Draw the value pointer triangle
        If ValWheel.Bitmap IsNot Nothing Then
            Dim centerX = ValWheel.Location.X + ValWheel.Radius + ValWheel.Padding
            Dim centerY = ValWheel.Location.Y + ValWheel.Radius + ValWheel.Padding
            ' Calculate the angle for the pointer based on the value
            'Dim angleRad = (ValWheel.Value * 360) * Math.PI / 180 ' Convert value to angle
            Dim angleRad = (ValWheel.Value * 360) * Math.PI / 180 ' Convert value to angle





            ' Tip of the pointer
            Dim pointerLength = ValWheel.Radius + 6 'Pointer gap from the edge
            Dim tip = New Point(
                CInt(centerX + pointerLength * Math.Cos(angleRad)),
                CInt(centerY + pointerLength * Math.Sin(angleRad)))
            ' Base angles
            Dim baseOffset = 150 * Math.PI / 180
            Dim triangleSize = 15
            ' Base points before reflection
            Dim leftBase = New Point(
                CInt(tip.X + triangleSize * Math.Cos(angleRad + baseOffset)),
                CInt(tip.Y + triangleSize * Math.Sin(angleRad + baseOffset)))
            Dim rightBase = New Point(
                CInt(tip.X + triangleSize * Math.Cos(angleRad - baseOffset)),
                CInt(tip.Y + triangleSize * Math.Sin(angleRad - baseOffset)))
            ' Reflect base to position triangle tip-forward
            Dim points = {
                tip,
                New Point(2 * tip.X - rightBase.X, 2 * tip.Y - rightBase.Y),
                New Point(2 * tip.X - leftBase.X, 2 * tip.Y - leftBase.Y)}
            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            ' Draw triangle with theme-adaptive outline
            Dim fillColor = Color.Black
            Using brush As New SolidBrush(fillColor),
                  pen As New Pen(Color.Black, 3)
                e.Graphics.FillPolygon(brush, points)
                e.Graphics.DrawPolygon(pen, points)
            End Using
        End If
    End Sub

    Public Function GetColorName(color As Color) As String

        ' Custom color-name pairs
        Dim customColors As Dictionary(Of Color, String) = New Dictionary(Of Color, String) From {
        {ColorFromHSV(0, 1, 1), "Red"},
        {ColorFromHSV(10, 1, 1), "Scarlet"},
        {ColorFromHSV(20, 1, 1), "International Orange"},
        {ColorFromHSV(30, 1, 1), "Orange"},
        {ColorFromHSV(40, 1, 1), "Neon Carrot"},
        {ColorFromHSV(50, 1, 1), "Cyber Yellow"},
        {ColorFromHSV(60, 1, 1), "Yellow"},
        {ColorFromHSV(70, 1, 1), "Volt"},
        {ColorFromHSV(80, 1, 1), "Lemon Green"},
        {ColorFromHSV(90, 1, 1), "Chartreuse"},
        {ColorFromHSV(100, 1, 1), "Hyper Green"},
        {ColorFromHSV(110, 1, 1), "Signal Green"},
        {ColorFromHSV(120, 1, 1), "Green or Lime"},
        {ColorFromHSV(130, 1, 1), "Electric Lime"},
        {ColorFromHSV(140, 1, 1), "Cathode Green"},
        {ColorFromHSV(150, 1, 1), "Spring Green"},
        {ColorFromHSV(160, 1, 1), "Medium Spring Green"},
        {ColorFromHSV(170, 1, 1), "Bright Turquoise"},
        {ColorFromHSV(180, 1, 1), "Aqua or Cyan"},
        {ColorFromHSV(190, 1, 1), "Vivid Sky Blue"},
        {ColorFromHSV(200, 1, 1), "Blue Bolt"},
        {ColorFromHSV(210, 1, 1), "Azure"},
        {ColorFromHSV(220, 1, 1), "Nīlā Blue"},
        {ColorFromHSV(230, 1, 1), "Vibrant Blue"},
        {ColorFromHSV(240, 1, 1), "Blue"},
        {ColorFromHSV(250, 1, 1), "Deep Indigo"},
        {ColorFromHSV(260, 1, 1), "Electric Ultramarine"},
        {ColorFromHSV(270, 1, 1), "Violet"},
        {ColorFromHSV(280, 1, 1), "Vivid Purple"},
        {ColorFromHSV(290, 1, 1), "Purple"},
        {ColorFromHSV(300, 1, 1), "Magenta or Fuchsia"},
        {ColorFromHSV(310, 1, 1), "Hot Magenta"},
        {ColorFromHSV(320, 1, 1), "Fashion Fuchsia"},
        {ColorFromHSV(330, 1, 1), "Rose or Bright Pink"},
        {ColorFromHSV(340, 1, 1), "Red Light Neon"},
        {ColorFromHSV(350, 1, 1), "Torch Red"},
        {ColorFromHSV(0, 0, 0.1), "Eerie Black or Gray 10"}, '0°, 0%, 10%
        {ColorFromHSV(0, 0, 0.2), "Dark Charcoal or Gray 20"},
        {ColorFromHSV(0, 0, 0.3), "Quartz or Gray 30"},
        {ColorFromHSV(0, 0, 0.4), "Granite or Gray 40"},
        {ColorFromHSV(0, 0, 0.5), "Gray or Gray 50"},
        {ColorFromHSV(0, 0, 0.6), "Basalt or Gray 60"},
        {ColorFromHSV(0, 0, 0.7), "Palladium or Gray 70"},
        {ColorFromHSV(0, 0, 0.8), "Cerebral or Gray 80"},
        {ColorFromHSV(0, 0, 0.9), "Platinum​​ or Gray 90"},
        {ColorFromHSV(222.857, 0.0583, 0.4706), "Space Gray"},
        {ColorFromHSV(217.5, 0.1013, 0.3098), "Cyberspace Gray"}, '217.5°, 10.13%, 30.98%
        {ColorFromHSV(210, 0.0584, 0.5373), "Gunmetal Gray"}, '210°, 5.84%, 53.73%
        {ColorFromHSV(0, 0.7455, 0.6471), "Brown"}, '0°, 74.55%, 64.71%
        {ColorFromHSV(23.514, 0.2948, 0.9843), "Apricot"}, '23.514°, 29.48%, 98.43%
        {ColorFromHSV(336.404, 0.349, 1), "Carnation Pink"}, '336.404°, 34.9%, 100%
        {ColorFromHSV(195.808, 1, 0.6549), "Cerulean"}, '336.404°, 34.9%, 100%
        {ColorFromHSV(5, 0.8, 1), "Red Orange"}, '336.404°, 34.9%, 100%
        {ColorFromHSV(336.22, 0.664, 0.9686), "Violet Red"}, '336.404°, 34.9%, 100%
        {ColorFromHSV(34.286, 0.7412, 1), "Yellow Orange"} '336.404°, 34.9%, 100%
    }

        ' Try to match with custom colors
        For Each kvp In customColors
            If ColorsAreEqual(kvp.Key, color) Then
                Return kvp.Value
            End If
        Next

        ' Fallback to known system colors
        Dim knownColorName As String = GetKnownColorName(color)
        If knownColorName IsNot Nothing Then
            Return knownColorName
        End If

        Return String.Empty

    End Function

    Private Shared Function ColorsAreEqual(c1 As Color, c2 As Color, Optional tolerance As Integer = 2) As Boolean

        Return Math.Abs(CInt(c1.R) - CInt(c2.R)) <= tolerance AndAlso
           Math.Abs(CInt(c1.G) - CInt(c2.G)) <= tolerance AndAlso
           Math.Abs(CInt(c1.B) - CInt(c2.B)) <= tolerance

    End Function

    Private Shared Function GetKnownColorName(color As Color) As String

        ' First check base known colors (skipping system ones)
        For Each knownColor As KnownColor In [Enum].GetValues(GetType(KnownColor))
            If knownColor <= KnownColor.YellowGreen And knownColor >= KnownColor.AliceBlue Then ' Excludes system-defined colors
                Dim knownColorValue As Color = Color.FromKnownColor(knownColor)
                If knownColorValue.ToArgb() = color.ToArgb() Then
                    Return knownColor.ToString()
                End If
            End If
        Next

        ' Fallback: check all known colors
        For Each knownColor As KnownColor In [Enum].GetValues(GetType(KnownColor))
            Dim knownColorValue As Color = Color.FromKnownColor(knownColor)
            If knownColorValue.ToArgb() = color.ToArgb() Then
                Return knownColor.ToString()
            End If
        Next

        Return Nothing

    End Function

    Public Function ColorFromHSV(hue As Double, saturation As Double, brightness As Double) As Color
        ' Function to convert HSV to RGB

        Dim r As Double = 0, g As Double = 0, b As Double = 0

        If saturation = 0 Then
            r = brightness
            g = brightness
            b = brightness
        Else
            Dim sector As Integer = CInt(Math.Floor(hue / 60)) Mod 6
            Dim fractional As Double = (hue / 60) - Math.Floor(hue / 60)

            Dim p As Double = brightness * (1 - saturation)
            Dim q As Double = brightness * (1 - saturation * fractional)
            Dim t As Double = brightness * (1 - saturation * (1 - fractional))

            Select Case sector
                Case 0
                    r = brightness
                    g = t
                    b = p
                Case 1
                    r = q
                    g = brightness
                    b = p
                Case 2
                    r = p
                    g = brightness
                    b = t
                Case 3
                    r = p
                    g = q
                    b = brightness
                Case 4
                    r = t
                    g = p
                    b = brightness
                Case 5
                    r = brightness
                    g = p
                    b = q
            End Select
        End If

        Return Color.FromArgb(CInt(r * 255), CInt(g * 255), CInt(b * 255))

    End Function

    Public Function RGBtoHSV(color As Color) As (Hue As Double, Saturation As Double, Value As Double)
        ' This function converts RGB color to HSV (Hue, Saturation, Value) format.

        Dim r As Double = color.R / 255.0
        Dim g As Double = color.G / 255.0
        Dim b As Double = color.B / 255.0

        Dim max As Double = Math.Max(r, Math.Max(g, b))
        Dim min As Double = Math.Min(r, Math.Min(g, b))
        Dim delta As Double = max - min

        Dim h As Double
        If delta = 0 Then
            h = 0
        ElseIf max = r Then
            h = 60 * (((g - b) / delta) Mod 6)
        ElseIf max = g Then
            h = 60 * (((b - r) / delta) + 2)
        Else
            h = 60 * (((r - g) / delta) + 4)
        End If

        If h < 0 Then h += 360

        Dim s As Double = If(max = 0, 0, delta / max)
        Dim v As Double = max

        Return (h, s, v)

    End Function

    Public Function HsvToHex(hue As Double, saturation As Double, value As Double) As String

        ' Convert HSV to RGB and then to hex
        Dim r As Integer, g As Integer, b As Integer

        If saturation = 0 Then
            ' If saturation is 0, the color is a shade of gray
            r = CInt(value * 255)
            g = r
            b = r
        Else
            Dim c As Double = value * saturation
            Dim x As Double = c * (1 - Math.Abs((hue / 60) Mod 2 - 1))
            Dim m As Double = value - c

            Select Case hue
                Case 0 To 60
                    r = CInt((c + m) * 255)
                    g = CInt((x + m) * 255)
                    b = CInt(m * 255)
                Case 60 To 120
                    r = CInt((x + m) * 255)
                    g = CInt((c + m) * 255)
                    b = CInt(m * 255)
                Case 120 To 180
                    r = CInt(m * 255)
                    g = CInt((c + m) * 255)
                    b = CInt((x + m) * 255)
                Case 180 To 240
                    r = CInt(m * 255)
                    g = CInt((x + m) * 255)
                    b = CInt((c + m) * 255)
                Case 240 To 300
                    r = CInt((x + m) * 255)
                    g = CInt(m * 255)
                    b = CInt((c + m) * 255)
                Case 300 To 360
                    r = CInt((c + m) * 255)
                    g = CInt(m * 255)
                    b = CInt((x + m) * 255)
            End Select
        End If

        ' Return the hex color string
        Return String.Format("{0:X2}{1:X2}{2:X2}", r, g, b)

    End Function

    Function IsPointInsideCircle(pointX As Double, pointY As Double,
             centerX As Double, centerY As Double, radius As Double) As Boolean
        ' Note: This function is a simplified version of the distance check.
        ' It is efficient and avoids unnecessary calculations by using squared distances.

        Dim dx As Double = pointX - centerX
        Dim dy As Double = pointY - centerY
        Dim distanceSquared As Double = dx * dx + dy * dy
        Return distanceSquared <= radius * radius

        ' If the squared distance from the point to the center of the circle
        ' (dx)^2 + (dy)^2 is less than or equal <= to the squared radius (radius^2),
        ' then the point is inside or on the edge of the circle.

        ' If the squared distance is greater than > the squared radius, then the point is outside the circle.
        ' This optimization is particularly useful in performance-critical applications
        ' such as real-time graphics rendering or physics simulations.

    End Function

    Private Sub InitializeApplication()

        ' Set the form properties
        DoubleBuffered = True ' Reduce flickering
        Text = "Color Picker - Code with Joe"

        UpDatingColor = True


        ' Initialize the HueWheel with default values
        HueWheel.Color = TheColor
        HueWheel.SelectedHueAngle = TheHue
        HueWheel.Location.X = 20
        HueWheel.Location.Y = 20
        HueWheel.Draw(225, 20, BackColor)

        HueTrackBar.Value = TheHue * 100
        HueNumericUpDown.Value = TheHue

        WedgesWheel.Location.X = HueWheel.Location.X - 25
        WedgesWheel.Location.Y = HueWheel.Location.Y - 25

        WedgesWheel.Size = New Size(275, 275)
        WedgesWheel.DrawWedges(275, 20, BackColor)

        ' Initialize the SatWheel with default values
        SatWheel.Color = TheColor
        'SatWheel.SelectedHueAngle = TheHue
        SatWheel.Saturation = TheSat
        SatWheel.Draw(175, 20, TheHue, BackColor)

        SatWheel.Location.X = HueWheel.Location.X + (HueWheel.Size.Width - SatWheel.Size.Width) \ 2
        SatWheel.Location.Y = HueWheel.Location.Y + (HueWheel.Size.Width - SatWheel.Size.Width) \ 2

        ' Initialize the ValWheel with default values
        ValWheel.Color = TheColor
        ValWheel.SelectedHueAngle = TheHue
        ValWheel.Size.Width = 125
        ValWheel.Size.Height = 125

        'ValWheel.Draw(100, 20, TheHue, BackColor)

        ValWheel.Location.X = HueWheel.Location.X + (HueWheel.Size.Width - ValWheel.Size.Width) \ 2
        ValWheel.Location.Y = HueWheel.Location.Y + (HueWheel.Size.Width - ValWheel.Size.Width) \ 2

        ValWheel.Value = TheVal

        SaturationTrackBar.Value = TheSat * 10000
        SaturationNumericUpDown.Value = TheSat * 100

        BrightnessTrackBar.Value = TheVal * 10000
        BrightnessNumericUpDown.Value = TheVal * 100

        HexTextBox.Text = HsvToHex(TheHue, TheSat, TheVal)

        Invalidate()

        UpDatingColor = False

    End Sub

    Private Sub ClearFocus()

        ' Clear focus from any active control
        If ActiveControl IsNot Nothing Then ActiveControl = Nothing

    End Sub

    Private Sub UpdateUIHueChange()

        UpDatingColor = True

        HueWheel.Color = ColorFromHSV(TheHue, TheSat, TheVal)

        HueWheel.SelectedHueAngle = TheHue

        HueTrackBar.Value = TheHue * 100

        HueNumericUpDown.Value = TheHue

        HexTextBox.Text = HsvToHex(TheHue, TheSat, TheVal)

        Invalidate()

        UpDatingColor = False

    End Sub

    Private Sub UpdateUISatChange()

        UpDatingColor = True

        SaturationTrackBar.Value = TheSat * 10000

        SaturationNumericUpDown.Value = TheSat * 100

        HueWheel.Color = ColorFromHSV(TheHue, TheSat, TheVal)

        HexTextBox.Text = HsvToHex(TheHue, TheSat, TheVal)

        Invalidate()

        UpDatingColor = False

    End Sub

    Private Sub UpdateUIValChange()

        UpDatingColor = True

        BrightnessTrackBar.Value = TheVal * 10000

        BrightnessNumericUpDown.Value = TheVal * 100

        HueWheel.Color = ColorFromHSV(TheHue, TheSat, TheVal)

        HexTextBox.Text = HsvToHex(TheHue, TheSat, TheVal)

        Invalidate()

        UpDatingColor = False

    End Sub

End Class
