using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meowtrix.ComponentModel;
using Windows.UI.Xaml;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media;

namespace nnui_test
{
    public class MainPageViewModel : NotificationObject
    {
        #region ViewModel definitions
        public string json;

        private ObservableCollection<OpItem> opItems = new ObservableCollection<OpItem>();
        public ObservableCollection<OpItem> OpItems
        {
            get => opItems;
            set { opItems = value; OnPropertyChanged(); }
        }
        private Visibility visib1;
        public Visibility Visib1
        {
            get => visib1;
            set { visib1 = value; OnPropertyChanged(); }
        }

        #endregion

        public void AddConv()
        {
            OpItem newItem = new OpItem();
            newItem.Name = "conv";
            newItem.Type = "Conv";
            newItem.Kernel = 3;
            newItem.DimOut = 16;
            newItem.Stride = 1;
            newItem.OpColor = new SolidColorBrush(Windows.UI.Colors.Orange);
            OpItems.Insert(OpItems.Count - 1, newItem);
        }
        public void AddBN()
        {
            OpItem newItem = new OpItem();
            newItem.Name = "bn";
            newItem.Type = "BN";
            newItem.Kernel = -1;
            newItem.DimOut = -1;
            newItem.Stride = -1;
            newItem.OpColor = new SolidColorBrush(Windows.UI.Colors.LightBlue);
            OpItems.Insert(OpItems.Count - 1, newItem);
        }
        public void AddReLU()
        {
            OpItem newItem = new OpItem();
            newItem.Name = "relu";
            newItem.Type = "ReLU";
            newItem.Kernel = -1;
            newItem.DimOut = -1;
            newItem.Stride = -1;
            newItem.OpColor = new SolidColorBrush(Windows.UI.Colors.LightGreen);
            OpItems.Insert(OpItems.Count - 1, newItem);
        }
        public void AddPooling()
        {
            OpItem newItem = new OpItem();
            newItem.Name = "pooling";
            newItem.Type = "MaxPool";
            newItem.Kernel = 2;
            newItem.DimOut = 16;
            newItem.Stride = 2;
            newItem.Pool = "Max";
            newItem.OpColor = new SolidColorBrush(Windows.UI.Colors.LightYellow);
            OpItems.Insert(OpItems.Count - 1, newItem);
        }
        public void AddFC()
        {
            OpItem newItem = new OpItem();
            newItem.Name = "fc";
            newItem.Type = "FC";
            newItem.Kernel = -1;
            newItem.DimOut = 16;
            newItem.Stride = -1;
            newItem.OpColor = new SolidColorBrush(Windows.UI.Colors.LightPink);
            OpItems.Insert(OpItems.Count - 1, newItem);
        }
        public void Remove()
        {
            if(OpItems.Count > 2)
                OpItems.RemoveAt(OpItems.Count - 2);
        }
        public void Compile()
        {
            json = JsonConvert.SerializeObject(OpItems);
        }
    }
}
