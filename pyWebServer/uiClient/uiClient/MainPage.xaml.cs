using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


//for udp
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Web.Http;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
//using CsuiServer;
//MainPageViewModel ViewModel = new MainPageViewModel();
namespace uiClient
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public class SendContent
    {
        public SendContent() { }
        private string usr;
        private string pwd;

        public string user
        {
            get { return usr; }
            set { usr = value; }
        }
        public string password
        {
            get { return pwd; }
            set { pwd = value; }
        }
    }

    public sealed partial class MainPage : Page
    {
        public MainPageViewModel ViewModel = new MainPageViewModel();
        //Thread th = Thread.CurrentThread;
        //Task t1;

        public MainPage()
        {
            this.InitializeComponent();
            //string[] lines = { "First line", "Second line", "Third line" };
            //System.IO.File.WriteAllLines(@"C:\Users\v-yurui\AppData\Local\Packages\767541c7-58e8-489e-9173-297c2e71340f_65tfhj3a2web8\LocalState\nothing.txt", lines);
        }

        public async void SendInfo()
        {
            Uri requestUri = new Uri("http://127.0.0.1:5000/post");
            HttpResponseMessage httpresponse = new HttpResponseMessage();
            string httpresponsebody;

            SendContent sendcontent = new SendContent();
            sendcontent.user = "yr";
            sendcontent.password = "abcd";

            string send = JsonConvert.SerializeObject(sendcontent);
            HttpClient httpclient = new HttpClient();
            try
            {
                httpclient.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("application/json"));
                httpresponse = await httpclient.PostAsync(requestUri, new HttpStringContent(send, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));

                httpresponsebody = await httpresponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpresponsebody = "Error: " + ex.HResult.ToString("x") + "Message: " + ex.Message;
            }
            string receivecontent = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(httpresponsebody));

            ViewModel.AddMessage("response from pyWebServre: " + receivecontent + "\n");


        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //http
            SendInfo();
        }
    }
}
