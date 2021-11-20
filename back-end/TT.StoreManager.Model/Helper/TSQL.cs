using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.StoreManager.Model
{
    public static class TSQL
    {
        /// <summary>
        /// Hàm thực hiện build câu query lấy dữ liệu data phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelType"></param>
        /// <param name="columnOption"></param>
        /// <returns></returns>
        /// created by vdthang 19.11.2021
        public static string BuildPagingData(PagingRequest request, Type modelType, List<string> columnOption = null)
        {
            // Điều kiện where
            var whereClause = BuildWhereClause(request, modelType);

            var columnString = "*";
            if (columnOption != null && columnOption.Count > 0)
                columnString = string.Join(", ", columnOption.Select(p => $"`{p}`"));

            var model = (BaseModel)Activator.CreateInstance(modelType);
            var finalClause = $"select {columnString} from {model.GetTableName()} {whereClause} order by `ModifiedDate` desc, `CreatedDate` desc limit {request.PageSize} offset {(request.PageIndex - 1) * request.PageSize}";

            return finalClause;
        }

        /// <summary>
        /// Hàm thực hiện build câu query lấy tổng số bản ghi data phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        /// created by vdthang 19.11.2021
        public static string BuildTotalPagingData(PagingRequest request, Type modelType)
        {
            // Điều kiện where
            var whereClause = BuildWhereClause(request, modelType);

            var model = (BaseModel)Activator.CreateInstance(modelType);
            var finalClause = $"select count(*) as 'Total' from {model.GetTableName()} {whereClause}";

            return finalClause;
        }

        /// <summary>
        /// Hàm thực hiện build điều kiện where từ filter UI đẩy lên
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        /// created by vdthang 19.11.2021
        /// TODO: Cần nâng cấp để đáp ứng các điều kiện dạng lồng nhau
        public static string BuildWhereClause(PagingRequest request, Type modelType)
        {
            if (request.PageIndex <= 0 || request.PageSize <= 0)
                throw new Exception("Tham số pageindex và pagesize sai định dạng!");

            if (string.IsNullOrEmpty(request.Filter))
                return string.Empty;

            // Giải mã filter
            var jsonObj = TCrypto.DecryptStringAES(request.Filter);
            var lstFilterItem = TConvert.Deserialize<List<FilterItem>>(jsonObj);
            if (lstFilterItem == null || lstFilterItem.Count == 0)
                return string.Empty;

            // Chống tấn công injection
            PreventInjection(lstFilterItem, modelType);

            // Build điều kiện động
            var whereClause = string.Empty;
            foreach (var item in lstFilterItem)
            {
                whereClause += MapPrefixFilterItem(item.Prefix);
                whereClause += MapConditionAndData(item.PropName, item.Condition, item.Value);
            }

            return $" where {whereClause}";
        }

        /// <summary>
        /// Hàm thực hiện map prefix của phần tử filter item
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        /// created by vdthang 19.11.2021
        private static string MapPrefixFilterItem(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                return string.Empty;

            switch (prefix.ToLower().Trim())
            {
                case "and":
                    return " and ";
                case "or":
                    return " or ";
                default:
                    throw new Exception("Prefix điều kiện filter sai định dạng!");
            }
        }

        /// <summary>
        /// Hàm thực hiện build điều kiện kèm theo data dữ liệu
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="condition"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// created by vdthang 19.11.2021
        private static string MapConditionAndData(string propName, string condition, object value)
        {
            if (string.IsNullOrEmpty(condition))
                throw new Exception("Condition bị trống!");

            var valueF = value;
            if (!string.IsNullOrEmpty(Convert.ToString(value)) && value.GetType().Name == typeof(string).Name)
                valueF = $"'{valueF}'";

            switch (condition.ToLower().Trim())
            {
                case "=":
                case "is":
                    return $" `{propName}` = {valueF} ";
                case "<>":
                case "is not":
                case "different":
                    return $" `{propName}` <> {valueF} ";
                case ">":
                case "bigger":
                    return $" `{propName}` > {valueF} ";
                case ">=":
                case "bigger than":
                    return $" `{propName}` >= {valueF} ";
                case "<":
                case "smaller":
                    return $" `{propName}` < {valueF} ";
                case "<=":
                case "smaller than":
                    return $" `{propName}` <= {valueF} ";
                case "like":
                case "contains":
                    return $" `{propName}` like '%{value}%' ";
                case "is null":
                    return $" `{propName}` is null ";
                case "is not null":
                    return $" `{propName}` is not null ";
                case "=&all":
                    if (string.IsNullOrEmpty(Convert.ToString(value)))
                        return $" (`{propName}` is null or `{propName}` is not null) ";
                    else
                        return $" `{propName}` = {valueF} ";
                default:
                    throw new Exception("Condition điều kiện filter sai định dạng");
            }
        }

        /// <summary>
        /// Hàm thực hiện chống tấn công SQL Injection
        /// </summary>
        /// <param name="filterItems"></param>
        /// <param name="modelType"></param>
        /// created by vdthang 19.11.2021
        private static void PreventInjection(List<FilterItem> filterItems, Type modelType)
        {
            if (filterItems == null || filterItems.Count == 0)
                return;

            var properties = modelType.GetProperties();
            foreach (var item in filterItems)
            {
                if (properties?.Select(p => p.Name?.ToLower())?.Contains(item.PropName?.ToLower()) == false)
                    throw new Exception("Lỗi tấn công SQLInjection!");
            }
        }

        /// <summary>
        /// Hàm thực hiện thực thi câu lệnh scalar comamnd text
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        /// created by vdthang 19.11.2021
        public static object ExecuteScalarCommandText(string query, object param = null, DbTransaction transaction = null)
        {
            object result = null;
            var connection = transaction != null ? transaction.Connection : null;
            var cmd = connection?.CreateCommand();

            if (cmd != null)
            {
                BuildCommand(ref cmd, query, param, CommandType.Text, transaction);
                result = cmd.ExecuteScalar();
            }
            else
            {
                using (connection = new Context().Database.GetDbConnection())
                {
                    connection.Open();
                    cmd = connection.CreateCommand();
                    BuildCommand(ref cmd, query, param, CommandType.Text, transaction);
                    result = cmd.ExecuteScalar();
                }
            }

            return result;
        }

        /// <summary>
        /// Hàm thực hiện thực thực thi câu lệnh scalar comamnd text sync
        /// </summary>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        /// created by vdthang 20.11.2021
        public static async Task<object> ExecuteScalarCommandTextSync(string query, object param = null, DbTransaction transaction = null)
        {
            object result = null;
            var connection = transaction != null ? transaction.Connection : null;
            var cmd = connection?.CreateCommand();

            if (cmd != null)
            {
                BuildCommand(ref cmd, query, param, CommandType.Text, transaction);
                result = await cmd.ExecuteScalarAsync();
            }
            else
            {
                using (connection = new Context().Database.GetDbConnection())
                {
                    connection.Open();
                    cmd = connection.CreateCommand();
                    BuildCommand(ref cmd, query, param, CommandType.Text, transaction);
                    result = await cmd.ExecuteScalarAsync();
                }
            }

            return result;
        }

        /// <summary>
        /// Hàm thực hiện thực thi câu lệnh execute reader command text
        /// </summary>
        /// <param name="modelTypes"></param>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// created by vdthang 20.11.2021
        public static object ExeceuteReaderCommandText(List<Type> modelTypes, string query, object param = null)
        {
            var result = new List<object>();
            using (var connection = new Context().Database.GetDbConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                BuildCommand(ref cmd, query, param, CommandType.Text);
                var reader = cmd.ExecuteReader();
                BuildDataReader(modelTypes, result, reader);
            }

            return result;
        }

        /// <summary>
        /// Hàm thực hiện thực thi câu lệnh execute reader command text sync
        /// </summary>
        /// <param name="modelTypes"></param>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// created by vdthang 20.11.2021
        public static async Task<List<object>> ExeceuteReaderCommandTextSync(List<Type> modelTypes, string query, object param = null)
        {
            var result = new List<object>();
            using (var connection = new Context().Database.GetDbConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                BuildCommand(ref cmd, query, param, CommandType.Text);
                var reader = await cmd.ExecuteReaderAsync();
                BuildDataReader(modelTypes, result, reader);
            }

            return result;
        }

        /// <summary>
        /// Hàm thực hiện build data object từ data reader
        /// </summary>
        /// <param name="modelTypes"></param>
        /// <param name="result"></param>
        /// <param name="reader"></param>
        /// created by vdthang 20.11.2021
        private static void BuildDataReader(List<Type> modelTypes, List<object> result, DbDataReader reader)
        {
            var index = 0;
            do
            {
                var tableData = new List<object>();
                while (reader.Read())
                {
                    var activator = Activator.CreateInstance(modelTypes[index]);
                    var props = activator.GetType().GetProperties();
                    if (props == null || props.Count() == 0)
                        break;

                    foreach (var item in props)
                    {
                        try
                        {
                            if (reader[item.Name] == DBNull.Value)
                                activator.SetValue(item.Name, null);
                            else
                                activator.SetValue(item.Name, reader[item.Name]);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    tableData.Add(activator);
                }
                result.Add(tableData);
                index++;
            } while (reader.NextResult());
        }

        /// <summary>
        /// Hàm thực hiện build command
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// created by vdthang 20.11.2021
        private static void BuildCommand(ref DbCommand dbCommand, string commandText, object param, CommandType commandType, DbTransaction transaction = null)
        {
            dbCommand.CommandText = commandText;
            dbCommand.CommandType = commandType;
            dbCommand.Transaction = transaction;
            var mysqlParam = BuildMySqlParameters(new List<object>() { param });
            if (mysqlParam != null && mysqlParam.Count > 0)
                dbCommand.Parameters.Add(mysqlParam);
        }

        /// <summary>
        /// Hàm thực hiện build parameters cho MYSQL
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// created by vdthang 20.11.2021
        private static List<MySqlParameter> BuildMySqlParameters(List<object> data)
        {
            var result = new List<MySqlParameter>();
            if (data == null || data.Count == 0)
                return result;

            foreach (var obj in data)
            {
                var props = obj?.GetType()?.GetProperties();
                if (props == null || props.Count() == 0)
                    continue;

                foreach (var item in props)
                {
                    if (item.PropertyType.IsGenericType
                        || item.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)
                        || item.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        continue;

                    var param = new MySqlParameter();
                    param.ParameterName = $"v_{item.PropertyType.Name}";
                    param.Value = item.GetValue(obj);
                    switch (item.PropertyType.Name.ToLower())
                    {
                        case "string":
                            param.MySqlDbType = MySqlDbType.VarChar;
                            if (string.IsNullOrEmpty(Convert.ToString(item.GetValue(data))))
                                param.Value = DBNull.Value;
                            break;
                        case "decimal":
                            param.MySqlDbType = MySqlDbType.Decimal;
                            break;
                        case "datetime":
                            param.MySqlDbType = MySqlDbType.DateTime;
                            break;
                        case "int":
                            param.MySqlDbType = MySqlDbType.Int32;
                            break;
                        case "long":
                            param.MySqlDbType = MySqlDbType.Int64;
                            break;
                        case "bool":
                            param.MySqlDbType = MySqlDbType.Bit;
                            break;

                    }
                    result.Add(param);
                }
            }

            return result;
        }
    }
}
