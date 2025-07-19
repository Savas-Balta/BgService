using BgService.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BgService.WebUI.Controllers
{
    public class ForexController : Controller
    {
        private readonly ForexRepository _repository;

        public ForexController(ForexRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var forexRates = await _repository.GetAllForexAsync();
            return View(forexRates);    
        }
    }
}
