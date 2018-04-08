﻿using Education.DAL.Entities;
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
            //-------------------------------------------------
            modelBuilder.Entity<UserGroup>().HasKey(x => new { x.UserId, x.GroupId });
            modelBuilder.Entity<UserGroup>()
                .HasOne(x => x.Group).WithMany(x => x.Users).HasForeignKey(x => x.GroupId);
            modelBuilder.Entity<UserGroup>()
                .HasOne(x => x.User).WithMany(x => x.Groups).HasForeignKey(x => x.UserId);
            //--------------------------------------------------
        }


    }
}
