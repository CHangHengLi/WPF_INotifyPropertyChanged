using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFMVVMDemo.Common;
using WPFMVVMDemo.Models;

namespace WPFMVVMDemo.ViewModels
{
    /// <summary>
    /// 产品视图模型，演示高级 INotifyPropertyChanged 功能
    /// </summary>
    public class ProductViewModel : AdvancedNotifyPropertyBase, IDisposable
    {
        #region 私有字段
        private Product _product;
        private string _name;
        private decimal _price;
        private int _quantity;
        private string _description;
        private bool _isInStock;
        private decimal _totalValue;
        private string _formattedPrice;
        private string _statusMessage;
        private bool _isBusy;
        private int _updateCounter = 0;
        private object _syncRoot = new object();
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductViewModel()
        {
            // 创建默认产品
            _product = new Product(1, "示例产品", 99.99m, 10, "电子产品", "这是一个示例产品，用于展示高级属性通知功能。");
            
            // 初始化视图模型属性
            LoadFromModel();
            
            // 初始化命令
            SaveCommand = new RelayCommand(async p => await ExecuteSaveAsync(), CanExecuteSave);
            UpdateAllCommand = new RelayCommand(async p => await ExecuteUpdateAllAsync());
            ResetCommand = new RelayCommand(async p => await ExecuteResetAsync());
            BatchUpdateCommand = new RelayCommand(async p => await ExecuteBatchUpdateAsync());
            DelayUpdateCommand = new RelayCommand(async p => await ExecuteDelayUpdateAsync());
        }
        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置产品名称
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetPropertyWithDependencies(ref _name, value, nameof(Name), nameof(StatusMessage));
        }

        /// <summary>
        /// 获取或设置产品价格
        /// </summary>
        public decimal Price
        {
            get => _price;
            set
            {
                try
                {
                    // 验证价格是否合法（必须大于等于0）
                    if (value < 0)
                    {
                        throw new ArgumentException("价格不能为负数");
                    }
                    
                    if (SetProperty(ref _price, value))
                    {
                        OnPropertyChanging(nameof(TotalValue));
                        OnPropertyChanging(nameof(FormattedPrice));
                        
                        // 更新相关联的属性
                        UpdateTotalValue();
                        UpdateFormattedPrice();
                    }
                }
                catch (Exception ex)
                {
                    UpdateStatusMessage($"价格设置错误: {ex.Message}");
                    throw; // 重新抛出异常，使绑定系统能捕获并显示错误
                }
            }
        }

        /// <summary>
        /// 获取或设置产品数量
        /// </summary>
        public int Quantity
        {
            get => _quantity;
            set
            {
                try
                {
                    // 验证数量是否合法（必须大于等于0）
                    if (value < 0)
                    {
                        throw new ArgumentException("数量不能为负数");
                    }
                    
                    if (SetProperty(ref _quantity, value))
                    {
                        // 更新相关联的属性
                        UpdateTotalValue();
                        UpdateIsInStock();
                    }
                }
                catch (Exception ex)
                {
                    UpdateStatusMessage($"数量设置错误: {ex.Message}");
                    throw; // 重新抛出异常，使绑定系统能捕获并显示错误
                }
            }
        }

        /// <summary>
        /// 获取或设置产品描述 (使用延迟通知)
        /// </summary>
        public string Description
        {
            get => _description;
            set => SetPropertyDelayed(ref _description, value, 500);
        }

        /// <summary>
        /// 获取或设置产品是否有货
        /// </summary>
        public bool IsInStock
        {
            get => _isInStock;
            private set => SetProperty(ref _isInStock, value);
        }

        /// <summary>
        /// 获取产品总价值 (只读，依赖于 Price 和 Quantity)
        /// </summary>
        public decimal TotalValue
        {
            get => _totalValue;
            private set => SetProperty(ref _totalValue, value);
        }

        /// <summary>
        /// 获取格式化后的价格 (只读，依赖于 Price)
        /// </summary>
        public string FormattedPrice
        {
            get => _formattedPrice;
            private set => SetProperty(ref _formattedPrice, value);
        }

        /// <summary>
        /// 获取状态消息
        /// </summary>
        public string StatusMessage
        {
            get => _statusMessage;
            private set => SetProperty(ref _statusMessage, value);
        }

        /// <summary>
        /// 获取或设置是否忙碌
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            private set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region 命令
        /// <summary>
        /// 保存命令
        /// </summary>
        public ICommand SaveCommand { get; private set; }

        /// <summary>
        /// 更新所有属性命令
        /// </summary>
        public ICommand UpdateAllCommand { get; private set; }

        /// <summary>
        /// 重置命令
        /// </summary>
        public ICommand ResetCommand { get; private set; }

        /// <summary>
        /// 批量更新命令
        /// </summary>
        public ICommand BatchUpdateCommand { get; private set; }

        /// <summary>
        /// 延迟更新命令
        /// </summary>
        public ICommand DelayUpdateCommand { get; private set; }
        #endregion

