using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;

namespace PersonnelRecode
    {
    /// <summary>
    /// Welcome.xaml 的交互逻辑
    /// </summary>
    public partial class Welcomefrm : Window
        {
        public Welcomefrm()
            {
            

            InitializeComponent();
            }
        private string startdate;
        private string enddate;
        private string incomeSource;
        CultureInfo cn = CultureInfo.GetCultureInfo("zh-chs");
        private Thread threadLoadData;
        CDataoption welcomedataoption = new CDataoption();
        private DataTable attTable;
        private DataTable incomeTable;
        private DataTable expendTable;
        private double expendCount;
        private double expendKBXCount;
        private string attCountstr;

        private string expendCountstr;
       

        /// <summary>
        /// 进入查询页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSerch_Click(object sender, RoutedEventArgs e)
            {
                rePortbynew portbynew = new rePortbynew();
                portbynew.Title = "查询页面 今天日期是:" + DateTime.Today.ToShortDateString();
                portbynew.ShowDialog();
                this.Hide();
            }
       
       
        /// <summary>
        /// 进入管理页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAmend_Click(object sender, RoutedEventArgs e)
            {
            Managefrm managefrm = new Managefrm();
            managefrm.Show();
            this.Hide();



            }
       
        /// <summary>
        /// 进入记录页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecord_Click(object sender, RoutedEventArgs e)
            {
                Recordfrm recordfrm = new Recordfrm();
                recordfrm.Title = "记录页面 今天日期是:" + DateTime.Today.ToShortDateString();
                recordfrm.ShowDialog();
                this.Hide();

            }

        //private void btnSetting_Click(object sender, RoutedEventArgs e)
        //    {
             
        //    }

        private void Window_Loaded(object sender, RoutedEventArgs e)
            {
                
            
            //初始化开始日期并获取程序配置里面的值
               startdate = "";
              startdate= ConfigurationManager.AppSettings["StartDate"];
               enddate = "";
              enddate = DateTime.Now.ToShortDateString().ToString(cn.DateTimeFormat);
               incomeSource = "";
               incomeSource = ConfigurationManager.AppSettings["IncomeSource"];

            //threadLoadData = new Thread(new ThreadStart(LoadData));
            //threadLoadData.Start();

            attCountstr = "";
            
            expendCountstr = "";

            LoadData();
           
            

            txbAttCountShow.SetValue(TextBlock.TextProperty, attCountstr);
            txbExpendCountShow.SetValue(TextBlock.TextProperty, expendCountstr);

            dgincomeshow.ItemsSource = incomeTable.DefaultView;

            if (string.IsNullOrEmpty(startdate))
                {
                MessageBox.Show("第一次使用，请设置统计开始日期");
                startDP.Focus();
                }
            else
                {
                startDP.Text = startdate;
                

                }

            if(string.IsNullOrEmpty(incomeSource))
                {
                MessageBox.Show("第一次使用，请设置进账来源");
                txbIncomeSource.Focus();

                }
            else
                {
                txbIncomeSource.Text = incomeSource;

                }

           

     


        }

        private void LoadData()
            {
            attTable = new DataTable();
            incomeTable = new DataTable();
            expendTable = new DataTable();
          

            attTable = welcomedataoption.GetAllAttendanceinfo(startdate, enddate);
            attCountstr=  "到目前为止，出勤统计如下:\r\n" +
                "正常出勤天数：" + welcomedataoption.NomalAttendancedays + " 天" +
                "半天数: " + welcomedataoption.HalfdayAttendancedays + " 个" +
                "加班时长: " + welcomedataoption.Workovertime + " 小时";


            expendTable = welcomedataoption.GetAllExpendinfo(startdate, enddate);
            expendCount = welcomedataoption.ExpendAmountCount;  //获取总支出

            incomeTable = welcomedataoption.GetIncomeCount(startdate,enddate); ;


            expendTable = welcomedataoption.GetExpendInfo(startdate, enddate, "可报销的");
            expendKBXCount = welcomedataoption.ExpendAmountCount; // 获取可报销支出 
           
            expendCountstr= "到目前为止,出账统计如下:\r\n" +"总出账金额 ：" + expendCount + " 元" + "其中可报销金额为:" + expendKBXCount + " 元";

            attTable.Dispose();
            incomeTable.Dispose();
            expendTable.Dispose();
            welcomedataoption.Dispose();


            }

        private void btnStartDateSetting_Click(object sender, RoutedEventArgs e)
            {

            if (startDP.SelectedDate.Value != null)
                {
                MessageBoxResult boxResult = MessageBox.Show("你已选择了一个日期,是否将它设定为统计起始日期", "设定提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (boxResult == MessageBoxResult.Yes)
                    {
                    Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    configuration.AppSettings.Settings["StartDate"].Value = startDP.SelectedDate.Value.ToShortDateString().ToString(cn.DateTimeFormat);
                    configuration.Save();
                    
                    }
                else
                    {

                    }
                }
            }

        private void txbIncomeSource_MouseDoubleClick(object sender, MouseButtonEventArgs e)
            {
               txbIncomeSource.Clear();

            }

        private void txbIncomeSource_LostFocus(object sender, RoutedEventArgs e)
            {
               if(string.IsNullOrEmpty(txbIncomeSource.Text))
                {
                MessageBox.Show("进账来源输入为空，将填入之前保存的数据");
                txbIncomeSource.Text = incomeSource;
                }
            }


        private void btnIncomeSourceSetting_Click(object sender, RoutedEventArgs e)
            {
            MessageBoxResult boxResult = MessageBox.Show("你已经输入了进账来源，是否保存","进账来源保存提示"
                ,MessageBoxButton.YesNo,MessageBoxImage.Question);

            if(boxResult==MessageBoxResult.Yes)
                {
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configuration.AppSettings.Settings["IncomeSource"].Value = txbIncomeSource.Text.Trim();
                configuration.Save();
                }

            }
       
        
        }
}
