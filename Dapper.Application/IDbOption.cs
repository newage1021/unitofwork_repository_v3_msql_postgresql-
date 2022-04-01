public interface IDbOption
{
    string dbType { get;}
    string connectionString { get;}
    string sqlParamIndexChar { get;}
    string insertedIdSql { get;}
}