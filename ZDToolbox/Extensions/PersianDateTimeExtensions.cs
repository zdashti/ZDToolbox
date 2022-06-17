namespace ZDToolbox.Extensions
{
    public static class PersianDateTimeExtensions
    {
        public static DateTime PersianDateToDateTime(this string persianDate)
        {
            if (string.IsNullOrWhiteSpace(persianDate))
                throw new Exception("Invalid PersianDate");
            return PersianDateTime.Parse(persianDate).ToDateTime();
        }

        public static string DateTimeToPersianDate(this DateTime? date)
        {
            return date == null || date <= DateTime.MinValue
                ? string.Empty
                : new PersianDateTime((DateTime)date).ToPersianDateString();
        }

        public static string DateTimeToPersianDigitalDateTimeString(this DateTime? date)
        {
            return date == null || date <= DateTime.MinValue
                ? string.Empty
                : new PersianDateTime((DateTime)date).ToPersianDigitalDateTimeString();
        }

        public static string DateTimeToPersianDigitalDateString(this DateTime? date)
        {
            return date == null || date <= DateTime.MinValue
                ? string.Empty
                : new PersianDateTime((DateTime)date).ToPersianDigitalDateString();
        }
    }
}
