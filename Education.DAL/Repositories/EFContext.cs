using Education.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Education.DAL.Repositories
{
    class EFContext : DbContext
    {
        public EFContext(DbContextOptions<EFContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public override DbSet<TEntity> Set<TEntity>()
        {
            var a = typeof(TEntity);
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                modelBuilder.Entity(typeof(User)).ToTable("Users");
                modelBuilder.Entity(typeof(UserClaim)).ToTable("Claims");
        }


    }
}
