Imports System.Drawing.Drawing2D

Public Class Form1

    Private Structure ColorWheelStruct

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
                    Dim dx As Integer = x - centerX
                    Dim dy As Integer = y - centerY
                    Dim dist As Double = Math.Sqrt(dx * dx + dy * dy)
                    If dist <= Radius Then
                        Dim angle As Double = (Math.Atan2(dy, dx) * 180.0 / Math.PI + 360) Mod 360
                        Bitmap.SetPixel(x, y, ColorFromHSV(angle, 1, 1))
                    Else
                        Bitmap.SetPixel(x, y, backcolor)
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

                Color = ColorFromHSV(angle, RGBtoHSV(Color).Saturation, RGBtoHSV(Color).Value)
                SelectedHueAngle = angle ' Save angle for rendering pointer

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

    Private HueWheel As ColorWheelStruct

    Private TheColor As Color = Color.Chartreuse ' Default color for the color wheel

    Private TheHue As Double = RGBtoHSV(TheColor).Hue

    Private TheSat As Double = RGBtoHSV(TheColor).Saturation

    Private TheVal As Double = RGBtoHSV(TheColor).Value

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.DoubleBuffered = True ' Reduce flickering

        ' Set the form's start position to center screen
        Me.StartPosition = FormStartPosition.CenterScreen
        ' Set the form's text
        Me.Text = "Color Picker - Code with Joe"


        BrightnessTrackBar.Value = TheVal * 100 ' Set initial value for the trackbar
        SaturationTrackBar.Value = TheSat * 100 ' Set initial saturation for the trackbar
        BrightnessNumericUpDown.Value = TheVal * 100
        SaturationNumericUpDown.Value = TheSat * 100

        HueWheel.Color = TheColor
        HueWheel.SelectedHueAngle = TheHue  ' Set initial hue angle based on the value


        HueWheel.Location.X = 400
        HueWheel.Location.Y = 10

        HueWheel.Draw(300, 20, BackColor)


        Invalidate()

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        DrawColorWheel(e)

        'HueWheel.Color = ColorFromHSV(TheHue, TheSat, TheVal)

        DrawHuePointer(e)

        DrawSelectedColor(e)

        e.Graphics.DrawString("Name: " & GetColorName(ColorFromHSV(TheHue, TheSat, TheVal)),
                             Me.Font, Brushes.Black, 130, 20)

        e.Graphics.DrawString("Hex: " & ColorToHex(ColorFromHSV(TheHue, TheSat, TheVal)),
                             Me.Font, Brushes.Black, 130, 40)

        'e.Graphics.DrawString("Hue: " & TheHue.ToString("0.#"),
        '                     Me.Font, Brushes.Black, 525, 350)


        e.Graphics.DrawString("Hue: " & ColorFromHSV(TheHue, TheSat, TheVal).GetHue.ToString("0.#"),
                             Me.Font, Brushes.Black, 525, 350)




        e.Graphics.DrawString("Saturation: " & (RGBtoHSV(ColorFromHSV(TheHue, TheSat, TheVal)).Saturation * 100).ToString("0.#") & "%",
                             Me.Font, Brushes.Black, 525, 370)

        'e.Graphics.DrawString("Brightness: " & (ColorWheel.Color.GetBrightness() * 100).ToString("0.#") & "%",
        '                          Me.Font, Brushes.Black, 525, 390)


        'e.Graphics.DrawString("Brightness: " & TheVal * 100.ToString("0.0") & "%",
        '                          Me.Font, Brushes.Black, 525, 390)



        e.Graphics.DrawString("Brightness: " & (TheVal * 100).ToString("0.#") & "%",
                      Me.Font, Brushes.Black, 525, 390)


    End Sub

    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown

        If e.Button = MouseButtons.Left Then

            HueWheel.GetColorAtPoint(New Point(e.X, e.Y))

            If TheHue <> HueWheel.SelectedHueAngle Then TheHue = HueWheel.SelectedHueAngle

            'HueWheel.Color = ColorFromHSV(TheHue, TheSat, TheVal)

            Invalidate()

        End If

    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove

        If e.Button = MouseButtons.Left Then

            HueWheel.GetColorAtPoint(New Point(e.X, e.Y))

            If TheHue <> HueWheel.SelectedHueAngle Then TheHue = HueWheel.SelectedHueAngle

            Invalidate()

        End If

    End Sub

    Private Sub BrightnessTrackBar_Scroll(sender As Object, e As EventArgs) Handles BrightnessTrackBar.Scroll

        TheVal = BrightnessTrackBar.Value / 100.0

        HueWheel.Color = ColorFromHSV(TheHue, TheSat, TheVal)

        BrightnessNumericUpDown.Value = TheVal * 100

        Invalidate()

    End Sub

    Private Sub SaturationTrackBar_Scroll(sender As Object, e As EventArgs) Handles SaturationTrackBar.Scroll

        ' Update the saturation based on the trackbar value
        TheSat = SaturationTrackBar.Value / 100.0
        ' Update the color based on the new saturation
        HueWheel.Color = ColorFromHSV(TheHue, TheSat, TheVal)


        ' Update the saturation numeric up-down value
        SaturationNumericUpDown.Value = TheSat * 100

        ' Redraw the form to reflect the changes
        Invalidate()
    End Sub

    Private Sub BrightnessNumericUpDown_TextChanged(sender As Object, e As EventArgs) Handles BrightnessNumericUpDown.TextChanged

        ' Update the brightness based on the numeric up-down value
        TheVal = BrightnessNumericUpDown.Value / 100.0

        BrightnessTrackBar.Value = TheVal * 100

        ' Update the color based on the new brightness
        HueWheel.Color = ColorFromHSV(TheHue, TheSat, TheVal)
        ' Redraw the form to reflect the changes
        Invalidate()
    End Sub

    Private Sub SaturationNumericUpDown_TextChanged(sender As Object, e As EventArgs) Handles SaturationNumericUpDown.TextChanged



        ' Update the saturation based on the numeric up-down value
        TheSat = SaturationNumericUpDown.Value / 100.0
        SaturationTrackBar.Value = TheSat * 100
        ' Update the color based on the new saturation
        HueWheel.Color = ColorFromHSV(TheHue, TheSat, TheVal)
        ' Redraw the form to reflect the changes
        Invalidate()

    End Sub


    Private Sub DrawColorWheel(e As PaintEventArgs)
        e.Graphics.DrawImage(HueWheel.Bitmap,
                             HueWheel.Location.X,
                             HueWheel.Location.Y,
                             HueWheel.Bitmap.Width,
                             HueWheel.Bitmap.Height)
    End Sub

    Private Sub DrawSelectedColor(e As PaintEventArgs)
        ' Draw the selected color rectangle
        Dim selectedColorRect As New Rectangle(20, 20, 100, 100)
        Using brush As New SolidBrush(ColorFromHSV(TheHue, TheSat, TheVal))
            e.Graphics.FillRectangle(brush, selectedColorRect)
        End Using
        Using pen As New Pen(Color.Black, 3)
            e.Graphics.DrawRectangle(pen, selectedColorRect)
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

    ' Method to get the color name
    Public Shared Function GetColorName(color As Color) As String
        ' Check if the color is a known color
        Dim knownColorName As String = GetKnownColorName(color)
        If knownColorName IsNot Nothing Then
            Return knownColorName
        End If

        ' If not known, return a custom message
        Return ""
    End Function

    ' Method to check for known colors
    Private Shared Function GetKnownColorName(color As Color) As String
        For Each knownColor As KnownColor In [Enum].GetValues(GetType(KnownColor))
            Dim knownColorValue As Color = Color.FromKnownColor(knownColor)
            If knownColorValue.ToArgb() = color.ToArgb() Then
                Return knownColor.ToString()
            End If
        Next
        Return Nothing
    End Function

    ' Method to convert Color to HEX
    Private Shared Function ColorToHex(color As Color) As String
        Return $"#{color.R:X2}{color.G:X2}{color.B:X2}"
    End Function


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





End Class
