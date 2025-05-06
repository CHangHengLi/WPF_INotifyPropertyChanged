using System;
using System.Globalization;
using System.Windows.Controls;

namespace WPFMVVMDemo.Common
{
    /// <summary>
    /// 数值验证规则 - 验证输入是否为有效的数值
    /// </summary>
    public class NumericValidationRule : ValidationRule
    {
        /// <summary>
        /// 获取或设置是否允许小数
        /// </summary>
        public bool AllowDecimal { get; set; } = true;

        /// <summary>
        /// 获取或设置最小值
        /// </summary>
        public double Minimum { get; set; } = double.MinValue;

        /// <summary>
        /// 获取或设置最大值
        /// </summary>
        public double Maximum { get; set; } = double.MaxValue;

        /// <summary>
        /// 验证输入值是否有效
        /// </summary>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = value as string;

            // 空值检查
            if (string.IsNullOrEmpty(strValue))
                return new ValidationResult(false, "请输入数值");

            // 尝试解析数值
            if (AllowDecimal)
            {
                // 检查是否为有效的小数
                if (!decimal.TryParse(strValue, out decimal decimalValue))
                    return new ValidationResult(false, "请输入有效的数字");

                // 检查范围
                double doubleValue = (double)decimalValue;
                if (doubleValue < Minimum)
                    return new ValidationResult(false, $"数值不能小于 {Minimum}");
                if (doubleValue > Maximum)
                    return new ValidationResult(false, $"数值不能大于 {Maximum}");
            }
            else
            {
                // 检查是否为有效的整数
                if (!int.TryParse(strValue, out int intValue))
                    return new ValidationResult(false, "请输入有效的整数");

                // 检查范围
                if (intValue < Minimum)
                    return new ValidationResult(false, $"数值不能小于 {Minimum}");
                if (intValue > Maximum)
                    return new ValidationResult(false, $"数值不能大于 {Maximum}");
            }

            // 验证通过
            return ValidationResult.ValidResult;
        }
    }
} 