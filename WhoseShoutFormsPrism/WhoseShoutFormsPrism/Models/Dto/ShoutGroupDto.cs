using System;
using System.Collections.Generic;

namespace WhoseShoutWebService.Models
{
    public class ShoutGroupDto
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public List<ShoutUserDto> Users { get; set; }
        public List<ShoutDto> Shouts { get; set; }
    }
    
}