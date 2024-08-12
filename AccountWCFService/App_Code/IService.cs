using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

[ServiceContract]
public interface IService
{

    [OperationContract]
    AccountDto GetAccountById(string matk);
    [OperationContract]
    bool CreateAccountById(string matk);
}

[DataContract]
public class AccountDto
{
    [DataMember]
    public string Matk { get; set; }

    [DataMember]
    public string Capbac { get; set; }

    [DataMember]
    public string Tendangnhap { get; set; }

    [DataMember]
    public string Matkhau { get; set; }
}