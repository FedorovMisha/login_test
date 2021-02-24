using System;
using Microsoft.AspNetCore.Identity;

namespace AccountWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime DateRegistration { get; set; }
    }
}