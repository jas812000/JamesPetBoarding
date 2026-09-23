using JamesPetBoarding.Models;
using JamesPetBoarding.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace JamesPetBoarding.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message =
                "The application description page.";

            return View();
        }

        public ActionResult OurServices()
        {
            return View();
        }

        public ActionResult OurTeam()
        {
            using (ApplicationDbContext dbContext =
                new ApplicationDbContext())
            {
                List<OurTeamMemberModel> teamMembers =
                    dbContext.OurTeamMembers
                        .Include(x => x.Employee)
                        .Where(x => x.Employee.IsActive)
                        .OrderBy(x => x.DisplayOrder)
                        .ToList();

                List<OurTeamMemberDisplayVM> teamMemberDisplays =
                    new List<OurTeamMemberDisplayVM>();

                foreach (OurTeamMemberModel teamMember in teamMembers)
                {
                    teamMemberDisplays.Add(
                        new OurTeamMemberDisplayVM
                        {
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
                                teamMember.DisplayOrder
                        });
                }

                return View(teamMemberDisplays);
            }
        }

        public ActionResult Hours()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "My contact page.";

            return View();
        }
    }
}
