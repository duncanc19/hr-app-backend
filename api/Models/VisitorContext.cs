using Microsoft.EntityFrameworkCore;
namespace HRApp.API.Models 
{
  public class VisitorContext : DbContext 
  {
    public VisitorContext(DbContextOptions<VisitorContext> options) : base(options)
    {

    }

    public DbSet<VisitorDb> Visitor { get; set; }
  }
}
