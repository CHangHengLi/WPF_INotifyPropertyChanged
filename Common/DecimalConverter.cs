using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFMVVMDemo.Common
{
    /// <summary>
    /// 小数值转换器 - 安全地在字符串和小数之间转换，避免异常
    /// </summary>
    public class DecimalConverter : IValueConverter
    {
        /// <summary>
        /// 从源对象（小数）转换到目标对象（字符串）
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString("F2", culture);
            }
            return "0.00";
        }

        /// <summary>
        /// 从目标对象（字符串）转换到源对象（小数）
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            if (string.IsNullOrEmpty(strValue))
                return 0m;

            // 尝试解析为小数，失败时返回0
            if (decimal.TryParse(strValue, NumberStyles.Any, culture, out decimal result))
            {
                return result;
            }
            
            // 返回上一个有效值并避免抛出异常
            return 0m;
        }
    }
} 