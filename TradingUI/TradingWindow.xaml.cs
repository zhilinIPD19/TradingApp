using System.Windows;
using TradingUI.ViewModels;

namespace TradingUI
{
    /// <summary>
    /// Interaction logic for TradingWindow.xaml
    /// </summary>
    public partial class TradingWindow : Window
    {
        public TradingWindow(TradingViewModel tradingViewModel)
        {
            InitializeComponent();
            DataContext = tradingViewModel;
        }
    }
}