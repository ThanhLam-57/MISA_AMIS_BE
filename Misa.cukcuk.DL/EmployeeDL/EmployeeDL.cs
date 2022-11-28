using Dapper;
using MISA.AMIS.Common.Entities;
using MISA.AMIS.Common.Entities.DTO;
using MISA.AMIS.DL.BaseDL;
using MySqlConnector;

namespace MISA.AMIS.DL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {

        /// <summary>
        /// Xoá danh sách nhân viên theo ID
        /// </summary>
        /// <param name="listEmployeeID">Danh sách ID nhân viên</param>
        /// <returns>Danh sách id nhân viên</returns>
        /// CreatedBy:NTLAM(12/11/2022)
        public bool DeleteMultipleEmployee(List<Guid> listEmployeeID)
        {
            MySqlTransaction transaction;

            var str = string.Join("','", listEmployeeID);

            //Chuẩn bị câu lệnh SQL
            string sql = $" DELETE FROM employee WHERE EmployeeID IN ('{str}');";

            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                mySqlConnection.Open();
                transaction = mySqlConnection.BeginTransaction();
                try
                {
                    //Thực hiện gọi vào DB
                    var res = mySqlConnection.Execute(sql, transaction: transaction);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
                finally
                {
                    transaction.Commit();
                    mySqlConnection.Close();
                }
            }
            //Xử lý kết quả trả về
            return true;
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
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);


            string storedProcdureName = "Proc_employee_GetPaging";

            var parameter = new DynamicParameters();
            parameter.Add("@v_Offset", (pageIndex - 1) * pageSize);
            parameter.Add("@v_limit", pageSize);
            parameter.Add("@v_Soft", "ModifiedDate DESC");

            var lstConditionKey = new List<string>();
            var orConditions = new List<string>();
            var andConditions = new List<string>();
            string whereClause = "";

            if (keyword != null)
            {
                lstConditionKey.Add($" EmployeeCode LIKE '%{keyword}%' ");
                lstConditionKey.Add($" EmployeeName LIKE '%{keyword}%' ");
                lstConditionKey.Add($" PhoneNumber LIKE '%{keyword}%' ");
            }
            if (lstConditionKey.Count > 0)
            {
                andConditions.Add($"({string.Join("OR", lstConditionKey)})");
            }
            if (positionID != null)
            {
                andConditions.Add($" PositionID = '{positionID}'");
            }
            if (departmentID != null)
            {
                andConditions.Add($" DepartmentID = '{departmentID}'");
            }
            if (andConditions.Count > 0)
            {
                whereClause += $" {string.Join(" AND ", andConditions)}";
            }
            parameter.Add("@v_Where", whereClause);

            //Thuc hien goij vaof DB de chay Proc voi tham so dau vao tren
            var multipleResults = mySqlConnection.QueryMultiple(storedProcdureName, parameter, commandType: System.Data.CommandType.StoredProcedure);

                var employees = multipleResults.Read<Employee>().ToList();
                var totalCount = multipleResults.Read<long>().Single();
                 return new PagingData()
                {
                    Data = employees,
                    TotalCount = totalCount
                };
        }


        /// <summary>
        /// Lấy mã nhân viên
        /// </summary>
        /// <returns>Mã nhân viên</returns>
        /// CreatedBy:NTLAM(12/11/2022)
        public String getMaxEmployeeCode()
        {
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            string storedProcedureName = "Proc_employee_GetMaxCode";

            string maxEmployeeCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

            string newEmployeeCode = "NV" + (Int64.Parse(maxEmployeeCode.Substring(2)) + 1).ToString();

            return newEmployeeCode;

        }

        /// <summary>
        /// Lấy mã Code nhân viên và ID
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <returns>checkEmpCode</returns>
        ///  CreatedBy:NTLAM(21/11/2022)
        public Employee DuplicateCode(string EmployeeCode)
        {
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            string storedProcedureName = "Proc_employee_GetCode";

            var parameter = new DynamicParameters();

            parameter.Add("v_EmployeeCode", EmployeeCode);

            //Lấy thông tin ID và Code 
            var checkEmpCode = mySqlConnection.QueryFirstOrDefault<Employee>(storedProcedureName, parameter, commandType: System.Data.CommandType.StoredProcedure);
                return checkEmpCode;
        }

    }
}
