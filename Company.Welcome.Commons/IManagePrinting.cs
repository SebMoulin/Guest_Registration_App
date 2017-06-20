using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Company.Welcome.Commons
{
    public interface IManagePrinting
    {
        Task ShowPrintUiAsync();
        void SetContent(Func<List<UIElement>> buildVisual);
        void RegistrationPrintManager();
        void UnRegistrationPrintManager();
    }
}