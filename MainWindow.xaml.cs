using System.Windows;
using WPFMVVMDemo.ViewModels;

namespace WPFMVVMDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // 设置DataContext为MainViewModel
            this.DataContext = new MainViewModel();
        }
    }
} 