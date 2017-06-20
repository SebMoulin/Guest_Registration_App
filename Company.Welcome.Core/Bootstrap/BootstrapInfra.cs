using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Company.Welcome.Business;
using Company.Welcome.Commons;
using Company.Welcome.Commons.Notifications;
using Company.Welcome.Ral;

namespace Company.Welcome.Core.Bootstrap
{
    public class BootstrapInfra
    {
        public static void Configure(UnityContainer container)
        {
            var mapper = new ApplicationMapping();
            mapper.Configure();
            container.RegisterInstance<IApplicationMapping>(mapper, new ContainerControlledLifetimeManager());
            container.RegisterType<ILogger, Logger>();
            container.RegisterType<ISerializer, Serializer>();
            container.RegisterType<IHttpRequestHandler, HttpRequestHandler>();
            container.RegisterType<IToastNotificationFactory, ToastNotificationFactory>();
            container.RegisterType<ISettingsProvider, SettingsProvider>();
            container.RegisterType<IProvideStorage, LocalFolderStorageProvider>();
            container.RegisterType<IProvideLocalizedString, LocalizedStringProvider>();
            container.RegisterType<IEncodingHelper, EncodingHelper>();
            container.RegisterType<IProvideDateTime, DateTimeProvider>();
        }
    }
}
