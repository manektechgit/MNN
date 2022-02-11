using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTNews.Interfaces
{
    public interface IBaseUrl
    {
       string GetBaseUrl();
    }
    public interface IKeyboardHelper
    {
        void ShowKeyboard();
        void HideKeyboard();
    }
}
