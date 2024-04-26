using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlatformApi.Models
{
    public class Response
    {
        public int ResponseId { get; set; }
        public string Content { get; set; }
        public DateTime UploadDate { get; set; }
        public int UserId { get; set; }
        public int RequestId { get; set; }
    }
}