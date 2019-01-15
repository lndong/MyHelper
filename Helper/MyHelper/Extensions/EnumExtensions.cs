using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace MyHelper.Extensions
{
    public static class EnumExtensions
    {
//        /// <summary>
//        /// 获取枚举项的描述(Description特性值)
//        /// </summary>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        public static string GetDescription(this Enum value)
//        {
//            Type enumType = value.GetType();
//            var name = Enum.GetName(enumType, value);
//            var field = enumType.GetField(name);
//            object[] arr = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
//            return arr.Length > 0 ? ((DescriptionAttribute)arr[0]).Description : name;
//        }

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
                //object[] arr = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                //var description = arr.Length > 0 ? ((DescriptionAttribute)arr[0]).Description : item;
                var description = GetDescription(field);
                dicEnum.Add((int)Enum.Parse(enumType, item),description);
            }
            return dicEnum;
        }

        private static readonly ConcurrentDictionary<Enum,string> ConcurrentDictionary = new ConcurrentDictionary<Enum, string>();

        /// <summary>
        /// 获取枚举项的描述(Description特性值)
        /// 引用至https://www.cnblogs.com/anding/p/5129178.html
        /// 性能优化后的方法
        /// </summary>
        /// <param name="this">枚举</param>
        /// <returns></returns>
        public static string GetDescription(this Enum @this)
        {
            return ConcurrentDictionary.GetOrAdd(@this, (key) =>
            {
                var type = key.GetType();
                var field = type.GetField(key.ToString());
                //return GetDescription(field);
                //如果field为null则应该是组合位域值，
                return field == null ? key.GetDescriptions() : GetDescription(field);
            });
        }

        /// <summary>
        /// 获取位域枚举的描述，多个按分隔符组合
        /// </summary>
        public static string GetDescriptions(this Enum @this, string separator = ",")
        {
            var names = @this.ToString().Split(',');
            var res = new string[names.Length];
            var type = @this.GetType();
            for (var i = 0; i < names.Length; i++)
            {
                var field = type.GetField(names[i].Trim());
                if (field == null) continue;
                res[i] = GetDescription(field);
            }
            return string.Join(separator, res);
        }

        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private static string GetDescription(FieldInfo field)
        {
            var att = System.Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute), false);
            return att == null ? field.Name : ((DescriptionAttribute)att).Description;
        }
    }
}
