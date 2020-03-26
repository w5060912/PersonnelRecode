using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelRecode
    {
    class  AttendanceModuel
        {
        /// <summary>
        /// 出勤日期
        /// </summary>
        
    
        public string Date
            {
            get;
            set;
            } 
        /// <summary>
        /// 出勤状态
        /// </summary>
        public string Attendancestatus
            {
            get;
            set;
            }
        /// <summary>
        /// 加班时长
        /// </summary>
         public double Workovertime
            {
            get;
            set;

            }
        /// <summary>
        /// 帮忙对象
        /// </summary>
          public string Helper
            {
            get;
            set;
            }
        /// <summary>
        /// 出勤备注
        /// </summary>
        public string AttendanceRemark
            {
            get;
            set;
            }
       
        
        }
    }
