using NautiHub.CrossCutting.Services.Templates.Enums;

namespace NautiHub.CrossCutting.Services.Templates.Interfaces;

public interface ITemplateService
{
    public Task<byte[]> RenderAsync<T>(TemplateTypeEnum templateType, OutputTypeEnum outputType, string templateFileName, T model, DocumentWidthEnum documentWidth = DocumentWidthEnum.A4, DocumentOrientationEnum orientacao = DocumentOrientationEnum.Portrait);
    public Task<string> RenderTemplateAsync<T>(string templateFileName, T model);
}
