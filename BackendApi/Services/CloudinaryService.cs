using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Househole_shop.Services
{
    public class CloudinaryService
    {
        private readonly string _cloudName;
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly Cloudinary _cloudinary;

        public CloudinaryService()
        {
            var configText = File.ReadAllText("D:\\Househole_shop\\cloudinary_config.json");
            var config = JsonConvert.DeserializeObject<CloudinaryConfig>(configText);
            if (config != null)
            {
                _cloudName = config.CloudName ?? throw new ArgumentNullException(nameof(config.CloudName));
                _apiKey = config.ApiKey ?? throw new ArgumentNullException(nameof(config.ApiKey));
                _apiSecret = config.ApiSecret ?? throw new ArgumentNullException(nameof(config.ApiSecret));
                _cloudinary = GetCloudinaryInstance();
            }
            else
            {
                throw new ArgumentNullException(nameof(config));
            }
        }

        private Cloudinary GetCloudinaryInstance()
        {
            if (_cloudName == null || _apiKey == null || _apiSecret == null)
            {
                throw new InvalidOperationException("One or more Cloudinary configuration parameters are null.");
            }

            Account account = new Account(
                _cloudName,
                _apiKey,
                _apiSecret
            );

            return new Cloudinary(account);
        }


        public string UploadImage(IFormFile file)
        {
            if (_cloudinary == null)
            {
                throw new Exception("_cloudinary instance is not initialized.");
            }
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "image",
                };

                var uploadResult = _cloudinary.Upload(uploadParams);
                return uploadResult.Url.ToString();
            }
        }

        public bool DeleteImage(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var deletionResult = _cloudinary.Destroy(deleteParams);
            return deletionResult.Result == "ok";
        }

        private class CloudinaryConfig
        {
            public required string CloudName { get; set; }
            public required string ApiKey { get; set; }
            public required string ApiSecret { get; set; }
        }
    }
}
