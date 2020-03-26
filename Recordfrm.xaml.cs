using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Controls;

namespace PersonnelRecode
    {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Recordfrm : Window
        {
        #region 类级变量声明区

        private double AccountsAmount;

        /// <summary>
        /// 账目备注
        /// </summary>
        private string AccountsRemark;

        /// <summary>
        /// 出勤备注
        /// </summary>
        private string AttendanceRemark;

        /// <summary>
        /// 出勤状态
        /// </summary>
        private string AttendanceStatus;

        /// <summary>
        /// 帮忙对象
        /// </summary>
        private string Helper;

        /// <summary>
        /// 账目金额
        /// </summary>
        /// <summary>
        /// 进账来源
        /// </summary>
        private String IncomeSource;

        /// <summary>
        /// 进账类型
        /// </summary>
        private string IncomeType;

        /// <summary>
        /// 判断
        /// </summary>
        private bool judge;

        /// <summary>
        /// 支出类型
        /// </summary>
        private string ExpendType;

        /// <summary>
        /// 对数据的记录操作
        /// </summary>
        private CDataoption RecordDataoptin;

        /// <summary>
        /// 记录日期
        /// </summary>
        private String RecordDate;

        /// <summary>
        /// 加班时长
        /// </summary>
        private double Workovertime;

        #endregion 类级变量声明区

        CultureInfo cn = CultureInfo.GetCultureInfo("zh-CN");
        ResourceManager stringManager = new ResourceManager("zh-CN", Assembly.GetExecutingAssembly());

        public Recordfrm()
            {
            InitializeComponent();


        
            RecordDate = "";

            Helper = "";
            AttendanceStatus = "";
            Workovertime = 0;
            AttendanceRemark = "";

            IncomeType = "";
            IncomeSource = "";
            ExpendType = "";
            AccountsAmount = 0;
            AccountsRemark = "";

            judge = false;

            RecordDataoptin = new CDataoption();

            //绑定到对应的数据表
            CombAttendanceStatus.ItemsSource = RecordDataoptin.GetAttendanceStatus().DefaultView;
            //设置要显示的列
            CombAttendanceStatus.DisplayMemberPath = "AttendanceStatus";
            //设置默认选项的下标
            CombAttendanceStatus.SelectedIndex = 3;

            combIncomeType.ItemsSource = RecordDataoptin.GetIncometype().DefaultView;
            combIncomeType.DisplayMemberPath = "IncomeType";
            combIncomeType.SelectedIndex = 1;

            CombExpendType.ItemsSource = RecordDataoptin.GetExpendtype().DefaultView;
            CombExpendType.DisplayMemberPath = "ExpendType";
            CombExpendType.SelectedIndex = 1;
            }

        /// <summary>
        /// 账目记录按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAccountsRecord_Click(object sender, RoutedEventArgs e)
            {
            RecordDate = RecordDTP.SelectedDate.Value.ToShortDateString();

            AccountsRemark = txbRemark.Text.Trim();

            //MessageBox.Show(RecordDTP.SelectedDate.Value);

            if (string.IsNullOrEmpty(txbAmountinput.Text.Trim()))
                {
                System.Windows.MessageBox.Show("金额输入不得为空", stringManager.GetString("        提示",CultureInfo.CurrentCulture), MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
                }

            string ResultStr = txbAmountinput.Text.Replace(" ", "");
            AccountsAmount = double.Parse(ResultStr ,cn.NumberFormat);

            if (RadIncomeRecord.IsChecked == true)
                {
                IncomeSource = txbIncomeSource.Text.Trim();
                IncomeType = combIncomeType.Text;
                MessShowBoxfrm messShow = new MessShowBoxfrm("进账信息存储提示", 
                    "进账信息如下:" 
                  + "\r\n进账日期: " + RecordDate
                  + "\r\n进账来源: " + IncomeSource
                  + "\r\n进账金额: " + AccountsAmount + " 元"
                  + "\r\n进账类型: " + IncomeType
                  + "\r\n进账备注: " + AccountsRemark
                   );
                
                judge = (bool)messShow.ShowDialog();
                if (!judge)
                    {
                    // MessageBox.Show("你选择了取消");
                    return;
                    }
                else
                    {
                    // MessageBox.Show("你选择了确认");
                    RecordDataoptin.InsertIncomeDataToDATABASE(RecordDate, IncomeSource, AccountsAmount, IncomeType, AccountsRemark);
                    MessageBox.Show("进账信息存储成功", stringManager.GetString("        提示", CultureInfo.CurrentCulture));
                    txbAmountinput.Clear();
                    txbRemark.Clear();
                    txbIncomeSource.Clear();
                    RecordDataoptin.Dispose();
                    }
                }
            else//选择记录支出
                {
                ExpendType = CombExpendType.Text;
                MessShowBoxfrm messShow = new MessShowBoxfrm("支出信息存储提示", 
                    "支出信息如下:" 
                +" \r\n支出日期: " + RecordDate
                + "\r\n支出金额: " + AccountsAmount + " 元"
                + "\r\n支出类型: " + ExpendType
                + "\r\n支出备注: " + AccountsRemark
                  );
               ;
                     judge = (bool)messShow.ShowDialog();
                     if (!judge)
                    {
                    // MessageBox.Show("你选择了取消");
                    return;
                    }
                    else
                       {
                       // MessageBox.Show("你选择了确认");
                         RecordDataoptin.InsertExpendDataToDATABASE(RecordDate, ExpendType, AccountsAmount, AccountsRemark);
                         MessageBox.Show("支出信息存储成功", stringManager.GetString("        提示", CultureInfo.CurrentCulture));
                    txbAmountinput.Clear();
                    txbRemark.Clear();
                    RecordDataoptin.Dispose();

                    }


                }
            }

        /// <summary>
        /// 单击出勤记录按钮触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAttendanceRecord_Click(object sender, RoutedEventArgs e)
            { 
                 RecordDate = RecordDTP.SelectedDate.Value.ToShortDateString();   
                 Workovertime = double.Parse(txbWorkOvertime.Text.Trim(),cn.NumberFormat);
                
            
            AttendanceRemark = TxbAttendanceRemark.Text.Trim();

                 DataRowView view = (DataRowView)CombAttendanceStatus.SelectedItem;
                 AttendanceStatus = view.Row["AttendanceStatus"].ToString();
              
                
                 Helper = txbhelp.Text;

            MessShowBoxfrm showBoxfrm = new MessShowBoxfrm("出勤记录信息存储提示","出勤信息如下:"
               + "\r\n 出勤日期:  "+RecordDate
               + "\r\n 出勤状态:  "+AttendanceStatus
               + "\r\n 加班时长:  "+Workovertime
               + "\r\n 帮忙对象:  "+Helper
               + "\r\n 出勤备注:  "+AttendanceRemark
                 );

            judge = (bool)showBoxfrm.ShowDialog();
            if (!judge)
                {
                // MessageBox.Show("你选择了取消");
                return;
                }
            else
                {
                // MessageBox.Show("你选择了确认");
                   RecordDataoptin.InsertAttendanceDatatoDATABASE(RecordDate, Helper, AttendanceStatus, Workovertime, AttendanceRemark);
                   MessageBox.Show("出勤信息存储成功", stringManager.GetString("        提示", CultureInfo.CurrentCulture));
                txbRemark.Clear();
                txbhelp.Clear();
                RecordDataoptin.Dispose();
                }



            }

        /// <summary>
        /// 窗体关闭后触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
            {
            Welcomefrm welcomefrm = new Welcomefrm();
            welcomefrm.Show();
            this.Close();
            }

        /// <summary>
        /// 窗体载入时触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
            {
            RecordDTP.DisplayDateEnd = System.DateTime.Now;
            RecordDTP.SelectedDate = System.DateTime.Today;
            Chkhelp.IsChecked = false;
            txbhelp.IsEnabled = false;
            txbWorkOvertime.IsEnabled = false;
            txbhelp.SetValue(TextBox.TextProperty, "未帮忙");   
            combIncomeType.IsEnabled = false;
            txbIncomeSource.SetValue(TextBox.TextProperty, ConfigurationManager.AppSettings["IncomeSource"]);
            txbIncomeSource.IsEnabled = false;
            }

        #region 帮忙选项框选择情况发生改变时的事件

        private void Chkhelp_Checked(object sender, RoutedEventArgs e)
            {
            txbhelp.IsEnabled = true;
            }

        private void Chkhelp_Unchecked(object sender, RoutedEventArgs e)
            {
            txbhelp.IsEnabled = false;
            }

        #endregion 帮忙选项框选择情况发生改变时的事件

        private void CombAttendanceStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
            DataRowView view = (DataRowView)CombAttendanceStatus.SelectedItem;
            string selectItem = view.Row["AttendanceStatus"].ToString();

            if (selectItem.Equals("有加班",StringComparison.InvariantCulture))
                {
                txbWorkOvertime.IsEnabled = true;
                }
            else
                {
                txbWorkOvertime.IsEnabled = false;
                }

            AttendanceStatus = selectItem;
            }

        private void Chkhelp_Checked_1(object sender, RoutedEventArgs e)
            {
               txbhelp.IsEnabled = true;
             
            }

        private void Chkhelp_Unchecked_1(object sender, RoutedEventArgs e)
            {
              txbhelp.IsEnabled = false;
              txbhelp.SetValue(TextBox.TextProperty,"帮忙");
            }

        private void RadIncomeRecord_Checked(object sender, RoutedEventArgs e)
            {
            txbIncomeSource.IsEnabled = true;
            combIncomeType.IsEnabled = true;
            CombExpendType.IsEnabled = false;

            }

        private void RadIncomeRecord_Unchecked(object sender, RoutedEventArgs e)
            {
            txbIncomeSource.IsEnabled = false;
            combIncomeType.IsEnabled = false;
            CombExpendType.IsEnabled = true;
            }
        }
    }