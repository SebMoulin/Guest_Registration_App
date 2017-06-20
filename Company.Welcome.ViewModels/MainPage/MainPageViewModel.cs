using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Company.Welcome.Commons;

namespace Company.Welcome.ViewModels.MainPage
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly INavigationService<ApplicationPages> _navigationService;
        private MenuItem _homeIcon;
        private MenuItem _newVisitorIcon;
        private MenuItem _settingsIcon;
        private Command<MenuItem> _navigateToHome;
        private Command<MenuItem> _navigateToNewVisitorForm;
        private Command<MenuItem> _navigateToSettings;

        public MainPageViewModel(INavigationService<ApplicationPages> navigationService)
        {
            if (navigationService == null) throw new ArgumentNullException(nameof(navigationService));
            _navigationService = navigationService;
        }

        public override async Task ViewLoaded()
        {
            await CoreWindow.GetForCurrentThread()
                .Dispatcher
                .RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    HomeIcon = new MenuItem() { Text = "Home", Page = ApplicationPages.Home, Symbol = Symbol.Home };
                    NewVisitorIcon = new MenuItem() { Text = "New", Page = ApplicationPages.NewVisitor, Symbol = Symbol.Add };
                    SettingsIcon = new MenuItem() { Text = "Settings", Page = ApplicationPages.Settings, Symbol = Symbol.Setting };

                    NavigateToHome = new Command<MenuItem>((icon) =>
                    {
                        _navigationService.ClearBackNavigationStack();
                        _navigationService.NavigateTo(icon.Page);

                    }, (icon) => true);
                    NavigateToNewVisitorForm = new Command<MenuItem>((icon) =>
                    {
                        _navigationService.ClearBackNavigationStack();
                        _navigationService.NavigateTo(icon.Page);
                    }, (icon) => true);
                    NavigateToSettings = new Command<MenuItem>((icon) =>
                    {
                        _navigationService.ClearBackNavigationStack();
                        _navigationService.NavigateTo(icon.Page);
                    }, (icon) => true);
                });
        }

        public override Task ViewUnLoaded()
        {
            return Task.Run(() => { });
        }

        public MenuItem HomeIcon
        {
            get { return _homeIcon; }
            set
            {
                SetProperty(ref _homeIcon, value);
            }
        }

        public MenuItem NewVisitorIcon
        {
            get { return _newVisitorIcon; }
            set
            {
                SetProperty(ref _newVisitorIcon, value);
            }
        }

        public MenuItem SettingsIcon
        {
            get { return _settingsIcon; }
            set
            {
                SetProperty(ref _settingsIcon, value);
            }
        }

        public Command<MenuItem> NavigateToHome
        {
            get { return _navigateToHome; }
            set
            {
                SetProperty(ref _navigateToHome, value);
            }
        }

        public Command<MenuItem> NavigateToNewVisitorForm
        {
            get { return _navigateToNewVisitorForm; }
            set
            {
                SetProperty(ref _navigateToNewVisitorForm, value);
            }
        }

        public Command<MenuItem> NavigateToSettings
        {
            get { return _navigateToSettings; }
            set
            {
                SetProperty(ref _navigateToSettings, value);
            }
        }
    }

    public class MenuItem
    {
        public Symbol Symbol { get; set; }
        public string Text { get; set; }
        public ApplicationPages Page { get; set; }
    }
}
