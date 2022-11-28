namespace MISA.AMIS.Common.Entities
{
    public class Position
    {
        /// <summary>
        /// ID chức danh
        /// </summary>
        public Guid PositionID { get; set; }
        /// <summary>
        /// Mã chức danh
        /// </summary>
        public string PositionCode { get; set; }
        /// <summary>
        /// Tên chức danh
        /// </summary>
        public string PositionName { get; set; }
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
