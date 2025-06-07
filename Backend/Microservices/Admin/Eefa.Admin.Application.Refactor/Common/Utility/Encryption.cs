using System.Security.Cryptography;
using System.Text;
using System;

public static class Encryption
{
    public static class Symmetric
    {
        public static string CreateMd5Hash(string input)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            var hashBaytes = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (var hashBayte in hashBaytes)
            {
                sb.Append(hashBayte.ToString("X2"));
            }

            var hashed = sb.ToString();
            return hashed;
        }

        public static bool VerifyHash(string hashedString, string simpleString)
        {
            return CreateMd5Hash(simpleString) == hashedString;
        }
    }
    private class KeyPair
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }

    private static KeyPair GenerateNewKeyPair(int keySize = 1024)
    {
        // KeySize is measured in bits. 1024 is the default, 2048 is better, 4096 is more robust but takes a fair bit longer to generate.
        using var rsa = new RSACryptoServiceProvider(keySize);
        return new KeyPair { PublicKey = rsa.ToXmlString(false), PrivateKey = rsa.ToXmlString(true) };
    }

    // Create a method to encrypt a text and save it to a specific file using a RSA algorithm public key   
    public static string EncryptString(string value, string publicKey)
    {
        return Convert.ToBase64String(EncryptData(Encoding.UTF8.GetBytes(value), publicKey));
    }

    public static string DecryptString(string value, string privateKey)
    {
        return Encoding.UTF8.GetString(DecryptData(Convert.FromBase64String(value), privateKey));
    }


    // Method to decrypt the data withing a specific file using a RSA algorithm private key   
    public static byte[] EncryptData(byte[] data, string publicKey)
    {
        using var asymmetricProvider = new RSACryptoServiceProvider();
        asymmetricProvider.FromXmlString(publicKey);
        return asymmetricProvider.Encrypt(data, false);
    }

    public static byte[] DecryptData(byte[] data, string privateKey)
    {
        using var asymmetricProvider = new RSACryptoServiceProvider();
        asymmetricProvider.FromXmlString(privateKey);
        if (asymmetricProvider.PublicOnly)
            throw new Exception("The key provided is a public key and does not contain the private key elements required for decryption");
        return asymmetricProvider.Decrypt(data, false);
    }

}