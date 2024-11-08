using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NGettextLocalizationSample.Models;

namespace NGettextLocalizationSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer _localizer;

        public HomeController(IStringLocalizer localizer)
        {
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            return View(new IndexViewModel
            {
                Message = _localizer.GetString("Some text from controller")
            });
        }
    }
}
