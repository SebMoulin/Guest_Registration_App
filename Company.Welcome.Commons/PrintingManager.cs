using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Printing;

namespace Company.Welcome.Commons
{
    public class PrintingManager : IManagePrinting
    {
        private readonly Page _page;
        private Func<List<UIElement>> _buildVisual;
        //https://msdn.microsoft.com/en-us/windows/uwp/devices-sensors/print-from-your-app
        //https://github.com/Microsoft/Windows-universal-samples/tree/master/Samples/Printing/cs
        //https://dotblogs.com.tw/evarichie/2015/11/29/134453

        private PrintDocument _printDoc;
        private PrintManager _printMgr;
        private List<UIElement> _pagesToPrint;
        //private Canvas _printCanvas;

        public PrintingManager(Page page)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            _page = page;
        }

        protected event EventHandler PreviewPagesCreated;

        public void SetContent(Func<List<UIElement>> buildVisual)
        {
            if (buildVisual == null) throw new ArgumentNullException(nameof(buildVisual));
            _buildVisual = buildVisual;
        }

        public async Task ShowPrintUiAsync()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView
                .CoreWindow
                .Dispatcher
                .RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                await PrintManager.ShowPrintUIAsync());
        }

        public void RegistrationPrintManager()
        {
            _printMgr = PrintManager.GetForCurrentView();
            _printMgr.PrintTaskRequested += MainPage_PrintTaskRequested;
        }

        private void MainPage_PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
        {
            var deferral = args.Request.GetDeferral();
            var printTask = args.Request.CreatePrintTask("SamplePrintTitle", new PrintTaskSourceRequestedHandler(PrintTaskHandler));
            deferral.Complete();
        }

        private async void PrintTaskHandler(PrintTaskSourceRequestedArgs args)
        {
            var deferral = args.GetDeferral();
            await Windows.ApplicationModel.Core.CoreApplication.MainView
                .CoreWindow
                .Dispatcher
                .RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                _printDoc = new PrintDocument();
                _printDoc.AddPages += PrintDoc_AddPages;
                _printDoc.Paginate += PrintDoc_Paginate;
                _printDoc.GetPreviewPage += PrintDoc_GetPreviewPage;
                args.SetSource(_printDoc.DocumentSource);
            });
            deferral.Complete();
        }

        private async void PrintDoc_GetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (_pagesToPrint == null 
                || _pagesToPrint.Count == 0)
                {
                    _pagesToPrint = _buildVisual();
                }
                var sentprintDoc = (PrintDocument)sender;
                sentprintDoc.SetPreviewPage(e.PageNumber, _pagesToPrint[e.PageNumber - 1]);
            });
        }

        private void PrintDoc_Paginate(object sender, PaginateEventArgs e)
        {
            //var contentToPrint = (Grid)_page.FindName("ContentToPrint");
            _pagesToPrint = _buildVisual();
            //_printCanvas = new Canvas();
            //if (_pagesToPrint != null)
            //{
            //    _pagesToPrint.Clear();
            //}
            //else
            //{
            //    _pagesToPrint = new List<UIElement>();
            //}
            //_printCanvas.Children.Clear();
            var printingOptions = ((PrintTaskOptions)e.PrintTaskOptions);
            var pageDescription = printingOptions.GetPageDescription(0);

            foreach (var pageToPrint in _pagesToPrint)
            {
                if (pageToPrint is RichTextBlock)
                {
                    ((RichTextBlock)pageToPrint).Width = pageDescription.PageSize.Width;
                    ((RichTextBlock)pageToPrint).Height = pageDescription.PageSize.Height;
                }
                if (pageToPrint is RichTextBlockOverflow)
                {
                    ((RichTextBlockOverflow)pageToPrint).Width = pageDescription.PageSize.Width;
                    ((RichTextBlockOverflow)pageToPrint).Height = pageDescription.PageSize.Height;
                }
                pageToPrint.InvalidateMeasure();
                pageToPrint.UpdateLayout();
            }

            //_pagesToPrint = _buildVisual();
            //contentToPrint.Width = pageDescription.PageSize.Width;
            //contentToPrint.Height = pageDescription.PageSize.Height;
            //contentToPrint.InvalidateMeasure();
            //contentToPrint.UpdateLayout();

            //_printCanvas.Children.Add(contentToPrint);
            //_printCanvas.InvalidateMeasure();
            //_printCanvas.UpdateLayout();

            //PreviewPagesCreated?.Invoke(_pagesToPrint, null);
            var printDoc = (PrintDocument)sender;
            // Report the number of preview pages created
            printDoc.SetPreviewPageCount(_pagesToPrint.Count, PreviewPageCountType.Intermediate);
        }

        private async void PrintDoc_AddPages(object sender, AddPagesEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView
                .CoreWindow
                .Dispatcher
                .RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                foreach (var page in _pagesToPrint)
                {
                    _printDoc.AddPage(page);
                }
                _printDoc.AddPagesComplete();
            });
        }

        public void UnRegistrationPrintManager()
        {
            if (_printMgr != null)
            {
                _printMgr.PrintTaskRequested -= MainPage_PrintTaskRequested;
            }
        }
    }
}


