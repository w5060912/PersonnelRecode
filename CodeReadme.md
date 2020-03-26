# Markdown file
##  本文件旨在记录在程序编写过程中，想到的一些好方法以及未解决某些问题而寻找到的一些好的方法

##### 1.WPF应用程序中ComboBox控件绑定数据库的方法:

`<p><a> href="/home" title="WPF应用程序中ComboBox控件绑定数据库的方法"></a></p>`

###### 代码区:
```将ComboBox控件绑定到PersonelInwork/AttendanceStatusTable
  <p><a> CombAttendanceStatus.ItemsSource = RecordDataoptin.GetAttendanceStatus().DefaultView; </a></p>
  <p><a> CombAttendanceStatus.DisplayMemberPath = "AttendanceStatus"; </a></p>
  <p><a> CombAttendanceStatus.SelectedIndex = 3; </a></p>
```
###### *通过这种个方法，可以将对应数据表里面的数据显示在列表选项框里面.还可以定义第一个要显示的项目。**
##### 2.在WPF应用程序中实时获取ComboBox控件中选择的项目：
 ```
private void CombAttendanceStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
            
   DataRowView view = (DataRowView)CombAttendanceStatus.SelectedItem;
            string selectItem = view.Row["AttendanceStatus"].ToString();

            if (selectItem.Equals("有加班"))
                {
                txbWorkOvertime.IsEnabled = true;
                }
            else
                {
                txbWorkOvertime.IsEnabled = false;
                }

            AttendanceStatus = selectItem;
            }
```

###### *因为绑定是数据库，所以实时选择项目的类型是DataRowView**


#### 通过后台代码设置DataGrid中的类并绑定数据
```
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
  DGTCworkovertime.Width = 80;
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

```  
###### *通过这种方法可以动态的设置DataGrid中的列，并绑定数据，实现在一个dataGrid里面显示不同的内容*
######

DataGridTextColumn 列在进入编辑状态前的控件是TextBlock,进入编辑状态后
会变成TextBox
在给单元格赋值的时要注意这个问题
DataGrid直接给某个单元格赋值的代码:
```
   //修改当前行里面的加班时长的值
    (DGresultshow.Columns[2].GetCellContent(DGresultshow.Items[rowIndex])
    as TextBlock).SetValue(TextBlock.TextProperty, changefrm.Workovertime);
```
   //修改当前行里面的出勤状态的值
    (DGresultshow.Columns[1].GetCellContent(DGresultshow.Items[rowIndex])
    as ComboBox).SelectedValue = changefrm.Attstatus;
```                                
  //修改当前行里面的加班时长的值
(DGresultshow.Columns[2].GetCellContent(DGresultshow.Items[rowIndex])
as TextBox).SetValue(TextBox.TextProperty, changefrm.Workovertime);

```
#### 格式：(DataGrid实例.Columns[列号(从左往右，从0开始)].GetCellContent(DataGrid实例.Items[行号(从上往下，从0开始)]) as 对应单元格的控件名称).属性=值

## 想要在DataGrid里面获取隔行换色效果,:
```
<Setter Property="AlternationCount" Value="2"/> <!-- 想要实现隔行换色就得修改这个属性-->
