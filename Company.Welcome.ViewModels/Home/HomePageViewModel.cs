using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Data.Xml.Dom;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using LinqToTwitter;
using Company.Welcome.Business;
using Company.Welcome.Business.GuestVisitor;
using Company.Welcome.Commons;
using Company.Welcome.Entities.GuestVisitor;

namespace Company.Welcome.ViewModels.Home
{
    public class HomePageViewModel : ViewModelBase
    {
        private readonly INavigationService<ApplicationPages> _navigationService;
        private readonly ITekGuestVisitorBusinessService _tekGuestVisitorBusinessService;
        private ObservableCollection<Visitor> _visitors;
        private Visitor _gridSelectedItem;
        private Command<Visitor> _selectionChangedCommand;

        public HomePageViewModel(INavigationService<ApplicationPages> navigationService,
            ITekGuestVisitorBusinessService tekGuestVisitorBusinessService)
        {
            if (navigationService == null) throw new ArgumentNullException(nameof(navigationService));
            if (tekGuestVisitorBusinessService == null)
                throw new ArgumentNullException(nameof(tekGuestVisitorBusinessService));
            _navigationService = navigationService;
            _tekGuestVisitorBusinessService = tekGuestVisitorBusinessService;
        }

        public Command<Visitor> SelectionChangedCommand
        {
            get { return _selectionChangedCommand; }
            set { SetProperty(ref _selectionChangedCommand, value); }
        }

        public override async Task ViewLoaded()
        {
            SelectionChangedCommand = new Command<Visitor>((visitor) =>
            {
                var navigationParam = new NavigationParams();
                navigationParam.AddParam("Visitor", visitor);
                _navigationService.NavigateTo(ApplicationPages.VisitorDetail, navigationParam);
            }, visitor => true);
            var collection = await _tekGuestVisitorBusinessService.GetAllGuest(DateTime.Today);
            Visitors = new ObservableCollection<Visitor>(collection);

            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _navigationService.HideBackButton();
            }).AsTask();
        }

        public ObservableCollection<Visitor> Visitors
        {
            get { return _visitors; }
            set
            {
                SetProperty(ref _visitors, value);
            }
        }

        public Visitor GridSelectedItem
        {
            get { return _gridSelectedItem; }
            set
            {
                SetProperty(ref _gridSelectedItem, value);
                _navigationService.NavigateTo(ApplicationPages.VisitorDetail, value);
            }
        }

        public override Task ViewUnLoaded()
        {
            return Task.Run(() => { });
        }
    }
}
