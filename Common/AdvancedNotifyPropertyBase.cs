using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Threading;

namespace WPFMVVMDemo.Common
{
    /// <summary>
    /// 实现高级 INotifyPropertyChanged 功能的增强基类
    /// </summary>
    public abstract class AdvancedNotifyPropertyBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region 事件

        /// <summary>
        /// 属性变化后事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性变化前事件
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region 批量更新模式

        private bool _isBatchUpdateMode = false;
        private readonly HashSet<string> _pendingNotifications = new HashSet<string>();

        /// <summary>
        /// 开始批量更新模式 - 属性变化通知会被延迟到调用 EndUpdate 方法时
        /// </summary>
        public void BeginUpdate()
        {
            _isBatchUpdateMode = true;
            _pendingNotifications.Clear();
        }

        /// <summary>
        /// 结束批量更新模式并发送所有挂起的通知
        /// </summary>
        public void EndUpdate()
        {
            _isBatchUpdateMode = false;

            // 发送所有挂起的通知
            foreach (var propertyName in _pendingNotifications)
            {
                OnPropertyChanged(propertyName);
            }

            _pendingNotifications.Clear();
        }

        /// <summary>
        /// 使用批量更新模式执行操作，确保在此过程中属性变化通知会被批量处理
        /// </summary>
        /// <param name="action">要在批量更新模式中执行的操作</param>
        public void UsingBatchMode(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            try
            {
                BeginUpdate();
                action();
            }
            finally
            {
                EndUpdate();
            }
        }

        #endregion

        #region 延迟通知

        private readonly Dictionary<string, DispatcherTimer> _delayedNotifications = new Dictionary<string, DispatcherTimer>();
        private readonly object _delayLock = new object();

        /// <summary>
        /// 延迟指定时间后触发属性变化通知，如果在延迟期间多次触发同一属性，只会发出一次通知
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="milliseconds">延迟毫秒数</param>
        protected void OnPropertyChangedDelayed(string propertyName, int milliseconds = 200)
        {
            if (string.IsNullOrEmpty(propertyName))
                return;

            // 使用锁确保线程安全
            lock (_delayLock)
            {
                // 如果已经存在该属性的延迟通知计时器，重置它
                if (_delayedNotifications.TryGetValue(propertyName, out var existingTimer))
                {
                    existingTimer.Stop();
                    existingTimer.Start();
                }
                else
                {
                    // 创建新的延迟计时器
                    var dispatcher = Dispatcher.CurrentDispatcher;
                    var timer = new DispatcherTimer(DispatcherPriority.DataBind, dispatcher)
                    {
                        Interval = TimeSpan.FromMilliseconds(milliseconds)
                    };

                    timer.Tick += (s, e) =>
                    {
                        timer.Stop();
                        lock (_delayLock)
                        {
                            _delayedNotifications.Remove(propertyName);
                        }
                        OnPropertyChanged(propertyName);
                    };

                    _delayedNotifications[propertyName] = timer;
                    timer.Start();
                }
            }
        }

        #endregion

        #region 通知方法

        /// <summary>
        /// 属性变化前通知方法
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            #if DEBUG
            // 调试模式下进行额外验证
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));
            #endif

            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// 属性变化后通知方法
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            #if DEBUG
            // 调试模式下进行额外验证
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));
            #endif

            if (_isBatchUpdateMode)
            {
                // 批量更新模式下，只记录属性名，不立即通知
                _pendingNotifications.Add(propertyName);
            }
            else
            {
                // 正常模式下，立即通知
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// 通知多个属性变化
        /// </summary>
        /// <param name="propertyNames">属性名称列表</param>
        protected void OnPropertiesChanged(params string[] propertyNames)
        {
            if (propertyNames == null || propertyNames.Length == 0)
                return;

            // 批量更新模式
            UsingBatchMode(() =>
            {
                foreach (var name in propertyNames)
                {
                    OnPropertyChanged(name);
                }
            });
        }

        /// <summary>
        /// 通知所有属性变化
        /// </summary>
        protected void OnAllPropertiesChanged()
        {
            OnPropertyChanged(string.Empty);
        }

        #endregion

        #region 属性设置方法

        /// <summary>
        /// 设置属性值并通知变化（包含变化前和变化后通知）
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="field">字段引用</param>
        /// <param name="value">新值</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>如果值已更改，则为true；否则为false</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            // 检查值是否变化
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            // 通知属性即将变化
            OnPropertyChanging(propertyName);

            // 设置新值
            field = value;

            // 通知属性已经变化
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 设置属性值并延迟通知变化
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="field">字段引用</param>
        /// <param name="value">新值</param>
        /// <param name="milliseconds">延迟毫秒数</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>如果值已更改，则为true；否则为false</returns>
        protected bool SetPropertyDelayed<T>(ref T field, T value, int milliseconds = 200, [CallerMemberName] string propertyName = null)
        {
            // 检查值是否变化
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            // 通知属性即将变化
            OnPropertyChanging(propertyName);

            // 设置新值
            field = value;

            // 延迟通知属性已经变化
            OnPropertyChangedDelayed(propertyName, milliseconds);
            return true;
        }

        /// <summary>
        /// 设置属性值，并同时通知其他相关联的属性变化
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="field">字段引用</param>
        /// <param name="value">新值</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="relatedProperties">相关联的属性名称列表</param>
        /// <returns>如果值已更改，则为true；否则为false</returns>
        protected bool SetPropertyWithDependencies<T>(ref T field, T value, string propertyName, params string[] relatedProperties)
        {
            if (!SetProperty(ref field, value, propertyName))
                return false;

            // 依次通知所有相关属性的变化
            foreach (var related in relatedProperties)
            {
                OnPropertyChanged(related);
            }

            return true;
        }

        #endregion

        #region IDisposable 支持

        private bool _disposed = false;

        /// <summary>
        /// 清理资源
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    lock (_delayLock)
                    {
                        foreach (var timer in _delayedNotifications.Values)
                        {
                            timer.Stop();
                        }
                        _delayedNotifications.Clear();
                    }
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~AdvancedNotifyPropertyBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
} 