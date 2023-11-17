using ApiProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Data.AppContextFile;

public class DatingAppContext : DbContext
{
    public DatingAppContext( DbContextOptions options) : base(options)
    {
        ChangeTracker.AutoDetectChangesEnabled = true;
    }

    // adding table mapping using DbSet
    public DbSet<AppUser> appUser { get; set; }
    public DbSet<UserLike> Likes {get;set;}
    public DbSet<Message> Messages {get;set;}

    protected override void OnModelCreating(ModelBuilder builder){
        base.OnModelCreating(builder);

        builder.Entity<UserLike>().HasKey(k=> new {k.SourceUserId,k.TargetUserId});
        
        builder.Entity<UserLike>()
            .HasOne(s=>s.SourceUser)
            .WithMany(l=>l.LikedUsers)
            .HasForeignKey(s=>s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserLike>()
            .HasOne(s=>s.TargetUser)
            .WithMany(l=>l.LikedByUsers)
            .HasForeignKey(s=>s.TargetUserId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.Entity<Message>()
        .HasOne(u => u.Recipient)
        .WithMany(m=>m.MessageReceived)
        .OnDelete(DeleteBehavior.Restrict);   

         builder.Entity<Message>()
        .HasOne(u => u.Sender)
        .WithMany(m=>m.MessageSent)
        .OnDelete(DeleteBehavior.Restrict); 
    }
}

