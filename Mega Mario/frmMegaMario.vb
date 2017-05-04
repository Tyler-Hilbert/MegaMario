Public Class frmMegaMario
    Dim marioPosition, koopaPosition, fireBallCount, hasFire, coinPosition As Integer
    Dim MARIOBOTTOM As Integer = 261
    Dim score As Long
    Dim jump, dJump, playing, dying, firstSong As Boolean
    Public lstEnemy As New List(Of enemy)
    Dim lblName(10), lblScores(10) As Label
    Dim pbxCloud(10) As PictureBox
    Dim picCoin(4) As PictureBox


    Private Sub frmMegaMario_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        My.Computer.Audio.Play("mario theme song.wav", AudioPlayMode.BackgroundLoop)
        lblPlay.Left = Me.Width / 2 - lblPlay.Width / 2

        createControlArray()
        createEnvironment()

        picMario.SendToBack()
        lblPlay.SendToBack()
    End Sub

    Private Sub createControlArray()
        lblName(1) = Label1
        lblName(2) = Label2
        lblName(3) = Label3
        lblName(4) = Label4
        lblName(5) = Label5
        lblName(6) = Label6
        lblName(7) = Label7
        lblName(8) = Label8
        lblName(9) = Label9
        lblName(10) = Label10

        lblScores(1) = Label20
        lblScores(2) = Label19
        lblScores(3) = Label18
        lblScores(4) = Label17
        lblScores(5) = Label16
        lblScores(6) = Label15
        lblScores(7) = Label14
        lblScores(8) = Label13
        lblScores(9) = Label12
        lblScores(10) = Label11
    End Sub

    Private Sub createEnvironment()
        Dim i, j As Integer

        For j = 0 To 1 'Floor
            For i = 0 To Me.Width / 19
                Dim pbx As New PictureBox
                pbx.Image = Image.FromFile(Application.StartupPath + "\Graphics\floor.png")
                pbx.Visible = True
                pbx.Top = (Me.Height - 67) + (j * 10)
                pbx.Left = 20 * i
                pbx.Width = 20
                pbx.Height = 20
                Controls.Add(pbx)
            Next
        Next

        For i = 1 To 3 'Clouds
            pbxCloud(i) = New PictureBox
            pbxCloud(i).Image = Image.FromFile(Application.StartupPath + "\Graphics\cloud.png")
            pbxCloud(i).Visible = True
            pbxCloud(i).Top = 50
            pbxCloud(i).Left = (200 * i) - 120
            pbxCloud(i).Width = 65
            pbxCloud(i).Height = 25
            Controls.Add(pbxCloud(i))
        Next

        For i = 1 To 4 'Create coins
            picCoin(i) = New PictureBox
            picCoin(i).Image = Image.FromFile(Application.StartupPath + "\Graphics\coin0.png")
            picCoin(i).Visible = True
            picCoin(i).Top = 255
            picCoin(i).Left = 5000 + (i * 100)
            picCoin(i).Width = 15
            picCoin(i).Height = 19
            Controls.Add(picCoin(i))
        Next
    End Sub



    Private Sub frmMegaMario_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If playing = False Then
            startGame()
        ElseIf e.KeyCode = Keys.Up And playing = True And dying = False Then 'Jump
            If jump = False Then
                jump = True
                marioPosition = 0
            ElseIf dJump = False Then
                dJump = True
                marioPosition = 0
            End If
        ElseIf e.KeyCode = Keys.Space And playing = True And dying = False And hasFire = 10 Then 'Fire ball
            fireBallCount = 0
            hasFire = 0
            picFireBall.Top = 10
            picFireBall.Left = Me.Width / 2 - picFireBall.Width / 2
            tmrFireBall.Enabled = True
        End If
    End Sub

    Private Sub tmrRandomCoin_Tick(sender As Object, e As EventArgs) Handles tmrRandomCoin.Tick
        Dim i, coinTop As Integer
        Randomize()

        If Int(Rnd() * 4) = 0 Then
            If Int(Rnd() * 2) = 0 Then
                coinTop = 255
            Else
                coinTop = 100
            End If

            For i = 1 To 4
                picCoin(i).Image = Image.FromFile(Application.StartupPath + "\Graphics\coin0.png")
                picCoin(i).Top = coinTop
                picCoin(i).Left = 500 + (i * 100)
            Next

            tmrCoinMove.Enabled = True
            tmrRandomCoin.Enabled = False
            coinPosition = 0
        End If
    End Sub

    Private Sub tmrCoinMove_Tick(sender As Object, e As EventArgs) Handles tmrCoinMove.Tick
        Dim i As Integer

        If coinPosition < 4 Then
            coinPosition += 1
        Else
            coinPosition = 0
        End If

        For i = 1 To 4
            picCoin(i).Image = Image.FromFile(Application.StartupPath + "\Graphics\coin" + Trim(Str(coinPosition)) + ".png")
            picCoin(i).Left -= 20
            If Me.picMario.Bounds.IntersectsWith(Me.picCoin(i).Bounds) Then 'Collision
                score += 50
                picCoin(i).Left = -10
            End If
        Next

        If picCoin(4).Left < -10 Then
            For i = 1 To 4
                picCoin(i).Left = 5000
            Next
            tmrCoinMove.Enabled = False
            tmrRandomCoin.Enabled = True
        End If
    End Sub

    Private Sub tmrMario_Tick(sender As Object, e As EventArgs) Handles tmrMario.Tick 'Mario movment
        Dim JUMPDISTANCE As Integer = 150
        If jump = True Then 'Jump or double jump
            If dJump = True Then 'Double jump
                Select Case marioPosition
                    Case 0
                        picMario.Image = Image.FromFile(Application.StartupPath + "\Graphics\run0.png")
                    Case 1
                        picMario.Image = Image.FromFile(Application.StartupPath + "\Graphics\jump0.png")
                        picMario.Top = picMario.Top - JUMPDISTANCE / 4
                    Case 2 And 3
                        picMario.Image = Image.FromFile(Application.StartupPath + "\Graphics\jump1.png")
                    Case 4
                        picMario.Image = Image.FromFile(Application.StartupPath + "\Graphics\jump0.png")
                        picMario.Top = picMario.Top + JUMPDISTANCE
                    Case Else
                        picMario.Image = Image.FromFile(Application.StartupPath + "\Graphics\run0.png")
                        picMario.Top = MARIOBOTTOM
                        dJump = False
                        jump = False
                End Select
            Else 'Jump
                Select Case marioPosition
                    Case 0
                        picMario.Image = Image.FromFile(Application.StartupPath + "\Graphics\run0.png")
                    Case 1
                        picMario.Image = Image.FromFile(Application.StartupPath + "\Graphics\jump0.png")
                        picMario.Top = picMario.Top - JUMPDISTANCE
                    Case 2
                        picMario.Image = Image.FromFile(Application.StartupPath + "\Graphics\jump1.png")
                    Case 3
                        picMario.Image = Image.FromFile(Application.StartupPath + "\Graphics\jump0.png")
                        picMario.Top = MARIOBOTTOM
                        marioPosition = 0
                        jump = False
                End Select
            End If


        ElseIf dying = True Then 'Dying
            Select Case marioPosition
                Case 0 To 2
                    picMario.Image = Image.FromFile(Application.StartupPath + "\Graphics\die0.png")
                Case 3 To 5
                    picMario.Top -= 20
                Case Else
                    If picMario.Top < Me.Height Then
                        picMario.Top += 50
                    Else
                        tmrMario.Enabled = False
                        dying = False
                        playing = False
                        Call highScore()
                    End If
            End Select


        Else 'Running
            If marioPosition < 5 And marioPosition >= 0 Then
            ElseIf marioPosition = 5 Then
                marioPosition = 0
            End If

            picMario.Image = Image.FromFile(Application.StartupPath + "\Graphics\run" + Trim(Str(marioPosition)) + ".png")
        End If

        marioPosition += 1
    End Sub

    Private Sub tmrFireBall_Tick(sender As Object, e As EventArgs) Handles tmrFireBall.Tick
        fireBallCount += 1
        picFireBall.Left += 20

        If fireBallCount = 1 Then
            picFireBall.Visible = True
            picFireBall.Top = picMario.Top + 5
            picFireBall.Left = picMario.Left + 20
        ElseIf fireBallCount = 8 Then
            tmrFireBall.Enabled = False
            picFireBall.Width = 0
            picFireBall.Top = 10
            picFireBall.Left = Me.Width / 2 - 15
            tmrLoadFire.Enabled = True
            Exit Sub
        End If

        For Each enemy As enemy In lstEnemy 'Check collision
            If Me.picFireBall.Bounds.IntersectsWith(enemy.enemy.Bounds) Then
                picFireBall.Width = 0
                picFireBall.Top = 10
                picFireBall.Left = Me.Width / 2 - 15
                tmrFireBall.Enabled = False
                tmrLoadFire.Enabled = True
                Me.Controls.Remove(enemy.enemy)
                enemy.enemy.Left = -10
            End If
        Next
        If fireBallCount Mod 2 = 0 Then
            picFireBall.Image = Image.FromFile(Application.StartupPath + "\Graphics\fireBall0.png")
        Else
            picFireBall.Image = Image.FromFile(Application.StartupPath + "\Graphics\fireBall1.png")
        End If
    End Sub

    Private Sub tmrLoadFire_Tick(sender As Object, e As EventArgs) Handles tmrLoadFire.Tick
        hasFire += 3
        picFireBall.Width = hasFire
        If hasFire = 10 Then
            hasFire = True
            tmrLoadFire.Enabled = False
        End If
    End Sub

    Private Sub tmrEnemyMove_Tick(sender As Object, e As EventArgs) Handles tmrEnemyMove.Tick
        Dim koopaAnimation As Boolean

        For Each enemy As enemy In lstEnemy
            enemy.enemy.Left = enemy.enemy.Left - 20

            If enemy.enemy.Tag = "koopa" Then 'Koopa
                enemy.enemy.Image = Image.FromFile(Application.StartupPath + "\Graphics\koopa" + Trim(Str(koopaPosition)) + ".png")
                If koopaAnimation = False Then
                    If koopaPosition = 0 Then
                        koopaPosition = 1
                    Else
                        koopaPosition = 0
                    End If
                End If
                koopaAnimation = True
            End If

            If enemy.enemy.Tag = "shell" Then
                enemy.enemy.Left -= 10
            End If

            If Me.picMario.Bounds.IntersectsWith(enemy.enemy.Bounds) Then 'Collision
                endGame()
            End If

            If enemy.enemy.Left < -5 Then
                Me.Controls.Remove(enemy.enemy)
            End If
        Next
    End Sub

    Private Sub tmrRandomEnemy_Tick(sender As Object, e As EventArgs) Handles tmrRandomEnemy.Tick
        Dim enemyType As String
        Dim enemyNum As Integer
        Randomize()

        enemyNum = Int(Rnd() * 4)
        If enemyNum = 0 Then
            enemyType = "goomba"
            Dim goomba As enemy = New enemy(enemyType)
            Me.Controls.Add(goomba.enemy)
            lstEnemy.Add(goomba)
        ElseIf enemyNum = 1 Then
            enemyType = "koopa"
            Dim koopa As enemy = New enemy(enemyType)
            Me.Controls.Add(koopa.enemy)
            lstEnemy.Add(koopa)
        ElseIf enemyNum = 2 Then
            enemyType = "bobomb"
            Dim bobomb As enemy = New enemy(enemyType)
            Me.Controls.Add(bobomb.enemy)
            lstEnemy.Add(bobomb)
        ElseIf enemyNum = 3 Then
            enemyType = "shell"
            Dim shell As enemy = New enemy(enemyType)
            Me.Controls.Add(shell.enemy)
            lstEnemy.Add(shell)
        End If

        tmrRandomEnemy.Interval = Int(Rnd() * 1000) + 1000
    End Sub

    Private Sub tmrScore_Tick(sender As Object, e As EventArgs) Handles tmrScore.Tick
        score = (score + 1) + Int(score / 100)
        lblScore.Text = score
    End Sub

    Private Sub tmrBackground_Tick(sender As Object, e As EventArgs) Handles tmrBackground.Tick
        For i = 1 To 3
            pbxCloud(i).Left -= 1
            If pbxCloud(i).Left < -50 Then
                pbxCloud(i).Left = Me.Width
            End If
        Next
    End Sub




    Private Sub startGame()
        Dim i As Integer

        For i = 0 To lstEnemy.Count - 1
            Me.Controls.Remove(lstEnemy(i).enemy)
        Next
        lstEnemy.Clear()

        marioPosition = 0
        score = 0
        tmrMario.Enabled = True
        tmrEnemyMove.Enabled = True
        tmrRandomEnemy.Enabled = True
        tmrScore.Enabled = True
        tmrRandomCoin.Enabled = True
        playing = True

        picMario.Image = Image.FromFile(Application.StartupPath + "\Graphics\run0.png")
        picMario.Visible = True
        picMario.Top = MARIOBOTTOM

        picFireBall.Image = Image.FromFile(Application.StartupPath + "\Graphics\fireBall0.png")
        picFireBall.Top = 10
        picFireBall.Left = Me.Width / 2 - 15
        picFireBall.Width = 30
        hasFire = 10
        tmrLoadFire.Enabled = False

        For i = 1 To 4
            picCoin(i).Left = 5000
        Next

        lblPlay.Visible = False
        For i = 1 To 10
            lblScores(i).Visible = False
            lblName(i).Visible = False
        Next

        If firstSong = True Then
            My.Computer.Audio.Play("mario theme song.wav", AudioPlayMode.BackgroundLoop)
        Else
            firstSong = True
        End If
    End Sub

    Private Sub endGame()
        My.Computer.Audio.Play("loose.wav")

        jump = False
        dJump = False
        dying = True
        marioPosition = 0

        tmrEnemyMove.Enabled = False
        tmrRandomEnemy.Enabled = False
        tmrScore.Enabled = False
        tmrRandomCoin.Enabled = False
        tmrCoinMove.Enabled = False

        lblPlay.Visible = True
        lblPlay.SendToBack()
    End Sub

    Public Sub highScore()
        Dim opentxt As System.IO.StreamReader
        Dim i, front, back As Byte
        Dim temp, arrScores(10) As scores
        Dim write As String

        opentxt = IO.File.OpenText(Application.StartupPath + "\hs.txt") 'Opens high scores
        While opentxt.Peek <> -1 And i < 20
            Dim strline As String = Trim(opentxt.ReadLine())
            If i Mod 2 = 0 Then
                arrScores((i / 2) + 1).name = strline
            Else
                arrScores(((i - 1) / 2) + 1).score = Val(strline)
            End If
            i += 1
        End While
        opentxt.Close()

        If score > arrScores(10).score Then 'Checks if you scored a high score
            arrScores(10).name = InputBox("Enter your name")
            arrScores(10).score = score
        End If

        For front = 1 To 9 'Orders Highscores
            For back = front + 1 To 10
                If arrScores(front).score < arrScores(back).score Then
                    temp = arrScores(front)
                    arrScores(front) = arrScores(back)
                    arrScores(back) = temp
                End If
            Next
        Next

        For i = 1 To 10  'Outputs Score
            lblScores(i).Visible = True
            lblScores(i).Text = arrScores(i).score
            lblName(i).Visible = True
            lblName(i).Text = arrScores(i).name
        Next

        For i = 1 To 10
            lblScores(i).SendToBack()
            lblName(i).SendToBack()
        Next

        Dim oFile As System.IO.File 'Saves highscore
        Dim oWrite As System.IO.StreamWriter
        oWrite = oFile.CreateText(Application.StartupPath + "\hs.txt")
        For i = 1 To 10
            oWrite.WriteLine(arrScores(i).name)
            oWrite.WriteLine(arrScores(i).score)
        Next
        oWrite.Close()
    End Sub
End Class