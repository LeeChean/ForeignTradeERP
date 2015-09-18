using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace FTERPWeb.Common
{
    public class SqlHelper
    {
        public static SqlConnection CreateConn()
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            conn.Open();
            return conn;
        }

        #region ExecuteReader

        public static IDataReader ExecuteReader(IDbConnection conn, IDbTransaction tran, CommandType cmdType, string cmdText,bool autoClose=false, params IDbDataParameter[] commandParameters)
        {
            IDbCommand cmd = conn.CreateCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
            IDataReader rdr = cmd.ExecuteReader();
            return rdr;
        }

        public static IDataReader ExecuteReader(string cmdText, params IDbDataParameter[] commandParameters)
        {
            IDbConnection conn = CreateConn();
            return ExecuteReader(conn, null, CommandType.Text, cmdText,true, commandParameters);
        }

        #endregion ExecuteReader

        #region ExecuteNonQuery


        public static int ExecuteNonQuery(IDbConnection conn, IDbTransaction trans, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {
            IDbCommand cmd = conn.CreateCommand();
            PrepareCommand(cmd, conn, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            return val;
        }

        public static int ExecuteNonQuery(string cmdText, params IDbDataParameter[] commandParameters)
        {
            int res = 0;
            using (IDbConnection conn = CreateConn())
            {
                res = ExecuteNonQuery(conn, null, CommandType.Text, cmdText, commandParameters);
            }
            return res;
        }


        #endregion ExecuteNonQuery

        #region ExecuteScalar

        public static object ExecuteScalar(IDbConnection conn, IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            IDbCommand cmd = conn.CreateCommand();
            PrepareCommand(cmd, conn, transaction, commandType, commandText, commandParameters);
            object retval = cmd.ExecuteScalar();
            return retval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string cmdText, params IDbDataParameter[] commandParameters)
        {
            object retval = null;
            using (IDbConnection conn = CreateConn())
            {
                retval = ExecuteScalar(conn, null, CommandType.Text, cmdText, commandParameters);
            }
            return retval;
        }


        #endregion ExecuteScalar

        #region Transaction

        /// <summary>
        /// 在一个事务环境中执行批量操作
        /// </summary>
        /// <param name="tcs">自定义包装类(TranCommandStruct)数组</param>
        /// <returns>执行成功结果</returns>
        public static int ExecuteNonQuery(TranCommandStruct[] tcs)
        {
            IDbConnection conn = CreateConn();
            IDbTransaction tran = conn.BeginTransaction();
            int iRes = 0;
            using (tran.Connection)
            {
                try
                {
                    foreach (TranCommandStruct item in tcs)
                    {
                        iRes += ExecuteNonQuery(conn,tran, CommandType.Text, item.CommandText, item.CommandParams);
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    iRes = 0;
                    tran.Rollback();
                }
            }

            return iRes;
        }

        #endregion

        #region 参数和命令处理

        /// <summary>
        /// Internal function to prepare a command for execution by the database
        /// </summary>
        /// <param name="cmd">Existing command object</param>
        /// <param name="conn">Database connection object</param>
        /// <param name="trans">Optional transaction object</param>
        /// <param name="cmdType">Command type, e.g. stored procedure</param>
        /// <param name="cmdText">Command test</param>
        /// <param name="commandParameters">Parameters for the command</param>
        protected static void PrepareCommand(IDbCommand cmd, IDbConnection conn, IDbTransaction trans, CommandType cmdType, string cmdText, IDbDataParameter[] commandParameters)
        {

            if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            if (null == cmd.Connection)
            {
                cmd.Connection = conn;
            }
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            if (commandParameters != null)
            {
                foreach (IDbDataParameter parm in commandParameters)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }

        #endregion

        #region 批量事务数据结构

        /// <summary>
        /// 用于封装批量事务操作的数据结构
        /// </summary>
        public class TranCommandStruct
        {
            public string CommandText { get; set; }
            public IDbDataParameter[] CommandParams { get; set; }

            public TranCommandStruct()
            {
            }

            public TranCommandStruct(string commandText, IDbDataParameter[] commandParams)
            {
                this.CommandText = commandText;
                this.CommandParams = commandParams;
            }
            public TranCommandStruct(string commandText, IDbDataParameter commandParam)
            {
                this.CommandText = commandText;
                this.CommandParams = new IDbDataParameter[] { commandParam };
            }
        }

        ///	<summary>
        ///	查询Table 事务查询
        ///	</summary>
        public static DataTable ExecuteDataTable(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException("The transaction was rollbacked	or commited, please	provide	an open	transaction.", "transaction");

            try
            {
                // Create a	command	and	prepare	it for execution
                SqlCommand cmd = new SqlCommand();

                PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

                SqlDataAdapter oa = new SqlDataAdapter(cmd);

                DataSet set = new DataSet();

                oa.Fill(set);

                return set.Tables[0];
            }
            catch
            {
                throw;
            }

        }
        #endregion

    }
}
