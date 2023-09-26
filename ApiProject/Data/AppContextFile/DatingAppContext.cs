using ApiProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Data.AppContextFile;

public class DatingAppContext : DbContext
{
    public DatingAppContext( DbContextOptions options) : base(options)
    {

    }

    // adding table mapping using DbSet
    public DbSet<AppUser> appUser { get; set; }
}

