using SkiaSharp;
using System.Text.RegularExpressions;

namespace NautiHub.Core.Extensions;

public static class FileExtension
{
    public static bool FileIsBase64Image(this string arquivoBase64)
    {
        return TransformBase64ImageToArrayByte(arquivoBase64) != null;
    }

    public static byte[] TransformBase64ImageToArrayByte(this string arquivoBase64)
    {
        try
        {
            string base64 = Regex.Replace(
                arquivoBase64,
                "data:image\\/(png|tiff|jpg|jpeg|gif);base64,",
                "",
                RegexOptions.IgnoreCase
            );

            return Convert.FromBase64String(base64);
        }
        catch
        {
            return null!;
        }
    }

    public static (int width, int height) GetSizeImage(this byte[] file)
    {
        using var bitmap = SKBitmap.Decode(file);
        return (bitmap.Width, bitmap.Height);
    }

    public static string? GetFileExtension(this byte[] file)
    {
        using var skData = SKData.CreateCopy(file);
        using var codec = SKCodec.Create(skData);
        if (codec == null)
            return null;

        return codec.EncodedFormat switch
        {
            SKEncodedImageFormat.Jpeg => "jpg",
            SKEncodedImageFormat.Png => "png",
            SKEncodedImageFormat.Gif => "gif",
            SKEncodedImageFormat.Bmp => "bmp",
            SKEncodedImageFormat.Webp => "webp",
            SKEncodedImageFormat.Ico => "ico",
            SKEncodedImageFormat.Heif => "heif",
            SKEncodedImageFormat.Avif => "avif",
            _ => codec.EncodedFormat.ToString().ToLower()
        };
    }

    public static byte[] ResizeBase64Image(this string imagemBase64, int? maxWidth = 500, int? maxHeight = 500, int? qualidade = 90)
    {
        string? mimeType = null;
        if (imagemBase64.Contains(","))
        {
            var header = imagemBase64[..imagemBase64.IndexOf(",")];
            imagemBase64 = imagemBase64[(imagemBase64.IndexOf(",") + 1)..];

            if (header.Contains("image/"))
                mimeType = header.Replace("data:", "").Replace(";base64", "");
        }

        byte[] bytes = Convert.FromBase64String(imagemBase64);

        using var inputStream = new SKMemoryStream(bytes);
        using var codec = SKCodec.Create(inputStream);

        if (codec == null)
            throw new ArgumentException("Imagem inválida.");

        using var original = SKBitmap.Decode(codec);

        if (original == null)
            throw new ArgumentException("Não foi possível decodificar a imagem base64.");

        float scaleX = (float)maxWidth! / original.Width;
        float scaleY = (float)maxHeight! / original.Height;
        float scale = Math.Min(scaleX, scaleY);

        if (scale > 1) scale = 1;

        int newWidth = (int)(original.Width * scale);
        int newHeight = (int)(original.Height * scale);

        using var resized = original.Resize(new SKImageInfo(newWidth, newHeight), new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.None));
        if (resized == null)
            throw new InvalidOperationException("Não foi possível redimensionar a imagem.");

        using var surface = SKSurface.Create(new SKImageInfo(newWidth, newHeight));
        surface.Canvas.Clear(SKColors.White);
        surface.Canvas.DrawBitmap(resized, 0, 0);
        surface.Canvas.Flush();

        using var finalImage = surface.Snapshot();

        SKEncodedImageFormat formatWanted = mimeType?.ToLower() switch
        {
            "image/jpeg" => SKEncodedImageFormat.Jpeg,
            "image/png" => SKEncodedImageFormat.Png,
            "image/webp" => SKEncodedImageFormat.Webp,
            "image/gif" => SKEncodedImageFormat.Gif,
            "image/bmp" => SKEncodedImageFormat.Bmp,
            _ => SKEncodedImageFormat.Jpeg
        };

        SKEncodedImageFormat[] formatFallback = { formatWanted, SKEncodedImageFormat.Png, SKEncodedImageFormat.Jpeg, SKEncodedImageFormat.Webp };

        foreach (var formato in formatFallback.Distinct())
        {
            using var data = finalImage.Encode(formato, qualidade ?? 90);
            if (data != null)
                return data.ToArray();
        }

        throw new InvalidOperationException("Nenhum codec válido encontrado para salvar a imagem.");
    }
}
