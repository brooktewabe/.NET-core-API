using LoginAPI.Interface;
using LoginAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginAPI.Repos
{
    public class GenericRepository<T> : iGenericRespoitory<T> where T : class
    {
        protected UserDBContext dbContext;
        internal DbSet<T> DbSet { get; set; }

        public GenericRepository(UserDBContext UserDB)
        {
            this.dbContext = UserDB;
            this.DbSet = this.dbContext.Set<T>();
        }

        public virtual Task<bool> AddEntity(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> DeleteEntity(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<List<T>> GetAllAsync()
        {
            return this.DbSet.ToListAsync();
        }

        public virtual Task<T> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> UpdateEntity(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
