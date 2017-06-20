using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Company.Welcome.ViewModels.Home;
using Company.Welcome.ViewModels.NewVisitor;
using Company.Welcome.ViewModels.Settings;
using Company.Welcome.ViewModels.VisitorDetail;

namespace Company.Welcome.Core.Bootstrap
{
    public class BootstrapViewModels
    {
        public static void Configure(UnityContainer container)
        {
            container.RegisterType<HomePageViewModel>(new PerResolveLifetimeManager());
            container.RegisterType<NewVisitorViewModel>(new PerResolveLifetimeManager());
            container.RegisterType<VisitorDetailViewModel>(new PerResolveLifetimeManager());
            container.RegisterType<SettingsPageViewModel>(new PerResolveLifetimeManager());
        }
    }
}
