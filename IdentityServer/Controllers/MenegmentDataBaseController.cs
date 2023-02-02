using IdentityServer.ViewModels;
using IdentityServer_DAL.MenegmentData;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    public class MenegmentDataBaseController : Controller
    {
        public MenegmentDataBaseController()
        {

        }

        [HttpGet]
        public IActionResult DeleteAllUsers(string returnActionName, string returnControllerName)
        {
            DeleteData.DeleteAllUsersAsync();
            SwitchActionReview(deleteAll: true);

            return RedirectToAction(actionName: returnActionName, controllerName: returnControllerName);
        }

        [HttpGet]
        public IActionResult CreateBaseUsers(string returnActionName, string returnControllerName)
        {
            SeedData.EnsureSeedDataAsync();
            SwitchActionReview(createDefault: true);

            return RedirectToAction(actionName: returnActionName, controllerName: returnControllerName);
        }


        private void SwitchActionReview(bool deleteAll = false, bool createDefault = false)
        {
            ErrorViewModel errorModel = new ErrorViewModel();

            errorModel.CreateDefaultUsersDone = createDefault;
            errorModel.DelateAllUsersDone = deleteAll;

            ViewBag.ErrorModel = errorModel;
        }
    }
}
