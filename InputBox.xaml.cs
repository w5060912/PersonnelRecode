using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// InputBox.xaml 的交互逻辑
    /// </summary>
    public partial class InputBox : Window
        {
        bool bl = false;
        private double _workovertime;
        public double Workovertime
            {
            get
                {
                return _workovertime;
                }
            }
        CultureInfo cn = CultureInfo.GetCultureInfo("zh-CN");
        public InputBox()
            {
            InitializeComponent();

            }
        private void Window_Loaded(object sender, RoutedEventArgs e)
            {

            }
        private void AttUIset()
            {
               
            }
        private void Button_Click(object sender, RoutedEventArgs e)
            {
              if(string.IsNullOrEmpty(txbinput.Text) ||(txbinput.Text=="0"))
                {
                MessageBox.Show("输入错误");
                bl = true;
                }
              else
                {
                MessageBoxResult boxResult = MessageBox.Show("你已经修改了加班时长，是否确认修改", "修改提示",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                 if(boxResult==MessageBoxResult.Yes)
                    {
                    this._workovertime = double.Parse(txbinput.Text, cn.NumberFormat);
                    this.DialogResult = true;
                    bl = false;

                    }
                    else
                    {
                    bl = true;
                    MessageBox.Show("请重新输入加班时长");
                    txbinput.Clear();

                    }
               
                }

              
            }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            {
            e.Cancel = bl;
            }

        private void txbinput_PreviewTextInput(object sender, TextCompositionEventArgs e)
            {
               Regex numbeRegex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
              if(!numbeRegex.IsMatch(txbinput.Text))
                {
                MessageBox.Show("请输入正确的加班时长");
                txbinput.Clear();
                }
            }
        }
    }
