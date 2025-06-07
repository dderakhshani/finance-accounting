
using Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Common
{
    public class TejaratBankServices:ITejaratBankServices
    {

        public string SetDepositId(string inputNumber)
        {
            int[] digits = Array.ConvertAll(inputNumber.ToCharArray(), c => (int)char.GetNumericValue(c));

            int sum1 = 0;
            for (int i = 0; i < digits.Length; i++)
            {
                sum1 += digits[i] * (i + 1);
            }

            int n1 =  sum1 % 11;
            int checkDigit = (n1 <= 9) ? n1 : 0;

            string result = inputNumber + checkDigit;

            return result;
        }



    }
}
