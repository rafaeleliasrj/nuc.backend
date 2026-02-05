using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Logging;
using NautiHub.Core.Resources;
using NautiHub.CrossCutting.Services.Templates.Enums;
using NautiHub.CrossCutting.Services.Templates.Interfaces;
using RazorLight;
using System.Text;

namespace NautiHub.CrossCutting.Services.Templates.Providers
{
    public class RazorTemplateService : ITemplateProviderService
    {
        private readonly RazorLightEngine _razorEngine;
        private readonly IConverter _converter;
        private readonly MessagesService _messagesService;
        private readonly ILogger<RazorTemplateService> _logger;
        private bool _ehTermica = false;

        public RazorTemplateService(IConverter converter, MessagesService messagesService, ILogger<RazorTemplateService> logger)
        {
            _razorEngine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(RazorLightEngine))
                .UseMemoryCachingProvider()
                .Build();

            _converter = converter ?? throw new ArgumentNullException(nameof(converter));
            _messagesService = messagesService;
            _logger = logger;
        }

        public async Task<byte[]> RenderAsync<T>(
            OutputTypeEnum outputType,
            string templateContent,
            T model,
            DocumentWidthEnum largura = DocumentWidthEnum.A4,
            DocumentOrientationEnum orientacao = DocumentOrientationEnum.Portrait)
        {
            var html = await _razorEngine.CompileRenderStringAsync(
                Guid.NewGuid().ToString(),
                templateContent,
                model
            );

            if (string.IsNullOrWhiteSpace(html))
                throw new InvalidOperationException(_messagesService.Template_Html_Empty);

            if (outputType != OutputTypeEnum.Pdf)
                return Encoding.UTF8.GetBytes(html);

            _ehTermica = largura is DocumentWidthEnum.Papel58mm or DocumentWidthEnum.Papel80mm;

            HtmlToPdfDocument doc = ConfiguraHtmlParaPdf(largura, html, orientacao);

            if (_ehTermica)
            {
                var pdfBytes = _converter.Convert(doc);
                var alturaUsada = PdfTightener.DetectUsedHeightPoints(pdfBytes, marginInPoints: 6.0);
                HtmlToPdfDocument docTermico = ConfiguraHtmlParaPdf(largura, html, DocumentOrientationEnum.Portrait, alturaUsada);
                return _converter.Convert(docTermico);
            }

            return _converter.Convert(doc);
        }

        private HtmlToPdfDocument ConfiguraHtmlParaPdf(DocumentWidthEnum largura, string html, DocumentOrientationEnum orientacao, double? alturaUsada = null)
        {
            var margens = new MarginSettings { Top = _ehTermica ? 2 : 20, Bottom = _ehTermica ? 2 : 20, Left = _ehTermica ? 1 : 20, Right = _ehTermica ? 1 : 20 };

            var htmlContent = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = orientacao == DocumentOrientationEnum.Portrait ? Orientation.Portrait : Orientation.Landscape,
                    PaperSize = GetPaperSize(largura, alturaUsada + margens.Top + margens.Bottom),
                    Margins = margens,
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { Spacing = 3 },
                        FooterSettings = { Spacing = 5 }
                    }
                }
            };

            if (!_ehTermica)
                htmlContent.Objects[0].FooterSettings = new FooterSettings { Spacing = 5, FontSize = 8, Center = "[page] de [toPage]" };

            return htmlContent;
        }

        private PechkinPaperSize GetPaperSize(DocumentWidthEnum width, double? height = null)
        {
            if (_ehTermica)
            {
                height = height ?? 2000;

                return width switch
                {
                    DocumentWidthEnum.Papel58mm => new PechkinPaperSize("58mm", height + "mm"),
                    _ => new PechkinPaperSize("80mm", height + "mm")
                };
            }

            return width switch
            {
                DocumentWidthEnum.Carta => PaperKind.Letter,
                _ => PaperKind.A4
            };
        }

        public async Task<string> RenderTemplateAsync<T>(string templateContent, T model)
        {
            return await _razorEngine.CompileRenderStringAsync(
                Guid.NewGuid().ToString(),
                templateContent,
                model
            );
        }
    }
}
