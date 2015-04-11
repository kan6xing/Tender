using System;
using System.Data;
using System.Configuration;
/// <summary>
/// DBConfiguration 的摘要说明
/// </summary>
/// 
namespace JinxiaocunApp
{
    public static class DBConfiguration
    {

        private static string dbConnectionString;
        private static string dbProviderName;
        private static string connectionStrings = ConfigurationManager.AppSettings["connectionProvider"];

        static DBConfiguration()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            dbConnectionString = ConfigurationManager.ConnectionStrings[connectionStrings].ConnectionString;
            dbProviderName = ConfigurationManager.ConnectionStrings[connectionStrings].ProviderName;

        }

        public static string DbConnectionString
        {
            get
            {
                return dbConnectionString;
            }
        }

        public static string DbProviderName
        {
            get
            {
                return dbProviderName;
            }
        }
    }
}