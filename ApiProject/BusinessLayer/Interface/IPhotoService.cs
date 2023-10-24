using CloudinaryDotNet.Actions;

namespace ApiProject.BusinessLayer.Interface{

    public interface IPhotoService{
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}