using System;
using WhoseShoutFormsPrism.Models;

namespace WhoseShoutWebService.Models
{
    public class ShoutUserDto
    {
        public Guid ID { get; set; }
        public String UserName { get; set; }
        public int ShoutCount { get; set; }
        public DateTime LastShoutUtc { get; set; }
        public string ShoutSocialID { get; set; }
        public AuthType AuthType { get; set; }

        public override string ToString()
        {
            return UserName;
        }
    }
}