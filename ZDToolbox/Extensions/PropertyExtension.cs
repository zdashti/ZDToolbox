using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace ZDToolbox.Extensions
{
    public static class PropertyExtension
    {
        public static string GetDisplayName<TSource>(this TSource T, Expression<Func<TSource, object>> expression)
        {
            var attribute = ((MemberExpression)expression.Body).Member.GetCustomAttribute<DisplayAttribute>();

            return attribute == null ? ((MemberExpression)expression.Body).Member.Name : attribute.Name;
        }
    }
}
