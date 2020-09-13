using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityMVC.Data;
using UniversityMVC.Models;

namespace UniversityMVC.Controllers
{
    public class GroupsController : Controller
    {
        private readonly UniversityContext _context;

        public GroupsController(UniversityContext context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            return View(await _context.Groups.Include(g => g.Courses).ToListAsync());
        }

        public async Task<IActionResult> GroupsView(int id)
        {
            var groups = await _context.Groups.Where(g => g.CourseId == id).ToListAsync();
            if (groups.Count == 0)
            {
                ViewBag.CountGroupsLessZero = "No groups found";
            }
            return View(groups);
        }


        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups.Include(g => g.Courses)
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId");
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId,CourseId,Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(group.Name))
                {
                    ModelState.AddModelError("Name", "Name cannot be empty");
                    ViewBag.CourseId = new SelectList(_context.Courses, "CourseId", "CourseId", group.CourseId);
                    return View(group);
                }
                _context.Add(group);
                await _context.SaveChangesAsync();
                TempData["GroupCreated"] = $"Group {group.Name} has been created";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", group.CourseId);
            return View(group);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", @group.CourseId);
            return View(@group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupId, Name")] Group gr)
        {
            if (id != gr.GroupId)
            {
                return NotFound();
            }

            var group = await _context.Groups.FindAsync(id);
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(gr.Name))
                    {
                        ModelState.AddModelError("Name","Name cannot be empty");
                        return View(gr);
                    }
                    group.Name = gr.Name;
                    _context.Update(group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(group.GroupId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["GroupEdit"] = $"Group {group.Name} has been changed";
                return RedirectToAction(nameof(Index));
            }
            return View(@group);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups.Include(g => g.Courses)
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @group = await _context.Groups.FindAsync(id);


            if (_context.Students.Where(e => e.GroupId == id).Count() > 0)
            {
                ViewBag.DeleteError = $"Cannot delete a group {@group.Name} that has students";
                return View(@group);
            }

            _context.Groups.Remove(@group);
            await _context.SaveChangesAsync();
            TempData["GroupDeleted"] = $"Group {@group.Name} has been deleted";
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.GroupId == id);
        }
    }
}
