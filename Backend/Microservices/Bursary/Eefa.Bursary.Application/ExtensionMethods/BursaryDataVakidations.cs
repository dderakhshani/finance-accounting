using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.ExtensionMethods
{

    public class BursaryDataVakidations
    {
        public static bool ValidateShebaNo(string sheba)
        {
            if (string.IsNullOrEmpty(sheba) || sheba.Length != 26 || !sheba.StartsWith("IR"))
            {
                return false;
            }

            string modifiedSheba = sheba.Substring(4) + "1827" + sheba.Substring(2, 2);

            BigInteger shebaNumber;
            if (!BigInteger.TryParse(modifiedSheba, out shebaNumber))
            {
                return false;
            }

            return shebaNumber % 97 == 1;
        }
    }

}
