using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Income_and_Expense.Data;


namespace Income_and_Expense.Pages
{

    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        // private readonly UserCacheService userCacheService;
        private readonly ApplicationDbContext context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<RegisterModel> _logger;

        protected IServiceScopeFactory ScopeFactory;
        public RegisterModel(SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            UserManager<ApplicationUser> userManager,
            //  UserCacheService userCacheService,
            ApplicationDbContext context, IServiceScopeFactory ScopeFactory)
        {
            _userManager = userManager;
            // this.userCacheService = userCacheService;
            this.context = context;
            _signInManager = signInManager;
            _logger = logger;
            this.ScopeFactory = ScopeFactory;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        // public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MobileNo { get; set; }
            public string CNIC { get; set; }
            public string Email { get; set; }
            [Display(Name = "New Password")]
            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [Compare("Password")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            returnUrl = returnUrl ?? Url.Content("~/");
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            returnUrl = returnUrl == null || returnUrl == "/" ? Url.Content("~/") : returnUrl;

            if (ModelState.IsValid)
            {
                try
                {
                    var context = ScopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var applicationUser = await _userManager.FindByEmailAsync(Input.Email);

                    if (applicationUser != null)
                    {
                        ModelState.AddModelError(string.Empty, "User Already Exists");
                        return Page();
                    }
                    ApplicationUser user = new ApplicationUser();
                    var hasher = new PasswordHasher<ApplicationUser>();
                    user.PhoneNumber = Input.MobileNo;
                    user.UserName = Input.Email;
                    user.Email = Input.Email;
                    user.ConcurrencyStamp = Guid.NewGuid().ToString();
                    user.Id = Guid.NewGuid().ToString();
                    user.EmailConfirmed = true;
                    // user.ParentBranch = ParentBranch.Ic_Licensing;
                    user.PasswordHash = hasher.HashPassword(user, Input.Password);
                    await _userManager.CreateAsync(user);
                    //var RoleId = context.Roles.Where(x => x.Name == "Ic_Licensing User").Select(x => x.Id).FirstOrDefault();
                    // var r = await _userManager.AddToRoleAsync(user, "admin");
                    return Redirect("/identity/account/login");
                    //if (r.Succeeded)
                    //{
                    //return Redirect("/identity/account/login");
                    //}
                    //else
                    //{
                    //    await _userManager.DeleteAsync(user);
                    //    return Page();
                    //}

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.ToString());
                    return Page();
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

