using System;
using System.Collections.Generic;
using System.Text;

namespace TradingUI.Infrastructure
{
    public interface IUiDispatcher
    {
        void Invoke(Action action);
    }
}
