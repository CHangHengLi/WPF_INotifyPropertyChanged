using System.Collections.ObjectModel;
using WPFMVVMDemo.Common;
using WPFMVVMDemo.Models;

namespace WPFMVVMDemo.ViewModels
{
    /// <summary>
    /// 主视图模型类
    /// </summary>
    public class MainViewModel : NotifyPropertyBase
    {
        #region 私有字段
        private PersonViewModel _selectedPerson;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainViewModel()
        {
            // 创建并初始化人员视图模型
            PersonViewModel = new PersonViewModel();
        }
        #endregion

        #region 属性
        /// <summary>
        /// 获取人员视图模型
        /// </summary>
        public PersonViewModel PersonViewModel { get; private set; }
        #endregion
    }
} 