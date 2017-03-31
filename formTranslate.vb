Public Class formTranslate
    Private Sub formTranslate_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        btnClose.Focus()
    End Sub

    Private Sub formTranslate_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'ładowany jest obraz z pliku zewnętrznego (grafika.jpg)
        Dim plikSplash As String = System.AppDomain.CurrentDomain.BaseDirectory() & "\grafika.jpg"

        If System.IO.File.Exists(plikSplash) = True Then
            Dim splashImage As Image = New Bitmap(plikSplash)
            pbGrafika.Image = splashImage
        End If

        lblKIerunekTlumaczenia.Text = formMain.translationLang
        txtTlumaczenie.Text = formMain.translation
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Clipboard.Clear()
        Me.Close()
    End Sub
End Class