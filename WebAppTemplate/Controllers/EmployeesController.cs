using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace JamesPetBoarding.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employees
        public ActionResult Index()
        {
            return View();
        }


        // GET: Employees/Create
        // /Employees/Create?lastName=Stevens&firstName=Lilly&role=Pet%20Caretaker&phone=2145559874&email=lily.stevens@pawsllc.net&isActive=false
        public ActionResult Create( 
            string lastName,
            string firstName,
            string role,
            string phone,
            string email,
            bool isActive
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (string.IsNullOrWhiteSpace(lastName)) { return Content("Last name is required."); }
            if (string.IsNullOrWhiteSpace(firstName)) { return Content("First name is required."); }
            if (string.IsNullOrWhiteSpace(role)) { return Content("Role / position is required."); }
            if (string.IsNullOrWhiteSpace(phone)) { return Content("Phone number is required."); }
            if (string.IsNullOrWhiteSpace(email)) { return Content("Email address is required."); }

            EmployeeModel employee = new EmployeeModel();

            employee.LastName = lastName;
            employee.FirstName = firstName;
            employee.Role = role;
            employee.Phone = phone;
            employee.Email = email;
            employee.IsActive = isActive;
         
            try
            {
                dbContext.Employees.Add( employee );
                dbContext.SaveChanges();
                return Content(employee.FirstName + " " + employee.LastName + " was successfully created.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        // GET: Employees/Read
        // /Employees/Read?employeeId=USE_EXISTING_EMPLOYEE_ID
        public ActionResult Read(Guid employeeId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel employee = dbContext.Employees.FirstOrDefault(x => x.EmployeeId == employeeId);

            if (employee == null) { return Content("Employee ID #" + employeeId + " does not exist."); }

            string employeeActive = employee.IsActive
                ? "Yes"
                : "No";

            return Content(
                "Employee ID #" + employee.EmployeeId +
                "<br />Last Name: " + employee.LastName +
                "<br />First Name: " + employee.FirstName +
                "<br />Role: " + employee.Role +
                "<br />Phone Number: " + employee.Phone +
                "<br />Email Address: " + employee.Email +
                "<br />Employee Active: " + employeeActive
            );
        }


        // GET: Employees/Update
        // /Employees/Update?employeeId=USE_EXISTING_EMPLOYEE_ID&lastName=Stevens&firstName=Lily&role=Pet%20Caretaker&phone=2145559874&email=lily.stevens@pawsllc.net&isActive=true
        public ActionResult Update(
            Guid employeeId,
            string lastName,
            string firstName,
            string role,
            string phone,
            string email,
            bool isActive
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel employee = dbContext.Employees.FirstOrDefault(x => x.EmployeeId == employeeId);

            if (employee == null) { return Content("Employee ID #" + employeeId + " does not exist."); }

            if (string.IsNullOrWhiteSpace(lastName)) { return Content("Last name is required."); }
            if (string.IsNullOrWhiteSpace(firstName)) { return Content("First name is required."); }
            if (string.IsNullOrWhiteSpace(role)) { return Content("Role / position is required."); }
            if (string.IsNullOrWhiteSpace(phone)) { return Content("Phone number is required."); }
            if (string.IsNullOrWhiteSpace(email)) { return Content("Email address is required."); }

            employee.LastName = lastName;
            employee.FirstName = firstName;
            employee.Role = role;
            employee.Phone = phone;
            employee.Email = email;
            employee.IsActive = isActive;

            try
            {
                dbContext.SaveChanges();
                return Content("Employee ID #" + employee.EmployeeId + " was updated in the database.");
            }
            catch (Exception ex) 
            {
                return Content(ex.Message);
            }
        }


        // GET: Employees/Delete
        // /Employees/Delete?employeeId=USE_EXISTING_EMPLOYEE_ID
        public ActionResult Delete(Guid employeeId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel employee = dbContext.Employees.FirstOrDefault(x => x.EmployeeId == employeeId);

            if (employee == null) { return Content("Employee ID #" + employeeId + " does not exist."); }

            try
            {
                //dbContext.Employees.Remove(employee);
                employee.IsActive = false;
                dbContext.SaveChanges();
                //return Content("Employee ID #" + employee.EmployeeId + " was successfully deleted.");
                return Content("Employee ID #" + employee.EmployeeId + " was successfully deactivated.");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
