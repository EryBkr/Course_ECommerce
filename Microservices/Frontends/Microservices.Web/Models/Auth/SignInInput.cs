using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microservices.Web.Models.Auth
{
    public class SignInInput
    {
        [Display(Name ="Email Adresiniz")]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Şifreniz")]
        [Required]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        [JsonIgnore] //Property json a dönüştürülmesin diye ekledik
        public bool IsRemember { get; set; }
    }
}
