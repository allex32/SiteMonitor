using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiteMonitor.Data;
using SiteMonitor.Models;
using Microsoft.AspNetCore.Authorization;
using SiteMonitor.Services.SiteAvailability;
using SiteMonitor.Models.Repositories;
using SiteMonitor.Models.SiteViewModels;
using System.Diagnostics;

namespace SiteMonitor.Controllers
{
    [Authorize(Roles = PolicyNames.AdminRole)]
    public class SiteController : Controller
    {

        private readonly ISiteAvailabilityService _siteAvailabilityService;
       
        public SiteController(ISiteAvailabilityService siteAvailabilityService)
        {
            _siteAvailabilityService = siteAvailabilityService;
        }

        // GET: Sites
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _siteAvailabilityService.Sites);
        }


        [Authorize(Roles = PolicyNames.AdminRole)]
        // GET: Sites/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewSiteViewModel newSiteViewModel)
        {
            if (ModelState.IsValid)
            {
                await _siteAvailabilityService.CreateAndSetUpAsync(newSiteViewModel);
                return RedirectToAction(nameof(Index));
            }

            return View(newSiteViewModel);
        }

        // GET: Sites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)           
                return NotFound();


            var site = await _siteAvailabilityService.GetAsync(id);

            if (site == null)          
                return NotFound();

            EditSiteViewModel editSiteViewModel = site;
            return View(editSiteViewModel);
        }

        // POST: Sites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //To-do : заменить на DTO
        public async Task<IActionResult> Edit(int id, EditSiteViewModel site)
        {
            if (id != site.SiteId)           
                return NotFound();

            try
            {
                var originalSite = await _siteAvailabilityService.GetAsync(site.SiteId);
                if (originalSite == null)
                    return NotFound();

                if (ModelState.IsValid)
                {
                    await _siteAvailabilityService.ChangeCheckTimeSpanAsync(originalSite, site.CheckOnlineTimespan);
                    return RedirectToAction(nameof(Index));
                }
                else
                    return View(site);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _siteAvailabilityService.SiteExistsAsync(site.SiteId))
                    return NotFound();
                else
                    throw;
            }
        }

        // GET: Sites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)           
                return NotFound();

            var site = await _siteAvailabilityService.GetAsync(id);
            if (site == null)            
                return NotFound();
            
            return View(site);
        }

        // POST: Sites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {           
            await _siteAvailabilityService.DeleteSiteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
