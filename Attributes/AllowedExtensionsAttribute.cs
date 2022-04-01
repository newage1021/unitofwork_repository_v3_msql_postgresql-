using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace sql_img.Attributes;
//[AllowedExtensions(new string[] { ".jpg", ".png" })]
public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;
    private string _message;
    public AllowedExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
        this._message = $"This photo extension is not allowed!";
    }

    public AllowedExtensionsAttribute(string[] extensions, string message)
    {
        _extensions = extensions;
        this._message = message;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        //List<IFormFile> Files = new List<IFormFile>();
        //var uFile = value as IFormFile;
        //var uFiles = value as List<IFormFile>;
        //if (uFile is null && uFiles is null) 
        //    return ValidationResult.Success;

        //if (uFile is not null)
        //    Files.Add(uFile);
        //if (uFiles is not null)
        //    Files.AddRange(uFiles);
        List<IFormFile> Files = new Helper().ToIFormFileLists(value);
        if (Files.Count == 0) return ValidationResult.Success;

        foreach (IFormFile file in Files)
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(this._message);// GetErrorMessage());
                }
            }
        
        return ValidationResult.Success;
    }

    public string GetErrorMessage()
    {
        return $"This photo extension is not allowed!";
    }
}