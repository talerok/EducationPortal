using Education.DAL.Entities;
using Education.DAL.Entities.Pages;
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
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserClaim>().ToTable("Claims");
            modelBuilder.Entity<Group>().ToTable("Groups");
            modelBuilder.Entity<Section>().ToTable("Sections");
            modelBuilder.Entity<Theme>().ToTable("Themes");
            modelBuilder.Entity<Note>().ToTable("Notes");
            //-------------------------------------------------
            modelBuilder.Entity<UserGroup>().HasKey(x => new { x.UserId, x.GroupId });
            modelBuilder.Entity<UserGroup>()
                .HasOne(x => x.Group).WithMany(x => x.Users).HasForeignKey(x => x.GroupId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserGroup>()
                .HasOne(x => x.User).WithMany(x => x.Groups).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            //--------------------------------------------------
            modelBuilder.Entity<Message>().HasOne(x => x.Theme).WithMany(x => x.Messages).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Theme>().HasOne(x => x.Section).WithMany(x => x.Themes).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Section>().HasOne(x => x.Group).WithMany(x => x.Sections).OnDelete(DeleteBehavior.Cascade);
            //--------------------------------------------------
            modelBuilder.Entity<Page>().HasOne(x => x.ParentPage).WithMany(x => x.ChildPages).OnDelete(DeleteBehavior.ClientSetNull);

        }


    }
}
