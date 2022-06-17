using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace ZDToolbox.Extensions
{
    public static class ValidatorHelperExtensions
    {
        /// <summary>
        /// تعیین معتبر بودن کد ملی
        /// </summary>
        /// <param name="nationalCode">کد ملی وارد شده</param>
        /// <returns>
        /// در صورتی که کد ملی صحیح باشد خروجی <c>true</c> و در صورتی که کد ملی اشتباه باشد خروجی <c>false</c> خواهد بود
        /// </returns>
        /// <exception cref="System.Exception"></exception>
        public static bool IsValidNationalCode(this string nationalCode)
        {
            //در صورتی که کد ملی وارد شده تهی باشد

            if (String.IsNullOrEmpty(nationalCode))
                throw new Exception("لطفا کد ملی را صحیح وارد نمایید");


            //در صورتی که کد ملی وارد شده طولش کمتر از 10 رقم باشد
            if (nationalCode.Length != 10)
                throw new Exception("طول کد ملی باید ده کاراکتر باشد");

            //در صورتی که کد ملی ده رقم عددی نباشد
            var regex = new Regex(@"\d{10}");
            if (!regex.IsMatch(nationalCode))
                throw new Exception("کد ملی تشکیل شده از ده رقم عددی می‌باشد؛ لطفا کد ملی را صحیح وارد نمایید");

            //در صورتی که رقم‌های کد ملی وارد شده یکسان باشد
            var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
            if (allDigitEqual.Contains(nationalCode)) return false;


            //عملیات شرح داده شده در بالا
            var chArray = nationalCode.ToCharArray();
            var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
            var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
            var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
            var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
            var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
            var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
            var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
            var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
            var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
            var a = Convert.ToInt32(chArray[9].ToString());

            var b = (((((((num0 + num2) + num3) + num4) + num5) + num6) + num7) + num8) + num9;
            var c = b % 11;

            return (((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a)));
        }

        /// <summary>
        /// چک کردن شبا
        /// </summary>
        /// <param name="input"></param>
        /// <param name="canBeNullOrWhiteSpace"></param>
        /// <returns></returns>
        public static bool CheckIban(this string input, bool canBeNullOrWhiteSpace = false)
        {
            if (!input.CheckCanBeNullOrWhiteSpace(canBeNullOrWhiteSpace))
                return false;
            //todo
            input = input.Replace(" ", "").ToLower();
            //بررسی رشته وارد شده برای اینکه در فرمت شبا باشد
            var isIban = Regex.IsMatch(input, "^[a-zA-Z]{2}\\d{2} ?\\d{4} ?\\d{4} ?\\d{4} ?\\d{4} ?[\\d]{0,2}",
                RegexOptions.Compiled);

            if (!isIban)
                return false;
            //طول شماره شبا را چک میکند کمتر نباشد
            if (input.Length < 26)
                return false;
            input = input.ToLower();
            //بررسی اعتبار سنجی اصلی شبا
            ////ابتدا گرفتن چهار رقم اول شبا
            var get4FirstDigit = input.Substring(0, 4);
            ////جایگزین کردن عدد 18 و 27 به جای آی و آر
            var replacedGet4FirstDigit = get4FirstDigit.ToLower().Replace("i", "18").Replace("r", "27");
            ////حذف چهار رقم اول از رشته شبا
            var removedShebaFirst4Digit = input.Replace(get4FirstDigit, "");
            ////کانکت کردن شبای باقیمانده با جایگزین شده چهار رقم اول
            var newSheba = removedShebaFirst4Digit + replacedGet4FirstDigit;
            ////تبدیل کردن شبا به عدد  - دسیمال تا 28 رقم را نگه میدارد
            var finalLongData = Convert.ToDecimal(newSheba);
            ////تقسیم عدد نهایی به مقدار 97 - اگر باقیمانده برابر با عدد یک شود این رشته شبا صحیح خواهد بود
            var finalReminder = finalLongData % 97;
            return finalReminder == 1;
        }

        private static bool CheckCanBeNullOrWhiteSpace(this string theValue, bool canBeNullOrWhiteSpace)
        {
            return canBeNullOrWhiteSpace || !string.IsNullOrWhiteSpace(theValue);
        }

        public static bool IsNumeric(this string theValue, bool canBeNullOrWhiteSpace = false)
        {
            return theValue.CheckCanBeNullOrWhiteSpace(canBeNullOrWhiteSpace) || long.TryParse(theValue, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out _);
        }

        public static bool CheckPhonePrefix(this string theValue, bool canBeNullOrWhiteSpace = false)
        {
            return theValue.CheckCanBeNullOrWhiteSpace(canBeNullOrWhiteSpace) && theValue.StartsWith("0");
        }

        public static bool IsDouble(this string theValue)
        {
            return !string.IsNullOrWhiteSpace(theValue) && double.TryParse(theValue, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out _);
        }

        public static bool IsValidString(this string theValue, bool canBeNullOrWhiteSpace = false)
        {
            return theValue.CheckCanBeNullOrWhiteSpace(canBeNullOrWhiteSpace) && !theValue.ToLower().Equals("undefined");
        }

        public static bool IsValidDate(this string theValue)
        {
            try
            {
                theValue.PersianDateToDateTime();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidEmail(this string email, bool canBeNullOrWhiteSpace = false)
        {
            if (!email.CheckCanBeNullOrWhiteSpace(canBeNullOrWhiteSpace))
                return false;

            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                return false;
            }

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                    RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                static string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static bool IsValidHttpSUrl(this string uriName, bool canBeNullOrWhiteSpace = false)
        {
            return uriName.CheckCanBeNullOrWhiteSpace(canBeNullOrWhiteSpace)
                   && Uri.TryCreate(uriName, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public static bool IsValidIp(this string theValue, bool canBeNullOrWhiteSpace = false)
        {
            if (!theValue.CheckCanBeNullOrWhiteSpace(canBeNullOrWhiteSpace))
                return false;

            if (!IPAddress.TryParse(theValue, out _))
                return false;

            return theValue.Split('.').Length == 4 && theValue.Split('.').All(r => byte.TryParse(r, out _));
        }
    }
}
