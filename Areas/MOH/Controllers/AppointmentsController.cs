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
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MOH/Appointments
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppointmentModel.ToListAsync());
        }


        // GET: MOH/Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentModel = await _context.AppointmentModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointmentModel == null)
            {
                return NotFound();
            }

            return View(appointmentModel);
        }

        // GET: MOH/Appointments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MOH/Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,DOB,Gender,NIC,Address,PhoneNumber,AppointmentDate,VaccineName,VaccineDose,VaccineCentreName")] AppointmentModel appointmentModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointmentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appointmentModel);
        }

        // GET: MOH/Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentModel = await _context.AppointmentModel.FindAsync(id);
            if (appointmentModel == null)
            {
                return NotFound();
            }
            return View(appointmentModel);
        }

        // POST: MOH/Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,DOB,Gender,NIC,Address,PhoneNumber,AppointmentDate,VaccineName,VaccineDose,VaccineCentreName")] AppointmentModel appointmentModel)
        {
            if (id != appointmentModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointmentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentModelExists(appointmentModel.Id))
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
            return View(appointmentModel);
        }

        // GET: MOH/Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentModel = await _context.AppointmentModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointmentModel == null)
            {
                return NotFound();
            }

            return View(appointmentModel);
        }

        // POST: MOH/Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointmentModel = await _context.AppointmentModel.FindAsync(id);
            _context.AppointmentModel.Remove(appointmentModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentModelExists(int id)
        {
            return _context.AppointmentModel.Any(e => e.Id == id);
        }
    }
}
