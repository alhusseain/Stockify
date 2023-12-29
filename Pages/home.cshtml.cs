using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class homeModel : PageModel
    {
        private readonly ILogger<homeModel> _logger;

        public homeModel(ILogger<homeModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}