using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.Common.Entities;
using MISA.AMIS.Controller.Controllers;
using MISA.AMIS.BL.PositionBL;

namespace MISA.AMIS.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : BaseController<Position>
    {
        #region Field
        private IPositionBL _positionBL;
        #endregion
        #region Constructer
        public PositionsController(PositionBL positionBL) : base(positionBL)
        {
            _positionBL = positionBL;
        }
        #endregion
        //private string sqlconnectstring = "Server=localhost;Port=3306;Database=hust.21h.2022.ntlam;Uid=root;Pwd=123456789;";
        ///// <summary>
        ///// Hàm thực hiện lấy danh sách vị trí
        ///// </summary>
        ///// <returns>Danh sách vị trí</returns>
        /////  CreatedBy:NTLAM(12/11/2022)
        //[HttpGet("get-all")]
        //public IActionResult GetAllPosition()
        //{
        //    try
        //    {
        //        var mySqlConnection = new MySqlConnection(sqlconnectstring);

        //        string getAllPosition = "SELECT*FROM positions";

        //        var positions = mySqlConnection.Query<Position>(getAllPosition);
        //        if (positions != null)
        //        {
        //            return StatusCode(StatusCodes.Status200OK, positions);
        //        }
        //        else
        //        {
        //            return StatusCode(StatusCodes.Status400BadRequest);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception.Message);
        //        return StatusCode(StatusCodes.Status400BadRequest, "e001");
        //    }
        //}
    }
}
