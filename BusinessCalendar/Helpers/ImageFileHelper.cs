using DAL.Common;
using Amazon.S3;
using Amazon.S3.Model;

namespace BusinessCalendar.Helpers
{
    public static class ImageFileHelper
    {
        private static string _accessKey;
        private static string _secretKey;
        private static string _serviceURL;
        private static string _bucketName;

        public static void InitConfiguration()
        {
            _accessKey = ConfigurationHelper.GetString("YCAccessKey");
            _secretKey = ConfigurationHelper.GetString("YCSecretKey");
            _serviceURL = ConfigurationHelper.GetString("YCServiceURL");
            _bucketName = ConfigurationHelper.GetString("YCBucketName");
        }

        private static AmazonS3Client CreateS3Client()
        {
            var config = new AmazonS3Config
            {
                ServiceURL = _serviceURL,
            };

            return new AmazonS3Client(_accessKey, _secretKey, config);
        }

        public static void SaveImage(IFormFile formFile, string name)
        {
            try
            {
                using var client = CreateS3Client();
                using var fileStream = formFile.OpenReadStream();
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = name,
                    InputStream = fileStream
                };

                _ = client.PutObjectAsync(request).Result;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the file.", ex);
            }
        }

        public static Stream GetImage(string name)
        {
            try
            {
                using var client = CreateS3Client();
                var request = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = name,
                };

                return client.GetObjectAsync(request).Result.ResponseStream;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while receiving the file.", ex);
            }
        }

        public static void DeleteImage(string name)
        {
            try
            {
                using var client = CreateS3Client();
                var request = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = name,
                };

                _ = client.DeleteObjectAsync(request).Result;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while receiving the file.", ex);
            }
        }
    }
}
