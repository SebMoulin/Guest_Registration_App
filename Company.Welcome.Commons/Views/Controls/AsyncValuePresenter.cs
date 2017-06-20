using System;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Company.Welcome.Commons.Views.Controls
{
    [TemplatePart(Name = LoadingTemplatePartName, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = ErrorTemplatePartName, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = ContentTemplatePartName, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = EmptyTemplatePartName, Type = typeof(ContentPresenter))]
    [ContentProperty(Name = "ContentDataTemplate")]
    public class AsyncValuePresenter : Control
    {
        private const string LoadingTemplatePartName = "LoadingTemplatePartName";
        private const string ErrorTemplatePartName = "ErrorTemplatePartName";
        private const string ContentTemplatePartName = "ContentTemplatePartName";
        private const string EmptyTemplatePartName = "EmptyTemplatePartName";

        public static readonly DependencyProperty ContentDataTemplateProperty =
            DependencyProperty.Register("ContentDataTemplate", typeof(DataTemplate), typeof(AsyncValuePresenter),
                new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ContentDataTemplate
        {
            get { return (DataTemplate)GetValue(ContentDataTemplateProperty); }
            set { SetValue(ContentDataTemplateProperty, value); }
        }

        public static readonly DependencyProperty ErrorDataTemplateProperty =
            DependencyProperty.Register("ErrorDataTemplate", typeof(DataTemplate), typeof(AsyncValuePresenter),
                new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ErrorDataTemplate
        {
            get { return (DataTemplate)GetValue(ErrorDataTemplateProperty); }
            set { SetValue(ErrorDataTemplateProperty, value); }
        }

        public static readonly DependencyProperty LoadingDataTemplateProperty =
            DependencyProperty.Register("LoadingDataTemplate", typeof(DataTemplate), typeof(AsyncValuePresenter),
                new PropertyMetadata(default(DataTemplate)));

        public DataTemplate LoadingDataTemplate
        {
            get { return (DataTemplate)GetValue(LoadingDataTemplateProperty); }
            set { SetValue(LoadingDataTemplateProperty, value); }
        }

        public static readonly DependencyProperty EmptyDataTemplateProperty =
            DependencyProperty.Register("EmptyDataTemplate", typeof(DataTemplate), typeof(AsyncValuePresenter),
                new PropertyMetadata(default(DataTemplate)));

        public DataTemplate EmptyDataTemplate
        {
            get { return (DataTemplate)GetValue(EmptyDataTemplateProperty); }
            set { SetValue(EmptyDataTemplateProperty, value); }
        }

        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(object), typeof(AsyncValuePresenter),
                new PropertyMetadata(null, OnDataSourceChange));

        private bool _applyTemplateDone;
        private ContentPresenter _loadingContentPresenter;
        private ContentPresenter _errorContentPresenter;
        private ContentPresenter _contentContentPresenter;
        private ContentPresenter _emptyContentPresenter;
        private object _refreshTrigger;
        private EventHandler<AsyncValueViewModelErrorRaisedEventArgs> _viewModelOnErrorRaised;
        EventHandler<ViewModelStateChangedEventArgs> _viewModelOnViewModelStateChanged;

        public AsyncValuePresenter()
        {
            this.DefaultStyleKey = typeof(AsyncValuePresenter);
            _applyTemplateDone = false;

            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _loadingContentPresenter = null;
            _errorContentPresenter = null;
            _contentContentPresenter = null;
            _emptyContentPresenter = null;
            var viewModel = DataSource as IAsyncValueViewModel;
            if (viewModel != null
                && !viewModel.IsDisposed)
            {
                viewModel.ErrorRaised -= _viewModelOnErrorRaised;
                viewModel.ViewModelStateChanged -= _viewModelOnViewModelStateChanged;
                viewModel.Dispose();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {

        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _loadingContentPresenter = this.GetTemplateChild(LoadingTemplatePartName) as ContentPresenter;
            _errorContentPresenter = this.GetTemplateChild(ErrorTemplatePartName) as ContentPresenter;
            _contentContentPresenter = this.GetTemplateChild(ContentTemplatePartName) as ContentPresenter;
            _emptyContentPresenter = this.GetTemplateChild(EmptyTemplatePartName) as ContentPresenter;

            if (_loadingContentPresenter == null) { throw new ArgumentException("LoadingContentPresenter is null"); }
            if (_errorContentPresenter == null) { throw new ArgumentException("ErrorContentPresenter is null"); }
            if (_contentContentPresenter == null) { throw new ArgumentException("ContentContentPresenter is null"); }
            if (_emptyContentPresenter == null) { throw new ArgumentException("EmptyContentPresenter is null"); }

            _loadingContentPresenter.Opacity = 0;
            _errorContentPresenter.Opacity = 0;
            _contentContentPresenter.Opacity = 0;
            _emptyContentPresenter.Opacity = 0;

            _applyTemplateDone = true;
            GoToState("Loading", null);
            InternalDataSourceChange(DataSource);
        }


        public object DataSource
        {
            get { return (object)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        private static void OnDataSourceChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AsyncValuePresenter).InternalDataSourceChange(e.NewValue);
        }

        private void InternalDataSourceChange(object dataSource)
        {
            if (_applyTemplateDone
                && dataSource != null)
            {
                var viewModel = dataSource as IAsyncValueViewModel;
                if (viewModel != null)
                {
                    viewModel.ViewInitialized();
                    GetDataFromViewModel();
                    SetRefreshTrigger();
                }
            }
        }

        private void SetRefreshTrigger()
        {
            var viewModel = DataSource as IAsyncValueViewModel;
            if (viewModel != null)
            {
                //_refreshTrigger = viewModel.RefreshTrigger;
                //if (_refreshTrigger != null)
                //{
                //    viewModel.Refresh();
                //}
            }
        }

        private void GetDataFromViewModel()
        {
            var viewModel = DataSource as IAsyncValueViewModel;

            _viewModelOnErrorRaised = delegate (object sender, AsyncValueViewModelErrorRaisedEventArgs args)
            {
                GoToState("Error", args.AsyncPresenterDataContext);
            };
            _viewModelOnViewModelStateChanged = delegate (object sender, ViewModelStateChangedEventArgs args)
            {
                if (args != null)
                {
                    switch (args.ViewModelState)
                    {
                        case ViewModelState.EmptyResult:
                            GoToState("Empty", args.AsyncPresenterDataContext);
                            break;
                        case ViewModelState.Error:
                            GoToState("Error", args.AsyncPresenterDataContext);
                            break;
                        case ViewModelState.ValueRetrieved:
                            GoToState("ValuePresent", args.AsyncPresenterDataContext);
                            break;
                    }
                }

            };

            if (viewModel != null)
            {
                viewModel.ErrorRaised += _viewModelOnErrorRaised;
                viewModel.ViewModelStateChanged += _viewModelOnViewModelStateChanged;
            }
        }

        private void GoToState(string stateName, AsyncPresenterDataContext data)
        {
            SetDataTemplateAndContent(stateName, data);
            SetVisibility(stateName);
            VisualStateManager.GoToState(this, stateName + "VisualState", true);
        }

        private void SetDataTemplateAndContent(string stateName, AsyncPresenterDataContext data)
        {
            switch (stateName)
            {
                case "Loading":
                    _loadingContentPresenter.ContentTemplate = LoadingDataTemplate;
                    break;

                case "Error":
                    _errorContentPresenter.Content = data.Error;
                    _errorContentPresenter.ContentTemplate = ErrorDataTemplate;
                    break;

                case "Empty":
                    _emptyContentPresenter.ContentTemplate = EmptyDataTemplate;
                    break;

                case "ValuePresent":
                    _contentContentPresenter.Content = data;
                    _contentContentPresenter.ContentTemplate = ContentDataTemplate;
                    _contentContentPresenter.DataContext = data;
                    break;
            }
        }

        private void SetVisibility(string stateName)
        {
            switch (stateName)
            {
                case "Loading":
                    _loadingContentPresenter.Opacity = 1;
                    _errorContentPresenter.Opacity = 0;
                    _contentContentPresenter.Opacity = 0;
                    _emptyContentPresenter.Opacity = 0;
                    break;

                case "Error":
                    _loadingContentPresenter.Opacity = 0;
                    _errorContentPresenter.Opacity = 1;
                    _contentContentPresenter.Opacity = 0;
                    _emptyContentPresenter.Opacity = 0;
                    break;

                case "ValuePresent":
                    _loadingContentPresenter.Opacity = 0;
                    _errorContentPresenter.Opacity = 0;
                    _contentContentPresenter.Opacity = 1;
                    _emptyContentPresenter.Opacity = 0;
                    break;

                case "Empty":
                    _loadingContentPresenter.Opacity = 0;
                    _errorContentPresenter.Opacity = 0;
                    _contentContentPresenter.Opacity = 0;
                    _emptyContentPresenter.Opacity = 1;
                    break;
            }
        }
    }

    public class AsyncPresenterDataContext : DependencyObject
    {
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(AsyncPresenterDataContext), new PropertyMetadata(default(object)));

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty ParentProperty =
            DependencyProperty.Register("Parent", typeof(object), typeof(AsyncPresenterDataContext), new PropertyMetadata(default(object)));

        public object Parent
        {
            get { return (object)GetValue(ParentProperty); }
            set { SetValue(ParentProperty, value); }
        }

        public static readonly DependencyProperty ErrorProperty =
            DependencyProperty.Register("Error", typeof(string), typeof(AsyncPresenterDataContext), new PropertyMetadata(default(string)));

        public string Error
        {
            get { return (string)GetValue(ErrorProperty); }
            set { SetValue(ErrorProperty, value); }
        }
    }
}
