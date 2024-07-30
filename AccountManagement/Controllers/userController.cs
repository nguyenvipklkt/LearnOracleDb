using AccountManagement.Requests;
using AccountManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Controllers
{
    public class userController
    {
        private readonly userService _procedureService;
        public userController (userService procedureService)
        {
            this._procedureService = procedureService;
        }

        [HttpPost("create-user")]
        public object CreateUser([FromBody]registerRequest request)
        {
            try
            {
                var definition = _procedureService.CreateCustomer(request);
                return new
                {
                    ProcedureName = request.procedureName,
                    Definition = definition
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
