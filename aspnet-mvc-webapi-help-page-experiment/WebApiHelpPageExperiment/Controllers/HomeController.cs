using System.Web.Http;
using System.Web.Mvc;
using WebApiHelpPageExperiment.Infrastructure;

namespace WebApiHelpPageExperiment.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {            
            var apiExplorer = GlobalConfiguration.Configuration.Services.GetApiExplorer();
            var model = ApiModelGenerator.BuildApiModel(apiExplorer);
            return View(model);
        }
    }
}
