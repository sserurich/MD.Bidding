using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD.Bidding.DTO;
using MD.Bidding.EF.Models;

namespace MD.Bidding.DAL.Interfaces
{
    public interface IUserDataService
    {
        AspNetUser GetLoggedInUser(string userId);       
        bool UserExists(string finder);
        AspNetUser SaveUser(AspNetUserDTO user, string userId);
        bool MarkAsDeleted(string Id);
        AspNetUser GetAspNetUser(string Id);
        IEnumerable<AspNetRole> GetAllRoles();

        void CreateAspNetUserRolesRecord(string userId, string roleId);
    }
}
