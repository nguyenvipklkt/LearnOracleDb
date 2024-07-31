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
            if (string.IsNullOrWhiteSpace(request.procedureName))
            {
                throw new ArgumentException("Procedure name does not exist.");
            }
            if (string.IsNullOrWhiteSpace(request.maKhachHang) || string.IsNullOrWhiteSpace(request.tenKhachHang) ||
                string.IsNullOrWhiteSpace(request.email) || string.IsNullOrWhiteSpace(request.password))
            {
                throw new ArgumentException("Missing parameters.");
            }

            try
            {
                var output = new OracleParameter("r_exists", OracleDbType.Int32, ParameterDirection.Output);
                var parameters = new Dictionary<string, object>
        {
            { "maKH", request.maKhachHang },
            { "tenKH", request.tenKhachHang },
            { "email", request.email },
            { "password", request.password },
            { "r_exists", output }
        };

                using (var command = _accountRepository.CallStoredProcedure(request.procedureName, parameters))
                {
                    int exists = Convert.ToInt32(output.Value);

                    if (exists == 1)
                    {
                        throw new Exception("User already exists.");
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
                // Ghi log hoặc xử lý ngoại lệ cụ thể
                throw new Exception("An error occurred while creating customer: " + ex.Message, ex);
            }
        }

        public object GetUser(string procedureName)
        {
            var users = new List<User>();

            try
            {
                var output = new OracleParameter("users", OracleDbType.RefCursor, ParameterDirection.Output);
                var parameters = new Dictionary<string, object>
        {
            { "users", output }
        };

                using (var command = _accountRepository.CallStoredProcedure(procedureName, parameters))
                {
                    using (var reader = ((OracleRefCursor)command.Parameters["users"].Value).GetDataReader())
                    {
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
                    }
                }

                return users;
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý ngoại lệ cụ thể
                throw new Exception("An error occurred while retrieving users: " + ex.Message, ex);
            }
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
