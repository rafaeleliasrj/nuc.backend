using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.SQS;

namespace NautiHub.Core.Aws;

public class ClientAws : IClientAws
{
    private readonly string _awsEventGroup = "";
    private readonly string _awsRegion = "";

    public ClientAws()
    {
        _awsEventGroup = Environment.GetEnvironmentVariable("AWS_SQS_EVENT_GROUP")! ?? "";
        _awsRegion = Environment.GetEnvironmentVariable("AWS_REGION") ?? "us-east-1";
    }

    public string GetEventGroup() => _awsEventGroup;

    public IAmazonS3 GetClientAmazonS3()
    {
        var awsProfile = Environment.GetEnvironmentVariable("AWS_PROFILE")!;
        var awsProfileFile = Environment.GetEnvironmentVariable("AWS_CREDENTIALFILES")!;
        var awsRegion = _awsRegion;
        var awsRegionEndpoint = RegionEndpoint.GetBySystemName(awsRegion);

        AWSCredentials awsCredentials;
        CredentialProfileStoreChain chain;

        var hasCredentialFile =
            !string.IsNullOrEmpty(awsProfile) && !string.IsNullOrEmpty(awsProfileFile);
        if (hasCredentialFile)
        {
            chain = new CredentialProfileStoreChain(awsProfileFile);
            if (chain.TryGetAWSCredentials(awsProfile, out awsCredentials))
            {
                return new AmazonS3Client(awsCredentials, awsRegionEndpoint);
            }

            throw new AmazonServiceException(
                $"Credenciais não encontradas. Profile: {awsProfile}. Localização: {chain.ProfilesLocation}"
            );
        }

        var awsServiceUrl = Environment.GetEnvironmentVariable("AWS_SERVICE_URL")!;
        var awsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")!;
        var awsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")!;

        var hasCredentials =
            !string.IsNullOrEmpty(awsAccessKey) && !string.IsNullOrEmpty(awsSecretKey);
        if (!hasCredentials)
        {
            throw new AmazonServiceException(
                "Credenciais não informadas nas variáveis de ambiente: AWS_SERVICE_URL, AWS_ACCESS_KEY_ID e AWS_SECRET_ACCESS_KEY"
            );
        }

        awsCredentials = new BasicAWSCredentials(awsAccessKey, awsSecretKey);

        if (!string.IsNullOrWhiteSpace(awsServiceUrl))
        {
            return new AmazonS3Client(
                awsCredentials,
                new AmazonS3Config
                {
                    ServiceURL = awsServiceUrl!,
                    RegionEndpoint = awsRegionEndpoint
                }
            );
        }

        return new AmazonS3Client(awsCredentials, awsRegionEndpoint);
    }

    public IAmazonSQS GetClientAmazonSQS()
    {
        var awsProfile = Environment.GetEnvironmentVariable("AWS_PROFILE")!;
        var awsProfileFile = Environment.GetEnvironmentVariable("AWS_CREDENTIALFILES")!;
        var awsRegion = _awsRegion;
        var awsRegionEndpoint = RegionEndpoint.GetBySystemName(awsRegion);

        AWSCredentials awsCredentials;
        CredentialProfileStoreChain chain;

        var hasCredentialFile =
            !string.IsNullOrEmpty(awsProfile) && !string.IsNullOrEmpty(awsProfileFile);
        if (hasCredentialFile)
        {
            chain = new CredentialProfileStoreChain(awsProfileFile);
            if (chain.TryGetAWSCredentials(awsProfile, out awsCredentials))
            {
                return new AmazonSQSClient(awsCredentials, awsRegionEndpoint);
            }

            throw new AmazonServiceException(
                $"Credenciais não encontradas. Profile: {awsProfile}. Localização: {chain.ProfilesLocation}"
            );
        }

        var awsServiceUrl = Environment.GetEnvironmentVariable("AWS_SERVICE_URL")!;
        var awsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")!;
        var awsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")!;

        var hasCredentials =
            !string.IsNullOrEmpty(awsAccessKey) && !string.IsNullOrEmpty(awsSecretKey);
        if (!hasCredentials)
        {
            throw new AmazonServiceException(
                "Credenciais não informadas nas variáveis de ambiente: AWS_SERVICE_URL, AWS_ACCESS_KEY_ID e AWS_SECRET_ACCESS_KEY"
            );
        }

        awsCredentials = new BasicAWSCredentials(awsAccessKey, awsSecretKey);

        if (!string.IsNullOrWhiteSpace(awsServiceUrl))
        {
            return new AmazonSQSClient(
                awsCredentials,
                new AmazonSQSConfig
                {
                    ServiceURL = awsServiceUrl!,
                    RegionEndpoint = awsRegionEndpoint
                }
            );
        }

        return new AmazonSQSClient(awsCredentials, awsRegionEndpoint);
    }

    public string GetAwsRegion() => _awsRegion;
}
