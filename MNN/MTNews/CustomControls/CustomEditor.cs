using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MTNews.CustomControls
{
    public class CustomEditor : Editor
    {
        public CustomEditor()
        {
            TextChanged += OnTextChanged;
        }

        ~CustomEditor()
        {
            TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                InvalidateMeasure();
            }
            catch (Exception ex)
            {
                Logger.SendErrorLog(ex);
            }
        }
    }
}
