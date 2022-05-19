using System;

namespace HeijunkaAppLibrary.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password {get; set; }
        public AuthenticationLevel RoleLevel { get; set; }
        public DateTime LastLoggedIn { get; set; }
    }
    public enum AuthenticationLevel
    {
        GeneralUser,
        Operator,
        Supervisor,
        ProductionControl,
        Admin,
        Superuser
    }
}


