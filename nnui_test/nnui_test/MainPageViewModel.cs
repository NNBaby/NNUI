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
using Windows.UI.Xaml.Controls;

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
        private int selectedIndex = 0;
        public int SelectedIndex
        {
            get => selectedIndex;
            set { selectedIndex = value; OnPropertyChanged(); }
        }
        private int shapeDisplay;
        public int ShapeDisplay
        {
            get => shapeDisplay;
            set { shapeDisplay = value; OnPropertyChanged(); }
        }
        private string nameDisplay;
        public string NameDisplay
        {
            get => nameDisplay;
            set { nameDisplay = value; OnPropertyChanged(); }
        }
        private string typeDisplay;
        public string TypeDisplay
        {
            get => typeDisplay;
            set { typeDisplay = value; OnPropertyChanged(); }
        }
        private string poolingDisplay;
        public string PoolingDisplay
        {
            get => poolingDisplay;
            set { poolingDisplay = value; OnPropertyChanged(); }
        }
        private int strideDisplay;
        public int StrideDisplay
        {
            get => strideDisplay;
            set { strideDisplay = value; OnPropertyChanged(); }
        }
        private int paddingDisplay;
        public int PaddingDisplay
        {
            get => paddingDisplay;
            set { paddingDisplay = value; OnPropertyChanged(); }
        }
        private Visibility visib1;
        public Visibility Visib1
        {
            get => visib1;
            set { visib1 = value; OnPropertyChanged(); }
        }

        #endregion

        private class SendItem
        {
            public string name;
            public string type;
            public int kernel;
            public int dim_out;
            public string pool;
            public int stride;
            public int padding;
        }

        public void AddConv()
        {
            if (SelectedIndex != OpItems.Count - 1)
            {
                OpItem newItem = new OpItem();
                newItem.Name = "conv";
                newItem.Type = "Convolution";
                newItem.Kernel = 3;
                newItem.DimOut = 16;
                newItem.Stride = 1;
                newItem.Padding = 1;
                newItem.OpColor = new SolidColorBrush(Windows.UI.Colors.Orange);
                OpItems.Insert(SelectedIndex + 1, newItem);
                SelectedIndex++;
            }
        }
        public void AddBN()
        {
            if (SelectedIndex != OpItems.Count - 1)
            {
                OpItem newItem = new OpItem();
                newItem.Name = "bn";
                newItem.Type = "Batch Normalization";
                newItem.Kernel = -1;
                newItem.DimOut = -1;
                newItem.Stride = -1;
                newItem.OpColor = new SolidColorBrush(Windows.UI.Colors.LightBlue);
                OpItems.Insert(SelectedIndex + 1, newItem);
                SelectedIndex++;
            }
        }
        public void AddReLU()
        {
            if (SelectedIndex != OpItems.Count - 1)
            {
                OpItem newItem = new OpItem();
                newItem.Name = "relu";
                newItem.Type = "ReLU";
                newItem.Kernel = -1;
                newItem.DimOut = -1;
                newItem.Stride = -1;
                newItem.OpColor = new SolidColorBrush(Windows.UI.Colors.LightGreen);
                OpItems.Insert(SelectedIndex + 1, newItem);
                SelectedIndex++;
            }
        }
        public void AddPooling()
        {
            if (SelectedIndex != OpItems.Count - 1)
            {
                OpItem newItem = new OpItem();
                newItem.Name = "pooling";
                newItem.Type = "MaxPooling";
                newItem.Kernel = 2;
                newItem.DimOut = 16;
                newItem.Stride = 2;
                newItem.Pool = "Max";
                newItem.OpColor = new SolidColorBrush(Windows.UI.Colors.LightYellow);
                OpItems.Insert(SelectedIndex + 1, newItem);
                SelectedIndex++;
            }
        }
        public void AddFC()
        {
            if (SelectedIndex != OpItems.Count - 1)
            {
                OpItem newItem = new OpItem();
                newItem.Name = "fc";
                newItem.Type = "FC";
                newItem.Kernel = -1;
                newItem.DimOut = 16;
                newItem.Stride = -1;
                newItem.OpColor = new SolidColorBrush(Windows.UI.Colors.LightPink);
                OpItems.Insert(SelectedIndex + 1, newItem);
                SelectedIndex++;
            }
        }
        public void Remove()
        {
            int temp = SelectedIndex;
            if(SelectedIndex > 0 && SelectedIndex < OpItems.Count - 1)
                OpItems.RemoveAt(SelectedIndex);
            SelectedIndex = temp;
        }
        public void SelectionChanged()
        {
            if (SelectedIndex != -1)
            {
                TypeDisplay = OpItems[SelectedIndex].Type;
                ShapeDisplay = OpItems[SelectedIndex].Kernel;
                NameDisplay = OpItems[SelectedIndex].Name;
                PaddingDisplay = OpItems[SelectedIndex].Padding;
                StrideDisplay = OpItems[SelectedIndex].Stride;
            }
        }
        public void PropertyModify()
        {
            OpItems[SelectedIndex].Type = TypeDisplay;
            OpItems[SelectedIndex].Kernel = ShapeDisplay;
            OpItems[SelectedIndex].Name = NameDisplay;
            OpItems[SelectedIndex].Padding = PaddingDisplay;
            OpItems[SelectedIndex].Stride = StrideDisplay;
        }
        public void TextBoxLostFocus(object sender)
        {
            TextBox textBox = sender as TextBox;
            OpItems[SelectedIndex].Type = TypeDisplay;
            OpItems[SelectedIndex].Kernel = ShapeDisplay;
            OpItems[SelectedIndex].Name = textBox.Text;
            OpItems[SelectedIndex].Padding = PaddingDisplay;
            OpItems[SelectedIndex].Stride = StrideDisplay;
        }
        public void Compile()
        {
            SendItem tempItem = new SendItem();
            List<SendItem> sendcontent = new List<SendItem>();
            foreach (OpItem item in OpItems)
            {
                tempItem.name = item.Name;
                tempItem.type = item.Type;
                tempItem.kernel = item.Kernel;
                tempItem.dim_out = item.DimOut;
                tempItem.padding = item.Padding;
                tempItem.pool = item.Pool;
                tempItem.stride = item.Stride;
                sendcontent.Add(tempItem);
            }
            json = JsonConvert.SerializeObject(sendcontent);
        }
    }
}
