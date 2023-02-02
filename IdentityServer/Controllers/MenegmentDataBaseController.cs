using IdentityServer.ViewModels;
using IdentityServer_DAL.MenegmentData;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    public class MenegmentDataBaseController : Controller
    {
        private readonly DeleteData _delateControll;
        private readonly SeedData _seedData;

        public MenegmentDataBaseController(DeleteData deleteData, SeedData seedData)
        {
            _delateControll = deleteData;
            _seedData = seedData;
        }

        [HttpGet]
        public IActionResult DeleteAllUsers(string returnActionName, string returnControllerName)
        {
            _delateControll.DeleteAllUsersAsync();
            SwitchActionReview(deleteAll: true);

            return RedirectToAction(actionName: returnActionName, controllerName: returnControllerName);
        }

        [HttpGet]
        public IActionResult CreateBaseUsers(string returnActionName, string returnControllerName)
        {
            _seedData.EnsureSeedDataAsync();
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
