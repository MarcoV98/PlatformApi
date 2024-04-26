using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlatformApi.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CommentDate { get; set; }
        public int UserId { get; set; }
        public int? RequestId { get; set; }
        public int? ResponseId { get; set; }
    }
}