using Amazon.S3.Transfer;

namespace LM.Storage.Api.External.Factories.Contracts
{
    public interface ITransferUtilityFactory
    {
        TransferUtility Create();
    }
}