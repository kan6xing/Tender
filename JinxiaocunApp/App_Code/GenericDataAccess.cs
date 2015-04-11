using System;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

/// <summary>
/// GenericDataAccess 的摘要说明
/// </summary>
/// 
namespace JinxiaocunApp
{
    public class GenericDataAccess
    {

        //DbConnection dconn = null;
        DbTransaction tran = null;
        public bool isError = false;
        DbCommand dcomm = null;


        public GenericDataAccess()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //

        }

        #region trans

        //public void BeginTransaction()
        //{
        //    string dataProviderName = DBConfiguration.DbProviderName;
        //    string dataConnectionString = DBConfiguration.DbConnectionString;
        //    DbProviderFactory factory = DbProviderFactories.GetFactory(dataProviderName);
        //    dconn = factory.CreateConnection();
        //    dconn.ConnectionString = dataConnectionString;
        //    dconn.Open();
        //    tran = dconn.BeginTransaction();

        //}

        public void BeginTran()
        {
            this.dcomm = CreateCommand();
            this.dcomm.Connection.Open();
            this.tran = this.dcomm.Connection.BeginTransaction();
            this.dcomm.Transaction = this.tran;

        }

        public bool TranCommit()
        {

            try
            {
                if (this.tran != null)
                {
                    if (this.isError)
                    {
                        this.tran.Rollback();
                    }
                    else
                    {
                        this.tran.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                isError = false;
                throw ex;
            }
            finally
            {
                CloseTran();

            }


            return !isError;
        }

        private void CloseTran()
        {
            if (tran != null)
            {
                this.tran.Dispose();
                this.tran = null;
            }
            if (this.dcomm != null)
            {
                if (this.dcomm.Connection != null)
                {
                    this.dcomm.Connection.Close();
                    this.dcomm.Connection.Dispose();
                    this.dcomm.Connection = null;
                    this.dcomm.Dispose();
                    this.dcomm = null;
                }
            }

        }

        //public DbCommand CreateCommandTran()
        //{
        //    if (this.dconn == null || this.dtran == null)
        //        BeginTransaction();
        //    DbCommand comm = dconn.CreateCommand();
        //    comm.CommandType = CommandType.StoredProcedure;
        //    return comm;
        //}

        //public DbCommand CreateCommandTran(string sqlstr)
        //{
        //    if (this.dconn == null || this.dtran == null)
        //        BeginTransaction();
        //    DbCommand comm = dconn.CreateCommand();
        //    comm.CommandText = sqlstr;
        //    return comm;
        //}

        public DataTable ExecuteSelectCommandTran(DbCommand comm)
        {
            DataTable dt = null;
            try
            {

                //comm.Connection.Open();
                DbDataReader reader = comm.ExecuteReader();
                dt = new DataTable();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                isError = true;
                throw ex;
            }
            finally
            {

            }
            return dt;
        }
        public DataTable ExecuteSelectCommandTran(string proc)
        {
            dcomm.CommandType = CommandType.StoredProcedure;
            dcomm.CommandText = proc;
            return ExecuteSelectCommandTran(dcomm);
        }

        public DataTable ExecuteSelectCommandSqlTran(string sqlStr)
        {
            dcomm.CommandType = CommandType.Text;
            return ExecuteSelectCommandTran(dcomm);

        }

        public bool UpdateBySqlTran(string sqlStr, string[,] pram)
        {
            CleardComm();
            dcomm.CommandText = sqlStr;
            dcomm.CommandType = CommandType.Text;
            int count = (pram.Length / 4);
            for (int i = 0; i < count; i++)
            {
                DbParameter mypram = dcomm.CreateParameter();
                mypram.ParameterName = pram[i, 0].ToString();

                if (pram[i, 2].ToString().Trim().Equals("DbType.Int32"))
                {
                    mypram.Value = Convert.ToInt32(pram[i, 1].ToString());
                    mypram.DbType = DbType.Int32;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.Boolean"))
                {
                    mypram.Value = Convert.ToBoolean(pram[i, 1].ToString());
                    mypram.DbType = DbType.Boolean;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.DateTime") || pram[i, 2].ToString().Trim().Equals("DbType.Date"))
                {
                    mypram.Value = Convert.ToDateTime(pram[i, 1].ToString());
                    mypram.DbType = DbType.Date;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.Double"))
                {
                    mypram.Value = Convert.ToDouble(pram[i, 1].ToString());
                    mypram.DbType = DbType.Double;
                }
                else
                {
                    mypram.Value = pram[i, 1].ToString();
                    mypram.DbType = DbType.String;
                }



                if (pram[i, 3] != null && Convert.ToInt32(pram[i, 3]) > 0)
                {
                    mypram.Size = Convert.ToInt32(pram[i, 3].ToString());
                }
                dcomm.Parameters.Add(mypram);
            }
            //comm.Connection.Open();
            try
            {
                return dcomm.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                this.isError = true;
                Console.Write(ex.Source + " " + ex.Message);
                throw ex;
            }
            finally
            {

            }
            return false;
        }

        public bool UpdateBySqlTran(string sqlStr)
        {

            try
            {
                CleardComm();
                //comm.Connection.Open();
                dcomm.CommandType = CommandType.Text;
                dcomm.CommandText = sqlStr;
                int count = dcomm.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.isError = true;
                throw ex;
            }
            finally
            {

            }
            return false;
        }

        public DataTable QueryTran(string sqlStr, string[,] pram)
        {
            CleardComm();
            dcomm.CommandType = CommandType.Text;
            dcomm.CommandText = sqlStr;
            int count = (pram.Length / 4);
            for (int i = 0; i < count; i++)
            {
                DbParameter mypram = dcomm.CreateParameter();
                mypram.ParameterName = pram[i, 0].ToString();

                if (pram[i, 2].ToString().Trim().Equals("DbType.Int32"))
                {
                    mypram.Value = Convert.ToInt32(pram[i, 1].ToString());
                    mypram.DbType = DbType.Int32;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.Boolean"))
                {
                    mypram.Value = Convert.ToBoolean(pram[i, 1].ToString());
                    mypram.DbType = DbType.Boolean;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.DateTime") || pram[i, 2].ToString().Trim().Equals("DbType.Date"))
                {
                    mypram.Value = Convert.ToDateTime(pram[i, 1].ToString());
                    mypram.DbType = DbType.Date;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.Double"))
                {
                    mypram.Value = Convert.ToDouble(pram[i, 1].ToString());
                    mypram.DbType = DbType.Double;
                }
                else
                {
                    mypram.Value = pram[i, 1].ToString();
                    mypram.DbType = DbType.String;
                }



                if (pram[i, 3] != null && Convert.ToInt32(pram[i, 3]) > 0)
                {
                    mypram.Size = Convert.ToInt32(pram[i, 3].ToString());
                }
                dcomm.Parameters.Add(mypram);
            }
            //comm.Connection.Open();
            DataTable dt = new DataTable();
            try
            {
                dt.Load(dcomm.ExecuteReader());
            }
            catch (Exception ex)
            {
                this.isError = true;
                Console.Write(ex.Source + " " + ex.Message);
                throw ex;
            }
            finally
            {

            }
            return dt;
        }
        public DataTable ExecuteSelectCommandTran(string proc, string[,] pram)
        {
            CleardComm();
            dcomm.CommandText = proc;
            int count = (pram.Length / 4);
            for (int i = 0; i < count; i++)
            {
                DbParameter mypram = dcomm.CreateParameter();
                mypram.ParameterName = pram[i, 0].ToString();

                if (pram[i, 2].ToString().Trim().Equals("DbType.Int32"))
                {
                    mypram.Value = Convert.ToInt32(pram[i, 1].ToString());
                    mypram.DbType = DbType.Int32;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.Boolean"))
                {
                    mypram.Value = Convert.ToBoolean(pram[i, 1].ToString());
                    mypram.DbType = DbType.Boolean;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.DateTime") || pram[i, 2].ToString().Trim().Equals("DbType.Date"))
                {
                    mypram.Value = Convert.ToDateTime(pram[i, 1].ToString());
                    mypram.DbType = DbType.Date;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.Double"))
                {
                    mypram.Value = Convert.ToDouble(pram[i, 1].ToString());
                    mypram.DbType = DbType.Double;
                }
                else
                {
                    mypram.Value = pram[i, 1].ToString();
                    mypram.DbType = DbType.String;
                }



                if (pram[i, 3] != null && Convert.ToInt32(pram[i, 3]) > 0)
                {
                    mypram.Size = Convert.ToInt32(pram[i, 3].ToString());
                }
                dcomm.Parameters.Add(mypram);
            }
            //comm.Connection.Open();
            DataTable dt = new DataTable();
            try
            {
                dt.Load(dcomm.ExecuteReader());
            }
            catch (Exception ex)
            {
                this.isError = true;
                Console.Write(ex.Source + " " + ex.Message);
                throw ex;
            }
            finally
            {

            }
            return dt;
        }
        public bool UpdateDataCommandTran(string proc, string[,] pram)
        {
            CleardComm();
            dcomm.CommandType = CommandType.StoredProcedure;
            dcomm.CommandText = proc;
            int count = (pram.Length / 4);
            for (int i = 0; i < count; i++)
            {
                DbParameter mypram = dcomm.CreateParameter();
                mypram.ParameterName = pram[i, 0].ToString();

                if (pram[i, 2].ToString().Trim().Equals("DbType.Int32"))
                {
                    mypram.Value = Convert.ToInt32(pram[i, 1].ToString());
                    mypram.DbType = DbType.Int32;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.Boolean"))
                {
                    mypram.Value = Convert.ToBoolean(pram[i, 1].ToString());
                    mypram.DbType = DbType.Boolean;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.DateTime") || pram[i, 2].ToString().Trim().Equals("DbType.Date"))
                {
                    mypram.Value = Convert.ToDateTime(pram[i, 1].ToString());
                    mypram.DbType = DbType.Date;
                }
                else
                {
                    mypram.Value = pram[i, 1].ToString();
                    mypram.DbType = DbType.String;
                }



                if (pram[i, 3] != null && Convert.ToInt32(pram[i, 3]) > 0)
                {
                    mypram.Size = Convert.ToInt32(pram[i, 3].ToString());
                }
                dcomm.Parameters.Add(mypram);
            }
            //comm.Connection.Open();
            bool ret = false;
            try
            {
                ret = (dcomm.ExecuteNonQuery() > 0);
            }
            catch (Exception ex)
            {
                this.isError = true;
                throw ex;
            }
            finally
            {

            }
            return ret;

        }

        #endregion trans

        public static DbCommand CreateCommand()
        {
            string dataProviderName = DBConfiguration.DbProviderName;
            string dataConnectionString = DBConfiguration.DbConnectionString;
            DbProviderFactory factory = DbProviderFactories.GetFactory(dataProviderName);
            DbConnection conn = factory.CreateConnection();
            conn.ConnectionString = dataConnectionString;
            DbCommand comm = conn.CreateCommand();
            comm.CommandType = CommandType.StoredProcedure;
            return comm;
        }

        public static DbCommand CreateCommand(string sqlstr)
        {
            string dataProviderName = DBConfiguration.DbProviderName;
            string dataConnectionString = DBConfiguration.DbConnectionString;
            DbProviderFactory factory = DbProviderFactories.GetFactory(dataProviderName);
            DbConnection conn = factory.CreateConnection();
            conn.ConnectionString = dataConnectionString;
            DbCommand comm = conn.CreateCommand();
            comm.CommandText = sqlstr;
            return comm;
        }

        public static DataTable ExecuteSelectCommand(DbCommand comm)
        {
            DataTable dt = null;
            try
            {
                comm.Connection.Open();
                DbDataReader reader = comm.ExecuteReader();
                dt = new DataTable();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                comm.Connection.Close();
            }
            return dt;
        }
        public static DataTable ExecuteSelectCommand(string proc)
        {
            DbCommand comm = CreateCommand();
            comm.CommandText = proc;
            return ExecuteSelectCommand(comm);
        }

        public static DataTable ExecuteSelectCommandSql(string sqlStr)
        {
            DbCommand comm = CreateCommand(sqlStr);
            return ExecuteSelectCommand(comm);

        }

        public static bool UpdateBySql(string sqlStr, string[,] pram)
        {//
            DbCommand comm = CreateCommand(sqlStr);
            int count = (pram.Length / 4);
            for (int i = 0; i < count; i++)
            {
                DbParameter mypram = comm.CreateParameter();
                mypram.ParameterName = pram[i, 0].ToString();

                if (pram[i, 2].ToString().Trim().Equals("DbType.Int32"))
                {
                    mypram.Value = Convert.ToInt32(pram[i, 1].ToString());
                    mypram.DbType = DbType.Int32;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.Boolean"))
                {
                    mypram.Value = Convert.ToBoolean(pram[i, 1].ToString());
                    mypram.DbType = DbType.Boolean;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.DateTime") || pram[i, 2].ToString().Trim().Equals("DbType.Date"))
                {
                    mypram.Value = Convert.ToDateTime(pram[i, 1].ToString());
                    mypram.DbType = DbType.Date;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.Double"))
                {
                    mypram.Value = Convert.ToDouble(pram[i, 1].ToString());
                    mypram.DbType = DbType.Double;
                }
                else
                {
                    mypram.Value = pram[i, 1].ToString();
                    mypram.DbType = DbType.String;
                }



                if (pram[i, 3] != null && Convert.ToInt32(pram[i, 3]) > 0)
                {
                    mypram.Size = Convert.ToInt32(pram[i, 3].ToString());
                }
                comm.Parameters.Add(mypram);
            }
            comm.Connection.Open();
            try
            {
                return comm.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Source + " " + ex.Message);
                throw ex;
            }
            finally
            {
                comm.Connection.Close();
            }
            return false;
        }

        public static bool UpdateBySql(string sqlStr)
        {
            DbCommand comm = CreateCommand(sqlStr);
            try
            {
                comm.Connection.Open();
                int count = comm.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                comm.Connection.Close();
            }
            return false;
        }

        public static DataTable Query(string sqlStr, string[,] pram)
        {
            DbCommand comm = CreateCommand(sqlStr);
            int count = (pram.Length / 4);
            for (int i = 0; i < count; i++)
            {
                DbParameter mypram = comm.CreateParameter();
                mypram.ParameterName = pram[i, 0].ToString();

                if (pram[i, 2].ToString().Trim().Equals("DbType.Int32"))
                {
                    mypram.Value = Convert.ToInt32(pram[i, 1].ToString());
                    mypram.DbType = DbType.Int32;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.Boolean"))
                {
                    mypram.Value = Convert.ToBoolean(pram[i, 1].ToString());
                    mypram.DbType = DbType.Boolean;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.DateTime") || pram[i, 2].ToString().Trim().Equals("DbType.Date"))
                {
                    mypram.Value = Convert.ToDateTime(pram[i, 1].ToString());
                    mypram.DbType = DbType.Date;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.Double"))
                {
                    mypram.Value = Convert.ToDouble(pram[i, 1].ToString());
                    mypram.DbType = DbType.Double;
                }
                else
                {
                    mypram.Value = pram[i, 1].ToString();
                    mypram.DbType = DbType.String;
                }



                if (pram[i, 3] != null && Convert.ToInt32(pram[i, 3]) > 0)
                {
                    mypram.Size = Convert.ToInt32(pram[i, 3].ToString());
                }
                comm.Parameters.Add(mypram);
            }
            comm.Connection.Open();
            DataTable dt = new DataTable();
            try
            {
                dt.Load(comm.ExecuteReader());
            }
            catch (Exception ex)
            {
                Console.Write(ex.Source + " " + ex.Message);
                throw ex;
            }
            finally
            {
                comm.Connection.Close();
            }
            return dt;
        }
        public static DataTable ExecuteSelectCommand(string proc, string[,] pram)
        {
            DbCommand comm = CreateCommand();
            comm.CommandText = proc;
            int count = (pram.Length / 4);
            for (int i = 0; i < count; i++)
            {
                DbParameter mypram = comm.CreateParameter();
                mypram.ParameterName = pram[i, 0].ToString();

                if (pram[i, 2].ToString().Trim().Equals("DbType.Int32"))
                {
                    mypram.Value = Convert.ToInt32(pram[i, 1].ToString());
                    mypram.DbType = DbType.Int32;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.Boolean"))
                {
                    mypram.Value = Convert.ToBoolean(pram[i, 1].ToString());
                    mypram.DbType = DbType.Boolean;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.DateTime") || pram[i, 2].ToString().Trim().Equals("DbType.Date"))
                {
                    mypram.Value = Convert.ToDateTime(pram[i, 1].ToString());
                    mypram.DbType = DbType.Date;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.Double"))
                {
                    mypram.Value = Convert.ToDouble(pram[i, 1].ToString());
                    mypram.DbType = DbType.Double;
                }
                else
                {
                    mypram.Value = pram[i, 1].ToString();
                    mypram.DbType = DbType.String;
                }



                if (pram[i, 3] != null && Convert.ToInt32(pram[i, 3]) > 0)
                {
                    mypram.Size = Convert.ToInt32(pram[i, 3].ToString());
                }
                comm.Parameters.Add(mypram);
            }
            comm.Connection.Open();
            DataTable dt = new DataTable();
            try
            {
                dt.Load(comm.ExecuteReader());
            }
            catch (Exception ex)
            {
                Console.Write(ex.Source + " " + ex.Message);
                throw ex;
            }
            finally
            {
                comm.Connection.Close();
            }
            return dt;
        }
        public static bool UpdateDataCommand(string proc, string[,] pram)
        {
            DbCommand comm = CreateCommand();
            comm.CommandText = proc;
            int count = (pram.Length / 4);
            for (int i = 0; i < count; i++)
            {
                DbParameter mypram = comm.CreateParameter();
                mypram.ParameterName = pram[i, 0].ToString();

                if (pram[i, 2].ToString().Trim().Equals("DbType.Int32"))
                {
                    mypram.Value = Convert.ToInt32(pram[i, 1].ToString());
                    mypram.DbType = DbType.Int32;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.Boolean"))
                {
                    mypram.Value = Convert.ToBoolean(pram[i, 1].ToString());
                    mypram.DbType = DbType.Boolean;
                }
                else if (pram[i, 2].ToString().Trim().Equals("DbType.DateTime") || pram[i, 2].ToString().Trim().Equals("DbType.Date"))
                {
                    mypram.Value = Convert.ToDateTime(pram[i, 1].ToString());
                    mypram.DbType = DbType.Date;
                }
                else
                {
                    mypram.Value = pram[i, 1].ToString();
                    mypram.DbType = DbType.String;
                }



                if (pram[i, 3] != null && Convert.ToInt32(pram[i, 3]) > 0)
                {
                    mypram.Size = Convert.ToInt32(pram[i, 3].ToString());
                }
                comm.Parameters.Add(mypram);
            }
            comm.Connection.Open();
            bool ret = false;
            try
            {
                ret = (comm.ExecuteNonQuery() > 0);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Source + " " + ex.Message);
                throw ex;
            }
            finally
            {
                comm.Connection.Close();
            }
            return ret;

        }


        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            try
            {
                CleardComm();

                dcomm.CommandType = CommandType.Text;
                dcomm.CommandText = SQLString;
                foreach (SqlParameter sqlp in cmdParms)
                {
                    DbParameter dbp = dcomm.CreateParameter();
                    dbp.ParameterName = sqlp.ParameterName;
                    dbp.Value = sqlp.Value;
                    switch (sqlp.SqlDbType)
                    {
                        case SqlDbType.VarChar:
                            dbp.DbType = DbType.String;
                            break;
                        case SqlDbType.Date:
                            dbp.DbType = DbType.Date;
                            break;
                        case SqlDbType.DateTime:
                            dbp.DbType = DbType.DateTime;
                            break;
                        case SqlDbType.DateTime2:
                            dbp.DbType = DbType.DateTime2;
                            break;
                        case SqlDbType.Text:
                            dbp.DbType = DbType.String;
                            break;
                        case SqlDbType.Time:
                            dbp.DbType = DbType.Time;
                            break;
                        case SqlDbType.Decimal:
                            dbp.DbType = DbType.Decimal;
                            break;
                        case SqlDbType.Float:
                            dbp.DbType = DbType.Decimal;
                            break;
                        case SqlDbType.Money:
                            dbp.DbType = DbType.Decimal;
                            break;
                        case SqlDbType.Int:
                            dbp.DbType = DbType.Int32;
                            break;
                        case SqlDbType.Bit:
                            dbp.DbType = DbType.Boolean;
                            break;

                    }

                    //if (sqlp.SqlDbType == SqlDbType.VarChar || sqlp.SqlDbType == SqlDbType.Char)
                    //{
                    dbp.Size = sqlp.Size;
                    //}

                    dcomm.Parameters.Add(dbp);
                }
                object obj = dcomm.ExecuteScalar();
                dcomm.Parameters.Clear();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    isError = true;
                    return null;
                }
                else
                {
                    isError = false;
                    return obj;
                }
            }
            catch (Exception e)
            {
                isError = true;
                throw e;
                return null;
            }
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            try
            {
                CleardComm();

                dcomm.CommandType = CommandType.Text;
                dcomm.CommandText = SQLString;

                foreach (SqlParameter sqlp in cmdParms)
                {
                    DbParameter dbp = dcomm.CreateParameter();
                    dbp.ParameterName = sqlp.ParameterName;
                    dbp.Value = sqlp.Value;
                    switch (sqlp.SqlDbType)
                    {
                        case SqlDbType.VarChar:
                            dbp.DbType = DbType.String;
                            break;
                        case SqlDbType.Date:
                            dbp.DbType = DbType.Date;
                            break;
                        case SqlDbType.DateTime:
                            dbp.DbType = DbType.DateTime;
                            break;
                        case SqlDbType.DateTime2:
                            dbp.DbType = DbType.DateTime2;
                            break;
                        case SqlDbType.Text:
                            dbp.DbType = DbType.String;
                            break;
                        case SqlDbType.Time:
                            dbp.DbType = DbType.Time;
                            break;
                        case SqlDbType.Decimal:
                            dbp.DbType = DbType.Decimal;
                            break;
                        case SqlDbType.Float:
                            dbp.DbType = DbType.Decimal;
                            break;
                        case SqlDbType.Money:
                            dbp.DbType = DbType.Decimal;
                            break;
                        case SqlDbType.Int:
                            dbp.DbType = DbType.Int32;
                            break;
                        case SqlDbType.Bit:
                            dbp.DbType = DbType.Boolean;
                            break;

                    }

                    //if (sqlp.SqlDbType == SqlDbType.VarChar || sqlp.SqlDbType == SqlDbType.Char)
                    //{
                    dbp.Size = sqlp.Size;
                    //}

                    dcomm.Parameters.Add(dbp);
                }

                int rows = dcomm.ExecuteNonQuery();
                dcomm.Parameters.Clear();

                return rows;
            }
            catch (Exception e)
            {
                isError = true;
                throw e;
                return 0;
            }
        }

        private void CleardComm()
        {
            dcomm.CommandText = "";
            dcomm.Parameters.Clear();
        }
    }
}