using System;
using System.Collections.Specialized;
using System.Web.Configuration;

namespace MyHelper.Helper
{
    //自定义带out参数并返回bool值的委托
    public delegate bool CustomFunc<T>(string arg1, out T arg2) where T : struct;

    /// <summary>
    /// 配置文件appSetting节点的帮助方法
    /// </summary>
    public class AppSettingHelper
    {
        private static readonly NameValueCollection AppSetting;

        /// <summary>
        /// 构造函数
        /// </summary>
        static AppSettingHelper()
        {
            AppSetting = WebConfigurationManager.AppSettings;
        }

        #region 返回string

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的字符串类型的的值
        /// </summary>
        /// <param name="key">索引建</param>
        /// <returns>字符串值</returns>
        public static string GetString(string key)
        {
            return GetValue(key, true, null);
        }

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的字符串类型的的值
        /// 不存在此索引建时返回默认值
        /// </summary>
        /// <param name="key">索引建</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetString(string key, string defaultValue)
        {
            return GetValue(key, false, defaultValue);
        }

        #endregion

        #region 返回string集合

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的string[]类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <param name="separator">分隔符</param>
        /// <returns>字符串数组</returns>
        public static string[] GetStringArray(string key, string separator)
        {
            return GetStringArray(key, separator, true, null);
        }

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的string[]类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <param name="separator">分隔符</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字符串数组</returns>
        public static string[] GetStringArray(string key, string separator, string[] defaultValue)
        {
            return GetStringArray(key, separator, false, defaultValue);
        }

        #endregion

        #region 返回值类型(eg:int,decmail,bool)

        /// <summary>
        /// 获取配置文件appSettings集合中指定索引的值(值类型值
        /// int,decmail,bool)
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="key">索引键</param>
        /// <param name="parseValue">将指定索引键的值转化为返回类型的值的委托方法</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T GetValue<T>(string key, CustomFunc<T> parseValue, T? defaultValue) where T : struct
        {
            var value = AppSetting[key];
            if (value != null)
            {
                if (parseValue(value, out var resValue))
                {
                    return resValue;
                }
                throw new ApplicationException($"Setting '{key}' was not a valid {typeof(T).FullName}");
            }

            if (!defaultValue.HasValue)
            {
                throw new ApplicationException($"在配置文件的appSettings节点集合中找不到key为{key}的子节点，且没有指定默认值");
            }

            return defaultValue.Value;
        }

        #endregion

        #region 返回int

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的Int类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <returns></returns>
        public static int GetInt32(string key)
        {
            return GetInt32(key, null);
        }

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的Int类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>Int</returns>
        public static int GetInt(string key, int defaultValue)
        {
            return GetInt32(key, defaultValue);
        }

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的Int类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        private static int GetInt32(string key, int? defaultValue)
        {
            return GetValue(key, (string v, out int pv) => int.TryParse(v, out pv), defaultValue);
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 获取配置文件appSetting集合中指定索引的值
        /// </summary>
        /// <param name="key">索引</param>
        /// <param name="valueRequied">指定配置文件中是否必须需要配置有该名称的元素
        /// 传入False则方法返回默认值，反之则抛出异常</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        private static string GetValue(string key, bool valueRequied, string defaultValue)
        {
            var value = AppSetting[key];

            if (value != null)
            {
                return value;
            }
            if (!valueRequied)
            {
                return defaultValue;
            }
            throw new ApplicationException("在配置文件的appSettings节点集合中找不到key为" + key + "的子节点");
        }

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的string[]类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <param name="separator">分隔符</param>
        /// <param name="valueRequired">指定配置文件中是否必须需要配置有该名称的元素，传入False则方法返回默认值，反之抛出异常</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字符串数组</returns>
        private static string[] GetStringArray(string key, string separator, bool valueRequired, string[] defaultValue)
        {
            var value = GetValue(key, valueRequired, null);
            if (!string.IsNullOrEmpty(value))
            {
                return value.Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries);
            }

            if (!valueRequired)
            {
                return defaultValue;
            }

            throw new ApplicationException("在配置文件的appSettings节点集合中找不到key为" + key + "的子节点，且没有指定默认值");
        }
        #endregion
    }
}
