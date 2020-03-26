using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using Microsoft.Win32;
using NPOI.SS.UserModel;
using System.Globalization;

namespace PersonnelRecode
    {
    class CSavetoExcel : IDisposable
        {
        private bool _accomplish;
        /// <summary>
        /// 指示程序运行是否出错
        /// </summary>
        public bool Accomplish
            {
            get
                {
                return _accomplish;
                }
              set
                {
                _accomplish = value;
                }
            }

        CultureInfo cn = CultureInfo.GetCultureInfo("zh-chs");
        CDataoption querydo = new CDataoption();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        //string filename = "";
        string saveFileName = "";
        FileStream fs = null;
        IWorkbook workbook = null;
       // private int dataRowcount =0;
        DataTable Table = new DataTable();
        string species = "";
        string sheetname = "";

        string summarizinginformation = "";

        /// <summary>
        ///  将查询到的结果保存到Excel
        /// </summary>
        /// <param name="dt">源数据表</param>
        /// <param name="Species">查询的数据的类型</param>
        /// <param name="SheetName">表的名称</param>
        /// <param name="Summarizinginformation">汇总信息</param>
        public CSavetoExcel(DataTable dt,string Species,string Summarizinginformation)
            {
            summarizinginformation = Summarizinginformation;

               saveFileDialog.Filter = "Excel 文件|*.xlsx";
               saveFileDialog.FileName = "";
               saveFileDialog.ShowDialog();
               saveFileName = saveFileDialog.FileName;
              // filename = "T";
               if (saveFileName.IndexOf(":", StringComparison.OrdinalIgnoreCase) < 0) return;  //被点了取消

                   species = Species;
                   Table = dt;

                 if(Species == "出勤查询")
                {
                sheetname = "出勤";
                }
                 else if(Species == "进账查询")
                {
                sheetname = "进账";
                }
                 else if(Species=="通用出账查询")
                {
                sheetname = "出账";
                }
                 else if(Species == "自己吃饭汇总")
                {
                sheetname = "吃饭";
                }
                  

            DataTableToExcel(Table,sheetname, saveFileName);
            Table.Dispose();
           }
        ///将DataTable数据导入到excel中
        ///<param name="data">要导入的数据</param>
        ///<param name="sheetName">要导入的excel的sheet的名称</param>
        ///<param name="isColumnWritten">DataTable的列名是否要导入</param>
        ///<param name="filepath">导出的文件路径</param>
        ///<returns>导入数据行数(包含列名那一行)</returns>
       private void DataTableToExcel(DataTable data, string sheetName,  string filepath)
            {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;

            fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook();
         

            try
                {
                if (workbook != null)
                    {
                    sheet = workbook.CreateSheet(sheetName);
                   //表的保护密码
                    sheet.ProtectSheet("fxf5201314");
                    }
                else
                    {
                 
                    }
                /*
                 *  创建顶部表头与列头
                 */
                   IRow Headerrow = sheet.CreateRow(0);
                Headerrow.HeightInPoints = 30;
               ICell Headercell = Headerrow.CreateCell(0);

                //汇总行
                IRow Summarizingrow = sheet.CreateRow(1);
                      Summarizingrow.HeightInPoints = 20;
                ICell Summarizingcell = Summarizingrow.CreateCell(0);
                     

                //列头
                IRow ColumnHeaderrow = sheet.CreateRow(3);
                   ColumnHeaderrow.HeightInPoints = 22;
                if (species == "出勤查询")
                    {
                    Headercell.SetCellValue("出勤查询报表");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 10));
                    Headercell.CellStyle = GetTitleCellStyle(workbook);

                    Summarizingcell.SetCellValue(summarizinginformation);
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 10));
                    Summarizingcell.CellStyle = GetColumnheadercellstyle(workbook);


                    ICell attdatecell = ColumnHeaderrow.CreateCell(0);
                          attdatecell.SetCellValue("出勤日期");
                          attdatecell.CellStyle = GetColumnheadercellstyle(workbook);


                    ICell attstatuscell = ColumnHeaderrow.CreateCell(1);
                          attstatuscell.SetCellValue("出勤状态");
                          attstatuscell.CellStyle = GetColumnheadercellstyle(workbook);


                    ICell atthelpercell = ColumnHeaderrow.CreateCell(2);
                          atthelpercell.SetCellValue("帮忙对象");
                          atthelpercell.CellStyle = GetColumnheadercellstyle(workbook);
                   
                    ICell attworkcell = ColumnHeaderrow.CreateCell(3);
                          attworkcell.SetCellValue("加班时长");
                          attworkcell.CellStyle = GetColumnheadercellstyle(workbook);
                   
                    ICell attreacell= ColumnHeaderrow.CreateCell(4);
                          attreacell.SetCellValue("出勤备注");
                          attreacell.CellStyle = GetColumnheadercellstyle(workbook); ;
                    
                    sheet.SetColumnWidth(0, 16 * 256);
                    sheet.SetColumnWidth(1, 12 * 256);
                    sheet.SetColumnWidth(2, 12* 256);
                    sheet.SetColumnWidth(3, 12 * 256);
                    sheet.SetColumnWidth(4, 25 * 256);


                    }
                else if (species == "进账查询")
                    {
                    Headercell.SetCellValue("进账查询报表");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 7));
                    Headercell.CellStyle = GetTitleCellStyle(workbook);

                    Summarizingcell.SetCellValue(summarizinginformation);
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 7));
                    Summarizingcell.CellStyle = GetColumnheadercellstyle(workbook);


                    ICell indatecell=ColumnHeaderrow.CreateCell(0);
                          indatecell.SetCellValue("进账日期");                  
                          indatecell .CellStyle= GetColumnheadercellstyle(workbook);
                    
                    ICell inamountcell= ColumnHeaderrow.CreateCell(1);
                          inamountcell.SetCellValue("进账金额");
                          inamountcell.CellStyle = GetColumnheadercellstyle(workbook);

                    ICell insourcecell= ColumnHeaderrow.CreateCell(2);
                          insourcecell.SetCellValue("进账来源");
                          insourcecell.CellStyle = GetColumnheadercellstyle(workbook);

                    ICell intypecell= ColumnHeaderrow.CreateCell(3);
                          intypecell.SetCellValue("进账类型");
                          intypecell.CellStyle = GetColumnheadercellstyle(workbook);

                    ICell inremcell =  ColumnHeaderrow.CreateCell(4);
                          inremcell.SetCellValue("进账备注");
                          inremcell.CellStyle = GetColumnheadercellstyle(workbook);

                      sheet.SetColumnWidth(0, 16 * 256);
                      sheet.SetColumnWidth(1, 12 * 256);
                      sheet.SetColumnWidth(2, 12 * 256);
                      sheet.SetColumnWidth(3, 12 * 256);
                      sheet.SetColumnWidth(4, 25 * 256);


                    }
                else if (species == "通用出账查询")
                    {
                    Headercell.SetCellValue("通用出账查询报表");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 5));
                    Headercell.CellStyle = GetTitleCellStyle(workbook);

                    Summarizingcell.SetCellValue(summarizinginformation);
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 5));
                    Summarizingcell.CellStyle = GetColumnheadercellstyle(workbook);

                    ICell expdatecell= ColumnHeaderrow.CreateCell(0);
                        expdatecell.SetCellValue("支出日期");
                        expdatecell.CellStyle = GetColumnheadercellstyle(workbook);

                  ICell expamountcell= ColumnHeaderrow.CreateCell(1);
                        expamountcell.SetCellValue("支出金额");
                        expamountcell.CellStyle = GetColumnheadercellstyle(workbook);
                      
                  ICell exptypecell= ColumnHeaderrow.CreateCell(2);
                        exptypecell.SetCellValue("支出类型");
                        exptypecell.CellStyle = GetColumnheadercellstyle(workbook);


                  ICell exprcell = ColumnHeaderrow.CreateCell(3);
                        exprcell.SetCellValue("支出备注");
                        exprcell.CellStyle = GetColumnheadercellstyle(workbook);



                    sheet.SetColumnWidth(0, 16 * 256);
                    sheet.SetColumnWidth(1, 12 * 256);
                    sheet.SetColumnWidth(2, 12 * 256);
                    sheet.SetColumnWidth(3, 25 * 256);
                    

                    }
                else if (species == "自己吃饭汇总")
                    {
                       Headercell.SetCellValue("自己吃饭汇总报表");
                       sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 8));
                       Headercell.CellStyle = GetTitleCellStyle(workbook);

                    Summarizingcell.SetCellValue(summarizinginformation);
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 8));
                    Summarizingcell.CellStyle = GetColumnheadercellstyle(workbook);

                    ICell expdatecell= ColumnHeaderrow.CreateCell(0);
                             expdatecell.SetCellValue("支出日期");
                             expdatecell.CellStyle = GetColumnheadercellstyle(workbook);
                        
                      ICell expamount= ColumnHeaderrow.CreateCell(1);
                            expamount.SetCellValue("支出金额(元)");
                            expamount.CellStyle = GetColumnheadercellstyle(workbook);

                       sheet.SetColumnWidth(0, 16 * 256);
                       sheet.SetColumnWidth(1, 14 * 256);

                    }

                count = 4;


                for (i = 0; i < data.Rows.Count; ++i)
                    {
                    IRow row = sheet.CreateRow(count);
                    row.HeightInPoints = 18;
                    for (j = 0; j < data.Columns.Count; ++j)
                        {
                        string typ = data.Rows[i][j].GetType().ToString();
                        if (data.Rows[i][j].GetType().ToString() == "System.DateTime")
                            {
                             ICell datecell= row.CreateCell(j);
                          
                             datecell.SetCellValue(Convert.ToDateTime(data.Rows[i][j],cn.DateTimeFormat));
                              datecell.CellStyle = Datecellstyle(workbook);
                            }
                       
                        else if (data.Rows[i][j].GetType().ToString() == "System.Int32")
                            {
                               ICell intcell= row.CreateCell(j);
                                     intcell.SetCellValue(Convert.ToInt32(data.Rows[i][j], cn.NumberFormat));
                                     intcell.CellStyle = Generalcellstyle(workbook);
                           
                            } 
                        else if (data.Rows[i][j].GetType().ToString() == "System.Double")
                            {
                            
                           ICell doublecell= row.CreateCell(j);
                                 doublecell.SetCellValue(Convert.ToDouble(data.Rows[i][j], cn.NumberFormat));
                                 doublecell.CellStyle = doublecellstyle(workbook);

                            }
                         else if(data.Rows[i][j].GetType().ToString() == "System.Decimal")
                            {
                               ICell moneycell = row.CreateCell(j);
                               moneycell.SetCellValue(Convert.ToDouble(data.Rows[i][j], cn.NumberFormat));                             moneycell.CellStyle = Moneycellstyle(workbook);
                            }

                        else if(data.Rows[i][j].GetType().ToString() == "System.String")
                            {
                            ICell strcell=    row.CreateCell(j);
                                  strcell.SetCellValue(data.Rows[i][j].ToString());
                                  strcell.CellStyle = Generalcellstyle(workbook);
                            }
                        }
                    ++count;

                    }

              
                workbook.Write(fs); //写入到excel
                fs.Dispose();
                // workbook.Dispose();
               
                _accomplish = true;
                Table.Dispose();

                }
            catch (Exception ex)
                {
                querydo.ShowExceptionMessage(ex);
                _accomplish = false;
                querydo.Dispose();
                }


            }

        /// <summary>
        /// 表头的样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        static ICellStyle GetTitleCellStyle(IWorkbook workbook)
            {
                XSSFCellStyle cell1Style = (XSSFCellStyle)workbook.CreateCellStyle();
                cell1Style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
              
                 //设置背景色
                  XSSFColor forcolor = new XSSFColor();   //
                  byte[] forrgb = { (byte)135, (byte)206, (byte)235 };
                  forcolor.SetRgb(forrgb);
                  cell1Style.FillForegroundColorColor = forcolor;
                  cell1Style.FillPattern = FillPattern.SolidForeground;
            //设置背景色

                     //设置字体
                    IFont font = workbook.CreateFont();
                      font.FontName = "楷体";
                    font.FontHeightInPoints = 25;
                    cell1Style.SetFont(font);
               
            cell1Style.IsLocked = true;
                 return cell1Style;
            }
        /// <summary>
        /// 列头及汇总的样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private static ICellStyle GetColumnheadercellstyle(IWorkbook workbook)
            {
                  XSSFCellStyle cell1Style = (XSSFCellStyle)workbook.CreateCellStyle();
           
                   cell1Style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            XSSFColor forcolor = new XSSFColor();
            byte[] forrgb = { (byte)124, (byte)252, (byte)0 };
            forcolor.SetRgb(forrgb);
            cell1Style.FillForegroundColorColor = forcolor;
            cell1Style.FillPattern = FillPattern.SolidForeground;

            IFont font = workbook.CreateFont();
            font.FontName = "华文楷体";
            cell1Style.IsLocked = true;

            font.FontHeightInPoints = 16;
            cell1Style.SetFont(font);
            
            return cell1Style;
            }
        /// <summary>
        /// 货币数据的样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        static ICellStyle Moneycellstyle(IWorkbook workbook)
            {
                XSSFCellStyle cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                IDataFormat dataFormat = workbook.CreateDataFormat();
                cellStyle.SetDataFormat(dataFormat.GetFormat("¥#,##0"));
               cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                cellStyle.IsLocked = true;
                 return cellStyle;
            }
       /// <summary>
       /// 普通的样式
       /// </summary>
       /// <param name="workbook"></param>
       /// <returns></returns>
        static ICellStyle Generalcellstyle(IWorkbook workbook)
            {
               XSSFCellStyle cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
               cellStyle.Alignment = HorizontalAlignment.Right;
               cellStyle.IsLocked = true;
                 return cellStyle;
            }

        /// <summary>
        /// 日期数据样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        static ICellStyle Datecellstyle(IWorkbook workbook)
            {
               XSSFCellStyle cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                cellStyle.Alignment = HorizontalAlignment.Center;  //水平方向对齐方式
                IDataFormat dataFormat = workbook.CreateDataFormat();
                cellStyle.SetDataFormat(dataFormat.GetFormat("yyyy年MM月dd日"));  //显示格式
                cellStyle.IsLocked = true;  //单元格锁定
               return cellStyle;
            
            }

        /// <summary>
        /// 小数样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        static ICellStyle doublecellstyle(IWorkbook workbook)
            {
               XSSFCellStyle cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
               cellStyle.Alignment = HorizontalAlignment.Center;
               IDataFormat dataFormat = workbook.CreateDataFormat();
                cellStyle.SetDataFormat(dataFormat.GetFormat("0.0"));
                cellStyle.IsLocked = true;  //设置单元格锁定,就会形成只读的效果
            return cellStyle;

            }
        
        
        public void Dispose()
            {
            throw new NotImplementedException();
            }
        }
    }
