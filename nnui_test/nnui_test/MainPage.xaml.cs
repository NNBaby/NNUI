using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

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

            ViewModel.OptimizerSelect.Add("SGD");
            ViewModel.OptimizerSelect.Add("SGD with momentum");
            ViewModel.OptimizerSelect.Add("Adam");

            ViewModel.ActivationSelect.Add("relu");
            ViewModel.ActivationSelect.Add("softmax");
            ViewModel.ActivationSelect.Add("sigmoid");

            OpItem newItem = new OpItem();
            newItem.Name = "data";
            newItem.OpType = "Input";
            newItem.InputShape = "[28, 28, 1]";
            newItem.OpColor = new SolidColorBrush(Colors.Azure);
            ViewModel.OpItems.Add(newItem);

            OpItem newItem2 = new OpItem();
            newItem2.Name = "pred";
            newItem2.OpType = "Activation";
            newItem2.Activation = ViewModel.ActivationSelect[1];
            newItem2.OpColor = new SolidColorBrush(Colors.Azure);
            ViewModel.OpItems.Add(newItem2);
            ViewModel.SelectionChanged();
        }
    }
}
