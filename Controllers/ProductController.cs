using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;
    public ProductController(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await unitOfWork.Product.GetAllAsync();
        return Ok (data);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var data = await unitOfWork.Product.GetByIdAsync(id);
        if (data == null) return Ok();
        return Ok(data);
    }
    [HttpPost]
    public async Task<IActionResult> Add(Product product)
    {

        var data1 = await unitOfWork.Product.AddAsync(product);
        return Ok(data1);

        // more than 2 records to test transaction
        unitOfWork.BeginTransaction();
        //product.Name = "123456789012345678901";
        product.Description = "mutli1";    
        var data = await unitOfWork.Product.AddAsync(product);
        product.Description = "mutli2";
        //product.Name = "123456789012345678901";
        data = await unitOfWork.Product.AddAsync(product);
        unitOfWork.Save();
        return Ok(data);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var data = await unitOfWork.Product.DeleteAsync(id);
        //unitOfWork.Save();
        return Ok(data);
    }
    [HttpPut]
    public async Task<IActionResult> Update(Product product)
    {
        var data = await unitOfWork.Product.UpdateAsync(product);
        //unitOfWork.Save();
        return Ok(data);
    }
}