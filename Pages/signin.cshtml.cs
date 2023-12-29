using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class signinModel : PageModel
    {
        private readonly ILogger<signinModel> _logger;

        public signinModel(ILogger<signinModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}