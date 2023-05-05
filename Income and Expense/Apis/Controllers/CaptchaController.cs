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
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var captchaImage = HttpContext.Request.Form["g-recaptcha-response"];
            if (string.IsNullOrEmpty(captchaImage))
            {
                return Content("hello");
            }
            var verified = await CheckCaptcha();
            if (!verified)
            {
                return Content("not verify");
            }
            if (verified)
            {
                return Content("Hello verified");
            }
            return View();
        }
        public async Task<bool> CheckCaptcha()
        {
            var postData= new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("secret", "6LfD6F4lAAAAAJ-9Mzm-G3TEwyMkBcZPcug-LS0c"),
                new KeyValuePair<string, string>("response", HttpContext.Request.Form["google-recaptcha-response"])
            };
            var client= new HttpClient();
            var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(postData));
            var o = (JObject)JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return (bool)o["success"];
        }
        
    }
}
