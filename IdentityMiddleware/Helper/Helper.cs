using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMiddleware.Helper
{
    public class Helper
    {

    }
    public class EnumValueValidation : ValidationAttribute
    {
        private string _errMsg = "";

        private readonly Type _validType;

        //private readonly bool _required;
        public EnumValueValidation(Type validType)
        {
            if (!validType.IsEnum) throw new ArgumentException("必须输入枚举类型来进行验证。");
            _validType = validType;
            //_required = required;
        }

        public override string FormatErrorMessage(string name)
        {
            return _errMsg;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var ValueStr = value?.ToString() ?? string.Empty;
            var isDefined = Enum.IsDefined(_validType, ValueStr);
            if (value == null) return ValidationResult.Success;
            if (!isDefined)
            {
                _errMsg = "输入的参数不在枚举范围内!";
                return new ValidationResult(_errMsg);
            }

            return ValidationResult.Success;
        }
    }
}
