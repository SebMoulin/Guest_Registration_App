using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.DirectX;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Graphics.Canvas;
using Company.Welcome.Business.GuestVisitor;
using Company.Welcome.Commons;
using Company.Welcome.Entities.GuestVisitor;
using Company.Welcome.ViewModels.Home;

namespace Company.Welcome.ViewModels.NewVisitor
{
    public class NewVisitorViewModel : ViewModelBase
    {
        private readonly INavigationService<ApplicationPages> _navigationService;
        private readonly ITekGuestVisitorBusinessService _tekGuestVisitorBusinessService;
        private string _fullName;
        private string _company;
        private TimeSpan _arrival;
        private string _tekContact;

        public NewVisitorViewModel(INavigationService<ApplicationPages> navigationService, 
            ITekGuestVisitorBusinessService tekGuestVisitorBusinessService )
        {
            if (navigationService == null) throw new ArgumentNullException(nameof(navigationService));
            if (tekGuestVisitorBusinessService == null)
                throw new ArgumentNullException(nameof(tekGuestVisitorBusinessService));
            _navigationService = navigationService;
            _tekGuestVisitorBusinessService = tekGuestVisitorBusinessService;
        }

        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        public string Company
        {
            get { return _company; }
            set { SetProperty(ref _company, value); }
        }

        public TimeSpan Arrival
        {
            get { return _arrival; }
            set { SetProperty(ref _arrival, value); }
        }

        public string TekContact
        {
            get { return _tekContact; }
            set { SetProperty(ref _tekContact, value); }
        }

        public override Task ViewLoaded()
        {
            var now = DateTime.Now;
            Arrival = new TimeSpan(now.Hour, now.Minute, 0);
            return Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _navigationService.ShowBackButton();
            }).AsTask();
        }

        public override Task ViewUnLoaded()
        {
            return Task.Run(() => { });
        }

        public async Task RegisterVisitor(Signature signature, uint width, uint height)
        {
            var now = DateTime.Now;
            var visitor = new Visitor
            {
                Id = Guid.NewGuid(),
                Name = FullName,
                Arrival = new DateTime(now.Year, now.Month, now.Day, Arrival.Hours, Arrival.Minutes, 0),
                Departure = DateTime.MinValue,
                Company = Company,
                TekContact = TekContact,
                Signature = signature
            };
            await _tekGuestVisitorBusinessService.SaveGuest(visitor);
            if (_navigationService.CanGoBack())
            {
                _navigationService.GoBack();
            }
        }
    }
}