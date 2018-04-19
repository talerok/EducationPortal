using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Education.DAL.Entities;

namespace Education.DAL.Repositories
{
    class EFRepos<TEntity> : Interfaces.IRepos<TEntity> where TEntity : class
    {
        protected DbSet<TEntity> dbSet;
        protected DbContext dbContext;

        public EFRepos(DbContext context)
        {
            dbContext = context;
            
            dbSet = dbContext.Set<TEntity>();

        }

        public void Add(TEntity item)
        {
            dbSet.Add(item);
        }

        public void Add(IEnumerable<TEntity> items)
        {
            dbSet.AddRange(items);
        }

        public void Delete(TEntity item)
        {
            dbContext.Entry(item).State = EntityState.Deleted;
        }

        public void Delete(IEnumerable<TEntity> items)
        {
            dbSet.RemoveRange(items);
        }

        public void Edited(TEntity item)
        {
            dbContext.Entry(item).State = EntityState.Modified;

        }

        public void Edited(IEnumerable<TEntity> items)
        {
            foreach (var item in items)
            {
                dbContext.Entry(item).State = EntityState.Modified;
            }
        }

        public IQueryable<TEntity> Get()
        {
            return dbSet;
        }
    }
}
