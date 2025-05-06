using System.Windows;
using WPFMVVMDemo.ViewModels;

namespace WPFMVVMDemo.Views
{
    /// <summary>
    /// AdvancedDemoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AdvancedDemoWindow : Window
    {
        public AdvancedDemoWindow()
        {
            InitializeComponent();
            
            // 设置DataContext为MainViewModel
            this.DataContext = new MainViewModel();
        }
    }
} 