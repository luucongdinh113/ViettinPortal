using Mita.Business.Helpers;
using System.Data.Common;

namespace Mita.Business.BusinessServices
{
    public class ConnectionService
    {
        public string EncryptUserId { get; set; }

        public string EncryptPassword { get; set; }

        public string EntityConnectionString { get; set; }

        //public string SqlConnectionString { get; set; }

        private static DbConnection DbConnection { get; set; }

        public DbConnection getEntityConnection()
        {
            if (DbConnection == null)
            {
                string connectionString = EntityConnectionString;
                connectionString = connectionString.Replace(":userId", CommonUtils.Decrypt(EncryptUserId));
                connectionString = connectionString.Replace(":password", CommonUtils.Decrypt(EncryptPassword));

                //DbConnection = new System.Data.EntityClient.EntityConnection(connectionString);//disabl
                DbConnection.Open();
            }

            return DbConnection;
        }

        //public SqlConnection getSqlConnection()
        //{
        //    string connectionString = SqlConnectionString;
        //    connectionString = connectionString.Replace(":userId", CommonUtils.Decrypt(EncryptUserId));
        //    connectionString = connectionString.Replace(":password", CommonUtils.Decrypt(EncryptPassword));

        //    return new SqlConnection(connectionString);
        //}
    }
}
