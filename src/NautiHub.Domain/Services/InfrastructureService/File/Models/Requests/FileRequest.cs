using NautiHub.Domain.Services.InfrastructureService.File.Models.Enums;

namespace NautiHub.Domain.Services.InfrastructureService.File.Models.Requests;

public class FileRequest(string name,
                      byte[] content,
                      FileVisibilityEnum disponibilidade = FileVisibilityEnum.Public,
                      int? expireIn = null)
{
    public string Name { get; private set; } = name;
    public byte[] Content { get; private set; } = content;
    public FileVisibilityEnum Disponibilidade { get; private set; } = disponibilidade;
    public int? ExpireIn { get; set; }
}
