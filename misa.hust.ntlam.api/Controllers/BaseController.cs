using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.BL.BaseBL;
using MISA.AMIS.Common.Enums;
using MISA.AMIS.Common;
using MySqlConnector;

namespace MISA.AMIS.Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase
    {
        #region Field
        private IBaseBL<T> _baseBL;
        #endregion

        #region Constructer
        public BaseController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }
        #endregion

        /// <summary>
        /// Lấy danh sách tất cả bảng ghi
        /// </summary>
        /// <returns>Danh sách tất cả bảng ghi</returns>
        /// CreatedBy:NTLAM(13/11/2022)
        [HttpGet]
        public IActionResult GetAllRecords()
        {
            try
            {
                var records = _baseBL.GetAllRecords();
                if (records != null)
                {
                    return StatusCode(StatusCodes.Status200OK, records);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, records);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    ErrorCode = AmisErrorCode.Exception,
                    DevMsg = Resource.DevMsg_Get_Fail,
                    UserMsg = Resource.UserMsg_Get_Fail,
                    MoreInfo = Resource.MoreInfor_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }

        /// <summary>
        /// Lấy thông tin bản ghi theo ID
        /// </summary>
        /// <param name="recordByID">ID của bảng ghi muốn lấy</param>
        /// <returns>Thông tin của bảng ghi</returns>
        /// CreatedBy:NTLAM(13/11/2022)
        [HttpGet]
        [Route("{recordID}")]
        public IActionResult GetRecordByID(Guid recordID)
        {
            try
            {
                var record = _baseBL.GetRecordByID(recordID);

                if (record != null)
                {
                    return StatusCode(StatusCodes.Status200OK, record);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, null);
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
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
        /// Xoá Bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi cần xoá</param>
        /// <returns></returns>
        /// CreatedBy:NTLAM(13/11/2022)
        [HttpDelete]
        [Route("{recordID}")]
        public IActionResult DeleteRecord(Guid recordID)
        {
            try
            {
                var recordId = _baseBL.DeleteRecord(recordID);
                if(recordId == Guid.Empty)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        ErrorCode = AmisErrorCode.Exception,
                        DevMsg = Resource.UserMsg_Delete_Empty,
                        UserMsg = Resource.UserMsg_Delete_Empty,
                        MoreInfo = Resource.MoreInfor_Exception,
                        TraceId = HttpContext.TraceIdentifier,
                    });
                }
                return StatusCode(StatusCodes.Status200OK, recordId);
            }
            catch (Exception exception)

            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
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
        /// Sửa thông tin bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns>ID của bản ghi cần sửa</returns>
        /// CreatedBy:NTLAM(13/11/2022)
        [HttpPut("{recordID}")]
        public IActionResult UpdateRecord(
                [FromRoute] Guid recordID,
                [FromBody] T record
                )
        {
            try
            {
                var numberOfRecord = _baseBL.UpdateRecord(recordID, record);
                return StatusCode(StatusCodes.Status200OK, numberOfRecord);

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = AmisErrorCode.Exception,
                    DevMsg = Resource.Dev_employee_Duplicate,
                    UserMsg = Resource.UserMsg_employee_Duplicate,
                    MoreInfo = Resource.MoreInfor_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }


        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="record">Bản ghi mới</param>
        /// <returns>bản ghi mới</returns>
        /// CreatedBy:NTLAM(12/11/2022)
        [HttpPost]
        public IActionResult InsertRecord([FromBody] T record)
        {
            try
            {
                var newRecord = _baseBL.InsertRecord(record);
                if(newRecord.Success == true)
                {
                   return StatusCode(StatusCodes.Status200OK, newRecord.Data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, newRecord.Data);
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = AmisErrorCode.Exception,
                    DevMsg = Resource.DevMsg_Insert_Fail,
                    UserMsg = Resource.UserMsg_Insert_Fail,
                    MoreInfo = Resource.MoreInfor_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }

        [HttpPost("UpdateOrInsert")]
        public IActionResult UpdateOrInsert(
            [FromQuery] Guid recordID,
            [FromBody] T record)
        {
            try
            {
                ////Call method UpdateOrInsert
                var result = _baseBL.UpdateOrInsert(recordID, record);
                if (result.Success)
                {
                    return recordID != Guid.Empty ? StatusCode(StatusCodes.Status200OK, result.Data) : StatusCode(StatusCodes.Status201Created, result.Data);
                }else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, result.Data);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = AmisErrorCode.Exception,
                    DevMsg = Resource.UserMsg_Exception,
                    UserMsg = Resource.DevMsg_Exception,
                    MoreInfo = Resource.MoreInfor_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                    Mess = "2"
                });
                throw;
            }
        }
    }
}
