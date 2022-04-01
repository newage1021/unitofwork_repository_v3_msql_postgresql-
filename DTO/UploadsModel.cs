using sql_img.Attributes;

namespace sql_img.DTO;

public class UploadsModel
{
    //[Required]
    public int Id { get; set; } = 0;
    //[Required]
    public string? Name { get; set; }
    //[Required]
    [AllowedExtensions(new string[] { ".jpg", ".png" }, "檔案副檔名錯誤")]
    [MaxFileSize(10000, "檔案大小超過範圍")]
    public IList<IFormFile>? Attachments { get; set; }
    [AllowedExtensions(new string[] { ".jpg", ".png" })]
    public IFormFile? Attachment { get; set; }
}