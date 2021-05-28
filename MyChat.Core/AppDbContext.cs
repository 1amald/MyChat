using Microsoft.AspNetCore.Identity;
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

            builder.Entity<AppUser>().HasData(
            new AppUser
            {
                Id = "647BACBA-FE1F-436A-9892-36B34034EC85",
                UserName = "orlov_a",
                NormalizedUserName = "ORLOV_A",
                Email = "somemail@gmail.com",
                NormalizedEmail = "SOMEMAIL@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "orlov_a"),
                SecurityStamp = string.Empty,
                Status = "Action may not always bring happiness; but there is no happiness without action.",
                AvatarPath = "/Avatars/orlov_a.jpg"
            }, 
            new AppUser {
                Id = "19432DCC-B055-4621-9D83-53C5CAD7DC26",
                UserName = "milena",
                NormalizedUserName = "MILENA",
                Email = "somemail1@gmail.com",
                NormalizedEmail = "SOMEMAIL1@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "milena"),
                SecurityStamp = string.Empty,
                Status = "Life is a dream for the wise, a game for the fool, a comedy for the rich, a tragedy for the poor.",
                AvatarPath = "/Avatars/milena.jpg"
            },
            new AppUser
            {
                Id = "74EE573F-2692-434E-B9BE-C83506CEA3A3",
                UserName = "olofmaister",
                NormalizedUserName = "OLOFMAISTER",
                Email = "somemail2@gmail.com",
                NormalizedEmail = "SOMEMAIL2@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "olofmaister"),
                SecurityStamp = string.Empty,
                Status = "Energy and persistence conquer all things.",
                AvatarPath = "/Avatars/olofmaister.jpg"
            }) ;

            builder.Entity<Message>().HasData(
                new Message
                {
                    Id = new Guid("182611AF-4E9A-4080-BFED-812962DC6398"),
                    UserName = "olofmaister",
                    Text = "Hey everyone!",
                    When = new DateTime(2021, 5, 28, 22, 1, 0),
                    AvatarPath = "/Avatars/olofmaister.jpg",
                    ShortDate = "22:01"
                },
                new Message
                {
                    Id = new Guid("6E45097F-8128-4A66-A56B-067B3B1AEE97"),
                    UserName = "milena",
                    Text = "Hey! Glad to see your message!",
                    When = new DateTime(2021, 5, 28, 22, 2, 0),
                    AvatarPath = "/Avatars/milena.jpg",
                    ShortDate = "22:02"
                },
                new Message
                {
                    Id = new Guid("E693030D-9990-47B8-B1C8-1A2CE22C78DA"),
                    UserName = "orlov_a",
                    Text = "How are u, guys?",
                    When = new DateTime(2021, 5, 28, 22, 3, 0),
                    AvatarPath = "/Avatars/orlov_a.jpg",
                    ShortDate = "22:03"
                },
                new Message
                {
                    Id = new Guid("2CFD6191-29AC-4ABF-A14A-7C1F3A23D50F"),
                    UserName = "milena",
                    Text = "It was a very stressful day, but now i feel great! What about u? And u olof?",
                    When = new DateTime(2021, 5, 28, 22, 4, 0),
                    AvatarPath = "/Avatars/milena.jpg",
                    ShortDate = "22:04"
                },
                new Message
                {
                    Id = new Guid("93E56F39-8E85-487E-8682-ED14929FA37A"),
                    UserName = "olofmaister",
                    Text = "Same as you!",
                    When = new DateTime(2021, 5, 28, 22, 5, 0),
                    AvatarPath = "/Avatars/olofmaister.jpg",
                    ShortDate = "22:05"
                }
                ) ;
        }
    }
}
