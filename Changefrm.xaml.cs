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

using TextBox = System.Windows.Controls.TextBox;

namespace PersonnelRecode
    {
    /// <summary>
    /// Changefrm.xaml 的交互逻辑
    /// </summary>
    public partial class Changefrm : Window
        {
        /// <summary>
        /// 接收传过来的值
        /// </summary>
      
        string species = "";
        IList<string> typelist = new List<string>();
        string columname = "";
        object[] arguments = new object[6];
        bool close = false;
        bool alter = false;

        #region  传输值的几个属性
        private string _attstatus;
        /// <summary>
        /// 出勤状态
        /// </summary>
        public string Attstatus
            {
            get
                {
                return _attstatus;
                }
            }
        private string _workovertime;
        /// <summary>
        /// 加班时长
        /// </summary>
        public string Workovertime
            {
            get
                {
                return _workovertime;
                }
            }
        private string _helper;
        /// <summary>
        /// 帮忙对象
        /// </summary>
        public string  Helper
            {
            get
                {
                return _helper;
                }
            }
        private string _attremark;
        /// <summary>
        /// 出勤备注
        /// </summary>
        public string AttRemark
            {
            get
                {
                return _attremark;
                }
            }


        #endregion


        MessageBoxResult BoxResult = MessageBoxResult.None;
       
        
        CultureInfo cn = CultureInfo.GetCultureInfo("zh-CN");
        modificationWin modification = new modificationWin();
      


        /// <summary>
        /// 经过改变的构造函数,用来传递数据
        /// </summary>
        
        /// <param name="Species">修改类别</param>
        /// <param name="Columname">列名</param>
        ///<param name="Arguments">接收原值的数组</param>
        /// <param name="list">接收combox使用的列表</param>
        public Changefrm( String Species,string Columname,object[]Arguments ,List<string> list)
            {
            InitializeComponent();
           
            
            species = Species;
            columname = Columname;
            arguments = Arguments;
             typelist = list;

            }
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
            {
            
           

            if (string.IsNullOrEmpty(species)) return;
            
               if(species =="修改出勤")
                 {
                AttendanceInterfaceSetting();
                 }
                else if(species=="修改进账")
                 {

                 }
                else if(species=="修改出账")
                 {

                 }
            }
      
        
       
        #region  界面设置
        private void AttendanceInterfaceSetting()
            {

            //设置日期
            txkdateshow.SetValue(TextBlock.TextProperty, Convert.ToDateTime(arguments[0], cn.DateTimeFormat).ToString("yyyy年MM月dd日", cn.DateTimeFormat));

            //设置标签
            this.Title = "出勤修改-";
               txkdatetable.SetValue(TextBlock.TextProperty,"出勤日期:");
               txbstatus.SetValue(TextBlock.TextProperty,"原出勤状态");
               txkstatusortype.SetValue(TextBlock.TextProperty,"出勤状态:");
               txkworkovertimeoramount.SetValue(TextBlock.TextProperty,"加班时长:");
               txkhelperorsource.SetValue(TextBlock.TextProperty,"帮忙对象:");
               txkremark.SetValue(TextBlock.TextProperty,"出勤备注:");


            
            
              if (string.IsNullOrEmpty(columname)) return;
                  if(columname=="出勤状态")
                  {
                     this.Title += "出勤状态";
                    
                   // 设置状态
                     combstatusortype.IsEnabled = true;
                     
                     txbhelperorsource.IsEnabled =false;
                     txbremark.IsEnabled = false;

                      txbstatusshow.SetValue(TextBlock.TextProperty, arguments[1].ToString());
                      
                      combstatusortype.ItemsSource = typelist;
                      combstatusortype.SelectedIndex = 0;
                      
                      combstatusortype.Focus();

                        txbworkovertimeoramount.SetValue(TextBlock.TextProperty, arguments[2].ToString());
                        txbhelperorsource.SetValue(TextBox.TextProperty,arguments[3].ToString());
                        txbremark.SetValue(TextBox.TextProperty,arguments[4].ToString());
                }
            else if(columname=="加班时长")
                {
                this.Title += "加班时长";

                   combstatusortype.IsEnabled = false;
                   txbhelperorsource.IsEnabled = false;
                   txbremark.IsEnabled = false;

                   txbstatusshow.SetValue(TextBlock.TextProperty, arguments[1].ToString());
                   txbworkovertimeoramount.SetValue(TextBox.TextProperty, arguments[2].ToString());
                   txbhelperorsource.SetValue(TextBox.TextProperty,arguments[3].ToString());
                   txbremark.SetValue(TextBox.TextProperty,arguments[4].ToString());
                          
                      if(arguments[1].ToString() == "有加班")
                       {
                       txbworkovertimeoramount.IsEnabled = true;
                       txbworkovertimeoramount.Focus();
                         alter = true;
                       }
                      else
                       {
                    alter = false;
                        txbworkovertimeoramount.IsEnabled = false;
                       }


                }
                else if(columname=="帮忙对象")
                {
                    this.Title += "帮忙对象";

                    combstatusortype.IsEnabled = false;
                    txbworkovertimeoramount.IsEnabled= false;
                    txbhelperorsource.IsEnabled=true;
                    txbremark.IsEnabled = false;

                   

                    txbhelperorsource.Focus();

                }
                else if (columname == "出勤备注")
                {
                   this.Title += "出勤备注";
              
                   combstatusortype.IsEnabled = false;
                   txbworkovertimeoramount.IsEnabled = false;
                   txbhelperorsource.IsEnabled = false;
                   txbremark.IsEnabled = true;

                   txbstatusshow.SetValue(TextBlock.TextProperty, arguments[1].ToString());
                   txbworkovertimeoramount.SetValue(TextBox.TextProperty, arguments[2].ToString());
                   txbhelperorsource.SetValue(TextBox.TextProperty, arguments[3].ToString());
                   txbremark.SetValue(TextBox.TextProperty, arguments[4].ToString());
                   txbremark.Focus();
                
                }
            }
           private void IncomeInterfaceSetting()
            {

            }
            private void ExpendInterdfacesetting()
            {

            }
        #endregion


        /// <summary>
        /// 确认按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnconfirm_Click(object sender, RoutedEventArgs e)
            {

            if (species == "修改出勤")
                {
                if (columname == "出勤状态")
                    {
                         
                       if((arguments[1].ToString() !="有加班" 
                        && combstatusortype.SelectionBoxItem.ToString()=="有加班") 
                        ||  (arguments[1].ToString() == "有加班" 
                        && combstatusortype.SelectionBoxItem.ToString() != "有加班"))
                         {
                        BoxResult = MessageBox.Show("   你修改了 出勤状态 与 加班时长 " + 
                            "\r\n出勤状态原值是:  " + arguments[1].ToString() +
                            "\r\n出勤状态新值是:  " + combstatusortype.SelectionBoxItem.ToString() + 
                            "\r\n加班时长原值是:  " + arguments[2].ToString() +
                            "\r\n加班时长新值是:  " +txbworkovertimeoramount.Text+
                            "\r\n是否确认修改?"
                            , "修改提示"
                            , MessageBoxButton.YesNo
                            , MessageBoxImage.Question);

                          if (BoxResult == MessageBoxResult.Yes)
                             {
                             this._attstatus = combstatusortype.SelectionBoxItem.ToString();
                             this._workovertime = txbworkovertimeoramount.Text;
                             close = false;
                             this.DialogResult = true;
                           
                             }
                         else
                             {

                             close = false;
                             this.DialogResult = false;
                             }

                        }
                    else
                        {
                          BoxResult = MessageBox.Show("   你修改了 出勤状态 " +
                          "\r\n出勤状态原值是:  " + arguments[1].ToString() +
                          "\r\n出勤状态新值是:  " + combstatusortype.SelectionBoxItem.ToString() +
                          "\r\n是否确认修改?"
                          , "修改提示"
                          , MessageBoxButton.YesNo
                          , MessageBoxImage.Question);

                         if (BoxResult == MessageBoxResult.Yes)
                            {
                            this._attstatus = combstatusortype.SelectionBoxItem.ToString();
                            close = false;
                            this.DialogResult = true;

                             }
                         else
                            {

                            close = false;
                            this.DialogResult = false;
                            }


                        }
                    
                     
                  
                    }

                else if (columname == "加班时长")
                    {
                      if (alter==false)
                        {
                        close = false;
                        this.DialogResult = false;
                        }
                      else
                        {
                        BoxResult = MessageBox.Show("你修改了: 加班时长 " +
                            "\r\n原值是:  " + arguments[2].ToString() +
                            "\r\n新值是:  " + txbworkovertimeoramount.Text +
                            "\r\n是否确认修改?",
                            "修改提示",
                             MessageBoxButton.YesNo,
                             MessageBoxImage.Question);

                        if (BoxResult == MessageBoxResult.Yes)
                            {
                            this._workovertime = txbworkovertimeoramount.Text;
                            close = false;
                            this.DialogResult = true;

                            }
                        else
                            {
                            
                            close = false;
                            this.DialogResult = false;
                            }


                        }

                      

                    }
                else if (columname == "帮忙对象")
                    {

                    BoxResult = MessageBox.Show("你修改了 帮忙对象 " + 
                               "\r\n原值是:  " + arguments[3].ToString() + 
                               "\r\n新值是:  " + txbhelperorsource.Text + 
                               "\r\n是否确认修改?", 
                               "修改提示", 
                               MessageBoxButton.YesNo, 
                               MessageBoxImage.Question);
                    if (BoxResult == MessageBoxResult.Yes)
                        {
                         this._helper = txbhelperorsource.Text;
                        this.DialogResult = true;
                        close = false;
                        }
                    else
                        {
                        
                        close = false;
                        this.DialogResult = false;
                        }
                    }
                else if (columname == "出勤备注")
                    {

                    BoxResult = MessageBox.Show("你修改了 出勤备注 " + 
                                "\r\n原值是:  " + arguments[4].ToString() + 
                                "\r\n新值是:  " + txbremark.Text + 
                                "\r\n是否确认修改?", 
                                "修改提示", 
                                MessageBoxButton.YesNo, 
                                MessageBoxImage.Question);
                    if (BoxResult == MessageBoxResult.Yes)
                        {
                        this._attremark = txbremark.Text;
                        this.DialogResult = true;
                        close = false;
                        }
                    else
                        {
                        
                        close = false;
                        this.DialogResult = false;

                        }
                    }

                }
            else if (species == "修改进账")
                {

                }
            else if (species == "修改出账")
                {

                }







         }
        private void btncancel_Click(object sender, RoutedEventArgs e)
            {
            close = false;
            this.DialogResult = false;
            
            }


        private void combstatusortype_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                 if(txbstatusshow.Text  !="有加班" &&combstatusortype.SelectedValue.ToString()=="有加班")
                {
                 MessageBox.Show("请输入加班时长");
                 txbworkovertimeoramount.IsEnabled = true;
                 
                txbworkovertimeoramount.Focus();
                }
                 else if(txbstatusshow.Text == "有加班" && combstatusortype.SelectedValue.ToString() != "有加班")
                {
                txbworkovertimeoramount.SetValue(TextBox.TextProperty,"0");
                txbworkovertimeoramount.IsEnabled = false;
                }
           
            }

        private void txbworkovertimeoramount_LostFocus(object sender, RoutedEventArgs e)
            {
                 
            }

        private void txbhelperorsource_LostFocus(object sender, RoutedEventArgs e)
            {

            }

        private void txbremark_LostFocus(object sender, RoutedEventArgs e)
            {
                 if(string.IsNullOrWhiteSpace(txbremark.Text))
                  {
                  MessageBox.Show("值不能为空！！！");
                  }
            }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            {
                 e.Cancel = close;
            }
        }
    }
