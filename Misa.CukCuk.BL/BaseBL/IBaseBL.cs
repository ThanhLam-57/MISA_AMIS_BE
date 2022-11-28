using MISA.AMIS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.BL.BaseBL
{
    public interface IBaseBL<T>
    {
        /// <summary>
        /// Lấy danh sách tất cả bảng ghi
        /// </summary>
        /// <returns>Danh sách tất cả bảng ghi</returns>
        /// CreatedBy:NTLAM(12/11/2022)
        public IEnumerable<T> GetAllRecords();

        /// <summary>
        /// Lấy thông tin bản ghi theo ID
        /// </summary>
        /// <param name="recordByID">ID của bảng ghi muốn lấy</param>
        /// <returns>Thông tin của bảng ghi</returns>
        /// CreatedBy:NTLAM(12/11/2022)
        public T GetRecordByID(Guid recordID);

        /// <summary>
        /// Sửa thông tin bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns>ID của bản ghi cần sửa</returns>
        /// CreatedBy:NTLAM(12/11/2022)
        public int UpdateRecord(Guid recordByID, T record);

        /// <summary>
        /// Xoá thông tin bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi cần xoá</param>
        /// <returns></returns>
        /// CreatedBy:NTLAM(12/11/2022)
        public Guid DeleteRecord(Guid recordID);

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="record">Bản ghi mới mới</param>
        /// <returns>Bản ghi mới mới</returns>
        /// CreatedBy:NTLAM(12/11/2022)
        public ResponseData InsertRecord(T record);
        /// <summary>
        /// Validate ngày không vượt quá ngày hiện tại
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        /// CreatedBy:NTLAM(22/11/2022)
        public ResponseData UpdateOrInsert(Guid recordID, T record);
    }
}
