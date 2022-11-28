namespace MISA.AMIS.Common.Entities
{
    public class Department
    {
        /// <summary>
        /// ID đơn vị
        /// </summary>
        public Guid DepartmentID { get; set; }
        /// <summary>
        /// Mã đơn vị
        /// </summary>
        public string DepartmentCode { get; set; }
        /// <summary>
        /// Tên đơn vị
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// Ngày thêm
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Người thêm
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime ModifiedDate { get; set; }
        /// <summary>
        /// Người sửa
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
