using System.Windows;

namespace TradingUI.Infrastructure
{
    public class UiDispatcher : IUiDispatcher
    {
        public void Invoke(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}
