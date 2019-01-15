using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelValid.Class
{
    public class ConstReg
    {
        public const string PhoneReg = @"^1[3|4|5|6|7|8|9]\d{9}$";  //手机正则字符串
         
        public const string EmailReg = @"^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$"; //邮箱正则

        public const string PassworReg = @"^(?![0-9]+$)(?![a-zA-Z]+$)[0-9a-zA-Z]{6,20}$"; //密码(只包含字母与数字的6到20位字符)
    }
}