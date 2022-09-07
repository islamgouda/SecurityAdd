using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SecurityAdd.Models
{
    public class Context:IdentityDbContext<ApplicationUser>
    {
        public Context():base() { }
        public Context(DbContextOptions options):base(options) { }
    }
}
