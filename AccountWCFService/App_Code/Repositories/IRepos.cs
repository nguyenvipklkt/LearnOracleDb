using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

[ServiceContract]
public interface IRepos
{

    [OperationContract]
    OracleCommand CallStoredProcedure(string procedureName, IDictionary<string, object> parameters);
    object ExcuteQueryByScalar(string query);
    OracleDataReader ExcuteQueryByReader(string query);
    object ExcuteQueryByNonQuery(string query);
    object ExcuteQueryByStream(string query);
}