namespace sql_img.Attributes;

public class Helper{
    public Helper()
    {
        
    }

    public List<IFormFile> ToIFormFileLists(object objectOrList)
    {
        List<IFormFile> Files = new List<IFormFile>();
        var uFile = objectOrList as IFormFile;
        var uFiles = objectOrList as List<IFormFile>;

        if (uFile is not null)
            Files.Add(uFile);
        if (uFiles is not null)
            Files.AddRange(uFiles);

        return Files;
    }
}