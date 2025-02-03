using Doctor_Appointment_Management.DataContext;
using Doctor_Appointment_Management.Repositories.Interfaces.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Doctor_Appointment_Management.Repositories.Implementations.Common;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AppointmentDataContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<T>();
    }

    public IQueryable<T> GetAll()
    {
        try
        {
            return _dbSet.AsQueryable();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<T> GetByIdAsync(object id)
    {
        try
        {
            return await _dbSet.FindAsync(id);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task InsertAsync(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task UpdateAsync(T entity)
    {
        try
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteAsync(object id)
    {
        try
        {
            var entityToDelete = await _dbSet.FindAsync(id);
            if (entityToDelete != null)
                _dbSet.Remove(entityToDelete);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<long> GetNextIdAsync(string propertyName)
    {
        try
        {
            // Get the PropertyInfo for the specified property name
            var property = typeof(T).GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).Name}'");
            }

            // Create a lambda expression to access the specified property and cast to long?
            var parameter = Expression.Parameter(typeof(T), "e");
            var propertyExpression = Expression.Convert(Expression.Property(parameter, property), typeof(long?)); // Cast to long?
            var lambda = Expression.Lambda<Func<T, long?>>(propertyExpression, parameter);

            // Find the maximum value of the specified property
            var maxId = await _dbSet.MaxAsync(lambda);
            return maxId.HasValue ? maxId.Value + 1 : 1; // Default to 1 if no existing IDs
        }
        catch (Exception)
        {
            throw;
        }
    }


}
