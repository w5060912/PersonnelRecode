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

namespace PersonnelRecode
    {
    /// <summary>
    /// MessShowBoxfrm.xaml 的交互逻辑
    /// </summary>
    public partial class MessShowBoxfrm : Window
        {
      /// <summary>
      /// 初始化消息提示对话框
      /// </summary>
      /// <param name="Title">消息提示对话框的标题</param>
      /// <param name="message">消息提示对话框中显示的文本</param>
        public MessShowBoxfrm( string Title,string message)
            {
            InitializeComponent();
            this.Title = Title;
            this.txbMessShow.Text = message;
            string tishi = "确认存储请单击确认键" + "\r\n返回修改请单击取消键";
            tbktishi.SetValue(TextBlock.TextProperty, tishi);

            }



        private void Btton_Click(object sender, RoutedEventArgs e)
            {
            Button button = e.Source as Button;
            
            
            
            if (button.Name == "BtnOK")
                {
                this.DialogResult = true;
                }
            else
                {
                this.DialogResult = false;
                }

            }

        }
    }
