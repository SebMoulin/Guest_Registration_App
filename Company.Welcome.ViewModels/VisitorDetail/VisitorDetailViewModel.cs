using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Company.Welcome.Business.GuestVisitor;
using Company.Welcome.Commons;
using Company.Welcome.Entities.GuestVisitor;

namespace Company.Welcome.ViewModels.VisitorDetail
{
    public class VisitorDetailViewModel : ViewModelBase
    {
        private readonly INavigationService<ApplicationPages> _navigationService;
        private readonly ITekGuestVisitorBusinessService _tekGuestVisitorBusinessService;
        private Visitor _visitor;
        private Command _visitorIsLeavingCommand;


        public VisitorDetailViewModel(INavigationService<ApplicationPages> navigationService, ITekGuestVisitorBusinessService tekGuestVisitorBusinessService)
        {
            if (navigationService == null) throw new ArgumentNullException(nameof(navigationService));
            if (tekGuestVisitorBusinessService == null)
                throw new ArgumentNullException(nameof(tekGuestVisitorBusinessService));
            _navigationService = navigationService;
            _tekGuestVisitorBusinessService = tekGuestVisitorBusinessService;
        }

        public Visitor Visitor
        {
            get { return _visitor; }
            set { SetProperty(ref _visitor, value); }
        }

        public Command VisitorIsLeavingCommand
        {
            get { return _visitorIsLeavingCommand; }
            set { SetProperty(ref _visitorIsLeavingCommand, value); }
        }

        public override async Task ViewLoaded()
        {
            var navigationParam = NavigationParams as NavigationParams;
            if (navigationParam != null)
            {
                var visitor = navigationParam.GetNavigationParam<Visitor>("Visitor");
                Visitor = await _tekGuestVisitorBusinessService.GetGuestDetails(visitor.Id);
                VisitorIsLeavingCommand = new Command(async () =>
                {
                    var result = await _tekGuestVisitorBusinessService.GuestIsLeaving(visitor.Id, DateTime.Now);
                    if (!result) return;
                    if (_navigationService.CanGoBack())
                    {
                        _navigationService.GoBack();
                    }
                }, () => true);
            }

            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _navigationService.ShowBackButton();
            }).AsTask();
        }

        public override Task ViewUnLoaded()
        {
            return Task.Run(() => { });
        }
    }
}