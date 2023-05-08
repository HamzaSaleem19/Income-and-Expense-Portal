using Income_and_Expense.Data.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Income_and_Expense.Services
{
    public class RecaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly RecaptchaSettings _recaptchaSettings;

        public RecaptchaService(HttpClient httpClient, IOptions<RecaptchaSettings> recaptchaSettings)
        {
            _httpClient = httpClient;
            _recaptchaSettings = recaptchaSettings.Value;
        }

        public async Task<bool> VerifyRecaptcha(string token)
        {
            var parameters = new Dictionary<string, string>
            {
                { "secret", _recaptchaSettings.SecretKey },
                { "response", token }
            };

            var response = await _httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(parameters));
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RecaptchaResponse>(responseBody);

            return result.Success;
        }
    }

    public class RecaptchaResponse
    {
        public bool Success { get; set; }
    }
}
