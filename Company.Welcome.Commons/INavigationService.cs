namespace Company.Welcome.Commons
{
    public interface INavigationService<in TPageKey>
    {
        bool NavigateTo(TPageKey page, object navigationParam = null);
        bool CanGoBack();
        void GoBack();
        bool CanGoForward();
        void ShowBackButton();
        void HideBackButton();
        void ClearBackNavigationStack();
    }
}