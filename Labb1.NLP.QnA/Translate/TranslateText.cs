using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Labb1.NLP.QnA.Translate
{
    public class TranslateText
    {
        public async Task<string> TranslateTe(string text, string lang)
        {

            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();
            string cogSvcKey = configuration["marvinTheTranslatorKey"];
            string cogSvcEndpoint = configuration["marvinEndpoint"];
            string location = configuration["marvinLocation"];

     
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            string route = $"/translate?api-version=3.0&to={lang}";
            object[] body = new object[] { new { Text = text } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(cogSvcEndpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", cogSvcKey);
                // location required if you're using a multi-service or regional (not global) resource.
                request.Headers.Add("Ocp-Apim-Subscription-Region", location);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                var result = await response.Content.ReadAsStringAsync();

                var results = JArray.Parse(result);
                var svar = results[0]["translations"][0]["text"].ToString();
                return svar;
            }   
        }
    }
}
