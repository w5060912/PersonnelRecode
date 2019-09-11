using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace PersonnelRecode
{
    /// <summary>
    /// 用于与数据库交互的接口
    ///
    /// </summary>
    ///<see cref=""/>

    interface IDataoption
    {
        #region  将数据存储到数据库里面的操作
        /// <summary>
        /// 将出勤信息数据存储到数据库的操作
        /// </summary>
        /// <param name="AttendanceDate">出勤日期</param>
        /// <param name="Helper">帮忙对象</param>
        /// <param name="AttendanceStatus">出勤状态</param>
        /// <param name="Workovertime">加班时长</param>
        /// <param name="AttendanceRemark">出勤备注</param>
        void InsertAttendanceDatatoDATABASE
            (DateTime AttendanceDate,String Helper,string AttendanceStatus,
            double Workovertime,string AttendanceRemark);
        /// <summary>
        /// 出勤防呆
        /// </summary>
        /// <returns>如果存在当天的出勤记录就返回true，否则就返回false</returns>
        bool Foolproofdesign();
        /// <summary>
        /// 将进账信息数据存储到数据库
        /// </summary>
        /// <param name="Incomedate">进账日期</param>
        /// <param name="Source">进账来源</param>
        /// <param name="IncomeAmount">进账金额</param>
        /// <param name="IncomeType">进账类型</param>
        /// <param name="IncomeRemark">进账备注</param>
        void InsertIncomeDataToDATABASE(DateTime Incomedate,string Source,double IncomeAmount,
            string IncomeType,string IncomeRemark);
        /// <summary>
        /// 将出账信息数据存储到数据库的操作
        /// </summary>
        /// <param name="Expenddate">支出日期</param>
        /// <param name="Expendtype">支出类型</param>
        /// <param name="ExpendAmount">支出金额</param>
        /// <param name="ExpendRemark">支出备注</param>
        void InsertExpendDataToDATABASE(DateTime Expenddate,string Expendtype,
           double ExpendAmount,string ExpendRemark );
        #endregion


        #region 程序后进入记录页面或查询页面的时候(出勤状态/进账类型/出账类型)选项框里面显示的内容
    
        /// <summary>
        /// 获取出勤状态表里面的数据
        /// </summary>
        /// <returns>以数据表的形式返回结果</returns>
        DataTable GetAttendanceStatus();
        /// <summary>
        /// 获取进账类型表里面的数据的操作
        /// </summary>
        /// <returns>返回数据表类型的结果</returns>
        DataTable GetIncometype();
        /// <summary>
        /// 获取出账表类型表里面的数据的操作
        /// </summary>
        /// <returns>返回数据表类型的结果</returns>
        DataTable GetExpendtype();
        #endregion

        //查询分类:
        /*
         1.对出勤的查询:
           (1)查询给别人帮忙的出勤信息
           (2)查询未给人帮忙的出勤信息

         * 
         */
        /*
         2.对进账的查询
           (1)查询指定日期区间内来自指定人员的进账

         * 
         */

        /*
         3.对出账的查询
         *分类查询
         *全部查询
         */

        #region 查询操作
        /// <summary>
        /// 查询指定日期区间内给别人帮忙的出勤信息
        /// </summary>
        /// <returns>返回指定日期区间内给指定人员帮忙的出勤记录</returns>
        /// <param name="Startdate">开始日期</param>
        /// <param name="Enddate">结束日期</param>
        /// <param name="helper">帮忙对象姓名</param>
        /// 
        DataTable GetGiveapersontohelp(String Startdate,string Enddate,string helper);
        /// <summary>
        /// 查询未给人帮忙的出勤信息
        /// </summary>
        /// <returns>返回指定日期区间内未给人帮忙的出勤记录</returns>
        /// <param name="Startdate">开始日期</param>
        /// <param name="Enddate">结束日期</param>
        DataTable GetNomalAttendance(String Startdate, string Enddate);
        /// <summary>
        /// 查询来自指定人的进账信息
        /// </summary>
        /// <returns>返回指定日期区间内来自指定来源的进账记录</returns>
        /// <param name="Startdate">开始日期</param>
        /// <param name="Enddate">结束日期</param>
        /// <param name="Source">来源人</param>
        DataTable GetIncomeofsomeone(String Startdate, string Enddate,string Source);
        /// <summary>
        ///分类查询出账信息
        /// </summary>
        /// <returns>返回指定日期区间内指定支出类型的支出记录</returns>
        /// <param name="Startdate">开始日期</param>
        /// <param name="Enddate">结束日期</param>
        /// <param name="Expendtype">支出类型</param>
        DataTable GetExpendInfo(String Startdate, string Enddate,string Expendtype);
        /// <summary>
        /// 查询全部支出信息
        /// </summary>
        /// <returns>返回指定日期区间内所有的支出记录</returns>
        /// <param name="Startdate">开始日期</param>
        /// <param name="Enddate">结束日期</param>
        DataTable GetAllExpendinfo(String Startdate, string Enddate);

        //定义几个用于接收查询返回结果的属性
        /// <summary>
        /// 正常出勤的天数
        /// </summary>
        int NomalAttendancedays { set; get; }
        /// <summary>
        /// 半天出勤的天数
        /// </summary>
        int HalfdayAttendancedays { set; get; }
        /// <summary>
        /// 加班时长
        /// </summary>
        double Workovertime { set; get; }

        /// <summary>
        /// 进账金额统计
        /// </summary>
        double IncomeAmountCount { set; get; }
        /// <summary>
        /// 支出金额统计
        /// </summary>
        double ExpendAmountCount { set; get; }

       #endregion
    }
}
