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
using System.Data;
using System.Globalization;

namespace PersonnelRecode
    {
    public partial class Alterfrm : Window, IDisposable
        {
        /// <summary>
        ///接收用户选择的要修改的项目名
        /// </summary>
        string AlterProjectname = "";

        /// <summary>
        /// 接收用户选择修改的项目的内容
        /// </summary>
        DataTable dtalteritems;

        /// <summary>
        /// 接收根据用户选择的修改项目于而得到的可供用户修改的列表内容
        /// </summary>
        DataTable dtlistsource;

        /// <summary>
        ///  介绍用户输入的日期
        /// </summary>
        string startdate;
        string enddate;

        /// <summary>
        /// 统计查询结果的条目数
        /// </summary>
        int resultCount;

        /// <summary>
        /// 统计更改了几项
        /// </summary>
        int edititemcount;



        /// <summary>
        /// 声明一个字符串的列表用于存储从数据库里面得到的
        /// 出勤状态，进账类型，出账类型之类的数据，与DGAlterView配合使用
        /// </summary>
        List<string> LSDGComboxColumnsItems = new List<string>();


        CDataoption alterDataOption = new CDataoption();

        public Alterfrm()
            {
            InitializeComponent();
            }

        private void Window_Closed(object sender, EventArgs e)
            {

            }

        private void Window_Loaded(object sender, RoutedEventArgs e)
            {
            DPStartddate.DisplayDateEnd = System.DateTime.Now;
            DPStartddate.SelectedDate = System.DateTime.Today;
            DPEnddate.DisplayDateEnd = System.DateTime.Now;
            DPEnddate.SelectedDate = System.DateTime.Today;

            LSDGComboxColumnsItems.Clear();
            }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
            {
            //RadioButton radioButton =e.Source as RadioButton;
            //if(radioButton.IsChecked==true)
            //{
            //    AlterProjectname = radioButton.Content.ToString();
            //}
            //  MessageBox.Show(AlterProjectname);
            }
        /// <summary>
        /// 查询按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSerch_Click(object sender, RoutedEventArgs e)
            {
            startdate = DPStartddate.SelectedDate.Value.ToShortDateString();
            enddate = DPEnddate.SelectedDate.Value.ToShortDateString();

            // MessageBox.Show("开始日期：" + startdate + "\r\n结束日期:" + enddate);


            foreach (object obj in SPItemSelect.Children)
                {
                RadioButton radio = obj as RadioButton;
                if (radio.IsChecked == true)
                    {
                    AlterProjectname = radio.Name.ToString();
                    }

                }

                switch (AlterProjectname)
                    {
                   case "radbtnAttendanceselect":    //选择修改出勤
                           dtalteritems = alterDataOption.GetAllAttendanceinfo(startdate, enddate);
                           dtlistsource = alterDataOption.GetAttendanceStatus();
                           DGview.Columns.Clear();
                           if (dtalteritems.Rows.Count > 0)
                             {
                               resultCount = dtalteritems.Rows.Count;
                             }
                            else
                             {
                               resultCount = 0;
                        
                             }

                             SBtxbinfo.Text = "     共查询到 "+ resultCount+" 条记录";

                             LSDGComboxColumnsItems.Clear();
                             foreach (DataRow dr in dtlistsource.Rows)
                               {
                                 string dstr = dr[0].ToString();
                                 LSDGComboxColumnsItems.Add(dstr);
                             

                               }

                               DGview.ItemsSource = dtalteritems.DefaultView;
                        
                               DataGridTextColumn DGTCattendancedate = new DataGridTextColumn();
                                   DGTCattendancedate.Header = "出勤日期";
                                   DGTCattendancedate.Width = 180;
                                   DGTCattendancedate.IsReadOnly = true;
                                   DGTCattendancedate.Binding = new Binding("AttendanceDate");
                                   DGTCattendancedate.Binding.StringFormat = "yyyy年/MM月/dd日";
                               DGview.Columns.Add(DGTCattendancedate);

                            DataGridComboBoxColumn DGTCattendancestatus = new DataGridComboBoxColumn();
                              DGTCattendancestatus.Header = "出勤状态";
                              DGTCattendancestatus.Width = 120;
                              DGTCattendancestatus.ItemsSource = LSDGComboxColumnsItems;
                              DGTCattendancestatus.SelectedValueBinding = new Binding("AttendanceStatus"); 
                           DGview.Columns.Add(DGTCattendancestatus);

                           DataGridTextColumn DGTCworkovertime = new DataGridTextColumn();
                             DGTCworkovertime.Header = "加班时长";
                             DGTCworkovertime.Width = 105;
                             DGTCworkovertime.Binding = new Binding("WorkOfTime");
                           DGview.Columns.Add(DGTCworkovertime);

                           DataGridTextColumn DGTChelper = new DataGridTextColumn();
                             DGTChelper.Header = "帮忙对象";
                             DGTChelper.Width = 120;
                             DGTChelper.Binding = new Binding("Helper");
                           DGview.Columns.Add(DGTChelper);

                        DataGridTextColumn DGTCattendanceremark = new DataGridTextColumn();
                          DGTCattendanceremark.Header = "出勤备注";
                          DGTCattendanceremark.Width = 250;
                          DGTCattendanceremark.Binding = new Binding("AttendanceRemark");
                        DGview.Columns.Add(DGTCattendanceremark);


                  break;
                   
                
                  case "radbtnIncomeselect":        //选择修改进账
                         dtalteritems = alterDataOption.GetAllIncomeinfo(startdate, enddate);
                         dtlistsource = alterDataOption.GetIncometype();

                         if(dtalteritems.Rows.Count>0)
                         {
                         resultCount = dtalteritems.Rows.Count;
                         }
                         else
                         {
                         resultCount = 0;
                         }

                         SBtxbinfo.Text = "     共查询到 " + resultCount + " 条记录";

                         LSDGComboxColumnsItems.Clear();

                          foreach (DataRow dr in dtlistsource.Rows)
                        {
                        string dstr = dr[0].ToString();
                        LSDGComboxColumnsItems.Add(dstr);


                        }
                          DGview.ItemsSource = dtalteritems.DefaultView;
                          DGview.Columns.Clear();

                          DataGridTextColumn DGTCincomedate = new DataGridTextColumn();
                            DGTCincomedate.Header = "进账日期";
                            DGTCincomedate.IsReadOnly = true;
                            DGTCincomedate.Width = 180;
                            DGTCincomedate.Binding = new Binding("IncomeDate");
                            DGTCincomedate.Binding.StringFormat = "yyyy年/MM月/dd日";
                          DGview.Columns.Add(DGTCincomedate);


                          DataGridTextColumn DGTCincomeamount = new DataGridTextColumn();
                            DGTCincomeamount.Header = "进账金额";
                            DGTCincomeamount.Width = 120;
                            DGTCincomeamount.Binding = new Binding("IncomeAmount");
                            DGTCincomeamount.Binding.StringFormat = "C";
                          DGview.Columns.Add(DGTCincomeamount);

                          DataGridTextColumn DGTCincomesource = new DataGridTextColumn();
                            DGTCincomesource.Header = "进账来源";
                            DGTCincomesource.Width = 120;
                            DGTCincomesource.Binding = new Binding("IncomeSource");
                          DGview.Columns.Add(DGTCincomesource);

                         DataGridComboBoxColumn DGTCincometype = new DataGridComboBoxColumn();
                           DGTCincometype.Header = "进账类型";
                           DGTCincometype.Width = 150;
                           DGTCincometype.ItemsSource = LSDGComboxColumnsItems;
                           DGTCincometype.SelectedValueBinding = new Binding("IncomeaType");
                         DGview.Columns.Add(DGTCincometype);

                         DataGridTextColumn DGTCincomeremark = new DataGridTextColumn();
                           DGTCincomeremark.Header = "进账备注";
                           DGTCincomeremark.Width = 200;
                           DGTCincomeremark.Binding = new Binding("IncomeRemark");
                         DGview.Columns.Add(DGTCincomeremark);

               break;
                    case "radbtnExpend":             //选择修改支出
                          dtalteritems = alterDataOption.GetAllExpendinfo(startdate, enddate);
                          dtlistsource = alterDataOption.GetExpendtype();
                          if (dtalteritems.Rows.Count > 0)
                        {
                        resultCount = dtalteritems.Rows.Count;
                        }
                          else
                        {
                        resultCount = 0;
                        }

                          SBtxbinfo.Text = "     共查询到 " + resultCount + " 条记录";

                          LSDGComboxColumnsItems.Clear();

                          foreach (DataRow dr in dtlistsource.Rows)
                        {
                        string dstr = dr[0].ToString();
                        LSDGComboxColumnsItems.Add(dstr);


                        }
                          DGview.ItemsSource = dtalteritems.DefaultView;
                          DGview.Columns.Clear();

                          DataGridTextColumn DGTCexpenddate = new DataGridTextColumn();
                            DGTCexpenddate.Header = "支出日期";
                            DGTCexpenddate.Width = 180;
                            DGTCexpenddate.IsReadOnly = true;
                            DGTCexpenddate.Binding = new Binding("ExpendDate");
                            DGTCexpenddate.Binding.StringFormat = "yyyy年MM月dd日";
                          DGview.Columns.Add(DGTCexpenddate);


                          DataGridTextColumn DGTCexpendamount = new DataGridTextColumn();
                            DGTCexpendamount.Header = "支出金额";
                            DGTCexpendamount.Width = 120;
                            DGTCexpendamount.Binding = new Binding("ExpendAmount");
                            DGTCexpendamount.Binding.StringFormat = "C";
                          DGview.Columns.Add(DGTCexpendamount);

                          DataGridComboBoxColumn DGTCexpendtype = new DataGridComboBoxColumn();
                            DGTCexpendtype.Header = "支出类型";
                            DGTCexpendtype.Width = 150;
                            DGTCexpendtype.ItemsSource = LSDGComboxColumnsItems;
                            DGTCexpendtype.SelectedValueBinding = new Binding("ExpendType");
                          DGview.Columns.Add(DGTCexpendtype);

                          DataGridTextColumn DGTCexpendremark = new DataGridTextColumn();
                            DGTCexpendremark.Header = "支出备注";
                            DGTCexpendremark.Width = 250;
                            DGTCexpendremark.Binding = new Binding("ExpendRemark");
                          DGview.Columns.Add(DGTCexpendremark);

                    break;


                    }

               


             

            }

        public void Dispose()
            {
            throw new NotImplementedException();
            }
        }


    }


       
       


    


    
    

         