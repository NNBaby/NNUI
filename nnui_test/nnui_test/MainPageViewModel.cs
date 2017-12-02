using System.Collections.Generic;
using Meowtrix.ComponentModel;
using Windows.UI.Xaml;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using System;

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

        private class SendContent
        {
            public string name = "TestNet";
            public Train train = new Train();
            public List<Operator> operators = new List<Operator>();
        }
        
        private class Train
        {
            public int epochs = 20;
            public int batch_size = 100;
            public string optimizer = "adam";
            public Input input = new Input();
        }

        private class Input
        {
            public Data data = new Data();
        }
        private class Data
        {
            public string shape = "[28, 28, 1]";
            public int batchsize = 100;
        }
        private class Output
        {
            public Pred pred = new Pred();
        }
        private class Pred
        {
            public string loss = "categorical_corssentropy";
            public double loss_weight = 1.0;
            public string metrics = "accuracy";
        }
        private class Operator
        {
            public string name;
            public string optype;
            public int filters;
            public int kernel_size;
            public string pool_size;
            public string strides;

            public Operator Copy()
            {
                string data = JsonConvert.SerializeObject(this);
                Operator copy = JsonConvert.DeserializeObject<Operator>(data);
                return copy;
            }
        }

        public void AddConv()
        {
            if (SelectedIndex != OpItems.Count - 1)
            {
                OpItem newItem = new OpItem();
                newItem.Name = "conv";
                newItem.OpType = "Convolution";
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
                newItem.OpType = "BatchNormalization";
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
                newItem.OpType = "ReLU";
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
                newItem.OpType = "MaxPooling";
                newItem.Kernel = 2;
                newItem.DimOut = 16;
                newItem.Stride = 2;
                newItem.Pool = "[2, 2]";
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
                newItem.OpType = "FC";
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
                TypeDisplay = OpItems[SelectedIndex].OpType;
                ShapeDisplay = OpItems[SelectedIndex].Kernel;
                NameDisplay = OpItems[SelectedIndex].Name;
                PaddingDisplay = OpItems[SelectedIndex].Padding;
                StrideDisplay = OpItems[SelectedIndex].Stride;
            }
        }
        public void PropertyModify()
        {
            OpItems[SelectedIndex].OpType = TypeDisplay;
            OpItems[SelectedIndex].Kernel = ShapeDisplay;
            OpItems[SelectedIndex].Name = NameDisplay;
            OpItems[SelectedIndex].Padding = PaddingDisplay;
            OpItems[SelectedIndex].Stride = StrideDisplay;
        }
        public void TextBoxLostFocus(object sender)
        {
            TextBox textBox = sender as TextBox;
            OpItems[SelectedIndex].OpType = TypeDisplay;
            OpItems[SelectedIndex].Kernel = ShapeDisplay;
            OpItems[SelectedIndex].Name = textBox.Text;
            OpItems[SelectedIndex].Padding = PaddingDisplay;
            OpItems[SelectedIndex].Stride = StrideDisplay;
        }
        public void Compile()
        {
            SendContent sendcontent = new SendContent();
            Operator tempItem = new Operator();
            foreach (OpItem item in OpItems)
            {
                tempItem.name = item.Name;
                tempItem.optype = item.OpType;
                tempItem.filters = item.DimOut;
                tempItem.kernel_size = item.Kernel;
                tempItem.pool_size = item.Pool;
                tempItem.strides = string.Format("[{0}, {0}]", item.Stride);
                sendcontent.operators.Add(tempItem.Copy());
            }
            string send = JsonConvert.SerializeObject(sendcontent);
            SendInfo(send);

        }

        public async void SendInfo(string send)
        {
            Uri requestUri = new Uri("http://127.0.0.1:5000/post");
            HttpResponseMessage httpresponse = new HttpResponseMessage();
            string httpresponsebody;
            
            HttpClient httpclient = new HttpClient();
            try
            {
                httpclient.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("application/json"));
                httpresponse = await httpclient.PostAsync(requestUri, new HttpStringContent(send, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));

                httpresponsebody = await httpresponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpresponsebody = JsonConvert.SerializeObject("Error: " + ex.HResult.ToString("x") + "Message: " + ex.Message);
            }
            SendContent receivecontent = JsonConvert.DeserializeObject<SendContent>(httpresponsebody);
            

        }
    }
}
