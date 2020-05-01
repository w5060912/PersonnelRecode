using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Configuration;

namespace PersonnelRecode
    {
    internal class CDataoption : IDataoption
        {
        private string Procname ;
        private SqlCommand getcmd ;
        private SqlDataAdapter getda;
        private DataTable gettb ;


        public CDataoption()
            {
               Procname = "";
               getcmd = new SqlCommand();
               getda = new SqlDataAdapter();
               gettb = new DataTable();
              _NomalAttendancedays = 0;
              _HalfdayAttendancedays = 0;
              _Workovertime = 0;
               _IncomeAmountCount = 0;
              _ExpendAmountCount = 0;
               _timecount = 0;
               judge = "";
          }
        
        
        
        
        
        /// <summary>
        /// 链接到数据库的方法
        /// </summary>
        private static SqlConnection LinkToPersonInwork()
            {
            string sqlconstr = ConfigurationManager.ConnectionStrings["conToPersonnelInwork"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(sqlconstr);
            return sqlcon;
            }

        #region 属性区

        /// <summary>
        /// 统计指定日期区间内的正常出勤天数
        /// </summary>
        private int _NomalAttendancedays;

        /// <summary>
        /// 统计指定日期区间内的正常出勤天数
        /// </summary>
        public int NomalAttendancedays
            {
            get
                {
                return _NomalAttendancedays;
                }
            }

        /// <summary>
        /// 统计指定日期区间内的半天出勤天数
        /// </summary>
        private int _HalfdayAttendancedays;

        /// <summary>
        /// 统计指定日期区间内的半天出勤天数
        /// </summary>
        public int HalfdayAttendancedays
            {
            get
                {
                return _HalfdayAttendancedays;
                }
            }

        /// <summary>
        /// 统计指定日期区间内加班时长
        /// </summary>
        private double _Workovertime;

        /// <summary>
        /// 统计指定日期区间内加班时长
        /// </summary>
        public double Workovertime
            {
            get
                {
                return _Workovertime;
                }
            }

        /// <summary>
        /// 统计指定日期区间内进账总额
        /// </summary>
        private double _IncomeAmountCount;

        /// <summary>
        /// 统计指定日期区间内进账总额
        /// </summary>
        public double IncomeAmountCount
            {
            get
                {
                return _IncomeAmountCount;
                }
            }

        /// <summary>
        /// 统计与金额相关的次数
        /// </summary>
        private int _timecount;
       
        /// <summary>
        /// 统计与金额相关的次数
        /// </summary>
        public int Timecount
            {
            get
                {
                return _timecount;
                }
            }

        /// <summary>
        /// 统计指定日期区间内出账总额
        /// </summary>
        private Double _ExpendAmountCount;

        /// <summary>
        /// 统计指定日期区间内出账总额
        /// </summary>
        public double ExpendAmountCount
            {
               get
                {
                return _ExpendAmountCount;
                }
               
            }


        /// <summary>
        /// 用于判断从数据库接收的数据是否为空
        /// </summary>
        private string judge = "";

        #endregion 属性区

        
        /// <summary>
        /// 防呆方法,返回True则代表数据库里面没有输入当天的出勤信息，反之就是已经输入过当天的出勤信息
        /// </summary>
        /// <param name="AttendanceDate">出勤日期</param>
        /// <returns></returns>
        public bool Foolproofdesign(string AttendanceDate)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    Procname = "Usp_foolproof";
                    getcmd = new SqlCommand(Procname, con)
                        {
                        CommandType = CommandType.StoredProcedure
                        };
                    SqlParameter[] foolvalue =
                        {
                             new SqlParameter("@AttendanceDate",SqlDbType.Date),
                             new SqlParameter("@Judge",SqlDbType.Int)
                        };
                    foolvalue[0].Direction = ParameterDirection.Input;
                    foolvalue[0].Value = AttendanceDate;
                    foolvalue[1].Direction = ParameterDirection.Output;

                    getcmd.Parameters.AddRange(foolvalue);
                    getcmd.ExecuteNonQuery();
                    getcmd.Dispose();
                    }
                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                }

            judge = "";
            judge = getcmd.Parameters[1].Value.ToString();
            if (judge != "0")
                {
                return false;
                }
            else
                {
                return true;
                }
          
            
            }
        
         #region  欢迎页面用到的
        /// <summary>
        /// 欢迎页面用到的进账查询方法
        /// </summary>
        /// <param name="Startdate"></param>
        /// <param name="Enddate"></param>
        /// <returns></returns>
        public DataTable GetIncomeCount(string Startdate, string Enddate)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    Procname = "USP_GetIncomeCount";
                    getcmd = new SqlCommand(Procname, con)
                        {
                        CommandType = CommandType.StoredProcedure
                        };
                    SqlParameter[] SendValue =
                      {
                              new SqlParameter ("@startdate",SqlDbType.Date),
                                  new SqlParameter ("@enddate",SqlDbType.Date)

                             };
                    SendValue[0].Direction = ParameterDirection.Input;
                    SendValue[0].Value = Startdate;
                    SendValue[1].Direction = ParameterDirection.Input;
                    SendValue[1].Value = Enddate;

                    getcmd.Parameters.AddRange(SendValue);
                    getcmd.ExecuteNonQuery();
                    getda = new SqlDataAdapter(getcmd);
                    gettb = new DataTable();
                    getda.Fill(gettb);

                    }

                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                }

            getcmd.Dispose();
            getda.Dispose();
            gettb.Dispose();
            return gettb;
            }
        #endregion

        #region 记录页面获取数据的

        /// <summary>
        /// 获取支出种类
        /// </summary>
        /// <returns></returns>
        public DataTable GetExpendtype()
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    getcmd = new SqlCommand();
                    Procname = "Usp_GetExpendType";
                    getcmd = new SqlCommand(Procname, con)
                        {
                        CommandType = CommandType.StoredProcedure
                        };
                    getcmd.ExecuteNonQuery();
                    getda = new SqlDataAdapter(getcmd);
                    gettb = new DataTable();
                    getda.Fill(gettb);


                    }
                }
            catch(SqlException sqlex)
                {
                ShowExceptionMessage(sqlex);
                }
            
            getcmd.Dispose();
            getda.Dispose();
            gettb.Dispose();
            return gettb;
            }

        /// <summary>
        /// 获取出勤状态
        /// </summary>
        /// <returns></returns>
        public DataTable GetAttendanceStatus()
            {
            //Procname = "GetAttendanceStatus";
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    getcmd = new SqlCommand();
                    Procname = "Usp_GetAttendanceStatus";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    getcmd.ExecuteNonQuery();
                    getda = new SqlDataAdapter(getcmd);
                    gettb = new DataTable();
                    getda.Fill(gettb);
                    }
                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                }
            //getcmd.Dispose();
            //getda.Dispose();
            //gettb.Dispose();
            return gettb;
            }

        /// <summary>
        /// 获取进账类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetIncometype()
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    getcmd = new SqlCommand();
                    Procname = "Usp_GetIncomeType";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;

                    getcmd.ExecuteNonQuery();
                    getda = new SqlDataAdapter(getcmd);
                    gettb = new DataTable();
                    getda.Fill(gettb);
                    }
                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                }
            //getcmd.Dispose();
            //getda.Dispose();
            //gettb.Dispose();
            return gettb;
            }

        #endregion 记录页面获取数据的

        #region 插入数据到数据库里面的操作

        /// <summary>
        /// 将出勤数据插入到数据库出勤记录表
        /// </summary>
        /// <param name="AttendanceDate">出勤日期</param>
        /// <param name="Helper">帮忙对象</param>
        /// <param name="AttendanceStatus">出勤状态</param>
        /// <param name="Workovertime">加班时长</param>
        /// <param name="AttendanceRemark">出勤备注</param>
        public void InsertAttendanceDatatoDATABASE(string AttendanceDate, string Helper, string AttendanceStatus, double Workovertime, string AttendanceRemark)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    getcmd = new SqlCommand();
                    Procname = "Usp_IARTART";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] SendValue =
                     {
                    new SqlParameter ("@AttendanceDate",SqlDbType.Date),
                    new SqlParameter ("@AttendanceStatus",SqlDbType.NVarChar),
                    new SqlParameter ("@Help",SqlDbType.NVarChar),
                    new SqlParameter ("@Workovertime",SqlDbType.Float),
                    new SqlParameter ("@AttendanceRemark",SqlDbType.NVarChar)
                   };

                    SendValue[0].Direction = ParameterDirection.Input;
                    SendValue[0].Value = AttendanceDate;
                    SendValue[1].Direction = ParameterDirection.Input;
                    SendValue[1].Value = AttendanceStatus;
                    SendValue[2].Direction = ParameterDirection.Input;
                    SendValue[2].Value = Helper;
                    SendValue[3].Direction = ParameterDirection.Input;
                    SendValue[3].Value = Workovertime;
                    SendValue[4].Direction = ParameterDirection.Input;
                    SendValue[4].Value = AttendanceRemark;
                    getcmd.Parameters.AddRange(SendValue);
                    getcmd.ExecuteNonQuery();
                    }
                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                }

            getcmd.Dispose();
            }

        /// <summary>
        /// 将支出信息记录到支出记录表里面
        /// </summary>
        /// <param name="Expenddate">支出日期</param>
        /// <param name="Expendtype">支出类型</param>
        /// <param name="ExpendAmount">支出金额</param>
        /// <param name="ExpendRemark">支出备注</param>
        public void InsertExpendDataToDATABASE(string Expenddate, string Expendtype, double ExpendAmount, string ExpendRemark)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    getcmd = new SqlCommand();
                    Procname = "Usp_IERIER";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] SendValue =
                     {
                    new SqlParameter ("@ExpendDate",SqlDbType.Date),
                    new SqlParameter ("@ExpendAmount",SqlDbType.Money),
                    new SqlParameter ("@ExpendType",SqlDbType.NVarChar),
                    new SqlParameter ("@ExpendRemark",SqlDbType.NVarChar)
                   };

                    SendValue[0].Direction = ParameterDirection.Input;
                    SendValue[0].Value = Expenddate;
                    SendValue[1].Direction = ParameterDirection.Input;
                    SendValue[1].Value = ExpendAmount;
                    SendValue[2].Direction = ParameterDirection.Input;
                    SendValue[2].Value = Expendtype;
                    SendValue[3].Direction = ParameterDirection.Input;
                    SendValue[3].Value = ExpendRemark;

                    getcmd.Parameters.AddRange(SendValue);
                    getcmd.ExecuteNonQuery();
                    }
                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                }

            getcmd.Dispose();
            }

        /// <summary>
        /// 将进账记录录入到数据库里面
        /// </summary>
        /// <param name="Incomedate">进账日期</param>
        /// <param name="Source">进账来源</param>
        /// <param name="IncomeAmount">进账金额</param>
        /// <param name="IncomeType">进账类型</param>
        /// <param name="IncomeRemark">进账备注</param>
        public void InsertIncomeDataToDATABASE(string Incomedate, string Source, double IncomeAmount, string IncomeType, string IncomeRemark)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    getcmd = new SqlCommand();
                    Procname = "Usp_IIIRTIRT";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] SendValue =
                     {
                    new SqlParameter ("@IncomeDate",SqlDbType.Date),
                    new SqlParameter ("@IncomeSource",SqlDbType.NVarChar),
                    new SqlParameter ("@IncomeAmount",SqlDbType.Money),
                    new SqlParameter ("@IncomeType",SqlDbType.NVarChar),
                    new SqlParameter ("@IncomeRemark",SqlDbType.NVarChar)
                   };

                    SendValue[0].Direction = ParameterDirection.Input;
                    SendValue[0].Value = Incomedate;
                    SendValue[1].Direction = ParameterDirection.Input;
                    SendValue[1].Value = Source;
                    SendValue[2].Direction = ParameterDirection.Input;
                    SendValue[2].Value = IncomeAmount;
                    SendValue[3].Direction = ParameterDirection.Input;
                    SendValue[3].Value = IncomeType;
                    SendValue[4].Direction = ParameterDirection.Input;
                    SendValue[4].Value = IncomeRemark;
                    getcmd.Parameters.AddRange(SendValue);
                    getcmd.ExecuteNonQuery();
                    }
                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                }

            getcmd.Dispose();
            }

        #endregion 插入数据到数据库里面的操作

        #region 防呆及卸载
        /// <summary>
        /// 错误处理的方法
        /// </summary>
        public void ShowExceptionMessage(Exception exmessage)
            {
            MessageBox.Show(exmessage.ToString());
            return;
            }

        public bool Foolproofdesign()
            {
            throw new NotImplementedException();
            }

      
        #endregion

        #region 查询页面的操作

        /// <summary>
        /// 获取指定日期区间内所有的出勤信息
        /// </summary>
        /// <param name="Startdate">开始日期</param>
        /// <param name="Enddate">结束日期</param>
        /// <returns></returns>
        public DataTable GetAllAttendanceinfo(string Startdate, string Enddate)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    Procname = "Usp_getallattendance";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] SendValue =
                    {
                              new SqlParameter ("@startdate",SqlDbType.Date),
                                  new SqlParameter ("@enddate",SqlDbType.Date),
                                  new SqlParameter ("@NomalDays",SqlDbType.Int),//返回参数
                                       new SqlParameter ("@HalfDays",SqlDbType.Int),//返回参数
                                        new SqlParameter ("@Workovertime",SqlDbType.Float)//返回参数

                           };
                          SendValue[0].Direction = ParameterDirection.Input;
                          SendValue[0].Value = Startdate;
                          SendValue[1].Direction = ParameterDirection.Input;
                          SendValue[1].Value = Enddate;
                          SendValue[2].Direction = ParameterDirection.Output;
                          SendValue[3].Direction = ParameterDirection.Output;
                          SendValue[4].Direction = ParameterDirection.Output;

                          getcmd.Parameters.AddRange(SendValue);
                          getcmd.ExecuteNonQuery();
                          getda = new SqlDataAdapter(getcmd);
                          gettb = new DataTable();
                          getda.Fill(gettb);

                         judge = "";
                          judge = getcmd.Parameters[2].Value.ToString();
                         if (!string.IsNullOrEmpty(judge))
                            {
                              _NomalAttendancedays = (int)getcmd.Parameters[2].Value;
                             }
                          else
                            {
                             _NomalAttendancedays = 0;
                            }

                           judge = "";
                            judge = getcmd.Parameters[3].Value.ToString();
                          if (!string.IsNullOrEmpty(judge))
                            {
                            _HalfdayAttendancedays = (int)getcmd.Parameters[3].Value;
                           }
                           else
                           {
                             _HalfdayAttendancedays = 0;
                            }

                           judge = "";
                           judge = getcmd.Parameters[4].Value.ToString();
                           if (!string.IsNullOrEmpty(judge))
                            {
                            _Workovertime = (double)getcmd.Parameters[4].Value;
                            }
                           else
                           {
                             _Workovertime = 0;
                           }


                    }
             }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                }

            getcmd.Dispose();
            getda.Dispose();
            gettb.Dispose();
            return gettb;
            }

      
        /// <summary>
        /// 获取一段日期区间内给某人帮忙的出勤信息
        /// </summary>
        /// <param name="Startdate">开始日期</param>
        /// <param name="Enddate">结束日期</param>
        /// <param name="helper">帮忙对象</param>
        /// <returns>返回形式为表格加变量</returns>
        public DataTable GetGiveapersontohelp(string Startdate, string Enddate, string helper)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    getcmd = new SqlCommand();
                    Procname = "Usp_QARFO";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] Getvalue =
                       {
                                new SqlParameter ("@Startdate",SqlDbType.Date),
                                  new SqlParameter ("@Enddate",SqlDbType.Date),
                                     new SqlParameter ("@Helper",SqlDbType.NVarChar),
                                      new SqlParameter ("@NomalDays",SqlDbType.Int),//返回参数
                                       new SqlParameter ("@HalfDays",SqlDbType.Int),//返回参数
                                        new SqlParameter ("@Workovertime",SqlDbType.Float)//返回参数
                             };
                    Getvalue[0].Direction = ParameterDirection.Input;
                    Getvalue[0].Value = Startdate;
                    Getvalue[1].Direction = ParameterDirection.Input;
                    Getvalue[1].Value = Enddate;
                    Getvalue[2].Direction = ParameterDirection.Input;
                    Getvalue[2].Value = helper;
                    Getvalue[3].Direction = ParameterDirection.Output;
                    Getvalue[4].Direction = ParameterDirection.Output;
                    Getvalue[5].Direction = ParameterDirection.Output;
                    getcmd.Parameters.AddRange(Getvalue);
                    getcmd.ExecuteNonQuery();
                    getda = new SqlDataAdapter(getcmd);
                    gettb = new DataTable();
                    getda.Fill(gettb);

                    //
                    judge = "";
                    judge = getcmd.Parameters[3].Value.ToString();
                    if (!string.IsNullOrEmpty(judge))
                        {
                        _NomalAttendancedays = (int)getcmd.Parameters[3].Value;
                        }
                    else
                        {
                        _NomalAttendancedays = 0;
                        }

                    judge = "";
                    judge = getcmd.Parameters[4].Value.ToString();
                    if (!string.IsNullOrEmpty(judge))
                        {
                        _HalfdayAttendancedays = (int)getcmd.Parameters[4].Value;
                        }
                    else
                        {
                        _HalfdayAttendancedays = 0;
                        }

                    judge = "";
                    judge = getcmd.Parameters[5].Value.ToString();
                    if (!string.IsNullOrEmpty(judge))
                        {
                        _Workovertime = (double)getcmd.Parameters[5].Value;
                        }
                    else
                        {
                        _Workovertime = 0;
                        }
                    }
                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                }

            return gettb;
            }

      
        /// <summary>
        /// 获取指定日期区间内未给人帮忙的出勤记录，返回形式是表格及变量
        /// </summary>
        /// <param name="Startdate">开始日期</param>
        /// <param name="Enddate">结束日期</param>
        /// <returns></returns>
        public DataTable GetNomalAttendance(string Startdate, string Enddate)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    getcmd = new SqlCommand();
                    Procname = "Usp_QFARTDHO";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] Getvalue =
                       {
                                new SqlParameter ("@Startdate",SqlDbType.Date),
                                  new SqlParameter ("@Enddate",SqlDbType.Date),
                                    new SqlParameter ("@NomalDays",SqlDbType.Int),//返回参数
                                      new SqlParameter ("@HalfDays",SqlDbType.Int),//返回参数
                                        new SqlParameter ("@Workovertime",SqlDbType.Float)//返回参数
                             };
                    Getvalue[0].Direction = ParameterDirection.Input;
                    Getvalue[0].Value = Startdate;
                    Getvalue[1].Direction = ParameterDirection.Input;
                    Getvalue[1].Value = Enddate;
                    Getvalue[2].Direction = ParameterDirection.Output;
                    Getvalue[3].Direction = ParameterDirection.Output;
                    Getvalue[4].Direction = ParameterDirection.Output;
                    getcmd.Parameters.AddRange(Getvalue);
                    getcmd.ExecuteNonQuery();
                    getda = new SqlDataAdapter(getcmd);
                    gettb = new DataTable();
                    getda.Fill(gettb);

                    //
                    judge = "";
                    judge = getcmd.Parameters[2].Value.ToString();
                    if (!string.IsNullOrEmpty(judge))
                        {
                        _NomalAttendancedays = (int)getcmd.Parameters[2].Value;
                        }
                    else
                        {
                        _NomalAttendancedays = 0;
                        }

                    judge = "";
                    judge = getcmd.Parameters[3].Value.ToString();
                    if (!string.IsNullOrEmpty(judge))
                        {
                        _HalfdayAttendancedays = (int)getcmd.Parameters[3].Value;
                        }
                    else
                        {
                        _HalfdayAttendancedays = 0;
                        }

                    judge = "";
                    judge = getcmd.Parameters[4].Value.ToString();
                    if (!string.IsNullOrEmpty(judge))
                        {
                        _Workovertime = (double)getcmd.Parameters[4].Value;
                        }
                    else
                        {
                        _Workovertime = 0;
                        }
                    }
                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                }

            return gettb;
            }


        /// <summary>
        /// 获取指定日期区间内所有的进账信息
        /// </summary>
        /// <param name="Startdate">开始日期</param>
        /// <param name="Enddate">结束日期</param>
        /// <returns></returns>
        public DataTable GetAllIncomeinfo(string Startdate, string Enddate)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    Procname = "Usp_getallincomeinfo";
                    getcmd = new SqlCommand(Procname, con)
                        {
                        CommandType = CommandType.StoredProcedure
                        };
                    SqlParameter[] SendValue =
                      {
                              new SqlParameter ("@startdate",SqlDbType.Date),
                                  new SqlParameter ("@enddate",SqlDbType.Date)

                             };
                    SendValue[0].Direction = ParameterDirection.Input;
                    SendValue[0].Value = Startdate;
                    SendValue[1].Direction = ParameterDirection.Input;
                    SendValue[1].Value = Enddate;
                    
                    getcmd.Parameters.AddRange(SendValue);
                    getcmd.ExecuteNonQuery();
                    getda = new SqlDataAdapter(getcmd);
                    gettb = new DataTable();
                    getda.Fill(gettb);

                 }
                   
            }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                }

            getcmd.Dispose();
            getda.Dispose();
            gettb.Dispose();
            return gettb;
            }
       

        /// <summary>
        /// 查询一段日期区间内来源于某人的进账信息
        /// </summary>
        /// <param name="Startdate">开始日期</param>
        /// <param name="Enddate">结束日期</param>
        /// <param name="Source">来源</param>
        /// <returns></returns>
        public DataTable GetIncomeofsomeone(string Startdate, string Enddate, string Source)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    getcmd = new SqlCommand();
                    Procname = "Usp_SIIBS";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] Getvalue =
                       {
                                new SqlParameter ("@Startdate",SqlDbType.Date),
                                  new SqlParameter ("@Enddate",SqlDbType.Date),
                                    new SqlParameter ("@Source",SqlDbType.NVarChar),
                                      new SqlParameter ("@IncomeAmountCount",SqlDbType.Float),
                                      new SqlParameter("@Timecount",SqlDbType.Int)
                             };
                    Getvalue[0].Direction = ParameterDirection.Input;
                    Getvalue[0].Value = Startdate;
                    Getvalue[1].Direction = ParameterDirection.Input;
                    Getvalue[1].Value = Enddate;
                    Getvalue[2].Direction = ParameterDirection.Input;
                    Getvalue[2].Value = Source;
                    Getvalue[3].Direction = ParameterDirection.Output;
                    Getvalue[4].Direction = ParameterDirection.Output;
                    

                    getcmd.Parameters.AddRange(Getvalue);
                    getcmd.ExecuteNonQuery();
                    getda = new SqlDataAdapter(getcmd);
                    gettb = new DataTable();
                    getda.Fill(gettb);

                    //进账金额汇总
                    judge = "";
                    judge = getcmd.Parameters["@IncomeAmountCount"].Value.ToString();
                      
                    
                    if (!string.IsNullOrEmpty(judge))
                        {
                       
                        _IncomeAmountCount =(double)getcmd.Parameters["@IncomeAmountCount"].Value;
                        }
                    else
                        {
                        _IncomeAmountCount = 0;
                        }

                    //进账次数
                    judge = "";
                    judge = getcmd.Parameters["@Timecount"].Value.ToString();
                    if (!string.IsNullOrEmpty(judge))
                        {
                        _timecount = (int)getcmd.Parameters["@Timecount"].Value;
                        }
                    else
                        {
                        _timecount = 0;
                        }
                    }
                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                }

            return gettb;
            }

        /// <summary>
        /// 查询并返回指定日期区间内所有的支出信息，返回格式是表格
        /// </summary>
        /// <param name="Startdate">开始日期</param>
        /// <param name="Enddate">结束日期</param>
        /// <returns>表格及变量</returns>
        public DataTable GetAllExpendinfo(string Startdate, string Enddate)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    Procname = "Usp_getEIOSAE";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] SendValue =
                     {
                              new SqlParameter ("@startdate",SqlDbType.Date),
                                  new SqlParameter ("@enddate",SqlDbType.Date),
                                     new SqlParameter ("@Allexpend",SqlDbType.Float)
                         };
                    SendValue[0].Direction = ParameterDirection.Input;
                    SendValue[0].Value = Startdate;
                    SendValue[1].Direction = ParameterDirection.Input;
                    SendValue[1].Value = Enddate;
                    SendValue[2].Direction = ParameterDirection.Output;
                    getcmd.Parameters.AddRange(SendValue);
                    getcmd.ExecuteNonQuery();
                    getda = new SqlDataAdapter(getcmd);
                    gettb = new DataTable();
                    getda.Fill(gettb);

                    judge = "";

                    //防止因从数据库接收的数据为空而导致程序运行错误
                    judge = getcmd.Parameters[2].Value.ToString();
                    if (!string.IsNullOrEmpty(judge))
                        {
                        _ExpendAmountCount = (double)getcmd.Parameters[2].Value;
                        }
                    else
                        {
                        _ExpendAmountCount = 0;
                        }
                    }
                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                }

            getcmd.Dispose();
            getda.Dispose();
            gettb.Dispose();
            return gettb;
            }

        /*
         * 
         * 出账查询主要包含查询可报销的，与自己吃饭的
         * 自己吃饭的涉及到是否需要以一天为单位形成汇总
         * 
         */

        /// <summary>
        ///按种类查询查询支出信息，查询详细的可报销与自己吃饭都靠这个
        /// </summary>
        /// <param name="Startdate">开始日期</param>
        /// <param name="Enddate">结束日期</param>
        /// <param name="Expendtype">支出种类</param>
        /// <remarks>查询详细的可报销与自己吃饭都靠这个</remarks>
        /// <returns></returns>
        public DataTable GetExpendInfo(string Startdate, string Enddate, string Expendtype)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    Procname = "Usp_QEIBET";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] SendValue =
                     {
                              new SqlParameter ("@StartDate",SqlDbType.Date),
                                  new SqlParameter ("@Enddate",SqlDbType.Date),
                                   new SqlParameter ("@Expendtype",SqlDbType.NVarChar),
                                     new SqlParameter ("@AllExpendAmount",SqlDbType.Float)
                         };
                    SendValue[0].Direction = ParameterDirection.Input;
                    SendValue[0].Value = Startdate;
                    SendValue[1].Direction = ParameterDirection.Input;
                    SendValue[1].Value = Enddate;
                    SendValue[2].Direction = ParameterDirection.Input;
                    SendValue[2].Value = Expendtype;
                    SendValue[3].Direction = ParameterDirection.Output;
                    getcmd.Parameters.AddRange(SendValue);
                    getcmd.ExecuteNonQuery();
                    getda = new SqlDataAdapter(getcmd);
                    gettb = new DataTable();
                    getda.Fill(gettb);

                    //防止因从数据库接收的数据为空而导致程序运行错误
                    judge = getcmd.Parameters[3].Value.ToString();
                    if (!string.IsNullOrEmpty(judge))
                        {
                        _ExpendAmountCount = (double)getcmd.Parameters[3].Value;
                        }
                    else
                        {
                        _ExpendAmountCount = 0;
                        }
                    }
                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                }

            getcmd.Dispose();
            getda.Dispose();
            gettb.Dispose();
            return gettb;
            }

     
        
        /// <summary>
        /// 查询自己吃饭的出账，单独出来是因为这个会以每日为单位汇总统计
        /// </summary>
        /// <param name="Startdate"></param>
        /// <param name="Enddate"></param>
        /// <returns></returns>
        public DataTable GetmyselfmealExpend(string Startdate, string Enddate)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    Procname = "Usp_QueryOneselfEatmealExpend";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] SendValue =
                     {
                              new SqlParameter ("@startdate",SqlDbType.Date),
                                  new SqlParameter ("@enddate",SqlDbType.Date),
                                   
                                     new SqlParameter ("@Amountsum",SqlDbType.Float),
                                     new SqlParameter ("@Statistics",SqlDbType.Int)
                         };
                    SendValue[0].Direction = ParameterDirection.Input;
                    SendValue[0].Value = Startdate;
                    SendValue[1].Direction = ParameterDirection.Input;
                    SendValue[1].Value = Enddate;
                    SendValue[2].Direction = ParameterDirection.Output;
                    SendValue[3].Direction = ParameterDirection.Output;
                    getcmd.Parameters.AddRange(SendValue);
                    getcmd.ExecuteNonQuery();
                    getda = new SqlDataAdapter(getcmd);
                    gettb = new DataTable();
                    getda.Fill(gettb);

                    judge = "";
                     //防止因从数据库接收的数据为空而导致程序运行错误
                    judge = getcmd.Parameters[2].Value.ToString();
                    if (!string.IsNullOrEmpty(judge))
                        {
                        _ExpendAmountCount = (double)getcmd.Parameters[2].Value;  //总金额
                        }
                    else
                        {
                        _ExpendAmountCount = 0;
                        }

                       judge = "";
                        judge = getcmd.Parameters[3].Value.ToString();  //天数
                      if(!string.IsNullOrEmpty(judge))
                          {
                        _timecount = (int)getcmd.Parameters[3].Value;
                          }
                         else
                         {
                        _timecount = 0;
                         }
                    
                    
                    
                 }
                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                }

            getcmd.Dispose();
            getda.Dispose();
            gettb.Dispose();
            return gettb;
            }

        #endregion

        #region 修改页面执行的操作
           #region 1. 将修改后记录更新到对应的数据库里面
        /// <summary>
        /// 将修改后的出勤记录插入到出勤记录表 
        /// </summary>
        /// <param name="Attdate">出勤日期</param>
        /// <param name="Columnname">将要操作的列名</param>
        /// <param name="Value">值</param>
        public int UpdateByAtt(string Attdate, string Columnname, string Value)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    getcmd = new SqlCommand();
                    Procname = "Usp_UpdateRecordByAtt";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] SendValue =
                     {
                    new SqlParameter ("@attdate",SqlDbType.Date),
                    new SqlParameter ("@columnname",SqlDbType.NVarChar),
                    new SqlParameter ("@value",SqlDbType.NVarChar)

                   };

                    SendValue[0].Direction = ParameterDirection.Input;
                    SendValue[0].Value = Attdate;
                    SendValue[1].Direction = ParameterDirection.Input;
                    SendValue[1].Value = Columnname;
                    SendValue[2].Direction = ParameterDirection.Input;
                    SendValue[2].Value = Value;

                    getcmd.Parameters.AddRange(SendValue);
                    getcmd.ExecuteNonQuery();
                    getcmd.Dispose();
                    return 1;
                    }
                }
            catch (SqlException ex)
                {
                //ShowExceptionMessage( ex.InnerException);
                MessageBox.Show("本错误的异常实例是:" + ex.InnerException);
                MessageBox.Show("本错误的存储过程是:" + ex.Procedure);
                MessageBox.Show("本错误发生在第:" + ex.LineNumber + " 行");
                MessageBox.Show("本错误的帮助文档连接是:" + ex.HelpLink);
                return 0;
                }




            }

        /// <summary>
        /// 将修改后的进账记录插入到进账记录表里面
        /// </summary>
        /// <param name="Incomedate">进账日期</param>
        /// <param name="Columnname">将要操作的列</param>
        /// <param name="Value">值</param>
        public int UpdateByIncome(int IncomeId, string Columnname, string Value)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    getcmd = new SqlCommand();
                    Procname = "Usp_UpdateRecordByIncome";
                    getcmd = new SqlCommand(Procname, con);

                    getcmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter[] SendValue =
                    {
                    new SqlParameter ("@incomeid",SqlDbType.Int),
                    new SqlParameter ("@columnname",SqlDbType.NVarChar),
                    new SqlParameter ("@value",SqlDbType.NVarChar)

                   };

                    SendValue[0].Direction = ParameterDirection.Input;
                    SendValue[0].Value = IncomeId;
                    SendValue[1].Direction = ParameterDirection.Input;
                    SendValue[1].Value = Columnname;
                    SendValue[2].Direction = ParameterDirection.Input;
                    SendValue[2].Value = Value;

                    getcmd.Parameters.AddRange(SendValue);
                    getcmd.ExecuteNonQuery();
                    getcmd.Dispose();
                    return 1;
                    }
                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);

                return 0;
                }



            }

        /// <summary>
        /// 将修改后的出账记录插入到出账记录表里面
        /// </summary>
        /// <param name="Expenddate">出账日期</param>
        /// <param name="Columnname">将要操作的列</param>
        /// <param name="Value">值</param>
        public int UpdateByExpend(int ExpendId, string Columnname, string Value)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    getcmd = new SqlCommand();
                    Procname = "Usp_UpdateRecordByExpend";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] SendValue =
                     {
                    new SqlParameter ("@expendid",SqlDbType.Int),
                    new SqlParameter ("@columnname",SqlDbType.NVarChar),
                    new SqlParameter ("@value",SqlDbType.NVarChar)

                   };

                    SendValue[0].Direction = ParameterDirection.Input;
                    SendValue[0].Value = ExpendId;
                    SendValue[1].Direction = ParameterDirection.Input;
                    SendValue[1].Value = Columnname;
                    SendValue[2].Direction = ParameterDirection.Input;
                    SendValue[2].Value = Value;

                    getcmd.Parameters.AddRange(SendValue);
                    getcmd.ExecuteNonQuery();
                    getcmd.Dispose();
                    return 1;
                    }
                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                return 0;
                }



            }

        #endregion

           #region 2.将修改记录插入到对应的表里面

        /// <summary>
        /// 将出勤修改记录保存到数据库
        /// </summary>
        /// <param name="Changedate">修改日期</param>
        /// <param name="attdate">用户指定的出勤记录中的出勤日期</param>
        /// <param name="Attcolumn">修改的出勤记录列名</param>
        /// <param name="Oldvalue">原值</param>
        /// <param name="Newvalue">修改后的值</param>
        /// <returns >返回0则代表执行失败，1则成功 </returns>

        public int SaveAttendanceModificationRecordToDatabase(string Changedate, string attdate, string Attcolumn, string Oldvalue, string Newvalue)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    getcmd = new SqlCommand();
                    Procname = "Usp_InsertintoAttendanceRevision";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] SendValue =
                     {
                    new SqlParameter ("@changedatetime",SqlDbType.DateTime),
                    new SqlParameter ("@attendancedate",SqlDbType.Date),
                    new SqlParameter ("@attendanceoptions",SqlDbType.NVarChar),
                    new SqlParameter ("@oldvalue",SqlDbType.NVarChar),
                    new SqlParameter ("@newvalue",SqlDbType.NVarChar)
                   };

                    SendValue[0].Direction = ParameterDirection.Input;
                    SendValue[0].Value = Changedate;
                    SendValue[1].Direction = ParameterDirection.Input;
                    SendValue[1].Value = attdate;
                    SendValue[2].Direction = ParameterDirection.Input;
                    SendValue[2].Value = Attcolumn;
                    SendValue[3].Direction = ParameterDirection.Input;
                    SendValue[3].Value = Oldvalue;
                    SendValue[4].Direction = ParameterDirection.Input;
                    SendValue[4].Value = Newvalue;
                    getcmd.Parameters.AddRange(SendValue);
                    getcmd.ExecuteNonQuery();
                    getcmd.Dispose();
                    return 1;
                    }
                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);

                return 0;
                }

            
            }

        /// <summary>
        /// 将进账修改记录保存到数据库
        /// </summary>
        /// <param name="Changedate">修改日期</param>
        /// <param name="Incomedate">进账记录的日期</param>
        /// <param name="Incomecolumn">进账记录的列名</param>
        /// <param name="Oldvalue">原值</param>
        /// <param name="Newvalue">新值</param>
        public int SaveIncomeModificationRecordToDatabase(string Changedate, int IncomeId, string Incomedate, string Incomecolumn, string Oldvalue, string Newvalue)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    getcmd = new SqlCommand();
                    Procname = "Usp_InsertintoIncomeRevision";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] SendValue =
                     {
                    new SqlParameter ("@changedatetime",SqlDbType.DateTime),
                    new SqlParameter ("@IncomeId",SqlDbType.Int),
                    new SqlParameter ("@incomedate",SqlDbType.Date),
                    new SqlParameter ("@incomeoptions",SqlDbType.NVarChar),
                    new SqlParameter ("@oldvalue",SqlDbType.NVarChar),
                    new SqlParameter ("@newvalue",SqlDbType.NVarChar)
                   };

                    SendValue[0].Direction = ParameterDirection.Input;
                    SendValue[0].Value = Changedate;
                    SendValue[1].Direction = ParameterDirection.Input;
                    SendValue[1].Value = IncomeId;
                    SendValue[2].Direction = ParameterDirection.Input;
                    SendValue[2].Value = Incomedate;
                    SendValue[3].Direction = ParameterDirection.Input;
                    SendValue[3].Value = Incomecolumn;
                    SendValue[4].Direction = ParameterDirection.Input;
                    SendValue[4].Value = Oldvalue;
                    SendValue[5].Direction = ParameterDirection.Input;
                    SendValue[5].Value = Newvalue;
                    getcmd.Parameters.AddRange(SendValue);
                    getcmd.ExecuteNonQuery();
                    getcmd.Dispose();
                    return 1;
                    }
                }
            catch (ArgumentException arex)
                {
                ShowExceptionMessage(arex);
                return 0;
                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                return 0;
                }

         
            }

        /// <summary>
        /// 将出账修改记录保存到数据库
        /// </summary>
        /// <param name="Changedate">修改出账的日期</param>
        /// <param name="ExpendId">出账记录表里面编号</param>
        /// <param name="Expenddate">原出账日期</param>
        /// <param name="Expendcolumn">将要修改的出账列</param>
        /// <param name="Oldvalue">原值</param>
        /// <param name="Newvalue">新值/param>
        public int SaveExpendModificationRecordToDatabase(string Changedate, int ExpendId, string Expenddate, string Expendcolumn, string Oldvalue, string Newvalue)
            {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    getcmd = new SqlCommand();
                    Procname = "Usp_InsertintoExpendRevision";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] SendValue =
                     {
                    new SqlParameter ("@changedatetime",SqlDbType.DateTime),
                    new SqlParameter ("@expendid",SqlDbType.Int),
                    new SqlParameter ("@expenddate",SqlDbType.Date),
                    new SqlParameter ("@expendoptions",SqlDbType.NVarChar),
                    new SqlParameter ("@oldvalue",SqlDbType.NVarChar),
                    new SqlParameter ("@newvalue",SqlDbType.NVarChar)
                   };

                    SendValue[0].Direction = ParameterDirection.Input;
                    SendValue[0].Value = Changedate;
                    SendValue[1].Direction = ParameterDirection.Input;
                    SendValue[1].Value = ExpendId;
                    SendValue[2].Direction = ParameterDirection.Input;
                    SendValue[2].Value = Expenddate;
                    SendValue[3].Direction = ParameterDirection.Input;
                    SendValue[3].Value = Expendcolumn;
                    SendValue[4].Direction = ParameterDirection.Input;
                    SendValue[4].Value = Oldvalue;
                    SendValue[5].Direction = ParameterDirection.Input;
                    SendValue[5].Value = Newvalue;

                    getcmd.Parameters.AddRange(SendValue);
                    getcmd.ExecuteNonQuery();
                    getcmd.Dispose();
                    return 1;
                    }
                }
            catch (ArgumentException arex)
                {
                ShowExceptionMessage(arex);
                return 0;
                }
            catch (SqlException ex)
                {
                ShowExceptionMessage(ex);
                return 0;
                }

         
            }





        #endregion

        #endregion

        #region 修改记录查看界面执行的操作
          #region 按修改日期查看
            /// <summary>
            ///  按修改日期查看出勤修改历史
            /// </summary>
            /// <param name="sDate">开始日期</param>
            /// <param name="eDate">结束日期</param>
            /// <returns></returns>
             public DataTable GetAttAlterHistoryByAlterDate(string sDate, string eDate)
              {
               throw new NotImplementedException();
              }
             
             /// <summary>
             /// 按修改日期查看进账修改历史 
             /// </summary>
             /// <param name="sDate">开始日期</param>
             /// <param name="eDate">结束日期</param>
             /// <returns></returns>
             public DataTable GetIncomeAlterHistoryByAlterDate(string sDate, string eDate)
              {
               throw new NotImplementedException();
              }
            
             /// <summary>
             /// 按修改日期查看出账修改历史
             /// </summary>
             /// <param name="sDate"></param>
             /// <param name="eDate"></param>
             /// <returns></returns>
             public DataTable GetExpendAlterHistoryByAlterDate(string sDate, string eDate)
              {
              throw new NotImplementedException();
              }

        #endregion
     
           //目前界面按照源记录日期来查询
           //查看指定日期内指定的记录的修改历史
        #region 按源记录日期查看
        /// <summary>
        /// 按源记录日期查看出勤修改历史
        /// </summary>
        /// <param name="sDate">开始日期</param>
        /// <param name="eDate">结束日期</param>
        /// <returns></returns>
        public DataTable GetAttAlterHistoryBySourceRecordDate(string sDate, string eDate)
              {
                try
                  {
                      Procname = "";
                     using (SqlConnection con =LinkToPersonInwork())
                     {
                    con.Open();
                    Procname = "Usp_GetAttAlterHistoryBySourceDate";
                        getcmd = new SqlCommand(Procname,con);
                         getcmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter[] sqlParameters =
                             {
                                 new SqlParameter("@startdate",SqlDbType.Date),
                                 new SqlParameter("@enddate",SqlDbType.Date)
                              };
                          sqlParameters[0].Direction = ParameterDirection.Input;
                          sqlParameters[0].Value = sDate;
                          sqlParameters[1].Direction = ParameterDirection.Input;
                          sqlParameters[1].Value = eDate;
                          getcmd.Parameters.AddRange(sqlParameters);
                           getcmd.ExecuteNonQuery();
                            getda = new SqlDataAdapter(getcmd);
                            gettb = new DataTable();
                            getda.Fill(gettb);

                    


                    }
                getcmd.Dispose();
                getda.Dispose();
              
                  }
               catch(SqlException sqlex)
                   {
                ShowExceptionMessage(sqlex);
                }

            return gettb;

            }
        /// <summary>
        /// 按原记录日期查看进账修改历史
        /// </summary>
        /// <param name="sDate">开始日期</param>
        /// <param name="eDate">结束日期</param>
        /// <returns></returns>
        public DataTable GetIncomeAlterHistoryBySourceRecordDate(string sDate, string eDate)
              {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    Procname = "Usp_GetIncomeAlterHistoryBySourceDate";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] sqlParameters =
                         {
                                 new SqlParameter("@startdate",SqlDbType.Date),
                                 new SqlParameter("@enddate",SqlDbType.Date)
                              };
                    sqlParameters[0].Direction = ParameterDirection.Input;
                    sqlParameters[0].Value = sDate;
                    sqlParameters[1].Direction = ParameterDirection.Input;
                    sqlParameters[1].Value = eDate;
                    getcmd.Parameters.AddRange(sqlParameters);
                    getcmd.ExecuteNonQuery();
                    getda = new SqlDataAdapter(getcmd);
                    gettb = new DataTable();
                    getda.Fill(gettb);

                   


                    }
                getcmd.Dispose();
                getda.Dispose();

                }
            catch (SqlException sqlex)
                {
                ShowExceptionMessage(sqlex);
                }

            return gettb;
            }
            
       
        /// <summary>
            /// 按原记录日期查看出账修改历史
            /// </summary>
            /// <param name="sDate">开始日期</param>
            /// <param name="eDate">结束日期</param>
            /// <returns></returns>
        public DataTable GetExpendAlterHistoryBySourceRecordDate(string sDate, string eDate)
              {
            try
                {
                Procname = "";
                using (SqlConnection con = LinkToPersonInwork())
                    {
                    con.Open();
                    Procname = "Usp_GetExpendAlterHistoryBySourceDate";
                    getcmd = new SqlCommand(Procname, con);
                    getcmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] sqlParameters =
                         {
                                 new SqlParameter("@startdate",SqlDbType.Date),
                                 new SqlParameter("@enddate",SqlDbType.Date)
                              };
                    sqlParameters[0].Direction = ParameterDirection.Input;
                    sqlParameters[0].Value = sDate;
                    sqlParameters[1].Direction = ParameterDirection.Input;
                    sqlParameters[1].Value = eDate;
                    getcmd.Parameters.AddRange(sqlParameters);
                    getcmd.ExecuteNonQuery();
                    getda = new SqlDataAdapter(getcmd);
                    gettb = new DataTable();
                    getda.Fill(gettb);


                    }
                getcmd.Dispose();
                getda.Dispose();

                }
            catch (SqlException sqlex)
                {
                ShowExceptionMessage(sqlex);
                }
             

            return gettb;
            }

        

        #endregion


        #endregion


        }
    }