        #region 私有方法
        /// <summary>
        /// 从模型加载数据到视图模型
        /// </summary>
        private void LoadFromModel()
        {
            Name = _product.Name;
            Price = _product.Price;
            Quantity = _product.Quantity;
            Description = _product.Description;
            IsInStock = _product.IsInStock;
            
            UpdateTotalValue();
            UpdateFormattedPrice();
            
            StatusMessage = "数据已加载";
        }

        /// <summary>
        /// 将视图模型数据保存到模型
        /// </summary>
        private void SaveToModel()
        {
            _product.Name = Name;
            _product.Price = Price;
            _product.Quantity = Quantity;
            _product.Description = Description;
            _product.IsInStock = IsInStock;
        }

        /// <summary>
        /// 更新总价值
        /// </summary>
        private void UpdateTotalValue()
        {
            TotalValue = Price * Quantity;
        }

        /// <summary>
        /// 更新格式化价格
        /// </summary>
        private void UpdateFormattedPrice()
        {
            FormattedPrice = $"￥{Price:N2}";
        }

        /// <summary>
        /// 更新库存状态
        /// </summary>
        private void UpdateIsInStock()
        {
            IsInStock = Quantity > 0;
        }

        /// <summary>
        /// 更新状态消息
        /// </summary>
        private void UpdateStatusMessage(string message)
        {
            lock (_syncRoot)
            {
                _updateCounter++;
                StatusMessage = $"[{_updateCounter}] {message}";
            }
        }
        
        /// <summary>
        /// 判断是否可以执行保存命令
        /// </summary>
        private bool CanExecuteSave(object parameter)
        {
            // 验证数据有效性
            return !string.IsNullOrWhiteSpace(Name) && 
                   Price >= 0 && 
                   Quantity >= 0;
        }
        #endregion

        #region 命令执行方法
        /// <summary>
        /// 执行保存命令
        /// </summary>
        private async Task ExecuteSaveAsync()
        {
            try
            {
                IsBusy = true;
                UpdateStatusMessage("正在保存...");
                
                // 模拟操作延迟
                await Task.Delay(1500);
                
                SaveToModel();
                UpdateStatusMessage("保存成功！");
            }
            catch (Exception ex)
            {
                UpdateStatusMessage($"保存失败: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// 执行更新所有属性命令（使用空字符串通知所有属性变化）
        /// </summary>
        private async Task ExecuteUpdateAllAsync()
        {
            try
            {
                IsBusy = true;
                UpdateStatusMessage("正在更新所有属性...");
                
                // 模拟操作延迟
                await Task.Delay(800);
                
                OnAllPropertiesChanged();
                UpdateStatusMessage("所有属性已更新！");
            }
            catch (Exception ex)
            {
                UpdateStatusMessage($"更新失败: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// 执行重置命令
        /// </summary>
        private async Task ExecuteResetAsync()
        {
            try
            {
                IsBusy = true;
                UpdateStatusMessage("正在重置...");
                
                // 模拟操作延迟
                await Task.Delay(1000);
                
                LoadFromModel();
                UpdateStatusMessage("已重置为原始数据！");
            }
            catch (Exception ex)
            {
                UpdateStatusMessage($"重置失败: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// 执行批量更新命令（演示批量更新模式）
        /// </summary>
        private async Task ExecuteBatchUpdateAsync()
        {
            try
            {
                IsBusy = true;
                UpdateStatusMessage("开始批量更新...");
                
                // 模拟操作延迟
                await Task.Delay(1000);
                
                // 使用批量更新模式进行多个属性更新
                UsingBatchMode(() =>
                {
                    // 先将名称重置为原始产品名称，去除可能存在的"(特惠版)"后缀
                    string baseName = _product.Name;
                    
                    // 同时修改多个属性，但只会在结束时发送一次通知
                    Price = Price * 1.1m;  // 提高10%价格
                    Quantity = Quantity + 5;  // 增加5个库存
                    Name = $"{baseName} (特惠版)";
                });
                
                UpdateStatusMessage("批量更新完成！");
            }
            catch (Exception ex)
            {
                UpdateStatusMessage($"批量更新失败: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// 执行延迟更新命令（演示延迟通知）
        /// </summary>
        private async Task ExecuteDelayUpdateAsync()
        {
            try
            {
                IsBusy = true;
                UpdateStatusMessage("开始延迟更新 (查看描述字段变化)...");
                
                // 模拟多次快速更新同一属性，但只会在最后一次触发通知
                for (int i = 1; i <= 5; i++)
                {
                    Description = $"更新中... ({i}/5)";
                    await Task.Delay(200);  // 模拟快速连续更新
                }
                
                Description = $"更新于 {DateTime.Now:HH:mm:ss}";
                UpdateStatusMessage("延迟更新完成！");
            }
            catch (Exception ex)
            {
                UpdateStatusMessage($"延迟更新失败: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion
    }
} 