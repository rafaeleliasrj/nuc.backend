using System.IO;
using System.Threading.Tasks;

namespace Avvo.Core.Services.Interfaces
{
    public interface IS3GetObjectService
    {
        Task<Stream> ExecuteAsync(string bucketName, string filePath);
    }
}
