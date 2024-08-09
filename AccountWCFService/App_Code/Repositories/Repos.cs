using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data;

public class Repos : DBService, IRepos
{
    private string connectionString = ConfigurationManager.ConnectionStrings["TEST_DEV"].ConnectionString;
    private readonly OracleConnection _connection;
    DBService _dBService;
    public Repos()
    {
        _connection = new OracleConnection(connectionString);
        _dBService = new DBService();
    }
    public OracleCommand CallStoredProcedure(string procedureName, IDictionary<string, object> parameters)
    {
        _dBService.OpenConnection();
        if (_connection.State != ConnectionState.Open)
        {
            throw new InvalidOperationException("Connection must be open for this operation.");
        }

        using (var command = new OracleCommand(procedureName, _connection))
        {
            command.CommandType = CommandType.StoredProcedure;

            foreach (var parameter in parameters)
            {
                OracleParameter oracleParameter = parameter.Value as OracleParameter;
                if (oracleParameter != null)
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
                _dBService.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception when calling stored procedure: " + ex.Message);
                throw;
            }

            return command;
        }
    }

    public object ExcuteQueryByScalar(string query)
    {
        try
        {

            _dBService.OpenConnection();
            using (var command = new OracleCommand(query, _connection))
            {
                command.CommandType = System.Data.CommandType.Text;
                var result = command.ExecuteScalar();
                _dBService.CloseConnection();
                return result;
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public OracleDataReader ExcuteQueryByReader(string query)
    {
        try
        {
            //_dBService.OpenConnection();
            _connection.Open();
            using (var command = new OracleCommand(query, _connection))
            {
                command.CommandType = System.Data.CommandType.Text;
                var result = command.ExecuteReader();
                _dBService.CloseConnection();
                return result;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception when calling stored procedure: " + ex.Message);
            throw;
        }
    }
    public object ExcuteQueryByNonQuery(string query)
    {
        try
        {
            _dBService.OpenConnection();
            using (var command = new OracleCommand(query, _connection))
            {
                command.CommandType = System.Data.CommandType.Text;
                var result = command.ExecuteNonQuery();
                _dBService.CloseConnection();
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
            _dBService.OpenConnection();

            using (var command = new OracleCommand(query, _connection))
            {
                command.CommandType = System.Data.CommandType.Text;
                var result = command.ExecuteStream();
                _dBService.CloseConnection();
                return result;
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
