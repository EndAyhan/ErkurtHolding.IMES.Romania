using System.Reflection;
using System;
using System.Linq;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Extensions
{
    public static class CastingExtension
    {
        public static T Casting<T>(this Object source)
        {
            Type sourceType = source.GetType();
            Type targetType = typeof(T);
            var target = Activator.CreateInstance(targetType, false);
            var sourceMembers = sourceType.GetMembers()
                .Where(x => x.MemberType == MemberTypes.Property)
                .ToList();
            var targetMembers = targetType.GetMembers()
                .Where(x => x.MemberType == MemberTypes.Property)
                .ToList();
            var members = targetMembers
                .Where(x => sourceMembers
                    .Select(y => y.Name)
                        .Contains(x.Name));
            PropertyInfo propertyInfo;
            object value;
            foreach (var memberInfo in members)
            {
                propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                value = source.GetType().GetProperty(memberInfo.Name).GetValue(source, null);
                propertyInfo.SetValue(target, value, null);
            }
            return (T)target;
        }
    }
}
