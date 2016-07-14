using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.BaseClass
{
    public interface IRepositoryBase<TEntity> where TEntity :class
    {
    TEntity Get(Guid Id);
    IEnumerable<TEntity> GetAll();
    IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    void Add(TEntity Entity);
    void AddRage(IEnumerable<TEntity> Entites);
    void Remove(TEntity Entity);
    void RemoveRage(IEnumerable<TEntity> Entites);
        
    }
}
