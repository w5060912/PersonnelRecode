using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.UI.MobileControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Data;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Controls.Primitives;

namespace PersonnelRecode
{
    /// <summary>
    /// modificationWin.xaml 的交互逻辑
    /// </summary>
    public partial class modificationWin : Window
        {

        #region  变量声明区
        /*----------------------------变量描述--------------------------------
         *  1. 接收用户输入的起止日期
         *  2. 记录用户修改前的值
         *  3. 记录用户修改后的值
         *  4 .存储用户查询出来的（出勤状态，进账类型，出账类型）数据,这是个字符串列表
         *  5. 存储用户查询出来的（出勤状态，进账类型，出账类型）数据,这是个数据表
         *  6. 存储用户修改的表
         *  7. 存储用户修改的列名
         *  8. 定时器，实时更新时间
         *  9. 存储用户选择的修改项目
         *  10.统计查询结果的条目数
         *  11.获取用户修改的记录的日期,方便后面更新到数据库
         *  12.获取当前的日期
         *  13.获取用户修改的列在数据库里面的列名
         *  14.获取用户修改的记录的行号
         *  15.标记是否已经结束编辑
         *  16.获取用户当前选取的行
         *  17.原加班时长
         *  18.新加时长
         *  19.判断存储的时候是否成功
         *  20.获取进账/ 出账原编号
         *  21.DEBUG处理错误的
         */

        /// <summary>
        /// 开始日期
        /// </summary>
        string startdate;

        /// <summary>
        /// 结束日期
        /// </summary>
        string enddate;

        /// <summary>
        /// 修改前的值
        /// </summary>
        string oldValue;

        /// <summary>
        /// 修改后的值
        /// </summary>
        string newValue;

        /// <summary>
        /// 存储用户查询出来的（出勤状态，进账类型，出账类型）数据,这是个字符串列表
        /// </summary>
        List<string> lsValue = new List<string>();

        /// <summary>
        /// 存储用户查询出来的（出勤状态，进账类型，出账类型）数据,这是个数据表
        /// </summary>
        DataTable listData;

        /// <summary>
        /// 存储用户修改的表
        /// </summary>
        DataTable alterTable;

        /// <summary>
        /// 存储用户修改的列名
        /// </summary>
        string alterColumnName;

        /// <summary>
        /// 定时器，实时更新时间
        /// </summary>
        private DispatcherTimer DTshowdatetime = new DispatcherTimer();

        /// <summary>
        /// 存储用户选择的要修改的项目
        /// </summary>
        string alterItemname;


        /// <summary>
        /// 统计查询结果的条目数
        /// </summary>
        int resultCount;

        /// <summary>
        /// 实例化数据处理类
        /// </summary>
        CDataoption Moddataoption = new CDataoption();

        /// <summary>
        /// 本地化的环境
        /// </summary>
        CultureInfo cn = CultureInfo.GetCultureInfo("zh-chs");

        /// <summary>
        /// 获取用户修改的记录的日期,方便后面更新到数据库
        /// </summary>
        string Date;

        /// <summary>
        /// 获取当前日期
        /// </summary>
        string Today;

        /// <summary>
        /// 获取用户修改的列在数据库里面的列名
        /// </summary>
        string Columnname;

        /// <summary>
        /// 获取用户修改的记录的行号
        /// </summary>
        int rowIndex = 0;

        /// <summary>
        /// 标记是否已经结束编辑
        /// </summary>
        bool Editend = false;

        /// <summary>
        /// 获取用户当前选取的行
        /// </summary>
        DataRowView drv;

        /// <summary>
        /// 原加班时长
        /// </summary>
        double OldWorkovertime;

        /// <summary>
        /// 新加班时长
        /// </summary>
        double NewWorkovertime;

        /// <summary>
        /// 标记执行更新的操作是否成功
        /// </summary>
        int Mark;

        /// <summary>
        /// 进账 /出账的源编号
        /// </summary>
        int SourceCode;

