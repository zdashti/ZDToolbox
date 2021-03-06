using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
namespace ZDToolbox.Extensions
{
    public static class EnumExtensions
    {
        public static int ToInt(this Enum enumName)
        {
            return Convert.ToInt32(enumName);
        }

        public static TAttribute? GetAttribute<TAttribute>(this Enum enumValue)
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<TAttribute>();
        }

        public static string DisplayName(this Enum enumName)
        {
            return enumName.GetAttribute<DisplayAttribute>()?.Name ?? enumName.ToString();
        }
        public static void EnumValidation<T>(this Enum enumName) where T : Enum
        {
            if (!Enum.IsDefined(typeof(T), enumName))
                throw new Exception("Undefined Enum value");
        }

        public static string DisplayName<T>(this Enum enumName, IStringLocalizer<T> localizer)
        {
            var name = enumName.GetAttribute<DisplayAttribute>()?.Name;
            return name != null ? localizer.GetString(name) : enumName.ToString();
        }

        public static bool IsValidEnum<T>(this Enum enumName) where T : Enum
        {
            return enumName != null && Enum.IsDefined(typeof(T), enumName);
        }
    }
}
