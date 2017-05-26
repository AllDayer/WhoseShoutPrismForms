using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoseShoutFormsPrism.Models
{
    public class ShoutGroup
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public bool TrackCost { get; set; }
        public List<ShoutUser> ShoutUsers { get; set; }
        public List<Shout> Shouts { get; set; }
    }
}
