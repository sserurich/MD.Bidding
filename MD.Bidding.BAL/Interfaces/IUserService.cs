using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD.Bidding.Models;

namespace MD.Bidding.BAL.Interfaces
{
    public interface IUserService
    {
        AspNetUser GetLoggedInUser(string userId);
        bool UserExists(string finder);
        AspNetUser SaveUser(AspNetUser user, string userId);
        string GetUserFullName(EF.Models.AspNetUser aspNetUser);
        void SendWelcomeEmail(string emailAddress, string firstName);
        bool MarkAsDeleted(string Id);
        AspNetUser GetAspNetUser(string Id);
        IEnumerable<AspNetRole> GetAllRoles();
    }
}
