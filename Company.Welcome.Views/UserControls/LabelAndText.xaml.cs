using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Company.Welcome.Views.UserControls
{
    public sealed partial class LabelAndText : UserControl
    {
        public LabelAndText()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty LabelLocalizedValueKeyProperty = DependencyProperty.Register(
            "LabelLocalizedValueKey", typeof(string), typeof(LabelAndText), new PropertyMetadata(default(string)));

        public string LabelLocalizedValueKey
        {
            get { return (string) GetValue(LabelLocalizedValueKeyProperty); }
            set { SetValue(LabelLocalizedValueKeyProperty, value); }
        }

        public static readonly DependencyProperty TextValueProperty = DependencyProperty.Register(
            "TextValue", typeof(string), typeof(LabelAndText), new PropertyMetadata(default(string)));

        public string TextValue
        {
            get { return (string) GetValue(TextValueProperty); }
            set { SetValue(TextValueProperty, value); }
        }
    }
}
