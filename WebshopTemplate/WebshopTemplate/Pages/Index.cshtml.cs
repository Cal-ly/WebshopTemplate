using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebshopTemplate.Pages
{
    public class IndexModel : PageModel
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ILogger<IndexModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
