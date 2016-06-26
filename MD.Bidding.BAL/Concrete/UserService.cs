using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using MD.Bidding.DTO;
using MD.Bidding.BAL.Interfaces;
using MD.Bidding.DAL.Interfaces;
using MD.Bidding.Models;
using MD.Bidding.Helpers;
using log4net;

namespace MD.Bidding.BAL.Concrete
{
    public class UserService : IUserService
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(UserService));
        private IUserDataService _dataService;

        public UserService(IUserDataService dataService)
        {
            this._dataService = dataService;
        }


        public AspNetUser GetLoggedInUser(string userId)
        {
            var result = this._dataService.GetLoggedInUser(userId);
            return MapEFToModel(result);
        }

        /// <summary>
        /// Converts an AspNetUser Models object to an AspNetUser DTO Object and passes the AspNetUser DTO
        /// along with the userId to the SaveUser Method in DAL for saving.
        /// </summary>
        /// <param name="user">AspNetUser Models object.</param>
        /// <param name="userId">UserId of the user saving the AspNetUser.</param>
        /// <returns>AspNetUser Models object.</returns>
        public AspNetUser SaveUser(AspNetUser user, string userId)
        {
            var aspNetUserDTO = new DTO.AspNetUserDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                Location = user.Location,                              
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,              
                DateOfBirth = user.DateOfBirth,
                PasswordHash = user.PasswordHash,
                UserName = user.UserName,

            };
            var result = this._dataService.SaveUser(aspNetUserDTO, userId);

            if (user.UserRoles != null)
            {
                var roles = user.UserRoles;
                foreach (var userRole in roles)
                {
                    _dataService.CreateAspNetUserRolesRecord(result.Id, userRole);
                }
            }

            return MapEFToModel(result);
        }


        /// <summary>
        /// checks whether the user with the specified email or userId exists,
        /// returns true if user exists or false.
        /// </summary>
        /// <param name="finder">EmailAddress or UserId of the user to check.</param>
        /// <returns>True or False</returns>
        public bool UserExists(string finder)
        {
            return this._dataService.UserExists(finder);
        }

                

        /// <summary>
        /// Gets the full name of AspNetUser by concatenating its FirstName and lastName
        /// and returns the full name.
        /// </summary>
        /// <param name="aspNetUser">AspNetUser EF Models passed to obtain full Name from. </param>
        /// <returns>AspNetUser fullname which is a string resulting from the concatenation of first name and last name of the aspnet user.</returns>
        public string GetUserFullName(EF.Models.AspNetUser aspNetUser)
        {
            string fullName = string.Empty;
            if (aspNetUser != null)
            {
                fullName = string.Concat(aspNetUser.FirstName, " ", aspNetUser.LastName);
            }
            return fullName;
        }

        public AspNetUser GetAspNetUser(string Id)
        {
            var result = _dataService.GetAspNetUser(Id);
            if (result == null)
            {
                AspNetUser user = null;
                return user;
            }
            else
            {
                return MapEFToModel(result);
            }
        }


        public IEnumerable<AspNetRole> GetAllRoles()
        {
            var results = _dataService.GetAllRoles();
            return MapEFRoleToModelRole(results);
        }

        #region Mapping Methods
        /// <summary>
        /// Maps An IEnumerable collection of EF AspNetUser objects to An IEnumerable collection of AspNetUser Models Objects.
        /// </summary>
        /// <param name="data">An IEnumerable collection of AspNetUser EF objects.</param>
        /// <returns>An IEnumerable collection of AspNetUser Models objects.</returns>
        private IEnumerable<AspNetUser> MapEFToModel(IEnumerable<EF.Models.AspNetUser> data)
        {
            var list = new List<AspNetUser>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps AspNetUser EF Object to AspNetUser Models Object
        /// and returns AspNetUser Model object.
        /// </summary>
        /// <param name="data">AspNet EF Object.</param>
        /// <returns>AspNet Models Object</returns>
        private AspNetUser MapEFToModel(EF.Models.AspNetUser data)
        {
            var user = new AspNetUser()
            {
                Id = data.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
                UserName = data.UserName,
                Location = data.Location,
                PhoneNumber = data.PhoneNumber,
               
                
                CreatedBy = data.CreatedBy,
                UpdatedBy = data.UpdatedBy,
                TimeStamp = data.TimeStamp,
                DateOfBirth = data.DateOfBirth,
                CreatedOn = data.CreatedOn,
            };


            List<string> userroles = new List<string>();

            var dbUserRoles = data.AspNetRoles;
            var numberOfRoles = dbUserRoles.Count;
            EF.Models.AspNetRole[] roles = new EF.Models.AspNetRole[numberOfRoles];
            if (dbUserRoles != null)
            {
                dbUserRoles.CopyTo(roles, 0);
                foreach (var role in roles)
                {
                    userroles.Add(role.Id);
                }
            }
            user.UserRoles = userroles;
            return user;
        }
        
       

        private IEnumerable<AspNetRole> MapEFRoleToModelRole(IEnumerable<EF.Models.AspNetRole> data)
        {
            var list = new List<AspNetRole>();
            foreach (var result in data)
            {
                list.Add(MapEFRoleToModelRole(result));
            }
            return list;
        }


        private AspNetRole MapEFRoleToModelRole(EF.Models.AspNetRole data)
        {
            var role = new AspNetRole()
            {
                Id = data.Id,
                Name = data.Name,
            };
            return role;
        }

        private IEnumerable<EF.Models.AspNetRole> MapModelRoleToEFRole(IEnumerable<AspNetRole> data)
        {
            var list = new List<EF.Models.AspNetRole>();
            foreach (var result in data)
            {
                list.Add(MapModelRoleToEFRole(result));
            }
            return list;
        }


        private EF.Models.AspNetRole MapModelRoleToEFRole(AspNetRole data)
        {
            var role = new EF.Models.AspNetRole()
            {
                Id = data.Id,
                Name = data.Name,
            };
            return role;
        }

        #endregion

        public void SendWelcomeEmail(string emailAddress, string firstName)
        {
            StringBuilder sb = new StringBuilder();
            string strNewPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["welcomeEmail"]);
            using (StreamReader sr = new StreamReader(strNewPath))
            {
                while (!sr.EndOfStream)
                {
                    sb.Append(sr.ReadLine());
                }
            }



            string body = sb.ToString().Replace("#FIRSTNAME#", firstName);
            body = body.Replace("#YOUREMAIL#", emailAddress);

            Helpers.Email email = new Helpers.Email();
            email.MailBodyHtml = body;
            email.MailToAddress = emailAddress;
            email.MailFromAddress = ConfigurationManager.AppSettings["no-reply-email"];
            email.Subject = ConfigurationManager.AppSettings["welcome_email_subject"];
            email.SendMail();
            logger.Debug("Email sent");

        }


        public bool MarkAsDeleted(string Id)
        {
            return _dataService.MarkAsDeleted(Id);
        }
    }
}
