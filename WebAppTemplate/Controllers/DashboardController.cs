using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            string loggedInEmail = User.Identity.Name;

            EmployeeModel employee = dbContext.Employees.FirstOrDefault(x => x.Email == loggedInEmail);

            if (employee == null)
            {
                return Content("No employee profile was found for the logged in user.");
            }

            if (!employee.IsActive)
            {
                return Content("This employee account is inactive.");
            }

            if (employee.Role == EmployeeRoleEnum.Admin || 
                employee.Role == EmployeeRoleEnum.Manager || 
                employee.Role == EmployeeRoleEnum.Supervisor)
            {
                return RedirectToAction("Index", "Admin");
            }
    
            return RedirectToAction("Index", "Staff");

        }
    }
}