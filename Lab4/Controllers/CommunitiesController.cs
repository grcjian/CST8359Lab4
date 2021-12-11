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
    public class CommunitiesController : Controller
    {
        private readonly SchoolCommunityContext _context;

        public CommunitiesController(SchoolCommunityContext context)
        {
            _context = context;
        }

        // GET: CommunityMemberships
        public async Task<IActionResult> Index(string id)
        {
            
            var viewModel = new CommunityViewModel();
            viewModel.Communities = await _context.Communities
                .Include(i => i.CommunityMemberships)
                //.ThenInclude(j => j.Student)
                .AsNoTracking()
                .OrderBy(i => i.Id)
                .ToListAsync();
            viewModel.Students = await _context.Students
                .Include(j => j.CommunityMemberships)
                .AsNoTracking()
                .OrderBy(j => j.Id)
                .ToListAsync();



            if (id != null)
            {
                ViewData["cId"] = id;
                viewModel.CommunityMemberships = viewModel.Communities.Where(
                    x => x.Id == id).Single().CommunityMemberships;
                var s = viewModel.CommunityMemberships.Where(z => z.CommunityId == id).Select(y=>y.StudentId);
                int[] si = s.ToArray();
                if(si.Length!>0)
                {
                    for(int a = 0; a < si.Length; a++)
                    {
                        int studentNumber = si[a];
                        ViewData["StudntId"] = studentNumber;
                        
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

        // GET: Communities/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var community = await _context.Communities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (community == null)
            {
                return NotFound();
            }

            return View(community);
        }

        // GET: Communities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Communities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Budget")] Community community)
        {
            if (ModelState.IsValid)
            {
                _context.Add(community);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(community);
        }

        // GET: Communities/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var community = await _context.Communities.FindAsync(id);
            if (community == null)
            {
                return NotFound();
            }
            return View(community);
        }

        // POST: Communities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Budget")] Community community)
        {
            if (id != community.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(community);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommunityExists(community.Id))
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
            return View(community);
        }

        // GET: Communities/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var community = await _context.Communities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (community == null)
            {
                return NotFound();
            }

            return View(community);
        }

        // POST: Communities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var community = await _context.Communities.FindAsync(id);
            _context.Communities.Remove(community);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommunityExists(string id)
        {
            return _context.Communities.Any(e => e.Id == id);
        }
    }
}
