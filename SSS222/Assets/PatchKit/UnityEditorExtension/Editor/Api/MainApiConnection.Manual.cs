using PatchKit.Api.Models.Main;
using System.Collections.Generic;

namespace PatchKit.Api
{
    public partial class MainApiConnection
    {
        public App PostUserApplication(string apiKey, string name, string platform)
        {
            string path = "/1/apps";
            string query = "api_key=" + apiKey;
            string data = string.Format("name={0}&platform={1}", name, platform);
            var response = Post(path, query, data);
            return ParseResponse<App>(response);
        }
    }
}