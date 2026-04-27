using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace Ecommerce.Repositories
{
    public class Repositories<T> where T : class

    {
        EcommerceDbContext _context;
        DbSet<T> _dbSet;
     

        public Repositories()
        {
            _context = new EcommerceDbContext();
            _dbSet = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> Getasync(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>[]? includes = null,
            bool tracking = true
            )

        {
            var entities = _dbSet.AsQueryable();
            if(filter is not null)
                entities = entities.Where(filter);
            if(includes is not null)
                foreach(var item in includes)
                    entities = entities.Include(item);
            if(!tracking)
                entities= entities.AsNoTracking();

            return await entities.ToListAsync();
        }
        public async Task<T?> GetOneAsync(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>[]? includes = null,
            bool tracking = true
            )

        {
           return (await Getasync(filter,includes,tracking)).FirstOrDefault();
        }

        public async Task<EntityEntry<T>> CreateAsync(T Entity)
        {
          return  await _dbSet.AddAsync(Entity);
       }
        public void Update(T Entity)
        {
            _dbSet.Update(Entity);
        }
        public void Delete(T Entity)
        {
            _dbSet.Remove(Entity);
        }
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
