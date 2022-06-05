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
    public class VaccineDosesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VaccineDosesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/VaccineDoses
        public async Task<IActionResult> Index()
        {
            return View(await _context.VaccineDose.ToListAsync());
        }

        // GET: Admin/VaccineDoses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccineDose = await _context.VaccineDose
                .FirstOrDefaultAsync(m => m.VaccineDoseId == id);
            if (vaccineDose == null)
            {
                return NotFound();
            }

            return View(vaccineDose);
        }

        // GET: Admin/VaccineDoses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/VaccineDoses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VaccineDoseId,VaccineDoses")] VaccineDose vaccineDose)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vaccineDose);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vaccineDose);
        }

        // GET: Admin/VaccineDoses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccineDose = await _context.VaccineDose.FindAsync(id);
            if (vaccineDose == null)
            {
                return NotFound();
            }
            return View(vaccineDose);
        }

        // POST: Admin/VaccineDoses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VaccineDoseId,VaccineDoses")] VaccineDose vaccineDose)
        {
            if (id != vaccineDose.VaccineDoseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vaccineDose);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VaccineDoseExists(vaccineDose.VaccineDoseId))
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
            return View(vaccineDose);
        }

        // GET: Admin/VaccineDoses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccineDose = await _context.VaccineDose
                .FirstOrDefaultAsync(m => m.VaccineDoseId == id);
            if (vaccineDose == null)
            {
                return NotFound();
            }

            return View(vaccineDose);
        }

        // POST: Admin/VaccineDoses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vaccineDose = await _context.VaccineDose.FindAsync(id);
            _context.VaccineDose.Remove(vaccineDose);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VaccineDoseExists(int id)
        {
            return _context.VaccineDose.Any(e => e.VaccineDoseId == id);
        }
    }
}
