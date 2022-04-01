using Microsoft.AspNetCore.Mvc;
using sql_img.DTO;
using Dapper;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;

namespace sql_img.Controllers;

[ApiController]
[Route("[controller]")]
public class ImgController : ControllerBase
{
    private readonly ILogger<ImgController> _logger;

    public ImgController(ILogger<ImgController> logger)
    {
        _logger = logger;
    }

    [HttpPost("up")]
    
    public IActionResult Get([FromForm]UploadsModel uploads)//415 error [FromBody]
    {
        var cs = @"Server=localhost;Database=testdb;Trusted_Connection=True;";

        using var con = new SqlConnection(cs);
        con.Open();

        byte[] fileBytes = new byte[]{};
        if (uploads.Attachment != null) {
            using (var ms = new MemoryStream())
            {
                uploads.Attachment.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
        }

        con.Execute("insert sql_img(name, attachment) values(@name, @attachment)",
            new {Name = uploads.Name, attachment=fileBytes});

        return Ok("success");
    }

    [HttpGet("Get")]
       public IActionResult Get()
    {
        var cs = @"Server=localhost;Database=testdb;Trusted_Connection=True;";

        using var con = new SqlConnection(cs);
        con.Open();
        var uploadsModelFromSQL = con.QueryFirst<UploadsModelFromSQL>("SELECT * FROM sql_img WHERE id=@id", 
                new { id = 1 });

        if (uploadsModelFromSQL.Attachment == null) return Ok(0);
        Image image;
        using (MemoryStream ms = new MemoryStream(uploadsModelFromSQL.Attachment))
        {
            image = Image.FromStream(ms);
            image.Save("output.jpg", ImageFormat.Jpeg);
        }
        return Ok(0);
    }

    [HttpGet("one/{id}")]
    public IActionResult GetOne(int? id)
    {
        var cs = @"Server=localhost;Database=testdb;Trusted_Connection=True;";

        using var con = new SqlConnection(cs);
        con.Open();
        try {
            var uploadsModelFromSQL = con.QueryFirst<UploadsModelFromSQL>("SELECT * FROM sql_img WHERE id=@id", 
                new { id = id });
            if (uploadsModelFromSQL.Attachment == null) return Ok(0);
            return File(uploadsModelFromSQL.Attachment, "image/jpeg");
        }
        catch{
            return Ok(0);
        }
        
    }

    private IActionResult NewMethod()
    {
        return Ok(0);
    }
}
