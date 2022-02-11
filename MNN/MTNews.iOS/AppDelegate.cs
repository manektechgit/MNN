using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace MTNews.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rg.Plugins.Popup.Popup.Init();

            global::Xamarin.Forms.Forms.Init();

            ImageCircleRenderer.Init();

            SetStatusBar();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public void SetStatusBar()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                    {
                        UIView statusBar = new UIView(UIApplication.SharedApplication.KeyWindow.WindowScene.StatusBarManager.StatusBarFrame);
                        statusBar.BackgroundColor = UIColor.FromRGB(238, 66, 84);
                        statusBar.TintColor = UIColor.White;
                        UIApplication.SharedApplication.KeyWindow.AddSubview(statusBar);
                    }
                    else
                    {
                        UIView statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
                        if (statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
                        {
                            statusBar.BackgroundColor = UIColor.FromRGB(238, 66, 84);
                            statusBar.TintColor = UIColor.White;
                        }
                    }
                    UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
                    //GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
                }
                catch (Exception ex)
                {

                }
            });
        }
    }
}
