using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Model;
using DapperExtensions;

namespace Dapper.DAL
{
    public class DapperHandle
    {
        private static readonly IDbConnection Conn = new SqlConnection(ConfigHelper.ConnecString);

        #region Dapper

        /// <summary>
        /// 使用dapper新增
        /// </summary>
        /// <param name="info"></param>
        /// <returns>返回影响行数</returns>
        public int Add(UserInfo info)
        {
            var sql =
                "INSERT INTO UserInfo VALUES(@UserId,@UserName,@Account,@Password,@Email,@Phone,@IsDel,@CreateTime,@LastModiTime)";
            return Conn.Execute(sql, info);
        }

        /// <summary>
        /// 使用dapper批量新增
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public int Add(IList<UserInfo> infos)
        {
            var sql =
                "INSERT INTO UserInfo VALUES(@UserId,@UserName,@Account,@Password,@Email,@Phone,@IsDel,@CreateTime,@LastModiTime)";
            return Conn.Execute(sql, infos);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int Upadate(UserInfo info)
        {
            var sql = "UPDATE UserInfo SET UserName=@UserName,Account=@Account ,LastModiTime=@LastModiTime WHERE UserId = @UserId";
            return Conn.Execute(sql, info);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int Delete(UserInfo info)
        {
            var sql = "DELETE UserInfo Where UserId = @UserId";
            return Conn.Execute(sql, new { info.UserId });
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserInfo> GetInfos()
        {
            var sql = "SELECT TOP(5) * FROM UserInfo ORDER BY CreateTime DESC";
            return Conn.Query<UserInfo>(sql);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public UserInfo GetInfo(string userName)
        {
            var sql = "SELECT * FROM UserInfo WHERE userName = @userName";
            return Conn.QueryFirstOrDefault<UserInfo>(sql, new { userName });
        }

        #endregion



        #region DapperExtension

        /// <summary>
        /// 使用DapperExtension新增
        /// </summary>
        /// <param name="info"></param>
        public void AddExtension(UserInfo info)
        {
           Conn.Insert(info); //此方法返回的是主键
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserInfo> GetInfosExtension()
        {
            var predicate = Predicates.Field<UserInfo>(x => x.IsDel, Operator.Eq, 0); //谓语条件
            var sort = Predicates.Sort<UserInfo>(x => x.CreateTime, false);
            var sortList = new List<ISort> {sort};
            return Conn.GetPage<UserInfo>(predicate, sortList, 1, 5);
        }

        #endregion
    }
}
