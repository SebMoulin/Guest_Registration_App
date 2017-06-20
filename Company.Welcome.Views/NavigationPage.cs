using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Company.Welcome.Commons;

namespace Company.Welcome.Views
{
    public class NavigationPage : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var viewModel = this.DataContext as ViewModelBase;
            if (viewModel != null)
            {
                viewModel.NavigationParams = e.Parameter;
            }
            base.OnNavigatedTo(e);
        }
    }
}