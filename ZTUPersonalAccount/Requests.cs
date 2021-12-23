using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ZTUPersonalAccount.ViewModels;

namespace ZTUPersonalAccount
{
    public class Requests
    {
        public const string LoginPersonalAccountUrl = "https://cabinet.ztu.edu.ua/site/login";
        public const string RozkladGroupUrl = "https://rozklad.ztu.edu.ua/schedule/group/";


        private static HttpClient httpClient = new HttpClient();

        public static async Task<IEnumerable<string>> LoginInPersonalAccount(string userName, string password)
        {
            HttpResponseMessage loginGetResponse = await httpClient.GetAsync(LoginPersonalAccountUrl);
            string loginGetResponseText = await loginGetResponse.Content.ReadAsStringAsync();
            string _csrf_frontend = await ParserShedule.GetCsrfFrontend(loginGetResponseText);
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("_csrf-frontend", _csrf_frontend ),
                new KeyValuePair<string, string>("LoginForm[username]", userName),
                new KeyValuePair<string, string>("LoginForm[password]", password),
                new KeyValuePair<string, string>("LoginForm[rememberMe]", "1")
            });
            HttpResponseMessage loginPostResponse = await httpClient.PostAsync(LoginPersonalAccountUrl, content);
            string loginPostResponseText = await loginPostResponse.Content.ReadAsStringAsync();
            if (loginPostResponseText.Contains("Неправильний логін або пароль"))
                return null;
            else
                return loginPostResponse.Headers.GetValues("Set-Cookie"); ;
        }

        public static async Task<List<Subject>> GetScheduleForTomorrowAsync(string groupName, int subGroup)
        {
            HttpResponseMessage scheduleGroupResponse = await httpClient.GetAsync(RozkladGroupUrl + groupName);
            string scheduleGroupResponseText = await scheduleGroupResponse.Content.ReadAsStringAsync();
            return await ParserShedule.GetScheduleForTomorrowAsync(scheduleGroupResponseText, subGroup);
        }

        public static async Task<Dictionary<string, Dictionary<string, List<Subject>>>> GetScheduleForTwoWeeksAsync(string groupName, int subGroup)
        {
            HttpResponseMessage scheduleGroupResponse = await httpClient.GetAsync(RozkladGroupUrl + groupName);
            string scheduleGroupResponseText = await scheduleGroupResponse.Content.ReadAsStringAsync();
            return await ParserShedule.GetScheduleForTwoWeekAsync(scheduleGroupResponseText, subGroup); ;
        }
    }
}