        /// <summary>
        /// 21 DEBUG处理错误 
        /// </summary>
        int y = 0;


        #endregion


        public modificationWin()
            {
            #region 变量初始化区

            startdate = "";
            enddate = "";
            oldValue = "";
            newValue = "";
            alterTable = new DataTable();
            alterColumnName = "";

            Date = "";
            Today = "";
            Columnname = "";
            rowIndex = 0;
            Editend = false;
            OldWorkovertime = 0;
            NewWorkovertime = 0;
            #endregion
            InitializeComponent();
            DTshowdatetime.Interval = TimeSpan.FromSeconds(0.1);
            DTshowdatetime.Tick += DTshowdatetime_Tick1;
            DTshowdatetime.Start();
            }

        /// <summary>
        /// 定时器实时更新显示时间的事件
        /// </summary>
        /// <param name = "sender" ></ param >
        /// < param name="e"></param>
        private void DTshowdatetime_Tick1(object sender, EventArgs e)
            {
            Today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", cn.DateTimeFormat);
            string nowDatetime = string.Concat(DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss", cn.DateTimeFormat));
            tbDatetimeShow.Text = "现在时间是:  " + nowDatetime;

            }

        private void Window_Loaded(object sender, RoutedEventArgs e)
            {
            Today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", cn.DateTimeFormat);
            DGresultshow.SelectionUnit = DataGridSelectionUnit.FullRow;
            DGresultshow.SelectionMode = DataGridSelectionMode.Single;

           

            }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
            {
            SBmessage.Margin = new Thickness(0, 0, 0, 0);
            this.tbDatetimeShow.HorizontalAlignment = HorizontalAlignment.Right;
            this.tbDatetimeShow.Margin = new Thickness(0, 0, 0, 0);
            }


        /// <summary>
        /// 查询按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSerch_Click(object sender, RoutedEventArgs e)
            {

         
           

            startdate = StartDP.SelectedDate.Value.ToShortDateString();
            enddate = EndDP.SelectedDate.Value.ToShortDateString();
            foreach (object obj in SPitemselect.Children)
                {
                RadioButton rad = obj as RadioButton;
                if (rad.IsChecked == true)
                    {
                    alterItemname = rad.Content.ToString();
                    }
                }

            switch (alterItemname)
                {
                case "修改出勤":
                    //获取用户指定的日期区间里面的出勤信息
                    alterTable = Moddataoption.GetAllAttendanceinfo(startdate, enddate);
                    //获取出勤状态
                    listData = Moddataoption.GetAttendanceStatus();

                    if (alterTable.Rows.Count > 0)
                        {
                        resultCount = alterTable.Rows.Count;
                        }  //获取查询结果的条目数
                    else
                        {
                        resultCount = 0;
                        MessageBox.Show("在输入的日期区间内未查询任何出勤记录数据");
                        }
                    tbCountShow.Text = "共查询到 " + tbCountShow + " 条记录";
                    //将获取的出勤状态集合添加到字符串列表里面
                    lsValue.Clear();
                    foreach (DataRow dr in listData.Rows)
                        {
                        String str = dr[0].ToString();
                        lsValue.Add(str);
                        }

                    DGresultshow.Columns.Clear();
                    DGresultshow.ItemsSource = alterTable.DefaultView;//设置Datagrid的数据源 


                    DataGridTextColumn DGTCattendancedate = new DataGridTextColumn();
                    DGTCattendancedate.Header = "出勤日期";
                    DGTCattendancedate.Width = 180;
                    DGTCattendancedate.IsReadOnly = true;
                    DGTCattendancedate.Binding = new Binding("AttendanceDate");
                    DGTCattendancedate.Binding.StringFormat = "yyyy年/MM月/dd日";
                    DGresultshow.Columns.Add(DGTCattendancedate);

                    DataGridComboBoxColumn DGTCattendancestatus = new DataGridComboBoxColumn();
                    DGTCattendancestatus.Header = "出勤状态";
                    DGTCattendancestatus.Width = 120;
                    DGTCattendancestatus.ItemsSource = lsValue;
                    DGTCattendancestatus.SelectedValueBinding = new Binding("AttendanceStatus");
                    DGresultshow.Columns.Add(DGTCattendancestatus);

                    DataGridTextColumn DGTCworkovertime = new DataGridTextColumn();
                    DGTCworkovertime.Header = "加班时长";
                    DGTCworkovertime.Width = 105;
                    DGTCworkovertime.Binding = new Binding("WorkOfTime");
                    DGresultshow.Columns.Add(DGTCworkovertime);

                    DataGridTextColumn DGTChelper = new DataGridTextColumn();
                    DGTChelper.Header = "帮忙对象";
                    DGTChelper.Width = 120;
                    DGTChelper.Binding = new Binding("Helper");
                    DGresultshow.Columns.Add(DGTChelper);

                    DataGridTextColumn DGTCattendanceremark = new DataGridTextColumn();
                    DGTCattendanceremark.Header = "出勤备注";
                    DGTCattendanceremark.Width = 250;
                    DGTCattendanceremark.Binding = new Binding("AttendanceRemark");
                    DGresultshow.Columns.Add(DGTCattendanceremark);
                     

                    break;

                case "修改进账":
                    //获取指定日期区间内查询到的进账记录
                    alterTable = Moddataoption.GetAllIncomeinfo(startdate, enddate);

                    
                    //获取进账类型
                    listData = Moddataoption.GetIncometype();

                    if (alterTable.Rows.Count > 0)
                        {

                        resultCount = alterTable.Rows.Count;
                        }
                    else
                        {
                        resultCount = 0;
                        MessageBox.Show("在指定的日期区间内未查询到任何进账记录数据");
                        }

                    tbCountShow.Text = "共查询到 " + tbCountShow + " 条记录";
                    //将获取的出勤状态集合添加到字符串列表里面
                    lsValue.Clear();
                    foreach (DataRow dr in listData.Rows)
                        {
                        String str = dr[0].ToString();
                        lsValue.Add(str);
                        }

                    DGresultshow.Columns.Clear();
                    DGresultshow.ItemsSource = alterTable.DefaultView;//设置Datagrid的数据源 

                    DataGridTextColumn DGTCID = new DataGridTextColumn();
                    DGTCID.Header = "编号";
                    DGTCID.IsReadOnly = true;
                    DGTCID.Width = 40;
                    DGTCID.Binding = new Binding("Id");
                    DGresultshow.Columns.Add(DGTCID);

                    DataGridTextColumn DGTCincomedate = new DataGridTextColumn();
                    DGTCincomedate.Header = "进账日期";
                    DGTCincomedate.IsReadOnly = true;
                    DGTCincomedate.Width = 180;
                    DGTCincomedate.Binding = new Binding("IncomeDate");
                    DGTCincomedate.Binding.StringFormat = "yyyy年/MM月/dd日";
                    DGresultshow.Columns.Add(DGTCincomedate);

                    DataGridTextColumn DGTCincomeamount = new DataGridTextColumn();
                    DGTCincomeamount.Header = "进账金额";
                    DGTCincomeamount.Width = 120;
                    
                    DGTCincomeamount.Binding = new Binding("IncomeAmount");
                    DGTCincomeamount.Binding.StringFormat ="C";
                    DGresultshow.Columns.Add(DGTCincomeamount);

                    DataGridTextColumn DGTCincomesource = new DataGridTextColumn();
                    DGTCincomesource.Header = "进账来源";
                    DGTCincomesource.Width = 120;
                    DGTCincomesource.Binding = new Binding("IncomeSource");
                    DGresultshow.Columns.Add(DGTCincomesource);

                    DataGridComboBoxColumn DGTCincometype = new DataGridComboBoxColumn();
                    DGTCincometype.Header = "进账类型";
                    DGTCincometype.Width = 150;
                    DGTCincometype.ItemsSource = lsValue;
                    DGTCincometype.SelectedValueBinding = new Binding("IncomeaType");
                    DGresultshow.Columns.Add(DGTCincometype);

                    DataGridTextColumn DGTCincomeremark = new DataGridTextColumn();
                    DGTCincomeremark.Header = "进账备注";
                    DGTCincomeremark.Width = 200;
                    DGTCincomeremark.Binding = new Binding("IncomeRemark");
                    DGresultshow.Columns.Add(DGTCincomeremark);
                   
                   
                    break;

                case "修改支出":
                    //指定日期区间内查询到的出账记录
                    alterTable = Moddataoption.GetAllExpendinfo(startdate, enddate);
                    //出账种类
                    listData = Moddataoption.GetExpendtype(); ;
                    if (alterTable.Rows.Count > 0)
                        {
                        resultCount = alterTable.Rows.Count;
                        }  //获取查询结果的条目数
                    else
                        {
                        resultCount = 0;
                        MessageBox.Show("在指定的日期区间内未查询到任何的出账记录数据");
                        }

                    tbCountShow.Text = "共查询到 " + tbCountShow + " 条记录";
                    //将获取的出勤状态集合添加到字符串列表里面
                    lsValue.Clear();
                    foreach (DataRow dr in listData.Rows)
                        {
                        String str = dr[0].ToString();
                        lsValue.Add(str);
                        }

                    DGresultshow.Columns.Clear();
                    DGresultshow.ItemsSource = alterTable.DefaultView;//设置Datagrid的数据源  

                  
                    DataGridTextColumn DGTCID1 = new DataGridTextColumn();
                    DGTCID1.Header = "编号";
                    DGTCID1.IsReadOnly = true;
                    DGTCID1.Width = 40;
                    DGTCID1.Binding = new Binding("Id");
                    DGresultshow.Columns.Add(DGTCID1);

                    DataGridTextColumn DGTCexpenddate = new DataGridTextColumn();
                    DGTCexpenddate.Header = "支出日期";
                    DGTCexpenddate.Width = 180;
                    DGTCexpenddate.IsReadOnly = true;
                    DGTCexpenddate.Binding = new Binding("ExpendDate");
                    DGTCexpenddate.Binding.StringFormat = "yyyy年MM月dd日";
                    DGresultshow.Columns.Add(DGTCexpenddate);

                    DataGridTextColumn DGTCexpendamount = new DataGridTextColumn();
                    DGTCexpendamount.Header = "支出金额";
                    DGTCexpendamount.Width = 120;
                    DGTCexpendamount.Binding = new Binding("ExpendAmount");
                    DGTCexpendamount.Binding.StringFormat = "C";
                    DGresultshow.Columns.Add(DGTCexpendamount);

                    DataGridComboBoxColumn DGTCexpendtype = new DataGridComboBoxColumn();
                    DGTCexpendtype.Header = "支出类型";
                    DGTCexpendtype.Width = 150;
                    DGTCexpendtype.ItemsSource = lsValue;
                    DGTCexpendtype.SelectedValueBinding = new Binding("ExpendType");
                    DGresultshow.Columns.Add(DGTCexpendtype);

                    DataGridTextColumn DGTCexpendremark = new DataGridTextColumn();
                    DGTCexpendremark.Header = "支出备注";
                    DGTCexpendremark.Width = 250;
                    DGTCexpendremark.Binding = new Binding("ExpendRemark");
                    DGresultshow.Columns.Add(DGTCexpendremark);


                    break;






                }



            }

        //进入编辑状态前
        private void DGresultshow_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
            {
            Editend = false;
            alterColumnName = e.Column.Header.ToString();
            drv = DGresultshow.SelectedItem as DataRowView;

            oldValue = drv[e.Column.DisplayIndex].ToString();
            rowIndex = e.Row.GetIndex();  //获取修改的行号

            //获取用户修改的列在数据库里面的列名
            Columnname = alterTable.Columns[e.Column.DisplayIndex].Caption;

            if(alterItemname=="修改出勤")
                {
                //获取修改的数据的日期
               

                }
            else  //修改进账及出账
                {
                

                }


            if (alterColumnName == "加班时长")
                {
                if (drv[1].ToString() != "有加班")
                    {
                    MessageBox.Show("当天出勤状态没有加班，所有不能修改加班时长", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                    e.Cancel = true;

                    }
                }
            }


        //编辑结束
        private void DGresultshow_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (Editend == true)
                {
                if (Mark > 0)
                    {
                    MessageBox.Show("修改成功");
                    }
                else
                    {
                    MessageBox.Show("修改失败");
                    }
                return;//避免进入循环状态

                }
            else
                {
                //获取新值     
                if (e.EditingElement is ComboBox)
                    {
                    newValue = ((e.EditingElement as ComboBox).SelectedValue.ToString());

                    }
                else if (e.EditingElement is TextBox)
                    {
                    newValue = ((e.EditingElement as TextBox).Text);
                    }

                if (newValue != oldValue) //代表用户修改了值
                    {
                    MessageBoxResult boxResult = MessageBox.Show("你已经修改了值，是否确认修改", "修改提示"
                    , MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (boxResult == MessageBoxResult.Yes) //保存修改
                        {
                        string tempdate;
                        switch (alterItemname)
                            {
                            case "修改出勤":
                                Date = alterTable.Rows[rowIndex][0].ToString();
                                tempdate = Date.Substring(0, 10);
                                Date = tempdate;
                                AttendanceUpdateOperation();
                                break;
                            case "修改进账":
                                //获取修改的数据的日期
                                Date = alterTable.Rows[rowIndex][1].ToString();
                                tempdate = Date.Substring(0, 10);
                                Date = tempdate;

                                //获取源编号，避免在修改/出账的时候一次修改多条记录
                                SourceCode = Convert.ToInt32(drv[0], cn.NumberFormat);
                                IncomeUpdateOperation();
                                break;
                            case "修改支出":
                                //获取修改的数据的日期
                                Date = alterTable.Rows[rowIndex][1].ToString();
                                tempdate = Date.Substring(0, 10);
                                Date = tempdate;
                                //获取源编号，避免在修改/出账的时候一次修改多条记录
                                SourceCode = Convert.ToInt32(drv[0], cn.NumberFormat);
                                ExpendUpdateOperation();
                                break;

                            }


                        }
                    else//取消修改回滚，只对当前操作有效
                        {
                        Editend = true;
                        DGresultshow.CancelEdit(DataGridEditingUnit.Cell);

                        }




                    }


                #region
                //if (alterColumnName == "出勤状态")
                //    {
                //    if (oldValue == "有加班" && newValue != "有加班")
                //        {
                //        Workovertime = 0;
                //        (DGresultshow.Columns[2].GetCellContent(DGresultshow.Items[rowIndex]) as TextBlock).SetValue(TextBlock.TextProperty, "0");
                //        Editend = true;
                //        DGresultshow.CommitEdit(DataGridEditingUnit.Cell, true);

                //        }
                //    else if (oldValue != "有加班" && newValue == "有加班")
                //        {
                //        MessageBox.Show("请输入加班时长");
                //        (DGresultshow.Columns[2].GetCellContent(DGresultshow.Items[rowIndex]) as TextBlock).Focus();
                //        Editend = true;
                //        DGresultshow.CommitEdit(DataGridEditingUnit.Cell, true);
                //        Workovertime = double.Parse((DGresultshow.Columns[2].GetCellContent(DGresultshow.Items[rowIndex]) as TextBlock).Text, cn.NumberFormat);

                //        }
                //    else
                //        {
                //        Editend = true;
                //        DGresultshow.CommitEdit(DataGridEditingUnit.Cell, true);
                //        }
                //    }
                //else
                //    {
                //    Editend = true;
                //    DGresultshow.CommitEdit(DataGridEditingUnit.Cell, true);
                //    }


                #endregion

                }




            }

        #region 更新的操作
        
        /// <summary>
        /// 出勤更新的操作
        /// </summary>
        private void AttendanceUpdateOperation()
            {
             if (alterColumnName == "出勤状态") //如果修改的是出勤状态
               {
                y = 0;
                #region 同时修改了出勤状态及加班时长

                if (oldValue == "有加班" && newValue != "有加班") //针对从有加班改为没有加班的
                    {

                        OldWorkovertime = Convert.ToDouble(drv[2], cn.NumberFormat);//原加班时长

                        (DGresultshow.Columns[2].GetCellContent(DGresultshow.Items[rowIndex]) as TextBlock).SetValue(TextBlock.TextProperty, "0");

                        NewWorkovertime = 0; //新加班时长

                    Mark = Moddataoption.UpdateByAtt(Date, Columnname, newValue);
                    y = Mark;
                      if (Mark == 0)
                          {

                           Editend = true;
                           DGresultshow.CancelEdit(DataGridEditingUnit.Cell);
                           return;
                          }
                    Mark =   Moddataoption.SaveAttendanceModificationRecordToDatabase
                        (Today, Date, Columnname, oldValue, newValue);
                    y = Mark + y;
                    //if(y==1)
                    //    {
                    //    Editend = true;
                    //    DGresultshow.CancelEdit(DataGridEditingUnit.Cell);
                    //    return;
                    //    }


                    Mark = Moddataoption.UpdateByAtt(Date, "WorkOfTime", NewWorkovertime.ToString(cn.NumberFormat));
                    y = Mark + y;
                    if(y==2)
                        {
                        Editend = true;
                        DGresultshow.CancelEdit(DataGridEditingUnit.Cell);
                        return;
                        }
                 
                    Mark = Moddataoption.SaveAttendanceModificationRecordToDatabase
                       (Today, Date, "WorkOfTime", OldWorkovertime.ToString(cn.NumberFormat), NewWorkovertime.ToString(cn.NumberFormat));
                    y = Mark + y;
                    //if(y==3)
                    //    {
                    //    Editend = true;
                    //    DGresultshow.CancelEdit(DataGridEditingUnit.Cell);
                    //    return;
                    //    }


                    Editend = true;
                       DGresultshow.CommitEdit(DataGridEditingUnit.Cell, true);
                    }
                else if (oldValue != "有加班" && newValue == "有加班")
                    {
                    y = 0;
                       MessageBox.Show("请输入加班时长");
                       InputBox input = new InputBox();
                       bool? bt = input.ShowDialog();
                        if (bt == true)
                        {

                          OldWorkovertime = 0;
                          (DGresultshow.Columns[2].GetCellContent(DGresultshow.Items[rowIndex]) as TextBlock).SetValue(TextBlock.TextProperty, input.Workovertime.ToString(cn.NumberFormat));


                          NewWorkovertime = input.Workovertime; //新加班时长

                        Mark = Moddataoption.UpdateByAtt(Date, Columnname, newValue);
                        y = Mark;
                        if(y==0)
                            {
                            Editend = true;
                            DGresultshow.CancelEdit(DataGridEditingUnit.Cell);
                            return;
                            }

                        Mark = Moddataoption.SaveAttendanceModificationRecordToDatabase(Today, Date, Columnname, oldValue, newValue);
                           y = Mark + y;
                            //if(y==1)
                            //{
                            //Editend = true;
                            //DGresultshow.CancelEdit(DataGridEditingUnit.Cell);
                            //return;
                            //}

                        Mark = Moddataoption.UpdateByAtt(Date, "WorkOfTime", NewWorkovertime.ToString(cn.NumberFormat));
                        y = Mark + y;
                        if(y==2)
                            {
                            Editend = true;
                            DGresultshow.CancelEdit(DataGridEditingUnit.Cell);
                            return;
                            }


                        Mark = Moddataoption.SaveAttendanceModificationRecordToDatabase(Today, Date, "WorkOfTime", OldWorkovertime.ToString(cn.NumberFormat), NewWorkovertime.ToString(cn.NumberFormat));
                        y = Mark + y;
                        //if(y==3)
                        //    {
                        //    Editend = true;
                        //    DGresultshow.CancelEdit(DataGridEditingUnit.Cell);
                        //    return;
                        //    }
                       
                        Editend = true;
                           DGresultshow.CommitEdit(DataGridEditingUnit.Cell, true);

                        }

                    #endregion
                    }
                else //单独修改出勤状态
                    {
                    y = 0;
                    Mark = Moddataoption.UpdateByAtt(Date, Columnname, newValue);
                    y = Mark;
                     if (Mark == 0)
                        {

                        Editend = true;
                        DGresultshow.CancelEdit(DataGridEditingUnit.Cell);
                        return;
                        }
                    Mark = Moddataoption.SaveAttendanceModificationRecordToDatabase(Today, Date, Columnname, oldValue, newValue);
                    y = Mark + y;
                    //if(y==1)
                    //    {
                    //    Editend = true;
                    //    DGresultshow.CancelEdit(DataGridEditingUnit.Cell);
                    //    return;
                    //    }
                    
                    Editend = true;
                    DGresultshow.CommitEdit(DataGridEditingUnit.Cell, true);
                    }

               }
             else //对出修改出勤状态以外的单元格的操作
                {
                y = 0;


                //执行一个保存到数据库的操作
                Mark = Moddataoption.UpdateByAtt(Date, Columnname, newValue);
                y = Mark;
                if (Mark == 0)
                    {
                    Editend = true;
                    DGresultshow.CancelEdit(DataGridEditingUnit.Cell);
                    return;

                    }
                Mark = Moddataoption.SaveAttendanceModificationRecordToDatabase(Today, Date, Columnname, oldValue, newValue);
                    //y = Mark + y;
                    //if(y==1)
                    //{
                    //  Editend = true;
                    //   DGresultshow.CancelEdit(DataGridEditingUnit.Cell);
                    //  return;
                    // }

                Editend = true;
                DGresultshow.CommitEdit(DataGridEditingUnit.Cell, true);

                }


            }
        /// <summary>
        /// 进账更新的操作
        /// </summary>
        private void IncomeUpdateOperation()
            {
            y = 0;
            Mark = Moddataoption.UpdateByIncome(SourceCode, Columnname, newValue);
            y = Mark;
             if(Mark==0)
                {

                Editend = true;
                DGresultshow.CancelEdit(DataGridEditingUnit.Cell);
                return;
                }
            Mark = Moddataoption.SaveIncomeModificationRecordToDatabase(Today, SourceCode, Date, Columnname, oldValue, newValue);
            y = Mark + y;
               //if(y==1)
               //  {
               //  Editend = true;
               //  DGresultshow.CancelEdit(DataGridEditingUnit.Cell);
               //  return;
               // }
             
            Editend = true;
            DGresultshow.CommitEdit(DataGridEditingUnit.Cell,true);
            }
        /// <summary>
        /// 出账更新的操作
        /// </summary>
        private void ExpendUpdateOperation()
            {
            y = 0;
            
            Mark = Moddataoption.UpdateByExpend(SourceCode, Columnname, newValue);
            y = Mark;
            if (Mark == 0)
                {

                Editend = true;
                DGresultshow.CancelEdit(DataGridEditingUnit.Cell);
                return;
                }

            Mark = Moddataoption.SaveExpendModificationRecordToDatabase(Today, SourceCode, Date, Columnname, oldValue, newValue);
            y = Mark + y;
              //if(y==1)
              //  {
              //  Editend = true;
              //  DGresultshow.CancelEdit(DataGridEditingUnit.Cell);
              //  return;
              //  }

            Editend = true;
            DGresultshow.CommitEdit(DataGridEditingUnit.Cell, true);

            }





        #endregion

        private void Window_Closed(object sender, EventArgs e)
            {
            Managefrm managefrm = new Managefrm();
            managefrm.Show();
          
            }
        }
    }









    

