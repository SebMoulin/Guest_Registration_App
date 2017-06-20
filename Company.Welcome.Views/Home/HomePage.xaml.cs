using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Printing;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LinqToTwitter;
using Microsoft.Practices.ServiceLocation;
using Company.Welcome.Business;
using Company.Welcome.Commons;
using Company.Welcome.Views.Printing;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Company.Welcome.Views.Home
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : NavigationPage
    {
        //private PrintHelper _printHelper;
        private readonly IManagePrinting _printingManager;

        public HomePage()
        {
            this.InitializeComponent();
            this.LoadViewModel<ApplicationPages>();
            _printingManager = new PrintingManager(this);
        }

        private async void OnPrintButtonClick(object sender, RoutedEventArgs e)
        {
            var childrenToPrint = new List<UIElement>();
            var richTextBlocks = new List<RichTextBlock>();
            var richTextBlockOverflows = new List<RichTextBlockOverflow>();
            FindChildren(richTextBlocks, ContentToPrint);
            FindChildren(richTextBlockOverflows, ContentToPrint);
            childrenToPrint.AddRange(richTextBlocks);
            childrenToPrint.AddRange(richTextBlockOverflows);
            Func<List<UIElement>> buildVisual2 = () => childrenToPrint;

            _printingManager.SetContent(buildVisual2);

            await _printingManager.ShowPrintUiAsync();
            ContentToPrint.Visibility = Visibility.Collapsed;
        }

        //private void Test()
        //{
        //    // Set "paper" width
        //    page.Width = printPageDescription.PageSize.Width;
        //    page.Height = printPageDescription.PageSize.Height;

        //    Grid printableArea = (Grid)page.FindName("PrintableArea");

        //    // Get the margins size
        //    // If the ImageableRect is smaller than the app provided margins use the ImageableRect
        //    double marginWidth = Math.Max(printPageDescription.PageSize.Width - printPageDescription.ImageableRect.Width, printPageDescription.PageSize.Width * ApplicationContentMarginLeft * 2);
        //    double marginHeight = Math.Max(printPageDescription.PageSize.Height - printPageDescription.ImageableRect.Height, printPageDescription.PageSize.Height * ApplicationContentMarginTop * 2);

        //    // Set-up "printable area" on the "paper"
        //    printableArea.Width = firstPage.Width - marginWidth;
        //    printableArea.Height = firstPage.Height - marginHeight;

        //    // Add the (newley created) page to the print canvas which is part of the visual tree and force it to go
        //    // through layout so that the linked containers correctly distribute the content inside them.
        //    PrintCanvas.Children.Add(page);
        //    PrintCanvas.InvalidateMeasure();
        //    PrintCanvas.UpdateLayout();

        //    // Find the last text container and see if the content is overflowing
        //    textLink = (RichTextBlockOverflow)page.FindName("ContinuationPageLinkedContainer");

        //    // Check if this is the last page
        //    if (!textLink.HasOverflowContent && textLink.Visibility == Windows.UI.Xaml.Visibility.Visible)
        //    {
        //        StackPanel footer = (StackPanel)page.FindName("Footer");
        //        footer.Visibility = Windows.UI.Xaml.Visibility.Visible;
        //    }
        //}

        //protected RichTextBlockOverflow AddOnePrintPreviewPage(RichTextBlockOverflow lastRTBOAdded, PrintPageDescription printPageDescription)
        //{
        //    // XAML element that is used to represent to "printing page"
        //    FrameworkElement page;

        //    // The link container for text overflowing in this page
        //    RichTextBlockOverflow textLink;

        //    // Check if this is the first page ( no previous RichTextBlockOverflow)
        //    if (lastRTBOAdded == null)
        //    {
        //        // If this is the first page add the specific scenario content
        //        page = firstPage;
        //        //Hide footer since we don't know yet if it will be displayed (this might not be the last page) - wait for layout
        //        StackPanel footer = (StackPanel)page.FindName("Footer");
        //        footer.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        // Flow content (text) from previous pages
        //        page = new ContinuationPage(lastRTBOAdded);
        //    }

        //    // Set "paper" width
        //    page.Width = printPageDescription.PageSize.Width;
        //    page.Height = printPageDescription.PageSize.Height;

        //    Grid printableArea = (Grid)page.FindName("PrintableArea");

        //    // Get the margins size
        //    // If the ImageableRect is smaller than the app provided margins use the ImageableRect
        //    double marginWidth = Math.Max(printPageDescription.PageSize.Width - printPageDescription.ImageableRect.Width, printPageDescription.PageSize.Width * ApplicationContentMarginLeft * 2);
        //    double marginHeight = Math.Max(printPageDescription.PageSize.Height - printPageDescription.ImageableRect.Height, printPageDescription.PageSize.Height * ApplicationContentMarginTop * 2);

        //    // Set-up "printable area" on the "paper"
        //    printableArea.Width = firstPage.Width - marginWidth;
        //    printableArea.Height = firstPage.Height - marginHeight;

        //    // Add the (newley created) page to the print canvas which is part of the visual tree and force it to go
        //    // through layout so that the linked containers correctly distribute the content inside them.
        //    PrintCanvas.Children.Add(page);
        //    PrintCanvas.InvalidateMeasure();
        //    PrintCanvas.UpdateLayout();

        //    // Find the last text container and see if the content is overflowing
        //    textLink = (RichTextBlockOverflow)page.FindName("ContinuationPageLinkedContainer");

        //    // Check if this is the last page
        //    if (!textLink.HasOverflowContent && textLink.Visibility == Windows.UI.Xaml.Visibility.Visible)
        //    {
        //        StackPanel footer = (StackPanel)page.FindName("Footer");
        //        footer.Visibility = Windows.UI.Xaml.Visibility.Visible;
        //    }

        //    // Add the page to the page preview collection
        //    printPreviewPages.Add(page);

        //    return textLink;
        //}

        internal static void FindChildren<T>(List<T> results, DependencyObject startNode)
            where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(startNode);
            for (int i = 0; i < count; i++)
            {
                DependencyObject current = VisualTreeHelper.GetChild(startNode, i);
                if ((current.GetType()).Equals(typeof(T)) || (current.GetType().GetTypeInfo().IsSubclassOf(typeof(T))))
                {
                    T asType = (T)current;
                    results.Add(asType);
                }
                FindChildren<T>(results, current);
            }
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _printingManager.RegistrationPrintManager();
            // Initalize common helper class and register for printing
            //_printHelper = new PrintHelper(this);
            //_printHelper.RegisterForPrinting();

            //// Initialize print content for this scenario
            //_printHelper.PreparePrintContent(new PageToPrint());

            //// Tell the user how to print
            //MainPage.Current.NotifyUser("Print contract registered with customization, use the Print button to print.", MainPage.NotifyType.StatusMessage);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _printingManager.UnRegistrationPrintManager();
        }
    }
}
