using LM.Responses;
using LM.Storage.Messages.Requests;
using LM.Storage.Messages.Responses;
using System.Threading.Tasks;

namespace LM.Storage.Api.External.Services.Contracts
{
    public interface IStorageExternalService
    {
        Task<Response<Base64ResponseMessage>> GetBase64Async(Base64RequestMessage requestMessage);

        Task<Response<UploadFileResponseMessage>> UploadAsync(UploadFileRequestMessage requestMessage);
    }
}