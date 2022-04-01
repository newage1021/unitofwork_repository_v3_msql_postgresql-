public class DbOption : IDbOption
{
    public string dbType { get; set;} = "";
    public string connectionString { get; set;} = "";
    public string sqlParamIndexChar { get; set;} = "";
    public string insertedIdSql { get; set;} = "";
}