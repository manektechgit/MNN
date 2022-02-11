using Foundation;
using MTNews.Interfaces;
using MTNews.iOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseURL))]
namespace MTNews.iOS
{
    public class BaseURL : IBaseUrl
    {
        public string GetBaseUrl()
        {
            return NSBundle.MainBundle.BundlePath;
        }
    }
}