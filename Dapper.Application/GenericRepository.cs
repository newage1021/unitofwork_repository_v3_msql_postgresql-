using System.Data;
using System.Data.SqlClient;
using Dapper;

public class GenericRepository_o<T> : IGenericRepository_o<T> where T : MyEntity, new()
{
    //private readonly IConfiguration _configuration;
    private IUnitOfWork _unitOfWork;
    public MyEntity _myEntity;
    public GenericRepository_o(IUnitOfWork unitOfWork)
    {
        //this._configuration = configuration;
        this._unitOfWork = unitOfWork;
        this._myEntity = new T();
    }
    public async Task<T> GetByIdAsync(int id)
    {
        var sql = this._myEntity.SqlSelectById();
        var result = await this._unitOfWork.connection.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });//, this._unitOfWork.transaction);
        return result;
    }
    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        var sql = this._myEntity.SqlSelect();
        var result = await this._unitOfWork.connection.QueryAsync<T>(sql);//, this._unitOfWork.transaction);
        return result.ToList();
    }
    public async Task<IReadOnlyList<T>> GetByAnotherIdAsync(int anotherId_value, string anotherId)
    {
        var sql = this._myEntity.SqlSelectById();
        sql = sql.ToLower().Replace("where id", $"where {anotherId}");
        var result = await this._unitOfWork.connection.QueryAsync<T>(sql, new { id = anotherId_value });//, this._unitOfWork.transaction);
        return result.ToList();
    }

    public async Task<int> AddAsync(T entity)
    {
        try{
            var sql = this._myEntity.SqlInsert();
            var result = this._unitOfWork.transaction == null ? 
                await this._unitOfWork.connection.ExecuteScalarAsync<int>(sql, entity)
                : await this._unitOfWork.connection.ExecuteScalarAsync<int>(sql, entity, this._unitOfWork.transaction);
            return result;
        }
        catch{
            if (this._unitOfWork.transaction != null) 
                this._unitOfWork.transaction.Rollback();
            return 0;
        }
    }
    public async Task<int> AddRangeAsync(IEnumerable<T> entityList)
    {
        try{
            var sql = this._myEntity.SqlInsert(true);
            var result = this._unitOfWork.transaction == null ? 
                await this._unitOfWork.connection.ExecuteScalarAsync<int>(sql, entityList)
                : await this._unitOfWork.connection.ExecuteScalarAsync<int>(sql, entityList, this._unitOfWork.transaction);
            return result;
        }
        catch(Exception ex){
            if (this._unitOfWork.transaction != null) 
                this._unitOfWork.transaction.Rollback();
            throw new Exception(ex.Message);
        }
    }
    public async Task<int> UpdateAsync(T entity)
    {
        try{
            var sql = this._myEntity.SqlUpdateById();
            var result = this._unitOfWork.transaction == null ? 
                await this._unitOfWork.connection.ExecuteAsync(sql, entity)
                : await this._unitOfWork.connection.ExecuteAsync(sql, entity, this._unitOfWork.transaction);
            return result;
        }
        catch{
            if (this._unitOfWork.transaction != null) 
                this._unitOfWork.transaction.Rollback();
            return 0;
        }
    }
    public async Task<int> DeleteAsync(int id)
    {
        try{
            var sql = this._myEntity.SqlDeleteById();
            var result = this._unitOfWork.transaction == null ? 
                await this._unitOfWork.connection.ExecuteAsync(sql, new { Id = id })
                : await this._unitOfWork.connection.ExecuteAsync(sql, new { Id = id }, this._unitOfWork.transaction);
            return result;
        }
        catch{
            if (this._unitOfWork.transaction != null) 
                this._unitOfWork.transaction.Rollback();
            return 0;
        }
    }

}

public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
{
    //private readonly IConfiguration _configuration;
    private IUnitOfWork _unitOfWork;
    public DbEntity<T> _myEntity;
    public GenericRepository(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
        this._myEntity = new DbEntity<T>(unitOfWork.dbOption);
    }
    public async Task<T> GetByIdAsync(int id)
    {
        var sql = this._myEntity.SqlSelectById();
        var result = await this._unitOfWork.connection.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });//, this._unitOfWork.transaction);
        return result;
    }
    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        var sql = this._myEntity.SqlSelect();
        var result = await this._unitOfWork.connection.QueryAsync<T>(sql);//, this._unitOfWork.transaction);
        return result.ToList();
    }
    public async Task<IReadOnlyList<T>> GetByAnotherIdAsync(int anotherId_value, string anotherId)
    {
        var sql = this._myEntity.SqlSelectById();
        sql = sql.ToLower().Replace("where id", $"where {anotherId}");
        var result = await this._unitOfWork.connection.QueryAsync<T>(sql, new { id = anotherId_value });//, this._unitOfWork.transaction);
        return result.ToList();
    }

    public async Task<int> AddAsync(T entity)
    {
        try{
            var sql = this._myEntity.SqlInsert();
            var result = this._unitOfWork.transaction == null ? 
                await this._unitOfWork.connection.ExecuteScalarAsync<int>(sql, entity)
                : await this._unitOfWork.connection.ExecuteScalarAsync<int>(sql, entity, this._unitOfWork.transaction);
            return result;
        }
        catch(Exception ex){
            if (this._unitOfWork.transaction != null) 
                this._unitOfWork.transaction.Rollback();
            return 0;
        }
    }
    public async Task<int> AddRangeAsync(IEnumerable<T> entityList)
    {
        try{
            var sql = this._myEntity.SqlInsert(true);
            var result = this._unitOfWork.transaction == null ? 
                await this._unitOfWork.connection.ExecuteScalarAsync<int>(sql, entityList)
                : await this._unitOfWork.connection.ExecuteScalarAsync<int>(sql, entityList, this._unitOfWork.transaction);
            return result;
        }
        catch(Exception ex){
            if (this._unitOfWork.transaction != null) 
                this._unitOfWork.transaction.Rollback();
            throw new Exception(ex.Message);
        }
    }
    public async Task<int> UpdateAsync(T entity)
    {
        try{
            var sql = this._myEntity.SqlUpdateById();
            var result = this._unitOfWork.transaction == null ? 
                await this._unitOfWork.connection.ExecuteAsync(sql, entity)
                : await this._unitOfWork.connection.ExecuteAsync(sql, entity, this._unitOfWork.transaction);
            return result;
        }
        catch{
            if (this._unitOfWork.transaction != null) 
                this._unitOfWork.transaction.Rollback();
            return 0;
        }
    }
    public async Task<int> DeleteAsync(int id)
    {
        try{
            var sql = this._myEntity.SqlDeleteById();
            var result = this._unitOfWork.transaction == null ? 
                await this._unitOfWork.connection.ExecuteAsync(sql, new { Id = id })
                : await this._unitOfWork.connection.ExecuteAsync(sql, new { Id = id }, this._unitOfWork.transaction);
            return result;
        }
        catch{
            if (this._unitOfWork.transaction != null) 
                this._unitOfWork.transaction.Rollback();
            return 0;
        }
    }

}