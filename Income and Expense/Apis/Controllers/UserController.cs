using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Income_and_Expense.Data;
using Income_and_Expense.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Income_and_Expense.Apis.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            this.context = context;
            _signInManager = signInManager;
        }



        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // login api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserloginVM userloginVM)
        {
            var userTemp = await _userManager.FindByNameAsync(userloginVM.Email.ToLower());
            if (userTemp == null)
            {
               
                return BadRequest("Invalid username/password");
            }

            var result = await _signInManager.PasswordSignInAsync(userloginVM.Email.ToLower(), userloginVM.Password,  false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }

        // Register api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationVM userRegistrationVM)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    var applicationUser =await  _userManager.FindByEmailAsync(userRegistrationVM.Email);
                   if (applicationUser == null)
                    {
                        var hasher = new PasswordHasher<ApplicationUser>();
                        ApplicationUser user = new ApplicationUser();
                        user.FirstName = userRegistrationVM.FirstName;
                        user.LastName = userRegistrationVM.LastName;
                        user.PhoneNumber = userRegistrationVM.MobileNo;
                        user.UserName = userRegistrationVM.Email;
                        user.Email = userRegistrationVM.Email;
                        user.ConcurrencyStamp = Guid.NewGuid().ToString();
                        user.Id = Guid.NewGuid().ToString();
                        user.EmailConfirmed = true;
                        user.PasswordHash = hasher.HashPassword(user, userRegistrationVM.Password);
                        await _userManager.CreateAsync(user);
                        return Ok("user created sucessfully");
                    }
                    else
                    {
                        return BadRequest("user already exists");

                    }


                }
                catch (Exception ex)
                {
                    return  BadRequest(ex.ToString());
                }
            }
            // If we got this far, something failed, redisplay form
            return Ok();

        }
        [HttpGet]
        public async Task<IActionResult> TestAPI() //For Apis testing
        {
            try
            {
                return Ok("Apis working fine");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }



    }
}
