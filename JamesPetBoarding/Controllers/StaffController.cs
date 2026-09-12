using JamesPetBoarding.Models;
using System.Linq;
using System.Web.Mvc;

namespace JamesPetBoarding.Controllers
{
    [Authorize]
    public class StaffController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            if(!IsCurrentEmployeeActive())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: User/BoardingManagement
        public ActionResult BoardingManagement()
        {
            if (!IsCurrentEmployeeActive())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: User/Billing
        public ActionResult Billing()
        {
            if (!IsCurrentEmployeeActive())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }


        private bool IsCurrentEmployeeActive()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            string email = User.Identity.Name;

            EmployeeModel employee = dbContext.Employees
                .FirstOrDefault(x => x.Email == email);

            return employee != null && employee.IsActive;    

        }

    }
}
