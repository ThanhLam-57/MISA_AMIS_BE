using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.Common.Entities;
using MySqlConnector;
using MISA.AMIS.BL;
using MISA.AMIS.Common;
using MISA.AMIS.Common.Enums;
using Dapper;
using MISA.AMIS.Controller.Controllers;
using OfficeOpenXml;

namespace MISA.AMIS.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController<Employee>
    {
        #region Field
        private IEmployeeBL _employeeBL;
        #endregion
        #region Constructer
        public EmployeesController(IEmployeeBL eployeeBL): base(eployeeBL)
        {
             _employeeBL = eployeeBL;
        }
        #endregion

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
        [HttpGet]
        [Route("filter")]//done
        public IActionResult FilterEmployees(
            [FromQuery] string? employeeFilter,
            [FromQuery] Guid? positionID,
            [FromQuery] Guid? departmentID,
            [FromQuery] int pageSize =15,
            [FromQuery] int pageNumber = 1)
        {
            try
            {
                var getEmployeeFilter = _employeeBL.FilterEmployees(employeeFilter, positionID, departmentID, pageSize, pageNumber);
                return StatusCode(StatusCodes.Status200OK, getEmployeeFilter);
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    ErrorCode = AmisErrorCode.InvalidData,
                    DevMsg = Resource.DevMsg_Get_Fail,
                    UserMsg = Resource.UserMsg_Get_Fail,
                    MoreInfo = Resource.MoreInfor_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                });

            }
        }


        /// <summary>
        /// API lay max employeeCode
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns></returns>
        /// CreatedBy:NTLAM(12/11/2022)   
        [HttpGet]
        [Route("new-code")]
        public IActionResult getMaxEmployeeCode()
        {
            try
            {
                string newEmployeeCode = _employeeBL.getMaxEmployeeCode();
                return StatusCode(StatusCodes.Status200OK, newEmployeeCode);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    ErrorCode = AmisErrorCode.Exception,
                    DevMsg = Resource.DevMsg_Exception,
                    UserMsg = Resource.UserMsg_Exception,
                    MoreInfo = Resource.MoreInfor_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }

        }

        /// <summary>
        /// Xoá danh sách nhân viên theo ID
        /// </summary>
        /// <param name="listEmployeeID">Danh sách ID nhân viên</param>
        /// <returns>Danh sách id nhân viên</returns>
        /// CreatedBy:NTLAM(12/11/2022)
        [HttpPost]
        [Route("DeleteBatch")]
        public bool DeleteMultipleEmployee(List<Guid> lstEmployeeID)
        {
            var res = _employeeBL.DeleteMultipleEmployee(lstEmployeeID);
            return res;
        }

        /// <summary>
        /// Xuấ khẩu file excel
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("export")]
        public IActionResult Export(CancellationToken cancellationToken)
        {
            var stream = _employeeBL.Export(cancellationToken);
            string excelName = $"Danh_Sach_Nhan_Vien-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}
