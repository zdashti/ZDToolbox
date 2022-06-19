namespace ZDToolbox.Helpers
{
    public static class StringHelper
    {
        public static string ConcatWithSplitter(string first, string second, string splitter)
        {
            return string.IsNullOrWhiteSpace(first) ? string.Empty : $"{first}{splitter}{second}";
        }

        public static string ConcatWithSplitter(string first, string second)
        {
            return ConcatWithSplitter(first, second, " ");
        }

        public static string Fix(this string text)
        {
            if (text is null)
                return null;

            text = text.Trim();

            if (text == string.Empty)
                return null;

            while (text.Contains("  "))
                text = text.Replace("  ", " ");

            return text;
        }

        public static string FixArabicCharacter(this string text)
        {
            return text?.Replace("ك", "ک")
                .Replace("ي", "ی")
                .Replace("اً", "ا");
        }

        public static string FixAll(this string text)
        {
            return text.Fix().FixArabicCharacter();
        }

        public static string FixNullStringToEmptyString(this string name)
        {
            return string.IsNullOrWhiteSpace(name) ? string.Empty : name.FixAll();
        }

        public static string FixNullStringToDefaultValue(this string name, string defaultValue)
        {
            return string.IsNullOrWhiteSpace(name) ? defaultValue : name.FixAll();
        }
        public static string ApplySeparator(this long number)
        {
            return number.ToString("#,#.###");
        }
        public static string ApplySeparator(this decimal number)
        {
            return number.ToString("#,#.###");
        }
        //public static string ApplyFormat(this decimal number, IStringLocalizer<General> localizer)
        //{
        //    const int million = 1000000;
        //    const int billion = 1000000000;

        //    if (number <= million)
        //        return number.ApplySeparator();

        //    var dividedTo = (Convert.ToDecimal(number) > billion ? billion : million);
        //    return Math.Round(number / dividedTo, 3).ApplySeparator() + " " + (dividedTo == 1000000 ? localizer.GetString(General.Million) : localizer.GetString(General.Billion));
        //}
        //public static string ApplyFormat(this long number, IStringLocalizer<General> localizer)
        //{
        //    return ((decimal)number).ApplyFormat(localizer);
        //}
    }
}
