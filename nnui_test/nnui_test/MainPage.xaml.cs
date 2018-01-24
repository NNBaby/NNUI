using System;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace nnui_test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel ViewModel = new MainPageViewModel();
        DispatcherTimer dispatchertimer = new DispatcherTimer();
        public MainPage()
        {
            this.InitializeComponent();
            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            ViewModel.TypeVisib = Visibility.Visible;
            ViewModel.NameVisib = Visibility.Visible;

            ViewModel.OptimizerSelect.Add("SGD");
            ViewModel.OptimizerSelect.Add("Adam");

            ViewModel.ActivationSelect.Add("relu");
            ViewModel.ActivationSelect.Add("softmax");
            ViewModel.ActivationSelect.Add("sigmoid");

            ViewModel.DatasetSelect.Add("MNIST");
            ViewModel.DatasetSelect.Add("CIFAR-10");
            ViewModel.DatasetSelect.Add("CIFAR-100");
            ViewModel.DatasetSelect.Add("little_dog_cat");

            OpItem newItem = new OpItem();
            newItem.Name = "data";
            newItem.OpType = "Input";
            newItem.InputShape = "[28, 28]";
            newItem.OpColor = new SolidColorBrush(Colors.Azure);
            ViewModel.OpItems.Add(newItem);

            OpItem newItem2 = new OpItem();
            newItem2.Name = "pred";
            newItem2.OpType = "Activation";
            newItem2.Activation = ViewModel.ActivationSelect[1];
            newItem2.OpColor = new SolidColorBrush(Colors.LightGreen);
            ViewModel.OpItems.Add(newItem2);
            ViewModel.SelectionInit();

            ViewModel.IpDisplay = "127.0.0.1:5000";

            dispatchertimer.Tick += dispatcherTimer_Tick;
            dispatchertimer.Interval = new TimeSpan(0, 0, 1);
            dispatchertimer.Start();

            //ViewModel.SaveModel();
            ViewModel.LoadModelClick();
        }

        public void dispatcherTimer_Tick(object sender, object e)
        {
            (LineChart.Series[0] as LineSeries).ItemsSource = null;
            (LineChart.Series[0] as LineSeries).ItemsSource = ViewModel.losslist;
        }
        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            (LineChart.Series[0] as LineSeries).ItemsSource = null;
            (LineChart.Series[0] as LineSeries).ItemsSource = ViewModel.losslist;
        }
    }
}
