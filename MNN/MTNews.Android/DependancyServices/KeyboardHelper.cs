using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MTNews.Droid;
using MTNews.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(KeyboardHelper))]
namespace MTNews.Droid
{
    public class KeyboardHelper : IKeyboardHelper
    {
        public void HideKeyboard()
        {
            try
            {
                TaskCompletionSource<bool> result = new TaskCompletionSource<bool>();

                Android.Views.View view = ((MainActivity)Forms.Context).CurrentFocus;

                IBinder windowToken = view?.WindowToken;

                if (windowToken != null)
                {
                    ((InputMethodManager)Forms.Context.GetSystemService(Context.InputMethodService)).HideSoftInputFromWindow(windowToken, HideSoftInputFlags.None);
                    result.TrySetResult(true);
                }
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
                var context = MainActivity.instance;
                var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;

                if (inputMethodManager != null && context is Activity)
                {
                    var activity = context as Activity;
                    var token = activity.CurrentFocus?.WindowToken;
                    inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
    }
}