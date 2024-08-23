namespace Couple_Quiz.Common.Interface.IFile
{
    public interface IFileUpload
    {
        Task<string> UploadImageAsync(IFormFile file);

    }
}
