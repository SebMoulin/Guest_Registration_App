using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Company.Welcome.Commons
{
    public abstract class PrintingViewModelBase : ViewModelBase
    {
        private readonly IManagePrinting _printingManager;

        protected PrintingViewModelBase(IManagePrinting printingManager)
        {
            if (printingManager == null) throw new ArgumentNullException(nameof(printingManager));
            _printingManager = printingManager;
        }

        public virtual void RegisterForPrinting()
        {
            _printingManager.RegistrationPrintManager();
        }

        public virtual async Task ShowPrintUiAsync()
        {
            await _printingManager.ShowPrintUiAsync();
        }

        public virtual void SetPrintedContent(Func<List<UIElement>> buildVisual)
        {
            _printingManager.SetContent(buildVisual);
        }

        public virtual void UnregisterForPrinting()
        {
            _printingManager.UnRegistrationPrintManager();
        }
    }
}