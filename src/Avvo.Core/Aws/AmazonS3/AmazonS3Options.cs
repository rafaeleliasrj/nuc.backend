namespace Avvo.Core.Aws.AmazonS3;

public class AmazonS3Options
{
    public string DefaultBucket { get; set; } = string.Empty;
    public int MaxFileSizeMb { get; set; } = 20;
    public TimeSpan PreSignedUrlExpiry { get; set; } = TimeSpan.FromHours(1);
}
