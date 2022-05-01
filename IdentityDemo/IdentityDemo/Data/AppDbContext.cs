using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityDemo.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
