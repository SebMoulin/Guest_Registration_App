using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Company.Welcome.Commons;
using Company.Welcome.Core.Navigation;

namespace Company.Welcome.Core.Bootstrap
{
    public class BootstrapPageRegistry
    {
        public static void Configure(UnityContainer container)
        {
            var pageRegistry = new PageRegistry<ApplicationPages>(ServiceLocator.Current.GetInstance<ILogger>());
            pageRegistry.RegisterPage(ApplicationPages.MainPage,
                typeof(Company.Welcome.Views.MainPage),
                typeof(Company.Welcome.ViewModels.MainPage.MainPageViewModel));

            pageRegistry.RegisterPage(ApplicationPages.Home,
                typeof(Company.Welcome.Views.Home.HomePage),
                typeof(Company.Welcome.ViewModels.Home.HomePageViewModel));

            pageRegistry.RegisterPage(ApplicationPages.NewVisitor,
                typeof(Company.Welcome.Views.NewVisitor.NewVisitorPage),
                typeof(Company.Welcome.ViewModels.NewVisitor.NewVisitorViewModel));

            pageRegistry.RegisterPage(ApplicationPages.VisitorDetail,
                typeof(Company.Welcome.Views.VisitorDetail.VisitorDetailPage),
                typeof(Company.Welcome.ViewModels.VisitorDetail.VisitorDetailViewModel));

            pageRegistry.RegisterPage(ApplicationPages.Settings,
                typeof(Company.Welcome.Views.Settings.SettingsPage),
                typeof(Company.Welcome.ViewModels.Settings.SettingsPageViewModel));

            container.RegisterInstance<IPageRegistry<ApplicationPages>>(pageRegistry, new ContainerControlledLifetimeManager());
        }
    }
}
