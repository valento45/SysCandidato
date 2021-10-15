﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;
namespace WebApi.Domain
{
    public class UserRole : IdentityUserRole<int>
    {
        public User User { get; set; }
        public Role Role { get; set; }
         
    }
}
