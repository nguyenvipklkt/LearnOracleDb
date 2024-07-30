

using AccountManagement.Database;
using Microsoft.VisualBasic.FileIO;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection;
using System.Text;

namespace AccountManagement.Repositories
{
    public class BaseRepository
    {
        protected DatabaseContext dbContext { get; set; }
        protected OracleConnection _connection { get; set; }

        public BaseRepository(DatabaseContext databaseContext, OracleConnection connection)
        {
            dbContext = databaseContext;
            _connection = connection;
        }

        public OracleCommand CallStoredProcedure(string procedureName, IDictionary<string, object> parameters)
        {
            var command = new OracleCommand(procedureName, _connection);
            command.CommandType = CommandType.StoredProcedure;

            foreach (var parameter in parameters)
            {
                if (parameter.Value is OracleParameter oracleParameter)
                {
                    command.Parameters.Add(oracleParameter);
                }
                else
                {
                    command.Parameters.Add(new OracleParameter(parameter.Key, parameter.Value));
                }
            }

            dbContext.OpenConnection();
            command.ExecuteNonQuery();
            dbContext.CloseConnection();
            return command;

        }
    }
}
