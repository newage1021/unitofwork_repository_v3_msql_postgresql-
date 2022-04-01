using System.Data;
using System.Data.SqlClient;
using Dapper;

public class UnitOfWork : IUnitOfWork
{
    public IDbOption dbOption { get;}
    public string dbType { get;}
    public string paramIndexChar { get;}
    public IDbConnection connection { get; set; }
    public IDbTransaction transaction { get; set; }
    public UnitOfWork(IDbConnectionFactory dbConnectionFactory, IConfiguration configuration)
    {
        this.dbOption = dbConnectionFactory.dbOption;
        this.connection = dbConnectionFactory.CreateConnection();
        this.connection.Open();
        //this.transaction = this.connection.BeginTransaction();   
    }

    public void BeginTransaction(){
        if (this.transaction == null) 
            this.transaction = this.connection.BeginTransaction();
    }
    public void Save(){
        if (this.transaction == null) return;
        try {
            this.transaction.Commit();
        }
        catch {
            this.transaction.Rollback();
        }
    }
    private IProductRepository? _product = null;
    private IImgMasterRepository? _imgMasterRepository = null;   
    private IImgDetailRepository? _imgDetailRepository = null;   
    public IProductRepository Product => _product ?? new ProductRepository(this);
    public IImgMasterRepository ImgMaster => _imgMasterRepository ?? new ImgMasterRepository(this);
    public IImgDetailRepository ImgDetail => _imgDetailRepository ?? new ImgDetailRepository(this); 
}