Imports System.Drawing.Drawing2D

Public Class Form1
    Private selectedColor As Color = Color.White

    Private ColorWheelSelectionLocation As Point

    Private bmp = New Bitmap(400, 400)
    Private gbmp As Graphics = Graphics.FromImage(bmp)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.DoubleBuffered = True ' Reduce flickering
        'Me.ClientSize = New Size(400, 400) ' Set initial size

        ' Set the form's background color to white
        'Me.BackColor = Color.White
        ' Set the form's border style to fixed single
        'Me.FormBorderStyle = FormBorderStyle.FixedSingle
        ' Set the form's start position to center screen
        Me.StartPosition = FormStartPosition.CenterScreen
        ' Set the form's text
        Me.Text = "Color Picker"
        ' Set the form's minimum size
        Me.MinimumSize = New Size(300, 300)
        ' Set the form's maximum size
        'Me.MaximumSize = New Size(800, 800)

        bmp = New Bitmap(Me.ClientSize.Width, Me.ClientSize.Height)
        gbmp = Graphics.FromImage(bmp)

        'Dim g As Graphics = Graphics.FromImage(bmp)


        'g.Clear(Color.White)
        'g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        'g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        'g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
        'g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality

        DrawColorWheel(gbmp, Math.Min(ClientSize.Width, ClientSize.Height) \ 2 - 20, New Point(ClientSize.Width \ 2, ClientSize.Height \ 2), 1440)

        'g.SmoothingMode = SmoothingMode.None
        ' Draw a rectangle to show the initial selected color
        gbmp.DrawRectangle(Pens.Black, 10, 10, 40, 40)
        Using brush As New SolidBrush(selectedColor)
            gbmp.FillRectangle(brush, 11, 11, 39, 39)
        End Using
        'gbmp.Dispose()

        Invalidate()
    End Sub

    Private Sub DrawColorWheel(g As Graphics, radius As Integer, center As Point, segments As Integer)


        g.Clear(BackColor)

        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias


        'For i As Integer = 0 To segments - 1

        '    Dim color As Color = ColorFromHSV(i, 1, 1)

        '    Using brush As New SolidBrush(color)

        '        Dim path As New Drawing2D.GraphicsPath()

        '        path.AddPie(center.X - radius, center.Y - radius, radius * 2, radius * 2, i, 1)

        '        g.FillPath(brush, path)

        '    End Using

        'Next

        'g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Dim width As Integer = Me.ClientSize.Width
        Dim height As Integer = Me.ClientSize.Height
        'Dim radius As Integer = Math.Min(width, height) / 2
        Dim centerX As Integer = width / 2
        Dim centerY As Integer = height / 2

        ' Loop through each pixel in the area of the circle
        For x As Integer = 0 To width - 1
            For y As Integer = 0 To height - 1
                Dim dx As Integer = x - centerX
                Dim dy As Integer = y - centerY
                Dim distance As Double = Math.Sqrt(dx * dx + dy * dy)

                If distance <= radius Then
                    Dim angle As Double = Math.Atan2(dy, dx) ' Angle in radians
                    Dim hue As Double = (angle + Math.PI) / (2 * Math.PI) * 360 ' Convert to hue
                    Dim color As Color = ColorFromHSV(hue, 1, 1) ' Full saturation and brightness

                    ' Set the pixel color
                    g.FillRectangle(New SolidBrush(color), x, y, 1, 1)
                End If
            Next
        Next








        ' Draw an elipse with a circual gradient the outter edge is gray and the center is transprent.
        'Dim g = e.Graphics
        'Dim rect As New Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2) ' Adjust as needed

        'Using path As New GraphicsPath()
        '    path.AddEllipse(rect)
        '    Using brush As New PathGradientBrush(path)
        '        brush.CenterColor = Color.Transparent
        '        brush.SurroundColors = New Color() {Color.Gray}
        '        g.FillPath(brush, path)
        '    End Using
        'End Using

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        'e.Graphics.Clear(Color.White)

        If bmp Is Nothing Then
            bmp = New Bitmap(ClientSize.Width, ClientSize.Height)
            gbmp = Graphics.FromImage(bmp)

        End If

        DrawColorWheel(gbmp, Math.Min(ClientSize.Width, ClientSize.Height) \ 2 - 20, New Point(ClientSize.Width \ 2, ClientSize.Height \ 2), 1440)

        'Using gbmp As Graphics = Graphics.FromImage(bmp)
        'gbmp.Clear(Color.White)
        'gbmp.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        'gbmp.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        'gbmp.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
        'gbmp.CompositingQuality = Drawing2D.CompositingQuality.HighQuality

        'End Using

        'End If


        'Dim gBmp As Graphics = Graphics.FromImage(bmp)
        'DrawColorWheel(gBmp, Math.Min(ClientSize.Width, ClientSize.Height) \ 2 - 20, New Point(ClientSize.Width \ 2, ClientSize.Height \ 2), 1440)


        'Dim g = e.Graphics

        'g.SmoothingMode = SmoothingMode.AntiAlias
        'g.CompositingQuality = CompositingQuality.GammaCorrected
        'g.CompositingMode = CompositingMode.SourceOver
        e.Graphics.DrawImageUnscaled(bmp, 0, 0, bmp.Width, bmp.Height)




        '' Draw a rectangle to show the selected color
        'g.DrawRectangle(Pens.Black, 10, 10, 40, 40)
        'Using brush As New SolidBrush(selectedColor)
        '    g.FillRectangle(brush, 11, 11, 39, 39)
        'End Using

        '' Draw luminosity gradient rectangle: white → selectedColor → black
        'Dim rectWidth As Integer = 40
        'Dim rectHeight As Integer = bmp.Height - 40
        'Dim rectX As Integer = bmp.Width - rectWidth - 10
        'Dim rectY As Integer = 20

        'Dim hue As Double, sat As Double, selVal As Double
        'ColorToHSV(selectedColor, hue, sat, selVal)
        'Dim midY As Integer = rectHeight \ 2

        '' Top half: white to selectedColor
        'For y As Integer = 0 To midY
        '    Dim t As Double = y / midY
        '    Dim color As Color = InterpolateHSV(Color.White, selectedColor, t)
        '    Using pen As New Pen(color)
        '        g.DrawLine(pen, rectX, rectY + y, rectX + rectWidth, rectY + y)
        '    End Using
        'Next

        '' Bottom half: selectedColor to black
        'For y As Integer = midY + 1 To rectHeight - 1
        '    Dim t As Double = (y - midY) / (rectHeight - 1 - midY)
        '    Dim color As Color = InterpolateHSV(selectedColor, Color.Black, t)
        '    Using pen As New Pen(color)
        '        g.DrawLine(pen, rectX, rectY + y, rectX + rectWidth, rectY + y)
        '    End Using
        'Next

        'g.DrawRectangle(Pens.Black, rectX, rectY, rectWidth, rectHeight)
    End Sub







    Private Sub Form1_MouseClick(sender As Object, e As MouseEventArgs) Handles MyBase.MouseClick

        'If e.Button = MouseButtons.Left Then

        '    Dim x As Integer = e.X
        '    Dim y As Integer = e.Y

        '    ' Check if the click is within the color wheel area
        '    Dim centerX As Integer = ClientSize.Width \ 2
        '    Dim centerY As Integer = ClientSize.Height \ 2
        '    Dim radius As Integer = Math.Min(ClientSize.Width, ClientSize.Height) \ 2 - 20
        '    If Math.Sqrt((x - centerX) ^ 2 + (y - centerY) ^ 2) <= radius Then

        '        ColorWheelSelectionLocation = New Point(e.X, e.Y)

        '        ' Get the color from the bitmap at the clicked position
        '        selectedColor = bmp.getpixel(e.X, e.Y)


        '        Invalidate() ' Redraw the form to show the new color

        '    End If

        'End If


        ' Get the color from the bitmap at the clicked position
        selectedColor = bmp.getpixel(e.X, e.Y)
        Invalidate() ' Redraw the form to show the new color


    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
        'If e.Button = MouseButtons.Left Then
        '    Dim x As Integer = e.X
        '    Dim y As Integer = e.Y
        '    ' Check if the mouse is within the color wheel area
        '    Dim centerX As Integer = ClientSize.Width \ 2
        '    Dim centerY As Integer = ClientSize.Height \ 2
        '    Dim radius As Integer = Math.Min(ClientSize.Width, ClientSize.Height) \ 2 - 20
        '    If Math.Sqrt((x - centerX) ^ 2 + (y - centerY) ^ 2) <= radius Then
        '        ' Calculate angle and saturation based on mouse position
        '        Dim angle As Double = Math.Atan2(y - centerY, x - centerX) * (180 / Math.PI)
        '        If angle < 0 Then angle += 360 ' Normalize to [0, 360)
        '        Dim saturation As Double = Math.Sqrt((x - centerX) ^ 2 + (y - centerY) ^ 2) / radius
        '        ' Set the selected color based on the angle and saturation
        '        selectedColor = ColorFromHSV(angle, saturation, 1.0)
        '        Invalidate() ' Redraw the form to show the new color
        '    End If
        'End If
    End Sub


    ' Linear interpolation between two colors in HSV space
    Private Function InterpolateHSV(c1 As Color, c2 As Color, t As Double) As Color
        Dim h1 As Double, s1 As Double, v1 As Double
        Dim h2 As Double, s2 As Double, v2 As Double
        ColorToHSV(c1, h1, s1, v1)
        ColorToHSV(c2, h2, s2, v2)
        ' Interpolate hue correctly (circular)
        Dim dh = h2 - h1
        If dh > 180 Then dh -= 360
        If dh < -180 Then dh += 360
        Dim h = (h1 + dh * t + 360) Mod 360
        Dim s = s1 + (s2 - s1) * t
        Dim v = v1 + (v2 - v1) * t
        Return ColorFromHSV(h, s, v)
    End Function

    'Private Function ColorFromHSV(hue As Double, saturation As Double, value As Double) As Color
    '    Dim hi = CInt(Math.Floor(hue / 60)) Mod 6
    '    Dim f = hue / 60 - Math.Floor(hue / 60)

    '    value = value * 255
    '    Dim v = CInt(value)
    '    Dim p = CInt(value * (1 - saturation))
    '    Dim q = CInt(value * (1 - f * saturation))
    '    Dim t = CInt(value * (1 - (1 - f) * saturation))

    '    Select Case hi
    '        Case 0 : Return Color.FromArgb(255, v, t, p)
    '        Case 1 : Return Color.FromArgb(255, q, v, p)
    '        Case 2 : Return Color.FromArgb(255, p, v, t)
    '        Case 3 : Return Color.FromArgb(255, p, q, v)
    '        Case 4 : Return Color.FromArgb(255, t, p, v)
    '        Case Else : Return Color.FromArgb(255, v, p, q)
    '    End Select
    'End Function

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



    Private Sub ColorToHSV(color As Color, ByRef hue As Double, ByRef sat As Double, ByRef val As Double)
        Dim r = color.R / 255.0
        Dim g = color.G / 255.0
        Dim b = color.B / 255.0

        Dim max = Math.Max(r, Math.Max(g, b))
        Dim min = Math.Min(r, Math.Min(g, b))
        val = max

        Dim delta = max - min

        If max = 0 Then
            sat = 0
        Else
            sat = delta / max
        End If

        If delta = 0 Then
            hue = 0
        ElseIf max = r Then
            hue = 60 * (((g - b) / delta) Mod 6)
        ElseIf max = g Then
            hue = 60 * (((b - r) / delta) + 2)
        Else
            hue = 60 * (((r - g) / delta) + 4)
        End If

        If hue < 0 Then hue += 360
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        'If bmp Is Nothing Then

        bmp = New Bitmap(ClientSize.Width, ClientSize.Height)
            gbmp = Graphics.FromImage(bmp)
        'DrawColorWheel(gbmp, Math.Min(ClientSize.Width, ClientSize.Height) \ 2 - 20, New Point(ClientSize.Width \ 2, ClientSize.Height \ 2), 1440)



        'End If

        Invalidate() ' Redraw the form when resized
    End Sub
End Class
