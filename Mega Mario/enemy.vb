Public Class enemy
    Public enemy As New PictureBox

    Public Sub New(enemyType As String)
        If enemyType = "goomba" Then
            enemy.Height = 20
            enemy.Width = 18
            enemy.Image = Image.FromFile(Application.StartupPath + "\Graphics\goomba0.png")
            enemy.Top = 274
            enemy.Left = 500
            enemy.Tag = "goomba"
        ElseIf enemyType = "bobomb" Then
            enemy.Height = 20
            enemy.Width = 18
            enemy.Image = Image.FromFile(Application.StartupPath + "\Graphics\bobomb0.png")
            enemy.Top = 277
            enemy.Left = 500
            enemy.Tag = "bobomb"
        ElseIf enemyType = "koopa" Then
            enemy.Height = 50
            enemy.Width = 40
            enemy.Image = Image.FromFile(Application.StartupPath + "\Graphics\koopa0.png")
            enemy.Top = 100
            enemy.Left = 500
            enemy.Tag = "koopa"
        ElseIf enemyType = "shell" Then
            enemy.Height = 17
            enemy.Width = 17
            enemy.Image = Image.FromFile(Application.StartupPath + "\Graphics\shell0.png")
            enemy.Top = 280
            enemy.Left = 500
            enemy.Tag = "shell"
        End If
    End Sub
End Class
