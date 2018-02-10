using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Recipes.Persistence
{
    public static class AdoNetSamples
    {
        private const string _connectionString = "....";

        public static void CallProcedureSimple()
        {
            string _commandText = "";
            string _someParam = "";

            try
            {
                using (var connecion = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(_commandText, connecion))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@paramName", _someParam);

                    // to be more specified
                    command.Parameters.Add(new SqlParameter("@oaram2Name", SqlDbType.NVarChar, 100)
                    {
                        Value = _someParam
                    });

                    int result = command.ExecuteNonQuery(); // -1 is ok
                }
            }
            catch (Exception ex)
            {
                // log error
                throw ex;
            }
        }


        public static void CallProcedureAndReturnScalar()
        {
            string _commandText = "";
            string _someParam = "";

            try
            {
                using (var connecion = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(_commandText, connecion))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@paramName", _someParam);

                    // to be more specified
                    command.Parameters.Add(new SqlParameter("@oaram2Name", SqlDbType.NVarChar, 100)
                    {
                        Value = _someParam
                    });

                    int result = command.ExecuteNonQuery();  // -1 is ok
                    // int result = command.ExecuteScalar(); // to retrieve scalar value
                }
            }
            catch (Exception ex)
            {
                // log error
                throw ex;
            }
        }


        public static void CallProcedureWithOutputParam()
        {
            string _commandText = "";
            string _someParam = "";

            try
            {
                using (var connecion = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(_commandText, connecion))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("comment", SqlDbType.NVarChar, 100)
                    {
                        Value = _someParam
                    });

                    var paramterOut = new SqlParameter("outid", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output // output param for sp
                    }; 
                    command.Parameters.Add(paramterOut);


                    command.ExecuteNonQuery();      // execute store procedure
                    object res = paramterOut.Value; // extract the value from output parameter
                }
            }
            catch (Exception ex)
            {
                // log error
                throw ex;
            }
        }


        public static void CallProcedureWithOutputParam2()
        {
            string _commandText = "";
            string _someParam = "";

            try
            {
                using (var connecion = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(_commandText, connecion))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("comment", SqlDbType.NVarChar, 100)
                    {
                        Value = _someParam
                    });

                    var paramterReturn = new SqlParameter("outid", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue  // output param for sp
                    };
                    command.Parameters.Add(paramterReturn);

                    command.ExecuteNonQuery();          // execute store procedure
                    object res = paramterReturn.Value;  // extract the value from output parameter
                }
            }
            catch (Exception ex)
            {
                // log error
                throw ex;
            }
        }
        

        public static DataTable GetTableData()
        {
            string _commandText = "";
            var table = new DataTable("Some table");
            SqlDataAdapter dataAdapter = null;

            try
            {
                using (var connecion = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(_commandText, connecion))
                {
                    command.Parameters.Add(new SqlParameter("appname", SqlDbType.NVarChar, 100));
                    command.Parameters["appname"].Value = "some value";

                    dataAdapter = new SqlDataAdapter(command);
                    int result = dataAdapter.Fill(table);   // fill the table with records from database
                                                            // result - number of record that has been returned to the table
                    return table;
                }
            }
            catch (Exception ex)
            {
                // log error
                throw ex;
            }
        }


        public static DataTable UpdateDataChanges(DataTable tableLog)
        {
            string _commandText = "SELECT * FROM Sometab";
            var table = new DataTable("Some table");
            SqlDataAdapter dataAdapter = null;

            try
            {
                using (var connecion = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(_commandText, connecion))
                {
                    var commandBuilder = new SqlCommandBuilder(dataAdapter);    // SqlCommandBuilder allows the SqlDataAdpater to 
                                                                                // propagate changes automatically to the database
                    dataAdapter.UpdateBatchSize = 100;

                    int result = dataAdapter.Update(tableLog);                  // return the number of records that was modified

                    tableLog.GetChanges();                                      // because select statement was provided the CommandBuilder is able to
                                                                                // infare the update, insert and delete statement automatically.
                    return table;
                }
            }
            catch (Exception ex)
            {
                // log error
                throw ex;
            }
        }
    }


    public class ConnectionStatistics
    {
        public IDictionary OriginalStats { get; set; }
        public long ExecutionTime { get; set; }         // all  metrics are long values
        public long BytesReceived { get; set; }

        public ConnectionStatistics(IDictionary stats)
        {
            OriginalStats = stats;
            if (stats.Contains("ExecutionTime")) ExecutionTime = long.Parse(stats["ExecutionTime"].ToString());
            if (stats.Contains("BytesReceived")) ExecutionTime = long.Parse(stats["BytesReceived"].ToString());
        }
    }


    public class DB
    {
        public static ConnectionStatistics LastStatistics { get; set; }

        public static bool EnableStatistics { get; set; }

        public static string ConnectionString
        {
            get
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AWConnection"].ConnectionString;

                var stringBuilder = new SqlConnectionStringBuilder(connectionString);
                stringBuilder.ApplicationName = ApplicationName ?? stringBuilder.ApplicationName;
                stringBuilder.ConnectTimeout = (ConnectionTimeout > 0) ? ConnectionTimeout : stringBuilder.ConnectTimeout;

                return stringBuilder.ConnectionString;
            }
        }


        public static SqlConnection GetSqlConnection()
        {
            var connection = new SqlConnection(ConnectionString);

            connection.Open();
            connection.StatisticsEnabled = EnableStatistics;
            connection.StateChange += Connection_StateChange;

            return connection;
        }


        private static void Connection_StateChange(object sender, StateChangeEventArgs e)
        {
            if (e.CurrentState == ConnectionState.Closed)    // Take place when the collection state changes
            {
                if (((SqlConnection)sender).StatisticsEnabled)
                    LastStatistics = new ConnectionStatistics(((SqlConnection)sender).RetrieveStatistics());
            }
        }

        public static string ApplicationName { get; set; }
        public static int ConnectionTimeout { get; set; }

    }


    public class Employees
    {
        public List<Employee> EmployeeList { get; set; }


        public Employee GetEmployee(int employeeId)
        {
            string _commandText = "";
            var employee = new Employee();

            try
            {
                using (var connecion = DB.GetSqlConnection())
                using (var command = new SqlCommand(_commandText, connecion))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("businessEntityId", System.Data.SqlDbType.Int)
                    {
                        Value = employeeId
                    });

                    var dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        employee.Load(dataReader);
                    }
                }
            }
            catch (Exception ex)
            {
                // log error
                throw ex;
            }

            return employee;
        }
    }


    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }


        public void Load(SqlDataReader reader)
        {
            EmployeeId = Int32.Parse(reader["BusinessEntityId"].ToString());
            FirstName = reader["FirstName"].ToString();
            LastName = reader["LastName"].ToString();
            DepartmentId = Int32.Parse(reader["DepartmentID"].ToString());
            DepartmentName = reader["Name"].ToString();
        }
    }
}
