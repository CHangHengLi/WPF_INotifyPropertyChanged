using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFMVVMDemo.Common
{
    /// <summary>
    /// 整数值转换器 - 安全地在字符串和整数之间转换，避免异常
    /// </summary>
    public class IntConverter : IValueConverter
    {
        /// <summary>
        /// 从源对象（整数）转换到目标对象（字符串）
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return intValue.ToString(culture);
            }
            return "0";
        }

        /// <summary>
        /// 从目标对象（字符串）转换到源对象（整数）
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            if (string.IsNullOrEmpty(strValue))
                return 0;

            // 尝试解析为整数，失败时返回0
            if (int.TryParse(strValue, NumberStyles.Integer, culture, out int result))
            {
                return result;
            }
            
            // 返回上一个有效值并避免抛出异常
            return 0;
        }
    }
} 