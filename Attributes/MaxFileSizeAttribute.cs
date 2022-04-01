using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace sql_img.Attributes;
//[MaxFileSize(5* 1024 * 1024)]
public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;
    private string _message;
    public MaxFileSizeAttribute(int maxFileSize)
    {
        _maxFileSize = maxFileSize;
        this._message = $"Maximum allowed file size is { _maxFileSize} bytes.";
    }

    public MaxFileSizeAttribute(int maxFileSize, string message)
    {
        _maxFileSize = maxFileSize;
        this._message = message;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        List<IFormFile> Files = new Helper().ToIFormFileLists(value);
        if (Files.Count == 0) return ValidationResult.Success;

        foreach (IFormFile file in Files)
            if (file != null)
            {
               if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(this._message);// GetErrorMessage());
                }
            }

        return ValidationResult.Success;
    }

    public string GetErrorMessage()
    {
        return $"Maximum allowed file size is { _maxFileSize} bytes.";
    }
}