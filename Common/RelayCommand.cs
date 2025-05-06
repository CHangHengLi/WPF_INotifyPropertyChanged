using System;
using System.Windows.Input;

namespace WPFMVVMDemo.Common
{
    /// <summary>
    /// 实现ICommand接口的命令类，用于MVVM模式中的命令绑定
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        /// <summary>
        /// 当命令的可执行状态发生变化时触发
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// 创建新的命令实例
        /// </summary>
        /// <param name="execute">命令的执行逻辑</param>
        /// <param name="canExecute">决定命令是否可执行的逻辑，可为null</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// 确定此命令是否可在其当前状态下执行
        /// </summary>
        /// <param name="parameter">命令参数</param>
        /// <returns>如果可执行此命令，则为true；否则为false</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// 执行命令的方法
        /// </summary>
        /// <param name="parameter">命令参数</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// 手动触发CanExecuteChanged事件
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
} 