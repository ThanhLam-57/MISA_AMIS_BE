using MISA.AMIS.Common.Entities;
using MISA.AMIS.DL.BaseDL;

namespace MISA.AMIS.DL 
{
    public interface IEmployeeDL : IBaseDL<Employee>
    {
        /// <summary>
        /// Lấy mã nhân viên
        /// </summary>
        /// <returns>Mã nhân viên</returns>
        /// CreatedBy:NTLAM(12/11/2022)
        public String getMaxEmployeeCode();

        /// <summary>
        /// API loc danh sach nhan vien co dieu kien tim kiem va phan trang
        /// </summary>
        /// <param name="keyword">tu khoas tim kiem</param>
        /// <param name="positionID">ID vi tri</param>
        /// <param name="departmentID">ID phong ban</param>
        /// <param name="limit">so ban ghi trong 1 trang</param>
        /// <param name="offset">vi tri ban ghi bat dau lay du lieu</param>
        /// <returns>Danh sach nhan vien</returns>   
        /// CreatedBy:NTLAM(12/11/2022)
        public object FilterEmployees(string? keyword, Guid? positionID, Guid? departmentID, int pageSize = 15, int pageIndex = 1);

        /// <summary>
        /// Xoá danh sách nhân viên theo ID
        /// </summary>
        /// <param name="listEmployeeID">Danh sách ID nhân viên</param>
        /// <returns>Danh sách id nhân viên</returns>
        /// CreatedBy:NTLAM(12/11/2022)
        public bool DeleteMultipleEmployee(List<Guid> listEmployeeID);

        /// <summary>
        /// Kiểm tra xem mã nhân viên đã có chưa
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <returns></returns>
        public Employee DuplicateCode(string EmployeeCode);
    }
}
