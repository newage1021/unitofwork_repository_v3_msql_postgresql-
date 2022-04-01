using System.Data;
public interface IDbConnectionFactory
{
    IDbOption dbOption {get;}
    public IDbConnection CreateConnection();
}