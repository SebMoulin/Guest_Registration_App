using System;
using System.Threading.Tasks;
using Windows.Devices.AllJoyn;
using Windows.UI.Notifications;
using Windows.UI.Xaml;

namespace Company.Welcome.Commons.Views.Controls
{
    public interface IAsyncValueViewModel<TResult> : IAsyncValueViewModel
    { }

    public interface IAsyncValueViewModel : IDisposable
    {
        //Func<Task> RefreshTrigger { get; set; }
        bool IsDisposed { get; set; }
        Task ViewInitialized();
        Task Refresh();

        event EventHandler<ViewModelStateChangedEventArgs> ViewModelStateChanged;
        event EventHandler<AsyncValueViewModelErrorRaisedEventArgs> ErrorRaised;
    }

    public class AsyncValueViewModel<TResult> : IAsyncValueViewModel<TResult>
    {
        private readonly Func<Task<TResult>> _sourceProvider;
        private readonly Func<TResult, bool> _isEmptyPredicate;
        private object _parent;
        private ViewModelState _viewModelState;
        private AsyncPresenterDataContext _asyncPresenterDataContext;
        private Exception _error;

        public AsyncValueViewModel(Func<Task<TResult>> sourceProvider, Func<TResult, bool> isEmptyPredicate, object parent)
        {
            if (sourceProvider == null) throw new ArgumentNullException(nameof(sourceProvider));
            if (isEmptyPredicate == null) throw new ArgumentNullException(nameof(isEmptyPredicate));
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            _sourceProvider = sourceProvider;
            _isEmptyPredicate = isEmptyPredicate;
            _parent = parent;
            _viewModelState = ViewModelState.NotInitialized;
        }

        public void Dispose()
        {
            _parent = null;
        }

       public bool IsDisposed { get; set; }


        public async Task ViewInitialized()
        {
            _viewModelState = ViewModelState.Initialized;
            RaiseViewModelStateChanged(_viewModelState, null);
            await GetDataFromSource();
        }

        private void RaiseViewModelStateChanged(ViewModelState viewModelState, AsyncPresenterDataContext asyncPresenterDataContext)
        {
            if (ViewModelStateChanged != null)
            {
                ViewModelStateChanged(this, new ViewModelStateChangedEventArgs()
                {
                    ViewModelState = viewModelState,
                    AsyncPresenterDataContext = asyncPresenterDataContext
                });
            }
        }

        private void RaiseAsyncValueViewModelErrorChanged(Exception error, AsyncPresenterDataContext asyncPresenterDataContext)
        {
            if (ErrorRaised != null)
            {
                ErrorRaised(this, new AsyncValueViewModelErrorRaisedEventArgs()
                {
                    ExceptionRaised = error,
                    AsyncPresenterDataContext = asyncPresenterDataContext
                });
            }
        }

        private async Task GetDataFromSource()
        {
            if (_viewModelState == ViewModelState.NotInitialized)
                return;
            _viewModelState = ViewModelState.Loading;
            RaiseViewModelStateChanged(_viewModelState, null);

            var sourceTask = _sourceProvider();
            if (!sourceTask.IsCompleted)
            {
                try
                {
                    var result = await sourceTask;
                    var isEmpty = _isEmptyPredicate(result);
                    if (isEmpty)
                    {
                        _viewModelState = ViewModelState.EmptyResult;
                        _asyncPresenterDataContext = new AsyncPresenterDataContext()
                        {
                            Error = null,
                            Data = null,
                            Parent = _parent
                        };
                    }
                    else
                    {
                        _viewModelState = ViewModelState.ValueRetrieved;
                        _asyncPresenterDataContext = new AsyncPresenterDataContext()
                        {
                            Error = null,
                            Data = result,
                            Parent = _parent
                        };
                    }
                    if (sourceTask.IsFaulted)
                    {
                        _asyncPresenterDataContext = new AsyncPresenterDataContext()
                        {
                            Error = sourceTask.Exception.Message,
                            Data = null,
                            Parent = _parent
                        };
                        _viewModelState = ViewModelState.Error;
                    }
                    if (sourceTask.IsCanceled)
                    {
                        _asyncPresenterDataContext = new AsyncPresenterDataContext()
                        {
                            Error = "The task to retrieve the requested data has been canceled",
                            Data = null,
                            Parent = _parent
                        };
                        _viewModelState = ViewModelState.Error;
                    }
                    RaiseViewModelStateChanged(_viewModelState, _asyncPresenterDataContext);
                }
                catch (Exception ex)
                {
                    _error = ex;
                    _asyncPresenterDataContext = new AsyncPresenterDataContext()
                    {
                        Error = _error.Message,
                        Data = null,
                        Parent = _parent
                    };
                    RaiseAsyncValueViewModelErrorChanged(ex, _asyncPresenterDataContext);
                }
            }
        }

        public Task Refresh()
        {
            return GetDataFromSource();
        }

        public event EventHandler<ViewModelStateChangedEventArgs> ViewModelStateChanged;
        public event EventHandler<AsyncValueViewModelErrorRaisedEventArgs> ErrorRaised;
    }

    public class AsyncValueViewModelErrorRaisedEventArgs : EventArgs
    {
        public Exception ExceptionRaised { get; set; }
        public AsyncPresenterDataContext AsyncPresenterDataContext { get; set; }
    }

    public class ViewModelStateChangedEventArgs : EventArgs
    {
        public ViewModelState ViewModelState { get; set; }
        public AsyncPresenterDataContext AsyncPresenterDataContext { get; set; }
    }

    public enum ViewModelState
    {
        NotInitialized = 1,
        Initialized = 2,
        Loading = 3,
        ValueRetrieved = 4,
        EmptyResult = 5,
        Error = 6,
    }
}