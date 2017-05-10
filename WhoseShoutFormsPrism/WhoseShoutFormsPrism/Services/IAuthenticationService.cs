﻿using System;
namespace WhoseShoutFormsPrism.Services
{
    public interface IAuthenticationService
    {
        bool Login(string username, string password);

        void Logout();
    }
}