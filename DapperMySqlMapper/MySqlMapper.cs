using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection;

namespace DapperMySqlMapper
{
    public class MySqlMapper
    {
        public string ConnectionString { get; set; }
        public int? UnableToConnectToHostErrorRetryInterval { get; private set; }
        public bool UnableToConnectToHostErrorRetryTillConnect { get; private set; }

        public MySqlMapper(string connectionString, int? unableToConnectToHostErrorRetryInterval = null, bool unableToConnectToHostErrorRetryTillConnect = false)
        {
            this.ConnectionString = connectionString;
            this.UnableToConnectToHostErrorRetryInterval = unableToConnectToHostErrorRetryInterval;
            this.UnableToConnectToHostErrorRetryTillConnect = unableToConnectToHostErrorRetryTillConnect;
        }

        public MySqlConnection GetConnection(bool openConnection = true, bool retryOnFailure = true)
        {
            var conn = new MySqlConnection(this.ConnectionString);

            if (openConnection)
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException ex)
                {
                    int? retryInterval = this.UnableToConnectToHostErrorRetryInterval;
                    bool retryTillConnect = this.UnableToConnectToHostErrorRetryTillConnect;

                    if (retryTillConnect == true && retryInterval.HasValue == false) { retryInterval = 5000; }

                    if (ex.Number == (int)MySqlErrorCode.UnableToConnectToHost
                        && retryOnFailure == true
                        && retryInterval.HasValue
                        && retryInterval.Value > 0)
                    {
                        Thread.Sleep(retryInterval.Value);

                        return this.GetConnection(openConnection, retryTillConnect);
                    }

                    throw;
                }
                catch
                {
                    throw;
                }
            }

            return conn;
        }

        public IEnumerable<dynamic> Query(string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = this.GetConnection())
            {
                return this.Query(connection, sql, param, transaction, buffered, commandTimeout, commandType);
            }
        }

        public IEnumerable<dynamic> Query(IDbConnection connection, string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.Query(connection, sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = this.GetConnection())
            {
                return this.Query<T>(connection, sql, param, transaction, buffered, commandTimeout, commandType);
            }
        }

        public void SetTypeMap<T>()
        {
            SqlMapper.SetTypeMap(typeof(T), new CustomPropertyTypeMap(typeof(T), (type, columnName) =>
                                type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                    .FirstOrDefault(prop => prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))
                                )
                       );
        }

        public IEnumerable<T> Query<T>(IDbConnection connection, string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            this.SetTypeMap<T>();

            return SqlMapper.Query<T>(connection, sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = this.GetConnection())
            {
                return this.Query<TFirst, TSecond, TReturn>(connection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
            }
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(IDbConnection connection, string sql, Func<TFirst, TSecond, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            this.SetTypeMap<TFirst>();
            this.SetTypeMap<TSecond>();

            return SqlMapper.Query<TFirst, TSecond, TReturn>(connection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = this.GetConnection())
            {
                return this.Query<TFirst, TSecond, TThird, TReturn>(connection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
            }
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(IDbConnection connection, string sql, Func<TFirst, TSecond, TThird, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            this.SetTypeMap<TFirst>();
            this.SetTypeMap<TSecond>();
            this.SetTypeMap<TThird>();

            return SqlMapper.Query<TFirst, TSecond, TThird, TReturn>(connection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public int Execute(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = this.GetConnection())
            {
                return this.Execute(connection, sql, param, transaction, commandTimeout, commandType);
            }
        }

        public int Execute(IDbConnection connection, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.Execute(connection, sql, param, transaction, commandTimeout, commandType);
        }

        public object ExecuteScalar(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = this.GetConnection())
            {
                return this.ExecuteScalar(connection, sql, param, transaction, commandTimeout, commandType);
            }
        }

        public object ExecuteScalar(IDbConnection connection, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.ExecuteScalar(connection, sql, param, transaction, commandTimeout, commandType);
        }

        public DataTable ExecuteDataTable(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool dataHasPrimaryKey = true)
        {
            using (var connection = this.GetConnection())
            {
                return this.ExecuteDataTable(connection, sql, param, transaction, commandTimeout, commandType, dataHasPrimaryKey);
            }
        }

        public DataTable ExecuteDataTable(IDbConnection connection, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool dataHasPrimaryKey = true)
        {
            DataTable dt = new DataTable();
            IDataReader dataReader = SqlMapper.ExecuteReader(connection, sql, param, transaction, commandTimeout, commandType);


            if (dataHasPrimaryKey)
            {
                dt.Load(dataReader);
            }
            else
            {
                bool columnsAdded = false;

                while (dataReader.Read())
                {
                    // add the data table columns
                    if (columnsAdded == false)
                    {
                        DataTable schemaTable = dataReader.GetSchemaTable();

                        foreach (DataRow row in schemaTable.Rows)
                        {
                            string colName = row.Field<string>("ColumnName");
                            Type t = row.Field<Type>("DataType");

                            dt.Columns.Add(colName, t);
                        }

                        columnsAdded = true;
                    }

                    // add the data table rows
                    var newRow = dt.Rows.Add();

                    foreach (DataColumn col in dt.Columns)
                    {
                        newRow[col.ColumnName] = dataReader[col.ColumnName];
                    }
                }

                dataReader.Close();
            }

            return dt;
        }
    }
}
