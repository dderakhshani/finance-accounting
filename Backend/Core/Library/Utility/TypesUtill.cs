using System;

namespace Library.Utility
{
    public static class TypesUtill
    {
        public static T ChangeType<T>(this object value)
        {
            var t = typeof(T);

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                t = Nullable.GetUnderlyingType(t);
            }

            return (T)Convert.ChangeType(value, t);
        }


        public static object ChangeType(this object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }
    }
}