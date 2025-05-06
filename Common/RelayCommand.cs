using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFMVVMDemo.Common
{
    /// <summary>
    /// 实现ICommand接口的命令类，用于MVVM模式中的命令绑定
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Func<object, Task> _asyncExecute;
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;
        private bool _isExecuting;

        /// <summary>
        /// 当命令的可执行状态发生变化时触发
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// 创建新的命令实例（同步）
        /// </summary>
        /// <param name="execute">命令的执行逻辑</param>
        /// <param name="canExecute">决定命令是否可执行的逻辑，可为null</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _asyncExecute = null;
            _canExecute = canExecute;
        }

        /// <summary>
        /// 创建新的命令实例（异步）
        /// </summary>
        /// <param name="asyncExecute">命令的异步执行逻辑</param>
        /// <param name="canExecute">决定命令是否可执行的逻辑，可为null</param>
        public RelayCommand(Func<object, Task> asyncExecute, Predicate<object> canExecute = null)
        {
            _asyncExecute = asyncExecute ?? throw new ArgumentNullException(nameof(asyncExecute));
            _execute = null;
            _canExecute = canExecute;
        }

        /// <summary>
        /// 确定此命令是否可在其当前状态下执行
        /// </summary>
        /// <param name="parameter">命令参数</param>
        /// <returns>如果可执行此命令，则为true；否则为false</returns>
        public bool CanExecute(object parameter)
        {
            return !_isExecuting && (_canExecute == null || _canExecute(parameter));
        }

        /// <summary>
        /// 执行命令的方法
        /// </summary>
        /// <param name="parameter">命令参数</param>
        public void Execute(object parameter)
        {
            if (_isExecuting)
                return;

            if (_asyncExecute != null)
            {
                ExecuteAsync(parameter);
            }
            else
            {
                _execute(parameter);
            }
        }

        /// <summary>
        /// 异步执行命令
        /// </summary>
        /// <param name="parameter">命令参数</param>
        private async void ExecuteAsync(object parameter)
        {
            _isExecuting = true;
            RaiseCanExecuteChanged();

            try
            {
                await _asyncExecute(parameter);
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
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