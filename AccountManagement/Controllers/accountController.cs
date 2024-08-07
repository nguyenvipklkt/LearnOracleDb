using AccountManagement.Requests;
using AccountManagement.Services;
using Microsoft.AspNetCore.Mvc;

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
        public object CreateUser([FromBody]createAccountRequest request)
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
                return new {
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
            }
            catch (Exception ex)
            {
                return new
                {
                    message = ex.Message,
                };
            }
        }
    }
}
