using System;

namespace Company.Welcome.ViewModels.Controls
{
    internal interface IAsyncValueViewModel : IDisposable
    {
        void ViewInitialized();
        bool IsDisposed { get; set; }
        object RefreshTrigger { get; set; }
        void Refresh();
    }
}