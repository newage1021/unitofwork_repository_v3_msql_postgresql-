using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("img_detail")]
public class ImgDetail_o : MyEntity
{
    [Key]
    public int Id { get; set; }
    public int Master_id { get; set; }
    public byte[]? img{ get; set; }
 }

[Table("img_detail")]
public class ImgDetail// : MyEntity
{
    [Key]
    public int Id { get; set; }
    public int Master_id { get; set; }
    public byte[]? img{ get; set; }
 }
