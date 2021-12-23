using AngleSharp;
using AngleSharp.Dom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZTUPersonalAccount
{
    public class Requests
    {
        public const string LoginPersonalAccount = "https://cabinet.ztu.edu.ua/site/login";

        private static HttpClient httpClient = new HttpClient();
        private static IConfiguration config = Configuration.Default.WithDefaultLoader();

        public static async Task<IEnumerable<string>> LoginInPersonalAccount()
        {
            HttpResponseMessage loginGetResponse = await httpClient.GetAsync(LoginPersonalAccount);
            string loginGetResponseText = await loginGetResponse.Content.ReadAsStringAsync();
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument document = await context.OpenAsync(req => req.Content(loginGetResponseText));
            string _csrf_frontend = document.QuerySelector("input[name=\"_csrf-frontend\"]").GetAttribute("value");
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("_csrf-frontend", _csrf_frontend ),
                new KeyValuePair<string, string>("LoginForm[username]", "ipz204_vvyu"),
                new KeyValuePair<string, string>("LoginForm[password]", "665989"),
                new KeyValuePair<string, string>("LoginForm[rememberMe]", "1")
            });
            HttpResponseMessage loginPostResponse = await httpClient.PostAsync(LoginPersonalAccount, content);
            string loginPostResponseText = await loginPostResponse.Content.ReadAsStringAsync();
            if (loginPostResponseText.Contains("Неправильний логін або пароль"))
                return null;
            else
                return loginPostResponse.Headers.GetValues("Set-Cookie"); ;
        }
    }
}
