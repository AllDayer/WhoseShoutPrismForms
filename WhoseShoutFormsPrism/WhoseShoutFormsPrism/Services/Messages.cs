﻿using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoseShoutWebService.Models;

namespace WhoseShoutFormsPrism.Services
{
    public class GroupsLoadedEvent : PubSubEvent
    {
        public GroupsLoadedEvent()
        {

        }
    }

    public class UserAddedToGroupEvent : PubSubEvent
    {
        public UserAddedToGroupEvent()
        {

        }
    }

    public class BuyRoundArgs : EventArgs
    {
        public BuyRoundArgs()
        {

        }

        public ShoutGroupDto Group { get; set; }
    }
}
