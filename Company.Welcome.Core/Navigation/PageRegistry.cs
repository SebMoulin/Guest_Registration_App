using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using Company.Welcome.Commons;

namespace Company.Welcome.Core.Navigation
{
    public class PageRegistry<TPageKey> : IPageRegistry<TPageKey>
    {
        private readonly ILogger _logger;
        private readonly IDictionary<Type, TPageKey> _views;
        private readonly IDictionary<TPageKey, Type> _viewTypes;
        private readonly IDictionary<TPageKey, Type> _viewModels;

        public PageRegistry(ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
            _views = new Dictionary<Type, TPageKey>();
            _viewTypes = new Dictionary<TPageKey, Type>();
            _viewModels = new Dictionary<TPageKey, Type>();
        }

        public void RegisterPage(TPageKey pageKey, Type viewType, Type viewModelType)
        {
            if (!_viewTypes.ContainsKey(pageKey))
            {
                _viewTypes.Add(pageKey, viewType);
            }
            if (!_views.ContainsKey(viewType))
            {
                _views.Add(viewType, pageKey);
            }
            if (!_viewModels.ContainsKey(pageKey))
            {
                _viewModels.Add(pageKey, viewModelType);
            }
        }

        public ViewModelBase GetViewModel(Type pageType)
        {
            var pageKey = _views[pageType];
            if (!_viewModels.ContainsKey(pageKey))
            {
                var ex = new KeyNotFoundException($"ViewModel not found for page {pageKey}");
                _logger.Log(ex);
                throw ex;
            }
            var viewModelType = _viewModels[pageKey];
            var viewModelInstance = ServiceLocator.Current.GetInstance(viewModelType);
            return viewModelInstance as ViewModelBase;
        }

        public Type GetView(TPageKey pageKey)
        {
            if (!_viewTypes.ContainsKey(pageKey))
            {
                var ex = new KeyNotFoundException($"View not found for page {pageKey}");
                _logger.Log(ex);
                throw ex;
            }
            return _viewTypes[pageKey];
        }
    }
}