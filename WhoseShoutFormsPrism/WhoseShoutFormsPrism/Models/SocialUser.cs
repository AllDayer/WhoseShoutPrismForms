﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoseShoutFormsPrism.Models
{
    public class SocialUser
    {
        public Guid ShoutUserID { get; set; }
        public String FacebookID { get; set; }//10158583186595237
        public String TwitterID { get; set; } //125192624


        public ShoutUser ShoutUser { get; set; }
    }
}
