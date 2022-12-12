using MISA.AMIS.Common.Enums;

namespace MISA.AMIS.Common.Entities
{
    /// <summary>
    /// Thông tin nhâ viên
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// ID nhân viên
        /// </summary>
        public Guid? EmployeeID { get; set; }
        /// <summary>
        /// Mã nhân viên
        /// </summary>

        [isNotNullOrEmpty("Mã nhân viên không được để trống")]
        [Caption("Mã nhân viên")]
        [MaxlengthRecordCode("Mã nhân viên không được quá 20 kí tự")]
        public string EmployeeCode { get; set; }
        /// <summary>
        /// Họ và tên nhân viên
        /// </summary>

        [isNotNullOrEmpty("Tên nhân viên không được để trống")]
        [Caption("Họ và tên")]
        public string? EmployeeName { get; set; }
        /// <summary>
        /// Ngày sinh
        /// </summary>

        [CheckDateTime("Ngày không được vượt quá ngày hiện tại")]
        [Caption("Ngày sinh")]
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// Giới tính
        /// </summary>
        [Caption("Giới tính")]
        public EnumGender Gender { get; set; }
        /// <summary>
        /// Số căn cước công dân
        /// </summary>

        [isNumber("^[0-9]+$","Số chứng minh nhân dân phải là số")]
        public string? IdentityNumber { get; set; }
        /// <summary>
        /// Ngày cấp căn cước
        /// </summary>
        public DateTime? IdentityIssuedDate { get; set; }
        /// <summary>
        /// Mơi cấp căn cước
        /// </summary>
        public string? IdentityIssuedPlace { get; set; }
        /// <summary>
        /// Địa chỉ email
        /// </summary>

        [validateEmail(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", "Email không đúng định dạng")]
        public string? Email { get; set; }
        /// <summary>
        /// Số điẹn thoại cố định
        /// </summary>
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// ID Chức danh
        /// </summary>
        public Guid? PositionID { get; set; }
        /// <summary>
        /// Mã Chức danh
        /// </summary>
        public string? PositionCode { get; set; }
        /// <summary>
        /// Tên chức danh
        /// </summary>
        [Caption("Chức danh")]
        public string? PositionName { get; set; }
        /// <summary>
        /// ID đơn vị
        /// </summary>
        public Guid? DepartmentID { get; set; }
        /// <summary>
        /// Mã đơn vị
        /// </summary>
        public string? DepartmentCode { get; set; }
        /// <summary>
        /// Tên đơn vị
        /// </summary>
        [isNotNullOrEmpty("Mã nhân viên không được để trống")]
        [Caption("Đơn vị")]
        public string? DepartmentName { get; set; }
        /// <summary>
        /// Ngày gia nhập công ty
        /// </summary>
        public DateTime? JoiningDate { get; set; }
        /// <summary>
        /// Ngày thêm
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Người thêm
        /// </summary>
        public string? CreatedBy { get; set; }
        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
        /// <summary>
        /// Người sửa
        /// </summary>
        public string? ModifiedBy { get; set; }
        /// <summary>
        /// Số điện thoại di động
        /// </summary>
        public string? TelephoneNumber { get; set; }
        /// <summary>
        /// Số tài khoản ngân hàng
        /// </summary>
        [isNumber("^[0-9]+$", "Số tài khoản phải là số")]
        [Caption("Số tài khoản")]
        public string? BankAccountNumber { get; set; }
        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        [Caption("Tên ngân hàng")]
        public string? BankName { get; set; }
        /// <summary>
        /// Chi nhánh ngân hàng
        /// </summary>
        public string? BankBranchName { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Address { get; set; }
    }
        public class ListEmployeeID
        {
            public List<Guid> EmployeeIDs { get; set; }
        }
}
