using System;

namespace MVC_Test.Controllers
{
    internal class CheckRoleAttribute : Attribute
    {
        public string[] AllowedRoles { get; set; }
    }
}