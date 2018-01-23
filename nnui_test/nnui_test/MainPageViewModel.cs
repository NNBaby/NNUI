using System.Collections.Generic;
using Meowtrix.ComponentModel;
using Windows.UI.Xaml;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using System;
using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
namespace nnui_test
{
    public class MainPageViewModel : NotificationObject
    {
        ContentDialogResult PromptResult = new ContentDialogResult();
        #region ViewModel definitions
        public string json;
        DispatcherTimer dispatchertimer = new DispatcherTimer();//for display info
        private ObservableCollection<OpItem> opItems = new ObservableCollection<OpItem>();
        public ObservableCollection<OpItem> OpItems
        {
            get => opItems;
            set { opItems = value; OnPropertyChanged(); }
        }
        private ObservableCollection<string> optimizerSelect = new ObservableCollection<string>();
        public ObservableCollection<string> OptimizerSelect
        {
            get => optimizerSelect;
            set { optimizerSelect = value; OnPropertyChanged(); }
        }
        private int selectedIndex = 0;
        public int SelectedIndex
        {
            get => selectedIndex;
            set { selectedIndex = value; OnPropertyChanged(); }
        }
        private int optimizerSelectedIndex = 0;
        public int OptimizerSelectedIndex
        {
            get => optimizerSelectedIndex;
            set { optimizerSelectedIndex = value; OnPropertyChanged(); }
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
        private int outDimDisplay;
        public int OutDimDisplay
        {
            get => outDimDisplay;
            set { outDimDisplay = value; OnPropertyChanged(); }
        }
        private ObservableCollection<string> activationSelect = new ObservableCollection<string>();
        public ObservableCollection<string> ActivationSelect
        {
            get => activationSelect;
            set { activationSelect = value; OnPropertyChanged(); }
        }
        private ObservableCollection<string> datasetSelect = new ObservableCollection<string>();
        public ObservableCollection<string> DatasetSelect
        {
            get => datasetSelect;
            set { datasetSelect = value; OnPropertyChanged(); }
        }
        private int activationSelectIndex;
        public int ActivationSelectIndex
        {
            get => activationSelectIndex;
            set { activationSelectIndex = value; OnPropertyChanged(); }
        }
        private int datasetSelectIndex;
        public int DatasetSelectIndex
        {
            get => datasetSelectIndex;
            set { datasetSelectIndex = value; OnPropertyChanged(); }
        }
        private string inputShapeDisplay;
        public string InputShapeDisplay
        {
            get => inputShapeDisplay;
            set { inputShapeDisplay = value; OnPropertyChanged(); }
        }
        private int batchSizeDisplay = 64;
        public int BatchSizeDisplay
        {
            get => batchSizeDisplay;
            set { batchSizeDisplay = value; OnPropertyChanged(); }
        }
        private int epochDisplay = 5;
        public int EpochDisplay
        {
            get => epochDisplay;
            set { epochDisplay = value; OnPropertyChanged(); }
        }
        private string ipDisplay;
        public string IpDisplay
        {
            get => ipDisplay;
            set { ipDisplay = value; OnPropertyChanged(); }
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
        private Visibility dimOutVisib;
        public Visibility DimOutVisib
        {
            get => dimOutVisib;
            set { dimOutVisib = value; OnPropertyChanged(); }
        }
        private Visibility padVisib;
        public Visibility PadVisib
        {
            get => padVisib;
            set { padVisib = value; OnPropertyChanged(); }
        }

        private Visibility activationVisib;
        public Visibility ActivationVisib
        {
            get => activationVisib;
            set { activationVisib = value; OnPropertyChanged(); }
        }
        private Visibility inputShapeVisib;
        public Visibility InputShapeVisib
        {
            get => inputShapeVisib;
            set { inputShapeVisib = value; OnPropertyChanged(); }
        }
        private Visibility datasetVisib;
        public Visibility DatasetVisib
        {
            get => datasetVisib;
            set { datasetVisib = value; OnPropertyChanged(); }
        }
        public string display_info = "information display";
        public string DisplayInfo
        {
            get => display_info;
            set { display_info = value; OnPropertyChanged(); }
        }

        public List<LossInfo> losslist = new List<LossInfo>();

        private Boolean enableCompile = false;
        public Boolean EnableCompile
        {
            get => enableCompile;
            set { enableCompile = value; OnPropertyChanged(); }
        }

        private string currentSavedModelName;
        public string CurrentSavedModelName
        {
            get => currentSavedModelName;
            set { currentSavedModelName = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> currentModels = new ObservableCollection<string>();
        public ObservableCollection<string> CurrentModels
        {
            get => currentModels;
            set { currentModels = value; OnPropertyChanged(); }
        }

        private int loadModelSelectIndex = -1;
        public int LoadModelSelectIndex
        {
            get => loadModelSelectIndex;
            set { loadModelSelectIndex = value; OnPropertyChanged(); }
        }

        #endregion

        #region Model SendContent definitions
        private class ModelSendContent
        {
            public string request_type = "Compile";
            public string name = "TestNet";
            public string dataset = "MNIST";
            public Train train = new Train();
            public List<Object> operators = new List<Object>();
        }

        private class Train
        {
            public int epochs = 20;
            public int batch_size = 100;
            public string optimizer = "adam";
            public List<Input> inputs = new List<Input>();
            public List<Output> outputs = new List<Output>();
        }

        private class Input
        {
            public string name;
            public List<int> shape;
            public int batch_size;
        }
        private class Data
        {
            public List<int> shape = new List<int>();
            public int batchsize = 100;
        }
        private class Output
        {
            public string name;
            public string loss;
            public double loss_weight;
            public List<string> metrics = new List<string>();
        }
        private class Pred
        {
            public string loss = "categorical_crossentropy";
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
            public int units;
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

        #region Result Request SendContent Definitions

        public class LossInfo
        {
            public int itr { get; set; }
            //int itr_end = -1;
            public float loss { get; set; }
        }
        public LossInfo curlossinfo = new LossInfo() { itr = 0, loss = -1 };
        private class ResultRequestSendContent
        {
            public string request_type = "ResultRequest";
            public LossInfo curlossinfo_send = new LossInfo() { itr = 0, loss = -1 };
        }


        #endregion

        #region TestConnection SendContent Definitions
        public  class TestConnectionSendContent
        {
            public string request_type = "Connect";
        }

        #endregion

        #region Op button logic
        public void AddConv()
        {
            if (SelectedIndex != OpItems.Count - 1)
            {
                OpItem newItem = new OpItem();
                newItem.Name = string.Format("op{0}", OpItems.Count);
                newItem.OpType = "Convolution2D";
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
                newItem.Activation = ActivationSelect[0];
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
                newItem.OpType = "MaxPooling2D";
                newItem.Kernel = 2;
                newItem.Stride = 2;
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
                newItem.OpColor = new SolidColorBrush(Windows.UI.Colors.LightSalmon);
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
                newItem.OpType = "Dense";
                newItem.DimOut = 16;
                newItem.OpColor = new SolidColorBrush(Windows.UI.Colors.LightPink);
                OpItems.Insert(SelectedIndex + 1, newItem);
                SelectedIndex++;
            }
        }
        public void Remove()
        {
            int temp = SelectedIndex;
            if (SelectedIndex > 0 && SelectedIndex < OpItems.Count - 1)
                OpItems.RemoveAt(SelectedIndex);
            SelectedIndex = temp;
        }
        #endregion

        #region Property modification
        int LastSelectedIndex = 0;
        public void SelectionChanged()
        {
            if (SelectedIndex != -1)
            {
                OpItems[LastSelectedIndex].OpType = TypeDisplay;
                OpItems[LastSelectedIndex].Kernel = ShapeDisplay;
                OpItems[LastSelectedIndex].Name = NameDisplay;
                OpItems[LastSelectedIndex].DimOut = OutDimDisplay;
                OpItems[LastSelectedIndex].Stride = StrideDisplay;

                if (ActivationSelectIndex != -1)
                    OpItems[LastSelectedIndex].Activation = ActivationSelect[ActivationSelectIndex];


                InputShapeDisplay = OpItems[SelectedIndex].InputShape;
                TypeDisplay = OpItems[SelectedIndex].OpType;
                ShapeDisplay = OpItems[SelectedIndex].Kernel;
                NameDisplay = OpItems[SelectedIndex].Name;
                OutDimDisplay = OpItems[SelectedIndex].DimOut;
                StrideDisplay = OpItems[SelectedIndex].Stride;
                OutDimDisplay = OpItems[SelectedIndex].DimOut;
                ActivationSelectIndex = GetActivationIndex(OpItems[SelectedIndex].Activation);
                DatasetSelectionChanged();
                switch (OpItems[SelectedIndex].OpType)
                {
                    case "Convolution2D":
                        KernelShapeVisib = Visibility.Visible;
                        DimOutVisib = Visibility.Visible;
                        StrideVisib = Visibility.Visible;
                        PadVisib = Visibility.Visible;
                        ActivationVisib = Visibility.Collapsed;
                        InputShapeVisib = Visibility.Collapsed;
                        DatasetVisib = Visibility.Collapsed;
                        NameVisib = Visibility.Visible;
                        break;
                    case "BatchNormalization":
                        KernelShapeVisib = Visibility.Collapsed;
                        DimOutVisib = Visibility.Collapsed;
                        StrideVisib = Visibility.Collapsed;
                        PadVisib = Visibility.Collapsed;
                        ActivationVisib = Visibility.Collapsed;
                        InputShapeVisib = Visibility.Collapsed;
                        DatasetVisib = Visibility.Collapsed;
                        NameVisib = Visibility.Visible;
                        break;
                    case "Activation":
                        KernelShapeVisib = Visibility.Collapsed;
                        DimOutVisib = Visibility.Collapsed;
                        StrideVisib = Visibility.Collapsed;
                        PadVisib = Visibility.Collapsed;
                        ActivationVisib = Visibility.Visible;
                        InputShapeVisib = Visibility.Collapsed;
                        DatasetVisib = Visibility.Collapsed;
                        NameVisib = Visibility.Visible;
                        break;
                    case "MaxPooling2D":
                        KernelShapeVisib = Visibility.Visible;
                        DimOutVisib = Visibility.Collapsed;
                        StrideVisib = Visibility.Visible;
                        PadVisib = Visibility.Collapsed;
                        ActivationVisib = Visibility.Collapsed;
                        InputShapeVisib = Visibility.Collapsed;
                        DatasetVisib = Visibility.Collapsed;
                        NameVisib = Visibility.Visible;
                        break;
                    case "Flatten":
                        KernelShapeVisib = Visibility.Collapsed;
                        DimOutVisib = Visibility.Collapsed;
                        StrideVisib = Visibility.Collapsed;
                        PadVisib = Visibility.Collapsed;
                        ActivationVisib = Visibility.Collapsed;
                        InputShapeVisib = Visibility.Collapsed;
                        DatasetVisib = Visibility.Collapsed;
                        NameVisib = Visibility.Visible;
                        break;
                    case "Dense":
                        KernelShapeVisib = Visibility.Collapsed;
                        DimOutVisib = Visibility.Visible;
                        StrideVisib = Visibility.Collapsed;
                        PadVisib = Visibility.Collapsed;
                        ActivationVisib = Visibility.Collapsed;
                        InputShapeVisib = Visibility.Collapsed;
                        DatasetVisib = Visibility.Collapsed;
                        NameVisib = Visibility.Visible;
                        break;
                    case "Input":
                        KernelShapeVisib = Visibility.Collapsed;
                        DimOutVisib = Visibility.Collapsed;
                        StrideVisib = Visibility.Collapsed;
                        PadVisib = Visibility.Collapsed;
                        ActivationVisib = Visibility.Collapsed;
                        InputShapeVisib = Visibility.Visible;
                        DatasetVisib = Visibility.Visible;
                        NameVisib = Visibility.Collapsed;
                        break;
                }
                LastSelectedIndex = SelectedIndex;
            }
        }
        public void SelectionInit()
        {
            if (SelectedIndex != -1)
            {
                InputShapeDisplay = OpItems[SelectedIndex].InputShape;
                TypeDisplay = OpItems[SelectedIndex].OpType;
                ShapeDisplay = OpItems[SelectedIndex].Kernel;
                NameDisplay = OpItems[SelectedIndex].Name;
                OutDimDisplay = OpItems[SelectedIndex].DimOut;
                StrideDisplay = OpItems[SelectedIndex].Stride;
                OutDimDisplay = OpItems[SelectedIndex].DimOut;
                ActivationSelectIndex = GetActivationIndex(OpItems[SelectedIndex].Activation);
                InputShapeDisplay = OpItems[SelectedIndex].InputShape;
                DatasetSelectionChanged();
                switch (OpItems[SelectedIndex].OpType)
                {
                    case "Convolution2D":
                        KernelShapeVisib = Visibility.Visible;
                        DimOutVisib = Visibility.Visible;
                        StrideVisib = Visibility.Visible;
                        PadVisib = Visibility.Visible;
                        ActivationVisib = Visibility.Collapsed;
                        InputShapeVisib = Visibility.Collapsed;
                        DatasetVisib = Visibility.Collapsed;
                        NameVisib = Visibility.Visible;
                        break;
                    case "BatchNormalization":
                        KernelShapeVisib = Visibility.Collapsed;
                        DimOutVisib = Visibility.Collapsed;
                        StrideVisib = Visibility.Collapsed;
                        PadVisib = Visibility.Collapsed;
                        ActivationVisib = Visibility.Collapsed;
                        InputShapeVisib = Visibility.Collapsed;
                        DatasetVisib = Visibility.Collapsed;
                        NameVisib = Visibility.Visible;
                        break;
                    case "Activation":
                        KernelShapeVisib = Visibility.Collapsed;
                        DimOutVisib = Visibility.Collapsed;
                        StrideVisib = Visibility.Collapsed;
                        PadVisib = Visibility.Collapsed;
                        ActivationVisib = Visibility.Visible;
                        InputShapeVisib = Visibility.Collapsed;
                        DatasetVisib = Visibility.Collapsed;
                        NameVisib = Visibility.Visible;
                        break;
                    case "MaxPooling2D":
                        KernelShapeVisib = Visibility.Visible;
                        DimOutVisib = Visibility.Collapsed;
                        StrideVisib = Visibility.Visible;
                        PadVisib = Visibility.Collapsed;
                        ActivationVisib = Visibility.Collapsed;
                        InputShapeVisib = Visibility.Collapsed;
                        DatasetVisib = Visibility.Collapsed;
                        NameVisib = Visibility.Visible;
                        break;
                    case "Flatten":
                        KernelShapeVisib = Visibility.Collapsed;
                        DimOutVisib = Visibility.Collapsed;
                        StrideVisib = Visibility.Collapsed;
                        PadVisib = Visibility.Collapsed;
                        ActivationVisib = Visibility.Collapsed;
                        InputShapeVisib = Visibility.Collapsed;
                        DatasetVisib = Visibility.Collapsed;
                        NameVisib = Visibility.Visible;
                        break;
                    case "Dense":
                        KernelShapeVisib = Visibility.Collapsed;
                        DimOutVisib = Visibility.Visible;
                        StrideVisib = Visibility.Collapsed;
                        PadVisib = Visibility.Collapsed;
                        ActivationVisib = Visibility.Collapsed;
                        InputShapeVisib = Visibility.Collapsed;
                        DatasetVisib = Visibility.Collapsed;
                        NameVisib = Visibility.Visible;
                        break;
                    case "Input":
                        KernelShapeVisib = Visibility.Collapsed;
                        DimOutVisib = Visibility.Collapsed;
                        StrideVisib = Visibility.Collapsed;
                        PadVisib = Visibility.Collapsed;
                        ActivationVisib = Visibility.Collapsed;
                        InputShapeVisib = Visibility.Visible;
                        DatasetVisib = Visibility.Visible;
                        NameVisib = Visibility.Collapsed;
                        break;
                }
                LastSelectedIndex = SelectedIndex;
            }
        }
        public void PropertyModify()
        {
            OpItems[SelectedIndex].OpType = TypeDisplay;
            OpItems[SelectedIndex].Kernel = ShapeDisplay;
            OpItems[SelectedIndex].Name = NameDisplay;
            OpItems[SelectedIndex].DimOut = OutDimDisplay;
            OpItems[SelectedIndex].Stride = StrideDisplay;

            if (ActivationSelectIndex != -1)
                OpItems[SelectedIndex].Activation = ActivationSelect[ActivationSelectIndex];

            OpItems[SelectedIndex].InputShape = InputShapeDisplay;
        }

        #endregion

        public async void Compile()
        {
            PropertyModify();
            ModelSendContent sendcontent = new ModelSendContent();
            InputLayer tempInput = new InputLayer();
            Conv tempConv = new Conv();
            Pool tempPool = new Pool();
            Flatten tempFlatten = new Flatten();
            Dense tempDense = new Dense();
            Activation tempActivation = new Activation();
            sendcontent.dataset = DatasetSelect[DatasetSelectIndex];
            Input input = new Input();
            input.name = "data";
            //input.shape = new List<int> { 28, 28, 1 };
            input.batch_size = BatchSizeDisplay;
            sendcontent.train.inputs.Add(input);
            sendcontent.train.batch_size = BatchSizeDisplay;
            sendcontent.train.epochs = EpochDisplay;
            sendcontent.train.optimizer = OptimizerSelect[OptimizerSelectedIndex];
            Output output = new Output();
            output.name = "pred";
            output.loss = "categorical_crossentropy";
            output.loss_weight = 1.0;
            output.metrics.Add("accuracy");
            sendcontent.train.outputs.Add(output);
            foreach (OpItem item in OpItems)
            {
                switch (item.OpType)
                {
                    case "Input":
                        tempInput.name = item.Name;
                        tempInput.optype = item.OpType;
                        if (sendcontent.dataset == "MNIST")
                            tempInput.shape = new List<int> { 28, 28, 1 };
                        else if (sendcontent.dataset == "CIFAR-10" || sendcontent.dataset == "CIFAR-100")
                            tempInput.shape = new List<int> { 32, 32, 3 };
                        sendcontent.operators.Add(tempInput.Copy());
                        break;
                    case "Convolution2D":
                        tempConv.name = item.Name;
                        tempConv.optype = item.OpType;
                        tempConv.filters = item.DimOut;
                        tempConv.kernel_size = item.Kernel;
                        sendcontent.operators.Add(tempConv.Copy());
                        break;
                    case "MaxPooling2D":
                        tempPool.name = item.Name;
                        tempPool.optype = item.OpType;
                        tempPool.pool_size = FormatConvert(string.Format("[{0}, {0}]", item.Kernel));
                        tempPool.strides = FormatConvert(string.Format("[{0}, {0}]", item.Stride));
                        sendcontent.operators.Add(tempPool.Copy());
                        break;
                    case "Flatten":
                        tempFlatten.name = item.Name;
                        tempFlatten.optype = item.OpType;
                        sendcontent.operators.Add(tempFlatten.Copy());
                        break;
                    case "Dense":
                        tempDense.name = item.Name;
                        tempDense.optype = item.OpType;
                        tempDense.units = item.DimOut;
                        sendcontent.operators.Add(tempDense.Copy());
                        break;
                    case "Activation":
                        tempActivation.name = item.Name;
                        tempActivation.optype = item.OpType;
                        tempActivation.activation = item.Activation;
                        sendcontent.operators.Add(tempActivation.Copy());
                        break;
                }
            }

            string send = JsonConvert.SerializeObject(sendcontent);
            try
            {
                await SendInfo(send, 0);
            }
            catch
            {

            }
        }



        public async void TestConnection()
        {
            TestConnectionSendContent sendcontent = new TestConnectionSendContent();
            string send = JsonConvert.SerializeObject(sendcontent);
            try
            {
                await SendInfo(send, 2);
            }catch{

            }
        }

        #region Result Update
        public void GetLossInfo()
        {
            dispatchertimer.Tick += dispatcherTimer_Tick;
            dispatchertimer.Interval = new TimeSpan(0, 0, 1);
            dispatchertimer.Start();
        }
        public async void dispatcherTimer_Tick(object sender, object e)
        {
            await displayinfo(curlossinfo.itr + 1, curlossinfo.itr + 3);
        }
        public async Task displayinfo(int startitr, int enditr)
        {

            for (int itr = startitr; itr < enditr; itr++)
            {
                curlossinfo.itr = itr;
                curlossinfo.loss = -1;
                ResultRequestSendContent sendcontent = new ResultRequestSendContent();
                sendcontent.curlossinfo_send = curlossinfo;
                string request = JsonConvert.SerializeObject(sendcontent);
                try
                {
                    await SendInfo(request, 1);
                }
                catch
                {
                    dispatchertimer.Stop();
                }
            }
        }
        #endregion
        private int GetActivationIndex(string str)
        {
            for (int i = 0; i < ActivationSelect.Count; i++)
                if (ActivationSelect[i] == str)
                    return i;
            return -1;
        }

        public async Task SendInfo(string send, int func)
        {
            string str_uri = string.Format("http://{0}/post", IpDisplay);
            HttpResponseMessage httpresponse = new HttpResponseMessage();
            string httpresponsebody;
            Uri requestUri = new Uri(str_uri);

            HttpClient httpclient = new HttpClient();
            try
            {
                httpclient.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("application/json"));
                httpresponse = await httpclient.PostAsync(requestUri, new HttpStringContent(send, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));

                httpresponsebody = await httpresponse.Content.ReadAsStringAsync();
                if (func == 1)
                {
                    curlossinfo = JsonConvert.DeserializeObject<LossInfo>(httpresponsebody);
                    if (curlossinfo.loss != -1)
                    {
                        losslist.Add(new LossInfo() { itr = curlossinfo.itr, loss = curlossinfo.loss });
                        DisplayInfo = DisplayInfo + '\n' + curlossinfo.itr.ToString() + "   " + curlossinfo.loss.ToString();
                    }
                }
                if (func == 2)
                {
                    string response = JsonConvert.DeserializeObject<string>(httpresponsebody);
                    if (response.Trim() == "success")
                        EnableCompile = true;
                }
            }
            catch (Exception ex)
            {
                EnableCompile = false;
                httpresponsebody = JsonConvert.SerializeObject("Error: " + ex.HResult.ToString("x") + "Message: " + ex.Message);
                DisplayDialog("Connection failed", "Please check server IP address and your network status and try again");
                throw (ex);
            }
            //SendContent receivecontent = JsonConvert.DeserializeObject<SendContent>(httpresponsebody);
            //DisplayInfo = "wait for response";

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

        private async void DisplayDialog(string title, string content)
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "Ok"
            };
            try
            {
                ContentDialogResult result = await noWifiDialog.ShowAsync();
            }
            catch
            {

            }
        }
        private async Task DisplayPromptDialog(string title, string content, string primaryButton, string closeButton)
        {
            ContentDialog locationPromptDialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = closeButton,
                PrimaryButtonText = primaryButton
            };

            PromptResult = await locationPromptDialog.ShowAsync();
        }

        private async Task SaveModel(string name)
        {
            StorageFolder storageFolder =
                ApplicationData.Current.LocalFolder;
            await GetModelList();
            if (CurrentModels.Contains(name))
                await DisplayPromptDialog("The model already existed", "Do you want to replace it? If continue, the previous model will be overwritten.", "Replace", "Cancel");
            if (PromptResult == ContentDialogResult.Primary)
            {
                StorageFile sampleFile =
                    await storageFolder.CreateFileAsync(String.Format("{0}.txt", name),
                        CreationCollisionOption.ReplaceExisting);
                List<OpItem4Save> OpList = new List<OpItem4Save>();
                OpItem4Save op_temp = new OpItem4Save();
                foreach (OpItem op in OpItems)
                {
                    op_temp.Name = op.Name;
                    op_temp.OpType = op.OpType;
                    op_temp.Kernel = op.Kernel;
                    op_temp.InputShape = op.InputShape;
                    op_temp.DimOut = op.DimOut;
                    op_temp.Activation = op.Activation;
                    op_temp.Padding = op.Padding;
                    op_temp.Pool = op.Pool;
                    op_temp.Stride = op.Stride;
                    OpList.Add(op_temp.Copy());
                }
                await FileIO.WriteTextAsync(sampleFile, JsonConvert.SerializeObject(OpList));
                //string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            }
        }

        public async void SaveModelButtonClicked()
        {
            PropertyModify();
            if (CurrentSavedModelName != "")
                await SaveModel(CurrentSavedModelName);
            else
                DisplayDialog("Ooops", "Please enter a valid model name");
        }

        private async Task GetModelList()
        {
            StorageFolder storageFolder =
                ApplicationData.Current.LocalFolder;
            IReadOnlyList<StorageFile> fileList = await storageFolder.GetFilesAsync();
            CurrentModels.Clear();
            foreach (StorageFile file in fileList)
            {
                CurrentModels.Add(file.DisplayName);
            }

        }

        public async void LoadModelClick()
        {
            await GetModelList();
        }

        public async void LoadModel()
        {
            try
            {
                if (LoadModelSelectIndex != -1)
                {
                    StorageFolder storageFolder =
                        ApplicationData.Current.LocalFolder;
                    StorageFile sampleFile =
                        await storageFolder.GetFileAsync(String.Format("{0}.txt", CurrentModels[LoadModelSelectIndex]));
                    string text = await FileIO.ReadTextAsync(sampleFile);
                    List<OpItem4Save> OpList = JsonConvert.DeserializeObject<List<OpItem4Save>>(text);
                    OpConvert(OpList);
                    SelectedIndex = 0;
                }
            }
            catch
            {
                DisplayDialog("Invalid Model", "The model is probably broken, please try another one");
            }

        }

        private void OpConvert(List<OpItem4Save> OpList)
        {
            OpItems.Clear();
            ObservableCollection<OpItem> OpItemReturn = new ObservableCollection<OpItem>();
            OpItem op_temp = new OpItem();
            foreach (OpItem4Save op in OpList)
            {
                op_temp.Name = op.Name;
                op_temp.OpType = op.OpType;
                op_temp.Kernel = op.Kernel;
                op_temp.InputShape = op.InputShape;
                op_temp.DimOut = op.DimOut;
                op_temp.Activation = op.Activation;
                op_temp.Padding = op.Padding;
                op_temp.Pool = op.Pool;
                op_temp.Stride = op.Stride;
                switch(op.OpType)
                {
                    case "Input":
                        op_temp.OpColor = new SolidColorBrush(Windows.UI.Colors.Azure);
                        break;
                    case "Convolution2D":
                        op_temp.OpColor = new SolidColorBrush(Windows.UI.Colors.Orange);
                        break;
                    case "BatchNormalization":
                        op_temp.OpColor = new SolidColorBrush(Windows.UI.Colors.LightBlue);
                        break;
                    case "Activation":
                        op_temp.OpColor = new SolidColorBrush(Windows.UI.Colors.LightGreen);
                        break;
                    case "MaxPooling2D":
                        op_temp.OpColor = new SolidColorBrush(Windows.UI.Colors.LightYellow);
                        break;
                    case "Flatten":
                        op_temp.OpColor = new SolidColorBrush(Windows.UI.Colors.LightSalmon);
                        break;
                    case "Dense":
                        op_temp.OpColor = new SolidColorBrush(Windows.UI.Colors.LightPink);
                        break;
                }
                OpItems.Add(CopyItem(op_temp));
            }
        }

        private OpItem CopyItem(OpItem op)
        {
            OpItem op_temp = new OpItem();
            op_temp.Name = op.Name;
            op_temp.OpType = op.OpType;
            op_temp.Kernel = op.Kernel;
            op_temp.InputShape = op.InputShape;
            op_temp.DimOut = op.DimOut;
            op_temp.Activation = op.Activation;
            op_temp.Padding = op.Padding;
            op_temp.Pool = op.Pool;
            op_temp.Stride = op.Stride;
            op_temp.OpColor = op.OpColor;
            return op_temp;
        }

        public void DatasetSelectionChanged()
        {
            if (DatasetSelectIndex == 0)
                InputShapeDisplay = "[28, 28]";
            else
                InputShapeDisplay = "[32, 32]";
        }
    }
}