using sql_img.Attributes;

namespace sql_img.DTO;

public class UploadsModelFromSQL
{
    //[Required]
    public int Id { get; set; } = 0;
    //[Required]
    public string? Name { get; set; }
    //[Required]
    public byte[]? Attachment { get; set; }
}