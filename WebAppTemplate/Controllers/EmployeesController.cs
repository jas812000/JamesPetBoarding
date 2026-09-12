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

            if (!Enum.IsDefined(typeof(EmployeeRoleEnum), employeeForm.Role))
            {
                ModelState.AddModelError(
                    "Role",
                    "Please select a valid employee role.");

                return View(employeeForm);
            }

            string normalizedEmail = employeeForm.Email.Trim().ToLower();

            EmployeeModel existingEmail = dbContext.Employees
                .FirstOrDefault(x =>
                x.Email.ToLower() == normalizedEmail);

            if (existingEmail != null)
            {
                ModelState.AddModelError(
                    "Email",
                    "An employee with this email address already exists.");

                return View(employeeForm);
            }

            ApplicationUser existingIdentityUser = UserManager.FindByEmail(employeeForm.Email.Trim());

            if (existingIdentityUser != null)
            {
                ModelState.AddModelError(
                    "Email",
                    "An account already exists with this email address. " +
                    "Please contact an administrator before creating the employee profile.");

                return View(employeeForm);
            }

            string normalizedFirstName = employeeForm.FirstName.Trim().ToLower();

            string normalizedLastName = employeeForm.LastName.Trim().ToLower();

            string normalizedPhone = NormalizePhone(employeeForm.Phone);

            List<EmployeeModel> employees = dbContext.Employees.ToList();

            EmployeeModel possibleDuplicate = employees
                .FirstOrDefault(x =>
                    NormalizePhone(x.Phone) == normalizedPhone ||
                    (
                        x.FirstName.Trim().ToLower() == normalizedFirstName &&
                        x.LastName.Trim().ToLower() == normalizedLastName
                    ));

            if (possibleDuplicate != null && !employeeForm.ConfirmPossibleDuplicate)
            {
                ViewBag.PossibleDuplicate = true;

                ModelState.AddModelError(
                    "",
                    "A possible duplicate employee was found: " +
                    possibleDuplicate.FirstName +
                    " " +
                    possibleDuplicate.LastName +
                    ". Review the existing employee before continuing.");

                return View(employeeForm);
            }

            EmployeeModel employee = new EmployeeModel();

            employee.LastName = employeeForm.LastName.Trim();
            employee.FirstName = employeeForm.FirstName.Trim();
            employee.Role = employeeForm.Role;
            employee.Phone = employeeForm.Phone.Trim();
            employee.Email = employeeForm.Email.Trim();
            employee.Notes = string.IsNullOrWhiteSpace(employeeForm.Notes)
                ? null
                : employeeForm.Notes.Trim();
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

            if (!Enum.IsDefined(typeof(EmployeeRoleEnum), employeeForm.Role))
            {
                ModelState.AddModelError(
                    "Role",
                    "Please select a valid employee role.");

                return View(employeeForm);
            }

            string normalizedEmail = employeeForm.Email.Trim().ToLower();

            string normalizedCurrentEmail = employee.Email.Trim().ToLower();

            bool emailChanged = normalizedEmail != normalizedCurrentEmail;

            EmployeeModel existingEmployee = dbContext.Employees
                .FirstOrDefault(x =>
                x.Email.ToLower() == normalizedEmail &&
                x.EmployeeId != employeeForm.EmployeeId);

            if (existingEmployee != null)
            {
                ModelState.AddModelError("Email", "An employee with this email address already exists.");

                return View(employeeForm);
            }

            if (emailChanged)
            {
                ApplicationUser conflictingIdentityUser = UserManager.FindByEmail(employeeForm.Email.Trim());

                if (conflictingIdentityUser != null)
                {
                    ModelState.AddModelError("Email", "An account already exists with this email address.");

                    return View(employeeForm);
                }
            }

            ApplicationUser employeeIdentityUser = UserManager.FindByEmail(employee.Email);

            if (emailChanged && employeeIdentityUser != null && currentEmployee.Role != EmployeeRoleEnum.Admin)
            {
                ModelState.AddModelError(
                    "Email",
                    "Only an administrator can change the email address of an employee who already has an account.");

                return View(employeeForm);
            }

            string normalizedFirstName = employeeForm.FirstName.Trim().ToLower();

            string normalizedLastName = employeeForm.LastName.Trim().ToLower();

            string normalizedPhone = NormalizePhone(employeeForm.Phone);

            List<EmployeeModel> employees = dbContext.Employees
                .Where(x => x.EmployeeId != employeeForm.EmployeeId)
                .ToList();

            EmployeeModel possibleDuplicate = employees
                .FirstOrDefault(x =>
                    NormalizePhone(x.Phone) == normalizedPhone ||
                    (
                        x.FirstName.Trim().ToLower() == normalizedFirstName &&
                        x.LastName.Trim().ToLower() == normalizedLastName
                    ));

            if (possibleDuplicate != null && !employeeForm.ConfirmPossibleDuplicate)
            {
                ViewBag.PossibleDuplicate = true;

                ModelState.AddModelError(
                    "",
                    "A possible duplicate employee was found: " +
                    possibleDuplicate.FirstName +
                    " " +
                    possibleDuplicate.LastName +
                    ". Review the existing employee before continuing."
                );

                return View(employeeForm);

            }

            if (emailChanged && employeeIdentityUser != null)
            {
                employeeIdentityUser.Email = employeeForm.Email.Trim();

                employeeIdentityUser.UserName = employeeForm.Email.Trim();

                IdentityResult identityResult = UserManager.Update(employeeIdentityUser);

                if (!identityResult.Succeeded)
                {
                    foreach (string error in identityResult.Errors)
                    {
                        ModelState.AddModelError("Email", error);
                    }

                    return View(employeeForm);

                }

            }

            bool currentUserEmailChanged =
                emailChanged &&
                employeeIdentityUser != null &&
                currentEmployee.EmployeeId == employee.EmployeeId;

            employee.LastName = employeeForm.LastName.Trim();

            employee.FirstName = employeeForm.FirstName.Trim();

            employee.Role = employeeForm.Role;

            employee.Phone = employeeForm.Phone.Trim();

            employee.Email = employeeForm.Email.Trim();

            employee.Notes = string.IsNullOrWhiteSpace(employeeForm.Notes)
                ? null
                : employeeForm.Notes.Trim();

            dbContext.SaveChanges();

            if (currentUserEmailChanged)
            {
                HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                return RedirectToAction("Login", "Account");
            }

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

            string normalizedEmail = email.Trim().ToLower();

            EmployeeModel employee = dbContext.Employees
                .FirstOrDefault(x => x.Email.ToLower() == normalizedEmail);

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

            string normalizedEmail = email.Trim().ToLower();

            EmployeeModel employee = dbContext.Employees
                .FirstOrDefault(x => x.Email.ToLower() == normalizedEmail);

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

            string normalizedCurrentEmail = email.Trim().ToLower();

            EmployeeModel employee = dbContext.Employees
                .FirstOrDefault(x => x.Email.ToLower() == normalizedCurrentEmail);

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

            string normalizedNewEmail = employeeProfileUpdate.Email.Trim().ToLower();

            bool duplicateEmail = dbContext.Employees.Any(x =>
                x.Email.ToLower() == normalizedNewEmail &&
                x.EmployeeId != employee.EmployeeId);

            if (duplicateEmail)
            {
                ModelState.AddModelError(
                    "Email",
                    "An employee with this email address already exists.");

                return View(employeeProfileUpdate);
            }

            bool emailChanged = employee.Email.Trim().ToLower() != normalizedNewEmail;

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

                identityUser.Email = employeeProfileUpdate.Email.Trim();
                identityUser.UserName = employeeProfileUpdate.Email.Trim();

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

            employee.Email = employeeProfileUpdate.Email.Trim();
            employee.Phone = employeeProfileUpdate.Phone.Trim();

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

            string normalizedEmail = loggedInEmail.Trim().ToLower();

            return dbContext.Employees
                .FirstOrDefault(x =>
                    x.Email.ToLower() == normalizedEmail &&
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

        private string NormalizePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return "";
            }

            return new string(phone.Where(char.IsDigit).ToArray());
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
