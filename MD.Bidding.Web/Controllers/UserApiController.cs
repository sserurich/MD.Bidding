using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MD.Bidding.Models;
using MD.Bidding.BAL.Interfaces;

using log4net;


namespace MD.Bidding.Web.Controllers
{
    [Authorize]
    public class UserApiController : ApiController
    {
        private IUserService _userService;

        ILog logger = log4net.LogManager.GetLogger(typeof(UserApiController));
        private string userId = string.Empty;

        public UserApiController()
        {

        }

        public UserApiController(IUserService userService)
        {
            this._userService = userService;
            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
        }

        [HttpPost]
        [ActionName("SaveUser")]
        public AspNetUser SaveUser(AspNetUser user)
        {
            return _userService.SaveUser(user, userId);
        }

        [HttpGet]
        [ActionName("UserExists")]
        public bool UserExists(string finder)
        {
            return _userService.UserExists(finder);
        }

        [HttpGet]
        [ActionName("GetLoggedInUser")]
        public AspNetUser GetLoggedInUser()
        {
            return _userService.GetLoggedInUser(userId);
        }



        [HttpGet]
        [System.Web.Http.ActionName("GetAllRoles")]
        public IEnumerable<AspNetRole> GetAllRoles()
        {
            return _userService.GetAllRoles();
        }

        [HttpGet]
        [ActionName("GetUser")]
        public AspNetUser GetUser()
        {
            return _userService.GetAspNetUser(userId);

        }
    }
}
