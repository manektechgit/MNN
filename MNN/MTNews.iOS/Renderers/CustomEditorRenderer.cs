using CoreGraphics;
using Foundation;
using MTNews.CustomControls;
using MTNews.iOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace MTNews.iOS
{
    public class CustomEditorRenderer : EditorRenderer
    {
        private string Placeholder { get; set; }
        double previousHeight = -1;
        int prevLines = 0;

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            try
            {
                base.OnElementChanged(e);

                if (Element != null)
                {
                    var element = this.Element as CustomEditor;

                    if (Control != null && element != null)
                    {
                        Control.ScrollEnabled = true;
                        Control.BackgroundColor = Color.Transparent.ToUIColor();

                        Placeholder = element.Placeholder;
                        this.Control.InputAccessoryView = null;
                        Control.SpellCheckingType = UITextSpellCheckingType.No;
                        Control.AutocorrectionType = UITextAutocorrectionType.No;
                        if (Control.Text == "")
                        {
                            Control.Text = Placeholder;
                        }
                        Control.ShouldBeginEditing += (UITextView textView) =>
                        {
                            if (textView.Text == Placeholder)
                            {
                                textView.Text = "";
                            }

                            return true;
                        };
                        Control.ShouldEndEditing += (UITextView textView) =>
                        {
                            if (textView.Text == "")
                            {
                                textView.Text = Placeholder;
                            }
                            return true;
                        };
                        if (!element.IsEnabled)
                        {
                            element.IsEnabled = true;
                            Control.Editable = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                base.OnElementPropertyChanged(sender, e);

                if (Element != null)
                {
                    var customControl = (CustomEditor)Element;
                    if (customControl != null)
                    {
                        if (e.PropertyName == Editor.TextProperty.PropertyName)
                        {

                            CGSize size = Control.Text.StringSize(Control.Font, Control.Frame.Size, UILineBreakMode.WordWrap);

                            int numLines = (int)(size.Height / Control.Font.LineHeight);

                            if (prevLines > numLines)
                            {
                                customControl.HeightRequest = -1;

                            }
                            else if (string.IsNullOrEmpty(Control.Text))
                            {
                                customControl.HeightRequest = -1;
                            }

                            prevLines = numLines;
                        }
                        else if (CustomEditor.HeightProperty.PropertyName == e.PropertyName)
                        {
                            CGSize size = Control.Text.StringSize(Control.Font, Control.Frame.Size, UILineBreakMode.WordWrap);

                            int numLines = (int)(size.Height / Control.Font.LineHeight);
                            if (numLines >= 4)
                            {
                                Control.ScrollEnabled = true;
                                customControl.HeightRequest = previousHeight;
                            }
                            else
                            {
                                Control.ScrollEnabled = false;
                                previousHeight = customControl.Height;
                            }
                        }
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
