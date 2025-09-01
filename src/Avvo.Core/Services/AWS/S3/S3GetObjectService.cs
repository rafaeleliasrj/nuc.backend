using System.ComponentModel.DataAnnotations;
using Amazon.S3;
using Amazon.S3.Model;
using Avvo.Core.Services.Interfaces;

namespace Avvo.Core.Services.Services
{
    public class S3GetObjectService : IS3GetObjectService
    {
        public async Task<Stream> ExecuteAsync(string bucketName, string filePath)
        {
            try
            {
                using AmazonS3Client s3Client = new AmazonS3Client();
                var objectRequest = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = filePath
                };

                GetObjectResponse response = await s3Client.GetObjectAsync(objectRequest);

                return response.ResponseStream;
            }
            catch (AmazonS3Exception e)
            {
                if (!(e.ErrorCode.ToUpper().Contains("NOSUCHKEY")))
                    throw new ValidationException($"Error encountered on server. Message:'{e.Message}' when Download an object");
            }
            catch (Exception e)
            {
                throw new ValidationException($"Unknown encountered on server. Message:'{e.Message}' when Download an object");
            }

            return null;
        }
    }
}
