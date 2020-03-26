using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace PersonnelRecode
    {
    /// <summary>
    /// Managefrm.xaml 的交互逻辑
    /// </summary>
    public partial class Managefrm : Window
        {
        CultureInfo cn = CultureInfo.GetCultureInfo("zh-chs");

        public Managefrm()
            {
            InitializeComponent();
            }

        private void Button_Click(object sender, RoutedEventArgs e)
            {
                  Button btn =e.Source as Button;
                  
            switch(btn.Name)
                {
                   case "btnAlter":
                         modificationWin modification = new modificationWin();
                          modification.Title += "今天是: " + DateTime.Now.ToString("yyyy年MM月dd日",cn.DateTimeFormat);
                          modification.Show();
                       this.Hide();
                       break;
                case "btnViewAlterHistory":
                      ViewAlterHistory viewAlter = new ViewAlterHistory();
                      viewAlter.Show();
                      this.Hide();
                       break;

                }

            }
        }
    }
