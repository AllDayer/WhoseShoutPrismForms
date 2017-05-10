﻿using System;

namespace WhoseShoutWebService.Models
{
    public class ShoutUserDto
    {
        public Guid ID { get; set; }
        public String UserName { get; set; }

        public override string ToString()
        {
            return UserName;
        }
    }
}