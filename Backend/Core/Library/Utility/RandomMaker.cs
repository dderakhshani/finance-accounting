using System;

namespace Library.Utility
{
    public static class RandomMaker
    {
        public static string GenerateRandomStringWithDash(int characterNumber)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[characterNumber];
            var random = new Random(DateTime.Now.Millisecond);

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = "-" + new string(stringChars);

            finalString = finalString[..characterNumber];

            return finalString;
        }


        public static string GenerateRandomString(int characterNumber, bool justAlphabet = false)
        {
            const string alphaetChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            const string numberChars = "0123456789";


            var chars = alphaetChars;
            if (!justAlphabet)
            {
                chars += numberChars;
            }
            var stringChars = new char[characterNumber];
            var random = new Random(DateTime.Now.Millisecond);

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new string(stringChars);

            finalString = finalString[..characterNumber];

            return finalString;
        }



        public static int GenerateRandomInt(int characterNumber)
        {
            const string chars = "0123456789";
            var stringChars = new char[characterNumber];
            var random = new Random(DateTime.Now.Millisecond);

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];

                while (stringChars[0] == '0')
                {
                    stringChars[0] = chars[random.Next(chars.Length)];
                }
            }

            var finalString = new string(stringChars);

            finalString = finalString[..characterNumber];

            int.TryParse(finalString, out var res);

            return res;
        }
        public static long GenerateRandomLong(int characterNumber)
        {
            const string chars = "0123456789";
            var stringChars = new char[characterNumber];
            var random = new Random(DateTime.Now.Millisecond);

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];

                while (stringChars[0] == '0')
                {
                    stringChars[0] = chars[random.Next(chars.Length)];
                }
            }

            var finalString = new string(stringChars);

            finalString = finalString[..characterNumber];

            long.TryParse(finalString, out var res);

            return res;
        }

        public static Double GenerateRandomDouble(int characterNumber)
        {
            const string chars = "0123456789";
            var stringChars = new char[characterNumber];
            var random = new Random(DateTime.Now.Millisecond);

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];

                while (stringChars[0] == '0')
                {
                    stringChars[0] = chars[random.Next(chars.Length)];
                }
            }

            var finalString = new string(stringChars);

            finalString = finalString[..characterNumber];

            Double.TryParse(finalString, out var res);

            return res;
        }

        public static string GenerateStringOfRandomNumber(int characterNumber)
        {
            const string chars = "0123456789";
            var stringChars = new char[characterNumber];
            var random = new Random(DateTime.Now.Millisecond);

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];

                while (stringChars[0] == '0')
                {
                    stringChars[0] = chars[random.Next(chars.Length)];
                }
            }

            var finalString = new string(stringChars);

            finalString = finalString[..characterNumber];

            return finalString;
        }

        public static int GenerateRandomNumber(int n)
        {
            const string chars = "0123456789";
            var stringChars = new char[n];
            var random = new Random(DateTime.Now.Millisecond);

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];

                while (stringChars[0] == '0')
                {
                    stringChars[0] = chars[random.Next(chars.Length)];
                }
            }

            var finalString = new string(stringChars);

            finalString = finalString[..n];
            return int.Parse(finalString);
        }
    }
}
