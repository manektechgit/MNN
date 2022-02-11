    using System;
using System.Collections.Generic;
using System.Text;

namespace MTNews
{
    public static class Logger
    {
        public static void SendErrorLog(Exception ex)
        {
            try
            {
                App.Current.MainPage.DisplayAlert(Constants.Error, ex.ToString(), Constants.Ok);
            }
            catch (Exception exc)
            {

            }
        }
    }
}