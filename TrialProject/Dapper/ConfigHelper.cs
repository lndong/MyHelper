using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper
{
    public class ConfigHelper
    {
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static readonly string ConnecString =
            ConfigurationManager.ConnectionStrings["ProductEntities"].ConnectionString;
    }
}
