using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Eefa.Accounting.Application.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToEnglishNumbers(this string number)
        {
            number = number.Trim();
            if (string.IsNullOrEmpty(number)) return number;
            number = number.Replace('٠', '0');
            number = number.Replace('١', '1');
            number = number.Replace('٢', '2');
            number = number.Replace('٣', '3');
            number = number.Replace('٤', '4');
            number = number.Replace('٥', '5');
            number = number.Replace('٦', '6');
            number = number.Replace('٧', '7');
            number = number.Replace('٨', '8');
            number = number.Replace('٩', '9');
            number = number.Replace('.', '.');
            return number;
        }


        public static string Encrypt(this string plainText, string secretKey)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] iv = new byte[16]; // AES requires a 16-byte IV.

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = iv; // Initialization vector should be either random or fixed.

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    byte[] encrypted = msEncrypt.ToArray();
                    return Convert.ToBase64String(encrypted); // Return encrypted string in Base64
                }
            }
        }


        public static string Decrypt(this string cipherText, string secretKey)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] iv = new byte[16]; // AES requires a 16-byte IV.
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd(); // Return the decrypted string
                        }
                    }
                }
            }
        }
    }
}
