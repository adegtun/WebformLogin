using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using UserLoader.Model;
//using Oracle.ManagedDataAccess.Client;
using Oracle.DataAccess.Client;
using System.Data;
using NLog;

namespace UserLoader
{
    public class DataAccess
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string SqlConnectionString;
        private string mpass;
        OracleConnection conn;
        public DataAccess()
        {
            SqlConnectionString = ConfigurationManager.AppSettings["upm_connectionstring"];
            mpass = ConfigurationManager.AppSettings["upmpass"];
        }

        public Employee GetEmployeeDetails(string email)
        {
            Employee employee;
            try
            {
                Crypto crypto = new Crypto();
                mpass = crypto.Decrypt(mpass);
                SqlConnectionString = SqlConnectionString.Replace("{upmpass}", mpass);
                logger.Info(SqlConnectionString);
                string sql = "select a.EMP_ID, a.EMP_NAME, a.EMP_EMAIL_ID, b.USER_ID from tbaadm.get a join tbaadm.upr b on b.USER_EMP_ID = a.EMP_ID and a.EMP_EMAIL_ID = '" + email + "'";
                conn = new OracleConnection(SqlConnectionString);  // C#
                conn.Open();
                logger.Info("connection opened");
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                logger.Info("User query: " + sql);
                cmd.CommandType = CommandType.Text;
                
                OracleDataReader dr = cmd.ExecuteReader();
                dr.Read();
                employee = new Employee
                {
                    StaffId = dr.GetString(0),
                    EmployeeName = dr.GetString(1),
                    Email = dr.GetString(2),
                    FinacleID = dr.GetString(3)
                };                 
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                throw;
            }
            finally
            {
                conn.Dispose();
            }
            return employee;
        }

    }
}
