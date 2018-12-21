using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Model
{
    /// <summary>
    /// 实体类
    /// </summary>
    public class UserInfo
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Account { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public byte IsDel { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastModiTime { get; set; }
    }
}
