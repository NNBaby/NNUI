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
        private Visibility typeVisib;
        public Visibility TypeVisib
        {
            get => typeVisib;
            set { typeVisib = value; OnPropertyChanged(); }
        }
        private Visibility nameVisib;
        public Visibility NameVisib
        {
            get => nameVisib;
            set { nameVisib = value; OnPropertyChanged(); }
        }
        private Visibility kernelShapeVisib;
        public Visibility KernelShapeVisib
        {
            get => kernelShapeVisib;
            set { kernelShapeVisib = value; OnPropertyChanged(); }
        }
        private Visibility strideVisib;
        public Visibility StrideVisib
        {
            get => strideVisib;
            set { strideVisib = value; OnPropertyChanged(); }
        }

        #endregion

        #region SendContent difinitions
        private class SendContent
        {
            public string name = "TestNet";
            public Train train = new Train();
            public List<Object> operators = new List<Object>();
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
            public List<int> shape = new List<int>();
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

        }
        private class InputLayer : Operator
        {
            public List<int> shape = new List<int>();
            public Operator Copy()
            {
                string data = JsonConvert.SerializeObject(this);
                InputLayer copy = JsonConvert.DeserializeObject<InputLayer>(data);
                return copy;
            }
        }
        private class Conv : Operator
        {
            public int filters;
            public int kernel_size;
            public Operator Copy()
            {
                string data = JsonConvert.SerializeObject(this);
                Conv copy = JsonConvert.DeserializeObject<Conv>(data);
                return copy;
            }
        }
        private class Pool : Operator
        {
            public List<int> pool_size;
            public List<int> strides;
            public Operator Copy()
            {
                string data = JsonConvert.SerializeObject(this);
                Pool copy = JsonConvert.DeserializeObject<Pool>(data);
                return copy;
            }
        }
        private class Flatten : Operator
        {
            public Operator Copy()
            {
                string data = JsonConvert.SerializeObject(this);
                Flatten copy = JsonConvert.DeserializeObject<Flatten>(data);
                return copy;
            }

        }
        private class Dense : Operator
        {
            public Operator Copy()
            {
                string data = JsonConvert.SerializeObject(this);
                Dense copy = JsonConvert.DeserializeObject<Dense>(data);
                return copy;
            }

        }
        private class Activation : Operator
        {
            public string activation;
            public Operator Copy()
            {
                string data = JsonConvert.SerializeObject(this);
                Activation copy = JsonConvert.DeserializeObject<Activation>(data);
                return copy;
            }
        }
        #endregion

        public void AddConv()
        {
            if (SelectedIndex != OpItems.Count - 1)
            {
                OpItem newItem = new OpItem();
                newItem.Name = string.Format("op{0}", OpItems.Count);
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
                newItem.Name = string.Format("op{0}", OpItems.Count);
                newItem.OpType = "BatchNormalization";
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
                newItem.Name = string.Format("op{0}", OpItems.Count);
                newItem.OpType = "Activation";
                newItem.Activatiion = "relu";
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
                newItem.Name = string.Format("op{0}", OpItems.Count);
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
        public void AddFlatten()
        {
            if (SelectedIndex != OpItems.Count - 1)
            {
                OpItem newItem = new OpItem();
                newItem.Name = string.Format("op{0}", OpItems.Count);
                newItem.OpType = "Flatten";
                newItem.OpColor = new SolidColorBrush(Windows.UI.Colors.LightGray);
                OpItems.Insert(SelectedIndex + 1, newItem);
                SelectedIndex++;
            }
        }
        public void AddFC()
        {
            if (SelectedIndex != OpItems.Count - 1)
            {
                OpItem newItem = new OpItem();
                newItem.Name = string.Format("op{0}", OpItems.Count);
                newItem.OpType = "FC";
                newItem.DimOut = 16;
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
            InputLayer tempInput = new InputLayer();
            Conv tempConv = new Conv();
            Pool tempPool = new Pool();
            Flatten tempFlatten = new Flatten();
            Dense tempDense = new Dense();
            Activation tempActivation = new Activation();
            sendcontent.train.input.data.shape.Add(28);
            sendcontent.train.input.data.shape.Add(28);
            sendcontent.train.input.data.shape.Add(1);
            foreach (OpItem item in OpItems)
            {
                switch(item.OpType)
                {
                    case "Input":
                        tempInput.name = item.Name;
                        tempInput.optype = item.OpType;
                        tempInput.shape.Add(28);
                        tempInput.shape.Add(28);
                        tempInput.shape.Add(1);
                        sendcontent.operators.Add(tempInput.Copy());
                        break;
                    case "Convolution":
                        tempConv.name = item.Name;
                        tempConv.optype = item.OpType;
                        tempConv.filters = item.DimOut;
                        tempConv.kernel_size = item.Kernel;
                        sendcontent.operators.Add(tempConv.Copy());
                        break;
                    case "MaxPooling":
                        tempPool.name = item.Name;
                        tempPool.optype = item.OpType;
                        tempPool.pool_size = FormatConvert(item.Pool);
                        tempPool.strides = FormatConvert(string.Format("[{0}, {0}]", item.Stride));
                        sendcontent.operators.Add(tempPool.Copy());
                        break;
                    case "Flatten":
                        tempFlatten.name = item.Name;
                        tempFlatten.optype = item.OpType;
                        sendcontent.operators.Add(tempFlatten.Copy());
                        break;
                    case "FC":
                        tempDense.name = item.Name;
                        tempDense.optype = item.OpType;
                        sendcontent.operators.Add(tempDense.Copy());
                        break;
                    case "Activation":
                        tempActivation.name = item.Name;
                        tempActivation.optype = item.OpType;
                        tempActivation.activation = item.Activatiion;
                        sendcontent.operators.Add(tempActivation.Copy());
                        break;
                }
            }

            string send = JsonConvert.SerializeObject(sendcontent);
            SendInfo(send);

        }

        private List<int> FormatConvert(string s)
        {
            List<int> list = new List<int>();
            try
            {
                list.Add(s[1] - 48);
                list.Add(s[4] - 48);
                return list;
            }
            catch
            {
                return list;
            }
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
