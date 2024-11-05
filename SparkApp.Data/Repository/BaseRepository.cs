using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using SparkApp.Data.Repository.Interfaces;


namespace SparkApp.Data.Repository
{
    public class BaseRepository<TType, TId> : IRepository<TType, TId>
        where TType : class
    {

        private readonly SparkDbContext db;
        private readonly DbSet<TType> dbSet;

        public BaseRepository(SparkDbContext db)
        {
            this.db = db;
            this.dbSet = db.Set<TType>();
        }

        public TType GetById(TId id)
        {
            TType entity = dbSet.Find(id);

            return entity;
        }

        public async Task<TType> GetByIdAsync(TId id)
        {
            TType entity = await dbSet.FindAsync(id);

            return entity;
        }

        public TType FirstOrDefault(Func<TType, bool> predicate)
        {
            TType entity = dbSet.FirstOrDefault(predicate);

            return entity;
        }

        public async Task<TType> FirstOrDefaultAsync(Expression<Func<TType, bool>> predicate)
        {
            TType entity = await dbSet.FirstOrDefaultAsync(predicate);

            return entity;
        }

        public IEnumerable<TType> GetAll()
        {
            return dbSet.ToList();
        }

        public async Task<IEnumerable<TType>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public IQueryable<TType> GetAllAttached()
        {
            return dbSet.AsQueryable();
        }

        public void Add(TType item)
        {
            dbSet.Add(item);
            db.SaveChanges();
        }

        public async Task AddAsync(TType item)
        {
            await dbSet.AddAsync(item);
            await db.SaveChangesAsync();
        }

        public void AddRange(TType[] items)
        {
            dbSet.AddRange(items);
            db.SaveChanges();
        }

        public async Task AddRangeAsync(TType[] items)
        {
            await dbSet.AddRangeAsync(items);
            await db.SaveChangesAsync();
        }

        public bool Delete(TType entity)
        {
            dbSet.Remove(entity);
            db.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteAsync(TType entity)
        {
            dbSet.Remove(entity);
            await db.SaveChangesAsync();

            return true;
        }

        public bool Update(TType item)
        {
            try
            {
                dbSet.Update(item);
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TType item)
        {
            try
            {
                dbSet.Attach(item);
                db.Entry(item).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
