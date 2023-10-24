using ApiProject.BusinessLayer.Interface;
using ApiProject.Helpers;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace ApiProject.BusinessLayer.Service{

    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config){
            var account  = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);    
        }

        /// <summary>
        ///  Method to Upload image into cloudinary.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
             //create variable to store the response like ( DeletionResult ,ImageUploadResult ).
            // create the parameter that we need to pass for particular task  ( publicID for delete , ImageUploadParams for upload).
            // using Cloudinary (created in controller) perform upload or delete and fetch the response inside response result.
            
                var uploadResult = new ImageUploadResult();

                if(file.Length >0){
                    using var stream = file.OpenReadStream();
                    var uploadParams = new ImageUploadParams{
                        File = new FileDescription(file.FileName,stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                        Folder = "DatingApp/UserProfile"
                    };

                    uploadResult = await  _cloudinary.UploadAsync(uploadParams);
                }
                return uploadResult; 
        }


        /// <summary>
        ///  Method to delete image using publicId using cloudinary.
        /// </summary>
        /// <param name="publicId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {   
            //create variable to store the response like ( DeletionResult ,ImageUploadResult ).
            // create the parameter that we need to pass for particular task  ( publicID for delete , ImageUploadParams for upload).
            // using Cloudinary (created in controller) perform upload or delete and fetch the response inside response result.
            
            var DeleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(DeleteParams);
        }
    }
}