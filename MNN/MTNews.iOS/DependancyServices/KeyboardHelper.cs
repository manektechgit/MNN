using Foundation;
using MTNews.Interfaces;
using MTNews.iOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(KeyboardHelper))]
namespace MTNews.iOS
{
    public class KeyboardHelper : IKeyboardHelper
    {
        public void HideKeyboard()
        {
            try
            {
                UIApplication.SharedApplication.KeyWindow.EndEditing(true);
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }

        public void ShowKeyboard()
        {
            try
            {
                UIApplication.SharedApplication.KeyWindow.EndEditing(false);
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
    }
}