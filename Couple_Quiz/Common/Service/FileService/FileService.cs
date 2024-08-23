using Couple_Quiz.Common.Interface.IFile;

namespace Couple_Quiz.Common.Service.FileService
{
    public class FileService : IFileUpload
    {
        private readonly IFileValidation fileValidation;
        private readonly IWebHostEnvironment webHostEnvironment;
        public FileService(IFileValidation fileValidation, IWebHostEnvironment webHostEnvironment)
        {
            this.fileValidation = fileValidation;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("File is required");
            }
            if (!fileValidation.IsValidImageFile(file.FileName))
            {
                throw new Exception("Invalid file type. Only PNG, JPG, JPEG, SVG, GIF, BMP, WEBP, TIFF files are allowed.");
            }
            var uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }
            var imageUniqueFileName = Guid.NewGuid().ToString() + Path.GetFileName(file.FileName);
            var imageFilepath = Path.Combine(uploadDir, imageUniqueFileName);
            using (var fileStream = new FileStream(imageFilepath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return "/uploads/" + imageUniqueFileName;
        }
    }

}
