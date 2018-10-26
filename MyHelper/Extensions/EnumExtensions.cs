using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MyHelper.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举项的描述(Description特性值)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            Type enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            var field = enumType.GetField(name);
            object[] arr = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return arr.Length > 0 ? ((DescriptionAttribute)arr[0]).Description : name;
        }

        /// <summary>
        /// 枚举转字典集合
        /// </summary>
        /// <param name="value">枚举</param>
        /// <param name="keyDefault">默认key</param>
        /// <param name="valueDefault">默认value</param>
        /// <returns>枚举int值为key，Description描述为value</returns>
        public static Dictionary<object,object> EnumListDic(this Enum value ,string keyDefault,string valueDefault = "")
        {
            var dicEnum = new Dictionary<object, object>();
            Type enumType = value.GetType();

            if (!string.IsNullOrEmpty(keyDefault)) //获取是否添加默认项
            {
                dicEnum.Add(keyDefault, valueDefault);
            }
            var nameStrs = Enum.GetNames(enumType);
            foreach (var item in nameStrs)
            {
                var field = enumType.GetField(item);
                object[] arr = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                var description = arr.Length > 0 ? ((DescriptionAttribute)arr[0]).Description : item;
                dicEnum.Add((int)Enum.Parse(enumType, item),description);
            }
            return dicEnum;
        }
    }
}
