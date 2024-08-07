

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
            _connection.Open();
            if (_connection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException("Connection must be open for this operation.");
            }

            using (var command = new OracleCommand(procedureName, _connection))
            {
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

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception when calling stored procedure: {ex.Message}");
                    throw;
                }

                return command;
            }
        }

        public object ExcuteQueryByScalar(string query)
        {
            try
            {

                _connection.Open();
                using (var command = new OracleCommand(query, _connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    var result = command.ExecuteScalar();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public object ExcuteQueryByReader(string query)
        {
            try
            {
                _connection.Open();
                using (var command = new OracleCommand(query, _connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    var result = command.ExecuteReader();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public object ExcuteQueryByNonQuery(string query)
        {
            try
            {

                _connection.Open();

                using (var command = new OracleCommand(query, _connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    var result = command.ExecuteNonQuery();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public object ExcuteQueryByStream(string query)
        {
            try
            {

                _connection.Open();

                using (var command = new OracleCommand(query, _connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    var result = command.ExecuteStream();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
