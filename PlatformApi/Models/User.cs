using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlatformApi.Models
{
    public class User
    {
        public int User_Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? Registration_Date { get; set; }
        public string Profile_Image { get; set; }
    }
}