//using System.Data.SqlClient;

//namespace Recipes.Common.DotNetFramework
//{
//    // TODO: refactor and prepare example below
//    /*
//   CREATE TABLE ApplicationLog (
//       id int IDENTITY(1,1) primary key,
//       date_added DateTime NOT NULL DEFAULT(GETUTCDATE()),
//       comment ntext NOT NULL,
//       application_name nvarchar(100)
//      )


//   // crtl + shift + R  - refresh intelisence


//   CREATE PROCEDURE AddAppLog
//   @comment ntext
//   AS
//   SET NOCOUNT ON

//   INSERT INTO ApplicationLog VALUES (
//       DEFAULT,
//       @comment,
//       (SELECT APP_NAME())
//   )

//   CREATE PROCEDURE AddAppLog2
//   @comment ntext
//   AS
//   SET NOCOUNT ON

//   INSERT INTO ApplicationLog VALUES (
//       DEFAULT,
//       @comment,
//       (SELECT APP_NAME()))

//   SELECT SCOPE_IDENTITY()         -- function getting the last identity column value that was inserted


//   CREATE PROCEDURE AddAppLog3
//   @comment ntext,
//   @outid int output
//   AS
//   SET NOCOUNT ON

//   INSERT INTO ApplicationLog VALUES (
//       DEFAULT,
//       @comment,
//       (SELECT APP_NAME()))

//   SET @outid = SCOPE_IDENTITY()


//   CREATE PROCEDURE AddAppLog4
//   @comment ntext
//AS
//   SET NOCOUNT ON

//   INSERT INTO ApplicationLog VALUES (
//       DEFAULT,
//       @comment,
//       (SELECT APP_NAME()))

//   Return SCOPE_IDENTITY()


//   // test stored procedure

//   AddAppLog 'sample message'

//   // test stored procedure with out parameter

//   DECLARE @out int 
//   EXEC AddAppLog3 'sample message 3', @out output
//   SELECT @out


//   // Deleting values
//   CREATE PROCEDURE DeleteAppLog
//       @appname nvarchar(100)
//   AS
//       SET NOCOUNT ON

//       DELETE FROM ApplicationLog WHERE application_name = @appname
//   GO

//   // Updating value

//   CREATE PROCEDURE UpdateCommentText
//       @id int,
//       @newComment nvarchar(100)
//   AS
//       SET NOCOUNT ON

//       UPDATE ApplicationLog
//       SET 
//           comment = @newComment
//       WHERE 
//           id = @id

//*/

//    // what is the difference between this one and AddWithValue??

//    public class ApplicationLog
//    {
//        public static void Add(string comment)
//        {
//            using (SqlConnection connecion = DB.GetSqlConnection())
//            {
//                using (SqlCommand command = connecion.CreateCommand())
//                {
//                    command.CommandText = "AddAppLog";
//                    command.CommandType = CommandType.StoredProcedure;

//                    var parameter = new SqlParameter("comment", SqlDbType.NVarChar, 100);
//                    parameter.Value = comment;
//                    command.Parameters.Add(parameter);

//                    int result = command.ExecuteNonQuery(); // result will be -1 if we set "SET NOCOUNT ON" in SQL server (a little better performance
//                                                            // otherwise the value will equal to the number of affected rows
//                }
//            }
//        }


//        public static void Add2(string comment)
//        {
//            using (SqlConnection connecion = DB.GetSqlConnection())
//            {
//                using (SqlCommand command = connecion.CreateCommand())
//                {
//                    command.CommandText = "AddAppLog2";
//                    command.CommandType = CommandType.StoredProcedure;

//                    var parameter = new SqlParameter("comment", SqlDbType.NVarChar, 100);
//                    parameter.Value = comment;
//                    command.Parameters.Add(parameter);

//                    object result = command.ExecuteScalar();  // return first column of the first row of the last selected statement that was executed
//                }
//            }
//        }


//        public static void Add3(string comment)
//        {

//            using (SqlConnection connecion = DB.GetSqlConnection())
//            {
//                using (SqlCommand command = connecion.CreateCommand())
//                {
//                    command.CommandText = "AddAppLog3";
//                    command.CommandType = CommandType.StoredProcedure;

