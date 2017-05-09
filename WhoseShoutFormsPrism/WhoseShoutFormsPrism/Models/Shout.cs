using System;

namespace WhoseShoutFormsPrism.Models
{
    public class Shout
    {
        public Guid ID { get; set; }
        public Guid ShoutGroupID { get; set; }
        public Guid ShoutUserID { get; set; }
        public DateTime PurchaseTimeUtc { get; set; }
        public float Cost { get; set; }
        public ShoutGroup ShoutGroup { get; set; }
        public ShoutUser ShoutUser { get; set; }
    }
}
