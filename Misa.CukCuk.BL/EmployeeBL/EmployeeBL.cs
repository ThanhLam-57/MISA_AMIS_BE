using MISA.AMIS.BL.BaseBL;
using MISA.AMIS.Common;
using MISA.AMIS.Common.Entities;
using MISA.AMIS.Common.Enums;
using MISA.AMIS.DL;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;

namespace MISA.AMIS.BL
{
    public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
    {
        #region Field
        private IEmployeeDL _employeeDL;
        #endregion
        #region Constructor
        public EmployeeBL( IEmployeeDL employeeDL) : base(employeeDL)
        {
            _employeeDL = employeeDL;

        }
        #endregion

        /// <summary>
        /// Xoá danh sách nhân viên theo ID
        /// </summary>
        /// <param name="listEmployeeID">Danh sách ID nhân viên</param>
        /// <returns>Danh sách id nhân viên</returns>
        /// CreatedBy:NTLAM(12/11/2022)
        public bool DeleteMultipleEmployee(List<Guid> listEmployeeID)
        {
            return _employeeDL.DeleteMultipleEmployee(listEmployeeID);
        }

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
        public object FilterEmployees(string? keyword, Guid? positionID, Guid? departmentID, int pageSize = 15, int pageIndex = 1)
        {
            return _employeeDL.FilterEmployees(keyword, positionID, departmentID, pageSize, pageIndex);
        }

        /// <summary>
        /// Lấy mã nhân viên
        /// </summary>
        /// <returns>Mã nhân viên</returns>
        /// CreatedBy:NTLAM(12/11/2022)
        public string getMaxEmployeeCode()
        {
            return _employeeDL.getMaxEmployeeCode();
        }
        /// <summary>
        /// Hàm thực hiện kiểm tra trùng mã nhân viên
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="record"></param>
        /// <returns>Lỗi mã trung</returns>
        /// CreatedBy:NTLAM(22/11/2022)
        public Employee DuplicateCode(string EmployeeCode)
        {
            return _employeeDL.DuplicateCode(EmployeeCode);
        }
        /// <summary>
        /// Hàm thực hiện kiểm tra trùng mã nhân viên
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="record"></param>
        /// <returns>Lỗi mã trung</returns>
        /// CreatedBy:NTLAM(22/11/2022)
        public override ResponseData ValidateCustom(Guid? recordID, Employee record)
        {
            var codeInData = DuplicateCode(record.EmployeeCode);
            //Kiểm tra nếu là sửa thì không bắt trùng mã nếu trùng ID
            if (recordID == Guid.Empty)
            {
                if (codeInData.EmployeeCode == record.EmployeeCode)
                {
                    return new ResponseData(false, new
                    {
                        errorMessage = "Mã nhân viên đã có trong hệ thống"
                    });
                }
            }
            else
            {
                //Điều kiện trùng mã code mà khác ID
                    if(codeInData != null && codeInData.EmployeeCode == record.EmployeeCode && codeInData.EmployeeID != record.EmployeeID)
                    {
                        return new ResponseData(false, new
                        {
                            errorMessage = "Mã nhân viên đã có trong hệ thống"
                        });
                    }
                }
            return new ResponseData(true, null);
        }
        /// <summary>
        /// Xuất flie excel thông tin các bản ghi
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// CreatedBy:NTLAM(22/11/2022)
        public MemoryStream Export(CancellationToken cancellationToken)
        {
            //Lấy danh sách nhân viên từ database
            var employees = (List<Employee>)GetAllRecords();

            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                //Tạo sheet
                var workSheet = package.Workbook.Worksheets.Add("Danh sách nhân viên");
                var datatable = new DataTable("Employees");
                //Tạo tiêu đề
                workSheet.Cells["A1"].Value = "Danh sách nhân viên";        
                //workSheet.Cells["A1"].value("DANH SÁCH NHÂN VIÊN");
                //Tạo header cột
                datatable.Columns.Add(
                    new DataColumn("STT", typeof(int))
                );
                var properties = typeof(Employee).GetProperties();
                foreach (var prop in properties)
                {
                    //Nếu có caption là cột cần xuất khẩu
                    if(Attribute.IsDefined(prop, typeof(CaptionAttribute)))
                    {
                        //Push vào danh sách header
                        var caption = prop.GetCustomAttributes(typeof(CaptionAttribute),true).FirstOrDefault();
                        datatable.Columns.Add(
                            new DataColumn((caption as CaptionAttribute).HeaderCell, typeof(string))
                        );
                    }
                }
                //Mapping dữ liệu data
                int stt = 1;
                foreach(var employee in employees)
                {
                    var row = datatable.NewRow();
                    row[0] = stt;
                    stt++;
                    int i = 1;
                    foreach (var prop in properties)
                    {
                        workSheet.Cells[3,i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[stt + 2, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[stt + 2, i].Style.WrapText = true;
                        //Nếu có caption là cột cần xuất khẩu
                        if (Attribute.IsDefined(prop, typeof(CaptionAttribute)))
                        {                           
                            //Push vào danh sách data
                            var value = prop.GetValue(employee);
                            //Kiểm tra có phải cột date không
                            if (Attribute.IsDefined(prop, typeof(CheckDateTimeAttribute)))
                            {
                                DateTime date;
                                DateTime.TryParse(value.ToString(),out date);
                                row[i] = date.ToString("dd/MM/yyyy");
                                workSheet.Cells[stt + 2, i+1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                            //Kiểm tra xem có phải cột giới tính không
                            else if(prop.Name == "Gender")
                            {
                                if (value.ToString() == EnumGender.Male.ToString())
                                {
                                    row[i] = "Nam";
                                }
                                else if (value.ToString() == EnumGender.FeMale.ToString())
                                {
                                    row[i] = "Nữ";
                                }
                                else row[i] = "Khác";
                            }
                            else
                            {
                                row[i] = value;
                            }
                            i++;
                        }
                    }
                    datatable.Rows.Add(row);
                }

                //Tạo style cho sheet
                workSheet.Cells.Style.Font.SetFromFont("Times New Roman", 11);

                //Merge tiêu đề
                workSheet.Cells["A1:I1"].Merge = true;
                workSheet.Cells["A2:I2"].Merge = true;
                workSheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["A1"].Style.Font.SetFromFont("Arial", 16, true);
                workSheet.Cells["A1"].Style.WrapText = true;

                //
                workSheet.Cells["A3:I3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["A3:I3"].Style.Fill.BackgroundColor.SetColor(60,178, 178, 178);
                workSheet.Cells["A3:I3"].Style.Font.SetFromFont("Arial", 10, true);
                workSheet.Cells["A3:I3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["A3:I3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                
                //Chỉnh độ rộng các cột
                workSheet.Column(1).Width = 5;
                workSheet.Column(2).Width = 15;
                workSheet.Column(3).Width = 26;
                workSheet.Column(4).Width = 12;
                workSheet.Column(5).Width = 15;
                workSheet.Column(6).Width = 26;
                workSheet.Column(7).Width = 26;
                workSheet.Column(8).Width = 16;
                workSheet.Column(9).Width = 26;
                ;
                

                //Insert dữ liệu vào Sheet
                workSheet.Cells["A3"].LoadFromDataTable(datatable, true);
                package.Save();
            }
            stream.Position = 0;

            return stream;
        }
    }
}
