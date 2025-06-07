using System;

namespace Eefa.Common
{
    public static class RandomMaker
    {
        public static string GenerateRandomString(int maxChar)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random(DateTime.Now.Millisecond);

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = "-" +  new string(stringChars);

            if (maxChar < 9)
            {
                finalString = finalString[..maxChar];
            }

            return finalString;
        }
    }
}
