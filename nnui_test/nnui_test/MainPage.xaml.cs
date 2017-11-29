using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace nnui_test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel ViewModel = new MainPageViewModel();
        public MainPage()
        {
            this.InitializeComponent();
            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            //TestSourceCVS.Source = TestSourse.GetContacts(20);
            //ViewModel.AddConv();
            OpItem newItem = new OpItem();
            newItem.Name = "";
            newItem.OpType = "Input";
            newItem.Kernel = -1;
            newItem.DimOut = -1;
            newItem.Stride = -1;
            newItem.OpColor = new SolidColorBrush(Colors.Azure);
            ViewModel.OpItems.Add(newItem);
            OpItem newItem2 = new OpItem();
            newItem2.Name = "";
            newItem2.OpType = "Softmax";
            newItem2.Kernel = -1;
            newItem2.DimOut = -1;
            newItem2.Stride = -1;
            newItem2.OpColor = new SolidColorBrush(Colors.Azure);
            ViewModel.OpItems.Add(newItem2);
        }
    }
}
