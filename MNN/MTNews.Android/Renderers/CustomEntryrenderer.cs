using Android.Content;
using Android.Views.InputMethods;
using MTNews.CustomControls;
using MTNews.Droid;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace MTNews.Droid
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            try
            {
                if (e.OldElement == null)
                {
                    Control.Background = null;
                    Control.LongClickable = false;
                }

                if (Control != null)
                {
                    Control.EditorAction += Control_EditorAction;
                    Control.SetPadding(0, 0, 0, 0);
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }

        private void Control_EditorAction(object sender, Android.Widget.TextView.EditorActionEventArgs e)
        {
            try
            {
                var entryExt = (Element as CustomEntry);

                if (entryExt != null)
                {
                    if (entryExt.ReturnType != ReturnType.Next)
                        entryExt.Unfocus();

                    ((IEntryController)Element).SendCompleted();
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

                if (e.PropertyName == VisualElement.IsFocusedProperty.PropertyName)
                {
                    var entryExt = (Element as CustomEntry);

                    if (entryExt != null && !entryExt.IsFocused)
                    {
                        InputMethodManager imm = (InputMethodManager)this.Context.GetSystemService(Context.InputMethodService);
                        imm.HideSoftInputFromWindow(this.Control.WindowToken, HideSoftInputFlags.None);
                    }
                    else if (entryExt != null && entryExt.IsFocused)
                    {
                        InputMethodManager imm = (InputMethodManager)this.Context.GetSystemService(Context.InputMethodService);
                        imm.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
    }
}