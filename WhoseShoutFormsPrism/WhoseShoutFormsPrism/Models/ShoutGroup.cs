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
        public List<User> Users { get; set; }
        public List<Shout> Shouts { get; set; }
    }
}
