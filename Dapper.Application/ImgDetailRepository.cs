public class ImgDetailRepository : GenericRepository<ImgDetail>, IImgDetailRepository
{
    private IUnitOfWork _unitOfWork;
    public ImgDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }
}