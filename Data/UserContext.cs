using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class UserContext : DbContext //Adding dbcontext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        public DbSet<AppUser> AppUsers { set; get; }

        protected override void OnModelCreating(ModelBuilder  modelBuilder) //Making the email 
        {
            modelBuilder.Entity<AppUser>(
                entity => { entity.HasIndex(e => e.Email).IsUnique(); });
        }
    }
}
