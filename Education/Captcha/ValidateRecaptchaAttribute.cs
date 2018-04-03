using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Education.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Microsoft.Extensions.Primitives;

namespace Education.Captcha
{
    public class ValidateRecaptchaAttribute : ActionFilterAttribute
    {
        private readonly string _propertyName;
        private readonly string _secretKey;
        private readonly string _errorViewName;
        private readonly string _errorMessage;
        private const string GoogleRecaptchaUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";

        public ValidateRecaptchaAttribute(string propertyName = "RepatchaValue", string secretKey = CaptchaInfo.SecretKey, string errorViewName = "Error", string errorMessage = "Invalid captcha!")
        {
            _propertyName = propertyName;
            _secretKey = secretKey;
            _errorViewName = errorViewName;
            _errorMessage = errorMessage;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            StringValues Headers;
            context.HttpContext.Request.Headers.TryGetValue("Captcha", out Headers);
            if (Headers.Count != 0)
            {
                string Captcha = Headers[0];
                var captchaValidationResult = ValidateRecaptcha(Captcha, _secretKey);
                if (captchaValidationResult.Success)
                {
                    base.OnActionExecuting(context);
                    return;
                }
            }

            SetInvalidResult(context);
            /*var model = context.ActionArguments.First().Value;
            var propertyInfo = model.GetType().GetProperty(_propertyName);
            if (propertyInfo != null)
            {
                var repatchaValue = propertyInfo.GetValue(model, null) as string;
                var captchaValidationResult = ValidateRecaptcha(repatchaValue, _secretKey);
                if (captchaValidationResult.Success)
                {
                    base.OnActionExecuting(context);
                    return;
                }
            }*/
        }

        private void SetInvalidResult(ActionExecutingContext context)
        {
            context.Result = new JsonResult(new ErrorMessage { Error = _errorMessage } );
        }

        private static RecaptchaResponse ValidateRecaptcha(string userEnteredCaptcha, string secretKey)
        {
            if (string.IsNullOrEmpty(userEnteredCaptcha))
            {
                return new RecaptchaResponse
                {
                    Success = false,
                    ErrorCodes = new[] { "missing-input-response" }
                };
            }

            using (var client = new HttpClient())
            {
                var result = client.GetStringAsync(string.Format((string)GoogleRecaptchaUrl, secretKey, userEnteredCaptcha)).Result;
                var captchaResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(result);
                return captchaResponse;
            }
        }

        public class RecaptchaResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("challenge_ts")]
            public string ChallengeTs { get; set; }   // timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)

            [JsonProperty("hostname")]
            public string Hostname { get; set; }      // the hostname of the site where the reCAPTCHA was solved

            [JsonProperty("error-codes")]
            public string[] ErrorCodes { get; set; }  // optional
        }
    }
}
