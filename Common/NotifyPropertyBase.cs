using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPFMVVMDemo.Common
{
    /// <summary>
    /// 实现INotifyPropertyChanged接口的基类，为MVVM模式提供属性变更通知功能
    /// </summary>
    public abstract class NotifyPropertyBase : INotifyPropertyChanged
    {
        /// <summary>
        /// 实现INotifyPropertyChanged接口的事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性变化通知方法
        /// </summary>
        /// <param name="propertyName">变化的属性名，默认使用调用者名称</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 设置属性值并通知变化的辅助方法
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="field">字段引用</param>
        /// <param name="value">新值</param>
        /// <param name="propertyName">属性名称，默认使用调用者名称</param>
        /// <returns>如果值已更改，则为true；否则为false</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            // 检查值是否变化
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            
            // 设置新值
            field = value;
            
            // 通知属性已变化
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 通知多个属性变化
        /// </summary>
        /// <param name="propertyNames">需要通知变化的属性名称列表</param>
        protected void OnPropertiesChanged(params string[] propertyNames)
        {
            if (propertyNames == null)
                return;
                
            foreach (var name in propertyNames)
            {
                OnPropertyChanged(name);
            }
        }
    }
} 