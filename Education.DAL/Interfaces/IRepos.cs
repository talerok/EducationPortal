using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Education.DAL.Interfaces
{
    public interface IRepos<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get();
        void Add(TEntity item);
        void Add(IEnumerable<TEntity> items);
        void Delete(TEntity item);
        void Delete(IEnumerable<TEntity> items);
        void Edited(TEntity item);
        void Edited(IEnumerable<TEntity> items);
    }
  
}
