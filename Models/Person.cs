using System;

namespace WPFMVVMDemo.Models
{
    /// <summary>
    /// 人员数据模型类
    /// </summary>
    public class Person
    {
        /// <summary>
        /// 获取或设置姓氏
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 获取或设置名字
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 获取或设置年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 获取或设置电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置手机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 获取或设置出生日期
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Person()
        {
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public Person(string firstName, string lastName, int age, string email = null, string phoneNumber = null)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Email = email;
            PhoneNumber = phoneNumber;
            BirthDate = DateTime.Now.AddYears(-age);
        }
    }
} 