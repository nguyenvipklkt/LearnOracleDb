using AccountManagement.Database;
using AccountManagement.Repositories;
using AccountManagement.Requests;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Data.Common;
using System.Reflection.PortableExecutable;

namespace AccountManagement.Services
{
    public class accountService
    {
        private readonly AccountRepository _accountRepository;
        private readonly OracleConnection _connection;
        public accountService(DatabaseContext databaseContext, OracleConnection connection)
        {
            _accountRepository = new AccountRepository(databaseContext, connection);
            _connection = connection;
        }

        public object CreateAccount(createAccountRequest request)
        {
            string procedureName = "tao_tk";
            if (string.IsNullOrWhiteSpace(request.maTK) || string.IsNullOrWhiteSpace(request.matKhau) ||
                string.IsNullOrWhiteSpace(request.tenDangNhap) || string.IsNullOrWhiteSpace(request.capBac))
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
            { "P_CAPBAC", request.capBac },
            { "P_TENDANGNHAP", request.tenDangNhap },
            { "P_MATKHAU", request.matKhau },
            { "P-EXISTS", output }
        };

                using (var command = _accountRepository.CallStoredProcedure(procedureName, parameters))
                {
                    var existsDecimal = (OracleDecimal)output.Value;
                    int exists = existsDecimal.IsNull ? 0 : existsDecimal.ToInt32();

                    if (exists == 1)
                    {
                        return new
                        {
                            messeage = "Account already exists.",
                        };
                    }
                }

                return new
                {
                    message = "Create successful",
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

        public object GetAccount(string maTK)
        {
            try
            {
                var query = $"SELECT * FROM taikhoan where matk = '{maTK}'";
                var res = _accountRepository.ExcuteQueryByReader(query);
                using (var reader = (OracleDataReader)res)
                {

                    if (reader.Read())
                    {
                        var matk = reader.GetString(0);
                        var capbac = reader.GetString(1);
                        var tendangnhap = reader.GetString(2);
                        _connection.Close();
                        return new
                        {
                            message = "Get success",
                            account = new { matk, capbac, tendangnhap },
                            Code = "Ok"
                        };
                    }
                }
                _connection.Close();
                return new
                {
                    message = "Create successful",
                    result = res
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

        public object UpdateAccount(updateAccountRequest request)
        {
            string procedureName = "update_tk";
            if (string.IsNullOrWhiteSpace(request.maTK) ||
                string.IsNullOrWhiteSpace(request.tenDangNhap) || string.IsNullOrWhiteSpace(request.capBac))
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
            { "P_CAPBAC", request.capBac },
            { "P_TENDANGNHAP", request.tenDangNhap },
            { "P-EXISTS", output }
        };

                using (var command = _accountRepository.CallStoredProcedure(procedureName, parameters))
                {
                    var existsDecimal = (OracleDecimal)output.Value;
                    int exists = existsDecimal.IsNull ? 0 : existsDecimal.ToInt32();

                    if (exists == 0)
                    {
                        return new
                        {
                            messeage = "Account does not exists.",
                        };
                    }
                }

                return new
                {
                    message = "Create successful",
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

        public class Result
        {
            public int Code { get; set; }
            public string Status { get; set; }
            public object Data { get; set; }

        }
    }
}
