using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("img_master")]
public class ImgMaster_o : MyEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Name{ get; set; }
 }
 
[Table("img_master")]
public class ImgMaster// : MyEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Name{ get; set; }
 }