using LM.Messages;
using LM.Responses;
using LM.Storage.Api.External.Services.Contracts;
using LM.Storage.Messages.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LM.Storage.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class StorageController : ControllerBase
    {
        IStorageExternalService StorageExternalService { get; }

        public StorageController(IStorageExternalService storageExternalService)
        {
            StorageExternalService = storageExternalService;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetBase64Async([FromQuery] Base64RequestMessage requestMessage)
            => await TryAsync(() => StorageExternalService.GetBase64Async(requestMessage));

        [HttpPost, Route("")]
        public async Task<IActionResult> UploadAsync([FromBody] UploadFileRequestMessage requestMessage)
            => await TryAsync(() => StorageExternalService.UploadAsync(requestMessage));

        async Task<IActionResult> TryAsync<TResponseMessage>(Func<Task<Response<TResponseMessage>>> func)
            where TResponseMessage : ResponseMessage
        {
            try
            {
                var response = await func.Invoke();

                if (response.HasError)
                     return BadRequest(response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}