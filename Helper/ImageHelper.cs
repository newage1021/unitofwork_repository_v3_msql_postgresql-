public class ImageHelper{
    public byte[] IFormFileToByteArray(IFormFile iFormFile)
    {
        byte[] fileBytes;
        using (var ms = new MemoryStream())
        {
            iFormFile.CopyTo(ms);
            fileBytes = ms.ToArray();
        }
        return fileBytes;
    }
    
    public byte[] ToThumbnail(byte[] byteArrayFullImage, int width, int height){
        System.Drawing.Image OriginalImage;
        MemoryStream ms = new MemoryStream();

        // Stream / Write Image to Memory Stream from the Byte Array.
        ms.Write(byteArrayFullImage, 0, byteArrayFullImage.Length);
        OriginalImage = System.Drawing.Image.FromStream(ms);

        // Shrink the Original Image to a thumbnail size.
        System.Drawing.Image imThumbnailImage = OriginalImage.GetThumbnailImage(width, height, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallBack), IntPtr.Zero);

        // Save Thumbnail to Memory Stream for Conversion to Byte Array.
        MemoryStream myMS = new MemoryStream();
        imThumbnailImage.Save(myMS, System.Drawing.Imaging.ImageFormat.Jpeg);
        byte[] test_imge = myMS.ToArray();
        return test_imge;
    }

    public bool ThumbnailCallBack(){
        return true;
    }
}