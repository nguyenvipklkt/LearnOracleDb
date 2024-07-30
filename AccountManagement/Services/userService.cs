using AccountManagement.Repositories;
using AccountManagement.Requests;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace AccountManagement.Services
{
    public class userService
    {
        private readonly AccountRepository _accountRepository;
        public userService(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public object CreateCustomer(registerRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.procedureName))
                {
                    throw new Exception("Procedure name does not exist.");
                }
                if (string.IsNullOrWhiteSpace(request.maKhachHang) || string.IsNullOrWhiteSpace(request.tenKhachHang) || string.IsNullOrWhiteSpace(request.email) || string.IsNullOrWhiteSpace(request.password))
                {
                    throw new Exception("Misssing params.");
                }
                var output = new OracleParameter("r_exists", OracleDbType.RefCursor, ParameterDirection.Output);
                var parameters = new Dictionary<string, object>
                {
                    { "maKH", request.maKhachHang },
                    { "tenKH", request.tenKhachHang },
                    { "email", request.email },
                    { "password", request.password },
                    { "r_exists", output }
                };
                var command = _accountRepository.CallStoredProcedure(request.procedureName, parameters);
                var reader = ((OracleRefCursor)output.Value).GetDataReader();
                int exists = 0;
                if (reader.Read())
                {
                    exists = reader.GetInt32(0);
                }
                if (exists == 1)
                {
                    throw new Exception("User existed.");
                };
                return new {
                    message = "Create successfull",
                    output.Value
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GetUser(string procedureName)
        {
            var users = new List<User>();
            var output = new OracleParameter("users", OracleDbType.RefCursor, ParameterDirection.Output);
            var parameters = new Dictionary<string, object>
        {
            { "users", output }
        };
            var command = _accountRepository.CallStoredProcedure(procedureName, parameters);
            var reader = ((OracleRefCursor)command.Parameters["users"].Value).GetDataReader();
            while (reader.Read())
            {
                var user = new User
                {
                    maKH = reader["MAKH"].ToString(),
                    tenKH = reader["TENKH"].ToString(),
                    email = reader["EMAIL"].ToString(),
                    password = reader["MATK"].ToString()
                };
                users.Add(user);
            }
            return users;

        }

        public class User
        {
            public string maKH { get; set; }
            public string tenKH { get; set; }
            public string email { get; set; }
            public string password { get; set; }
        }

    }
}
