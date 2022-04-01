using  System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("products")]
public class Product_o : MyEntity
{
    [Key]
    public int Id { get; set; }
    public string? Name{ get; set; }
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public decimal Rate { get; set; }
    public DateTime AddedOn { get; set; }
    [Editable(false)]
    public DateTime ModifiedOn { get; set; }
    [Editable(false)]
    public int abc { get; set; }
}

[Table("products")]
public class Product// : MyEntity
{
    [Key]
    public int Id { get; set; }
    public string? Name{ get; set; }
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public decimal Rate { get; set; }
    public DateTime AddedOn { get; set; }
    [Editable(false)]
    public DateTime ModifiedOn { get; set; }
    [Editable(false)]
    public int abc { get; set; }
}