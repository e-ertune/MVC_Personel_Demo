using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Personel.Data;
using MVC_Personel.Models;

namespace MVC_Personel.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly EmployeeContext _context;

        public DepartmentsController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: Departments
        [Authorize]
        public async Task<IActionResult> Index()
        {
            List<Employee> empList = _context.Employees.ToList();
            ViewBag.empList = empList;
            return View(await _context.Departments.ToListAsync());
        }


        // GET: Departments/Create
        [Authorize]
        public IActionResult Create()
        {
            var employeeContext = _context.Employees.Include(e => e.Department).Include(e => e.Position);
            var emp = _context.Employees.Where(e => e.Username == HttpContext.User.Identity.Name).FirstOrDefault();
            var pos = _context.Positions.Find(emp.PositionID);
            var dep = _context.Departments.Find(emp.DepartmentID);
            if (dep.Name == "İnsan Kaynakları" && pos.Authority == 4)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Departments");
            }
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ID,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }


        // GET: Departments/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            var employeeContext = _context.Employees.Include(e => e.Department).Include(e => e.Position);
            var emp = _context.Employees.Where(e => e.Username == HttpContext.User.Identity.Name).FirstOrDefault();
            var pos = _context.Positions.Find(emp.PositionID);
            var dep = _context.Departments.Find(emp.DepartmentID);
            if (dep.Name == "İnsan Kaynakları" && pos.Authority == 4)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var department = await _context.Departments
                    .FirstOrDefaultAsync(m => m.ID == id);
                if (department == null)
                {
                    return NotFound();
                }

                return View(department);
            }
            else
            {
                return RedirectToAction("Index", "Departments");
            }   
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.ID == id);
        }
    }
}
