using Microsoft.Extensions.Logging;
using NautiHub.Core.Resources;
using NautiHub.CrossCutting.Services.Templates.Enums;
using NautiHub.CrossCutting.Services.Templates.Interfaces;
using System.Security;
using System.IO;

namespace NautiHub.CrossCutting.Services.Templates;

public class TemplateService : ITemplateService
{
    private readonly ITemplateProviderService _razorTemplateService;
    private readonly string _templatesPath;
    private readonly MessagesService _messagesService;
    private readonly ILogger<TemplateService> _logger;

    public TemplateService(string templatesPath, ITemplateProviderService razorTemplateService, MessagesService messagesService, ILogger<TemplateService> logger)
    {
        if (string.IsNullOrWhiteSpace(templatesPath))
            throw new ArgumentException(
                _messagesService.Template_Path_Null_Empty,
                nameof(templatesPath)
            );

        this._razorTemplateService = razorTemplateService;
        this._templatesPath = templatesPath;
        this._messagesService = messagesService;
        this._logger = logger;
    }

    public async Task<byte[]> RenderAsync<T>(TemplateTypeEnum templateType, OutputTypeEnum outputType, string templateFileName, T model, DocumentWidthEnum documentWidth = DocumentWidthEnum.A4, DocumentOrientationEnum orientacao = DocumentOrientationEnum.Portrait)
    {
        var templateContent = await GetTemplateContentAsync(templateType, templateFileName);

        if (templateType == TemplateTypeEnum.Razor)
            return await this._razorTemplateService.RenderAsync(outputType, templateContent, model, documentWidth, orientacao);
        else
            throw new NotSupportedException();
    }

    public async Task<string> RenderTemplateAsync<T>(string templateFileName, T model)
    {
        var templateContent = await GetTemplateContentAsync(TemplateTypeEnum.Razor, templateFileName);
        return await _razorTemplateService.RenderTemplateAsync(templateContent, model);
    }

    private async Task<string> GetTemplateContentAsync(TemplateTypeEnum templateType, string templateFileName)
    {
        if (string.IsNullOrWhiteSpace(templateFileName))
            throw new ArgumentException(nameof(templateFileName));

        templateFileName = templateFileName.Replace('\\', '/');

        if (templateFileName.Contains("../") || templateFileName.Contains("..\\"))
            throw new SecurityException(_messagesService.Security_Path_Traversal);

        string templateExtension = templateType == TemplateTypeEnum.Razor ? "cshtml" : "frx";
        string fullTemplateFileName = string.Empty;
        string templateContent = string.Empty;

        if (templateType == TemplateTypeEnum.Razor)
        {
            var templateDir = Path.Combine(this._templatesPath, "Razor");
            fullTemplateFileName = Path.Combine(templateDir, $"{templateFileName}.{templateExtension}");

            var normalizedPath = Path.GetFullPath(fullTemplateFileName);
            var normalizedTemplateDir = Path.GetFullPath(templateDir);

            if (!normalizedPath.StartsWith(normalizedTemplateDir, StringComparison.OrdinalIgnoreCase))
                throw new SecurityException(_messagesService.Security_Path_Traversal);
        }
        else
            throw new NotSupportedException();

        if (!System.IO.File.Exists(fullTemplateFileName))
            throw new FileNotFoundException($"{_messagesService.Template_Not_Found}: {fullTemplateFileName}");

        templateContent = await System.IO.File.ReadAllTextAsync(fullTemplateFileName);

        return templateContent;
    }
}
