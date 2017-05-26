using System;
using System.Collections.Generic;
using System.Linq;

namespace WhoseShoutWebService.Models
{
    public class ShoutGroupDto
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public bool TrackCost { get; set; }
        public List<ShoutUserDto> Users { get; set; }
        public List<ShoutDto> Shouts { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string Colour { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public String WhoseShoutDisplay
        {
            get
            {
                var user = WhoseShout;
                if (!String.IsNullOrEmpty(user.UserName))
                {
                    return user.UserName;
                }
                else
                {
                    return user.Email;
                }
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public ShoutUserDto WhoseShout
        {
            get
            {
                ShoutUserDto user = null;
                if (Users.Count > 0)
                {
                    if (Shouts.Count == 0)
                    {
                        return Users.First();
                    }

                    List<ShoutUserDto> tiedShout = new List<ShoutUserDto>();
                    int lowestShoutCount = int.MaxValue;
                    foreach (var u in Users)
                    {
                        if (u.ShoutCount < lowestShoutCount)
                        {
                            user = u;
                            lowestShoutCount = u.ShoutCount;
                            tiedShout.Clear();
                        }
                        else if (u.ShoutCount == lowestShoutCount)
                        {
                            tiedShout.Add(u);
                        }
                    }

                    if (tiedShout.Count > 1)
                    {
                        foreach (var shout in Shouts)
                        {
                            tiedShout.Remove(tiedShout.FirstOrDefault(x => x.ID == shout.ShoutUserID));
                            if (tiedShout.Count <= 1)
                            {
                                return tiedShout[0];
                            }
                        }
                    }
                }
                return user;
            }
        }
    }

}