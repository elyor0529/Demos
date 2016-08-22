using System;
using System.Globalization;

namespace Task2.Common.Helpers
{
  public static class ConvertExtension
  {
    public static T ChangeType<T>(this object value)
    {
      return (T)ChangeType(value, typeof(T));
    }

    public static T ChangeType<T>(this object value, T whenNull)
    {
      return (value == null || value == DBNull.Value)
          ? whenNull
          : (T)ChangeType(value, typeof(T));
    }

    public static object ChangeType(this object value, Type convertToType)
    {
      if (convertToType == null)
      {
        throw new ArgumentNullException("convertToType");
      }

      // return null if the value is null or DBNull
      if (value == null || value is DBNull)
      {
        return null;
      }

      // non-nullable types, which are not supported by Convert.ChangeType(), 
      // unwrap the types to determine the underlying time
      if (convertToType.IsGenericType &&
      convertToType.GetGenericTypeDefinition() == typeof(Nullable<>))
      {
        convertToType = Nullable.GetUnderlyingType(convertToType);
      }

      // deal with conversion to enum types when input is a string 
      if (convertToType.IsEnum && value is string)
      {
        return Enum.Parse(convertToType, (string)value);
      }

      // deal with conversion to enum types when input is a integral primitive 
      if (convertToType.IsEnum && value.GetType().IsPrimitive && !(value is bool) &&
          !(value is char) && !(value is float) && !(value is double))
      {
        return Enum.ToObject(convertToType, value);
      }

      return Convert.ChangeType(value, convertToType, CultureInfo.InvariantCulture);
    } 

  }
}