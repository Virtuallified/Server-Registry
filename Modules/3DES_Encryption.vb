Module _3DES_Encryption
    ' 
#Region "--------- CipherText into a .txt ---------"
    Sub TripleDES_Encoding()
        Try
            Console.WriteLine("Enter the plain text:")
            Dim plainText As String = Console.ReadLine()
            Console.WriteLine("Enter the password:")
            Dim password As String = Console.ReadLine()

            Dim wrapper As New _3DES(password)
            Dim cipherText As String = wrapper.EncryptData(plainText)

            Console.WriteLine("The cipher text is: " & cipherText)
            System.IO.File.WriteAllText(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) &
            "\cipherText.txt", cipherText)
        Catch ex As Exception
            Console.WriteLine("Something Wrong!, Contact Developer.")
            Console.ReadLine()
        End Try
    End Sub
    Sub TripleDES_Decoding()
        Dim cipherText As String = System.IO.File.ReadAllText(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) &
               "\cipherText.txt")
        Console.WriteLine("Enter the password:")
        Dim password As String = Console.ReadLine()
        Dim wrapper As New _3DES(password)

        ' DecryptData throws if the wrong password is used.
        Try
            Dim plainText As String = wrapper.DecryptData(cipherText)
            Console.WriteLine("The plain text is: " & plainText)
            Console.ReadLine()
        Catch ex As System.Security.Cryptography.CryptographicException
            Console.WriteLine("The data could not be decrypted with the password.")
            Console.ReadLine()
        End Try
    End Sub
#End Region

End Module
