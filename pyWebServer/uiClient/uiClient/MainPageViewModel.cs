using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meowtrix.ComponentModel;

namespace uiClient
{
    public class MainPageViewModel : NotificationObject
    {
        private string recieve_info = "";
        public string RecieveInfo
        {
            get { return recieve_info; }
            set { recieve_info = value; OnPropertyChanged(); }
        }
        public void AddMessage(string newmsg)
        {
            RecieveInfo = recieve_info + newmsg;
            //OnPropertyChanged();
        }
    }
}
