using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Unity;
using Company.Welcome.Commons;
using Company.Welcome.Core.Navigation;

namespace Company.Welcome.Core.Bootstrap
{
    public class BootstrapNavigationService
    {
        public static void Configure(UnityContainer container, Frame mainFrame)
        {
            var pageRegistry = container.Resolve<IPageRegistry<ApplicationPages>>();
            var navigationSevice = new NavigationService<ApplicationPages>(pageRegistry, mainFrame);
            container.RegisterInstance<INavigationService<ApplicationPages>>(navigationSevice);
        }
    }
}
