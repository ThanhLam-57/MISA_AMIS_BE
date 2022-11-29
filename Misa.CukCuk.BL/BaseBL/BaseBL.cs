using MISA.AMIS.Common;
using MISA.AMIS.Common.Entities;
using MISA.AMIS.Common.Enums;
using MISA.AMIS.DL.BaseDL;
using System.Text.RegularExpressions;

namespace MISA.AMIS.BL.BaseBL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        private IBaseDL<T> _baseDL;

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;

        }

        /// <summary>
        /// Xoá thông tin bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi cần xoá</param>
        /// <returns></returns>
        /// CreatedBy:NTLAM(12/11/2022)
        public Guid DeleteRecord(Guid recordID)
        {
            return _baseDL.DeleteRecord(recordID);
        }

        /// <summary>
        /// Lấy danh sách tất cả bảng ghi
        /// </summary>
        /// <returns>Danh sách tất cả bảng ghi</returns>
        /// CreatedBy:NTLAM(12/11/2022)
        public IEnumerable<T> GetAllRecords()
        {
            return _baseDL.GetAllRecords();
        }

        /// <summary>
        /// Lấy thông tin bản ghi theo ID
        /// </summary>
        /// <param name="recordByID">ID của bảng ghi muốn lấy</param>
        /// <returns>Thông tin của bảng ghi</returns>
        /// CreatedBy:NTLAM(12/11/2022)
        public T GetRecordByID(Guid recordID)
        {
            return _baseDL.GetRecordByID(recordID);
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="record">Bản ghi mới mới</param>
        /// <returns>Bản ghi mới mới</returns>
        /// CreatedBy:NTLAM(12/11/2022)
        public ResponseData InsertRecord(T record)
        {
            var result = ValidateData(null, record);
            if(result.Success == false)
            {
                return new ResponseData (false,result.Data);
            }
            return new ResponseData(true, _baseDL.InsertRecord(record));
        }

        /// <summary>
        /// Sửa thông tin bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns>ID của bản ghi cần sửa</returns>
        /// CreatedBy:NTLAM(12/11/2022)
        public int UpdateRecord(Guid recordByID, T record)
        {
            return _baseDL.UpdateRecord(recordByID, record);
        }
        /// <summary>
        /// Sửa hoặc thêm mới bản ghi
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="record"></param>
        /// <returns>Thông tin của bản ghi</returns>
        /// CreatedBy:NTLAM(22/11/2022)
        public ResponseData UpdateOrInsert(Guid recordID, T record)
        {
            var result = ValidateData(null, record);
            if (result.Success == false)
            {
                return new ResponseData(false, result.Data);
            }
            return new ResponseData(true, _baseDL.UpdateOrInsert(recordID,record));
        }
        /// <summary>
        /// Hàm thực hiện validate Data
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="record"></param>
        /// <returns>Lỗi validate</returns>
        /// CreatedBy:NTLAM(22/11/2022)
        public ResponseData ValidateData(Guid? recordID, T record)
        {
            var properties = record.GetType().GetProperties();
            List<object> listErr = new List<object>();
            foreach (var prop in properties)
            {
                var propName = prop.Name;
                // lấy giá trị của property truyền lên
                var propValue = prop.GetValue(record);

                // Kiểm tra xem property có attribute là isNotNullOrEmpty không
                var IsNotNullOrEmpty = Attribute.IsDefined(prop, typeof(isNotNullOrEmptyAttribute));

                // Kiểm tra xem property có attribute là isEmail không
                var validateEmail = Attribute.IsDefined(prop, typeof(validateEmailAttribute));

                //Kiểm tra xem property có attribute là OnlyNumber không
                var isOnlyNumber = Attribute.IsDefined(prop, typeof(isNumberAttribute));

                //Kiểm tra property có attribute là CheckDateTime không
                var checkDateTime = Attribute.IsDefined(prop, typeof(CheckDateTimeAttribute));

                if (IsNotNullOrEmpty == true)
                {
                    var attribute = prop.GetCustomAttributes(typeof(isNotNullOrEmptyAttribute), true).FirstOrDefault();
                    var errorMessage = (attribute as isNotNullOrEmptyAttribute).ErrorMessage;
                    if (propValue == null || propValue.ToString().Trim() == "")
                    {
                        listErr.Add(new
                        {
                            name = propName,
                            value = errorMessage,
                        });
                        //return new ResponseData(false, new
                        //{
                        //    ErrorMessage = errorMessage,
                        //});
                    }
                }

                if (validateEmail == true)
                {
                    var attribute = prop.GetCustomAttributes(typeof(validateEmailAttribute), true).FirstOrDefault();
                    var errorMessage = (attribute as validateEmailAttribute).ErrorMessage;
                    bool checkEmail = IsValidEmail(propValue?.ToString());
                    if (propValue != null && !checkEmail)
                    {
                        listErr.Add(new
                        {
                            name = propName,
                            value = errorMessage,
                        });
                        //return new ResponseData(false, new
                        //{
                        //    ErrorMessage = errorMessage,
                        //});
                    }
                }

                if (isOnlyNumber == true)
                {
                    //lấy ra attribute
                    var attribute = prop.GetCustomAttributes(typeof(isNumberAttribute), true).FirstOrDefault();

                    // lấy ra regex 
                    var regex = new Regex((attribute as isNumberAttribute).Format);

                    var errorMessage = (attribute as isNumberAttribute).ErrorMessage;

                    // thêm điều kiện nếu propValue = "" 
                    if (propValue != null && !regex.IsMatch(propValue.ToString()))
                    {
                        listErr.Add(new
                        {
                            name = propName,
                            value = errorMessage,
                        });
                        //return new ResponseData(false, new
                        //{
                        //    errorMessage = errorMessage,
                        //});
                    }
                }
                if(checkDateTime == true)
                {
                    var attribute = prop.GetCustomAttributes(typeof(CheckDateTimeAttribute), true).FirstOrDefault();
                    var errorMessage = (attribute as CheckDateTimeAttribute).ErrorMessage;
                    if (propValue != null && !IsValidDate(propValue.ToString()) && propValue.ToString() != "")
                    {
                        listErr.Add(new
                        {
                            name = propName,
                            value = errorMessage,
                        });
                        //return new ResponseData(false, new
                        //{
                        //    errorMessage = errorMessage,
                        //});
                    }
                }
            }
            //chạy validate riêng
            var res = ValidateCustom(recordID, record);
            if (!res.Success)
            {
                listErr.Add(res.Data);
            }
            if (listErr.Count > 0)
            {
                return new ResponseData(false,listErr);
            }
            return res;
        }
        /// <summary>
        /// Hàm thực hiện kiểm tra trùng mã nhân viên
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="record"></param>
        /// <returns>Lỗi mã trung</returns>
        /// CreatedBy:NTLAM(22/11/2022)
        public virtual ResponseData ValidateCustom(Guid? recordID, T record)
        {
            return new ResponseData(true, null);
        }
        /// <summary>
        /// Hàm thực hiện validate email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// CreatedBy:NTLAM(22/11/2022)
        public bool IsValidEmail(string email) 
        {
            if(email == null)
            {
                return true;
            }
            else
            {
                var trimmedEmail = email.Trim();

                if (trimmedEmail.EndsWith("."))
                {
                    return false;
                }
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == trimmedEmail;
                }
                catch
                {
                    return false;
                }

            }
        }
        /// <summary>
        /// Validate ngày không vượt quá ngày hiện tại
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        /// CreatedBy:NTLAM(22/11/2022)
        public static bool IsValidDate(string date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                if (dt > DateTime.Now)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
