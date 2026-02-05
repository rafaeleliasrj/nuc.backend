using Amazon.S3;
using Amazon.SQS;

namespace NautiHub.Core.Aws;

public interface IClientAws
{
    public string GetAwsRegion();

    public string GetEventGroup();

    public IAmazonS3 GetClientAmazonS3();

    public IAmazonSQS GetClientAmazonSQS();
}
