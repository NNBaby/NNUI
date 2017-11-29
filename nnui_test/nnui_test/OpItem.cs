using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
//using Meowtrix.ComponentModel;

namespace nnui_test
{
    public class OpItem
    {
        private string name;
        private string optype;
        private int kernel;
        private int dim_out;
        private string pool;
        private int stride;
        private int padding;
        private SolidColorBrush opColor;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string OpType
        {
            get { return optype; }
            set { optype = value; }
        }
        public int Kernel
        {
            get { return kernel; }
            set { kernel = value; }
        }
        public int DimOut
        {
            get { return dim_out; }
            set { dim_out = value; }
        }
        public string Pool
        {
            get { return pool; }
            set { pool = value; }
        }
        public int Stride
        {
            get { return stride; }
            set { stride = value; }
        }
        public int Padding
        {
            get { return padding; }
            set { padding = value; }
        }
        public SolidColorBrush OpColor
        {
            get { return opColor; }
            set { opColor = value; }
        }

    }
}
