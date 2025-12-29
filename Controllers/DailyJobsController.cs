using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DailyJobTracker.Data;
using DailyJobTracker.Models;

namespace DailyJobTracker.Controllers
{
    public class DailyJobsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DailyJobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dashboard (Landing Page)
        public async Task<IActionResult> Dashboard()
        {
            var jobs = await _context.DailyJobs
                                     .OrderByDescending(j => j.WorkDate)
                                     .ToListAsync();

            ViewBag.TotalJobs = jobs.Count;
            ViewBag.TotalTime = jobs.Where(j => j.TimeTrack.HasValue)
                                    .Aggregate(TimeSpan.Zero, (sum, j) => sum + j.TimeTrack.Value);

            var statuses = await _context.Statuses.ToListAsync();

            var statusCounts = statuses
                .Select(s => new
                {
                    StatusName = s.StatusName,
                    Count = jobs.Count(j => j.Status == s.StatusName)
                })
                .ToList();

            ViewBag.StatusCounts = statusCounts;

            return View(jobs);
        }

        // GET: DailyJobs (Job History)
        public async Task<IActionResult> Index(DateTime? workDate, string engineerName, string statusFilter, string companyName)
        {
            var jobs = _context.DailyJobs.AsQueryable();

            if (workDate.HasValue)
                jobs = jobs.Where(j => j.WorkDate == workDate.Value.Date);

            if (!string.IsNullOrEmpty(engineerName))
                jobs = jobs.Where(j => j.EngineerName == engineerName);

            if (!string.IsNullOrEmpty(statusFilter))
                jobs = jobs.Where(j => j.Status == statusFilter);

            if (!string.IsNullOrEmpty(companyName))
                jobs = jobs.Where(j => j.CompanyName == companyName);

            jobs = jobs.OrderByDescending(j => j.WorkDate);

            ViewBag.Engineers = await _context.EngineerNames
                .Select(e => e.Name)
                .OrderBy(n => n)
                .ToListAsync();

            ViewBag.Statuses = await _context.Statuses
                .Select(s => s.StatusName)
                .OrderBy(n => n)
                .ToListAsync();

            ViewBag.Companies = await _context.Companies
                .Select(c => c.Name)
                .OrderBy(n => n)
                .ToListAsync();

            ViewBag.SelectedEngineer = engineerName;
            ViewBag.SelectedStatus = statusFilter;
            ViewBag.SelectedCompany = companyName;
            ViewBag.SelectedWorkDate = workDate?.ToString("yyyy-MM-dd");

            return View(await jobs.ToListAsync());
        }

        // GET: DailyJobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var dailyJob = await _context.DailyJobs.FirstOrDefaultAsync(m => m.Id == id);
            if (dailyJob == null) return NotFound();

            return View(dailyJob);
        }

        // GET: DailyJobs/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Shifts = await _context.Shifts.ToListAsync();
            ViewBag.Engineers = await _context.EngineerNames.ToListAsync();

            // Distinct categories only
            ViewBag.Categories = await _context.Categories
                .Select(c => c.CategoryName)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            ViewBag.CaseTypes = await _context.CaseTypes.ToListAsync();
            ViewBag.CasePriorities = await _context.CasePriorities.ToListAsync();
            ViewBag.Companies = await _context.Companies.ToListAsync();
            ViewBag.Statuses = await _context.Statuses.ToListAsync();

            var model = new DailyJob
            {
                WorkDate = DateTime.Today
            };
            return View(model);
        }

        // AJAX: Get subcategories for a selected category
        [HttpGet]
        public async Task<JsonResult> GetSubCategories(string categoryName)
        {
            var subcategories = await _context.Categories
                .Where(c => c.CategoryName == categoryName)
                .Select(c => c.SubCategoryName)
                .Distinct()
                .OrderBy(sc => sc)
                .ToListAsync();

            return Json(subcategories);
        }

        // POST: DailyJobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DailyJob dailyJob)
        {
            if (ModelState.IsValid)
            {
                dailyJob.CreatedAt = DateTime.Now;
                dailyJob.UpdatedAt = DateTime.Now;

                _context.Add(dailyJob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns if validation fails
            ViewBag.Categories = await _context.Categories
                .Select(c => c.CategoryName)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            return View(dailyJob);
        }

        // GET: DailyJobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var dailyJob = await _context.DailyJobs.FindAsync(id);
            if (dailyJob == null) return NotFound();

            ViewBag.Shifts = await _context.Shifts.ToListAsync();
            ViewBag.Engineers = await _context.EngineerNames.ToListAsync();
            ViewBag.Categories = await _context.Categories
                .Select(c => c.CategoryName)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
            ViewBag.CaseTypes = await _context.CaseTypes.ToListAsync();
            ViewBag.CasePriorities = await _context.CasePriorities.ToListAsync();
            ViewBag.Companies = await _context.Companies.ToListAsync();
            ViewBag.Statuses = await _context.Statuses.ToListAsync();

            return View(dailyJob);
        }

        // POST: DailyJobs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DailyJob dailyJob)
        {
            if (id != dailyJob.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingJob = await _context.DailyJobs.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id);
                    if (existingJob == null) return NotFound();

                    dailyJob.CreatedAt = existingJob.CreatedAt;
                    dailyJob.UpdatedAt = DateTime.Now;

                    _context.Update(dailyJob);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyJobExists(dailyJob.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dailyJob);
        }

        // GET: DailyJobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dailyJob = await _context.DailyJobs.FirstOrDefaultAsync(m => m.Id == id);
            if (dailyJob == null) return NotFound();

            return View(dailyJob);
        }

        // POST: DailyJobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dailyJob = await _context.DailyJobs.FindAsync(id);
            if (dailyJob != null)
            {
                _context.DailyJobs.Remove(dailyJob);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DailyJobExists(int id)
        {
            return _context.DailyJobs.Any(e => e.Id == id);
        }
    }
}
