using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Net.Http.Headers;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PhotosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult List(int id)
        {
            IEnumerable<Photo> photos = _context.Photo.Where(p => p.OfferId == id);
            return View(photos);
        }

        public IActionResult Upload(int id)
        {
            Photo photo = new Photo() { OfferId = id };
            IEnumerable<Photo> photos = _context.Photo.Where(p => p.OfferId == id);
            dynamic model = new ExpandoObject();
            model.Photo = photo;
            model.Photos = photos;
            return View("Upload", model);
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload([Bind("OfferId")] Photo photo)
        {
            try
            {

                if (ModelState.IsValid)
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

                        photo.FileAddress = folderName + "/" + offerId + "/" + fileName;

                        _context.Add(photo);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Upload", "Photos", offerId);
                    }
                    return BadRequest();
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

        public async Task<IActionResult> Delete(int id, int offerId)
        {
            var photo = await _context.Photo.FindAsync(id);
            if (photo != null)
            {
                _context.Photo.Remove(photo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Upload", "Photos", new { id = offerId });
        }
    }
}
