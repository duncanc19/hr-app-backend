using Microsoft.EntityFrameworkCore;
namespace HRApp.API.Models 
{
  public class UserContext : DbContext 
  {
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {

    }

    public DbSet<UserDb> User { get; set; }
  }
}
