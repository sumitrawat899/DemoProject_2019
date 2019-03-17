using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Data
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetList();
        T Get(int id);
        T Find(Expression<Func<T, bool>> condition);
        void Add(T entity);
        void Modify(T entity);
        void Remove(T entity);
        bool IsEntityExists(Expression<Func<T, bool>> condition);
    }
}
