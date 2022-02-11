using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MTNews.CustomControls;
using MTNews.Droid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace MTNews.Droid
{
    public class CustomEditorRenderer : EditorRenderer
    {
        public CustomEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            try
            {
                base.OnElementChanged(e);

                if (Control != null)
                {
                    GradientDrawable gd = new GradientDrawable();
                    gd.SetColor(global::Android.Graphics.Color.Transparent);
                    this.Control.SetBackgroundDrawable(gd);
                    Control.SetPadding(10, 10, 10, 10);
                    Control.TextAlignment = Android.Views.TextAlignment.Center;
                    Control.SetMaxLines(4);
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                base.OnElementPropertyChanged(sender, e);

                if (Element != null && this.Control != null)
                {
                    var entryExt = (Element as CustomEditor);

                    Control.SetCursorVisible(true);
                    if (entryExt != null && !string.IsNullOrEmpty(entryExt.Placeholder))
                    {
                        Control.Hint = entryExt.Placeholder;
                        Control.SetPadding(10, 10, 10, 10);
                    }
                    Control.SetHintTextColor(Android.Graphics.Color.DimGray);
                    Control.SetBackground(null);
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
    }
}

