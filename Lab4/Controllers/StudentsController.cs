using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab4.Data;
using Lab4.Models;
using Lab4.Models.ViewModels;
namespace Lab4.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolCommunityContext _context;

        public StudentsController(SchoolCommunityContext context)
        {
            _context = context;
        }

        // GET: Communities
        public async Task<IActionResult> Index(int id)
        {
            var viewModel = new StudentViewModel();
            viewModel.Students = await _context.Students
                .Include(i => i.CommunityMemberships)
                //.ThenInclude(j => j.Student)
                .AsNoTracking()
                .OrderBy(i => i.Id)
                .ToListAsync();
            viewModel.Communities = await _context.Communities
                .Include(j => j.CommunityMemberships)
                .AsNoTracking()
                .OrderBy(j => j.Id)
                .ToListAsync();



            if (id != 0)
            {
                ViewData["sId"] = id;

                viewModel.CommunityMemberships = viewModel.Students.Where(
                    x => x.Id == id).Single().CommunityMemberships;
                var s = viewModel.CommunityMemberships.Where(z => z.StudentId == id).Select(y => y.CommunityId);
                string[] community = s.ToArray();
                if (community.Length! > 0)
                {
                    for (int a = 0; a < community.Length; a++)
                    {
                        string communityNumber = community[a];
                        ViewData["CommunityId"] = communityNumber;

                        //viewModel.Students = viewModel.CommunityMemberships.Where(cm=>cm.StudentId==studentNumber).Single().Students;
                        //viewModel.Students = viewModel.CommunityMemberships.Where(cm => cm.StudentId == studentNumber).Single().Students;
                        //viewModel.CommunityMemberships = viewModel.Students.Where(stu => stu.Id == studentNumber).Single().CommunityMemberships;


                    }
                    //int studentNumber = si[0];
                    //ViewData["StudentId"] = studentNumber;
                }
            }
            return View(viewModel);

        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,EnrollmentDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,EnrollmentDate")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
