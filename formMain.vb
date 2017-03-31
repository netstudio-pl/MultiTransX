Imports System.Net
Imports System.Text
Imports MultiTransX.TranslatorForAzure
Imports System.IO
Imports System.Xml

Public Class formMain
    ' virtual key codes: https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx
    ' dokumentacja Microsoft Translator: http://docs.microsofttranslator.com/text-translate.html
    ' rejestracja usługi: https://datamarket.azure.com/developer/applications/

    Private WithEvents MainMenu As ContextMenuStrip
    Private WithEvents mnuIE As ToolStripMenuItem
    Private WithEvents mnuFirefox As ToolStripMenuItem
    Private WithEvents mnuChrome As ToolStripMenuItem
    Private WithEvents mnuAPI As ToolStripMenuItem
    Private WithEvents mnuExit As ToolStripMenuItem
    Private WithEvents Tray As NotifyIcon

    Public IEPath As String
    Public FirefoxPath As String
    Public ChromePath As String
    Public DefaultTranslateMethod As String

    Private Declare Function GetForegroundWindow Lib "User32" () As IntPtr
    Private Declare Function RegisterHotKey Lib "User32.dll" (ByVal hWnd As IntPtr, ByVal ID As Integer, ByVal Modifiers As UInteger, ByVal VK As UInteger) As Boolean
    Private Declare Function UnregisterHotKey Lib "User32.dll" (ByVal hWnd As IntPtr, ByVal ID As Integer) As Boolean
    Private Const WM_HOTKEY As Integer = &H312
    Private Const MOD_CONTROL As UInteger = 2
    Private Const VK_F9 As UInteger = &H78
    Private Const VK_F10 As UInteger = &H79
    Private Const VK_F11 As UInteger = &H7A
    Private Const VK_F12 As UInteger = &H7B

    Public translation As String
    Public translationLang As String
    Public clientID As String = "WindowsMultiTranslator_v1"
    Public clientSecret As String = "W6Ab4V3/kFwOSw7rFHqL70GGYdj42SwPV54g6EipidY="

    Public Sub StartApp()
        mnuIE = New ToolStripMenuItem("Internet Explorer")
        mnuIE.CheckOnClick = True
        mnuFirefox = New ToolStripMenuItem("Mozilla Firefox")
        mnuFirefox.CheckOnClick = True
        mnuChrome = New ToolStripMenuItem("Google Chrome")
        mnuChrome.CheckOnClick = True
        mnuAPI = New ToolStripMenuItem("Tłumacz wbudowany")
        mnuAPI.CheckOnClick = True
        mnuExit = New ToolStripMenuItem("Zamknij program")
        MainMenu = New ContextMenuStrip
        MainMenu.Items.AddRange(New ToolStripItem() {mnuIE, mnuFirefox, mnuChrome, mnuAPI, New ToolStripSeparator, mnuExit})

        pobierz_konfiguracje() 'pobiera zapisany rodzaj tłumaczenia i ścieżki do przeglądarek

        Tray = New NotifyIcon
        Tray.Icon = My.Resources.mtx_icon
        Tray.ContextMenuStrip = MainMenu
        Tray.Text = "MultiTransX"
        Tray.Visible = True

        RegisterHotKey(Me.Handle, 1, MOD_CONTROL, VK_F9)
        RegisterHotKey(Me.Handle, 2, MOD_CONTROL, VK_F10)
        RegisterHotKey(Me.Handle, 3, MOD_CONTROL, VK_F11)
        RegisterHotKey(Me.Handle, 4, MOD_CONTROL, VK_F12)
    End Sub

    Private Sub mnuIE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuIE.Click
        mnuIE.Checked = True
        mnuFirefox.Checked = False
        mnuChrome.Checked = False
        mnuAPI.Checked = False
        DefaultTranslateMethod = "IE"
    End Sub

    Private Sub mnuFirefox_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuFirefox.Click
        mnuIE.Checked = False
        mnuFirefox.Checked = True
        mnuChrome.Checked = False
        mnuAPI.Checked = False
        DefaultTranslateMethod = "Firefox"
    End Sub

    Private Sub mnuChrome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuChrome.Click
        mnuIE.Checked = False
        mnuFirefox.Checked = False
        mnuChrome.Checked = True
        mnuAPI.Checked = False
        DefaultTranslateMethod = "Chrome"
    End Sub

    Private Sub mnuAPI_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuAPI.Click
        mnuIE.Checked = False
        mnuFirefox.Checked = False
        mnuChrome.Checked = False
        mnuAPI.Checked = True
        DefaultTranslateMethod = "API"
    End Sub

    Private Sub mnuExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        UnregisterHotKey(Nothing, 1)
        UnregisterHotKey(Nothing, 2)
        UnregisterHotKey(Nothing, 3)
        UnregisterHotKey(Nothing, 4)
        zapisz_konfiguracje()
        Application.Exit()
    End Sub

    Private Sub pobierz_konfiguracje()
        'funkcja pobierająca konfigurację programu
        Dim objFile As StreamReader
        Dim objXml As XmlDocument
        objFile = File.OpenText("app.config.xml")
        objXml = New XmlDocument()
        objXml.LoadXml(objFile.ReadToEnd())

        IEPath = objXml.SelectSingleNode("//IEPath").InnerText
        FirefoxPath = objXml.SelectSingleNode("//FirefoxPath").InnerText
        ChromePath = objXml.SelectSingleNode("//ChromePath").InnerText
        DefaultTranslateMethod = objXml.SelectSingleNode("//DefaultTranslateMethod").InnerText

        'ustawia zapisany wcześniej rodzaj tłumaczenia
        Select Case DefaultTranslateMethod
            Case "IE"
                mnuIE.Checked = True
            Case "Firefox"
                mnuFirefox.Checked = True
            Case "Chrome"
                mnuChrome.Checked = True
            Case "API"
                mnuAPI.Checked = True
        End Select

        objFile.Dispose()
        objFile.Close()
    End Sub

    Private Sub zapisz_konfiguracje()
        'funkcja zapisująca konfigurację programu
        Dim objXml As New XmlDocument()
        objXml.Load("app.config.xml")
        Dim defTranslate As XmlNode = objXml.SelectSingleNode("//DefaultTranslateMethod")
        defTranslate.ChildNodes(0).InnerText = DefaultTranslateMethod
        objXml.Save("app.config.xml")
    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = WM_HOTKEY Then
            Dim Modif As UInteger = (CType(m.LParam, Integer) And &HFFFF)
            Dim Key As UInteger = CType(m.LParam, Integer) >> 16

            If Key = VK_F9 And Modif = MOD_CONTROL Then 'holenderski
                If GetForegroundWindow <> IntPtr.Zero Then
                    'Send the Ctrl+C command to the active window  
                    SendKeys.SendWait("^c")
                    Try
                        Select Case DefaultTranslateMethod
                            Case "IE"
                                Dim processStartInfo As System.Diagnostics.ProcessStartInfo
                                processStartInfo = New System.Diagnostics.ProcessStartInfo()
                                processStartInfo.FileName = IEPath
                                processStartInfo.Arguments = " https://translate.google.pl/#nl/pl/" & Clipboard.GetText.Replace(" ", "%20")
                                Dim process As System.Diagnostics.Process = System.Diagnostics.Process.Start(processStartInfo)
                            Case "Firefox"
                                Dim processStartInfo As System.Diagnostics.ProcessStartInfo
                                processStartInfo = New System.Diagnostics.ProcessStartInfo()
                                processStartInfo.FileName = FirefoxPath
                                processStartInfo.Arguments = " https://translate.google.pl/#nl/pl/" & Clipboard.GetText.Replace(" ", "%20")
                                Dim process As System.Diagnostics.Process = System.Diagnostics.Process.Start(processStartInfo)
                            Case "Chrome"
                                Dim processStartInfo As System.Diagnostics.ProcessStartInfo
                                processStartInfo = New System.Diagnostics.ProcessStartInfo()
                                processStartInfo.FileName = ChromePath
                                processStartInfo.Arguments = " https://translate.google.pl/#nl/pl/" & Clipboard.GetText.Replace(" ", "%20")
                                Dim process As System.Diagnostics.Process = System.Diagnostics.Process.Start(processStartInfo)
                            Case "API"
                                translationLang = "NL -> PL"
                                translateAPI(Clipboard.GetText, "nl")
                        End Select
                    Catch ex As Exception
                        MsgBox("Wybrana przeglądarka jest niedostępna", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Błąd programu")
                    End Try
                End If
            End If

            If Key = VK_F10 And Modif = MOD_CONTROL Then 'francuski
                If GetForegroundWindow <> IntPtr.Zero Then
                    'Send the Ctrl+C command to the active window  
                    SendKeys.SendWait("^c")
                    Try
                        Select Case DefaultTranslateMethod
                            Case "IE"
                                Dim processStartInfo As System.Diagnostics.ProcessStartInfo
                                processStartInfo = New System.Diagnostics.ProcessStartInfo()
                                processStartInfo.FileName = IEPath
                                processStartInfo.Arguments = " https://translate.google.pl/#fr/pl/" & Clipboard.GetText.Replace(" ", "%20")
                                Dim process As System.Diagnostics.Process = System.Diagnostics.Process.Start(processStartInfo)
                            Case "Firefox"
                                Dim processStartInfo As System.Diagnostics.ProcessStartInfo
                                processStartInfo = New System.Diagnostics.ProcessStartInfo()
                                processStartInfo.FileName = FirefoxPath
                                processStartInfo.Arguments = " https://translate.google.pl/#fr/pl/" & Clipboard.GetText.Replace(" ", "%20")
                                Dim process As System.Diagnostics.Process = System.Diagnostics.Process.Start(processStartInfo)
                            Case "Chrome"
                                Dim processStartInfo As System.Diagnostics.ProcessStartInfo
                                processStartInfo = New System.Diagnostics.ProcessStartInfo()
                                processStartInfo.FileName = ChromePath
                                processStartInfo.Arguments = " https://translate.google.pl/#fr/pl/" & Clipboard.GetText.Replace(" ", "%20")
                                Dim process As System.Diagnostics.Process = System.Diagnostics.Process.Start(processStartInfo)
                            Case "API"
                                translationLang = "FR -> PL"
                                translateAPI(Clipboard.GetText, "fr")
                        End Select
                    Catch ex As Exception
                        MsgBox("Wybrana przeglądarka jest niedostępna", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Błąd programu")
                    End Try
                End If
            End If

            If Key = VK_F11 And Modif = MOD_CONTROL Then 'angielski
                If GetForegroundWindow <> IntPtr.Zero Then
                    'Send the Ctrl+C command to the active window  
                    SendKeys.SendWait("^c")
                    Try
                        Select Case DefaultTranslateMethod
                            Case "IE"
                                Dim processStartInfo As System.Diagnostics.ProcessStartInfo
                                processStartInfo = New System.Diagnostics.ProcessStartInfo()
                                processStartInfo.FileName = IEPath
                                processStartInfo.Arguments = " https://translate.google.pl/#en/pl/" & Clipboard.GetText.Replace(" ", "%20")
                                Dim process As System.Diagnostics.Process = System.Diagnostics.Process.Start(processStartInfo)
                            Case "Firefox"
                                Dim processStartInfo As System.Diagnostics.ProcessStartInfo
                                processStartInfo = New System.Diagnostics.ProcessStartInfo()
                                processStartInfo.FileName = FirefoxPath
                                processStartInfo.Arguments = " https://translate.google.pl/#en/pl/" & Clipboard.GetText.Replace(" ", "%20")
                                Dim process As System.Diagnostics.Process = System.Diagnostics.Process.Start(processStartInfo)
                            Case "Chrome"
                                Dim processStartInfo As System.Diagnostics.ProcessStartInfo
                                processStartInfo = New System.Diagnostics.ProcessStartInfo()
                                processStartInfo.FileName = ChromePath
                                processStartInfo.Arguments = " https://translate.google.pl/#en/pl/" & Clipboard.GetText.Replace(" ", "%20")
                                Dim process As System.Diagnostics.Process = System.Diagnostics.Process.Start(processStartInfo)
                            Case "API"
                                translationLang = "EN -> PL"
                                translateAPI(Clipboard.GetText, "en")
                        End Select
                    Catch ex As Exception
                        MsgBox("Wybrana przeglądarka jest niedostępna", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Błąd programu")
                    End Try

                End If
            End If

            If Key = VK_F12 And Modif = MOD_CONTROL Then 'niemiecki
                If GetForegroundWindow <> IntPtr.Zero Then
                    'Send the Ctrl+C command to the active window  
                    SendKeys.SendWait("^c")
                    Try
                        Select Case DefaultTranslateMethod
                            Case "IE"
                                Dim processStartInfo As System.Diagnostics.ProcessStartInfo
                                processStartInfo = New System.Diagnostics.ProcessStartInfo()
                                processStartInfo.FileName = IEPath
                                processStartInfo.Arguments = " https://translate.google.pl/#de/pl/" & Clipboard.GetText.Replace(" ", "%20")
                                Dim process As System.Diagnostics.Process = System.Diagnostics.Process.Start(processStartInfo)
                            Case "Firefox"
                                Dim processStartInfo As System.Diagnostics.ProcessStartInfo
                                processStartInfo = New System.Diagnostics.ProcessStartInfo()
                                processStartInfo.FileName = FirefoxPath
                                processStartInfo.Arguments = " https://translate.google.pl/#de/pl/" & Clipboard.GetText.Replace(" ", "%20")
                                Dim process As System.Diagnostics.Process = System.Diagnostics.Process.Start(processStartInfo)
                            Case "Chrome"
                                Dim processStartInfo As System.Diagnostics.ProcessStartInfo
                                processStartInfo = New System.Diagnostics.ProcessStartInfo()
                                processStartInfo.FileName = ChromePath
                                processStartInfo.Arguments = " https://translate.google.pl/#de/pl/" & Clipboard.GetText.Replace(" ", "%20")
                                Dim process As System.Diagnostics.Process = System.Diagnostics.Process.Start(processStartInfo)
                            Case "API"
                                translationLang = "DE -> PL"
                                translateAPI(Clipboard.GetText, "de")
                        End Select
                    Catch ex As Exception
                        MsgBox("Wybrana przeglądarka jest niedostępna", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Błąd programu")
                    End Try
                End If
            End If
        Else
            MyBase.WndProc(m)
        End If
    End Sub

    Public Function translateAPI(ByVal textToTranslate As String, ByVal lang As String) As Boolean
        'funkcja tłumacząca za pomocą API (Microsoft Translator)
        If textToTranslate.Length = 0 Then
            MsgBox("Nie zaznaczono tekstu do tłumaczenia", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Błąd programu")
        Else
            'Obtaining an access token
            Dim authentication As New AdmAuthentication(clientID, clientSecret)
            Dim headerValue As String
            Dim token As AdmAccessToken = authentication.GetAccessToken()
            headerValue = "Bearer " + token.access_token

            'Add access token to header
            Dim uri As String = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + System.Web.HttpUtility.UrlEncode(textToTranslate) + "&from=" + lang + "&to=pl"
            Dim translationWebRequest As WebRequest = WebRequest.Create(uri)
            translationWebRequest.Headers.Add("Authorization", headerValue)

            'Call API method 
            Dim response As WebResponse = Nothing
            response = translationWebRequest.GetResponse()
            Dim stream As IO.Stream = response.GetResponseStream()
            Dim encode As Encoding = System.Text.Encoding.GetEncoding("utf-8")
            Dim translatedStream As New System.IO.StreamReader(stream, encode)
            Dim xTranslation As New System.Xml.XmlDocument()
            xTranslation.LoadXml(translatedStream.ReadToEnd())
            translation = xTranslation.InnerText
            formTranslate.ShowDialog()
        End If
        Return True
    End Function
End Class