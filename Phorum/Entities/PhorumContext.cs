using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Configuration;
using System.Xml;

namespace Phorum.Entities
{
    public class PhorumContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public PhorumContext(DbContextOptions<PhorumContext> options) : base(options){}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Like> Like { get; set; }
        
        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //User
            modelBuilder.Entity<User>().HasOne(u => u.Role);
            modelBuilder.Entity<User>()
           .HasIndex(e => e.Email)
            .IsUnique();

            //Post
            modelBuilder.Entity<Post>().HasOne(p => p.User).WithMany(u => u.Posts);
            modelBuilder.Entity<Post>().HasMany(p => p.Likes);

            //Like
            modelBuilder.Entity<Like>().HasOne(l => l.Post);
            modelBuilder.Entity<Like>().HasOne(l => l.User);
            modelBuilder.Entity<Like>()
               .HasOne(p => p.User)
               .WithMany(l => l.Likes)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Like>()
                .HasOne(p => p.Post)
                .WithMany(l => l.Likes)
                .OnDelete(DeleteBehavior.NoAction);


            //Refresh Token
            modelBuilder.Entity<RefreshToken>().HasOne(r => r.User);
            modelBuilder.Entity<RefreshToken>().HasIndex(r => r.TokenId).IsUnique();
            modelBuilder.Entity<RefreshToken>().HasKey(r => r.TokenId);

            //Role
            modelBuilder.Entity<Role>().HasData(new List<Role>() {
                new Role {Id=1,Name = "Admin" },
                new Role {Id=2,Name = "Default User"},
                new Role {Id=3,Name = "Owner"}
            });

        }
    }
}
