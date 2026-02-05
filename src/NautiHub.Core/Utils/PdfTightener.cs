public static class PdfTightener
{
    public static double DetectUsedHeightPoints(byte[] inputPdfBytes, double marginInPoints)
    {
        if (inputPdfBytes == null || inputPdfBytes.Length == 0)
            throw new ArgumentException("inputPdfBytes vazio", nameof(inputPdfBytes));

        using var ms = new MemoryStream(inputPdfBytes);
        using var doc = UglyToad.PdfPig.PdfDocument.Open(ms);

        if (doc.NumberOfPages < 1)
            return -1;

        var page = doc.GetPage(1);
        double pageHeight = page.Height;

        double minY = double.MaxValue;
        bool foundAny = false;

        try
        {
            var words = page.GetWords();
            if (words != null && words.Any())
            {
                foreach (var w in words)
                {
                    var bbox = w.BoundingBox;
                    if (bbox != null)
                    {
                        minY = Math.Min(minY, bbox.Bottom);
                        foundAny = true;
                    }
                }
            }
        }
        catch
        {
        }

        if (!foundAny)
        {
            try
            {
                var letters = page.Letters;
                if (letters != null && letters.Any())
                {
                    foreach (var l in letters)
                    {
                        double y = l.Location.Y;
                        minY = Math.Min(minY, y);
                    }
                    foundAny = true;
                }
            }
            catch { }
        }

        if (!foundAny)
            return pageHeight;

        double usedHeight = pageHeight - minY + marginInPoints;

        if (usedHeight > pageHeight)
            usedHeight = pageHeight;

        if (usedHeight < 20)
            usedHeight = pageHeight;

        return PointsToMm(usedHeight);
    }

    public static double PointsToMm(double points) => Math.Ceiling(points * 25.4 / 72.0);
}