//                    var parameter = new SqlParameter("comment", SqlDbType.NVarChar, 100);
//                    parameter.Value = comment;
//                    command.Parameters.Add(parameter);

//                    var paramterOut = new SqlParameter("outid", SqlDbType.Int); // Define our output parameter for a store procedure
//                    paramterOut.Direction = ParameterDirection.Output;
//                    command.Parameters.Add(paramterOut);

//                    command.ExecuteNonQuery();      // execute store procedure

//                    object res = paramterOut.Value; // extract the value from output parameter
//                }
//            }
//        }


//        public static void Add4(string comment)
//        {

//            using (SqlConnection connecion = DB.GetSqlConnection())
//            {
//                using (SqlCommand command = connecion.CreateCommand())
//                {
//                    command.CommandText = "AddAppLog4";
//                    command.CommandType = CommandType.StoredProcedure;

//                    var parameter = new SqlParameter("comment", SqlDbType.NVarChar, 100);
//                    parameter.Value = comment;
//                    command.Parameters.Add(parameter);


//                    var paramterReturn = new SqlParameter("outid", SqlDbType.Int); // Define our output parameter for a store procedure
//                    paramterReturn.Direction = System.Data.ParameterDirection.ReturnValue;
//                    command.Parameters.Add(paramterReturn);  // add required output parameter to store procedure

//                    command.ExecuteNonQuery();          // execute store procedure

//                    object res = paramterReturn.Value;  // extract the value from output parameter
//                }
//            }
//        }


//        public static void DeleteCommentsForApp(string appName)
//        {
//            using (SqlConnection connecion = DB.GetSqlConnection())
//            {
//                using (SqlCommand command = connecion.CreateCommand())
//                {
//                    command.CommandText = "DeleteAppLog";
//                    command.CommandType = CommandType.StoredProcedure;

//                    var parameter = new SqlParameter("appName", SqlDbType.NVarChar, 100);
//                    parameter.Value = appName;
//                    command.Parameters.Add(parameter);

//                    command.ExecuteNonQuery();  // execute store procedure
//                }
//            }
//        }


//        public static void UpdateCommentForApp(int commentId, string newComment)
//        {
//            using (SqlConnection connecion = DB.GetSqlConnection())
//            {
//                using (SqlCommand command = connecion.CreateCommand())
//                {
//                    command.CommandText = "UpdateCommentText";
//                    command.CommandType = CommandType.StoredProcedure;

//                    var parameterId = new SqlParameter("id", SqlDbType.Int);
//                    parameterId.Value = commentId;
//                    command.Parameters.Add(parameterId);

//                    var paramterNewComment = new SqlParameter("newComment", SqlDbType.NVarChar, 100);
//                    paramterNewComment.Value = newComment;
//                    command.Parameters.Add(paramterNewComment);

//                    command.ExecuteNonQuery();  // execute store procedure
//                }
//            }
//        }


//        public static DataTable GetLog(string appName)
//        {
//            var table = new DataTable("ApplicationLog");
//            SqlDataAdapter dataAdapter = null;

//            using (SqlConnection connection = DB.GetSqlConnection())
//            {
//                string queryText = @"SELECT* FROM ApplicationLog where application_name = @appname";

//                var command = new SqlCommand(queryText, connection);
//                command.Parameters.Add(new SqlParameter("appname", SqlDbType.NVarChar, 100));
//                command.Parameters["appname"].Value = appName;

//                dataAdapter = new SqlDataAdapter(command);

//                int result = dataAdapter.Fill(table);  // fill the table with records from database
//                                                       // result - number of record that has been returned to the table
//                return table;
//            }
//        }


//        public static DataTable UpdateLogChanges(DataTable tableLog)
//        {
//            var dataAdapter = new SqlDataAdapter();

//            using (SqlConnection connection = DB.GetSqlConnection())
//            {
//                dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM ApplicationLog", connection);
//                var commandBuilder = new SqlCommandBuilder(dataAdapter);    // SqlCommandBuilder allows the SqlDataAdpater to 
//                                                                            // propagate changes automatically to the database
//                dataAdapter.UpdateBatchSize = 100;

