using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data.Common;


public class Service : IService
{
    private string connectionString = ConfigurationManager.ConnectionStrings["TEST_DEV"].ConnectionString;
    private readonly OracleConnection _connection;
    public Service()
    {
        _connection = new OracleConnection(connectionString);
    }

    public bool CreateAccountById(string matk)
    {
        try
        {
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public AccountDto GetAccountById(string matk)
    {
        try
        {
            var query = "SELECT * FROM taikhoan where matk = " + "'" + matk + "'";
            _connection.Open();
            using (var command = new OracleCommand(query, _connection))
            {
                command.CommandType = System.Data.CommandType.Text;
                var result = command.ExecuteReader();
                _connection.Close();    
                AccountDto account = new AccountDto();
                using (var reader = result)
                {
                    if (reader.Read())
                    {
                        account.Matk = reader["matk"].ToString();
                        account.Capbac = reader["capbac"].ToString();
                        account.Tendangnhap = reader["tendangnhap"].ToString();
                        account.Matkhau = reader["matkhau"].ToString();
                    }
                    return account;
                }
            }

            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
