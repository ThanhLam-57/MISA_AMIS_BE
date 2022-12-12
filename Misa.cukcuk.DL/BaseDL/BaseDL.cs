using Dapper;
using MISA.AMIS.Common;
using MISA.AMIS.Common.Constants;
using MySqlConnector;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MISA.AMIS.DL.BaseDL
{
    public class BaseDL<T> : IBaseDL<T>
    {
        /// <summary>
        /// Xoá Bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi cần xoá</param>
        /// <returns></returns>
        /// CreatedBy:NTLAM(13/11/2022)
        public Guid DeleteRecord(Guid recordID)
        {
            string storedProcedureName = String.Format(Procedues.DELETE_BY_ID, typeof(T).Name);

            var parameters = new DynamicParameters();
            parameters.Add($"@{typeof(T).Name}Id", recordID);
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                int numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                    if (numberOfAffectedRows > 0)
                    {
                        return recordID;
                    }
                    else return Guid.Empty;

            }
        }

        /// <summary>
        /// Lấy danh sách tất cả bảng ghi
        /// </summary>
        /// <returns>Danh sách tất cả bảng ghi</returns>
        /// CreatedBy:NTLAM(13/11/2022)
        public IEnumerable<T> GetAllRecords()
        {
            string storedProcedureName = String.Format(Procedues.GET_ALL, typeof(T).Name); ;
            //var records = new List<T>();
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var records = mySqlConnection.Query<T>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);
                    return records;
            }

        }

        /// <summary>
        /// Lấy thông tin bản ghi theo ID
        /// </summary>
        /// <param name="recordByID">ID của bảng ghi muốn lấy</param>
        /// <returns>Thông tin của bảng ghi</returns>
        /// CreatedBy:NTLAM(13/11/2022)
        public T GetRecordByID(Guid recordID)
        {

            string storedProcedureName = String.Format(Procedues.GET_BY_ID, typeof(T).Name);

            var parameters = new DynamicParameters();
            parameters.Add($"@{typeof(T).Name}ID", recordID);
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var record = mySqlConnection.QueryFirstOrDefault<T>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                    return record;

            }
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="record">Bản ghi mới</param>
        /// <returns>bản ghi mới</returns>
        /// CreatedBy:NTLAM(12/11/2022)
        public T InsertRecord(T record)
        {
            //lấy chuỗi kết nối
            var connectionString = DatabaseContext.ConnectionString;

            //lấy tên Procedure 
            string sqlCommand = String.Format(Procedues.INSERT_RECORD, typeof(T).Name);

            //Khai báo tham số cho procedure
            var parameters = new DynamicParameters();

            //lấy ra mảng các thuộc tính của Generic
            var properties = record.GetType().GetProperties();

            // Khai báo giá trị của thuộc tính
            object propValue;

            //Tạo Guid cho ID
            var newRecordID = Guid.NewGuid();

            //Lặp qua mảng các thuộc  tính
            foreach (var prop in properties)
            {
                // Kiểm tra xem có phải ID không
                var primaryKeyAttribute = KeyAttribute.GetCustomAttribute(prop, typeof(KeyAttribute));

                if (primaryKeyAttribute != null)
                {
                    propValue = newRecordID;
                }
                else
                {
                    propValue = prop.GetValue(record);
                }
                parameters.Add($"@{prop.Name}", propValue);
            }
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                {
                    mySqlConnection.Open();
                    var transaction = mySqlConnection.BeginTransaction();
                    var result = mySqlConnection.Execute(sqlCommand, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        transaction.Commit();
                        mySqlConnection.Close();
                        return record;
                    }
                    else
                    {
                        transaction.Rollback();
                        mySqlConnection.Close();
                        return record;
                    }
                }
            }
        }
        /// <summary>
        /// Sửa hoặc thêm mới bản ghi
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="record"></param>
        /// <returns>Thông tin của bản ghi</returns>
        /// CreatedBy:NTLAM(22/11/2022)
        public object UpdateOrInsert(Guid recordID, T record)
        {

            var newRecordID = Guid.Empty;
            string sqlCommand = "";

            //lấy chuỗi kết nối
            var connectionString = DatabaseContext.ConnectionString;

            //Khai báo tham số cho procedure
            var parameters = new DynamicParameters();
            //lấy ra mảng các thuộc tính của Generic
            var properties = record.GetType().GetProperties();
            if (recordID == Guid.Empty)
            {
                //lấy tên Procedure 
                sqlCommand = String.Format(Procedues.INSERT_RECORD, typeof(T).Name);
                newRecordID = Guid.NewGuid();
            }
            else
            {
                //lấy tên Procedure 
                sqlCommand = String.Format(Procedues.UPDATE_RECORD, typeof(T).Name);
                newRecordID = recordID;
            }
            object propValue;

            foreach (var prop in properties)
            {
                // Kiểm tra xem có phải ID không
                var primaryKeyAttribute = KeyAttribute.GetCustomAttribute(prop, typeof(KeyAttribute));

                if (primaryKeyAttribute != null)
                {
                    propValue = newRecordID;
                }
                else
                {
                    propValue = prop.GetValue(record);
                }
                parameters.Add($"@{prop.Name}", propValue);
            }

            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                if (mySqlConnection.State != ConnectionState.Open)
                {
                    mySqlConnection.Open();
                }
                var transaction = mySqlConnection.BeginTransaction();
                var result = mySqlConnection.Execute(sqlCommand, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);
                if (result > 0)
                {
                    transaction.Commit();
                    if (mySqlConnection.State == ConnectionState.Open)
                    {
                        mySqlConnection.Close();
                    }
                    return newRecordID;
                }
                else
                {
                    transaction.Rollback();
                    if (mySqlConnection.State == ConnectionState.Open)
                    {
                        mySqlConnection.Close();
                    }
                    return result;
                }
            }
        } 

        /// <summary>
        /// Sửa thông tin bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns>ID của bản ghi cần sửa</returns>
        /// CreatedBy:NTLAM(13/11/2022)
        public int UpdateRecord(Guid recordID, T record)
        {
            //lấy chuỗi kết nối
            var connectionString = DatabaseContext.ConnectionString;

            //lấy tên Procedure 
            string sqlCommand = String.Format(Procedues.UPDATE_RECORD, typeof(T).Name);

            //Khai báo tham số cho procedure
            var parameters = new DynamicParameters();

            //lấy ra mảng các thuộc tính của Generic
            var properties = record.GetType().GetProperties();

            object propValue;

            foreach (var prop in properties)
            {
                // Kiểm tra xem có phải ID không
                var primaryKeyAttribute = KeyAttribute.GetCustomAttribute(prop, typeof(KeyAttribute));

                if (primaryKeyAttribute != null)
                {
                    propValue = recordID;
                }
                else
                {
                    propValue = prop.GetValue(record);
                }
                parameters.Add($"@{prop.Name}", propValue);
            }

            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();
                var transaction = mySqlConnection.BeginTransaction();
                var result = mySqlConnection.Execute(sqlCommand, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);
                if (result > 0)
                {
                    transaction.Commit();
                    mySqlConnection.Close();
                    return result;
                }
                else
                {
                    transaction.Rollback();
                    mySqlConnection.Close();
                    return result;
                }
            }
        }
    }
}

