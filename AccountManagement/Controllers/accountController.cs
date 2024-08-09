using AccountManagement.Requests;
using AccountManagement.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServiceReference1;
using System;
using System.Data;
using System.Xml.Linq;
using System.Xml;
using static AccountManagement.Services.accountService;

namespace AccountManagement.Controllers
{
    public class accountController
    {
        private readonly accountService _accountService;
        public accountController(accountService accountService)
        {
            this._accountService = accountService;
        }

        [HttpPost("create-account")]
        public object CreateUser([FromBody] createAccountRequest request)
        {
            try
            {
                var definition = _accountService.CreateAccount(request);
                return new
                {
                    Data = definition
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

        [HttpPost("update-account")]
        public object UdateUser([FromBody] updateAccountRequest request)
        {
            try
            {
                var definition = _accountService.UpdateAccount(request);
                return new
                {
                    Data = definition
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

        [HttpGet("get-account")]
        public object GetUser(string maTk)
        {
            try
            {
                var definition = _accountService.GetAccount(maTk);
                return new
                {
                    Data = definition
                };
                //var definition = await serviceClient.GetAccountByIdAsync(maTk);
                //return new Result { Code = 200, Status = "Ok" };
            }
            catch (Exception ex)
            {
                //return new Result { Code = 500, Status = ex.Message };
                return ex.Message;
            }
        }

        [HttpGet("get-account-service")]
        public async Task<Result> getuserservice(string maTk)
        {
            ServiceClient serviceClient = new ServiceClient();
            try
            {
                var accountData = await Task.Run(() => serviceClient.GetAccountByIdAsync(new GetAccountByIdRequest(maTk)));
                //var a = ConvertXmlToDataSet(accountData.GetAccountByIdResult);
                return new Result { Code = 200, Status = "Ok", Data = accountData.GetAccountByIdResult };
            }
            catch (Exception ex)
            {
                return new Result { Code = 500, Status = ex.Message };
            }
        }
        public static DataSet ConvertXmlToDataSet(List<XElement> elements)
        {
            DataSet dataSet = new DataSet();
            if (elements?.Count > 0)
            {
                // Đọc schema vào DataSet
                using (XmlReader schemaReader = XmlReader.Create(new StringReader(elements[0].ToString())))
                {
                    dataSet.ReadXmlSchema(schemaReader);
                }

                // Đọc dữ liệu XML vào DataSet
                using (XmlReader dataReader = XmlReader.Create(new StringReader(elements[1].ToString())))
                {
                    dataSet.ReadXml(dataReader);
                }
            }
            return dataSet;
        }
    }
}
