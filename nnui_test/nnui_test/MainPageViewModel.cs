using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meowtrix.ComponentModel;
using Windows.UI.Xaml;
using Newtonsoft.Json;

namespace nnui_test
{
    public class MainPageViewModel : NotificationObject
    {
        #region ViewModel definitions
        public string json;

        private List<OpItem> opItems = new List<OpItem>();
        public List<OpItem> OpItems
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
            OpItems.Add(newItem);
            Visib1 = Visibility.Collapsed;
        }
        public void AddBN()
        {
            OpItem newItem = new OpItem();
            newItem.Name = "bn";
            newItem.Type = "BN";
            newItem.Kernel = -1;
            newItem.DimOut = -1;
            newItem.Stride = -1;
            OpItems.Add(newItem);
        }
        public void AddReLU()
        {
            OpItem newItem = new OpItem();
            newItem.Name = "relu";
            newItem.Type = "ReLU";
            newItem.Kernel = -1;
            newItem.DimOut = -1;
            newItem.Stride = -1;
            OpItems.Add(newItem);
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
            OpItems.Add(newItem);
        }
        public void AddFC()
        {
            OpItem newItem = new OpItem();
            newItem.Name = "fc";
            newItem.Type = "FC";
            newItem.Kernel = -1;
            newItem.DimOut = 16;
            newItem.Stride = -1;
            OpItems.Add(newItem);
        }
        public void Compile()
        {
            json = JsonConvert.SerializeObject(OpItems);
        }
    }
}
