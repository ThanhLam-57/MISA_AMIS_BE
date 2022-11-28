using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Common.Enums
{
    /// <summary>
    /// Enum mô tả chi tiết các lỗi khi gọi api
    /// </summary>
    public enum AmisErrorCode
    {
        /// <summary>
        /// Lỗi gặp exception
        /// </summary>
        Exception = 1,

        /// <summary>
        /// Lỗi trùng mã
        /// </summary>
        DuplicateCode = 2,

        /// <summary>
        /// Lỗi dữ liệu đầu vào không hợp lệ
        /// </summary>
        InvalidData=3
    }
}
