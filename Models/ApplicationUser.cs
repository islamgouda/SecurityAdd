using Microsoft.AspNetCore.Identity;

namespace SecurityAdd.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FName { get; set; }
        public string Lname { get; set; }
        public string Address { get; set; }

    }
}
