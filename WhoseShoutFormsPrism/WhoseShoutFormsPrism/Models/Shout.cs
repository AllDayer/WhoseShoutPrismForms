using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoseShoutFormsPrism.Models
{
    public class Shout
    {
        public Guid ID { get; set; }
        public Guid ShoutGroupID { get; set; }
        public Guid UserID { get; set; }
        public DateTime PurchaseTimeUtc { get; set; }
        public float Cost { get; set; }
    }
}
