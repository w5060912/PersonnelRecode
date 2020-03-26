using System;
using System.Collections.Generic;
using System.Data;
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
using System.Threading;

namespace PersonnelRecode
    {


    /// <summary>
    /// ViewAlterHistory.xaml 的交互逻辑
    /// </summary>
    public partial class ViewAlterHistory : Window
        {
        public ViewAlterHistory()
            {
            InitializeComponent();
            }
        
        /// <summary>
        /// 接收查询结果的数据表
        /// </summary>
        DataTable resultDT;
      
        /// <summary>
        /// 开始日期
        /// </summary>
        string startDate;
       
        /// <summary>
        /// 结束日期
        /// </summary>
        string endDate;
        
        /// <summary>
        /// 本地化的区域设置
        /// </summary>
        CultureInfo cn = CultureInfo.GetCultureInfo("zh-chs");
        /// <summary>
        /// 数据库数据处理类的实例
        /// </summary>
        private CDataoption viewdataoption = new CDataoption();
        /// <summary>
        /// 一个字符串的列表用来存储供用户选择的选项
        /// </summary>
        private List<string> items = new List<string>();

        /// <summary>
        /// 字符转换的一个线程
        /// </summary>
       private Thread threadConver;

        /// <summary>
        /// 窗体载入初始化，本窗体用到的变量将在这里进行初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
            {
                 items.Add("选择查看种类");
                 items.Add("查看出勤修改历史");
                 items.Add("查看进账修改历史");
                 items.Add("查看出账修改历史");
                 combSelectItem.ItemsSource = items;
                 combSelectItem.SelectedIndex = 0;

              startDate = "";
              endDate = "";
              resultDT = new DataTable();

            }

        private void combSelectItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
            
            }
        /// <summary>
        /// 查看按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, RoutedEventArgs e)
            {


            startDate = startDP.SelectedDate.Value.ToShortDateString().ToString(cn.DateTimeFormat);
            endDate = endDP.SelectedDate.Value.ToShortDateString().ToString(cn.DateTimeFormat);
            resultDT = new DataTable();
            if (combSelectItem.SelectedValue.ToString() == "查看出勤修改历史")
                {
                resultDT = viewdataoption.GetAttAlterHistoryBySourceRecordDate(startDate, endDate);
                AttHistoryShowSetting(resultDT, this.DGsee);
                if(resultDT.Rows.Count > 0)
                    {
                    threadConver = new Thread(new ThreadStart(AttConvet));
                    threadConver.Start();

                    }

                }
            else if (combSelectItem.SelectedValue.ToString() == "查看进账修改历史")
                {
                resultDT = viewdataoption.GetIncomeAlterHistoryBySourceRecordDate(startDate, endDate);
                IncomeHistroyShowSetting(resultDT, this.DGsee);
                if (resultDT.Rows.Count > 0)
                    {
                    threadConver = new Thread(new ThreadStart(IncomeConvert));
                    threadConver.Start();

                    }

                }

            else if (combSelectItem.SelectedValue.ToString() == "查看出账修改历史")
                {
                resultDT = viewdataoption.GetExpendAlterHistoryBySourceRecordDate(startDate, endDate);
                ExpendHistoryShowSetting(resultDT, this.DGsee);
                if (resultDT.Rows.Count > 0)
                    {
                    threadConver = new Thread(new ThreadStart(ExpendConvert));
                    threadConver.Start();

                    }
                }

            }
           

       
        /// <summary>
        /// 开始日期选择好后执行的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startDP_CalendarClosed(object sender, RoutedEventArgs e)
            {
               if (startDP.SelectedDate == null) return;
              this.endDP.DisplayDateStart = startDP.SelectedDate;
            }


        #region 数据显示界面设置
        private void AttHistoryShowSetting(DataTable parrentDT,DataGrid dg)
            {
                  if (parrentDT.Rows.Count < 0) return;
                 

            dg.ItemsSource = parrentDT.DefaultView;
                  dg.Columns.Clear();

                DataGridTextColumn DGCTID = new DataGridTextColumn();
                DGCTID.Header = "ID";
                DGCTID.Width = 40;
                DGCTID.Binding = new Binding("ID");
                dg.Columns.Add(DGCTID);

               DataGridTextColumn DGCTChangeTime = new DataGridTextColumn();
               DGCTChangeTime.Header = "修改时间";
               DGCTChangeTime.Width = 360;
               DGCTChangeTime.Binding = new Binding("ChangeTime");
               DGCTChangeTime.Binding.StringFormat = "yyyy年MM月dd日 HH时mm分ss秒";
                dg.Columns.Add(DGCTChangeTime);

                DataGridTextColumn DGCTAttendancedate = new DataGridTextColumn();
                DGCTAttendancedate.Header = "源出勤日期";
                DGCTAttendancedate.Width = 200;
                DGCTAttendancedate.Binding = new Binding("AttendanceDate");
                DGCTAttendancedate.Binding.StringFormat = "yyyy年MM月dd日";
                dg.Columns.Add(DGCTAttendancedate);

                 DataGridTextColumn DGCTAttColumnname = new DataGridTextColumn();
                 DGCTAttColumnname.Header = "源列名";
                 DGCTAttColumnname.Width = 120;
                 DGCTAttColumnname.Binding = new Binding("AttendanceOptions");
                 dg.Columns.Add(DGCTAttColumnname);

                  DataGridTextColumn DGCToldvalue = new DataGridTextColumn();
                  DGCToldvalue.Header = "原值";
                  DGCToldvalue.Width = 250;
                  DGCToldvalue.Binding = new Binding("Oldvalue");
                 dg.Columns.Add(DGCToldvalue);

                 DataGridTextColumn DGCTnewvalue = new DataGridTextColumn();
                 DGCTnewvalue.Header = " 新值";
                 DGCTnewvalue.Width = 250;
                 DGCTnewvalue.Binding = new Binding("Newvalue");
                dg.Columns.Add(DGCTnewvalue);


            }

        private void IncomeHistroyShowSetting(DataTable parrentDT, DataGrid dg)
            {
               if (parrentDT.Rows.Count < 0) return;
               dg.ItemsSource = parrentDT.DefaultView;
               dg.Columns.Clear();

               DataGridTextColumn DGCTIncomeId = new DataGridTextColumn();
               DGCTIncomeId.Header = "ID";
               DGCTIncomeId.Width = 40;
               DGCTIncomeId.Binding = new Binding("ID");
               dg.Columns.Add(DGCTIncomeId);

               DataGridTextColumn DGTCChangeTime = new DataGridTextColumn();
               DGTCChangeTime.Header = "修改时间 ";
               DGTCChangeTime.Width = 360;
               DGTCChangeTime.Binding = new Binding("ChangeTime");
               DGTCChangeTime.Binding.StringFormat = "yyyy年MM月dd日 HH时mm分ss秒";
                dg.Columns.Add(DGTCChangeTime);

              DataGridTextColumn DGTCIncomeId = new DataGridTextColumn();
              DGTCIncomeId.Header = "源ID";
              DGTCIncomeId.Width = 60;
              DGTCIncomeId.Binding = new Binding("IncomeId");
              dg.Columns.Add(DGTCIncomeId);

              DataGridTextColumn DGTCIncomeDate = new DataGridTextColumn();
              DGTCIncomeDate.Header = "源进账日期";
            DGTCIncomeDate.Width = 200;
              DGTCIncomeDate.Binding = new Binding("IncomeDate");
              DGTCIncomeDate.Binding.StringFormat = "yyyy年/MM月dd日";
              dg.Columns.Add(DGTCIncomeDate);

               DataGridTextColumn DGTCIncomeOptions = new DataGridTextColumn();
               DGTCIncomeOptions.Header = "源列名";
               DGTCIncomeOptions.Width = 120;
               DGTCIncomeOptions.Binding = new Binding("IncomeOptions");
               dg.Columns.Add(DGTCIncomeOptions);

              DataGridTextColumn DGTCOldvalue = new DataGridTextColumn();
              DGTCOldvalue.Header = "原值";
              DGTCOldvalue.Width = 250;
              DGTCOldvalue.Binding = new Binding("Oldvalue");
              dg.Columns.Add(DGTCOldvalue);

              DataGridTextColumn DGTCNewvalue = new DataGridTextColumn();
              DGTCNewvalue.Header = "新值";
              DGTCNewvalue.Width = 250;
              DGTCNewvalue.Binding = new Binding("Newvalue");
              dg.Columns.Add(DGTCNewvalue);

            }

        private void ExpendHistoryShowSetting(DataTable parrentDT, DataGrid dg)
            {
                if (parrentDT.Rows.Count < 0) return;
                dg.ItemsSource = parrentDT.DefaultView;
                dg.Columns.Clear();

                DataGridTextColumn DGTCId = new DataGridTextColumn();
                DGTCId.Header = "编号";
                DGTCId.Width = 40;
                DGTCId.Binding = new Binding("ID");
                dg.Columns.Add(DGTCId);

                DataGridTextColumn DGTCChangeTime = new DataGridTextColumn();
                DGTCChangeTime.Header = "修改时间";
                DGTCChangeTime.Width = 360;
                DGTCChangeTime.Binding = new Binding("ChangeTime");
                DGTCChangeTime.Binding.StringFormat = "yyyy年MM月dd日 HH时mm分ss秒";
                dg.Columns.Add(DGTCChangeTime);

                DataGridTextColumn DGTCExpendId = new DataGridTextColumn();
                DGTCExpendId.Header = "源ID";
                DGTCExpendId.Width = 60;
                DGTCExpendId.Binding = new Binding("ExpendId");
                dg.Columns.Add(DGTCExpendId);

                DataGridTextColumn DGTCExpendDate = new DataGridTextColumn();
                DGTCExpendDate.Header = "源出账日期";
                DGTCExpendDate.Width = 200;
                DGTCExpendDate.Binding = new Binding("ExpendDate");
                DGTCExpendDate.Binding.StringFormat = "yyyy年/MM月dd日";
                 dg.Columns.Add(DGTCExpendDate);

               DataGridTextColumn DGTCExpendOptions = new DataGridTextColumn();
               DGTCExpendOptions.Header = "源列名";
               DGTCExpendOptions.Width = 120;
               DGTCExpendOptions.Binding = new Binding("ExpendOptions");
               dg.Columns.Add(DGTCExpendOptions);

               DataGridTextColumn DGTCOldvalue = new DataGridTextColumn();
               DGTCOldvalue.Header = "原值";
               DGTCOldvalue.Width = 250;
               DGTCOldvalue.Binding = new Binding("Oldvalue");
               dg.Columns.Add(DGTCOldvalue);

               DataGridTextColumn DGTCNewvalue = new DataGridTextColumn();
               DGTCNewvalue.Header = "新值";
               DGTCNewvalue.Width = 250;
               DGTCNewvalue.Binding = new Binding("Newvalue");
               dg.Columns.Add(DGTCNewvalue);

             }
        #endregion

        #region 字符转换的多线程方法
        /// <summary>
        /// 出勤修改记录中英文换成中文的一个多线程方法
        /// </summary>
        private void AttConvet()
            {
            for (int i = 0; i < resultDT.Rows.Count; i++)
                {
                object obj = resultDT.Rows[i][3];
                if (obj.ToString() == "Helper")
                    {
                    resultDT.Rows[i][3] = "帮忙对象";
                    }
                else if (obj.ToString() == "AttendanceStatus")
                    {
                    resultDT.Rows[i][3] = "出勤状态";
                    }
                else if (obj.ToString() == "WorkOfTime")
                    {
                    resultDT.Rows[i][3] = "加班时长";
                    }
                else if (obj.ToString() == "AttendanceRemark")
                    {
                    resultDT.Rows[i][3] = "出勤备注";
                    }


                }
            threadConver.Abort();
            }
        /// <summary>
        /// 进账修改记录中英文换成中文的一个多线程方法
        /// </summary>
        private void IncomeConvert()
            {
            for (int i = 0; i < resultDT.Rows.Count; i++)
                {
                object obj = resultDT.Rows[i][4];
                if (obj.ToString() == "IncomeSource")
                    {
                    resultDT.Rows[i][4] = "进账来源";
                    }
                else if (obj.ToString() == "IncomeAmount")
                    {
                    resultDT.Rows[i][4] = "进账金额";
                    }
                else if (obj.ToString() == "IncomeaType")
                    {
                    resultDT.Rows[i][4] = "进账类型";
                    }
                else if (obj.ToString() == "IncomeRemark")
                    {
                    resultDT.Rows[i][4] = "进账备注";
                    }
                }
            }
        /// <summary>
        /// 出账修改记录中英文换成中文的一个多线程方法
        /// </summary>
        private void ExpendConvert()
            {
            for (int i = 0; i < resultDT.Rows.Count; i++)
                {
                object obj = resultDT.Rows[i][4];
                if (obj.ToString() == "ExpendType")
                    {
                    resultDT.Rows[i][4] = "出账类型";
                    }
                else if (obj.ToString() == "ExpendAmount")
                    {
                    resultDT.Rows[i][4] = "出账金额";
                    }
               
                else if (obj.ToString() == "ExpendRemark")
                    {
                    resultDT.Rows[i][4] = "出账备注";
                    }
                }
            threadConver.Abort();
            }
        #endregion

        private void Window_Closed(object sender, EventArgs e)
            {
            Managefrm managefrm = new Managefrm();
            managefrm.Show();
            }
        }
    }
