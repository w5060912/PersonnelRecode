using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;

namespace PersonnelRecode
    {
    /// <summary>
    /// Serchfrm.xaml 的交互逻辑
    /// </summary>
    public partial class Serchfrm : Window
        {
        public Serchfrm()
            {
           
            InitializeComponent();
            
            }

        private void Window_Loaded(object sender, RoutedEventArgs e)
            {
            StartDTP.DisplayDate = DateTime.Parse("2019");
            }

        private void Window_Closed(object sender, EventArgs e)
            {

            }
        }
    }
