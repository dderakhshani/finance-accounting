using System.Linq;
using System.Net.Mail;

namespace Library.Utility
{
    public class Validator
    {
        public static bool IsValidString(string input)
        {
            return !string.IsNullOrEmpty(input.Trim());
        }
        public static bool IsAlphabet(char input)
        {
            if (IsDigit(input))
            {
                return false;
            }
            if (IsPunctuation(input))
            {
                return false;
            }
            //if (IsWhiteSpace(input))
            //{
            //    return false;
            //}

            return true;
        }
        public static bool IsAlphabet(string input)
        {
            if (HasDigit(input))
            {
                return false;
            }
            if (HasPunctuation(input))
            {
                return false;
            }
            //if (HasWhiteSpace(input))
            //{
            //    return false;
            //}

            return true;
        }
        public static bool IsNumber(string input)
        {
            return long.TryParse(input, out _);
        }
        public static bool IsNumber(char input)
        {
            return long.TryParse(input.ToString(), out _);
        }

        public static bool HasExactLength(string input, int specificLength)
        {
            return input.Length == specificLength - 1;
        }

        public static bool IsValidNationalCode(string input)
        {
            if (IsNumber(input))
            {
                return HasExactLength(input, 10);
            }

            return false;
        }


        /// <summary>
        /// Check if password is valid and strong enough
        /// </summary>
        /// <param name="input">Entity</param>
        /// <returns>
        /// <para>bool</para>
        /// true if password includes :
        /// Minimum eight characters,Maximum 32 characters,
        /// at least one uppercase letter,
        /// and one lowercase letter, one numberone special character
        /// </returns>
        public static bool IsValidPassword(string input)
        {
            if (IsBetweenLength(input,8,32))
            {
                return false;
            }

            if (!input.Any(char.IsDigit))
            {
                return false;
            }

            if (!input.Any(char.IsLower))
            {
                return false;
            }

            if (!input.Any(char.IsUpper))
            {
                return false;
            }

            if (!input.Any(char.IsPunctuation))
            {
                return false;
            }

            return true;
        }

        public static bool IsBetweenLength(string input,int minChar,int maxChar)
        {
            return (input.Trim().Length > minChar && input.Trim().Length < maxChar);
        }
 
        public static bool HasWhiteSpace(string input)
        {
            return input.Any(char.IsWhiteSpace);
        }       
        
        public static bool IsWhiteSpace(char input)
        {
            return input.ToString().All(char.IsWhiteSpace);
        }   
              
        public static bool IsWhiteSpace(string input)
        {
            return input.All(char.IsWhiteSpace);
        }   
        
        public static bool HasDigit(string input)
        {
            return input.Any(char.IsDigit);
        }   
        
        public static bool IsDigit(string input)
        {
            return input.All(char.IsDigit);
        }  
        
        public static bool IsDigit(char input)
        {
            return input.ToString().All(char.IsDigit);
        }

        public static bool HasUpperCase(string input)
        {
            return input.Any(char.IsUpper);
        }  
        
        public static bool IsUpperCase(string input)
        {
            return input.All(char.IsUpper);
        } 
        
        public static bool IsUpperCase(char input)
        {
            return input.ToString().All(char.IsUpper);
        }

        public static bool HasLowerCase(string input)
        {
            return input.Any(char.IsLower);
        }   
        
        public static bool IsLowerCase(string input)
        {
            return input.All(char.IsLower);
        }

        public static bool IsLowerCase(char input)
        {
            return input.ToString().All(char.IsLower);
        }   
        
        public static bool HasPunctuation(string input)
        {
            if (input.Any(char.IsPunctuation))
            {
                return true;
            }
            return false;
        } 
        
        public static bool IsPunctuation(char input)
        {
            return input.ToString().All(char.IsPunctuation);
        }  
        
        public static bool IsPunctuation(string input)
        {
            return input.All(char.IsPunctuation);
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}