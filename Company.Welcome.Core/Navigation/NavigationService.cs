using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Company.Welcome.Commons;

namespace Company.Welcome.Core.Navigation
{
    public class NavigationService<TPageKey> : INavigationService<TPageKey>
    {
        private readonly IPageRegistry<TPageKey> _pageRegistry;
        private readonly Frame _mainFrame;

        public NavigationService(IPageRegistry<TPageKey> pageRegistry, Frame mainFrame)
        {
            _pageRegistry = pageRegistry;
            _mainFrame = mainFrame;
        }

        public bool NavigateTo(TPageKey page, object navigationParam = null)
        {
            var viewType = _pageRegistry.GetView(page);
            return _mainFrame.Navigate(viewType, navigationParam);
        }
        public bool CanGoBack()
        {
            return _mainFrame.CanGoBack;
        }
        public void GoBack()
        {
            _mainFrame.GoBack();
        }
        public bool CanGoForward()
        {
            return _mainFrame.CanGoForward;
        }
        public void ShowBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
        public void HideBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }
        public void ClearBackNavigationStack()
        {
            _mainFrame.BackStack.Clear();
        }
    }
}
