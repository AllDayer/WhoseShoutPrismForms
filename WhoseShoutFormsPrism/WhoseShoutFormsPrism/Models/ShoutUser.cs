using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoseShoutFormsPrism.Models
{
    public class ShoutUser
    {
        public Guid ID { get; set; }
        public String UserName { get; set; }
        public List<ShoutGroup> ShoutGroups { get; set; }
        public List<Shout> Shouts { get; set; }
    }
}
