using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlatformApi.Models
{
    public class Request
    {
        public int RequestId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        public string PreferredMedium { get; set; }
    }
}