using Amazon.S3;
using Amazon.S3.Transfer;
using LM.Storage.Api.External.Factories.Contracts;
using Microsoft.Extensions.Configuration;

namespace LM.Storage.Api.External.Factories
{
    public class TransferUtilityFactory : ITransferUtilityFactory
    {
        IConfiguration Configuration { get; }

        public TransferUtilityFactory(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public TransferUtility Create()
        {
            var section = Configuration.GetSection("StorageAws:ConfigSection").Value;

            var options = Configuration.GetAWSOptions(section);

            var clientS3 = options.CreateServiceClient<IAmazonS3>();

            return new TransferUtility(clientS3);
        }
    }
}