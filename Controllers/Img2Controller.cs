using Microsoft.AspNetCore.Mvc;
using sql_img.DTO;
using Dapper;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;

namespace sql_img.Controllers;

/// <summary>
/// 讀取資料庫圖檔相關
/// </summary>
[ApiController]
[Route("[controller]")]
public class Img2Controller : ControllerBase
{
    private readonly ILogger<Img2Controller> _logger;
    private readonly IUnitOfWork unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly int _thumbnailWidth;
    private readonly int _thumbnailHeight;
    public Img2Controller(ILogger<Img2Controller> logger, IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _logger = logger;
        this.unitOfWork = unitOfWork;
        this._configuration = configuration;
        // this._thumbnailWidth = this._configuration.GetSection("Image").GetValue<int>("Thumbnail-Width", 20);
        // this._thumbnailHeight = this._configuration.GetSection("Image").GetValue<int>("Thumbnail-Height", 20);
        this._thumbnailWidth = this._configuration.GetValue<int>("Image:Thumbnail-Width", 20);
        this._thumbnailHeight = this._configuration.GetValue<int>("Image:Thumbnail-Height", 20);
    }

    [HttpPost("up")]
    public async Task<IActionResult> Get([FromForm]Uploads2Model uploads)//415 error [FromBody]
    {
        unitOfWork.BeginTransaction();
        ImgMaster imgMaster = new ImgMaster{Id = 0, Name = uploads.Name};
        var img_master_id = await unitOfWork.ImgMaster.AddAsync(imgMaster);
        if (uploads.Attachments == null) {
            unitOfWork.Save();
            return Ok(0);
        }
        List<ImgDetail> imgDetails = new List<ImgDetail>();
        foreach(IFormFile f in uploads.Attachments){
            // byte[] fileBytesInner;
            // using (var ms = new MemoryStream())
            // {
            //     f.CopyTo(ms);
            //     fileBytesInner = ms.ToArray();
            // }
            imgDetails.Add(new ImgDetail(){
                Id = 0, Master_id = img_master_id, img = new ImageHelper().IFormFileToByteArray(f)});
        }
        //var data = await unitOfWork.Img_Details.AddRangeAsync(img_details);
        var data = await unitOfWork.ImgDetail.AddAsync(imgDetails[0]);
        data = await unitOfWork.ImgDetail.AddAsync(imgDetails[1]);
        unitOfWork.Save();
        return Ok(img_master_id);
    }

    [HttpGet("Get")]
    public async Task<IActionResult> GetAll()
    {
        //new DbEntity<ImgMaster>().ListTable();
        var data = await unitOfWork.ImgMaster.GetAllAsync();
        return Ok (data);
    }

    [HttpGet("master/{master_id}")]
    public async Task<IActionResult> GetMasterOne(int master_id)
    {
        var master = await unitOfWork.ImgMaster.GetByIdAsync(master_id);
        var detail = await unitOfWork.ImgDetail.GetByAnotherIdAsync(master.Id, "master_id");
        if (master == null) return Ok();
        return Ok(new { master=master, 
            detail = detail.Select(s=>$"img2/master/{master_id}/image/{s.Id}").ToList(),
            thumbnail = detail.Select(s=>$"img2/master/{master_id}/thumbnail/{s.Id}").ToList()
            });
    }

    [HttpGet("master/{master_id}/image/{image_id}")]
    public async Task<IActionResult> GetDetailOne(int master_id, int image_id)
    {
        var detail = await unitOfWork.ImgDetail.GetByIdAsync(image_id);
        if (detail == null || detail.img == null) return Ok();
        return File(detail.img, "image/jpeg");
    }

    [HttpGet("master/{master_id}/thumbnail/{image_id}")]
    public async Task<IActionResult> GetDetailOneThumbnail(int master_id, int image_id, int width = 0, int height = 0)
    {
        // Console.WriteLine(System.DateTime.Now.ToString("yyyyMMdd hhmmss:ffffff"));
        if (width == 0 || height == 0){
            width = width == 0 ? this._thumbnailWidth : width;
            height = height == 0 ? this._thumbnailHeight : height;
        }
        var detail = await unitOfWork.ImgDetail.GetByIdAsync(image_id);
        if (detail == null || detail.img == null) return Ok();
        // Console.WriteLine(System.DateTime.Now.ToString("yyyyMMdd hhmmss:ffffff"));
        return File(new ImageHelper().ToThumbnail(detail.img, width, height), "image/jpeg");
    }

}
