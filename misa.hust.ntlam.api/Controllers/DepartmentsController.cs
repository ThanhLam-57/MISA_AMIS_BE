using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.Common.Entities;
using MySqlConnector;
using Dapper;

namespace MISA.AMIS.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private string sqlconnectstring = "Server=localhost;Port=3306;Database=hust.21h.2022.ntlam;Uid=root;Pwd=123456789;";
        /// <summary>
        /// Hàm thực hiện lấy danh sách phòng ban
        /// </summary>
        /// <returns>Danh sách phòng ban</returns>
        ///  CreatedBy:NTLAM(12/11/2022)
        [HttpGet("get-all")]
        public IActionResult GetAllDepartment()
        {
            try
            {
                var mySqlConnection = new MySqlConnection(sqlconnectstring);

                string getAllDepartment = "SELECT * FROM department";

                var departments = mySqlConnection.Query<Department>(getAllDepartment);
                if (departments != null)
                {
                    return StatusCode(StatusCodes.Status200OK, departments);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }
    }
}
