using System.Data;
using System.Data.SqlClient;
using Npgsql;
using Dapper;

public class DbConnectionFactory : IDbConnectionFactory
{
    IConfiguration _configuration;
    public IDbOption dbOption {get;}
    public DbConnectionFactory(IConfiguration configuration)
    {
        this._configuration = configuration;
        IConfigurationSection section = this._configuration.GetSection("DB");
        this.dbOption = new DbOption(){
            dbType = section.GetValue<string>("Db-Type") ?? "ms-sql",
            connectionString = section.GetValue<string>("Connection-String") ?? "Data Source=localhost;Initial Catalog=testdb;Integrated Security=True;MultipleActiveResultSets=True",
            sqlParamIndexChar = section.GetValue<string>("Sql-Param-Index-Char") ?? "@",
            insertedIdSql = section.GetValue<string>("Inserted-Id-Sql") ?? "select SCOPE_IDENTITY();"
        };
    }
    /// <summary>
    /// 獲取資料庫連接
    /// </summary>
    /// <returns></returns>
    public IDbConnection CreateConnection()
    {
        IDbConnection dbConnection;
        // return new SqlConnection(this._configuration.GetConnectionString("DefaultConnection"));
        switch(this.dbOption.dbType.ToLower())
        {
            case "ms-sql":
                dbConnection = new SqlConnection(this.dbOption.connectionString);
                break;
            case "postgres":
                dbConnection = new NpgsqlConnection(this.dbOption.connectionString);
                break;
            default:
                dbConnection = new SqlConnection(this.dbOption.connectionString);
                break;
        }
        return dbConnection;//new SqlConnection(this._configuration.GetConnectionString("DefaultConnection"));
    } 
}