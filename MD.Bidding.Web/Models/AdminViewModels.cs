using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MD.Bidding.Web.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }

        public string Description { get; set; }

    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        //public IEnumerable<SelectListItem> RolesList { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [RegularExpression(@"^(2567|2563)\d{8}$", ErrorMessage = "Mobile number should start with 256 and should be of length 12 characters")]
        public string PhoneNumber { get; set; }


    }
}