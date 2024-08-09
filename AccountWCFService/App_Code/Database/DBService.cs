using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Reflection;

public class DBService : IDBService
{
    private string connectionString = ConfigurationManager.ConnectionStrings["TEST_DEV"].ConnectionString;
    private OracleConnection _connection;
    public DBService()
    {
        var connectionString = ConfigurationManager.ConnectionStrings["TEST_DEV"].ConnectionString;
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException("Connection string is null or empty.");
        }
        try
        {
            _connection = new OracleConnection(connectionString);
            if (_connection == null)
            {
                throw new Exception("Failed to initialize OracleConnection.");
            }
        }
        catch (TargetInvocationException ex)
        {
            Console.WriteLine("TargetInvocationException:" + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
            }
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
            }
            throw;
        }
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
            Console.WriteLine("Exception when opening connection: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
            }
            throw;
        }
    }

    public void CloseConnection()
    {
        try
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception when closing connection: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
            }
            throw;
        }
    }

    public void Dispose()
    {
        try
        {
            _connection.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception when disposing connection: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
            }
            throw;
        }
    }
}
