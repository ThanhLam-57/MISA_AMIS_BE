using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Common.Entities
{
    public class ResponseData
    {
        /// <summary>
        /// Tráng thái kết quả trả về
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public object? Data { get; set; }

        public ResponseData(bool success, object? data)
        {
            Success = success;
            Data = data;
        }
    }
}
