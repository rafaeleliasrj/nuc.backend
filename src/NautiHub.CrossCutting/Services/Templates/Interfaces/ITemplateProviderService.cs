using NautiHub.CrossCutting.Services.Templates.Enums;

namespace NautiHub.CrossCutting.Services.Templates.Interfaces;

public interface ITemplateProviderService
{
    public Task<byte[]> RenderAsync<T>(OutputTypeEnum outputType, string templateContent, T model, DocumentWidthEnum documentWidth = DocumentWidthEnum.A4, DocumentOrientationEnum orientacao = DocumentOrientationEnum.Portrait);
    public Task<string> RenderTemplateAsync<T>(string templateContent, T model);
}
