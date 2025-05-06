using System;
using System.Windows.Input;
using WPFMVVMDemo.Common;
using WPFMVVMDemo.Models;

namespace WPFMVVMDemo.ViewModels
{
    /// <summary>
    /// 人员视图模型类
    /// </summary>
    public class PersonViewModel : NotifyPropertyBase
    {
        #region 私有字段
        private Person _person;
        private string _firstName;
        private string _lastName;
        private string _fullName;
        private int _age;
        private string _email;
        private string _phoneNumber;
        private bool _isEditMode;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public PersonViewModel()
        {
            // 创建一个默认的Person对象
            _person = new Person("三", "张", 30, "zhangsan@example.com", "13800138000");
            
            // 初始化ViewModel的属性
            UpdateViewModelProperties();
            
            // 初始化命令
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
            ResetCommand = new RelayCommand(ExecuteReset, CanExecuteReset);
            EditCommand = new RelayCommand(ExecuteEdit);
        }
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置名字
        /// </summary>
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (SetProperty(ref _firstName, value))
                {
                    UpdateFullName();
                }
            }
        }

        /// <summary>
        /// 获取或设置姓氏
        /// </summary>
        public string LastName
        {
            get => _lastName;
            set
            {
                if (SetProperty(ref _lastName, value))
                {
                    UpdateFullName();
                }
            }
        }

        /// <summary>
        /// 获取全名（只读）
        /// </summary>
        public string FullName
        {
            get => _fullName;
            private set => SetProperty(ref _fullName, value);
        }

        /// <summary>
        /// 获取或设置年龄
        /// </summary>
        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        /// <summary>
        /// 获取或设置电子邮箱
        /// </summary>
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        /// <summary>
        /// 获取或设置手机号码
        /// </summary>
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        /// <summary>
        /// 是否处于编辑模式
        /// </summary>
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }
        #endregion

        #region 命令
        /// <summary>
        /// 保存命令
        /// </summary>
        public ICommand SaveCommand { get; private set; }

        /// <summary>
        /// 重置命令
        /// </summary>
        public ICommand ResetCommand { get; private set; }

        /// <summary>
        /// 编辑命令
        /// </summary>
        public ICommand EditCommand { get; private set; }
        #endregion

        #region 私有方法
        /// <summary>
        /// 更新全名属性
        /// </summary>
        private void UpdateFullName()
        {
            FullName = $"{LastName}{FirstName}";
        }

        /// <summary>
        /// 从模型更新视图模型的属性
        /// </summary>
        private void UpdateViewModelProperties()
        {
            FirstName = _person.FirstName; // 将自动更新FullName
            LastName = _person.LastName;
            Age = _person.Age;
            Email = _person.Email;
            PhoneNumber = _person.PhoneNumber;
        }
        #endregion

        #region 命令执行方法
        /// <summary>
        /// 保存命令执行方法
        /// </summary>
        private void ExecuteSave(object parameter)
        {
            // 将视图模型的数据保存到模型
            _person.FirstName = FirstName;
            _person.LastName = LastName;
            _person.Age = Age;
            _person.Email = Email;
            _person.PhoneNumber = PhoneNumber;
            
            // 退出编辑模式
            IsEditMode = false;
        }

        /// <summary>
        /// 判断是否可以执行保存命令
        /// </summary>
        private bool CanExecuteSave(object parameter)
        {
            // 简单的验证逻辑
            return !string.IsNullOrWhiteSpace(FirstName) && 
                   !string.IsNullOrWhiteSpace(LastName) && 
                   Age > 0 && 
                   IsEditMode;
        }

        /// <summary>
        /// 重置命令执行方法
        /// </summary>
        private void ExecuteReset(object parameter)
        {
            // 重置回模型中的数据
            UpdateViewModelProperties();
        }

        /// <summary>
        /// 判断是否可以执行重置命令
        /// </summary>
        private bool CanExecuteReset(object parameter)
        {
            return IsEditMode;
        }

        /// <summary>
        /// 编辑命令执行方法
        /// </summary>
        private void ExecuteEdit(object parameter)
        {
            IsEditMode = true;
        }
        #endregion
    }
} 