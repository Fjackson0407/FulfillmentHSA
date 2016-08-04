using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.BaseClass
{
  public   class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;


        public RepositoryBase(DbContext _conntext)
        {
            Context = _conntext;
        }
        public TEntity Get(Guid Id)
        {
            return Context.Set<TEntity>().Find(Id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);

        }

        public void Add(TEntity Entity)
        {
            Context.Set<TEntity>().Add(Entity);
        }

        public void AddRage(IEnumerable<TEntity> Entites)
        {
            Context.Set<TEntity>().AddRange(Entites);

        }

        public void Remove(TEntity Entity)
        {
            Context.Set<TEntity>().Remove(Entity);
        }

        public void RemoveRage(IEnumerable<TEntity> Entites)
        {
            Context.Set<TEntity>().RemoveRange(Entites);
        }
    }
        
}
