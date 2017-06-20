using System;
using Windows.UI.Xaml.Controls;

namespace Company.Welcome.Commons
{
    public interface IPageRegistry<TPageKey>
    {
        void RegisterPage(TPageKey pageKey, Type viewType, Type viewModelType);
        ViewModelBase GetViewModel(Type pageType);

        Type GetView(TPageKey pageKey);
    }
}