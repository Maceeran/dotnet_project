using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace WebApp.Controllers
{
    public class OffersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public OffersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        bool marketplaceWhereConditions(Offer o, string filterCategory, string searchString)
        {
            bool condition1 = true;
            bool condition2 = true;

            if (filterCategory != null)
            {
                Category parsedFilterCategory = (Category)Enum.Parse(typeof(Category), filterCategory);
                condition1 = o.Category == parsedFilterCategory;
            };
            if (searchString != null)
            {
                condition2 = o.Category.ToString().Contains(searchString)
                    || o.Description.Contains(searchString)
                    || o.RetrievalAddress.Contains(searchString);
            };

            return condition1 && condition2;
        }

        public async Task<IActionResult> Marketplace(string filterCategory, string searchString)
        {
            ViewData["filterCategory"] = filterCategory;
            ViewData["searchString"] = searchString;

            List<Offer> offers = await _context.Offer
                .Include(o => o.Photos)
                .Include(o => o.InterestedUsers)
                .Where(o => !o.isRealized && DateTime.Compare(o.VoidDate, DateTime.Now) > 0)
                .ToListAsync();

            List<Offer> filteredOffers = new List<Offer>();
            foreach(Offer o in offers)
            {
                if(marketplaceWhereConditions(o, filterCategory, searchString))
                {
                    filteredOffers.Add(o);
                }
            }
            return View(filteredOffers);
        }
        public async Task<IActionResult> ToggleOfferInterest(string offerId)
        {
            var currentUser = this.User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentUser == null) return View(Consts.UnauthErrorPagePath);

            UserInterestedOffer userInterestedOffer = await _context.UserInterestedOffer
                .Where(uio => uio.OfferId == int.Parse(offerId) && uio.UserId == currentUser.Value)
                .FirstOrDefaultAsync();

            if (userInterestedOffer == null)
            {
                userInterestedOffer = new UserInterestedOffer();
                userInterestedOffer.OfferId = int.Parse(offerId);
                userInterestedOffer.UserId = currentUser.Value;
                userInterestedOffer.User = await _userManager.FindByIdAsync(currentUser.Value);
                _context.Add(userInterestedOffer);
            } else
            {
                _context.Remove(userInterestedOffer);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Marketplace");
        }

        // GET: Offers
        public async Task<IActionResult> Index()
        {
            var currentUser = this.User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentUser == null) return View(Consts.UnauthErrorPagePath);
            
            return _context.Offer != null ?
                View(
                    await _context.Offer
                        .Include(o => o.Photos)
                        .Where(o => o.UserId == currentUser.Value)
                        .ToListAsync()
                )
                :   Problem("Entity set 'ApplicationDbContext.Offer'  is null.");
        }

        // GET: Offers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var currentUser = this.User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentUser == null) return View(Consts.UnauthErrorPagePath);

            if (id == null || _context.Offer == null)
            {
                return NotFound();
            }

            var offer = await _context.Offer
                .Include(o => o.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // GET: Offers/Create
        public IActionResult Create()
        {
            var currentUser = this.User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentUser == null) return View(Consts.UnauthErrorPagePath);

            return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Description,RetrievalAddress,VoidDate,Category")] Offer offer)
        {
            var currentUser = this.User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentUser == null) return View(Consts.UnauthErrorPagePath);

            if (ModelState.IsValid)
            {
                offer.UserId = currentUser.Value;
                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(offer);
        }

        // GET: Offers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var currentUser = this.User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentUser == null) return View(Consts.UnauthErrorPagePath);

            if (id == null || _context.Offer == null)
            {
                return NotFound();
            }


            var offer = await _context.Offer
                .Include(o => o.InterestedUsers)
                .FirstOrDefaultAsync(o => o.Id == id);
            
            if (offer == null)
            {
                return NotFound();
            }

            var interestedUsers = await _context.UserInterestedOffer
                .Where(uio => uio.OfferId == offer.Id)
                .ToListAsync();

            var interestedUsersMapping = new List<(string, string)>();
            foreach (var interestedUser in interestedUsers)
            {
                interestedUsersMapping.Add((interestedUser.UserId, (await _userManager.FindByIdAsync(interestedUser.UserId)).Email));
            }
            ViewBag.interestedUsersMapping = interestedUsersMapping;

            return View(offer);
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,RetrievalAddress,VoidDate,isRealized")] Offer offer, int reservedInterestedOfferId)
        {
            var currentUser = this.User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentUser == null) return View(Consts.UnauthErrorPagePath);

            if (id != offer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (reservedInterestedOfferId != null)
                    {
                        List<UserInterestedOffer> userInterestedOffers = await _context.UserInterestedOffer
                            .Where(uio => uio.OfferId == offer.Id)
                            .ToListAsync();

                        foreach (UserInterestedOffer elem in userInterestedOffers)
                        {
                            if (elem.Id == reservedInterestedOfferId)
                            {
                                elem.ReservedForUser = true;
                            }
                            else
                            {
                                elem.ReservedForUser = false;
                            }
                            _context.Update(elem);
                        }
                    }

                    offer.UserId = currentUser.Value;
                    _context.Update(offer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferExists(offer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(offer);
        }

        // GET: Offers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var currentUser = this.User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentUser == null) return View(Consts.UnauthErrorPagePath);

            if (id == null || _context.Offer == null)
            {
                return NotFound();
            }

            var offer = await _context.Offer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentUser = this.User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentUser == null) return View(Consts.UnauthErrorPagePath);

            if (_context.Offer == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Offer'  is null.");
            }
            var offer = await _context.Offer.FindAsync(id);
            if (offer != null)
            {
                _context.Offer.Remove(offer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfferExists(int id)
        {
          return (_context.Offer?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
