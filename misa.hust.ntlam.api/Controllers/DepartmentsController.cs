using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.Common.Entities;
using MySqlConnector;
using Dapper;
using MISA.AMIS.Controller.Controllers;
using MISA.AMIS.BL;

namespace MISA.AMIS.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : BaseController<Department>
    {
        #region Field
        private IDepartmentBL _departmentBL;
        #endregion
        #region Constructer
        public DepartmentsController(IDepartmentBL departmentBL) : base(departmentBL)
        {
            _departmentBL = departmentBL;
        }
        #endregion
        //private string sqlconnectstring = "Server=localhost;Port=3306;Database=hust.21h.2022.ntlam;Uid=root;Pwd=123456789;";
        ///// <summary>
        ///// Hàm thực hiện lấy danh sách phòng ban
        ///// </summary>
        ///// <returns>Danh sách phòng ban</returns>
        /////  CreatedBy:NTLAM(12/11/2022)
        //[HttpGet("get-all")]
        //public IActionResult GetAllDepartment()
        //{
        //    try
        //    {
        //        var mySqlConnection = new MySqlConnection(sqlconnectstring);

        //        string getAllDepartment = "SELECT * FROM department";

        //        var departments = mySqlConnection.Query<Department>(getAllDepartment);
        //        if (departments != null)
        //        {
        //            return StatusCode(StatusCodes.Status200OK, departments);
        //        }
        //        else
        //        {
        //            return StatusCode(StatusCodes.Status400BadRequest);
        //        }
        //    }
        //    catch(Exception exception)
        //    {
        //        Console.WriteLine(exception.Message);
        //        return StatusCode(StatusCodes.Status400BadRequest, "e001");
        //    }
        //}
    }
}