//                int result = dataAdapter.Update(tableLog);                  // return the number of records that was modified

//                tableLog.GetChanges();                                      // because select statement was provided the CommandBuilder is able to
//                                                                            // infare the update, insert and delete statement automatically.
//            }
//            return tableLog;
//        }
//    }

//    public class ConnectionStatistics
//    {
//        public IDictionary OriginalStats { get; set; }
//        public long ExecutionTime { get; set; }         // all  metrics are long values
//        public long BytesReceived { get; set; }

//        public ConnectionStatistics(IDictionary stats)
//        {
//            OriginalStats = stats;
//            if (stats.Contains("ExecutionTime")) ExecutionTime = long.Parse(stats["ExecutionTime"].ToString());
//            if (stats.Contains("BytesReceived")) ExecutionTime = long.Parse(stats["BytesReceived"].ToString());
//        }
//    }


//    public class DB
//    {
//        public static ConnectionStatistics LastStatistics { get; set; }

//        public static bool EnableStatistics { get; set; }

//        public static string ConnectionString
//        {
//            get
//            {
//                string connectionString = ConfigurationManager.ConnectionStrings["AWConnection"].ConnectionString;

//                var stringBuilder = new SqlConnectionStringBuilder(connectionString);
//                stringBuilder.ApplicationName = ApplicationName ?? stringBuilder.ApplicationName;
//                stringBuilder.ConnectTimeout = (ConnectionTimeout > 0) ? ConnectionTimeout : stringBuilder.ConnectTimeout;

//                return stringBuilder.ConnectionString;
//            }
//        }


//        public static SqlConnection GetSqlConnection()
//        {
//            var connection = new SqlConnection(ConnectionString);

//            connection.Open();
//            connection.StatisticsEnabled = EnableStatistics;
//            connection.StateChange += conn_StateChange;

//            return connection;
//        }


//        private static void conn_StateChange(object sender, StateChangeEventArgs e)
//        {
//            if (e.CurrentState == ConnectionState.Closed)    // Take place when the collection state changes
//            {
//                if (((SqlConnection)sender).StatisticsEnabled)
//                    LastStatistics = new ConnectionStatistics(((SqlConnection)sender).RetrieveStatistics());
//            }
//        }

//        public static string ApplicationName { get; set; }
//        public static int ConnectionTimeout { get; set; }

//    }

//    public class Employees
//    {
//        public List<Employee> EmployeeList { get; set; }


//        public Employee GetEmployee(int employeeId)
//        {

//            var employee = new Employee();

//            using (SqlConnection connection = DB.GetSqlConnection())
//            {
//                using (SqlCommand command = connection.CreateCommand())
//                {
//                    command.CommandText = "GetEmployeeDetails";
//                    command.CommandType = System.Data.CommandType.StoredProcedure;

//                    SqlParameter param1 = new SqlParameter("businessEntityId", System.Data.SqlDbType.Int);
//                    param1.Value = employeeId;
//                    command.Parameters.Add(param1);

//                    // You can just use: 
//                    //  command.Parameters.AddWithValue("businessEntityId", employeeId);

//                    SqlDataReader dataReader = command.ExecuteReader();


//                    if (dataReader.Read())
//                    {
//                        employee.Load(dataReader);

//                    }

//                }
//            }

//            return employee;
//        }
//    }


//    public class Employee
//    {
//        public int EmployeeId { get; set; }
//        public string FirstName { get; set; }
//        public string LastName { get; set; }
//        public int DepartmentId { get; set; }
//        public string DepartmentName { get; set; }


//        public void Load(SqlDataReader reader)
//        {
//            EmployeeId = Int32.Parse(reader["BusinessEntityId"].ToString());
//            FirstName = reader["FirstName"].ToString();
//            LastName = reader["LastName"].ToString();
//            DepartmentId = Int32.Parse(reader["DepartmentID"].ToString());
//            DepartmentName = reader["Name"].ToString();
//        }
//    }

//}
