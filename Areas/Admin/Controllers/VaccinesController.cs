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

namespace CoVaccinate.now.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    public class VaccinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VaccinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Vaccines
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vaccine.Include(v => v.AgeGroup).Include(v => v.VaccineDose);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Vaccines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccine = await _context.Vaccine
                .Include(v => v.AgeGroup)
                .Include(v => v.VaccineDose)
                .FirstOrDefaultAsync(m => m.VaccineID == id);
            if (vaccine == null)
            {
                return NotFound();
            }

            return View(vaccine);
        }

        // GET: Admin/Vaccines/Create
        public IActionResult Create()
        {
            ViewData["AgeGroupId"] = new SelectList(_context.AgeGroup, "AgeGroupId", "RecommendedAgeGroup");
            ViewData["VaccineDoseId"] = new SelectList(_context.VaccineDose, "VaccineDoseId", "VaccineDoses");
            return View();
        }

        // POST: Admin/Vaccines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VaccineID,VaccineName,AgeGroupId,VaccineDoseId,SpecialNote")] Vaccine vaccine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vaccine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AgeGroupId"] = new SelectList(_context.AgeGroup, "AgeGroupId", "RecommendedAgeGroup", vaccine.AgeGroupId);
            ViewData["VaccineDoseId"] = new SelectList(_context.VaccineDose, "VaccineDoseId", "VaccineDoses", vaccine.VaccineDoseId);
            return View(vaccine);
        }

        // GET: Admin/Vaccines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccine = await _context.Vaccine.FindAsync(id);
            if (vaccine == null)
            {
                return NotFound();
            }
            ViewData["AgeGroupId"] = new SelectList(_context.AgeGroup, "AgeGroupId", "RecommendedAgeGroup", vaccine.AgeGroupId);
            ViewData["VaccineDoseId"] = new SelectList(_context.VaccineDose, "VaccineDoseId", "VaccineDoses", vaccine.VaccineDoseId);
            return View(vaccine);
        }

        // POST: Admin/Vaccines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VaccineID,VaccineName,AgeGroupId,VaccineDoseId,SpecialNote")] Vaccine vaccine)
        {
            if (id != vaccine.VaccineID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vaccine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VaccineExists(vaccine.VaccineID))
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
            ViewData["AgeGroupId"] = new SelectList(_context.AgeGroup, "AgeGroupId", "RecommendedAgeGroup", vaccine.AgeGroupId);
            ViewData["VaccineDoseId"] = new SelectList(_context.VaccineDose, "VaccineDoseId", "VaccineDoses", vaccine.VaccineDoseId);
            return View(vaccine);
        }

        // GET: Admin/Vaccines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccine = await _context.Vaccine
                .Include(v => v.AgeGroup)
                .Include(v => v.VaccineDose)
                .FirstOrDefaultAsync(m => m.VaccineID == id);
            if (vaccine == null)
            {
                return NotFound();
            }

            return View(vaccine);
        }

        // POST: Admin/Vaccines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vaccine = await _context.Vaccine.FindAsync(id);
            _context.Vaccine.Remove(vaccine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VaccineExists(int id)
        {
            return _context.Vaccine.Any(e => e.VaccineID == id);
        }
    }
}
