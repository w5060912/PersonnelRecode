using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
    /// rePortbynew.xaml 的交互逻辑
    /// </summary>
    public partial class rePortbynew : Window
        {
        /// <summary>
        /// 数据操作类
        /// </summary>
        CDataoption rePortDp;

        public rePortbynew()
            {
            InitializeComponent();
            }

        /// <summary>
        /// 默认的进账来源,  当用户没有输入进账来源的时候
        /// </summary>
        /// <remarks>后续考虑从配置文件读取</remarks>
        string IcomeSource = ConfigurationManager.AppSettings["IncomeSource"];

        /// <summary>
        /// 帮忙对象默认值，当用户没有输入帮忙对象的时候
        /// </summary>
        /// /// <remarks>后续考虑从配置文件读取</remarks>
        string helpobject = "未帮忙";

        /// <summary>
        /// 开始日期
        /// </summary>
        string SDate = "";
       
        /// <summary>
        /// 结束日期
        /// </summary>
        string Edate = "";

        /// <summary>
        /// 进账来源
        /// </summary>
        string IncomeSource = "";
        
        /// <summary>
        /// 帮忙对象
        /// </summary>
        string helper = "";

        /// <summary>
        /// 出勤查询选项列表
        /// </summary>
        List<string> AttendanceQuerylList = new List<string>();
       
        /// <summary>
        /// 进账查询选项列表
        /// </summary>
        List<string> IncomeQueryList = new List<string>();
        
        /// <summary>
        /// 出账查询选项列表 
        /// </summary>
        List<string> ExpendQueryList = new List<string>();

        /// <summary>
        /// 记录用户选择的查询选项
        /// </summary>
        string SelectitemofQuery = "";


        /// <summary>
        /// 标志用户选择的查询方式的枚举
        /// </summary>
        public enum SelectionItemEnu
            {
            /// <summary>
            /// 查询全部出勤记录
            /// </summary>
            Allattendance,

            /// <summary>
            /// 查询给某人帮忙的出勤记录
            /// </summary>
            allAttendanceBYsomeone,

            /// <summary>
            /// 没有帮忙的出勤记录
            /// </summary>
            Nhelpattendance,

            /// <summary>
            /// 查询来自于某人的进账信息
            /// </summary>
            allIncomeByfromsomeone,

            /// <summary>
            /// 查询所有的出账记录
            /// </summary>
            allExpend,

            /// <summary>
            /// 查询所有可报销的出账记录
            /// </summary>
            allexpendbyKBX,
            /// <summary>
            /// 查询自己吃饭出账,详细账目
            /// </summary>
            ExpendByQMED,

            /// <summary>
            /// 查询自己吃饭的汇总账目
            /// </summary>
            ExpendByQMES
           
            
            
            
            }

        /// <summary>
        /// 记录用户选择的主选项
        /// </summary>
        string PrimaryOPtion = "";

        //结果数据种类
        string species = "";
               
        /// <summary>
        /// 存储查询结果的数据表
        /// </summary>
        DataTable Resultstb;

        //窗体载入初始化事件
        private void Window_Loaded(object sender, RoutedEventArgs e)
            {
                AttendanceQuerylList.Add("选择出勤查询方式");
                AttendanceQuerylList.Add("查询全部出勤");
                AttendanceQuerylList.Add("查询帮忙出勤");
                AttendanceQuerylList.Add("查询未帮忙的出勤");

                IncomeQueryList.Add("选择进账查询方式");
                IncomeQueryList.Add("按进账来源查询");

                ExpendQueryList.Add("选择出账查询方式");
                ExpendQueryList.Add("查询全部出账");
                ExpendQueryList.Add("查询可报销的");
                ExpendQueryList.Add("查询自己吃饭的详细账单");
                ExpendQueryList.Add("查询自己吃饭的汇总账单");


                TxbOTE.Visibility = Visibility.Hidden;
                btnSave.IsEnabled = false;

            



            }

        /// <summary>
        /// 主选项单选按钮被选取后的事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
            {
              btnSave.IsEnabled = false;
              RadioButton rab = e.Source as RadioButton;
              PrimaryOPtion = rab.Content.ToString();


             

            switch (PrimaryOPtion)
                {
                case "出勤查询":
                  
                    CombQMS.ItemsSource = AttendanceQuerylList;
                    CombQMS.SelectedIndex = 0;
                    TxbOTE.SetValue(TextBlock.TextProperty, "输入帮忙对象");
                    TxbOTE.Visibility = Visibility.Hidden;
                    break;
                case "进账查询":
                   
                    CombQMS.ItemsSource = IncomeQueryList;
                    CombQMS.SelectedIndex = 0;
                    TxbOTE.SetValue(TextBlock.TextProperty, "输入进账来源");
                    TxbOTE.Visibility = Visibility.Visible;
                    break;
                case "出账查询":
                    
                    CombQMS.ItemsSource = ExpendQueryList;
                    CombQMS.SelectedIndex = 0;
                    TxbOTE.Visibility = Visibility.Hidden;
                    break;

                }

            }

        /// <summary>
        /// 详细查询选项内容发生改变后的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CombQMS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


              


                 if (CombQMS.SelectedValue == null)
                   {
                     CombQMS.SelectedIndex = 0;
                   }
                    string selectstr = CombQMS.SelectedValue.ToString();
                    if (selectstr == "查询全部出勤")
                    {
                        species = "出勤查询";
                        SelectitemofQuery = SelectionItemEnu.Allattendance.ToString();
                        TxbOTE.Visibility = Visibility.Hidden;
                     }
                    else if (selectstr == "查询帮忙出勤")
                     {
                        species = "出勤查询";
                         SelectitemofQuery = SelectionItemEnu.allAttendanceBYsomeone.ToString();
                         TxbOTE.Visibility = Visibility.Visible;
                     }
                     else if (selectstr == "查询未帮忙的出勤")
                    {
                         species = "出勤查询";
                         SelectitemofQuery = SelectionItemEnu.Nhelpattendance.ToString();
                         TxbOTE.Visibility = Visibility.Hidden;
                     }
                     else if (selectstr == "按进账来源查询")
                     {
                         species = "进账查询";
                         SelectitemofQuery = SelectionItemEnu.allIncomeByfromsomeone.ToString();
                         TxbOTE.Visibility = Visibility.Visible;
                     }
                        else if (selectstr == "查询全部出账")
                      {
                       species = "通用出账查询";
                        SelectitemofQuery = SelectionItemEnu.allExpend.ToString();
                       }
                        else if (selectstr == "查询可报销的")
                       {
                         species = "通用出账查询";
                          SelectitemofQuery = SelectionItemEnu.allexpendbyKBX.ToString();
                         }
                         else if (selectstr == "查询自己吃饭的详细账单")
                         {
                              species = "通用出账查询";
                               SelectitemofQuery = SelectionItemEnu.ExpendByQMED.ToString();
                          }       
                         else if(selectstr == "查询自己吃饭的汇总账单")
                         {
                              species = "自己吃饭汇总";
                               SelectitemofQuery = SelectionItemEnu.ExpendByQMES.ToString();
                         }
                         
                        
                         
               
         }

        //查询按钮单击事件
        private void BtnQuery_Click(object sender, RoutedEventArgs e)
            {

               Resultstb = new DataTable();
               rePortDp = new CDataoption();

                  if (DPStart.SelectedDate==null||DPend.SelectedDate==null)
                     {
                        MessageBox.Show("查询日期输入错误，请重新选择","警告");
                        return;
                     }
            
                      SDate = DPStart.SelectedDate.Value.ToShortDateString();
                      Edate = DPend.SelectedDate.Value.ToShortDateString();

                       if (SelectitemofQuery == "allAttendanceBYsomeone")
                          {
                                  if (TxbOTE.Text.Trim() == "输入帮忙对象")
                                  {
                                           TxbOTE.Focus();
                                  }    
                                  else
                                  {
                                             helper = TxbOTE.Text.Trim();
                                   }
                            }
                 
                           if(SelectitemofQuery == "allIncomeByfromsomeone")
                            {
                                  if (TxbOTE.Text.Trim() == "输入进账来源")
                                 {
                                          TxbOTE.Focus();
                                  }
                                 else
                                  {
                                        IncomeSource = TxbOTE.Text.Trim();
                                     }
                             }
               
                     switch (SelectitemofQuery)
                     {
                            case   "Allattendance":  //查询全部查询
                                    Resultstb= rePortDp.GetAllAttendanceinfo(SDate,Edate);
                                    SetDataGridHearedOfAttendance(DGview,Resultstb);
                                 break;
                             case   "allAttendanceBYsomeone": //给某人帮忙的出勤    
                                     Resultstb = rePortDp.GetGiveapersontohelp(SDate, Edate, helper);
                                     SetDataGridHearedOfAttendance(DGview, Resultstb);
                                 break;
                             case    "Nhelpattendance"://未给人帮忙的出勤
                                      Resultstb = rePortDp.GetNomalAttendance(SDate,Edate);
                                      SetDataGridHearedOfAttendance(DGview, Resultstb);
                                  break;
                             
                              case   "allIncomeByfromsomeone": //查询来自于某人的进账
                                       Resultstb = rePortDp.GetIncomeofsomeone(SDate, Edate, IncomeSource);
                                       SetDataGridHearedOfIncome(DGview, Resultstb);
                                    break;
                              case   "allExpend":
                                          Resultstb = rePortDp.GetAllExpendinfo(SDate,Edate);
                                          SetDataGridHearedOfExpend(DGview,Resultstb);
                                     break;

                                case   "allexpendbyKBX":
                                         Resultstb = rePortDp.GetExpendInfo(SDate, Edate, "可报销的");
                                         SetDataGridHearedOfExpend(DGview, Resultstb);
                                    break;
                                case    "ExpendByQMED":
                                         Resultstb = rePortDp.GetExpendInfo(SDate, Edate, "自己吃饭");
                                         SetDataGridHearedOfExpend(DGview, Resultstb);
                                    break;
                                 case   "ExpendByQMES":
                                         Resultstb = rePortDp.GetmyselfmealExpend(SDate, Edate);
                                         SetDataGridHearedOfEat(DGview,Resultstb);
                                    break;
                          
                       }
            rePortDp.Dispose();
            }

        //导出按钮单击事件
        private void BtnSave_Click(object sender, RoutedEventArgs e)
            {

                    CSavetoExcel savetoExcel = new CSavetoExcel(Resultstb, species,TBCountMessage.Text.Trim());
                     if(savetoExcel.Accomplish)
                        {
                           MessageBox.Show("保存成功");
                        }
                       else
                        {
                         MessageBox.Show("保存失败");
                         }
                 savetoExcel.Dispose();



            }
     
        
        #region  查询到结果后设置DataGrid显示的内容及列头

        /// <summary>
        /// 通用的出勤表头设置
        /// </summary>
        /// <param name="dgView">DataGrid实例</param>
        /// <param name="dataTable">源数据表</param>
        private void SetDataGridHearedOfAttendance(DataGrid dgView, DataTable dataTable)
            {
            dgView.ItemsSource = dataTable.DefaultView;
            dgView.Columns.Clear();

            TBCountMessage.Text = "    从  " + SDate + "  到  " + Edate + "   " +
                "期间的出勤统计信息为: 正常出勤天数为:  "+rePortDp.NomalAttendancedays +"  天，半天数为:  "+rePortDp.HalfdayAttendancedays +"  个,加班时长为:  "+rePortDp.Workovertime+"  小时";

            DataGridTextColumn dategridcolumn = new DataGridTextColumn();
            dategridcolumn.Header = "出勤日期";
            dategridcolumn.Width = 180;
            dategridcolumn.Binding = new Binding("AttendanceDate");
            dategridcolumn.Binding.StringFormat = "yyyy年MM月dd日";
            dgView.Columns.Add(dategridcolumn);

            DataGridTextColumn DGTCattendancestatus = new DataGridTextColumn();
            DGTCattendancestatus.Header = "出勤状态";
            DGTCattendancestatus.Width = 120;
            DGTCattendancestatus.Binding = new Binding("AttendanceStatus");
            dgView.Columns.Add(DGTCattendancestatus);


            DataGridTextColumn DGTCattendanceHelper = new DataGridTextColumn();
            DGTCattendanceHelper.Header = "帮忙对象";
            DGTCattendanceHelper.Width = 120;
            DGTCattendanceHelper.Binding = new Binding("Helper");
            dgView.Columns.Add(DGTCattendanceHelper);


            DataGridTextColumn DGTCworkovertime = new DataGridTextColumn();
            DGTCworkovertime.Header = "加班时长";
            DGTCworkovertime.Width = 105;
            DGTCworkovertime.Binding = new Binding("WorkOfTime");
            DGTCworkovertime.Binding.StringFormat="0.0";
            dgView.Columns.Add(DGTCworkovertime);

            DataGridTextColumn DGTCattendanceremark = new DataGridTextColumn();
            DGTCattendanceremark.Header = "出勤备注";
            DGTCattendanceremark.Width = 300;
            DGTCattendanceremark.Binding = new Binding("AttendanceRemark");
            dgView.Columns.Add(DGTCattendanceremark);
            SetBtnSaveIsEnable(dgView);


            }
      
        /// <summary>
        /// 通用的进账表头设置
        /// </summary>
        /// <param name="dgView">目标DataGrid实例</param>
        /// <param name="dataTable">源数据表</param>
        private void SetDataGridHearedOfIncome(DataGrid dgView, DataTable dataTable)
            {

            dgView.ItemsSource = dataTable.DefaultView;
            dgView.Columns.Clear();

            TBCountMessage.Text = "  从 " + SDate + " 到 " + Edate + " 期间来自 " + IncomeSource + "  的进账一共有 " + rePortDp.Timecount + " 次,共" + rePortDp.IncomeAmountCount + " 元";

            DataGridTextColumn DGTCincomedate = new DataGridTextColumn();
            DGTCincomedate.Header = "进账日期";
            DGTCincomedate.IsReadOnly = true;
            DGTCincomedate.Width = 180;
            DGTCincomedate.Binding = new Binding("IncomeDate");
            DGTCincomedate.Binding.StringFormat = "yyyy年/MM月/dd日";
            dgView.Columns.Add(DGTCincomedate);


            DataGridTextColumn DGTCincomeamount = new DataGridTextColumn();
            DGTCincomeamount.Header = "进账金额(元)";
            DGTCincomeamount.Width = 120;
            DGTCincomeamount.Binding = new Binding("IncomeAmount");
            DGTCincomeamount.Binding.StringFormat = "C";
            dgView.Columns.Add(DGTCincomeamount);


            DataGridTextColumn DGTCincomeSource = new DataGridTextColumn();
            DGTCincomeSource.Header = "进账来源";
            DGTCincomeSource.Width = 150;
            DGTCincomeSource.Binding = new Binding("IncomeSource");
            dgView.Columns.Add(DGTCincomeSource);



            DataGridTextColumn DGTCincometype = new DataGridTextColumn();
            DGTCincometype.Header = "进账类型";
            DGTCincometype.Width = 150;
            DGTCincometype.Binding = new Binding("IncomeaType");
            dgView.Columns.Add(DGTCincometype);

            DataGridTextColumn DGTCincomeremark = new DataGridTextColumn();
            DGTCincomeremark.Header = "进账备注";
            DGTCincomeremark.Width = 300;
            DGTCincomeremark.Binding = new Binding(" IncomeRemark");
            dgView.Columns.Add(DGTCincomeremark);
            SetBtnSaveIsEnable(dgView);
            }
       
        
        /// <summary>
        /// 通用的出账表头设置
        /// </summary>
        /// <param name="dgView">目标DataGrid实例</param>
        /// <param name="dataTable">源数据表</param>
        private void SetDataGridHearedOfExpend(DataGrid dgView, DataTable dataTable)
            {

            dgView.ItemsSource = dataTable.DefaultView;
            dgView.Columns.Clear();

            TBCountMessage.Text = "  从 "+SDate +"  到 "+Edate+"  期间，总出账: "+rePortDp.ExpendAmountCount + " 元";

            DataGridTextColumn DGTCexpenddate = new DataGridTextColumn();
            DGTCexpenddate.Header = "支出日期";
            DGTCexpenddate.Width = 180;
            DGTCexpenddate.IsReadOnly = true;
            DGTCexpenddate.Binding = new Binding("ExpendDate");
            DGTCexpenddate.Binding.StringFormat = "yyyy年MM月dd日";
            dgView.Columns.Add(DGTCexpenddate);


            DataGridTextColumn DGTCexpendamount = new DataGridTextColumn();
            DGTCexpendamount.Header = "支出金额(元)";
            DGTCexpendamount.Width = 120;
            DGTCexpendamount.Binding = new Binding("ExpendAmount");
            DGTCexpendamount.Binding.StringFormat = "C";
            dgView.Columns.Add(DGTCexpendamount);

            DataGridTextColumn DGTCexpendtype = new DataGridTextColumn();
            DGTCexpendtype.Header = "支出类型";
            DGTCexpendtype.Width = 150;

            DGTCexpendtype.Binding = new Binding("ExpendType");
            dgView.Columns.Add(DGTCexpendtype);

            DataGridTextColumn DGTCexpendremark = new DataGridTextColumn();
            DGTCexpendremark.Header = "支出备注";
            DGTCexpendremark.Width = 300;
            DGTCexpendremark.Binding = new Binding("ExpendRemark");
            dgView.Columns.Add(DGTCexpendremark);
            SetBtnSaveIsEnable(dgView);

            }
        
        
        /// <summary>
        /// 吃饭汇总表头设置
        /// </summary>
        /// <param name="dgView">目标DataGrid实例</param>
        /// <param name="dataTable">源数据表</param>
        private void SetDataGridHearedOfEat(DataGrid dgView, DataTable dataTable)
          {
          
            dgView.ItemsSource = dataTable.DefaultView;
            dgView.Columns.Clear();

            TBCountMessage.Text = "  从 "+SDate+"  到 "+Edate +"  期间,自己吃饭的天数为:  "+rePortDp .Timecount+" 天,一共花费: "+rePortDp.ExpendAmountCount +" 元";

             DataGridTextColumn DGTCexpenddate = new DataGridTextColumn();
             DGTCexpenddate.Header = "支出日期";
             DGTCexpenddate.Width = 180;
              
             DGTCexpenddate.IsReadOnly = true;
             DGTCexpenddate.Binding = new Binding("ExpendDate");
             DGTCexpenddate.Binding.StringFormat = "yyyy年MM月dd日";
             dgView.Columns.Add(DGTCexpenddate);


             DataGridTextColumn DGTCexpendamount = new DataGridTextColumn();
             DGTCexpendamount.Header = "支出金额(元)";
             DGTCexpendamount.Width = 120;
             DGTCexpendamount.Binding = new Binding("EatmealAmount");
             DGTCexpendamount.Binding.StringFormat = "C";
             dgView.Columns.Add(DGTCexpendamount);

              SetBtnSaveIsEnable(dgView);

            }


        #endregion
        




        /// <summary>
        /// 当查询出结果后设置导出(保存)按钮的 IsEnable属性
        /// </summary>
        /// <param name="dgView"></param>
        private void SetBtnSaveIsEnable(DataGrid dgView)
            {
                if (dgView.Items.Count > 0)
                   {
                      btnSave.IsEnabled = true;
                   }
                 else
                  {
                     btnSave.IsEnabled = false;
                  }
            }

        /// <summary>
        /// 条件输入文本框得到焦点时的事件  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbOTE_GotFocus(object sender, RoutedEventArgs e)
            {
                Color backcolor = new Color();
                Color forcolor = new Color();
                backcolor = Color.FromRgb((byte)204, (byte)232, (byte)207);//199,237,204
                forcolor = Color.FromRgb((byte)28, (byte)28, (byte)28);//144,238,144
                TxbOTE.Clear();
                TxbOTE.Background = new SolidColorBrush(backcolor);
                TxbOTE.Foreground = new SolidColorBrush(forcolor);
                TxbOTE.TextAlignment = TextAlignment.Center;
            }

        /// <summary>
        /// 条件输入文本框失去焦点后的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>用于做一些默认的设置</remarks>
        private void TxbOTE_LostFocus(object sender, RoutedEventArgs e)
        {
                if (string.IsNullOrEmpty(TxbOTE.Text))
                 {
                    if (PrimaryOPtion == "出勤查询")
                     {
                         TxbOTE.Text = helpobject;
                      }
                     else if (PrimaryOPtion == "进账查询")
                       {
                          TxbOTE.Text = IcomeSource;
                       }
                 }


        }

        private void Window_Closed(object sender, EventArgs e)
            {
            Welcomefrm welcomefrm = new Welcomefrm();
            welcomefrm.Show();
            this.Close();

            }

      
        }   
    
 }
