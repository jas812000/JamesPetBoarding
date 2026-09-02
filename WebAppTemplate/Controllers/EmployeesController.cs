using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using JamesPetBoarding.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace JamesPetBoarding.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {

        // GET: Employees/Search
        public ActionResult Search()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewEmployees(currentEmployee))
            {
                return RedirectToAction("Index", "User");
            }

            ViewBag.CanManageEmployees = CanManageEmployees(currentEmployee);

            EmployeeSearchVM employeeSearch = new EmployeeSearchVM();

            return View(employeeSearch);
        }


        //POST: Employees/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(EmployeeSearchVM employeeSearch)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewEmployees(currentEmployee))
            {
                return RedirectToAction("Index", "User");
            }

            ViewBag.CanManageEmployees = CanManageEmployees(currentEmployee);

            if (!ModelState.IsValid)
            {
                return View(employeeSearch);
            }

            List<EmployeeModel> employees = dbContext.Employees.ToList();

            if (!string.IsNullOrWhiteSpace(employeeSearch.FirstName))
            {
                employees = employees
                    .Where(x => x.FirstName.ToLower().Contains(employeeSearch.FirstName.ToLower()))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(employeeSearch.LastName))
            {
                employees = employees
                    .Where(x => x.LastName.ToLower().Contains(employeeSearch.LastName.ToLower()))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(employeeSearch.Email))
            {
                employees = employees
                    .Where(x => x.Email.ToLower().Contains(employeeSearch.Email.ToLower()))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(employeeSearch.Phone))
            {
                employees = employees
                    .Where(x => x.Phone.Contains(employeeSearch.Phone))
                    .ToList();
            }

            if (employeeSearch.Role.HasValue)
            {
                employees = employees
                    .Where(x => x.Role == employeeSearch.Role.Value)
                    .ToList();
            }

            if (employeeSearch.IsActive.HasValue)
            {
                employees = employees
                    .Where(x => x.IsActive == employeeSearch.IsActive.Value)
                    .ToList();
            }

            employeeSearch.EmployeeSearchResults.Clear();

            foreach (EmployeeModel employee in employees)
            {
                employeeSearch.EmployeeSearchResults.Add(new EmployeeSummaryVM
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeNameDisplay = employee.FirstName + " " + employee.LastName,
                    RoleDisplay = employee.Role.ToString(),
                    PhoneDisplay = employee.Phone,
                    EmailDisplay = employee.Email,
                    IsActive = employee.IsActive,
                    ActiveStatusDisplay = employee.IsActive
                        ? "Active"
                        : "Inactive"
                });
            }

            return View(employeeSearch);

        }


        // GET: Employees/Create
        public ActionResult Create()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageEmployees(currentEmployee))
            {
                return RedirectToAction("Index", "User");
            }
    
            EmployeeFormVM employeeForm = new EmployeeFormVM();

            return View(employeeForm);
        }


        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeFormVM employeeForm)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageEmployees(currentEmployee))
            {
                return RedirectToAction("Index", "User");
            }

            if (!ModelState.IsValid)
            {
                return View(employeeForm);
            }

            EmployeeModel existingEmail = dbContext.Employees
                .FirstOrDefault(x =>
                x.Email == employeeForm.Email);

            if (existingEmail != null)
            {
                ModelState.AddModelError("",
                    "The email address " +
                    employeeForm.Email +
                    " for " +
                    employeeForm.FirstName +
                    " " +
                    employeeForm.LastName +
                    " already exists.");

                return View(employeeForm);
            }

            EmployeeModel employee = new EmployeeModel();

            employee.LastName = employeeForm.LastName;
            employee.FirstName = employeeForm.FirstName;
            employee.Role = employeeForm.Role;
            employee.Phone = employeeForm.Phone;
            employee.Email = employeeForm.Email;
            employee.Notes = employeeForm.Notes;
            employee.IsActive = true;

            dbContext.Employees.Add(employee);
            dbContext.SaveChanges();

            return RedirectToAction("Read", new { employeeId = employee.EmployeeId });

        }


        // GET: Employees/Read
        public ActionResult Read(Guid employeeId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewEmployees(currentEmployee))
            {
                return RedirectToAction("Index", "User");
            }

            EmployeeModel employee = dbContext.Employees.FirstOrDefault(x => x.EmployeeId == employeeId);

            if (employee == null)
            {
                return Content("Employee ID #" + employeeId + " does not exist.");
            }

            EmployeeDetailsVM employeeDetails = new EmployeeDetailsVM();

            employeeDetails.EmployeeId = employee.EmployeeId;
            employeeDetails.EmployeeNameDisplay = employee.FirstName + " " + employee.LastName;
            employeeDetails.RoleDisplay = employee.Role.ToString();
            employeeDetails.PhoneDisplay = employee.Phone;
            employeeDetails.EmailDisplay = employee.Email;
            employeeDetails.IsActive = employee.IsActive;
            employeeDetails.ActiveStatusDisplay = employee.IsActive
                ? "Active"
                : "Inactive";

            employeeDetails.InactivationReasonDisplay = employee.InactivationReason.HasValue
                ? employee.InactivationReason.Value.ToString()
                : "";

            employeeDetails.InactivationDateDisplay = employee.InactivationDate.HasValue
                ? employee.InactivationDate.Value.ToString("MM/dd/yyyy")
                : "";

            employeeDetails.InactivationNotesDisplay = string.IsNullOrWhiteSpace(employee.InactivationNotes)
                ? ""
                : employee.InactivationNotes;

            employeeDetails.ReactivationDateDisplay = employee.ReactivationDate.HasValue
                ? employee.ReactivationDate.Value.ToString("MM/dd/yyyy")
                : "";

            employeeDetails.ReactivationNotesDisplay = string.IsNullOrWhiteSpace(employee.ReactivationNotes)
               ? ""
               : employee.ReactivationNotes;

            employeeDetails.NotesDisplay = string.IsNullOrWhiteSpace(employee.Notes)
                ? "No notes"
                : employee.Notes;


            return View(employeeDetails);
        }


        // GET: Employees/Update
        public ActionResult Update(Guid employeeId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageEmployees(currentEmployee))
            {
                return RedirectToAction("Index", "User");
            }

            EmployeeModel employee = dbContext.Employees.FirstOrDefault(x => x.EmployeeId == employeeId);

            if (employee == null)
            {
                return Content("Employee ID #" + employeeId + " does not exist.");
            }

            EmployeeFormVM employeeForm = new EmployeeFormVM();

            employeeForm.EmployeeId = employeeId;
            employeeForm.LastName = employee.LastName;
            employeeForm.FirstName = employee.FirstName;
            employeeForm.Role = employee.Role;
            employeeForm.Phone = employee.Phone;
            employeeForm.Email = employee.Email;
            employeeForm.Notes = employee.Notes;

            return View(employeeForm);
        }


        // POST: Employees/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(EmployeeFormVM employeeForm)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageEmployees(currentEmployee))
            {
                return RedirectToAction("Index", "User");
            }

            EmployeeModel employee = dbContext.Employees.FirstOrDefault(x => x.EmployeeId == employeeForm.EmployeeId);

            if (employee == null)
            {
                return Content("Employee ID #" + employeeForm.EmployeeId + " does not exist.");
            }

            if (!ModelState.IsValid)
            {

                return View(employeeForm);
            }

            EmployeeModel existingEmployee = dbContext.Employees
                .FirstOrDefault(x =>
                x.Email == employeeForm.Email &&
                x.EmployeeId != employeeForm.EmployeeId);

            if (existingEmployee != null)
            {
                ModelState.AddModelError("", "An employee with this email address already exists.");

                return View(employeeForm);
            }

            employee.LastName = employeeForm.LastName;
            employee.FirstName = employeeForm.FirstName;
            employee.Role = employeeForm.Role;
            employee.Phone = employeeForm.Phone;
            employee.Email = employeeForm.Email;
            employee.Notes = employeeForm.Notes;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { employeeId = employee.EmployeeId });
        }


        // GET: Employees/Delete
        public ActionResult Delete(Guid employeeId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageEmployees(currentEmployee))
            {
                return RedirectToAction("Index", "User");
            }

            EmployeeModel employee = dbContext.Employees.FirstOrDefault(x => x.EmployeeId == employeeId);

            if (employee == null)
            {
                return Content("Employee ID #" + employeeId + " does not exist.");
            }

            EmployeeDeactivateVM employeeDeactivate = new EmployeeDeactivateVM();

            employeeDeactivate.EmployeeId = employeeId;
            employeeDeactivate.EmployeeNameDisplay = employee.FirstName + " " + employee.LastName;
            employeeDeactivate.RoleDisplay = employee.Role.ToString();
            employeeDeactivate.PhoneDisplay = employee.Phone;
            employeeDeactivate.EmailDisplay = employee.Email;
            employeeDeactivate.ActiveStatusDisplay = employee.IsActive
                ? "Active"
                : "Inactive";

            return View(employeeDeactivate);
        }


        // POST: Employees/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(EmployeeDeactivateVM employeeDeactivate)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageEmployees(currentEmployee))
            {
                return RedirectToAction("Index", "User");
            }

            EmployeeModel employee = dbContext.Employees.FirstOrDefault(x => x.EmployeeId == employeeDeactivate.EmployeeId);

            if (employee == null)
            {
                return Content("Employee ID #" + employeeDeactivate.EmployeeId + " does not exist.");
            }

            if (!ModelState.IsValid)
            {
                employeeDeactivate.EmployeeId = employee.EmployeeId;
                employeeDeactivate.EmployeeNameDisplay = employee.FirstName + " " + employee.LastName;
                employeeDeactivate.RoleDisplay = employee.Role.ToString();
                employeeDeactivate.PhoneDisplay = employee.Phone;
                employeeDeactivate.EmailDisplay = employee.Email;
                employeeDeactivate.ActiveStatusDisplay = employee.IsActive
                    ? "Active"
                    : "Inactive";

                return View(employeeDeactivate);
            }

            employee.IsActive = false;
            employee.InactivationReason = employeeDeactivate.InactivationReason;
            employee.InactivationDate = DateTime.Now;
            employee.InactivationNotes = employeeDeactivate.InactivationNotes;

            employee.ReactivationDate = null;
            employee.ReactivationNotes = null;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { employeeId = employee.EmployeeId });

        }


        // GET: Employees/Reactivate
        public ActionResult Reactivate(Guid employeeId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageEmployees(currentEmployee))
            {
                return RedirectToAction("Index", "User");
            }

            EmployeeModel employee = dbContext.Employees.FirstOrDefault(x => x.EmployeeId == employeeId);

            if (employee == null)
            {
                return Content("Employee ID #" + employeeId + " does not exist.");
            }

            if (employee.IsActive)
            {
                return Content(
                    "Employee " +
                    employee.FirstName +
                    " " +
                    employee.LastName +
                    " is already active.");
            }

            EmployeeReactivateVM employeeReactivate = new EmployeeReactivateVM();

            employeeReactivate.EmployeeId = employeeId;
            employeeReactivate.EmployeeNameDisplay = employee.FirstName + " " + employee.LastName;
            employeeReactivate.RoleDisplay = employee.Role.ToString();
            employeeReactivate.PhoneDisplay = employee.Phone;
            employeeReactivate.EmailDisplay = employee.Email;
            employeeReactivate.ActiveStatusDisplay = employee.IsActive
                ? "Active"
                : "Inactive";
            employeeReactivate.InactivationReasonDisplay = employee.InactivationReason.HasValue
                ? employee.InactivationReason.ToString()
                : "";
            employeeReactivate.InactivationDateDisplay = employee.InactivationDate.HasValue
                ? employee.InactivationDate.Value.ToString("MM/dd/yyyy")
                : "";
            employeeReactivate.InactivationNotesDisplay = string.IsNullOrWhiteSpace(employee.InactivationNotes)
                ? ""
                : employee.InactivationNotes;

            return View(employeeReactivate);
        }


        // POST: Employees/Reactivate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reactivate(EmployeeReactivateVM employeeReactivate)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageEmployees(currentEmployee))
            {
                return RedirectToAction("Index", "User");
            }

            EmployeeModel employee = dbContext.Employees.FirstOrDefault(x => x.EmployeeId == employeeReactivate.EmployeeId);

            if (employee == null)
            {
                return Content("Employee ID #" + employeeReactivate.EmployeeId + " does not exist.");
            }

            if (employee.IsActive)
            {
                return Content(
                    "Employee " +
                    employee.FirstName +
                    " " +
                    employee.LastName +
                    " is already active.");
            }

            if (!ModelState.IsValid)
            {
                employeeReactivate.EmployeeNameDisplay = employee.FirstName + " " + employee.LastName;
                employeeReactivate.RoleDisplay = employee.Role.ToString();
                employeeReactivate.PhoneDisplay = employee.Phone;
                employeeReactivate.EmailDisplay = employee.Email;
                employeeReactivate.ActiveStatusDisplay = employee.IsActive
                    ? "Active"
                    : "Inactive";
                employeeReactivate.InactivationReasonDisplay = employee.InactivationReason.HasValue
                    ? employee.InactivationReason.Value.ToString()
                    : "";
                employeeReactivate.InactivationDateDisplay = employee.InactivationDate.HasValue
                    ? employee.InactivationDate.Value.ToString("MM/dd/yyyy")
                    : "";
                employeeReactivate.InactivationNotesDisplay = string.IsNullOrWhiteSpace(employee.InactivationNotes)
                    ? ""
                    : employee.InactivationNotes;

                return View(employeeReactivate);
            }

            employee.IsActive = true;
            employee.ReactivationDate = DateTime.Now;
            employee.ReactivationNotes = employeeReactivate.ReactivationNotes;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { employeeId = employee.EmployeeId });

        }

        // GET: Employees/MyProfile
        [Authorize]
        public ActionResult MyProfile()
        {
            string email = User.Identity.Name;

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel employee = dbContext.Employees
                .FirstOrDefault(x => x.Email == email);

            if (employee == null)
            {
                return Content(
                    "The employee with the email address " +
                    email +
                    " does not exist.");
            }

            if (!employee.IsActive)
            {
                return Content("This employee account is inactive.");
            }

            EmployeeProfileVM employeeProfile = new EmployeeProfileVM();

            employeeProfile.EmployeeId = employee.EmployeeId;
            employeeProfile.EmployeeNameDisplay = employee.FirstName + " " + employee.LastName;
            employeeProfile.RoleDisplay = employee.Role.ToString();
            employeeProfile.EmailDisplay = employee.Email;
            employeeProfile.PhoneDisplay = employee.Phone;
            employeeProfile.ActiveStatusDisplay = employee.IsActive
                ? "Active"
                : "Inactive";
            employeeProfile.IsAdmin =
                employee.Role == EmployeeRoleEnum.Admin ||
                employee.Role == EmployeeRoleEnum.Manager ||
                employee.Role == EmployeeRoleEnum.Supervisor;

            return View(employeeProfile);
        }


        // GET: Employees/UpdateMyProfile
        [Authorize]
        public ActionResult UpdateMyProfile()
        {
            string email = User.Identity.Name;

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel employee = dbContext.Employees
                .FirstOrDefault(x => x.Email == email);

            if (employee == null)
            {
                return Content(
                    "The employee with the email address " +
                    email +
                    " does not exist.");
            }

            if (!employee.IsActive)
            {
                return Content("This employee account is inactive.");
            }

            EmployeeProfileUpdateVM employeeProfileUpdate = new EmployeeProfileUpdateVM();

            employeeProfileUpdate.EmployeeId = employee.EmployeeId;
            employeeProfileUpdate.Email = employee.Email;
            employeeProfileUpdate.Phone = employee.Phone;

            return View(employeeProfileUpdate);
        }


        // POST: Employees/UpdateMyProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult UpdateMyProfile(EmployeeProfileUpdateVM employeeProfileUpdate)
        {
            string email = User.Identity.Name;
            
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel employee = dbContext.Employees
                .FirstOrDefault(x => x.Email == email);

            if (employee == null)
            {
                return Content(
                    "The employee with the email address " +
                    email +
                    " does not exist.");
            }

            if (!employee.IsActive) 
            {
                return Content("This employee account is inactive.");
            }

            if (!ModelState.IsValid)
            {

                return View(employeeProfileUpdate);
            }

            bool duplicateEmail = dbContext.Employees.Any(x => 
                x.Email == employeeProfileUpdate.Email && 
                x.EmployeeId != employee.EmployeeId);

            if (duplicateEmail)
            {
                ModelState.AddModelError(
                    "Email",
                    "An employee with this email address already exists.");

                return View(employeeProfileUpdate);
            }

            bool emailChanged = employee.Email != employeeProfileUpdate.Email;

            if (emailChanged)
            {
                ApplicationUser identityUser = UserManager.FindByName(email);

                if (identityUser == null) 
                {
                    return Content(
                    "The identity account with the email address " +
                    email +
                    " does not exist.");

                }

                identityUser.Email = employeeProfileUpdate.Email;
                identityUser.UserName = employeeProfileUpdate.Email;

                IdentityResult identityResult = UserManager.Update(identityUser);

                if (!identityResult.Succeeded) 
                { 
                    foreach (string error in identityResult.Errors)
                    {
                        ModelState.AddModelError("Email", error);
                    }
                    
                    return View(employeeProfileUpdate);
                }
            }

            employee.Email = employeeProfileUpdate.Email;
            employee.Phone = employeeProfileUpdate.Phone; 

            dbContext.SaveChanges();

            if (emailChanged)
            {
                HttpContext.GetOwinContext().Authentication
                    .SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("MyProfile");
        
        }


        private EmployeeModel GetCurrentEmployee(ApplicationDbContext dbContext) 
        { 
            string loggedInEmail = User.Identity.Name;

            return dbContext.Employees
                .FirstOrDefault(x => 
                    x.Email == loggedInEmail && 
                    x.IsActive);

        }

        private bool CanViewEmployees(EmployeeModel employee) 
        {
            return employee != null &&
                (employee.Role == EmployeeRoleEnum.Admin ||
                 employee.Role == EmployeeRoleEnum.Manager ||
                 employee.Role == EmployeeRoleEnum.Supervisor);

        }


        private bool CanManageEmployees(EmployeeModel employee)
        {
            return employee != null &&
                (employee.Role == EmployeeRoleEnum.Admin ||
                 employee.Role == EmployeeRoleEnum.Manager);

        }


        private ApplicationUserManager _userManager;


        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager 
                    ?? HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}
