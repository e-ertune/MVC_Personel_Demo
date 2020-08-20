using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_Personel.Controllers;
using MVC_Personel.Data;
using MVC_Personel.Models;

namespace MVC_Personel.Areas.Identity.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly EmployeeContext _context;
        public AccountController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: Login
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(Employee employee)
        {
            List<Employee> list = _context.Employees.ToList();
            EmployeesController empc = new EmployeesController(_context);
            foreach (var item in list)
            {
                string pass = empc.EncryptPassword(employee.Password);
                if (item.Username == employee.Username && item.Password == pass)
                {
                    if (item.IsActive == false)
                    {
                        ViewBag.mesaj = "Erişim yetkiniz artık bulunmamaktadır.";
                        return View();
                    }
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, employee.Username) };
                    var userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(principal);
                    Position p = _context.Positions.Find(item.PositionID);
                    ViewBag.auth = p.Authority;
                    return RedirectToAction("Index", "Employees", new { area = "" });
                }
            }
            ViewBag.mesaj = "Kullanıcı adı veya parola hatalı.";
            return View();
        }

        //GET: Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
