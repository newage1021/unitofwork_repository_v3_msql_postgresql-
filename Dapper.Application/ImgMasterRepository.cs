public class ImgMasterRepository : GenericRepository<ImgMaster>, IImgMasterRepository
{
    private IUnitOfWork _unitOfWork;
    public ImgMasterRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }
}