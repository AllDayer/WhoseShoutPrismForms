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
        public String Email { get; set; }
        public String FacebookID { get; set; }//10158583186595237
        public String TwitterID { get; set; } //125192624
        public String AvatarUrl { get; set; }

        public List<ShoutGroup> ShoutGroups { get; set; }
        public List<Shout> Shouts { get; set; }
    }
}
