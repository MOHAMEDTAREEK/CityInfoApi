﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;
        public FilesController(
             FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                ?? throw new System.ArgumentNullException(
                    nameof(fileExtensionContentTypeProvider));
        }


        [HttpGet("{fileId}")]
        public ActionResult GetFiles(string fileId)
        {
           var filePath = "getting-started-with-rest-slides.pdf";
           if(!System.IO.File.Exists(filePath))
            {
               return NotFound();
           }
            if (!_fileExtensionContentTypeProvider.TryGetContentType(
                 filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }


            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, contentType, Path.GetFileName(filePath));
        }
        public async Task<ActionResult> CreateFile(IFormFile file)
        {
            // Validate the input. Put a limit on filesize to avoid large uploads attacks. 
            // Only accept .pdf files (check content-type)
            if (file.Length == 0 || file.Length > 20971520 || file.ContentType != "application/pdf")
            {
                return BadRequest("No file or an invalid one has been inputted.");
            }

            // Create the file path.  Avoid using file.FileName, as an attacker can provide a
            // malicious one, including full paths or relative paths.  
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                $"uploaded_file_{Guid.NewGuid()}.pdf");

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok("Your file has been uploaded successfully.");
        }

    }
}
