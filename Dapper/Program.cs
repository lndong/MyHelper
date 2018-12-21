using System;
using System.Collections.Generic;
using System.Linq;
using Dapper.DAL;
using Dapper.Model;

namespace DapperSample
{
    /// <summary>
    /// Dapper简单的应用
    /// DapperExtensions是对Dapper的一个扩展，能使用谓词
    /// git地址：https://github.com/tmsmith/Dapper-Extensions
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var helper = new DapperHandle();
            #region 新增

            var list = NewUserInfo();
            //for (var i = 0; i < 2; i++)
            //{
            //    Console.WriteLine(helper.Add(list[i]));
            //}

            //for (var i = 2; i < 5; i++)
            //{
            //    helper.AddExtension(list[i]);
            //}

            //Console.WriteLine(helper.Add(list.Skip(5).ToList()));

            #endregion

            //helper.Add(list[0]);
            //helper.Add(list.Skip(8).ToList());

            Console.WriteLine(helper.GetInfo("Dapper8").UserId);
            Console.WriteLine(helper.GetInfos().Count());
            Console.WriteLine(helper.GetInfosExtension().Count());

            Console.ReadKey();
        }

        static IList<UserInfo> NewUserInfo()
        {
            var list = new List<UserInfo>();
            for (var i = 0; i < 10; i++)
            {
                var userInfo = new UserInfo
                {
                    UserId = Guid.NewGuid(),
                    UserName = "Dapper" + i,
                    Account = "Dapper" + i,
                    Password = "12345" + i,
                    Email = "12" + i + "@qq.com",
                    Phone = "1234567980" + i,
                    IsDel = 0,
                    CreateTime = DateTime.Now,
                    LastModiTime = DateTime.Now,
                };
                list.Add(userInfo);
            }
            return list;
        }
    }
}
