'---------------------------------------------------------------------------------------------------------
'  Console application to store/retrieve/delete registry key with user defined server details
'---------------------------------------------------------------------------------------------------------

Imports System.Net.Dns
Imports Microsoft.Win32

Module Program

#Region "Variable Declaration"

    Private Ops As Integer
    Private Ip, Port, DB, User, Pass As String

    ' Assigning DNS details
    Private hostName = System.Net.Dns.GetHostName()

    ' The name of the key must include a valid root.
    Const userRoot As String = "HKEY_CURRENT_USER"
    Const subkey As String = "Environment\ServerConfig"
    Const keyName As String = userRoot & "\" & subkey
    Private exportPath As String = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\ServerREG.reg"
    Private proc As Process = New Process()

    ' Encryption
    Const EncryptionKey As String = "SECRET"
    Private wrapper As New _3DES(EncryptionKey)

#End Region

#Region "Method Declaration"
    Sub Main(args As String())

Menu:
        Console.WriteLine("|------------------------------|" + vbCrLf + "|--- Welcome to IP Registry ---|" + vbCrLf + "|------------------------------|" + vbCrLf)
        Console.WriteLine("Press [1] : View current hostname and ipv4 address" + vbCrLf + "Press [2] : Set current local ip to the registry for application's use" + vbCrLf + "Press [3] : Set server details to the registry for application's use" + vbCrLf + "Press [4] : View current server details settings saved in registry" + vbCrLf + "Press [5] : Delete current server details saved in registry" + vbCrLf + "Press [6] : Export server details registry to .REG file" + vbCrLf + "Press [7] : For exit")

        Do Until Ops = 7
            Try
                Ops = Console.ReadLine
                Select Case Ops
                    Case "1"
                        GoTo Option1
                    Case "2"
                        GoTo Option2
                    Case "3"
                        GoTo Option3
                    Case "4"
                        GoTo Option4
                    Case "5"
                        GoTo Option5
                    Case "6"
                        GoTo Option6
                    Case "7"
                        GoTo Option7
                End Select
            Catch ex As Exception
                Console.WriteLine("Wrong Input!")
                Console.ReadLine()
                Console.Clear()
                GoTo Menu
            End Try

        Loop
Option1:
        Try
            For Each hostAdr In System.Net.Dns.GetHostEntry(hostName).AddressList()
                ' If you just want to write every internal IP address
                Console.WriteLine("|-----------------------------------|")
                Console.WriteLine("| Hostname:      | " & hostName)
                Console.WriteLine("|-----------------------------------|")
                Console.WriteLine("| IPv4 Address:  | " & hostAdr.ToString())
                Console.WriteLine("|-----------------------------------|")
            Next
            Console.ReadLine()
            Console.Clear()
            GoTo Menu
        Catch ex As Exception
            Console.WriteLine("Something Wrong!, Contact Developer.")
        End Try
Option2:
        Try
            Registry.SetValue(keyName, "IP", GetHostEntry(hostName).AddressList(1).ToString())
            Console.WriteLine("IP updated successfully!")
            Console.ReadLine()
            Console.Clear()
            GoTo Menu
        Catch ex As Exception
            Console.WriteLine("Something Wrong!, Contact Developer.")
        End Try
Option3:
        Try
            Console.WriteLine("Enter an IP Address:")
            Ip = Console.ReadLine
            Registry.SetValue(keyName, "IP", Ip)
            Console.WriteLine("Enter a Port:")
            Port = Console.ReadLine()
            Registry.SetValue(keyName, "PORT", Port)
            Console.WriteLine("Enter a Database:")
            DB = Console.ReadLine()
            Registry.SetValue(keyName, "DATABASE", DB)
            Console.WriteLine("Enter a DB User:")
            User = Console.ReadLine()
            Registry.SetValue(keyName, "USER", User)
            Console.WriteLine("Enter a DB Password:")
            Pass = wrapper.EncryptData(Console.ReadLine())
            Registry.SetValue(keyName, "PASS", Pass)
            Console.WriteLine("Registry created successfully!")
            'Console.WriteLine(Registry.GetValue(keyName, "IP", Nothing))
            Console.ReadLine()
            Console.Clear()
            GoTo Menu
        Catch ex As Exception
            Console.WriteLine("Something Wrong!, Contact Developer.")
        End Try
Option4:
        Try
            Console.WriteLine("|-----------------------------------|")
            Console.WriteLine("| IPv4 Address: | " & Registry.GetValue(keyName, "IP", Nothing))
            Console.WriteLine("|-----------------------------------|")
            Console.WriteLine("| Port:         | " & Registry.GetValue(keyName, "PORT", Nothing))
            Console.WriteLine("|-----------------------------------|")
            Console.WriteLine("| Database:     | " & Registry.GetValue(keyName, "DATABASE", Nothing))
            Console.WriteLine("|-----------------------------------|")
            Console.WriteLine("| DB User:      | " & Registry.GetValue(keyName, "USER", Nothing))
            Console.WriteLine("|-----------------------------------|")
            Console.WriteLine("| Password:     | " & Registry.GetValue(keyName, "PASS", Nothing))
            Console.WriteLine("|-----------------------------------|")
            Console.ReadLine()
            Console.Clear()
            GoTo Menu
        Catch ex As Exception
            Console.WriteLine("Something Wrong!, Contact Developer.")
        End Try
Option5:
        Try
            Registry.CurrentUser.DeleteSubKeyTree(subkey)
            Console.WriteLine("Registry deleted successfully!")
            Console.ReadLine()
            Console.Clear()
            GoTo Menu
        Catch ex As Exception
            Console.WriteLine("Something Wrong!, Contact Developer.")
        End Try
Option6:
        Try
            proc.StartInfo.FileName = "reg.exe"
            proc.StartInfo.UseShellExecute = False
            proc.StartInfo.RedirectStandardOutput = True
            proc.StartInfo.RedirectStandardError = True
            proc.StartInfo.CreateNoWindow = True
            proc.StartInfo.Arguments = "export """ & keyName & """ """ + exportPath & """ /y"
            proc.Start()
            proc.StandardOutput.ReadToEnd()
            proc.StandardError.ReadToEnd()
            proc.WaitForExit()
            Console.WriteLine("Registry exported successfully!")
            Console.ReadLine()
            Console.Clear()
            GoTo Menu
        Catch ex As Exception
            Console.WriteLine("Something Wrong!, Contact Developer.")
        Finally
            proc.Dispose()
        End Try
Option7:
        Try
            Console.WriteLine("Press enter to exit..")
            Console.ReadLine()
            Environment.Exit(0)
        Catch ex As Exception
            Console.WriteLine("Something Wrong!, Contact Developer.")
        End Try
    End Sub
#End Region

End Module
