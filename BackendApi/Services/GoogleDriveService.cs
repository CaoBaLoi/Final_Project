using System;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Grocery.Services
{
    public class GoogleDriveService
    {
        private DriveService _driveService;
        private const string PathToServiceAccountKeyFile = @"D:\Grocery\grocery-420615-c6642a4c6540.json";
        public GoogleDriveService()
        {
            try
            {
                var credential = GoogleCredential.FromFile(PathToServiceAccountKeyFile)
                                                 .CreateScoped(DriveService.ScopeConstants.DriveFile);
                _driveService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Grocery",
                });
            }
            catch (Exception ex)
            {
                // Xử lý các trường hợp lỗi và ném ngoại lệ để thông báo cho người dùng
                throw new Exception("Error initializing Google Drive service", ex);
            }
        }
        public string? UploadImageToGoogleDrive(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                Console.WriteLine("No image upload");
                return null;
            }

            try
            {
                // Thiết lập danh sách các loại MIME của tệp tin (ví dụ: "image/jpeg", "image/png")
                var mimeTypes = new List<string> { "image/jpeg", "image/png", "image/jpg"};

                // Thiết lập thông tin tệp tin
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = image.FileName,
                    Parents = new List<string> { "1gEhavA7tErlXtpXdqBjodCkXZ6giCi8o" }
                };

                // Tải lên tệp tin
                FilesResource.CreateMediaUpload? request = null; // Initialize request outside the using block
                using (var stream = image.OpenReadStream())
                {
                    try
                    {
                        request = _driveService.Files.Create(fileMetadata, stream, mimeTypes[0]);
                        request.Fields = "id";
                        request.Upload();
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that occur during the upload process
                        Console.WriteLine($"Error uploading file: {ex.Message}");
                        return null;
                    }
                }

                // Kiểm tra nếu request đã được khởi tạo
                if (request != null && request.ResponseBody != null)
                {
                    // Lấy thông tin tệp tin đã tải lên
                    var file = request.ResponseBody;
                    return $"https://drive.google.com/uc?id={file.Id}";
                }
                else
                {
                    // Log more information to diagnose the issue
                    if (request == null)
                    {
                        Console.WriteLine("Upload request is null.");
                    }
                    else if (request.ResponseBody == null)
                    {
                        Console.WriteLine("Response body is null.");
                    }
                    else
                    {
                        Console.WriteLine("Unknown issue.");
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Xử lý các trường hợp lỗi và trả về lỗi cho client
                Console.WriteLine($"Error uploading image: {ex.Message}");
                return null;
            }
        }
        public void DeleteImageFromGoogleDrive(string fileID){
            _driveService.Files.Delete(fileID).Execute();
        }
    }
}
