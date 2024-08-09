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
public interface IDBService
{

    [OperationContract]
    void OpenConnection();
    void CloseConnection();
    void Dispose();
}