using Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DemoEntityContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        public Repository(DemoEntityContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public IEnumerable<T> GetList()
        {
            return entities.AsEnumerable();
        }
        public T Get(int id)
        {
            return context.Set<T>().Find(id);
            //return entities.SingleOrDefault(s => s.id == id);
        }
        public void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }
        public void Modify(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }
        public void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        public bool IsEntityExists(Expression<Func<T, bool>> condition)
        {
            bool status = false;

            if (condition != null)
            {
                status = entities.Any(condition);
            }

            return status;
        }
        public T Find(Expression<Func<T, bool>> condition)
        {
            return entities.SingleOrDefault(condition);
        }
    }
}
