using Microsoft.Extensions.Localization;
using ZDToolbox.Classes;
using ZDToolbox.Extensions;

namespace ZDToolbox.Helpers
{
    public static class EnumHelper
    {
        public static T? GetEnumFromStringOrNull<T>(string value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            try
            {
                return (T)Enum.Parse(typeof(T), value);
            }
            catch
            {
                return null;
            }
        }

        public static List<KeyValue<string, int>> GetEnumList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<Enum>()
                .Select(item => new KeyValue<string, int>
                {
                    Value = item.ToInt(),
                    Key = item.DisplayName() ?? Enum.GetName(typeof(T), item)
                }).ToList();
        }

        public static T GetEnumFromString<T>(string value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            try
            {
                return (T)Enum.Parse(typeof(T), value);
            }
            catch
            {
                throw new Exception("Undefined Enum");
            }
        }

        public static List<KeyValue<string, int>> GetEnumList<T, TL>(IStringLocalizer<TL> localizer) where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<Enum>()
                .Select(item => new KeyValue<string, int>
                {
                    Value = item.ToInt(),
                    Key = item.DisplayName(localizer) ?? Enum.GetName(typeof(T), item)
                }).ToList();
        }

        public static T GetEnumFromInt<T>(int x) where T : Enum
        {
            var val = (T)Enum.ToObject(typeof(T), x);
            return val.IsValidEnum<T>() ? val : Enum.GetValues(typeof(T)).Cast<T>().First();
        }
    }
}
