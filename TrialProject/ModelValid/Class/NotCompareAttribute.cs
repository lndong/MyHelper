using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ModelValid.Class
{
    /// <summary>
    /// 自定义验证规则，验证两者不能相同
    /// </summary>
    public class NotCompareAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// 要对比的属性值
        /// </summary>
        public string OtherProperty { get; set; }

        public NotCompareAttribute(string otherProperty)
        {
            OtherProperty = otherProperty;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="value">需要验证的值</param>
        /// <param name="validationContext">验证上下文</param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //从验证上下文中可以获取我们想要的属性
            var property = validationContext.ObjectType.GetProperty(OtherProperty);
            if (property == null)
            {
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, "{0}不存在", OtherProperty));
            }

            //获取属性的值
            var otherValue = property.GetValue(validationContext.ObjectInstance, null);
            if (object.Equals(value, otherValue))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return null;
        }

        /// <summary>
        /// 前端页面显示错误信息
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "notcompare", //js扩展验证规则名称，只能是小写字母
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            rule.ValidationParameters["other"] = OtherProperty;//other对应参数的key
            yield return rule;
        }
    }
}