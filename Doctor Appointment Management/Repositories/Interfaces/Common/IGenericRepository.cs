namespace Doctor_Appointment_Management.Repositories.Interfaces.Common;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> GetAll();
    Task<T> GetByIdAsync(object id);
    Task InsertAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(object id);
    Task<long> GetNextIdAsync(string propertyName);
}
