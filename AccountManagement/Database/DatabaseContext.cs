using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace AccountManagement.Database
{
    public class DatabaseContext
    {
        private readonly OracleConnection _connection;

        public DatabaseContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new OracleConnection(connectionString);
        }

        public void OpenConnection()
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

    }
}
