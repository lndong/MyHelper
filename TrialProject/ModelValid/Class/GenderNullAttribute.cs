using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ModelValid.Class
{
    public class GenderNullAttribute: ValidationAttribute, IClientValidatable
    {
        public override bool IsValid(object value)
        {
            return !Equals(value, "0");
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rules = new ModelClientValidationRule()
            {
                ValidationType = "gendernull", //js扩展验证规则名称，只能是小写字母
                ErrorMessage = FormatErrorMessage(metadata.DisplayName)
            };
            yield return rules;
        }
    }
}