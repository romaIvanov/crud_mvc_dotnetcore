using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityMVC.Data;
using UniversityMVC.Models;

namespace UniversityMVC.Controllers
{
    public class StudentsController : Controller
    {
        private readonly UniversityContext _context;

        public StudentsController(UniversityContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.Include(s => s.Groups).ToListAsync());
        }

        public async Task<IActionResult> StudentsView(int id)
        {
            var students = await _context.Students.Include(s => s.Groups).Where(s => s.GroupId == id).ToListAsync();

            if (students.Count == 0)
            {
                var group = await _context.Groups.FindAsync(id);
                TempData["CountStudentsError"] = $"No students found in group {group.Name}";
                return RedirectToAction("Index", "Groups");
            }
            return View(students);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Groups)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,GroupId,FirstName,LastName")] Student student)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(student.FirstName) || string.IsNullOrEmpty(student.LastName))
                {
                    ModelState.AddModelError("FirstName", "Firstname cannot be empty");
                    ModelState.AddModelError("LastName", "Lastname cannot be empty");
                    ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId", student.GroupId);
                    return View(student);
                }
                _context.Add(student);
                await _context.SaveChangesAsync();
                TempData["StudentCreated"] = $"Student {student.FirstName} {student.LastName} has been created";
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId", student.GroupId);
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
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId", student.GroupId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FirstName,LastName")] Student st)
        {
            if (id != st.StudentId)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(st.FirstName) || string.IsNullOrEmpty(st.LastName))
                    {
                        ModelState.AddModelError("FirstName", "Firstname cannot be empty");
                        ModelState.AddModelError("LastName", "Lastname cannot be empty");
                        return View(st);
                    }
                    student.FirstName = st.FirstName;
                    student.LastName = st.LastName;
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["StudentEdit"] = $"Student {student.FirstName} {student.LastName} has been changed";
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
                .Include(s => s.Groups)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            TempData["StudentDeleted"] = $"Student {student.LastName} {student.FirstName} has been deleted";
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
