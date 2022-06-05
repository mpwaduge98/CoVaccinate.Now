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
    public class VaccineCentresController : Controller
    {

        private readonly ApplicationDbContext _context;

        public VaccineCentresController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Admin/VaccineCentres
        public async Task<IActionResult> Index()
        {
            return View(await _context.VaccineCentre.ToListAsync());

        }

        // GET: Admin/VaccineCentres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccineCentre = await _context.VaccineCentre
                .FirstOrDefaultAsync(m => m.VaccineCentreId == id);
            if (vaccineCentre == null)
            {
                return NotFound();
            }

            return View(vaccineCentre);
        }

        // GET: Admin/VaccineCentres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/VaccineCentres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VaccineCentreId,VaccineCentreName,Address,ZIPCode,Email,PhoneNo,longitude,latitude")] VaccineCentre vaccineCentre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vaccineCentre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vaccineCentre);
        }

        // GET: Admin/VaccineCentres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccineCentre = await _context.VaccineCentre.FindAsync(id);
            if (vaccineCentre == null)
            {
                return NotFound();
            }
            return View(vaccineCentre);
        }

        // POST: Admin/VaccineCentres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VaccineCentreId,VaccineCentreName,Address,ZIPCode,Email,PhoneNo,longitude,latitude")] VaccineCentre vaccineCentre)
        {
            if (id != vaccineCentre.VaccineCentreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vaccineCentre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VaccineCentreExists(vaccineCentre.VaccineCentreId))
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
            return View(vaccineCentre);
        }

        // GET: Admin/VaccineCentres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccineCentre = await _context.VaccineCentre
                .FirstOrDefaultAsync(m => m.VaccineCentreId == id);
            if (vaccineCentre == null)
            {
                return NotFound();
            }

            return View(vaccineCentre);
        }

        // POST: Admin/VaccineCentres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vaccineCentre = await _context.VaccineCentre.FindAsync(id);
            _context.VaccineCentre.Remove(vaccineCentre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VaccineCentreExists(int id)
        {
            return _context.VaccineCentre.Any(e => e.VaccineCentreId == id);
        }


    }
}
