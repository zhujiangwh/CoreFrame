using System;
using System.Collections.Generic;
using System.Text;

namespace JZT.Utility
{
    public class UtilEnum
    {
        private static void AssertTypeIsEnum(Type tp)
        {
            if (!tp.IsEnum)
            {
                throw new Exception("目标类型不是枚举");
            }
        }

        /// <summary>
        /// 获取某个枚举类型中所有的元素名称,并以String[]方式返回
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns>枚举元素名称集合</returns>
        public static string[] GetEnumKeys<T>()
        {
            AssertTypeIsEnum(typeof(T));
            return Enum.GetNames(typeof(T));
        }

        /// <summary>
        /// 获取某个枚举类型中所有的元素值,并以Array方式返回
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns>枚举值集合</returns>
        public static Array GetEnumValues<T>()
        {
            AssertTypeIsEnum(typeof(T));
            return Enum.GetValues(typeof(T));
        }

        /// <summary>
        /// 将任意对象转换为指定的枚举类型，并返回
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">转换对象</param>
        /// <returns>枚举值</returns>
        public static T GetEnumValue<T>(object value)
        {
            AssertTypeIsEnum(typeof(T));
            return GetEnumValue<T>(value, default(T));
        }

        /// <summary>
        /// 将任意对象转换为指定的枚举类型，并返回对应的Int值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">转换对象</param>
        /// <returns>Int枚举值</returns>
        public static int GetEnumValueInt<T>(object value)
        {
            AssertTypeIsEnum(typeof(T));
            return Convert.ToInt32(GetEnumValue<T>(value, default(T)));
        }

        /// <summary>
        /// 将任意对象转换为指定的枚举类型，如果转换失败则返回defaultValue值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">转换对象</param>
        /// <param name="defaultValue">转换失败之后的返回值</param>
        /// <returns>枚举值</returns>
        public static T GetEnumValue<T>(object value, T defaultValue)
        {
            AssertTypeIsEnum(typeof(T));
            try
            {
                return (T)Enum.Parse(typeof(T), Conv.NS(value));
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 根据任意对象，返回枚举类型对应的枚举名称
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">转换对象(Int类型)</param>
        /// <param name="defaultValue">转换失败之后的返回值</param>
        /// <returns>枚举项的名称</returns>
        public static string GetEnumKey<T>(object value, string defaultValue)
        {
            AssertTypeIsEnum(typeof(T));
            try
            {
                return Enum.GetName(typeof(T), value);
            }
            catch
            {
                return defaultValue;
            }
        }

    }
}
