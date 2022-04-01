using System.Data;
using System.Data.SqlClient;
using Dapper;
public class ProductRepository_o //: IProductRepository
{
    private readonly IConfiguration configuration;
    private IUnitOfWork _unitOfWork;
    public ProductRepository_o(IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        this.configuration = configuration;
        this._unitOfWork = unitOfWork;
    }
    public async Task<int> AddAsync(Product entity)
    {
        entity.AddedOn = DateTime.Now;
        var sql = "Insert into Products (Name,Description,Barcode,Rate,AddedOn) VALUES (@Name,@Description,@Barcode,@Rate,@AddedOn)";
        this._unitOfWork.connection.Open();
        var result = await this._unitOfWork.connection.ExecuteAsync(sql, entity);
        return result;
    }
    public async Task<int> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Products WHERE Id = @Id";
        using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
        {
            connection.Open();
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return result;
        }
    }
    public async Task<IReadOnlyList<Product>> GetAllAsync()
    {
        Console.WriteLine(System.DateTime.Now.ToString("yyyy/mm/dd hh:MM:ss:ffffff"));
        new DbEntity<Product>().ListTable();
        Console.WriteLine(System.DateTime.Now.ToString("yyyy/mm/dd hh:MM:ss:ffffff"));
        
        var sql = "SELECT * FROM Products";
        this._unitOfWork.connection.Open();
        var result = await this._unitOfWork.connection.QueryAsync<Product>(sql);
        return result.ToList();
    }
    public async Task<Product> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM Products WHERE Id = @Id";
        using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
        {
            connection.Open();
            var result = await connection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
            return result;
        }
    }
    public async Task<int> UpdateAsync(Product entity)
    {
        entity.ModifiedOn = DateTime.Now;
        var sql = "UPDATE Products SET Name = @Name, Description = @Description, Barcode = @Barcode, Rate = @Rate, ModifiedOn = @ModifiedOn  WHERE Id = @Id";
        using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
        {
            connection.Open();
            var result = await connection.ExecuteAsync(sql, entity);
            return result;
        }
    }
}