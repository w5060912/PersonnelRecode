using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Threading;

namespace PersonnelRecode
    {
    /// <summary>
    /// StartWindows.xaml 的交互逻辑
    /// </summary>
    public partial class StartWindows : Window, IDisposable
        {
        public StartWindows()
            {
            InitializeComponent();
            startdate = "";
            enddate = "";
            incomeDT = new DataTable();
            attDT = new DataTable();
            expendDT = new DataTable();
            _expendAmountCount = 0;
            _expendCountStr = "";
            _KBXexpendAmountCount = 0;
            _attCountStr = "";
            }
        DispatcherTimer timer = new DispatcherTimer();
        CDataoption dataoption = new CDataoption();
        CultureInfo cn =new CultureInfo("zh-chs");
        string startdate ="";
        string enddate ="";

        DataTable incomeDT =new DataTable();
        DataTable attDT =new DataTable();
        DataTable expendDT =new DataTable();
       
        double _expendAmountCount =0;

        double _KBXexpendAmountCount = 0;

        string _attCountStr ="";

        string _expendCountStr ="";
       


        private void Window_Loaded(object sender, RoutedEventArgs e)
            {
              //初始化
                attDT = new DataTable();
                incomeDT = new DataTable();
                expendDT = new DataTable();
                
              
             

               _expendAmountCount = 0;
               _KBXexpendAmountCount = 0;

                startdate = ConfigurationManager.AppSettings["StartDate"];
                enddate = DateTime.Now.ToShortDateString().ToString(cn.DateTimeFormat);

              TxkStatusShow.SetValue(TextBlock.TextProperty,"初始化对象");
              PBLoadData.Value = 1;

            //获取数据
            attDT = dataoption.GetAllAttendanceinfo(startdate, enddate);
              _attCountStr = "到目前为止,出勤统计如下\r\n" +
                  "正常出勤天数为: " + dataoption.NomalAttendancedays + "  天" +
                  "半天出勤数: " + dataoption.HalfdayAttendancedays + "  个" +
                  "加班时长为: " + dataoption.Workovertime + "  小时";
            TxkStatusShow.SetValue(TextBlock.TextProperty,"获取出勤查询数据");
            PBLoadData.Value = 2;
            
            incomeDT = dataoption.GetIncomeCount(startdate,enddate);
            TxkStatusShow.SetValue(TextBlock.TextProperty,"获取进账查询数据");
            PBLoadData.Value = 3;
             
            expendDT = dataoption.GetAllExpendinfo(startdate,enddate);
            _expendAmountCount = dataoption.ExpendAmountCount;

            expendDT = dataoption.GetExpendInfo(startdate, enddate, "可报销的");
            _KBXexpendAmountCount= dataoption.ExpendAmountCount;

            _expendCountStr = "到目前为止,出账统计如下\r\n" +
                "总出账金额: "+_expendAmountCount +" 元" +
                "其中可报销金额为 : "+_KBXexpendAmountCount +" 元";
           
            TxkStatusShow.SetValue(TextBlock.TextProperty,"获取出账查询数据");
            PBLoadData.Value = 4;


            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;
            timer.Start();

            
                 
            }

        private void Timer_Tick(object sender, EventArgs e)
            {
            double i = 0;
            i++;
            if(i==1)
                {
                 TxkStatusShow.SetValue(TextBlock.TextProperty,"数据载入完成");
                 PBLoadData.Value = 5;

                Welcomefrm welcomefrm = new Welcomefrm();
                welcomefrm.txbAttCountShow.SetValue(TextBlock.TextProperty,_attCountStr);
                welcomefrm.dgincomeshow.ItemsSource = incomeDT.DefaultView;
                welcomefrm.txbExpendCountShow.SetValue(TextBlock.TextProperty,_expendCountStr);
                welcomefrm.Show();
                timer.Stop();
                
                
                this.Close();

                }
            
            }

        public void Dispose()
            {
            attDT.Dispose();
            incomeDT.Dispose();
            expendDT.Dispose();
            }
        }
    }
