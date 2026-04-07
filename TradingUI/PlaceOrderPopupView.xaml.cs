using System.Windows;
using TradingUI.ViewModels;

namespace TradingUI
{
    /// <summary>
    /// Interaction logic for PlaceOrderPopupView.xaml
    /// </summary>
    public partial class PlaceOrderPopupView : Window
    {
        public PlaceOrderPopupView(PlaceOrderPopupViewModel placeOrderViewModel)
        {
            InitializeComponent();
            DataContext = placeOrderViewModel;
            placeOrderViewModel.RequestClose += () => this.Close();
        }
    }
}
