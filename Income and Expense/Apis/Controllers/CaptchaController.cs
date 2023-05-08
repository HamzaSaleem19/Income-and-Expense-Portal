using AspNetCore.ReCaptcha;
using Income_and_Expense.Areas.Identity.Pages.Account;
using Income_and_Expense.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Income_and_Expense.Apis.Controllers
{
    public class CaptchaController : Controller
    {

        //[ValidateReCaptcha]
        //[HttpPost]
        //public IActionResult SubmitForm(LoginModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View("Index");
        //    TempData["Message"] = "Your form has been sent!";
        //    return RedirectToAction("Index");
        //}

        private readonly RecaptchaService _recaptchaService;

        public CaptchaController(RecaptchaService recaptchaService)
        {
            _recaptchaService = recaptchaService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitForm(string token)
        {
            var isRecaptchaValid = await _recaptchaService.VerifyRecaptcha(token);

            if (!isRecaptchaValid)
            {
                ModelState.AddModelError("Recaptcha", "Invalid reCAPTCHA response.");
            }

            // Process the form submission here

            return View("Index");
        }
    }
}
