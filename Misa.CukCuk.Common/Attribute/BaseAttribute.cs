using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Common
{
    /// <summary>
    /// Attrbute kiểm tra chuỗi rông
    /// </summary>
    /// CreatedBy:NTLAM(22/11/2022)
    public class isNotNullOrEmptyAttribute : Attribute
    {

        public string ErrorMessage { get; set; }

        public isNotNullOrEmptyAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
    /// <summary>
    /// Attrbute kiểm tra chuỗi có phải là số không
    /// </summary>
    /// CreatedBy:NTLAM(22/11/2022)
    public class isNumberAttribute : Attribute
    {
        public string Format { get; set; }
        public string ErrorMessage { get; set; }

        public isNumberAttribute(string format, string errorMessage)
        {
            Format = format;
            ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// Attrbute kiểm tra email
    /// </summary>
    /// CreatedBy:NTLAM(22/11/2022)
    public class validateEmailAttribute : Attribute
    {
        public string Format { get; set; }
        public string ErrorMessage { get; set; }

        public validateEmailAttribute(string format, string errorMessage)
        {
            Format = format;
            ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// Attrbute kiểm tra mã nhân viên
    /// </summary>
    /// CreatedBy:NTLAM(22/11/2022)
    public class CheckCodeAttribute : Attribute
    {
        public bool Check { get; set; }
        public string ErrorMessage { get; set; }

        public CheckCodeAttribute(bool check, string errorMessage)
        {
            Check = check;
            ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// Attrbute kiểm tra ngày
    /// </summary>
    /// CreatedBy:NTLAM(22/11/2022)
    public class CheckDateTimeAttribute : Attribute
    {
        public string ErrorMessage { get; set; }

        public CheckDateTimeAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// Attrbute lấy ra các Header excel
    /// </summary>
    /// CreatedBy:NTLAM(22/11/2022)
    public class CaptionAttribute : Attribute
    {
        public string HeaderCell { get; set; }

        public CaptionAttribute(string headerCell)
        {
            HeaderCell = headerCell;
        }
    }

    public class MaxlengthRecordCodeAttribute : Attribute
    {
        public string ErrorMessage { get; set; }
        public MaxlengthRecordCodeAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
