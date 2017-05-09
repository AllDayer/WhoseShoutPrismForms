using System;
using System.Collections.Generic;
using System.Linq;

namespace WhoseShoutWebService.Models
{
    public class ShoutDto
    {
        public Guid ID { get; set; }
        public Guid ShoutGroupID { get; set; }
        public Guid ShoutUserID { get; set; }
        public DateTime PurchaseTimeUtc { get; set; }
        public float Cost { get; set; }
        public String ShoutGroupName { get; set; }
        public String ShoutCategory { get; set; }
        public String ShoutUserName { get; set; }
    }
    
}