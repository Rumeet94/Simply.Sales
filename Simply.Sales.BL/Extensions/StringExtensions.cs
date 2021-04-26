using System.Text.RegularExpressions;

namespace Simply.Sales.BLL.Extensions {
	public static class StringExtensions {
        private const int _phoneDigitsCount = 11;

        /// <summary>
        /// Checks to be sure a phone number contains 10 digits as per American phone numbers.  
        /// If 'IsRequired' is true, then an empty string will return False. 
        /// If 'IsRequired' is false, then an empty string will return True.
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="IsRequired"></param>
        /// <returns></returns>
        public static bool ValidatePhoneNumber(this string phone, bool IsRequired) {
            if (string.IsNullOrEmpty(phone) & !IsRequired)
                return true;

            if (string.IsNullOrEmpty(phone) & IsRequired)
                return false;

            var cleaned = phone.RemoveNonNumeric();
            if (IsRequired) {
                if (cleaned.Length == _phoneDigitsCount)
                    return true;
                else
                    return false;
            }
            else {
                if (cleaned.Length == 0)
                    return true;
                else if (cleaned.Length > 0 & cleaned.Length < _phoneDigitsCount)
                    return false;
                else if (cleaned.Length == _phoneDigitsCount)
                    return true;
                else
                    return false; // should never get here
            }
        }

        /// <summary>
        /// Removes all non numeric characters from a string
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string RemoveNonNumeric(this string phone) {
            return Regex.Replace(phone, @"[^0-9]+", "");
        }
    }
}
