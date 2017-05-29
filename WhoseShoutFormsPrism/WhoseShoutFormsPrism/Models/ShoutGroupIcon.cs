using System;
using System.Collections.Generic;
#if WebService
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endif
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoseShoutFormsPrism.Models
{
    public class ShoutGroupIcon
    {
#if WebService
        [Key, ForeignKey("ShoutGroup")]
#endif
        public Guid ShoutGroupID { get; set; }
        public int ShoutIconIndex { get; set; }

        public ShoutGroup ShoutGroup { get; set; }
    }
}
