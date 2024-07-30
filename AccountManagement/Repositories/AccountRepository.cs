

using AccountManagement.Database;
using Microsoft.VisualBasic.FileIO;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection;
using System.Text;

namespace AccountManagement.Repositories
{
    public class AccountRepository : BaseRepository
    {
        public AccountRepository(DatabaseContext databaseContext, OracleConnection connection) : base(databaseContext, connection)
        {
        }
    }
}
