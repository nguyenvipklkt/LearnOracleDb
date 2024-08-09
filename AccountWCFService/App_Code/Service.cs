using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;

public class Service : Repos, IService
{
    private readonly Repos _repos;
    public Service()
    {
        _repos = new Repos();
    }
    public AccountDto GetAccountById(string matk)
    {
        try
        {
            var query = "SELECT * FROM taikhoan where matk = " + "'" + matk + "'";
            var res = _repos.ExcuteQueryByReader(query);
            
            AccountDto account = new AccountDto();
            using (var reader = res)
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
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
