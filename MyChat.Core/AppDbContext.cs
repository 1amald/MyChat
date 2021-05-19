using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyChat.Models;
using System;

namespace MyChat.Core
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Message> Messages { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Message>().HasOne(m => m.Sender).WithMany(u => u.Messages).HasForeignKey(m => m.UserName).HasPrincipalKey(u => u.UserName);
        }
    }
}
