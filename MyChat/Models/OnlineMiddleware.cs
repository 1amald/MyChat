using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MyChat.Data;
using System;
using System.Threading.Tasks;

namespace MyChat.Models
{
    public class OnlineMiddleware
    {
        private AppDbContext db;
        private UserManager<AppUser> um;
        private readonly RequestDelegate _next;
        public OnlineMiddleware(AppDbContext db,UserManager<AppUser> userManager) {
            um = userManager;
            this.db = db;
        }
        public OnlineMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var current = System.Security.Claims.ClaimsPrincipal.Current;
            if(current == null)
            {
                return;
            }
            AppUser u = um.FindByNameAsync(System.Security.Claims.ClaimsPrincipal.Current.Identity.Name).Result;
            u.LastAction = DateTime.Now;
            await db.SaveChangesAsync();
        }
    }
}
