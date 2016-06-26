using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD.Bidding.DAL.Interfaces;
using MD.Bidding.DTO;
using MD.Bidding.EF.UnitOfWork;
using MD.Bidding.EF.Models;

namespace MD.Bidding.DAL.Concrete
{
    public class UserDataService : DataServiceBase, IUserDataService
    {
        public UserDataService(IUnitOfWork<BiddingEntities> unitOfWork)
            : base(unitOfWork)
        {

        }

        public AspNetUser GetLoggedInUser(string userId)
        {
            return this.UnitOfWork.Get<AspNetUser>().AsQueryable()
                .FirstOrDefault(c => c.Id == userId);
        }


        /// <summary>
        /// Saves a new user or updates an already existing user.
        /// </summary>
        /// <param name="user">User to be saved or updated.</param>
        /// <param name="userId">UserId of the user creating or updating</param>
        /// <returns>User</returns>
        public AspNetUser SaveUser(AspNetUserDTO user, string userId)
        {
            var exists = this.UserExists(user.Id);
            if (exists == false)
            {
                exists = this.UserExists(user.Email);
            }

            if (exists != true)
            {
                var aspnetUser = new AspNetUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = user.Email,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,                    
                    DateOfBirth = user.DateOfBirth,
                    EmailConfirmed = user.EmailConfirmed,                   
                    PhoneNumber = user.PhoneNumber,
                    CreatedBy = userId,
                    TimeStamp = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    Deleted = false,
                    GenderId = user.GenderId,
                    AccessFailedCount = 0,
                    PasswordHash = user.PasswordHash,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,

                };

                this.UnitOfWork.Get<AspNetUser>().AddNew(aspnetUser);
                this.UnitOfWork.SaveChanges();
                return aspnetUser;
            }

            else
            {
                var result = this.UnitOfWork.Get<AspNetUser>().AsQueryable()
                    .FirstOrDefault(u => u.Email == user.Email || u.Id == user.Id);
                if (result != null)
                {
                    result.FirstName = user.FirstName;
                    result.LastName = user.LastName;
                    result.UserName = user.UserName;                   
                    result.DateOfBirth = user.DateOfBirth;
                    result.GenderId = user.GenderId;
                    result.PhoneNumber = user.PhoneNumber;
                    result.PasswordHash = user.PasswordHash;
                    result.TimeStamp = DateTime.Now;
                    result.UpdatedBy = userId;

                    this.UnitOfWork.Get<AspNetUser>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return result;
            }

        }




        public void CreateAspNetUserRolesRecord(string userId, string roleId)
        {
            PurgeAspNetUserRole(userId, roleId);
            using (var dbContext = new BiddingEntities())
            {
              //  dbContext.AspNetUserRole_Create(userId, roleId);
            }
        }

        public void PurgeAspNetUserRole(string userId, string roleId)
        {
            using (var dbContext = new BiddingEntities())
            {
               // dbContext.AspNetUserRole_Purge(userId, roleId);
            }
        }
        /// <summary>
        /// Checks whether a user with the specified finder string(emailAddress or userId) exists in the AspNetUsers
        /// and returns true if exists otherwise returns false.
        /// </summary>       
        /// <param name="finder">specified email address or us.</param>
        /// <returns>True or False</returns>
        public bool UserExists(string finder)
        {
            bool exists = false;
            var user = this.UnitOfWork.Get<AspNetUser>().AsQueryable()
                .FirstOrDefault(u => u.Email == finder || u.Id == finder);
            if (user != null)
            {
                exists = true;
            }
            return exists;
        }


        public bool MarkAsDeleted(string Id)
        {
            bool IsDeleted = false;
            if (Id != null)
            {
                var aspNetUser = (from n in this.UnitOfWork.Get<AspNetUser>().AsQueryable()
                                  where n.Id == Id
                                  select n
                           ).FirstOrDefault();
                if (aspNetUser != null)
                {
                    aspNetUser.DeletedOn = DateTime.Now;
                    aspNetUser.Deleted = true;
                    this.UnitOfWork.Get<AspNetUser>().Update(aspNetUser);
                    this.UnitOfWork.SaveChanges();
                }


                IsDeleted = true;
            }
            else
            {
                IsDeleted = false;
            }

            return IsDeleted;

        }

        public AspNetUser GetAspNetUser(string Id)
        {
            return this.UnitOfWork.Get<AspNetUser>().AsQueryable()
                .FirstOrDefault(m => (m.Id == Id || m.Email == Id) &&
                   (m.Deleted == false || m.Deleted == null)
                );
        }


        public IEnumerable<AspNetRole> GetAllRoles()
        {
            return this.UnitOfWork.Get<AspNetRole>().AsQueryable();
        }

    }
}
