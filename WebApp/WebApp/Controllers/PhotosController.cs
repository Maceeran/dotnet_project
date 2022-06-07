using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IActionResult Upload(int id)
        {
            Photo model = new Photo() { OfferId = id };
            return View("Upload", model);
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload(int id, Photo photo)
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = "images";
                var offerId = photo.OfferId;
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName, offerId.ToString());
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    // If folder doesn't exist - create it
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
