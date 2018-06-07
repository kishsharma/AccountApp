Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Public Class RijndaelSimple
    Public Function Encrypt(ByVal plainText As String) As String

        '    passPhrase = "admin"                ' can be any string
        '    saltValue = "s@1tValue"                 ' can be any string
        '    hashAlgorithm = "SHA1"                  ' can be "MD5"
        '    passwordIterations = 2                  ' can be any number
        '    initVector = "@1B2c3D4e5F6g7H8"         ' must be 16 bytes
        '    keySize = 256                           ' can be 192 or 128

        Dim passPhrase As String = "admin" '= "yourPassPhrase"
        Dim saltValue As String = "s@1tValue" '= "mySaltValue"
        Dim hashAlgorithm As String = "SHA1"
        Dim passwordIterations As Integer = 2
        Dim initVector As String = "@1B2c3D4e5F6g7H8"
        Dim keySize As Integer = 256

        Dim initVectorBytes As Byte() = Encoding.ASCII.GetBytes(initVector)
        Dim saltValueBytes As Byte() = Encoding.ASCII.GetBytes(saltValue)

        Dim plainTextBytes As Byte() = Encoding.UTF8.GetBytes(plainText)


        Dim password As New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)

        Dim keyBytes As Byte() = password.GetBytes(keySize \ 8)
        Dim symmetricKey As New RijndaelManaged()

        symmetricKey.Mode = CipherMode.CBC

        Dim encryptor As ICryptoTransform = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes)

        Dim memoryStream As New MemoryStream()
        Dim cryptoStream As New CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)

        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length)
        cryptoStream.FlushFinalBlock()
        Dim cipherTextBytes As Byte() = memoryStream.ToArray()
        memoryStream.Close()
        cryptoStream.Close()
        Dim cipherText As String = Convert.ToBase64String(cipherTextBytes)
        Return cipherText
    End Function

    Public Function Decrypt(ByVal cipherText As String) As String

        Dim passPhrase As String = "admin" '= "yourPassPhrase"
        Dim saltValue As String = "s@1tValue" '= "mySaltValue"
        Dim hashAlgorithm As String = "SHA1"
        Dim passwordIterations As Integer = 2
        Dim initVector As String = "@1B2c3D4e5F6g7H8"
        Dim keySize As Integer = 256

        'Dim passPhrase As String = "yourPassPhrase"
        'Dim saltValue As String = "mySaltValue"
        'Dim hashAlgorithm As String = "SHA1"

        'Dim passwordIterations As Integer = 2
        'Dim initVector As String = "@1B2c3D4e5F6g7H8"
        'Dim keySize As Integer = 256

        ' Convert strings defining encryption key characteristics into byte
        ' arrays. Let us assume that strings only contain ASCII codes.
        ' If strings include Unicode characters, use Unicode, UTF7, or UTF8
        ' encoding.
        Dim initVectorBytes As Byte() = Encoding.ASCII.GetBytes(initVector)
        Dim saltValueBytes As Byte() = Encoding.ASCII.GetBytes(saltValue)

        ' Convert our ciphertext into a byte array.
        Dim cipherTextBytes As Byte() = Convert.FromBase64String(cipherText)

        ' First, we must create a password, from which the key will be
        ' derived. This password will be generated from the specified
        ' passphrase and salt value. The password will be created using
        ' the specified hash algorithm. Password creation can be done in
        ' several iterations.
        Dim password As New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)

        ' Use the password to generate pseudo-random bytes for the encryption
        ' key. Specify the size of the key in bytes (instead of bits).
        Dim keyBytes As Byte() = password.GetBytes(keySize \ 8)

        ' Create uninitialized Rijndael encryption object.
        Dim symmetricKey As New RijndaelManaged()

        ' It is reasonable to set encryption mode to Cipher Block Chaining
        ' (CBC). Use default options for other symmetric key parameters.
        symmetricKey.Mode = CipherMode.CBC

        ' Generate decryptor from the existing key bytes and initialization
        ' vector. Key size will be defined based on the number of the key
        ' bytes.
        Dim decryptor As ICryptoTransform = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes)

        ' Define memory stream which will be used to hold encrypted data.
        Dim memoryStream As New MemoryStream(cipherTextBytes)

        ' Define cryptographic stream (always use Read mode for encryption).
        Dim cryptoStream As New CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)

        ' Since at this point we don't know what the size of decrypted data
        ' will be, allocate the buffer long enough to hold ciphertext;
        ' plaintext is never longer than ciphertext.
        Dim plainTextBytes As Byte() = New Byte(cipherTextBytes.Length - 1) {}

        ' Start decrypting.
        Dim decryptedByteCount As Integer = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length)

        ' Close both streams.
        memoryStream.Close()
        cryptoStream.Close()

        ' Convert decrypted data into a string.
        ' Let us assume that the original plaintext string was UTF8-encoded.
        Dim plainText As String = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount)

        ' Return decrypted string.  
        Return plainText
    End Function

    'Public Shared Function Encrypt(ByVal plainText As String, _
    '                               ByVal passPhrase As String, _
    '                               ByVal saltValue As String, _
    '                               ByVal hashAlgorithm As String, _
    '                               ByVal passwordIterations As Integer, _
    '                               ByVal initVector As String, _
    '                               ByVal keySize As Integer) _
    '                       As String
    '    Dim StrReturn As String = String.Empty
    '    Try
    '        Dim initVectorBytes As Byte()
    '        initVectorBytes = Encoding.ASCII.GetBytes(initVector)

    '        Dim saltValueBytes As Byte()
    '        saltValueBytes = Encoding.ASCII.GetBytes(saltValue)

    '        Dim plainTextBytes As Byte()
    '        plainTextBytes = Encoding.UTF8.GetBytes(plainText)

    '        Dim password As PasswordDeriveBytes
    '        password = New PasswordDeriveBytes(passPhrase, _
    '                                           saltValueBytes, _
    '                                           hashAlgorithm, _
    '                                           passwordIterations)
    '        Dim keyBytes As Byte()
    '        keyBytes = password.GetBytes(keySize / 8)


    '        Dim symmetricKey As RijndaelManaged
    '        symmetricKey = New RijndaelManaged
    '        symmetricKey.Mode = CipherMode.CBC

    '        Dim encryptor As ICryptoTransform
    '        encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes)

    '        Dim memoryStream As MemoryStream
    '        memoryStream = New MemoryStream

    '        Dim cryptoStream As CryptoStream
    '        cryptoStream = New CryptoStream(memoryStream, _
    '                                        encryptor, _
    '                                        CryptoStreamMode.Write)
    '        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length)

    '        cryptoStream.FlushFinalBlock()

    '        Dim cipherTextBytes As Byte()
    '        cipherTextBytes = memoryStream.ToArray()

    '        memoryStream.Close()
    '        cryptoStream.Close()

    '        Dim cipherText As String
    '        cipherText = Convert.ToBase64String(cipherTextBytes)

    '        Return cipherText
    '    Catch ex As Exception
    '        Return StrReturn
    '    End Try
    'End Function
    'Public Shared Function Decrypt(ByVal cipherText As String, _
    '                      ByVal passPhrase As String, _
    '                      ByVal saltValue As String, _
    '                      ByVal hashAlgorithm As String, _
    '                      ByVal passwordIterations As Integer, _
    '                      ByVal initVector As String, _
    '                      ByVal keySize As Integer) _
    '              As String
    '    Dim StrReturn As String = String.Empty
    '    Try
    '        Dim initVectorBytes As Byte()
    '        initVectorBytes = Encoding.ASCII.GetBytes(initVector)

    '        Dim saltValueBytes As Byte()
    '        saltValueBytes = Encoding.ASCII.GetBytes(saltValue)

    '        Dim cipherTextBytes As Byte()
    '        cipherTextBytes = Convert.FromBase64String(cipherText)

    '        Dim password As PasswordDeriveBytes
    '        password = New PasswordDeriveBytes(passPhrase, _
    '                                           saltValueBytes, _
    '                                           hashAlgorithm, _
    '                                           passwordIterations)

    '        Dim keyBytes As Byte()
    '        keyBytes = password.GetBytes(keySize / 8)

    '        Dim symmetricKey As RijndaelManaged
    '        symmetricKey = New RijndaelManaged

    '        symmetricKey.Mode = CipherMode.CBC

    '        Dim decryptor As ICryptoTransform
    '        decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes)

    '        Dim memoryStream As MemoryStream
    '        memoryStream = New MemoryStream(cipherTextBytes)

    '        Dim cryptoStream As CryptoStream
    '        cryptoStream = New CryptoStream(memoryStream, _
    '                                        decryptor, _
    '                                        CryptoStreamMode.Read)

    '        Dim plainTextBytes As Byte()
    '        ReDim plainTextBytes(cipherTextBytes.Length)

    '        Dim decryptedByteCount As Integer
    '        decryptedByteCount = cryptoStream.Read(plainTextBytes, _
    '                                               0, _
    '                                               plainTextBytes.Length)

    '        memoryStream.Close()
    '        cryptoStream.Close()

    '        Dim plainText As String
    '        plainText = Encoding.UTF8.GetString(plainTextBytes, _
    '                                            0, _
    '                                            decryptedByteCount)

    '        Return plainText
    '    Catch ex As Exception
    '        Return StrReturn
    '    End Try
    'End Function
    'Public Function fnEncryptData(ByVal pwd As String) As String
    '    Dim plainText As String
    '    Dim cipherText As String
    '    Dim passPhrase As String
    '    Dim saltValue As String
    '    Dim hashAlgorithm As String
    '    Dim passwordIterations As Integer
    '    Dim initVector As String
    '    Dim keySize As Integer
    '    plainText = pwd

    '    passPhrase = "admin"                ' can be any string
    '    saltValue = "s@1tValue"                 ' can be any string
    '    hashAlgorithm = "SHA1"                  ' can be "MD5"
    '    passwordIterations = 2                  ' can be any number
    '    initVector = "@1B2c3D4e5F6g7H8"         ' must be 16 bytes
    '    keySize = 256                           ' can be 192 or 128
    '    cipherText = RijndaelSimple.Encrypt(plainText, _
    '                                        passPhrase, _
    '                                        saltValue, _
    '                                        hashAlgorithm, _
    '                                        passwordIterations, _
    '                                        initVector, _
    '                                        keySize)
    '    fnEncryptData = cipherText
    'End Function
    'Public Function fnDecryptData(ByVal pwd As String) As String
    '    Dim plainText As String
    '    ' Dim cipherText As String
    '    Dim passPhrase As String
    '    Dim saltValue As String
    '    Dim hashAlgorithm As String
    '    Dim passwordIterations As Integer
    '    Dim initVector As String
    '    Dim keySize As Integer
    '    Dim tempcipherText As String

    '    tempcipherText = pwd
    '    passPhrase = "admin"                ' can be any string
    '    saltValue = "s@1tValue"                 ' can be any string
    '    hashAlgorithm = "SHA1"                  ' can be "MD5"
    '    passwordIterations = 2                  ' can be any number
    '    initVector = "@1B2c3D4e5F6g7H8"         ' must be 16 bytes
    '    keySize = 256                           ' can be 192 or 128
    '    plainText = RijndaelSimple.Decrypt(tempcipherText, _
    '                                        passPhrase, _
    '                                        saltValue, _
    '                                        hashAlgorithm, _
    '                                        passwordIterations, _
    '                                        initVector, _
    '                                        keySize)
    '    fnDecryptData = plainText
    'End Function
End Class
