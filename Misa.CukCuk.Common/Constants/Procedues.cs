using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Common.Constants
{
    public class Procedues
    {

        /// <summary>
        /// Format tên của Procedues lấy tất cả bản ghi
        /// </summary>
        public static string GET_ALL = "Proc_{0}_GetAll";

        /// <summary>
        /// Format tên của Procedues lấy thông tin bản ghi theo ID
        /// </summary>
        public static string GET_BY_ID = "Proc_{0}_GetByID";

        /// <summary>
        /// Format tên của Procedues xoá thông tin bản ghi theo ID
        /// </summary>
        public static string DELETE_BY_ID = "Proc_{0}_DeleteByID";

        /// <summary>
        /// Format tên của Procedues lấy mã mới cho bản ghi
        /// </summary>
        public static string GET_MAX_RECORD_CODE = "Proc_{0}_GetMaxCode";

        /// <summary>
        /// Format tên của Procedues thêm mới một bản ghi
        /// </summary>
        public static string INSERT_RECORD = "Proc_{0}_InsertOne";

        /// <summary>
        /// Format tên của Procedues sửa một bản ghi
        /// </summary>
        public static string UPDATE_RECORD = "Proc_{0}_Update";
    }
}
