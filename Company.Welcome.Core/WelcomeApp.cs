using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Unity;
using Company.Welcome.Core.Bootstrap;

namespace Company.Welcome.Core
{
    public static class WelcomeApp
    {
        public static void Start(UnityContainer container)
        {
            BootstrapInfra.Configure(container);
            BootstrapRal.Configure(container);
            BootstrapBusiness.Configure(container);
            BootstrapViewModels.Configure(container);
            BootstrapPageRegistry.Configure(container);
        }
    }
}