using Amazon.S3.Transfer;
using LM.Responses;
using LM.Responses.Extensions;
using LM.Storage.Api.External.Factories.Contracts;
using LM.Storage.Api.External.Services.Contracts;
using LM.Storage.Api.Settings;
using LM.Storage.Messages.Requests;
using LM.Storage.Messages.Responses;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LM.Storage.Api.External.Services
{
    public class AS3ExternalService : IStorageExternalService
    {
        TransferUtility TransferUtility { get; }

        IOptions<StorageSettings> Options { get; }

        public AS3ExternalService(ITransferUtilityFactory transferUtilityFactory
            , IOptions<StorageSettings> options)
        {
            TransferUtility = transferUtilityFactory.Create();
            Options = options;
        }

        public async Task<Response<Base64ResponseMessage>> GetBase64Async(Base64RequestMessage requestMessage)
        {
            var response = Response<Base64ResponseMessage>.Create();

            if (response.WithMessages(ValidateRequest(requestMessage).Messages).HasError)
                return response;

            using (var stream = await TransferUtility.OpenStreamAsync(requestMessage.BucketName, $"{requestMessage.FolderName}/{requestMessage.FileName}"))
            {
                byte[] bytes;

                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    bytes = memoryStream.ToArray();
                }

                var images = new List<string> { "jpg", "jpeg", "gif", "png", "bmp" };

                if (requestMessage.FileExtension.Contains("pdf"))
                    response.Data.Value.Base64 = $"data:application/pdf;base64,{Convert.ToBase64String(bytes)}";
                else if (images.Contains(requestMessage.FileExtension.Replace(".", "")))
                    response.Data.Value.Base64 = $"data:image/{requestMessage.FileExtension.Replace(".", "")};base64,{Convert.ToBase64String(bytes)}";
                else
                    response.Data.Value.Base64 = $"data:application/octet-stream;base64,{Convert.ToBase64String(bytes)}";
            }

            response.Data.Value.BucketName = requestMessage.BucketName;
            response.Data.Value.FolderName = requestMessage.FolderName;
            response.Data.Value.FileName = requestMessage.FileName;
            response.Data.Value.FileExtension = requestMessage.FileExtension;
            response.Data.Value.Link = $"{Options.Value.StorageAws.BaseUrl}/{requestMessage.FolderName}/{requestMessage.FileName}";

            return response;
        }

        Response ValidateRequest(Base64RequestMessage requestMessage)
        {
            var response = Response.Create();

            if (requestMessage == null)
                requestMessage = new Base64RequestMessage(Guid.NewGuid());

            if (string.IsNullOrEmpty(requestMessage.BucketName))
                response.WithBusinessError(nameof(requestMessage.BucketName), "O nome do bucket é obrigatório.");

            if (string.IsNullOrEmpty(requestMessage.BucketName))
                response.WithBusinessError(nameof(requestMessage.BucketName), "O nome do bucket é obrigatório.");

            if (string.IsNullOrEmpty(requestMessage.FolderName))
                response.WithBusinessError(nameof(requestMessage.FolderName), "O nome do folder é obrigatório.");

            if (string.IsNullOrEmpty(requestMessage.FileExtension))
                response.WithBusinessError(nameof(requestMessage.FileExtension), "A extensão do arquivo é obrigatória.");

            return response;
        }

        public async Task<Response<UploadFileResponseMessage>> UploadAsync(UploadFileRequestMessage requestMessage)
        {
            var response = Response<UploadFileResponseMessage>.Create(new UploadFileResponseMessage(Guid.NewGuid()));

            foreach (var file in requestMessage.Files)
            {
                if (string.IsNullOrEmpty(file.FolderName))
                    return response.WithBusinessError(nameof(file.FolderName), "O nome do folder é obrigatório.");

                var bytes = Convert.FromBase64String(Regex.Replace(file.Base64, "data:.*.base64,", ""));

                var memoryStream = new MemoryStream(bytes, 0, bytes.Length);

                await TransferUtility.UploadAsync(memoryStream, $"{Options.Value.StorageAws.BucketName}/{file.FolderName}", file.Name);

                response.Data.Value.Files.Add(new UploadFileResponseMessage.FileResponseMessage
                {
                    Name = file.Name,
                    UploadFileURL = $"{Options.Value.StorageAws.BaseUrl}/{file.FolderName}/{file.Name}"
                });
            }

            return response;
        }
    }
}