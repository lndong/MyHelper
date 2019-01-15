using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ModelValid.Class;

namespace ModelValid.Models
{
    public class UserInfo
    {
        [Display(Name ="用户名")]
        [Required(ErrorMessage ="请输入用户名")]
        [Remote("ValidAccount","User","",ErrorMessage = "此账号已存在")]
        public string Account { get; set; }

        [Display(Name ="密码")]
        [Required(ErrorMessage ="请输入密码")]
        [RegularExpression(ConstReg.PassworReg,ErrorMessage = "只包含字母与数字的6到20位字符")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="姓名")]
        [Required(ErrorMessage = "请输入姓名")]
        public string UserName { get; set; }

        [Display(Name ="手机号码")]
        [Required(ErrorMessage = "请输入手机号码")]
        [RegularExpression(ConstReg.PhoneReg,ErrorMessage = "手机号格式不正确")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name ="邮箱")]
        [Required(ErrorMessage = "请输入邮箱")]
        [RegularExpression(ConstReg.EmailReg,ErrorMessage = "邮箱格式不正确")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "性别")]
        [GenderNull(ErrorMessage = "请选择性别")]
        public GenderType Gender { get; set; }

        [Display(Name = "验证忽略")]
        [RegularExpression(ConstReg.PhoneReg, ErrorMessage = "手机号格式不正确")]
        public string IgnorePhone { get; set; }
    }

    public enum GenderType
    {
        [Description("男")] Male = 1,
        [Description("女")] Female = 2
    }
}