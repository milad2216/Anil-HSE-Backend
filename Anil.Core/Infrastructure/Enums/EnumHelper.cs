using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel;

namespace Anil.Core.Infrastructure.Enums
{
    public static class EnumHelper
    {
        public static TAttribute GetAttribute<TEnum, TAttribute>(object value) where TEnum : struct, IConvertible
        {
            TEnum enumValue = GetEnum<TEnum>(value);

            return typeof(TEnum).GetField(enumValue.ToString()).GetCustomAttributes(true).OfType<TAttribute>().FirstOrDefault();

        }

        public static IEnumerable<string> GetValues<T>(bool isPersian = true, bool justBrowsable = true) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            if (!justBrowsable)
            {
                foreach (var item in Enum.GetValues(typeof(T)))
                    yield return /*isPersian ? GetPersianTitle<T>(item) :*/ item.ToString();
            }

            else

                foreach (var item in Enum.GetValues(typeof(T)))
                {
                    FieldInfo finfo = typeof(T).GetField(item.ToString());
                    BrowsableAttribute attrib = finfo.GetCustomAttributes(false).OfType<BrowsableAttribute>().FirstOrDefault();

                    //if (attrib == null)
                    //    yield return isPersian ? GetPersianTitle<T>(item) : item.ToString();

                    //else if (attrib.Browsable)
                    //    yield return isPersian ? GetPersianTitle<T>(item) : item.ToString();
                }
        }
        public static IEnumerable<KeyValuePair<int, string>> GetKeyValues<T>(bool isPersian = true, bool justBrowsable = true) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            if (!justBrowsable)
            {
                foreach (var item in Enum.GetValues(typeof(T)))
                    yield return /*isPersian ? new KeyValuePair<int, string>((int)item, GetPersianTitle<T>(item)) :*/ new KeyValuePair<int, string>((int)item, item.ToString());
            }

            else

                foreach (var item in Enum.GetValues(typeof(T)))
                {
                    FieldInfo finfo = typeof(T).GetFields().FirstOrDefault(fInfo => fInfo.Name == item.ToString());
                    BrowsableAttribute attrib = finfo.GetCustomAttributes(false).OfType<BrowsableAttribute>().FirstOrDefault();

                    if (attrib == null)
                        yield return /*isPersian ? new KeyValuePair<int, string>((int)item, GetPersianTitle<T>(item)) :*/ new KeyValuePair<int, string>((int)item, item.ToString());


                    else if (attrib.Browsable)
                        yield return /*isPersian ? new KeyValuePair<int, string>((int)item, GetPersianTitle<T>(item)) :*/ new KeyValuePair<int, string>((int)item, item.ToString());

                }
        }

        public static TEnum GetEnum<TEnum>(object value) where TEnum : struct, IConvertible
        {
            return (TEnum)GetEnum(typeof(TEnum), value);
        }

        public static object GetEnum(Type enumType, object value)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("TEnum must be an enumerated type");

            int i;
            string enumStr = value.ToString();

            if (int.TryParse(enumStr, out i))
                enumStr = Enum.GetName(enumType, i);


            return Enum.Parse(enumType, enumStr);
        }
    }
}