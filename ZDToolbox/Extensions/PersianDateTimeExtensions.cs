using ZDToolbox.Helpers;

namespace ZDToolbox.Extensions
{
    public static class PersianDateTimeExtension
    {
        public static DateTime PersianDateToDateTime(this string persianDate)
        {
            if (string.IsNullOrWhiteSpace(persianDate))
                throw new Exception("Invalid PersianDate");
            return PersianDateTime.Parse(persianDate).ToDateTime();
        }

        public static int DateTimeToPersianDigitalDateInt(this DateTime date)
        {
            var strDate = date.DateTimeToPersianDigitalDate();
            return string.IsNullOrWhiteSpace(strDate) ? 0 :
                int.Parse(strDate
                .Replace("/", "")
                .Replace(@"\", "")
                .Replace("-", ""));
        }

        public static string DateTimeToPersianDigitalDate(this DateTime date)
        {
            var strDate = date <= DateTime.MinValue
                ? string.Empty :
                new PersianDateTime(date).ToString();

            return strDate[..strDate.IndexOf(" ", StringComparison.Ordinal)];
        }
        public static string DateTimeToPersianDate(this DateTime? date)
        {
            return date == null ? string.Empty
                : new PersianDateTime((DateTime)date).ToPersianDateString();
        }

        private static string FixPersianDateLength(this string date)
        {
            string newDate = date;
            //if (newDate.Length < 10)
            //{
            //    var index = newDate.LastIndexOf("/", StringComparison.Ordinal);
            //    if (index < 0) return newDate;
            //    if(newDate[index..].Length < 2)

            //}
            return newDate;
        }

        public static string DateTimeToPersianDigitalDateTimeString(this DateTime? date)
        {
            return date == null || date <= DateTime.MinValue
                ? string.Empty
                : ((DateTime)date).DateTimeToPersianDigitalDate();
        }

        public static string DateTimeToPersianDigitalDateString(this DateTime? date)
        {
            return date == null || date <= DateTime.MinValue
                ? string.Empty
                : new PersianDateTime((DateTime)date).ToPersianDigitalDateString();
        }
        public static string DateTimeToPersianDigitalDateString(this DateTime date)
        {
            return date <= DateTime.MinValue
                ? string.Empty
                : new PersianDateTime(date).ToPersianDigitalDateString();
        }

        public static PersianDateTime ToPersianDateTime(this DateTime? dt)
        {
            return dt?.ToPersianDateTime();
        }

        public static string DateTimeToDigital24HourString(this DateTime date)
        {
            return date.ToString("HH:mm");
        }

        public static string MakeDateTimeToPersianDateAdn24Hours(this DateTime dateTime)
        {
            return StringHelper.ConcatWithSplitter(dateTime.DateTimeToPersianDigitalDate(),
                dateTime.DateTimeToDigital24HourString(), " - ");
        }
    }
}
