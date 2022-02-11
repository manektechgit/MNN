using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MTNews.Droid;
using MTNews.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseURL))]
namespace MTNews.Droid
{
    public class BaseURL : IBaseUrl
    {
        //https://stackoverflow.com/questions/45100122/loading-local-html-with-webview-xamarin-forms/51431621
        public string GetBaseUrl()
        {
            return "file:///android_asset/";
        }
    }
}