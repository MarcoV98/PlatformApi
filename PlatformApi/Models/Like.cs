using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlatformApi.Models
{
    public class Like
    {
        public int Like_Id { get; set; }
        public int User_Id { get; set; }
        public int? Request_Id { get; set; }
        public int? Response_Id { get; set; }
    }
}