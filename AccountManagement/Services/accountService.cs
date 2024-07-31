using AccountManagement.Repositories;
using AccountManagement.Requests;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace AccountManagement.Services
{
    public class accountService
    {
        private readonly AccountRepository _accountRepository;
        public accountService(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public object CreateAccount(createAccountRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.procedureName))
            {
                return new
                {
                    messeage = "Procedure name does not exist.",
                };
            }
            if (string.IsNullOrWhiteSpace(request.maTK) || string.IsNullOrWhiteSpace(request.matKhau) ||
                string.IsNullOrWhiteSpace(request.tenDangNhap) || string.IsNullOrWhiteSpace(request.capBac) || string.IsNullOrWhiteSpace(request.maKH))
            {
                return new
                {
                    messeage = "Missing parameters.",
                };
            }

            try
            {
                var output = new OracleParameter("P-EXISTS", OracleDbType.Int32, ParameterDirection.Output);
                var parameters = new Dictionary<string, object>
        {
            { "P_MATK", request.maTK },
            { "P_TENDANGNHAP", request.tenDangNhap },
            { "P_MATKHAU", request.matKhau },
            { "P_CAPBAC", request.capBac },
            { "P_MAKH", request.maKH },
            { "P-EXISTS", output }
        };

                using (var command = _accountRepository.CallStoredProcedure(request.procedureName, parameters))
                {
                    var existsDecimal = (OracleDecimal)output.Value;
                    int exists = existsDecimal.IsNull ? 0 : existsDecimal.ToInt32();

                    if (exists == 1)
                    {
                        return new
                        {
                            messeage = "Account rr customer already exists.",
                        };
                    }
                }

                return new
                {
                    message = "Create successful",
                    result = output.Value
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    message = ex.Message,
                };
            }
        }

        public object GetAllAccounts(string procedureName)
        {
            var accounts = new List<Account>();

            try
            {
                var output = new OracleParameter("accounts", OracleDbType.RefCursor, ParameterDirection.Output);
                var parameters = new Dictionary<string, object>
        {
            { "accounts", output }
        };

                using (var command = _accountRepository.CallStoredProcedure(procedureName, parameters))
                {
                    using (var reader = ((OracleRefCursor)command.Parameters["accounts"].Value).GetDataReader())
                    {
                        while (reader.Read())
                        {
                            var account = new Account
                            {
                                maTK = reader["MAKH"].ToString(),
                                capBac = reader["TENKH"].ToString(),
                                tenDangNhap = reader["EMAIL"].ToString(),
                                matKhau = reader["MATK"].ToString()
                            };
                            accounts.Add(account);
                        }
                    }
                }

                return accounts;
            }
            catch (Exception ex)
            {
                return new
                {
                    message = ex.Message,
                };
            }
        }


        public class Account
        {
            public string maTK { get; set; }
            public string capBac { get; set; }
            public string tenDangNhap { get; set; }
            public string matKhau { get; set; }
            public string maKH { get; set; }
        }

    }
}
