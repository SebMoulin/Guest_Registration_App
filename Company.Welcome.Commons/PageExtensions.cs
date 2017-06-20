using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;

namespace Company.Welcome.Commons
{
    public static class PageExtensions
    {
        public static void LoadViewModel<TPage>(this Page page)
        {
            var serviceLocator = ServiceLocator.Current;
            if (page == null) throw new ArgumentNullException("page");
            if (serviceLocator == null) throw new NullReferenceException("ServiceLocator not initialized");

            var pageRegistry = serviceLocator.GetInstance<IPageRegistry<TPage>>();
            var viewModelInstance = pageRegistry.GetViewModel(page.GetType());
            if (viewModelInstance != null)
            {
                RoutedEventHandler viewInstanceOnLoaded = async delegate (object sender, RoutedEventArgs args)
                {
                    await viewModelInstance.ViewLoaded();
                };
                page.Loaded += viewInstanceOnLoaded;
                RoutedEventHandler viewInstanceOnUnloaded = async delegate (object sender, RoutedEventArgs args)
                {
                    await viewModelInstance.ViewUnLoaded();
                };
                page.Unloaded += viewInstanceOnUnloaded;
                page.DataContext = viewModelInstance;
            }
        }
    }
}
