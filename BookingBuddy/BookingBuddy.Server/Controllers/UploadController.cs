using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Azure.Storage.Blobs;
using System.Text;
using Azure.Storage;
using Azure.Storage.Sas;

namespace BookingBuddy.Server.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly BlobServiceClient _blobServiceClient;
        public UploadController(IConfiguration configuration)
        {
            var credential = new StorageSharedKeyCredential("bookingbuddystorage", configuration.GetSection("AzureStorageAccountKey").Value!);
            var serviceUri = new Uri($"https://bookingbuddystorage.blob.core.windows.net");
            _blobServiceClient = new BlobServiceClient(serviceUri, credential);
        }


        /// <summary>
        /// escrever
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadFiles()
        {
            if(Request.Form.Files.Count >= 1){
                var containerClient = _blobServiceClient.GetBlobContainerClient("images");
                var files = Request.Form.Files;
                var response = new List<string>();

                foreach (var file in files)
                {
                    var sanitizedFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var blobClient = containerClient.GetBlobClient(sanitizedFileName);
                    await blobClient.UploadAsync(file.OpenReadStream(), true);
                    response.Add(blobClient.Uri.ToString());
                    Console.WriteLine(blobClient.Uri.ToString());
                }

                return Ok(response);
            }

            return BadRequest();
        }
    }
}
