using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoVaccinate.now.Data;
using CoVaccinate.now.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoVaccinate.now.Areas.MOH.Controllers
{
    [Authorize(Roles = "MOH")]
    [Area("MOH")]
    public class QuotasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuotasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MOH/Quotas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Quota.Include(q => q.VaccineCentre);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MOH/Quotas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quota = await _context.Quota
                .Include(q => q.VaccineCentre)
                .FirstOrDefaultAsync(m => m.QuotaID == id);
            if (quota == null)
            {
                return NotFound();
            }

            return View(quota);
        }

        // GET: MOH/Quotas/Create
        public IActionResult Create()
        {
            ViewData["VaccineCentreId"] = new SelectList(_context.VaccineCentre, "VaccineCentreId", "VaccineCentreName");
            return View();
        }

        // POST: MOH/Quotas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuotaID,VaccineCentreId,OpenDays,OpenHours")] Quota quota)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quota);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VaccineCentreId"] = new SelectList(_context.VaccineCentre, "VaccineCentreId", "VaccineCentreName", quota.VaccineCentreId);
            return View(quota);
        }

        // GET: MOH/Quotas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quota = await _context.Quota.FindAsync(id);
            if (quota == null)
            {
                return NotFound();
            }
            ViewData["VaccineCentreId"] = new SelectList(_context.VaccineCentre, "VaccineCentreId", "VaccineCentreName", quota.VaccineCentreId);
            return View(quota);
        }

        // POST: MOH/Quotas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuotaID,VaccineCentreId,OpenDays,OpenHours")] Quota quota)
        {
            if (id != quota.QuotaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quota);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuotaExists(quota.QuotaID))
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
            ViewData["VaccineCentreId"] = new SelectList(_context.VaccineCentre, "VaccineCentreId", "VaccineCentreName", quota.VaccineCentreId);
            return View(quota);
        }

        // GET: MOH/Quotas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quota = await _context.Quota
                .Include(q => q.VaccineCentre)
                .FirstOrDefaultAsync(m => m.QuotaID == id);
            if (quota == null)
            {
                return NotFound();
            }

            return View(quota);
        }

        // POST: MOH/Quotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quota = await _context.Quota.FindAsync(id);
            _context.Quota.Remove(quota);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuotaExists(int id)
        {
            return _context.Quota.Any(e => e.QuotaID == id);
        }
    }
}
