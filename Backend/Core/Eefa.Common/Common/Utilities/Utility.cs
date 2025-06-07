namespace Eefa.Common.Common.Utilities
{
    public class Utility
    {
        public static string ConvertNumberToEnglish(string number)
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
            return number;
        }
    }
}
