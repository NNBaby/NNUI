using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
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
        public MainPage()
        {
            this.InitializeComponent();
            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            ViewModel.TypeVisib = Windows.UI.Xaml.Visibility.Visible;
            ViewModel.NameVisib = Windows.UI.Xaml.Visibility.Visible;

            OpItem newItem = new OpItem();
            newItem.Name = "data";
            newItem.OpType = "Input";
            newItem.OpColor = new SolidColorBrush(Colors.Azure);
            ViewModel.OpItems.Add(newItem);

            OpItem newItem2 = new OpItem();
            newItem2.Name = "pred";
            newItem2.OpType = "Activation";
            newItem.Activatiion = "softmax";
            newItem2.OpColor = new SolidColorBrush(Colors.Azure);
            ViewModel.OpItems.Add(newItem2);
            ViewModel.SelectionChanged();
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            (LineChart.Series[0] as LineSeries).ItemsSource = null;
            (LineChart.Series[0] as LineSeries).ItemsSource = ViewModel.losslist;
        }
    }
}
