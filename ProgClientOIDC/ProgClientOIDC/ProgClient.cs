using ProgClientOIDC.GetConf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ProgClientOIDC
{
    public class ProgClient
    {
        public async Task GetAuthorizationCode()
        {
            var conf = GetConfSettings.GetSettings();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var uriBuilder = new UriBuilder("https://login.microsoftonline.com/common/oauth2/v2.0/authorize?");
            uriBuilder.Port = -1;

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query.Add("client_id", conf.ClientId);
            query.Add("response_type", "id_token");
            query.Add("redirect_uri", conf.RedirectUri);
            query.Add("response_mode", "form_post");
            query.Add("state", Guid.NewGuid().ToString());
            query.Add("nonce", Guid.NewGuid().ToString());
            query.Add("scope", "openid");
            //query.Add("prompt", "none");

            uriBuilder.Query = query.ToString();

            HttpResponseMessage response = await client.GetAsync(uriBuilder.ToString()).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync();

            var psi = new ProcessStartInfo
            {
                FileName = "cmd",
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = $"/c start {uriBuilder.ToString()}"
            };
            var process = Process.Start(psi);

            process.WaitForExit();
        }
    }
}
