using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Personel.Data;
using MVC_Personel.Models;

namespace MVC_Personel.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeContext _context;

        public EmployeesController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: Employees
        [Authorize]
        public IActionResult Index()
        {
            var employeeContext = _context.Employees.Include(e => e.Department).Include(e => e.Position);
            var emp = _context.Employees.Where(e => e.Username == HttpContext.User.Identity.Name).FirstOrDefault();
            var pos = _context.Positions.Find(emp.PositionID);
            var dep = _context.Departments.Find(emp.DepartmentID);
            List<Employee> list;
            if (pos.Authority < 1)
            {
                return RedirectToAction("Index", "Departments");
            }
            if (emp.DepartmentID == 1 && pos.Authority >= 2)
            {
                list = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position).ToList();
            }
            else
            {
                list = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position).Where(e => e.DepartmentID == emp.DepartmentID).ToList();
            }
            return View(list);
        }

        // GET: Employees/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employee == null)
            {
                return NotFound();
            }
            var emp = _context.Employees.Where(e => e.Username == HttpContext.User.Identity.Name).FirstOrDefault();
            var pos = _context.Positions.Find(emp.PositionID);
            var dep = _context.Departments.Find(emp.DepartmentID);
            if (pos.Authority < 2)
            {
                return RedirectToAction("Index", "Employees");
            }

            return View(employee);
        }

        // GET: Employees/Create
        [Authorize]
        public IActionResult Create()
        {
            var employeeContext = _context.Employees.Include(e => e.Department).Include(e => e.Position);
            var emp = _context.Employees.Where(e => e.Username == HttpContext.User.Identity.Name).FirstOrDefault();
            var pos = _context.Positions.Find(emp.PositionID);
            var dep = _context.Departments.Find(emp.DepartmentID);
            if (dep.Name == "İnsan Kaynakları" && pos.Authority >= 3)
            {
                ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "Name");
                ViewData["PositionID"] = new SelectList(_context.Positions, "ID", "Name");
                return View();
            }
            else
            {
                ViewData["Hata"] = "Yeni kayıt yapmaya yetkiniz yok.";
                return RedirectToAction("Index", "Employees");
            }
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ID,LastName,FirstName,IdentityNumber,PhoneNumber,Gender,Username,Password,Address,RegistirationNumber,DateOfStart,Active,DateOfLeave,DepartmentID,PositionID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Password = EncryptPassword(employee.Password);
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "Name", employee.DepartmentID);
            ViewData["PositionID"] = new SelectList(_context.Positions, "ID", "Name", employee.PositionID);
            return View(employee);
        }

        // GET: Employees/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            var employeeContext = _context.Employees.Include(e => e.Department).Include(e => e.Position);
            var emp = _context.Employees.Where(e => e.Username == HttpContext.User.Identity.Name).FirstOrDefault();
            var pos = _context.Positions.Find(emp.PositionID);
            var dep = _context.Departments.Find(emp.DepartmentID);
            if ((dep.Name == "Bilgi Teknolojileri" && pos.Authority >= 2) || (dep.Name == "İnsan Kaynakları" && pos.Authority >= 3))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "Name", employee.DepartmentID);
                ViewData["PositionID"] = new SelectList(_context.Positions, "ID", "Name", employee.PositionID);
                return View(employee);
            }
            else
            {
                return RedirectToAction("Index", "Employees");
            }
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstName,IdentityNumber,PhoneNumber,Gender,Username,Address,RegistirationNumber,DateOfStart,Active,DateOfLeave,DepartmentID,PositionID")] Employee employee)
        {
            if (id != employee.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    employee.IsActive = true;
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.ID))
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
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "Name", employee.DepartmentID);
            ViewData["PositionID"] = new SelectList(_context.Positions, "ID", "Name", employee.PositionID);
            return View(employee);
        }

        // GET: Employees/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            var employeeContext = _context.Employees.Include(e => e.Department).Include(e => e.Position);
            var emp = _context.Employees.Where(e => e.Username == HttpContext.User.Identity.Name).FirstOrDefault();
            var pos = _context.Positions.Find(emp.PositionID);
            var dep = _context.Departments.Find(emp.DepartmentID);
            if (dep.Name == "İnsan Kaynakları" && pos.Authority >= 3)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var employee = await _context.Employees
                    .Include(e => e.Department)
                    .Include(e => e.Position)
                    .FirstOrDefaultAsync(m => m.ID == id);
                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }
            else
            {
                return RedirectToAction("Index", "Employees");
            }
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id, [Bind("ID,LastName,FirstName,IdentityNumber,PhoneNumber,Gender,Username,Password,Address,RegistirationNumber,DateOfStart,Active,DateOfLeave,DepartmentID,PositionID")] Employee emp)
        {
            var employee = await _context.Employees.FindAsync(id);
            //_context.Employees.Remove(employee);
            employee.IsActive = false;
            employee.DateOfLeave = DateTime.Now.Date;
            _context.Update(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.ID == id);
        }

        public string EncryptPassword(string pass)
        {
            MD5 md5 = MD5.Create();
            byte[] encPass = md5.ComputeHash(Encoding.Default.GetBytes(pass));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < encPass.Length; i++)
            {
                builder.Append(encPass[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
