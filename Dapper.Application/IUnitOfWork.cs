using System.Data;
public interface IUnitOfWork
{
    IDbOption dbOption { get;}
    string dbType { get;}// set; }
    string paramIndexChar { get;}// set; }
    IDbConnection connection { get; set; }
    IDbTransaction transaction { get; set; }
    public void BeginTransaction();
    public void Save();
    IProductRepository Product { get; }
    IImgMasterRepository ImgMaster { get; }
    IImgDetailRepository ImgDetail { get; }
}