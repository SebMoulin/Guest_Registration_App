﻿using System;
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
using Microsoft.Practices.ServiceLocation;
using Company.Welcome.Commons;
using Company.Welcome.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Company.Welcome.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;

        public Frame MainFrame
        {
            get { return RootFrame; }
            set { RootFrame = value; }
        }

        public MainPage()
        {
            this.InitializeComponent();
            Current = this;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            RootSplitView.IsPaneOpen = (RootSplitView.IsPaneOpen != true);
        }

        public void NotifyUser(string strMessage, NotifyType type)
        {
        }

        public enum NotifyType
        {
            StatusMessage,
            ErrorMessage
        };
    }
}
