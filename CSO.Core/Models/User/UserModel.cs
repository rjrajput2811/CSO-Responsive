using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace CSO.Core.Model.User
{
    public class UserListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Role { get; set; }
        public string Rights { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Division { get; set; }
        public string Plant { get; set; }
        public string ADid { get; set; }
        public string UserType { get; set; }
    }

    public class UserAddModel
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Display(Name = "Designation")]
        [Required(ErrorMessage = "Designation is required.")]
        public string Designation { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Display(Name = "Mobile No")]
        [Required(ErrorMessage = "Mobile No is required.")]
        public string MobileNo { get; set; }

        [Display(Name = "RoleId")]
        [Required(ErrorMessage = "Role is required.")]
        public int RoleId { get; set; }

        [Display(Name = "Rights")]
        //[Required(ErrorMessage = "Rights is required.")]
        public string Rights { get; set; }

        //[Display(Name = "User Name")]
        //[Required(ErrorMessage = "User Name is required.")]
        public string UserName { get; set; }

        //[Display(Name = "Password")]
        //[DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Password must have minimum 8 characters and contain 1 uppercase letter, 1 lowercase letter, 1 digit and 1 special character.")]
        //[Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Display(Name = "Division")]
        [Required(ErrorMessage = "Division is required.")]
        public string DivisionId { get; set; }

        [Display(Name = "Plant")]
        [Required(ErrorMessage = "Plant is required.")]
        public string PlantId { get; set; }

        [Display(Name = "Plant / Dept Responsible")]
        //[Required(ErrorMessage = "Nearest Plant is required.")]
        public string NearestPlantId { get; set; }

        [Display(Name = "Product Type")]
        [Required(ErrorMessage = "Product Type is required.")]
        public string ProductTypeId { get; set; }

        [Display(Name = "Brand Name")]
        [Required(ErrorMessage = "Brand is required.")]
        public string BrandId { get; set; }

        [Display(Name = "ADid")]
        [Required(ErrorMessage = "ADid is required.")]
        public string ADid { get; set; }

        [Display(Name = "User Type")]
        [Required(ErrorMessage = "User type is required.")]
        public int UserType { get; set; }

        [Display(Name = "Allow Mail Notification")]
        public bool IsInMailMatrix { get; set; }

        public string SearchText { get; set; }

        public List<SelectListItem> DivisionList { get; set; }

        public List<SelectListItem> PlantList { get; set; }

        public List<SelectListItem> NearestPlantList { get; set; }
        public List<SelectListItem> BrandList { get; set; }
        public List<SelectListItem> ProductTypeList { get; set; }
        public List<SelectListItem> RoleList { get; set; }
        public List<SelectListItem> UserTypeList { get; set; }
        public List<SelectListItem> RightsList { get; set; }
    }


    public class UserEditModel
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Display(Name = "Designation")]
        [Required(ErrorMessage = "Designation is required.")]
        public string Designation { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Display(Name = "Mobile No")]
        [Required(ErrorMessage = "Mobile No is required.")]
        public string MobileNo { get; set; }

        [Display(Name = "RoleId")]
        [Required(ErrorMessage = "Role is required.")]
        public int RoleId { get; set; }

        [Display(Name = "Rights")]
        [Required(ErrorMessage = "Rights is required.")]
        public string Rights { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "User Name is required.")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Password must have minimum 8 characters and contain 1 uppercase letter, 1 lowercase letter, 1 digit and 1 special character.")]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Display(Name = "Division")]
        [Required(ErrorMessage = "Division is required.")]
        public string DivisionId { get; set; }

        [Display(Name = "Plant")]
        [Required(ErrorMessage = "Plant is required.")]
        public string PlantId { get; set; }

        [Display(Name = "Plant / Dept Responsible")]
        [Required(ErrorMessage = "Nearest Plant is required.")]
        public int NearestPlantId { get; set; }

        [Display(Name = "Product Type")]
        [Required(ErrorMessage = "Product Type is required.")]
        public string ProductTypeId { get; set; }

        [Display(Name = "Brand Name")]
        [Required(ErrorMessage = "Brand is required.")]
        public string BrandId { get; set; }

        [Display(Name = "ADid")]
        [Required(ErrorMessage = "ADid is required.")]
        public string ADid { get; set; }

        [Display(Name = "User Type")]
        [Required(ErrorMessage = "User type is required.")]
        public int UserType { get; set; }
    }
}
