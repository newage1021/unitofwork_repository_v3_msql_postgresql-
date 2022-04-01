public interface IGenericRepository_o<T> where T : MyEntity
{
    Task<T> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetByAnotherIdAsync(int anotherId_value, string anotherId);
    Task<int> AddAsync(T entity);
    Task<int> AddRangeAsync(IEnumerable<T> entityList);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(int id);
}

public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetByAnotherIdAsync(int anotherId_value, string anotherId);
    Task<int> AddAsync(T entity);
    Task<int> AddRangeAsync(IEnumerable<T> entityList);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(int id);
}