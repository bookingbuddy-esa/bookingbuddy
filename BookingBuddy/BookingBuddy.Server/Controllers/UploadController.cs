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
    /// <summary>
    /// Controller para enviar ficheiros para o armazenamento Azure.
    /// </summary>
    [Route("api/upload")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly BlobServiceClient _blobServiceClient;

        /// <summary>
        /// Construtor para o controller de upload.
        /// </summary>
        /// <param name="configuration"></param>
        public UploadController(IConfiguration configuration)
        {
            _blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("AzureStorageAccount"));
        }


        /// <summary>
        /// Faz o upload de ficheiros para o armazenamento de blobs.
        /// </summary>
        /// <returns>
        /// Um código de estado 200 (OK) juntamente com os URLs dos ficheiros carregados, se o carregamento for bem-sucedido.
        /// Um código de estado 400 (Pedido Inválido) se nenhum ficheiro for enviado na solicitação.
        /// </returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadFiles()
        {
            if (Request.Form.Files.Count >= 1)
            {
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