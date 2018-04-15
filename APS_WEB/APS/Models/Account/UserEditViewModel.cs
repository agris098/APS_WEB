using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace APS.Models
{
    [BsonIgnoreExtraElements]
    public class UserEditViewModel
    {

        public string UserId { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }     
        [Required]
        [Display(Name = "Full name")]
        public string FullName { get; set; }    

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Skype")]
        public string Skype { get; set; }

        [Display(Name = "Web Address")]
        public string WebAddress { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        public string sm_image { get; set; }
        public string lg_image { get; set; }

        [Required]
        [Display(Name = "Date of birth")]
        public DateTime DOB { get; set; }

        public string DOBString { get { return DOB.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);} }
    }
}