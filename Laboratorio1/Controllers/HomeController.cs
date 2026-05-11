using Microsoft.AspNetCore.Mvc;

namespace personapi_dotnet.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
