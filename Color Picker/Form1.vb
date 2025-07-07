
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
                ' Calculate the angle and set the color based on the position
                Dim angle As Double = (Math.Atan2(dy, dx) * 180.0 / Math.PI + 360) Mod 360

                Color = ColorFromHSV(angle, 1, 1)

            End If

        End Sub

        ' Function to convert HSV to RGB
        Private Function ColorFromHSV(hue As Double, saturation As Double, brightness As Double) As Color

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

    Private ColorWheel As ColorWheelStruct

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.DoubleBuffered = True ' Reduce flickering

        ' Set the form's start position to center screen
        Me.StartPosition = FormStartPosition.CenterScreen
        ' Set the form's text
        Me.Text = "Color Picker"

        ColorWheel.Location.X = 400
        ColorWheel.Location.Y = 10

        ColorWheel.Draw(360, 10, BackColor)

        Invalidate()

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        e.Graphics.DrawImage(ColorWheel.Bitmap,
                             ColorWheel.Location.X,
                             ColorWheel.Location.Y,
                             ColorWheel.Bitmap.Width,
                             ColorWheel.Bitmap.Height)

        ' Draw the selected color rectangle
        Dim selectedColorRect As New Rectangle(20, 20, 100, 100)
        Using brush As New SolidBrush(ColorWheel.Color)
            e.Graphics.FillRectangle(brush, selectedColorRect)
        End Using
        Using pen As New Pen(Color.Black, 3)
            e.Graphics.DrawRectangle(pen, selectedColorRect)
        End Using

        e.Graphics.DrawString("Name: " & GetColorName(ColorWheel.Color),
                             Me.Font, Brushes.Black, 130, 20)

        e.Graphics.DrawString("Name: " & ColorToHex(ColorWheel.Color),
                             Me.Font, Brushes.Black, 130, 40)




    End Sub

    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown

        If e.Button = MouseButtons.Left Then
            ColorWheel.GetColorAtPoint(New Point(e.X, e.Y))
            Invalidate()
        End If

    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove

        If e.Button = MouseButtons.Left Then
            ColorWheel.GetColorAtPoint(New Point(e.X, e.Y))
            Invalidate()
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

End Class
