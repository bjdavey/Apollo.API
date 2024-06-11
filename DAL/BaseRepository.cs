using API.Helpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace API.DAL
{
    public class BaseRepository<TEntity, TDbContext>
        where TEntity : EntityBase, new()
        where TDbContext : DbContext, new()
    {
        public readonly TDbContext DbContext;
        //public IQueryable<TEntity> Query;

        public BaseRepository(TDbContext dbContext)
        {
            this.DbContext = dbContext;
            //Query = DbContext.Set<TEntity>().Where(x => x.IsDeleted == false);
        }

        public IQueryable<TEntity> Query(bool includeCreatedBy)
        {
            var Query = DbContext.Set<TEntity>().Where(x => x.IsDeleted == false);
            return includeCreatedBy ? Query.Include(x => x.CreatedByUser) : Query;
        }

        public virtual async Task<List<TEntity>> GetAllList(bool includeCreatedBy)
        {
            return await Query(includeCreatedBy).ToListAsync();
        }

        public virtual async Task<TEntity> Add(TEntity item)
        {
            var newItem = DbContext.Set<TEntity>().Add(item);
            await DbContext.SaveChangesAsync();
            return newItem.Entity;
        }

        public virtual async Task<bool> Delete(int key)
        {
            var result = await Query(false)
                .Where(x => x.Id == key)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true));
            return (result > 0);
        }

        public virtual async Task<bool> HardDelete(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await DbContext.Set<TEntity>().Where(predicate).ExecuteDeleteAsync();
            return (result > 0);
        }

        public virtual async Task<bool> Update(int key, string json)
        {
            var item = await Query(false).FirstOrDefaultAsync(x => x.Id == key);
            if (item == null)
                return false;
            JsonConvert.PopulateObject(json, item);
            //item.UpdatedAt = DateTime.Now;

            return (await DbContext.SaveChangesAsync() > 0);
        }

        public virtual async Task<bool> Update(TEntity item)
        {
            //item.UpdatedAt = DateTime.Now;
            //DbContext.Entry<TEntity>(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return (await DbContext.SaveChangesAsync() > 0);
        }

        // Now, I wish I could write you a melody so plain,
        // That could ease you and cool you and cease the pain 
        // Of your useless and pointless knowledge


    }
}