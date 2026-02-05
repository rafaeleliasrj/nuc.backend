using System.Text.RegularExpressions;

namespace NautiHub.Domain.Services.DomainService.Utilitarios;

public static class XmlExtensions
{
    public static string RemoveCData(this string xml)
    {
        var regex = new Regex(@"<!\[CDATA\[(.*?)\]\]>", RegexOptions.Singleline);
        return regex.Replace(xml, "$1");
    }
}
