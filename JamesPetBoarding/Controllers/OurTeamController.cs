using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using JamesPetBoarding.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace JamesPetBoarding.Controllers
{
    [Authorize]
    public class OurTeamController : Controller
    {
        // GET: OurTeam
        public ActionResult Index()
        {
            using (ApplicationDbContext dbContext =
                new ApplicationDbContext())
            {
                if (!CanManageOurTeam(dbContext))
                {
                    return RedirectToAction("Index", "Staff");
                }

                List<OurTeamMemberModel> teamMembers =
                    dbContext.OurTeamMembers
                        .Include(x => x.Employee)
                        .OrderBy(x => x.DisplayOrder)
                        .ToList();

                OurTeamManagementVM ourTeamManagement =
                    new OurTeamManagementVM();

                foreach (OurTeamMemberModel teamMember in teamMembers)
                {
                    ourTeamManagement.OurTeamMembers.Add(
                        BuildSummaryVM(teamMember));
                }

                return View(ourTeamManagement);
            }
        }

        // GET: OurTeam/Create
        public ActionResult Create()
        {
            using (ApplicationDbContext dbContext =
                new ApplicationDbContext())
            {
                if (!CanManageOurTeam(dbContext))
                {
                    return RedirectToAction("Index", "Staff");
                }

                OurTeamMemberFormVM ourTeamMember =
                    new OurTeamMemberFormVM();

                ourTeamMember.DisplayOrder =
                    dbContext.OurTeamMembers.Count() + 1;

                ourTeamMember.EmployeeSelectList =
                    BuildEmployeeSelectList(
                        dbContext,
                        null,
                        null);

                return View(ourTeamMember);
            }
        }

        // POST: OurTeam/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            OurTeamMemberFormVM ourTeamMember)
        {
            using (ApplicationDbContext dbContext =
                new ApplicationDbContext())
            {
                if (!CanManageOurTeam(dbContext))
                {
                    return RedirectToAction("Index", "Staff");
                }

                ValidateCreate(
                    dbContext,
                    ourTeamMember);

                if (!ModelState.IsValid)
                {
                    ourTeamMember.EmployeeSelectList =
                        BuildEmployeeSelectList(
                            dbContext,
                            ourTeamMember.EmployeeId,
                            null);

                    return View(ourTeamMember);
                }

                int requestedOrder =
                    ourTeamMember.DisplayOrder.Value;

                using (var transaction =
                    dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        List<OurTeamMemberModel> membersToMove =
                            dbContext.OurTeamMembers
                                .Where(x =>
                                    x.DisplayOrder >= requestedOrder)
                                .OrderByDescending(x =>
                                    x.DisplayOrder)
                                .ToList();

                        foreach (OurTeamMemberModel member in
                            membersToMove)
                        {
                            member.DisplayOrder++;

                            dbContext.SaveChanges();
                        }

                        OurTeamMemberModel newTeamMember =
                            new OurTeamMemberModel
                            {
                                EmployeeId =
                                    ourTeamMember.EmployeeId.Value,

                                PublicJobTitle =
                                    ourTeamMember.PublicJobTitle.Trim(),

                                DisplayOrder = requestedOrder
                            };

                        dbContext.OurTeamMembers.Add(newTeamMember);

                        dbContext.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                TempData["SuccessMessage"] =
                    "The employee was added to the Our Team page.";

                return RedirectToAction("Index");
            }
        }

        // GET: OurTeam/Update
        public ActionResult Update(Guid ourTeamMemberId)
        {
            using (ApplicationDbContext dbContext =
                new ApplicationDbContext())
            {
                if (!CanManageOurTeam(dbContext))
                {
                    return RedirectToAction("Index", "Staff");
                }

                OurTeamMemberModel teamMember =
                    dbContext.OurTeamMembers
                        .FirstOrDefault(x =>
                            x.OurTeamMemberId == ourTeamMemberId);

                if (teamMember == null)
                {
                    return HttpNotFound();
                }

                OurTeamMemberFormVM ourTeamMember =
                    new OurTeamMemberFormVM
                    {
                        OurTeamMemberId =
                            teamMember.OurTeamMemberId,

                        EmployeeId =
                            teamMember.EmployeeId,

                        PublicJobTitle =
                            teamMember.PublicJobTitle,

                        DisplayOrder =
                            teamMember.DisplayOrder
                    };

                ourTeamMember.EmployeeSelectList =
                    BuildEmployeeSelectList(
                        dbContext,
                        teamMember.EmployeeId,
                        teamMember.OurTeamMemberId);

                return View(ourTeamMember);
            }
        }

        // POST: OurTeam/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(
            OurTeamMemberFormVM ourTeamMember)
        {
            using (ApplicationDbContext dbContext =
                new ApplicationDbContext())
            {
                if (!CanManageOurTeam(dbContext))
                {
                    return RedirectToAction("Index", "Staff");
                }

                if (!ourTeamMember.OurTeamMemberId.HasValue)
                {
                    return HttpNotFound();
                }

                OurTeamMemberModel existingTeamMember =
                    dbContext.OurTeamMembers
                        .FirstOrDefault(x =>
                            x.OurTeamMemberId ==
                            ourTeamMember.OurTeamMemberId.Value);

                if (existingTeamMember == null)
                {
                    return HttpNotFound();
                }

                ValidateUpdate(
                    dbContext,
                    ourTeamMember,
                    existingTeamMember);

                if (!ModelState.IsValid)
                {
                    ourTeamMember.EmployeeSelectList =
                        BuildEmployeeSelectList(
                            dbContext,
                            ourTeamMember.EmployeeId,
                            existingTeamMember.OurTeamMemberId);

                    return View(ourTeamMember);
                }

                int originalOrder =
                    existingTeamMember.DisplayOrder;

                int requestedOrder =
                    ourTeamMember.DisplayOrder.Value;

                using (var transaction =
                    dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (originalOrder != requestedOrder)
                        {
                            int temporaryOrder =
                                dbContext.OurTeamMembers
                                    .Max(x => x.DisplayOrder) + 1000;

                            existingTeamMember.DisplayOrder =
                                temporaryOrder;

                            dbContext.SaveChanges();

                            if (requestedOrder < originalOrder)
                            {
                                List<OurTeamMemberModel>
                                    membersToMove =
                                    dbContext.OurTeamMembers
                                        .Where(x =>
                                            x.OurTeamMemberId !=
                                                existingTeamMember
                                                    .OurTeamMemberId &&
                                            x.DisplayOrder >=
                                                requestedOrder &&
                                            x.DisplayOrder <
                                                originalOrder)
                                        .OrderByDescending(x =>
                                            x.DisplayOrder)
                                        .ToList();

                                foreach (OurTeamMemberModel member in
                                    membersToMove)
                                {
                                    member.DisplayOrder++;

                                    dbContext.SaveChanges();
                                }
                            }
                            else
                            {
                                List<OurTeamMemberModel>
                                    membersToMove =
                                    dbContext.OurTeamMembers
                                        .Where(x =>
                                            x.OurTeamMemberId !=
                                                existingTeamMember
                                                    .OurTeamMemberId &&
                                            x.DisplayOrder >
                                                originalOrder &&
                                            x.DisplayOrder <=
                                                requestedOrder)
                                        .OrderBy(x =>
                                            x.DisplayOrder)
                                        .ToList();

                                foreach (OurTeamMemberModel member in
                                    membersToMove)
                                {
                                    member.DisplayOrder--;

                                    dbContext.SaveChanges();
                                }
                            }

                            existingTeamMember.DisplayOrder =
                                requestedOrder;
                        }

                        existingTeamMember.EmployeeId =
                            ourTeamMember.EmployeeId.Value;

                        existingTeamMember.PublicJobTitle =
                            ourTeamMember.PublicJobTitle.Trim();

                        dbContext.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                TempData["SuccessMessage"] =
                    "The Our Team member was updated.";

                return RedirectToAction("Index");
            }
        }

        // GET: OurTeam/Delete
        public ActionResult Delete(Guid ourTeamMemberId)
        {
            using (ApplicationDbContext dbContext =
                new ApplicationDbContext())
            {
                if (!CanManageOurTeam(dbContext))
                {
                    return RedirectToAction("Index", "Staff");
                }

                OurTeamMemberModel teamMember =
                    dbContext.OurTeamMembers
                        .Include(x => x.Employee)
                        .FirstOrDefault(x =>
                            x.OurTeamMemberId == ourTeamMemberId);

                if (teamMember == null)
                {
                    return HttpNotFound();
                }

                return View(BuildSummaryVM(teamMember));
            }
        }

        // POST: OurTeam/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(
            Guid ourTeamMemberId)
        {
            using (ApplicationDbContext dbContext =
                new ApplicationDbContext())
            {
                if (!CanManageOurTeam(dbContext))
                {
                    return RedirectToAction("Index", "Staff");
                }

                OurTeamMemberModel teamMember =
                    dbContext.OurTeamMembers
                        .FirstOrDefault(x =>
                            x.OurTeamMemberId == ourTeamMemberId);

                if (teamMember == null)
                {
                    return HttpNotFound();
                }

                int deletedOrder = teamMember.DisplayOrder;

                using (var transaction =
                    dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        dbContext.OurTeamMembers.Remove(teamMember);

                        dbContext.SaveChanges();

                        List<OurTeamMemberModel> membersToMove =
                            dbContext.OurTeamMembers
                                .Where(x =>
                                    x.DisplayOrder > deletedOrder)
                                .OrderBy(x =>
                                    x.DisplayOrder)
                                .ToList();

                        foreach (OurTeamMemberModel member in
                            membersToMove)
                        {
                            member.DisplayOrder--;

                            dbContext.SaveChanges();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                TempData["SuccessMessage"] =
                    "The employee was removed from the Our Team page. " +
                    "The employee profile was not deleted.";

                return RedirectToAction("Index");
            }
        }

        // POST: OurTeam/MoveUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MoveUp(Guid ourTeamMemberId)
        {
            using (ApplicationDbContext dbContext =
                new ApplicationDbContext())
            {
                if (!CanManageOurTeam(dbContext))
                {
                    return RedirectToAction("Index", "Staff");
                }

                OurTeamMemberModel teamMember =
                    dbContext.OurTeamMembers
                        .FirstOrDefault(x =>
                            x.OurTeamMemberId == ourTeamMemberId);

                if (teamMember == null)
                {
                    return HttpNotFound();
                }

                OurTeamMemberModel previousMember =
                    dbContext.OurTeamMembers
                        .Where(x =>
                            x.DisplayOrder <
                            teamMember.DisplayOrder)
                        .OrderByDescending(x =>
                            x.DisplayOrder)
                        .FirstOrDefault();

                if (previousMember != null)
                {
                    SwapDisplayOrders(
                        dbContext,
                        teamMember,
                        previousMember);
                }

                return RedirectToAction("Index");
            }
        }

        // POST: OurTeam/MoveDown
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MoveDown(Guid ourTeamMemberId)
        {
            using (ApplicationDbContext dbContext =
                new ApplicationDbContext())
            {
                if (!CanManageOurTeam(dbContext))
                {
                    return RedirectToAction("Index", "Staff");
                }

                OurTeamMemberModel teamMember =
                    dbContext.OurTeamMembers
                        .FirstOrDefault(x =>
                            x.OurTeamMemberId == ourTeamMemberId);

                if (teamMember == null)
                {
                    return HttpNotFound();
                }

                OurTeamMemberModel nextMember =
                    dbContext.OurTeamMembers
                        .Where(x =>
                            x.DisplayOrder >
                            teamMember.DisplayOrder)
                        .OrderBy(x =>
                            x.DisplayOrder)
                        .FirstOrDefault();

                if (nextMember != null)
                {
                    SwapDisplayOrders(
                        dbContext,
                        teamMember,
                        nextMember);
                }

                return RedirectToAction("Index");
            }
        }

        private bool CanManageOurTeam(
            ApplicationDbContext dbContext)
        {
            string loggedInEmail = User.Identity.Name;

            EmployeeModel currentEmployee =
                dbContext.Employees
                    .FirstOrDefault(x =>
                        x.Email == loggedInEmail);

            return currentEmployee != null &&
                   currentEmployee.IsActive &&
                   currentEmployee.Role ==
                       EmployeeRoleEnum.Admin;
        }

        private void ValidateCreate(
            ApplicationDbContext dbContext,
            OurTeamMemberFormVM ourTeamMember)
        {
            ValidateEmployee(
                dbContext,
                ourTeamMember,
                null);

            int maximumOrder =
                dbContext.OurTeamMembers.Count() + 1;

            ValidateDisplayOrder(
                ourTeamMember.DisplayOrder,
                maximumOrder);
        }

        private void ValidateUpdate(
            ApplicationDbContext dbContext,
            OurTeamMemberFormVM ourTeamMember,
            OurTeamMemberModel existingTeamMember)
        {
            ValidateEmployee(
                dbContext,
                ourTeamMember,
                existingTeamMember.OurTeamMemberId);

            int maximumOrder =
                dbContext.OurTeamMembers.Count();

            ValidateDisplayOrder(
                ourTeamMember.DisplayOrder,
                maximumOrder);
        }

        private void ValidateEmployee(
            ApplicationDbContext dbContext,
            OurTeamMemberFormVM ourTeamMember,
            Guid? currentOurTeamMemberId)
        {
            if (!ourTeamMember.EmployeeId.HasValue)
            {
                return;
            }

            EmployeeModel employee =
                dbContext.Employees
                    .FirstOrDefault(x =>
                        x.EmployeeId ==
                        ourTeamMember.EmployeeId.Value);

            if (employee == null || !employee.IsActive)
            {
                ModelState.AddModelError(
                    "EmployeeId",
                    "Please select an active employee.");

                return;
            }

            bool employeeAlreadyAdded =
                dbContext.OurTeamMembers.Any(x =>
                    x.EmployeeId ==
                        ourTeamMember.EmployeeId.Value &&
                    (!currentOurTeamMemberId.HasValue ||
                     x.OurTeamMemberId !=
                        currentOurTeamMemberId.Value));

            if (employeeAlreadyAdded)
            {
                ModelState.AddModelError(
                    "EmployeeId",
                    "This employee is already on the Our Team page.");
            }
        }

        private void ValidateDisplayOrder(
            int? displayOrder,
            int maximumOrder)
        {
            if (!displayOrder.HasValue)
            {
                return;
            }

            if (displayOrder.Value < 1 ||
                displayOrder.Value > maximumOrder)
            {
                ModelState.AddModelError(
                    "DisplayOrder",
                    "Display order must be between 1 and " +
                    maximumOrder + ".");
            }
        }

        private List<SelectListItem> BuildEmployeeSelectList(
            ApplicationDbContext dbContext,
            Guid? selectedEmployeeId,
            Guid? currentOurTeamMemberId)
        {
            List<Guid> assignedEmployeeIds =
                dbContext.OurTeamMembers
                    .Where(x =>
                        !currentOurTeamMemberId.HasValue ||
                        x.OurTeamMemberId !=
                            currentOurTeamMemberId.Value)
                    .Select(x => x.EmployeeId)
                    .ToList();

            List<EmployeeModel> employees =
                dbContext.Employees
                    .Where(x =>
                        x.IsActive &&
                        !assignedEmployeeIds.Contains(
                            x.EmployeeId))
                    .OrderBy(x => x.LastName)
                    .ThenBy(x => x.FirstName)
                    .ToList();

            List<SelectListItem> employeeSelectList =
                new List<SelectListItem>();

            foreach (EmployeeModel employee in employees)
            {
                employeeSelectList.Add(
                    new SelectListItem
                    {
                        Value = employee.EmployeeId.ToString(),

                        Text =
                            employee.LastName + ", " +
                            employee.FirstName,

                        Selected =
                            selectedEmployeeId.HasValue &&
                            employee.EmployeeId ==
                                selectedEmployeeId.Value
                    });
            }

            return employeeSelectList;
        }

        private OurTeamMemberSummaryVM BuildSummaryVM(
            OurTeamMemberModel teamMember)
        {
            return new OurTeamMemberSummaryVM
            {
                OurTeamMemberId =
                    teamMember.OurTeamMemberId,

                EmployeeId =
                    teamMember.EmployeeId,

                EmployeeNameDisplay =
                    teamMember.Employee.FirstName + " " +
                    teamMember.Employee.LastName,

                PublicJobTitle =
                    teamMember.PublicJobTitle,

                ProfileImagePath =
                    string.IsNullOrWhiteSpace(
                        teamMember.Employee.ProfileImagePath)
                    ? "~/Content/Images/Employees/default-profile.png"
                    : teamMember.Employee.ProfileImagePath,

                DisplayOrder =
                    teamMember.DisplayOrder,

                IsEmployeeActive =
                    teamMember.Employee.IsActive
            };
        }

        private void SwapDisplayOrders(
            ApplicationDbContext dbContext,
            OurTeamMemberModel firstMember,
            OurTeamMemberModel secondMember)
        {
            int firstOrder = firstMember.DisplayOrder;
            int secondOrder = secondMember.DisplayOrder;

            using (var transaction =
                dbContext.Database.BeginTransaction())
            {
                try
                {
                    int temporaryOrder =
                        dbContext.OurTeamMembers
                            .Max(x => x.DisplayOrder) + 1000;

                    firstMember.DisplayOrder = temporaryOrder;

                    dbContext.SaveChanges();

                    secondMember.DisplayOrder = firstOrder;

                    dbContext.SaveChanges();

                    firstMember.DisplayOrder = secondOrder;

                    dbContext.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
